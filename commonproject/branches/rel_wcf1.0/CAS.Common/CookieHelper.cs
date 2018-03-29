using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CAS.Common
{
    public class CookieHelper
    {

        #region 读取当前域名的主域

        /// <summary>
        /// 当前的cookie域
        /// </summary>
        public static string CookieDomain
        {
            get
            {
                //本地使用"localhost"为DOMAIN读不出来，需要置为空
                return System.Configuration.ConfigurationManager.AppSettings["domain"].ToString();
            }
        }

        #endregion

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="cookieKey"></param>
        public static void Remove(string cookieKey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieKey];

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);

                if (!string.IsNullOrEmpty(CookieDomain))
                    cookie.Domain = CookieDomain;

                cookie.Path = "/";

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void RemoveAll()
        {
            int cnt = HttpContext.Current.Request.Cookies.Count;
            for (int i=0;i<cnt;i++)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[i];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    if (!string.IsNullOrEmpty(CookieDomain))
                        cookie.Domain = CookieDomain;
                    cookie.Path = "/";
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }

        public static void Set(string cookieKey, string cookieValue, int minutes)
        {
            Set(cookieKey,cookieValue,DateTime.Now.AddMinutes(minutes));
        }

        /// <summary>
        /// 修改Cookie的值 如果不存在键则创建
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <param name="cookieValue"></param>
        /// <param name="expires">过期时间</param>
        public static void Set(string cookieKey, string cookieValue, DateTime? expires)
        {
            HttpCookie cookie = new HttpCookie(cookieKey, cookieValue);

            if (!string.IsNullOrEmpty(CookieDomain))
                cookie.Domain = CookieDomain;
            cookie.Path = "/";

            if (expires != null && expires.Value != DateTime.MinValue)
                cookie.Expires = expires.Value;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 修改Cookie的值 如果不存在键则创建 (不设置过期时间)
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <param name="cookieValue"></param>
        public static void Set(string cookieKey, string cookieValue)
        {
            Set(cookieKey, cookieValue, DateTime.MinValue);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <returns></returns>
        public static string Get(string cookieKey)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.Cookies[cookieKey] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookieKey].Value);
            }
            else
            {
                return "";
            }
        }

        public static HttpCookie GetCookie(string cookieKey)
        {
            return HttpContext.Current.Request.Cookies[cookieKey];
        }

        /// <summary>
        /// 设置Cookie (不设置过期时间)
        /// </summary>
        /// <param name="cookieKey"></param>
        /// <param name="itemKey">子项</param>
        /// <param name="cookieValue">子项的值</param>
        public static void Set(string cookieKey, string itemKey, string cookieValue)
        {
            Set(cookieKey, itemKey, cookieValue, null);
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieKey">键值</param>
        /// <param name="itemKey">子项</param>
        /// <param name="cookieValue">子项的值</param>
        /// <param name="expires">过期时间</param>
        public static void Set(string cookieKey, string itemKey, string cookieValue, DateTime? expires)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[cookieKey];

            cookie[itemKey] = cookieValue;

            if (!string.IsNullOrEmpty(CookieDomain))
                cookie.Domain = CookieDomain;
            cookie.Path = "/";

            if (expires != null)
                cookie.Expires = expires.Value;

        }
    }

}

