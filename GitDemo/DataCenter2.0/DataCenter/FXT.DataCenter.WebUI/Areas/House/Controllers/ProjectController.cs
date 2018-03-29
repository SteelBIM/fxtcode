using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.QueryObjects;
using FXT.DataCenter.Domain.Models.QueryObjects.House;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.BatchAddPicture;
using FXT.DataCenter.WebUI.Infrastructure.ModelBinder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Areas.House.Models;
using Webdiyer.WebControls.Mvc;
using System.Configuration;
using FXT.DataCenter.Domain.Models.FxtProject;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Net.Http;
using System.Text;
using System.Net;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;
        private readonly IDropDownList _dropDownList;
        private readonly IDAT_Project _project;
        private readonly IDAT_Building _build;
        private readonly ILog _log;
        private readonly IWaitBuildingProject _wait;
        private readonly IProjectCaseTask _projectCaseTask;
        private readonly IImportTask _importTask;
        private readonly IProjectWeightRevised _projectWeightRevised;
        private readonly IShare _share;

        private IEnumerable<DAT_Project> _tempProject;

        public ProjectController(IDropDownList drop, IDAT_Project proj, IDAT_Building build, ILog log, IImportTask importTask, IWaitBuildingProject wait, IProjectCaseTask projectCaseTask, IProjectWeightRevised projectWeightRevised, IShare share)
        {
            this._dropDownList = drop;
            this._project = proj;
            this._build = build;
            this._log = log;
            this._wait = wait;
            this._projectCaseTask = projectCaseTask;
            this._importTask = importTask;
            this._projectWeightRevised = projectWeightRevised;
            this._share = share;
        }

        public ActionResult Index(ProjectQueryParams project, int? search)
        {
            project.CityId = Passport.Current.CityId;
            project.FxtCompanyId = Passport.Current.FxtCompanyId;
            BindViewData(project.AreaId, project.SubAreaId, project.PlanPurpose, project.RightCode, project.BuildingTypeCode);

            var cp = FxtUserCenterService_GetFPInfo(Passport.Current.FxtCompanyId, Passport.Current.CityId, Passport.Current.ProductTypeCode, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }
            //判断是否有导出权限
            //var ret = _share.IsExport(Passport.Current.CityId, Passport.Current.FxtCompanyId);
            ViewBag.IsExport = (cp.IsExportHose == 1);

            //判断查看自己或全部权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View(new PagedList<DAT_Project>(new List<DAT_Project>().AsQueryable(), project.pageIndex, project.pageSize, 0));
            if (operate == (int)PermissionLevel.All) self = false;

            var result = _project.GetProjectInfoList(project).OrderByDescending(t => t.projectid).AsQueryable();
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.fxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                }
                else { result = result.Where(t => t.creator == Passport.Current.UserName).AsQueryable(); }
            }
            var projectResult = result.ToPagedList(project.pageIndex, project.pageSize);
            return View(projectResult);
        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create(string waitId, bool isWait = false)
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标
            ViewBag.cityId = Passport.Current.CityId;
            GetAreaName();
            BindProjectName(0, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            ViewBag.isWait = false;

            //土地规划用途
            BindDropDownList();

            if (isWait)
            {
                var splitArray = waitId.Split('#');
                var project = _wait.GetWaitProjectById(int.Parse(splitArray[1])).FirstOrDefault();
                var pro = new DAT_Project
                {
                    cityid = Passport.Current.CityId,
                    projectid = int.Parse(splitArray[1]),
                    projectname = project == null ? "" : project.WaitProjectName
                };
                return View("Edity", pro);
            }

            return View("Edity", new DAT_Project { projectname = string.Empty });
        }

        //新增状态时保存
        [HttpPost]
        public ActionResult Create(DAT_Project project)
        {
            try
            {
                int waitProjectId = project.projectid;
                project.cityid = Passport.Current.CityId;
                project.fxtcompanyid = Passport.Current.FxtCompanyId;
                project.creator = Passport.Current.ID;
                project.projectname = project.projectname.Trim();
                project.weight = project.weight == null ? 1 : project.weight;

                var temp = "";
                if (project.Description != null && project.Description.Any())
                {
                    temp = string.Join(",", project.Description);
                }
                project.opValue = temp;
                project.planpurpose = project.opValue;
                var result = _project.AddProject(project);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.新增, result.ToString(), project.projectname, "新增楼盘", RequestHelper.GetIP());

                if (result > 0 && waitProjectId < 0)
                {
                    //删除待建楼盘，并复制案例。
                    _wait.DeleteWaitProject(waitProjectId);
                    _projectCaseTask.MatchProjectCaseWaitProject(waitProjectId, result, project.areaid, project.projectname, Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName);

                    //操作日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.删除, "waitProjectId:" + waitProjectId, project.projectname, "删除待建楼盘", RequestHelper.GetIP());
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/project/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //编辑
        [HttpGet]
        public ActionResult Edity(string projectid)
        {
            var splitArray = projectid.Split('#');

            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标
            ViewBag.cityId = Passport.Current.CityId;
            GetAreaName();
            BindProjectName(int.Parse(splitArray[1]), Passport.Current.CityId, Passport.Current.FxtCompanyId);

            //土地规划用途
            BindDropDownList();

            var pro = _project.GetProjectInfo(new ProjectQueryParams(Passport.Current.CityId, Passport.Current.FxtCompanyId, int.Parse(splitArray[1]))).FirstOrDefault();
            if (pro != null&&!string.IsNullOrEmpty(pro.planpurpose))
            {
                var array = pro.planpurpose.Split(',');
                if (array != null && array.Count() > 0)
                    BindDropDownList(array.ToList());
            }
            return View("Edity", pro ?? new DAT_Project());
        }

        //编辑状态时保存
        [HttpPost]
        public ActionResult Edity(DAT_Project project)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.Back("对不起，您没有修改权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }
            var result = _project.GetProjectInfo(new ProjectQueryParams(Passport.Current.CityId, Passport.Current.FxtCompanyId, project.projectid)).FirstOrDefault();
            if (self)//self && result.creator != Passport.Current.UserName
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && result.fxtcompanyid != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    return base.Back("对不起，该条数据您没有修改权限！");
                }
                if (Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && result.creator != Passport.Current.UserName)
                {
                    return base.Back("对不起，该条数据您没有修改权限！");
                }
            }

            project.cityid = Passport.Current.CityId;
            project.saveuser = Passport.Current.ID;
            project.savedatetime = DateTime.Now;
            project.projectname = project.projectname.Trim();
            var temp = "";

            try
            {

                if (project.Description != null && project.Description.Any())
                {
                    temp = string.Join(",", project.Description);
                }
                project.opValue = temp;
                project.planpurpose = project.opValue;

                _project.ModifyProject(project, Passport.Current.FxtCompanyId);

                // 操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.修改, project.projectid.ToString(), project.projectname, "修改楼盘", RequestHelper.GetIP());

                //return RedirectToAction("Index", new { project.areaid, project.buildingtypecode, project.subareaid, planpurpose = project.purposecode, key = project.projectname });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/project/Edity", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //删除
        public ActionResult DeleteProject(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {
                var cp = FxtUserCenterService_GetFPInfo(Passport.Current.FxtCompanyId, Passport.Current.CityId, Passport.Current.ProductTypeCode, Passport.Current.UserName, out _msg, _productTypeCode);
                if (cp == null)
                {
                    return this.Back("操作失败！");
                }
                //判断操作权限是删除自己还是删除全部
                int operate;
                var self = true;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    int projectid = int.Parse(array[1]);
                    var result = _project.GetProjectInfo(new ProjectQueryParams(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectid)).FirstOrDefault();
                    if (self)//self && result.creator != Passport.Current.UserName
                    {
                        if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && result.fxtcompanyid != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                        if (Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && result.creator != Passport.Current.UserName)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                    }
                    _project.DeleteProject(projectid, Passport.Current.CityId, fxtcompanyid, Passport.Current.UserName, Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId, (int)cp.IsDeleteTrue);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除楼盘", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/DeleteProject", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        [NonAction]
        private void BindDropDownList(ICollection<string> purposeDescList = null)
        {
            this.ViewBag.LandPlanUse = GetLandPlanUse(purposeDescList ?? new List<string>());
        }

        //规划用途
        [NonAction]
        private List<SelectListItem> GetLandPlanUse(ICollection<string> selected)
        {
            var landUse = _dropDownList.GetLandPurpose(SYS_Code_Dict._土地规划用途);
            var landUseResult = new List<SelectListItem>();
            landUse.ToList().ForEach(m =>
                landUseResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName, Selected = selected.Contains(m.Code.ToString()) }
                ));
            return landUseResult;
        }

        #region 项目配套

        //获取项目配套
        [HttpGet]
        public ActionResult ProjectPeiTao(BaseParams para, int projectid = 0)
        {

            ViewBag.projectPara = Passport.Current.FxtCompanyId + "#" + projectid;
            ViewBag.projectid = projectid;
            var result = _project.GetProjectAppendageById(projectid, Passport.Current.CityId);
            ViewBag.projectName = _build.GetProjectNameById(projectid.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId).projectname;
            var projectResult = result.ToPagedList(para.pageIndex, para.pageSize);
            return View(projectResult);

        }

        //编辑项目配套
        [HttpGet]
        public ActionResult EdityProjectPeitao(int projectid = 0, int pid = 0)
        {
            ViewBag.projectid = projectid;
            if (pid > 0)
            {
                var result = _project.GetProjectAppendageById(projectid, Passport.Current.CityId, pid).FirstOrDefault();
                return View(result);
            }
            return View(new LNK_P_Appendage());
        }

        [HttpPost]
        public ActionResult EdityProjectPeitao(LNK_P_Appendage obj)
        {
            int operateCreate; int operateUpdate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.新增, SYS_Code_Dict.页面权限.新增, out operateCreate);
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operateUpdate);

            string msg = "";
            try
            {
                if (obj.id > 0)
                {
                    if (operateUpdate == (int)PermissionLevel.None)
                    {
                        return this.RefreshParent("对不起，您没有修改权限！");
                    }

                    //修改楼盘配套
                    int reslut = _project.ModifyProjectAppendage(obj);

                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.修改, obj.id.ToString(), obj.p_aname, "修改楼盘配套", RequestHelper.GetIP());

                    if (reslut > 0) msg = "修改楼盘项目配套成功";
                    else msg = "修改楼盘项目配套失败";
                    return this.RefreshParent(msg);
                }
                else
                {
                    if (operateCreate == (int)PermissionLevel.None)
                    {
                        return this.RefreshParent("对不起，您没有新增权限！");
                    }
                    //新增楼盘配套
                    obj.cityid = Passport.Current.CityId;
                    int reslut = _project.AddProjectAppendage(obj);
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.新增, "", obj.p_aname, "新增楼盘配套", RequestHelper.GetIP());

                    if (reslut > 0) msg = "新增楼盘项目配套成功";
                    return this.RefreshParent(msg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/EdityProjectPeitao", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //删除项目配套
        [HttpPost]
        public ActionResult DeleteProjectPeiTao(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {

                var index = ids.Select(item => _project.DeleteProjectPeiTao(item)).Count(result => result == 0);

                // 操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除楼盘配套", RequestHelper.GetIP());


                return Json(index > 0 ? new { result = true, msg = "有" + index + "条数据删除失败！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("House/Project/DeleteProjectPeitao", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

        }

        #endregion

        #region 楼盘附属房屋价格
        // 获取楼盘附属房的价格
        [HttpGet]
        public ActionResult GetProjectPrice(BaseParams para, int projectid = 0, string codename = "")
        {
            ViewBag.projectPara = Passport.Current.FxtCompanyId + "#" + projectid;
            ViewBag.projectid = projectid;
            var result = _project.GetSubHousePriceByProjectId(Passport.Current.CityId, projectid, Passport.Current.FxtCompanyId, codename);
            var projectResult = result.ToPagedList(para.pageIndex, 30);
            ViewBag.projectName = _build.GetProjectNameById(projectid.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId).projectname;
            return View("ProjectPrice", projectResult);
        }

        // 编辑楼盘附属房的价格
        [HttpGet]
        public ActionResult EdityProjectPrice(int projectid, string codename = "")
        {
            ViewBag.projectid = projectid;
            codename = Server.UrlDecode(codename);
            var result = _project.GetSubHousePriceByProjectId(Passport.Current.CityId, projectid, Passport.Current.FxtCompanyId, codename).FirstOrDefault();
            return View(result);
        }

        [HttpPost]
        public ActionResult EdityProjectPrice(DAT_SubHousePrice price)
        {

            try
            {
                var result = _project.SaveSubHousePrice(Passport.Current.CityId, price.projectid.ToString(), Passport.Current.FxtCompanyId.ToString(), price.id.ToString(), price.subhouseunitprice.ToString(), price.Code.ToString(), Passport.Current.ID);
                #region 操作日志
                _log.InsertLog(
       Passport.Current.CityId,
       Passport.Current.FxtCompanyId,
       Passport.Current.ID,
       Passport.Current.ID,
       SYS_Code_Dict.功能模块.楼盘,
       SYS_Code_Dict.操作.修改,
       price.projectid.ToString(),
       "",
       "编辑楼盘附属房的价格",
       RequestHelper.GetIP());
                #endregion
                return this.RefreshParent();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("House/Project/EdityProjectPrice", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //获取楼盘名称的中文拼音
        public ActionResult GetPinYin(string proName)
        {
            string pinyin = StringHelper.GetPYString(proName);//简拼
            //string quanpin = StringHelper.ConvertPinYin(proName, 100);
            string quanpin = StringHelper.GetAllPYString(proName);
            return Json(new { jianpin = pinyin, quanpin });
        }

        #endregion

        #region 项目图片

        // 楼盘项目图片
        public ActionResult ProjectPhoto(BaseParams para, int projectId = 0)
        {
            ViewBag.projectPara = Passport.Current.FxtCompanyId + "#" + projectId;
            ViewBag.projectId = projectId;

            var photoList = _project.GetProjectPhotoList(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectId);
            var photoListResult = photoList.ToPagedList(para.pageIndex, 10);
            ViewBag.projectName = _build.GetProjectNameById(projectId.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId).projectname;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            return View(photoListResult);
        }

        // 批量新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult BatchAddPicture(int projectId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型_住宅);
            return View();
        }

        [HttpPost]
        public ActionResult BatchAddPictureSave()
        {
            var isSavedSuccessfully = true;
            var count = 0;

            var projectId = Request["projectId"];
            var photoTypeCode = Request["photoTypeCode"];

            if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(photoTypeCode))
            {
                return Json(new { Result = false, Count = count });
            }

            try
            {
                var virtualPath = "/ProjectPic/" + Passport.Current.CityId + "/" + projectId + "/";

                var IsOss = ConfigurationHelper.IsOss;
                //IsOss默认false为上传图片或附件到本地；true，为上传到OSS。
                if (IsOss)
                {
                    foreach (string f in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[f];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileExtension = Path.GetExtension(file.FileName);//扩展名 如：.jpg
                            var fileName = file.FileName.Substring(0, file.FileName.IndexOf('.'));//扩展名前的名称
                            var fileNewName = new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                            var virtualFilePath = Path.Combine(virtualPath, fileNewName);//服务器虚拟路径

                            //OSS存储
                            var fs = file.InputStream;
                            StreamContent content = new StreamContent(fs);
                            var result = OssHelper.UpFileAsync(content, virtualFilePath);

                            //保存该条图片信息到数据库
                            _project.AddProjectPhoto(Passport.Current.CityId, Passport.Current.FxtCompanyId, int.Parse(projectId), int.Parse(photoTypeCode), virtualFilePath, fileName);
                            count++;
                        }
                    }
                }
                else
                {
                    var directoryPath = Server.MapPath(virtualPath);
                    if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }

                    var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                    var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);
                    foreach (string f in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[f];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileExtension = Path.GetExtension(file.FileName);//扩展名 如：.jpg
                            var fileName = file.FileName.Substring(0, file.FileName.IndexOf('.'));//扩展名前的名称
                            var fileNewName = new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                            var virtualFilePath = Path.Combine(virtualPath, fileNewName);//服务器虚拟路径

                            //本地服务器存储
                            var physicalFilePath = Server.MapPath(virtualFilePath);//服务器物理路径
                            //保存图片
                            file.SaveAs(physicalFilePath);
                            //保存缩略图
                            var thumbnailPath = physicalFilePath.Insert(physicalFilePath.LastIndexOf(".", StringComparison.Ordinal), "_t");
                            ImageHandler.MakeThumbnail(physicalFilePath, thumbnailPath, width, height, "H");

                            //保存该条图片信息到数据库
                            _project.AddProjectPhoto(Passport.Current.CityId, Passport.Current.FxtCompanyId, int.Parse(projectId), int.Parse(photoTypeCode), virtualFilePath, fileName);
                            count++;
                        }
                    }
                }
                ////var directoryPath = Server.MapPath(virtualPath);
                ////if (!Directory.Exists(directoryPath)){ Directory.CreateDirectory(directoryPath);}

                //var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                //var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);

                //foreach (string f in Request.Files)
                //{
                //    HttpPostedFileBase file = Request.Files[f];

                //    if (file != null && file.ContentLength > 0)
                //    {
                //        var fileExtension = Path.GetExtension(file.FileName);//扩展名 如：.jpg
                //        var fileName = file.FileName.Substring(0, file.FileName.IndexOf('.'));//扩展名前的名称
                //        var fileNewName = new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                //        var virtualFilePath = Path.Combine(virtualPath, fileNewName);//服务器虚拟路径

                //        //本地服务器存储
                //        //var physicalFilePath = Server.MapPath(virtualFilePath);//服务器物理路径
                //        //保存图片
                //        //file.SaveAs(physicalFilePath);
                //        ////保存缩略图
                //        //var thumbnailPath = physicalFilePath.Insert(physicalFilePath.LastIndexOf(".", StringComparison.Ordinal), "_t");
                //        //ImageHandler.MakeThumbnail(physicalFilePath, thumbnailPath, width, height, "H");

                //        //OSS存储
                //        var fs = file.InputStream;
                //        StreamContent content = new StreamContent(fs);
                //        var result = OssHelper.UpFileAsync(content, virtualFilePath);

                //        //保存该条图片信息到数据库
                //        _project.AddProjectPhoto(Passport.Current.CityId, Passport.Current.FxtCompanyId, int.Parse(projectId), int.Parse(photoTypeCode), virtualFilePath, fileName);
                //        count++;
                //    }
                //}
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.新增, "projectId:" + projectId, "", "新增楼盘项目图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/BatchAddPictureSave", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        // 编辑项目图片图片
        [HttpGet]
        public ActionResult EdityProjectPhoto(int projectId, int photoId)
        {
            ViewBag.projectId = projectId;
            ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型_住宅);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var photoList = _project.GetProjectPhotoList(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectId, photoId).FirstOrDefault();

            return View(photoList);
        }

        [HttpPost]
        public ActionResult EdityProjectPhoto(LNK_P_Photo photo)
        {
            try
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.Back("对不起，您没有修改权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                var result = _project.GetProjectPhotoList(Passport.Current.CityId, Passport.Current.FxtCompanyId, photo.projectid, photo.id).FirstOrDefault();
                if (self && Passport.Current.FxtCompanyId != photo.fxtcompanyid)
                {
                    return base.Back("对不起，该条数据您没有修改权限！");
                }

                _project.UpdataProjectPhoto(photo.id, photo.phototypecode ?? 0, photo.path, photo.photoname, Passport.Current.CityId, photo.fxtcompanyid, Passport.Current.FxtCompanyId);
                //日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.修改, photo.id.ToString(), photo.photoname, "编辑楼盘项目图片", RequestHelper.GetIP());
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/EdityProjectPrice", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 删除楼盘项目图片
        public ActionResult DeletePhoto(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "请选择要删除数据！" });
            }
            try
            {
                //判断操作权限是删除自己还是删除全部
                int operate;
                var self = true;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    int fxtcompanyid = int.Parse(array[2]);
                    int projectid = int.Parse(array[1]);
                    int photoid = int.Parse(array[0]);
                    var result = _project.GetProjectPhotoList(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectid, photoid).FirstOrDefault();
                    if (self && Passport.Current.FxtCompanyId != fxtcompanyid)
                    {
                        failList.Add(array[1]);
                        continue;
                    }
                    _project.DeleteProjectPhoto(photoid, Passport.Current.CityId, fxtcompanyid, Passport.Current.FxtCompanyId);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除楼盘项目图片", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/DeletePhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "操作失败！" });
            }
        }

        // 楼盘项目图片
        public ActionResult ProjectPicUp(int projectId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/" + Passport.Current.CityId + "/" + projectId + "/";//原图路径

                var IsOss = ConfigurationHelper.IsOss;
                //IsOss默认false为上传图片或附件到本地；true，为上传到OSS。
                if (IsOss)
                {
                    //取得文件的扩展名,并转换成小写
                    var fileExtension = Path.GetExtension(upImg.FileName).ToLower();
                    if (!ImageHandler.CheckValidExt(fileExtension))
                    {
                        return Json(new { result = false, msg = "您上传的文件格式有误!" });
                    }
                    string fileName = upImg.FileName.Substring(0, upImg.FileName.IndexOf('.')).Trim();

                    //存到服务器上的虚拟路径
                    var virpath = filePath + new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                    //修改OSS存储
                    var fs = upImg.InputStream;
                    StreamContent content = new StreamContent(fs);
                    var result = OssHelper.UpFileAsync(content, virpath);
                    return Json(new { result = true, msg = "上传的图片成功", path = virpath, fileName = fileName });
                }
                else
                {
                    //如果不存在就创建file文件夹
                    if (!Directory.Exists(Server.MapPath(filePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(filePath));
                    }

                    //取得文件的扩展名,并转换成小写
                    var fileExtension = Path.GetExtension(upImg.FileName).ToLower();
                    if (!ImageHandler.CheckValidExt(fileExtension))
                    {
                        return Json(new { result = false, msg = "您上传的文件格式有误!" });
                    }
                    string fileName = upImg.FileName.Substring(0, upImg.FileName.IndexOf('.')).Trim();

                    //存到服务器上的虚拟路径
                    var virpath = filePath + new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                    //转换成服务器上的物理路径
                    var mapPath = Server.MapPath(virpath);
                    upImg.SaveAs(mapPath);

                    //生成缩略图
                    var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                    var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);

                    var thumbnailPath = mapPath.Insert(mapPath.LastIndexOf("."), "_t");
                    ImageHandler.MakeThumbnail(mapPath, thumbnailPath, width, height, "H");

                    return Json(new { result = true, msg = "上传的图片成功", path = virpath, fileName = fileName });
                }
                //////如果不存在就创建file文件夹
                ////if (!Directory.Exists(Server.MapPath(filePath)))
                ////{
                ////    Directory.CreateDirectory(Server.MapPath(filePath));
                ////}

                ////取得文件的扩展名,并转换成小写
                //var fileExtension = Path.GetExtension(upImg.FileName).ToLower();
                //if (!ImageHandler.CheckValidExt(fileExtension))
                //{
                //    return Json(new { result = false, msg = "您上传的文件格式有误!" });
                //}
                //string fileName = upImg.FileName.Substring(0, upImg.FileName.IndexOf('.')).Trim();

                ////存到服务器上的虚拟路径
                //var virpath = filePath + new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                //////转换成服务器上的物理路径
                ////var mapPath = Server.MapPath(virpath);
                ////upImg.SaveAs(mapPath);

                //////生成缩略图
                ////var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                ////var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);

                ////var thumbnailPath = mapPath.Insert(mapPath.LastIndexOf("."), "_t");
                ////ImageHandler.MakeThumbnail(mapPath, thumbnailPath, width, height, "H");

                ////修改OSS存储
                //var fs = upImg.InputStream;
                //StreamContent content = new StreamContent(fs);
                //var result = OssHelper.UpFileAsync(content, virpath);
                //return Json(new { result = true, msg = "上传的图片成功", path = virpath, fileName = fileName });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/ProjectPicUp", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        #endregion

        // 楼盘复制：0:没有开通权限;-1:原始楼盘暂无可以复制的楼栋;-2:添加楼栋失败;-3:目标楼盘存在数据,不能复制楼盘;-4：添加房号失败; -5://程序异常
        [HttpPost]
        public ActionResult ProjectCopy(string ProjectName, int AreaID, int projectId, string othername, string address, int fxtcompanyid)
        {
            try
            {
                var cp = FxtUserCenterService_GetFPInfo(Passport.Current.FxtCompanyId, Passport.Current.CityId, Passport.Current.ProductTypeCode, Passport.Current.UserName, out _msg, _productTypeCode);
                if (cp == null)
                {
                    return this.Back("操作失败！");
                }
                int operateCreate; int operateUpdate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.新增, SYS_Code_Dict.页面权限.新增, out operateCreate);
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operateUpdate);
                if (operateCreate == (int)PermissionLevel.None || operateUpdate != (int)PermissionLevel.All)
                {
                    return Json(new { reslut = "对不起，您没有足够的权限复制楼盘！" });
                }

                //string msg = "";
                #region -------------楼盘赋值-------------
                DAT_Project project = new DAT_Project();
                project.projectid = projectId;
                if (ProjectName.Contains("|"))
                {
                    project.projectname = (ProjectName.Split('|')[0]).Trim();
                    project.AreaIdTo = _dropDownList.GetAreaIdByName(Passport.Current.CityId, (ProjectName.Split('|')[1]).Trim());
                }
                else
                {
                    project.projectname = ProjectName.Trim();
                    project.AreaIdTo = AreaID;
                }
                if (string.IsNullOrWhiteSpace(project.projectname))
                {
                    return Json(new { reslut = "目标楼盘不能为空！" });
                }
                project.areaid = AreaID;
                if (project.AreaIdTo == -1) project.areaid = AreaID;
                project.cityid = Passport.Current.CityId;
                project.fxtcompanyid = fxtcompanyid;
                project.creator = Passport.Current.UserName;
                project.othername = othername;
                project.address = address;
                #endregion

                string reslut = _project.CopyProject(project, Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId, (int)cp.IsDeleteTrue);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.新增, "复制楼盘ID：" + projectId.ToString(), "目标楼盘：" + ProjectName, "楼盘复制", RequestHelper.GetIP());
                return Json(new { reslut = reslut });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/ProjectCopy", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 楼盘拆分：projectId：原始楼盘ID；proNameTo：目标楼盘名称；build_list：原始楼栋集合
        [HttpPost]
        public ActionResult ProjectSplit(string ProjectName, int AreaID, int projectId, string buidIdList, string build_Name, int fxtcompanyid)
        {
            try
            {
                #region -------------楼盘拆分-------------
                DAT_Project project = new DAT_Project();
                if (ProjectName.Contains("|"))
                {
                    project.projectname = (ProjectName.Split('|')[0]).Trim();
                    project.AreaIdTo = _dropDownList.GetAreaIdByName(Passport.Current.CityId, (ProjectName.Split('|')[1]).Trim());
                }
                else
                {
                    project.projectname = ProjectName.Trim();
                    project.AreaIdTo = AreaID;
                }
                if (project.AreaIdTo == -1) project.areaid = AreaID;
                project.projectid = projectId;
                project.saveuser = Passport.Current.UserName;
                project.fxtcompanyid = fxtcompanyid;
                project.cityid = Passport.Current.CityId;
                project.areaid = AreaID;
                #endregion
                string reslut = _project.SplitProject(project, buidIdList, build_Name, Passport.Current.FxtCompanyId);
                //msg = ShowMsg(msg, reslut);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.修改, "拆分楼盘ID：" + projectId.ToString(), "目标楼盘：" + ProjectName, "楼盘拆分", RequestHelper.GetIP());
                return Json(new { reslut = reslut });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/ProjectSplit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 楼盘合并
        [HttpGet]
        public ActionResult ProjectMerger(int id)
        {            
            BindProjectName(id, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var result = _project.GetProjectInfo(new ProjectQueryParams(Passport.Current.CityId, Passport.Current.FxtCompanyId, id)).FirstOrDefault();
            this.ViewBag.select2ProjectId = result.projectid;
            this.ViewBag.select2ProjectName = result.projectname + "|" + result.AreaName;
            return View(result);
        }

        // 楼盘合并
        [HttpPost]
        public ActionResult ProjectMerger(int projectId, int AreaID, int projectidTo, int fxtcompanyid)
        {
            try
            {
                #region -------------楼盘合并-------------
                DAT_Project project = new DAT_Project();
                project.projectid = projectId;
                project.saveuser = Passport.Current.UserName;
                project.fxtcompanyid = fxtcompanyid;
                project.cityid = Passport.Current.CityId;
                project.areaid = AreaID;
                #endregion
                int reslut = _project.MergerProject(project, projectidTo, Passport.Current.FxtCompanyId);
                //合并成功后，复制案例跟图片。projectidTo是合并楼盘，projectid是被合并楼盘。
                if (reslut > 0)
                {
                    _project.MergerProjectCase(projectId, projectidTo, Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName);
                    _project.MergerProjectPhoto(projectId, projectidTo, fxtcompanyid, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.修改, projectidTo.ToString() + "合并" + projectId.ToString(), "", "楼盘合并", RequestHelper.GetIP());
                return Json(new { reslut = reslut });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/ProjectMerger", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //获取楼盘
        [HttpGet]
        public JsonResult ProjectSelect(string key)
        {
            var result = _project.GetAllProjectName(Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var data = result.Where(m => m.projectname.Contains(key));
            return Json(data.Select(m => new { id = m.projectid, text = m.projectname + "|" + m.AreaName }).ToList(), JsonRequestBehavior.AllowGet);
        }


        // 删除目标楼盘
        public ActionResult ProjectMergerDel(int projectidTo, int fxtcompanyid)
        {
            try
            {
                var cp = FxtUserCenterService_GetFPInfo(Passport.Current.FxtCompanyId, Passport.Current.CityId, Passport.Current.ProductTypeCode, Passport.Current.UserName, out _msg, _productTypeCode);
                if (cp == null)
                {
                    return this.Back("操作失败！");
                }
                int reslut = _project.MergerProjectDel(projectidTo, Passport.Current.CityId, fxtcompanyid, Passport.Current.UserName, Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId, (int)cp.IsDeleteTrue);
                #region 操作日志
                _log.InsertLog(
       Passport.Current.CityId,
       Passport.Current.FxtCompanyId,
       Passport.Current.ID,
       Passport.Current.ID,
       SYS_Code_Dict.功能模块.楼盘,
       SYS_Code_Dict.操作.修改,
      projectidTo.ToString(),
       "",
       "删除目标楼盘",
       RequestHelper.GetIP());
                #endregion
                return Json(new { reslut = reslut });
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("House/Project/ProjectMergerDel", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 楼盘统计 自定义导出
        [HttpGet]
        public ActionResult ProjectStatistics()
        {
            #region 判断操作权限是查看自己还是查看全部
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }

            //var ret = _share.IsExport(Passport.Current.CityId, Passport.Current.FxtCompanyId);
            //if (!ret) return base.AuthorizeWarning("对不起，您没有导出权限！");
            #endregion
            BindViewData();
            return View();
        }

        // 楼盘统计之导出楼盘
        [HttpPost]
        public ActionResult ProImportStat(List<string> project, ProStatiParam param)
        {
            try
            {
                //判断导出自己或全部权限
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                if (self)
                {
                    param.Creator = Passport.Current.UserName;
                }
                DataTable dt = _project.ProjectStatistcs(project, param, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region header 信息
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_自定义导出楼盘_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";
                    #endregion

                    //日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.导出, "", "", "自定义导出楼盘", RequestHelper.GetIP());

                    using (var ms = ExcelHandle.RenderToExcel(dt))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }
                }
                return this.Back("没有您要导出的数据");
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("House/Project/ProImportStat", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 楼盘统计之导出楼栋
        [HttpPost]
        public ActionResult BuidImportStat(List<string> building, BuildStatiParam param)
        {
            try
            {
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                if (self)
                {
                    param.Creator = Passport.Current.UserName;
                }
                DataTable dt = _project.BuildStatistcs(building, param, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region header 信息
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_自定义导出楼栋_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";
                    #endregion

                    //日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.导出, "", "", "自定义导出楼栋", RequestHelper.GetIP());

                    using (var ms = ExcelHandle.RenderToExcel(dt))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }
                }
                return this.Back("没有您要导出的数据");
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("House/Project/BuidImportStat", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 楼盘统计之导出房号
        [HttpPost]
        public ActionResult HouseImportStat(List<string> house, HouseStatiParam parame)
        {
            parame.AreaId = parame.houseareaid;
            parame.SubAreaId = parame.housesubareaid;
            parame.PurposeCode = parame.HousePurposeCode;
            try
            {
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                if (self)
                {
                    parame.Creator = Passport.Current.UserName;
                }
                DataTable dt = _project.HouseStatistcs(house, parame, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region header 信息
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_自定义导出房号_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";
                    #endregion

                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.导出, "", "", "自定义导出房号", RequestHelper.GetIP());

                    using (var ms = ExcelHandle.RenderToExcel(dt))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }

                }
                return this.Back("没有您要导出的数据");
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("House/Project/HouseImportStat", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 获取公司名称
        [HttpGet]
        public JsonResult BindCompan(int companypecode = 0)
        {
            IList<CompanyProduct_Module> data = _dropDownList.GetCompanyName(Passport.Current.CityId, companypecode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // 获取片区
        public JsonResult GetSubArea(int? areaid)
        {
            return Json(GetSubAreaName(areaid ?? 0), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHouseSubArea(int? areaid)
        {
            return Json(GetSubAreaName(areaid ?? 0), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public void BindProjectName(int projectId, int CityId, int FxtCompanyId)
        {
            ViewData.Add("Merger_projectname", new SelectList(DropListProjectName(CityId, FxtCompanyId), "Value", "Text", projectId));
        }

        [NonAction]
        private IEnumerable<SelectListItem> DropListProjectName(int CityId, int FxtCompanyId)
        {
            var list = _project.GetAllProjectName(CityId, FxtCompanyId);
            _tempProject = list;
            //if (list != null)
            //{
            var projectgList = new List<SelectListItem>();
            list.ToList().ForEach(m =>
                projectgList.Add(
                new SelectListItem { Text = m.projectname + "|" + m.AreaName, Value = m.projectid.ToString() }
                ));
            //projectgList.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            return projectgList;
            // }
        }

        // 获取楼栋集合
        [NonAction]
        private IQueryable<DAT_Building> GetBuildList(int projectId)
        {
            return _build.GetBuildingInfo(Passport.Current.CityId, projectId, Passport.Current.FxtCompanyId);
        }
        [NonAction]
        private void BindViewData()
        {
            this.ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text"));
            ViewData.Add("houseareaid", new SelectList(GetSubAreaName(), "Value", "Text"));
            ViewData.Add("housesubareaid", new SelectList(GetSubAreaName(HouseSubid), "Value", "Text"));
            ViewData.Add("subareaid", new SelectList(GetSubAreaName(0), "Value", "Text"));
            ViewData.Add("rightcode", new SelectList(GetDictById(SYS_Code_Dict._产权形式), "Value", "Text"));
            ViewData.Add("planpurpose", new SelectList(GetDictById(SYS_Code_Dict._土地用途), "Value", "Text"));
            ViewData.Add("PurposeCode", new SelectList(GetDictById(SYS_Code_Dict._土地用途), "Value", "Text"));
            ViewData.Add("HousePurposeCode", new SelectList(GetDictById(SYS_Code_Dict._居住用途), "Value", "Text"));
            ViewData.Add("Wall", new SelectList(GetDictById(SYS_Code_Dict._外墙装修), "Value", "Text"));
            ViewData.Add("StructureCode", new SelectList(GetDictById(SYS_Code_Dict._建筑结构), "Value", "Text"));
            ViewData.Add("HouseTypeCode", new SelectList(GetDictById(SYS_Code_Dict._户型), "Value", "Text"));
            ViewData.Add("HouseStructureCode", new SelectList(GetDictById(SYS_Code_Dict._户型结构), "Value", "Text"));
            ViewData.Add("SightCode", new SelectList(GetDictById(SYS_Code_Dict._景观), "Value", "Text"));
            ViewData.Add("buildingtypecode", new SelectList(GetDictById(SYS_Code_Dict._建筑类型), "Value", "Text"));
            ViewData.Add("AppendageClass", new SelectList(GetDictById(SYS_Code_Dict._建筑等级), "Value", "Text"));
            ViewData.Add("FrontCode", new SelectList(GetDictById(SYS_Code_Dict._朝向), "Value", "Text"));
        }
        [NonAction]
        private void BindViewData(int areaid, int subareaid, int planpurpose, int rightcode, int buildingtypecode)
        {
            ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text", areaid));
            ViewData.Add("subareaid", new SelectList(GetSubAreaName(areaid), "Value", "Text", subareaid));
            ViewData.Add("rightcode", new SelectList(GetDictById(SYS_Code_Dict._产权形式), "Value", "Text", rightcode));
            ViewData.Add("planpurpose", new SelectList(GetDictById(SYS_Code_Dict._土地用途), "Value", "Text", planpurpose));
            ViewData.Add("buildingtypecode", new SelectList(GetDictById(SYS_Code_Dict._建筑类型), "Value", "Text", buildingtypecode));
        }

        // 导出
        [HttpGet]
        public ActionResult Export([JsonModelBinder]ProjectQueryParams project)
        {
            try
            {
                //判断导出自己或全部权限
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                project.CityId = Passport.Current.CityId;
                project.FxtCompanyId = Passport.Current.FxtCompanyId;

                var result = _project.ExportProjectInfoList(project);
                if (self)
                {
                    if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                    {
                        result = result.Where(t => t.fxtcompanyid == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                    }
                    else { result = result.Where(t => t.creator == Passport.Current.UserName).AsQueryable(); }
                }
                foreach (var re in result)
                {
                    re.opValue = re.opValue == null ? null : re.opValue.TrimEnd(',');
                }
                if (result.Count() > 0)
                {
                    #region header 信息
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼盘基础数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";
                    #endregion

                    //插入日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.导出, "", "", "一键导出楼盘基础数据", RequestHelper.GetIP());

                    using (var ms = ExcelHandle.ListToExcel(result.ToList()))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }
                }
                return base.AuthorizeWarning("对不起，没有需要导出的数据！");
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("House/Project/Export", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        // 楼盘导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.住宅楼盘信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.导入, "", "", "导入楼盘", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "Project");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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

        //项目配套
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult ImportPeiTao(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.住宅项目配套, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectResult = result.ToPagedList(pageIndex ?? 1, 30);
            return View(projectResult);
        }

        // 导入项目配套
        [HttpPost]
        public ActionResult ImportPeiTao(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId);

            if (null == file) return this.RedirectToAction("ImportPeiTao");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.导入, "", "", "导入项目配套", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "ProjectPeiTao");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/ImportPeiTao", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("ImportPeiTao");
        }

        //楼栋导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult ImportBuilding(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.住宅楼栋信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectResult = result.ToPagedList(pageIndex ?? 1, 30);
            return View(projectResult);
        }

        [HttpPost]
        public ActionResult ImportBuilding(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId);

            if (null == file) return this.RedirectToAction("ImportBuilding");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.导入, "", "", "导入楼栋", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "Building");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/ImportBuilding", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("ImportBuilding");
        }

        //房号导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult ImportHouseNo(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.住宅房号信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectResult = result.ToPagedList(pageIndex ?? 1, 30);
            return View(projectResult);
        }

        [HttpPost]
        public ActionResult ImportHouseNo(HttpPostedFileBase file, string taskNameHiddenValue)
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
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.导入, "", "", "导入房号标准模板", RequestHelper.GetIP());

                    // 异步调用WCF服务
                    var filePath = Path.Combine(folder, filename);
                    var cityId = Passport.Current.CityId;
                    var fxtCompanyId = Passport.Current.FxtCompanyId;
                    var userId = Passport.Current.ID;
                    Task.Factory.StartNew(() =>
                    {
                        var client = new ExcelUploadServices.ExcelUploadClient();
                        client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                            taskNameHiddenValue, "HouseNo");
                        try { client.Close(); }
                        catch { client.Abort(); }
                    });
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/Project/ImportHouseNo", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                    return base.Back("操作失败！");
                }
            }
            return this.RedirectToAction("ImportHouseNo");
        }

        [HttpPost]
        public ActionResult ImportHouseNoNew(HttpPostedFileBase file2, string taskNameHiddenValue2)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());

            if (null != file2)
            {
                try
                {
                    //获得文件名
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file2.FileName);
                    SaveFile(file2, folder, filename);

                    //操作日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.导入, "", "", "导入房号跑盘模板", RequestHelper.GetIP());

                    // 异步调用WCF服务
                    var filePath = Path.Combine(folder, filename);
                    var cityId = Passport.Current.CityId;
                    var fxtCompanyId = Passport.Current.FxtCompanyId;
                    var userId = Passport.Current.ID;
                    Task.Factory.StartNew(() =>
                    {
                        var client = new ExcelUploadServices.ExcelUploadClient();
                        client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                            taskNameHiddenValue2, "HouseNoNew");
                        try { client.Close(); }
                        catch { client.Abort(); }
                    });
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/Project/ImportHouseNo", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                    return base.Back("操作失败！");
                }
            }
            return this.RedirectToAction("ImportHouseNo");
        }

        //删除楼盘导入记录
        public ActionResult DeleteProjectImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteProjectImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //删除楼栋导入记录
        public ActionResult DeleteBuildingImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteBuildingImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //删除房号导入记录
        public ActionResult DeleteHouseNoImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteHouseNoImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        // 验证楼盘是否存在
        [HttpGet]
        public ActionResult ValidProject(int projectId, string projectName, int areaId)
        {
            ViewBag.cityId = Passport.Current.CityId;
            var pinyin = StringHelper.GetPYString(projectName);//简拼
            var quanpin = StringHelper.GetAllPYString(projectName);
            //var proObj = _project.GetProjectNameList(Passport.Current.CityId, projectname.Trim(), Passport.Current.FxtCompanyId, areaId);
            var projectid = _project.IsExistsProjectOnEdit(Passport.Current.CityId, Passport.Current.FxtCompanyId, areaId, projectId, projectName);
            return Json(projectid > 0 ? new { flag = true, jianpin = pinyin, quanpin = quanpin } : new { flag = false, jianpin = pinyin, quanpin = quanpin }, JsonRequestBehavior.AllowGet);
        }

        // 验证宗地号是否存在
        [HttpPost]
        public ActionResult ValidFieldNo(string fieldNo)
        {
            bool flg = _project.ValidFieldNo(Passport.Current.CityId, Passport.Current.FxtCompanyId, fieldNo);
            return Json(flg);
        }
        [HttpPost]
        public ActionResult BindBuildList(int projectId)
        {
            var list = _build.GetBuildingInfo(Passport.Current.CityId, projectId, Passport.Current.FxtCompanyId);
            return Json(new { data = list });
        }

        // 批量添加楼盘图片
        [HttpGet]
        public ActionResult BatchAddProjectPic(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.楼盘图片批量上传, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectResult = result.ToPagedList(pageIndex ?? 1, 30);
            return View("ProjectPicUpload", projectResult);
        }

        [HttpPost]
        public ActionResult BatchAddProjectPic(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var virtualPath = "~/NeedHandledFiles/ImageProcessCenter/" + Passport.Current.FxtCompanyId;
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var fullName = fileName + Path.GetExtension(file.FileName);

            SaveFile(file, Server.MapPath(virtualPath), fullName);//保存压缩文件

            var zipFilePath = Path.Combine(Server.MapPath(virtualPath), fullName); //压缩文件所在物理路径 
            var unZipFilePath = Path.Combine(Server.MapPath(virtualPath), fileName);//解压后的文件夹物理路径

            // 异步调用WCF服务
            var cityId = Passport.Current.CityId;
            var fxtCompanyId = Passport.Current.FxtCompanyId;
            var userId = Passport.Current.ID;
            Task.Factory.StartNew(() =>
            {
                var client = new BatchAddPictureClient();
                client.ProjectPicturesAsync(zipFilePath, unZipFilePath, userId, cityId, fxtCompanyId, taskNameHiddenValue);
                try { client.Close(); }
                catch { client.Abort(); }
            });

            //操作日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.导入, "", "", "楼盘图片批量导入", RequestHelper.GetIP());
            return this.RedirectToAction("BatchAddProjectPic");
        }

        //删除楼盘图片导入记录
        public ActionResult DeleteProjectPicRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteProjectPicRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        // 批量添加楼栋图片
        [HttpGet]
        public ActionResult BatchAddBuildingPic(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.楼栋图片批量上传, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            var buildingResult = result.ToPagedList(pageIndex ?? 1, 30);
            return View("BuildingPicUpload", buildingResult);
        }

        [HttpPost]
        public ActionResult BatchAddBuildingPic(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var virtualPath = "~/NeedHandledFiles/ImageProcessCenter/" + Passport.Current.FxtCompanyId;
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var fullName = fileName + Path.GetExtension(file.FileName);

            SaveFile(file, Server.MapPath(virtualPath), fullName);//保存压缩文件

            var zipFilePath = Path.Combine(Server.MapPath(virtualPath), fullName); //压缩文件所在物理路径 
            var unZipFilePath = Path.Combine(Server.MapPath(virtualPath), fileName);//解压后的文件夹物理路径

            // 异步调用WCF服务
            var cityId = Passport.Current.CityId;
            var fxtCompanyId = Passport.Current.FxtCompanyId;
            var userId = Passport.Current.ID;
            Task.Factory.StartNew(() =>
            {
                var client = new BatchAddPictureClient();
                client.BuildingPicturesAsync(zipFilePath, unZipFilePath, userId, cityId, fxtCompanyId, taskNameHiddenValue);
                try { client.Close(); }
                catch { client.Abort(); }

            });

            //操作日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.导入, "", "", "楼栋图片批量导入", RequestHelper.GetIP());
            return this.RedirectToAction("BatchAddBuildingPic");
        }

        //删除楼栋图片导入记录
        public ActionResult DeleteBuildingPicRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteBuildingPicRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #region 统计
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.统计)]
        public ActionResult Count(ProjectParams ps)
        {
            this.ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text"));
            ProjectStatics model;
            switch (ps.Type)
            {
                case "BHCount":
                    model = BHCount(ps.pageIndex, ps.bhareaname);
                    break;
                case "PPCount":
                    model = PPCount(ps.pageIndex, ps.ppareaname, ps.ProjectName);
                    break;
                default:
                    model = new ProjectStatics();
                    break;
            }
            return View(model);
        }

        //楼盘房号统计
        private ProjectStatics BHCount(int pageIndex, List<int> bhareaname)
        {
            //判断查看自己或全部权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return null;
            if (operate == (int)PermissionLevel.All) self = false;

            this.ViewBag.active = "#divone";
            this.ViewBag.bhareaname = bhareaname == null ? "" : string.Join(",", bhareaname);
            var resultlist = _project.GetBHCount(bhareaname, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    resultlist = resultlist.Where(r => r.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                }
                else
                {
                    resultlist = resultlist.Where(r => r.Creator == Passport.Current.UserName).AsQueryable();
                }
            }
            var result = resultlist.ToPagedList(pageIndex, 30);
            return new ProjectStatics { BHCount = result };
        }

        //楼盘房号统计导出
        public ActionResult BHCountExport(List<int> bhareaname)
        {
            //判断导出自己或全部权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None) return null;
            if (operate == (int)PermissionLevel.All) self = false;

            var result = _project.GetBHCount(bhareaname, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(r => r.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                }
                else
                {
                    result = result.Where(r => r.Creator == Passport.Current.UserName);
                }
            }

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.导出, "", "", "导出楼盘房号统计", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼盘房号统计_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //楼盘图片统计
        private ProjectStatics PPCount(int pageIndex, List<int> ppareaname, string ProjectName)
        {
            //判断查看自己或全部权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return null;
            if (operate == (int)PermissionLevel.All) self = false;

            this.ViewBag.active = "#divtwo";
            this.ViewBag.ppareaname = ppareaname == null ? "" : string.Join(",", ppareaname);
            var resultlist = _project.GetPPCount(ppareaname, ProjectName, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    resultlist = resultlist.Where(r => r.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                }
                else
                {
                    resultlist = resultlist.Where(r => r.Creator == Passport.Current.UserName).AsQueryable();
                }
            }
            var result = resultlist.ToPagedList(pageIndex, 30);
            return new ProjectStatics { PPCount = result };
        }

        //楼盘图片统计导出
        public ActionResult PPCountExport(List<int> ppareaname, string ProjectName)
        {
            //判断导出自己或全部权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None) return null;
            if (operate == (int)PermissionLevel.All) self = false;

            var result = _project.GetPPCount(ppareaname, ProjectName, Passport.Current.CityId, Passport.Current.FxtCompanyId).AsQueryable();
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(r => r.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                }
                else
                {
                    result = result.Where(r => r.Creator == Passport.Current.UserName);
                }
            }

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼盘, SYS_Code_Dict.操作.导出, "", "", "导出楼盘图片统计", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼盘图片统计_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }
        #endregion

        #region 辅助函数
        //获取公司
        [HttpGet]
        public JsonResult GetCompany()
        {
            var data = _dropDownList.GetCompanyName(Passport.Current.CityId, Passport.Current.ProductTypeCode).Select(m => m.ChineseName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //获取楼盘
        [HttpGet]
        public JsonResult GetProject()
        {

            var data = _tempProject.Any() ? _tempProject.AsQueryable() : _project.GetAllProjectName(Passport.Current.CityId, Passport.Current.FxtCompanyId);
            return Json(data.Select(m => new { project = m.projectname + "|" + m.AreaName }), JsonRequestBehavior.AllowGet);
        }

        public object IsOrNot(string str)
        {
            if (str.Trim() == "是") return 1;
            if (str.Trim() == "否") return 0;
            return null;
        }

        public int GetProjectIdByProjectName(int cityId, int areaId, int fxtCompanyId, string projectName)
        {
            var query = _project.GetProjectIdByName(cityId, areaId, fxtCompanyId, projectName).FirstOrDefault();

            return query == null ? -1 : query.projectid;
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

        public int HouseSubid = 0;
        //行政区列表
        [NonAction]
        private IEnumerable<SelectListItem> GetSubAreaName()
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Text = m.AreaName, Value = m.AreaId.ToString() }
                ));
            HouseSubid = area.ToList()[0].AreaId;
            return areaResult;
        }
        //片区列表
        [HttpGet]
        public List<SelectListItem> GetSubAreaName(int areaid)
        {
            var area = _dropDownList.GetSubAreaName(areaid);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Value = m.SubAreaId.ToString(), Text = m.SubAreaName.ToString() }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "不限" });

            return areaResult;
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
            casetypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "全部" });
            return casetypeResult;
        }

        #endregion


        #region 用户中心API
        private CompanyProduct FxtUserCenterService_GetFPInfo(int fxtcompanyid, int cityid, int producttypecode, string username, out Exception msg, int procode)
        {
            string api = ConfigurationManager.AppSettings["fxtusercenterservice"];
            string appid = ConfigurationManager.AppSettings["usercenterserviceappid"];
            string apppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
            string appkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
            string signname = ConfigurationManager.AppSettings["signname"];
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string functionname = "companythirteen";

            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            CompanyProduct cp = null;
            msg = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname };
                    var info = new
                    {
                        uinfo = new { username, token = "" },
                        appinfo = new ApplicationInfo(procode.ToString()),
                        funinfo = new { companyid = fxtcompanyid, producttypecode = producttypecode, cityid = cityid }
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        cp = new CompanyProduct();
                        var returnSInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString())[0];
                        int ParentProductTypeCode = Convert.ToInt32(returnSInfo["parentproducttypecode"]);
                        int ParentShowDataCompanyId = Convert.ToInt32(returnSInfo["parentshowdatacompanyid"]);
                        int IsExportHose = Convert.ToInt32(returnSInfo["isexporthose"]);
                        int IsDeleteTrue = Convert.ToInt32(returnSInfo["isdeletetrue"]);

                        cp.IsExportHose = IsExportHose;
                        cp.IsDeleteTrue = IsDeleteTrue;
                        cp.ParentProductTypeCode = ParentProductTypeCode;
                        cp.ParentShowDataCompanyId = ParentShowDataCompanyId;
                        return cp;
                    }
                    msg = new Exception(rtn.returntext.ToString());
                }
            }
            catch (Exception ex)
            {
                msg = ex;
            }
            return cp;
        }

        private string APIPostBack(string url, string post, string contentType)
        {
            byte[] postData = Encoding.UTF8.GetBytes(post);
            var client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            var result = "";
            try
            {
                byte[] responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                result = JSONHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }
        #endregion

    }
}
