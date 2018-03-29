using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using CAS.Entity.DBEntity;

namespace CAS.Common.MVC4
{
    public class WebCacheBase
    {
        /* 采用System.Web.Caching的优点是可以方便的控制缓存的过期时间和通知策略。
         * 但是在缓存对象类型更多和业务压力增长时应当考虑不同的缓存策略。
         * 例如产品级的设置可能会不定期改变，可以使用cache, 7*24小时的过期策略，
         * 系统(站点)级别的设置相对比较稳定，可以使用哈希表做永久缓存，手动更新的策略。
         * Norman 2013-03-01
        */
        //Cache读写锁
        private static ReaderWriterLockSlim cacheLock;
        /// <summary>
        /// 用字典做缓存
        /// </summary>
        public static Dictionary<string, object> cache = new Dictionary<string, object>();
        /// <summary>
        /// 取数据的过程一般比较长，需要在锁内执行
        /// </summary>
        protected delegate object DelegateGetData();
        static WebCacheBase()
        {
            //默认禁用递归锁定（LockRecursionPolicy.NoRecursion）
            cacheLock = new ReaderWriterLockSlim();
        }


        public static void RemoveCache(string name)
        {
            cacheLock.EnterWriteLock();
            try
            {
                cache.Remove(name);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        protected static T SafeGetCache<T>(string name) where T : class
        {
            T t = default(T);
            cacheLock.EnterReadLock();
            try
            {
                if (cache.Keys.Contains(name))
                {
                    t = cache[name] as T;
                }
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
            return t;
        }
        /// <summary>
        /// 通常用于耗时比较长，需要在锁中执行的情况，比如：通过数据库取得数据。
        /// </summary>
        protected static void SafeAddCache(string name, DelegateGetData getData)
        {
            cacheLock.EnterWriteLock();
            try
            {
                if (cache.Keys.Contains(name))
                {
                    cache[name] = getData();
                }
                else
                {
                    cache.Add(name, getData());
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
    }

    /// <summary>
    /// 全局缓存，用于缓存多个项目都可能用到的资源
    /// </summary>
    public class GlobleCache
    {
        /// <summary>
        /// 数据库连接的缓存
        /// </summary>
        public class ConnectionStringCache : WebCacheBase
        {
            public static void Add(string name, string value)
            {
                SafeAddCache(name, delegate { return value; });
            }

            public static string Get(string name)
            {
                return SafeGetCache<string>(name);
            }
        }

        /// <summary>
        /// 中心数据库的城市表缓存
        /// </summary>
        public class CenterDBCityTable : WebCacheBase
        {
            const string CacheName = "CenterDBCityTable";

            public static void Add(List<CityTable> value)
            {
                SafeAddCache(CacheName, delegate { return value; });
            }

            public static List<CityTable> Get()
            {
                return SafeGetCache<List<CityTable>>(CacheName);
            }
            //缓存置空
            public static void Reset()
            {
                RemoveCache(CacheName);
            }
        }
    }
}
