using AutoMapper;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Web.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace FxtDataAcquisition.Web.Controllers
{
    public class HomeController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public HomeController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }
        [Common.AuthorizeFilterAttribute(IsCheckLogin = true)]
        public ActionResult Index(UserCenter_LoginUserInfo userInfo)
        {
            ViewBag.UserName = userInfo.TrueName;
            var province = WebUserHelp.GetNowProvinceId();
            var city = WebUserHelp.GetNowCityId();
            //省份
            List<FxtApi_SYSProvince> provinceList = _unitOfWork.CityService.GetProvinceCityListBy(userInfo.UserName, userInfo.SignName, userInfo.AppList);

            if (provinceList.Where(m => m.CityList.Any(c => c.CityId == city)).Count() < 1)
            {
                WebUserHelp.SetNowProvinceAndCity(0, 0);
            }

            ViewBag.ProvinceList = provinceList;
            ViewBag.NowProvinceId = province;
            ViewBag.NowCityId = city;
            ViewBag.MenuList = GetMenuDtoList(userInfo.UserName, userInfo.NowCityId, userInfo.FxtCompanyId, userInfo);

            return View();
        }

        public ActionResult SelectCity(int provinceId, UserCenter_LoginUserInfo userInfo)
        {
            AjaxResult result = new AjaxResult("");
            List<FxtApi_SYSProvince> provinceList = _unitOfWork.CityService.GetProvinceCityListBy(userInfo.UserName, userInfo.SignName, userInfo.AppList);
            //城市
            var cl = provinceList.Where(m => m.ProvinceId == provinceId).FirstOrDefault();
            List<FxtApi_SYSCity> cityList = provinceList.Count > 0 && cl != null ? cl.CityList : new List<FxtApi_SYSCity>();
            result.Data = cityList;
            return Json(result);
        }

        public ActionResult SelectMenuTotalCount(UserCenter_LoginUserInfo userInfo)
        {
            AjaxResult result = new AjaxResult("");
            result.Data = GetMenuDtoList(userInfo.UserName, userInfo.NowCityId, userInfo.FxtCompanyId, userInfo);
            return AjaxJson(result);
        }

        public List<MenuDto> GetMenuDtoList(string username, int cityid, int fxtCompanyId, UserCenter_LoginUserInfo userInfo)
        {
            var allotFlowFilter = PredicateBuilder.True<AllotFlow>();
            #region 小组权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(userInfo.UserName, userInfo.FxtCompanyId, userInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager)
    .Select(m => m.FunctionCode).ToList();
            //根据操作权限
            if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_3))//查看公司全部(管理员+分配人+审核人)
            {
                log.Info("查看公司全部");
            }
            else if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_2))//查看小组内(组长)
            {
                log.Info("查看小组内");
                var nowDepartmentUser = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == userInfo.FxtCompanyId
                    && m.CityID == userInfo.NowCityId && m.UserName == userInfo.UserName).FirstOrDefault();
                if (nowDepartmentUser != null)
                {
                    //小组下所有用户
                    var departmentUser = _unitOfWork.DepartmentUserRepository.Get()
                                            .Where(m => m.FxtCompanyID == userInfo.FxtCompanyId && m.CityID == userInfo.NowCityId
                                            && m.DepartmentID == nowDepartmentUser.DepartmentID).ToList();
                    var userids = departmentUser.Select(m => m.UserName);

                    if (userids.Count() > 0)
                    {
                        allotFlowFilter = allotFlowFilter.And(m => userids.Contains(m.UserName) || userids.Contains(m.SurveyUserName) 
                            || m.SurveyUserName == userInfo.UserName || m.UserName == userInfo.UserName);
                    }
                }
            }
            else//查看自己(查勘员)
            {
                log.Info("查看自己");
                allotFlowFilter = allotFlowFilter.And(m => m.UserName == userInfo.UserName || m.SurveyUserName == userInfo.UserName);
            }
            #endregion

            var roleUser = _unitOfWork.SysRoleUserRepository.Get(m => m.UserName == username && (m.CityID == cityid || m.CityID == 0) && (m.FxtCompanyID == fxtCompanyId || m.FxtCompanyID == 0));
            var roleMenu = _unitOfWork.SysRoleMenuRepository.Get(m => (m.CityID == cityid || m.CityID == 0) && (m.FxtCompanyID == fxtCompanyId || m.FxtCompanyID == 0));
            var menu = _unitOfWork.SysMenuRepository.Get(m => m.Valid == 1 && m.ModuleCode == 1001);

            var menus = (
                from ru in roleUser
                join rm in roleMenu on ru.RoleID equals rm.RoleID
                join m in menu on rm.MenuID equals m.Id
                select m
                               ).Distinct().ToList();

            List<MenuDto> menuDtod = new List<MenuDto>();
            menus.ForEach((o) =>
            {
                var dto = Mapper.Map<SYS_Menu, MenuDto>(o);
                switch (o.MenuName)
                {
                    case "全部":
                        var all = allotFlowFilter.And(m => m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(all).Count();
                        dto.ClassId = "all";
                        break;
                    case "未分配任务":
                        var wfp = allotFlowFilter.And(m => m.StateCode == SYSCodeManager.STATECODE_1 && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(wfp).Count();
                        dto.ClassId = "wfp";
                        break;
                    case "已分配任务":
                        var yfp = allotFlowFilter.And(m => (m.StateCode == SYSCodeManager.STATECODE_2 || m.StateCode == SYSCodeManager.STATECODE_4) && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(yfp).Count();
                        dto.ClassId = "yfp";
                        break;
                    case "待查勘":
                        var dck = allotFlowFilter.And(m => m.StateCode == SYSCodeManager.STATECODE_2 && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(dck).Count();
                        dto.ClassId = "dck";
                        break;
                    case "查勘中":
                        var ckz = allotFlowFilter.And(m => m.StateCode == SYSCodeManager.STATECODE_4 && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(ckz).Count();
                        dto.ClassId = "ckz";
                        break;
                    case "已传回任务":
                        var ych = allotFlowFilter.And(m => (m.StateCode == SYSCodeManager.STATECODE_5
                            || m.StateCode == SYSCodeManager.STATECODE_6 || m.StateCode == SYSCodeManager.STATECODE_8
                            || m.StateCode == SYSCodeManager.STATECODE_10) && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(ych).Count();
                        dto.ClassId = "ych";
                        break;
                    case "已查勘":
                        var yck = allotFlowFilter.And(m => m.StateCode == SYSCodeManager.STATECODE_5 && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(yck).Count();
                        dto.ClassId = "yck";
                        break;
                    case "待审批":
                        var dsp = allotFlowFilter.And(m => m.StateCode == SYSCodeManager.STATECODE_6 && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(dsp).Count();
                        dto.ClassId = "dsp";
                        break;
                    case "审批已通过":
                        var spytg = allotFlowFilter.And(m => m.StateCode == SYSCodeManager.STATECODE_8 && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(spytg).Count();
                        dto.ClassId = "spytg";
                        break;
                    case "已入库":
                        var yrk = allotFlowFilter.And(m => m.StateCode == SYSCodeManager.STATECODE_10 && m.CityId == cityid);
                        dto.TotalCount = _unitOfWork.AllotFlowRepository.Get(yrk).Count();
                        dto.ClassId = "yrk";
                        break;
                    default:
                        dto.TotalCount = -1;
                        break;
                }
                menuDtod.Add(dto);
            });
            return menuDtod;
        }

    }
}
