using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpider.FxtApi.Model;
using FxtSpider.Bll.FxtApiManager;
using FxtSpider.FxtApi.Common;
using FxtSpider.Bll;
using System.Text;
using FxtSpider.Common;
using FxtSpider.FxtApi.ApiManager;
using FxtSpider.Manager.Common;
using Newtonsoft.Json.Linq;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Manager.Web.Controllers
{
    public class CommonController : Controller
    {

        /// <summary>
        /// 根据城市名称获取fxtapi中的行政区
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public ActionResult GetFxtSysAreaByCityName_Api(string cityName)
        {
            cityName = JsonHelp.DecodeField(cityName);
            List<FxtApi_SYSArea> list = AreaApiManager.GetAreaByCityName(cityName);
            list = list.EncodeField<FxtApi_SYSArea>();
            string jsonStr = list.ToJSONjss().MvcResponseJson(1, "", "");
            Response.Write(jsonStr);
            Response.End();
            return null;
        }
        /// <summary>
        /// 根据城市ID获取fxtapi中的行政区
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ActionResult GetFxtSysAreaByCityId_Api(int cityId)
        {
            List<FxtApi_SYSArea> list = AreaApiManager.GetAreaByCityId(cityId);
            list = list.EncodeField<FxtApi_SYSArea>();
            string jsonStr = list.ToJSONjss().MvcResponseJson(1, "", ""); ;
            Response.Write(jsonStr);
            Response.End();
            return null;
        }
        /// <summary>
        /// 根据省份ID获取城市信息
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public ActionResult GetFxtSysCityByProvinceId(int provinceId)
        {
            List<FxtApi_SYSCity> list = CityApi.GetCityByProvinceId(provinceId);
            list = list.EncodeField<FxtApi_SYSCity>();
            string jsonStr = list.ToJSONjss().MvcResponseJson(1, "", ""); ;
            Response.Write(jsonStr);
            Response.End();
            return null;
        }
        /// <summary>
        /// 获取楼盘名模糊下拉(爬取库楼盘)
        /// </summary>
        /// <param name="likeName"></param>
        /// <returns></returns>
        public ActionResult GetProjectNameByLike_Api()
        {
            string q = Request["q"];
            string limit = Request["limit"];
            int? cityId = null;
            int maxCount = 10;
            if (!string.IsNullOrEmpty(limit) && StringHelp.IsInteger(limit))
            {
                maxCount = Convert.ToInt32(limit);
            }
            if (StringHelp.IsInteger(Request["cityId"]))
            {
                cityId = Convert.ToInt32(Request["cityId"]);
            }
            if (maxCount > 100)
            {
                maxCount = 100;
            }


            string[] strings = CaseManager.GetProjectInfoStringsByLike(q, maxCount, cityId: cityId);

            StringBuilder sb = new StringBuilder("");
            if (strings != null)
            {
                foreach (string str in strings)
                {
                    sb.Append(str).Append("\n");
                }
            }

            string result = sb.ToString();
            Response.Write(result);
            Response.End();
            return null;
        }
        /// <summary>
        /// 获取楼盘名模糊下拉(正式库楼盘)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFxtProjectByLikeAndCityName_Api()
        {
            string q = Request["q"];
            string limit = Request["limit"];
            string cityName = null;
            int maxCount = 10;
            if (!string.IsNullOrEmpty(limit) && StringHelp.IsInteger(limit))
            {
                maxCount = Convert.ToInt32(limit);
            }
            if (Request["cityName"]!=null)
            {
                cityName = Convert.ToString(Request["cityName"]).DecodeField();
            }
            if (maxCount > 100)
            {
                maxCount = 100;
            }
            string[] strings = ProjectApiManager.GetProjectInfoStringsByLikeNameAndCityName(q, cityName, maxCount);
            StringBuilder sb = new StringBuilder("");
            if (strings != null)
            {
                foreach (string str in strings)
                {
                    sb.Append(str).Append("\n");
                }
            }
            string result = sb.ToString();
            Response.Write(result);
            Response.End();
            return null;
        }
        /// <summary>
        /// 模糊获取片区
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSubAreaByLikeAndCityId_Api()
        {

            string q = Request["q"];
            string limit = Request["limit"];
            int cityId = 0;
            int maxCount = 10;
            if (!string.IsNullOrEmpty(limit) && StringHelp.IsInteger(limit))
            {
                maxCount = Convert.ToInt32(limit);
            }
            if (Request["cityId"] != null&&StringHelp.IsInteger(Request["cityId"]))
            {
                cityId = Convert.ToInt32(Request["cityId"]);
            }
            if (maxCount > 100)
            {
                maxCount = 100;
            }
            List<SysData_SubArea> list = SubAreaManager.GetSubAreaByLikeAndCityId(q, cityId, maxCount);
            StringBuilder sb = new StringBuilder("");
            if (list != null)
            {
                foreach (SysData_SubArea subArea in list)
                {
                    sb.Append(subArea.SubAreaName).Append("\n");
                }
            }
            string result = sb.ToString();
            Response.Write(result);
            Response.End();
            return null;
        }
    }
}
