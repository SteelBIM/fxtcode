using System.Collections.Generic;
using System.Linq;

namespace CAS.Common
{
    /// <summary>
    /// 用于更新本地应用程序缓存
    /// </summary>
    /// <returns></returns>
    public delegate void UpdateOtherProjectCache();
    /// <summary>
    /// 用于动态切换用redis缓存还是应用程序缓存
    /// </summary>
    public class PublicCacheCommon 
    {
        /// <summary>
        /// 返回指定的缓存，如果不存在将返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (RedisHelper.ExistsConfig)
            {
                return RedisHelper.Get<T>(key, null);
            }
            else
            {
                return CacheHelper.Get<T>(key);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(string key, T value)
        {

            if (value != null)
            {
                if (RedisHelper.ExistsConfig)
                {
                    RedisHelper.Set<T>(key, value, null);
                }
                else
                {
                    CacheHelper.Set<T>(key, value);
                }
            }
            else
            {
                setIsNullVal(key);
            }
        }
        /// <summary>
        /// 移除指定的缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public static void Remove(string key)
        {
            if (RedisHelper.ExistsConfig)
            {
                RedisHelper.Remove(key, null);
            }
            else
            {
                CacheHelper.Remove(key);
            }
        }
        /// <summary>
        /// 清除系统中当前产品应用程序中的所有缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public static void Clear()
        {
            if (RedisHelper.ExistsConfig)
            {
                RedisHelper.Clear(null);
            }
            else
            {
                CacheHelper.Clear();
            }
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">相对时间有效期多少久</param>
        public static void Set<T>(string key, T value, CacheTime cacheTime)
        {
            if (value != null)
            {
                if (RedisHelper.ExistsConfig)
                {
                    RedisHelper.Set<T>(key, value, cacheTime, null);
                }
                else
                {
                    CacheHelper.Set<T>(key, value, cacheTime);
                }
            }
            else
            {
                setIsNullVal(key);
            }
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">有效期类型</param>
        /// <param name="cacheExpiresType">是绝对时间还是相对时间</param>
        public static void Set<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {

            if (value != null)
            {
                if (RedisHelper.ExistsConfig)
                {
                    RedisHelper.Set<T>(key, value, cacheTime, cacheExpiresType, null);
                }
                else
                {
                    CacheHelper.Set<T>(key, value, cacheTime, cacheExpiresType);
                }
            }
            else
            {
                setIsNullVal(key);
            }
            
        }
        
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">有效期相对时间多久，单位：分钟</param>
        /// <param name="upOtherProjCache">要操作的指定前缀的其他项目的缓存（针对web应用程序缓存）</param>
        /// <param name="upOtherPrefix">是否同时操作配置里指定前缀的其他项目的缓存（针对redis）</param>
        /// <param name="otherPrefixs">要操作的指定前缀的其他项目的缓存,逗号分隔（针对redis）,在参数upOtherPrefix==true时起作用</param>
        public static void Set<T>(string key, T value, int cacheTime, UpdateOtherProjectCache upOtherProjCache = null, bool upOtherPrefix = false, string otherPrefixs = null)
        {

            if (value != null)
            {
                if (RedisHelper.ExistsConfig)
                {
                    RedisHelper.Set<T>(key, value, cacheTime, null);
                    if (upOtherPrefix)
                    {
                        List<string> list = RedisHelper.OtherPrefix;
                        if (!string.IsNullOrEmpty(otherPrefixs))
                        {
                            List<string> list2 = otherPrefixs.Split(',').ToList();
                            if (list2 != null && list2.Count() > 0)
                            {
                                list.AddRange(list2);
                            }
                        }
                        list = list.Where(_s => !string.IsNullOrEmpty(_s) && _s != RedisHelper.KeyPrefix).Distinct().ToList();
                        foreach (string p in list)
                        {
                            if (string.IsNullOrEmpty(p))
                            {
                                continue;
                            }
                            RedisHelper.Set<T>(key, value, cacheTime, p);
                        }
                    }
                }
                else
                {
                    CacheHelper.Set<T>(key, value, cacheTime);
                    if (upOtherProjCache != null)
                    {
                        upOtherProjCache();
                    }
                }
            }
            else
            {
                setIsNullVal(key);
            }

        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">时长，单位分钟</param>
        /// <param name="cacheExpiresType">相对时间还是绝对时间</param>
        public static void Set<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType)
        {
            if (value != null)
            {
                if (RedisHelper.ExistsConfig)
                {
                    RedisHelper.Set<T>(key, value, cacheTime, cacheExpiresType, null);
                }
                else
                {
                    CacheHelper.Set<T>(key, value, cacheTime, cacheExpiresType);
                }
            }
            else
            {
                setIsNullVal(key);
            }

        }
        static string IsNullVal_Key = "IsNullVal_Key";
        /// <summary>
        /// 存储value传null的key
        /// </summary>
        /// <param name="nullValKey"></param>
        static void setIsNullVal(string nullValKey)
        {
            string nowVal = getIsNullVal();
            if (!string.IsNullOrEmpty(nullValKey))
            {
                if (!(nowVal + ",").Contains(nullValKey + ","))
                {
                    nowVal = nowVal + (string.IsNullOrEmpty(nowVal) ? "" : ",") + nullValKey;
                    Set<string>(IsNullVal_Key, nowVal, 60 * 24 * 365, CacheExpiresType.Absolute);
                }
            }
        }
        /// <summary>
        /// 获取value传null的各key
        /// </summary>
        /// <returns></returns>
        static string getIsNullVal()
        {
            return Get<string>(IsNullVal_Key) ?? "";
        }
        /// <summary>
        /// 检查系统中是否存在指定的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains<T>(string key)
        {
            if (RedisHelper.ExistsConfig)
            {
                return RedisHelper.Contains<T>(key, null);
            }
            else
            {
                return CacheHelper.Contains<T>(key);
            }
        }
        /// <summary>
        /// 获取当前产品中的所有缓存信息(主要针对Redis)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> GetAllCache()
        {

            if (RedisHelper.ExistsConfig)
            {
                return RedisHelper.GetAllCache(null);
            }
            else
            {
                return CacheHelper.GetAllCache();
            }
        }
    }
}
