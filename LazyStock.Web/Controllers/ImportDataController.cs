using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Extensions;
using Common.Tools;
using Newtonsoft.Json;
using LazyStock.Web.Models;
using System.Collections;
using Newtonsoft.Json.Linq;
using LiteDB;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using CsvHelper.Configuration;

namespace LazyStock.Web.Controllers
{
    public class ImportDataController : Controller
    {
        #region
        /// <summary>
        /// 資料庫存放位置
        /// </summary>
        /// LazyStockDBPath
        public static String StockInfoJsonDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\JsonData\";
        public static String LazyStockDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LazyStockDB.db";
        #endregion

        /// <summary>
        /// 抓取(TWSE証券交易所)，並上傳
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult UploadDeilyPriceByTWSE(String StockNum = "")
        {
            BaseResModel result = new BaseResModel();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                String downloadedData = "";
                using (WebClient wClient = new WebClient())
                {
                    try
                    {
                        String StockPriceCsvFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\StockPrice-" + TokenGenerator.GetTimeStamp(3) + ".csv";
                        String yyyyMMdd = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                        String url = Setting.AppSettings("TWSEStockPriceURL").Replace("@yyyyMMdd", yyyyMMdd);
                        //wClient.Encoding = Encoding.UTF8;
                        downloadedData = wClient.DownloadString(url);

                        if (String.IsNullOrEmpty(downloadedData)) {
                            result.Code = ResponseCodeEnum.Failed;
                            result.Message = "下載失敗!";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }

                        if (downloadedData.IndexOf(@"證券代號") < 0) {
                            result.Code = ResponseCodeEnum.Failed;
                            result.Message = "格式有誤!";
                            System.IO.File.WriteAllText(StockPriceCsvFilePath, downloadedData);
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }

                        System.IO.File.WriteAllText(StockPriceCsvFilePath, downloadedData.Substring(downloadedData.IndexOf(@"證券代號")-1).Replace("=\"","\""),Encoding.UTF8);
                        
                        var csv = new CsvHelper.CsvReader(System.IO.File.OpenText(StockPriceCsvFilePath));
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
                            }catch {
                                continue;
                            }

                            float CsvPrice = 0;
                            try {
                                CsvPrice = csv.GetField<float>(8);
                            }catch {}

                            float CsvPER = 0;
                            try{
                                CsvPER = csv.GetField<float>(15);
                            }catch { }

                            StockPricesList.Add(new StockPriceDataModel
                            {
                                StockNum = CsvStockNum,
                                StockPrice = CsvPrice,
                                PER = CsvPER
                            });
                        }
                        
                        //ReceiveByDeilyPriceReqModel StockPricesReq = new ReceiveByDeilyPriceReqModel();
                        //StockPricesReq.StockPrices = sp;
                        LogHelper.doLog("UploadDeilyPriceByTWSE", "==準備發送==\r\n" + StockPricesList.Count.ToString() + "筆");

                        for (int i = 0; i < StockPricesList.Count; i = i + 100)
                        {
                            var items = StockPricesList.Skip(i).Take(100);

                            ReceiveByDeilyPriceReqModel StockPricesReq = new ReceiveByDeilyPriceReqModel();
                            StockPricesReq.StockPrices = items.ToList<StockPriceDataModel>();
                            var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(StockPricesReq) + @"}";
                            LogHelper.doLog("UploadDeilyPriceByTWSE", "==發送(" + StockPricesReq.StockPrices.Count().ToString() + "筆)==\r\n" + jsonText);
                            WebClient client = new WebClient();
                            client.Encoding = Encoding.UTF8;
                            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                            client.Headers.Add("PowerAdmin", Setting.AppSettings("PowerAdmin"));

                            var response = client.UploadData(Common.Tools.Setting.AppSettings("ReceiveByDeilyPriceURL"), "POST", Encoding.UTF8.GetBytes(jsonText));
                            string resResult = Encoding.UTF8.GetString(response);
                            result = JsonConvert.DeserializeObject<BaseResModel>(resResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                            LogHelper.doLog("UploadDeilyPriceByTWSE", "==接收==\r\n" + resResult);
                        }

                        result.Code = ResponseCodeEnum.Success;
                        LogHelper.doLog("UploadDeilyPriceByTWSE", "[Succ]Done");
                    }
                    catch (WebException ex)
                    {
                        result.Code = ResponseCodeEnum.Failed;
                        result.Message = ex.Message;
                        LogHelper.doLog("UploadDeilyPriceByTWSE", "[Fail]" + ex.Message);
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 接收(Json)更新股價
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult ReceiveByDeilyPrice(ReceiveByDeilyPriceReqModel ReqParam)
        {
            BaseResModel result = new BaseResModel();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                int SuccCount = 0;
                int FailCount = 0;
                // Open database (or create if not exits)
                using (var db = new LiteDatabase(LazyStockDBPath))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");

                    for (int i = 0; i < ReqParam.StockPrices.Count; i++)
                    {
                        StockPriceDataModel items = ReqParam.StockPrices[i];
                        try
                        {
                            int IDKey = Int32.Parse(items.StockNum);
                            var DbStockPrice = StockInfos.FindById(IDKey);
                            if (DbStockPrice == null)
                                continue;


                            DbStockPrice.Price = items.StockPrice;
                            StockInfos.Update(IDKey, DbStockPrice);
                            SuccCount++;
                        }
                        catch
                        {
                            FailCount++;
                        }
                    }
                }

                result.Message = "成功:" + SuccCount.ToString() + ",失敗:" + FailCount.ToString();
                result.Code = ResponseCodeEnum.Success;
                LogHelper.doLog("ReceiveByDeilyPrice", result.Message);
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上傳完整JsonFile Update Stock Info(完整的)
        /// (暫時用不到)
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult ReceiveStockInfo(HttpPostedFileBase file)
        {

            BaseResModel result = new BaseResModel();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                String Context = "";

                if (file == null)
                    return Json(new BaseResModel() { Code = ResponseCodeEnum.FieldsThatAreRequired }, JsonRequestBehavior.AllowGet);

                if (file.ContentLength <= 0)
                    return Json(new BaseResModel() { Code = ResponseCodeEnum.FieldsThatAreRequired }, JsonRequestBehavior.AllowGet);

                String fileName = System.IO.Path.GetFileName(file.FileName);
                String path = System.IO.Path.Combine(Server.MapPath(@"~/StockJson/JsonTemp/"), fileName);


                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                file.SaveAs(path);
                Context = System.IO.File.ReadAllText(path);

                int SuccCount = 0;
                int FailCount = 0;
                using (var db = new LiteDatabase(LazyStockDBPath))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");

                    try
                    {
                        String JsonString = Context;
                        StockInfoResModel StockInfo = JsonConvert.DeserializeObject<StockInfoResModel>(JsonString);
                        StockInfos.Upsert(StockInfo.Num, StockInfo);
                        SuccCount++;
                    }
                    catch
                    {
                        FailCount++;
                    }
                }

                result.Message = "成功:" + SuccCount.ToString() + ",失敗:" + FailCount.ToString();
                result.Code = ResponseCodeEnum.Success;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        #region 抓取(MSSQL)，產生JsonFile

        #region Sql
        //每年股息股利
        //public static String SqlDivi = @"select [Year],StockNum AS Num,Cash as CashDivi,Dividends as Divi,Cash+Dividends as AllDivi from StockDividends where [Year] >= 2014 order by 2,1 desc";
        //每季EPS
        //public static String SqlEPS = @"select Convert(nvarchar,[Year],10)+'-'+[Q]+'-'+[StockNum] as [Index],[Year],[Q],[StockNum] as Num,KeepEPS,EPS,Quarter3EPSAvg from StockEPS where YEAR >=2015 order by 4,2 desc";
        public static String SqlGetStockInfo = @"select StockNum as Num ,StockName as Name,Value,Industry,DebtRatio,InvestorRatio,Price,PERatio,IsPromisingEPS,
            IsGrowingUpEPS,IsAlwaysIncomeEPS,IsAlwaysPayDivi,IsOverDiffDivi,IsSafeValue,IsSafePB,IsSafeInvestor,IsSafeDebt,ModifyDate,
            FutureFromEPS,CurrFromEPS,PrevDiviFrom3YearAvgByEPS 
            from [Stock].dbo.CalStockInfo order by 1,2 desc";
        public static String SqlGetStockEPS_Divi = @" select StockNum,Year,LastQ,TotalEPS,TotalDivi,EachDiviFromEPS from CalStockEPS_Divi order by 1,2 desc ";
        public static String SqlGetStockEachQAvgEPS = @" select StockNum, Year,Q, Quarter3EPSAvg, IsCurrYear from CalStockEachQAvgEPS order by 1,2 desc,3 desc ";

        public static String SqlStockPrice = "SELECT [StockNum],[StockPrice],[PER],[PBR] FROM [Stock].[dbo].[StockPrice] ";
        #endregion

        /// <summary>
        /// 批次產生大量Jason File Data
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateStockInfoToJsonFile()
        {
            BaseResModel result = new BaseResModel();

            //取得年
            int PrevYear = DateTime.Now.AddYears(-1).Year;
            int CurrentYear = DateTime.Now.Year;

            //取得
            List<StockInfoResModel> StockInfos = new List<StockInfoResModel>();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                //取得股票基本資料
                System.Data.DataTable dt = Common.DataAccess.Dao.QueryDataTable(SqlGetStockInfo, null, Common.Tools.Setting.ConnectionString("Stock"));
                StockInfos = dt.ToList<StockInfoResModel>();

                //股息股利
                //dt = Common.DataAccess.Dao.QueryDataTable(SqlDivi, null, Common.Tools.Setting.ConnectionString("Stock"));
                //List<Dividend> Divis = dt.ToList<Dividend>();

                //ＥＰＳ（每季）
                //dt = Common.DataAccess.Dao.QueryDataTable(SqlEPS, null, Common.Tools.Setting.ConnectionString("Stock"));
                //List<EP> EPS = dt.ToList<EP>();

                //取得每年度股利、EPS
                dt = Common.DataAccess.Dao.QueryDataTable(SqlGetStockEPS_Divi, null, Common.Tools.Setting.ConnectionString("Stock"));
                List<EPS_DiviDataModel> EPS_Divi = dt.ToList<EPS_DiviDataModel>();


                StockInfos.ForEach(t =>
                {
                    //設定版號
                    //t.Ver = Common.Tools.TokenGenerator.GetTimeStamp(3);

                    //股利
                    /*
                    var queryDivi = from StockDividends in Divis
                                where StockDividends.Num == t.Num
                                select new Dividend() {
                                    Year = StockDividends.Year,
                                    Num = StockDividends.Num,
                                    CashDivi = StockDividends.CashDivi,
                                    Divi = StockDividends.Divi,
                                    AllDivi = StockDividends.AllDivi
                                };
                    t.Dividends = queryDivi.ToList();
                    */

                    //EPS & Divi
                    var queryEPS_Divi = from StockEPS_Divi in EPS_Divi
                                        where StockEPS_Divi.StockNum == t.Num
                                        select new EPS_DiviDataModel()
                                        {
                                            Year = StockEPS_Divi.Year,
                                            LastQ = StockEPS_Divi.LastQ,
                                            StockNum = StockEPS_Divi.StockNum,
                                            TotalEPS = StockEPS_Divi.TotalEPS,
                                            TotalDivi = StockEPS_Divi.TotalDivi,
                                            EachDiviFromEPS = StockEPS_Divi.EachDiviFromEPS
                                        };
                    t.EPS_Divi = queryEPS_Divi.ToList();



                    //EPS
                    /*
                    var queryEPS = from StockEPS in EPS
                                   where StockEPS.Num == t.Num
                                    select new EP()
                                    {
                                        Index = StockEPS.Index,
                                        Year = StockEPS.Year,
                                        Num = StockEPS.Num,
                                        Q = StockEPS.Q,
                                        EPS = StockEPS.EPS,
                                        KeepEPS = StockEPS.KeepEPS,
                                        Quarter3EPSAvg = StockEPS.Quarter3EPSAvg
                                    };
                    t.EPS = queryEPS.ToList();
                    */
                    #region 健檢報告（EPS)
                    //健檢報告
                    /*
                    dynamic CurrentKeepEPS =
                            (from StockEPS in queryEPS
                             where
                               StockEPS.Year == CurrentYear &&
                               StockEPS.Num == t.Num
                             orderby
                               StockEPS.Year,
                               StockEPS.Q descending
                             select new
                             {
                                 StockEPS.KeepEPS
                             }).Take(1);

                    dynamic PrevTotalEPS =
                            (from StockEPS in queryEPS
                             where
                               StockEPS.Year == PrevYear &&
                               StockEPS.Num == t.Num
                             orderby
                               StockEPS.Year,
                               StockEPS.Q descending
                             select new
                             {
                                 StockEPS.KeepEPS
                             }).Take(1);
                             */
                    //if ( CurrentKeepEPS  >= PrevTotalEPS)
                    //t.HS.EPSPromising = true;
                    #endregion
                });

                
                StockInfos.ForEach(t =>
                {
                    string json = JsonConvert.SerializeObject(t);
                    LogHelper.FileOverWrite(StockInfoJsonDirPath + t.Num + ".json", json, false);
                });

                result.Code = ResponseCodeEnum.Success;
                result.Result = StockInfos;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [更新股票訊息][Use File][Call Api]
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult UploadStockInfoFromFile(String StockNum = "")
        {
            BaseResModel result = new BaseResModel();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);


                List<string> files = System.IO.Directory.GetFiles(StockInfoJsonDirPath).ToList<string>();
                for (int i = 0; i < files.Count; i = i + 20)
                {
                    var items = files.Skip(i).Take(20);

                    ReceiveByStockInfoReqModel r = new ReceiveByStockInfoReqModel();

                    r.StockInfos = new List<StockInfoResModel>();
                    foreach (String fp in items) 
                        r.StockInfos.Add(JsonConvert.DeserializeObject<StockInfoResModel>(System.IO.File.ReadAllText(fp)));

                    var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(r) + @"}";
                    LogHelper.doLog("UploadStockInfoFromFile", "==發送(" + r.StockInfos.Count().ToString() + "筆)==\r\n" + jsonText);
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Headers.Add("PowerAdmin", Setting.AppSettings("PowerAdmin"));
                    var response = client.UploadData(Common.Tools.Setting.AppSettings("ReceiveByStockInfoURL"), "POST", Encoding.UTF8.GetBytes(jsonText));
                    string resResult = Encoding.UTF8.GetString(response);
                    result = JsonConvert.DeserializeObject<BaseResModel>(resResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                    LogHelper.doLog("UploadStockInfoFromFile", "==接收==\r\n" + resResult);

                }

                result.Code = ResponseCodeEnum.Success;
                result.Message = "";//"[Key]" + Key + "[Val]" + Val;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [接收股票訊息][修改LiteDB]
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult ReceiveByStockInfo(ReceiveByStockInfoReqModel ReqParam)
        {
            BaseResModel result = new BaseResModel();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                int SuccCount = 0;
                int FailCount = 0;
                // Open database (or create if not exits)
                using (var db = new LiteDatabase(LazyStockDBPath))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");

                    for (int i = 0; i < ReqParam.StockInfos.Count; i++)
                    {
                        StockInfoResModel items = ReqParam.StockInfos[i];
                        try
                        {
                            //int IDKey = Int32.Parse(items.Num);
                            //var DbStockInfo = StockInfos.FindById(items.Num.ToString());
                            //if (DbStockInfo == null)
                            //  continue;
                            StockInfos.Delete(items.Num);
                            StockInfos.Insert(items.Num, items);
                            SuccCount++;
                        }
                        catch
                        {
                            FailCount++;
                        }
                    }
                }

                result.Message = "成功:" + SuccCount.ToString() + ",失敗:" + FailCount.ToString();
                result.Code = ResponseCodeEnum.Success;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [更新股價][MSSQL][Call API use JSON Data]
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult UploadDeilyPriceByDB(String StockNum = "")
        {
            BaseResModel result = new BaseResModel();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                System.Data.DataTable dt = Common.DataAccess.Dao.QueryDataTable(SqlStockPrice, null, Common.Tools.Setting.ConnectionString("Stock"));

                List<StockPriceDataModel> StockPrices = new List<StockPriceDataModel>();
                StockPrices = dt.ToList<StockPriceDataModel>();

                ReceiveByDeilyPriceReqModel StockPricesReq = new ReceiveByDeilyPriceReqModel();
                StockPricesReq.StockPrices = (from a in StockPrices
                                              select new StockPriceDataModel()
                                              {
                                                  StockNum = a.StockNum,
                                                  StockPrice = a.StockPrice,
                                                  PER = a.PER
                                              }).ToList<StockPriceDataModel>();
                LogHelper.doLog("UploadDeilyPriceByDB", "==準備發送==\r\n" + StockPricesReq.StockPrices.Count.ToString() + "筆");

                for (int i = 0; i < StockPricesReq.StockPrices.Count; i = i + 100)
                {
                    var items = StockPricesReq.StockPrices.Skip(i).Take(100);

                    ReceiveByDeilyPriceReqModel StockPricesReq2 = new ReceiveByDeilyPriceReqModel();
                    StockPricesReq2.StockPrices = items.ToList<StockPriceDataModel>();
                    var jsonText = "{\"ReqParam\" : " + JsonConvert.SerializeObject(StockPricesReq2) + @"}";
                    LogHelper.doLog("UploadDeilyPriceByDB", "==發送(" + StockPricesReq2.StockPrices.Count().ToString() + "筆)==\r\n" + jsonText);
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Headers.Add("PowerAdmin", Setting.AppSettings("PowerAdmin"));
                    var response = client.UploadData(Common.Tools.Setting.AppSettings("UploadByDeilyURL"), "POST", Encoding.UTF8.GetBytes(jsonText));
                    string resResult = Encoding.UTF8.GetString(response);
                    result = JsonConvert.DeserializeObject<BaseResModel>(resResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                    LogHelper.doLog("UploadDeilyPriceByDB", "==接收==\r\n" + resResult);
                }

                result.Code = ResponseCodeEnum.Success;
                result.Message = "";//"[Key]" + Key + "[Val]" + Val;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}