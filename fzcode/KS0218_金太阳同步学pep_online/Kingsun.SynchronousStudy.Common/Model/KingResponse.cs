using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
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

        /// <summary>
        /// 按错误数据获取错误输出字符串
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string GetErrorResponseString(string errorMsg, KingRequest request = null)
        {
            return JsonHelper.EncodeJson(GetErrorResponse(errorMsg, request));
        }

        public static KingResponse GetResponse(KingRequest request, object data)
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

        #region webapi 返回
        public static HttpResponseMessage HttpMsg(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                str = JsonHelper.EncodeJson(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        public static HttpResponseMessage GetErrorResponseMessage(string errorMsg, KingRequest request = null)
        {
            object obj = new { Success = false, data = "", Message = errorMsg };

            //var obj = GetErrorResponse(errorMsg, request);
            return HttpMsg(obj);
        }
        public static HttpResponseMessage GetResponseMessage(KingRequest request, object data)
        {
            object obj = new { Success = true, data = data, Message = "" };
            //var obj = GetResponse(request, data);
            return HttpMsg(obj);
        }
        #endregion webapi 返回
    }
}
