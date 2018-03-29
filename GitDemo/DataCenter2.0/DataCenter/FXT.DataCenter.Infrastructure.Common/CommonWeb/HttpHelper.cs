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
    /// Http��ظ�����
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// ��õ�ǰPage����
        /// </summary>
        public static Page CurrentPage
        {
            get
            {
                return (Page)HttpContext.Current.Handler;
            }
        }

        /// <summary>
        /// ��õ�ǰCache����
        /// </summary>
        public static Cache CurrentCache
        {
            get
            {
                return HttpContext.Current.Cache;
            }
        }

        /// <summary>
        /// ��õ�ǰRequset����
        /// </summary>
        public static HttpRequest CurrentRequest
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
        
        /// <summary>
        /// ��õ�ǰResponse����
        /// </summary>
        public static HttpResponse CurrentResponse
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }

        /// <summary>
        /// ��õ�ǰServer����
        /// </summary>
        public static HttpServerUtility CurrentServer
        {
            get
            {
                return HttpContext.Current.Server;
            }
        }

        /// <summary>
        /// ��õ�ǰSession����
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
