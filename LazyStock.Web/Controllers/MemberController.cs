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

using LazyStock.Web.CacheData;
using LazyStock.Web.Services;

namespace LazyStock.Web.Controllers
{
    public class MemberController : Controller
    {

        //實體檔案路徑
        public static String LazyStockMemberDBPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\MemberInfo.db";

        [HttpGet]
        public ActionResult CallbackFromLine(String code)
        {

            //BaseResModel result = new BaseResModel();

            if (String.IsNullOrEmpty(code))
                return Redirect("/default.aspx?error=" + "登錄失敗");

            //顯示，測試用
            
            //從Code取回toke
            var token = isRock.LineLoginV21.Utility.GetTokenFromCode(
                code,
                "1593644840",  //TODO:請更正為你自己的 client_id
                "760934e2ecec09b4e73af7f18a0abfcc", //TODO:請更正為你自己的 client_secret
                "https://lazystock.azurewebsites.net/Member/CallbackFromLine");  //TODO:請更正為你自己的 callback url
                                                                   //顯示，測試用
                                                                   //(注意這是範例，token不該用明碼傳遞，也不該出現在用戶端，你應該自行記錄在資料庫或ServerSite session中)
                                                                   //Response.Write("<br/> token : " + token.access_token);
                                                                   //利用token順手取得用戶資訊
            var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);
            MemberInfoModel member = new MemberInfoModel();
            using (var db = new LiteDatabase(LazyStockMemberDBPath))
            {
                member = new MemberInfoModel()
                {
                    Id = Common.Tools.TokenGenerator.GetTimeStamp(3),
                    UserName = user.displayName,
                    LineId = user.userId,
                    PicUrl = user.pictureUrl,
                    Permission = "1"
                };
                member.HashCode = AuthServcies.EncryptMemberInfo(member);

                var MemberInfos = db.GetCollection<MemberInfoModel>("MemberInfo");
                if (MemberInfos.FindById(member.Id) == null)
                    MemberInfos.Upsert(member.Id, member);
            }

            HttpCookie AutoCookie = new HttpCookie("_UserInfo");
            AutoCookie.Value = JsonConvert.SerializeObject(member);
            AutoCookie.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(AutoCookie);
            
            return Redirect("/");
        }

    }
}
