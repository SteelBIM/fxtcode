using System;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace Kingsun.ExamPaper.WebAPI
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
        public static HttpResponseMessage GetResult(object Data, string message = null)
        {
            object obj = new { Success = true, Data = Data, Message = message };
            return toJson(obj);
        }
    }
}