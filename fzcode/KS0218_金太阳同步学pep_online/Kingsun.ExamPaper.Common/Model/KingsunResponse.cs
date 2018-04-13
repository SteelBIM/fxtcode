using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Kingsun.ExamPaper.Common.Model
{
    class KingsunResponse
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

        //传入Json字符串
        public static HttpResponseMessage toJson(string message)
        {
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(message, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}
