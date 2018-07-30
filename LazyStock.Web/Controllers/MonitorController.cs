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
using LazyStock.Web.Services;

namespace LazyStock.Web.Controllers
{
    public class MonitorController : Controller
    {
       
        public ActionResult Member()
        {

            BaseResModel<List<MemberInfoModel>> result = new BaseResModel<List<MemberInfoModel>>();

            try
            {
                String LazyStockMemberDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\MemberInfo.db";
                MemberInfoModel member = new MemberInfoModel();
                using (var db = new LiteDatabase(LazyStockMemberDBPath))
                {
                    var MemberInfos = db.GetCollection<MemberInfoModel>("MemberInfo");
                    
                    result.Result = MemberInfos.FindAll().ToList();
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

        public ActionResult DDOS()
        {

            BaseResModel<List<DDOSModel>> result = new BaseResModel<List<DDOSModel>>();
            
            try
            {
                
                String LazyStockMemberDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\MemberInfo.db";
                result.Result = AuthServcies.ddos.Values.ToList<DDOSModel>();
                result.Code = ResponseCodeEnum.Success;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ClearDDOS()
        {

            BaseResModel<List<DDOSModel>> result = new BaseResModel<List<DDOSModel>>();

            try
            {
                AuthServcies.ddos.Clear();
                result.Result = AuthServcies.ddos.Values.ToList<DDOSModel>();
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