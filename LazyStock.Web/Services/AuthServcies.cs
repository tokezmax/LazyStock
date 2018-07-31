using Common;
using Common.Extensions;
using LazyStock.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Web;

namespace LazyStock.Web.Services
{
    public class AuthServcies
    {
        public static ConcurrentDictionary<String, DDOSModel> ddos = new ConcurrentDictionary<String, DDOSModel>();

        public static MemberInfoModel GetHeaderToMemberInfo(HttpRequestBase req)
        {
            try
            {
                return JsonConvert.DeserializeObject<MemberInfoModel>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(req.Headers["_UserInfo"].ToGetValue<string>())));
            }
            catch { }

            try
            {
                return JsonConvert.DeserializeObject<MemberInfoModel>(HttpUtility.UrlDecode(req.Cookies["_UserInfo"].Value));
            }
            catch { }
            return null;
        }

        public static bool IsOverQuery(HttpRequestBase req)
        {
            try
            {
                MemberInfoModel MemberInfo = null;
                String UseKey = req.UserHostAddress;
                DDOSModel OldQuery = null;
                DDOSModel NewQuery = new DDOSModel();
                NewQuery.ip = req.UserHostAddress;
                NewQuery.QueryCount = 0;

                try
                {
                    //req.Cookies["_UserInfo"].Value
                    MemberInfo = GetHeaderToMemberInfo(req);
                    NewQuery.userid = MemberInfo.LineId;
                }
                catch
                {
                    MemberInfo = null;
                }

                //cookie是否存在 (是否有login)
                if (MemberInfo != null)
                {
                    if (ddos.ContainsKey(MemberInfo.LineId))
                        OldQuery = ddos[MemberInfo.LineId];
                }
                else
                {
                    if (ddos.ContainsKey(req.UserHostAddress))
                        OldQuery = ddos[req.UserHostAddress];
                }

                //沒有舊資料，代表0次查詢，以新的為主
                UseKey = (String.IsNullOrEmpty(NewQuery.userid) ? NewQuery.ip : NewQuery.userid);
                if (OldQuery == null)
                {
                    ddos.TryAdd(UseKey, NewQuery);
                    return false;
                }

                //有新有舊，count+1
                if (NewQuery.Min == OldQuery.Min)
                {
                    NewQuery.QueryCount = OldQuery.QueryCount + 1;
                    if (OldQuery.QueryCount > int.Parse(Common.Tools.Setting.AppSettings("QueryStockInfoCount")))
                    {
                        return true;
                    }
                }

                ddos.TryUpdate(UseKey, NewQuery, OldQuery);
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static int Islogin(HttpRequestBase req)
        {
            return IsSuccAuth(GetHeaderToMemberInfo(req));
        }

        public static String EncryptMemberInfo(MemberInfoModel req)
        {
            String res = "";
            try
            {
                res = CryptoHelper.Encrypt3DES(DateTime.Now.AddDays(1).ToString("o"), req.LineId);
            }
            catch
            {// (Exception ex) {
                res = "";
            }
            return res;
        }

        //0 驗証失敗/未登錄 ，1 有效，2驗証碼過期
        public static int IsSuccAuth(MemberInfoModel req)
        {
            int res = 0;
            try
            {
                string MakeTime = CryptoHelper.Decrypt3DES(req.HashCode.Replace(" ", "").Replace("/n", "").Replace("/r", ""), req.LineId);
                DateTime LoginDate = DateTime.Parse(MakeTime);
                if (DateTime.Now > LoginDate)
                    res = 2;
                else
                    res = 1;
            }
            catch
            {// (Exception ex) {
            }
            return res;
        }
    }
}