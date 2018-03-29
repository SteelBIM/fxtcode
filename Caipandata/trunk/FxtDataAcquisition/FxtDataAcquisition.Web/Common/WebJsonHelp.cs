using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FxtDataAcquisition.Common;

namespace FxtDataAcquisition.Web.Common
{
    public static class WebJsonHelp
    {
        /// <summary>
        /// 输出json格式(ajax统一输出json格式)
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="result">请求是否成功</param>
        /// <param name="errorType">请求失败时的错误类型(0:无,1(WebUserHelp.NotLogin):未登陆,2(WebUserHelp.NotRight):无权限,3(WebUserHelp.SysError):系统错误)</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static string MvcResponseJson(this string detail, int result = 1, int errorType = 0, string message = "")
        {
            StringBuilder sb = new StringBuilder("{\"result\":");
            sb.Append(result).Append(",\"errorType\":");
            sb.Append(errorType).Append(",\"message\":\"");
            sb.Append(message.EncodeField()).Append("\",\"detail\":");
            if (!string.IsNullOrEmpty(detail) && (detail[0].Equals('[') || detail[0].Equals('{')))
            {
                sb.Append(detail).Append("}");
            }
            else
            {
                sb.Append("\"").Append(detail).Append("\"}");
            }
            string resultStr = sb.ToString();
            return resultStr;
        }
    }
}