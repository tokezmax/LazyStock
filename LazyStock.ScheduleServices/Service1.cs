using Common.Tools;
using LazyStock.ScheduleServices.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Common.Extensions;
using System.Data.SqlClient;
using System.Collections;
using LazyStock.ScheduleServices.Services;

namespace LazyStock.ScheduleServices
{
    public partial class Service1 : ServiceBase
    {
        public static bool IsBusy = false;
        public Service1()
        {
            InitializeComponent();
            IsBusy = false;
            Common.Tools.Setting.ReLoad();
        }

        protected override void OnStart(string[] args)
        {
            //建立一個timer
            System.Timers.Timer MyTimer = new System.Timers.Timer();
            //Elapsed代表，timer設定的時間到後要做什麼事情
            MyTimer.Elapsed += new ElapsedEventHandler(doSomething);
            int RunInterval = (60 * 60 * 1000); //預設1小時
            try
            {
                RunInterval = Int32.Parse(Common.Tools.Setting.AppSettings("RunSec")) * 1000;
            }catch { }
            //做什麼事情可以寫成method丟進去，sample為寫log
            MyTimer.Interval = RunInterval; //代表時間間隔，單位為毫秒，設定2000代表2秒
            MyTimer.Start(); //啟動timer            
        }

        private void doSomething(object sender, ElapsedEventArgs e) //寫log
        {
            Common.Tools.Setting.ReLoad();
            String TraSn = Common.Tools.TokenGenerator.GetTimeStamp(3);

            try
            {
                if (IsBusy)
                {
                    Common.Tools.LogHelper.doLog("Common", "[" + TraSn + "]IsBusy");
                    return;
                }

                string[] typenames = "DeilyPriceTWSECrawlerServcies,DeilyPriceTPEXCrawlerServcies".Split(',');
                foreach (String typeFullName in typenames)
                {
                    Common.Tools.LogHelper.doLog(typeFullName, "[" + TraSn + "]Go");

                    try
                    {

                        BaseCrawlerServcies DataCrawler = (BaseCrawlerServcies)Activator.CreateInstance(Type.GetType("LazyStock.ScheduleServices.Services." + typeFullName));
                        DataCrawler.Init();
                        DataCrawler.CheckIsDone();
                        DataCrawler.Download();
                        DataCrawler.ImportDate();
                    }
                    catch (Exception eex)
                    {
                        Common.Tools.LogHelper.doLog(typeFullName, "[Exception][" + TraSn + "]" + eex.Message);
                    }

                    Common.Tools.LogHelper.doLog(typeFullName, "[" + TraSn + "]Done");

                }
            }
            catch (Exception ex)
            {
                Common.Tools.LogHelper.doLog("Common", "[Exception][" + TraSn + "]" + ex.Message);
            }
            finally
            {
                Common.Tools.LogHelper.doLog("Common", "[" + TraSn + "]UnBusy");
                IsBusy = false;
            }
           





            /*
            String TraSn = Common.Tools.TokenGenerator.GetTimeStamp(3);
            Common.Tools.LogHelper.doLog("UploadStockPrice", "["+ TraSn + "]Start("+ Common.Tools.Setting.AppSettings("RunSec") + ")");

            try
            {
                if (IsBusy) {
                    Common.Tools.LogHelper.doLog("UploadStockPrice", "[" + TraSn + "]IsBusy");
                    return;
                }

                IsBusy = true;
                
                
                Common.Tools.LogHelper.doLog("UploadStockPrice", "[" + TraSn + "]Go");
                //上市資料
                UploadDeilyPriceByTWSE();
                //上櫃資料
                UploadDeilyPriceByTPEX();
                Common.Tools.LogHelper.doLog("UploadStockPrice", "[EachTriggerEvent]" + DateTime.Now.ToString("yyyyMMdd"));
                
                Common.Tools.LogHelper.doLog("UploadStockPrice", "[" + TraSn + "]Done");
            }
            catch (Exception ex)
            {
                Common.Tools.LogHelper.doLog("UploadStockPrice", "[Exception][" + TraSn + "]" + ex.Message);
            }
            finally {
                IsBusy = false;
            }
            */

        }




