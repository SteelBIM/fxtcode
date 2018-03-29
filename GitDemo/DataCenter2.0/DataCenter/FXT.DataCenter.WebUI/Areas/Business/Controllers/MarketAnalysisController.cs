using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Application.Interfaces.Share;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.IoC.Binder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Ninject;

namespace FXT.DataCenter.WebUI.Areas.Business.Controllers
{
    [Authorize]
    public class MarketAnalysisController : BaseController
    {

        private static readonly IMarketAnalysisServices Services = new StandardKernel(new MarketAnalysisModule()).Get<IMarketAnalysisServices>();
        [HttpGet]
        public ActionResult Index(DatAnalysisMarket model)
        {
            BindViewData();
            #region 权限验证
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.市场背景分析, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion

            model.DataTypeCode = SYS_Code_Dict.数据大类.商业;
            model.FxtCompanyId = Passport.Current.FxtCompanyId;
            model.CityId = Passport.Current.CityId;
            ViewBag.SubAreaId = model.SubAreaId;
            var analyList = Services.GetAnalysisList(model, self);
            return View(analyList);
        }

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.市场背景分析, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindViewData();
            return View("Edit", new DatAnalysisMarket());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DatAnalysisMarket model)
        {
            try
            {
                var obj = new DatAnalysisMarket
                {
                    #region 商业区域分析Model
                    DataTypeCode = SYS_Code_Dict.数据大类.商业,
                    FxtCompanyId = Passport.Current.FxtCompanyId,
                    CityId = Passport.Current.CityId,
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
                int result = Services.AddAnalysis(obj);
                Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.商业数据分类.市场背景分析, SYS_Code_Dict.操作.新增, "", "", "商业区域分析", RequestHelper.GetIP());
                return this.CloseThickboxToLoad();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/MarketAnalysis/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.CloseThickboxToLoad("操作失败！");
            }
        }

        [HttpPost]
        public ActionResult IsExist(int areaId, int subAreaId)
        {
            bool flag = IsExist(SYS_Code_Dict.数据大类.商业, Passport.Current.FxtCompanyId, Passport.Current.CityId, areaId, subAreaId);
            return Json(flag);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');
            #region 权限判断
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.市场背景分析, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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
            var result = Services.GetAnalysisById(Convert.ToInt32(splitArray[1]), SYS_Code_Dict.数据大类.商业, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            BindViewData();
            return View(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DatAnalysisMarket model)
        {
            try
            {
                model.FxtCompanyId = Passport.Current.FxtCompanyId;
                model.CityId = Passport.Current.CityId;
                model.SaveUser = Passport.Current.UserName;
                model.SaveDateTime = DateTime.Now;
                model.DataTypeCode = SYS_Code_Dict.数据大类.商业;
                bool resul = Services.Update(model);
                Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.商业数据分类.市场背景分析, SYS_Code_Dict.操作.修改, "", "", "修改市场背景分析", RequestHelper.GetIP());
                return this.RefreshParent();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Business/MarketAnalysis/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        [HttpPost]
        public ActionResult GetSubAreaList(int areaId)
        {
            var result = Services.GetSubAreaName(areaId);
            if (result != null && result.Any())
                return Json(new { flag = true, data = result });
            return Json(new { flag = false });
        }

        private bool IsExist(int dataCode, int fxtCompanyId, int cityId, int areaId, int subAreaId)
        {
            var list = Services.GetAnalysisList(new DatAnalysisMarket
            {
                DataTypeCode = dataCode,
                FxtCompanyId = fxtCompanyId,
                CityId = cityId,
                AreaId = areaId,
                SubAreaId = subAreaId
            });
            return list != null && list.Any();
        }

        private void BindViewData()
        {
            ViewBag.AreaList = GetAreaName();
        }

        [NonAction]
        private List<SelectListItem> GetAreaName()
        {
            var area = Services.GetAreaName(Passport.Current.CityId);
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
