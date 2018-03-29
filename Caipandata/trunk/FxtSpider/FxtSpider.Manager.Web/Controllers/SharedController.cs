using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.ApiManager;

namespace FxtSpider.Manager.Web.Controllers
{
    public class SharedController : Controller
    {
        //
        // GET: /Shared/

        public PartialViewResult Partial_SelectCity()
        {
            List<城市表> cityList = CityManager.GetAllCity();
            List<省份表> provinceList = ProvinceManager.所有省份;
            ViewBag.Citylist = cityList;
            ViewBag.ProvinceList = provinceList;

            return PartialView();
        }
        public PartialViewResult Partial_SelectFxtCity()
        {
            List<FxtApi_SYSCity> cityList = CityApi.GetAllCity();
            List<FxtApi_SYSProvince> provinceList = ProvinceApi.GetAllProvince();
            ViewBag.Citylist = cityList;
            ViewBag.ProvinceList = provinceList;
            return PartialView();

        }

    }
}
