using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.RunSource.黄石信息港
{
    public class 其他城市 : NewDataSpider, INewDataRum
    {
        public 其他城市()
        {

        }
        public 其他城市(string 城市名称)
        {
            this.CityName = 城市名称;
        }
        public void start()
        {
            网站爬取配置 obj = SpiderWebConfigManager.根据城市获取黄石信息港爬取配置(CityName);
            if (obj != null)
            {
                start(obj.域名, obj.列表页链接, obj.详细页面爬取频率, obj.列表页面爬取频率);
            }
        }
        public void start(string 域名, string 列表页链接, int 详细页面爬取频率, int 列表页面爬取频率)
        {
            NewDataRum newDataRum = new NewDataRum(CityName, 域名, 列表页链接, 详细页面爬取频率, 列表页面爬取频率);
            newDataRum.start(this);

        }
    }
}
