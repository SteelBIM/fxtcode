using System;
using System.Web.Caching;

namespace FxtDataAcquisition.Common
{
    /// <summary>
    /// 数据缓存辅助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获得缓存的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存的对象</returns>
        public static object GetCache(string key)
        {
            return HttpHelper.CurrentCache[key];
        }


        /// <summary>
        /// 设置缓存(无时间设置，永久)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void SetCache(string key, string value)
        {
            SetCache(key, value, null,Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 设置缓存(滑动过期)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="ts">缓存的时间</param>
        public static void SetCache(string key, string value, TimeSpan ts)
        {
            SetCache(key, value, null, ts);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="cd">数据依赖项</param>
        /// <param name="ts">缓存的时间</param>
        public static void SetCache(string key, string value, CacheDependency cd, TimeSpan ts)
        {
            HttpHelper.CurrentCache.Insert(key, value, cd, Cache.NoAbsoluteExpiration, ts);
        }


        /// <summary>
        /// 设置页面不被缓存
        /// </summary>
        public static void SetPageNoCache()
        {
            HttpHelper.CurrentResponse.Buffer = true;
            HttpHelper.CurrentResponse.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            HttpHelper.CurrentResponse.Expires = 0;
            HttpHelper.CurrentResponse.CacheControl = "no-cache";
            HttpHelper.CurrentResponse.AddHeader("Pragma", "No-Cache");
        }

        /// <summary>
        /// 获得剩余缓存大小(M)
        /// </summary>
        public string Remains
        {
            get
            {
                return string.Format("{0}M", HttpHelper.CurrentCache.EffectivePrivateBytesLimit / 1024 / 1024);
            }
        }
    }
}
