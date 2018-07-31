using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Common.CacheObject
{
    public class CacheItems
    {
        private static readonly ObjectCache cache = MemoryCache.Default;

        //加入Lock機制限定同一Key同一時間只有一個Callback執行
        private const string AsyncLockPrefix = "$$CacheAsyncLock#";

        /// <summary>
        /// 取得每個Key專屬的鎖定對象
        /// </summary>
        /// <param name="key">Cache保存號碼牌</param>
        /// <returns></returns>
        private static object GetAsyncLock(string key)
        {
            //取得每個Key專屬的鎖定對象（object）
            string asyncLockKey = AsyncLockPrefix + key;
            lock (cache)
            {
                if (cache[asyncLockKey] == null) cache.Add(asyncLockKey,
                    new object(),
                    new CacheItemPolicy()
                    {
                        SlidingExpiration = new TimeSpan(0, 10, 0)
                    });
            }
            return cache[asyncLockKey];
        }

        /// <summary>
        /// 取得ObjectCache資料
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)cache[key];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 建立可以被Cache的資料(注意：非Thread-Safe)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback">被Cache的查詢資料函數</param>
        /// <param name="key">Cache保存號碼牌</param>
        /// <param name="absExpire">有效期限；單位 DateTimeOffset</param>
        /// <param name="forceRefresh">是否清除Cache，重新查詢</param>
        /// <returns></returns>
        public static void Add<T>(Func<T> callback, string key, DateTimeOffset absExpire, bool forceRefresh = false) where T : class
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            lock (GetAsyncLock(key))
            {
                T res = cache[cacheKey] as T;
                //是否清除Cache，強制重查
                if (res != null && forceRefresh)
                {
                    cache.Remove(cacheKey);
                    res = null;
                }
                if (res == null)
                {
                    res = callback();
                    cache.Add(cacheKey, res, new CacheItemPolicy()
                    {
                        AbsoluteExpiration = absExpire
                    });
                }
            }
        }

        /// <summary>
        /// 建立可以被Cache的資料(注意：非Thread-Safe)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCache">被Cache的查詢資料物件</param>
        /// <param name="key">Cache保存號碼牌</param>
        /// <param name="absExpire">有效期限；單位 DateTimeOffset</param>
        /// <param name="forceRefresh">是否清除Cache，重新查詢</param>
        /// <returns></returns>
        public static void Add(object objectToCache, string key, DateTimeOffset absExpire, bool forceRefresh = false)
        {
            string cacheKey = key;
            //取得每個Key專屬的鎖定對象
            lock (GetAsyncLock(key))
            {
                object res = cache[cacheKey];
                //是否清除Cache，強制重查
                if (res != null && forceRefresh)
                {
                    cache.Remove(cacheKey);
                    res = null;
                }
                if (res == null)
                {
                    res = objectToCache;
                    cache.Add(cacheKey, res, new CacheItemPolicy()
                    {
                        AbsoluteExpiration = absExpire
                    });
                }
            }
        }

        /// <summary>
        /// 確認objectToCache是否存在
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return cache.Get(key) != null;
        }

        /// <summary>
        /// Gets all cached items as a list by their key.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAll()
        {
            return cache.Select(keyValuePair => keyValuePair.Key).ToList();
        }
    }
}