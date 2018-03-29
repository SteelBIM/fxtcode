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

namespace FXT.DataCenter.WebUI.Areas.Industry.Controllers
{
    [Authorize]
    public class IndustryCaseController : BaseController
    {
        private readonly IIndustryCase _industryCase;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IIndustrySubArea _industrySubArea;
        private readonly IIndustryProject _industryProject;
        private readonly IImportTask _importTask;
        private readonly IIndustryBuilding _industryBuilding;
        private readonly IIndustryHouse _industryHouse;
        public IndustryCaseController(IIndustryCase industryCase, ILog log, IDropDownList dropDownList, IIndustrySubArea industrySubArea, IIndustryProject industryProject, IImportTask importTask, IIndustryBuilding industryBuilding, IIndustryHouse industryHouse)
        {
            this._industryCase = industryCase;
            this._log = log;
            this._dropDownList = dropDownList;
            this._industrySubArea = industrySubArea;
            this._industryProject = industryProject;
            this._importTask = importTask;
            this._industryBuilding = industryBuilding;
            this._industryHouse = industryHouse;
        }
        //
        // GET: /Industry/IndustryCase/

        public ActionResult Index(DatCaseIndustry datCase, int? pageIndex)
        {
            this.ViewBag.CaseTypeName = GetCaseTypeCode(datCase.CaseType);
            this.ViewBag.CaseType = datCase.CaseType;

            if (datCase.CaseDateStart == default(DateTime) || datCase.CaseDateEnd == default(DateTime))
            {
                return View();
            }

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            datCase.CityId = Passport.Current.CityId;
            datCase.FxtCompanyId = Passport.Current.FxtCompanyId;
            datCase.ProjectName = !string.IsNullOrEmpty(datCase.ProjectName) ? datCase.ProjectName.Trim() : datCase.ProjectName;

            int pageSize = 30;
            int totalCount = 0;
            var result = _industryCase.GetIndustryCases(datCase, pageIndex ?? 1, pageSize, out totalCount, self);
            var caseResult = new PagedList<DatCaseIndustry>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", caseResult);
        }

        #region 编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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

            var projectCase = _industryCase.GetIndustryCase(int.Parse(splitArray[1]));

            BindDropDownList(projectCase.AreaId);
            this.ViewBag.CaseTypeName = GetCaseTypeCode();
            return View(projectCase);
        }

