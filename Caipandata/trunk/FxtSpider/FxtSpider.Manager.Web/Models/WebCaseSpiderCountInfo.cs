using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;

namespace FxtSpider.Manager.Web.Models
{
    /// <summary>
    /// 案例析取详情实体)
    /// </summary>
    public class WebCaseSpiderCountInfo
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 安居客(上周)总量
        /// </summary>
        public int Ajk_LastWeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 安居客(本周)总量
        /// </summary>
        public int Ajk_WeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 安居客浮动值
        /// </summary>
        public decimal Ajk_FloatValue
        {
            get;
            set;
        }
        /// <summary>
        /// 安居客已入库案例
        /// </summary>
        public int Ajk_ImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 安居客未入库案例
        /// </summary>
        public int Ajk_NotImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 搜房网(上周)总量
        /// </summary>
        public int Sfw_LastWeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 搜房网(本周)总量
        /// </summary>
        public int Sfw_WeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 搜房网浮动值
        /// </summary>
        public decimal Sfw_FloatValue
        {
            get;
            set;
        }
        /// <summary>
        /// 搜房网已入库案例
        /// </summary>
        public int Sfw_ImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 搜房网未入库案例
        /// </summary>
        public int Sfw_NotImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 新浪二手房(上周)总量
        /// </summary>
        public int Xl_LastWeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 新浪二手房(本周)总量
        /// </summary>
        public int Xl_WeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 新浪二手房浮动值
        /// </summary>
        public decimal Xl_FloatValue
        {
            get;
            set;
        }
        /// <summary>
        /// 新浪二手房已入库案例
        /// </summary>
        public int Xl_ImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 新浪二手房未入库案例
        /// </summary>
        public int Xl_NotImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 赶集网(上周)总量
        /// </summary>
        public int Gjw_LastWeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 赶集网(本周)总量
        /// </summary>
        public int Gjw_WeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 赶集网浮动值
        /// </summary>
        public decimal Gjw_FloatValue
        {
            get;
            set;
        }
        /// <summary>
        /// 赶集网已入库案例
        /// </summary>
        public int Gjw_ImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 赶集网未入库案例
        /// </summary>
        public int Gjw_NotImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 58同城(上周)总量
        /// </summary>
        public int Wbtc_LastWeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 58同城(本周)总量
        /// </summary>
        public int Wbtc_WeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 58同城浮动值
        /// </summary>
        public decimal Wbtc_FloatValue
        {
            get;
            set;
        }
        /// <summary>
        /// 58同城已入库案例
        /// </summary>
        public int Wbtc_ImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 赶58同城入库案例
        /// </summary>
        public int Wbtc_NotImportCount
        {
            get;
            set;
        }

