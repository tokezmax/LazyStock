using Common.Extensions;
using LazyStock.Web.Models;
using LazyStock.Web.Services;
using LiteDB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LazyStock.Web.Controllers
{
    public class LazySlotController : Controller
    {
        //實體檔案路徑
        public static String LazySlotDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LazySlotDB.db";

        #region 對外服務

        /// <summary>
        /// 取得股票資訊
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearMemberQuery()
        {
            var result = new BaseResModel<Object>();

            try
            {
                using (var conn = new LiteDatabase(LazySlotDBPath))
                {
                    var db = conn.GetCollection<MemberQueryRecordModel>("MemberQueryRecord");
                    var lists = db.FindAll();
                    foreach (var row in lists)
                        db.Delete(row.Index);
                }
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得股票資訊
        /// </summary>
        /// <returns></returns>
        public ActionResult GetNum()
        {
            var result = new BaseResModel<Object>();

            try
            {
                if (AuthServcies.IsOverQuery(Request))
                    throw new Exception("查詢過於繁複，請稍候再試!");

                MemberInfoModel MemberInfo = null;
                String LindId = "";
                try
                {
                    // Request.Cookies["_UserInfo"].Value
                    MemberInfo = AuthServcies.GetHeaderToMemberInfo(Request);
                    //JsonConvert.DeserializeObject<MemberInfoModel>(Request.Headers["_UserInfo"].ToGetValue<string>());
                    LindId = MemberInfo.LineId;
                }
                catch (Exception ex)
                {
                    MemberInfo = null;
                    LindId = "";
                }

                if (String.IsNullOrEmpty(LindId))
                    throw new Exception("此功能需登錄Line");

                using (var conn = new LiteDatabase(LazySlotDBPath))
                {
                    var db = conn.GetCollection<MemberQueryRecordModel>("MemberQueryRecord");
                    String Index = DateTime.Now.ToString("yyyyMMdd") + LindId;
                    var item = db.FindById(Index);
                    if (item != null)
                        throw new Exception("咳~咳~今天累了~明天請早~");

                    db.Insert(Index, new MemberQueryRecordModel() { Index = Index });
                }

                using (var conn = new LiteDatabase(LazySlotDBPath))
                {
                    // Get customer collection
                    var db = conn.GetCollection<HighQualityResModel>("HighQualityStock");
                    int index = new Random().Next(0, db.Count());
                    var item = db.FindAll().Select(x => new
                    {
                        x.StockNum,
                        x.StockName
                    }).ToList()[index];

                    result.Result = new
                    {
                        item.StockNum,
                        item.StockName
                    };
                }
                result.Code = ResponseCodeEnum.Success;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion 對外服務
    }
}