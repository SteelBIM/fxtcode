using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.Bll.SpiderCommon.Models;
using log4net;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll.SpiderCommon;
using System.Text.RegularExpressions;
using FxtSpider.Common;
using System.Collections;
using System.Threading;

namespace FxtSpider.RunSource._58同城
{
    public class NewDataSpider : INewDataRum
    {
        #region(初始化变量)
        protected static readonly ILog log = LogManager.GetLogger(typeof(NewDataSpider));
        protected static string 网站名称 = WebsiteManager.五八同城;
        protected static int 网站ID = WebsiteManager.五八同城_ID;
        protected List<NewDataRum> NewDataRumList = new List<NewDataRum>();
        protected DateTime nowDate = DateTime.Now;
        protected bool isNowPageStop = false;
        protected static 网站表 WebObj = WebsiteManager.GetWebById(WebsiteManager.五八同城_ID);
        /// <summary>
        /// 存储频率(用于线程方法ProcessQueue)
        /// </summary>
        public int Rate
        {
            get;
            set;
        }
        /// <summary>
        /// 是否停止(用于线程方法ProcessQueue)
        /// </summary>
        public bool IsStop
        {
            get;
            set;
        }
        #endregion
        #region(出售房源_页面需要爬取的各字段正则)
        ///// <summary>
        ///// 楼盘名
        ///// </summary>
        //protected RegexInfo regex_lpm = new RegexInfo("xiaoqu\\:{name:'([^']*)',", "$1");
        ///// <summary>
        ///// 行政区
        ///// </summary>
        //protected RegexInfo regex_xzq = new RegexInfo("locallist:\\[\\{dispid:'[^']*', name:'[^']*', listname:'[^']*'\\},\\{dispid:'[^']*', name:'([^']*)', listname:'[^']*'\\}", "$1");
        ///// <summary>
        ///// 片区
        ///// </summary>
        //protected RegexInfo regex_pq = new RegexInfo("locallist:\\[\\{dispid:'[^']*', name:'[^']*', listname:'[^']*'\\},\\{dispid:'[^']*', name:'[^']*', listname:'[^']*'\\},\\{dispid:'[^']*', name:'([^']*)', listname:'[^']*'\\}", "$1");
        ///// <summary>
        ///// 用途
        ///// </summary>
        //protected RegexInfo regex_yt = new RegexInfo("类型：</div><div class=\"[^\"]*\">([^<>\\&]*)\\&nbsp;", "$1");
        ///// <summary>
        ///// 面积
        ///// </summary>
        //protected RegexInfo regex_mj = new RegexInfo("户型：</div><div class=\"[^\"]*\">[^<>\\&]*\\&nbsp;\\&nbsp;([\\.\\d]*)㎡", "$1");
        //protected RegexInfo regex_mj2 = new RegexInfo("户型：</div><div class=\"[^\"]*\">[^<>\\(\\)]+[^\\d]+([\\.\\d]+)㎡", "$1");
        ///// <summary>
        ///// 单价
        ///// </summary>
        //protected RegexInfo regex_dj = new RegexInfo("售价：</div>[^\\&]*\\&nbsp;（([\\.\\d]*)元/㎡）", "$1");
        ///// <summary>
        ///// 户型结构
        ///// </summary>
        //protected RegexInfo regex_jg = new RegexInfo("", "$2");
        ///// <summary>
        ///// 总价
        ///// </summary>
        //protected RegexInfo regex_zj = new RegexInfo("售价：</div><div class=\"[^\"]*\"><span class=\"[^\"]*\">([\\.\\d]*)</span> 万", "$1");
        ///// <summary>
        ///// 所在楼层
        ///// </summary>
        //protected RegexInfo regex_szlc = new RegexInfo("类型：</div><div class=\"[^\"]*\">[^<>]*(\\d+)/\\d+层", "$1");
        //protected RegexInfo regex_szlc2 = new RegexInfo("装修：</div><div class=\"[^\"]*\">[^<>]*(\\d+)/\\d+层", "$1");
        ///// <summary>
        ///// 总楼层
        ///// </summary>
        //protected RegexInfo regex_zlc = new RegexInfo("类型：</div><div class=\"[^\"]*\">[^<>]*\\d+/(\\d+)层", "$1");
        //protected RegexInfo regex_zlc2 = new RegexInfo("装修：</div><div class=\"[^\"]*\">[^<>]*\\d+/(\\d+)层", "$1");
        ///// <summary>
        ///// 户型
        ///// </summary>
        //protected RegexInfo regex_hx = new RegexInfo("户型：</div><div class=\"[^\"]*\">([^<>\\&]*)\\&nbsp;", "$1");
        //protected RegexInfo regex_hx2 = new RegexInfo("户型：</div><div class=\"[^\"]*\">([^<>\\.\\(\\)㎡]+[^\\.\\d<>\\(\\)㎡]+)[^<>]*</li>", "$1");
        ///// <summary>
        ///// 朝向
        ///// </summary>
        //protected RegexInfo regex_cx = new RegexInfo("装修：</div><div class=\"[^\"]*\">[^<>\\&]*\\&nbsp;\\&nbsp;([^<>]*)</div>", "$1");
        //protected RegexInfo regex_cx2 = new RegexInfo("装修：</div><div class=\"[^\"]*\">[^<>\\&]+\\&nbsp;([^<>\\&]+)[^<>]*</div>", "$1");
        ///// <summary>
        ///// 装修
        ///// </summary>
        //protected RegexInfo regex_zx = new RegexInfo("装修：</div><div class=\"[^\"]*\">([^<>\\&]*)&nbsp;&nbsp;[^<>]*</div>", "$1");
        //protected RegexInfo regex_zx2 = new RegexInfo("装修：</div><div class=\"[^\"]*\">([^<>\\&]+)&nbsp;[^<>]*</div>", "$1");
        ///// <summary>
        ///// 建筑年代
        ///// </summary>
        //protected RegexInfo regex_jznd = new RegexInfo("<div class=\"[^\"]*\">[^<>]*(\\d{4})年建</div>", "$1");//*
        ///// <summary>
        ///// 信息(备注)
        ///// </summary>
        //protected RegexInfo regex_title = new RegexInfo("<span class=\"bigtit_span\">((?:(?!</span>).)*)</span>", "$1");
        ///// <summary>
        ///// 电话
        ///// </summary>
        //protected RegexInfo regex_phone = new RegexInfo("<span [^<>]*id="t_phone"[^<>]*>([^<>]+)</span>", "$1");
        ///// <summary>
        ///// URL
        ///// </summary>
        //protected RegexInfo regex_infUrl = new RegexInfo("<td[^<>]*><h1><a[^<>]*href=\"([^\"]+)\"[^<>]*>(?:(?!</a>).)*</a>", "$1");//(<img src=\"[^\"]*\" align=\"absmiddle\"/>|[^<>]*)
        ////<a id='[^']*' class='list-title' [^<>]*href='([^']*)' target='_blank'[^<>]*>[^<>]*(?:(?!</a>).)*</a>"
        ///// <summary>
        ///// 下一页正则
        ///// </summary>
        //protected RegexInfo regex_nextPage = new RegexInfo("<a class=\"next\" href=\"([^\"]*)\"><span>下一页</span></a>", "$1");
        ///// <summary>
        ///// 建筑形式
        ///// </summary>
        //protected RegexInfo regex_jzxs = new RegexInfo("", "$2");
        ///// <summary>
        ///// 配套设施
        ///// </summary>
        //protected RegexInfo regex_ptss = new RegexInfo("", "$2$3");
        ///// <summary>
        ///// 地址
        ///// </summary>
        //protected RegexInfo regex_address = new RegexInfo("地址：</div><div class=\"su_con\">([^<>]*)(?:(?!</div>).)*</div>", "$1");
        //protected RegexInfo regex_address2 = new RegexInfo("<p>小区地址：([^<>]*)</p>", "$1");
        ///// <summary>
        ///// 信息的发布时间
        ///// </summary>
        //protected RegexInfo regex_datetime = new RegexInfo("<span class='c_ccc'>/</span>([^<>]*)</td>", "$1");//*
        //protected RegexInfo regex_datetime2 = new RegexInfo("</span>[^<>]*<span class='c_ccc'>/</span>([^<>]*)<", "$1");
        ///// <summary>
        ///// 花园面积
        ///// </summary>
        //protected RegexInfo regex_hymj = new RegexInfo("", "$1");
        ///// <summary>
        ///// 厅结构
        ///// </summary>
        //protected RegexInfo regex_tjg = new RegexInfo("", "$1");
        ///// <summary>
        ///// 车位数量
        ///// </summary>
        //protected RegexInfo regex_cwsl = new RegexInfo("", "$1");
        ///// <summary>
        ///// 地下室面积
        ///// </summary>
        //protected RegexInfo regex_dxsmj = new RegexInfo("", "$1");
        ///// <summary>
        ///// 列表信息
        ///// </summary>
        //protected RegexInfo regex_listinfo = new RegexInfo("(<td class=\"t\"><h1(?:(?!</td>).)*</td>)", "$1");//new RegexInfo("<td[^<>]*><h1><a[^<>]*href=\"([^\"]+)\"[^<>]*>[^<>]+</a>", "$1"); 
        ///// <summary>
        ///// 中介公司
        ///// </summary>
        //protected RegexInfo regex_comName = new RegexInfo("corpname:'([^']+)'", "$1");
        ///// <summary>
        ///// 门店地址
        ///// </summary>
        //protected RegexInfo regex_comArea = new RegexInfo("", "$1");
        //protected RegexInfo 行政区文本正则 = new RegexInfo("<dl class=\"secitem\"><dt>区域：</dt><dd>((?:(?!</dd>).)*)</dd></dl>", "$1");
        //protected RegexInfo 行政区链接正则 = new RegexInfo("<a [^<>]*href=\"([^\"]+)\">[^<>]+</a>", "$1");
        //protected RegexInfo 片区文本正则 = new RegexInfo("<div class=\"subarea\">((?:(?!</div>).)*)</div>", "$1");
        //protected RegexInfo 片区链接正则 = new RegexInfo("<a [^<>]*href=\"([^\"]+)\">[^<>]+</a>", "$1");