        public WebCaseSpiderCountInfo()
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="ajk_LastWeekCount">安居客(上周)总量</param>
        /// <param name="ajk_WeekCount">安居客(本周)总量</param>
        /// <param name="ajk_FloatValue">安居客浮动值</param>
        /// <param name="ajk_ImportCount"> 安居客已入库案例</param>
        /// <param name="ajk_NotImportCount"> 安居客未入库案例</param>
        /// <param name="sfw_LastWeekCount">搜房网(上周)总量</param>
        /// <param name="sfw_WeekCount">搜房网(本周)总量</param>
        /// <param name="sfw_FloatValue">搜房网浮动值</param>
        /// <param name="sfw_ImportCount">搜房网已入库案例</param>
        /// <param name="sfw_NotImportCount"> 搜房网未入库案例</param>
        /// <param name="xl_LastWeekCount">新浪二手房(上周)总量</param>
        /// <param name="xl_WeekCount"> 新浪二手房(本周)总量</param>
        /// <param name="xl_FloatValue">新浪二手房浮动值</param>
        /// <param name="xl_ImportCount">新浪二手房已入库案例</param>
        /// <param name="xl_NotImportCount"> 新浪二手房未入库案例</param>
        /// <param name="gjw_LastWeekCount">赶集网(上周)总量</param>
        /// <param name="gjw_WeekCount">赶集网(本周)总量</param>
        /// <param name="gjw_FloatValue">赶集网浮动值</param>
        /// <param name="gjw_ImportCount">赶集网已入库案例</param>
        /// <param name="gjw_NotImportCount"> 赶集网未入库案例</param>
        /// <param name="wbtc_LastWeekCount">58同城(上周)总量</param>
        /// <param name="wbtc_WeekCount">58同城(本周)总量</param>
        /// <param name="wbtc_FloatValue">58同城浮动值</param>
        /// <param name="wbtc_ImportCount">58同城已入库案例</param>
        /// <param name="wbtc_NotImportCount"> 58同城未入库案例</param>
        public WebCaseSpiderCountInfo(string cityName,
            int ajk_LastWeekCount, int ajk_WeekCount, decimal ajk_FloatValue, int ajk_ImportCount, int ajk_NotImportCount,
            int sfw_LastWeekCount, int sfw_WeekCount, decimal sfw_FloatValue, int sfw_ImportCount, int sfw_NotImportCount,
            int xl_LastWeekCount, int xl_WeekCount, decimal xl_FloatValue, int xl_ImportCount, int xl_NotImportCount,
            int gjw_LastWeekCount, int gjw_WeekCount, decimal gjw_FloatValue, int gjw_ImportCount, int gjw_NotImportCount,
            int wbtc_LastWeekCount, int wbtc_WeekCount, decimal wbtc_FloatValue, int wbtc_ImportCount, int wbtc_NotImportCount)
        {
            this.CityName = cityName;
            this.Ajk_LastWeekCount = ajk_LastWeekCount;
            this.Ajk_WeekCount = ajk_WeekCount;
            this.Ajk_FloatValue = ajk_FloatValue;
            this.Ajk_ImportCount = ajk_ImportCount;
            this.Ajk_ImportCount = ajk_NotImportCount;
            this.Sfw_LastWeekCount = sfw_LastWeekCount;
            this.Sfw_WeekCount = sfw_WeekCount;
            this.Sfw_FloatValue = sfw_FloatValue;
            this.Sfw_ImportCount = sfw_ImportCount;
            this.Sfw_ImportCount = sfw_NotImportCount;
            this.Xl_LastWeekCount = xl_LastWeekCount;
            this.Xl_WeekCount = xl_WeekCount;
            this.Xl_FloatValue = xl_FloatValue;
            this.Xl_ImportCount = xl_ImportCount;
            this.Xl_ImportCount = xl_NotImportCount;
            this.Gjw_LastWeekCount = gjw_LastWeekCount;
            this.Gjw_WeekCount = gjw_WeekCount;
            this.Gjw_FloatValue = gjw_FloatValue;
            this.Gjw_ImportCount = gjw_ImportCount;
            this.Gjw_ImportCount = gjw_NotImportCount;
            this.Wbtc_LastWeekCount = wbtc_LastWeekCount;
            this.Wbtc_WeekCount = wbtc_WeekCount;
            this.Wbtc_FloatValue = wbtc_FloatValue;
            this.Wbtc_ImportCount = wbtc_ImportCount;
            this.Wbtc_ImportCount = wbtc_NotImportCount;
        }

