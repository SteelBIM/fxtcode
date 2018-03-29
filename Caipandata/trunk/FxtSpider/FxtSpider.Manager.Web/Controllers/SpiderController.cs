using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpider.Manager.Common;
using System.Net.Http;
using Newtonsoft.Json;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;
using FxtSpider.Common;
using System.Text;
using System.IO;
using FxtSpider.Manager.Web.Models;
using FxtSpider.DAL.LinqToSqlModels;
using Newtonsoft.Json.Linq;
using FxtSpider.Manager.Web.Common;
using System.Web.Caching;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.ApiManager;
using System.Linq.Expressions;
using FxtSpider.Bll.FxtApiManager;
using System.Net;

namespace FxtSpider.Manager.Web.Controllers
{
    public class SpiderController : BaseController
    {
        //
        // GET: /Spider/
        [HttpPost]
        public ActionResult Index()
        {
            string strR = string.Empty;
            HttpClient httpClient = new HttpClient();

            //var postData = new List<KeyValuePair<string, string>>();
            //postData.Add(new KeyValuePair<string, string>("", "1"));
            //postData.Add(new KeyValuePair<string, string>("aa", "2"));
            //var content = new FormUrlEncodedContent(postData);
            ////Post
            //var response = httpClient.PostAsync(GetUrl("api/Test/Posts/"), null);

            //string aa = response.Result.Content.ReadAsStringAsync().Result;

            HttpClient httpClient1 = new HttpClient();
            //Get
            var result = httpClient1.GetAsync(GetUrl("api/Test/GetTest/?Name=aa&Name1=ddd")).Result;
            strR = result.Content.ReadAsAsync<string>().Result;
            return Json(strR);
        }

        #region (CaseSearch.cshtml)

        /// <summary>
        /// /View/CaseSearch.cshtml
        /// </summary>
        /// <returns></returns>
        public ActionResult CaseSearch()
        {
            List<城市表> cityList = CityManager.GetAllCity();
            List<网站表> webList = WebsiteManager.GetAllWebsite();
            ViewBag.Citylist = cityList;
            ViewBag.WebList = webList;
            return View();
        }
        /// <summary>
        /// /View/CaseSearch.cshtml (ajax)
        /// 爬取案例高级查询
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="webId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="start"></param>
        /// <param name="pageLength"></param>
        /// <param name="_isGetCount"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType=RequestType.AJAX)]
        public ActionResult CaseSearch_Api(string cityId, string webId, string startDate, string endDate, string start, string pageLength, string _isGetCount)
        {
            int count = 0;
            bool isGetCount = true;
            int? _cityId = null;
            int? _webId = null;
            if (_isGetCount.Equals("0"))
            {
                isGetCount = false;
            }
            if (StringHelp.IsInteger(cityId) && !cityId.Equals("0"))
            {
                _cityId = Convert.ToInt32(cityId);
            }
            if (StringHelp.IsInteger(webId) && !webId.Equals("0"))
            {
                _webId = Convert.ToInt32(webId);
            }
            if (string.IsNullOrEmpty(startDate) || !StringHelp.CheckStrIsDate(startDate))
            {
                startDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(endDate) || !StringHelp.CheckStrIsDate(endDate))
            {
                endDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(start) || !StringHelp.IsInteger(start))
            {
                start = "1";
            }
            if (string.IsNullOrEmpty(pageLength) || !StringHelp.IsInteger(pageLength))
            {
                pageLength = "1";
            }
            if (Convert.ToInt32(pageLength) > 100)
            {
                pageLength = "100";
            }

            startDate = startDate + " 00:00:00";
            endDate = endDate + " 23:59:59";
            string message = "";
            List<VIEW_案例信息_城市表_网站表> list = CaseManager.GetVIEW案例信息_根据城市网站案例日期的区间获取案例(
                _cityId,_webId, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), Convert.ToInt32(start), Convert.ToInt32(pageLength),
                isGetCount, out count, out message);

            string listJson = list.Web_GetJson();
            string resultJson = new StringBuilder()
                              .Append("{\"List\":").Append(string.IsNullOrEmpty(listJson) ? "null" : listJson)
                              .Append(",\"Count\":").Append(count).Append("}").ToString();

