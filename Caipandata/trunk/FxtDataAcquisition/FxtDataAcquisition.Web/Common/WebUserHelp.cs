namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

    public class WebUserHelp
    {
        /// <summary>
        /// 登录超时
        /// </summary>
        public const int NotLogin = 1;
        /// <summary>
        /// 无权限
        /// </summary>
        public const int NotRight = 2;
        /// <summary>
        /// 系统错误
        /// </summary>
        public const int SysError = 3;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        //public static LoginUser User 
        //{
        //    get
        //    {
        //        if (!HttpContext.Current.User.Identity.IsAuthenticated)
        //        {
        //            return null;
        //        }

        //        string str = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
        //        JObject jobj = JObject.Parse(str);
        //        string json = jobj.Value<string>("user");
        //        LoginUser User = json.ParseJSONjss<LoginUser>();

        //        string json2 = jobj.Value<string>("apps");
        //        User.AppList = json2.ParseJSONList<Apps>();

        //        User.NowCityId = WebUserHelp.GetNowCityId();

        //        return User;
        //    }
        //}

        public static bool CheckUser(ActionExecutingContext filterContextint, int[] andFunctionCode, int[] orFunctionCode, string nowFunctionPageUrl, out int errorType, out string message)
        {
            message = "";
            errorType = 0;
            //验证登录
            if (GetNowLoginUser() == null)
            {
                errorType = NotLogin;
                return false;
            }
            //验证权限
            //if ((
            //    (andFunctionCode != null && andFunctionCode.Length > 0) || (orFunctionCode != null && orFunctionCode.Length > 0)
            //    )
            //    && !string.IsNullOrEmpty(nowFunctionPageUrl))
            //{
            //    if (!CheckNowPageFunctionCode(nowFunctionPageUrl, andFunctionCode, orFunctionCode))
            //    {
            //        errorType = NotRight;
            //        return false;
            //    }
            //}
            return true;
        }

        #region 当前用户信息操作
        /// <summary>
        /// 获取当前登陆的用户名
        /// </summary>
        /// <returns></returns>
        public static string GetNowLoginUserName()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            string str = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.Name;
            return str;
        }

        /// <summary>
        /// 获取当前登陆的用户
        /// </summary>
        /// <param name="appList">当前用户所拥有的applist</param>
        /// <returns></returns>
        public static LoginUser GetNowLoginUser(out List<Apps> appList)
        {
            appList = new List<Apps>();
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            string str = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
            JObject jobj = JObject.Parse(str);
            string json = jobj.Value<string>("user");// Convert.ToString(HttpContext.Current.Session["LoginUserInfo"]);
            string json2 = jobj.Value<string>("apps");
            LoginUser obj = json.ParseJSONjss<LoginUser>();
            appList = json2.ParseJSONList<Apps>();
            //UserCenter_LoginUserInfo obj = new UserCenter_LoginUserInfo { CompanyName = "房讯通", SignName = "4106DEF5-A760-4CD7-A6B2-8250420FCB18", FxtCompanyId = 25, Password = "123456", SubCompany = "", SubCompanyId = null, TrueName = "管理员", UserName = "admin@fxt" };
            return obj;
        }/// <summary>
         /// 获取当前登陆的用户
         /// </summary>
         /// <returns></returns>
        public static LoginUser GetNowLoginUser()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            //if (HttpContext.Current.Session["LoginUserInfo"] == null)
            //{
            //    return null;
            //}
            string str = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
            //Convert.ToString(HttpContext.Current.Session["LoginUserInfo"]);
            JObject jobj = JObject.Parse(str);
            string json2 = jobj.Value<string>("apps");
            string json = jobj.Value<string>("user");// Convert.ToString(HttpContext.Current.Session["LoginUserInfo"]);
            LoginUser obj = json.ParseJSONjss<LoginUser>();
            obj.AppList = json2.ParseJSONList<Apps>();
            //UserCenter_LoginUserInfo obj = new UserCenter_LoginUserInfo { CompanyName = "房讯通", SignName = "4106DEF5-A760-4CD7-A6B2-8250420FCB18", FxtCompanyId = 25, Password = "123456", SubCompany = "", SubCompanyId = null, TrueName = "管理员", UserName = "admin@fxt" };
            return obj;
        }
        /// <summary>
        /// 登录成功后的操作(存储当前登陆的用户信息+拥有产品权限的城市等)
        /// </summary>
        /// <param name="appList">当前用户所拥有的applist</param>
        /// <param name="obj"></param>
        /// <param name="cityIds">当前用户权限城市</param>
        /// <param name="menuList">当前用户权限菜单</param>
        public static void SetNowLoginUser(LoginUser obj, List<Apps> appList, int[] cityIds)
        {
            //设置当前用户权限城市
            //SetNowRightCityList(cityIds);
            //设置当前用户权限菜单
            //SetNowLoginMenu(menuList);
            //设置当前的用户信息
            var _obj = new { user = obj.ToJSONjss(), apps = appList.ToJSONjss() };

            var userInfo = _obj.ToJSONjss();
            FormsAuthenticationTicket tk = new FormsAuthenticationTicket(1,
                obj.UserName,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                true,
                userInfo,
                FormsAuthentication.FormsCookiePath
                );
            string key = System.Web.Security.FormsAuthentication.Encrypt(tk); //得到加密后的身份验证票字串 
            HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, key);
            ck.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(ck);

            //用户信息写入cookies
            //FormsAuthentication.SetAuthCookie(userInfo,false);

            HttpCookie cookie = new HttpCookie("LoginUserName");
            cookie.Value = obj.UserName;
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);

            //HttpContext.Current.Session["LoginUserInfo"] = _obj.ToJSONjss();
            //HttpContext.Current.Response.Cookies["LoginUserName"].Value = obj.UserName;
        }
        /// <summary>
        /// 获取上一次登陆的用户（用于登陆操作完成后）
        /// </summary>
        /// <returns></returns>
        public static string GetLastUserName()
        {

            if (HttpContext.Current.Request.Cookies["LoginUserName"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["LoginUserName"].Value))
            {
               return HttpContext.Current.Request.Cookies["LoginUserName"].Value;
            }
            return "";
            //return HttpContext.Current.Request.Cookies["LoginUserName"].Value;
        }
        /// <summary>
        /// 退出操作
        /// </summary>
        public static void SignOutLoginUser()
        {
            var user = GetNowLoginUser();
            if (user != null)
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
            //HttpContext.Current.Session.RemoveAll();
        }
        #endregion

        #region 当前城市信息操作
        /// <summary>
        /// 设置保存当前用户拥有产品权限的城市
        /// </summary>
        /// <param name="cityIds"></param>
        //public static void SetNowRightCityList(int[] cityIds)
        //{
        //    //设置当前用户拥有产品权限的城市
        //    List<FxtApi_SYSProvince> provinceList = new List<FxtApi_SYSProvince>();
        //    List<FxtApi_SYSCity> cityList = new List<FxtApi_SYSCity>();
        //    int cityResult = WebCommon.GetCityDataBaseByCityIds(cityIds, out provinceList, out cityList);
        //    cityIds = cityList == null || cityList.Count < 1 ? null : cityList.Select(obj => obj.CityId).ToArray<int>();
        //    var cityobj = new { result = cityResult, cityids = cityIds == null || cityIds.Length < 1 ? "" : cityIds.ConvertToString() };
        //    HttpContext.Current.Session["NowCityList"] = cityobj.ToJSONjss().EncodeField();
        //    //设置选择默认城市
        //    if (cityList.Count > 0)
        //    {
        //        int lastCityId = GetNowCityId();
        //        FxtApi_SYSCity nowCity = cityList.Where(obj => obj.CityId == lastCityId).FirstOrDefault();
        //        if (nowCity != null)
        //        {
        //            SetNowProvinceAndCity(nowCity.ProvinceId, nowCity.CityId);
        //        }
        //        else
        //        {
        //            SetNowProvinceAndCity(cityList[0].ProvinceId, cityList[0].CityId);
        //        }
        //    }
        //    else
        //    {
        //        SetNowProvinceAndCity(0, 0);
        //    }
        //}
        /// <summary>
        /// 获取登陆时保存的当前用户拥有产品权限的城市
        /// </summary>
        /// <param name="provinceList"></param>
        /// <param name="cityList"></param>
        /// <returns>1:所有城市,0:部分城市</returns>
        //public static void GetNowRightCityList(out List<FxtApi_SYSProvince> provinceList, out List<FxtApi_SYSCity> cityList)
        //{
        //    provinceList = new List<FxtApi_SYSProvince>();
        //    cityList = new List<FxtApi_SYSCity>();
        //    int result = 1;
        //    if (HttpContext.Current.Session["NowCityList"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["NowCityList"].ToString()))
        //    {
        //        string json = Convert.ToString(HttpContext.Current.Session["NowCityList"]).DecodeField();
        //        JObject jobj = JObject.Parse(json);
        //        result = jobj.Value<int>("result");
        //        string cityids = jobj.Value<string>("cityids");
        //        WebCommon.GetCityDataBaseByCityIds(cityids.ConvertToInts(','), out provinceList, out cityList);

        //    }
        //}
        /// <summary>
        /// 获取当前选择的城市
        /// </summary>
        /// <returns></returns>
        public static int GetNowCityId()
        {
            string str = "";

            if (HttpContext.Current.Request.Cookies["ProvinceAndCity"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["ProvinceAndCity"].Value))
            {
                str = HttpContext.Current.Request.Cookies["ProvinceAndCity"].Value;
                SetNowProvinceAndCity(Convert.ToInt32(str.Split(',')[0]), Convert.ToInt32(str.Split(',')[1]));
                return Convert.ToInt32(str.Split(',')[1]);
            }
            else
            {
                SetNowProvinceAndCity(0, 0);
                return 0;
            }

        }
        /// <summary>
        /// 获取当前或上次选择的省份
        /// </summary>
        /// <returns></returns>
        public static int GetNowProvinceId()
        {
            string str = "";

            if (HttpContext.Current.Request.Cookies["ProvinceAndCity"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["ProvinceAndCity"].Value))
            {
                str = HttpContext.Current.Request.Cookies["ProvinceAndCity"].Value;
                return Convert.ToInt32(str.Split(',')[0]);
            }
            else
            {
                SetNowProvinceAndCity(0, 0);
                return 0;
            }
        }
        /// <summary>
        /// 设置存储当前城市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <param name="cityId"></param>
        public static void SetNowProvinceAndCity(int provinceId, int cityId)
        {
            HttpCookie cookie = new HttpCookie("ProvinceAndCity");
            cookie.Value = provinceId.ToString() + "," + cityId.ToString();
            cookie.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        #endregion

        #region 当前菜单操作
        /// <summary>
        /// 设置当前用户拥有的菜单到缓存中
        /// </summary>
        /// <param name="menuList"></param>
        //public static void SetNowLoginMenu(IList<SYSMenu> menuList)
        //{
        //    if (menuList == null)
        //    {
        //        menuList = new List<SYSMenu>();
        //    }
        //    HttpContext.Current.Session["NowMenuList"] = menuList.ToJSONjss().EncodeField();
        //}
        /// <summary>
        /// 从缓存中获取当前用户拥有的菜单
        /// </summary>
        /// <returns></returns>
        //public static List<WebMenuClass> GetNowLoginMenu()
        //{
        //    List<WebMenuClass> list = new List<WebMenuClass>();
        //    if (HttpContext.Current.Session["NowMenuList"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["NowMenuList"].ToString()))
        //    {
        //        string json = Convert.ToString(HttpContext.Current.Session["NowMenuList"]).DecodeField();
        //        IList<SYSMenu> menuList = json.ParseJSONList<SYSMenu>();
        //        list = WebMenuHelp.GetNowUserMenu(menuList);
        //    }
        //    return list;
        //}
        #endregion

        #region 权限Common
        /// <summary>
        /// 获取当前用户在当前页面的所有操作权限
        /// </summary>
        /// <param name="nowUserName">当前登录用户</param>
        /// <param name="nowCompanyId">当前企业</param>
        /// <param name="pageUrl">当前页面链接</param>
        /// <returns></returns>
        //public static List<int> GetNowPageFunctionCodes(string nowUserName, int nowCompanyId, string pageUrl)
        //{
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrl(GetNowCityId(), nowCompanyId, nowUserName, pageUrl);
        //    if (list != null)
        //    {
        //        intList = list.Select(obj => obj.FunctionCode).ToList();
        //    }
        //    return intList;
        //    //return new List<int>{ 
        //    //    SYSCodeManager.FunOperCode_1 ,
        //    //    SYSCodeManager.FunOperCode_2 ,
        //    //    SYSCodeManager.FunOperCode_3,
        //    //    SYSCodeManager.FunOperCode_4 ,
        //    //    SYSCodeManager.FunOperCode_5 ,
        //    //    SYSCodeManager.FunOperCode_7 ,
        //    //    SYSCodeManager.FunOperCode_8 ,
        //    //    SYSCodeManager.FunOperCode_9 ,
        //    //    SYSCodeManager.FunOperCode_10 ,
        //    //    SYSCodeManager.FunOperCode_11 ,
        //    //    SYSCodeManager.FunOperCode_12, 
        //    //    SYSCodeManager.FunOperCode_13 ,
        //    //    SYSCodeManager.FunOperCode_14 ,
        //    //    SYSCodeManager.FunOperCode_15 ,
        //    //    SYSCodeManager.FunOperCode_16 ,
        //    //    SYSCodeManager.FunOperCode_17 ,
        //    //    SYSCodeManager.FunOperCode_18, 
        //    //    SYSCodeManager.FunOperCode_19, 
        //    //    SYSCodeManager.FunOperCode_20 
        //    //};
        //}
        /// <summary>
        /// 获取当前用户在当前页面的所有删除类型的操作权限
        /// </summary>
        /// <param name="nowUserName"></param>
        /// <param name="nowCompanyId"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        //public static List<int> GetNowPageFunctionCodesByDelete(string nowUserName, int nowCompanyId, string pageUrl)
        //{
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrlAndFunctionCodes(GetNowCityId(), nowCompanyId, nowUserName, pageUrl, SYSCodeManager.FunOperCodes_Delete);
        //    if (list != null)
        //    {
        //        intList = list.Select(obj => obj.FunctionCode).ToList();
        //    }
        //    return intList;
        //    //return SYSCodeManager.FunOperCodes_Delete.ToList(); ;
        //}  /// <summary>
           /// <summary>
           /// 获取当前用户在当前页面的所有修改类型的操作权限
           /// </summary>
           /// <param name="nowUserName"></param>
           /// <param name="nowCompanyId"></param>
           /// <param name="pageUrl"></param>
           /// <returns></returns>
        //public static List<int> GetNowPageFunctionCodesByUpdate(string nowUserName, int nowCompanyId, string pageUrl)
        //{
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrlAndFunctionCodes(GetNowCityId(), nowCompanyId, nowUserName, pageUrl, SYSCodeManager.FunOperCodes_Update);
        //    if (list != null)
        //    {
        //        intList = list.Select(obj => obj.FunctionCode).ToList();
        //    }
        //    return intList;
        //    //return SYSCodeManager.FunOperCodes_Update.ToList(); 
        //}
        /// <summary>
        /// 获取当前用户在当前页面的所有审核类型的操作权限
        /// </summary>
        /// <param name="nowUserName"></param>
        /// <param name="nowCompanyId"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        //public static List<int> GetNowPageFunctionCodesByAudit(string nowUserName, int nowCompanyId, string pageUrl)
        //{
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrlAndFunctionCodes(GetNowCityId(), nowCompanyId, nowUserName, pageUrl, SYSCodeManager.FunOperCodes_Audit);
        //    if (list != null)
        //    {
        //        intList = list.Select(obj => obj.FunctionCode).ToList();
        //    }
        //    return intList;
        //    //return SYSCodeManager.FunOperCodes_Audit.ToList();
        //}
        /// <summary>
        /// 获取当前用户在当前页面的指定操作权限中包含的操作权限
        /// </summary>
        /// <param name="nowUserName"></param>
        /// <param name="nowCompanyId"></param>
        /// <param name="pageUrl"></param>
        /// <param name="functionCodes">指定操作权限</param>
        /// <returns></returns>
        //public static List<int> GetNowPageFunctionCodesByFunctionCodes(string nowUserName, int nowCompanyId, string pageUrl, int[] functionCodes)
        //{
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrlAndFunctionCodes(GetNowCityId(), nowCompanyId, nowUserName, pageUrl, functionCodes);
        //    if (list != null)
        //    {
        //        intList = list.Select(obj => obj.FunctionCode).ToList();
        //    }
        //    return intList;
        //    //if (functionCodes == null)
        //    //{
        //    //    return new List<int>();
        //    //}
        //    //return functionCodes.ToList<int>();
        //}

        /// <summary>
        /// 检测当前用户在当前页面的操作Code
        /// </summary>
        /// <param name="nowUserName">当前登录用户</param>
        /// <param name="nowCompanyId">当前企业</param>
        /// <param name="pageUrl">当前页面链接</param>
        /// <param name="functionCode">当前进行的操作</param>
        /// <returns></returns>
        //public static bool CheckNowPageFunctionCode(string nowUserName, int nowCompanyId, string pageUrl, int functionCode)
        //{
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrlAndFunctionCodes(GetNowCityId(), nowCompanyId, nowUserName, pageUrl, new int[] { functionCode });
        //    if (list != null && list.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// 检测当前用户在当前页面的操作Code
        /// </summary>
        /// <param name="pageUrl">当前页面链接</param>
        /// <param name="functionCode">当前进行的操作</param>
        /// <returns></returns>
        //public static bool CheckNowPageFunctionCode(string pageUrl, int functionCode)
        //{
        //    UserCenter_LoginUserInfo user = GetNowLoginUser();
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrlAndFunctionCodes(GetNowCityId(), user.FxtCompanyId, user.UserName, pageUrl, new int[] { functionCode });
        //    if (list != null && list.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// 检测当前用户在当前页面的操作Code
        /// </summary>
        /// <param name="pageUrl">当前页面链接</param>
        /// <param name="andFunctionCodes">前验证通过必须包含的操作项CODE</param>
        /// <param name="orFunctionCodes">当前验证通过可选包含的操作项CODE</param>
        /// <returns></returns>
        //public static bool CheckNowPageFunctionCode(string pageUrl, int[] andFunctionCodes, int[] orFunctionCodes)
        //{
        //    if ((andFunctionCodes == null || andFunctionCodes.Length < 1) && (orFunctionCodes == null || orFunctionCodes.Length < 1))
        //    {
        //        return true;
        //    }
        //    UserCenter_LoginUserInfo user = GetNowLoginUser();
        //    List<int> intList = new List<int>();
        //    IList<SYSRoleMenuFunction> list = SYSRoleMenuFunctionManager.GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrl(GetNowCityId(), user.FxtCompanyId, user.UserName, pageUrl);
        //    if (andFunctionCodes != null && andFunctionCodes.Length > 0)
        //    {
        //        if (list.Where(obj => andFunctionCodes.Contains(obj.FunctionCode)).Count() < andFunctionCodes.Length)
        //        {
        //            return false;
        //        }
        //    }
        //    if (orFunctionCodes != null && orFunctionCodes.Length > 0)
        //    {
        //        if (list.Where(obj => orFunctionCodes.Contains(obj.FunctionCode)).Count() < 1)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        /// <summary>
        /// 验证用于对信息的查看权限
        /// </summary>
        /// <param name="nowLoginUserFunctionCodes">当前用户所拥有的functionCodes</param>
        /// <param name="nowLoginUserName">当前登录的用户名</param>
        /// <param name="nowInfoUserName">当前信息发起人</param>
        /// <param name="nowInfoSurveyUserName">当前信息拥有人</param>
        /// <param name="nowLoginUserDepartmentId">当前登录的用户所在组ID,无时为0</param>
        /// <param name="nowInfoDepartmentId">当前信息拥有人所在组ID,无时为0</param>
        /// <returns></returns>
        public static bool CheckNowPageViewFunctionCode(int[] nowLoginUserFunctionCodes, string nowLoginUserName, string nowInfoUserName, string nowInfoSurveyUserName,
            int nowLoginUserDepartmentId, int nowInfoStartDepartmentId, int nowInfoDepartmentId)
        {
            if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_3))
            {
                return true;
            }
            else if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_2))//拥有审核小组权限 并且在同一个
            {
                if (nowInfoUserName == nowLoginUserName)
                {
                    return true;
                }
                if (nowInfoStartDepartmentId == nowLoginUserDepartmentId)
                {
                    return true;
                }
                if ((nowLoginUserDepartmentId != 0 && nowInfoDepartmentId != 0 && nowLoginUserDepartmentId == nowInfoDepartmentId) || nowLoginUserName == nowInfoSurveyUserName)
                {
                    return true;
                }
            }
            else if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_1) && nowLoginUserName == nowInfoSurveyUserName)//拥有审核自己权限 并且是自己的信息
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 验证用于对信息的审核权限
        /// </summary>
        /// <param name="nowLoginUserFunctionCodes">当前用户所拥有的functionCodes</param>
        /// <param name="nowLoginUserName">当前登录的用户名</param>
        /// <param name="nowInfoUserName">当前信息拥有人</param>
        /// <param name="nowLoginUserDepartmentId">当前登录的用户所在组ID,无时为0</param>
        /// <param name="nowInfoDepartmentId">当前信息拥有人所在组ID,无时为0</param>
        /// <returns></returns>
        public static bool CheckNowPageAuditFunctionCode(int[] nowLoginUserFunctionCodes, string nowLoginUserName, string nowInfoUserName, int nowLoginUserDepartmentId, int nowInfoDepartmentId)
        {
            if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_20))
            {
                return true;
            }
            else if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_19))//拥有审核小组权限 并且在同一个
            {
                if ((nowLoginUserDepartmentId != 0 && nowInfoDepartmentId != 0 && nowLoginUserDepartmentId == nowInfoDepartmentId) || nowLoginUserName == nowInfoUserName)
                {
                    return true;
                }
            }
            else if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_18) && nowLoginUserName == nowInfoUserName)//拥有审核自己权限 并且是自己的信息
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 验证用于对信息的修改权限
        /// </summary>
        /// <param name="nowLoginUserFunctionCodes">当前用户所拥有的functionCodes</param>
        /// <param name="nowLoginUserName">当前登录的用户名</param>
        /// <param name="nowInfoUserName">当前信息拥有人</param>
        /// <param name="nowLoginUserDepartmentId">当前登录的用户所在组ID,无时为0</param>
        /// <param name="nowInfoDepartmentId">当前信息拥有人所在组ID,无时为0</param>
        /// <returns></returns>
        public static bool CheckNowPageUpdateFunctionCode(int[] nowLoginUserFunctionCodes, string nowLoginUserName, string nowInfoUserName, int nowLoginUserDepartmentId, int nowInfoDepartmentId,string surveyUserName)
        {
            if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_7))
            {
                return true;
            }
            else if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_6))//拥有审核小组权限 并且在同一个
            {
                if ((nowLoginUserDepartmentId != 0 && nowInfoDepartmentId != 0 && nowLoginUserDepartmentId == nowInfoDepartmentId || surveyUserName.IsNullOrEmpty()) || nowLoginUserName == nowInfoUserName)
                {
                    return true;
                }
            }
            else if (nowLoginUserFunctionCodes.Contains(SYSCodeManager.FunOperCode_5) && nowLoginUserName == nowInfoUserName)//拥有审核自己权限 并且是自己的信息
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取无权限重定向页面
        /// </summary>
        /// <returns></returns>
        public static RedirectResult GetActionNotRightPage()
        {
            return new RedirectResult(WebCommon.Url_Login_NotRight);
        }
        /// <summary>
        /// 获取登陆限重定向页面
        /// </summary>
        /// <returns></returns>
        public static RedirectResult GetActionLoginPage()
        {
            return new RedirectResult(WebCommon.Url_Login_Login);
        }
        /// <summary>
        /// 获取登陆限重定向页面_告诉登录页此页面未弹出框
        /// </summary>
        /// <returns></returns>
        public static RedirectResult GetActionLoginPageOpen()
        {
            return new RedirectResult(WebCommon.Url_Login_Login_Open);
        }
        /// <summary>
        /// 获取404页面
        /// </summary>
        /// <returns></returns>
        public static RedirectResult GetAction404Page()
        {
            return new RedirectResult(WebCommon.Url_Login_NotRight);
        }

        #endregion
    }
}