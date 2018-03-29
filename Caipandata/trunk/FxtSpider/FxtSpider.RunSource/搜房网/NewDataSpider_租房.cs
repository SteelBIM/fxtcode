using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Interface;
using log4net;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll.SpiderCommon;
using System.Text.RegularExpressions;
using FxtSpider.Common;
using System.Threading;

namespace FxtSpider.RunSource.搜房网
{
     public class NewDataSpider_租房: INewDataRum
    {        
        #region(初始化变量)
        protected static readonly ILog log = LogManager.GetLogger(typeof(NewDataSpider));
        protected static string 网站名称 = WebsiteManager.搜房网;
        protected static int 网站ID = WebsiteManager.搜房网_ID;
        protected List<NewDataRum> NewDataRumList = new List<NewDataRum>();
        protected static 网站表 WebObj = WebsiteManager.GetWebById(WebsiteManager.搜房网_ID);
        /// <summary>
        /// 开始爬取时间
        /// </summary>
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
        #endregion
        #region(出售房源_页面需要爬取的各字段正则)
        ///// <summary>
        ///// 楼盘名
        ///// </summary>
        //protected RegexInfo regex_lpm = new RegexInfo("<span class=\"gray6 \">楼盘名称：</span>(([^<>\\(]*)\\(|<a href=\"[^\"]*\" target=\"_blank\" title=\"[^\"]+\" id=\"esf[^\"]+xq_11\">([^<>]*)</a>)", "$2$3");
        //protected RegexInfo regex_lpm2 = new RegexInfo("<span class=\"gray9\">楼盘名称：</span><strong[^<>]*>([^<>]*)</strong>", "$1");       
        ///// <summary>
        ///// 行政区
        ///// </summary>
        //protected RegexInfo regex_xzq = new RegexInfo("\\( <a href=\"[^\"]*\" target=\"_blank\" id=\"esf[^\"]+xq_12\">([^<>]*)</a><a href=\"[^\"]*\" target=\"_blank\" id=\"esf[^\"]+xq_13\">[^<>]*</a>\\)[^\\[]*\\[", "$1");
        //protected RegexInfo regex_xzq2 = new RegexInfo("<a href=\"[^\"]*\" target=\"_blank\" id=\"esfjxxq_12\">([^<>]*)</a>", "$1");
        ///// <summary>
        ///// 片区
        ///// </summary>
        //protected RegexInfo regex_pq = new RegexInfo("\\( <a href=\"[^\"]*\" target=\"_blank\" id=\"esf[^\"]+xq_12\">[^<>]*</a><a href=\"[^\"]*\" target=\"_blank\" id=\"esf[^\"]+xq_13\">([^<>]*)</a>\\)[^\\[]*\\[", "$1");
        //protected RegexInfo regex_pq2 = new RegexInfo("<a [^<>]*target=\"_blank\" id=\"esfjxxq_13\">([^<>]*)</a>", "$1");
        ///// <summary>
        ///// 用途
        ///// </summary>
        //protected RegexInfo regex_yt = new RegexInfo("<span class=\"gray6\">住宅类别：</span>([^<>]*)</dd>", "$1");
        ///// <summary>
        ///// 面积
        ///// </summary>
        //protected RegexInfo regex_mj = new RegexInfo("<dd class=\"gray6\">建筑面积：<span class=\"black \">([\\.\\d]*)㎡</span></dd>", "$1");
        //protected RegexInfo regex_mj2 = new RegexInfo("建筑面积：<span class=\"red20b\">([\\.\\d]*)</span>平方米</dd>", "$1");
        ///// <summary>
        ///// 单价
        ///// </summary>
        //protected RegexInfo regex_dj = new RegexInfo("万</span>\\(([\\.\\d]+)元/㎡\\)", "$1");
        //protected RegexInfo regex_dj2 = new RegexInfo("<dd>单　　价：([\\.\\d]+)元/平方米</dd>", "$1");
        ///// <summary>
        ///// 结构
        ///// </summary>
        //protected RegexInfo regex_jg = new RegexInfo("<span class=\"gray6[^\"]*\">(结<span class=\"padl27\"></span>构：|厅 结 构：)</span>([^<>]*)</dd>", "$2");
        ///// <summary>
        ///// 总价
        ///// </summary>
        //protected RegexInfo regex_zj = new RegexInfo("总<span class=\"padl27\"></span>价：<span class=\"red20b\">([\\.\\d]*)</span><span([^<>]*)class=\"black\">万</span>\\(\\d*元/㎡\\)", "$1");
        //protected RegexInfo regex_zj2 = new RegexInfo("总　　价：<span class=\"red20b\">([\\.\\d]*)</span>万元</dd>", "$1");
        ///// <summary>
        ///// 所在楼层
        ///// </summary>
        //protected RegexInfo regex_szlc = new RegexInfo("<span class=\"gray6\">楼<span class=\"padl27\"></span>层：</span>第(\\d*)层\\(共\\d*层\\)</dd>", "$1");
        //protected RegexInfo regex_szlc2 = new RegexInfo("<span class=\"gray9\">楼　　层：</span>第(\\d*)层", "$1");   
        ///// <summary>
        ///// 总楼层
        ///// </summary>
        //protected RegexInfo regex_zlc = new RegexInfo("<span class=\"gray6\">(楼<span class=\"padl27\"></span>层：|地上层数：)</span>(第\\d*层\\(共|[^\\d]*)(\\d*)层(\\)|[^<>]*)</dd>", "$3");
        //protected RegexInfo regex_zlc2 = new RegexInfo("共(\\d*)层</dd>", "$1");
        ///// <summary>
        ///// 户型
        ///// </summary>
        //protected RegexInfo regex_hx = new RegexInfo("<dd class=\"gray6\"><span class=\"gray6\">户<span class=\"padl27\"></span>型：</span>([^<>]*)</dd>", "$1");
        //protected RegexInfo regex_hx2 = new RegexInfo("户　　型：</span>([^<>]*)</dt>", "$1");
        ///// <summary>
        ///// 朝向
        ///// </summary>
        //protected RegexInfo regex_cx = new RegexInfo("<span class=\"gray6\">(朝<span class=\"padl27\"></span>向：|进门朝向：)</span>([^<>]*)</dd>", "$2");
        //protected RegexInfo regex_cx2 = new RegexInfo("朝　　向：</span>([^<>]*)</dd>", "$1");
        ///// <summary>
        ///// 装修
        ///// </summary>
        //protected RegexInfo regex_zx = new RegexInfo("<span class=\"gray6\">(装<span class=\"padl27\"></span>修：|装修程度：)</span>([^<>]*)</dd>", "$2");
        //protected RegexInfo regex_zx2 = new RegexInfo("装　　修：</span>([^<>]*)</dd>", "$1");
        ///// <summary>
        ///// 建筑年代
        ///// </summary>
        //protected RegexInfo regex_jznd = new RegexInfo("<span class=\"gray6\">(年<span class=\"padl27\"></span>代：|建筑年代：)</span>(\\d*)年</dd>", "$2");
        ///// <summary>
        ///// 信息(备注)
        ///// </summary>
        //protected RegexInfo regex_title = new RegexInfo("<input type=\"hidden\" name=\"talkTitle\" id=\"talkTitle\" value=\"([^\"]*)\" />", "$1");
        //protected RegexInfo regex_title2 = new RegexInfo("<div class=\"title flc\"><h1><span>([^\"]*)</span></h1>", "$1");
        ///// <summary>
        ///// 电话
        ///// </summary>
        //protected RegexInfo regex_phone = new RegexInfo("<span id=\"phone\">([^<>]*)</span>", "$1");
        //protected RegexInfo regex_phone2 = new RegexInfo("联系电话:<span class=\"font20\">([^<>]*)</span>", "$1");
        ///// <summary>
        ///// URL
        ///// </summary>
        //protected RegexInfo regex_infUrl = new RegexInfo("<dd[^<>]*><p class=\"title\"><a[^<>]*href=\"([^\"]*)\"[^<>]*>[^<>]*</a>", "$1");//(<img src=\"[^\"]*\" align=\"absmiddle\"/>|[^<>]*)
        //protected RegexInfo regex_infUrl2 = new RegexInfo("<p class=\"housetitle\"><a href=\"([^\"]*)\" target=\"_blank\" title=\"\">[^<>]*</a>(?:(?!</p>).)*</p>", "$1");//(<img src=\"[^\"]*\" align=\"absmiddle\"/>|[^<>]*)
        ///// <summary>
        ///// 下一页正则
        ///// </summary>
        //protected RegexInfo regex_nextPage = new RegexInfo("<a id=\"PageControl1_hlk_next\" href=\"([^\"]*)\">下一页</a>", "$1");
        ///// <summary>
        ///// 建筑形式
        ///// </summary>
        //protected RegexInfo regex_jzxs = new RegexInfo("<span class=\"gray6\">(建筑形式：)</span>([^<>]*)</dd>", "$2");
        ///// <summary>
        ///// 产权性质
        ///// </summary>
        //protected RegexInfo regex_cqxz = new RegexInfo("<span class=\"gray6 \">产权性质：</span>([^<>]*)</dd>", "$1");
        ///// <summary>
        ///// 配套设施
        ///// </summary>
        //protected RegexInfo regex_ptss = new RegexInfo("<dt><span class=\"gray6[^\"]*\">配套设施：</span>(<span class=\"sheshi\">([^<>]*)</span>|([^<>]*))</dt>", "$2$3");
        ///// <summary>
        ///// 地址
        ///// </summary>
        //protected RegexInfo regex_address = new RegexInfo("<p><span class=\"gray6\">地<span class=\"padl27\"></span>址：</span>([^<>]*)</p>", "$1");
        //protected RegexInfo regex_address2 = new RegexInfo("地　　址：</span>([^<>]*)</dt>", "$1");
        ///// <summary>
        ///// 信息的发布时间
        ///// </summary>
        //protected RegexInfo regex_datetime = new RegexInfo("发布时间：([^\\(]*)\\(", "$1");
        ///// <summary>
        ///// 信息的最后更新间隔(用于计算案例时间)
        ///// </summary>
        //protected RegexInfo regex_updatetime = new RegexInfo("<span id=\"Time\">([^<>]*)前更新</span>", "$1");
        ///// <summary>
        ///// 花园面积
        ///// </summary>
        //protected RegexInfo regex_hymj = new RegexInfo("花园面积：</span>([\\d\\.]*)平米", "$1");
        ///// <summary>
        ///// 厅结构
        ///// </summary>
        //protected RegexInfo regex_tjg = new RegexInfo("厅 结 构：</span>([^<>]*)</dd>", "$1");
        ///// <summary>
        ///// 车位数量
        ///// </summary>
        //protected RegexInfo regex_cwsl = new RegexInfo("车位数量：</span>(\\d*)个", "$1");
        ///// <summary>
        ///// 地下室面积
        ///// </summary>
        //protected RegexInfo regex_dxsmj = new RegexInfo("地下室面积：</span>([\\d\\.]*)平米", "$1");
        ///// <summary>
        ///// 中介公司
        ///// </summary>
        //protected RegexInfo regex_comName = new RegexInfo("jjrcompanyname = '([^']+)'", "$1");
        ///// <summary>
        ///// 门店地址
        ///// </summary>
        //protected RegexInfo regex_comArea = new RegexInfo("comArea = \"([^\"]+)\"", "$1");

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
        /// 下一页正则
        /// </summary>
        protected RegexInfo regex_nextPage = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_nextPage", 网站名称);
        /// <summary>
        /// 建筑形式
        /// </summary>
        protected RegexInfo regex_jzxs = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_jzxs", 网站名称);
        /// <summary>
        /// 产权性质
        /// </summary>
        protected RegexInfo regex_cqxz = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_cqxz", 网站名称);
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
        /// 信息的最后更新间隔(用于计算案例时间)
        /// </summary>
        protected RegexInfo regex_updatetime = SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_updatetime", 网站名称);
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
        public NewDataSpider_租房()
        {
            NewDataRumList.Add(new NewDataRum("海口", "http://esf.hn.soufun.com", "http://esf.hn.soufun.com/house-a012907/h316-j3100-w32/", 4000, 2000));
     
            //regex_lpm.RegexInfoList.Add(regex_lpm2);
            //regex_xzq.RegexInfoList.Add(regex_xzq2);
            //regex_pq.RegexInfoList.Add(regex_pq2);
            //regex_mj.RegexInfoList.Add(regex_mj2);
            //regex_dj.RegexInfoList.Add(regex_dj2);
            //regex_zj.RegexInfoList.Add(regex_zj2);
            //regex_szlc.RegexInfoList.Add(regex_szlc2);
            //regex_zlc.RegexInfoList.Add(regex_zlc2);
            //regex_hx.RegexInfoList.Add(regex_hx2);
            //regex_cx.RegexInfoList.Add(regex_cx2);
            //regex_zx.RegexInfoList.Add(regex_zx2);
            //regex_title.RegexInfoList.Add(regex_title2);
            //regex_phone.RegexInfoList.Add(regex_phone2);
            //regex_address.RegexInfoList.Add(regex_address2);
            //regex_infUrl.RegexInfoList.Add(regex_infUrl2);
           
        }
        #endregion
        #region INewDataRum 成员
        /// <summary>
        /// 城市名称
        /// </summary>
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
        /// 开始
        /// </summary>
        public void start()
        {

            //new 深圳().start();
            //new 广州().start();
            //new 上海().start();
            //new 北京().start();
            //new 贵阳().start();
            //new 哈尔滨().start();
            new 海口().start();
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

            List<VIEW_网站爬取配置_城市表_网站表> list = SpiderWebConfigManager.获取搜房网下所有城市爬取配置();
            foreach (VIEW_网站爬取配置_城市表_网站表 _view in list)
            {
                NewDataRum exists = NewDataRumList.Find(
                 delegate(NewDataRum _newDataRum) { return _newDataRum.CityName.Equals(_view.城市名称); });
                if (exists == null)
                {
                     new 其他城市(_view.城市名称).start(_view.域名, _view.列表页链接, _view.详细页面爬取频率, _view.列表页面爬取频率,_view.规则编号,_view.主要用途,_view.主要案例类型);
                }
            }

            ////new 安阳().start();
            ////new 鞍山().start();
            ////new 包头().start();
            ////new 北海().start();
            ////new 滨州().start();
            ////new 常州().start();
            ////new 成都().start();
            ////new 大连().start();
            ////new 东莞().start();
            ////new 鄂尔多斯().start();
            ////new 佛山().start();
            ////new 福州().start();
            ////new 赣州().start();
            ////new 杭州().start();
            ////new 衡水().start();
            ////new 湖州().start();
            ////new 黄石().start();
            ////new 惠州().start();
            ////new 济南().start();
            ////new 济宁().start();
            ////new 嘉兴().start();
            ////new 江门().start();
            ////new 金华().start();
            ////new 九江().start();
            ////new 乐山().start();
            ////new 丽江().start();
            ////new 聊城().start();
            ////new 临沂().start();
            ////new 柳州().start();
            ////new 泸州().start();
            ////new 洛阳().start();
            ////new 马鞍山().start();
            ////new 绵阳().start();
            ////new 南昌().start();
            ////new 南京().start();
            ////new 南通().start();
            ////new 宁波().start();
            ////new 青岛().start();
            ////new 清远().start();
            ////new 衢州().start();
            ////new 曲靖().start();
            ////new 泉州().start();
            ////new 厦门().start();
            ////new 上饶().start();
            ////new 绍兴().start();
            ////new 沈阳().start();
            ////new 苏州().start();
            ////new 台州().start();
            ////new 泰安().start();
            ////new 泰州().start();
            ////new 天津().start();
            ////new 威海().start();
            ////new 潍坊().start();
            ////new 温州().start();
            ////new 乌鲁木齐().start();
            ////new 无锡().start();
            ////new 芜湖().start();
            ////new 武汉().start();
            ////new 西安().start();
            ////new 咸阳().start();
            ////new 襄阳().start();
            ////new 徐州().start();
            ////new 烟台().start();
            ////new 盐城().start();
            ////new 扬州().start();
            ////new 宜昌().start();
            ////new 湛江().start();
            ////new 漳州().start();
            ////new 肇庆().start();
            ////new 镇江().start();
            ////new 中山().start();
            ////new 珠海().start();
            ////new 淄博().start();
            ////new 榆林().start();
            ////new 营口().start();
            ////new 日照().start();
            ////new 东营().start();
            ////new 衡阳().start();
            ////new 秦皇岛().start();
            ////new 唐山().start();
            ////new 邯郸().start();

        }
        /// <summary>
        /// 根据列表页url获取详细信息url
        /// </summary>
        /// <param name="hotUrl">列表页域名</param>
        /// <param name="pageListIndexUrl">列表页首页url</param>
        /// <param name="rate">爬取频率(毫秒)</param>
        /// <param name="pageCheckRate">页面监测频率(毫秒)</param>
        /// <param name="下一页链接">输出下一页的链接</param>
        public virtual void SpiderHouse(string hostUrl, string pageListIndexUrl, int rate, int pageCheckRate)
        {
            //RegexInfo 总条数正则 = new RegexInfo("<[^<>]*class=\"[^\"]*floatl[^\"]*\"[^<>]*>共找到<[^<>]*>([\\d]*)</[^<>]+>条", "$1");
            //RegexInfo 行政区文本正则 = new RegexInfo("<[^<>]+id=\"list_38\"[^<>]*><[^<>]+>区域：</[^<>]+><[^<>]+class=\"[^\"]*qxName[^\"]*\"[^<>]*>((?:(?!div).)*)<", "$1");
            //行政区文本正则.RegexInfoList.Add(new RegexInfo("<dl class=\"quxian\" id=\"dl_quxian\"><dd>区<span class='pl5'>域：</span></dd>((?:(?!dl).)*)</dl>", "$1"));
            //RegexInfo 行政区链接正则 = new RegexInfo("<a  href=([^<>]+) >[^<>]*</a>", "$1");
            //行政区链接正则.RegexInfoList.Add(new RegexInfo("<dd ><a  href=([^<>]+) >[^<>]*</a></dd>", "$1"));
            //RegexInfo 片区文本正则 = new RegexInfo("<[^<>]+id=\"shangQuancontain\"[^<>]*>((?:(?!p>).)*)</[^<>]+>", "$1");
            //片区文本正则.RegexInfoList.Add(new RegexInfo("<div class=\"mid\" id=\"tagContent0\">((?:(?!div).)*)</div>", "$1"));
            //RegexInfo 片区链接正则 = new RegexInfo("<a href=\"([^\"]+)\" >[^<>]+</a>", "$1");
            //片区链接正则.RegexInfoList.Add(new RegexInfo("<a href=\"([^\"]+)\" >[^<>]+</a>", "$1"));
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
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_nextPage, "regex_nextPage", "下一页正则"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_jzxs, "regex_jzxs", "建筑形式"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_cqxz, "regex_cqxz", "产权性质"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_ptss, "regex_ptss", "配套设施"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_hymj, "regex_hymj", "花园面积"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_tjg, "regex_tjg", "厅结构"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_cwsl, "regex_cwsl", "车位数量"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_dxsmj, "regex_dxsmj", "地下室面积"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(总条数正则, "总条数正则", "总条数正则"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区文本正则, "行政区文本正则", "行政区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(行政区链接正则, "行政区链接正则", "行政区链接正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区文本正则, "片区文本正则", "片区文本正则")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(片区链接正则, "片区链接正则", "片区链接正则")); ;
            //string str = stest.ToString();
            #endregion
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
                根页面正则字典集合.Add("*行政区文本", 行政区文本正则);
                Dictionary<string, RegexInfo> 行政区链接字典集合 = new Dictionary<string, RegexInfo>();
                行政区链接字典集合.Add("*行政区链接", 行政区链接正则);
                Dictionary<string, RegexInfo> 行政区页面正则字典集合 = new Dictionary<string, RegexInfo>();
                行政区页面正则字典集合.Add("*总条数", 总条数正则);
                行政区页面正则字典集合.Add("*片区文本", 片区文本正则);
                Dictionary<string, RegexInfo> 片区链接正则字典 = new Dictionary<string, RegexInfo>();
                片区链接正则字典.Add("*片区链接", 片区链接正则);
                Dictionary<string, RegexInfo> 片区页面正则字典集合 = new Dictionary<string, RegexInfo>();
                片区页面正则字典集合.Add("*总条数", 总条数正则);

