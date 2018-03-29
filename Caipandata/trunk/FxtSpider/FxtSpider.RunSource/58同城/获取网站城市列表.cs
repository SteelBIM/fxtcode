using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Common;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;
using System.IO;
using System.Xml;

namespace FxtSpider.RunSource._58同城
{
    public class 获取网站城市列表 : INewDataRum
    {
        public void start()
        {
            RegexInfo cityRegexInfo = new RegexInfo("<dl id=\"clist\">((?:(?!</dl>).)*)</dl>", "$1");
            RegexInfo cityRegexInfo2 = new RegexInfo("(<a href=\"[^\"]+\" onclick=\"co\\([^\"]+\">[^<>]+</a>)", "$1");
            Dictionary<string, RegexInfo> cityRegexDic = new Dictionary<string, RegexInfo>();
            cityRegexDic.Add("城市列表Text", cityRegexInfo);
            Dictionary<string, List<string>> dicCitylistText = SpiderHelp.GetHtmlByRegex("http://www.58.com/ershoufang/changecity/", "utf-8", cityRegexDic, WebsiteManager.GetWebById(WebsiteManager.五八同城_ID), CityId);
            string cityText = dicCitylistText["城市列表Text"].Count > 0 ? dicCitylistText["城市列表Text"][0] : "";
            cityRegexDic.Add("城市列表", cityRegexInfo2);
            Dictionary<string, List<string>> dicCitylist = SpiderHelp.GetStrByRegex(cityText, cityRegexDic);
            List<string> cityList = dicCitylist["城市列表"];
            StringBuilder citySb = new StringBuilder();
            foreach (string cityInfoStr in cityList)
            {
                RegexInfo regexCityName = new RegexInfo("<a href=\"[^\"]+\" onclick=\"co\\([^\"]+\">([^<>]+)</a>", "$1");
                RegexInfo regexCityUrl = new RegexInfo("<a href=\"([^\"]+)\" onclick=\"co\\([^\"]+\">[^<>]+</a>", "$1");
                Dictionary<string, RegexInfo> cityRegexDic2 = new Dictionary<string, RegexInfo>();
                cityRegexDic2.Add("regexCityName", regexCityName);
                cityRegexDic2.Add("regexCityUrl", regexCityUrl);
                Dictionary<string, List<string>> dicCityInfo = SpiderHelp.GetStrByRegex(cityInfoStr, cityRegexDic2);
                string cityName = StringHelp.TrimBlank(dicCityInfo["regexCityName"].Count > 0 ? dicCityInfo["regexCityName"][0] : "");
                string cityUrl = dicCityInfo["regexCityUrl"].Count > 0 ? dicCityInfo["regexCityUrl"][0] : "";
                string execStr = " exec 往网站爬取配置表添加配置信息 '{0}','{1}','{2}','{3}','{4}',{5},{6}";
               
                城市表 city = CityManager.Get城市_byLike城市名称(cityName);
                if (city != null && SpiderWebConfigManager.get网站爬取配置_by城市ID_网站ID(city.ID, WebsiteManager.五八同城_ID) == null)
                {
                    execStr = string.Format(execStr, WebsiteManager.五八同城, city.城市名称, cityUrl.Replace("/ershoufang", ""), cityUrl.TrimEnd('/') + "/",
                      "", "2000", "2000");
                    citySb.Append(execStr).Append("\r\n");
                }

            }
            string result = citySb.ToString();
            导出任务计划配置文件();
        }
        public void 导出任务计划配置文件()
        {
            List<城市表> list = CityManager.Get网站爬取配置城市_by网站Id(WebsiteManager.五八同城_ID);
            List<string> strList = new List<string>();
            for (int i=0;i<list.Count;i++)
            {
                strList.Add(list[i].城市名称);
                if (strList.Count >= 10 || i == (list.Count-1))
                {
                    导出任务计划配置文件(strList.ToArray());
                    strList = new List<string>();
                }
            }
        }
        public void 导出任务计划配置文件(string[] 城市名称)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            foreach (string str in 城市名称)
            {
                sb.Append("_58同城.").Append(str).Append(" ");
                sb2.Append(str).Append(".");
            }
            string cityResult = sb.ToString().TrimEnd(' ');
            string filaName = "任务计划_58同城(" + sb2.ToString().TrimEnd('.') + ").xml";
            任务计划.Help.get_配置任务计划文件字符串(filaName,Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+" 00:16:16"), 1, 23, cityResult);
        }
        public string CityName
        {
            get;
            set;
        }

        /// <summary>
        /// 规则编号
        /// </summary>
        public int? RegexNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 主用途
        /// </summary>
        public int? BasePurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 主用案例类型
        /// </summary>
        public int? BaseCaseTypeCode
        {
            get;
            set;
        }
        public int CityId
        {
            get;
            set;
        }

        public void SpiderHouse(string hostUrl, string pageListUrl, int rate, int pageCheckRate)
        {

        }

        public void SaveNowData(Bll.SpiderCommon.Models.NewHouse newHouse)
        {

        }

        public Queue<string> Url_workload
        {
            get;
            set;
        }
    }
}
