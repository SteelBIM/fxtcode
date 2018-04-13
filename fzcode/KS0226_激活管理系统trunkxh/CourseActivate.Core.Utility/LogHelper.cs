using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TestLog4Net
{
    public class LogHelper
    {
        public static log4net.ILog CASLog
        {
            get
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4Net.config"));
                return log4net.LogManager.GetLogger("CASLog");
            }
        }

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        #region static void WriteLog(Type t, Exception ex)

        public static void WriteLog(Type t, Exception ex)
        {
            CASLog.Error("Error", ex);
        }

        #endregion

        /// <summary>
        /// 写简单日志
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            CASLog.Info(info);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="error"></param>
        public static void Error(string error)
        {
            CASLog.Error(error);
        }


    }
}
