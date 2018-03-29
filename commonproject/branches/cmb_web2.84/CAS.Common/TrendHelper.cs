using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace CAS.Common
{
    public class TrendHelper
    {
        public static string GetTrendZS(string title, string labels, int min, int max, string values)
        {
            return GetTrendPub(title, labels, min, max, values);
        }


        public static string GetTrend(string title, string labels, int min, int max, string values)
        {
            int remove = min % 1000; //补余数凑个整数
            min = min - remove;
            remove = max % 1000;
            max = max + (1000 - remove);
            return GetTrendPub(title, labels, min, max, values);
        }

        public static string GetTrendPub(string title, string labels, int min, int max, string values)
        {
            int vtop = max + (max - min);//最大值
            int vbottom = min - (max - min);//最小值
            if (vbottom < 0) vbottom = 0;
            int step = Convert.ToInt32((vtop - vbottom) / 6);
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.AppendFormat("{{\"title\":{{\"text\": \"{0}\",\"style\": \"font-size:12px;color:#333333;\"}},", title);
            str.AppendFormat("\"x_axis\":{{\"labels\":{{\"labels\":[{0}],\"size\": 11}},", labels);
            str.Append("\"colour\": \"#636363\",\"grid-colour\": \"#e8e8e8\",\"steps\": 1,\"3d\": 0,\"offset\": true},");
            str.Append("\"y_axis\":{\"tick-length\": 0,\"labels\":{\"size\": 11},");
            str.AppendFormat("\"colour\": \"#636363\",\"grid-colour\": \"#e8e8e8\",\"steps\": {0},\"min\": {1},\"max\": {2},\"3d\": 0,\"offset\": 1}},", step, vbottom, vtop);
            //str.Append("\"x_legend\":{\"text\":\"asdf\",\"style\":\"{font-size:12px;font-family:Tahoma;color:#736AFF;}\"},");
            str.AppendFormat("\"elements\":[{{\"width\": 2,\"colour\": \"#F24D00\",\"values\":[{0}],", values);
            str.Append("\"font-size\": 10,\"fill-alpha\": 0.35,\"type\": \"line\",\"dot-style\":{\"type\": \"solid-dot\",\"dot-size\": 3}}],");
            str.Append("\"bg_colour\": \"#ffffff\",\"tooltip\": {\"shadow\": 1,\"mouse\": 1,\"stroke\": 1,\"rounded\": 5,\"colour\": \"#cccccc\",");
            str.Append("\"background\": \"#ffffee\",\"title\": \"{font-size: 12px; color: #0066cc;font-weight:bold;}\", \"body\": \"{font-size: 12px; color: #666666;}\"}}");
            return str.ToString();
        }
    }
}
