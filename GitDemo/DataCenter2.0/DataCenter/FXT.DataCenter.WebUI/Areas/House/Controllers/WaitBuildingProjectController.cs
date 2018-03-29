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
using System.Data;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class WaitBuildingProjectController : BaseController
    {
        private readonly IWaitBuildingProject _waitBuildingProject;
        private readonly ILog _log;

        public WaitBuildingProjectController(IWaitBuildingProject waitBuildingProject, ILog log)
        {
            this._waitBuildingProject = waitBuildingProject;
            this._log = log;
        }

        public ActionResult Index(string name, int? pageIndex)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.待建楼盘, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            var waitProjectList = _waitBuildingProject.GetWaitProject(name, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    waitProjectList = waitProjectList.Where(t => t.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    waitProjectList = waitProjectList.Where(t => t.UserId == Passport.Current.UserName);
                }
            }
            waitProjectList = waitProjectList.OrderByDescending(t => t.CreateDate);
            var model = waitProjectList.ToPagedList(pageIndex ?? 1, 30);
            if (Request.IsAjaxRequest())
                return PartialView("_Projects", model);
            return View(model);
        }

        //删除
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            try
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.待建楼盘, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None) return Json(new { result = false, msg = "无权限删除！" });
                if (operate == (int)PermissionLevel.All) self = false;

                var failList = new List<int>();
                foreach (var item in ids)
                {
                    var wp = _waitBuildingProject.GetWaitProjectById(item).FirstOrDefault();
                    if (self && wp.UserId != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                    {
                        failList.Add(item);
                        continue;
                    }
                    _waitBuildingProject.DeleteWaitProject(item);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.待建楼盘, SYS_Code_Dict.操作.删除, "", "", "删除待建楼盘", RequestHelper.GetIP());
                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/WaitBuildingProject/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //验证待建楼盘是否存在
        public ActionResult IsExist(string waitProjectName)
        {
            var waitProject = _waitBuildingProject.GetSingleWaitProject(waitProjectName, Passport.Current.CityId,
                    Passport.Current.FxtCompanyId);
            return Json(waitProject == null, JsonRequestBehavior.AllowGet);
        }

        //创建
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.待建楼盘, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            return View("Edit");
        }
        //创建保存
        [HttpPost]
        public ActionResult Create(string waitProjectName)
        {
            try
            {

                //var waitProject = _waitBuildingProject.GetSingleWaitProject(waitProjectName, Passport.Current.CityId,
                //    Passport.Current.FxtCompanyId);
                //if (waitProject != null)
                //{
                //    return base.Back("已存在该待建楼盘！");
                //}

                var wp = new Dat_WaitProject
                {
                    CityId = Passport.Current.CityId,
                    FxtCompanyId = Passport.Current.FxtCompanyId,
                    CreateDate = DateTime.Now,
                    UserId = Passport.Current.ID,
                    WaitProjectName = waitProjectName
                };

                _waitBuildingProject.AddWaitProject(wp);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.待建楼盘, SYS_Code_Dict.操作.新增, "", "", "新增待建楼盘", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/WaitBuildingProject/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }

            return this.RefreshParent();
        }
        //编辑
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var wp = _waitBuildingProject.GetWaitProjectById(id).FirstOrDefault();
            this.ViewBag.WaitProjectId = id;
            this.ViewBag.WaitProjectName = wp.WaitProjectName;
            return View();
        }
        //编辑保存
        [HttpPost]
        public ActionResult Edit(int waitProjectId, string waitProjectName)
        {
            try
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.待建楼盘, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None) return this.RefreshParent("您没有修改权限！"); ;
                if (operate == (int)PermissionLevel.All) self = false;
                var wp = _waitBuildingProject.GetWaitProjectById(waitProjectId).FirstOrDefault();
                if (self && wp.UserId != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    return this.RefreshParent("您没有该条数据的修改权限！");
                }

                _waitBuildingProject.UpdateWaitProject(waitProjectId, waitProjectName);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.待建楼盘, SYS_Code_Dict.操作.修改, "", "", "编辑待建楼盘", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/WaitBuildingProject/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        [HttpGet]
        public ActionResult ToProject(string chValue)
        {
            int waitProjectId = Int16.Parse(chValue.Split('#')[1]);
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.待建楼盘, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.Back("您没有修改权限！"); ;
            if (operate == (int)PermissionLevel.All) self = false;
            var wp = _waitBuildingProject.GetWaitProjectById(waitProjectId).FirstOrDefault();
            if (self && wp.UserId != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
            {
                return base.Back("您没有该条数据的修改权限！");
            }

            return this.RedirectToAction("Create", "Project", new { waitId = chValue, isWait = true });
        }

        //导出
        [HttpGet]
        public ActionResult Export(string name)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.待建楼盘, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            var waitProjectList = _waitBuildingProject.GetWaitProject(name, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    waitProjectList = waitProjectList.Where(t => t.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    waitProjectList = waitProjectList.Where(t => t.UserId == Passport.Current.UserName);
                }
            }
            waitProjectList = waitProjectList.OrderByDescending(t => t.CreateDate);

            _excelExportHeader(Passport.Current.CityName + "_待建楼盘_");
            var ms = ExcelHandle.ListToExcel(waitProjectList.ToList());

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘别名, SYS_Code_Dict.操作.导出, "", "", "导出待建楼盘", RequestHelper.GetIP());

            return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
        }

        //excel 导出头部信息
        readonly Action<string> _excelExportHeader = m =>
        {
            var curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition","attachment;filename*=UTF-8''" +
                                             HttpUtility.UrlEncode(m, System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
        };

        //导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.待建楼盘, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                var isSuccess = SaveFile(file, folder, filename);

                if (isSuccess)
                {
                    var filePath = Path.Combine(folder, filename);
                    var excelHelper = new ExcelHandle(filePath);
                    var data = excelHelper.ExcelToDataTable("Sheet1", true);

                    var cityId = Passport.Current.CityId;
                    var fxtCompanyId = Passport.Current.FxtCompanyId;

                    List<Dat_WaitProject> wpList;
                    DataToList(data, out wpList);
                    foreach (var item in wpList)
                    {
                        var waitProject = _waitBuildingProject.GetSingleWaitProject(item.WaitProjectName, cityId,
                  fxtCompanyId);
                        if (waitProject != null)
                        {
                            continue;
                        }
                        _waitBuildingProject.AddWaitProject(item);
                    }
                    // wpList.ForEach(m => _waitBuildingProject.AddWaitProject(m));
                }
            }
            catch (Exception ex)
            {
                return this.RefreshParent("操作失败");
            }

            return this.RefreshParent();
        }

        [NonAction]
        private static bool SaveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            var result = false;
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
                throw new ApplicationException(e.Message);
            }
            return result;
        }
        [NonAction]
        private static void DataToList(DataTable dt, out List<Dat_WaitProject> wpList)
        {
            if (dt.Rows.Count > 0)
            {
                var result = new List<Dat_WaitProject>();
                var cityid = Passport.Current.CityId;
                var fxtcompanyid = Passport.Current.FxtCompanyId;
                var userid = Passport.Current.ID;
                
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    result.Add(new Dat_WaitProject
                    {
                        CityId = cityid,
                        FxtCompanyId = fxtcompanyid,
                        CreateDate = DateTime.Now,
                        UserId = userid,
                        WaitProjectName = dt.Rows[i][0].ToString()

                    });
                }
                wpList = result;
            }
            else
            {
                wpList = new List<Dat_WaitProject>();
            }
        }

    }
}
