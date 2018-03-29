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

namespace FxtSpider.RunSource.搜房网
{
    public class 获取网站城市列表 : INewDataRum
    {
        public void start()
        {

            RegexInfo 总条数正则 = new RegexInfo("共找到<strong class=\"number orange\">([\\d]*)</strong>条", "$1");
            RegexInfo cityRegexInfo = new RegexInfo("<div class=\"onCont\" id=\"c01\"[^<>]*>((?:(?!</div>).)*)</div>", "$1");
            RegexInfo cityRegexInfo2 = new RegexInfo("(<a href=\"[^\"]+\"[^<>]*>[^<>]+</a>)", "$1");
            Dictionary<string, RegexInfo> cityRegexDic = new Dictionary<string, RegexInfo>();
            cityRegexDic.Add("城市列表Text", cityRegexInfo);
            Dictionary<string, List<string>> dicCitylistText = SpiderHelp.GetHtmlByRegex("http://soufun.com/SoufunFamily.htm", "utf-8", cityRegexDic, WebsiteManager.GetWebById(WebsiteManager.搜房网_ID), CityId);
            string cityText = dicCitylistText["城市列表Text"].Count > 0 ? dicCitylistText["城市列表Text"][0] : "";
            cityRegexDic.Add("城市列表", cityRegexInfo2);
            Dictionary<string, List<string>> dicCitylist = SpiderHelp.GetStrByRegex(cityText, cityRegexDic);
            List<string> cityList = dicCitylist["城市列表"];
            StringBuilder citySb = new StringBuilder();
            cityRegexDic.Add("总条数", 总条数正则);
            List<string> list2 = new List<string>();
            foreach (string cityInfoStr in cityList)
            {
                RegexInfo regexCityName = new RegexInfo("<a href=\"[^\"]+\"[^<>]*>([^<>]+)</a>", "$1");
                RegexInfo regexCityUrl = new RegexInfo("<a href=\"([^\"]+)\"[^<>]*>[^<>]+</a>", "$1");
                Dictionary<string, RegexInfo> cityRegexDic2 = new Dictionary<string, RegexInfo>();
                cityRegexDic2.Add("regexCityName", regexCityName);
                cityRegexDic2.Add("regexCityUrl", regexCityUrl);
                Dictionary<string, List<string>> dicCityInfo = SpiderHelp.GetStrByRegex(cityInfoStr, cityRegexDic2);
                string cityName = StringHelp.TrimBlank(dicCityInfo["regexCityName"].Count > 0 ? dicCityInfo["regexCityName"][0] : "");
                string cityUrl = dicCityInfo["regexCityUrl"].Count > 0 ? dicCityInfo["regexCityUrl"][0] : "";
                string execStr = " exec 往网站爬取配置表添加配置信息 '{0}','{1}','{2}','{3}','{4}',{5},{6}";

                城市表 city = CityManager.Get城市_byLike城市名称(cityName);
                if (city != null && SpiderWebConfigManager.get网站爬取配置_by城市ID_网站ID(city.ID, WebsiteManager.搜房网_ID) == null)//&& 
                {
                    string houseUrl1 = cityUrl.Replace("http://", "http://esf.").TrimEnd('/');
                    if (city.城市名称.Contains("北京"))
                    {
                        houseUrl1 = houseUrl1.Replace("bj.", "");
                    }
                    string houseUrl2 = houseUrl1 + "/house/h316-j3100-w32/";
                    Dictionary<string, List<string>> dicCountlistText = SpiderHelp.GetHtmlByRegex(houseUrl2, "gbk", cityRegexDic, WebsiteManager.GetWebById(WebsiteManager.搜房网_ID), CityId);
                    string count = dicCountlistText["总条数"].Count > 0 ? dicCountlistText["总条数"][0] : "";
                    if (!string.IsNullOrEmpty(count))
                    {
                        execStr = string.Format(execStr, WebsiteManager.搜房网, city.城市名称, houseUrl1, houseUrl2,
                            "", "4000", "2000");
                        citySb.Append(execStr).Append("\r\n");
                        list2.Add(execStr);
                    }
                }

            }
            string result = citySb.ToString();
        }

        public string CityName
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
        public Queue<string> Url_workload
        {
            get;
            set;
        }
    }
}
