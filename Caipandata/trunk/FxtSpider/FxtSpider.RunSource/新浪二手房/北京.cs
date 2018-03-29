using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;
using FxtSpider.Bll.SpiderCommon;

namespace FxtSpider.RunSource.新浪二手房
{
    public class 北京 :NewDataSpider, INewDataRum
    {


        protected DataClassesDataContext db2 = new DataClassesDataContext();
        public void start()
        {
            NowPageEncoding = "gbk";
            //regex_lpm.RegexInfoList.Add(new RegexInfo("<div class=\"approve_price\">小区名称：<strong>(<a [^<>]*>([^<>]*)</a>|([^<>]+))</strong>", "$2$3"));
            //regex_xzq.RegexInfoList.Add(new RegexInfo("<div class=\"approve_price\">小区名称：<strong>(?:(?!</strong>).)*</strong> （<strong>([^<>\\-]*)\\-\\-[^<>\\-]*</strong>）", "$1"));
            //regex_pq.RegexInfoList.Add(new RegexInfo("<div class=\"approve_price\">小区名称：<strong>(?:(?!</strong>).)*</strong> （<strong>[^<>\\-]*\\-\\-([^<>\\-]*)</strong>）", "$1"));
            //regex_mj.RegexInfoList.Add(new RegexInfo("<li class=\"w400\"><strong>面积：([\\d\\.]*)㎡</strong></li>", "$1"));
            //regex_dj.RegexInfoList.Add(new RegexInfo("<li class=\"w240\"><strong>单价：([\\d\\.]*)元/㎡</strong>", "$1"));
            //regex_zj.RegexInfoList.Add(new RegexInfo("<li class=\"w240\"><strong>总价：([\\d\\.]*)万元</strong></li>", "$1"));
            //regex_szlc.RegexInfoList.Add(new RegexInfo("<li class=\"w240\">楼层：第([\\d]*)层（共[\\d]*层）</li>", "$1"));
            //regex_zlc.RegexInfoList.Add(new RegexInfo("<li class=\"w240\">楼层：第[\\d\\-]*层（共([\\d]*)层）</li>", "$1"));
            //regex_hx.RegexInfoList.Add(new RegexInfo("<li class=\"w400\"><strong>户型：([^<>]*)</strong></li>", "$1"));
            //regex_cx.RegexInfoList.Add(new RegexInfo("<li class=\"w240\">朝向：([^<>]*)</li>", "$1"));
            //regex_zx.RegexInfoList.Add(new RegexInfo("<li class=\"w400\">装修：([^<>]*)</li>", "$1"));
            //regex_jznd.RegexInfoList.Add(new RegexInfo("<li class=\"w400\">建筑年代：(\\d*)</li>", "$1"));
            //regex_title.RegexInfoList.Add(new RegexInfo("<div class=\"approvecot_titbg\"><span>([^<>]*)</span></div>", "$1"));
            //regex_phone.RegexInfoList.Add(new RegexInfo("<span class=\"telbg\"><strong>([^<>]*)</strong></span>", "$1"));
            //regex_infUrl.RegexInfoList.Add(new RegexInfo("<div class=\"search_font_line1_tit_list\"><a href=\"([^\"]*)\" target=\"_blank\" title=\"[^\"]*\">[^<>]*</a></div>", "$1"));
            //regex_address.RegexInfoList.Add(new RegexInfo("<li class=\"w650\">地址：([^<>]*)</li>", "$1"));
            //regex_nextPage.RegexInfoList.Add(new RegexInfo("<a href=\"([^\"]+)\" class=\"nextpage\">下一页</a>", "$1"));
            regex_lpm.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_lpm",网站名称,cityName:"NewDataSpider2"));
            regex_xzq.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_xzq", 网站名称, cityName: "NewDataSpider2"));
            regex_pq.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_pq", 网站名称, cityName: "NewDataSpider2"));
            regex_mj.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_mj", 网站名称, cityName: "NewDataSpider2"));
            regex_dj.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_dj", 网站名称, cityName: "NewDataSpider2"));
            regex_zj.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zj", 网站名称, cityName: "NewDataSpider2"));
            regex_szlc.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_szlc", 网站名称, cityName: "NewDataSpider2"));
            regex_zlc.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zlc", 网站名称, cityName: "NewDataSpider2"));
            regex_hx.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_hx", 网站名称, cityName: "NewDataSpider2"));
            regex_cx.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_cx", 网站名称, cityName: "NewDataSpider2"));
            regex_zx.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zx", 网站名称, cityName: "NewDataSpider2"));
            regex_jznd.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_jznd", 网站名称, cityName: "NewDataSpider2"));
            regex_title.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_title", 网站名称, cityName: "NewDataSpider2"));
            regex_phone.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_phone", 网站名称, cityName: "NewDataSpider2"));
            regex_infUrl.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_infUrl", 网站名称, cityName: "NewDataSpider2"));
            regex_address.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_address", 网站名称, cityName: "NewDataSpider2"));
            regex_nextPage.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_nextPage", 网站名称, cityName: "NewDataSpider2"));
            #region 生成xml
            //StringBuilder stest = new StringBuilder();
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_lpm.RegexInfoList[regex_lpm.RegexInfoList.Count-1], "regex_lpm", "楼盘名"));
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_xzq.RegexInfoList[regex_xzq.RegexInfoList.Count - 1], "regex_xzq", "行政区")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_pq.RegexInfoList[regex_pq.RegexInfoList.Count - 1], "regex_pq", "片区")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_hx.RegexInfoList[regex_hx.RegexInfoList.Count - 1], "regex_hx", "户型")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_mj.RegexInfoList[regex_mj.RegexInfoList.Count - 1], "regex_mj", "面积")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_dj.RegexInfoList[regex_dj.RegexInfoList.Count - 1], "regex_dj", "单价")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_zj.RegexInfoList[regex_zj.RegexInfoList.Count - 1], "regex_zj", "总价")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_jznd.RegexInfoList[regex_jznd.RegexInfoList.Count - 1], "regex_jznd", "建筑年代")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_cx.RegexInfoList[regex_cx.RegexInfoList.Count - 1], "regex_cx", "朝向")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_szlc.RegexInfoList[regex_szlc.RegexInfoList.Count - 1], "regex_szlc", "所在楼层")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_zlc.RegexInfoList[regex_zlc.RegexInfoList.Count - 1], "regex_zlc", "总楼层")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_zx.RegexInfoList[regex_zx.RegexInfoList.Count - 1], "regex_zx", "装修")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_title.RegexInfoList[regex_title.RegexInfoList.Count - 1], "regex_title", "信息")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_phone.RegexInfoList[regex_phone.RegexInfoList.Count - 1], "regex_phone", "电话")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_address.RegexInfoList[regex_address.RegexInfoList.Count - 1], "regex_address", "地址")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_infUrl.RegexInfoList[regex_infUrl.RegexInfoList.Count - 1], "regex_infUrl", "url")); ;
            //stest.Append(SpiderRegexInfoHelp.GetRegexInfoXmlByObj(regex_nextPage.RegexInfoList[regex_nextPage.RegexInfoList.Count - 1], "regex_nextPage", "下一页正则"));
            //string str = stest.ToString();
            #endregion
            网站爬取配置 obj = SpiderWebConfigManager.根据城市获取新浪二手房爬取配置("北京");
            if (obj != null)
            {
                NewDataRum newDataRum = new NewDataRum("北京", obj.域名, obj.列表页链接, obj.详细页面爬取频率, obj.列表页面爬取频率);
                newDataRum.start(this);
            }
        }

    }
}
