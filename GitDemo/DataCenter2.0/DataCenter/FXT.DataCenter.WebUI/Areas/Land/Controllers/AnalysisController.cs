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

namespace FXT.DataCenter.WebUI.Areas.Land.Controllers
{
    [Authorize]
    public class AnalysisController : BaseController
    {
        private readonly IDAT_Analysis_City _analy;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;

        public AnalysisController(IDAT_Analysis_City analy, ILog log, IDropDownList dropDownList)
        {
            this._analy = analy;
            this._log = log;
            this._dropDownList = dropDownList;
        }
        [HttpGet]
        public ActionResult Index(DAT_Analysis_City model)
        {
            BindViewData();
            #region 权限验证
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion
            model.DataTypeCode = SYS_Code_Dict.数据大类.土地;
            model.FxtCompanyId = Passport.Current.FxtCompanyId;
            model.CityID = Passport.Current.CityId;
            ViewBag.SubAreaId = model.SubAreaId;
            var analyList = _analy.GetAnalysisList(model, self);
            return View(analyList);
        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindViewData();
            return View("EdityAnaly", new DAT_Analysis_City());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DAT_Analysis_City model)
        {
            try
            {
                var obj = new DAT_Analysis_City
                {
                    #region 土地区域分析Model
                    DataTypeCode = SYS_Code_Dict.数据大类.土地,
                    FxtCompanyId = Passport.Current.FxtCompanyId,
                    CityID = Passport.Current.CityId,
                    Analysis = model.Analysis,
                    Creator = Passport.Current.UserName,
                    SaveUser = Passport.Current.UserName,
                    CreateTime = DateTime.Now,
                    SaveDateTime = DateTime.Now,
                    Valid = 1,
                    AreaId = model.AreaId,
                    SubAreaId = model.SubAreaId
                    #endregion
                };
                int result = _analy.AddAnalysis(obj);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.土地数据分类.土地区域分析, SYS_Code_Dict.操作.新增, "", "", "土地区域分析", RequestHelper.GetIP());
                return this.CloseThickboxToLoad();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/Analysis/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.CloseThickboxToLoad("操作失败！");
            }
        }

        // 是否存在该分析记录
        [HttpPost]
        public ActionResult IsExist(int areaId, int subAreaId)
        {
            bool flag = IsExist(SYS_Code_Dict.数据大类.土地, Passport.Current.FxtCompanyId, Passport.Current.CityId, areaId, subAreaId);
            return Json(flag);
        }

        //编辑
        [HttpGet]
        public ActionResult EdityAnaly(string id)
        {
            var splitArray = id.Split('#');
            #region 权限判断
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地区域分析, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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
            #endregion
            var result = _analy.GetAnalysisById(Convert.ToInt32(splitArray[1]), SYS_Code_Dict.数据大类.土地, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            BindViewData();
            return View(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EdityAnaly(DAT_Analysis_City model)
        {
            try
            {
                model.FxtCompanyId = Passport.Current.FxtCompanyId;
                model.CityID = Passport.Current.CityId;
                model.SaveUser = Passport.Current.UserName;
                model.SaveDateTime = DateTime.Now;
                model.DataTypeCode = SYS_Code_Dict.数据大类.土地;
                bool resul = _analy.Update(model);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.土地数据分类.土地区域分析, SYS_Code_Dict.操作.修改, "", "", "修改区域分析", RequestHelper.GetIP());
                return this.RefreshParent();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Land/Analysis/EdityAnaly", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        [HttpPost]
        public ActionResult GetSubAreaList(int areaId)
        {
            var result = _dropDownList.GetSubAreaName(areaId);
            if (result != null && result.Count() > 0)
                return Json(new { flag = true, data = result });
            else
                return Json(new { flag = false });

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
            if (list != null && list.Any())
                return true;
            else
                return false;
        }

        private void BindViewData()
        {
            ViewBag.AreaList = GetAreaName();
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
    }
}
