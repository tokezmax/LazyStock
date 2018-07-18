using Common;
using LazyStock.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Extensions;

namespace LazyStock.Web.Services
{
    public class AuthServcies
    {

        public static HashSet<DDOSModel> ddos = new HashSet<DDOSModel>();
        public static bool IsOverQuery(HttpRequestBase req)
        {
            try
            {
                DDOSModel NewQuery = new DDOSModel();
                MemberInfoModel m = JsonConvert.DeserializeObject<MemberInfoModel>(req.Cookies["_UserInfo"].Value);
                if(m !=null)
                    NewQuery.userid = m.LineId;

                NewQuery.ip = req.UserHostAddress;
                NewQuery.QueryCount = 0;

                DDOSModel OldQuery=null;
                if (m != null)
                    OldQuery = ddos.Where(x => x.userid == m.LineId).FirstOrDefault();


                if (OldQuery == null)
                    OldQuery = ddos.Where(x => x.ip == req.UserHostAddress).FirstOrDefault();

                if (OldQuery == null) { 
                    ddos.Add(NewQuery);
                    return false;
                }

                if (NewQuery.Min == OldQuery.Min) {
                    if (OldQuery.QueryCount > 20) {
                        return true;
                    }
                    NewQuery.QueryCount = OldQuery.QueryCount + 1;
                }

                ddos.Remove(OldQuery);
                ddos.Add(NewQuery);
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static bool Islogin(HttpRequestBase req) {

            try {
                return IsSuccAuth(JsonConvert.DeserializeObject<MemberInfoModel>(req.Cookies["_UserInfo"].Value));
            }
            catch{// (Exception ex) {
                return false;
            }
            
        }

        public static String EncryptMemberInfo(MemberInfoModel req)
        {
            String res = "";
            try
            {
                res= CryptoHelper.Encrypt3DES(DateTime.Now.AddDays(1).ToString("o"), req.LineId);
            }
            catch
            {// (Exception ex) {
                res = "";
            }
            return res;
        }

        public static bool IsSuccAuth(MemberInfoModel req)
        {
            bool res = false;
            try{
                string MakeTime = CryptoHelper.Decrypt3DES(req.HashCode, req.LineId);
                DateTime LoginDate = DateTime.Parse(MakeTime);
                if (DateTime.Now < LoginDate)
                    res = false;
                res = true;
            }
            catch{// (Exception ex) {
                
            }
            return res;
        }

    }
}