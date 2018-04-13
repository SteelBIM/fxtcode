using System;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace Kingsun.IBS.Api
{
    public class KingsunResponse
    {
        public static HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        public static HttpResponseMessage GetErrorResult(string message)
        {
            object obj = new { Success = false, Data = "", Message = message };
            return toJson(obj);
        }
        public static HttpResponseMessage GetResult(object data, string message = null)
        {
            object obj = new { Success = true, data = data, Message = message };
            return toJson(obj);
        }
        #region 同步学是小写"data"
        public static HttpResponseMessage GetSyncErrorResult(string message)
        {
            object obj = new { Success = false, data = "", Message = message };
            return toJson(obj);
        }
        public static HttpResponseMessage GetSyncResult(object Data, string message = null)
        {
            object obj = new { Success = true, data = Data, Message = message };
            return toJson(obj);
        }
        #endregion
    }
}