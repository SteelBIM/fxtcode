using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Domain.Models.QueryObjects;
using FXT.DataCenter.Domain.Models.QueryObjects.House;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Areas.House.Models;
using FXT.DataCenter.WebUI.Infrastructure.ModelBinder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using System.IO;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class ProjectCaseController : BaseController
    {
        private readonly IDropDownList _dropDownList;
        private readonly IProjectCase _projectCase;
        private readonly ILog _log;
        private readonly IProjectCaseTask _projectCaseTask;
        private readonly IImportTask _importTask;
        private readonly IDAT_Project _datProject;
        private readonly IDAT_Building _datBuilding;
        private readonly IWaitBuildingProject _waitBuilding;


        public ProjectCaseController(IImportTask importTask, IDAT_Project datProject, IDAT_Building datBuilding, IProjectCaseTask projectCaseTask, IDropDownList dropDownList, IProjectCase projectCase, ILog log, IWaitBuildingProject waitBuilding)
        {
            this._projectCaseTask = projectCaseTask;
            this._dropDownList = dropDownList;
            this._projectCase = projectCase;
            this._log = log;
            this._importTask = importTask;
            this._datProject = datProject;
            this._datBuilding = datBuilding;
            this._waitBuilding = waitBuilding;
        }

        public ActionResult Index(ProjectCaseParams request, int? searchId)
        {
            ViewBag.fxtcompanyid = Passport.Current.FxtCompanyId;
            BindViewData(request.areaid, request.caseTypeCode, request.purposeCode, request.buildingTypeCode);
            if (request.casedateStart == null || request.casedateEnd == null)
            {
                return View();
            }

            var projectself = true; int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None) return View();
            if (projectoperate == (int)PermissionLevel.All) projectself = false;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            request.cityid = Passport.Current.CityId;
            request.fxtcompanyid = Passport.Current.FxtCompanyId;
            request.casedateStart = Convert.ToDateTime(request.casedateStart.Value.ToString("yyyy-MM-dd") + " 00:00:00");
            request.casedateEnd = Convert.ToDateTime(request.casedateEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59");

            var result = _projectCase.GetProjectCase(request).AsQueryable();
            if (projectself)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.projectFxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.projectCreator == Passport.Current.UserName);
                }
            }
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.fxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.creator == Passport.Current.UserName);
                }
            }
            var ProjectCaseResult = result.ToPagedList(request.pageIndex, 30);
            return View(ProjectCaseResult);
        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindDropDownList();
            return View();
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(DAT_Case projectCase)
        {
            try
            {
                projectCase.cityid = Passport.Current.CityId;
                projectCase.fxtcompanyid = Passport.Current.FxtCompanyId;
                projectCase.createdate = DateTime.Now;
                projectCase.creator = Passport.Current.ID;
                projectCase.buildingid = _datBuilding.GetBuildingId(projectCase.projectid, projectCase.buildingname, Passport.Current.CityId, Passport.Current.FxtCompanyId);

                _projectCase.AddProjectCase(projectCase);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.新增, "", "", "新增住宅案例", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

            return this.RefreshParent();
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var projectCase = _projectCase.GetProjectCaseById(int.Parse(splitArray[1]), int.Parse(splitArray[0]), Passport.Current.CityId).FirstOrDefault();
            this.ViewBag.select2ProjectId = projectCase.projectid;
            this.ViewBag.select2ProjectName = projectCase.ProjectName;

            BindDropDownList();
            return View("Create", projectCase);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(DAT_Case model)
        {
            try
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                if (model.fxtcompanyid != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，您不能修改其他公司数据！");
                }
                if (self && model.creator != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))//self && model.creator != Passport.Current.UserName
                {
                    return base.AuthorizeWarning("对不起，您没有该条数据的修改权限！");
                }

                model.savedatetime = DateTime.Now;
                model.saveuser = Passport.Current.ID;
                model.cityid = Passport.Current.CityId;
                model.fxtcompanyid = Passport.Current.FxtCompanyId;
                model.buildingid = _datBuilding.GetBuildingId(model.projectid, model.buildingname, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                _projectCase.UpdateProjectCase(model);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.修改, model.caseid.ToString(), "", "修改住宅案例", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
            return this.RefreshParent();
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
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    int fxtcompanyid = int.Parse(array[0]);
                    int caseid = int.Parse(array[1]);
                    var projectCase = _projectCase.GetProjectCaseById(caseid, fxtcompanyid, Passport.Current.CityId).FirstOrDefault();
                    if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                    {
                        failList.Add(array[1]);
                        continue;
                    }
                    if (self && projectCase.creator != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))//self && projectCase.creator != Passport.Current.UserName
                    {
                        failList.Add(array[1]);
                        continue;
                    }

                    _projectCase.DeleteProjectCase(caseid, Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除住宅案例", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //一键删除
        [HttpPost]
        public ActionResult DeleteAll(int areaid, DateTime casedateStart, DateTime casedateEnd, int caseTypeCode, decimal? buildingAreaFrom, decimal? buildingAreaTo, int purposeCode, decimal? unitPriceFrom, decimal? unitPriceTo, string key, int buildingTypeCode)
        {
            try
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                if (self)
                {
                    return Json(new { result = false, msg = "对不起，您只有删除自己的权限，无法操作该功能！" });
                }

                casedateStart = Convert.ToDateTime(casedateStart.ToString("yyyy-MM-dd") + " 00:00:00");
                casedateEnd = Convert.ToDateTime(casedateEnd.ToString("yyyy-MM-dd") + " 23:59:59");
                var result = _projectCase.GetProjectCaseAll(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName, areaid, casedateStart, casedateEnd, caseTypeCode, buildingAreaFrom, buildingAreaTo, purposeCode, unitPriceFrom, unitPriceTo, key, buildingTypeCode, self);
                if (result > 1000)
                {
                    return Json(new { result = false, msg = "删除条数超过1000条，删除失败！" });
                }

                result = _projectCase.DeleteProjectCaseAll(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName, areaid, casedateStart, casedateEnd, caseTypeCode, buildingAreaFrom, buildingAreaTo, purposeCode, unitPriceFrom, unitPriceTo, key, buildingTypeCode, self);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.删除, "", "DeleteAll", "删除住宅案例", RequestHelper.GetIP());

                return Json(new { result = true, msg = "您已删除" + result + "条数据！" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/DeleteAll", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        ////管理员删除案例DeleteCase
        //[HttpGet]
        //[DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.删除全部)]
        //public ActionResult DeleteCaseCount(string createtimefrom, string createtimeto,string creator)
        //{
        //    if (string.IsNullOrWhiteSpace(createtimefrom))
        //    {
        //        return Json(new { result = false, msg = "删除失败！" });
        //    }
        //    var data = _projectCase.DeleteCaseCount(Passport.Current.FxtCompanyId, Passport.Current.CityId, createtimefrom, createtimeto, creator);
        //    return Json(new { result = true, msg = "共计" + data + "条案例。" });
        //}

        //删除重复案例(暂定：操作者具有‘删除全部’的权限，才能进行当前操作)
        [HttpPost]
        public ActionResult DeleteSameCase(string time)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，该条数据您没有删除权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }
            if (self)
            {
                return base.AuthorizeWarning("对不起，您只有删除自己的权限，无法操作该功能！");
            }

            DateTime caseDateFrom = DateTime.Now, caseDateTo = DateTime.Now;

            switch (time)
            {
                case "0": //近一个月
                    caseDateFrom = DateTime.Now.AddMonths(-1);
                    caseDateTo = DateTime.Now;
                    break;
                case "1"://近三个月
                    caseDateFrom = DateTime.Now.AddMonths(-3);
                    caseDateTo = DateTime.Now;
                    break;
                case "2"://近半年
                    caseDateFrom = DateTime.Now.AddMonths(-6);
                    caseDateTo = DateTime.Now;
                    break;
                case "3"://近一年
                    caseDateFrom = DateTime.Now.AddYears(-1);
                    caseDateTo = DateTime.Now;
                    break;
            }
            var result = _projectCase.DeleteSameProjectCaseCount(Passport.Current.FxtCompanyId, Passport.Current.CityId, caseDateFrom, caseDateTo);

            // 异步调用WCF服务
            var cityId = Passport.Current.CityId;
            var fxtCompanyId = Passport.Current.FxtCompanyId;
            var saveUser = Passport.Current.UserName;
            Task.Factory.StartNew(() =>
            {
                var client = new ResidentialServices.ResidentialClient();
                client.DeleteSameProjectCaseAsync(fxtCompanyId, cityId, caseDateFrom, caseDateTo, saveUser);
                try
                {
                    client.Close();
                }
                catch
                {
                    client.Abort();
                }

            });

            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.删除, "", "", "删除重复案例", RequestHelper.GetIP());
            return base.Back("已删除" + result + "条案例数据。");
        }

        //删除异常案例(暂定：操作者具有‘删除全部’的权限，才能进行当前操作)
        [HttpGet]
        public ActionResult ExceptionCase()
        {
            BindDropDownList();
            return View();
        }

        //删除异常案例
        [HttpPost]
        public ActionResult ExceptionCase(ExceptionCaseParams ec)
        {
            try
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有删除权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                if (self)
                {
                    return base.AuthorizeWarning("对不起，您只有删除自己的权限，无法操作该功能！");
                }

                ec.cityId = Passport.Current.CityId;
                ec.fxtCompanyId = Passport.Current.FxtCompanyId;
                ec.SaveUserName = Passport.Current.UserName;
                ec.Uprate = ec.Uprate / 100;
                ec.Downrate = 0 - ec.Downrate / 100;

                var result = _projectCase.DeleteExProjectCase(ec);

                //日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.删除, "", "", "删除异常案例", RequestHelper.GetIP());
                return base.Back("已删除" + result + "条案例数据。");
                //return base.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/DeleteExCase", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }

        }
        //导出
        public ActionResult Export([JsonModelBinder]ProjectCaseParams request)
        {
            var projectself = true; int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有基础数据的查看权限！");
            }
            if (projectoperate == (int)PermissionLevel.All)
            {
                projectself = false;
            }

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            request.cityid = Passport.Current.CityId;
            request.fxtcompanyid = Passport.Current.FxtCompanyId;
            request.casedateStart = Convert.ToDateTime(request.casedateStart.Value.ToString("yyyy-MM-dd") + " 00:00:00");
            request.casedateEnd = Convert.ToDateTime(request.casedateEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59");
            var result = _projectCase.GetProjectCase(request).AsQueryable();
            if (projectself)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.projectFxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.projectCreator == Passport.Current.UserName);
                }
            }
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.fxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.creator == Passport.Current.UserName);
                }
            }

            //操作日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.导出, "", "", "导出楼盘案例", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition", "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼盘案例_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }
        //exce导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.楼盘案例, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectCaseResult = result.ToPagedList(pageIndex ?? 1, 30);

            return View(projectCaseResult);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());

            if (null != file)
            {
                try
                {
                    //获得文件名
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    SaveFile(file, folder, filename);

                    //操作日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.导入, "", "", "导入住宅案例", RequestHelper.GetIP());

                    // 异步调用WCF服务
                    var filePath = Path.Combine(folder, filename);
                    var cityId = Passport.Current.CityId;
                    var fxtCompanyId = Passport.Current.FxtCompanyId;
                    var userId = Passport.Current.ID;
                    Task.Factory.StartNew(() =>
                    {
                        var client = new ExcelUploadServices.ExcelUploadClient();
                        client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                            taskNameHiddenValue, "ProjectCase");
                        try { client.Close(); }
                        catch { client.Abort(); }

                    });

                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/ProjectCase/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                    return this.Back("操作失败！");
                }

            }
            return this.RedirectToAction("UploadFile");
        }

        public ActionResult MatchProjectCase(List<string> ids)
        {
            try
            {
                string idstring = string.Empty;
                if (ids != null)
                {
                    foreach (string id in ids)
                    {
                        idstring += id + ",";
                    }
                }
                idstring = idstring.TrimEnd(',');
                var info = _projectCaseTask.MatchProjectCase(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName, idstring);
                if (info < 0)
                {
                    return Json(new { result = false, msg = "匹配失败！" });
                }
                return Json(new { result = true, msg = "自动匹配完成！" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/MatchProjectCase", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "匹配失败！" });
            }
        }

        //删除楼盘导入记录
        public ActionResult DeleteProjectCaseImportRecord(List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return Json(new { result = false, msg = "删除失败！" });
                }

                //删除导入任务时，同时删除临时库案例
                ids.ForEach(m =>
                {
                    _importTask.DeleteTask(Int64.Parse(m));
                    _projectCase.DeleteAllMisMatchProjectCase(Int32.Parse(m));
                });

                return Json(new { result = true, msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/DeleteProjectCaseImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        [HttpGet]
        public ActionResult MisMatchProject(int? taskid, int? pageIndex, string key)
        {
            this.ViewBag.taskId = taskid;
            var result = _projectCaseTask.GetDatCaseTemp(taskid ?? -1, Passport.Current.CityId, Passport.Current.FxtCompanyId, key);
            var model = result.ToPagedList(pageIndex ?? 1, 30);
            return View(model);
        }

        //删除不匹配楼盘
        [HttpPost]
        public ActionResult DeleteMisMatchProjectCase(int taskid, List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "请选择删除项！" });
            }
            try
            {
                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    string areaName = array[0];
                    string projectName = array[1];
                    int result = _projectCase.DeleteMisMatchProjectCase(taskid, areaName, projectName);
                    if (result > 0)
                    {
                        _importTask.UpdateCaseTaskNameErrNumber(taskid);
                    }
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除不匹配案例数据", RequestHelper.GetIP());

                return Json(new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/DeleteMisMatchProjectCase", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //删除所有不匹配楼盘
        [HttpPost]
        public ActionResult DeleteAllMisMatchProjectCase(int taskid)
        {
            try
            {
                int result = _projectCase.DeleteAllMisMatchProjectCase(taskid);
                if (result > 0)
                {
                    _importTask.UpdateCaseTaskNameErrNumber(taskid);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.删除, "taskid：" + taskid, "", "删除所有不匹配案例数据", RequestHelper.GetIP());

                return Json(new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/DeleteMisMatchProjectCase", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        [HttpGet]
        public ActionResult EditMisMatchProject(int taskId, string projectname, string areaname)
        {
            this.ViewBag.areaname = string.IsNullOrEmpty(areaname) ? "" : areaname.TrimEnd('?');
            this.ViewBag.projectname = projectname.TrimEnd('?');
            this.ViewBag.taskId = taskId;
            return View();
        }
        [HttpPost]
        public ActionResult EditMisMatchProject(int taskId, string sourceName, string areaName, int destId, string destName)
        {
            try
            {
                var updateResult = _projectCaseTask.UpdateDatCaseTemp(taskId, sourceName, areaName, destId, Passport.Current.CityId, Passport.Current.FxtCompanyId);

                if (updateResult == 0) return base.RefreshParent("操作失败！");

                var casetemps = _projectCaseTask.GetDatCaseTemp(taskId, Passport.Current.CityId, Passport.Current.FxtCompanyId, destId).ToList();

                //把案例临时数据表数据添加到案例表中并删除临时表数据
                //casetemps.ToList().ForEach(m =>
                //{
                //    _projectCase.AddProjectCase(m);
                //    _projectCaseTask.DelteDatCaseTemp(m.CaseID);
                //});

                //删除临时表数据
                var caseIds = casetemps.Select(m => m.CaseID).ToList();
                _projectCaseTask.DelteDatCaseTemp(caseIds);

                casetemps.ToList().ForEach(m =>
                {
                    m.Creator = Passport.Current.UserName;
                });
                //异步处理不需要马上响应的数据
                Task.Factory.StartNew(() =>
                {
                    //把案例临时表数据数据添加到案例表中
                    _projectCase.AddProjectCase(casetemps.ToArray());
                    var casetempslist = (from c in casetemps
                                         group c by c.TaskID into c1
                                         select new
                                         {
                                             taskId = c1.Key,
                                             totalnum = c1.Count()
                                         }).ToList();

                    foreach (var c in casetempslist)
                    {
                        //更新任务列表中楼盘名称不匹配数量
                        _importTask.UpdateTask(c.taskId, c.totalnum);
                    }
                });

                //添加网络名与楼盘名对应关系，以便下次自动校对
                var pm = new SYS_ProjectMatch
                {
                    CityId = Passport.Current.CityId,
                    FXTCompanyId = Passport.Current.FxtCompanyId,
                    NetName = sourceName,
                    NetAreaName = areaName,
                    ProjectName = destName,
                    ProjectNameId = destId,
                    CreateTime = DateTime.Now,
                    Creator = Passport.Current.UserName
                };
                _projectCaseTask.AddProjectMatch(pm);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/EditMisMatchProject", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("修改失败！");
            }
            return this.RefreshParent("修改成功！");
        }
        //添加到待建楼盘
        [HttpPost]
        public JsonResult AddToWaitProject(int taskId, string projectName)
        {
            try
            {
                var wp = _waitBuilding.GetSingleWaitProject(projectName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (wp != null)
                {
                    _projectCaseTask.UpdateDatCaseTempWaitProject(taskId, wp.WaitProjectId, projectName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                    return Json("该楼盘已在待建楼盘中存在！");
                }

                var waitProject = new Dat_WaitProject
                {
                    CityId = Passport.Current.CityId,
                    FxtCompanyId = Passport.Current.FxtCompanyId,
                    CreateDate = DateTime.Now,
                    UserId = Passport.Current.ID,
                    WaitProjectName = projectName
                };
                int waitProjectId = _waitBuilding.AddWaitProject(waitProject);
                _projectCaseTask.UpdateDatCaseTempWaitProject(taskId, waitProjectId, projectName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectCase/AddToWait", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json("操作失败！");
            }
            return Json("操作成功！");

        }

        //导出不匹配楼盘案例
        public ActionResult MisMatchProjectCaseExport(int taskid)
        {
            var dt = _projectCase.GetMisMatchProjectCase(taskid);
            if (dt != null && dt.Rows.Count > 0)
            {
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                curContext.Response.AddHeader("content-disposition",
                                                 "attachment;filename*=UTF-8''" +
                                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_不匹配楼盘案例_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                curContext.Response.Charset = "UTF-8";

                //日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.导出, "", "", "导出不匹配楼盘案例", RequestHelper.GetIP());

                using (var ms = ExcelHandle.RenderToExcel(dt))
                {
                    return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                }

            }
            return this.Back("没有您要导出的数据");

        }

        [NonAction]
        private static bool SaveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = true;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            try
            {
                postedFile.SaveAs(Path.Combine(filepath, saveName));
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                throw new ApplicationException(e.Message);
            }
            return result;
        }

        #region 统计
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅案例数据, SYS_Code_Dict.页面权限.统计)]
        public ActionResult Statistics(ProjectCaseStatistcParams pcs)
        {
            int? casemonth = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault().CaseMonth;
            ViewBag.casemonth = casemonth != null && casemonth > 0 ? casemonth : 6;
            BindViewData(false);
            ProjectCaseStastics model;
            switch (pcs.Type)
            {
                case "WorkLoad":
                    model = WorkLoad(pcs.pageIndex, pcs.CaseUpLoadtimeFrom.Trim(), pcs.CaseUpLoadtimeTo.Trim());
                    break;
                case "Statist":
                    model = ProjectStatist(pcs.pageIndex, pcs.CaseDateFrom.Trim(), pcs.CaseDateTo.Trim(), pcs.CaseTypeCode, pcs.Condition, pcs.Amount);
                    break;
                case "AvePrice":
                    //var avgPrice = new ProjectCase_AvgPrice { pageIndex = pcs.pageIndex, timeFrom = pcs.TimeFrom, timeTo = pcs.TimeTo, groupcycle = pcs.Groupcycle, grouparea = pcs.Grouparea, buildingtypecode = pcs.Buildingtypecode, purposecode = pcs.Purposecode, sampleproject = pcs.Sampleproject, buildingdatecode = pcs.Buildingdatecode };
                    //model = AvgPrice(avgPrice);
                    model = null;
                    break;
                case "ProjectEValue":
                    model = ProjectEValue(pcs.pageIndex, pcs.ProjectEValueTimeFrom, pcs.ProjectEValueTimeTo, pcs.peareaname, pcs.ProjectEValueProjectName,pcs.ProjectUEReason);
                    break;
                case "BuildingEValue":
                    model = BuildingEValue(pcs.pageIndex, pcs.BuildingEValueTimeFrom, pcs.BuildingEValueTimeTo, pcs.bpeareaname, pcs.BuildingEValueProjectName,pcs.BuildingUEReason);
                    break;
                default:
                    model = new ProjectCaseStastics();
                    break;
            }
            return View(model);
        }

        [NonAction]
        private void BindViewData(bool showAll = true)
        {
            this.ViewBag.cityname = Passport.Current.CityName;
            this.ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text"));
            this.ViewData.Add("subareaid", new SelectList(GetSubAreaName(0), "Value", "Text"));

            var purposeCodes = GetDictById(SYS_Code_Dict._居住用途);
            var buildingTypeCodes = GetDictById(SYS_Code_Dict._建筑类型);
            var buildingDateCodes = GetDictById(SYS_Code_Dict._建筑年代);
            if (!showAll)
            {
                purposeCodes.RemoveAt(0);
                buildingTypeCodes.RemoveAt(0);
                buildingDateCodes.RemoveAt(0);
            }

            ViewData.Add("purposecode", new SelectList(purposeCodes, "Value", "Text"));
            ViewData.Add("buildingtypecode", new SelectList(buildingTypeCodes, "Value", "Text"));
            ViewData.Add("buildingdatecode", new SelectList(buildingDateCodes, "Value", "Text"));
            this.ViewBag.CaseTypeName = GetDictById(SYS_Code_Dict._案例类型);
        }

        //工作量统计
        private ProjectCaseStastics WorkLoad(int pageIndex, string caseUpLoadtimeFrom, string caseUpLoadtimeTo)
        {
            this.ViewBag.active = "#divone";
            if (string.IsNullOrEmpty(caseUpLoadtimeFrom) || string.IsNullOrWhiteSpace(caseUpLoadtimeTo))
            {
                return new ProjectCaseStastics();
            }

            var result = _projectCase.GetProjectCaseCount(caseUpLoadtimeFrom, caseUpLoadtimeTo, Passport.Current.CityId, Passport.Current.FxtCompanyId).ToPagedList(pageIndex, 30);

            return new ProjectCaseStastics { WorkLoad = result };

        }

        //楼盘案例统计
        public ProjectCaseStastics ProjectStatist(int pageIndex, string casedateFrom, string casedateTo, int caseTypeCode, string condition, int? amount)
        {

            this.ViewBag.active = "#divtwo";

            if (string.IsNullOrEmpty(casedateFrom) || string.IsNullOrWhiteSpace(casedateTo))
            {
                return new ProjectCaseStastics();
            }

            casedateTo = casedateTo + " 23:59:59";

            var count = amount ?? 0;
            var result = CaseStatistData(casedateFrom, casedateTo, caseTypeCode, condition, count).ToPagedList(pageIndex, 30);
            return new ProjectCaseStastics { Statist = result };

        }

        [NonAction]
        private IEnumerable<ProjectCase_Statist> CaseStatistData(string casedateFrom, string casedateTo, int caseTypeCode, string condition, int count)
        {
            condition = condition == "0" ? ">=" : "<";
            var cityid = Passport.Current.CityId;
            var fxtcompanyid = Passport.Current.FxtCompanyId;

            var result = _projectCase.GetProjectCaseCount(casedateFrom, casedateTo, caseTypeCode, condition, count, cityid, fxtcompanyid);
            return result;
        }

        //楼盘案例导出
        public ActionResult CaseExport(string caseDateFrom, string caseDateTo, int caseTypeCode, string condition, int amount)
        {
            caseDateTo = caseDateTo + " 23:59:59";
            var result = CaseStatistData(caseDateFrom, caseDateTo, caseTypeCode, condition, amount);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.导出, "", "", "导出楼盘案例统计", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼盘案例统计_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        public JsonResult GetSubArea(int? areaid)
        {
            return Json(GetSubAreaName(areaid ?? 0), JsonRequestBehavior.AllowGet);
        }

        //楼盘均价统计
        //[HttpPost]
        //public ProjectCaseStastics AvgPrice(ProjectCase_AvgPrice pa)
        //{
        //    if (!pa.timeFrom.HasValue || !pa.timeTo.HasValue)
        //    {
        //        return new ProjectCaseStastics();
        //    }

        //    this.ViewBag.active = "#divthree";
        //    pa.cityid = Passport.Current.CityId;
        //    pa.fxtcompanyid = Passport.Current.FxtCompanyId;
        //    var result = _projectCase.GetProjectCaseAvePrice(pa).ToPagedList(pa.pageIndex, 30);
        //    //var model3 = result.ToPagedList(pa.pageIndex,  2 );
        //    // this.ViewBag.model3 = model3;
        //    return new ProjectCaseStastics { AvePrice = result };
        //}

        //案例均价导出
        [HttpGet]
        public ActionResult AvgPriceExport(ProjectCase_AvgPrice pa)
        {
            pa.cityid = Passport.Current.CityId;
            pa.fxtcompanyid = Passport.Current.FxtCompanyId;
            var result = _projectCase.GetProjectCaseAvePrice(pa);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.导出, "", "", "导出楼盘均价统计", RequestHelper.GetIP());

            //System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            //curContext.Response.AddHeader("content-disposition",
            //                                 "attachment;filename*=UTF-8''" +
            //                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_案例均价统计_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            //curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //curContext.Response.Charset = "UTF-8";
            //using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            //{
            //    return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            //}


            if (result != null && result.Rows.Count > 0)
            {
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                curContext.Response.AddHeader("content-disposition",
                                                 "attachment;filename*=UTF-8''" +
                                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_案例均价统计_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                curContext.Response.Charset = "UTF-8";

                using (var ms = ExcelHandle.RenderToExcel(result))
                {
                    return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                }
            }
            return this.Back("没有您要导出的数据");
        }

        //楼盘可估统计
        public ProjectCaseStastics ProjectEValue(int pageIndex, string datefrom, string dateto, List<int> peareaname, string projectEValueProjectName, string projectUEReason)
        {
            this.ViewBag.active = "#divfour";
            this.ViewBag.peareaname = peareaname == null ? "" : string.Join(",", peareaname);
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrWhiteSpace(dateto))
            {
                return new ProjectCaseStastics();
            }
            dateto = dateto + " 23:59:59";
            var result = _projectCase.GetProjectEValueCount(datefrom, dateto, peareaname, projectEValueProjectName, Passport.Current.CityId, Passport.Current.FxtCompanyId, projectUEReason).ToPagedList(pageIndex, 30);
            return new ProjectCaseStastics { ProjectEValue = result };
        }

        //楼盘可估导出
        public ActionResult ProjectEValueExport(string ProjectEValueTimeFrom, string ProjectEValueTimeTo, List<int> peareaname, string ProjectEValueProjectName, string ProjectUEReason)
        {
            ProjectEValueTimeTo = ProjectEValueTimeTo + " 23:59:59";
            var result = _projectCase.GetProjectEValueCount(ProjectEValueTimeFrom, ProjectEValueTimeTo, peareaname, ProjectEValueProjectName, Passport.Current.CityId, Passport.Current.FxtCompanyId, ProjectUEReason);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.导出, "", "", "导出楼盘可估统计", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼盘可估统计_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //楼栋可估统计
        public ProjectCaseStastics BuildingEValue(int pageIndex, string datefrom, string dateto, List<int> bpeareaname, string buildingEValueProjectName,string buildingUEReason)
        {
            this.ViewBag.active = "#divfive";
            this.ViewBag.bpeareaname = bpeareaname == null ? "" : string.Join(",", bpeareaname);
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrWhiteSpace(dateto))
            {
                return new ProjectCaseStastics();
            }
            dateto = dateto + " 23:59:59";
            var result = _projectCase.GetBuildingEValueCount(datefrom, dateto, bpeareaname, buildingEValueProjectName, Passport.Current.CityId, Passport.Current.FxtCompanyId, buildingUEReason).ToPagedList(pageIndex, 30);
            return new ProjectCaseStastics { BuildingEValue = result };
        }

        //楼栋可估导出
        public ActionResult BuildingEValueExport(string BuildingEValueTimeFrom, string BuildingEValueTimeTo, List<int> bpeareaname, string BuildingEValueProjectName, string BuildingUEReason)
        {
            BuildingEValueTimeTo = BuildingEValueTimeTo + " 23:59:59";
            var result = _projectCase.GetBuildingEValueCount(BuildingEValueTimeFrom, BuildingEValueTimeTo, bpeareaname, BuildingEValueProjectName, Passport.Current.CityId, Passport.Current.FxtCompanyId, BuildingUEReason);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.住宅案例, SYS_Code_Dict.操作.导出, "", "", "导出楼栋可估统计", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition", "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼栋可估统计_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        #endregion

        #region 帮助程序
        [NonAction]
        private void BindDropDownList()
        {
            var areaName = GetAreaName();
            var caseTypeName = GetDictById(SYS_Code_Dict._案例类型);
            var purposeName = GetDictById(SYS_Code_Dict._居住用途);
            var structureName = GetDictById(SYS_Code_Dict._户型结构);
            var buildingTypeName = GetDictById(SYS_Code_Dict._建筑类型);
            var houseTypeName = GetDictById(SYS_Code_Dict._户型);
            var frontName = GetDictById(SYS_Code_Dict._朝向);
            var momenyUnitCode = GetDictById(SYS_Code_Dict._币种);


            this.ViewBag.AreaName = areaName; //行政区
            this.ViewBag.CaseTypeName = caseTypeName;//案例类型
            this.ViewBag.PurposeName = purposeName; //居住用途
            this.ViewBag.StructureName = structureName; //_户型结构
            this.ViewBag.BuildingTypeName = buildingTypeName;//_建筑类型
            this.ViewBag.HouseTypeName = houseTypeName; //_户型
            this.ViewBag.FrontName = frontName; //_朝向
            this.ViewBag.MomenyUnitCode = momenyUnitCode; //_币种


        }

        [NonAction]
        private void BindViewData(int areaid, int caseTypeCode, int purposeCode, int buildingTypeCode)
        {
            ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text", areaid));
            ViewData.Add("caseTypeCode", new SelectList(GetDictById(SYS_Code_Dict._案例类型), "Value", "Text", caseTypeCode));
            ViewData.Add("purposeCode", new SelectList(GetDictById(SYS_Code_Dict._居住用途), "Value", "Text", purposeCode));
            ViewData.Add("buildingTypeCode", new SelectList(GetDictById(SYS_Code_Dict._建筑类型), "Value", "Text", buildingTypeCode));

        }
        //行政区列表
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
            //this.ViewBag.areaResult = areaResult;
            return areaResult;
        }

        //片区列表
        [NonAction]
        private List<SelectListItem> GetSubAreaName(int areaid)
        {
            var subArea = _dropDownList.GetSubAreaName(areaid);
            var subAreaResult = new List<SelectListItem>();
            subArea.ToList().ForEach(m =>
                subAreaResult.Add(
                new SelectListItem { Value = m.SubAreaId.ToString(), Text = m.SubAreaName }
                ));
            subAreaResult.Insert(0, new SelectListItem { Value = "-1", Text = "全部" });
            return subAreaResult;
        }
        //根据ID查找相应值
        [NonAction]
        private List<SelectListItem> GetDictById(int id)
        {
            var casetype = _dropDownList.GetDictById(id);
            var casetypeResult = new List<SelectListItem>();
            casetype.ToList().OrderBy(m => m.Code).ToList().ForEach(m =>
                casetypeResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            casetypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return casetypeResult;
        }

        //获取楼盘
        [HttpGet]
        public JsonResult ProjectSelect(string key)
        {
            var projectself = true;
            int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None) return null;
            if (projectoperate == (int)PermissionLevel.All) projectself = false;

            var result = _projectCase.GetProjectList(Passport.Current.FxtCompanyId, Passport.Current.CityId).AsQueryable();
            if (projectself)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.fxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.creator == Passport.Current.UserName).AsQueryable();
                }
            }

            var data = result.Where(m => string.IsNullOrWhiteSpace(m.othername) ? (m.projectname.Contains(key)) : (m.projectname.Contains(key) || m.othername.Contains(key)));

            return Json(data.Select(m => new { id = m.projectid, text = m.projectname + "(" + (string.IsNullOrEmpty(m.othername) ? "" : m.othername + " | ") + m.AreaName + ")" }).ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
