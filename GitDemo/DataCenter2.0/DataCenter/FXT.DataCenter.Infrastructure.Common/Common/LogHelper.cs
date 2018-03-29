using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;


namespace FXT.DataCenter.Infrastructure.Common.Common
{
    public class LogHelper
    {
        private static object m_LogObj = new object();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //private static readonly Logger Logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);

        public static void Debug(string msg)
        {
            Logger.Debug(msg);
        }

        public static void Info(string msg)
        {
            Logger.Info(msg);
        }

        public static void SaveLog(string message)
        {
            SaveLog(message, true);
        }

        public static void SaveLog(string message, Boolean saveLog)
        {
            try
            {
                if (!saveLog) return;
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string fileName = path + "\\log.txt";
                lock (m_LogObj)
                {
                    if (File.Exists(fileName))
                    {
                        FileInfo fileInfo = new FileInfo(fileName);
                        if (fileInfo.Length > 1024 * 1024 * 3)
                        {
                            string newFileName = path + "\\log" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";
                            File.Move(fileName, newFileName);
                        }
                    }
                    message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff: ") + message;
                    File.AppendAllText(fileName, String.Concat(message, "\r\n"));
                }
            }
            catch { }
        }

        public static void Trace(string msg)
        {
            Logger.Trace(msg);
        }

        public static void WriteLog(string url, string userip, string userid, int cityid, int fxtcompanyid, Exception ex,
            string customErr = null)
        {
            string infoStr = "Url：{0}\r\n"
                             + "UserIP：{1}\r\n"
                             + "UserID：{2}\r\n"
                             + "CityID：{3}\r\n"
                             + "FXTCompanyID：{4}\r\n"
                             + "Message：{5}\r\n"
                             + "Source：{6}\r\n"
                             + "StackTrace：{7}\r\n";
            var err = ex.ToString();
            if (!string.IsNullOrEmpty(customErr)) err = err + "*********" + customErr;
            var info = string.Format(infoStr, url, userip, userid, cityid, fxtcompanyid, err, ex.Source, ex.ToString());

            Logger.Error(info);
        }

        public static void Fatal(string msg)
        {
            Logger.Fatal(msg);
        }
    }

}
