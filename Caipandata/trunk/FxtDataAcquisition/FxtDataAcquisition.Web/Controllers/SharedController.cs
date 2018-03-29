namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Web.Models;
    using FxtDataAcquisition.Application.Interfaces;
    using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
    using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;

    public class SharedController : Controller
    {
        private readonly IAdminService _unitOfWork;

        public SharedController(IAdminService unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public ActionResult Partial_SelectCity(string callbackUrl)
        {
            var userInfo = WebUserHelp.GetNowLoginUser();
            //省份
            List<FxtApi_SYSProvince> provinceList = _unitOfWork.CityService.GetProvinceCityListBy(userInfo.UserName, userInfo.SignName, userInfo.AppList);

            ViewBag.NowProvinceList = provinceList;

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
            return PartialView();
        }
        public ActionResult Partial_UserInfo()
        {
            LoginUser loginUserInfo = WebUserHelp.GetNowLoginUser();

            ViewBag.UserName = loginUserInfo.UserName;

            if (!string.IsNullOrEmpty(loginUserInfo.TrueName))
            {
                ViewBag.UserName = loginUserInfo.TrueName;
            }
            return PartialView();
        }

    }
}