                //Dictionary<string, RegexInfo> 正则字典集合 = new Dictionary<string, RegexInfo>();
                //正则字典集合.Add("总条数", 总条数正则);
                //正则字典集合.Add("行政区文本", 行政区文本正则);
                //正则字典集合.Add("行政区链接", 行政区链接正则);
                //正则字典集合.Add("片区文本", 片区文本正则);
                //正则字典集合.Add("片区链接", 片区链接正则);
                log.Debug(string.Format("搜房SpiderHouse()--获取根页面的总条数,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName));
                Dictionary<string, List<string>> 根页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(pageListIndexUrl, "GBK", 根页面正则字典集合, WebObj, CityId);
                int count = 根页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(根页面正则字典集合结果["*总条数"][0]);
                log.Debug(string.Format("搜房SpiderHouse()--获取根页面的总条数为{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", count.ToString(), hostUrl, pageListIndexUrl, CityName));
                //当前根页面总个数大于maxCount
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
                        string nowUrl = _url;
                        if (!_url.ToLower().Contains("http://"))
                        {
                            nowUrl = hostUrl + _url;
                        }
                        //根据开始爬取的日期和当前日期转换url
                        nowUrl = GetSpiderUrlByDate(nowUrl, nowDate);
                        //个数获取
                        log.Debug(string.Format("搜房SpiderHouse()--获取当前行政区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl, hostUrl, pageListIndexUrl, CityName));
                        Dictionary<string, List<string>> 行政区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl, "GBK", 行政区页面正则字典集合, WebObj, CityId);
                        int _count = 行政区页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(行政区页面正则字典集合结果["*总条数"][0]);
                        log.Debug(string.Format("搜房SpiderHouse()--获取当前行政区页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count.ToString(), nowUrl, hostUrl, pageListIndexUrl, CityName));

                        //当前行政区页面总个数大于maxCount
                        string 片区文本 = 行政区页面正则字典集合结果["*片区文本"].Count < 1 ? "" : 行政区页面正则字典集合结果["*片区文本"][0];
                        Dictionary<string, List<string>> 片区链接结果 = SpiderHelp.GetStrByRegex(片区文本, 片区链接正则字典);
                        List<string> 片区链接List = 片区链接结果["*片区链接"];
                        if (_count > maxCount && 片区链接List.Count>0)
                        {
                            foreach (string _url2 in 片区链接List)
                            {
                                string nowUrl2 = _url2;
                                if (!_url2.ToLower().Contains("http://"))
                                {
                                    nowUrl2 = hostUrl + _url2;
                                }
                                //根据开始爬取的日期和当前日期转换url
                                nowUrl2 = GetSpiderUrlByDate(nowUrl2, nowDate);
                                //获取个数
                                log.Debug(string.Format("搜房SpiderHouse()--获取当前片区页面的总条数,当前链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl2, hostUrl, pageListIndexUrl, CityName));
                                Dictionary<string, List<string>> 片区页面正则字典集合结果 = SpiderHelp.GetHtmlByRegex(nowUrl2, "GBK", 片区页面正则字典集合, WebObj, CityId);
                                int _count2 = 片区页面正则字典集合结果["*总条数"].Count < 1 ? 0 : Convert.ToInt32(片区页面正则字典集合结果["*总条数"][0]);
                                log.Debug(string.Format("搜房SpiderHouse()--获取当前片区页面的总条数为{0},当前链接{1},hostUrl:{2}, pageListUrl:{3}, cityName{4}", _count2.ToString(), nowUrl2, hostUrl, pageListIndexUrl, CityName));
                                //*******************片区页面下信息列表爬取***********************//
                                string 片区页面分页链接参数 = nowUrl2.Replace("h316-j3100-w32", "h316-i3{0}-j3100-w32");
                                片区页面分页链接参数 = nowUrl2.Replace("h316-j3100-w31", "h316-i3{0}-j3100-w31");
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
                                    log.Debug(string.Format("搜房SpiderHouse()--获取片区页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                                    SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 片区页面下一页链接);
                                    当前页码++;
                                    //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                                    if (string.IsNullOrEmpty(片区页面下一页链接) && 当前页码 <= 当前总页数)
                                    {
                                        片区页面下一页链接 = string.Format(片区页面分页链接参数, 当前页码.ToString());
                                    }
                                }
                                log.Debug(string.Format("搜房SpiderHouse()--获取片区页面下信息吸取完成,{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl2, hostUrl, pageListIndexUrl, CityName));
                            }
                        }
                        else
                        {
                            //*******************行政区页面下信息列表爬取***********************//
                            string 行政区页面分页链接参数 = nowUrl.Replace("h316-j3100-w32", "h316-i3{0}-j3100-w32");
                            行政区页面分页链接参数 = nowUrl.Replace("h316-j3100-w31", "h316-i3{0}-j3100-w31");
                            string 行政区页面下一页链接 = nowUrl;
                            int 当前总页数 = (_count - 1) / maxPageLength + 1;
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
                                log.Debug(string.Format("搜房SpiderHouse()--获取行政区页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                                SpiderHouseByPageListUrl(hostUrl, nowPageList,  rate, pageCheckRate, out 行政区页面下一页链接);
                                当前页码++;
                                //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                                if (string.IsNullOrEmpty(行政区页面下一页链接) && 当前页码 <= 当前总页数)
                                {
                                    行政区页面下一页链接 = string.Format(行政区页面分页链接参数, 当前页码.ToString());
                                }
                            }
                            log.Debug(string.Format("搜房SpiderHouse()--获取行政区页面下信息吸取完成,{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowUrl, hostUrl, pageListIndexUrl, CityName));
                        }
                    }

                }
                else
                {

                    //*******************根页面下信息列表爬取***********************//
                    string 根页面分页链接参数 = pageListIndexUrl.Replace("h316-j3100-w32", "h316-i3{0}-j3100-w32");
                    根页面分页链接参数 = pageListIndexUrl.Replace("h316-j3100-w31", "h316-i3{0}-j3100-w31");
                    string 根页面下一页链接 = pageListIndexUrl;
                    int 当前总页数 = (count - 1) / maxPageLength + 1;
                    int 当前页码 = 1;
                    while (!string.IsNullOrEmpty(根页面下一页链接))
                    {
                        string nowPageList = 根页面下一页链接;
                        if (!根页面下一页链接.ToLower().Contains("http://"))
                        {
                            nowPageList = hostUrl + 根页面下一页链接;
                        }
                        //根据开始爬取的日期和当前日期转换url
                        nowPageList = GetSpiderUrlByDate(nowPageList, nowDate);
                        log.Debug(string.Format("搜房SpiderHouse()--获取根页面下一页链接{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", nowPageList, hostUrl, pageListIndexUrl, CityName));
                        SpiderHouseByPageListUrl(hostUrl, nowPageList, rate, pageCheckRate, out 根页面下一页链接);
                        当前页码++;
                        //如果当前页码还不到最后一页&&但返回的下一页链接为null(用于封ip或者网络异常时)
                        if (string.IsNullOrEmpty(根页面下一页链接) && 当前页码 <= 当前总页数)
                        {
                            根页面下一页链接 = string.Format(根页面分页链接参数, 当前页码.ToString());
                        }
                    }
                    log.Debug(string.Format("搜房SpiderHouse()--获取根页面下信息吸取完成,{0},hostUrl:{1}, pageListUrl:{2}, cityName{3}", 根页面下一页链接, hostUrl, pageListIndexUrl, CityName));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("搜房SpiderHouse()异常,hostUrl:{0}, pageListIndexUrl:{1}, cityName{2}", hostUrl, pageListIndexUrl, CityName), ex);
            }
            log.Debug(string.Format("搜房SpiderHouse()--获取{0}页面下详细信息Url吸取完成,详细页面url内容正则析取中-,hostUrl:{1}, pageListUrl:{2}", CityName, hostUrl, pageListIndexUrl));
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
                    string urlResult = Url_workload.Dequeue().ToString();
                    GetHouseByUrl(urlResult); ;
                }
                else
                {
                    if (IsStop)
                    {
                        log.Debug(string.Format("搜房ProcessQueue()--获取{0}页面下详细信息Url内容吸取完成", CityName));
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
                dicRegexInfo.Add("regex_cqxz", regex_cqxz);
                dicRegexInfo.Add("regex_ptss", regex_ptss);
                dicRegexInfo.Add("regex_title", regex_title);
                dicRegexInfo.Add("regex_phone", regex_phone);
                dicRegexInfo.Add("regex_address", regex_address);
                dicRegexInfo.Add("regex_datetime", regex_datetime);
                dicRegexInfo.Add("regex_updatetime", regex_updatetime);
                dicRegexInfo.Add("regex_hymj", regex_hymj);
                dicRegexInfo.Add("regex_tjg", regex_tjg);
                dicRegexInfo.Add("regex_cwsl", regex_cwsl);
                dicRegexInfo.Add("regex_dxsmj", regex_dxsmj);
                dicRegexInfo.Add("regex_comName", regex_comName);
                dicRegexInfo.Add("regex_comArea", regex_comArea);
                Dictionary<string, List<string>> dicRegexInfo_List = new Dictionary<string, List<string>>();

                //根据规则获取数据
                dicRegexInfo_List = SpiderHelp.GetHtmlByRegex(url, "GBK", dicRegexInfo, WebObj, CityId);
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
                string value_cqxz = dicRegexInfo_List["regex_cqxz"].Count < 1 ? "" : dicRegexInfo_List["regex_cqxz"][0];
                string value_ptss = dicRegexInfo_List["regex_ptss"].Count < 1 ? "" : dicRegexInfo_List["regex_ptss"][0];
                string value_phone = dicRegexInfo_List["regex_phone"].Count < 1 ? "" : dicRegexInfo_List["regex_phone"][0];
                string value_address = dicRegexInfo_List["regex_address"].Count < 1 ? "" : dicRegexInfo_List["regex_address"][0];
                string value_datetime = dicRegexInfo_List["regex_datetime"].Count < 1 ? "" : dicRegexInfo_List["regex_datetime"][0];
                string value_updatetime = dicRegexInfo_List["regex_updatetime"].Count < 1 ? "" : dicRegexInfo_List["regex_updatetime"][0];
                string value_hymj = dicRegexInfo_List["regex_hymj"].Count < 1 ? "" : dicRegexInfo_List["regex_hymj"][0];
                string value_tjg = dicRegexInfo_List["regex_tjg"].Count < 1 ? "" : dicRegexInfo_List["regex_tjg"][0];
                string value_cwsl = dicRegexInfo_List["regex_cwsl"].Count < 1 ? "" : dicRegexInfo_List["regex_cwsl"][0];
                string value_dxsmj = dicRegexInfo_List["regex_dxsmj"].Count < 1 ? "" : dicRegexInfo_List["regex_dxsmj"][0];
                string value_comName = dicRegexInfo_List["regex_comName"].Count < 1 ? "" : dicRegexInfo_List["regex_comName"][0];
                string value_comArea = dicRegexInfo_List["regex_comArea"].Count < 1 ? "" : dicRegexInfo_List["regex_comArea"][0];
                value_hx = GetConvertToHouseType(value_hx);
                if (dicRegexInfo_List.ContainsKey("NotData") && dicRegexInfo_List["NotData"][0] == "1")
                {
                    log.Debug(string.Format("因此房源已售出,找不到数据url:{0}--cityname:{1}--value_title:{2}--value_lpm{3}", url, CityName, value_title, value_lpm));
                    goto saveend;
                }
                if (value_yt.Contains("商住楼"))
                {
                    value_yt = "商住";
                }
                else
                {
                    value_yt = "";
                }
                //将数据添加到字典 
                NewHouse newHouse = new NewHouse(value_lpm, GetCaseDate(value_datetime, value_updatetime), value_xzq, value_pq, "", "", value_yt, value_mj, value_dj,
                                "", value_jg, "", value_zj, value_szlc, value_zlc, value_hx, value_cx, value_zx, value_jznd,
                                value_title, value_phone, url, "", 网站名称, value_address, value_jzxs, value_hymj, value_tjg, value_cwsl, value_ptss, value_dxsmj, value_comName, value_comArea);
                //保存数据
                SaveNowData(newHouse);;
                saveend:
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
            houseType = Regex.Replace(houseType, @"\d*厨\d*卫", "", RegexOptions.IgnoreCase);
            houseType = houseType.Replace("室", "房");
            houseType = StringHelp.NumberConvertToChinese(houseType);
            return houseType;
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
            if(string.IsNullOrEmpty(upTime))
            {
                return pubDate;
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
        /// 根据列表页url获取详细信息url
        /// </summary>
        /// <param name="hotUrl">列表页域名</param>
        /// <param name="pageListUrl">列表页url</param>
        /// <param name="rate">爬取频率(毫秒)</param>
        /// <param name="pageCheckRate">页面监测频率(毫秒)</param>
        /// <param name="下一页链接">输出下一页的链接</param>
        public void SpiderHouseByPageListUrl(string hostUrl, string pageListUrl,  int rate, int pageCheckRate, out string 下一页链接)
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
                Dictionary<string, List<string>> dicRegexItem_List = SpiderHelp.GetHtmlByRegex(url_sz, "GBK", dicRegexItem, WebObj, CityId);
                List<string> list = dicRegexItem_List["*regex_infUrl"];
                下一页链接 = dicRegexItem_List["*regex_nextPage"].Count < 1 ? "" : dicRegexItem_List["*regex_nextPage"][0];
                foreach (string strUrl in list)
                {
                    //if (rate > 0)
                    //{
                    //    System.Threading.Thread.Sleep(rate);
                    //}
                    string nowUrl = strUrl;// "http://esf.sz.soufun.com/chushou/3_33449384.htm" strUrl;
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
                    //nowUrl = "http://esf.soufun.com/chushou/1_105441578_-1.htm";
                    Url_workload.Enqueue(nowUrl);
                    //GetHouseByUrl(nowUrl);
                    //GetHouseByUrl(nowUrl, cityName);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("SpiderHouseByPageListUrl()异常,hostUrl:{0}, pageListUrl:{1}, cityName{2}", hostUrl, pageListUrl, CityName), ex);
            }
        }
        /// <summary>
        /// 根据当前日期和开始爬取的日期转换url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="spiderDate"></param>
        /// <returns></returns>
        public string GetSpiderUrlByDate(string url, DateTime spiderDate)
        {
            int nowH = Convert.ToInt32(spiderDate.ToString("HH"));
            if (nowH > 12)
            {
                //当前时间已经在爬取时间的第二天
                if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) > Convert.ToDateTime(spiderDate.ToString("yyyy-MM-dd")))
                {
                    url = url.Replace("-w31", "-w32");
                }
                else
                {
                    url = url.Replace("-w32", "-w31");
                }
            }
            return url;
        }
        #endregion
    }
}
