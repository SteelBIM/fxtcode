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


namespace FxtSpider.RunSource.新浪二手房
{
    public class NewDataSpider : INewDataRum
    {
        #region 初始化变量
        /// <summary>
        /// 当前页面编码
        /// </summary>
        protected string NowPageEncoding = "utf-8";
        protected static string 网站名称 = WebsiteManager.新浪二手房;
        protected static int 网站ID = WebsiteManager.新浪二手房_ID;
        protected static readonly ILog log = LogManager.GetLogger(typeof(NewDataSpider));
        protected List<NewDataRum> NewDataRumList = new List<NewDataRum>();
        protected static 网站表 WebObj = WebsiteManager.GetWebById(WebsiteManager.新浪二手房_ID);
        //protected 新浪二手房_Log 新浪Log;
        protected DateTime nowDate = DateTime.Now;
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
        //protected RegexInfo regex_lpm = new RegexInfo("<li class=\"CommunityName\">小区名称：(<a href=\"[^\"]+\" target=\"_blank\">([^<>]+)（查看小区信息）</a>|([^<>]+))</li>", "$2$3");
        //protected RegexInfo regex_lpm2 = new RegexInfo("<li>小区名称：(<a class=\"c_default\"[^<>]*>([^<>]+)</a>|([^<>]+))", "$2$3");
        //protected RegexInfo regex_lpm3 = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //    + "[^<>]*<em>&gt;</em>[^<>]*<em>&gt;</em>"
        //    + "([^<>]*)<em>&gt;</em>[^<>]*</", "$1");
        //protected RegexInfo regex_lpm5 = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //                        + "(<a href=\"[^\"]*\" target=\"_blank\">[^<>]*</a><em>&gt;</em>"
        //                        + "<a href=\"[^\"]*\" target=\"_blank\">[^<>]*</a><em>&gt;</em>"
        //                        + "(<a target=\"_blank\" href=\"[^\"]*\">([^<>]*)</a>|([^<>]*))|"
        //                        + "([^<>]*)<em>&gt;</em>[^<>]*</)", "$3$4$5");
        //protected RegexInfo regex_lpm4 = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //    + "<a href=\"[^\"]*\" target=\"_blank\">[^<>]*</a><em>&gt;</em>[^<>]*<em>&gt;</em>([^<>]*)<em>&gt;</em>[^<>]*</", "$1");
        ///// <summary>
        ///// 行政区
        ///// </summary>
        //protected RegexInfo regex_xzq = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //                        + "<a href=\"[^\"]*\" target=\"_blank\">([^<>]*)</a>", "$1");
        //protected RegexInfo regex_xzq2 = new RegexInfo("<span class=\"name\">您现在的位置：</span><a href=\"[^\"]*\">[^<>]+</a><span>></span>"
        //                                              + "<a href=\"[^\"]*\">([^<>]*)</a>", "$1");
        //protected RegexInfo regex_xzq3 = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //                                + "([^<>]+)", "$1");
        ////protected RegexInfo regex_xzq4 = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        ////                                + "<a href=\"[^\"]*\" target=\"_blank\">([^<>]*)</a>", "$1");
        ///// <summary>
        ///// 片区
        ///// </summary>
        //protected RegexInfo regex_pq = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //                        + "<a href=\"[^\"]*\" target=\"_blank\">[^<>]*</a><em>&gt;</em>"
        //                        + "<a href=\"[^\"]*\" target=\"_blank\">([^<>]*)</a><em>&gt;</em>"
        //                        + "(<a target=\"_blank\" href=\"[^\"]*\">[^<>]*</a>|[^<>]*)", "$1");
        //protected RegexInfo regex_pq2 = new RegexInfo("<span class=\"name\">您现在的位置：</span><a href=\"[^\"]*\">[^<>]+</a><span>></span>"
        //                                              + "<a href=\"[^\"]*\">[^<>]*</a><span>></span>[^<>]*"
        //                                              + "<a href=\"[^\"]*\">([^<>]*)</a><span>></span>[^<>]*"
        //                                              + "(<a href=\"[^\"]*\">[^<>]*</a>|[^<>]*)", "$1");
        //protected RegexInfo regex_pq3 = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //                                                + "[^<>]*<em>&gt;</em>([^<>]*)<em>&gt;</em>"
        //                                                + "[^<>]*<em>&gt;</em>[^<>]*</", "$1");
        //protected RegexInfo regex_pq4 = new RegexInfo("<strong>您现在的位置：</strong><a href=\"[^\"]*\">[^<>]*</a><em>&gt;</em>"
        //    + "<a href=\"[^\"]*\" target=\"_blank\">[^<>]*</a><em>&gt;</em>([^<>]*)<em>&gt;</em>[^<>]*<em>&gt;</em>[^<>]*</", "$1");
        ///// <summary>
        ///// 面积
        ///// </summary>
        //protected RegexInfo regex_mj = new RegexInfo("(面&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;积：([^<>]*)&nbsp;平米|<span class=\"rz-infomore\">([^<>]*)㎡<span class=\"mlr5 c_a5\">\\|</span>)", "$2$3");
        //protected RegexInfo regex_mj2 = new RegexInfo("面  积：</span><strong>([^<>]*)</strong>平米", "$1");
        ///// <summary>
        ///// 单价
        ///// </summary>
        //protected RegexInfo regex_dj = new RegexInfo("（<span class=\"c_red\">([^<>]*)</span>[^/]*/㎡）", "$1");
        //protected RegexInfo regex_dj2 = new RegexInfo("单 价：</span><strong>([^<>]*)</strong>元/平米", "$1");
        ///// <summary>
        ///// 结构
        ///// </summary>
        //protected RegexInfo regex_jg = new RegexInfo("", "$2");
        ///// <summary>
        ///// 总价
        ///// </summary>
        //protected RegexInfo regex_zj = new RegexInfo("(<span class=\"bold yahei c_red f18\">([^<>]*)</em>&nbsp;万元</span>|<em class=\"f24 bold c_red tahoma\">([^<>]*)</em>&nbsp;万元（)", "$2$3");
        //protected RegexInfo regex_zj2 = new RegexInfo("总 价：</span><span class=\"f12br hIPrice\">([^<>]*)</span> 万元", "$1");
        ///// <summary>
        ///// 所在楼层
        ///// </summary>
        //protected RegexInfo regex_szlc = new RegexInfo("(<span title=\"[^\"]*\">第(\\d*)层、共\\d*层</span>|楼&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;层：第(\\d*)层、共\\d*层)", "$2$3");
        //protected RegexInfo regex_szlc2 = new RegexInfo("楼 层</span>：<span class=\"hs\">第(\\d+)层[^<>]*</span>", "$1");
        ///// <summary>
        ///// 总楼层
        ///// </summary>
        //protected RegexInfo regex_zlc = new RegexInfo("(<span title=\"[^\"]*\">(第\\d*层、|)共(\\d*)层</span>|楼&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;层：第\\d*层、共(\\d*)层)", "$3$4");
        //protected RegexInfo regex_zlc2 = new RegexInfo("楼 层</span>：<span class=\"hs\">[^<>]*共(\\d+)层</span>", "$1");
        ///// <summary>
        ///// 户型
        ///// </summary>
        //protected RegexInfo regex_hx = new RegexInfo("(\\d*室\\d*厅\\d*卫)", "$1");
        ///// <summary>
        ///// 朝向
        ///// </summary>
        //protected RegexInfo regex_cx = new RegexInfo("<span class=\"mr50\">朝向：([^<>]*)</span>", "$1");
        //protected RegexInfo regex_cx2 = new RegexInfo("朝 向</span>：<span class=\"hs\">([^<>]*)</span>", "$1");
        ///// <summary>
        ///// 装修
        ///// </summary>
        //protected RegexInfo regex_zx = new RegexInfo("<span class=\"mr50\">装修情况：([^<>]*)</span>", "$1");
        //protected RegexInfo regex_zx2 = new RegexInfo("装修情况：<span class=\"hs\">([^<>]*)</span>", "$1");
        ///// <summary>
        ///// 建筑年代
        ///// </summary>
        //protected RegexInfo regex_jznd = new RegexInfo("<span class=\"mr50\">建造年代：(\\d*)(年|)</span>", "$1");
        //protected RegexInfo regex_jznd2 = new RegexInfo("建筑年代：<span class=\"hs\">([^<>]*)</span>", "$1");
        ///// <summary>
        ///// 信息(备注)
        ///// </summary>
        //protected RegexInfo regex_title = new RegexInfo("(<h1 class=\"f16 yahei normal c_000\">([^<>]*)</h1>|<h1 class=\"f24 yahei\">([^<>]*))", "$2$3");
        //protected RegexInfo regex_title2 = new RegexInfo("<div class=\"titlebg\"><span>([^<>]*)</span></div>", "$1");
        ///// <summary>
        ///// 电话
        ///// </summary>
        //protected RegexInfo regex_phone = new RegexInfo("<p class=\"pt10\">联系电话：<span class=\"c_red bold\">([^<>]*)</span></p>", "$1");
        //protected RegexInfo regex_phone2 = new RegexInfo("<span class=\"Tel_t f12br\">([^<>]*)</span>", "$1");
        ///// <summary>
        ///// URL
        ///// </summary>
        //protected RegexInfo regex_infUrl = new RegexInfo("<a target=\"_blank\" href=\"([^\"]*)\" class=\"c_default f14 bold\">[^<>]*</a>", "$1");
        ///// <summary>
        ///// 获取地址
        ///// </summary>
        //protected RegexInfo regex_address = new RegexInfo("<span class=\"mr50\">地址：([^<>]*)</span>", "$1");
        //protected RegexInfo regex_address2 = new RegexInfo("<li><span class=\"bh\">地址：</span>([^<>]*)</li>", "$1");
        ///// <summary>
        ///// 下一页正则
        ///// </summary>
        //protected RegexInfo regex_nextPage = new RegexInfo("<a class='next' href=\'([^']+)\'>下一页</a>", "$1");
        ///// <summary>
        ///// 信息的更新时间
        ///// </summary>
        //protected RegexInfo regex_updatetime = new RegexInfo("<span class=\"[^\"]*\">更新时间：([^<>]+)</span>", "$1");
        //protected RegexInfo regex_updatetime2 = new RegexInfo("更新时间：([^<>]+)</div>", "$1");