        [HttpPost]
        public ActionResult Edit(DatCaseIndustry datCase)
        {
            try
            {
                if (datCase.FxtCompanyId != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
                datCase.SaveDateTime = DateTime.Now;
                datCase.SaveUser = Passport.Current.ID;
                datCase.BuildingId = _industryBuilding.GetIndustryBuildingId(datCase.ProjectId, -1, datCase.BuildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (datCase.BuildingId > 0)
                {
                    datCase.HouseId = 0;// _industryHouse.GetIndustryHouseId(datCase.BuildingId, datCase.HouseName, Passport.Current.CityId, Passport.Current.FxtCompanyId); 
                }

                _industryCase.UpdateIndustryCase(datCase);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.操作.修改, datCase.Id.ToString(), "", "修改工业案例", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryCase/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
            return this.RefreshParent();
        }
        #endregion

        #region 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindDropDownList(-1);
            this.ViewBag.CaseTypeName = GetCaseTypeCode();
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(DatCaseIndustry datCase)
        {
            try
            {
                datCase.CityId = Passport.Current.CityId;
                datCase.FxtCompanyId = Passport.Current.FxtCompanyId;
                datCase.CreateTime = DateTime.Now;
                datCase.Creator = Passport.Current.ID;
                datCase.BuildingId = _industryBuilding.GetIndustryBuildingId(datCase.ProjectId, -1, datCase.BuildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (datCase.BuildingId > 0)
                {
                    datCase.HouseId = 0;// _industryHouse.GetIndustryHouseId(datCase.BuildingId, datCase.HouseName, Passport.Current.CityId, Passport.Current.FxtCompanyId); 
                }

                _industryCase.AddIndustryCase(datCase);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.操作.新增, "", "", "新增工业案例", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryCase/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

            return base.RefreshParent();
        }
        #endregion

        #region 导出
        public ActionResult Export(DatCaseIndustry datCase)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            datCase.CityId = Passport.Current.CityId;
            datCase.FxtCompanyId = Passport.Current.FxtCompanyId;
            datCase.ProjectName = !string.IsNullOrEmpty(datCase.ProjectName) ? datCase.ProjectName.Trim() : datCase.ProjectName;

            int totalCount;
            var result = _industryCase.GetIndustryCases(datCase, 1, int.MaxValue, out totalCount, self);

            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.操作.导出, "", "", "导出工业案例", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_工业_案例数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }
        #endregion

        #region 导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.工业案例, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业案例, SYS_Code_Dict.操作.导入, "", "", "导入工业案例", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "IndustryCase");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryCase/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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
        #endregion

        #region 删除
        //删除
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.删除自己)]
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
                    var result = _industryCase.GetIndustryCase(int.Parse(array[1]));
                    result.SaveDateTime = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _industryCase.DeleteIndustryCase(result);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除工业案例", RequestHelper.GetIP());

                return Json(failList.Any()
                        ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" }
                        : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryCase/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //删除重复案例(暂定：操作者具有‘删除全部’的权限，才能进行当前操作)
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.删除自己)]
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

                _industryCase.DeleteSameIndustryCase(Passport.Current.FxtCompanyId, Passport.Current.CityId, caseDateFrom, caseDateTo, Passport.Current.ID);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.操作.删除, "", "", "删除重复工业案例", RequestHelper.GetIP());
                return base.Back();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryCase/DeleteSameCase", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        //删除导入记录
        public ActionResult DeleteCaseImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Industry/IndustryCase/DeleteIndustryCaseImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #endregion

        #region 帮助程序
        private void BindDropDownList(int areaId)
        {
            this.ViewBag.AreaName = GetAreaName();
            this.ViewBag.SubAreaName = GetSubAreaNamesByAreaId(areaId);
            this.ViewBag.CaseTypeName = GetDictById(SYS_Code_Dict._案例类型);
            this.ViewBag.RentTypeName = GetDictById(SYS_Code_Dict._租金方式);
        }

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

        [NonAction]
        private IEnumerable<SelectListItem> GetSubAreaNamesByAreaId(int areaId)
        {
            var list = new List<SelectListItem>();
            var subAreaNames = _industrySubArea.GetSubAreaNamesByAreaId(areaId, Passport.Current.FxtCompanyId, Passport.Current.CityId);

            subAreaNames.ToList().ForEach(m => list.Add(new SelectListItem
                {
                    Value = m.SubAreaId.ToString(),
                    Text = m.SubAreaName,
                }));

            list.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return list;
        }

        [HttpPost]
        public ActionResult GetSubAreaName(int areaId)
        {
            var result = GetSubAreaNamesByAreaId(areaId);
            return Json(result);
        }

        //[HttpPost]
        //public ActionResult GetProjectBizAddress(int projectId)
        //{
        //    var self = true; int operate;
        //    PermissionLevelCheck.Check(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
        //    if (operate == (int)PermissionLevel.All) self = false;

        //    var projects = _industryProject.GetProjectIndustrys(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectId: projectId, self: self);
        //    var result = projects.FirstOrDefault().Address;
        //    return Json(projectId > 0 ? result : "");
        //}

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

        //自动匹配商家信息
        [HttpGet]
        public JsonResult ProjectNameSelect(int areaId, int subAreaId)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.All) self = false;

            var projects = _industryProject.GetProjectIndustrys(Passport.Current.CityId, Passport.Current.FxtCompanyId, areaId, subAreaId, self: self);
            var data = new List<dynamic>();
            foreach (var item in projects)
            {
                data.Add(new { name = item.ProjectName, id = item.ProjectId, address = item.Address });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
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
