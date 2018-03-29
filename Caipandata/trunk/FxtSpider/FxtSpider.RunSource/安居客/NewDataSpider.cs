using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;
using log4net;
using FxtSpider.Bll.SpiderCommon.Models;
using System.Text.RegularExpressions;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.DAL.LinqToSql;
using System.Threading;
using FxtSpider.Bll;
using FxtSpider.Bll.SpiderCommon;
using System.Collections;

namespace FxtSpider.RunSource.安居客
{
    public class NewDataSpider : INewDataRum
    {
        #region 初始化变量
        protected static string 网站名称 = WebsiteManager.安居客;
        protected static int 网站ID = WebsiteManager.安居客_ID;
        protected static readonly ILog log = LogManager.GetLogger(typeof(NewDataSpider));
        protected List<NewDataRum> NewDataRumList = new List<NewDataRum>();
        protected static 网站表 WebObj = WebsiteManager.GetWebById(WebsiteManager.安居客_ID);
        protected DateTime nowDate = DateTime.Now;
        protected bool isNowPageStop = false;
        #endregion
        #region(出售房源_页面需要爬取的各字段正则)
        ///// <summary>
        ///// 楼盘名
        ///// </summary>
        //protected RegexInfo regex_lpm = new RegexInfo("<p class=\"wid-40\">小区：</p><p>(([^<>（]*)（|<a id=\"text_for_school_1\" href=\"[^\"]*\" target=\"_blank\" title=\"[^\"]*\" _soj=\"saleup\">([^<>]*)</a>)", "$2$3");
        //protected RegexInfo regex_lpm2 = new RegexInfo("<div class=\"rightpart\">([^<>]+)</div>", "$1");
        ///// <summary>
        ///// 行政区
        ///// </summary>
        //protected RegexInfo regex_xzq = new RegexInfo("<p class=\"wid-40\">地址：</p><p><a href=\"[^\"]*\" target=\"_blank\">([^<>]*)</a>－", "$1");
        //protected RegexInfo regex_xzq2 = new RegexInfo("_zpq.push\\(\\['_setParams','[^']*','[^']*','([^']*)','[^']*','[^']*'\\]\\);", "$1");
        ///// <summary>
        ///// 片区
        ///// </summary>
        //protected RegexInfo regex_pq = new RegexInfo("<p class=\"wid-40\">地址：</p><p><a href=\"[^\"]*\" target=\"_blank\">[^<>]*</a>－<a href=\"[^\"]*\" target=\"_blank\">([^<>]*)</a>", "$1");
        //protected RegexInfo regex_pq2 = new RegexInfo("房源</a>&gt; [^\\&]*&gt; <a href=\"[^\"]*\" target=\"_blank\">([^<>]*)</a>", "$1");
        //protected RegexInfo regex_pq3 = new RegexInfo("<p[^<>]*>板块：</p><p><a[^<>]*>[^<>]*</a>－<a[^<>]*>([^<>]*)</a>", "$1");
        ///// <summary>
        ///// 面积
        ///// </summary>
        //protected RegexInfo regex_mj = new RegexInfo("<p[^<>]*>面积：</p><p[^<>]*>([^<>]*)平米</p>", "$1");
        //protected RegexInfo regex_mj2 = new RegexInfo("面积：([\\.\\d]+)平米", "$1");
        ///// <summary>
        ///// 单价
        ///// </summary>
        //protected RegexInfo regex_dj = new RegexInfo("<p[^<>]*>单价：</p><p>([^<>]*)元/平米</p>", "$1");
        //protected RegexInfo regex_dj2 = new RegexInfo("单价：([\\.\\d]+) 元/平米</li>", "$1");
        ///// <summary>
        ///// 结构
        ///// </summary>
        //protected RegexInfo regex_jg = new RegexInfo("", "$2");
        ///// <summary>
        ///// 总价
        ///// </summary>
        //protected RegexInfo regex_zj = new RegexInfo("<p class=\"wid-40\">报价：</p><p class=\"A-common-price fs-16\"><span class=\"fs-24 highlightPrice\">([^<>]*)</span>&nbsp;万</p>", "$1");
        //protected RegexInfo regex_zj2 = new RegexInfo("售价：<em>([^<>]*)</em>", "$1");
        //protected RegexInfo regex_zj3 = new RegexInfo("售价：</[^<>]*><[^<>]*><[^<>]*>([^<>]*)</[^<>]*>", "$1");
        ///// <summary>
        ///// 所在楼层
        ///// </summary>
        //protected RegexInfo regex_szlc = new RegexInfo("<p class=\"[^\"]*\">楼层：</p><p class=\"[^\"]*\">(\\d*)/\\d*</p>", "$1");
        //protected RegexInfo regex_szlc2 = new RegexInfo("楼层：(\\d*)/\\d+[^<>]*</li>", "$1");
        ///// <summary>
        ///// 总楼层
        ///// </summary>
        //protected RegexInfo regex_zlc = new RegexInfo("<p class=\"[^\"]*\">楼层：</p><p class=\"[^\"]*\">(\\d*/(\\d*)|共(\\d*)层)</p>", "$2$3");
        //protected RegexInfo regex_zlc2 = new RegexInfo("楼层：\\d*/(\\d+)[^<>]*</li>", "$1");
        ///// <summary>
        ///// 户型
        ///// </summary>
        //protected RegexInfo regex_hx = new RegexInfo("<p[^<>]*>房型：</p><p[^<>]*>([^<>]*)</p>", "$1");
        //protected RegexInfo regex_hx2 = new RegexInfo("房型：([^<>]*)</li>", "$1");
        ///// <summary>
        ///// 朝向
        ///// </summary>
        //protected RegexInfo regex_cx = new RegexInfo("<p[^<>]*>朝向：</p><p[^<>]*>([^<>]*)</p>", "$1");
        //protected RegexInfo regex_cx2 = new RegexInfo("朝向：([^<>]*)</li>", "$1");
        ///// <summary>
        ///// 装修
        ///// </summary>
        //protected RegexInfo regex_zx = new RegexInfo("<p class=\"wid-40\">装修：</p><p>([^<>]*)</p>", "$1");
        //protected RegexInfo regex_zx2 = new RegexInfo("装修：([^<>]*)</li>", "$1");
        ///// <summary>
        ///// 建筑年代
        ///// </summary>
        //protected RegexInfo regex_jznd = new RegexInfo("<p[^<>]*>建造年代：</p><p[^<>]*>(\\d*)年</p>", "$1");
        //protected RegexInfo regex_jznd2 = new RegexInfo("建造年代：(\\d*)年</li>", "$1");
        ///// <summary>
        ///// 信息(备注)
        ///// </summary>
        //protected RegexInfo regex_title = new RegexInfo("<h1 class=\"A-fangyuan-title\">([^<>]*)</h1>", "$1");
        //protected RegexInfo regex_title2 = new RegexInfo("<h1 class=\"propInfoTitle\">([^<>]+)</h1>", "$1");
        ///// <summary>
        ///// 电话
        ///// </summary>
        //protected RegexInfo regex_phone = new RegexInfo("<p class=\"number\">([\\d\\s]*)</p>", "$1");
        //protected RegexInfo regex_phone2 = new RegexInfo("<div class=\"number\"><b style=\"font-size: 20px;\">([^<>]+)</b></div>", "$1");
        ///// <summary>
        ///// URL
        ///// </summary>
        //protected RegexInfo regex_infUrl = new RegexInfo("<a class=\"t\" id=\"prop_name_qt_prop_\\d+\" href=\"([^\"]*)\" target=\"_blank\"[^<>]*>[^<>]*</a>", "$1");
        ///// <summary>
        ///// 下一页正则
        ///// </summary>
        //protected RegexInfo regex_nextPage = new RegexInfo("<a href=\"([^\"]+)\" class=\"aNxt\">下一页 \\&gt;</a>", "$1");
        ///// <summary>
        ///// 地址
        ///// </summary>
        //protected RegexInfo regex_address = new RegexInfo("<dd class=\"dd1\">小区地址：\\[[^\\[\\]]*\\]([^<>]*)</dd>", "$1");
        //protected RegexInfo regex_address2 = new RegexInfo("地&nbsp;&nbsp;&nbsp;&nbsp;址：</th><td style=\"overflow:hidden;word-break:break-all;width:200px;\">([^<>]+)</td>", "$1");
        ///// <summary>
        ///// 信息的发布时间
        ///// </summary>
        //protected RegexInfo regex_datetime = new RegexInfo("<span class=\"[^\"]*\">([^<>]+)发布</span>", "$1");

