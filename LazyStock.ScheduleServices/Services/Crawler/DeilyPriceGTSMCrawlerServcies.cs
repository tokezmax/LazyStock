using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Tools;
using LazyStock.ScheduleServices.Model;
using Newtonsoft.Json;

namespace LazyStock.ScheduleServices.Services
{
    public class DeilyPriceGTSMCrawlerServcies : BaseCrawlerServcies
    {
        public const String SQLUpdateLocalDB = "UPDATE [StockPrice] set StockPrice =@StockPrice,ModifyDate=getdate() where StockNum=@StockNum";

        public string ClassName = "DeilyPriceGTSMCrawlerServcies";
        private const string TraType = "GTSM";
        protected string DownloadUrl;
        protected string UploadUrl;
        protected DateTime GetDate;
        protected String DownloadDirPath;
        protected String DownloadFileName;
        protected String DownloadFullPath;
        protected String CheckDoneDirPath;
        protected String CheckDoneFileName;
        protected String CheckDoneFullPath;
        protected String DownloadData = "";
        List<StockPriceDataModel> StockPricesList;


        public DeilyPriceGTSMCrawlerServcies()
        {
            DownloadUrl = Setting.AppSettings("GTSMStockPriceURL");
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
            CheckDoneDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\CheckDone\DeilyPriceGTSM\";
            CheckDoneFileName = GetDate.ToString("yyyyMMdd") + ".txt";
            CheckDoneFullPath = CheckDoneDirPath + CheckDoneFileName;

            if (!System.IO.Directory.Exists(CheckDoneDirPath))
                System.IO.Directory.CreateDirectory(CheckDoneDirPath);

            DownloadDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\GTSMStockPrice\";
            DownloadFileName = TokenGenerator.GetTimeStamp(3) + ".csv";
            DownloadFullPath = DownloadDirPath + DownloadFileName;

            if (!System.IO.Directory.Exists(DownloadDirPath))
                System.IO.Directory.CreateDirectory(DownloadDirPath);
        }

        public override void CheckIsDone()
        {
            if (System.IO.File.Exists(CheckDoneFullPath))
                throw new Exception("[" + GetDate.ToString("yyyyMMdd") + "]Before Done, do nothing!");

            if (Setting.AppSettings("DeilyPriceGTSMAlwaysGetYN") != "Y")
                if ((GetDate.Hour <= 18 && GetDate.Hour >=8))
                    throw new Exception("[" + GetDate.ToString("yyyyMMdd") + "]8~18交易時間禁止抓取!");
        }

        public override void Download()
        {

            String url = DownloadUrl.Replace("@t", GetDate.Ticks.ToString());

            using (WebClient wClient = new WebClient())
                DownloadData = wClient.DownloadString(url);


            if (DownloadData.IndexOf(@"""") < 0)
                throw new Exception("錯誤的資料格式!");

            DownloadData = DownloadData.Substring(DownloadData.IndexOf(@"""") - 1);
            System.IO.File.WriteAllText(DownloadFullPath, DownloadData);
        }

        public override void ImportDate()
        {
            var csv = new CsvHelper.CsvReader(System.IO.File.OpenText(DownloadFullPath));

            while (csv.Read())
            {
                string CsvStockNum = "";
                try
                {
                    CsvStockNum = csv.GetField<string>(0);
                    CsvStockNum = CsvStockNum.Replace(" ", "");
                    if (CsvStockNum.Length != 4)
                        continue;
                    Int32.Parse(CsvStockNum);
                }
                catch
                {
                    continue;
                }

                double CsvPrice = 0;
                try
                {
                    CsvPrice = csv.GetField<double>(10);
                }
                catch { }

                StockPricesList.Add(new StockPriceDataModel
                {
                    StockNum = CsvStockNum,
                    StockPrice = Math.Round(CsvPrice,2)
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
