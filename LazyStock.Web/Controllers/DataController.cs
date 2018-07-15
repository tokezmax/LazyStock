using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Extensions;
using Newtonsoft.Json;
using LazyStock.Web.Models;
using System.Collections;
using LiteDB;

namespace LazyStock.Web.Controllers
{
    public class DataController : Controller
    {

        //實體檔案路徑
        //public static String StockFileLocalDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\StockJson\";
        public static String LazyStockDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LazyStockDB.db";


        #region 測試用
        /// <summary>
        /// 測試用
        /// </summary>
        /// <returns></returns>
        /*
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
        */
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
                    
                using (var db = new LiteDatabase(LazyStockDBPath))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");
                    var StockInfo = StockInfos.FindById(_StockNum);
                    if (StockInfo == null)
                        return Json(new BaseResModel() { Code = ResponseCodeEnum.DataNotFound }, JsonRequestBehavior.AllowGet);

                    result.Result = StockInfo;
                }
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