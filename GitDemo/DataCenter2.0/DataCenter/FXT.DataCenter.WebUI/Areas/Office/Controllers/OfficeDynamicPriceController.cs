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
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Office.Controllers
{
    [Authorize]
    public class OfficeDynamicPriceController : BaseController
    {
        private readonly IOfficeDynamicPrice _dynamicPriceSurvey;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IImportTask _importTask;
        private readonly IOfficeProject _officeProject;
        private readonly IOfficeBuilding _officeBuilding;

        public OfficeDynamicPriceController(IOfficeDynamicPrice dynamicPriceSurvey, ILog log, IDropDownList dropDownList, IImportTask importTask, IOfficeProject officeProject, IOfficeBuilding officeBuilding)
        {
            this._dynamicPriceSurvey = dynamicPriceSurvey;
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
            this._officeProject = officeProject;
            this._officeBuilding = officeBuilding;
        }

        public ActionResult Index(DatPbPriceOffice dynamicPriceSurvey, int? pageIndex)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.动态价格, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            
            dynamicPriceSurvey.CityId = Passport.Current.CityId;
            dynamicPriceSurvey.FxtCompanyId = Passport.Current.FxtCompanyId;

            var pageSize = 30; int totalCount;
            var result = _dynamicPriceSurvey.GetDynamicPriceSurveys(dynamicPriceSurvey, pageIndex ?? 1, pageSize, out totalCount, self).AsQueryable();
            var dynamicPriceResult = new PagedList<DatPbPriceOffice>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", dynamicPriceResult);
        }

        //删除
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.动态价格, SYS_Code_Dict.页面权限.删除自己)]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                //return this.Back("请选择要删除的数据！");
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
                    var result = _dynamicPriceSurvey.GetDynamicPriceSurveyById(int.Parse(array[1]));
                    result.SaveDateTime = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _dynamicPriceSurvey.DeleteDynamicPriceSurvey(result);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼盘价格调查, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除动态价格调查", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeDynamicPrice/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.动态价格, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindDropDownList(-1);
            return View();
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(DatPbPriceOffice dynamicPriceSurvey)
        {
            try
            {
                dynamicPriceSurvey.CityId = Passport.Current.CityId;
                dynamicPriceSurvey.FxtCompanyId = Passport.Current.FxtCompanyId;
                dynamicPriceSurvey.CreateTime = DateTime.Now;
                dynamicPriceSurvey.Creator = Passport.Current.ID;
                _dynamicPriceSurvey.AddDynamicPriceSurvey(dynamicPriceSurvey);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼盘价格调查, SYS_Code_Dict.操作.新增, dynamicPriceSurvey.Id.ToString(), "", "新增动态价格调查", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeDynamicPrice/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

            return this.RefreshParent();
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公案例数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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

            var dynamicPriceSurvey = _dynamicPriceSurvey.GetDynamicPriceSurveyById(int.Parse(splitArray[1]));

            BindDropDownList(dynamicPriceSurvey.ProjectId);
            return View("Create", dynamicPriceSurvey);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(DatPbPriceOffice dynamicPriceSurvey)
        {
            try
            {
                if (dynamicPriceSurvey.FxtCompanyId != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
                dynamicPriceSurvey.SaveDateTime = DateTime.Now;
                dynamicPriceSurvey.SaveUser = Passport.Current.ID;
                _dynamicPriceSurvey.UpdateDynamicPriceSurvey(dynamicPriceSurvey);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼盘价格调查, SYS_Code_Dict.操作.修改, dynamicPriceSurvey.Id.ToString(), "", "修改动态价格调查", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeDynamicPrice/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
            return this.RefreshParent();
        }

        //导出
        public ActionResult Export(int? projectId)
        {

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.动态价格, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;


            var dynamicPriceSurvey = new DatPbPriceOffice
            {
                ProjectId = projectId ?? -1,
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId
            };

            int totalCount;
            var result = _dynamicPriceSurvey.GetDynamicPriceSurveys(dynamicPriceSurvey, 1, int.MaxValue, out totalCount, self);

            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼盘价格调查, SYS_Code_Dict.操作.导出, "", "", "动态价格调查", RequestHelper.GetIP());

            _excelExportHeader(Passport.Current.CityName + "_办公_动态价格_");
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //exce导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.动态价格, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.办公动态价格, Passport.Current.CityId, Passport.Current.FxtCompanyId);

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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼盘价格调查, SYS_Code_Dict.操作.导入, "", "", "导入动态价格", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "OfficeDynamicPrice");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/DynamicPriceSurvey/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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


        //删除楼盘导入记录
        public ActionResult DeleteDynamicImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Office/OfficeDynamicPrice/DeleteDynamicImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #region 帮助程序
        //获取商业楼栋
        [HttpPost]
        public ActionResult GetOfficeBuilding(long projectId)
        {
            var result = GetOfficeBuildingById(projectId);
            return Json(result);
        }

        //下拉列表数据绑定
        private void BindDropDownList(long? projectId)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.动态价格, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.All) self = false;

            var projects = _officeProject.GetOfficeProjects(Passport.Current.CityId, Passport.Current.FxtCompanyId, -1, -1, -1, self);
            var result = new List<SelectListItem>();
            projects.ToList().ForEach(m => result.Add(new SelectListItem { Value = m.ProjectId.ToString(), Text = m.ProjectName + "(" + m.AreaName + ")" }));
            result.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });

            this.ViewBag.ProjectName = result;
            this.ViewBag.BuildingName = GetOfficeBuildingById(projectId);
            this.ViewBag.SurveyTypeName = GetDictById(SYS_Code_Dict._调查方式);
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetOfficeBuildingById(long? projectId)
        {
            var result = _officeBuilding.GetOfficeBuildings(projectId ?? -1, Passport.Current.FxtCompanyId);
            var selectListItems = new List<SelectListItem>();
            result.ToList().ForEach(m => selectListItems.Add(new SelectListItem { Value = m.BuildingId.ToString(), Text = m.BuildingName }));
            selectListItems.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            return selectListItems;
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

        //excel 导出头部信息
        readonly Action<string> _excelExportHeader = m =>
        {
            var curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             HttpUtility.UrlEncode(m, System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

        };

        #endregion
    }
}