        ///// <summary>
        ///// 中介公司
        ///// </summary>
        //protected RegexInfo regex_comName = new RegexInfo("公司：</p><p[^<>]*><a[^<>]*>([^<>]*)</a>", "$1");
        ///// <summary>
        ///// 门店地址
        ///// </summary>
        //protected RegexInfo regex_comArea = new RegexInfo("门店：</p><p[^<>]*><a[^<>]*>([^<>]*)</a>", "$1");
        //RegexInfo 总条数正则 = new RegexInfo("房<i>(\\d+)</i>套</span>", "$1");
        //RegexInfo 行政区文本正则 = new RegexInfo("<div class=\"[^\"]*\" id=\"apf_id_10_areacontainer\">((?:(?!div).)*)</div>", "$1");
        //RegexInfo 行政区链接正则 = new RegexInfo("<a href=\"([^\"]+)\"[^<>]*>[^<>]*</a>", "$1");
        //RegexInfo 片区文本正则 = new RegexInfo("<div class=\"[^\"]*\" id=\"apf_id_10_blockcontainer\">((?:(?!div).)*)</div>", "$1");
        //RegexInfo 片区链接正则 = new RegexInfo("<a href=\"([^\"]+)\"[^<>]*>[^<>]+</a>", "$1");
        //RegexInfo 房型文本正则 = new RegexInfo("<div class=\"condition_title\">房型：</div>[^<>]*<div class=\"container\">((?:(?!div).)*)</div>[^<>]*</div>", "$1");
        //RegexInfo 房型链接正则 = new RegexInfo("<a href=\"([^\"]+)\" rel=\"nofollow\">[^<>]+</a>", "$1");

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
        /// URL整体信息
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
        protected RegexInfo 总条数正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("总条数正则", 网站名称);// new RegexInfo("房<i>(\\d+)</i>套</span>", "$1");
        protected RegexInfo 行政区文本正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("行政区文本正则", 网站名称);// new RegexInfo("<div class=\"[^\"]*\" id=\"apf_id_10_areacontainer\">((?:(?!div).)*)</div>", "$1");
        protected RegexInfo 行政区链接正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("行政区链接正则", 网站名称);// new RegexInfo("<a href=\"([^\"]+)\"[^<>]*>[^<>]*</a>", "$1");
        protected RegexInfo 片区文本正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("片区文本正则", 网站名称);// new RegexInfo("<div class=\"[^\"]*\" id=\"apf_id_10_blockcontainer\">((?:(?!div).)*)</div>", "$1");
        protected RegexInfo 片区链接正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("片区链接正则", 网站名称);// new RegexInfo("<a href=\"([^\"]+)\"[^<>]*>[^<>]+</a>", "$1");
        protected RegexInfo 房型文本正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("房型文本正则", 网站名称);// new RegexInfo("<div class=\"condition_title\">房型：</div>[^<>]*<div class=\"container\">((?:(?!div).)*)</div>[^<>]*</div>", "$1");
        protected RegexInfo 房型链接正则 = SpiderRegexInfoHelp.GetRegexInfoByXmlName("房型链接正则", 网站名称);// new RegexInfo("<a href=\"([^\"]+)\" rel=\"nofollow\">[^<>]+</a>", "$1");

