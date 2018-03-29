using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace OpenPlatform.Framework.Utils.Log
{
    public class NLogHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //private readonly Logger _logger;

        //private NLogHelper(Logger logger)
        //{
        //    this._logger = logger;
        //}

        //public NLogHelper(string name)
        //    : this(LogManager.GetLogger(name))
        //{
        //}

        public static void Debug(string msg)
        {
            Logger.Debug(msg);
        }

        public static void Info(string msg)
        {
            Logger.Info(msg);
        }

        public static void Trace(string msg)
        {
            Logger.Trace(msg);
        }

        public static void Error(ErrorLog errorLog)
        {
            var errorStr = "请求地址：{0}\r\n"
                       + "请求IP：{1}\r\n"
                       + "用户ID：{2}\r\n"
                       + "城市ID：{3}\r\n"
                       + "公司ID：{4}\r\n"
                       + "异常信息：{5}\r\n"
                       + "异常来源：{6}\r\n"
                       + "堆栈跟踪：{7}\r\n"
                       + "附加信息：{8}\r\n";

            var error = string.Format(errorStr, errorLog.Url, errorLog.Ip, errorLog.UserId, errorLog.CityId, errorLog.FxtCompanyId, errorLog.Exception.Message, errorLog.Exception.Source,errorLog.Exception.ToString(), errorLog.CustomError);

            Logger.Error(error);
        }

        public static void Fatal(string msg)
        {
            Logger.Fatal(msg);
        }


    }

}
