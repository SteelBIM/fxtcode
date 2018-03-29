using System;
using System.Collections.Generic;
using System.Configuration;
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
using FXT.DataCenter.WebUI.Infrastructure.ModelBinder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.Industry.Controllers
{
    [Authorize]
    public class IndustryBuildingController : BaseController
    {
        private readonly ILog _log;
        private readonly IImportTask _importTask;
        private readonly IDropDownList _dropDownList;
        private readonly IIndustryBuilding _industryBuilding;
        private readonly IIndustryProject _industryProject;
        public IndustryBuildingController(IIndustryBuilding industryBuilding, IIndustryProject industryProject, ILog log, IDropDownList dropDownList, IImportTask importTask)
        {
            this._industryBuilding = industryBuilding;
            this._industryProject = industryProject;
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
        }
        //
        // GET: /Industry/IndustryBuilding/

        public ActionResult Index(DatBuildingIndustry building, int? pageIndex)
        {
            this.ViewBag.ProjectId = building.ProjectId;
            this.ViewBag.FxtCompanyId = _industryProject.GetProjectNameById(building.ProjectId, Passport.Current.FxtCompanyId).FxtCompanyId;
            this.ViewBag.ProjectName = _industryProject.GetProjectNameById(building.ProjectId, Passport.Current.FxtCompanyId).ProjectName;

            QueryInDropDownList();

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            building.CityId = Passport.Current.CityId;
            building.FxtCompanyId = Passport.Current.FxtCompanyId;

            var pageSize = 30; int totalCount;
            var buildings = _industryBuilding.GetIndustryBuildings(building, self, pageIndex ?? 1, pageSize, out totalCount);
            var result = new PagedList<DatBuildingIndustry>(buildings, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", result);
        }

        #region 编辑
        [HttpGet]
        public ActionResult Edit(string id, int projectId)
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            this.ViewBag.ProjectId = projectId;
            var splitArray = id.Split('#');

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有修改权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            if (self)
            {
                if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }

            var result = _industryBuilding.GetIndustryBuilding(int.Parse(splitArray[1]), int.Parse(splitArray[0]));
            EditInDropDownList();
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(DatBuildingIndustry building)
        {
            try
            {
                building.SaveDateTime = DateTime.Now;
                building.SaveUser = Passport.Current.ID;

                _industryBuilding.UpdateIndustryBuilding(building, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.修改, building.BuildingId.ToString(), building.BuildingName, "修改工业楼栋", RequestHelper.GetIP());

                return RedirectToAction("Index", new { projectId = building.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        //复制楼栋
        public ActionResult CopyBuilding(string buildingName, string destBuildingName, int buildingId, int projectId)
        {
            try
            {
                var ret = _industryBuilding.CopyBuilding(Passport.Current.CityId, Passport.Current.FxtCompanyId, buildingName, destBuildingName, buildingId, projectId);
                return Json(new { result = ret > 0, ret = ret });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/CopyBuilding", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, ret = 0 });
            }
        }
        #endregion

        #region 新增
        [HttpPost]
        public JsonResult IsExistBuildingIndustry(long projectId, long buildingId, string buildingName)
        {
            return Json(_industryBuilding.GetIndustryBuildingId(projectId, buildingId, buildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId));
        }

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create(int projectId)
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            this.ViewBag.ProjectId = projectId;
            EditInDropDownList();
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(DatBuildingIndustry building)
        {
            try
            {
                building.CityId = Passport.Current.CityId;
                building.FxtCompanyId = Passport.Current.FxtCompanyId;
                building.CreateTime = DateTime.Now;
                building.Creator = Passport.Current.ID;

                _industryBuilding.AddIndustryBuilding(building);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.新增, "", "", "新增工业楼栋", RequestHelper.GetIP());

                return RedirectToAction("Index", new { projectId = building.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        //复制楼栋
        //public ActionResult CopyBuilding(string buildingName, string destBuildingName, int buildingId, int projectId)
        //{
        //    try
        //    {
        //        var ret = _industryBuilding.CopyBuilding(Passport.Current.CityId, Passport.Current.FxtCompanyId,buildingName,destBuildingName, buildingId, projectId);
        //        return Json(ret > 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog("Industry/IndustryBuilding/CopyBuilding", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
        //        return Json(false);
        //    }
        //}
        #endregion

        #region 删除
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
                Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None) return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                if (operate == (int)PermissionLevel.All) self = false;

                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    if (self)
                    {
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                    }
                    var building = _industryBuilding.GetIndustryBuilding(int.Parse(array[1]), int.Parse(array[0]));
                    building.SaveDateTime = DateTime.Now;
                    building.SaveUser = Passport.Current.ID;
                    _industryBuilding.DeleteIndustryBuilding(building, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除工业楼栋", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }
        #endregion

        #region 图片
        public ActionResult IndustryBuildingPicture(int projectId, int buildingId, int? pageIndex)
        {
            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View("Picture", new List<LNKBPhoto>().ToPagedList(1, 30));
            if (operate == (int)PermissionLevel.All) self = false;

            var lnkPhoto = new LNKBPhoto
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                BuildingId = buildingId
            };

            var result = _industryBuilding.GetIndustryBuildingPhotoes(lnkPhoto, self).AsQueryable();
            var lnkPhotoResult = result.ToPagedList(pageIndex ?? 1, 30);

            return View("Picture", lnkPhotoResult);
        }

        [HttpGet]
        public ActionResult PictureCreate(int projectId, string buildingId)
        {
            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = int.Parse(buildingId.TrimEnd('?'));
            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            return View();
        }

        [HttpPost]
        public ActionResult PictureCreate()
        {
            var isSavedSuccessfully = true;
            var count = 0;

            var projectId = Request["projectId"];
            var buildingId = Request["buildingId"].TrimEnd('?');
            var photoTypeCode = Request["photoTypeCode"];

            if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(photoTypeCode))
            {
                return Json(new { Result = false, Count = count });
            }

            try
            {
                var virtualPath = "/ProjectPic/Industry/IndustryBuilding/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";

                var IsOss = ConfigurationHelper.IsOss;
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
                            //修改OSS存储
                            var fs = file.InputStream;
                            StreamContent content = new StreamContent(fs);
                            var result = OssHelper.UpFileAsync(content, virtualFilePath);

                            //保存该条图片信息到数据库
                            var lnkBPhoto = new LNKBPhoto()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                BuildingId = int.Parse(buildingId),
                                Valid = 1,
                                SaveUser = Passport.Current.ID,
                                SaveDate = DateTime.Now,
                            };
                            _industryBuilding.AddIndustryBuildingPhoto(lnkBPhoto);
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
                            var lnkBPhoto = new LNKBPhoto()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                BuildingId = int.Parse(buildingId),
                                Valid = 1,
                                SaveUser = Passport.Current.ID,
                                SaveDate = DateTime.Now,
                            };
                            _industryBuilding.AddIndustryBuildingPhoto(lnkBPhoto);
                            count++;
                        }
                    }
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.新增, "", "", "批量新增商业街项目图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/PictureCreate", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        [HttpGet]
        public ActionResult PictureEdit(string chValue, int projectId, string buildingId)
        {
            var splitArray = chValue.Split('#');
            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId.TrimEnd('?');
            this.ViewBag.PhotoTypeCodeName = GetDictById(SYS_Code_Dict._图片类型);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var picture = _industryBuilding.GetIndustryBuildingPhoto(int.Parse(splitArray[1]), int.Parse(splitArray[0]));
            return View(picture);
        }

        [HttpPost]
        public ActionResult PictureEdit(LNKBPhoto lnkbhoto)
        {
            try
            {
                lnkbhoto.SaveDate = DateTime.Now;
                lnkbhoto.SaveUser = Passport.Current.ID;

                _industryBuilding.UpdateIndustryBuildingPhoto(lnkbhoto, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.修改, lnkbhoto.Id.ToString(), lnkbhoto.PhotoName, "修改工业楼栋图片", RequestHelper.GetIP());

                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/PictureEdit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        [HttpPost]
        public ActionResult UploadPicture(int projectId, int buildingId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/Industry/IndustryBuilding/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";

                var IsOss = ConfigurationHelper.IsOss;
                if (IsOss)
                {
                    //取得文件的扩展名,并转换成小写
                    var fileExtension = Path.GetExtension(upImg.FileName).ToLower();
                    if (!ImageHandler.CheckValidExt(fileExtension))
                    {
                        return Json(new { result = false, msg = "您上传的文件格式有误!" });
                    }
                    string fileName = upImg.FileName.Substring(0, upImg.FileName.IndexOf('.'));

                    //存到服务器上的虚拟路径
                    var virpath = filePath + DateTime.Now.ToString("yyyyMMddmmHHss") + DateTime.Now.Millisecond.ToString() + fileExtension;

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
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/UploadPicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        [HttpPost]
        public ActionResult DeletePicture(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }

            try
            {
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None) return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                if (operate == (int)PermissionLevel.All) self = false;

                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    if (self)
                    {
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                    }
                    var result = _industryBuilding.GetIndustryBuildingPhoto(int.Parse(array[1]), int.Parse(array[0]));
                    result.SaveDate = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _industryBuilding.DeleteIndustryBuildingPhoto(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除工业楼栋图片", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/DeletePicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.工业楼栋信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);

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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.导入, "", "", "导入工业楼栋", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "IndustryBuilding");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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

        //删除导入记录
        public ActionResult DeleteIndustryBuildingImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Industry/IndustryBuilding/DeleteIndustryBuildingImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 导出
        [HttpGet]
        public ActionResult Export([JsonModelBinder]DatBuildingIndustry datBuildingIndustry)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            datBuildingIndustry.FxtCompanyId = Passport.Current.FxtCompanyId;
            datBuildingIndustry.CityId = Passport.Current.CityId;

            int totalCount;
            var result = _industryBuilding.GetIndustryBuildings(datBuildingIndustry, self, 1, int.MaxValue, out totalCount);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.导出, "", "", "导出工业楼栋", RequestHelper.GetIP());

            _excelExportHeader(Passport.Current.CityName + "_工业_楼栋数据_");
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        #region 自定义导出
        [HttpGet]
        public ActionResult SelfDefineExport()
        {
            QueryInDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult SelfDefineExport(DatBuildingIndustry buildingIndustry, List<string> building)
        {
            try
            {
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                buildingIndustry.FxtCompanyId = Passport.Current.FxtCompanyId;
                buildingIndustry.CityId = Passport.Current.CityId;
                var dt = _industryBuilding.BuildingSelfDefineExport(buildingIndustry, building, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
                if (dt != null && dt.Rows.Count > 0)
                {
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode("工业楼栋自定义导出", System.Text.Encoding.GetEncoding("UTF-8")) + DateTime.Now.ToShortDateString() + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";

                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼栋, SYS_Code_Dict.操作.导出, "", "", "自定义导出工业楼栋", RequestHelper.GetIP());

                    using (MemoryStream ms = ExcelHandle.RenderToExcel(dt))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }

                }
                return this.Back("温馨提示:没有您要导出的数据");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryBuilding/SelfDefineExport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        #endregion
        #endregion

        #region 统计
        [HttpGet]
        public JsonResult IndustryHouseCount(int buildingId)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var result = _industryBuilding.GetHouseCounts(buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 辅助方法
        [NonAction]
        private void QueryInDropDownList()
        {
            this.ViewBag.IndustryTypeName = GetDictById(SYS_Code_Dict._工业类型);
            this.ViewBag.PurposeCodeName = GetDictById(SYS_Code_Dict._土地用途);
            this.ViewBag.StructureCodeName = GetDictById(SYS_Code_Dict._建筑结构);
            this.ViewBag.BuildingTypeCodeName = GetDictById(SYS_Code_Dict._建筑类型);
            this.ViewBag.RentSaleTypeName = GetDictById(SYS_Code_Dict._经营方式);
        }

        [NonAction]
        private void EditInDropDownList()
        {
            QueryInDropDownList();
            this.ViewBag.LobbyFitmentName = GetDictById(SYS_Code_Dict._客厅装修);
            this.ViewBag.LiftFitmentName = GetDictById(SYS_Code_Dict._客厅装修);
            this.ViewBag.PublicFitmentName = GetDictById(SYS_Code_Dict._客厅装修);
            this.ViewBag.WallFitmentName = GetDictById(SYS_Code_Dict._办公外墙装修);
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
