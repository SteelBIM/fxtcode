using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll;
using log4net;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Common;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace FxtSpider.RunSource.常州房产网
{
    public class NewDataSpider : INewDataRum
    {
        #region 初始化变量
        protected DateTime nowDate = DateTime.Now;
        protected bool isNowPageStop = false;
        protected static string 网站名称 = WebsiteManager.常州房产网;
        protected static int 网站ID = WebsiteManager.常州房产网_ID;
        protected static readonly ILog log = LogManager.GetLogger(typeof(NewDataSpider));
        protected List<NewDataRum> NewDataRumList = new List<NewDataRum>();
        protected static 网站表 WebObj = WebsiteManager.GetWebById(WebsiteManager.常州房产网_ID);
        /// <summary>
        /// 网站用途对应库用途字典
        /// </summary>
        protected Dictionary<string, string> purposeDic = new Dictionary<string, string>();
        /// <summary>
        /// 是否爬取以前的数据
        /// </summary>
        protected bool IsPrevious = false;
        #endregion
        #region(出售房源_页面需要爬取的各字段正则)
        ///// <summary>
        ///// 楼盘名
        ///// </summary>
        //protected RegexInfo regex_lpm = new RegexInfo("小区：</font><[^<>]+>([^<>]+)</[^<>]+>", "$1");
        //protected RegexInfo regex_lpm2 = new RegexInfo("小区：</font>([^<>]+)<", "$1");
        ///// <summary>
        ///// 行政区
        ///// </summary>
        //protected RegexInfo regex_xzq = new RegexInfo("区域：</font>常州([^<>\\-]+)\\-[^<>]+<", "$1");
        ///// <summary>
        ///// 片区
        ///// </summary>
        //protected RegexInfo regex_pq = new RegexInfo("", "$1");
        ///// <summary>
        ///// 面积
        ///// </summary>
        //protected RegexInfo regex_mj = new RegexInfo("面积：</[^<>]+>([^<>]+)㎡<", "$1");
        ///// <summary>
        ///// 单价
        ///// </summary>
        //protected RegexInfo regex_dj = new RegexInfo("总价：</[^<>]+><[^<>]+>[^<>]+</[^<>]+>[^<>]*万元\\((\\d+)元/㎡\\)<", "$1");
        ///// <summary>
        ///// 结构
        ///// </summary>
        //protected RegexInfo regex_jg = new RegexInfo("", "$1");
        ///// <summary>
        ///// 用途
        ///// </summary>
        //protected RegexInfo regex_yt = new RegexInfo("建筑：</[^<>]+>([^<>]+)/", "$1");
        ///// <summary>
        ///// 总价
        ///// </summary>
        //protected RegexInfo regex_zj = new RegexInfo("总价：</[^<>]+><[^<>]+>([^<>]+)</[^<>]+>", "$1");
        ///// <summary>
        ///// 所在楼层
        ///// </summary>
        //protected RegexInfo regex_szlc = new RegexInfo("楼层：</font>楼层\\:第(\\d+)层[^<>]+<", "$1");
        ///// <summary>
        ///// 总楼层
        ///// </summary>
        //protected RegexInfo regex_zlc = new RegexInfo("楼层：</font>楼层\\:第\\d+层，共(\\d*)层[^<>]+<", "$1");        
        ///// <summary>
        ///// 户型
        ///// </summary>
        //protected RegexInfo regex_hx = new RegexInfo("房型：</font>(\\d+房\\d+厅)[^<>]+<", "$1");
        ///// <summary>
        ///// 朝向(待定)
        ///// </summary>
        //protected RegexInfo regex_cx = new RegexInfo("", "$1");
        ///// <summary>
        ///// 装修
        ///// </summary>
        //protected RegexInfo regex_zx = new RegexInfo("装修：</font>([^<>]+)<", "$1");
        ///// <summary>
        ///// 建筑年代
        ///// </summary>
        //protected RegexInfo regex_jznd = new RegexInfo("", "$1");
        ///// <summary>
        ///// 信息(备注)
        ///// </summary>
        //protected RegexInfo regex_title = new RegexInfo("<[^<>]+class=\"FyName\"[^<>]*><[^<>]+>([^<>]+)</[^<>]+>", "$1");
        ///// <summary>
        ///// 电话
        ///// </summary>
        //protected RegexInfo regex_phone = new RegexInfo("", "$1");
        ///// <summary>
        ///// URL
        ///// </summary>
        //protected RegexInfo regex_infUrl = new RegexInfo("<p class=\"InfoTitle\"><a[^<>]+>[^<>]*</a><a href=\"([^\"]+)\"[^<>]*>[^<>]*<strong>", "$1");
        ///// <summary>
        ///// 列表页详细页面链接所在区域块html
        ///// </summary>
        //protected RegexInfo regex_infPanel = new RegexInfo("<li class=\"HouseInfo\">((?:(?!</li>).)*)</li>", "$1");
        ///// <summary>
        ///// 下一页正则
        ///// </summary>
        //protected RegexInfo regex_nextPage = new RegexInfo("<a href=\"([^\"]+)\"[^<>]*>下页</a>", "$1");
        ///// <summary>
        ///// 地址
        ///// </summary>
        //protected RegexInfo regex_address = new RegexInfo("区域：</font>([^<>]+)<", "$1");
        ///// <summary>
        ///// 信息的发布时间
        ///// </summary>
        //protected RegexInfo regex_datetime = new RegexInfo(">发布时间：([^<>]+)<", "$1");
        ///// <summary>
        ///// 信息更新时间
        ///// </summary>
        //protected RegexInfo regex_updatetime = new RegexInfo("更新时间：([^<>]+)前", "$1");
        ///// <summary>
        ///// 中介公司
        ///// </summary>
        //protected RegexInfo regex_comName = new RegexInfo("<[^<>]+>中介</[^<>]+><br /><[^<>]+>([^<>]+)<", "$1");
        ///// <summary>
        ///// 门店地址
        ///// </summary>
        //protected RegexInfo regex_comArea = new RegexInfo("", "$1");
        //protected RegexInfo  总条数正则 = new RegexInfo("找到<[^<>]+>([^<>]+)</[^<>]+>条房源", "$1");
        //protected RegexInfo  行政区文本正则 = new RegexInfo("区域：</dt><dd>((?:(?!dd>).)*)</dd>", "$1");
        //protected RegexInfo  行政区链接正则 = new RegexInfo("<a href=\"([^\"]+)\"[^<>]*><span[^<>]*>[^<>]+</span>", "$1");
        //protected RegexInfo  片区文本正则 = new RegexInfo("<p class=\"box\"><a[^<>]+><[^<>]+>地段不限</[^<>]+></a>((?:(?!</p>).)*)</p>", "$1");
        //protected RegexInfo  片区链接正则 = new RegexInfo("<a href=\"([^\"]+)\"[^<>]*>", "$1");
      
        #endregion

        #region(出售房源_页面需要爬取的各字段正则)
        /// <summary>
        /// 楼盘名
        /// </summary>
        protected RegexInfo regex_lpm = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_lpm", 网站名称);
        /// <summary>
        /// 行政区
        /// </summary>
        protected RegexInfo regex_xzq = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_xzq", 网站名称);
        /// <summary>
        /// 片区
        /// </summary>
        protected RegexInfo regex_pq = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_pq", 网站名称);
        /// <summary>
        /// 面积
        /// </summary>
        protected RegexInfo regex_mj = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_mj", 网站名称);
        /// <summary>
        /// 单价
        /// </summary>
        protected RegexInfo regex_dj = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_dj", 网站名称);
        /// <summary>
        /// 结构
        /// </summary>
        protected RegexInfo regex_jg = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_jg", 网站名称);
        /// <summary>
        /// 用途
        /// </summary>
        protected RegexInfo regex_yt = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_yt", 网站名称);
        /// <summary>
        /// 总价
        /// </summary>
        protected RegexInfo regex_zj = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zj", 网站名称);
        /// <summary>
        /// 所在楼层
        /// </summary>
        protected RegexInfo regex_szlc = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_szlc", 网站名称);
        /// <summary>
        /// 总楼层
        /// </summary>
        protected RegexInfo regex_zlc = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zlc", 网站名称);
        /// <summary>
        /// 户型
        /// </summary>
        protected RegexInfo regex_hx = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_hx", 网站名称);
        /// <summary>
        /// 朝向(待定)
        /// </summary>
        protected RegexInfo regex_cx = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_cx", 网站名称);
        /// <summary>
        /// 装修
        /// </summary>
        protected RegexInfo regex_zx = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zx", 网站名称);
        /// <summary>
        /// 建筑年代
        /// </summary>
        protected RegexInfo regex_jznd = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_jznd", 网站名称);
        /// <summary>
        /// 信息(备注)
        /// </summary>
        protected RegexInfo regex_title = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_title", 网站名称);
        /// <summary>
        /// 电话
        /// </summary>
        protected RegexInfo regex_phone = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_phone", 网站名称);
        /// <summary>
        /// URL
        /// </summary>
        protected RegexInfo regex_infUrl = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_infUrl", 网站名称);
        /// <summary>
        /// 列表页详细页面链接所在区域块html
        /// </summary>
        protected RegexInfo regex_infPanel = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_infPanel", 网站名称);
        /// <summary>
        /// 下一页正则
        /// </summary>
        protected RegexInfo regex_nextPage = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_nextPage", 网站名称);
        /// <summary>
        /// 地址
        /// </summary>
        protected RegexInfo regex_address = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_address", 网站名称);
        /// <summary>
        /// 信息的发布时间
        /// </summary>
        protected RegexInfo regex_datetime = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_datetime", 网站名称);
        /// <summary>
        /// 信息更新时间
        /// </summary>
        protected RegexInfo regex_updatetime = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_updatetime", 网站名称);
        /// <summary>
        /// 中介公司
        /// </summary>
        protected RegexInfo regex_comName = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_comName", 网站名称);
        /// <summary>
        /// 门店地址
        /// </summary>
        protected RegexInfo regex_comArea = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_comArea", 网站名称);
        protected RegexInfo 总条数正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("总条数正则", 网站名称);
        protected RegexInfo 行政区文本正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("行政区文本正则", 网站名称);
        protected RegexInfo 行政区链接正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("行政区链接正则", 网站名称);
        protected RegexInfo 片区文本正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("片区文本正则", 网站名称);
        protected RegexInfo 片区链接正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("片区链接正则", 网站名称);

        #endregion

        #region 构造函数
        public NewDataSpider()
        {
            //regex_lpm.RegexInfoList.Add(regex_lpm2);
            purposeDic.Add("住宅", "普通住宅");
            purposeDic.Add("别墅", "别墅");
            purposeDic.Add("商住楼", "商住");

            string configPath = AppDomain.CurrentDomain.BaseDirectory + "常州房产网/SpiderConfig.txt";
            StreamReader sr = new StreamReader(configPath);
            while (true)
            {
                string str = sr.ReadLine();
                if (str == "1")
                {
                    IsPrevious = true;
                }
                break;
            }
            sr.Close();
            sr.Dispose();
        }
        #endregion
        #region INewDataRum 成员
        public string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 城市名称
        /// </summary>
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
        /// <summary>
        /// 用于线程调用的先进先出的url(用于线程方法ProcessQueue)
        /// </summary>
        public Queue<string> Url_workload
        {
            get;
            set;
        }
        public void start()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = SpiderWebConfigManager.获取常州房产网下所有城市爬取配置();
            foreach (VIEW_网站爬取配置_城市表_网站表 _view in list)
            {
                NewDataRum exists = NewDataRumList.Find(
                 delegate(NewDataRum _newDataRum) { return _newDataRum.CityName.Equals(_view.城市名称); });
                if (exists == null)
                {
                    new 其他城市(_view.城市名称).start(_view.域名, _view.列表页链接, _view.详细页面爬取频率, _view.列表页面爬取频率);
                }
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="newHouse"></param>
        public virtual void SaveNowData(NewHouse newHouse)
        {
            if (newHouse == null)
            {
                return;
            }
            //保存数据
            log.Debug(string.Format("{0}-数据保存中:网站:{1}--城市:{2}-(url:{3}--)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 网站名称, CityName, newHouse.Url));

            try
            {
                CaseManager.往案例表插入爬取数据(网站名称: 网站名称, 城市名称: CityName, 网站ID: 网站ID, 城市ID: CityId,
                    楼盘名: newHouse.Lpm,
                    案例时间: newHouse.Alsj,
                    行政区: newHouse.Xzq,
                    片区: newHouse.Pq,
                    楼栋: newHouse.Ld,
                    房号: newHouse.Fh,
                    用途: newHouse.Yt,
                    面积: newHouse.Mj,
                    单价: newHouse.Dj,
                    案例类型: newHouse.Allx,
                    结构: newHouse.Jg,
                    建筑类型: newHouse.Jzlx,
                    总价: newHouse.Zj,
                    所在楼层: newHouse.Szlc,
                    总楼层: newHouse.Zlc,
                    户型: newHouse.Hx,
                    朝向: newHouse.Cx,
                    装修: newHouse.Zx,
                    建筑年代: newHouse.Jznd,
                    信息: newHouse.Title,
                    电话: newHouse.Phone,
                    URL: newHouse.Url,
                    币种: newHouse.Bz,
                    地址: newHouse.Addres,
                    创建时间: DateTime.Now,
                    车位数量: newHouse.Cwsl,
                    地下室面积: newHouse.Dxsmj,
                    花园面积: newHouse.Hymj,
                    建筑形式: newHouse.Jzxs,
                    配套设施: newHouse.Ptss,
                    厅结构: newHouse.Tjg,
                    中介公司: newHouse.ComName,
                    门店: newHouse.ComArea,
                    startSpiderDate: nowDate
                );
            }
            catch (Exception ex)
            {

                log.Error(string.Format("数据保存中异常:网站:{0}--城市:{1}-(url:{2}--)", 网站名称, CityName, newHouse.Url), ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="pageListIndexUrl"></param>
        /// <param name="rate"></param>
        /// <param name="pageCheckRate"></param>
        public void SpiderHouse(string hostUrl, string pageListIndexUrl, int rate, int pageCheckRate)
        {
            #region 生成xml
            //StringBuilder stest = new StringBuilder();
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_lpm, "regex_lpm", "楼盘名"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_xzq, "regex_xzq", "行政区")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_pq, "regex_pq", "片区")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_yt, "regex_yt", "用途")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_hx, "regex_hx", "户型")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_mj, "regex_mj", "面积")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_dj, "regex_dj", "单价")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_zj, "regex_zj", "总价")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_jznd, "regex_jznd", "建筑年代")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_cx, "regex_cx", "朝向")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_szlc, "regex_szlc", "所在楼层")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_zlc, "regex_zlc", "总楼层")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_jg, "regex_jg", "结构")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_zx, "regex_zx", "装修")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_title, "regex_title", "信息")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_phone, "regex_phone", "电话")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_address, "regex_address", "地址")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_datetime, "regex_datetime", "发布时间")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_updatetime, "regex_updatetime", "更新时间")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comName, "regex_comName", "公司")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comArea, "regex_comArea", "门店")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infUrl, "regex_infUrl", "url")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infPanel, "regex_infPanel", "列表页详细页面链接所在区域块html")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_nextPage, "regex_nextPage", "下一页正则"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(总条数正则, "总条数正则", "总条数正则"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区文本正则, "行政区文本正则", "行政区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区链接正则, "行政区链接正则", "行政区链接正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区文本正则, "片区文本正则", "片区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区链接正则, "片区链接正则", "片区链接正则")); ;
            //string str = stest.ToString();
            #endregion
            int maxPageCount = 500;
            int maxPageLength = 10;
            int maxCount = maxPageCount * maxPageLength;

            if (pageCheckRate > 0)
            {
                System.Threading.Thread.Sleep(pageCheckRate);
            }
            //开始获取页面
            try
            {
                

                Dictionary<string, RegexInfo> 根页面正则字典集合 = new Dictionary<string, RegexInfo>();
                根页面正则字典集合.Add("*总条数", 总条数正则);
                根页面正则字典集合.Add("*行政区文本", 行政区文本正则);
                Dictionary<string, RegexInfo> 行政区链接字典集合 = new Dictionary<string, RegexInfo>();
                行政区链接字典集合.Add("*行政区链接", 行政区链接正则);
                Dictionary<string, RegexInfo> 行政区页面正则字典集合 = new Dictionary<string, RegexInfo>();
                行政区页面正则字典集合.Add("*总条数", 总条数正则);
                行政区页面正则字典集合.Add("*片区文本", 片区文本正则);
                Dictionary<string, RegexInfo> 片区链接字典集合 = new Dictionary<string, RegexInfo>();
                片区链接字典集合.Add("*片区链接", 片区链接正则);
                Dictionary<string, RegexInfo> 片区页面正则字典集合 = new Dictionary<string, RegexInfo>();
                片区页面正则字典集合.Add("*总条数", 总条数正则);

                log.Debug(string.Format("常州房产网SpiderHouse()--获取根页面的总条数,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName));
                Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(pageListIndexUrl, "gb2312", 根页面正则字典集合, WebObj, CityId, timeout: 30000);
                int count = 根页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(根页面正则字典集合结果["*总条数"][0]);
                log.Debug(string.Format("常州房产网SpiderHouse()--获取根页面的总条数为{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", count.ToString(), hostUrl, pageListIndexUrl, CityName));
                //当前根页面总个数大于最大值
                string 行政区文本 = 根页面正则字典集合结果["*行政区文本"].Count < 1 ? "" : 根页面正则字典集合结果["*行政区文本"][0];
                Dictionary<string, List<string>> 行政区链接结果 = SpiderHelp.GetStrByRegex(行政区文本, 行政区链接字典集合);
                List<string> 行政区链接List = 行政区链接结果["*行政区链接"];
                if (count > maxCount && 行政区链接List.Count > 0)
                {
                    if (rate > 0)
                    {
                        System.Threading.Thread.Sleep(rate);
                    }
                    foreach (string _url in 行政区链接List)
                    {
                        if (_url.Contains("/sale_0_"))
                        {
                            continue;
                        }
                        isNowPageStop = false;
                        string nowUrl = _url;
                        if (!_url.ToLower().Contains("http://"))
                        {
                            nowUrl = hostUrl + _url;
                        }
                        //个数获取
                        log.Debug(string.Format("常州房产网SpiderHouse()--获取当前行政区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl, hostUrl, pageListIndexUrl, CityName));
                        Dictionary<string, List<string>> 行政区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl, "gb2312", 行政区页面正则字典集合, WebObj, CityId, timeout: 30000);
                        int _count = 行政区页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(行政区页面正则字典集合结果["*总条数"][0]);
                        log.Debug(string.Format("常州房产网SpiderHouse()--获取当前行政区页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count.ToString(), nowUrl, hostUrl, pageListIndexUrl, CityName));

                        //当前行政区页面总个数大于最大值
                        string 片区文本 = 行政区页面正则字典集合结果["*片区文本"].Count < 1 ? "" : 行政区页面正则字典集合结果["*片区文本"][0];
                        Dictionary<string, List<string>> 片区链接结果 = SpiderHelp.GetStrByRegex(片区文本, 片区链接字典集合);
                        List<string> 片区链接List = 片区链接结果["*片区链接"];
                        if (_count > maxCount && 片区链接List.Count > 0)
                        {
                            //获取片区下信息
                            foreach (string _url2 in 片区链接List)
                            {
                                #region(片区下爬取)
                                isNowPageStop = false;
                                string nowUrl2 = _url2;
                                if (!_url2.ToLower().Contains("http://"))
                                {
                                    nowUrl2 = hostUrl + _url2;
                                }
                                //获取个数
                                log.Debug(string.Format("常州房产网SpiderHouse()--获取当前片区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl2, hostUrl, pageListIndexUrl, CityName));
                                Dictionary<string, List<string>> 片区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl2, "gb2312", 片区页面正则字典集合, WebObj, CityId, timeout: 30000);
                                int _count2 = 片区页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(片区页面正则字典集合结果["*总条数"][0]);
                                log.Debug(string.Format("常州房产网SpiderHouse()--获取当前片区页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count2.ToString(), nowUrl2, hostUrl, pageListIndexUrl, CityName));
                                //当前片区页面总条数大于最大值
                                //*******************片区页面下信息列表爬取***********************//
                                SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, nowUrl2, pageListIndexUrl, rate, pageCheckRate, _count2, "片区");
                                #endregion
                            }
                        }
                        else
                        {
                            //*******************行政区页面下信息列表爬取***********************//
                            SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, nowUrl, pageListIndexUrl, rate, pageCheckRate, _count, "行政区");
                        }

                    }
                }
                else
                {
                    //*******************根页面下信息列表爬取***********************//
                    SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, pageListIndexUrl, pageListIndexUrl, rate, pageCheckRate, count, "根");
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("常州房产网SpiderHouse()异常,hostUrl:{0}, pageListIndexUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName), ex);
            }
            log.Debug(string.Format("常州房产网SpiderHouse()--获取{0}页面下详细信息Url吸取完成,详细页面url内容正则析取中-,hostUrl:{1}, pageListUrl:{2}", CityName, hostUrl, pageListIndexUrl));

        }

        #endregion

        #region 处理方法
        /// <summary>
        /// 根据详细页url获取信息
        /// </summary>
        /// <param name="url">详细页url</param>
        public void GetHouseByUrl(string url, string urlPanelHtml)
        {
            try
            {
                //设置各字段规则(正则)
                Dictionary<string, RegexInfo> dicRegexInfo = new Dictionary<string, RegexInfo>();
                dicRegexInfo.Add("*regex_lpm", regex_lpm);
                dicRegexInfo.Add("*regex_xzq", regex_xzq);
                dicRegexInfo.Add("regex_pq", regex_pq);
                dicRegexInfo.Add("regex_hx", regex_hx);
                dicRegexInfo.Add("*regex_mj", regex_mj);
                dicRegexInfo.Add("*regex_dj", regex_dj);
                dicRegexInfo.Add("*regex_zj", regex_zj);
                dicRegexInfo.Add("regex_jznd", regex_jznd);
                dicRegexInfo.Add("regex_cx", regex_cx);
                dicRegexInfo.Add("regex_szlc", regex_szlc);
                dicRegexInfo.Add("regex_zlc", regex_zlc);
                dicRegexInfo.Add("regex_jg", regex_jg);
                dicRegexInfo.Add("regex_zx", regex_zx);
                dicRegexInfo.Add("regex_title", regex_title);
                dicRegexInfo.Add("regex_phone", regex_phone);
                dicRegexInfo.Add("regex_address", regex_address);
                dicRegexInfo.Add("regex_datetime", regex_datetime);
                dicRegexInfo.Add("regex_comName", regex_comName);
                dicRegexInfo.Add("regex_comArea", regex_comArea);
                dicRegexInfo.Add("regex_yt", regex_yt);
                Dictionary<string, List<string>> dicRegexInfo_List = new Dictionary<string, List<string>>();

                //根据规则获取数据
                dicRegexInfo_List = SpiderHelp.GetHtmlByRegex(url, "gb2312", dicRegexInfo, WebObj, CityId, timeout: 30000);
                List<string> dateList = SpiderHelp.GetStrByRegexByIndex(urlPanelHtml, regex_updatetime);
                string value_title = dicRegexInfo_List["regex_title"].Count < 1 ? "" : dicRegexInfo_List["regex_title"][0];
                string value_lpm = dicRegexInfo_List["*regex_lpm"].Count < 1 ? "" : dicRegexInfo_List["*regex_lpm"][0];
                string value_xzq = dicRegexInfo_List["*regex_xzq"].Count < 1 ? "" : dicRegexInfo_List["*regex_xzq"][0];
                string value_pq = dicRegexInfo_List["regex_pq"].Count < 1 ? "" : dicRegexInfo_List["regex_pq"][0];
                string value_hx = dicRegexInfo_List["regex_hx"].Count < 1 ? "" : dicRegexInfo_List["regex_hx"][0];
                string value_mj = dicRegexInfo_List["*regex_mj"].Count < 1 ? "" : dicRegexInfo_List["*regex_mj"][0];
                string value_dj = dicRegexInfo_List["*regex_dj"].Count < 1 ? "" : dicRegexInfo_List["*regex_dj"][0];
                string value_zj = dicRegexInfo_List["*regex_zj"].Count < 1 ? "" : dicRegexInfo_List["*regex_zj"][0];
                string value_jznd = dicRegexInfo_List["regex_jznd"].Count < 1 ? "" : dicRegexInfo_List["regex_jznd"][0];
                string value_cx = dicRegexInfo_List["regex_cx"].Count < 1 ? "" : dicRegexInfo_List["regex_cx"][0];
                string value_szlc = dicRegexInfo_List["regex_szlc"].Count < 1 ? "" : dicRegexInfo_List["regex_szlc"][0];
                string value_zlc = dicRegexInfo_List["regex_zlc"].Count < 1 ? "" : dicRegexInfo_List["regex_zlc"][0];
                string value_jg = dicRegexInfo_List["regex_jg"].Count < 1 ? "" : dicRegexInfo_List["regex_jg"][0];
                string value_yt = dicRegexInfo_List["regex_yt"].Count < 1 ? "" : dicRegexInfo_List["regex_yt"][0];
                string value_zx = dicRegexInfo_List["regex_zx"].Count < 1 ? "" : dicRegexInfo_List["regex_zx"][0];
                string value_phone = dicRegexInfo_List["regex_phone"].Count < 1 ? "" : dicRegexInfo_List["regex_phone"][0];
                string value_address = dicRegexInfo_List["regex_address"].Count < 1 ? "" : dicRegexInfo_List["regex_address"][0];
                string value_datetime = dicRegexInfo_List["regex_datetime"].Count < 1 ? "" : dicRegexInfo_List["regex_datetime"][0];
                string value_comName = dicRegexInfo_List["regex_comName"].Count < 1 ? "" : dicRegexInfo_List["regex_comName"][0];
                string value_comArea = dicRegexInfo_List["regex_comArea"].Count < 1 ? "" : dicRegexInfo_List["regex_comArea"][0];
                string value_updatetime = dateList.Count < 1 ? "" : dateList[0];
                string _value_yt = CheckPurpose(value_yt);
                if (_value_yt == "0")
                {
                    log.Debug(string.Format("GetHouseByUrl()用途无效,url:{0}, cityName:{1},用途:{2}", url, CityName, Convert.ToString(value_yt)));
                    return;
                }
                value_cx = value_cx.Replace("朝", "").TrimBlank();
                value_yt = _value_yt;
                //将数据添加到字典 
                NewHouse newHouse = new NewHouse(value_lpm, GetCaseDate(value_datetime, value_updatetime), value_xzq, value_pq, "", "", "", value_mj, value_dj,
                                "", value_jg, "", value_zj, value_szlc, value_zlc, value_hx, value_cx, value_zx, value_jznd,
                                value_title, value_phone, url, "", 网站名称, value_address, "", "", "", "", "", "", value_comName, value_comArea);

                //当前数据为一天前的数据时
                newHouse.Alsj = newHouse.Alsj != null ? newHouse.Alsj.Trim() : newHouse.Alsj;
                if (!newHouse.Alsj.CheckStrIsDate())
                {
                    newHouse.Alsj = DateTime.Now.ToString();
                }
                //获取刚开始爬取时的小时单位
                int nowH = Convert.ToInt32(nowDate.ToString("HH"));
                if (nowH < 12)//如果是在12点之前开始 则析取昨天的数据
                {
                    if (Convert.ToDateTime(newHouse.Alsj) < Convert.ToDateTime(nowDate.ToString("yyyy-MM-dd")).AddDays(-1))
                    {
                        isNowPageStop = true;
                    }
                }
                else //如果是在12点之后开始 则析取当天的数据
                {
                    if (Convert.ToDateTime(newHouse.Alsj) < Convert.ToDateTime(nowDate.ToString("yyyy-MM-dd")))
                    {
                        isNowPageStop = true;
                    }
                }
                //由于类型页面多线程爬取,赞定为永不停止
                isNowPageStop = false;
                //保存数据
                SaveNowData(newHouse);
                log.Debug(string.Format("{0}-数据保存完成url:{1}--cityname:{2}--value_title:{3}--value_lpm{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), url, CityName, value_title, value_lpm));
            }
            catch (Exception ex)
            {
                log.Error(string.Format("GetHouseByUrl()异常,url:{0}, cityName:{1}", url, CityName), ex);
            }
        }
        /// <summary>
        /// (获取案例时间)根据信息的发布日期,当前时间和最后更新的间隔
        /// </summary>
        /// <param name="pubDate">根据信息的发布日期</param>
        /// <param name="upTime">最后更新与发布日期的间隔</param>
        public string GetCaseDate(string pubDate, string upTime)
        {
            int 时间区间 = 0;
            DateTime 当前时间 = DateTime.Now;
            if (string.IsNullOrEmpty(upTime))
            {
                return pubDate;
            }
            if (Regex.IsMatch(upTime, @"^(\d+)月(\d+)日$", RegexOptions.IgnoreCase))
            {
                string result = Regex.Replace(upTime, @"^(\d+)月(\d+)日$", "$1-$2", RegexOptions.IgnoreCase);
                return 当前时间.ToString("yyyy-") + result;
            }
            if (Regex.IsMatch(upTime, @"^[^\d]*(\d+)[^\d]*$", RegexOptions.IgnoreCase))
            {
                string result = Regex.Replace(upTime, @"^[^\d]*(\d+)[^\d]*$", "$1", RegexOptions.IgnoreCase);
                时间区间 = Convert.ToInt32(result);
            }
            if (upTime.Contains("年"))
            {
                当前时间 = 当前时间.AddYears(0 - 时间区间);
            }
            else if (upTime.Contains("月"))
            {
                当前时间 = 当前时间.AddMonths(0 - 时间区间);
            }
            else if (upTime.Contains("天") || upTime.Contains("日"))
            {
                当前时间 = 当前时间.AddDays(0 - 时间区间);
            }
            else if (upTime.Contains("时"))
            {
                当前时间 = 当前时间.AddHours(0 - 时间区间);
            }
            else if (upTime.Contains("分"))
            {
                当前时间 = 当前时间.AddMinutes(0 - 时间区间);
            }
            else if (upTime.Contains("秒"))
            {
                当前时间 = 当前时间.AddSeconds(0 - 时间区间);
            }
            return 当前时间.ToString();
        }

        /// <summary>
        /// 验证用途
        /// </summary>
        /// <param name="value_yt"></param>
        /// <returns></returns>
        public string CheckPurpose(string value_yt)
        {
            value_yt = value_yt.TrimBlank();
            if (string.IsNullOrEmpty(value_yt))
            {
                return value_yt;
            }
            if (value_yt.Contains("别墅"))
            {
                value_yt = "别墅";
            }
            else if (value_yt.Contains("住宅"))
            {
                value_yt = "住宅";
            }
            else if (value_yt.Contains("商住楼"))
            {
                value_yt = "商住楼";
            }
            if (purposeDic.ContainsKey(value_yt))
            {
                return purposeDic[value_yt];
            }
            else
            {
                return "0";
            }
            return value_yt;
        }
        /// <summary>
        /// 根据列表页url获取详细信息url
        /// </summary>
        /// <param name="hotUrl">列表页域名</param>
        /// <param name="pageListUrl">列表页url</param>
        /// <param name="cityName">当前列表页对应的城市名称</param>
        /// <param name="rate">爬取频率(毫秒)</param>
        /// <param name="pageCheckRate">页面监测频率(毫秒)</param>
        /// <param name="下一页链接">输出下一页的链接</param>
        public void SpiderHouseByPageListUrl(string hostUrl, string pageListUrl, int rate, int pageCheckRate, out string 下一页链接)
        {
            //pageListUrl = "http://esf.czfcw.com/rent_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_2_10912_0_0/";
            if (pageCheckRate > 0)
            {
                System.Threading.Thread.Sleep(pageCheckRate);
            }
            下一页链接 = "";
            try
            {
                string url_sz = pageListUrl;
                Dictionary<string, RegexInfo> dicRegexItem = new Dictionary<string, RegexInfo>();
                dicRegexItem.Add("*regex_infPanel", regex_infPanel);
                dicRegexItem.Add("*regex_nextPage", regex_nextPage);
                //发送请求获取根据正则获取网页html信息
                Dictionary<string, List<string>> dicRegexItem_List = SpiderHelp.GetHtmlByRegex(url_sz, "gb2312", dicRegexItem, WebObj, CityId,timeout:30000);
                List<string> list = dicRegexItem_List["*regex_infPanel"];
                下一页链接 = dicRegexItem_List["*regex_nextPage"].Count < 1 ? "" : dicRegexItem_List["*regex_nextPage"][0];
                if (!下一页链接.ToLower().Contains("http://"))
                {
                    下一页链接 = hostUrl + 下一页链接;
                }
                foreach (string urlHtml in list)
                {
                    if (rate > 0)
                    {
                        System.Threading.Thread.Sleep(rate);
                    }
                    List<string> urlList = SpiderHelp.GetStrByRegexByIndex(urlHtml, regex_infUrl);
                    if (urlList == null || urlList.Count < 1)
                    {
                        log.Error(string.Format("SpiderHouseByPageListUrl()未获取到详细页面url,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListUrl, CityName));
                        continue;
                    }
                    string strUrl = urlList[0];
                    string nowUrl = strUrl;
                    //如果当前url不带域名
                    if (!strUrl.ToLower().Contains("http://"))
                    {
                        nowUrl = hostUrl + strUrl;
                    }
                    GetHouseByUrl(nowUrl, urlHtml);
                    if (isNowPageStop)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("SpiderHouse()异常,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListUrl, CityName), ex);
            }
        }
        /// <summary>
        /// 获取类型页面下案例
        /// </summary>
        /// <param name="param"></param>
        public void SpiderPage(object param)
        {
            ArrayList al = (ArrayList)param;
            int maxPageCount = Convert.ToInt32(al[0]);
            int maxPageLength = Convert.ToInt32(al[1]);
            string hostUrl = Convert.ToString(al[2]);
            string pageUrl = Convert.ToString(al[3]);
            string pageListIndexUrl = Convert.ToString(al[4]);
            int rate = Convert.ToInt32(al[5]);
            int pageCheckRate = Convert.ToInt32(al[6]);
            int count = Convert.ToInt32(al[7]);
            string pageRemark = Convert.ToString(al[8]);

            string 页面分页链接参数 = pageUrl.Replace("_2_1_0", "_2_{0}_0");
            string 页面下一页链接 = pageUrl;
            int 当前总页数 = (count - 1) / maxPageLength + 1;
            int 当前页码 = 1;
            //获取上一次爬取到的页码
            if (IsPrevious)
            {
                Dat_KeyValueConfig keyValueConfig = KeyValueConfigManager.GetKeyValueConfig(CityId, WebsiteManager.常州房产网_ID, pageUrl);
                if (keyValueConfig != null)
                {
                    当前页码 = Convert.ToInt32(keyValueConfig.KeyValue);
                }
            }
            while (!string.IsNullOrEmpty(页面下一页链接))
            {
                string nowPageList = 页面下一页链接;
                if (!页面下一页链接.ToLower().Contains("http://"))
                {
                    nowPageList = hostUrl + 页面下一页链接;
                }
                log.Debug(string.Format("常州房产网SpiderHouse()--获取根页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 页面下一页链接);
                //存储当前页面到数据库(用于吸取历史案例)
                if (IsPrevious)
                {
                    KeyValueConfigManager.SetKeyValueConfig(CityId, WebsiteManager.常州房产网_ID, pageUrl, 当前页码.ToString());
                }
                当前页码++;
                //如果当前页码还不到最后一页
                if (当前页码 <= 当前总页数)
                {
                    页面下一页链接 = string.Format(页面分页链接参数, 当前页码.ToString());
                }
                else
                {
                    break;
                }
                if (isNowPageStop)
                {
                    break;
                }
            }
            log.Debug(string.Format("常州房产网SpiderHouse()--获取{0}页面下信息吸取完成,{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", pageRemark, 页面下一页链接, hostUrl, pageListIndexUrl, CityName));
        }
        /// <summary>
        /// 发布获取类型页面下案例线程
        /// </summary>
        /// <param name="maxPageCount"></param>
        /// <param name="maxPageLength"></param>
        /// <param name="hostUrl">域名</param>
        /// <param name="pageUrl">类型下首页链接</param>
        /// <param name="pageListIndexUrl">整个城市根链接</param>
        /// <param name="rate"></param>
        /// <param name="pageCheckRate"></param>
        /// <param name="pageRemark"></param>
        public void SpiderPage_Thread(int maxPageCount, int maxPageLength, string hostUrl, string pageUrl, string pageListIndexUrl, int rate, int pageCheckRate, int count, string pageRemark)
        {
            ArrayList al = new ArrayList();
            al.Add(maxPageCount);
            al.Add(maxPageLength);
            al.Add(hostUrl);
            al.Add(pageUrl);
            al.Add(pageListIndexUrl);
            al.Add(rate);
            al.Add(pageCheckRate);
            al.Add(count);
            al.Add(pageRemark);
            //SpiderPage(al);
            Thread m_thread = new Thread(new ParameterizedThreadStart(SpiderPage));
            m_thread.Start(al);
            log.Debug(string.Format("" + 网站名称 + "SpiderPage_Thread()--发布{0}页面Url:{1}下爬取线程,hostUrl:{2}, pageListUrl:{3}", CityName, pageUrl, hostUrl, pageListIndexUrl));
        }
        #endregion
    }
}
