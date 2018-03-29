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
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.Office.Controllers
{
    [Authorize]
    public class OfficeBuildingController : BaseController
    {
        private readonly ILog _log;
        private readonly IImportTask _importTask;
        private readonly IDropDownList _dropDownList;
        private readonly IOfficeBuilding _officeBuilding;
        private readonly IOfficeProject _officeProject;

        public OfficeBuildingController(IOfficeBuilding officeBuilding, IOfficeProject officeProject, ILog log, IDropDownList dropDownList, IImportTask importTask)
        {
            this._officeBuilding = officeBuilding;
            this._officeProject = officeProject;
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
        }

        public ActionResult Index(DatBuildingOffice datBuildingOffice, int? pageIndex)
        {
            this.ViewBag.ProjectId = datBuildingOffice.ProjectId;
            this.ViewBag.FxtCompanyId = _officeProject.GetProjectNameById(datBuildingOffice.ProjectId, Passport.Current.FxtCompanyId).FxtCompanyId;
            this.ViewBag.ProjectName = _officeProject.GetProjectNameById(datBuildingOffice.ProjectId, Passport.Current.FxtCompanyId).ProjectName;

            QueryInDropDownList();

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            datBuildingOffice.CityId = Passport.Current.CityId;
            datBuildingOffice.FxtCompanyId = Passport.Current.FxtCompanyId;

            var pageSize = 30; int totalCount;
            var officeBuildings = _officeBuilding.GetOfficeBuildings(datBuildingOffice, self, pageIndex ?? 1, pageSize, out totalCount);
            var result = new PagedList<DatBuildingOffice>(officeBuildings, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", result);

        }

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create(int projectId)
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            this.ViewBag.ProjectId = projectId;
            EditInDropDownList();
            return View();
        }

        [HttpPost]
        public JsonResult IsExistBuildingOffice(long projectId, long buildingId, string buildingName)
        {
            return Json(_officeBuilding.GetOfficeBuildingId(projectId, buildingId, buildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId));
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(DatBuildingOffice datBuildingOffice)
        {
            try
            {
                datBuildingOffice.CityId = Passport.Current.CityId;
                datBuildingOffice.FxtCompanyId = Passport.Current.FxtCompanyId;
                datBuildingOffice.CreateTime = DateTime.Now;
                datBuildingOffice.Creator = Passport.Current.ID;
                datBuildingOffice.Weight = datBuildingOffice.Weight.HasValue ? datBuildingOffice.Weight : 1;
                _officeBuilding.AddOfficeBuilding(datBuildingOffice);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.新增, "", "", "新增办公楼栋", RequestHelper.GetIP());

                return RedirectToAction("Index", new { projectId = datBuildingOffice.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }


        //编辑
        [HttpGet]
        public ActionResult Edit(string id, int projectId)
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            this.ViewBag.ProjectId = projectId;

            var splitArray = id.Split('#');

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有修改权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            if (self)
            {
                if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }

            var result = _officeBuilding.GetOfficeBuilding(int.Parse(splitArray[1]), int.Parse(splitArray[0]));
            EditInDropDownList();
            return View("Create", result);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(DatBuildingOffice datBuildingOffice)
        {
            try
            {
                datBuildingOffice.SaveDateTime = DateTime.Now;
                datBuildingOffice.SaveUser = Passport.Current.ID;
                datBuildingOffice.Weight = datBuildingOffice.Weight.HasValue ? datBuildingOffice.Weight : 1;
                _officeBuilding.UpdateOfficeBuilding(datBuildingOffice, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.修改, datBuildingOffice.BuildingId.ToString(), datBuildingOffice.BuildingName, "修改办公楼栋", RequestHelper.GetIP());

                return RedirectToAction("Index", new { projectId = datBuildingOffice.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        //复制楼栋
        public ActionResult CopyBuilding(string buildingName, string destBuildingName, int buildingId, int projectId)
        {
            try
            {
                var ret = _officeBuilding.CopyBuilding(Passport.Current.CityId, Passport.Current.FxtCompanyId, buildingName, destBuildingName, buildingId, projectId);
                return Json(new { result = ret > 0, ret = ret });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/CopyBuilding", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, ret = 0 });
            }
        }


        //删除办公楼栋
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
                Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    var officeBuilding = _officeBuilding.GetOfficeBuilding(int.Parse(array[1]), int.Parse(array[0]));
                    officeBuilding.SaveDateTime = DateTime.Now;
                    officeBuilding.SaveUser = Passport.Current.ID;
                    _officeBuilding.DeleteOfficeBuilding(officeBuilding, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除办公楼栋", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        //导出
        [HttpGet]
        public ActionResult Export([JsonModelBinder]DatBuildingOffice datBuildingOffice)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            datBuildingOffice.FxtCompanyId = Passport.Current.FxtCompanyId;
            datBuildingOffice.CityId = Passport.Current.CityId;

            int totalCount;
            var result = _officeBuilding.GetOfficeBuildings(datBuildingOffice, self, 1, int.MaxValue, out totalCount);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.导出, "", "", "导出办公楼栋", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_办公_楼栋数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
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
        public ActionResult SelfDefineExport(DatBuildingOffice buildingOffice, List<string> building)
        {
            try
            {
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                buildingOffice.FxtCompanyId = Passport.Current.FxtCompanyId;
                buildingOffice.CityId = Passport.Current.CityId;
                var dt = _officeBuilding.BuildingSelfDefineExport(buildingOffice, building, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
                if (dt != null && dt.Rows.Count > 0)
                {
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.导出, "", "", "自定义导出办公楼栋", RequestHelper.GetIP());

                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_办公_自定义导出楼栋_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";

                    using (var ms = ExcelHandle.RenderToExcel(dt))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }

                }
                return this.Back("没有您要导出的数据");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/SelfDefineExport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        #endregion

        //办公楼栋图片
        public ActionResult OfficeBuildingPicture(int projectId, int buildingId, int? pageIndex)
        {
            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View("Picture", new List<LnkBPhoto>().ToPagedList(1, 30));
            if (operate == (int)PermissionLevel.All) self = false;

            var lnkPhoto = new LnkBPhoto
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                BuildingId = buildingId

            };

            var result = _officeBuilding.GetOfficeBuildingPhotoes(lnkPhoto, self).AsQueryable();
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
                var virtualPath = "/ProjectPic/Office/OfficeBuilding/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";
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
                            var lnkBPhoto = new LnkBPhoto()
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
                            _officeBuilding.AddOfficeBuildingPhoto(lnkBPhoto);
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
                            var lnkBPhoto = new LnkBPhoto()
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
                            _officeBuilding.AddOfficeBuildingPhoto(lnkBPhoto);
                            count++;
                        }
                    }
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.新增, "", "", "新增办公楼栋图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/PictureCreate", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        //图片上传
        [HttpPost]
        public ActionResult UploadPicture(int projectId, int buildingId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/Office/OfficeBuilding/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";

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
                LogHelper.WriteLog("Office/OfficeBuilding/UploadPicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        public ActionResult PictureEdit(string chValue, int projectId, string buildingId)
        {
            var splitArray = chValue.Split('#');
            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId.TrimEnd('?');
            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var picture = _officeBuilding.GetOfficeBuildingPhoto(int.Parse(splitArray[1]), int.Parse(splitArray[0]));
            return View(picture);
        }
        //办公楼栋图片编辑保存
        [HttpPost]
        public ActionResult PictureEdit(LnkBPhoto lnkbhoto)
        {
            try
            {
                lnkbhoto.SaveDate = DateTime.Now;
                lnkbhoto.SaveUser = Passport.Current.ID;

                _officeBuilding.UpdateOfficeBuildingPhoto(lnkbhoto, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.修改, "", "", "修改办公楼栋图片", RequestHelper.GetIP());

                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/PictureEdit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }


        //删除办公楼栋图片
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
                Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    var result = _officeBuilding.GetOfficeBuildingPhoto(int.Parse(array[1]), int.Parse(array[0]));
                    result.SaveDate = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _officeBuilding.DeleteOfficeBuildingPhoto(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除办公楼栋图片", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/DeletePicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        //导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.办公楼栋信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);

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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.办公楼栋, SYS_Code_Dict.操作.导入, "", "", "导入办公楼栋", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "OfficeBuilding");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeBuilding/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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


        //删除办公楼栋导入记录
        public ActionResult DeleteOfficeBuildingImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Office/OfficeBuilding/DeleteOfficeBuildingImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }



        #region 帮助程序

        //查询时下拉选择项
        [NonAction]
        private void QueryInDropDownList()
        {
            this.ViewBag.OfficeTypeName = GetDictById(SYS_Code_Dict._办公楼等级);
            this.ViewBag.PurposeName = GetDictById(SYS_Code_Dict._土地用途);
            this.ViewBag.StructureName = GetDictById(SYS_Code_Dict._建筑结构);
            this.ViewBag.BuildingTypeName = GetDictById(SYS_Code_Dict._建筑类型);
            this.ViewBag.RentSaleTypeName = GetDictById(SYS_Code_Dict._经营方式);
        }

        //创建、编辑时下拉选择项
        [NonAction]
        private void EditInDropDownList()
        {
            QueryInDropDownList();
            this.ViewBag.LobbyFitmentName = GetDictById(SYS_Code_Dict._客厅装修);
            this.ViewBag.LiftFitmentName = GetDictById(SYS_Code_Dict._客厅装修);
            this.ViewBag.PublicFitmentName = GetDictById(SYS_Code_Dict._客厅装修);
            this.ViewBag.WallFitmentName = GetDictById(SYS_Code_Dict._办公外墙装修);
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
        #endregion

        [HttpGet]
        public JsonResult OfficeHouseCount(int buildingId)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var result = _officeBuilding.GetHouseCounts(buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
