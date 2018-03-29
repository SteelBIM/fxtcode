using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;

namespace FXT.DataCenter.WebUI.Areas.Office.Controllers
{
    [Authorize]
    public class OfficeAnalysisController : BaseController
    {
        private readonly IDAT_Analysis_City _analy;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IOfficeSubArea _officeSubArea;

        public OfficeAnalysisController(IDAT_Analysis_City analy, ILog log, IDropDownList dropDownList, IOfficeSubArea officeSubArea)
        {
            this._analy = analy;
            this._log = log;
            this._dropDownList = dropDownList;
            this._officeSubArea = officeSubArea;
        }

        [HttpGet]
        public ActionResult Index(DAT_Analysis_City analysisCity)
        {
            BindViewData(-1, Passport.Current.FxtCompanyId);

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.区域分析, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View();
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            analysisCity.DataTypeCode = SYS_Code_Dict.数据大类.办公;
            analysisCity.FxtCompanyId = Passport.Current.FxtCompanyId;
            analysisCity.CityID = Passport.Current.CityId;
            ViewBag.SubAreaId = analysisCity.SubAreaId;
            var analyList = _analy.GetAnalysisList(analysisCity, self);
            return View(analyList);
        }

        #region 编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.区域分析, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有修改权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            if (self)
            {
                if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }

            var result = _analy.GetAnalysisById(Convert.ToInt32(splitArray[1]), SYS_Code_Dict.数据大类.办公, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            BindViewData(result.AreaId, Passport.Current.FxtCompanyId);
            return View(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DAT_Analysis_City analysisCity)
        {
            try
            {
                analysisCity.FxtCompanyId = Passport.Current.FxtCompanyId;
                analysisCity.CityID = Passport.Current.CityId;
                analysisCity.SaveUser = Passport.Current.UserName;
                analysisCity.SaveDateTime = DateTime.Now;
                analysisCity.DataTypeCode = SYS_Code_Dict.数据大类.办公;
                bool resul = _analy.Update(analysisCity);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.办公数据分类.区域分析, SYS_Code_Dict.操作.修改, "", "", "修改办公区域分析", RequestHelper.GetIP());
                return this.RefreshParent();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Office/OfficeAnalysis/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }
        #endregion

        #region 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.区域分析, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindViewData(-1, Passport.Current.FxtCompanyId);
            return View("Edit", new DAT_Analysis_City());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DAT_Analysis_City analysisCity)
        {
            try
            {
                var obj = new DAT_Analysis_City
                {
                    DataTypeCode = SYS_Code_Dict.数据大类.办公,
                    FxtCompanyId = Passport.Current.FxtCompanyId,
                    CityID = Passport.Current.CityId,
                    Analysis = analysisCity.Analysis,
                    Creator = Passport.Current.UserName,
                    SaveUser = Passport.Current.UserName,
                    CreateTime = DateTime.Now,
                    SaveDateTime = DateTime.Now,
                    Valid = 1,
                    AreaId = analysisCity.AreaId,
                    SubAreaId = analysisCity.SubAreaId
                };
                int result = _analy.AddAnalysis(obj);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.办公数据分类.区域分析, SYS_Code_Dict.操作.新增, "", "", "新增办公区域分析", RequestHelper.GetIP());
                return this.CloseThickboxToLoad();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeAnalysis/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.CloseThickboxToLoad("操作失败！");
            }
        }

        [HttpPost]
        public ActionResult IsExist(int areaId, int subAreaId)
        {
            bool flag = IsExist(SYS_Code_Dict.数据大类.办公, Passport.Current.FxtCompanyId, Passport.Current.CityId, areaId, subAreaId);
            return Json(flag);
        }

        private bool IsExist(int dataCode, int fxtCompanyId, int cityId, int areaId, int subAreaId)
        {
            var list = _analy.GetAnalysisList(new DAT_Analysis_City
            {
                DataTypeCode = dataCode,
                FxtCompanyId = fxtCompanyId,
                CityID = cityId,
                AreaId = areaId,
                SubAreaId = subAreaId
            });
            if (list != null && list.Count() > 0)
                return true;
            else
                return false;
        }

        #endregion

        private void BindViewData(int areaId, int fxtCompanyId)
        {
            ViewBag.AreaList = GetAreaName();
            ViewBag.SubAreaName = GetSubAreaName(areaId, fxtCompanyId);
        }

        [NonAction]
        private List<SelectListItem> GetAreaName()
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Value = m.AreaId.ToString(), Text = m.AreaName }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            return areaResult;
        }

        [HttpGet]
        public ActionResult GetSubArea(int areaId)
        {
            return Json(GetSubAreaName(areaId, Passport.Current.FxtCompanyId), JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        private List<SelectListItem> GetSubAreaName(int areaId, int fxtCompanyId)
        {
            var list = new List<SelectListItem>();
            var subAreaNames = _officeSubArea.GetSubAreaNamesByAreaId(areaId, fxtCompanyId, Passport.Current.CityId);

            subAreaNames.ToList().ForEach(m => list.Add(
                new SelectListItem
                {
                    Value = m.SubAreaId.ToString(),
                    Text = m.SubAreaName,
                }
                ));
            list.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return list;
        }
    }
}
