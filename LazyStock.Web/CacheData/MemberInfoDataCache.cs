using LazyStock.Web.Models;
using System.Collections;

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