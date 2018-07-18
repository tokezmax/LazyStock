using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Extensions;
using Newtonsoft.Json;
using LazyStock.Web.Models;
using System.Collections;
using LazyStock.Web.Services;
using LiteDB;

namespace LazyStock.Web.Controllers
{
    public class DataController : Controller
    {

        //實體檔案路徑
        public static String LazyStockDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LazyStockDB.db";


        #region 對外服務

        /// <summary>
        /// 取得股票資訊
        /// </summary>
        /// <param name="StockNum"></param>
        /// <returns></returns>
        public ActionResult GetStockInfo(String StockNum)
        {
            BaseResModel<StockInfoResModel> result = new BaseResModel<StockInfoResModel>();

            try
            {
                if (AuthServcies.IsOverQuery(Request))
                    throw new Exception("查詢過於繁複，請稍候再試!");

                using (var db = new LiteDatabase(LazyStockDBPath))
                {
                    // Get customer collection
                    var StockInfos = db.GetCollection<StockInfoResModel>("StockInfo");
                    var StockInfo = StockInfos.FindById(StockNum);
                    if (StockInfo == null)
                        return Json(new BaseResModel<StockInfoResModel>() { Code = ResponseCodeEnum.DataNotFound }, JsonRequestBehavior.AllowGet);

                    result.Result = StockInfo;

                    if (!AuthServcies.Islogin(Request)) {
                        result.Result.CurrFromEPS = null;
                        result.Result.FutureFromEPS = null;
                        result.Result.PrevDiviFrom3YearAvgByEPS = null;
                        result.Result.EstimateStableDivi = null;
                        result.Result.EstimateUnstableDivi = null;
                        result.Result.EstimateStablePrice5 = null;
                        result.Result.EstimateUnstablePrice5 = null;
                        result.Result.EstimateStablePrice7 = null;
                        result.Result.EstimateUnstablePrice7 = null;
                        result.Result.EstimateStablePrice10 = null;
                        result.Result.EstimateUnstablePrice10 = null;
                    }
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