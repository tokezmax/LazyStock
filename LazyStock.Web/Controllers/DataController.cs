using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LazyStock.Web.Controllers
{
    public class DataController : Controller
    {
        /// <summary>
        /// 上傳更新股票資訊
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadByDeily(SetStockPriceReqModel ReqParam)
        {
            BaseResModel result = new BaseResModel();
            try
            {
                int SuccCount = 0;
                int FailCount = 0;
                // Open database (or create if not exits)
                using (var db = new LiteDatabase(StockFileLocalDirPath + @"LazyStockDB.db"))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");

                    for (int i = 0; i < ReqParam.StockPrices.Count; i++)
                    {
                        StockPrice items = ReqParam.StockPrices[i];
                        try
                        {
                            int IDKey = Int32.Parse(items.StockNum);
                            var DbStockPrice = StockInfos.FindById(IDKey);
                            if (DbStockPrice ==null) 
                                continue;
                        

                            DbStockPrice.Price = items.Price;
                            StockInfos.Update(IDKey,DbStockPrice);
                            SuccCount++;
                        }
                        catch (Exception e)
                        {
                            FailCount++;
                        }
                    }

                }

                // Use Linq to query documents
                //result.Result = StockInfos.FindById(1101);
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
        /// 上傳更新股票資訊
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadStockInfo(HttpPostedFileBase file)
        {

            BaseResModel result = new BaseResModel();
            try
            {
                String Context = "";
                //foreach (string file in Request.Files) {
                //HttpPostedFileBase uploadFile = Request.Files[file] as HttpPostedFileBase;
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
                Context= System.IO.File.ReadAllText(path);
                //}

                int SuccCount = 0;
                int FailCount = 0;
                // Open database (or create if not exits)
                using (var db = new LiteDatabase(StockFileLocalDirPath + @"LazyStockDB.db"))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");

                    //foreach upload data
                    //查出id多少
                    //再做insert or update
                    //並註明更新時間為多少
                    
                    //foreach (String filepath in System.IO.Directory.GetFiles(StockFileLocalDirPath, "*.json"))
                    //{}
                        try {
                            //String StockNum = System.IO.Path.GetFileNameWithoutExtension(filepath);
                            String JsonString = Context;
                            StockInfoResModel StockInfo = JsonConvert.DeserializeObject<StockInfoResModel>(JsonString);
                            StockInfos.Upsert(StockInfo.Num, StockInfo);
                            SuccCount++;
                        }
                        catch (Exception e) {
                            FailCount++;
                        }
                }

                // Use Linq to query documents
                //result.Result = StockInfos.FindById(1101);
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

        //實體檔案路徑
        public static String StockFileLocalDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\StockJson\";
        #region Sql
        //每年股息股利
        //public static String SqlDivi = @"select [Year],StockNum AS Num,Cash as CashDivi,Dividends as Divi,Cash+Dividends as AllDivi from StockDividends where [Year] >= 2014 order by 2,1 desc";
        //每季EPS
        //public static String SqlEPS = @"select Convert(nvarchar,[Year],10)+'-'+[Q]+'-'+[StockNum] as [Index],[Year],[Q],[StockNum] as Num,KeepEPS,EPS,Quarter3EPSAvg from StockEPS where YEAR >=2015 order by 4,2 desc";
        public static String SqlGetStockInfo = @"select top 2  StockNum as Num ,StockName as Name,Value,Industry,DebtRatio,InvestorRatio,Price,PERatio,IsPromisingEPS,
            IsGrowingUpEPS,IsAlwaysIncomeEPS,IsAlwaysPayDivi,IsOverDiffDivi,IsSafeValue,IsSafePB,IsSafeInvestor,IsSafeDebt,ModifyDate,
            FutureFromEPS,CurrFromEPS,PrevDiviFrom3YearAvgByEPS 
            from [Stock].dbo.CalStockInfo order by 1,2 desc";

        public static String SqlGetStockEPS_Divi = @" select  top 20 StockNum,Year,LastQ,TotalEPS,TotalDivi,EachDiviFromEPS from CalStockEPS_Divi order by 1,2 desc ";
        public static String SqlGetStockEachQAvgEPS = @" select  top 20 StockNum, Year,Q, Quarter3EPSAvg, IsCurrYear from CalStockEachQAvgEPS order by 1,2 desc,3 desc ";


        #endregion

        
        /// <summary>
        /// 批次產生大量資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStockInfo()
        {
            
            BaseResModel result = new BaseResModel();

            //取得年
            int PrevYear = DateTime.Now.AddYears(-1).Year;
            int CurrentYear = DateTime.Now.Year;

            //取得
            List<StockInfoResModel> StockInfos = new List<StockInfoResModel>();
            try
            {
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
                List<EPS_Divi> EPS_Divi = dt.ToList<EPS_Divi>();


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
                                    select new EPS_Divi()
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

                String FileName = AppDomain.CurrentDomain.BaseDirectory + @"\StockJson\";
                StockInfos.ForEach(t =>
                {
                    string json = JsonConvert.SerializeObject(t);
                    LogHelper.FileOverWrite(FileName + t.Num + ".json", json,false);
                });

                result.Code = ResponseCodeEnum.Success;
                result.Result = StockInfos;
            }
            catch(Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 測試用
        /// <summary>
        /// 測試用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenData()
        {
            //string ExecSQL = @"exec [GetStockInfo] ''";
            //取得
            List<StockInfoResModel> StockInfos = new List<StockInfoResModel>();
            Hashtable Conditions = new Hashtable();
            Conditions.Add("StockNum", "");
            int result = Common.DataAccess.Dao.EditUSP("GetStockInfo", Conditions, Common.Tools.Setting.ConnectionString("Stock"));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 對外服務

        /// <summary>
        /// 取得股票資訊
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult GetStockInfo(String StockNum)
        {
            BaseResModel result = new BaseResModel();

            try
            {
                
                int _StockNum = Int32.Parse(StockNum);
                    
                using (var db = new LiteDatabase(StockFileLocalDirPath + @"LazyStockDB.db"))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");
                    var StockInfo = StockInfos.FindById(_StockNum);
                    if (StockInfo == null)
                        return Json(new BaseResModel() { Code = ResponseCodeEnum.DataNotFound }, JsonRequestBehavior.AllowGet);

                    result.Result = StockInfo;
                }
                /*
                if(!System.IO.Directory.Exists(StockFileLocalDirPath))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(StockFileLocalDirPath));

                if (!System.IO.File.Exists(StockFileLocalDirPath+ StockNum + ".json")) 
                    return Json(new BaseResModel() { Code = ResponseCodeEnum.DataNotFound }, JsonRequestBehavior.AllowGet);

                String JsonString = System.IO.File.ReadAllText(StockFileLocalDirPath + StockNum + ".json");
                result.Code = ResponseCodeEnum.Success;
                result.Result = JsonConvert.DeserializeObject<StockInfoResModel>(JsonString);
                //JObject.Parse(JsonString);
                */
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