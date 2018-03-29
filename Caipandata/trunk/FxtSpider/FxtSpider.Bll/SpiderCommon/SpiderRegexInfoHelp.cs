using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Models;
using System.Xml;
using FxtSpider.Common;

namespace FxtSpider.Bll.SpiderCommon
{
    public static class SpiderRegexInfoHelp
    {
        private static Dictionary<string, XmlDocument> RegexInfoXmlDic = new Dictionary<string, XmlDocument>();
        public static string GetRegexInfoXmlByObj(RegexInfo obj,string name,string remark)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<regexinfo Name=\"").Append(name).Append("\" Remark=\"").Append(remark).Append("\"> ");
            sb.Append("<!--").Append(obj.RegexStr).Append("-->");
            sb.Append("<regex>").Append(obj.RegexStr.EncodeField()).Append("</regex>");
            sb.Append("<index>").Append(obj.RegexIndex).Append("</index>");
            if (obj.RegexInfoList != null && obj.RegexInfoList.Count > 0)
            {
                sb.Append("<regexinfolist>");
                foreach (RegexInfo _obj in obj.RegexInfoList)
                {
                    sb.Append("<regexinfo>");
                    sb.Append("<!--").Append(_obj.RegexStr).Append("-->");
                    sb.Append("<regex>").Append(_obj.RegexStr.EncodeField()).Append("</regex>");
                    sb.Append("<index>").Append(_obj.RegexIndex).Append("</index>");
                    sb.Append("</regexinfo>");
                }
                sb.Append("</regexinfolist>");
            }
            sb.Append("</regexinfo>");
            return sb.ToString();
        }
        public static RegexInfo GetRegexInfoByXmlName(string name, string webName, string cityName = "NewDataSpider", int? number=null)
        {
            //根据城市+网站获取xml文件
            XmlDocument RegexInfoConfig = new XmlDocument();
            if (number != null)
            {
                cityName = cityName + "_" + number.ToString();
            }
            string key = webName + "." + cityName;
            //如果未加载过在加载一次 缓存到字典
            if (!RegexInfoXmlDic.ContainsKey(key))
            {
                string configPath = AppDomain.CurrentDomain.BaseDirectory + "RegexInfoXML/" + webName + "/" + cityName + ".xml";
                RegexInfoConfig.Load(configPath);
                RegexInfoXmlDic[key] = RegexInfoConfig;
            }
            else
            {
                RegexInfoConfig = RegexInfoXmlDic[key];
            }

            RegexInfo obj = new RegexInfo();
            XmlNode RegexInfoXml = RegexInfoConfig.SelectSingleNode("/RegexInfos/regexinfo[@Name='" + name + "']");
            if (RegexInfoXml != null)
            {
                XmlNode regex = RegexInfoXml.SelectSingleNode("regex");
                XmlNode index = RegexInfoXml.SelectSingleNode("index");
                obj.RegexStr = regex.InnerText.DecodeField();
                obj.RegexIndex = index.InnerText;
                XmlNode regexinfolist = RegexInfoXml.SelectSingleNode("regexinfolist");
                if (regexinfolist != null)
                {
                    XmlNodeList regexinfos = regexinfolist.SelectNodes("regexinfo");
                    foreach (XmlNode node in regexinfos)
                    {
                        XmlNode _regex = node.SelectSingleNode("regex");
                        XmlNode _index = node.SelectSingleNode("index");
                        RegexInfo _obj = new RegexInfo(_regex.InnerText.DecodeField(), _index.InnerText.DecodeField());
                        obj.RegexInfoList.Add(_obj);
                    }
                }
            }
            else
            {
                obj.RegexStr = "";
                obj.RegexIndex = "$1";
            }
            return obj;
        }
        
    }
}
