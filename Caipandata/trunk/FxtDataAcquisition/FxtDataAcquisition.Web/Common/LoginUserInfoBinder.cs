namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Web.Mvc;
    using System.Text.RegularExpressions;

    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

    public class LoginUserInfoBinder : DefaultModelBinder
    {
        private static readonly Regex LISTREG = new Regex(@"[\d+]", RegexOptions.Compiled);
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var loginUserInfo = WebUserHelp.GetNowLoginUser();
            loginUserInfo.NowCityId = WebUserHelp.GetNowCityId();
            return loginUserInfo;
        }

    }
}