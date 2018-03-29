using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using FxtDataAcquisition.Web.Common;
using FxtDataAcquisition.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FxtDataAcquisition.Web.Controllers
{
    public class SharedController : Controller
    {
        private readonly IAdminService _unitOfWork;

        public SharedController(IAdminService unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        //
        // GET: /Shared/

        public ActionResult Partial_SelectCity(string callbackUrl)
        {
            var userInfo = WebUserHelp.GetNowLoginUser();
            //省份
            //List<FxtApi_SYSProvince> provinceList = DataCenterProvinceApi.GetProvinceAll(userInfo.UserName, userInfo.SignName, userInfo.AppList);
            List<FxtApi_SYSProvince> provinceList = _unitOfWork.CityService.GetProvinceCityListBy(userInfo.UserName, userInfo.SignName, userInfo.AppList);
            ////城市
            //List<FxtApi_SYSCity> cityList = DataCenterCityApi.GetCityAll(userInfo.UserName, userInfo.SignName, userInfo.AppList);
            ////获取当前公司开通产品城市ID
            //int[] cityIds = UserCenterUserInfoApi.GetCompanyProductCityIds(userInfo.SignName, FxtAPI.FxtUserCenter.Common.systypeCode, userInfo.UserName, userInfo.SignName, userInfo.AppList);
            ////所有城市
            //if (!cityIds.Contains(0))
            //{
            //    cityList = cityList.Where(m => cityIds.Contains(m.CityId)).ToList();
            //    provinceList = provinceList.Where(m => cityList.Any(c => m.ProvinceId == c.ProvinceId)).ToList();
            //}

            ViewBag.NowProvinceList = provinceList;


            //List<FxtApi_SYSProvince> provinceList = new List<FxtApi_SYSProvince>();
            //List<FxtApi_SYSCity> cityList = new List<FxtApi_SYSCity>();
            ////获取当前拥有产品权限的城市 并输出到页面
            //WebUserHelp.GetNowRightCityList(out provinceList, out cityList);
            //StringBuilder sb = new StringBuilder();
            //sb.Append("[");
            //foreach (FxtApi_SYSProvince pro in provinceList)
            //{
            //    sb.Append("{\"provinceid\":").Append(pro.ProvinceId).Append(",");
            //    sb.Append("\"provincename\":\"").Append(pro.ProvinceName).Append("\",");
            //    List<FxtApi_SYSCity> _cityList = cityList.Where(obj => obj.ProvinceId == pro.ProvinceId).ToList();
            //    sb.Append("\"citylist\":[");
            //    StringBuilder sb2 = new StringBuilder();
            //    foreach (FxtApi_SYSCity city in _cityList)
            //    {
            //        sb2.Append("{\"cityid\":").Append(city.CityId).Append(",");
            //        sb2.Append("\"cityname\":\"").Append(city.CityName).Append("\",");
            //        sb2.Append("\"provinceid\":").Append(pro.ProvinceId).Append("},");
            //    }
            //    sb.Append(sb2.ToString().TrimEnd(',')).Append("]");
            //    sb.Append("},");
            //}
            //string result = sb.ToString().TrimEnd(',') + "]";
            //ViewBag.RightCityData = result;
            //ViewBag.NowCityId = WebUserHelp.GetNowCityId();
            //ViewBag.NowProvinceId = WebUserHelp.GetNowProvinceId();
            ViewBag.CallbackUrl = callbackUrl;
            return PartialView();
        }
        /// <summary>
        /// 选择城市弹出框
        /// </summary>
        /// <returns></returns>
        public ActionResult Open_SelectCity()
        {
            return View();
        }
        public ActionResult Partial_Menu()
        {
            List<WebMenuClass> list = WebUserHelp.GetNowLoginMenu();
            ViewBag.Menus = list;
            return PartialView();
        }
        public ActionResult Partial_UserInfo()
        {
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            ViewBag.UserName = loginUserInfo.UserName;
            if (!string.IsNullOrEmpty(loginUserInfo.TrueName))
            {
                ViewBag.UserName = loginUserInfo.TrueName;
            }
            return PartialView();
        }

    }
}
