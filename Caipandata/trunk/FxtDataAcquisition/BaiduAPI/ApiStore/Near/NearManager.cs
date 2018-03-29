using FxtDataAcquisition.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BaiduAPI
{
    /// <summary>
    /// 周边搜索
    /// </summary>
    public static class NearManager
    {
        private static string _serviceUrl = "http://apis.baidu.com/apistore/location/near";
        public static NearResponse Search(NearRequest requst)
        {
            StringBuilder sb = new StringBuilder(_serviceUrl + "?");
            sb.Append("keyWord=" + HttpUtility.HtmlEncode(requst.keyWord));
            sb.Append("&location=" + requst.location.lng + "," + requst.location.lat);
            sb.Append("&radius=" + requst.radius+"m");
            sb.Append("&sort_rule=" + requst.sort_rule);
            sb.Append("&number=" + requst.number);
            sb.Append("&page=" + requst.page);
            sb.Append("&sort_rule=" + requst.sort_rule);
            sb.Append("&output=" + requst.output.ToString());
            sb.Append("&coord_type=" + requst.coord_type.ToString());
            sb.Append("&out_coord_type=" + requst.out_coord_type.ToString());
            if (requst.tag != TagType.全部)
            {
                sb.Append("&tag=" + HttpUtility.HtmlEncode(requst.tag.ToString()));
            }
            if (!string.IsNullOrEmpty(requst.cityName))
            {
                sb.Append("&cityName=" + requst.cityName);
            }

            HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(sb.ToString());
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", requst.apikey);
            HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            string value = Reader.ReadToEnd();
            NearResponse res = JsonHelp.ParseJSONjss<NearResponse>(value);
            Reader.Dispose();
            return res;
        }
    }
}
