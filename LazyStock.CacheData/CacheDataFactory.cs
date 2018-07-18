
namespace LazyStock.CacheData
{
    public class CacheDataFactory
    {
        static CacheDataFactory()
        {
            InitCacheData("");
        }

        public static void InitCacheData(string cacheName)
        {
            switch (cacheName)
            {
                case "":
                    MemberInfoData = new MemberInfoDataCache2();
                    break;
                case "MemberInfoData":
                    MemberInfoData = new MemberInfoDataCache2();
                    break;
            }
        }

        public static void ReLoadCacheData(string cacheName)
        {
            switch (cacheName)
            {
                case "":
                    MemberInfoData.CreateData();
                    break;
                case "SmsCorpData":
                    MemberInfoData.CreateData();
                    break;
            }
        }
        
        /// <summary>
        /// 會員基本資料
        /// </summary>
        public static MemberInfoDataCache2 MemberInfoData { get; set; }
    }
}
