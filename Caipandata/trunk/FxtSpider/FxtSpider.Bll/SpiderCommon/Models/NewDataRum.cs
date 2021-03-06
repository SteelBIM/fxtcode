﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll.SpiderCommon.Models
{
    /// <summary>
    /// 楼盘案例运行启动类
    /// </summary>
    public class NewDataRum
    {
        public NewDataRum()
        {}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cityName">城市名称</param>
        /// <param name="_hostUrl">域名</param>
        /// <param name="_pageListUrl">列表页面链接</param>
        /// <param name="_rate">详细页面爬取频率</param>
        /// <param name="_pageCheckRate">页面监测频率</param>
        /// <param name="_pageCheckRate">规则编号</param>
        /// <param name="_pageCheckRate">主用途</param>
        /// <param name="_pageCheckRate">主案例类型</param>
        public NewDataRum(string _cityName, string _hostUrl, string _pageListUrl, int _rate, int _pageCheckRate, int? _RegexNumber, int? _BasePurposeCode, int? _BaseCaseTypeCode)
        {
            this.cityName = _cityName;
            this.hostUrl = _hostUrl;
            this.pageListUrl = _pageListUrl;
            this.rate = _rate;
            this.pageCheckRate = _pageCheckRate;
            this.RegexNumber = _RegexNumber;
            this.BaseCaseTypeCode = _BaseCaseTypeCode;
            this.BasePurposeCode = _BasePurposeCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cityName">城市名称</param>
        /// <param name="_hostUrl">域名</param>
        /// <param name="_pageListUrl">列表页面链接</param>
        /// <param name="_rate">详细页面爬取频率</param>
        /// <param name="_pageCheckRate">页面监测频率</param>
        public NewDataRum(string _cityName, string _hostUrl, string _pageListUrl, int _rate, int _pageCheckRate)
        {
            this.cityName = _cityName;
            this.hostUrl = _hostUrl;
            this.pageListUrl = _pageListUrl;
            this.rate = _rate;
            this.pageCheckRate = _pageCheckRate;
            this.RegexNumber = null;
            this.BaseCaseTypeCode = null;
            this.BasePurposeCode = null;
        }
        private string cityName;
        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }
        private string hostUrl;
        public string HostUrl
        {
            get { return hostUrl; }
            set { hostUrl = value; }
        }
        private string pageListUrl;
        public string PageListUrl
        {
            get { return pageListUrl; }
            set { pageListUrl = value; }
        }
        public int rate;
        /// <summary>
        /// 爬取频率(毫秒)
        /// </summary>
        public int Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public int pageCheckRate;
        /// <summary>
        /// 页面监测频率(毫秒)
        /// </summary>
        public int PageCheckRate
        {
            get { return pageCheckRate; }
            set { pageCheckRate = value; }
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
        private INewDataRum iNewDataRum;
        public INewDataRum IRum
        {
            get { return iNewDataRum; }
            set { iNewDataRum = value; }
        }
        public void start(object obj)
        {
            IRum = obj as INewDataRum;
            IRum.CityName = cityName;
            IRum.CityId = 0;
            IRum.RegexNumber = RegexNumber;
            IRum.BaseCaseTypeCode = BaseCaseTypeCode;
            IRum.BasePurposeCode = BasePurposeCode;
             using (DataClassesDataContext db = new DataClassesDataContext())
            {
                城市表 city = db.城市表.FirstOrDefault(p => p.城市名称 == cityName);
                if (city != null)
                {
                    IRum.CityId = city.ID;
                }
             }
            ThreadStart ts = new ThreadStart(this.Process);
            Thread m_thread = new Thread(ts);            
            m_thread.Start();
        }
        public void Process()
        {
            IRum.SpiderHouse(hostUrl, pageListUrl,rate,pageCheckRate);
            //NewDataSpider.SpiderHouse(hostUrl, pageListUrl, cityName);
        }
    }
}
