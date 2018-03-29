using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Threading.Tasks;
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.Industry.Controllers
{
    [Authorize]
    public class IndustryProjectController : BaseController
    {
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IIndustrySubArea _industrySubArea;
        private readonly IIndustryProject _industryProject;
        private readonly IImportTask _importTask;
        public IndustryProjectController(ILog log, IDropDownList dropdownlist, IIndustrySubArea industrySubArea, IIndustryProject industryProject, IImportTask importTask)
        {
            this._log = log;
            this._dropDownList = dropdownlist;
            this._industrySubArea = industrySubArea;
            this._industryProject = industryProject;
            this._importTask = importTask;
        }
        //
        // GET: /Industry/IndustryProject/

        public ActionResult Index(DatProjectIndustry project, int? pageIndex)
        {
            BindDropDownList(project.AreaId == 0 ? -1 : project.AreaId, Passport.Current.FxtCompanyId);

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View(new List<DatProjectIndustry>().ToPagedList(1, 30));
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            project.FxtCompanyId = Passport.Current.FxtCompanyId;
            project.CityId = Passport.Current.CityId;

            var pageSize = 30;
            int totalCount;
            var result = _industryProject.GetProjectIndustrys(project, pageIndex ?? 1, pageSize, out totalCount, self).AsQueryable();

            var projectList = new PagedList<DatProjectIndustry>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", projectList);
        }

        #region 编辑
        [HttpPost]
        public JsonResult IsExistProjectIndustry(int areaId, long projectId, string projectName)
        {
            return Json(_industryProject.IsExistProjectIndustry(areaId, projectId, projectName, Passport.Current.CityId, Passport.Current.FxtCompanyId));
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            var splitArray = id.Split('#');

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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

            var result = _industryProject.GetProjectNameById(int.Parse(splitArray[1]), int.Parse(splitArray[0]));
            var subAreaOther = new SubAreaOther();
            if (result.FxtCompanyId != Passport.Current.FxtCompanyId && result.SubAreaId > 0)
            {
                subAreaOther.areaId = result.AreaId;
                subAreaOther.subAreaId = result.SubAreaId;
                subAreaOther.subAreaName = result.SubAreaName;

                this.ViewBag.areaIdOther = result.AreaId;
                this.ViewBag.subAreaIdOther = result.SubAreaId;
                this.ViewBag.subAreaNameOther = result.SubAreaName;
            }
            BindDropDownList(result.AreaId, Passport.Current.FxtCompanyId, subAreaOther);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(DatProjectIndustry project)
        {
            try
            {
                project.SaveDateTime = DateTime.Now;
                project.SaveUser = Passport.Current.ID;

                _industryProject.UpdateProjectIndustry(project, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.修改, project.ProjectId.ToString(), project.ProjectName, "修改工业楼盘", RequestHelper.GetIP());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }
        #endregion

        #region 图片
        public ActionResult IndustryProjectPicture(int? projectId, int? pageIndex)
        {
            ViewBag.projectId = projectId;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View("Picture", new List<LNKPPhoto>().ToPagedList(1, 30));
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var lnkPhoto = new LNKPPhoto
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                ProjectId = projectId ?? -1
            };

            this.ViewBag.ProjectId = lnkPhoto.ProjectId;
            this.ViewBag.FxtCompanyId = lnkPhoto.FxtCompanyId;

            var result = _industryProject.GetIndustryProjectPhotoes(lnkPhoto, self).AsQueryable();
            var lnkPhotoResult = result.ToPagedList(pageIndex ?? 1, 30);

            return View("Picture", lnkPhotoResult);
        }

        [HttpGet]
        //图片创建
        public ActionResult PictureCreate(int projectId)
        {
            this.ViewBag.projectId = projectId;
            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            return View();
        }

        [HttpPost]
        public ActionResult PictureCreate()
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
                var virtualPath = "/ProjectPic/Industry/IndustryProject/" + Passport.Current.CityId + "/" + projectId + "/";
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
                            var lnkPPhoto = new LNKPPhoto()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                ProjectId = int.Parse(projectId),
                                Valid = 1,
                                SaveUser = Passport.Current.ID,
                                SaveDate = DateTime.Now,
                            };
                            _industryProject.AddIndustryProjectPhoto(lnkPPhoto);
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
                            var lnkPPhoto = new LNKPPhoto()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                ProjectId = int.Parse(projectId),
                                Valid = 1,
                                SaveUser = Passport.Current.ID,
                                SaveDate = DateTime.Now,
                            };
                            _industryProject.AddIndustryProjectPhoto(lnkPPhoto);
                            count++;
                        }
                    }
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.新增, "", "", "新增工业楼盘图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/PictureCreate", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        //图片编辑
        [HttpGet]
        public ActionResult PictureEdit(string chValue)
        {
            var splitArray = chValue.TrimEnd('?').Split('#');
            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            var picture = _industryProject.GetIndustryProjectPhoto(int.Parse(splitArray[1]), int.Parse(splitArray[0]));
            this.ViewBag.projectId = picture.ProjectId;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            return View(picture);
        }

        //图片编辑保存
        [HttpPost]
        public ActionResult PictureEdit(LNKPPhoto lnkPhoto)
        {
            try
            {
                lnkPhoto.SaveDate = DateTime.Now;
                lnkPhoto.SaveUser = Passport.Current.ID;

                _industryProject.UpdateIndustryProjectPhoto(lnkPhoto, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.修改, lnkPhoto.Id.ToString(), lnkPhoto.PhotoName, "修改工业楼盘图片", RequestHelper.GetIP());

                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/PictureEdit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        //图片删除
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
                    if (self)
                    {
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                    }
                    var result = _industryProject.GetIndustryProjectPhoto(int.Parse(array[1]), int.Parse(array[0]));
                    result.SaveDate = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _industryProject.DeleteIndustryProjectPhoto(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除工业楼盘图片", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/DeletePicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //图片上传
        [HttpPost]
        public ActionResult UploadPicture(string projectId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/Industry/IndustryProject/" + Passport.Current.CityId + "/" + projectId + "/";

                var IsOss = ConfigurationHelper.IsOss;
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
                LogHelper.WriteLog("Industry/IndustryProject/UploadPicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }
        #endregion

        #region 新增

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            BindDropDownList(-1, Passport.Current.FxtCompanyId);
            return View("Edit");
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(DatProjectIndustry project)
        {
            try
            {
                project.CityId = Passport.Current.CityId;
                project.FxtCompanyId = Passport.Current.FxtCompanyId;
                project.CreateTime = DateTime.Now;
                project.Creator = Passport.Current.ID;

                _industryProject.AddProjectIndustry(project);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.新增, "", "", "新增工业楼盘", RequestHelper.GetIP());

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }

        }
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

                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    if (self)
                    {
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                    }
                    var result = _industryProject.GetProjectNameById(int.Parse(array[1]), int.Parse(array[0]));
                    result.SaveDateTime = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _industryProject.DeleteProjectIndustry(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除工业楼盘", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #endregion

        #region 导出

        #region 楼盘导出
        [HttpGet]
        public ActionResult Export(int? areaid, int? subareaid, string projectName, string otherName, int? IndustryType, int? RentSaleType)
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

            var project = new DatProjectIndustry
            {
                FxtCompanyId = Passport.Current.FxtCompanyId,
                CityId = Passport.Current.CityId,
                AreaId = areaid ?? 0,
                SubAreaId = subareaid ?? 0,
                ProjectName = projectName,
                OtherName = otherName,
                IndustryType = IndustryType ?? 0,
                RentSaleType = RentSaleType,
            };

            int totalCount;
            var result = _industryProject.GetProjectIndustrys(project, 1, int.MaxValue, out totalCount, self);

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_工业_楼盘数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.导出, "", "", "导出工业楼盘", RequestHelper.GetIP());

            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }
        #endregion

        #region 自定义导出
        [HttpGet]
        public ActionResult SelfDefineExport()
        {
            BindDropDownList(-1, Passport.Current.FxtCompanyId);
            return View();
        }

        [HttpPost]
        public ActionResult SelfDefineExport(DatProjectIndustry projectIndustry, List<string> project)
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

                projectIndustry.FxtCompanyId = Passport.Current.FxtCompanyId;
                projectIndustry.CityId = Passport.Current.CityId;
                var dt = _industryProject.ProjectSelfDefineExport(projectIndustry, project, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
                if (dt != null && dt.Rows.Count > 0)
                {
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_工业_自定义导出工业楼盘_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";

                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.导出, "", "", "自定义导出工业楼盘", RequestHelper.GetIP());

                    using (MemoryStream ms = ExcelHandle.RenderToExcel(dt))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }

                }
                return this.Back("温馨提示:没有您要导出的数据");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/SelfDefineExport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        #endregion
        #endregion

        #region 导入

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.工业楼盘信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var taskList = result.ToPagedList(pageIndex ?? 1, 30);
            return View(taskList);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());
            if (null == file) return this.RedirectToAction("UploadFile");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业楼盘, SYS_Code_Dict.操作.导入, "", "", "导入工业楼盘", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "IndustryProject");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryProject/UploadFile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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
        public ActionResult DeleteIndustryProjectImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Industry/IndustryProject/DeleteIndustryProjectImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #endregion

        #region 统计
        [HttpGet]
        public JsonResult IndustryBuildingCount(int projectId)
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

            var result = _industryProject.GetBuildingCounts(projectId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private void BindDropDownList(int areaId, int fxtCompanyId, SubAreaOther subAreaOther = null)
        {
            this.ViewBag.AreaName = GetAreaName();
            this.ViewBag.SubAreaName = GetSubAreaNamesByAreaId(areaId, fxtCompanyId, subAreaOther);
            this.ViewBag.CorrelationTypeName = GetDictById(SYS_Code_Dict._商圈关联度);
            this.ViewBag.PurposeCodeName = GetDictById(SYS_Code_Dict._土地用途);
            this.ViewBag.BuildingTypeName = GetDictById(SYS_Code_Dict._建筑类型);
            this.ViewBag.IndustryTypeName = GetDictById(SYS_Code_Dict._工业类型);
            this.ViewBag.TrafficTypeName = GetDictById(SYS_Code_Dict._交通便捷度);
            this.ViewBag.ParkingLevelName = GetDictById(SYS_Code_Dict._交通便捷度);
            this.ViewBag.ParkingTypeName = GetDictById(SYS_Code_Dict._停车位类型);
            this.ViewBag.RentSaleTypeName = GetDictById(SYS_Code_Dict._经营方式);
            this.ViewBag.AirConditionTypeName = GetDictById(SYS_Code_Dict._空调系统类型);
        }

        //行政区列表
        [NonAction]
        private IEnumerable<SelectListItem> GetAreaName()
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Value = m.AreaId.ToString(), Text = m.AreaName }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            return areaResult;
        }

        //根据行政区ID获取所有片区
        [NonAction]
        private IEnumerable<SelectListItem> GetSubAreaNamesByAreaId(int areaId, int fxtCompanyId, SubAreaOther subAreaOther = null)
        {
            var list = new List<SelectListItem>();
            var subAreaNames = _industrySubArea.GetSubAreaNamesByAreaId(areaId, fxtCompanyId, Passport.Current.CityId);

            subAreaNames.ToList().ForEach(m => list.Add(
                new SelectListItem
                {
                    Value = m.SubAreaId.ToString(),
                    Text = m.SubAreaName,
                }
                ));
            if (subAreaOther != null && subAreaOther.areaId != 0 && (subAreaOther.areaId == areaId || areaId == -1))
            {
                list.Add(new SelectListItem
                    {
                        Value = subAreaOther.subAreaId.ToString(),
                        Text = subAreaOther.subAreaName,
                    });
            }
            list.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return list;
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

        [HttpPost]
        public ActionResult GetSubAreaName(int areaId, int? areaIdOther, int? subAreaIdOther, string subAreaNameOther)
        {
            var subAreaOther = new SubAreaOther();
            if (areaIdOther != null && subAreaIdOther != null)
            {
                subAreaOther.areaId = areaIdOther ?? 0;
                subAreaOther.subAreaId = subAreaIdOther ?? 0;
                subAreaOther.subAreaName = subAreaNameOther;
            }
            var result = GetSubAreaNamesByAreaId(areaId, Passport.Current.FxtCompanyId, subAreaOther);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetProjectNamePinYin(string ProjectName)
        {
            string PinYin = StringHelper.GetPYString(ProjectName);
            //string PinYinAll = StringHelper.ConvertPinYin(ProjectName, 500);
            string PinYinAll = StringHelper.GetAllPYString(ProjectName);
            return Json(new { PinYin = PinYin, PinYinAll = PinYinAll });
        }

        public class SubAreaOther
        {
            public int areaId { get; set; }
            public int subAreaId { get; set; }
            public string subAreaName { get; set; }
        }
    }
}
