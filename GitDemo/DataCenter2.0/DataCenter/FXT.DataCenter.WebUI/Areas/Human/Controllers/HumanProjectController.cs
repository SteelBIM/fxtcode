using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.QueryObjects;
using FXT.DataCenter.Domain.Services;
//using FXT.DataCenter.Entity;
//using FXT.DataCenter.Entity.POCO.StatisticsModel;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.BatchAddPicture;
using FXT.DataCenter.WebUI.Infrastructure.ModelBinder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;
using System.Configuration;
using FXT.DataCenter.Domain.Models.FxtProject;
using OfficeOpenXml;
using FXT.DataCenter.WebUI.Tools;
using System.Threading.Tasks;

namespace FXT.DataCenter.WebUI.Areas.Human.Controllers
{
    [Authorize]
    public class HumanProjectController : BaseController
    {
        private readonly IHumanProject _humanProject;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IImportTask _importTask;

        public HumanProjectController(IHumanProject humanProject, ILog log, IDropDownList dropDownList, IImportTask importTask)
        {
            this._humanProject = humanProject;
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
        }

        public ActionResult Index(int? areaid, string key, int? pageIndex)
        {
            BindViewData();
            #region 判断基础数据权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.业主信息分类.业主楼盘数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View(new PagedList<DAT_Human>(new List<DAT_Human>().AsQueryable(), 1, 10, 0));
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion

            var humanprojects = _humanProject.GetHumanProjects(areaid, string.IsNullOrWhiteSpace(key) ? key : key.Trim(), Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            var model = humanprojects.ToPagedList(pageIndex ?? 1, 30);
            return View(model);
        }

        #region 新增
        [HttpGet]
        public ActionResult Create()
        {
            var dh = new DAT_Human();
            dh.CityId = Passport.Current.CityId;
            dh.FxtcompanyId = Passport.Current.FxtCompanyId;
            BindDropDownList();
            return View("Edit", dh);
        }

        [HttpPost]
        public ActionResult Create(DAT_Human dh)
        {
            try
            {
                dh.CreateTime = DateTime.Now;
                dh.Creator = Passport.Current.UserName;
                dh.AreaId = _humanProject.GetProjectList(Passport.Current.FxtCompanyId, Passport.Current.CityId, dh.ProjectId).FirstOrDefault().AreaId;
                dh.IsGroup = 1;
                dh.Valid = 1;
                _humanProject.AddHumanProject(dh);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.业主楼盘信息, SYS_Code_Dict.操作.新增, "", "", "新增业主楼盘", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Human/HumanProject/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.RefreshParent("操作失败！");
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');
            if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
            {
                return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
            }

            BindDropDownList();
            var dh = _humanProject.GetHumanProjectById(int.Parse(splitArray[1]), Passport.Current.FxtCompanyId, Passport.Current.CityId);
            this.ViewBag.select2ProjectId = dh.ProjectId;
            this.ViewBag.select2ProjectName = dh.ProjectName;
            this.ViewBag.Sex = dh.Sex;

            return View(dh);
        }

        [HttpPost]
        public ActionResult Edit(DAT_Human dh)
        {
            try
            {
                dh.SaveTime = DateTime.Now;
                dh.Saver = Passport.Current.UserName;
                dh.AreaId = _humanProject.GetProjectList(Passport.Current.FxtCompanyId, Passport.Current.CityId, dh.ProjectId).FirstOrDefault().AreaId;
                _humanProject.UpdateHumanProject(dh);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.业主楼盘信息, SYS_Code_Dict.操作.新增, "", "", "新增业主楼盘", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Human/HumanProject/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.RefreshParent("操作失败！");
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region 删除

        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            try
            {
                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');

                    if (TryParseHelper.StrToInt32(array[0], -1) != Passport.Current.FxtCompanyId)
                    {
                        failList.Add(array[1]);
                        continue;
                    }

                    _humanProject.DeleteHumanProject(int.Parse(array[1]), Passport.Current.UserName, DateTime.Now);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.业主楼盘信息, SYS_Code_Dict.操作.删除, "", "", "删除业主楼盘数据", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Human/HumanProject/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "操作失败！" });
            }
        }

        #endregion

        #region 导出
        public ActionResult Export(int? areaid, string key, int? pageIndex)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.业主信息分类.业主楼盘数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            var result = _humanProject.ExportHumanProjects(areaid, key, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);

            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.业主信息分类.业主楼盘数据, SYS_Code_Dict.操作.导出, "", "", "导出业主楼盘", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_业主楼盘数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.GetBuffer(), "application/vnd.ms-excel");
            }
        }
        #endregion

        #region 导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.业主信息分类.业主楼盘数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.业主楼盘信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectCaseResult = result.ToPagedList(pageIndex ?? 1, 30);

            return View(projectCaseResult);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.业主楼盘信息, SYS_Code_Dict.操作.导入, "", "", "导入业主楼盘", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "HumanProject");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Human/HumanProject/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("UploadFile");
        }

        public ActionResult DeleteHumanProjectImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Human/HumanProject/DeleteHumanProjectImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #endregion

        [NonAction]
        private void BindViewData()
        {
            this.ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text"));
        }

        private void BindDropDownList()
        {
            this.ViewBag.AreaNameList = GetAreaName();
            this.ViewBag.AgeGroupList = GetDictById(SYS_Code_Dict._年龄段);
            this.ViewBag.EducationList = GetDictById(SYS_Code_Dict._学历);
            this.ViewBag.MarriageList = GetDictById(SYS_Code_Dict._婚姻状态);
            this.ViewBag.OccupationList = GetDictById(SYS_Code_Dict._行业大类);
            this.ViewBag.PositionList = GetDictById(SYS_Code_Dict._职位);
            this.ViewBag.SalaryList = GetDictById(SYS_Code_Dict._年薪资范围);
            this.ViewBag.TransportationList = GetDictById(SYS_Code_Dict._常用交通工具);
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

        //行政区列表
        [NonAction]
        private List<SelectListItem> GetAreaName()
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Text = m.AreaName, Value = m.AreaId.ToString() }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            ViewBag.areaList = areaResult;
            return areaResult;
        }

        //获取楼盘
        [HttpGet]
        public JsonResult ProjectSelect(string key)
        {
            var result = _humanProject.GetProjectList(Passport.Current.FxtCompanyId, Passport.Current.CityId, 0);
            var data = result.Where(m => m.ProjectName.Contains(key));
            return Json(data.Select(m => new { id = m.ProjectId, text = m.ProjectName }).ToList(), JsonRequestBehavior.AllowGet);
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
    }
}
