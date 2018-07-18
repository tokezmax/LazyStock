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

namespace LazyStock.Web.Controllers
{
    public class SettingController : Controller
    {
        //實體檔案路徑
        public static String StockFileLocalDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\";

        public ActionResult SetOption(String Key,String Val)
        {
            BaseResModel<Object> result = new BaseResModel<Object>();
            try
            {
                // Open database (or create if not exits)
                using (var db = new LiteDatabase(StockFileLocalDirPath + @"Setting.db"))
                {
                    // Get customer collection
                    var SystemOptions = db.GetCollection<SystemOptionModel>("SystemOption");
                    SystemOptionModel SystemOption = new SystemOptionModel();
                    SystemOption.Key = Key;
                    SystemOption.Val = Val;
                    SystemOptions.Upsert(SystemOption.Key, SystemOption);
                }

                result.Code = ResponseCodeEnum.Success;
                result.Message = "[Key]" + Key + "[Val]" + Val;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Query(String Groups, String Category, String Keys)
        {
            BaseResModel<Object> result = new BaseResModel<Object>();

            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                Common.Tools.Setting.GetConfig(Groups, Category, Keys);
                result.Code = ResponseCodeEnum.Success;
                result.Result = Common.Tools.Setting.GetConfig(Groups, Category, Keys);
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(String Groups, String Category, String Keys, String Value)
        {
            BaseResModel<Object> result = new BaseResModel<Object>();
            
            try {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                Common.Tools.Setting.SetConfig(Groups, Category, Keys, Value);
                result.Code = ResponseCodeEnum.Success;
            }
            catch (Exception e)
            {
                result.Code = ResponseCodeEnum.Failed;
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Load()
        {
            BaseResModel<Object> result = new BaseResModel<Object>();

            try
            {
                Common.Tools.AuthHelper.IsPowerAdmin(Request);
                Common.Tools.Setting.ReLoad();
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