        /*
        /// <summary>
        /// 抓取(TWSE証券交易所)，並上傳
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public static void UploadDeilyPriceByTWSE()
        {
            try
            {
                String downloadedData = "";
               
                try
                {
                    int DaysDiff = -1;
                    if (DateTime.Now.Hour >= 18)
                        DaysDiff = 0;

                    if (!String.IsNullOrEmpty(Setting.AppSettings("SpecifiedDate")))
                        DaysDiff = Int32.Parse(Setting.AppSettings("SpecifiedDate"));

                    

                    String yyyyMMdd = DateTime.Now.AddDays(DaysDiff).ToString("yyyyMMdd");
                    String CheckDoneFileName = GetCheckDonePath("TWSE", yyyyMMdd);
                    String StockPriceCsvDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TWSEStockPrice\";
                    String StockPriceCsvFileName = TokenGenerator.GetTimeStamp(3) + ".csv";
                    

                    if (!System.IO.Directory.Exists(StockPriceCsvDirPath))
                        System.IO.Directory.CreateDirectory(StockPriceCsvDirPath);

                    if (System.IO.File.Exists(CheckDoneFileName))
                        throw new Exception("["+ yyyyMMdd+"]Before Done, do nothing!");
                    
                    StockPriceCsvFileName = StockPriceCsvDirPath + StockPriceCsvFileName;

                    

                    String url = Setting.AppSettings("TWSEStockPriceURL").Replace("@yyyyMMdd", yyyyMMdd);

                    using (WebClient wClient = new WebClient())
                        downloadedData = wClient.DownloadString(url);

                    if (String.IsNullOrEmpty(downloadedData))
                    {
                        throw new Exception("TWSE檔案下載失敗!"+ url);
                    }


                    if (downloadedData.IndexOf(@"證券代號") < 0)
                    {

                        System.IO.File.WriteAllText(StockPriceCsvFileName, downloadedData);
                        throw new Exception("錯誤的資料格式!" + url);
                    }
                    
                    System.IO.File.WriteAllText(StockPriceCsvFileName, downloadedData.Substring(downloadedData.IndexOf(@"證券代號") - 1).Replace("=\"", "\""), Encoding.UTF8);

                    var csv = new CsvHelper.CsvReader(System.IO.File.OpenText(StockPriceCsvFileName));
                    List<StockPriceDataModel> StockPricesList = new List<StockPriceDataModel>();
                    while (csv.Read())
                    {
                        string CsvStockNum = "";
                        try{
                            CsvStockNum = csv.GetField<string>(0);
                            CsvStockNum = CsvStockNum.Replace(" ", "");
                            if (CsvStockNum.Length != 4)
                                continue;
                            Int32.Parse(CsvStockNum);
                        }catch{
                            continue;
                        }

                        float CsvPrice = 0;
                        try{
                            CsvPrice = csv.GetField<float>(8);
                        }catch {}

                        StockPricesList.Add(new StockPriceDataModel
                        {
                            StockNum = CsvStockNum,
                            StockPrice = CsvPrice
                        });
                    }
                    UploadDeilyPrice(StockPricesList, CheckDoneFileName);
                    UpdateMsSQL(StockPricesList, CheckDoneFileName);
                }  
                catch (WebException ex)
                {
                    throw ex;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取(TWSE証券交易所)，並上傳
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public static void UploadDeilyPriceByTPEX()
        {
            try
            {
                String downloadedData = "";

                try
                {
                    int DaysDiff = -1;
                    if (DateTime.Now.Hour >= 18)
                        DaysDiff = 0;

                    if (!String.IsNullOrEmpty(Setting.AppSettings("SpecifiedDate")))
                        DaysDiff = Int32.Parse(Setting.AppSettings("SpecifiedDate"));

                    DateTime TWDate = DateTime.Now.AddDays(DaysDiff);
                    String yyyyMMdd = TWDate.ToString("yyyyMMdd");
                    String CheckDoneFileName = GetCheckDonePath("TPEX", yyyyMMdd);
                    String StockPriceJsonDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataSource\TPEXStockPrice\";
                    String StockPriceJsonFileName = TokenGenerator.GetTimeStamp(3) + ".json";

                    
                    if (!System.IO.Directory.Exists(StockPriceJsonDirPath))
                        System.IO.Directory.CreateDirectory(StockPriceJsonDirPath);
                    
                    if (System.IO.File.Exists(CheckDoneFileName))
                        throw new Exception("[" + yyyyMMdd + "]Before Done, do nothing!");

                    StockPriceJsonFileName = StockPriceJsonDirPath + StockPriceJsonFileName;



                    String url = Setting.AppSettings("TPEXStockPriceURL").Replace("@yyyMMdd", TWDate.ToSimpleTaiwanDate());

                    using (WebClient wClient = new WebClient())
                        downloadedData = wClient.DownloadString(url);

                    if (String.IsNullOrEmpty(downloadedData))
                        throw new Exception("TPEX檔案下載失敗!" + url);

                    TPEXStockPriceDataModel TPEXStockData = null;
                    try
                    {
                        TPEXStockData = JsonConvert.DeserializeObject<TPEXStockPriceDataModel>(downloadedData);
                    }catch {
                        throw new Exception("錯誤的資料格式!" + url);
                    }

                    System.IO.File.WriteAllText(StockPriceJsonFileName, downloadedData, Encoding.UTF8);

                    List<StockPriceDataModel> StockPricesList = new List<StockPriceDataModel>();
                    foreach (String[] Rows in TPEXStockData.aaData)
                    {
                        String StockNum = "";
                        float Price = 0;

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
                            Price = float.Parse(Rows[2]);
                        }
                        catch { }

                        StockPricesList.Add(new StockPriceDataModel
                        {
                            StockNum = StockNum,
                            StockPrice = Price
                        });
                    }
                    UploadDeilyPrice(StockPricesList, CheckDoneFileName);

                    UpdateMsSQL(StockPricesList,CheckDoneFileName);
                }
                catch (WebException ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Upload
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public static void UploadDeilyPrice(List<StockPriceDataModel> StockPricesList ,String CheckDonePath)
        {
            try
            {
                LogHelper.doLog("UploadStockPrice", "==準備發送==\r\n" + StockPricesList.Count.ToString() + "筆");

                for (int i = 0; i < StockPricesList.Count; i = i + 100)
                {
                    var items = StockPricesList.Skip(i).Take(100);

                    ReceiveByDeilyPriceReqModel StockPricesReq = new ReceiveByDeilyPriceReqModel();
                    StockPricesReq.StockPrices = items.ToList<StockPriceDataModel>();
                    var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(StockPricesReq) + @"}";
                    LogHelper.doLog("UploadStockPrice", "==發送(" + StockPricesReq.StockPrices.Count().ToString() + "筆)==\r\n" + jsonText);
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Headers.Add("PowerAdmin", "1qaz2wsx0p;/9ol.");
                    var response = client.UploadData(Common.Tools.Setting.AppSettings("ReceiveByDeilyPriceURL"), "POST", Encoding.UTF8.GetBytes(jsonText));
                    string resResult = Encoding.UTF8.GetString(response);
                    LogHelper.doLog("UploadStockPrice", "==接收==\r\n" + resResult);
                }
                System.IO.File.WriteAllText(CheckDonePath, "done");
                LogHelper.doLog("UploadStockPrice", "[Succ]Done");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 更新mssql 至股票價格
        /// </summary>
        /// <param name="StockPricesList"></param>
        public static void UpdateMsSQL(List<StockPriceDataModel> StockPricesList,String CheckDonePath)
        {
            try
            {
                foreach (StockPriceDataModel spdm in StockPricesList)
                {

                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@StockPrice", spdm.StockPrice));
                    list.Add(new SqlParameter("@StockNum", spdm.StockNum));
                    Common.DataAccess.Dao.execute("UPDATE [StockPrice] set StockPrice =@StockPrice,ModifyDate=getdate() where StockNum=@StockNum", list, Setting.ConnectionString("Stock"));

                }
                System.IO.File.WriteAllLines(CheckDonePath, "SQL done".Split(' '));
            }
            catch {
                System.IO.File.WriteAllLines(CheckDonePath, "SQL No done".Split(' '));
            }
            
        }
       
        /// <summary>
        /// 產生checkdone path
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="yyyyMMdd"></param>
        /// <returns></returns>
        public static String GetCheckDonePath(String Type, String yyyyMMdd)
        {
            String CheckDoneDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\CheckDone\" + Type + @"\";
            String CheckDoneFileName = yyyyMMdd + ".txt";
            if (!System.IO.Directory.Exists(CheckDoneDirPath))
                System.IO.Directory.CreateDirectory(CheckDoneDirPath);
            return CheckDoneDirPath + CheckDoneFileName;
        }
        */
        protected override void OnStop()
        {

        }
    }
}
