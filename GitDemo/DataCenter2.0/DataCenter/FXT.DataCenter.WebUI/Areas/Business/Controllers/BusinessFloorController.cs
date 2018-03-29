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
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.Business.Controllers
{
    [Authorize]
    public class BusinessFloorController : BaseController
    {
        private readonly IBusinessStreet _project;
        private readonly IDat_Building_Biz _build;
        private readonly IDat_Floor_Biz _floor;
        private readonly IDat_House_Biz _house;
        private readonly IBusinessFloorPhoto _linkFPhoto;
        private readonly ILog _log;
        private readonly IImportTask _importTask;
        private readonly IDropDownList _dropDownList;
        public BusinessFloorController(IBusinessStreet project, IDat_Building_Biz build, IDat_Floor_Biz florr, IDat_House_Biz house, IBusinessFloorPhoto linkFPhoto, ILog log, IDropDownList dropDownList, IImportTask importTask)
        {
            this._project = project;
            this._linkFPhoto = linkFPhoto;
            this._floor = florr;
            this._house = house;
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
            this._build = build;
        }

        [HttpGet]
        public ActionResult Index(Dat_Floor_Biz floor_biz, int? pageIndex)
        {
            SaveVal(floor_biz.ProjectId, Convert.ToInt32(floor_biz.BuildingId));
            floor_biz.CityId = Passport.Current.CityId;
            floor_biz.FxtCompanyId = Passport.Current.FxtCompanyId;
            ViewBag.ProjectId = floor_biz.ProjectId;
            ViewBag.BuildingId = floor_biz.BuildingId;
            #region 查看权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion
            var result = _floor.GetDat_Floor_BizList(floor_biz, self).ToPagedList(pageIndex ?? 1, 30);
            ViewBag.rentSaleTypeName = BindDropDown(SYS_Code_Dict._经营方式);
            ViewBag.bizTypeName = BindDropDown(SYS_Code_Dict._商业细分类型);
            return View(result);
        }

        //新建
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create(string Id)
        {
            string projectId = Id.Split('#')[0], buildingId = Id.Split('#')[1];
            SaveVal(int.Parse(projectId), int.Parse(buildingId));
            ViewBag.rentSaleTypeName = BindDropDown(SYS_Code_Dict._经营方式);
            ViewBag.bizTypeName = BindDropDown(SYS_Code_Dict._商业细分类型);
            var floor = new Dat_Floor_Biz
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId
            };
            return View("EdityFloor", floor);
        }

        [HttpPost]
        public ActionResult Create(Dat_Floor_Biz floor)
        {
            try
            {
                floor.Weight = floor.Weight.HasValue ? floor.Weight : 1;
                floor.CityId = Passport.Current.CityId;
                floor.FxtCompanyId = Passport.Current.FxtCompanyId;
                _floor.AddDat_Floor_Biz(floor);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼层, SYS_Code_Dict.操作.新增, "", "", "商业楼层", RequestHelper.GetIP());
                return RedirectToAction("index", new { projectId = floor.ProjectId, buildingId = floor.BuildingId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessFloor/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        //编辑
        [HttpGet]
        public ActionResult EdityFloor(string id)
        {
            var splitArray = id.Split('#');
            #region 操作权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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
            #endregion
            int floorId = int.Parse(splitArray[1]), cityId = Passport.Current.CityId, fxtCompanyId = Passport.Current.FxtCompanyId;
            var result = _floor.GetDat_Floor_BizById(floorId, cityId, fxtCompanyId);
            SaveVal(Convert.ToInt32(splitArray[2]), Convert.ToInt32(result.BuildingId));
            ViewBag.rentSaleTypeName = BindDropDown(SYS_Code_Dict._经营方式);
            ViewBag.BizTypeName = BindDropDown(SYS_Code_Dict._商业细分类型);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            return View(result);
        }

        [HttpPost]
        public ActionResult EdityFloor(Dat_Floor_Biz floor)
        {
            try
            {
                floor.Weight = floor.Weight.HasValue ? floor.Weight : 1;
                int result = _floor.UpdateDat_Floor_Biz(floor, Passport.Current.FxtCompanyId);
                return RedirectToAction("Index", new { projectId = floor.ProjectId, buildingId = floor.BuildingId });
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Business/BusinessFloor/EdityFloor", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        //删除
        [HttpPost]
        public ActionResult DeleteFloor(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                //return this.Back("请选择要删除的数据！");
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {
                #region 权限验证
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                #endregion
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
                    _floor.DeleteDat_Floor_Biz(int.Parse(array[1]), Passport.Current.UserName, Passport.Current.CityId, int.Parse(array[0]), Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼层, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业楼层", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Business/BusinessFloor/DeleteFloor", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        // 楼层平面图上传
        [HttpPost]
        public ActionResult FloorPhotoUp(int projectId, int buildingId)
        {
            HttpPostedFileBase filedata = Request.Files["Filedata"];
            if (filedata != null && filedata.ContentLength > 0)
            {
                if (filedata.ContentLength > 524288)
                {
                    return Json(new { msg = "图片大小不能超过512K，请压缩后再上传!", imagePath = "", flag = false });
                }
                else
                {
                    string filepath = "/ProjectPic/Business/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";//原图路径

                    var IsOss = ConfigurationHelper.IsOss;
                    if (IsOss)
                    {
                        string fileExtension = Path.GetExtension(filedata.FileName).ToLower(); //取得文件的扩展名,并转换成小写
                        if (!ImageHandler.CheckValidExt(fileExtension))
                        {
                            return Json(new { msg = "您上传的文件格式有误!", imagePath = "", flag = false });
                        }
                        else
                        {
                            string virpath = filepath + filedata.FileName.Substring(0, filedata.FileName.IndexOf('.') - 1) + DateTime.Now.ToString("yyyyMMddmmHHss") + fileExtension;//这是存到服务器上的虚拟路径

                            //修改OSS存储
                            var fs = filedata.InputStream;
                            StreamContent content = new StreamContent(fs);
                            var result = OssHelper.UpFileAsync(content, virpath);
                            return Json(new { msg = "图片上传成功!", imagePath = virpath, flag = true });
                        }
                    }
                    else
                    {
                        //如果不存在就创建file文件夹
                        if (!Directory.Exists(Server.MapPath(filepath)))
                        {
                            Directory.CreateDirectory(Server.MapPath(filepath));
                        }

                        string fileExtension = Path.GetExtension(filedata.FileName).ToLower(); //取得文件的扩展名,并转换成小写
                        if (!ImageHandler.CheckValidExt(fileExtension))
                        {
                            return Json(new { msg = "您上传的文件格式有误!", imagePath = "", flag = false });
                        }
                        string fileName = filepath + filedata.FileName.Substring(0, filedata.FileName.IndexOf('.') - 1) + DateTime.Now.ToString("yyyyMMddmmHHss") + fileExtension;//这是存到服务器上的虚拟路径

                        //存到服务器上的虚拟路径
                        var virpath = filepath + new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                        //转换成服务器上的物理路径
                        var mapPath = Server.MapPath(virpath);
                        filedata.SaveAs(mapPath);

                        //生成缩略图
                        var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                        var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);

                        var thumbnailPath = mapPath.Insert(mapPath.LastIndexOf("."), "_t");
                        ImageHandler.MakeThumbnail(mapPath, thumbnailPath, width, height, "H");

                        return Json(new { msg = "图片上传成功!", imagePath = virpath, flag = true });
                    }
                }
            }
            return Json(new { msg = "请选择您要上传的图片!", imagePath = "", flag = false });
        }

        ////楼层图片
        //[HttpPost]
        //public ActionResult FloorPicture(int projectId, int buildingId, int floorId)
        //{
        //    HttpPostedFileBase filedata = Request.Files["Filedata"];
        //    if (filedata != null && filedata.ContentLength > 0)
        //    {
        //        if (filedata.ContentLength > 512000)
        //        {
        //            return Json(new { msg = "图片大小不能超过500K，请压缩后再上传!", imagePath = "", flag = false });
        //        }
        //        else
        //        {
        //            string filepath = "/ProjectPic/Business/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/" + floorId + "/";//原图路径
        //            string fileExtension = Path.GetExtension(filedata.FileName).ToLower(); //取得文件的扩展名,并转换成小写
        //            if (!ImageHandler.CheckValidExt(fileExtension))
        //            {
        //                return Json(new { msg = "您上传的文件格式有误!", imagePath = "", flag = false });
        //            }
        //            else
        //            {
        //                string virpath = filepath + filedata.FileName.Substring(0, filedata.FileName.IndexOf('.') - 1) + DateTime.Now.ToString("yyyyMMddmmHHss") + fileExtension;//这是存到服务器上的虚拟路径

        //                //修改OSS存储
        //                var fs = filedata.InputStream;
        //                StreamContent content = new StreamContent(fs);
        //                var result = OssHelper.UpFileAsync(content, virpath);
        //                return Json(new { msg = "图片上传成功!", imagePath = virpath, flag = true });
        //            }
        //        }
        //    }
        //    return Json(new { msg = "请选择您要上传的图片!", imagePath = "", flag = false });
        //}

        // 导出
        public ActionResult ExportFloor(Dat_Floor_Biz floor)
        {
            #region 权限验证
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }
            #endregion
            floor.CityId = Passport.Current.CityId;
            floor.FxtCompanyId = Passport.Current.FxtCompanyId;
            var result = _floor.GetDat_Floor_BizList(floor, self);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.导出, "", "", "导出商业楼层", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_楼层数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        // 导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.商业楼层信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var taskList = result.ToPagedList(pageIndex ?? 1, 30);
            return View(taskList);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼层, SYS_Code_Dict.操作.导入, "", "", "导入商业楼层", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "BusinessFloor");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessFloor/UploadFile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        //删除导入记录
        public ActionResult DeleteBusinessFloorImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Business/BusinessFloor/DeleteBusinessFloorImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #region 楼层图片
        // 获取楼层图片
        [HttpGet]
        public ActionResult FloorPhoto(int projectId, int buildingId, int floorId, int? pageIndex)
        {
            SaveVal(projectId, buildingId, floorId);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            ViewBag.FloorId = floorId;
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            var model = new LNK_F_Photo
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                FloorId = floorId
            };
            var FPhotoList = _linkFPhoto.GetLNK_F_PhotoList(model, self).ToPagedList(pageIndex ?? 1, 30);
            return View(FPhotoList);
        }

        // 初始化 新增楼层图片 
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult AddPhoto(string Id)
        {
            ViewBag.PhotoTypeName = GetPhotoTypeName();
            ViewBag.ProjectId = int.Parse(Id.Split('#')[0]);
            ViewBag.BuildingId = int.Parse(Id.Split('#')[1]);
            ViewBag.FloorId = int.Parse(Id.Split('#')[2]);
            return View();
        }

        [HttpPost]
        public ActionResult BatchAddPictureSave()
        {
            var isSavedSuccessfully = true;
            var count = 0;

            var projectId = Request["projectId"];
            var buildingId = Request["buildingId"];
            var floorId = Request["floorId"];
            var photoTypeCode = Request["photoTypeCode"];

            if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(photoTypeCode))
            {
                return Json(new { Result = false, Count = count });
            }

            try
            {
                var virtualPath = "/ProjectPic/Business/BusinessFloor/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/" + floorId + "/";

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

                            var lnkFPhoto = new LNK_F_Photo()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                FloorId = int.Parse(floorId),
                                SaveDate = DateTime.Now,
                                SaveUser = Passport.Current.UserName,
                                Valid = 1,
                            };
                            _linkFPhoto.AddLNK_F_Photo(lnkFPhoto);
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

                            var lnkFPhoto = new LNK_F_Photo()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                FloorId = int.Parse(floorId),
                                SaveDate = DateTime.Now,
                                SaveUser = Passport.Current.UserName,
                                Valid = 1,
                            };
                            _linkFPhoto.AddLNK_F_Photo(lnkFPhoto);
                            count++;
                        }
                    }
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼层, SYS_Code_Dict.操作.新增, "", "", "新增商业楼层图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessFloor/AddPhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        // 初始化 楼层图片编辑
        [HttpGet]
        public ActionResult EdityPhoto(string chValue)
        {
            string projectId = chValue.Split('#')[0], buildingId = chValue.Split('#')[1];
            SaveVal(int.Parse(projectId), int.Parse(buildingId));
            ViewBag.photoType = GetPhotoTypeName();
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            #region 权限判断
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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
                if (int.Parse(chValue.Split('#')[3]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }
            #endregion
            var photo = _linkFPhoto.GetLNK_F_PhotoById(int.Parse(chValue.Split('#')[2]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
            return View("EdityPhoto", photo);
        }

        // 楼层图片编辑
        public ActionResult EdityPhoto(LNK_F_Photo photo)
        {
            try
            {
                var obj = new LNK_F_Photo
                {
                    CityId = Passport.Current.CityId,
                    FxtCompanyId = photo.FxtCompanyId,
                    SaveUser = Passport.Current.UserName,
                    SaveDate = DateTime.Now,
                    PhotoTypeCode = photo.PhotoTypeCode,
                    Path = photo.Path,
                    PhotoName = photo.PhotoName,
                    Id = photo.Id
                };
                int result = _linkFPhoto.UpdateLNK_F_Photo(obj, Passport.Current.FxtCompanyId);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼层, SYS_Code_Dict.操作.修改, obj.Id.ToString(), obj.PhotoName, "修改商业楼层图片", RequestHelper.GetIP());
                if (result > 0)
                    return RefreshParent();
                else
                    return this.Back("更新失败");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessFloor/EdityPhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        [HttpPost]
        public ActionResult UploadPicture(int projectId, int buildingId, int floorId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/Business/BusinessFloor/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/" + floorId + "/";

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
                LogHelper.WriteLog("Business/BusinessFloor/UploadPicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        // 删除楼层图片
        [HttpPost]
        public ActionResult DeleteFPhoto(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {
                ids.ForEach(m =>
                {
                    _linkFPhoto.DeleteLNK_F_Photo(Convert.ToInt32(m.Split('#')[1]), Passport.Current.CityId, Convert.ToInt32(m.Split('#')[0]), Passport.Current.UserName, Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId);
                });
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼层, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业楼层图片", RequestHelper.GetIP());
                return Json(new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessFloor/DeleteFPhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        // 楼层复制：floorId：原始楼层Id;buildingId：楼栋Id;floorNoTo：目标物理层;floorNumTo：目标实际层
        [HttpPost]
        public ActionResult FloorCopy(string floorId, string buildingId, string floorNoTo, string floorNumTo = "")
        {

            #region 操作权限
            //var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有修改权限！");
            }
            //if (operate == (int)PermissionLevel.All)
            //{
            //    self = false;
            //}

            //if (self)
            //{
            //    if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
            //    {
            //        return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
            //    }
            //}
            #endregion
            //原始楼层Info
            var floor_obj = _floor.GetDat_Floor_BizById(GetInt(floorId), Passport.Current.CityId, Passport.Current.FxtCompanyId);

            //原始楼层下的房号列表
            var house_list = _house.GetHouseList(GetInt(buildingId), GetInt(floorId), Passport.Current.CityId, Passport.Current.FxtCompanyId);
            try
            {
                //目标楼层Info
                var floor_obj_to = _floor.FormValidFloor(floorNoTo, buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (floor_obj_to == null)//不存在该楼层
                {
                    #region 楼层信息
                    floor_obj.FloorNo = GetInt(floorNoTo);
                    floor_obj.FloorNum = floorNoTo + "F";
                    floor_obj.CreateTime = DateTime.Now;
                    floor_obj.SaveDateTime = DateTime.Now;
                    floor_obj.Creator = floor_obj.SaveUser = Passport.Current.UserName;
                    #endregion
                    int newfloorId = _floor.AddDat_Floor_Biz(floor_obj);
                    foreach (var item in house_list)
                    {
                        item.FloorId = newfloorId;
                        _house.AddDat_House_Biz(item);
                    }
                    return Json(new { flag = true, msg = "楼层复制成功" });
                }
                else
                {     //存在该楼层
                    //目标楼层下的房号列表
                    var house_listTo = _house.GetHouseList(GetInt(buildingId), GetInt(floor_obj_to.FloorId), Passport.Current.CityId, Passport.Current.FxtCompanyId);
                    bool flag = CompareHouseName(house_list, house_listTo);
                    if (flag)
                    {
                        return Json(new { flag = false, msg = "原始楼层和目标楼层存在相同的房号名称" });
                    }
                    else
                    {
                        foreach (var house in house_list)
                        {
                            house.FloorId = floor_obj_to.FloorId;
                            _house.AddDat_House_Biz(house);
                        }
                        return Json(new { flag = true, msg = "楼层复制成功" });
                    }
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Business/BusinessFloor/FloorCopy", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { flag = false, msg = "楼层复制失败！" });
            }
        }

        // 验证物理层、实际层唯一性：floor：楼层(物理层、实际层);dataAttr：属性(物理层:FloorNo、实际层:FloorNum)
        [HttpPost]
        public ActionResult ValidFloor(string floor, string dataAttr, string buildingId)
        {
            bool isvalid = _floor.ValidFloor(floor, dataAttr, buildingId);
            return Json(isvalid);
        }

        // 获取房号数量
        public int BusinessHouseCount(int projectId, int buildingId, int floorId)
        {
            return _house.GetHouseCount(projectId, buildingId, floorId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
        }

        // form提交验证
        public ActionResult FormValidFloor(string floorNo, string floorNum, string buildingId)
        {
            var isvalid = _floor.FormValidFloor(floorNo, buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            return Json(isvalid);
        }

        [NonAction]
        private List<SelectListItem> BindDropDown(int code)
        {
            var rentSaleType = _dropDownList.GetDictById(code);
            var Result = new List<SelectListItem>();
            rentSaleType.ToList().ForEach(m =>
                Result.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            Result.Insert(0, new SelectListItem { Value = "-1", Text = "请选择" });
            return Result;
        }

        // 保存商圈和楼栋的ID
        [NonAction]
        private void SaveVal(int projectId, int buildingId, int floorId = 0)
        {
            ViewBag.projectId = projectId;
            ViewBag.buildingId = buildingId;
            ViewBag.floorId = floorId;
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

        // 图片类型
        [NonAction]
        private List<SelectListItem> GetPhotoTypeName()
        {
            var area = _dropDownList.GetDictById(SYS_Code_Dict._图片类型);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "请选择" });
            return areaResult;
        }

        // 比较房号名称是否重名
        [NonAction]
        private bool CompareHouseName(IEnumerable<Dat_House_Biz> house_list, IEnumerable<Dat_House_Biz> house_listTo)
        {
            bool flag = false;
            try
            {
                foreach (var house in house_list)
                {
                    foreach (var houseTo in house_listTo)
                    {
                        if (house.HouseName == houseTo.HouseName)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private int GetInt(object obj)
        {
            if (obj == DBNull.Value)
                return int.MinValue;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return int.MinValue;
            }
        }

    }
}
