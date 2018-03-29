using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXTExcelAddIn
{
    public class LogHelper
    {
        private const string SourceName = "FXTExcelAddin";
        /// <summary>
        /// 异常处理写日志
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message">自定义的消息</param>
        public static void Error(Exception e, string message = null)
        {
            
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }            
            CASException ex = new CASException();
            ex.Source = e.Source;
            ex.StackTrace = e.StackTrace;
            ex.MethodName = e.TargetSite == null ? "" : e.TargetSite.Name;
            ex.ClassName = e.TargetSite == null ? "" : e.TargetSite.DeclaringType.FullName;            
            string messageStr = "";
            if (!string.IsNullOrEmpty(message))
            {
                messageStr = "\r\nMessage2:" + message;
            }
            string errstr = "Message:{0}\r\n"
                            + "Source:{1}\r\n"
                            + "StackTrace:{2}\r\n"
                            + "Method:{3}\r\n"
                            + "Class:{4}\r\n";
            string error = string.Format(errstr
                                        , e.Message + messageStr, ex.Source, ex.StackTrace, ex.MethodName, ex.ClassName
                                        );
            EventLog.WriteEntry(SourceName, error);
        }
    }


    public class CASException 
    {      

        private string className;
        /// <summary>
        /// 错误的类名
        /// </summary>

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string source;
        /// <summary>
        /// 错误源
        /// </summary>

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        private string stackTrace;
        /// <summary>
        /// 错误出处
        /// </summary>

        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value; }
        }

        private string methodName;
        /// <summary>
        /// 出错方法
        /// </summary>

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

    }

}
