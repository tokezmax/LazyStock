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
        public static String StockFileLocalDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\StockJson\";

        public ActionResult SetOption(String Key,String Val)
        {
            BaseResModel result = new BaseResModel();
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
    }
}