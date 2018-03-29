using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class ProjectOtherNameController : BaseController
    {

        private readonly IProjectOtherName _projectOtherName;
        private readonly ILog _log;
        private readonly IImportTask _importTask;
        private readonly IProjectCase _projectCase;

        public ProjectOtherNameController(IProjectOtherName projectOtherName, ILog log, IImportTask importTask, IProjectCase projectCase)
        {
            this._projectOtherName = projectOtherName;
            this._log = log;
            this._importTask = importTask;
            this._projectCase = projectCase;
        }

        public ActionResult Index(string name, int? pageIndex)
        {
            var projectself = true;
            int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None) return View();
            if (projectoperate == (int)PermissionLevel.All) projectself = false;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.楼盘别名, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            var model = new SYS_ProjectMatch
            {
                CityId = Passport.Current.CityId,
                FXTCompanyId = Passport.Current.FxtCompanyId,
                NetName = name,
                ProjectName = name
            };

            var result = _projectOtherName.GetProjectMatch(model).AsQueryable();
            if (projectself)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.projectFxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
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
                    result = result.Where(t => t.FXTCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.Creator == Passport.Current.UserName);
                }
            }
            result = result.OrderByDescending(t => t.CreateTime);
            var viewModel = result.ToPagedList(pageIndex ?? 0, 30);
            return View(viewModel);
        }
        //删除
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.楼盘别名, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
            if (operate == (int)PermissionLevel.None) return Json(new { result = false, msg = "您没有删除权限！" });
            if (operate == (int)PermissionLevel.All) self = false;

            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {
                var failList = new List<int>();
                foreach (var item in ids)
                {
                    var result = _projectOtherName.GetProjectMatchById(item, Passport.Current.CityId, Passport.Current.FxtCompanyId).FirstOrDefault();
                    if (self && result.Creator != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                    {
                        failList.Add(item);
                        continue;
                    }
                    _projectOtherName.DeleteProjectOtherName(item);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘别名, SYS_Code_Dict.操作.删除, "", "", "删除楼盘别名", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectOtherName/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.楼盘别名, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            return View("Edit");
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(SYS_ProjectMatch pm, string ProjectNameTemp)
        {
            try
            {
                pm.CityId = Passport.Current.CityId;
                pm.FXTCompanyId = Passport.Current.FxtCompanyId;
                pm.Creator = Passport.Current.UserName;
                pm.CreateTime = DateTime.Now;
                pm.NetAreaName = string.IsNullOrWhiteSpace(pm.NetAreaName) ? "" : pm.NetAreaName;
                pm.ProjectName = string.IsNullOrWhiteSpace(pm.ProjectName) ? ProjectNameTemp : pm.ProjectName;

                if (pm.ProjectNameId == null || pm.ProjectNameId <= 0)
                {
                    return base.Back("对不起，请选择系统中已有的楼盘！");
                }
                var result = _projectOtherName.GetProjectMatchProjectId(pm.NetName, pm.NetAreaName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (result != null) return base.Back("对不起，系统中已存在相同的网络名！");

                _projectOtherName.DeleteProjectOtherName(pm.NetName, pm.NetAreaName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                _projectOtherName.AddProjectOtherName(pm);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘别名, SYS_Code_Dict.操作.新增, "", "", "新增楼盘别名", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectOtherName/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }

            return this.RefreshParent();
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var result = _projectOtherName.GetProjectMatchById(id, Passport.Current.CityId, Passport.Current.FxtCompanyId).FirstOrDefault();

            ViewBag.select2ProjectId = result.ProjectNameId;
            ViewBag.select2ProjectName = result.ProjectName + "(" + result.AreaName + ")";

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(SYS_ProjectMatch pm, string ProjectNameTemp)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.楼盘别名, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None) return this.RefreshParent("对不起，您没有修改权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            try
            {
                pm.CityId = Passport.Current.CityId;
                pm.FXTCompanyId = Passport.Current.FxtCompanyId;
                pm.SaveUser = Passport.Current.UserName;
                pm.SaveTime = DateTime.Now;
                pm.NetAreaName = string.IsNullOrWhiteSpace(pm.NetAreaName) ? "" : pm.NetAreaName;
                pm.ProjectName = string.IsNullOrWhiteSpace(pm.ProjectName) ? ProjectNameTemp : pm.ProjectName;

                var othername = _projectOtherName.GetProjectMatchById(pm.Id, Passport.Current.CityId, Passport.Current.FxtCompanyId).FirstOrDefault();
                if (self && othername.Creator != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    return this.RefreshParent("对不起，该条数据您没有修改权限！");
                }
                var othername1 = _projectOtherName.GetProjectMatchProjectId(pm.NetName, pm.NetAreaName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (othername1 != null)
                {
                    return this.RefreshParent("对不起，系统中已存在相同的网络名！");
                }

                _projectOtherName.DeleteProjectOtherName(pm.NetName, pm.NetAreaName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                _projectOtherName.UpdateProjectOtherName(pm);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘别名, SYS_Code_Dict.操作.修改, "", "", "修改楼盘别名", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectOtherName/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        //导出
        [HttpGet]
        public ActionResult Export(string name)
        {
            var projectself = true; int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有查看基础数据的查看权限！");
            }
            if (projectoperate == (int)PermissionLevel.All)
            {
                projectself = false;
            }

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.楼盘别名, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var model = new SYS_ProjectMatch()
            {
                CityId = Passport.Current.CityId,
                FXTCompanyId = Passport.Current.FxtCompanyId,
                NetName = name,
                ProjectName = name
            };

            var result = _projectOtherName.GetProjectMatch(model).AsQueryable();
            if (projectself)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.projectFxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
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
                    result = result.Where(t => t.FXTCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.Creator == Passport.Current.UserName);
                }
            }

            _excelExportHeader(Passport.Current.CityName + "_楼盘别名_");

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘别名, SYS_Code_Dict.操作.导出, "", "", "导出楼盘别名", RequestHelper.GetIP());

            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.楼盘别名, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.楼盘别名, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectResult = result.ToPagedList(pageIndex ?? 1, 30);
            return View(projectResult);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId);

            if (null == file) return this.RedirectToAction("UploadFile");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘别名, SYS_Code_Dict.操作.导入, "", "", "导入楼盘别名", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "ProjectOtherName");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectOtherName/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("UploadFile");
        }

        //删除楼盘导入记录
        public ActionResult DeleteProjectOtherNameImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/ProjectOtherName/DeleteProjectOtherNameImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());
        //        //获得文件名
        //        var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
        //        var isSuccess = SaveFile(file, folder, filename);

        //        if (isSuccess)
        //        {
        //            string filePath = Path.Combine(folder, filename);
        //            var excelHelper = new ExcelHandle(filePath);
        //            var data = excelHelper.ExcelToDataTable("Sheet1", true);

        //            List<SYS_ProjectMatch> pmList;
        //            DataToList(data, out pmList);
        //            foreach (var pm in pmList)
        //            {
        //                var p = _projectOtherName.GetProjectMatchProjectId(pm.NetName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
        //                if (p != null)
        //                {
        //                    pm.Id = p.Id;
        //                    pm.NetAreaName = pm.AreaName;
        //                    pm.SaveUser = Passport.Current.UserName;
        //                    pm.SaveTime = DateTime.Now;
        //                    _projectOtherName.UpdateProjectOtherName(pm);
        //                }
        //                else
        //                {
        //                    pm.NetAreaName = pm.AreaName;
        //                    pm.Creator = Passport.Current.UserName;
        //                    pm.CreateTime = DateTime.Now;
        //                    _projectOtherName.AddProjectOtherName(pm);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog("House/ProjectOtherName/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
        //        return this.Back("操作失败");
        //    }

        //    return this.RefreshParent();
        //}

        //[NonAction]
        //private void DataToList(DataTable dt, out List<SYS_ProjectMatch> pmList)
        //{
        //    if (dt.Rows.Count > 0)
        //    {
        //        var result = new List<SYS_ProjectMatch>();
        //        var cityid = Passport.Current.CityId;
        //        var fxtcompanyid = Passport.Current.FxtCompanyId;
        //        _project = _projectCase.GetProjectList(Passport.Current.FxtCompanyId, Passport.Current.CityId);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            var projectTemp = _project.FirstOrDefault(m => m.projectname == dt.Rows[i]["*系统名"].ToString() && m.AreaName == dt.Rows[i]["*行政区"].ToString());
        //            result.Add(new SYS_ProjectMatch
        //            {
        //                CityId = cityid,
        //                FXTCompanyId = fxtcompanyid,
        //                NetName = dt.Rows[i]["*网络名"].ToString(),
        //                AreaName = dt.Rows[i]["*行政区"].ToString(),
        //                ProjectName = dt.Rows[i]["*系统名"].ToString(),
        //                ProjectNameId = projectTemp == null ? -1 : projectTemp.projectid
        //            });
        //        }
        //        pmList = result;
        //    }
        //    else
        //    {
        //        pmList = new List<SYS_ProjectMatch>();
        //    }
        //}

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

        #region Assist Func

        //获取楼盘
        [HttpGet]
        public JsonResult ProjectSelect(string key, string projectId)
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

            if (!string.IsNullOrEmpty(projectId))
            {
                result = result.Where(m => m.projectid.ToString() != projectId);
            }
            var data = result.Where(m => m.projectname.Contains(key));
            return Json(data.Select(m => new { id = m.projectid, text = m.projectname + "(" + m.AreaName + ")" }).ToList(), JsonRequestBehavior.AllowGet);
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