        public static WebCaseSpiderCountInfo GetCaseSpiderInfo(List<get_案例信息_获取时间段内城市网站的爬取个数Result> lastWeekSpiderCaseCountList,
                 List<get_案例信息_获取时间段内城市网站的爬取个数Result> weekSpiderCaseCountList,
                  List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result> importCaseCountList,
                 List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result> notImportCaseCountList, int cityId, string cityName)
        {

            int ajk_LastWeekCount = 0; int ajk_WeekCount = 0; decimal ajk_FloatValue = 0; int ajk_ImportCount = 0; int ajk_NotImportCount = 0;
            int sfw_LastWeekCount = 0; int sfw_WeekCount = 0; decimal sfw_FloatValue = 0; int sfw_ImportCount = 0; int sfw_NotImportCount = 0;
            int xl_LastWeekCount = 0; int xl_WeekCount = 0; decimal xl_FloatValue = 0; int xl_ImportCount = 0; int xl_NotImportCount = 0;
            int gjw_LastWeekCount = 0; int gjw_WeekCount = 0; decimal gjw_FloatValue = 0; int gjw_ImportCount = 0; int gjw_NotImportCount = 0;
            int wbtc_LastWeekCount = 0; int wbtc_WeekCount = 0; decimal wbtc_FloatValue = 0; int wbtc_ImportCount = 0; int wbtc_NotImportCount = 0;
            //***安居客*****//
            GetCaseSpiderInfoByWebIdAndCityId(lastWeekSpiderCaseCountList, weekSpiderCaseCountList, importCaseCountList, notImportCaseCountList,
                cityId, WebsiteManager.安居客_ID, out ajk_LastWeekCount, out ajk_WeekCount, out ajk_FloatValue, out ajk_ImportCount, out ajk_NotImportCount);
            //***搜房网*****//
            GetCaseSpiderInfoByWebIdAndCityId(lastWeekSpiderCaseCountList, weekSpiderCaseCountList, importCaseCountList, notImportCaseCountList,
                cityId, WebsiteManager.搜房网_ID, out sfw_LastWeekCount, out sfw_WeekCount, out sfw_FloatValue, out sfw_ImportCount, out sfw_NotImportCount);
            //***新浪二手房*****//
            GetCaseSpiderInfoByWebIdAndCityId(lastWeekSpiderCaseCountList, weekSpiderCaseCountList, importCaseCountList, notImportCaseCountList,
                cityId, WebsiteManager.新浪二手房_ID, out xl_LastWeekCount, out xl_WeekCount, out xl_FloatValue, out xl_ImportCount, out xl_NotImportCount);
            //***赶集网*****//
            GetCaseSpiderInfoByWebIdAndCityId(lastWeekSpiderCaseCountList, weekSpiderCaseCountList, importCaseCountList, notImportCaseCountList,
                cityId, WebsiteManager.赶集网_ID, out gjw_LastWeekCount, out gjw_WeekCount, out gjw_FloatValue, out gjw_ImportCount, out gjw_NotImportCount);
            //***58同城*****//
            GetCaseSpiderInfoByWebIdAndCityId(lastWeekSpiderCaseCountList, weekSpiderCaseCountList, importCaseCountList, notImportCaseCountList,
                cityId, WebsiteManager.五八同城_ID, out wbtc_LastWeekCount, out wbtc_WeekCount, out wbtc_FloatValue, out wbtc_ImportCount, out wbtc_NotImportCount);

            WebCaseSpiderCountInfo en = new WebCaseSpiderCountInfo(cityName,
                ajk_LastWeekCount, ajk_WeekCount, ajk_FloatValue, ajk_ImportCount, ajk_NotImportCount,
               sfw_LastWeekCount, sfw_WeekCount, sfw_FloatValue, sfw_ImportCount, sfw_NotImportCount,
             xl_LastWeekCount, xl_WeekCount, xl_FloatValue, xl_ImportCount, xl_NotImportCount,
             gjw_LastWeekCount, gjw_WeekCount, gjw_FloatValue, gjw_ImportCount, gjw_NotImportCount,
             wbtc_LastWeekCount, wbtc_WeekCount, wbtc_FloatValue, wbtc_ImportCount, wbtc_NotImportCount);
            return en;
        }

        public static void GetCaseSpiderInfoByWebIdAndCityId(List<get_案例信息_获取时间段内城市网站的爬取个数Result> lastWeekSpiderCaseCountList,
                 List<get_案例信息_获取时间段内城市网站的爬取个数Result> weekSpiderCaseCountList,
                  List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result> importCaseCountList,
                 List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result> notImportCaseCountList,
                int cityId, int webId, out int lastWeekCount, out int weekCount, out  decimal floatValue, out  int importCount, out int notImportCount)
        {
            lastWeekCount = 0; weekCount = 0; floatValue = 0; importCount = 0; notImportCount = 0;
            //上周数量
            get_案例信息_获取时间段内城市网站的爬取个数Result ajkCount1 = lastWeekSpiderCaseCountList.Find(
                delegate(get_案例信息_获取时间段内城市网站的爬取个数Result obj)
                { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (ajkCount1 != null) { lastWeekCount = Convert.ToInt32(ajkCount1.个数); }
            //本周数量
            get_案例信息_获取时间段内城市网站的爬取个数Result ajkCount2 = weekSpiderCaseCountList.Find(
                delegate(get_案例信息_获取时间段内城市网站的爬取个数Result obj)
                { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (ajkCount2 != null) { weekCount = Convert.ToInt32(ajkCount2.个数); }
            //浮动值
            floatValue = (Convert.ToDecimal(weekCount) / Convert.ToDecimal(lastWeekCount)) - 1;
            floatValue = Decimal.Round(floatValue, 4);
            //已入库案例数
            get_案例信息_获取时间段内城市网站的已入库的案例个数Result ajkImport = importCaseCountList.Find(
                 delegate(get_案例信息_获取时间段内城市网站的已入库的案例个数Result obj)
                 { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (ajkImport != null) { importCount = Convert.ToInt32(ajkImport.个数); }
            //已入库案例数
            get_案例信息_获取时间段内城市网站的未入库的案例个数Result ajkNotImport = notImportCaseCountList.Find(
                 delegate(get_案例信息_获取时间段内城市网站的未入库的案例个数Result obj)
                 { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (ajkNotImport != null) { importCount = Convert.ToInt32(ajkNotImport.个数); }
        }
    }
}