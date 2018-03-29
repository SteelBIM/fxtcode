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

namespace FxtSpider.RunSource.邯郸恋家网
{
    public class NewDataSpider : INewDataRum
    {
        #region 初始化变量      
        protected DateTime nowDate = DateTime.Now;
        protected bool isNowPageStop = false;
        protected static string 网站名称 = WebsiteManager.邯郸恋家网;
        protected static int 网站ID = WebsiteManager.邯郸恋家网_ID;
        protected static readonly ILog log = LogManager.GetLogger(typeof(NewDataSpider));
        protected List<NewDataRum> NewDataRumList = new List<NewDataRum>();
        protected static 网站表 WebObj = WebsiteManager.GetWebById(WebsiteManager.邯郸恋家网_ID);
        /// <summary>
        /// 网站用途对应库用途字典
        /// </summary>
        protected Dictionary<string, string> purposeDic = new Dictionary<string, string>();
        #endregion
        #region(出售房源_页面需要爬取的各字段正则)
        ///// <summary>
        ///// 楼盘名
        ///// </summary>
        //protected RegexInfo regex_lpm = new RegexInfo("小区：</[^<>]+><[^<>]+><[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_lpm2 = new RegexInfo("小区：</[^<>]+><[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_lpm3 = new RegexInfo("<META content=\"([^<>\"\\,]+),[^\"]*\" name=keywords>", "$1");
        ///// <summary>
        ///// 行政区
        ///// </summary>
        //protected RegexInfo regex_xzq = new RegexInfo(">地址：</[^<>]+><[^<>]+>\\[([^<>\\[\\]]+)\\]", "$1");
        //protected RegexInfo regex_xzq2 = new RegexInfo(">地址：\\[([^<>\\[\\]]+)\\]", "$1");
        //protected RegexInfo regex_xzq3 = new RegexInfo(">地址：</[^<>]+>[^<>\\[]*\\[([^<>\\[\\]]+)\\]", "$1");
        ///// <summary>
        ///// 片区
        ///// </summary>
        //protected RegexInfo regex_pq = new RegexInfo("", "$1");
        ///// <summary>
        ///// 面积
        ///// </summary>
        //protected RegexInfo regex_mj = new RegexInfo(">面积：</[^<>]+><[^<>]+>([^<>]+)平米<", "$1");
        //protected RegexInfo regex_mj2 = new RegexInfo("房证面积：</[^<>]+><[^<>]+>([^<>]+)M<", "$1");
        ///// <summary>
        ///// 单价
        ///// </summary>
        //protected RegexInfo regex_dj = new RegexInfo(">\\(<[^<>]+>([^<>]+)</[^<>]+>元/", "$1");
        //protected RegexInfo regex_dj2 = new RegexInfo("单价：</[^<>]+>([^<>]+)元/m<", "$1");
        ///// <summary>
        ///// 结构
        ///// </summary>
        //protected RegexInfo regex_jg = new RegexInfo("", "$1");
        ///// <summary>
        ///// 用途
        ///// </summary>
        //protected RegexInfo regex_yt = new RegexInfo("", "$1");
        ///// <summary>
        ///// 总价
        ///// </summary>
        //protected RegexInfo regex_zj = new RegexInfo("售价：</[^<>]+><[^<>]+><[^<>]+>([^<>]+)<[^<>]+>万<", "$1");
        //protected RegexInfo regex_zj2 = new RegexInfo("售价：</[^<>]+><[^<>]+>([^<>]+)<", "$1");
        ///// <summary>
        ///// 所在楼层
        ///// </summary>
        //protected RegexInfo regex_szlc = new RegexInfo(">楼层：</[^<>]+><[^<>]+>第([^<>]+)层/[^<>]+<", "$1");
        //protected RegexInfo regex_szlc2 = new RegexInfo("楼层：</[^<>]+>([^<>]+)/[^<>]+<", "$1");
        ///// <summary>
        ///// 总楼层
        ///// </summary>
        //protected RegexInfo regex_zlc = new RegexInfo(">楼层：</[^<>]+><[^<>]+>第[^<>]*层/总([^<>]+)层<", "$1");
        //protected RegexInfo regex_zlc2 = new RegexInfo("楼层：</[^<>]+>[^<>]+/([^<>]+)层<", "$1");
        ///// <summary>
        ///// 户型
        ///// </summary>
        //protected RegexInfo regex_hx = new RegexInfo(">户型：</[^<>]+><[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_hx2 = new RegexInfo(">房型：</[^<>]+><[^<>]+>([^<>]+)<", "$1");
        ///// <summary>
        ///// 朝向
        ///// </summary>
        //protected RegexInfo regex_cx = new RegexInfo("朝向：</[^<>]+><[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_cx2 = new RegexInfo("朝向：</[^<>]+>([^<>]+)<", "$1");
        ///// <summary>
        ///// 装修
        ///// </summary>
        //protected RegexInfo regex_zx = new RegexInfo(">装修：</[^<>]+><[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_zx2 = new RegexInfo(">装修：</[^<>]+>([^<>]+)<", "$1");
        ///// <summary>
        ///// 建筑年代
        ///// </summary>
        //protected RegexInfo regex_jznd = new RegexInfo(">房龄：</[^<>]+><[^<>]+>([^<>]+)年<", "$1");
        //protected RegexInfo regex_jznd2 = new RegexInfo(">房龄：</[^<>]+>([^<>]+)年<", "$1");
        ///// <summary>
        ///// 信息(备注)
        ///// </summary>
        //protected RegexInfo regex_title = new RegexInfo("<[^<>]+class=\"de_info[^\"]*\"[^<>]*><h[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_title2 = new RegexInfo("<[^<>]+class=propInfoTitle>([^<>]+)<", "$1");
        ///// <summary>
        ///// 电话
        ///// </summary>
        //protected RegexInfo regex_phone = new RegexInfo("<[^<>]+class=\"tel[^\"]*\"[^<>]*><[^<>]+>[^<>]*<[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_phone2 = new RegexInfo("<[^<>]+class=telenumb><[^<>]+>([^<>]+)<", "$1");
        ///// <summary>
        ///// URL
        ///// </summary>
        //protected RegexInfo regex_infUrl = new RegexInfo("<ul[^<>]*class=\"boxText\"[^<>]*><[^<>]+><a[^<>]*href=\"([^\"]+)\"[^<>]*>[^<>]+", "$1");
        ///// <summary>
        ///// 列表页详细页面链接所在区域块html
        ///// </summary>
        //protected RegexInfo regex_infPanel = new RegexInfo("(<ul[^<>]*class=\"boxText\"[^<>]*>(?:(?!</ul>).)*</ul>)", "$1");
        ///// <summary>
        ///// 下一页正则
        ///// </summary>
        //protected RegexInfo regex_nextPage = new RegexInfo("<a href=\"([^<>]+)\"[^<>]*class=\"next\"[^<>]*>下一页</a>", "$1");
        ///// <summary>
        ///// 地址
        ///// </summary>
        //protected RegexInfo regex_address = new RegexInfo(">地址：</[^<>]+><[^<>]+>([^<>]+)<", "$1");
        //protected RegexInfo regex_address2 = new RegexInfo(">地址：</[^<>]+>([^<>]+)<", "$1");
        ///// <summary>
        ///// 信息的发布时间
        ///// </summary>
        //protected RegexInfo regex_datetime = new RegexInfo("<[^<>]+class=\"gray6[^\"]*\"[^<>]*>[^\\d]*([^<>\\-\\s]+\\-[^<>\\-\\s]+\\-[^<>\\-\\s]+[^<>]*)</li>", "$1");

