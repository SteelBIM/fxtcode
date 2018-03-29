using FxtDataAcquisition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BaiduAPI.WebAPI.GeocodingAPI
{
    public static class GeocodingManager
    {
        private static string _serviceUrl = "http://api.map.baidu.com/geocoder/v2/";

        public static GeocodingResponse Search(GeocodingRequest request)
        {
            StringBuilder sb = new StringBuilder(_serviceUrl);
            sb.Append("?ak=LfyRpG6vY5V0ZnGX0oh0MfqA");
            sb.Append("&output=" + request.output.ToString());
            sb.Append("&location="+request.location);
            sb.Append("&pois=" + request.pois);
            
            var result = "";
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                //这里url要组装安全标记等参数
                byte[] responseData = client.UploadData(sb.ToString(), "POST", new byte[0]);
                result = Encoding.UTF8.GetString(responseData);
            }
            GeocodingResponse re = JsonHelp.ParseJSONjss<GeocodingResponse>(result);
            return re;
        }
    }
}
