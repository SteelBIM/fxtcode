using CBSS.Framework.Contract;
using CBSS.IBS.Contract;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class UserInfoController : ControllerBase
    {
        //
        // GET: /Tbx/UserInfo/

        public ActionResult Index(UserInfoRequest request)
        {
            int totalcount = 0;
            List<UserInfoModel> UserInfoModelList = new List<UserInfoModel>();
            IEnumerable<TB_UserInfo> tbUserInfo = this.TbxService.GetUserInfoList(out totalcount, request).ToList();
            if (tbUserInfo != null && tbUserInfo.Count() > 0)
            {
                foreach (var item in tbUserInfo)
                {
                    TBX_UserInfo tBX_UserInfo = IBSService.GetUserAllInfoByUserId(item.UserID);
                    if (tBX_UserInfo != null)
                    {
                        UserInfoModel model = new UserInfoModel();
                        model.UserID = tBX_UserInfo.iBS_UserInfo.UserID;
                        model.UserName = tBX_UserInfo.iBS_UserInfo.UserName;
                        model.TrueName = tBX_UserInfo.iBS_UserInfo.TrueName;
                        model.TelePhone= tBX_UserInfo.iBS_UserInfo.TelePhone;
                        model.UserType = tBX_UserInfo.iBS_UserInfo.UserType;
                        model.Regdate = tBX_UserInfo.iBS_UserInfo.Regdate;
                        model.Province = tBX_UserInfo.Province;
                        model.City = tBX_UserInfo.City;
                        model.Area = (tBX_UserInfo.ClassSchDetailList != null && tBX_UserInfo.ClassSchDetailList.Count() > 0) ? tBX_UserInfo.ClassSchDetailList[0].AreaName : "";
                        model.SchoolName = (tBX_UserInfo.ClassSchDetailList != null && tBX_UserInfo.ClassSchDetailList.Count() > 0) ? tBX_UserInfo.ClassSchDetailList[0].SchName : "";
                        model.ClassName = (tBX_UserInfo.ClassSchDetailList != null && tBX_UserInfo.ClassSchDetailList.Count() > 0) ? tBX_UserInfo.ClassSchDetailList[0].ClassName : "";
                        model.DeviceType = tBX_UserInfo.iBS_UserInfo.DeviceType.ToString();
                        model.AppName = GetAppName(string.IsNullOrEmpty(tBX_UserInfo.iBS_UserInfo.AppID) ? "" : tBX_UserInfo.iBS_UserInfo.AppID);
                        UserInfoModelList.Add(model);
                    }
                }
            }
            //筛选
            if (!string.IsNullOrEmpty(request.UserName))
            {
                UserInfoModelList = UserInfoModelList.Where(a => a.UserName.Contains(request.UserName)).ToList();
            }
            if (!string.IsNullOrEmpty(request.TrueName))
            {
                UserInfoModelList = UserInfoModelList.Where(a => a.TrueName.Contains(request.TrueName)).ToList();
            }
            if (!string.IsNullOrEmpty(request.TelePhone))
            {
                UserInfoModelList = UserInfoModelList.Where(a => a.TelePhone.Contains(request.TelePhone)).ToList();
            }

            var list = new PagedList<UserInfoModel>(UserInfoModelList, request.PageIndex, PageSize, totalcount);
            return View(list);
        }

        public ActionResult MarketBookDetail()
        {
            return View("MarketBookDetail");
        }
        /// <summary>
        /// 根据AppID获取应用名称
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public string GetAppName(string AppID)
        {
            if (!string.IsNullOrEmpty(AppID))
            {
                App app = TbxService.GetApp(AppID);
                if (app != null)
                {
                    return app.AppName;
                } 
            } 
            return "";
        }
    }
}
