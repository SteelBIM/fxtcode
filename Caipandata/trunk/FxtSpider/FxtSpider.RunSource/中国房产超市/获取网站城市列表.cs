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

namespace FxtSpider.RunSource.中国房产超市
{
    public class 获取网站城市列表 : INewDataRum
    {
        public void start()
        {
            string[] citys = new string[] { "济宁","杭州","漳州","威海","宜昌","北海","包头","滨州","长春","大连","东营",
                                            "衡水","湖州","金华","九江","盐城","嘉兴","聊城","芜湖","临沂","南通","秦皇岛",
                                            "衢州","日照","上海","石家庄","绍兴","泰安","潍坊","襄阳","银川","烟台","淄博","镇江"};
            网站表 webObj = WebsiteManager.GetWebById(WebsiteManager.中国房产超市_ID);
            RegexInfo 总页数正则 = new RegexInfo(">下一页</a><div[^<>]*><span[^<>]*>[^<>]*</span>/([^<>]+)<", "$1");
            Dictionary<string, RegexInfo> 根页面正则字典集合 = new Dictionary<string, RegexInfo>();
            根页面正则字典集合.Add("总页数", 总页数正则);
            RegexInfo cityRegexListText = new RegexInfo("<span class=\"hui6c\"[^<>]*>((?:(?!</span).)*)</span>", "$1");
            RegexInfo cityRegexInfo = new RegexInfo("(<a [^<>]+>[^<>]+</a>)", "$1");            
            Dictionary<string, RegexInfo> cityRegexDic = new Dictionary<string, RegexInfo>();
            cityRegexDic.Add("城市列表文本", cityRegexListText);
            //获取所有城市的文本区域
            Dictionary<string, List<string>> dicCitylistText = SpiderHelp.GetHtmlByRegex("http://www.fccs.com/city/", "gb2312", cityRegexDic, WebsiteManager.GetWebById(WebsiteManager.中国房产超市_ID), CityId);
            string cityText = dicCitylistText["城市列表文本"].Count < 1 ? "" : dicCitylistText["城市列表文本"][0];
            //从文本区域中提取城市列表
            cityRegexDic.Add("城市列表", cityRegexInfo);
            dicCitylistText = SpiderHelp.GetStrByRegex(cityText, cityRegexDic);
            List<string> cityList = dicCitylistText["城市列表"];
            StringBuilder citySb = new StringBuilder();
            StringBuilder citySb2 = new StringBuilder();
            StringBuilder citySb3 = new StringBuilder();
            foreach (string cityInfoStr in cityList)
            {
                //获取城市名称+url
                RegexInfo regexCityName = new RegexInfo("<a [^<>]+>([^<>]+)</a>", "$1");
                RegexInfo regexCityUrl = new RegexInfo("<a[^<>]+href=\"([^<>]+)\"[^<>]*>[^<>]+</a>", "$1");
                Dictionary<string, RegexInfo> cityRegexDic2 = new Dictionary<string, RegexInfo>();
                cityRegexDic2.Add("regexCityName", regexCityName);
                cityRegexDic2.Add("regexCityUrl", regexCityUrl);
                Dictionary<string, List<string>> dicCityInfo = SpiderHelp.GetStrByRegex(cityInfoStr, cityRegexDic2);
                string cityName = StringHelp.TrimBlank(dicCityInfo["regexCityName"].Count > 0 ? dicCityInfo["regexCityName"][0] : "");
                string cityUrl = dicCityInfo["regexCityUrl"].Count > 0 ? dicCityInfo["regexCityUrl"][0] : "";
                //判断城市是否需要爬取
                if (citys.Where(obj => obj.Equals(cityName)).FirstOrDefault() == null)
                {
                    continue;
                }
                //生成sql
                string execStr = " exec 往网站爬取配置表添加配置信息 '{0}','{1}','{2}','{3}','{4}',{5},{6}";               
                城市表 city = CityManager.Get城市_byLike城市名称(cityName);
                if (city != null && !citySb2.ToString().Contains(city.城市名称) && !citySb.ToString().Contains(city.城市名称) && SpiderWebConfigManager.get网站爬取配置_by城市ID_网站ID(city.ID, WebsiteManager.中国房产超市_ID) == null)
                {
                    string host = cityUrl.Replace("http://", "http://second.");
                    string hostlist = host;
                    Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(hostlist, "gb2312", 根页面正则字典集合, webObj, CityId);

                    execStr = string.Format(execStr, WebsiteManager.中国房产超市, city.城市名称, host, hostlist,
                      "", "1", "1");
                    if (根页面正则字典集合结果["总页数"].Count() < 1)
                    {
                        citySb2.Append(execStr).Append("\r\n");
                        continue;
                    }
                    citySb.Append(execStr).Append("\r\n");                    
                }

            }
            string result = citySb.ToString();
            string result2 = citySb2.ToString();
            string result3 = citySb3.ToString();
            List<string> test = citys.Where(obj => !result.Contains(obj)).ToList();
            导出任务计划配置文件();
        }
        public void 导出任务计划配置文件()
        {
            List<城市表> list = CityManager.Get网站爬取配置城市_by网站Id(WebsiteManager.中国房产超市_ID);
            List<string> strList = new List<string>();
            int j = 0;
            for (int i=0;i<list.Count;i++)
            {
                strList.Add(list[i].城市名称);
                if (strList.Count >= 10 || i == (list.Count-1))
                {
                    导出任务计划配置文件(strList.ToArray(),j);
                    strList = new List<string>();
                    j = j + 1;
                }
            }
        }
        public void 导出任务计划配置文件(string[] 城市名称,int index)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            foreach (string str in 城市名称)
            {
                sb.Append("中国房产超市.").Append(str).Append(" ");
                sb2.Append(str).Append(".");
            }
            string cityResult = sb.ToString().TrimEnd(' ');
            string filaName = "任务计划_中国房产超市(" + sb2.ToString().TrimEnd('.') + ").xml";
            任务计划.Help.get_配置任务计划文件字符串(filaName, Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:16:" + (index +10).ToString()+ ""), 1, 22, cityResult);
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