            resultJson = resultJson.MvcResponseJson();
            Response.Write(resultJson);
            Response.End();
            return null;
        }
        /// <summary>
        /// /View/CaseSearch.cshtml (ajax)
        /// 下载查询出的数据到excel
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="webId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public FileStreamResult DownloadCaseSearch_Api(string cityId, string webId, string startDate, string endDate)
        {
            return null;
            string cityName = "全部";
            string webName = "全部";
            int? _cityId = null;
            int? _webId = null;
            if (StringHelp.IsInteger(cityId) && !cityId.Equals("0"))
            {
                _cityId = Convert.ToInt32(cityId);
            }
            if (StringHelp.IsInteger(webId) && !webId.Equals("0"))
            {
                _webId = Convert.ToInt32(webId);
            }
            if (string.IsNullOrEmpty(startDate) || !StringHelp.CheckStrIsDate(startDate))
            {
                startDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(endDate) || !StringHelp.CheckStrIsDate(endDate))
            {
                endDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            城市表 city = CityManager.GetCityById(Convert.ToInt32(cityId));
            if (city != null) { cityName = city.城市名称; }
            网站表 web = WebsiteManager.GetWebById(Convert.ToInt32(webId));
            if (web != null) { webName = web.网站名称; }
            startDate = startDate + " 00:00:00";
            endDate = endDate + " 23:59:59";
            //Excel导出
            string _excelSaveFileName = string.Format("{0}_{1}_({2}_{3})_{4}.xls", cityName, webName, Convert.ToDateTime(startDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(endDate).ToString("yyyy-MM-dd"),"");// Convert.ToString(System.Guid.NewGuid())
            string filePath = Server.MapPath("/DownloadFile/" + _excelSaveFileName);
            
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            if (!System.IO.File.Exists(filePath))
            {
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.GetEncoding("gb2312"));
                #region(设置表头)
                StringBuilder sb = new StringBuilder();
                sb.Append("来源").Append("\t");
                sb.Append("城市").Append("\t");
                sb.Append("行政区").Append("\t");
                sb.Append("片区").Append("\t");
                sb.Append("楼盘名*").Append("\t");
                sb.Append("案例时间").Append("\t");
                sb.Append("楼栋").Append("\t");
                sb.Append("房号").Append("\t");
                sb.Append("用途*").Append("\t");
                sb.Append("面积*").Append("\t");
                sb.Append("单价*").Append("\t");
                sb.Append("总价*").Append("\t");
                sb.Append("案例类型*").Append("\t");
                sb.Append("结构").Append("\t");
                sb.Append("建筑类型").Append("\t");
                sb.Append("总楼层").Append("\t");
                sb.Append("所在层").Append("\t");
                sb.Append("户型").Append("\t");
                sb.Append("朝向").Append("\t");
                sb.Append("装修").Append("\t");
                sb.Append("建筑年代").Append("\t");
                sb.Append("电话").Append("\t");
                sb.Append("URL").Append("\t");
                sb.Append("地址").Append("\t");
                sb.Append("创建时间").Append("\t");
                sb.Append("建筑形式").Append("\t");
                sb.Append("花园面积").Append("\t");
                sb.Append("厅结构").Append("\t");
                sb.Append("车位数量").Append("\t");
                sb.Append("配套设施").Append("\t");
                sb.Append("地下室面积").Append("\t");
                sb.Append("币种").Append("\t");
                sb.Append("备注").Append("\t");
                sb.Append(Environment.NewLine);
                sw.Write(sb.ToString());
                #endregion
                int pageLength = 40000;
                int start = 1;
                int count = 0;
                int pageCount = 0;
                string message = "";
                List<VIEW_案例信息_城市表_网站表> list = CaseManager.GetVIEW案例信息_根据城市网站案例日期的区间获取案例(
                    _cityId, _webId, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), 1, 1,
                    true, out count,out message);
                pageCount = ((count - 1) / pageLength) + 1;
                if (count > 0)
                {
                    for (int i = start; i <= pageCount; i++)
                    {
                        int _count = 0;

                        list = CaseManager.GetVIEW案例信息_根据城市网站案例日期的区间获取案例(
                                _cityId, _webId, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), i, pageLength,
                                false, out _count, out message);
                        #region(设置表内容)
                        StringBuilder sb2 = new StringBuilder();
                        for (int j = 0; j < list.Count; j++)
                        {
                            string projectName = "";
                            string areaName = "";
                            string subAreaName = "";
                            if (list[j].ProjectName == null)
                            {
                                if (!string.IsNullOrEmpty(list[j].楼盘名))
                                {
                                    projectName = list[j].楼盘名;
                                }
                            }
                            else
                            {
                                projectName = list[j].ProjectName;
                            }
                            if (list[j].AreaName == null)
                            {
                                if (!string.IsNullOrEmpty(list[j].行政区))
                                {
                                    areaName = list[j].行政区;
                                }
                            }
                            else
                            {
                                areaName = list[j].AreaName;
                            }
                            if (list[j].SubAreaName == null)
                            {
                                if (!string.IsNullOrEmpty(list[j].片区))
                                {
                                    subAreaName = list[j].片区;
                                }
                            }
                            else
                            {
                                subAreaName = list[j].SubAreaName;
                            }
                            sb2.Append(list[j].来源).Append("\t");
                            sb2.Append(list[j].城市).Append("\t");
                            sb2.Append(areaName).Append("\t");
                            sb2.Append(subAreaName).Append("\t");
                            sb2.Append(projectName).Append("\t");
                            sb2.Append(list[j].案例时间).Append("\t");
                            sb2.Append(list[j].楼栋).Append("\t");
                            sb2.Append(list[j].房号).Append("\t");
                            sb2.Append(list[j].SysData用途).Append("\t");
                            sb2.Append(list[j].面积).Append("\t");
                            sb2.Append(list[j].单价).Append("\t");
                            sb2.Append(list[j].总价).Append("\t");
                            sb2.Append(list[j].SysData案例类型).Append("\t");
                            sb2.Append(list[j].SysData结构).Append("\t");
                            sb2.Append(list[j].SysData建筑类型).Append("\t");
                            sb2.Append(list[j].总楼层).Append("\t");
                            sb2.Append(list[j].所在楼层).Append("\t");
                            sb2.Append(list[j].SysData户型).Append("\t");
                            sb2.Append(list[j].SysData朝向).Append("\t");
                            sb2.Append(list[j].SysData装修).Append("\t");
                            sb2.Append(list[j].建筑年代).Append("\t");
                            sb2.Append(list[j].电话).Append("\t");
                            sb2.Append(list[j].URL).Append("\t");
                            sb2.Append(list[j].地址).Append("\t");
                            sb2.Append(list[j].创建时间).Append("\t");
                            sb2.Append(list[j].建筑形式).Append("\t");
                            sb2.Append(list[j].花园面积).Append("\t");
                            sb2.Append(list[j].厅结构).Append("\t");
                            sb2.Append(list[j].车位数量).Append("\t");
                            sb2.Append(list[j].配套设施).Append("\t");
                            sb2.Append(list[j].地下室面积).Append("\t");
                            sb2.Append(list[j].SysData币种).Append("\t");
                            sb2.Append(list[j].信息).Append("\t");
                            sb2.Append(Environment.NewLine);

                        }

                        sw.Write(sb2.ToString());
                        #endregion
                    }

                }
                sw.Flush();
                sw.Close();

            }

