using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Application.Interfaces.House;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.IoC.Binder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Ninject;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class CityAvgPriceController : BaseController
    {
        private static readonly ICityAvgPriceServices Services = new StandardKernel(new CityAvgPriceModule()).Get<ICityAvgPriceServices>();

        public ActionResult Index(DateTime? caseDateFrom, DateTime? caseDateTo, int? pageIndex, int areaId = 0)
        {
            ViewBag.AreaName = GetAreaName();

            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.城市均价, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();

            if (!caseDateFrom.HasValue || !caseDateTo.HasValue) return View();

            int totalCount, pageSize = 30, currIndex = pageIndex ?? 1;

            var data = Services.GetCityAvgPrices((DateTime)caseDateFrom, Convert.ToDateTime(caseDateTo.Value.ToShortDateString() + " 23:59:59"), areaId, Passport.Current.CityId, pageSize, currIndex, out totalCount);

            var model = new PagedList<DAT_AvgPrice_Month>(data, currIndex, pageSize, totalCount);

            return View(model);
        }

        //删除
        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }

            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.城市均价, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }

                foreach (var item in ids)
                {
                    Services.DeleteAvgPrice(int.Parse(item));
                }

                //日志
                Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.删除, "", "", "删除城市均价", RequestHelper.GetIP());

                return Json(new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/CityAvgPrice/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.城市均价, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            var areaId = CookieHelper.GetCookie("areaId");
            ViewBag.AreaName = GetAreaName(areaId);
            return View("Edit");
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(DAT_AvgPrice_Month ap)
        {
            try
            {
                ap.CityId = Passport.Current.CityId;
                ap.ProjectId = 0;
                ap.SubAreaId = 0;
                ap.BuildingAreaType = 0;
                ap.AvgPriceDate = DateTime.Parse(ap.CaseDate.Trim() + "-01");

                var isAp = Services.IsExistCityAvgPrice(ap.CityId, ap.AreaId, ap.AvgPriceDate);
                if (isAp == null)
                {
                    Services.AddCityAvgPrice(ap);
                }
                else
                {
                    ap.Id = isAp.Id;
                    Services.UpdateAvgPrice(ap);
                }
                Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.新增, "", "", "新增城市均价", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/CityAvgPrice/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.AreaName = GetAreaName();

            var result = Services.GetCityAvgPrice(id);
            result.CaseDate = result.AvgPriceDate.ToString("yyyy-MM");

            return View("Edit", result);
        }
        //编辑保存
        [HttpPost]
        public ActionResult Edit(DAT_AvgPrice_Month ap)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.城市均价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有修改权限！");
                }

                Services.UpdateAvgPrice(ap);

                Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.修改, ap.Id.ToString(), "", "编辑城市均价", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/CityAvgPrice/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }

            return this.RefreshParent();
        }

        [HttpGet]
        public ActionResult GetAvgPrice(int areaId, string caseDate)
        {
            var splitStrs = caseDate.Split('-');
            var year = int.Parse(splitStrs[0]);
            var month = int.Parse(splitStrs[1]);
            var lastDayOfThisMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var ap = new DAT_AvgPrice_Month
            {
                AreaId = areaId,
                DateFrom = DateTime.Parse(caseDate + "-01"),
                DateTo = lastDayOfThisMonth,
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId
            };
            var result = Services.GetAvgPrice(ap);

            return Json(new { ret = true, data = result.AvgPrice }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAvgPriceByFloatvalue(int areaId, string caseDate, decimal floatvalue)
        {
            var DateFrom = DateTime.Parse(caseDate + "-01").AddMonths(-1);
            var DateTo = DateTime.Parse(caseDate + "-01").AddDays(-1);
            int totalcount = 0;
            var result = Services.GetCityAvgPrices(DateFrom, DateTo, areaId, Passport.Current.CityId, int.MaxValue, 1, out totalcount).FirstOrDefault();

            return Json(new { ret = true, data = result != null ? Math.Round(result.AvgPrice * (1 + floatvalue / 100), 0) : 0 }, JsonRequestBehavior.AllowGet);
        }
        #region 帮助程序
        //行政区列表
        [NonAction]
        private List<SelectListItem> GetAreaName(string areaId = "")
        {
            var area = Services.GetAreaName(Passport.Current.CityId);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Value = m.AreaId.ToString(), Text = m.AreaName, Selected = (m.AreaId.ToString() == areaId) }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "0", Text = "全市" });
            return areaResult;
        }
        #endregion

    }
}
