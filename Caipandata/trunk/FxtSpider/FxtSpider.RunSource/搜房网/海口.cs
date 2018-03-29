using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Common;
using System.Threading;
using FxtSpider.Bll;
using FxtSpider.Bll.SpiderCommon;

namespace FxtSpider.RunSource.搜房网
{
    public class 海口 : NewDataSpider, INewDataRum
    {
        public void start()
        {

            List<网站爬取配置> objlist = SpiderWebConfigManager.根据城市获取搜房网爬取配置("海口");
            foreach(网站爬取配置 obj in objlist)
            {
                NewDataRum newDataRum = new NewDataRum("海口", obj.域名, obj.列表页链接, obj.详细页面爬取频率, obj.列表页面爬取频率,obj.规则编号,obj.主要用途,obj.主要案例类型);
                newDataRum.start(this);
            }
        }

        /// <summary>
        /// 根据列表页url获取详细信息url
        /// </summary>
        /// <param name="hotUrl">列表页域名</param>
        /// <param name="pageListIndexUrl">列表页首页url</param>
        /// <param name="rate">爬取频率(毫秒)</param>
        /// <param name="pageCheckRate">页面监测频率(毫秒)</param>
        /// <param name="下一页链接">输出下一页的链接</param>
        public override void SpiderHouse(string hostUrl, string pageListIndexUrl, int rate, int pageCheckRate)
        {


            //根据开始爬取的日期和当前日期转换url
            pageListIndexUrl = GetSpiderUrlByDate(pageListIndexUrl, nowDate);
            int maxPageCount = 100;
            int maxPageLength = 100;
            int maxCount = maxPageCount * maxPageLength;
            if (pageCheckRate > 0)
            {
                System.Threading.Thread.Sleep(pageCheckRate);
            }
            //发布单独爬取详细url的线程方法
            Url_workload = new Queue<string>();
            IsStop = false;
            Rate = rate;
            ThreadStart ts2 = new ThreadStart(this.ProcessQueue);
            Thread m_thread2 = new Thread(ts2);
            m_thread2.Start();
            //开始获取页面
            try
            {
                Dictionary<string, RegexInfo> 根页面正则字典集合 = new Dictionary<string, RegexInfo>();
                根页面正则字典集合.Add("*总条数", 总条数正则);
                根页面正则字典集合.Add("*片区文本", 片区文本正则);
                Dictionary<string, RegexInfo> 片区链接正则字典 = new Dictionary<string, RegexInfo>();
                片区链接正则字典.Add("*片区链接", 片区链接正则);
                Dictionary<string, RegexInfo> 片区页面正则字典集合 = new Dictionary<string, RegexInfo>();
                片区页面正则字典集合.Add("*总条数", 总条数正则);

                log.Debug(string.Format("SpiderHouse()--获取根页面的总条数,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName));
                Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(pageListIndexUrl, "GBK", 根页面正则字典集合, WebObj, CityId);
                int count = 根页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(根页面正则字典集合结果["*总条数"][0]);
                log.Debug(string.Format("SpiderHouse()--获取根页面的总条数为{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", count.ToString(), hostUrl, pageListIndexUrl, CityName));
                //行政区页面总个数大于10000
                if (count > maxCount)
                {
                    string 片区文本 = 根页面正则字典集合结果["*片区文本"].Count < 1 ? "" : 根页面正则字典集合结果["*片区文本"][0];
                    Dictionary<string, List<string>> 片区链接结果 = SpiderHelp.GetStrByRegex(片区文本, 片区链接正则字典);
                    List<string> 片区链接List = 片区链接结果["*片区链接"];
                    foreach (string _url2 in 片区链接List)
                    {
                        string nowUrl2 = _url2;
                        if (!_url2.ToLower().Contains("http://"))
                        {
                            nowUrl2 = hostUrl + _url2;
                        }

                        //根据开始爬取的日期和当前日期转换url
                        nowUrl2 = GetSpiderUrlByDate(nowUrl2, nowDate);
                        log.Debug(string.Format("SpiderHouse()--获取当前片区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl2, hostUrl, pageListIndexUrl, CityName));
                        Dictionary<string, List<string>> 片区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl2, "GBK", 片区页面正则字典集合, WebObj, CityId);
                        int _count2 = 片区页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(片区页面正则字典集合结果["*总条数"][0]);
                        log.Debug(string.Format("SpiderHouse()--获取当前片区页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count2.ToString(), nowUrl2, hostUrl, pageListIndexUrl, CityName));
                        //*******************片区页面下信息列表爬取***********************//
                        string 片区页面分页链接参数 = nowUrl2.Replace("j3100", "i3{0}-j3100");
                        string 片区页面下一页链接 = nowUrl2;
                        int 当前总页数 = (_count2 - 1) / maxPageLength + 1;
                        int 当前页码 = 1;
                        while (!string.IsNullOrEmpty(片区页面下一页链接))
                        {
                            string nowPageList = 片区页面下一页链接;
                            if (!片区页面下一页链接.ToLower().Contains("http://"))
                            {
                                nowPageList = hostUrl + 片区页面下一页链接;
                            }
                            //根据开始爬取的日期和当前日期转换url
                            nowPageList = GetSpiderUrlByDate(nowPageList, nowDate);
                            log.Debug(string.Format("SpiderHouse()--获取片区页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                            SpiderHouseByPageListUrl(hostUrl, nowPageList,rate, pageCheckRate, out 片区页面下一页链接);
                            当前页码++;
                            //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                            if (string.IsNullOrEmpty(片区页面下一页链接) && 当前页码 <= 当前总页数)
                            {
                                片区页面下一页链接 = string.Format(片区页面分页链接参数, 当前页码.ToString());
                            }
                        }
                        log.Debug(string.Format("SpiderHouse()--获取片区页面下信息吸取完成,{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl2, hostUrl, pageListIndexUrl, CityName));
                    }
                }
                else
                {
                    //*******************行政区页面下信息列表爬取***********************//
                    string 行政区页面分页链接参数 = pageListIndexUrl.Replace("j3100", "i3{0}-j3100");
                    string 行政区页面下一页链接 = pageListIndexUrl;
                    int 当前总页数 = (count - 1) / 100 + 1;
                    int 当前页码 = 1;
                    while (!string.IsNullOrEmpty(行政区页面下一页链接))
                    {
                        string nowPageList = 行政区页面下一页链接;
                        if (!行政区页面下一页链接.ToLower().Contains("http://"))
                        {
                            nowPageList = hostUrl + 行政区页面下一页链接;
                        }
                        //根据开始爬取的日期和当前日期转换url
                        nowPageList = GetSpiderUrlByDate(nowPageList, nowDate);
                        log.Debug(string.Format("SpiderHouse()--获取当前行政区页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                        SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 行政区页面下一页链接);
                        当前页码++;
                        //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                        if (string.IsNullOrEmpty(行政区页面下一页链接) && 当前页码 <= 当前总页数)
                        {
                            行政区页面下一页链接 = string.Format(行政区页面分页链接参数, 当前页码.ToString());
                        }
                    }
                    log.Debug(string.Format("SpiderHouse()--获取行政区页面下信息吸取完成,{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", pageListIndexUrl, hostUrl, pageListIndexUrl, CityName));
                }

            }
            catch (Exception ex)
            {
                log.Error(string.Format("SpiderHouse()异常,hostUrl:{0}, pageListIndexUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName), ex);
            }
            log.Debug(string.Format("SpiderHouse()--获取{0}页面下信息吸取完成,详细信息Url吸取完成,详细页面url内容正则析取中-,hostUrl:{1}, pageListUrl:{2}", CityName, hostUrl, pageListIndexUrl));
        }
    }
}
