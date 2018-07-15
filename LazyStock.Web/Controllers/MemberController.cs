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
    public class MemberController : Controller
    {

        //實體檔案路徑
        //public static String StockFileLocalDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\StockJson\";
        public static String LazyStockMemberDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\MemberInfo.db";


        #region 測試用
        /// <summary>
        /// 測試用
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult CallbackFromLine(String code)
        {
            //string ExecSQL = @"exec [GetStockInfo] ''";
            //取得
            //List<StockInfoResModel> StockInfos = new List<StockInfoResModel>();
            //Hashtable Conditions = new Hashtable();
            //Conditions.Add("StockNum", "");
            //int result = Common.DataAccess.Dao.EditUSP("GetStockInfo", Conditions, Common.Tools.Setting.ConnectionString("Stock"));
            BaseResModel result = new BaseResModel();

            if (String.IsNullOrEmpty(code))
                return Redirect("/default.aspx?error=" + "登錄失敗");

            //顯示，測試用
            Response.Write("<br/> code : " + code);
            //從Code取回toke
            var token = isRock.LineLoginV21.Utility.GetTokenFromCode(
                code,
                "1593644840",  //TODO:請更正為你自己的 client_id
                "760934e2ecec09b4e73af7f18a0abfcc", //TODO:請更正為你自己的 client_secret
                "http://localhost:2458/Member/CallbackFromLine");  //TODO:請更正為你自己的 callback url
                                                                   //顯示，測試用
                                                                   //(注意這是範例，token不該用明碼傳遞，也不該出現在用戶端，你應該自行記錄在資料庫或ServerSite session中)
                                                                   //Response.Write("<br/> token : " + token.access_token);
                                                                   //利用token順手取得用戶資訊
            var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);
            //Response.Write("<br/> user: " + user.displayName);


            using (var db = new LiteDatabase(LazyStockMemberDBPath))
            {

                MemberInfo member = new MemberInfo()
                {
                    displayName = user.displayName,
                    userId = user.userId,
                    pictureUrl = user.pictureUrl,
                    statusMessage = user.statusMessage
                };

                // Get customer collection
                var MemberInfos = db.GetCollection<MemberInfo>("MemberInfo");
                if (MemberInfos.FindById(user.userId) == null)
                    MemberInfos.Upsert(user.userId, member);
            }
            return Redirect("/default.aspx?error=" + JsonConvert.SerializeObject(user));
        }

        #endregion
    }
}
