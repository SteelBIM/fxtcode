using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingsun.SynchronousStudy.App.Common
{
    public class ProjectConstant
    {
        private static string _connectionString = string.Empty;
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunConnectionStr"].ConnectionString;
                }
                return _connectionString;
            }
        }

        private static string _appID;

        public static string AppID
        {
            get
            {
                if (string.IsNullOrEmpty(_appID))
                {
                    _appID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
                }
                return _appID;
            }

        }

        private static string _messageToken;

        public static string MessageToken
        {
            get
            {
                if (string.IsNullOrEmpty(_messageToken))
                {
                    _messageToken = System.Configuration.ConfigurationManager.AppSettings["MessageToken"];
                }
                return _messageToken;
            }

        }

        private static string _messageModel;

        public static string MessageModel
        {
            get
            {
                if (string.IsNullOrEmpty(_messageModel))
                {
                    _messageModel = System.Configuration.ConfigurationManager.AppSettings["MessageModel"];
                }
                return _messageModel;
            }
        }

        private static string _payToken;

        public static string PayToken
        {
            get
            {
                if (string.IsNullOrEmpty(_payToken))
                {
                    _payToken = System.Configuration.ConfigurationManager.AppSettings["PayToken"];
                }
                return _payToken;
            }
        }


        private static string _payTokenToApple;

        public static string PayTokenToApple
        {
            get
            {
                if (string.IsNullOrEmpty(_payTokenToApple))
                {
                    _payTokenToApple = System.Configuration.ConfigurationManager.AppSettings["MessageTokenToApple"];
                }
                return _payTokenToApple;
            }
        }

        private static string _payURL;

        public static string PayURL
        {
            get
            {
                if (string.IsNullOrEmpty(_payURL))
                {
                    _payURL = System.Configuration.ConfigurationManager.AppSettings["PayUrl"];
                }
                return _payURL;
            }
        }

        private static string _orderOverTime;

        /// <summary>
        /// 订单过期时间（分钟）
        /// </summary>
        public static string OrderOverTime
        {
            get
            {
                if (string.IsNullOrEmpty(_orderOverTime))
                {
                    _orderOverTime = System.Configuration.ConfigurationManager.AppSettings["OrderOverTime"];
                }
                return _orderOverTime;
            }
        }

        private static int _messageCountMax = 0;

        public static int MessageCountMax
        {
            get
            {
                if (_messageCountMax == 0)
                {
                    _messageCountMax = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MessageCountMax"]);
                }
                return _messageCountMax;
            }
        }

    }
}