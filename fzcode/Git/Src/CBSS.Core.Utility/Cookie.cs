using System;
using System.Web;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class Cookie
    {
        /// <summary>
        /// 取Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        /// <summary>
        /// 取Cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            var httpCookie = Get(name);
            if (httpCookie != null)
                return httpCookie.Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            Cookie.Remove(Cookie.Get(name));
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Cookie.Remove(HttpCookie)”的 XML 注释
        public static void Remove(HttpCookie cookie)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Cookie.Remove(HttpCookie)”的 XML 注释
        {
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
                Cookie.Save(cookie);
            }
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expiresHours"></param>
        public static void Save(string name, string value, int expiresHours = 0)
        {
            var httpCookie = Get(name);
            if (httpCookie == null)
                httpCookie = Set(name);

            httpCookie.Value = value;
            Cookie.Save(httpCookie, expiresHours);
        }


#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Cookie.Save(HttpCookie, int)”的 XML 注释
        public static void Save(HttpCookie cookie, int expiresHours = 0)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Cookie.Save(HttpCookie, int)”的 XML 注释
        {
            string domain = Fetch.ServerDomain;
            string urlHost = HttpContext.Current.Request.Url.Host.ToLower();
            if (domain != urlHost)
                cookie.Domain = domain;

            if (expiresHours > 0)
                cookie.Expires = DateTime.Now.AddHours(expiresHours);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Cookie.Set(string)”的 XML 注释
        public static HttpCookie Set(string name)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Cookie.Set(string)”的 XML 注释
        {
            return new HttpCookie(name);
        }
    }
}
