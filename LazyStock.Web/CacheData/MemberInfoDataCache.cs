using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Common.DataAccess;
using Common.Tools;
using Common.Extensions;
using LazyStock.Web.Models;

namespace LazyStock.Web.CacheData
{
    public class MemberInfoDataCache 
    {
        private static MemberInfoModel _defobj = new MemberInfoModel { Id = "", Email = "", UserName = "", PicUrl = "", LineId = "", Permission = "1" };
        public static Hashtable self = new Hashtable();

        public MemberInfoDataCache()
        {
            
        }
    }
}
