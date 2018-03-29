using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.WebUI.Areas.Office.Controllers
{
    [Authorize]
    public class OfficeCaseController : BaseController
    {
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IImportTask _importTask;
        private readonly IOfficeCase _officeCase;
        private readonly IOfficeProject _officeProject;
        private readonly IOfficeBuilding _officeBuilding;
        private readonly IOfficeHouse _officeHouse;

        public OfficeCaseController(IOfficeCase officeCase, IOfficeProject officeProject, IOfficeHouse officeHouse, IOfficeBuilding officeBuilding, ILog log, IDropDownList dropDownList, IImportTask importTask)
        {
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
            this._officeCase = officeCase;
            this._officeProject = officeProject;
            this._officeBuilding = officeBuilding;
            this._officeHouse = officeHouse;
        }

        //
        // GET: /Office/OfficeCase/
        public ActionResult Index(DatCaseOffice datCaseOffice, int? pageIndex)
        {
            BindDropDownList(true);
            this.ViewBag.CaseTypeName = GetCaseTypeCode(0);
            this.ViewBag.CaseType = datCaseOffice.CaseType;

            if (datCaseOffice.CaseDateStart == default(DateTime) || datCaseOffice.CaseDateEnd == default(DateTime))
            {
                return View();
            }

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            datCaseOffice.CityId = Passport.Current.CityId;
            datCaseOffice.FxtCompanyId = Passport.Current.FxtCompanyId;
            if (!string.IsNullOrEmpty(datCaseOffice.ProjectName))
            {
                datCaseOffice.ProjectName = datCaseOffice.ProjectName.Trim();
            }

            var pageSize = 30; int totalCount;
            var result = _officeCase.GetOfficeCases(datCaseOffice, pageIndex ?? 1, pageSize, out totalCount, self);
            var officeCaseResult = new PagedList<DatCaseOffice>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", officeCaseResult);
        }

        //删除
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.删除自己)]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }

            try
            {
                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');

                    if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                    {
                        failList.Add(array[1]);
                        continue;
                    }
                    var result = _officeCase.GetOfficeCase(int.Parse(array[1]));
                    result.SaveDateTime = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _officeCase.DeleteOfficeCase(result);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除办公案例", RequestHelper.GetIP());

                return
                    Json(failList.Any()
                        ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" }
                        : new { result = true, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeCase/Delete", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        //删除重复案例(暂定：操作者具有‘删除全部’的权限，才能进行当前操作)
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.删除自己)]
        public ActionResult DeleteSameCase(string time)
        {
            try
            {
                DateTime caseDateFrom = DateTime.Now, caseDateTo = DateTime.Now;

                switch (time)
                {
                    case "0": //近一个月
                        caseDateFrom = DateTime.Now.AddMonths(-1);
                        caseDateTo = DateTime.Now;
                        break;
                    case "1": //近三个月
                        caseDateFrom = DateTime.Now.AddMonths(-3);
                        caseDateTo = DateTime.Now;
                        break;
                    case "2": //近半年
                        caseDateFrom = DateTime.Now.AddMonths(-6);
                        caseDateTo = DateTime.Now;
                        break;
                    case "3": //近一年
                        caseDateFrom = DateTime.Now.AddYears(-1);
                        caseDateTo = DateTime.Now;
                        break;
                }

                int result = _officeCase.DeleteSameOfficeCase(Passport.Current.FxtCompanyId, Passport.Current.CityId, caseDateFrom, caseDateTo, Passport.Current.ID);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.操作.删除, "", "", "删除重复办公案例", RequestHelper.GetIP());
                return base.Back("已删除" + result + "条案例数据。");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeCase/DeleteSameCase", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }

        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindDropDownList();
            this.ViewBag.CaseTypeName = GetCaseTypeCode();
            this.ViewBag.OfficeTypeName = GetDictById(SYS_Code_Dict._办公楼等级);
            this.ViewBag.FitmentName = GetDictById(SYS_Code_Dict._装修情况);
            return View();
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(DatCaseOffice datCaseOffice)
        {
            try
            {
                datCaseOffice.CityId = Passport.Current.CityId;
                datCaseOffice.FxtCompanyId = Passport.Current.FxtCompanyId;
                datCaseOffice.CreateTime = DateTime.Now;
                datCaseOffice.Creator = Passport.Current.ID;
                datCaseOffice.BuildingId = _officeBuilding.GetOfficeBuildingId(datCaseOffice.ProjectId, -1,
                    datCaseOffice.BuildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (datCaseOffice.BuildingId > 0)
                    datCaseOffice.HouseId = _officeHouse.GetOfficeHouseId(datCaseOffice.BuildingId, datCaseOffice.HouseName, Passport.Current.CityId, Passport.Current.FxtCompanyId);

                _officeCase.AddOfficeCase(datCaseOffice);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.操作.新增, "", "", "新增办公案例", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeCase/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

            return base.RefreshParent();
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var projectCase = _officeCase.GetOfficeCase(int.Parse(splitArray[1]));

            BindDropDownList();
            this.ViewBag.CaseTypeName = GetCaseTypeCode();
            this.ViewBag.OfficeTypeName = GetDictById(SYS_Code_Dict._办公楼等级);
            this.ViewBag.FitmentName = GetDictById(SYS_Code_Dict._装修情况);
            return View("Create", projectCase);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(DatCaseOffice datCaseOffice)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
                if (datCaseOffice.FxtCompanyId != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，您不能修改其他公司数据！");
                }

                datCaseOffice.SaveDateTime = DateTime.Now;
                datCaseOffice.SaveUser = Passport.Current.ID;
                datCaseOffice.BuildingId = _officeBuilding.GetOfficeBuildingId(datCaseOffice.ProjectId, -1,
                    datCaseOffice.BuildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (datCaseOffice.BuildingId > 0)
                    datCaseOffice.HouseId = _officeHouse.GetOfficeHouseId(datCaseOffice.BuildingId, datCaseOffice.HouseName, Passport.Current.CityId, Passport.Current.FxtCompanyId);

                _officeCase.UpdateOfficeCase(datCaseOffice);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.操作.修改, datCaseOffice.Id.ToString(), "", "修改办公案例", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeCase/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
            return this.RefreshParent();
        }

        //导出
        public ActionResult Export(DatCaseOffice datCaseOffice)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;


            datCaseOffice.CityId = Passport.Current.CityId;
            datCaseOffice.FxtCompanyId = Passport.Current.FxtCompanyId;
            if (!string.IsNullOrEmpty(datCaseOffice.ProjectName))
            {
                datCaseOffice.ProjectName = datCaseOffice.ProjectName.Trim();
            }

            int totalCount;
            var result = _officeCase.GetOfficeCases(datCaseOffice, 1, int.MaxValue, out totalCount, self);

            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.操作.导出, "", "", "导出办公案例", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_办公_案例数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //删除导入记录
        public ActionResult DeleteOfficeCaseImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Office/OfficeCase/DeleteOfficeCaseImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }


        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.办公案例, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            var taskList = result.ToPagedList(1, 30);

            return View(taskList);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId);

            if (null == file) return this.Back("操作失败！");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公案例, SYS_Code_Dict.操作.导入, "", "", "导入办公案例", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "OfficeCase");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeCase/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

        }

        [NonAction]
        private static void SaveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
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
        }

        #region 帮助程序

        //下拉列表数据绑定
        private void BindDropDownList(bool isIndexPageRequest = false)
        {
            if (!isIndexPageRequest)
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
                if (operate == (int)PermissionLevel.All) self = false;

                var projects = _officeProject.GetOfficeProjects(Passport.Current.CityId, Passport.Current.FxtCompanyId, -1, -1, -1, self);
                var result = new List<SelectListItem>();
                projects.ToList().ForEach(m => result.Add(new SelectListItem { Value = m.ProjectId.ToString(), Text = m.ProjectName + "(" + m.AreaName + ")" }));
                result.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
                this.ViewBag.ProjectName = result;
            }

        }

        //根据ID查找相应值
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

        public IEnumerable<SelectListItem> GetCaseTypeCode(int? caseType = null)
        {
            var casetypecode = _dropDownList.GetDictById(SYS_Code_Dict._案例类型);

            var caseTypeSaleList = new List<SelectListItem>();
            casetypecode.ToList().Where(m => !m.CodeName.Contains("平方米租")).ToList().ForEach(m => caseTypeSaleList.Add(new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }));
            caseTypeSaleList.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });

            var caseTypeRentList = new List<SelectListItem>();
            casetypecode.ToList().Where(m => m.CodeName.Contains("平方米租")).ToList().ForEach(m => caseTypeRentList.Add(new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }));
            caseTypeRentList.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return caseType != null ? (caseType == 0 ? caseTypeSaleList : caseTypeRentList) : GetDictById(SYS_Code_Dict._案例类型);
        }

        public ActionResult GetCaseTypeCodeList(int caseType)
        {
            return Json(GetCaseTypeCode(caseType));
        }

        //excel 导出头部信息
        readonly Action<string> _excelExportHeader = m =>
        {
            var curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             HttpUtility.UrlEncode(m, System.Text.Encoding.GetEncoding("UTF-8")) + DateTime.Now.ToShortDateString() + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

        };

        #endregion
    }
}
