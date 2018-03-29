using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.FxtDataBiz;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.Business.Controllers
{
    [Authorize]
    public class BusinessStreetController : BaseController
    {
        private readonly IBusinessStreet _businessStreet;
        private readonly IDat_Building_Biz _businessBuilding;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IBusinessCircle _businessCircle;
        private readonly IImportTask _importTask;

        public BusinessStreetController(IBusinessStreet businessStreet, ILog log, IDropDownList dropDownList, IBusinessCircle businessCircle, IDat_Building_Biz businessBuilding, IImportTask importTask)
        {
            this._businessStreet = businessStreet;
            this._businessBuilding = businessBuilding;
            this._log = log;
            this._dropDownList = dropDownList;
            this._businessCircle = businessCircle;
            this._importTask = importTask;
        }

        public ActionResult Index(Dat_Project_Biz projectBiz, int? pageIndex, bool? isExternalRequest)
        {
            BindViewData(-1);

            //if (isExternalRequest != null) Session["SubAreaId"] = projectBiz.SubAreaId;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View(new List<Dat_Project_Biz>().ToPagedList(1, 30));
            if (operate == (int)PermissionLevel.All) self = false;

            projectBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
            projectBiz.CityId = Passport.Current.CityId;

            var pageSize = 30;
            int totalCount;
            var result = _businessStreet.GetProjectBizs(projectBiz, pageIndex ?? 1, pageSize, out totalCount, self).AsQueryable();
            var projectBizResult = new PagedList<Dat_Project_Biz>(result, pageIndex ?? 1, pageSize, totalCount);

            return View("Index", projectBizResult);
        }

        //商业街对应的楼栋数
        public int BusinessBuildingCount(int projectId)
        {
            return _businessBuilding.GetBusinessBuildingCount(projectId, Passport.Current.CityId,
                Passport.Current.FxtCompanyId);
        }

        //删除商业街
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
                Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    var result = _businessStreet.GetProjectBizById(int.Parse(array[1]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
                    _businessStreet.DeleteProjectBiz(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业街", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        [NonAction]
        private void BindViewData(int areaId)
        {
            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text", areaId));
        }

        [NonAction]
        private void BindDropDownList(int areaId)
        {
            this.ViewBag.AreaName = GetAreaName();
            this.ViewBag.SubAreaName = GetSubAreaBizByAreaId(areaId);
            this.ViewBag.CorrelationTypeName = GetDictById(SYS_Code_Dict._商圈关联度);
            this.ViewBag.TrafficTypeName = GetDictById(SYS_Code_Dict._交通便捷度);
            this.ViewBag.ParkingLevelName = GetDictById(SYS_Code_Dict._交通便捷度);
        }

        [HttpPost]
        public ActionResult GetSubAreaBiz(int areaId)
        {
            var result = GetSubAreaBizByAreaId(areaId);
            return Json(result);
        }

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标
            BindDropDownList(-1);
            //return View("Edit");
            return View("Edit", new Dat_Project_Biz { });
        }

        //获取拼音
        [HttpGet]
        public JsonResult GetPinYin(string projectName)
        {
            var jianpin = StringHelper.GetPYString(projectName);
            //var quanpin = StringHelper.ConvertPinYin(projectName, 100);
            var quanpin = StringHelper.GetAllPYString(projectName);

            return Json(new { jianpin, quanpin }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult IsExistProjectBiz(int areaId, long projectId, string projectName)
        {
            return
                Json(
                    _businessStreet.ValidateProjectBiz(areaId, projectId, projectName,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId) > 0);
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(Dat_Project_Biz projectBiz)
        {
            try
            {
                projectBiz.Weight = projectBiz.Weight.HasValue ? projectBiz.Weight : 1;
                projectBiz.CityId = Passport.Current.CityId;
                projectBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                projectBiz.CreateTime = DateTime.Now;
                projectBiz.Creator = Passport.Current.ID;

                _businessStreet.AddProjectBiz(projectBiz);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.新增, "", "", "新增商业街", RequestHelper.GetIP());

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }

        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有修改权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            if (self)
            {
                if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }

            var result = _businessStreet.GetProjectBizById(int.Parse(splitArray[1]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
            BindDropDownList(result.AreaId);
            return View(result);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(Dat_Project_Biz projectBiz)
        {
            try
            {
                //projectBiz.CityId = Passport.Current.CityId;
                //projectBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                projectBiz.SaveDateTime = DateTime.Now;
                projectBiz.SaveUser = Passport.Current.ID;
                projectBiz.Weight = projectBiz.Weight.HasValue ? projectBiz.Weight : 1;
                _businessStreet.UpdateProjectBiz(projectBiz, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.修改, projectBiz.ProjectId.ToString(), projectBiz.ProjectName, "修改商业街", RequestHelper.GetIP());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        #region 图片
        //商业街道图片
        public ActionResult BusinessStreetPicture(int projectId, int? pageIndex)
        {
            SetViewBag(projectId);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View("Picture", new List<LNK_P_Photo>().ToPagedList(1, 30));
            if (operate == (int)PermissionLevel.All) self = false;

            var lnkPhoto = new LNK_P_Photo
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                ProjectId = projectId
            };

            var result = _businessStreet.GetBusinessStreetPhotoes(lnkPhoto, self).AsQueryable();
            var lnkPhotoResult = result.ToPagedList(pageIndex ?? 1, 30);

            return View("Picture", lnkPhotoResult);
        }

        //商业街道图片创建
        [HttpGet]
        public ActionResult PictureCreate(int projectId)
        {
            SetViewBag(projectId);
            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            return View();
        }

        //商业街道图片创建保存
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
                var virtualPath = "/ProjectPic/Business/BusinessProject/" + Passport.Current.CityId + "/" + projectId + "/";
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
                            var lnkPPhoto = new LNK_P_Photo()
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
                            _businessStreet.AddBusinessStreetPhoto(lnkPPhoto);
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
                            var lnkPPhoto = new LNK_P_Photo()
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
                            _businessStreet.AddBusinessStreetPhoto(lnkPPhoto);
                            count++;
                        }
                    }
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.新增, projectId, "", "批量新增商业街项目图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessProject/BatchAddPictureSave", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        //商业街道图片编辑
        public ActionResult PictureEdit(int id)
        {
            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            var picture = _businessStreet.GetBusinessStreetPhoto(id, Passport.Current.FxtCompanyId);
            this.ViewBag.projectId = picture.ProjectId;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            return View(picture);
        }

        //商业街道图片编辑保存
        [HttpPost]
        public ActionResult PictureEdit(LNK_P_Photo lnkPhoto)
        {
            try
            {
                lnkPhoto.SaveDate = DateTime.Now;
                lnkPhoto.SaveUser = Passport.Current.ID;

                _businessStreet.UpdateBusinessStreetPhoto(lnkPhoto, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.修改, lnkPhoto.Id.ToString(), lnkPhoto.PhotoName, "修改商业街图片", RequestHelper.GetIP());

                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/PictureEdit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        //图片上传
        [HttpPost]
        public ActionResult UploadPicture(int projectId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/Business/BusinessProject/" + Passport.Current.CityId + "/" + projectId + "/";
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
                    var virpath = filePath + new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHss") + DateTime.Now.Millisecond.ToString() + fileExtension;

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
                LogHelper.WriteLog("Business/BusinessStreet/UploadPicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        //删除商业街图片
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
                Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    var result = _businessStreet.GetBusinessStreetPhoto(int.Parse(array[1]), Passport.Current.FxtCompanyId);
                    _businessStreet.DeleteBusinessStreetPhoto(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业街图片", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/DeletePicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion
        //导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.商业街信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.导入, "", "", "导入商业街", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "BusinessStreet");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
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

        //删除商业街导入记录
        public ActionResult DeleteBusinessStreetImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Business/BusinessStreet/DeleteBusinessStreetImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #region 导出
        [HttpGet]
        public ActionResult Export(int? areaId, string projectName)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            var projectBiz = new Dat_Project_Biz
            {
                FxtCompanyId = Passport.Current.FxtCompanyId,
                CityId = Passport.Current.CityId,
                ProjectName = projectName
            };

            int totalCount;
            var result = _businessStreet.GetProjectBizs(projectBiz, 1, int.MaxValue, out totalCount, self);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.导出, "", "", "导出商业街", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_商业街_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //商业街自定义导出
        [HttpGet]
        public ActionResult SelfDefineExport()
        {
            BindViewData(-1);
            return View();
        }

        [HttpPost]
        public ActionResult SelfDefineExport(Dat_Project_Biz projectBiz, List<string> project)
        {
            try
            {
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

                projectBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                projectBiz.CityId = Passport.Current.CityId;
                var dt = _businessStreet.ProjectSelfDefineExport(projectBiz, project, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
                if (dt != null && dt.Rows.Count > 0)
                {
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_自定义导出商业街_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";

                    MemoryStream ms = ExcelHandle.RenderToExcel(dt);
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业街, SYS_Code_Dict.操作.导出, "", "", "自定义导出商业街", RequestHelper.GetIP());

                    return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                }
                return this.Back("没有您要导出的数据");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/SelfDefineExport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }
        #endregion

        #region 帮助程序

        //保存projectId 到 viewbag
        private void SetViewBag(int projectId)
        {
            this.ViewBag.projectId = projectId;
        }

        //根据行政区ID 获取商圈
        private IEnumerable<SelectListItem> GetSubAreaBizByAreaId(int areaId)
        {
            var list = new List<SelectListItem>();
            var subAreaBizs = _businessCircle.GetSubAreaBizByAreaId(areaId, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            subAreaBizs.ToList().ForEach(m => list.Add(
                new SelectListItem
                {
                    Value = m.SubAreaId.ToString(),
                    Text = m.SubAreaName

                }
                ));
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
            //this.ViewBag.areaResult = areaResult;
            return areaResult;
        }

        #endregion

    }
}
