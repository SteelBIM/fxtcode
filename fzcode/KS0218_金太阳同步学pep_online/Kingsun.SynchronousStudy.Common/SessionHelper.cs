using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Kingsun.SynchronousStudy.Common
{
    public class SessionHelper 
    {
        private static HttpSessionState _session = HttpContext.Current.Session;

        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="key">Session名称</param>
        /// <param name="value">Session值</param>
        public static void SetSession(string key, string value)
        {
            //HttpContext con = HttpContext.Current;
           
            //con.Session[key] = value;
            _session[key] = value;
        }

        /// <summary>
        /// 获取数字类型Session
        /// </summary>
        /// <param name="key">Session名称</param>
        /// <returns></returns>
        public static int GetSessionNumber(string key)
        {
            int result = 0;
            if (_session[key] != null)
            {
                int.TryParse(_session[key].ToString(), out result);
            }
            return result;
        }

        /// <summary>
        /// 获取字符型Session
        /// </summary>
        /// <param name="key">Session名称</param>
        /// <returns></returns>
        public static string GetSessionString(string key)
        {
            string result = "";
            if (_session[key] != null)
            {
                result = _session[key].ToString();
            }
            return result;
        }

        /// <summary>
        /// 清理Seeion
        /// </summary>
        public static void Clear()
        {
            _session.Clear();
        }
    }
}
