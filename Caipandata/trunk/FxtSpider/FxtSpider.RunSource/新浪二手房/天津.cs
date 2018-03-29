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
    public class 天津 : NewDataSpider, INewDataRum
    {
        public void start()
        {
            NowPageEncoding = "gbk";
            regex_lpm.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_lpm", 网站名称, "NewDataSpider2"));
            regex_xzq.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_xzq", 网站名称, "NewDataSpider2"));
            regex_pq.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_pq", 网站名称, "NewDataSpider2"));
            regex_mj.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_mj", 网站名称, "NewDataSpider2"));
            regex_dj.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_dj", 网站名称, "NewDataSpider2"));
            regex_zj.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zj", 网站名称, "NewDataSpider2"));
            regex_szlc.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_szlc", 网站名称, "NewDataSpider2"));
            regex_zlc.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zlc", 网站名称, "NewDataSpider2"));
            regex_hx.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_hx", 网站名称, "NewDataSpider2"));
            regex_cx.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_cx", 网站名称, "NewDataSpider2"));
            regex_zx.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_zx", 网站名称, "NewDataSpider2"));
            regex_jznd.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_jznd", 网站名称, "NewDataSpider2"));
            regex_title.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_title", 网站名称, "NewDataSpider2"));
            regex_phone.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_phone", 网站名称, "NewDataSpider2"));
            regex_infUrl.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_infUrl", 网站名称, "NewDataSpider2"));
            regex_address.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_address", 网站名称, "NewDataSpider2"));
            regex_nextPage.RegexInfoList.Add(SpiderRegexInfoHelp.GetRegexInfoByXmlName("regex_nextPage", 网站名称, "NewDataSpider2"));
            网站爬取配置 obj = SpiderWebConfigManager.根据城市获取新浪二手房爬取配置("天津");
            if (obj != null)
            {
                NewDataRum newDataRum = new NewDataRum("天津", obj.域名, obj.列表页链接, obj.详细页面爬取频率, obj.列表页面爬取频率);
                newDataRum.start(this);
            }
        }
    }
}
