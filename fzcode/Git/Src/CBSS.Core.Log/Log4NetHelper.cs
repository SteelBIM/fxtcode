using System;
using System.IO;
using System.Text;
using log4net;
using Newtonsoft.Json;
using CBSS.Core.Config;

namespace CBSS.Core.Log
{
    /// <summary>
    /// 日志记录，并写入DB
    /// </summary>
    public static class Log4NetHelper
    {
        static Log4NetHelper()
        {
            //初始化log4net配置
            var config = CachedConfigContext.Current.ConfigService.GetConfig("log4net");
            //重写log4net配置里的连接字符串
            config = config.Replace("{connectionString}", CachedConfigContext.Current.DaoConfig.Log);
            var ms = new MemoryStream(Encoding.Default.GetBytes(config));
            log4net.Config.XmlConfigurator.Configure(ms);
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="loggerType"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public static void Debug(LoggerType loggerType, object message, Exception e)
        {
            var logger = LogManager.GetLogger(loggerType.ToString());
            logger.Debug(SerializeObject(message), e);
        }

        /// <summary>
        /// 系统异常，但不影响系统继续运行的信息
        /// </summary>
        /// <param name="loggerType"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public static void Error(LoggerType loggerType, object message, Exception e)
        {
            var logger = LogManager.GetLogger(loggerType.ToString());
            logger.Error(SerializeObject(message), e);
        }

        /// <summary>
        /// 一般日志,关键系统参数的回显、后台服务的初始化状态、需要开发者确认的信息
        /// </summary>
        /// <param name="loggerType"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public static void Info(LoggerType loggerType, object message, Exception e=null)
        {
            var logger = LogManager.GetLogger(loggerType.ToString());
            logger.Info(SerializeObject(message), e);
        }

        /// <summary>
        /// 重大错误,影响系统正常运行的信息
        /// </summary>
        /// <param name="loggerType"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public static void Fatal(LoggerType loggerType, object message, Exception e)
        {
            var logger = LogManager.GetLogger(loggerType.ToString());
            logger.Fatal(SerializeObject(message), e);
        }

        /// <summary>
        /// 一般异常,在业务合理范围内的异常信息
        /// </summary>
        /// <param name="loggerType"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public static void Warn(LoggerType loggerType, object message, Exception e)
        {
            var logger = LogManager.GetLogger(loggerType.ToString());
            logger.Warn(SerializeObject(message), e);
        }

        private static object SerializeObject(object message)
        {
            if (message is string || message == null)
                return message;
            else
                return JsonConvert.SerializeObject(message, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }

    public enum LoggerType
    {
        /// <summary>
        /// 系统异常
        /// </summary>
        ServiceExceptionLog,
        /// <summary>
        /// Web层
        /// </summary>
        WebExceptionLog,
        /// <summary>
        /// Api层
        /// </summary>
        ApiExceptionLog,
        /// <summary>
        /// Fs层
        /// </summary>
        FsExceptionLog,
    }
}
