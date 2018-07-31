using Common.Extensions;
using Common.Tools;
using LazyStock.ScheduleServices.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;

namespace LazyStock.ScheduleServices.Services
{
    public class DeilyPriceTPEXCrawlerServcies : BaseCrawlerServcies
    {
        public string ClassName = "DeilyPriceTPEXCrawlerServcies";
        public const String SQLUpdateLocalDB = "UPDATE [StockPrice] set StockPrice =@StockPrice,ModifyDate=getdate() where StockNum=@StockNum";
        private const string TraType = "TPEX";
        protected string DownloadUrl;
        protected string UploadUrl;
        protected int _retry;
        protected DateTime GetDate;
        protected String DownloadDirPath;
        protected String DownloadFileName;
        protected String DownloadFullPath;
        protected String CheckDoneDirPath;
        protected String CheckDoneFileName;
        protected String CheckDoneFullPath;
        protected String DownloadData = "";
        private List<StockPriceDataModel> StockPricesList;

        public DeilyPriceTPEXCrawlerServcies()
        {
            DownloadUrl = Setting.AppSettings("TPEXStockPriceURL");
            UploadUrl = Setting.AppSettings("ReceiveByDeilyPriceURL");
            GetDate = DateTime.Now;
            StockPricesList = new List<StockPriceDataModel>();
        }

        public override void Init()
        {
            int DaysDiff = -1;
            if (DateTime.Now.Hour >= 18)
                DaysDiff = 0;
            if (!String.IsNullOrEmpty(Setting.AppSettings("SpecifiedDate")))
                DaysDiff = Int32.Parse(Setting.AppSettings("SpecifiedDate"));

            GetDate = GetDate.AddDays(DaysDiff);
            CheckDoneDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\CheckDone\DeilyPriceTPEX\";
            CheckDoneFileName = GetDate.ToString("yyyyMMdd") + ".txt";
            CheckDoneFullPath = CheckDoneDirPath + CheckDoneFileName;

            if (!System.IO.Directory.Exists(CheckDoneDirPath))
                System.IO.Directory.CreateDirectory(CheckDoneDirPath);

            DownloadDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TPEXStockPrice\";
            DownloadFileName = TokenGenerator.GetTimeStamp(3) + ".json";
            DownloadFullPath = DownloadDirPath + DownloadFileName;

            if (!System.IO.Directory.Exists(DownloadDirPath))
                System.IO.Directory.CreateDirectory(DownloadDirPath);
        }

        public override void CheckIsDone()
        {
            if (System.IO.File.Exists(CheckDoneFullPath))
                throw new Exception("[" + GetDate.ToString("yyyyMMdd") + "]Before Done, do nothing!");
        }

        public override void Download()
        {
            String url = DownloadUrl.Replace("@yyyMMdd", GetDate.ToSimpleTaiwanDate());

            using (WebClient wClient = new WebClient())
                DownloadData = wClient.DownloadString(url);

            if (String.IsNullOrEmpty(DownloadData))
                throw new Exception("TPEX檔案下載失敗!" + url);

            System.IO.File.WriteAllText(DownloadFullPath, DownloadData, Encoding.UTF8);
        }

        public override void ImportDate()
        {
            TPEXStockPriceDataModel TPEXStockData = null;
            try
            {
                TPEXStockData = JsonConvert.DeserializeObject<TPEXStockPriceDataModel>(DownloadData);
            }
            catch
            {
                throw new Exception("錯誤的資料格式!");
            }

            if (TPEXStockData.aaData.Length == 0)
            {
                throw new Exception("查無資料!");
            }

            foreach (String[] Rows in TPEXStockData.aaData)
            {
                String StockNum = "";
                double Price = 0;

                try
                {
                    StockNum = Rows[0].Replace(" ", "");
                    if (StockNum.Length != 4)
                        continue;
                    Int32.Parse(StockNum);
                }
                catch
                {
                    continue;
                }

                try
                {
                    Price = Math.Round(double.Parse(Rows[2]), 2);
                }
                catch { }

                StockPricesList.Add(new StockPriceDataModel
                {
                    StockNum = StockNum,
                    StockPrice = Price
                });
            }

            UploadData();
            UpdateLocalDB();
        }

        protected override void UploadData()
        {
            try
            {
                LogHelper.doLog(this.ClassName, "==準備發送==\r\n" + StockPricesList.Count.ToString() + "筆");

                for (int i = 0; i < StockPricesList.Count; i = i + 100)
                {
                    var items = StockPricesList.Skip(i).Take(100);

                    ReceiveByDeilyPriceReqModel StockPricesReq = new ReceiveByDeilyPriceReqModel();
                    StockPricesReq.StockPrices = items.ToList();
                    var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(StockPricesReq) + @"}";
                    LogHelper.doLog(this.ClassName, "==發送(" + StockPricesReq.StockPrices.Count().ToString() + "筆)==\r\n" + jsonText);
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Headers.Add("PowerAdmin", Common.Tools.Setting.AppSettings("PowerAdmin"));
                    var response = client.UploadData(UploadUrl, "POST", Encoding.UTF8.GetBytes(jsonText));
                    string resResult = Encoding.UTF8.GetString(response);
                    LogHelper.doLog(this.ClassName, "==接收==\r\n" + resResult);
                }
                System.IO.File.WriteAllText(this.CheckDoneFullPath, "done");
                LogHelper.doLog(this.ClassName, "[Succ]Done");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void UpdateLocalDB()
        {
            try
            {
                foreach (StockPriceDataModel spdm in StockPricesList)
                {
                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@StockPrice", spdm.StockPrice));
                    list.Add(new SqlParameter("@StockNum", spdm.StockNum));
                    Common.DataAccess.Dao.execute(SQLUpdateLocalDB, list, Setting.ConnectionString("Stock"));
                }
                System.IO.File.WriteAllLines(CheckDoneFullPath, "SQL done".Split('|'));
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllLines(CheckDoneFullPath, ("SQL No done\r\n" + e.Message).Split('|'));
            }
        }
    }
}