        ///// <summary>
        ///// 中介公司
        ///// </summary>
        //protected RegexInfo regex_comName = new RegexInfo("(所在公司|所属公司)：<a[^<>]*>([^<>]+)</a>", "$2");
        ///// <summary>
        ///// 门店地址
        ///// </summary>
        //protected RegexInfo regex_comArea = new RegexInfo("所在门店：([^<>]+)<", "$1");
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
        /// 获取地址
        /// </summary>
        protected RegexInfo regex_address = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_address", 网站名称);
        /// <summary>
        /// 下一页正则
        /// </summary>
        protected RegexInfo regex_nextPage = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_nextPage", 网站名称);
        /// <summary>
        /// 信息的更新时间
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
        #endregion
        #region 构造函数
        public NewDataSpider()
        {
            
            NewDataRumList.Add(new NewDataRum("北京", "http://bj.esf.sina.com.cn", "http://bj.esf.sina.com.cn/house/p0-m3/", 0, 0));//*
            NewDataRumList.Add(new NewDataRum("上海", "http://sh.esf.sina.com.cn", "http://sh.esf.sina.com.cn/house/p0-m3/", 0, 0));//*            
            NewDataRumList.Add(new NewDataRum("天津", "http://tj.esf.sina.com.cn", "http://tj.esf.sina.com.cn/house/p0-m3/", 0, 0));//*
            //regex_lpm.RegexInfoList.Add(regex_lpm2);
            //regex_lpm.RegexInfoList.Add(regex_lpm3);
            //regex_lpm.RegexInfoList.Add(regex_lpm4);
            //regex_lpm.RegexInfoList.Add(regex_lpm5);
            //regex_xzq.RegexInfoList.Add(regex_xzq2);
            //regex_xzq.RegexInfoList.Add(regex_xzq3);
            ////regex_xzq.RegexInfoList.Add(regex_xzq4);
            //regex_pq.RegexInfoList.Add(regex_pq2);
            //regex_pq.RegexInfoList.Add(regex_pq3);
            //regex_pq.RegexInfoList.Add(regex_pq4);
            //regex_mj.RegexInfoList.Add(regex_mj2);
            //regex_dj.RegexInfoList.Add(regex_dj2);
            //regex_zj.RegexInfoList.Add(regex_zj2);
            //regex_szlc.RegexInfoList.Add(regex_szlc2);
            //regex_zlc.RegexInfoList.Add(regex_zlc2);
            //regex_cx.RegexInfoList.Add(regex_cx2);
            //regex_zx.RegexInfoList.Add(regex_zx2);
            //regex_jznd.RegexInfoList.Add(regex_jznd2);
            //regex_title.RegexInfoList.Add(regex_title2);
            //regex_phone.RegexInfoList.Add(regex_phone2);
            //regex_address.RegexInfoList.Add(regex_address2);
            //regex_updatetime.RegexInfoList.Add(regex_updatetime2);
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
        /// 
        /// </summary>
        public void start()
        {
            //new 深圳().start();
            //new 广州().start();
            new 上海().start();
            new 北京().start();
            //new 贵阳().start();
            //new 哈尔滨().start();
            //new 海口().start();
            //new 合肥().start();
            //new 呼和浩特().start();
            //new 兰州().start();
            //new 南宁().start();
            //new 石家庄().start();
            //new 太原().start();
            //new 西宁().start();
            //new 银川().start();
            //new 长春().start();
            //new 郑州().start();
            //new 重庆().start();
            //new 昆明().start();
            new 天津().start();
            List<VIEW_网站爬取配置_城市表_网站表> list = SpiderWebConfigManager.获取新浪二手房下所有城市爬取配置();
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
        /// 
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="pageListIndexUrl"></param>
        /// <param name="rate"></param>
        /// <param name="pageCheckRate"></param>
        public void SpiderHouse(string hostUrl, string pageListIndexUrl,int rate, int pageCheckRate)
        {
            //RegexInfo 总条数正则 = new RegexInfo("<p class=\"fr mt3 pr10\">共找到<strong class=\"c_red\">([\\d]+)</strong>套", "$1");
            //总条数正则.RegexInfoList.Add(new RegexInfo("<div class=\"search_main_list_tit_num\">共找到<span>([\\d]+)</span>套", "$1"));
            //总条数正则.RegexInfoList.Add(new RegexInfo(">共找到<em class=\"c_red mlr5\">([\\d]+)</em>条房源<", "$1"));
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
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_updatetime, "regex_updatetime", "更新时间")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comName, "regex_comName", "公司")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_comArea, "regex_comArea", "门店")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infUrl, "regex_infUrl", "url")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_nextPage, "regex_nextPage", "下一页正则"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(总条数正则, "总条数正则", "总条数正则"));
            //string str = stest.ToString();
            #endregion

            int maxPageLength = 50;

            if (pageCheckRate > 0)
            {
                System.Threading.Thread.Sleep(pageCheckRate);
            }
            //获取爬取记录
            //新浪Log =新浪二手房LogManager.获取新浪二手房_Log(CityName);
            //if (新浪Log == null)
            //{
            //    新浪Log = 新浪二手房LogManager.初始化新浪二手房_Log(CityName);
            //}
            //发布单独爬取详细url的线程方法
            Url_workload = new Queue<string>();
            IsStop = false;
            Rate = rate;
            ThreadStart ts2 = new ThreadStart(this.ProcessQueue);
            Thread m_thread2 = new Thread(ts2);
            m_thread2.Start();
            //开始获取页面
            Dictionary<string, RegexInfo> 正则字典集合 = new Dictionary<string, RegexInfo>();
            正则字典集合.Add("*总条数", 总条数正则);
            log.Debug(string.Format("新浪二手房SpiderHouse()--获取根页面的总条数,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName));
            Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(pageListIndexUrl, NowPageEncoding, 正则字典集合, WebObj, CityId);
            int count = 根页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(根页面正则字典集合结果["*总条数"][0]);
            log.Debug(string.Format("新浪二手房SpiderHouse()--获取根页面的总条数为{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", count.ToString(), hostUrl, pageListIndexUrl, CityName));


            //*******************根页面下信息列表爬取***********************//
            string 根页面分页链接参数 = pageListIndexUrl.Replace("p0-m2", "p0-m2-n{0}");
            根页面分页链接参数 = 根页面分页链接参数.Replace("p0-m3", "p0-m3-n{0}");
            string 根页面下一页链接 = pageListIndexUrl;
            int 当前总页数 = (count - 1) / maxPageLength + 1;
            int 当前页码 = 1;
            //if (StringHelp.获取相差天数(新浪Log.开始爬取时间) < 1 && 新浪Log.当前列表页面页码 != null)
            //{
            //    当前页码 = Convert.ToInt32(新浪Log.当前列表页面页码);
            //    根页面下一页链接 = string.Format(根页面分页链接参数, 当前页码.ToString());
            //}
            //else
            //{
            //    新浪Log.开始爬取时间 = DateTime.Now;
            //    新浪二手房LogManager.设置Log(新浪Log);
            //}
            while (!string.IsNullOrEmpty(根页面下一页链接))
            {
                string nowPageList = 根页面下一页链接;
                if (!根页面下一页链接.ToLower().Contains("http://"))
                {
                    nowPageList = hostUrl + 根页面下一页链接;
                }
                log.Debug(string.Format("新浪二手房SpiderHouse()--获取根页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 根页面下一页链接);
                if (!string.IsNullOrEmpty(根页面下一页链接))
                {
                    //新浪Log.当前列表页面页码 = 当前页码;
                }
                //新浪二手房LogManager.设置Log(新浪Log);
                当前页码++;
                //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                if (string.IsNullOrEmpty(根页面下一页链接) && 当前页码 <= 当前总页数)
                {
                    根页面下一页链接 = string.Format(根页面分页链接参数, 当前页码.ToString());
                }
            }
            log.Debug(string.Format("新浪二手房SpiderHouse()--获取{0}页面下详细信息Url吸取完成,详细页面url内容正则析取中-,hostUrl:{1}, pageListUrl:{2}", CityName, hostUrl, pageListIndexUrl));
            IsStop = true;
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
                CaseManager.往案例表插入爬取数据(网站名称: 网站名称,城市名称:CityName, 网站ID: 网站ID,城市ID:CityId,
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
        /// 用于线程调用的先进先出的url(用于线程方法ProcessQueue)
        /// </summary>
        public Queue<string> Url_workload
        {
            get;
            set;
        }
        #endregion

        #region 处理方法
        /// <summary>
        /// 子线程处理详细页面URL
        /// </summary>
        public void ProcessQueue()
        {
            while (true)
            {
                if (Rate > 0)
                {
                    System.Threading.Thread.Sleep(Rate);
                }
                if (Url_workload != null && Url_workload.Count > 0)
                {
                    //log.Debug("哈哈哈哈哈");
                    //Thread.Sleep(50000000);
                    string urlResult = Url_workload.Dequeue().ToString();
                    GetHouseByUrl(urlResult); ;
                }
                else
                {
                    if (IsStop)
                    {
                        log.Debug(string.Format("新浪二手房ProcessQueue()--获取{0}页面下详细信息Url内容吸取完成", CityName));
                        break;
                    }
                }

            }
        }
        /// <summary>
        /// 根据详细页url获取信息
        /// </summary>
        /// <param name="url">详细页url</param>
        public void GetHouseByUrl(string url)
        {
            //url = "http://sz.esf.sina.com.cn/detail/7946016";
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
                dicRegexInfo.Add("regex_updatetime", regex_updatetime);
                dicRegexInfo.Add("regex_comName", regex_comName);
                dicRegexInfo.Add("regex_comArea", regex_comArea);
                Dictionary<string, List<string>> dicRegexInfo_List = new Dictionary<string, List<string>>();

                //根据规则获取数据
                dicRegexInfo_List = SpiderHelp.GetHtmlByRegex(url, NowPageEncoding, dicRegexInfo, WebObj, CityId);
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
                string value_phone = dicRegexInfo_List["regex_phone"].Count < 1 ? "" : dicRegexInfo_List["regex_phone"][0];
                string value_address = dicRegexInfo_List["regex_address"].Count < 1 ? "" : dicRegexInfo_List["regex_address"][0];
                string value_updatetime = dicRegexInfo_List["regex_updatetime"].Count < 1 ? "" : dicRegexInfo_List["regex_updatetime"][0];
                string value_comName = dicRegexInfo_List["regex_comName"].Count < 1 ? "" : dicRegexInfo_List["regex_comName"][0];
                string value_comArea = dicRegexInfo_List["regex_comArea"].Count < 1 ? "" : dicRegexInfo_List["regex_comArea"][0];
                //将数据添加到实体
                NewHouse newHouse = new NewHouse(value_lpm, value_updatetime, value_xzq, value_pq, "", "", "", value_mj, value_dj,
                                "", value_jg, "", value_zj, value_szlc, value_zlc, value_hx, value_cx, value_zx, value_jznd,
                                value_title, value_phone, url, "", 网站名称, value_address, "", "", "", "", "", "",value_comName,value_comArea);
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
        /// 转换户型中的字符
        /// </summary>
        /// <param name="houseType"></param>
        /// <returns></returns>
        public string GetConvertToHouseType(string houseType)
        {
            houseType = Regex.Replace(houseType, @"(\d*厨|\d*卫)", "", RegexOptions.IgnoreCase);
            houseType = houseType.Replace("室", "房");
            houseType = StringHelp.NumberConvertToChinese(houseType);
            return houseType;
        }
        /// <summary>
        /// 根据列表页url获取详细信息url
        /// </summary>
        /// <param name="hotUrl">列表页域名</param>
        /// <param name="pageListUrl">列表页url</param>
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
                dicRegexItem.Add("*regex_infUrl", regex_infUrl);
                dicRegexItem.Add("*regex_nextPage", regex_nextPage);
                //发送请求获取根据正则获取网页html信息
                Dictionary<string, List<string>> dicRegexItem_List = SpiderHelp.GetHtmlByRegex(url_sz, NowPageEncoding, dicRegexItem, WebObj, CityId);
                List<string> list = dicRegexItem_List["*regex_infUrl"];
                下一页链接 = dicRegexItem_List["*regex_nextPage"].Count < 1 ? "" : dicRegexItem_List["*regex_nextPage"][0];
                foreach (string strUrl in list)
                {
                    //if (rate > 0)
                    //{
                    //    System.Threading.Thread.Sleep(rate);
                    //}
                    string nowUrl = strUrl;
                    //如果当前url不带域名
                    if (!strUrl.ToLower().Contains("http://"))
                    {
                        nowUrl = hostUrl + strUrl;
                    }
                beginPubUrl:
                    if (Url_workload.Count > 2000)
                    {
                        Thread.Sleep(5000);
                        goto beginPubUrl;
                    }
                    Url_workload.Enqueue(nowUrl);
                    //GetHouseByUrl(nowUrl);
                    //新浪Log.当前详细页面Url = nowUrl;
                    //新浪Log.更新时间 = DateTime.Now;
                    //新浪二手房LogManager.设置Log(新浪Log);
                    //GetHouseByUrl(nowUrl, cityName);//nowUrl"http://yn.esf.sina.com.cn/detail/493517" "http://sh.esf.sina.com.cn/detail/40594597/" "http://sh.esf.sina.com.cn/detail/40661944/"
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