        //#region 网站独有
        ///// <summary>
        ///// 用户ID
        ///// </summary>
        //protected RegexInfo regex_userId = new RegexInfo("var userid=(\\d+);", "$1");
        

        //#endregion

        #endregion

        #region(出售房源_页面需要爬取的各字段正则)
        /// <summary>
        /// 楼盘名
        /// </summary>
        protected RegexInfo regex_lpm =SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_lpm",网站名称);// new RegexInfo("xiaoqu\\:{name:'([^']*)',", "$1");
        /// <summary>
        /// 行政区
        /// </summary>
        protected RegexInfo regex_xzq = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_xzq", 网站名称);
        /// <summary>
        /// 片区
        /// </summary>
        protected RegexInfo regex_pq = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_pq", 网站名称);
        /// <summary>
        /// 用途
        /// </summary>
        protected RegexInfo regex_yt = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_yt", 网站名称);
        /// <summary>
        /// 面积
        /// </summary>
        protected RegexInfo regex_mj = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_mj", 网站名称);
        /// <summary>
        /// 单价
        /// </summary>
        protected RegexInfo regex_dj = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_dj", 网站名称);
        /// <summary>
        /// 户型结构
        /// </summary>
        protected RegexInfo regex_jg = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_jg", 网站名称);
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
        /// 朝向
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
        /// 下一页正则
        /// </summary>
        protected RegexInfo regex_nextPage = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_nextPage", 网站名称);
        /// <summary>
        /// 建筑形式
        /// </summary>
        protected RegexInfo regex_jzxs = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_jzxs", 网站名称);
        /// <summary>
        /// 配套设施
        /// </summary>
        protected RegexInfo regex_ptss = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_ptss", 网站名称);
        /// <summary>
        /// 地址
        /// </summary>
        protected RegexInfo regex_address = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_address", 网站名称);
        /// <summary>
        /// 信息的发布时间
        /// </summary>
        protected RegexInfo regex_datetime = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_datetime", 网站名称);
        /// <summary>
        /// 花园面积
        /// </summary>
        protected RegexInfo regex_hymj = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_hymj", 网站名称);
        /// <summary>
        /// 厅结构
        /// </summary>
        protected RegexInfo regex_tjg = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_tjg", 网站名称);
        /// <summary>
        /// 车位数量
        /// </summary>
        protected RegexInfo regex_cwsl = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_cwsl", 网站名称);
        /// <summary>
        /// 地下室面积
        /// </summary>
        protected RegexInfo regex_dxsmj = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_dxsmj", 网站名称);
        /// <summary>
        /// 列表信息
        /// </summary>
        protected RegexInfo regex_listinfo = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_listinfo", 网站名称);
        /// <summary>
        /// 中介公司
        /// </summary>
        protected RegexInfo regex_comName = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_comName", 网站名称);
        /// <summary>
        /// 门店地址
        /// </summary>
        protected RegexInfo regex_comArea = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_comArea", 网站名称);


