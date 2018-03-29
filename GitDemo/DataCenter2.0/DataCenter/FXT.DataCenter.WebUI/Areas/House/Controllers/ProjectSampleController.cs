using System;
using System.Collections.Generic;
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
using System.IO;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class ProjectSampleController : BaseController
    {
        private readonly IImportTask _importTask;
        private readonly IDropDownList _dropDownList;
        private readonly IProjectSample _projectSample;
        private readonly IProjectCase _projectCase;
        private readonly ILog _log;

        public ProjectSampleController(IDropDownList dropDownList, IProjectSample projectSample, IProjectCase projectCase, ILog log, IImportTask importTask)
        {
            this._importTask = importTask;
            this._dropDownList = dropDownList;
            this._projectSample = projectSample;
            this._projectCase = projectCase;
            this._log = log;
        }

        public ActionResult Index(DAT_SampleProject sp, int? pageIndex)
        {

            BindViewData(sp.AreaId, sp.SubAreaId, sp.BuildingTypeCode, sp.PurposeCode, -1);
            bool projectself = true;
            int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None) return View();
            if (projectoperate == (int)PermissionLevel.All) projectself = false;

            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();

            sp.CityId = Passport.Current.CityId;
            sp.FxtCompanyId = Passport.Current.FxtCompanyId;
            var result = _projectSample.GetProjectSample(sp).AsQueryable();
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
            var viewMode = result.ToPagedList(pageIndex ?? 1, 30);
            return View("Index", viewMode);
        }

        [NonAction]
        private void BindViewData(int areaid, int? subareaid, int buildingTypeCode, int purposeCode, int projectId, bool isAll = true)
        {
            var areaName = GetAreaName();
            var subAreaName = GetSubAreaName(areaid);
            var buildingTypeName = GetDictById(SYS_Code_Dict._建筑类型);
            var purposeName = GetDictById(SYS_Code_Dict._土地用途);

            areaName.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            ViewData.Add("areaid", new SelectList(areaName, "Value", "Text", areaid));
            ViewData.Add("subareaid", new SelectList(subAreaName, "Value", "Text", subareaid));
            ViewData.Add("buildingtypecode", new SelectList(buildingTypeName, "Value", "Text", buildingTypeCode));
            ViewData.Add("purposecode", new SelectList(purposeName, "Value", "Text", purposeCode));
            //ViewData.Add("projectid", new SelectList(projectIds, "Value", "Text", projectId));

        }
        //删除
        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            //由于别名表里没有加创建人字段，所以这里无法做到控制。
            if (ids == null || ids.Count == 0)
            {
                //return this.Back("请选择要删除的数据！");
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }

                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    _projectSample.DeleteProjectSample(int.Parse(array[1]));
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.样本楼盘, SYS_Code_Dict.操作.删除, "", "", "删除样本楼盘", RequestHelper.GetIP());

                return Json(new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectSample/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindViewData(-1, -1, -1, -1, -1, false);
            return View("Edit");
        }
        //新增保存
        [HttpPost]
        public ActionResult Create(DAT_SampleProject sp)
        {
            try
            {
                var id = _projectSample.SampleProjectIsExit(sp.ProjectId, Passport.Current.CityId,
                    Passport.Current.FxtCompanyId);
                if (id > 0) return base.Back("已存在该样本楼盘,请重新选择！");

                sp.CityId = Passport.Current.CityId;
                sp.FxtCompanyId = Passport.Current.FxtCompanyId;

                _projectSample.AddProjectSample(sp);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.样本楼盘, SYS_Code_Dict.操作.新增, "", "", "新增样本楼盘", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectSample/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }

            return this.RefreshParent();
        }

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

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');
            this.ViewBag.modal = "edit";
            //由于别名表里没有加创建人字段，所以这里无法做到控制。
            var projectSample = _projectSample.GetProjectSample(int.Parse(splitArray[1]), Passport.Current.CityId, int.Parse(splitArray[0])).FirstOrDefault();

            ViewBag.select2ProjectId = projectSample.ProjectId;
            ViewBag.select2ProjectName = projectSample.ProjectName;

            BindViewData(projectSample.AreaId, projectSample.SubAreaId, projectSample.BuildingTypeCode, projectSample.PurposeCode, projectSample.ProjectId, false);

            return View("Edit", projectSample);
        }
        //编辑保存
        [HttpPost]
        public ActionResult Edit(DAT_SampleProject sp, int projectIdTemp)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return this.RefreshParent("对不起，您没有修改权限！");
                }

                sp.ProjectId = projectIdTemp;
                _projectSample.UpdateProjectSample(sp);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.样本楼盘, SYS_Code_Dict.操作.修改, "", "", "编辑样本楼盘", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectSample/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }

            return this.RefreshParent();
        }
        //导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.样本楼盘, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            var taskList = result.ToPagedList(1, 30);

            return View(taskList);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());

            if (null == file) return this.Back("操作失败！");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.样本楼盘, SYS_Code_Dict.操作.导入, "", "", "导入样本楼盘", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "SampleProject");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectSample/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

        }

        //删除导入记录
        public ActionResult DeleteSampleProjectImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteSampleProjectImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
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

        //导出
        [HttpGet]
        public ActionResult Export(int areaid, string buildingdate, int purposecode, int buildingtypecode)
        {
            var projectself = true;
            int projectoperate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out projectoperate);
            if (projectoperate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有基础数据的查看权限！");
            }
            if (projectoperate == (int)PermissionLevel.All)
            {
                projectself = false;
            }

            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }

            var cityId = Passport.Current.CityId;
            var fxtCompanyId = Passport.Current.FxtCompanyId;

            var sp = new DAT_SampleProject
            {
                CityId = cityId,
                FxtCompanyId = fxtCompanyId,
                AreaId = areaid,
                BuildingDate = string.IsNullOrEmpty(buildingdate) ? default(DateTime) : Convert.ToDateTime(buildingdate),
                PurposeCode = purposecode,
                BuildingTypeCode = buildingtypecode
            };

            var result = _projectSample.GetProjectSample(sp).AsQueryable();
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
            var projectIds = result.Select(m => m.ProjectId);
            var result1 = _projectSample.GetProjectSampleWeights(projectIds, cityId, fxtCompanyId);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.样本楼盘, SYS_Code_Dict.操作.导出, "", "", "导出样本楼盘", RequestHelper.GetIP());

            _excelExportHeader(Passport.Current.CityName + "_样本楼盘_");
            using (var ms = ExcelHandle.ListToExcel(result.ToList(), result1.ToList(), "样本楼盘", "关联楼盘"))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //详情（关联楼盘）
        [HttpGet]
        public ActionResult Detail(int? companyId, int projectId, string projectName, int? pageIndex)
        {
            if (pageIndex == null)
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有查看权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                //Session["sampleprojectid"] = projectId;
                //Session["sampleprojectname"] = projectName;

                this.ViewBag.sampleprojectid = projectId;
                this.ViewBag.sampleprojectname = projectName;
            }


            projectId = ViewBag.sampleprojectid == null ? -1 : Convert.ToInt32(ViewBag.sampleprojectid);
            ViewBag.projectId = projectId;
            ViewBag.BuildingTypeCodeNameList = GetDictById(SYS_Code_Dict._建筑类型);

            var model = _projectSample.GetProjectSampleWeightById((int)projectId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var result = model.ToPagedList(pageIndex ?? 1, 30);
            return View(result);
        }
        //增加样本楼盘的关联楼盘
        [HttpPost]
        public ActionResult DetailAdd(int sampleProjectId, int projectId, decimal weight, int buildingTypeCode)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有修改样本楼盘的权限！" });
                }

                var sampleProjectWeight = _projectSample.GetProjectSampleWeight(projectId, Passport.Current.CityId, Passport.Current.FxtCompanyId, buildingTypeCode).FirstOrDefault();
                if (sampleProjectWeight != null) return Json(new { result = false, msg = "该楼盘已关联到样本楼盘：" + sampleProjectWeight.SampleProjectName + " 请重新选择" });

                var spw = new DAT_SampleProject_Weight
                {
                    CityId = Passport.Current.CityId,
                    FxtCompanyId = Passport.Current.FxtCompanyId,
                    SampleProjectId = sampleProjectId,
                    ProjectId = projectId,
                    Weight = weight,
                    BuildingTypeCode = buildingTypeCode
                };

                _projectSample.AddProjectSampleWeight(spw);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.样本楼盘, SYS_Code_Dict.操作.新增, "", "", "增加样本楼盘关联楼盘", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectSample/DetailAdd", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "" });
            }

            return Json(new { result = true, msg = "" });

        }

        //删除关联楼盘
        [HttpPost]
        public ActionResult DetailDelete(List<int> ids)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.样本楼盘, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(false);
                }

                ids.ForEach(m => _projectSample.DeleteProjectSampleWeight(m));
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.样本楼盘, SYS_Code_Dict.操作.删除, "", "", "删除样本楼盘关联楼盘", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/ProjectSample/DetailDelete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(false);
            }

            return Json(true);
        }

        #region Assist Func(辅助功能）
        [HttpGet]
        public JsonResult ValidateProject(int projectId)
        {
            var isExit = _projectSample.SampleProjectIsExit(projectId, Passport.Current.CityId,
                Passport.Current.FxtCompanyId);
            return Json(isExit, JsonRequestBehavior.AllowGet);
        }

        //excel 导出头部信息
        readonly Action<string> _excelExportHeader = m =>
        {
            var curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             HttpUtility.UrlEncode(m, System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

        };


        //可关联楼盘列表
        public List<SelectListItem> SelectedProjects()
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

            var listItem = new List<SelectListItem>();
            result.ToList().ForEach(m => listItem.Add(new SelectListItem
            {
                Value = m.projectid.ToString(),
                Text = m.projectname
            }));

            return listItem;
        }

        //样本楼盘关联的楼盘
        [HttpGet]
        public int RelationProject(int projectId)
        {
            var relationProject = _projectSample.GetProjectSampleWeightById(projectId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            //var relationProjectStr = string.Join(",", relationProject.Select(m => m.ProjectName));
            return relationProject.Count();
        }
        //行政区片区联动
        public JsonResult GetSubArea(int? areaid)
        {
            return Json(GetSubAreaName(areaid ?? 0), JsonRequestBehavior.AllowGet);
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

            return subAreaResult;
        }
        //根据ID查找相应值
        [NonAction]
        private List<SelectListItem> GetDictById(int id)
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

        public JsonResult GetProjectInfo(int? projectid)
        {
            if (projectid == null)
            {
                return Json("");
            }
            var result = _projectSample.GetProjectInfo((int)projectid, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var data = new { projectid = result.projectid, purposecode = result.purposecode, buildingtypecode = result.buildingtypecode, areaid = result.areaid, areaname = result.AreaName, subareaid = result.subareaid, enddate = result.enddate == null ? "" : ((DateTime)result.enddate).ToString("yyyy-MM-dd") };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
