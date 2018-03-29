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
    public class PropertyAddressController : BaseController
    {
        private readonly IPropertyAddress _propertyAddress;
        private readonly ILog _log;
        private readonly IImportTask _importTask;
        private readonly IProjectCase _projectcase;

        public PropertyAddressController(IPropertyAddress propertyAddress, ILog log, IImportTask importTask, IProjectCase projectcase)
        {
            this._propertyAddress = propertyAddress;
            this._log = log;
            this._importTask = importTask;
            this._projectcase = projectcase;
        }

        public ActionResult Index(string Name, int? pageIndex)
        {
            var projectself = true;
            int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None) return View();
            if (projectoperate == (int)PermissionLevel.All) projectself = false;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.房产证地址, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            var result = _propertyAddress.GetPropertyAddress(Name, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (projectself)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.projectfxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.projectcreator == Passport.Current.UserName);
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
            result = result.OrderBy(t => t.projectid).ThenBy(t => t.propertyaddress);
            var viewModel = result.ToPagedList(pageIndex ?? 0, 30);
            return View(viewModel);
        }

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.房产证地址, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(LNK_P_PAddress pa)
        {
            try
            {
                if (pa.projectid == null || pa.projectid <= 0)
                {
                    return base.Back("对不起，请选择系统中已有的楼盘！");
                }
                if (string.IsNullOrWhiteSpace(pa.propertyaddress))
                {
                    return base.Back("请填写房产证地址！");
                }
                var p = _propertyAddress.IsExistPropertyAddressByProjectid((int)pa.projectid, pa.propertyaddress, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (p != null)
                {
                    return base.Back("对不起，改楼盘中已存在相同的房产证地址！");
                }

                pa.cityid = Passport.Current.CityId;
                pa.fxtcompanyid = Passport.Current.FxtCompanyId;
                pa.creator = Passport.Current.UserName;
                pa.createdatetime = DateTime.Now;

                _propertyAddress.AddPropertyAddress(pa);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房产证地址, SYS_Code_Dict.操作.新增, "", "", "新增房产证地址", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/PropertyAddress/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var result = _propertyAddress.GetPropertyAddressById(id, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            ViewBag.select2ProjectId = result.projectid;
            ViewBag.select2ProjectName = result.projectname + "(" + result.areaname + ")";
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(LNK_P_PAddress pa, string propertyaddress)
        {
            if (pa.fxtcompanyid != Passport.Current.FxtCompanyId)
            {
                return this.RefreshParent("对不起，您没有修改该条数据的权限！");
            }
            if (pa.projectid == null || pa.projectid <= 0)
            {
                return base.Back("对不起，请选择系统中已有的楼盘！");
            }
            if (string.IsNullOrWhiteSpace(propertyaddress))
            {
                return this.RefreshParent("请填写房产证地址！");
            }

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.房产证地址, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None) return this.RefreshParent("对不起，您没有修改权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            try
            {
                pa.cityid = Passport.Current.CityId;
                pa.creator = Passport.Current.UserName;
                pa.createdatetime = DateTime.Now;

                var p = _propertyAddress.GetPropertyAddressById(pa.id, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (self && p.creator != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    return this.RefreshParent("对不起，该条数据您没有修改权限！");
                }
                var p1 = _propertyAddress.IsExistPropertyAddressByProjectid((int)pa.projectid, propertyaddress, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (p1 != null)
                {
                    return this.RefreshParent("对不起，系统中已存在相同的房产证地址！");
                }

                _propertyAddress.UpdatePropertyAddress(pa);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房产证地址, SYS_Code_Dict.操作.修改, "", "", "修改房产证地址", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/PropertyAddress/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        //删除
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.房产证地址, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    var result = _propertyAddress.GetPropertyAddressById(item, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                    if (result.fxtcompanyid != Passport.Current.FxtCompanyId)
                    {
                        failList.Add(item);
                        continue;
                    }
                    if (self && result.creator != Passport.Current.UserName && Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                    {
                        failList.Add(item);
                        continue;
                    }
                    _propertyAddress.DeletePropertyAddress(item);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房产证地址, SYS_Code_Dict.操作.删除, "", "", "删除房产证地址", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/PropertyAddress/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //导出
        [HttpGet]
        public ActionResult Export(string projectname)
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
            Permission.Check(SYS_Code_Dict.住宅数据分类.房产证地址, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var result = _propertyAddress.GetPropertyAddress(projectname, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (projectself)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.projectfxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId));
                }
                else
                {
                    result = result.Where(t => t.projectcreator == Passport.Current.UserName);
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
            result = result.OrderBy(t => t.projectid).ThenBy(t => t.propertyaddress);

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition", "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_导出房产证地址_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房产证地址, SYS_Code_Dict.操作.导出, "", "", "导出房产证地址", RequestHelper.GetIP());

            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.房产证地址, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.房产证地址, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房产证地址, SYS_Code_Dict.操作.导入, "", "", "导入房产证地址", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "PropertyAddress");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/PropertyAddress/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("UploadFile");
        }

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

        //删除导入记录
        public ActionResult DeletePropertyAddressImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/PropertyAddress/DeletePropertyAddressImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        [HttpGet]
        public JsonResult ProjectSelect(string key, string projectId)
        {
            var projectself = true;
            int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None) return null;
            if (projectoperate == (int)PermissionLevel.All) projectself = false;

            var result = _projectcase.GetProjectList(Passport.Current.FxtCompanyId, Passport.Current.CityId).AsQueryable();
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

    }
}
