using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract.API
{
    /// <summary>
    /// 服务处理结果输出
    /// </summary>
    public class APIResponse
    {
        /// <summary>
        /// 请求方法
        /// </summary>
        public string RequestID
        {
            get;
            set;
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success
        {
            get;
            set;
        }

        /// <summary>
        /// 业务数据
        /// </summary>
        public object Data
        {
            get;
            set;
        }
        
        /// <summary>
        /// 系统时间
        /// </summary>
        public DateTime SystemTime
        {
            get;
            set;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 接口返回对象
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static APIResponse GetResponse(object data=null)
        {
            APIResponse res = new APIResponse();
            res.Success = true;
            res.Data = data;
            res.SystemTime = DateTime.UtcNow;
            return res;
        }

        /// <summary>
        /// 按错误数据创建输出对象,并记录日志
        /// </summary>
        /// <param name="errorCode">错误编码</param>
        /// <param name="logLevel">日志级别</param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static APIResponse GetErrorResponse(ErrorCodeEnum errorCode, LogLevelEnum logLevel, Exception ex = null)
        {
            #region 接口错误返回实体
            APIResponse response = new APIResponse();
            response.Success = false;
            response.ErrorMsg = EnumHelper.GetDescription(errorCode);
            response.ErrorCode = (int)errorCode;
            //if (request != null)
            //{
            //    response.RequestID = request.ID;
            //}
            //else
            //{
            //    response.RequestID = "";
            //}
            response.RequestID = "";
            response.Data = null;
            response.SystemTime = DateTime.UtcNow;
            #endregion

            #region 接口异常记录日志
            switch (logLevel)
            {
                case LogLevelEnum.Debug:
                    Log4NetHelper.Debug(LoggerType.ApiExceptionLog, response.ErrorMsg, ex);
                    break;
                case LogLevelEnum.Error:
                    Log4NetHelper.Error(LoggerType.ApiExceptionLog, response.ErrorMsg, ex);
                    break;
                case LogLevelEnum.Info:
                    Log4NetHelper.Info(LoggerType.ApiExceptionLog, response.ErrorMsg, ex);
                    break;
                case LogLevelEnum.Fatal:
                    Log4NetHelper.Fatal(LoggerType.ApiExceptionLog, response.ErrorMsg, ex);
                    break;
                case LogLevelEnum.Warn:
                    Log4NetHelper.Warn(LoggerType.ApiExceptionLog, response.ErrorMsg, ex);
                    break;
                default:
                    break;
            }
            #endregion

            return response;
        }

#pragma warning disable CS1573 // 参数“msg”在“APIResponse.GetErrorResponse(ErrorCodeEnum, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 按错误数据创建输出对象
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static APIResponse GetErrorResponse(ErrorCodeEnum errorCode,string msg="")
#pragma warning restore CS1573 // 参数“msg”在“APIResponse.GetErrorResponse(ErrorCodeEnum, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            APIResponse response = new APIResponse();
            response.Success = false;
            response.ErrorMsg = EnumHelper.GetDescription(errorCode)+","+msg;
            response.ErrorCode = (int)errorCode;
            //if (request != null)
            //{
            //    response.RequestID = request.ID;
            //}
            //else
            //{
            //    response.RequestID = "";
            //}
            response.RequestID = "";
            response.Data = null;
            response.SystemTime = DateTime.UtcNow;
            return response;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“APIResponse.GetErrorResponse(string)”的 XML 注释
        public static APIResponse GetErrorResponse(string errMsg)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“APIResponse.GetErrorResponse(string)”的 XML 注释
        {
            APIResponse response = new APIResponse();
            response.Success = false;
            response.ErrorMsg = errMsg;
            response.RequestID = "";
            response.Data = null;
            response.SystemTime = DateTime.UtcNow;
            return response;
        }
    }
}
