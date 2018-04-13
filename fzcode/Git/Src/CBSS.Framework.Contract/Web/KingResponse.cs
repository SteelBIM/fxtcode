using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 服务处理结果输出
    /// </summary>
    public class KingResponse
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

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“KingResponse.ErrorCode”的 XML 注释
        public int ErrorCode { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“KingResponse.ErrorCode”的 XML 注释

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
        /// <param name="data"></param>
        /// <returns></returns>
        public static KingResponse GetResponse(object data)
        {
            KingResponse res = new KingResponse();
            res.Success = true;
            res.Data = data;
            return res;
        }

#pragma warning disable CS1573 // 参数“request”在“KingResponse.GetErrorResponse(string, KingRequest)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 按错误数据创建输出对象
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static KingResponse GetErrorResponse(string errorMsg, KingRequest request = null)
#pragma warning restore CS1573 // 参数“request”在“KingResponse.GetErrorResponse(string, KingRequest)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            KingResponse response = new KingResponse();
            response.Success = false;
            response.ErrorMsg = errorMsg;
            if (request != null)
            {
                response.RequestID = request.ID;
            }
            else
            {
                response.RequestID = "";
            }
            response.Data = null;
            return response;
        }

        /// <summary>
        /// 按错误数据创建输出对象
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMsg"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static KingResponse GetErrorResponse(int errorCode, string errorMsg, KingRequest request = null)
        {
            KingResponse response = new KingResponse();
            response.Success = false;
            response.ErrorMsg = errorMsg;
            response.ErrorCode = errorCode;
            if (request != null)
            {
                response.RequestID = request.ID;
            }
            else
            {
                response.RequestID = "";
            }
            response.Data = null;
            return response;
        }
    }
}
