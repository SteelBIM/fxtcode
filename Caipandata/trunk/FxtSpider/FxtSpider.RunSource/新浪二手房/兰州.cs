using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Dll.LinqToSql;
using FxtSpider.Bll;

namespace FxtSpider.RunSource.新浪二手房
{
    public class 兰州 : NewDataSpider, INewDataRum
    {
        protected DataClassesDataContext db2 = new DataClassesDataContext();
        public void start()
        {
            网站爬取配置 obj = SpiderWebConfigManager.根据城市获取新浪二手房爬取配置("兰州");
            if (obj != null)
            {
                NewDataRum newDataRum = new NewDataRum("兰州", obj.域名, obj.列表页链接, obj.详细页面爬取频率, obj.列表页面爬取频率);
                newDataRum.start(this);
            }
        }
    }
}
