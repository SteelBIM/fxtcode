using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;

namespace FxtSpider.RunSource.搜房网
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
            List<网站爬取配置> objlist = SpiderWebConfigManager.根据城市获取搜房网爬取配置(CityName);
            foreach (网站爬取配置 obj in objlist)
            {
                其他城市 runObj = new 其他城市();
                runObj.CityName = CityName;
                start(runObj,obj.域名, obj.列表页链接, obj.详细页面爬取频率, obj.列表页面爬取频率, obj.规则编号, obj.主要用途, obj.主要案例类型);

            }
        }
        public void start(object runObj,string 域名, string 列表页链接, int 详细页面爬取频率, int 列表页面爬取频率, int? _RegexNumber, int? _BasePurposeCode, int? _BaseCaseTypeCode)
        {
            NewDataRum newDataRum = new NewDataRum(CityName, 域名, 列表页链接, 详细页面爬取频率, 列表页面爬取频率,_RegexNumber,_BasePurposeCode,_BaseCaseTypeCode);
            newDataRum.start(runObj);
        }
        public void start(string 域名, string 列表页链接, int 详细页面爬取频率, int 列表页面爬取频率, int? _RegexNumber, int? _BasePurposeCode, int? _BaseCaseTypeCode)
        {
            start(this,域名, 列表页链接, 详细页面爬取频率, 列表页面爬取频率,  _RegexNumber,  _BasePurposeCode,  _BaseCaseTypeCode);        
        }
    }
}
