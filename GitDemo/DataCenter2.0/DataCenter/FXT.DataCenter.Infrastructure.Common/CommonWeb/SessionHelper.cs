using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    public class SessionHelper
    {
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        public static void AddSession(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
        }
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string GetSession(string strSessionName)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session[strSessionName] == null)
            {
                return "";
            }
            else
            {
                return HttpContext.Current.Session[strSessionName].ToString();
            }
        }
        /// <summary>
        /// 根据Session名删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void DelSessionBySName(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
        /// <summary>
        /// 清除所以Session
        /// </summary>
        public static void DelAllSession()
        {
            HttpContext.Current.Session.RemoveAll();
        }
        /// <summary>
        /// 添加特定类型Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        public static void AddSession<T>(string strSessionName, T strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
        }

        public static T GetSession<T>(string strSessionName)
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Session[strSessionName] == null)
            {
                return default(T);
            }
            else
            {
                return (T)HttpContext.Current.Session[strSessionName];
            }
        }
    }
}
