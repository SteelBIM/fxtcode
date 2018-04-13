using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.PSO
{
    public class ProjectConstant
    {
        private static string _cookieName;

        public static string CookieName
        {
            get
            {
                if (string.IsNullOrEmpty(_cookieName))
                {
                    _cookieName = System.Configuration.ConfigurationManager.AppSettings["cookieName"].ToString();
                }
                return _cookieName;
            }

        }

        //uums根目录
        private static string _uumsRootUrl;

        /// <summary>
        /// uums根目录
        /// </summary>
        public static string UumsRootUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_uumsRootUrl))
                {
                    _uumsRootUrl = System.Configuration.ConfigurationManager.AppSettings["uumsRoot"].ToString();
                }
                return _uumsRootUrl;
            }
        }
    }
}
