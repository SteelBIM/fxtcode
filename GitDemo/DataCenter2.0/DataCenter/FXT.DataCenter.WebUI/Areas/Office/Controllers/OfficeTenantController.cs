using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace FXT.DataCenter.WebUI.Areas.Office.Controllers
{
    [Authorize]
    public class OfficeTenantController : BaseController
    {
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IImportTask _importTask;
        private readonly ICompany _company;
        private readonly IOfficeSubArea _officeSubArea;
        private readonly IOfficeProject _officeProject;
        private readonly IOfficeBuilding _officeBuilding;
        private readonly IOfficeTenant _officeTenant;

        public OfficeTenantController(ILog log, IDropDownList dropDownList, IImportTask importTask, IOfficeSubArea officeSubArea, IOfficeProject officeProject, IOfficeBuilding officeBuilding, IOfficeTenant officeTenant, ICompany company)
        {
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
            this._officeSubArea = officeSubArea;
            this._officeProject = officeProject;
            this._officeBuilding = officeBuilding;
            this._officeTenant = officeTenant;
            this._company = company;
        }

        public ActionResult Index(DatTenantOffice officeTenant, int? pageIndex, bool? isExternalRequest)
        {
            BindViewData(-1);

            if (officeTenant.SurveyDateFrom == default(DateTime) || officeTenant.SurveyDateTo == default(DateTime))
            {
                return View();
            }

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View(new List<DatTenantOffice>().ToPagedList(1, 30));
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }
            officeTenant.FxtCompanyId = Passport.Current.FxtCompanyId;
            officeTenant.CityId = Passport.Current.CityId;

            var pageSize = 30;
            int totalCount;
            var result = _officeTenant.GetTenants(officeTenant, pageIndex ?? 1, pageSize, out totalCount, self).AsQueryable();

            var tenantResult = new PagedList<DatTenantOffice>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", tenantResult);
        }

        #region 编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.租客数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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

            var result = _officeTenant.GetTenantNameById(int.Parse(splitArray[1]), int.Parse(splitArray[0]));
            BindDropDownList(result.AreaId, result.TypeCode, Passport.Current.FxtCompanyId);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(DatTenantOffice officeTenant)
        {
            try
            {
                officeTenant.CreateTime = DateTime.Now;
                officeTenant.Creator = Passport.Current.ID;
                officeTenant.SaveDateTime = DateTime.Now;
                officeTenant.SaveUser = Passport.Current.ID;

                //增加新增租户商家
                if (_company.GetCompany_office(officeTenant.TenantName, Passport.Current.CityId).FirstOrDefault() == null)
                {
                    var company = new DAT_Company();
                    company.ChineseName = officeTenant.TenantName;
                    company.CreateDate = DateTime.Now;
                    company.CityId = Passport.Current.CityId;
                    company.FxtCompanyId = Passport.Current.FxtCompanyId;
                    _company.AddCompany(company);
                }
                officeTenant.TenantID = _company.GetCompany_office(officeTenant.TenantName, Passport.Current.CityId).FirstOrDefault().CompanyId;
                _officeTenant.UpdateTenantOffice(officeTenant, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公租客信息, SYS_Code_Dict.操作.修改, officeTenant.HouseTenantId.ToString(), officeTenant.HouseName, "修改办公租客", RequestHelper.GetIP());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeTenant/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        #endregion

        #region 新增
        [HttpGet]
        public ActionResult Create()
        {
            BindDropDownList(-1, -1, Passport.Current.FxtCompanyId);
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(DatTenantOffice officeTenant)
        {
            try
            {
                officeTenant.CityId = Passport.Current.CityId;
                officeTenant.FxtCompanyId = Passport.Current.FxtCompanyId;
                officeTenant.CreateTime = DateTime.Now;
                officeTenant.Creator = Passport.Current.ID;

                //增加新增租户商家
                if (_company.GetCompany_office(officeTenant.TenantName, Passport.Current.CityId).FirstOrDefault() == null)
                {
                    var company = new DAT_Company();
                    company.ChineseName = officeTenant.TenantName;
                    company.CreateDate = DateTime.Now;
                    company.CityId = Passport.Current.CityId;
                    company.FxtCompanyId = Passport.Current.FxtCompanyId;
                    _company.AddCompany(company);
                }
                officeTenant.TenantID = _company.GetCompany_office(officeTenant.TenantName, Passport.Current.CityId).FirstOrDefault().CompanyId;

                _officeTenant.AddTenantOffice(officeTenant);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公租客信息, SYS_Code_Dict.操作.新增, "", "", "新增办公租客", RequestHelper.GetIP());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeTenant/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }
        #endregion

        #region 删除
        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.办公数据分类.租客数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    if (self)
                    {
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                    }
                    var result = _officeTenant.GetTenantNameById(int.Parse(array[1]), int.Parse(array[0]));
                    result.SaveDateTime = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _officeTenant.DeleteTenantOffice(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公租客信息, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除办公租客信息", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeTenant/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 导出
        [HttpGet]
        public ActionResult Export(int? areaid, int? subareaid, string projectName, string projectOtherName, string buildingName, string buildingOtherName, string SurveyDateFrom, string SurveyDateTo)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var tenantOffice = new DatTenantOffice
            {
                FxtCompanyId = Passport.Current.FxtCompanyId,
                CityId = Passport.Current.CityId,
                AreaId = areaid ?? 0,
                SubAreaId = subareaid ?? 0,
                ProjectName = projectName,
                ProjectOtherName = projectOtherName,
                BuildingName = buildingName,
                BuildingOtherName = buildingOtherName,
                SurveyDateFrom = DateTime.Parse(SurveyDateFrom),
                SurveyDateTo = DateTime.Parse(SurveyDateTo),
            };

            int totalCount;
            var result = _officeTenant.GetTenants(tenantOffice, 1, int.MaxValue, out totalCount, self);

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_办公_租客数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼盘, SYS_Code_Dict.操作.导出, "", "", "导出办公租客", RequestHelper.GetIP());

            using (var ms = ExcelHandle.ListToExcel<DatTenantOffice>(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        #endregion

        #region 导入

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.办公租客信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var taskList = result.ToPagedList(pageIndex ?? 1, 30);
            return View(taskList);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());
            if (null == file) return this.RedirectToAction("UploadFile");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公租客信息, SYS_Code_Dict.操作.导入, "", "", "导入办公租客信息", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "OfficeTenant");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeTenant/UploadFile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("UploadFile");
        }
        /// <summary>
        /// 导入文件保存
        /// </summary>
        [NonAction]
        private static bool SaveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            try
            {
                postedFile.SaveAs(Path.Combine(filepath, saveName));
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return true;
        }

        //根据CodeName获取Code
        private int GetCodeByName(string name)
        {
            return _dropDownList.GetCodeByName(name);
        }

        //删除导入记录
        public ActionResult DeleteOfficeTenantImportRecord(List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return Json(new { result = false, msg = "删除失败！" });
                }

                ids.ForEach(m => _importTask.DeleteTask(Int64.Parse(m)));

                return Json(new { result = true, msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeTenant/DeleteOfficeTenantImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        private void BindViewData(int areaId)
        {
            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text", areaId));
            ViewData.Add("SubAreaId", new SelectList(GetSubAreaNamesByAreaId(areaId), "Value", "Text"));
        }

        private void BindDropDownList(int areaId, int typeCode, int fxtCompanyId)
        {
            this.ViewBag.AreaName = GetAreaName();
            this.ViewBag.SubAreaName = GetSubAreaNamesByAreaId(-1);
            this.ViewBag.ProjectName = ProjectList(-1, -1);
            this.ViewBag.ProjectOtherName = "";
            this.ViewBag.BuildingName = BuildingList(-1, -1, -1, -1);
            this.ViewBag.BuildingOtherName = "";
            this.ViewBag.SubAreaName = GetSubAreaNamesByAreaId(areaId, fxtCompanyId);
            this.ViewBag.TypeCodeName = GetDictById(SYS_Code_Dict._行业大类);
            this.ViewBag.SubTypeCodeName = GetDictBySubCode(typeCode);
        }

        //根据行政区ID获取所有片区
        private IEnumerable<SelectListItem> GetSubAreaNamesByAreaId(int areaId, int fxtCompanyId)
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

        [NonAction]
        private IEnumerable<SelectListItem> GetDictById(int id)
        {
            var casetype = _dropDownList.GetDictById(id);
            var casetypeResult = new List<SelectListItem>();
            casetype.ToList().ForEach(m =>
                casetypeResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            casetypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return casetypeResult;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetDictBySubCode(int id)
        {
            var casetype = _dropDownList.GetDictBySubCode(id);
            var casetypeResult = new List<SelectListItem>();
            casetype.ToList().ForEach(m =>
                casetypeResult.Add(
                new SelectListItem { Value = m.code.ToString(), Text = m.codename }
                ));
            casetypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return casetypeResult;
        }

        //行政区列表
        [NonAction]
        private IEnumerable<SelectListItem> GetAreaName()
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
        //根据行政区ID获取所有片区
        private IEnumerable<SelectListItem> GetSubAreaNamesByAreaId(int areaId)
        {
            var list = new List<SelectListItem>();
            var subAreaNames = _officeSubArea.GetSubAreaNamesByAreaId(areaId, Passport.Current.FxtCompanyId, Passport.Current.CityId);

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

        [HttpPost]
        public ActionResult GetSubAreaName(int areaId)
        {
            var result = GetSubAreaNamesByAreaId(areaId);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetSubTypeCode(int TypeCode)
        {
            var result = GetDictBySubCode(TypeCode);
            return Json(result);
        }

        //自动匹配商家信息
        [HttpGet]
        public JsonResult TenantNameSelect()
        {
            var result = _company.GetCompany_office(null, Passport.Current.CityId);

            var data = new List<dynamic>();
            foreach (var item in result)
            {
                data.Add(new { name = item.ChineseName, id = item.CompanyId.ToString() });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //自动匹配商务中心
        [HttpGet]
        public JsonResult GetSubAreaId(int AreaId)
        {
            return Json(GetSubAreaNamesByAreaId(AreaId), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IEnumerable<SelectListItem> ProjectList(int areaId, int subAreaId)
        {
            var result = new List<SelectListItem>();

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.租客数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                result.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
                return result;
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var query = _officeProject.GetOfficeProjects(Passport.Current.CityId, Passport.Current.FxtCompanyId, areaId, subAreaId, -1, self);
            query.ToList().ForEach(m => result.Add(new SelectListItem
            {
                Value = m.ProjectId.ToString(),
                Text = m.ProjectName
            }));
            result.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return result;
        }
        [HttpGet]
        public JsonResult GetOfficeProject(int areaId, int subAreaId)
        {
            return Json(ProjectList(areaId, subAreaId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetOfficeProjectOtherName(int areaId, int subAreaId, int projectId)
        {
            var result = "";

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.租客数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                result = "";
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            result = _officeProject.GetOfficeProjects(Passport.Current.CityId, Passport.Current.FxtCompanyId, areaId, subAreaId, projectId, self).FirstOrDefault().OtherName;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IEnumerable<SelectListItem> BuildingList(int areaId, int subAreaId, int projectId, int buildingId)
        {
            var result = new List<SelectListItem>();

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.租客数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                result.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
                return result;
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var query = _officeBuilding.GetOfficeBuildings(Passport.Current.CityId, Passport.Current.FxtCompanyId, areaId, subAreaId, projectId, buildingId, self);
            query.ToList().ForEach(m => result.Add(new SelectListItem
            {
                Value = m.BuildingId.ToString(),
                Text = m.BuildingName
            }));
            result.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return result;
        }
        [HttpGet]
        public JsonResult GetOfficeBuilding(int areaId, int subAreaId, int projectId)
        {
            return Json(BuildingList(areaId, subAreaId, projectId, -1), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetOfficeBuildingOtherName(int projectId, int buildingId)
        {
            var result = "";

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.租客数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                result = "";
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            result = _officeBuilding.GetOfficeBuildings(Passport.Current.CityId, Passport.Current.FxtCompanyId, -1, -1, projectId, buildingId, self).FirstOrDefault().OtherName;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
