using Kingsun.Common;
using System;
using System.Text;

namespace CourseActivate.Core.Utility
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
        /// 按错误数据创建输出对象
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static KingResponse GetErrorResponse(string errorMsg, KingRequest request = null)
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

        public static KingResponse GetResponse(object data)
        {
            KingResponse res = new KingResponse();
            res.Success = true;
            res.Data = data;
            return res;
        }


        public static KingResponse RESGetResponse(KingRequest request, object data)
        {
            KingResponse response = new KingResponse();
            response.Success = true;
            response.ErrorMsg = "";
            if (request != null)
            {
                response.RequestID = request.ID;
            }
            else
            {
                response.RequestID = "";
            }
            response.Data = data;
            return response;
        }
    }
}
