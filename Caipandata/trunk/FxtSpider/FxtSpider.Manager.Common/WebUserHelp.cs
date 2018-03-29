using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FxtSpider.Manager.Common
{
    public static class WebUserHelp
    {
        /// <summary>
        /// 登录超时
        /// </summary>
        public const int NotLogin = 1;
        /// <summary>
        /// 无权限
        /// </summary>
        public const int NotRight = 2;
        /// <summary>
        /// 系统错误
        /// </summary>
        public const int SysError = 3;
        public static bool CheckUser(int operationType,out int errorType,out string message)
        {
            message = "";
            errorType = 0;
            return true;
        }
    }
}