        #endregion
        #region 构造函数
        public NewDataSpider()
        {
            //regex_lpm.RegexInfoList.Add(regex_lpm2);
            //regex_xzq.RegexInfoList.Add(regex_xzq2);
            //regex_pq.RegexInfoList.Add(regex_pq2);
            //regex_pq.RegexInfoList.Add(regex_pq3);
            //regex_mj.RegexInfoList.Add(regex_mj2);
            //regex_dj.RegexInfoList.Add(regex_dj2);
            //regex_zj.RegexInfoList.Add(regex_zj2);
            //regex_zj.RegexInfoList.Add(regex_zj3);
            //regex_szlc.RegexInfoList.Add(regex_szlc2);
            //regex_zlc.RegexInfoList.Add(regex_zlc2);
            //regex_hx.RegexInfoList.Add(regex_hx2);
            //regex_cx.RegexInfoList.Add(regex_cx2);
            //regex_zx.RegexInfoList.Add(regex_zx2);
            //regex_jznd.RegexInfoList.Add(regex_jznd2);
            //regex_title.RegexInfoList.Add(regex_title2);
            //regex_phone.RegexInfoList.Add(regex_phone2);
            //regex_address.RegexInfoList.Add(regex_address2);
           
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
        /// 用于线程调用的先进先出的url(用于线程方法ProcessQueue)
        /// </summary>
        public Queue<string> Url_workload
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
        public void start()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = SpiderWebConfigManager.获取安居客下所有城市爬取配置();
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
                CaseManager.往案例表插入爬取数据(网站名称: 网站名称, 城市名称:CityName, 网站ID: 网站ID, 城市ID: CityId,
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
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_datetime, "regex_datetime", "时间")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comName, "regex_comName", "公司")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comArea, "regex_comArea", "门店")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infUrl, "regex_infUrl", "url")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_nextPage, "regex_nextPage", "下一页正则")); 
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(总条数正则, "总条数正则", "总条数正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区文本正则, "行政区文本正则", "行政区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区链接正则, "行政区链接正则", "行政区链接正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区文本正则, "片区文本正则", "片区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区链接正则, "片区链接正则", "片区链接正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(房型文本正则, "房型文本正则", "房型文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(房型链接正则, "房型链接正则", "房型链接正则")); ;
            //string str = stest.ToString();
            #endregion
            int maxPageCount = 100;
            int maxPageLength = 25;
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
                片区页面正则字典集合.Add("*房型文本", 房型文本正则);
                Dictionary<string, RegexInfo> 房型链接字典集合 = new Dictionary<string, RegexInfo>();
                房型链接字典集合.Add("*房型链接", 房型链接正则);
                Dictionary<string, RegexInfo> 房型页面正则字典集合 = new Dictionary<string, RegexInfo>();
                log.Debug(string.Format("安居客SpiderHouse()--获取根页面的总条数,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName));
                Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(pageListIndexUrl, "utf-8", 根页面正则字典集合, WebObj, CityId);
                int count = 根页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(根页面正则字典集合结果["*总条数"][0]);
                log.Debug(string.Format("安居客SpiderHouse()--获取根页面的总条数为{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", count.ToString(), hostUrl, pageListIndexUrl, CityName));
                //当前根页面总个数大于最大值
                string 行政区文本 = 根页面正则字典集合结果["*行政区文本"].Count < 1 ? "" : 根页面正则字典集合结果["*行政区文本"][0];
                Dictionary<string, List<string>> 行政区链接结果 = SpiderHelp.GetStrByRegex(行政区文本, 行政区链接字典集合);
                List<string> 行政区链接List = 行政区链接结果["*行政区链接"];
                if (count > maxCount && 行政区链接List.Count>0)
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
                        //个数获取
                        log.Debug(string.Format("安居客SpiderHouse()--获取当前行政区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl, hostUrl, pageListIndexUrl, CityName));
                        Dictionary<string, List<string>> 行政区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl, "utf-8", 行政区页面正则字典集合, WebObj, CityId);
                        int _count = 行政区页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(行政区页面正则字典集合结果["*总条数"][0]);
                        log.Debug(string.Format("安居客SpiderHouse()--获取当前行政区页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count.ToString(), nowUrl, hostUrl, pageListIndexUrl, CityName));

                        //当前行政区页面总个数大于最大值
                        string 片区文本 = 行政区页面正则字典集合结果["*片区文本"].Count < 1 ? "" : 行政区页面正则字典集合结果["*片区文本"][0];
                        Dictionary<string, List<string>> 片区链接结果 = SpiderHelp.GetStrByRegex(片区文本, 片区链接字典集合);
                        List<string> 片区链接List = 片区链接结果["*片区链接"];
                        if (_count > maxCount && 片区链接List.Count>0)
                        {
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
                                log.Debug(string.Format("安居客SpiderHouse()--获取当前片区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl2, hostUrl, pageListIndexUrl, CityName));
                                Dictionary<string, List<string>> 片区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl2, "utf-8", 片区页面正则字典集合, WebObj, CityId);
                                int _count2 = 片区页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(片区页面正则字典集合结果["*总条数"][0]);
                                log.Debug(string.Format("安居客SpiderHouse()--获取当前片区页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count2.ToString(), nowUrl2, hostUrl, pageListIndexUrl, CityName));
                                //当前片区页面总条数大于最大值
                                if (_count2 > maxCount)
                                {
                                    string 房型文本 = 片区页面正则字典集合结果["*房型文本"].Count < 1 ? "" : 片区页面正则字典集合结果["*房型文本"][0];
                                    Dictionary<string, List<string>> 房型链接结果 = SpiderHelp.GetStrByRegex(房型文本, 房型链接字典集合);
                                    List<string> 房型链接List = 房型链接结果["*房型链接"];
                                    foreach (string _url3 in 房型链接List)
                                    {
                                        isNowPageStop = false;
                                        string nowUrl3 = _url3;
                                        if (!_url2.ToLower().Contains("http://"))
                                        {
                                            nowUrl3 = hostUrl + _url3;
                                        }
                                        //获取个数
                                        log.Debug(string.Format("安居客SpiderHouse()--获取当前房型页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl3, hostUrl, pageListIndexUrl, CityName));
                                        Dictionary<string, List<string>> 房型页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl2, "utf-8", 房型页面正则字典集合, WebObj, CityId);
                                        int _count3 = 房型页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(房型页面正则字典集合结果["*总条数"][0]);
                                        log.Debug(string.Format("安居客SpiderHouse()--获取当前房型页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count3.ToString(), nowUrl2, hostUrl, pageListIndexUrl, CityName));
                                        //*******************房型页面下信息列表爬取***********************//
                                        SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, nowUrl3, pageListIndexUrl, rate, pageCheckRate, _count3, "房型");
                                        
                                    }
                                }
                                else
                                {
                                    //*******************片区页面下信息列表爬取***********************//
                                    SpiderPage_Thread(maxPageCount, maxPageLength, hostUrl, nowUrl2, pageListIndexUrl, rate, pageCheckRate, _count2, "片区");
                                }
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
                log.Error(string.Format("安居客SpiderHouse()异常,hostUrl:{0}, pageListIndexUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName), ex);
            }
            log.Debug(string.Format("安居客SpiderHouse()--获取{0}页面下详细信息Url吸取完成,详细页面url内容正则析取中-,hostUrl:{1}, pageListUrl:{2}", CityName, hostUrl, pageListIndexUrl));
         
        }

        #endregion

        #region 处理方法
        /// <summary>
        /// 根据详细页url获取信息
        /// </summary>
        /// <param name="url">详细页url</param>
        public void GetHouseByUrl(string url,string urlHtml)
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
                //dicRegexInfo.Add("regex_jznd", regex_jznd);
                dicRegexInfo.Add("regex_cx", regex_cx);
                dicRegexInfo.Add("regex_szlc", regex_szlc);
                dicRegexInfo.Add("regex_zlc", regex_zlc);
                dicRegexInfo.Add("regex_jg", regex_jg);
                dicRegexInfo.Add("regex_zx", regex_zx);
                dicRegexInfo.Add("regex_title", regex_title);
                dicRegexInfo.Add("regex_phone", regex_phone);
                dicRegexInfo.Add("regex_address", regex_address);
                //dicRegexInfo.Add("regex_datetime", regex_datetime);
                dicRegexInfo.Add("regex_comName", regex_comName);
                dicRegexInfo.Add("regex_comArea", regex_comArea);
                Dictionary<string, List<string>> dicRegexInfo_List = new Dictionary<string, List<string>>();

                //根据规则获取数据
                dicRegexInfo_List = SpiderHelp.GetHtmlByRegex(url, "utf-8", dicRegexInfo, WebObj, CityId);
                List<string> jzndList = SpiderHelp.GetStrByRegexByIndex(urlHtml, regex_jznd);
                List<string> dateList = SpiderHelp.GetStrByRegexByIndex(urlHtml, regex_datetime);
                string value_title = dicRegexInfo_List["regex_title"].Count < 1 ? "" : dicRegexInfo_List["regex_title"][0];
                string value_lpm = dicRegexInfo_List["*regex_lpm"].Count < 1 ? "" : dicRegexInfo_List["*regex_lpm"][0];
                string value_xzq = dicRegexInfo_List["*regex_xzq"].Count < 1 ? "" : dicRegexInfo_List["*regex_xzq"][0];
                string value_pq = dicRegexInfo_List["regex_pq"].Count < 1 ? "" : dicRegexInfo_List["regex_pq"][0];
                string value_hx = dicRegexInfo_List["regex_hx"].Count < 1 ? "" : dicRegexInfo_List["regex_hx"][0];
                string value_mj = dicRegexInfo_List["*regex_mj"].Count < 1 ? "" : dicRegexInfo_List["*regex_mj"][0];
                string value_dj = dicRegexInfo_List["*regex_dj"].Count < 1 ? "" : dicRegexInfo_List["*regex_dj"][0];
                string value_zj = dicRegexInfo_List["*regex_zj"].Count < 1 ? "" : dicRegexInfo_List["*regex_zj"][0];
                string value_jznd = jzndList.Count < 1 ? "" : jzndList[0]; //dicRegexInfo_List["regex_jznd"].Count < 1 ? "" : dicRegexInfo_List["regex_jznd"][0];
                string value_cx = dicRegexInfo_List["regex_cx"].Count < 1 ? "" : dicRegexInfo_List["regex_cx"][0];
                string value_szlc = dicRegexInfo_List["regex_szlc"].Count < 1 ? "" : dicRegexInfo_List["regex_szlc"][0];
                string value_zlc = dicRegexInfo_List["regex_zlc"].Count < 1 ? "" : dicRegexInfo_List["regex_zlc"][0];
                string value_jg = dicRegexInfo_List["regex_jg"].Count < 1 ? "" : dicRegexInfo_List["regex_jg"][0];
                string value_zx = dicRegexInfo_List["regex_zx"].Count < 1 ? "" : dicRegexInfo_List["regex_zx"][0];
                string value_phone = dicRegexInfo_List["regex_phone"].Count < 1 ? "" : dicRegexInfo_List["regex_phone"][0];
                string value_address = dicRegexInfo_List["regex_address"].Count < 1 ? "" : dicRegexInfo_List["regex_address"][0];
                string value_datetime = dateList.Count < 1 ? "" : dateList[0]; //dicRegexInfo_List["regex_datetime"].Count < 1 ? "" : dicRegexInfo_List["regex_datetime"][0];
                string value_comName = dicRegexInfo_List["regex_comName"].Count < 1 ? "" : dicRegexInfo_List["regex_comName"][0];
                string value_comArea = dicRegexInfo_List["regex_comArea"].Count < 1 ? "" : dicRegexInfo_List["regex_comArea"][0];
                //将数据添加到字典 用于excel
                NewHouse newHouse = new NewHouse(value_lpm, GetCaseDate(value_datetime), value_xzq, value_pq, "", "", "", value_mj, value_dj,
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
                log.Debug(string.Format("{0}数据保存完成url:{1}--cityname:{2}--value_title:{3}--value_lpm{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), url, CityName, value_title, value_lpm));
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
                //dicRegexItem.Add("*regex_infUrl", regex_infUrl);
                 dicRegexItem.Add("*regex_infPanel", regex_infPanel);
                dicRegexItem.Add("*regex_nextPage", regex_nextPage);
                //发送请求获取根据正则获取网页html信息
                Dictionary<string, List<string>> dicRegexItem_List = SpiderHelp.GetHtmlByRegex(url_sz, "utf-8", dicRegexItem, WebObj, CityId);
                List<string> list = dicRegexItem_List["*regex_infPanel"];
                下一页链接 = dicRegexItem_List["*regex_nextPage"].Count < 1 ? "" : dicRegexItem_List["*regex_nextPage"][0];
                foreach (string strUrlHtml in list)
                {

                    if (rate > 0)
                    {
                        System.Threading.Thread.Sleep(rate);
                    }
                    List<string> urlList = SpiderHelp.GetStrByRegexByIndex(strUrlHtml, regex_infUrl);
                    string strUrl = urlList == null || urlList.Count < 1 ? "" : urlList[0];
                    string nowUrl = strUrl;
                    //如果当前url不带域名
                    if (!strUrl.ToLower().Contains("http://"))
                    {
                        nowUrl = hostUrl + strUrl;
                    }
                    GetHouseByUrl(nowUrl, strUrlHtml);
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

            string 页面分页链接参数 = pageUrl.Replace("o5/", "o5-p{0}/");
            string 页面下一页链接 = pageUrl;
            int 当前总页数 = (count - 1) / maxPageLength + 1;
            int 当前页码 = 1;
            while (!string.IsNullOrEmpty(页面下一页链接))
            {
                string nowPageList = 页面下一页链接;
                if (!页面下一页链接.ToLower().Contains("http://"))
                {
                    nowPageList = hostUrl + 页面下一页链接;
                }
                log.Debug(string.Format("安居客SpiderHouse()--获取根页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 页面下一页链接);
                 当前页码++;
                //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                if (string.IsNullOrEmpty(页面下一页链接) && 当前页码 <= 当前总页数)
                {
                    页面下一页链接 = string.Format(页面分页链接参数, 当前页码.ToString());
                }
                if (isNowPageStop)
                {
                    break;
                }
            }
            log.Debug(string.Format("安居客SpiderHouse()--获取{0}页面下信息吸取完成,{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}",pageRemark, 页面下一页链接, hostUrl, pageListIndexUrl, CityName));
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
        public void SpiderPage_Thread(int maxPageCount, int maxPageLength, string hostUrl, string pageUrl, string pageListIndexUrl, int rate, int pageCheckRate,int count, string pageRemark)
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

        /// <summary>
        /// (获取案例时间)根据信息的发布日期,当前时间和最后更新的间隔
        /// </summary>
        /// <param name="pubDate">根据信息的发布日期</param>
        /// <param name="upTime">最后更新与发布日期的间隔</param>
        public string GetCaseDate(string upTime)
        {
            int 时间区间 = 0;
            DateTime 当前时间 = DateTime.Now;
            if (string.IsNullOrEmpty(upTime))
            {
                return 当前时间.ToString("yyyy-MM-dd");
            }
            if (upTime.CheckStrIsDate() && Regex.IsMatch(upTime, "\\d+\\-\\d+\\-\\d+[^<>]*", RegexOptions.IgnoreCase))
            {
                return Convert.ToDateTime(upTime).ToString("yyyy-MM-dd HH:mm");
            }
            if (Regex.IsMatch(upTime, @"^(\d+)[^\d]+$", RegexOptions.IgnoreCase))
            {
                string result = Regex.Replace(upTime, @"^(\d+)[^\d]+$", "$1", RegexOptions.IgnoreCase);
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
                string h = "";
                if (upTime.Contains("前天"))
                {
                    当前时间 = Convert.ToDateTime(当前时间.AddDays(-2).ToString("yyyy-MM-dd"));
                }
                else if (upTime.Contains("昨天"))
                {
                    当前时间 = Convert.ToDateTime(当前时间.AddDays(-1).ToString("yyyy-MM-dd"));
                }
                else if (upTime.Contains("今天") || upTime.Contains("当天"))
                {
                    当前时间 = Convert.ToDateTime(当前时间.ToString("yyyy-MM-dd"));
                }
                else 
                {
                    当前时间 = 当前时间.AddDays(0 - 时间区间);
                }
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
            return 当前时间.ToString("yyyy-MM-dd HH:mm");
        }
        #endregion
    }
}
