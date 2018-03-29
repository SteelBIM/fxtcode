namespace FxtDataAcquisition.Web.Controllers
{
    using System.Web.Mvc;
    using System.Collections.Generic;

    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Application.Interfaces;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;

    public class LoginController : BaseController
    {
        public LoginController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        #region(Login.cshtml)
        [Common.AuthorizeFilterAttribute(IsCheckLogin = false)]
        //[AllowAnonymous] 
        public ActionResult Login(string type, string path)
        {
            ViewBag.LastUserName = WebUserHelp.GetLastUserName();
            //LNKPPhotoManager.GetLNKPPhotoCountByProjectId(0, 6, 25);
            if (type == "signout")
            {
                WebUserHelp.SignOutLoginUser();

                Response.Redirect(WebCommon.Url_Login_Login);

                return null;
            }
            else if (type == "open")
            {
                ViewBag.LastPageType = "open";
            }
            if (WebUserHelp.GetNowLoginUser() != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        //[Common.AuthorizeFilterAttribute(IsCheckLogin = false)]
        //public ActionResult GetDataBase(string type, string id)
        //{
        //    if (type == "city")
        //    {
        //        List<FxtApi_SYSProvince> provinceList = SYSProvinceManager.GetAllProvince();
        //        List<FxtApi_SYSCity> cityList = SYSCityManager.GetAllCity();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("[");
        //        foreach (FxtApi_SYSProvince pro in provinceList)
        //        {
        //            sb.Append("{provinceid:").Append(pro.ProvinceId).Append(",");
        //            sb.Append("provincename:\"").Append(pro.ProvinceName).Append("\",");
        //            List<FxtApi_SYSCity> _cityList = cityList.Where(obj => obj.ProvinceId == pro.ProvinceId).ToList();
        //            sb.Append("citylist:[");
        //            StringBuilder sb2 = new StringBuilder();
        //            foreach (FxtApi_SYSCity city in _cityList)
        //            {
        //                sb2.Append("{cityid:").Append(city.CityId).Append(",");
        //                sb2.Append("cityname:\"").Append(city.CityName).Append("\",");
        //                sb2.Append("provinceid:").Append(pro.ProvinceId).Append("},");
        //            }
        //            sb.Append(sb2.ToString().TrimEnd(',')).Append("]");
        //            sb.Append("},");
        //        }
        //        string result = sb.ToString().TrimEnd(',') + "]";
        //        ViewBag.DataBase = result;
        //    }
        //    else if (type == "allotstatus")
        //    {
        //        List<SYSCode> list = SYSCodeApi.GetSYSCodeById(1035);
        //        ViewBag.DataBase = list.ToJSONjss();
        //    }
        //    if (type == "code")
        //    {
        //        int _id = Convert.ToInt32(id);
        //        List<SYSCode> list = SYSCodeApi.GetSYSCodeById(_id);
        //        ViewBag.DataBase = list.ToJSONjss();
        //    }
        //    else if (type == "cityandprovince")
        //    {
        //        List<FxtApi_SYSProvince> provinceList = SYSProvinceManager.GetAllProvince();
        //        List<FxtApi_SYSCity> cityList = SYSCityManager.GetAllCity();

        //        string json = "{{\"provinceList\":{0}, \"cityList\":{1}}}";
        //        string proJson = "[]";
        //        string cityJson = "[]";
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("[");
        //        foreach (FxtApi_SYSProvince pro in provinceList)
        //        {
        //            sb.Append("{\"provinceid\":").Append(pro.ProvinceId).Append(",");
        //            sb.Append("\"provincename\":\"").Append(pro.ProvinceName).Append("\"");
        //            sb.Append("},");
        //        }
        //        proJson = sb.ToString().TrimEnd(',') + "]";
        //        sb = new StringBuilder();
        //        sb.Append("[");
        //        foreach (FxtApi_SYSCity ci in cityList)
        //        {
        //            sb.Append("{\"cityid\":").Append(ci.CityId).Append(",");
        //            sb.Append("\"cityname\":\"").Append(ci.CityName).Append("\",");
        //            sb.Append("\"provinceid\":").Append(ci.ProvinceId).Append("");
        //            sb.Append("},");
        //        }
        //        cityJson = sb.ToString().TrimEnd(',') + "]";
        //        string result = string.Format(json, proJson, cityJson);
        //        ViewBag.DataBase = result;
        //    }
        //    return View();
        //}

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, IsCheckLogin = false)]
        public ActionResult Login_SubmitDate_Api(string userName, string pwd)
        {
            AjaxResult result = new AjaxResult("登陆成功");
            string message = "";
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwd))
            {
                result.Result = false;
                result.Message = "请填写用户名和密码";
                return AjaxJson(result);
            }
            var appList = new List<Apps>();
            var userResult = UserCenterUserInfoApi.UserLogin(userName, pwd, out appList, out message);
            if (userResult == null)
            {
                result.Result = false;
                result.Message = "用户名或密码错误";
                return AjaxJson(result);
            }

            //获取当前用户拥有产品的城市
            int nowCityId = WebUserHelp.GetNowCityId();
            string lastuser = WebUserHelp.GetLastUserName();
            //int[] ints = UserCenterUserInfoApi.GetCompanyProductCityIds(userResult.SignName, FxtAPI.FxtUserCenter.Common.systypeCode, userResult.UserName, userResult.SignName, appList);
            //WebUserHelp.SetNowRightCityList(ints);

            //IList <SYSMenu> menuList = SYSMenuManager.GetSYSMenuPageByUserNameAndCompanyIdAndCityId(userName, userResult.FxtCompanyId, nowCityId);
            WebUserHelp.SetNowLoginUser(userResult, appList, null);

            result.Data = new
            {
                userinfo = userResult,
                islastuser = userName == lastuser ? "1" : "0",
                cityid = nowCityId
            };
            return AjaxJson(result);
        }
        #endregion

        [Common.AuthorizeFilterAttribute(IsCheckLogin = false)]
        public ActionResult LoginBox()
        {
            ViewBag.LastUserName = WebUserHelp.GetLastUserName();
            return View();
        }

        /// <summary>
        /// 设置省份城市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult SubmitChangeCity_Api(int provinceId, int cityId)
        {
            AjaxResult result = new AjaxResult("");

            if (provinceId < 1 || cityId < 1)
            {
                result.Result = false;
                result.Message = "请选择城市";
                return AjaxJson(result);
            }

            WebUserHelp.SetNowProvinceAndCity(provinceId, cityId);

            return AjaxJson(result);
        }

        [Common.AuthorizeFilterAttribute(IsCheckLogin = false)]
        public ActionResult NotRight()
        {
            return View();
        }
    }
}
