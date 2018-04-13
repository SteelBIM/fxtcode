using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CourseActivate.Core.Utility
{
    /// <summary>
    /// Cookie相关辅助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 保存一个Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="ts">时间间隔</param>
        public static void SetCookie(string cookieName, string cookieValue, int day)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Expires = DateTime.Now.AddDays(day);
            HttpContext.Current.Response.Cookies.Add(cookie);
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
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
