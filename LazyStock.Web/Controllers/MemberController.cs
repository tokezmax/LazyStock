using LazyStock.Web.Models;
using LazyStock.Web.Services;
using LiteDB;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;

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

            //從Code取回toke
            var token = isRock.LineLoginV21.Utility.GetTokenFromCode(
                code,
                Common.Tools.Setting.AppSettings("LineAuthId"),
                Common.Tools.Setting.AppSettings("LineAuthClientSecret"),
                Common.Tools.Setting.AppSettings("LineAuthCallBackUrl"));

            var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);
            MemberInfoModel member = new MemberInfoModel();
            using (var db = new LiteDatabase(LazyStockMemberDBPath))
            {
                var MemberInfos = db.GetCollection<MemberInfoModel>("MemberInfo");
                var a = MemberInfos.FindOne(Query.EQ("LineId", user.userId));

                member.Id = (a == null ? Common.Tools.TokenGenerator.GetTimeStamp(3) : a.Id);
                member.UserName = HttpUtility.UrlEncode(user.displayName);
                member.LineId = user.userId;
                member.PicUrl = user.pictureUrl;
                member.Permission = "1";
                member.HashCode = AuthServcies.EncryptMemberInfo(member);
                member.LastloginDate = Common.Tools.TokenGenerator.GetTimeStamp(0);
                MemberInfos.Upsert(member.Id, member);
            }

            HttpCookie AutoCookie = new HttpCookie("_UserInfo");
            AutoCookie.Value = JsonConvert.SerializeObject(member);
            AutoCookie.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(AutoCookie);

            String v = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(member)));

            return Redirect(Common.Tools.Setting.AppSettings("LineAuthAfterRedirect") + "/?v=" + v);
        }
    }
}