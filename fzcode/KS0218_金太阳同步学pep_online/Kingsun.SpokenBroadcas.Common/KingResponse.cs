using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kingsun.SpokenBroadcas.Common
{
    public class KingResponse
    {   /// <summary>
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
        public static object GetErrObj(string message)
        {
            object obj = new { Success = false, Data = "", Message = message };
            return obj;
        }

        public static object GetObj(object Data, string message = "")
        {
            object obj = new { Success = true, Data = Data, Message = message };
            return obj;
        }

        public static void ResponseWriteError(string message)
        {
            HttpContext.Current.Response.Write(JsonHelper.EncodeJson(GetErrObj(message)));
            HttpContext.Current.Response.End();
        }
        public static void ResponseWrite(object Data, string message = "")
        {
            HttpContext.Current.Response.Write(JsonHelper.EncodeJson(GetObj(Data, message)));
            HttpContext.Current.Response.End();
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
    }
}
