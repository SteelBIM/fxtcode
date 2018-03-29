using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// Cookie相关辅助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 保存一个Cookie(临时性)
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="ts">时间间隔</param>
        public static void SetCookie(string cookieName, string cookieValue)
        {
            SetCookie(cookieName, cookieValue, TimeSpan.Zero);
        }


        /// <summary>
        /// 保存一个Cookie(持久性)
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="ts">时间间隔</param>
        public static void SetCookie(string cookieName, string cookieValue, TimeSpan ts)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            if (ts != TimeSpan.Zero)
                cookie.Expires = DateTime.Now.Add(ts);
            HttpHelper.CurrentResponse.Cookies.Add(cookie);
        }

        /// <summary>
        /// 取得CookieValue
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>Cookie的值</returns>
        public static string GetCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];

            if (cookie != null)
                return cookie.Value;
            else
                return null;
        }

        /// <summary>
        /// 清除CookieValue
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpHelper.CurrentResponse.Cookies.Add(cookie);
            }
        }
    }
}