        #region 网站独有
        /// <summary>
        /// 用户ID
        /// </summary>
        protected RegexInfo regex_userId = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_userId", 网站名称);

        protected RegexInfo 行政区文本正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("行政区文本正则", 网站名称);
        protected RegexInfo 行政区链接正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("行政区链接正则", 网站名称);
        protected RegexInfo 片区文本正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("片区文本正则", 网站名称);
        protected RegexInfo 片区链接正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("片区链接正则", 网站名称);
        #endregion

        #endregion

        #region 构造函数
        public NewDataSpider()
        {
            //regex_mj.RegexInfoList.Add(regex_mj2);
            //regex_hx.RegexInfoList.Add(regex_hx2);
            //regex_zlc.RegexInfoList.Add(regex_zlc2);
            //regex_szlc.RegexInfoList.Add(regex_szlc2);
            //regex_cx.RegexInfoList.Add(regex_cx2);
            //regex_zx.RegexInfoList.Add(regex_zx2);
            //regex_datetime.RegexInfoList.Add(regex_datetime2);
            //regex_address.RegexInfoList.Add(regex_address2);
        }
        #endregion
        #region INewDataRum 成员
        public void start()
        {
            //GetHouseByUrl("http://lw.58.com/ershoufang/16131733324033x.shtml", "");
            //new 其他城市("深圳").start("http://sz.58.com", "http://sz.58.com/ershoufang/", 2000, 1000);
            //return;
            List<VIEW_网站爬取配置_城市表_网站表> list = SpiderWebConfigManager.获取五八同城下所有城市爬取配置();
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
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comName, "regex_comName", "公司")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comArea, "regex_comArea", "门店")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infUrl, "regex_infUrl", "url")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_nextPage, "regex_nextPage", "下一页正则"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_jzxs, "regex_jzxs", "建筑形式"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_listinfo, "regex_listinfo", "列表信息"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_ptss, "regex_ptss", "配套设施"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_hymj, "regex_hymj", "花园面积"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_tjg, "regex_tjg", "厅结构"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_cwsl, "regex_cwsl", "车位数量"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_dxsmj, "regex_dxsmj", "地下室面积"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区文本正则, "行政区文本正则", "行政区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区链接正则, "行政区链接正则", "行政区链接正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区文本正则, "片区文本正则", "片区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区链接正则, "片区链接正则", "片区链接正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_userId, "regex_userId", "用户ID(网站独有)")); ;
            //string str = stest.ToString();
            #endregion
            int maxPageCount = 70;
            int maxPageLength = 92;
            int maxCount = maxPageCount * maxPageLength;
            if (pageCheckRate > 0)
            {
                System.Threading.Thread.Sleep(pageCheckRate);
            }
            //开始获取页面
            try
            {
                
                Dictionary<string, RegexInfo> 根页面正则字典集合 = new Dictionary<string, RegexInfo>();
                根页面正则字典集合.Add("*行政区文本", 行政区文本正则);
                Dictionary<string, RegexInfo> 行政区链接字典集合 = new Dictionary<string, RegexInfo>();
                行政区链接字典集合.Add("*行政区链接", 行政区链接正则);
                Dictionary<string, RegexInfo> 行政区页面正则字典集合 = new Dictionary<string, RegexInfo>();
                行政区页面正则字典集合.Add("*片区文本", 片区文本正则);
                Dictionary<string, RegexInfo> 片区链接字典集合 = new Dictionary<string, RegexInfo>();
                片区链接字典集合.Add("*片区链接", 片区链接正则);



                //Dictionary<string, RegexInfo> 正则字典集合 = new Dictionary<string, RegexInfo>();
                //正则字典集合.Add("行政区文本", 行政区文本正则);
                //正则字典集合.Add("行政区链接", 行政区链接正则);
                //正则字典集合.Add("片区文本", 片区文本正则);
                //正则字典集合.Add("片区链接", 片区链接正则);
                log.Debug(string.Format(网站名称 + "SpiderHouse()--获取根页面的总条数,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName));
                Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(pageListIndexUrl, "utf-8", 根页面正则字典集合, WebObj, CityId);
                //当前根页面片区个数大于0
                string 行政区文本 = 根页面正则字典集合结果["*行政区文本"].Count < 1 ? "" : 根页面正则字典集合结果["*行政区文本"][0];
                Dictionary<string, List<string>> 行政区链接结果 = SpiderHelp.GetStrByRegex(行政区文本, 行政区链接字典集合);
                List<string> 行政区链接List = 行政区链接结果["*行政区链接"];
                log.Debug(string.Format(网站名称 + "SpiderHouse()--获取根页面的行政区数为{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", 行政区链接List.Count.ToString(), hostUrl, pageListIndexUrl, CityName));
                if (行政区链接List.Count > 0)
                {
                    if (rate > 0)
                    {
                        System.Threading.Thread.Sleep(rate);
                    }
                    foreach (string _url in 行政区链接List)
                    {
                        isNowPageStop = false;
                        string nowUrl = _url;
                        if (!_url.ToLower().Contains("http://"))
                        {
                            nowUrl = hostUrl + _url;
                        }
                        //当前行政区页面片区个数
                        log.Debug(string.Format(网站名称 + "SpiderHouse()--获取当前行政区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl, hostUrl, pageListIndexUrl, CityName));
                        Dictionary<string, List<string>> 行政区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl, "utf-8", 行政区页面正则字典集合, WebObj, CityId);
                        //当前行政区页面片区个数>0
                        string 片区文本 = 行政区页面正则字典集合结果["*片区文本"].Count < 1 ? "" : 行政区页面正则字典集合结果["*片区文本"][0];
                        Dictionary<string, List<string>> 片区链接结果 = SpiderHelp.GetStrByRegex(片区文本, 片区链接字典集合);
                        List<string> 片区链接List = 片区链接结果["*片区链接"];
                        log.Debug(string.Format(网站名称 + "SpiderHouse()--获取当前行政区页面的片区个数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", 片区链接List.Count.ToString(), nowUrl, hostUrl, pageListIndexUrl, CityName));
                        //当前行政区页面片区个数大于0
                        if (片区链接List.Count > 0)
                        {
                            foreach (string _url2 in 片区链接List)
                            {
                                isNowPageStop = false;
                                string nowUrl2 = _url2;
                                if (!_url2.ToLower().Contains("http://"))
                                {
                                    nowUrl2 = hostUrl + _url2;
                                }
                                //*******************片区页面下信息列表爬取***********************//
                                SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, nowUrl2, pageListIndexUrl, rate, pageCheckRate, "片区");
                               
                            }
                        }
                        else
                        {
                            //*******************行政区页面下信息列表爬取***********************//
                            SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, nowUrl, pageListIndexUrl, rate, pageCheckRate, "行政区");
                           
                        }
                    }
                }
                else
                {

                    //*******************根页面下信息列表爬取***********************//
                    SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, pageListIndexUrl, pageListIndexUrl, rate, pageCheckRate, "根");                    
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("" + 网站名称 + "SpiderHouse()异常,hostUrl:{0}, pageListIndexUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName), ex);
            }
            log.Debug(string.Format("" + 网站名称 + "SpiderHouse()--获取{0}页面下详细信息Url吸取完成,详细页面url内容正则析取中-,hostUrl:{1}, pageListUrl:{2}", CityName, hostUrl, pageListIndexUrl));
            IsStop = true;
        }
        public void SaveNowData(NewHouse newHouse)
        {
            if (newHouse == null)
            {
                return;
            }
            //保存数据
            log.Debug(string.Format("{0}数据保存中:网站:{1}--城市:{2}-(url:{3}--)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 网站名称, CityName, newHouse.Url));

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
                    中介公司:newHouse.ComName,
                    门店: newHouse.ComArea,
                    startSpiderDate: nowDate
                );
            }
            catch (Exception ex)
            {
                log.Error(string.Format("数据保存中异常:网站:{0}--城市:{1}-(url:{2}--)", 网站名称, CityName, newHouse.Url), ex);
            }
        }
        public Queue<string> Url_workload
        {
            get;
            set;
        }
        #endregion
        #region 处理方法
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
            if (pageCheckRate > 0)
            {
                System.Threading.Thread.Sleep(pageCheckRate);
            }
            下一页链接 = "";

            try
            {
                //pageListUrl = "http://sz.58.com/huangbeiling/ershoufang/pn50/?PGTID=14062595822230.7245780795346946&ClickID=2";
                string url_sz = pageListUrl;
                Dictionary<string, RegexInfo> dicRegexItem = new Dictionary<string, RegexInfo>();
                dicRegexItem.Add("*regex_nextPage", regex_nextPage);
                dicRegexItem.Add("*regex_listinfo", regex_listinfo);
                Dictionary<string, RegexInfo> dicRegexItem2 = new Dictionary<string, RegexInfo>();
                dicRegexItem2.Add("regex_infUrl", regex_infUrl);
                //发送请求获取根据正则获取网页html信息
                Dictionary<string, List<string>> dicRegexItem_List = SpiderHelp.GetHtmlByRegex(url_sz, "utf-8", dicRegexItem, WebObj, CityId);
                List<string> list = dicRegexItem_List["*regex_listinfo"];
                下一页链接 = dicRegexItem_List["*regex_nextPage"].Count < 1 ? "" : dicRegexItem_List["*regex_nextPage"][0];
                foreach (string infoText in list)
                {

                    if (rate > 0)
                    {
                        System.Threading.Thread.Sleep(rate);
                    }
                    Dictionary<string, List<string>> infoText_list = SpiderHelp.GetStrByRegex(infoText, dicRegexItem2);
                    string nowUrl = infoText_list["regex_infUrl"].Count > 0 ? infoText_list["regex_infUrl"][0] : "";
                    //如果当前url不带域名
                    if (!nowUrl.ToLower().Contains("http://"))
                    {
                        nowUrl = hostUrl + nowUrl;
                    }
                    GetHouseByUrl(nowUrl, infoText);
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
        /// 根据详细页url获取信息
        /// </summary>
        /// <param name="url">详细页url</param>
        /// <param name="_infoText">详细页对应列表页中的文本</param>
        public void GetHouseByUrl(string url, string _infoText)
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
                dicRegexInfo.Add("regex_yt", regex_yt);
                dicRegexInfo.Add("regex_jzxs", regex_jzxs);
                dicRegexInfo.Add("regex_ptss", regex_ptss);
                dicRegexInfo.Add("regex_title", regex_title);
                dicRegexInfo.Add("regex_phone", regex_phone);
                dicRegexInfo.Add("regex_address", regex_address);
                //dicRegexInfo.Add("regex_datetime", regex_datetime);
                dicRegexInfo.Add("regex_hymj", regex_hymj);
                dicRegexInfo.Add("regex_tjg", regex_tjg);
                dicRegexInfo.Add("regex_cwsl", regex_cwsl);
                dicRegexInfo.Add("regex_dxsmj", regex_dxsmj);
                dicRegexInfo.Add("regex_userId", regex_userId);
                Dictionary<string, RegexInfo> dicRegexInfo2 = new Dictionary<string, RegexInfo>();
                dicRegexInfo2.Add("regex_datetime", regex_datetime);
                //根据规则获取数据
                Dictionary<string, List<string>> dicRegexInfo_List = SpiderHelp.GetHtmlByRegex(url, "utf-8", dicRegexInfo, WebObj, CityId);
                Dictionary<string, List<string>> dicRegexInfo_List2 = SpiderHelp.GetStrByRegex(_infoText, dicRegexInfo2);
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
                string value_zx = dicRegexInfo_List["regex_zx"].Count < 1 ? "" : dicRegexInfo_List["regex_zx"][0];
                string value_yt = dicRegexInfo_List["regex_yt"].Count < 1 ? "" : dicRegexInfo_List["regex_yt"][0];
                string value_jzxs = dicRegexInfo_List["regex_jzxs"].Count < 1 ? "" : dicRegexInfo_List["regex_jzxs"][0];
                string value_ptss = dicRegexInfo_List["regex_ptss"].Count < 1 ? "" : dicRegexInfo_List["regex_ptss"][0];
                string value_phone = dicRegexInfo_List["regex_phone"].Count < 1 ? "" : dicRegexInfo_List["regex_phone"][0];
                string value_address = dicRegexInfo_List["regex_address"].Count < 1 ? "" : dicRegexInfo_List["regex_address"][0];
                string value_hymj = dicRegexInfo_List["regex_hymj"].Count < 1 ? "" : dicRegexInfo_List["regex_hymj"][0];
                string value_tjg = dicRegexInfo_List["regex_tjg"].Count < 1 ? "" : dicRegexInfo_List["regex_tjg"][0];
                string value_cwsl = dicRegexInfo_List["regex_cwsl"].Count < 1 ? "" : dicRegexInfo_List["regex_cwsl"][0];
                string value_dxsmj = dicRegexInfo_List["regex_dxsmj"].Count < 1 ? "" : dicRegexInfo_List["regex_dxsmj"][0];
                string value_userId = dicRegexInfo_List["regex_userId"].Count < 1 ? "" : dicRegexInfo_List["regex_userId"][0];
                string value_datetime = dicRegexInfo_List2["regex_datetime"].Count < 1 ? "" : dicRegexInfo_List2["regex_datetime"][0];
                //获取中介公司
                string userInfoUrl = "http://user.58.com/userdata?userid={0}&type=10";
                Dictionary<string, RegexInfo> dicRegexInfo3 = new Dictionary<string, RegexInfo>();
                dicRegexInfo3.Add("regex_comName", regex_comName);
                Dictionary<string, List<string>> dicRegexInfo_List3 = SpiderHelp.GetHtmlByRegex(string.Format(userInfoUrl, value_userId), "utf-8", dicRegexInfo3, WebObj, CityId);
                string value_comName = dicRegexInfo_List3["regex_comName"].Count < 1 ? "" : dicRegexInfo_List3["regex_comName"][0];

                //value_jznd = 转换建筑年代(value_jznd);
                value_datetime = 转换案例时间(value_datetime);
                //将数据添加到字典 
                NewHouse newHouse = new NewHouse(value_lpm, value_datetime, value_xzq, value_pq, "", "", "", value_mj, value_dj,
                                "", value_jg, "", value_zj, value_szlc, value_zlc, value_hx, value_cx, value_zx, value_jznd,
                                value_title, value_phone, url, "", 网站名称, value_address, value_jzxs, value_hymj, value_tjg, value_cwsl, value_ptss, value_dxsmj, value_comName,"");

                //当前数据为一天前的数据时
                newHouse.Alsj = newHouse.Alsj != null ? newHouse.Alsj.Trim() : newHouse.Alsj;

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
                log.Debug(string.Format("{0}数据保存完成url:{1}--cityname:{2}--value_title:{3}--value_lpm{4}--案例时间:{5}--爬取时间:{6}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), url, CityName, value_title, value_lpm, newHouse.Alsj, nowDate));
            }
            catch (Exception ex)
            {
                log.Error(string.Format("GetHouseByUrl()异常,url:{0}, cityName:{1}", url, CityName), ex);
            }
        }
        public string 转换案例时间(string _value_datetime)
        {
            int 时间区间 = 0;
            DateTime 当前时间 = DateTime.Now;
            if (string.IsNullOrEmpty(_value_datetime))
            {
                return 当前时间.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (_value_datetime.CheckStrIsDate())
            {
                return Convert.ToDateTime(_value_datetime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (Regex.IsMatch(_value_datetime, @"^[^\d]*(\d+)[^\d]*$", RegexOptions.IgnoreCase))
            {
                string result = Regex.Replace(_value_datetime, @"^[^\d]*(\d+)[^\d]*$", "$1", RegexOptions.IgnoreCase);
                时间区间 = Convert.ToInt32(result);
            }
            if (_value_datetime.Contains("年"))
            {
                当前时间 = 当前时间.AddYears(0 - 时间区间);
            }
            else if (_value_datetime.Contains("月"))
            {
                当前时间 = 当前时间.AddMonths(0 - 时间区间);
            }
            else if (_value_datetime.Contains("天") || _value_datetime.Contains("日"))
            {
                if (_value_datetime.Contains("昨"))
                {
                    时间区间 = 1;
                }
                当前时间 = 当前时间.AddDays(0 - 时间区间);
            }
            else if (_value_datetime.Contains("时"))
            {
                当前时间 = 当前时间.AddHours(0 - 时间区间);
            }
            else if (_value_datetime.Contains("分"))
            {
                当前时间 = 当前时间.AddMinutes(0 - 时间区间);
            }
            else if (_value_datetime.Contains("秒"))
            {
                当前时间 = 当前时间.AddSeconds(0 - 时间区间);
            }
            else
            {
                string[] strings = _value_datetime.Split('-');
                if (strings == null)
                {
                    return 当前时间.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (strings.Length < 3)
                {
                    _value_datetime = 当前时间.Year.ToString() + "-" + _value_datetime;
                }
                if (StringHelp.CheckStrIsDate(_value_datetime))
                {
                    当前时间 = Convert.ToDateTime(_value_datetime);
                }
            }
            return 当前时间.ToString("yyyy-MM-dd HH:mm:ss"); ;
        }
        /// <summary>
        /// 获取类型页面下案例
        /// </summary>
        /// <param name="param"></param>
        public void SpiderPage(object param)
        {
            ArrayList al = (ArrayList)param;
            int maxPageCount=Convert.ToInt32(al[0]);
            int maxPageLength=Convert.ToInt32(al[1]);
            string hostUrl=Convert.ToString(al[2]);
            string pageUrl=Convert.ToString(al[3]);
            string pageListIndexUrl=Convert.ToString(al[4]);
            int rate = Convert.ToInt32(al[5]);
            int pageCheckRate = Convert.ToInt32(al[6]);
            string pageRemark = Convert.ToString(al[7]);

            string 页面分页链接参数 = pageUrl + "pn{0}/";
            string 页面下一页链接 = pageUrl;
            int 当前总页数 = (maxPageCount - 1) / maxPageLength + 1;
            int 当前页码 = 1;
            while (!string.IsNullOrEmpty(页面下一页链接))
            {
                string nowPageList = 页面下一页链接;
                if (!页面下一页链接.ToLower().Contains("http://"))
                {
                    nowPageList = hostUrl + 页面下一页链接;
                }
                log.Debug(string.Format(网站名称 + "SpiderHouse()--获取{0}页面下一页链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", pageRemark, pageUrl, hostUrl, pageListIndexUrl, CityName));
                int reqCount = 0;
            begin3:
                SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 页面下一页链接);

                //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                if (string.IsNullOrEmpty(页面下一页链接))
                {
                    if (reqCount <= 3)
                    {
                        页面下一页链接 = string.Format(页面分页链接参数, 当前页码.ToString());
                        reqCount++;
                        System.Threading.Thread.Sleep(5000);
                        goto begin3;
                    }
                }
                当前页码++;
                if (isNowPageStop)
                {
                    break;
                }
            }
            log.Debug(string.Format(网站名称 + "SpiderHouse()--获取{0}页面下信息吸取完成,{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", 页面下一页链接,pageRemark, hostUrl, pageUrl, CityName));
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
        public void SpiderPage_Thread(int maxPageCount, int maxPageLength, string hostUrl, string pageUrl, string pageListIndexUrl, int rate, int pageCheckRate, string pageRemark)
        {
            ArrayList al = new ArrayList();
            al.Add(maxPageCount);
            al.Add(maxPageLength);
            al.Add(hostUrl);
            al.Add(pageUrl);
            al.Add(pageListIndexUrl);
            al.Add(rate);
            al.Add(pageCheckRate);
            al.Add(pageRemark);
            //SpiderPage(al);
            Thread m_thread = new Thread(new ParameterizedThreadStart(SpiderPage));
            m_thread.Start(al);
            log.Debug(string.Format("" + 网站名称 + "SpiderPage_Thread()--发布{0}页面Url:{1}下爬取线程,hostUrl:{2}, pageListUrl:{3}", CityName, pageUrl,hostUrl, pageListIndexUrl));
        }
        #endregion
    }
}