            return File(new FileStream(filePath, FileMode.Open), "application/octet-stream", _excelSaveFileName);
            ////FileInfo file = new FileInfo(filePath);
            //Response.Clear();
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + _excelSaveFileName);
            ////Response.AddHeader("Content-Length", file.Length.ToString());
            //Response.ContentType = "application/octet-stream";
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.WriteFile(filePath);
            //Response.Flush();
            //Response.End();
            //return null;
        }

        #endregion

        #region (CaseSpiderCountInfoList.cshtml)

        /// <summary>
        /// /View/CaseSpiderCountInfoList.cshtml
        /// </summary>
        /// <returns></returns>
        public ActionResult CaseSpiderCountInfoList()
        {
            DateTime nowDate = DateTime.Now.AddDays(-1);
            List<string> strList = new List<string>();
            int dayCount = 7;
            strList.Add(nowDate.ToString("yyyy-MM-dd"));
            for (int i = 1; i < dayCount; i++)
            {
                strList.Add(nowDate.AddDays((0 - i)).ToString("yyyy-MM-dd"));
            }
            ViewBag.NowDates = strList;
            int nowWeek = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
            string lastWeekStartTimeStr = nowDate.AddDays(0 - (14 + nowWeek)).ToString("yyyy-MM-dd");
            string lastWeekEndTimeStr = nowDate.AddDays(0 - (8 + nowWeek)).ToString("yyyy-MM-dd") + " 23:59:59";
            string weekStartTimeStr = nowDate.AddDays(0 - (7 + nowWeek)).ToString("yyyy-MM-dd");
            string weekEndTimeStr = nowDate.AddDays(0 - (1 + nowWeek)).ToString("yyyy-MM-dd") + " 23:59:59"; 

            ViewBag.lastWeek1=Convert.ToDateTime(lastWeekStartTimeStr).ToString("MM月dd");
            ViewBag.lastWeek2 = Convert.ToDateTime(lastWeekEndTimeStr).ToString("MM月dd");
            ViewBag.week1 = Convert.ToDateTime(weekStartTimeStr).ToString("MM月dd");
            ViewBag.week2 = Convert.ToDateTime(weekEndTimeStr).ToString("MM月dd");


            return View();
        }
        /// <summary>
        /// /View/CaseSpiderCountInfoList.cshtml(ajax)
        /// 获取案例爬取详情(按周)
        /// </summary>
        /// <param name="floatValue">浮动值查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="direction">降序或升序</param>
        /// <param name="nowJsonData">当前页面中已经查询出的数据(用于排序)</param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseSpiderCountInfoListByWeek_Api(string floatValue, string orderBy, string direction, string nowJsonData)
        {

            int nowWeek = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
            DateTime nowDate = DateTime.Now;


            string lastWeekStartTimeStr = nowDate.AddDays(0 - (14 + nowWeek)).ToString("yyyy-MM-dd");
            string lastWeekEndTimeStr = nowDate.AddDays(0 - (8 + nowWeek)).ToString("yyyy-MM-dd") + " 23:59:59";
            string weekStartTimeStr = nowDate.AddDays(0 - (7 + nowWeek)).ToString("yyyy-MM-dd");
            string weekEndTimeStr = nowDate.AddDays(0 - (1 + nowWeek)).ToString("yyyy-MM-dd") + " 23:59:59"; 

            //上周
            DateTime lastWeekStartTime = Convert.ToDateTime(lastWeekStartTimeStr);
            DateTime lastWeekEndTime = Convert.ToDateTime(lastWeekEndTimeStr);
            //本周
            DateTime weekStartTime = Convert.ToDateTime(weekStartTimeStr);
            DateTime weekEndTime = Convert.ToDateTime(weekEndTimeStr);
            List<CaseSpiderCountInfo> list = new List<CaseSpiderCountInfo>();
            //如果只是排序(则会回传当前列表数据)
            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(direction))
            {
                if (!string.IsNullOrEmpty(nowJsonData))
                {
                    list = JsonHelp.ParseJSONList<CaseSpiderCountInfo>(nowJsonData);
                }
            }
            else
            {
                list = GetCaseSpiderCountInfoListByWeek_Cache(lastWeekStartTime, lastWeekEndTime, weekStartTime, weekEndTime);
                ////获取上周爬取数量
                //List<get_案例信息_获取时间段内城市网站的爬取个数Result> lastWeekSpiderCaseCountList = CaseManager.GetCaseSpiderCountInfoByDate(lastWeekStartTime, lastWeekEndTime);
                ////获取本周爬取数量
                //List<get_案例信息_获取时间段内城市网站的爬取个数Result> weekSpiderCaseCountList = CaseManager.GetCaseSpiderCountInfoByDate(weekStartTime, weekEndTime);
                ////获取两周已入库案例
                //List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result> importCaseCountList = CaseManager.GetCaseImportCountInfoByDate(lastWeekStartTime, weekEndTime);
                ////获取两周未入库案例
                //List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result> notImportCaseCountList = CaseManager.GetCaseNotImportCountInfoByDate(lastWeekStartTime, weekEndTime);
                ////数据转换
                //List<VIEW_网站爬取配置_城市表_网站表> vList = SpiderWebConfigManager.GetAllVIEW网站爬取配置();
                //vList = vList.Sort_CityId_Ase();
                //foreach (VIEW_网站爬取配置_城市表_网站表 v in vList)
                //{
                //    CaseSpiderCountInfo info = CaseSpiderCountInfo.GetCaseSpiderInfo(lastWeekSpiderCaseCountList, weekSpiderCaseCountList,
                //        importCaseCountList, notImportCaseCountList, v.城市ID, v.城市名称, v.网站ID, v.网站名称);
                //    info = JsonHelp.EncodeField<CaseSpiderCountInfo>(info);
                //    list.Add(info);
                //}
            }
            //过滤查询条件
            if (!string.IsNullOrEmpty(floatValue) && Convert.ToDouble(floatValue) > 0)
            {
                double nowFloatValue=Convert.ToDouble(floatValue);
                List<CaseSpiderCountInfo> list2 = new List<CaseSpiderCountInfo>();
                list.ForEach(delegate(CaseSpiderCountInfo obj)
                {
                    if (Math.Abs(Convert.ToDecimal(obj.FloatValue)) >= Convert.ToDecimal(nowFloatValue))
                    {
                        list2.Add(obj);
                    }
                });
                list = list2;
            }
            //排序
            if (!string.IsNullOrEmpty(orderBy))
            {
                //获取排序字段
                if (orderBy.ToLower().Equals("floatvalue"))
                {
                    orderBy = "FloatValue";
                }
                //获取排序顺序
                ReverserInfo.Direction directionEnum = ReverserInfo.Direction.DESC;
                if (!string.IsNullOrEmpty(direction))
                {
                    if (direction.ToLower().Equals("desc"))
                    {
                        directionEnum = ReverserInfo.Direction.DESC;
                    }
                    else
                    {
                        directionEnum = ReverserInfo.Direction.ASC;
                    }
                }
                //进行排序计算
                Reverser<CaseSpiderCountInfo> reverser = new Reverser<CaseSpiderCountInfo>
                    (new CaseSpiderCountInfo().GetType(), orderBy, directionEnum);

                list.Sort(reverser);

            }
             
            //输出数据
            string resultJson = JsonHelp.ToJSONjss(list);
            string dateJson = new StringBuilder()
                .Append("{\"lastweek_start\":\"").Append(lastWeekStartTimeStr.EncodeField()).Append("\",")
                .Append("\"lastweek_end\":\"").Append(lastWeekEndTimeStr.EncodeField()).Append("\",")
                .Append("\"week_start\":\"").Append(weekStartTimeStr.EncodeField()).Append("\",")
                .Append("\"week_end\":\"").Append(weekEndTimeStr.EncodeField()).Append("\"}").ToString();
            resultJson = string.Format("{{\"List\":{0},\"Date\":{1}}}", resultJson, dateJson);
            Response.Write(resultJson.MvcResponseJson());
            Response.End();
            return null;
        }
        //缓存
        private List<CaseSpiderCountInfo> GetCaseSpiderCountInfoListByWeek_Cache(DateTime lastWeekStartTime, DateTime lastWeekEndTime, DateTime weekStartTime, DateTime weekEndTime)
        {
            List<CaseSpiderCountInfo> list = new List<CaseSpiderCountInfo>();
            string sbCacheKey = new StringBuilder().Append("GetCaseSpiderCountInfoListByWeek_Cache_")
                .Append(lastWeekStartTime.ToString("yyyy-MM-dd")).Append("_")
                .Append(lastWeekEndTime.ToString("yyyy-MM-dd")).Append("_")
                .Append(weekStartTime.ToString("yyyy-MM-dd")).Append("_")
                .Append(weekEndTime.ToString("yyyy-MM-dd")).ToString();
            if (null == HttpRuntime.Cache[sbCacheKey])
            {
                //获取上周爬取数量
                List<get_案例信息_获取时间段内城市网站的爬取个数Result> lastWeekSpiderCaseCountList = CaseManager.GetCaseSpiderCountInfoByDate(lastWeekStartTime, lastWeekEndTime);
                //获取本周爬取数量
                List<get_案例信息_获取时间段内城市网站的爬取个数Result> weekSpiderCaseCountList = CaseManager.GetCaseSpiderCountInfoByDate(weekStartTime, weekEndTime);
                //获取两周已入库案例
                List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result> importCaseCountList = CaseManager.GetCaseImportCountInfoByDate(lastWeekStartTime, weekEndTime);
                //获取两周未入库案例
                List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result> notImportCaseCountList = CaseManager.GetCaseNotImportCountInfoByDate(lastWeekStartTime, weekEndTime);
                //数据转换
                List<VIEW_网站爬取配置_城市表_网站表> vList = SpiderWebConfigManager.GetAllVIEW网站爬取配置();
                vList = vList.Sort_CityId_Ase();
                foreach (VIEW_网站爬取配置_城市表_网站表 v in vList)
                {
                    CaseSpiderCountInfo info = CaseSpiderCountInfo.GetCaseSpiderInfo(lastWeekSpiderCaseCountList, weekSpiderCaseCountList,
                        importCaseCountList, notImportCaseCountList, v.城市ID, v.城市名称, v.网站ID, v.网站名称);
                    info = JsonHelp.EncodeField<CaseSpiderCountInfo>(info);
                    list.Add(info);
                }
                string resultJson = JsonHelp.ToJSONjss(list);
                HttpRuntime.Cache.Add(sbCacheKey, resultJson, null, DateTime.Now.AddDays(7), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            else
            {
                string resultJson = Convert.ToString(HttpRuntime.Cache[sbCacheKey]);
                if (!string.IsNullOrEmpty(resultJson))
                {
                    list = JsonHelp.ParseJSONList<CaseSpiderCountInfo>(resultJson);
                }
            }
            return list;
        }
        /// <summary>
        /// /View/CaseSpiderCountInfoList.cshtml(ajax)
        /// 获取爬取详情(按天)
        /// </summary>
        /// <param name="dates">日期</param>
        /// <param name="orderBy"></param>
        /// <param name="direction"></param>
        /// <param name="nowJsonData"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseSpiderCountInfoListByDays_Api(string datesStr,string orderBy, string direction,  string nowJsonData)
        {
            string[] dates = null;
            string dateJson = "";
            if (!string.IsNullOrEmpty(datesStr))
            {
                dates = datesStr.Split(',');
            }
            List<JObject> allCountList = new List<JObject>();
            //如果只是排序(则会回传当前列表数据)
            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(direction))
            {
                if (!string.IsNullOrEmpty(nowJsonData))
                {
                    allCountList = JsonHelp.ParseJSONList<JObject>(nowJsonData);
                }
            }
            else
            {
                //根据传过来的日期数组获取个数信息
                Dictionary<string, List<get_案例信息_获取时间段内城市网站的爬取个数Result>> dicCountList = new Dictionary<string, List<get_案例信息_获取时间段内城市网站的爬取个数Result>>();
                if (dates != null && dates.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dates.Length; i++)
                    {
                        DateTime startDate = Convert.ToDateTime(Convert.ToDateTime(dates[i]).ToString("yyyy-MM-dd"));
                        DateTime endDate = Convert.ToDateTime(startDate.ToString("yyyy-MM-dd") + " 23:59:59");
                        List<get_案例信息_获取时间段内城市网站的爬取个数Result> countList = GetCaseSpiderCountInfoListByDays_Cache(startDate, endDate);//CaseManager.GetCaseSpiderCountInfoByDate(startDate, endDate);
                        dicCountList.Add("DateColumn" + i.ToString(), countList);

                        sb.Append("\"DateColumn" + i.ToString() + "_start\":\"").Append(startDate.ToString("yyyy-MM-dd").EncodeField()).Append("\",")
                          .Append("\"DateColumn" + i.ToString() + "_end\":\"").Append(endDate.ToString("yyyy-MM-dd HH:mm:ss").EncodeField()).Append("\",");
                    }
                    dateJson = "{" + sb.ToString().TrimEnd(',') + "}";
                }
                //循环绑个数信息到list
                List<城市表> cityList = CityManager.GetAllCity();
                List<网站表> webList = WebsiteManager.GetAllWebsite();  //数据转换
                List<VIEW_网站爬取配置_城市表_网站表> vList = SpiderWebConfigManager.GetAllVIEW网站爬取配置();
                vList = vList.Sort_CityId_Ase();
                foreach (VIEW_网站爬取配置_城市表_网站表 v in vList)
                {
                    //绑定行对象到list
                    JObject jobject = new JObject();
                    #region(绑定属性和值到对象jobject)
                    //给行对象绑定属性和值
                    JProperty prop1 = new JProperty("CityId", v.城市ID);
                    JProperty prop2 = new JProperty("CityName", v.城市名称);
                    JProperty prop3 = new JProperty("WebId", v.网站ID);
                    JProperty prop4 = new JProperty("WebName", v.网站名称);
                    jobject.Add(prop1);
                    jobject.Add(prop2);
                    jobject.Add(prop3);
                    jobject.Add(prop4);
                    //根据传过来的日期绑定属性和值
                    for (int i = 0; i < dates.Length; i++)
                    {
                        string column = "DateColumn" + i.ToString();
                        int value = 0;
                        if (dicCountList.ContainsKey(column))
                        {
                            List<get_案例信息_获取时间段内城市网站的爬取个数Result> countList = dicCountList[column];
                            get_案例信息_获取时间段内城市网站的爬取个数Result obj = countList.Find(
                                delegate(get_案例信息_获取时间段内城市网站的爬取个数Result _obj)
                                { return _obj.城市ID == v.城市ID && _obj.网站ID == v.网站ID; });
                            if (obj != null)
                            {
                                value = Convert.ToInt32(obj.个数);
                            }
                        }
                        JProperty propColumn = new JProperty(column, value);
                        jobject.Add(propColumn);
                    }
                    #endregion
                    allCountList.Add(jobject);
                }
            }
            //排序计算
            if (!string.IsNullOrEmpty(orderBy))
            {
                //获取排序顺序
                ReverserInfo.Direction directionEnum = ReverserInfo.Direction.DESC;
                if (!string.IsNullOrEmpty(direction))
                {
                    if (direction.ToLower().Equals("desc"))
                    {
                        directionEnum = ReverserInfo.Direction.DESC;
                    }
                    else
                    {
                        directionEnum = ReverserInfo.Direction.ASC;
                    }
                }
                //进行排序计算 
                #region (升序降序计算)
                if (directionEnum == ReverserInfo.Direction.DESC)
                {
                    allCountList.Sort((left, rigth) =>
                    {
                        if (left[orderBy].Value<double>() > rigth[orderBy].Value<double>())
                        {
                            return 1;
                        }
                        if (left[orderBy].Value<double>() == rigth[orderBy].Value<double>())
                        {
                            return 0;
                        }
                        return -1;
                    }
                    );
                }
                else
                {
                    allCountList.Sort((left, rigth) =>
                    {
                        if (left[orderBy].Value<double>() < rigth[orderBy].Value<double>())
                        {
                            return 1;
                        }
                        if (left[orderBy].Value<double>() == rigth[orderBy].Value<double>())
                        {
                            return 0;
                        }
                        return -1;
                    }
                    );
                }
                #endregion
            }
            //输出数据
            string resultJson = JsonConvert.SerializeObject(allCountList);// JsonHelp.ToJSONjss(allCountList);
            resultJson = string.Format("{{\"List\":{0},\"Date\":{1}}}", resultJson, dateJson);
            Response.Write(resultJson.MvcResponseJson());
            Response.End();
            return null;
        }
        //缓存
        private List<get_案例信息_获取时间段内城市网站的爬取个数Result> GetCaseSpiderCountInfoListByDays_Cache(DateTime startDate, DateTime endDate)
        {
            List<get_案例信息_获取时间段内城市网站的爬取个数Result> list = new List<get_案例信息_获取时间段内城市网站的爬取个数Result>();
            string sbCacheKey = new StringBuilder().Append("GetCaseSpiderCountInfoListByDays_Cache_")
                .Append(startDate.ToString("yyyy-MM-dd")).Append("_")
                .Append(endDate.ToString("yyyy-MM-dd")).ToString();
            if (null == HttpRuntime.Cache[sbCacheKey])
            {
                list = CaseManager.GetCaseSpiderCountInfoByDate(startDate, endDate);
                string resultJson = JsonHelp.ToJSONjss(list.EncodeField<get_案例信息_获取时间段内城市网站的爬取个数Result>());
                HttpRuntime.Cache.Add(sbCacheKey, resultJson, null, DateTime.Now.AddDays(7), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            else
            {
                string resultJson = Convert.ToString(HttpRuntime.Cache[sbCacheKey]);
                if (!string.IsNullOrEmpty(resultJson))
                {
                    list = JsonHelp.ParseJSONList<get_案例信息_获取时间段内城市网站的爬取个数Result>(resultJson);
                    list = list.DecodeField<get_案例信息_获取时间段内城市网站的爬取个数Result>();
                }
            }
            return list;
        }
        #endregion

        #region (CaseImportManager.cshtml)
        public ActionResult CaseImportManager()
        {            
            //List<城市表> cityList = CityManager.GetAllCity();
            List<网站表> webList = WebsiteManager.GetAllWebsite();
            //List<省份表> provinceList = ProvinceManager.所有省份;
            List<SysData_用途> purposeList = 用途Manager.GetExistsPurpose();
            List<SysData_案例类型> caseTypeList = 案例类型Manager.所有案例类型;
            List<SysData_结构> structureList = 结构Manager.所有结构;
            List<SysData_建筑类型> buildingTypeList = 建筑类型Manager.所有建筑类型;
            List<SysData_户型> houseTypeList = 户型Manager.所有户型;
            List<SysData_朝向> frontList = 朝向Manager.所有朝向;
            List<SysData_币种> moneyUnitList = 币种Manager.所有币种;
            List<SysData_装修> zhuangxiuList = 装修Manager.GetAll();

            //ViewBag.Citylist = cityList;
            ViewBag.WebList = webList;
            //ViewBag.ProvinceList = provinceList;

            ViewBag.purposeList = purposeList;
            ViewBag.caseTypeList = caseTypeList;
            ViewBag.structureList = structureList;
            ViewBag.buildingTypeList = buildingTypeList;
            ViewBag.houseTypeList = houseTypeList;
            ViewBag.frontList = frontList;
            ViewBag.moneyUnitList = moneyUnitList;
            ViewBag.zhuangxiuList = zhuangxiuList;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="webIdsStr"></param>
        /// <param name="purposeIdsStr"></param>
        /// <param name="area"></param>
        /// <param name="projectName"></param>
        /// <param name="startCreateDate"></param>
        /// <param name="endCreateDate"></param>
        /// <param name="startCaseDate"></param>
        /// <param name="endCaseDate"></param>
        /// <param name="startImportDate"></param>
        /// <param name="endImportDate"></param>
        /// <param name="isImport"></param>
        /// <param name="isGetCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseImportManager_Search_Api(string cityId, string webIdsStr, string purposeIdsStr, string area, string projectName,
           string startCreateDate, string endCreateDate, string startCaseDate, string endCaseDate, string startImportDate, string endImportDate, 
            string isImport,string isGetCount,string pageIndex,string pageSize)
        {
            //ProjectCaseCountManager.test();
            //return null;
            int count=0;
            bool isGetCount2 = false;
            int[] webIds = null; int[] 用途IDs = null; bool? 是否成功入库 = null;
            bool? 是否为整理入库时过滤掉的信息 = null; //string 行政区 = null; string 楼盘名 = null;
            DateTime? 创建时间start = null; DateTime? 创建时间end = null;
            DateTime? 案例时间start = null; DateTime? 案例时间end = null;
            DateTime? 入库整理时间start = null; DateTime? 入库整理时间end = null;
            if (!StringHelp.IsInteger(cityId))
            {
                cityId ="0";
            }
            //网站ID
            if (!string.IsNullOrEmpty(webIdsStr))
            {
                string[] strings = webIdsStr.Split(',');
                webIds = strings.ConvertToInts();
            }
            //用途
            if (!string.IsNullOrEmpty(purposeIdsStr))
            {
                string[] strings = purposeIdsStr.Split(',');
                用途IDs = strings.ConvertToInts();
            }
            //是否为未入库案例
            if (!string.IsNullOrEmpty(isImport))
            {
                if (isImport.Equals("1"))
                {
                    是否成功入库 = true;
                    //导入时间
                    if (StringHelp.CheckStrIsDate(startImportDate) && StringHelp.CheckStrIsDate(endImportDate))
                    {
                        入库整理时间start = Convert.ToDateTime(Convert.ToDateTime(startImportDate).ToString("yyyy-MM-dd") + " 00:00:00");
                        入库整理时间end = Convert.ToDateTime(Convert.ToDateTime(endImportDate).ToString("yyyy-MM-dd") + " 23:59:59");
                    }
                }
                else
                {
                    是否为整理入库时过滤掉的信息 = true;
                }
            }
            //创建时间
            if (StringHelp.CheckStrIsDate(startCreateDate) && StringHelp.CheckStrIsDate(endCreateDate))
            {
                创建时间start = Convert.ToDateTime(Convert.ToDateTime(startCreateDate).ToString("yyyy-MM-dd") + " 00:00:00");
                创建时间end = Convert.ToDateTime(Convert.ToDateTime(endCreateDate).ToString("yyyy-MM-dd") + " 23:59:59");
            }
            //案例时间
            if (StringHelp.CheckStrIsDate(startCaseDate) && StringHelp.CheckStrIsDate(endCaseDate))
            {
                案例时间start = Convert.ToDateTime(Convert.ToDateTime(startCaseDate).ToString("yyyy-MM-dd") + " 00:00:00");
                案例时间end = Convert.ToDateTime(Convert.ToDateTime(endCaseDate).ToString("yyyy-MM-dd") + " 23:59:59");
            }
            //页码
            if (!StringHelp.IsInteger(pageIndex))
            {
                pageIndex = "1";
            }
            //页长度
            if (!StringHelp.IsInteger(pageSize))
            {
                pageSize = "20";
            }
            //是否获取总个数
            if (!string.IsNullOrEmpty(isGetCount) && isGetCount.Equals("1"))
            {
                isGetCount2 = true;
            }
            //检索行政区
            long[] areaIds = null;
            if (!string.IsNullOrEmpty(area))
            {
                SysData_Area areaObj = AreaManager.GetAreaByAreaNameLikeByCityId(area, Convert.ToInt32(cityId));
                if (areaObj != null)
                {
                    areaIds = new long[] { areaObj.ID };
                }
            }
            //检索楼盘名
            long[] projectIds = null;
            if (!string.IsNullOrEmpty(projectName))
            {
                List<SysData_Project> projectObj = ProjectManager.GetProjectByProjectNameLikeAndCityId(projectName, Convert.ToInt32(cityId), 1);
                if (projectObj != null && projectObj.Count > 0)
                {
                    projectIds = new long[] { projectObj[0].ID };
                }
            }

            List<VIEW_案例信息_城市表_网站表2> list = CaseManager.GetVIEW案例信息2_根据高级查询条件(Convert.ToInt32(cityId), Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), isGetCount2, out count,
            webIds: webIds, 用途IDs: 用途IDs, 是否成功入库: 是否成功入库, 是否为整理入库时过滤掉的信息: 是否为整理入库时过滤掉的信息,
            行政区IDs: areaIds, 楼盘IDs: projectIds, 创建时间start: 创建时间start, 创建时间end: 创建时间end,
            案例时间start: 案例时间start, 案例时间end: 案例时间end, 入库整理时间start: 入库整理时间start, 入库整理时间end: 入库整理时间end);
            string listJson = list.Web_GetJson();
            string resultJson = new StringBuilder()
                    .Append("{\"List\":").Append(string.IsNullOrEmpty(listJson) ? "null" : listJson)
                    .Append(",\"Count\":").Append(count)
                    .Append(",\"IsGetCount\":").Append(isGetCount2 ? 1 : 0)
                    .Append(",\"IsImport\":").Append(Convert.ToBoolean(是否成功入库) ? 1 : 0)
                    .Append("}").ToString();
                    
            Response.Write(resultJson.MvcResponseJson());
            Response.End();
            return null;
        }
        /// <summary>
        /// 根据城市ID and 多个案例ID获取案例信息和已入库楼盘
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtIds"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseImportManager_GetFxtCaseProjectName_Api(int cityId, string fxtIds)
        {
            int[] _fxtIds = null;
            string cityName = "";
            if (!string.IsNullOrEmpty(fxtIds))
            {
                string[] strings = fxtIds.TrimEnd(',').Split(',');
                _fxtIds = strings.ConvertToInts();
            }
            城市表 city = CityManager.GetCityById(cityId);
            if (city != null)
            {
                cityName = city.城市名称;
            }
            Dictionary<int, string> dic = CaseApi.GetCaseIdJoinProjectNameByCityNameAndCaseIds(cityName, _fxtIds);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (_fxtIds != null && dic != null)
            {
                string str = "{{\"FxtId\":\"{0}\",\"ProjectName\":\"{1}\"}}";
                foreach (int fxtId in _fxtIds)
                {
                    string projectName = "";
                    if (dic.ContainsKey(fxtId))
                    {
                        projectName = dic[fxtId].EncodeField();
                    }
                    sb.Append(string.Format(str, fxtId, projectName)).Append(",");
                }
            }
            string resultJson = sb.ToString().TrimEnd(',') + "]";
            Response.Write(resultJson.MvcResponseJson());
            Response.End();
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="projectNamesStr"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseImportManager_GetFxtCaseProjectNameByNowProjectName_Api(string cityName, string projectNamesStr)
        {
            string[] projectNames = null;
            cityName = cityName.DecodeField();            
            if (!string.IsNullOrEmpty(projectNamesStr))
            {
                projectNamesStr = projectNamesStr.TrimEnd(',');
                projectNames = projectNamesStr.Split(',');
                for (int i = 0; i < projectNames.Length; i++)
                {
                    projectNames[i] = projectNames[i].DecodeField();
                }
            }
            IDictionary<string, string> dic = ProjectApiManager.GetNowProjectNameJoinFxtProjectName(projectNames, cityName);
            StringBuilder sb = new StringBuilder("[");
            if (dic != null)
            {                
                IEnumerator<KeyValuePair<string, string>> e = dic.GetEnumerator();
                while (e.MoveNext())
                {
                    sb.Append("{\"projectName\":\"").Append(e.Current.Key.EncodeField()).Append("\",")
                      .Append("\"fxtProjectName\":\"").Append(e.Current.Value.EncodeField()).Append("\"")
                      .Append("},");
                }
            }
            string resultJson = sb.ToString().TrimEnd(',') + "]";
            Response.Write(resultJson.MvcResponseJson());
            Response.End();
            return null;
        }
        /// <summary>
        /// 跟新单元格各列信息
        /// </summary>
        /// <param name="caseIds">要修改的案例ID</param>
        /// <param name="columnValue">更改值</param>
        /// <param name="columnName">更改列</param>
        /// <param name="nowValue">更改前的值</param>
        /// <param name="cityName"></param>
        /// <param name="areaName"></param>
        /// <param name="submCount">all:修改nowValue+cityName+areaName条件下的所有,now:修改指定案例ID的信息</param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]        
        public ActionResult CaseImportManager_UpdateColumnInfo_Api(string caseIds, string columnValue, string columnName,string nowValue,string cityName,string areaName,string submCount)
        {
            string message="";
            int result = 1;
            if (string.IsNullOrEmpty(caseIds))
            {
                string str = WebJsonHelp.MvcResponseJson("", result: 0, errorType: WebUserHelp.SysError.ToString(), message: "参数错误");
                Response.Write(str);
                Response.End();
                return null;
            }
            string[] strings = caseIds.TrimEnd(',').Split(',');
            long[] longs = strings.ConvertToLongs();
            if (longs == null || longs.Length < 1)
            {
                string str = WebJsonHelp.MvcResponseJson("", result: 0, errorType: WebUserHelp.SysError.ToString(), message: "参数错误");
                Response.Write(str);
                Response.End();
                return null;
            }
            columnName = columnName.DecodeField();
            columnValue = columnValue.DecodeField();
            cityName = cityName.DecodeField();
            nowValue = nowValue.DecodeField();
            areaName = areaName.DecodeField();
            城市表 cityObj = CityManager.GetCityByCityName(cityName);
            if (cityObj == null)
            {
                string str = WebJsonHelp.MvcResponseJson("", result: 0, errorType: WebUserHelp.SysError.ToString(), message: "城市不存在");
                Response.Write(str);
                Response.End();
                return null;
            }
            List<案例信息> list = new List<案例信息>();
            List<VIEW_案例信息_城市表_网站表> vList = new List<VIEW_案例信息_城市表_网站表>();
            if (submCount != null && submCount.Equals("all"))
            {
                bool allResult = false;
                SysData_Project project = ProjectManager.GetProjectByProjectNameAndCityId(nowValue, cityObj.ID);
                if (project == null)
                {
                    message = "楼盘不存在";
                    goto End;
                }
                #region 批量更新
                SysData_Area areaObj = null;
                if (!string.IsNullOrEmpty(areaName))
                {
                    areaObj = AreaManager.GetAreaByAreaNameByCityId(areaName, cityObj.ID);
                }
                switch (columnName)
                {
                    case "loupan":
                        //list = CaseManager.GetCaseByProjectNameAndCityNameAndLikeAreaName(nowValue, cityName, areaName);
                        allResult = CaseManager.UpdateImportFailCaseProjectBy(columnValue, cityObj.ID, project.ID, areaObj == null ? 0 : areaObj.ID, out message);
                        break;
                    default:
                        break;
                }
                End:
                if (!allResult)
                {
                    result = 0;
                }
                #endregion

            }
            else
            {
                list = CaseManager.GetCaseByIds(longs);
                vList = CaseManager.GetCaseViewByIds(longs);
                if (list == null || list.Count < 1)
                {
                    string str = WebJsonHelp.MvcResponseJson("", result: 0, errorType: WebUserHelp.SysError.ToString(), message: "次信息不存在或已被其他用户删除");
                    Response.Write(str);
                    Response.End();
                    return null;
                }
                if (!string.IsNullOrEmpty(columnName))
                {
                    if (!columnName.Equals("fxtloupan"))
                    {
                        List<案例信息> addList = new List<案例信息>();
                        List<案例信息> upList = new List<案例信息>();
                        Dictionary<long, long> addDic = new Dictionary<long, long>();
                        int? id = null;
                        #region (判断修改字段)
                        switch (columnName)
                        {
                            case "xingzhengqu":
                                long areaId = 0;
                                columnValue = columnValue.TrimBlank();
                                if (!string.IsNullOrEmpty(columnValue))
                                {
                                    SysData_Area area = AreaManager.GetLikeOrInsertArea(columnValue, cityObj.ID);
                                    if (area != null)
                                    {
                                        areaId = area.ID;
                                    }
                                }
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    if (_case.ProjectId != null)
                                    {
                                        upList.Add(new 案例信息 { ProjectId = _case.ProjectId, AreaId = Convert.ToInt64(_case.AreaId) });
                                        addList.Add(new 案例信息 { ProjectId = _case.ProjectId, AreaId = areaId });
                                    }
                                    _case.行政区 = null;
                                    _case.AreaId = areaId;
                                });
                                break;
                            case "pianqu":
                                long? subAreaId = null;
                                columnValue = columnValue.TrimBlank();
                                if (!string.IsNullOrEmpty(columnValue))
                                {
                                    SysData_SubArea subArea = SubAreaManager.GetLikeOrInsertArea(columnValue, cityObj.ID);
                                    if (subArea != null)
                                    {
                                        subAreaId = subArea.ID;
                                    }
                                }
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.片区 = null;
                                    _case.SubAreaId = subAreaId;
                                });
                                break;
                            case "anlishijian":
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.案例时间 = Convert.ToDateTime(columnValue);
                                });
                                break;
                            case "yongtu":
                                id = 用途Manager.Get用途_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.用途 = null;
                                    _case.用途ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            case "mianji":
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.面积 = CaseManager.CaseColumnConvertToDecimal(columnValue);
                                });
                                break;
                            case "danjia":
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.单价 = CaseManager.CaseColumnConvertToDecimal(columnValue);
                                });
                                break;
                            case "zongjia":
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.总价 = CaseManager.CaseColumnConvertToDecimal(columnValue);
                                });
                                break;
                            case "anlileixing":
                                id = 建筑类型Manager.Get建筑类型_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.案例类型 = null;
                                    _case.案例类型ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            case "jiegou":
                                id = 结构Manager.Get结构_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.结构 = null;
                                    _case.结构ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            case "jianzhuleixing":
                                id = 建筑类型Manager.Get建筑类型_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.建筑类型 = null;
                                    _case.建筑类型ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            case "zonglouceng":
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.总楼层 = CaseManager.CaseColumnConvertToInt(columnValue);
                                });
                                break;
                            case "suozailouceng":
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.所在楼层 = CaseManager.CaseColumnConvertToInt(columnValue);
                                });
                                break;
                            case "huxing":
                                id = 户型Manager.Get户型_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.户型 = null;
                                    _case.户型ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            case "chaoxiang":
                                id = 朝向Manager.Get朝向_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.朝向 = null;
                                    _case.朝向ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            case "bizhong":
                                id = 币种Manager.Get币种_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.币种 = null;
                                    _case.币种ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            case "niandai":
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.建筑年代 = columnValue;
                                });
                                break;
                            case "zhuangxiu":
                                id = 装修Manager.Get装修ID_根据名称(columnValue);
                                list.ForEach(delegate(案例信息 _case)
                                {
                                    _case.装修 = null;
                                    _case.装修ID = Convert.ToInt32(id) == 0 ? null : id;
                                });
                                break;
                            default:
                                break;
                        }
                        #endregion
                        CaseManager.Update(list);
                        foreach (案例信息 caseObj in upList)
                        {
                            ProjectCaseCountManager.UpdateNotImportCaseCount(Convert.ToInt64(caseObj.ProjectId), Convert.ToInt64(caseObj.AreaId), -1);
                        }
                        foreach (案例信息 caseObj in addList)
                        {
                            ProjectCaseCountManager.UpdateNotImportCaseCount(Convert.ToInt64(caseObj.ProjectId), Convert.ToInt64(caseObj.AreaId), 1);
                        }
                        
                    }
                    else
                    {
                        bool result2 = ProjectMatchApiManager.InsertProjectMatchByCaseListAndProjectNameAndCityName(vList, columnValue, cityName, out message);
                        if (!result2)
                        {
                            result = 0;
                        }

                    }

                }
            }
            EndResult:
            string resultJson = WebJsonHelp.MvcResponseJson("", result: result, message: message);
            Response.Write(resultJson);
            Response.End();

            return null;
        }
        /// <summary>
        /// 将案例导入到正式库
        /// </summary>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseImportManager_ImportCaseByIds_Api(string caseIds)
        {
            string data = "{{\"upCount\":{0},\"succeedCount\":{1},\"failId\":\"{2}\"}}";
            string message = "";
            int result = 1;
            if (string.IsNullOrEmpty(caseIds))
            {
                string str = WebJsonHelp.MvcResponseJson(string.Format(data,0,0,""), result: 0, errorType: WebUserHelp.SysError.ToString(), message: "参数错误");
                Response.Write(str);
                Response.End();
                return null;
            }
            string[] strings = caseIds.TrimEnd(',').Split(',');
            long[] longs = strings.ConvertToLongs();
            if (longs == null || longs.Length < 1)
            {
                string str = WebJsonHelp.MvcResponseJson(string.Format(data, 0, 0, ""), result: 0, errorType: WebUserHelp.SysError.ToString(), message: "参数错误");
                Response.Write(str);
                Response.End();
                return null;
            }
            List<VIEW_案例信息_城市表_网站表> list = CaseManager.GetCaseViewByIds(longs);
            if (list == null || list.Count < 1)
            {
                string str = WebJsonHelp.MvcResponseJson(string.Format(data, 0, 0, ""), result: 0, errorType: WebUserHelp.SysError.ToString(), message: "此信息不存在或已被其他用户删除");
                Response.Write(str);
                Response.End();
                return null;
            }
            List<VIEW_案例信息_城市表_网站表> list2 = new List<VIEW_案例信息_城市表_网站表>();
            bool result2 = CaseApiManager.ImportCase(list, out list2, out message);
            if (!result2) { result = 0; }
            int upCount = list == null ? 0 : list.Count;
            int succeedCount = upCount - (list2 == null ? 0 : list2.Count);
            StringBuilder failId = new StringBuilder("");
            if (list2 != null)
            {
                list2.ForEach(delegate(VIEW_案例信息_城市表_网站表 obj)
                {
                    failId.Append(obj.ID).Append(",");
                });
            }
            string dataJson = string.Format("{{\"upCount\":{0},\"succeedCount\":{1},\"failId\":\"{2}\"}}", upCount, succeedCount, failId.ToString().TrimEnd(','));
            string resultJson = WebJsonHelp.MvcResponseJson(dataJson, result: result, message: message);
            Response.Write(resultJson);
            Response.End();

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseImportManager_DeleteCaseByIds_Api(string caseIds)
        {
            string message = "";
            int result = 1;
            if (string.IsNullOrEmpty(caseIds))
            {
                string str = WebJsonHelp.MvcResponseJson("", result: 0, errorType: WebUserHelp.SysError.ToString(), message: "参数错误");
                Response.Write(str);
                Response.End();
                return null;
            }
            string[] strings = caseIds.TrimEnd(',').Split(',');
            long[] longs = strings.ConvertToLongs();
            if (longs == null || longs.Length < 1)
            {
                string str = WebJsonHelp.MvcResponseJson("", result: 0, errorType: WebUserHelp.SysError.ToString(), message: "参数错误");
                Response.Write(str);
                Response.End();
                return null;
            }
            List<VIEW_案例信息_城市表_网站表> list2 = new List<VIEW_案例信息_城市表_网站表>();
            bool result2 = CaseManager.DeleteCaseByIds(longs,out message);
            if (!result2) { result = 0; }

            string resultJson = WebJsonHelp.MvcResponseJson("", result: result, message: message);
            Response.Write(resultJson);
            Response.End();

            return null;
        }

        #endregion


        #region (CaseSpiderErrorList)
        public ActionResult CaseSpiderErrorList(int cityId, int webId, string startDate, string endDate)
        {
            startDate = startDate.DecodeField();
            endDate = endDate.DecodeField();
            城市表 city = CityManager.GetCityById(cityId);
            网站表 web = WebsiteManager.GetWebById(webId);
            List<SYS_Code> codeList = SysCodeManager.GetAllSpiderErrorCode();
            ViewBag.CityId = city == null ? 0 : cityId;
            ViewBag.CityName = city == null ? "" : city.城市名称;
            ViewBag.WebId = web == null ? 0 : webId;
            ViewBag.WebName = web == null ? "" : web.网站名称;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.NetWorkErrorCount = 0;
            ViewBag.RegexErrorCount = 0;
            ViewBag.VCodeErrorCount = 0;
            ViewBag.ErrorCodeList = codeList;
            if (startDate.CheckStrIsDate() && endDate.CheckStrIsDate())
            {
                ViewBag.NetWorkErrorCount = DatSpiderErrorLogManager.GetDatSpiderErrorLogCountByWebIdAndCityIdAndErrorCodeAndDate(cityId, webId, SysCodeManager.Code_1_1, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                ViewBag.RegexErrorCount = DatSpiderErrorLogManager.GetDatSpiderErrorLogCountByWebIdAndCityIdAndErrorCodeAndDate(cityId, webId, SysCodeManager.Code_1_2, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                ViewBag.VCodeErrorCount = DatSpiderErrorLogManager.GetDatSpiderErrorLogCountByWebIdAndCityIdAndErrorCodeAndDate(cityId, webId, SysCodeManager.Code_1_3, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
            }
            ViewBag.ErrorCount = Convert.ToInt32(ViewBag.NetWorkErrorCount) + Convert.ToInt32(ViewBag.RegexErrorCount) + Convert.ToInt32(ViewBag.VCodeErrorCount);
            return View();
        }
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseSpiderErrorListByAndCityAndWebAndDate_Api(int cityId, int webId, string startDate, string endDate, int pageIndex, int pageSize, int isGetCount)
        {            
            int count=0;
            bool isGetCount2 = false;
            string dateJson = "{{\"List\":{0},\"Count\":{1}}}";
            startDate = startDate.DecodeField();
            endDate = endDate.DecodeField();
            if (!startDate.CheckStrIsDate() || !endDate.CheckStrIsDate())
            {
                dateJson = string.Format(dateJson, "null", count).MvcResponseJson();
                Response.Write(dateJson);
                Response.End();
                return null;
            }
            //是否获取总个数
            if (isGetCount == 1)
            {
                isGetCount2 = true;
            }
            DateTime _startDate = Convert.ToDateTime(startDate);
            DateTime _endDate = Convert.ToDateTime(endDate);
            List<Dat_SpiderErrorLog> list = DatSpiderErrorLogManager.GetDatSpiderErrorLogByWebIdAndCityIdAndDate(cityId, webId, _startDate, _endDate, pageIndex, pageSize, isGetCount2, out count);
            dateJson = string.Format(dateJson, list == null ? "null" : list.ToJSONjss(), count).MvcResponseJson();
            Response.Write(dateJson);
            Response.End();
            return null;

        }
        #endregion

        #region (ProxyIpManager.cshtml)
        public ActionResult ProxyIpManager()
        {
            List<网站表> webList = WebsiteManager.GetAllWebsite();
            ViewBag.WebList = webList;
            ViewBag.Default = new int[] { WebsiteManager.赶集网_ID, WebsiteManager.搜房网_ID };
            return View();
        }
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult ProxyIpManagerGetList_Api(int pageIndex, int pageSize, int isGetCount)
        {
            int count = 0;
            bool isGetCount2 = false;
            string dateJson = "{{\"List\":{0},\"Count\":{1}}}";
            //是否获取总个数
            if (isGetCount == 1)
            {
                isGetCount2 = true;
            }
            List<View_WebJoinProxyIp> list = WebJoinProxyIpManager.GetAllViewWebJoinProxyIp(pageIndex, pageSize, isGetCount2, out count);
            dateJson = string.Format(dateJson, list == null ? "null" : list.ToJSONjss(), count).MvcResponseJson();
            Response.Write(dateJson);
            Response.End();
            return null;
        }
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult ProxyIpManagerAddWebProxyIp_Api(string ip,string ipArea, string webIds)
        {
            ip=ip.TrimBlank();
            if (!CheckProxyIp(ip))
            {
                Response.Write("".MvcResponseJson(result: 0, message: "无效的ip"));
                Response.End();
                return null;   
            }
            if (string.IsNullOrEmpty(ipArea.TrimBlank()))
            {
                Response.Write("".MvcResponseJson(result: 0, message: "请填写ip所在地区"));
                Response.End();
                return null;   
            }
            int[] _webIds = webIds.ConvertToInts(',');
            if (_webIds == null || _webIds.Length < 1)
            {
                Response.Write("".MvcResponseJson(result: 0, message: "请选择要分配的网站"));
                Response.End();
                return null;   
            }
            List<SysData_WebJoinProxyIp> _addList=new List<SysData_WebJoinProxyIp> ();
            string message = "";
            int result = WebJoinProxyIpManager.InsertWebJoinProxyIp(ip, ipArea, _webIds, out _addList, out message);
            if (result != 1)
            {
                Response.Write("".MvcResponseJson(result: 0, message: message));
                Response.End();
                return null;   
            }
            List<long> longs = new List<long>();
            foreach (SysData_WebJoinProxyIp obj in _addList)
            {
                longs.Add(obj.ID);
            }
            List<View_WebJoinProxyIp> addList = WebJoinProxyIpManager.GetViewWebJoinProxyIp(longs.ToArray());
            string jsonData = addList.ToJSONjss();
            Response.Write(jsonData.MvcResponseJson());
            Response.End();
            return null;    
        }
        #endregion

        #region 功能方法
        /// <summary>
        /// 检查代理ip是否可用
        /// </summary>
        /// <param name="proxyIp"></param>
        /// <returns></returns>
        bool CheckProxyIp(string proxyIp)
        {
            if (string.IsNullOrEmpty(proxyIp))
            {
                return false;
            }
            proxyIp = proxyIp.ToLower().Replace("http://", "");
            if (string.IsNullOrEmpty(proxyIp))
            {
                return false;
            }
            proxyIp = "http://" + proxyIp;
            try
            {
                WebProxy wp = new WebProxy(new Uri(proxyIp));
                HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri("http://www.baidu.com", false));
                request.Proxy = wp;
                request.Method = "get";
                request.Timeout = 6000;                
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        #endregion
    }
}
