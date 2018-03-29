using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Caching;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// Http相关辅助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 获得当前Page对象
        /// </summary>
        public static Page CurrentPage
        {
            get
            {
                return (Page)HttpContext.Current.Handler;
            }
        }

        /// <summary>
        /// 获得当前Cache对象
        /// </summary>
        public static Cache CurrentCache
        {
            get
            {
                return HttpContext.Current.Cache;
            }
        }

        /// <summary>
        /// 获得当前Requset对象
        /// </summary>
        public static HttpRequest CurrentRequest
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
        
        /// <summary>
        /// 获得当前Response对象
        /// </summary>
        public static HttpResponse CurrentResponse
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }

        /// <summary>
        /// 获得当前Server对象
        /// </summary>
        public static HttpServerUtility CurrentServer
        {
            get
            {
                return HttpContext.Current.Server;
            }
        }

        /// <summary>
        /// 获得当前Session对象
        /// </summary>
        public static HttpSessionState CurrentSession
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }
    }
}