        ///// <summary>
        ///// 中介公司
        ///// </summary>
        //protected RegexInfo regex_comName = new RegexInfo(">公司：([^<>]+)<", "$1");
        //protected RegexInfo regex_comName2 = new RegexInfo(">所属公司： ([^<>]+)<", "$1");
        ///// <summary>
        ///// 门店地址
        ///// </summary>
        //protected RegexInfo regex_comArea = new RegexInfo("", "$1");

        //protected RegexInfo 总页数正则 = new RegexInfo("<[^<>]+class=\"pageleft[^\"]*\"[^<>]*><[^<>]+></[^<>]+></[^<>]+><[^<>]+>[^<>]*/([^<>]+)</span>", "$1");

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
        /// 中介公司
        /// </summary>
        protected RegexInfo regex_comName = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_comName", 网站名称);
        /// <summary>
        /// 门店地址
        /// </summary>
        protected RegexInfo regex_comArea = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_comArea", 网站名称);
        protected RegexInfo 总页数正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("总页数正则", 网站名称);

        #endregion

        #region 构造函数
        public NewDataSpider()
        {
            //regex_lpm.RegexInfoList.Add(regex_lpm2);
            //regex_lpm.RegexInfoList.Add(regex_lpm3);
            //regex_xzq.RegexInfoList.Add(regex_xzq2);
            //regex_xzq.RegexInfoList.Add(regex_xzq3);
            //regex_mj.RegexInfoList.Add(regex_mj2);
            //regex_dj.RegexInfoList.Add(regex_dj2);
            //regex_zj.RegexInfoList.Add(regex_zj2);
            //regex_szlc.RegexInfoList.Add(regex_szlc2);
            //regex_zlc.RegexInfoList.Add(regex_zlc2);
            //regex_hx.RegexInfoList.Add(regex_hx2);
            //regex_cx.RegexInfoList.Add(regex_cx2);
            //regex_zx.RegexInfoList.Add(regex_zx2);
            //regex_jznd.RegexInfoList.Add(regex_jznd2);
            //regex_title.RegexInfoList.Add(regex_title2);
            //regex_phone.RegexInfoList.Add(regex_phone2);
            //regex_address.RegexInfoList.Add(regex_address2);
            //regex_comName.RegexInfoList.Add(regex_comName2);
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
            List<VIEW_网站爬取配置_城市表_网站表> list = SpiderWebConfigManager.获取邯郸恋家网下所有城市爬取配置();
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
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comName, "regex_comName", "公司")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comArea, "regex_comArea", "门店")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infUrl, "regex_infUrl", "url")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infPanel, "regex_infPanel", "列表页详细页面链接所在区域块html")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_nextPage, "regex_nextPage", "下一页正则"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(总页数正则, "总页数正则", "总页数正则"));
            
            //string str = stest.ToString();
            #endregion
            if (pageCheckRate > 0)
            {
                System.Threading.Thread.Sleep(pageCheckRate);
            }
            //开始获取页面
            try
            {

                //*******************根页面下信息列表爬取***********************//

                Dictionary<string, RegexInfo> 根页面正则字典集合 = new Dictionary<string, RegexInfo>();
                根页面正则字典集合.Add("*总页数", 总页数正则);
                Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(pageListIndexUrl, "gb2312", 根页面正则字典集合, WebObj, CityId, referer: pageListIndexUrl);
                int pageCount = 根页面正则字典集合结果["*总页数"].Count < 1 ? 0 : Convert.ToInt32(根页面正则字典集合结果["*总页数"][0]);
                string 根页面分页链接参数 = pageListIndexUrl.Replace("p-1", "p-{0}");
                string 根页面下一页链接 = pageListIndexUrl;
                int 当前总页数 = pageCount;
                int 当前页码 = 1;
                while (!string.IsNullOrEmpty(根页面下一页链接))
                {
                    string nowPageList = 根页面下一页链接;
                    if (!根页面下一页链接.ToLower().Contains("http://"))
                    {
                        nowPageList = hostUrl + 根页面下一页链接;
                    }
                    log.Debug(string.Format("邯郸恋家网SpiderHouse()--获取根页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                    SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 根页面下一页链接);
                    当前页码++;
                    //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                    if (string.IsNullOrEmpty(根页面下一页链接) && 当前页码 <= 当前总页数)
                    {
                        根页面下一页链接 = string.Format(根页面分页链接参数, 当前页码.ToString());
                    }
                    if (isNowPageStop)
                    {
                        break;
                    }
                }
                log.Debug(string.Format("邯郸恋家网SpiderHouse()--获取根页面下信息吸取完成,{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", 根页面下一页链接, hostUrl, pageListIndexUrl, CityName));

            }
            catch (Exception ex)
            {
                log.Error(string.Format("邯郸恋家网SpiderHouse()异常,hostUrl:{0}, pageListIndexUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName), ex);
            }
            log.Debug(string.Format("邯郸恋家网SpiderHouse()--获取{0}页面下详细信息Url吸取完成,详细页面url内容正则析取中-,hostUrl:{1}, pageListUrl:{2}", CityName, hostUrl, pageListIndexUrl));

        }

        #endregion

        #region 处理方法
        /// <summary>
        /// 根据详细页url获取信息
        /// </summary>
        /// <param name="url">详细页url</param>
        public void GetHouseByUrl(string url,string urlPanelHtml)
        {
            //url = "http://www.ljia.net/esf/esf/532875.html";
            try
            {
                //设置各字段规则(正则)
                Dictionary<string, RegexInfo> dicRegexInfo = new Dictionary<string, RegexInfo>();
                dicRegexInfo.Add("regex_lpm", regex_lpm);
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
                dicRegexInfo.Add("regex_yt", regex_yt);
                dicRegexInfo.Add("regex_zx", regex_zx);
                dicRegexInfo.Add("regex_title", regex_title);
                dicRegexInfo.Add("regex_phone", regex_phone);
                dicRegexInfo.Add("*regex_address", regex_address);
                dicRegexInfo.Add("regex_datetime", regex_datetime);
                dicRegexInfo.Add("regex_comName", regex_comName);
                dicRegexInfo.Add("regex_comArea", regex_comArea);
                Dictionary<string, List<string>> dicRegexInfo_List = new Dictionary<string, List<string>>();

                //根据规则获取数据
                dicRegexInfo_List = SpiderHelp.GetHtmlByRegex(url, "gb2312", dicRegexInfo, WebObj, CityId);
                List<string> dateList = SpiderHelp.GetStrByRegexByIndex(urlPanelHtml, regex_datetime);
                string value_title = dicRegexInfo_List["regex_title"].Count < 1 ? "" : dicRegexInfo_List["regex_title"][0];
                string value_lpm = dicRegexInfo_List["regex_lpm"].Count < 1 ? "" : dicRegexInfo_List["regex_lpm"][0];
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
                string value_address = dicRegexInfo_List["*regex_address"].Count < 1 ? "" : dicRegexInfo_List["*regex_address"][0];
                string value_datetime = dateList.Count < 1 ? "" : dateList[0];
                string value_comName = dicRegexInfo_List["regex_comName"].Count < 1 ? "" : dicRegexInfo_List["regex_comName"][0];
                string value_comArea = dicRegexInfo_List["regex_comArea"].Count < 1 ? "" : dicRegexInfo_List["regex_comArea"][0];
                
                //如果所在楼层和总楼层顺序颠倒
                if (StringHelp.IsInteger(value_zlc.TrimBlank()) && StringHelp.IsInteger(value_szlc.TrimBlank()))
                {
                    if (Convert.ToInt32(value_szlc.TrimBlank()) > Convert.ToInt32(value_zlc.TrimBlank()))
                    {
                        string a = value_szlc.TrimBlank();
                        value_szlc = value_zlc.TrimBlank();
                        value_zlc = a;
                    }
                }
                //将数据添加到字典 用于excel
                NewHouse newHouse = new NewHouse(value_lpm, value_datetime, value_xzq, value_pq, "", "", value_yt, value_mj, value_dj,
                                "", value_jg, "", value_zj, value_szlc, value_zlc, value_hx, value_cx, value_zx, value_jznd,
                                value_title, value_phone, url, "", 网站名称, value_address, "", "", "", "", "", "", value_comName, value_comArea);

                //当前数据为一天前的数据时
                newHouse.Alsj = newHouse.Alsj != null ? newHouse.Alsj.Trim() : newHouse.Alsj;
                if (!newHouse.Alsj.CheckStrIsDate())
                {
                    newHouse.Alsj = DateTime.Now.AddDays(-1).ToString();
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
                isNowPageStop = false;//暂时不限制时间
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
                string url_sz = pageListUrl;
                Dictionary<string, RegexInfo> dicRegexItem = new Dictionary<string, RegexInfo>();
                dicRegexItem.Add("*regex_infPanel", regex_infPanel);
                dicRegexItem.Add("*regex_nextPage", regex_nextPage);
                //发送请求获取根据正则获取网页html信息
                Dictionary<string, List<string>> dicRegexItem_List = SpiderHelp.GetHtmlByRegex(url_sz, "gb2312", dicRegexItem, WebObj, CityId, referer: url_sz);
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

        #endregion
    }
}
