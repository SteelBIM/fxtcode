using FxtDataAcquisition.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace BaiduAPI
{
    public static class PlaceAPIManger
    {
        private static string _serviceUrl = "http://api.map.baidu.com/place/v2/search";
        public static PlaceResponse SearchPOI(PlaceRequest request)
        {
            StringBuilder sb = new StringBuilder(_serviceUrl);
            sb.Append("?ak=" + request.ak);
            sb.Append("&query=" + HttpUtility.UrlEncode(request.query));
            int a = (int)request.scope;
            sb.Append("&scope=" + a.ToString());
            sb.Append("&output=" + request.output.ToString());
            sb.Append("&region=" + HttpUtility.UrlEncode(request.region));

            if (!string.IsNullOrEmpty(request.tag))
            {
                sb.Append("&tag=" + request.tag);
            }
            if (!string.IsNullOrEmpty(request.filter))
            {
                sb.Append("&filter=" + request.filter);
            }
            if (request.page_size > 0)
            {
                sb.Append("&page_size=" + request.page_size.ToString());
            }
            if (request.page_num > 0)
            {
                sb.Append("&page_num=" + request.page_num.ToString());
            }
            if (!string.IsNullOrEmpty(request.sn))
            {
                sb.Append("&sn=" + request.sn);
                sb.Append("&timestamp=" + request.timestamp);
            }
            if (!string.IsNullOrEmpty(request.location))
            {
                sb.Append("&location=" + request.location);
            }
            if (request.radius > 0)
            {
                sb.Append("&radius=" + request.radius.ToString());
            }

            var result = "";
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                //这里url要组装安全标记等参数
                byte[] responseData = client.UploadData(sb.ToString(), "POST", new byte[0]);
                result = Encoding.UTF8.GetString(responseData);
            }
            PlaceResponse re = JsonHelp.ParseJSONjss<PlaceResponse>(result);
            return re;
        }
    }
}
