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
        public static String LazySlotDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LazySlotDB.db";
        #endregion


        /// <summary>
        /// 接收(Json)更新股價
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiveByDeilyPrice(ReceiveByDeilyPriceReqModel ReqParam)
        {
            BaseResModel<Object> result = new BaseResModel<Object>();
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
                            var DbStockPrice = StockInfos.FindById(items.StockNum);
                            if (DbStockPrice == null)
                                continue;


                            DbStockPrice.Price = Math.Round( items.StockPrice,2);
                            DbStockPrice.PriceModifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            StockInfos.Update(items.StockNum, DbStockPrice);
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
        /// [接收股票訊息][修改LiteDB]
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiveByStockInfo(ReceiveByStockInfoReqModel ReqParam)
        {
            BaseResModel<Object> result = new BaseResModel<Object>();
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
                            StockInfos.Delete(items.StockNum);
                            StockInfos.Insert(items.StockNum, items);
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
        /// [接收高品質高股訊息][修改LiteDB]
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiveByHighQualityList(ReceiveByHighQualityListReqModel ReqParam)
        {
            BaseResModel<HighQualityResModel> result = new BaseResModel<HighQualityResModel>();
            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                int SuccCount = 0;
                int FailCount = 0;
                // Open database (or create if not exits)
                using (var db = new LiteDatabase(LazySlotDBPath))
                {
                    // Get customer collection
                    var HighQualityStock = db.GetCollection<HighQualityResModel>("HighQualityStock");

                    var lists = HighQualityStock.FindAll();
                    foreach (var row in lists)
                        HighQualityStock.Delete(row.StockNum);

                    for (int i = 0; i < ReqParam.HighQualityStock.Count; i++)
                    {
                        HighQualityResModel items = ReqParam.HighQualityStock[i];
                        try
                        {
                            HighQualityStock.Insert(items.StockNum, items);
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





    }
}