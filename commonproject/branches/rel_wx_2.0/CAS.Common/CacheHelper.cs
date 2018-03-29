using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Runtime;
namespace CAS.Common
{
    /// <summary>
    /// 缓存的过期时间
    /// </summary>
    public enum CacheExpiresType
    {

        /// <summary>
        /// 绝对时间
        /// </summary>
        Absolute,

        /// <summary>
        /// 相对时间
        /// </summary>
        Sliding
    }

    /// <summary>
    /// 缓存时长
    /// </summary>
    public enum CacheTime
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = -1,

        /// <summary>
        /// 永不移除
        /// </summary>
        NotRemovable = 4,

        /// <summary>
        /// 长时间
        /// </summary>
        Long = 3,

        /// <summary>
        /// 正常时间
        /// </summary>
        Normal = 2,

        /// <summary>
        /// 短时间
        /// </summary>
        Short = 1,
    }

    public class CacheHelper 
    {
        /// <summary>
        /// 移除指定开头的所有缓存
        /// </summary>
        /// <param name="startText"></param>
        public static void RemoveBySearch(string startText)
        {
            if (string.IsNullOrEmpty(startText))
                return;

            List<string> removeKeys = new List<string>();

            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                string cacheKey = elem.Key.ToString();

                if (cacheKey.StartsWith(startText, StringComparison.OrdinalIgnoreCase))
                    removeKeys.Add(cacheKey);
            }

            foreach (string removeKey in removeKeys)
            {
                try
                {
                    HttpRuntime.Cache.Remove(removeKey);
                }
                catch { }
            }
        }


        /// <summary>
        /// 移除指定开头的所有缓存
        /// </summary>
        /// <param name="startText"></param>
        public static void RemoveBySearch(params string[] startTexts)
        {
            if (startTexts == null || startTexts.Length == 0)
                return;

            List<string> removeKeys = new List<string>();

            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                string cacheKey = elem.Key.ToString();

                foreach (string startText in startTexts)
                {
                    if (cacheKey.IndexOf(startText, StringComparison.OrdinalIgnoreCase) == 0)
                        removeKeys.Add(cacheKey);
                }
            }

            foreach (string removeKey in removeKeys)
            {
                try
                {
                    HttpRuntime.Cache.Remove(removeKey);
                }
                catch { }
            }
        }




        /// <summary>
        /// 返回指定开头的所有缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startText"></param>
        /// <returns></returns>
        public static IList<T> GetBySearch<T>(string startText)
        {

            IList<T> objects = new List<T>();

            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                string key = elem.Key.ToString();

                if (key.StartsWith(startText, StringComparison.OrdinalIgnoreCase))
                {
                    object value = HttpRuntime.Cache[key];

                    if (value != null && value is T)
                        objects.Add((T)value);
                }
            }

            return objects;
        }

        public static CacheDependency GetCacheDependencyFromKey(string key)
        {
            return new System.Web.Caching.CacheDependency(null, new string[1] { key });
        }

        /// <summary>
        /// 返回指定的缓存，如果不存在将导致异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache[key];
        }

        public static bool WhetherCache(string key) 
        {
            bool success = false;
            try
            {
                string cache = HttpRuntime.Cache[key].ToString();
                if (cache != null)
                {
                    success = true;
                }
            }
            catch (Exception ex )
            {
                return success;
            }
            return success;
        }

        /// <summary>
        /// 尝试返回指定的缓存
        /// </summary>
        /// <typeparam name="T">缓存内容的类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <param name="value">缓存的内容</param>
        /// <returns>是否存在这个缓存</returns>
        public static bool TryGetValue<T>(string key, out T value)
        {

            object temp = HttpRuntime.Cache[key];

            if (temp != null && temp is T)
            {
                value = (T)temp;
                return true;
            }

            value = default(T);
            return false;
        }

        /// <summary>
        /// 检查系统中是否存在指定的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains<T>(string key)
        {
            object value = HttpRuntime.Cache[key];

            if (value == null)
                return false;

            else if (value is T)
                return true;

            return false;
        }

        /// <summary>
        /// 增加一个缓存
        /// </summary>
        /// <typeparam name="T">缓存内容的类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <param name="value">缓存的内容</param>
        [Obsolete("慎用Add，因为两次调用Add第二次不会把第一次的值覆盖")]
        public static void Add<T>(string key, T value)
        {
            Add(key, value, CacheTime.Default, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Add<T>(string key, T value, CacheTime cacheTime)
        {
            Add(key, value, cacheTime, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            Add(key, value, cacheTime, cacheExpiresType, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, CacheItemPriority.NotRemovable, null);
        }

        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, CacheItemPriority.NotRemovable, null);
        }

        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback callback)
        {
            //Remove(key);

            DateTime absoluteExpiration = GetAbsoluteExpiration(cacheTime, cacheExpiresType);
            TimeSpan slidingExpiration = GetSlidingExpiration(cacheTime, cacheExpiresType);

            if (cacheTime == CacheTime.NotRemovable)
                cacheItemPriority = CacheItemPriority.NotRemovable;

            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, cacheItemPriority, callback);
        }

        public static void Add<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback callback)
        {
            //Remove(key);

            DateTime absoluteExpiration;
            if (cacheExpiresType == CacheExpiresType.Sliding)
                absoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration;
            else
                absoluteExpiration = DateTime.Now.AddMinutes(cacheTime);

            TimeSpan slidingExpiration;
            if (cacheExpiresType == CacheExpiresType.Absolute)
                slidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
            else
                slidingExpiration = TimeSpan.FromMinutes(cacheTime);


            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, cacheItemPriority, callback);
        }

        /// <summary>
        /// 设置一个缓存的值。如果缓存已存在，将自动覆盖之前的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(string key, T value)
        {
            Set(key, value, CacheTime.Default, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Set<T>(string key, T value, CacheTime cacheTime)
        {
            Set(key, value, cacheTime, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Set<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            Set(key, value, cacheTime, cacheExpiresType, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Set<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies)
        {
            Set(key, value, cacheTime, cacheExpiresType, dependencies, CacheItemPriority.NotRemovable, null);
        }

        public static void Set<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority)
        {
            Set(key, value, cacheTime, cacheExpiresType, dependencies, cacheItemPriority, null);
        }

        public static void Set<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback callback)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, cacheItemPriority, callback);
        }

        public static void Set<T>(string key, T value, int cacheTime)
        {
            Set(key, value, cacheTime, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Set<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType)
        {
            Set(key, value, cacheTime, cacheExpiresType, null, CacheItemPriority.NotRemovable, null);
        }

        public static void Set<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies)
        {
            Set(key, value, cacheTime, cacheExpiresType, dependencies, CacheItemPriority.NotRemovable, null);
        }

        public static void Set<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority)
        {
            Set(key, value, cacheTime, cacheExpiresType, dependencies, cacheItemPriority, null);
        }

        public static void Set<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback callback)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, cacheItemPriority, callback);
        }





        /// <summary>
        /// 清除系统中所有缓存
        /// </summary>
        public static void Clear()
        {
            List<string> keys = new List<string>();

            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                keys.Add(elem.Key.ToString());
            }

            foreach (string key in keys)
            {
                try
                {
                    HttpRuntime.Cache.Remove(key);
                }
                catch { }
            }
        }

        /// <summary>
        /// 移除指定的缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public static void Remove(string key)
        {
            try
            {
                HttpRuntime.Cache.Remove(key);
            }
            catch { }
        }


        private static DateTime GetAbsoluteExpiration(CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            if (cacheTime == CacheTime.NotRemovable || cacheExpiresType == CacheExpiresType.Sliding)
                return System.Web.Caching.Cache.NoAbsoluteExpiration;

            switch (cacheTime)
            {
                case CacheTime.Short:
                    return DateTime.Now.AddMinutes(20);
                default:
                case CacheTime.Default:
                case CacheTime.Normal:
                    return DateTime.Now.AddMinutes(60 * 24);//一天
                case CacheTime.Long:
                    return DateTime.Now.AddMinutes(60 * 24 * 365);//一年
            }
        }

        private static TimeSpan GetSlidingExpiration(CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            if (cacheTime == CacheTime.NotRemovable || cacheExpiresType == CacheExpiresType.Absolute)
                return System.Web.Caching.Cache.NoSlidingExpiration;

            switch (cacheTime)
            {
                case CacheTime.Short:
                    return TimeSpan.FromMinutes(20);
                default:
                case CacheTime.Default:
                case CacheTime.Normal:
                    return TimeSpan.FromMinutes(60 * 24);//一天
                case CacheTime.Long:
                    return TimeSpan.FromMinutes(60 * 24 * 365);//一年
            }
        }
    }
}
