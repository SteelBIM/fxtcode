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
    public class BusinessStoreController : BaseController
    {
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IBusinessStore _businessStore;
        private readonly ICompany _company;
        private readonly IImportTask _importTask;
        private readonly IBusinessCircle _businessCircle;
        private readonly IBusinessStreet _businessStreet;
        private readonly IDat_Building_Biz _businessBuilding;
        private readonly IDat_House_Biz _businessHouse;

        public BusinessStoreController(ILog log, IDropDownList dropDownList, IBusinessStore businessStore, ICompany company, IImportTask importTask, IBusinessCircle businessCircle, IBusinessStreet businessStreet, IDat_Building_Biz businessBuilding, IDat_House_Biz businessHouse)
        {
            this._log = log;
            this._dropDownList = dropDownList;
            this._businessStore = businessStore;
            this._company = company;
            this._importTask = importTask;
            this._businessCircle = businessCircle;
            this._businessStreet = businessStreet;
            this._businessBuilding = businessBuilding;
            this._businessHouse = businessHouse;
        }

        public ActionResult Index(Dat_Tenant_Biz tenantBiz, int? pageIndex)
        {
            var areaNameResult = GetAreaName();
            this.ViewBag.AreaName = areaNameResult;
            this.ViewBag.SubAreaBizName = SubAreaBizList(tenantBiz.AreaId);

            if (tenantBiz.AreaId <= 0) return View();

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View(new List<Dat_Tenant_Biz>().ToPagedList(1, 30));
            if (operate == (int)PermissionLevel.All) self = false;


            tenantBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
            tenantBiz.CityId = Passport.Current.CityId;

            var pageSize = 30;
            int totalCount;
            var result = _businessStore.GetTenantBiz(tenantBiz, pageIndex ?? 1, pageSize, out totalCount, self).AsQueryable();
            var projectBizResult = new PagedList<Dat_Tenant_Biz>(result, pageIndex ?? 1, pageSize, totalCount);

            return View("Index", projectBizResult);
        }

        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商铺数据, SYS_Code_Dict.页面权限.删除自己)]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }

            try
            {

                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');


                    if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                    {
                        failList.Add(array[1]);
                        continue;
                    }

                    var result = _businessStore.GetTenantBizById(int.Parse(array[1]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
                    _businessStore.DeleteTenantBiz(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商铺", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStore/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        [HttpGet]
        public ActionResult GetBizSubCode(int bizCode)
        {
            var result = GetDictBySubCode(bizCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindDropDownList(null, null, null);
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(Dat_Tenant_Biz tenantBiz)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenantBiz.HouseName))
                    tenantBiz.HouseId = _businessHouse.GetHouseId(tenantBiz.BuildingId, tenantBiz.HouseName,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId);

                tenantBiz.CityId = Passport.Current.CityId;
                tenantBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                tenantBiz.CreateTime = DateTime.Now;
                tenantBiz.Creator = Passport.Current.ID;

                _businessStore.AddTenantBiz(tenantBiz);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.新增, "", "", "新增商铺", RequestHelper.GetIP());

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStore/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

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

            var result = _businessStore.GetTenantBizById(int.Parse(splitArray[1]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
            BindDropDownList(result.BizCode, result.AreaId, result.ProjectId);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(Dat_Tenant_Biz tenantBiz)
        {
            try
            {

                //tenantBiz.CityId = Passport.Current.CityId;
                //tenantBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                tenantBiz.SaveDateTime = DateTime.Now;
                tenantBiz.SaveUser = Passport.Current.ID;

                _businessStore.UpdateTenantBiz(tenantBiz, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.修改, tenantBiz.HouseTenantId.ToString(), tenantBiz.HouseName, "修改商铺", RequestHelper.GetIP());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStreet/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //导出
        [HttpGet]
        public ActionResult Export(int? areaId, int? subAreaBizId, string projectName, string buildingName, string floorNum, string houseName, string storeName)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;

            var tenantBiz = new Dat_Tenant_Biz
            {
                FxtCompanyId = Passport.Current.FxtCompanyId,
                CityId = Passport.Current.CityId,
                AreaId = areaId ?? -1,
                SubAreaBizId = subAreaBizId ?? -1,
                ProjectName = projectName,
                BuildingName = buildingName,
                FloorNum = floorNum,
                HouseName = houseName,
                BizHouseName = storeName,
            };
            int totalCount;
            var result = _businessStore.GetTenantBiz(tenantBiz, 1, int.MaxValue, out totalCount, self).AsQueryable();

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.导出, "", "", "导出商铺", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_商铺数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //exce导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.商铺, Passport.Current.CityId, Passport.Current.FxtCompanyId);

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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.导入, "", "", "导入商铺", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "BusinessStore");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStore/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

        }

        //删除导入记录
        public ActionResult DeleteBusinessStoreImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Business/BusinessStore/DeleteBusinessStoreImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
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

        #region 商铺图片
        public ActionResult BusinessStorePicture(long projectId, long buildingId, long houseId, int houseTenantId, int? pageIndex)
        {
            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId;
            this.ViewBag.houseId = houseId;
            this.ViewBag.houseTenantId = houseTenantId;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View("Picture", new List<LNK_H_Photo>().ToPagedList(1, 30));
            if (operate == (int)PermissionLevel.All) self = false;

            var lnkPhoto = new LNK_H_Photo
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                HouseId = houseId,
                TenantId = houseTenantId,
            };

            var result = _businessStore.GetBusinessStorePhotoes(lnkPhoto, self).AsQueryable();
            var lnkPhotoResult = result.ToPagedList(pageIndex ?? 1, 30);

            return View("Picture", lnkPhotoResult);
        }

        [HttpGet]
        public ActionResult PictureCreate(long projectId, long buildingId, long houseId, int houseTenantId)
        {
            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId;
            this.ViewBag.houseId = houseId;
            this.ViewBag.houseTenantId = houseTenantId;

            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            return View();
        }

        [HttpPost]
        public ActionResult BatchAddPictureSave()
        {
            var isSavedSuccessfully = true;
            var count = 0;

            var projectId = Request["projectId"];
            var buildingId = Request["buildingId"];
            var houseId = Request["houseId"];
            var houseTenantId = Request["houseTenantId"];
            var photoTypeCode = Request["photoTypeCode"];

            if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(photoTypeCode))
            {
                return Json(new { Result = false, Count = count });
            }

            try
            {
                var virtualPath = "/ProjectPic/Business/BusinessStore/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/" + houseId + "/" + houseTenantId + "/";

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

                            var lnkPhoto = new LNK_H_Photo()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                HouseId = int.Parse(houseId),
                                TenantId = int.Parse(houseTenantId),
                                SaveDate = DateTime.Now,
                                SaveUser = Passport.Current.UserName,
                                Valid = 1,
                            };
                            _businessStore.AddBusinessStorePhoto(lnkPhoto);
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

                            var lnkPhoto = new LNK_H_Photo()
                            {
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                HouseId = int.Parse(houseId),
                                TenantId = int.Parse(houseTenantId),
                                SaveDate = DateTime.Now,
                                SaveUser = Passport.Current.UserName,
                                Valid = 1,
                            };
                            _businessStore.AddBusinessStorePhoto(lnkPhoto);
                            count++;
                        }
                    }
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.新增, "", "", "新增商铺图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStore/BatchAddPictureSave", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        public ActionResult PictureEdit(string chValue)
        {
            this.ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型);
            this.ViewBag.projectId = chValue.Split('#')[0];
            this.ViewBag.buildingId = chValue.Split('#')[1];
            this.ViewBag.houseId = chValue.Split('#')[2];
            this.ViewBag.houseTenantId = chValue.Split('#')[3];
            this.ViewBag.id = chValue.Split('#')[4];
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            this.ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            var picture = _businessStore.GetBusinessStorePhoto(int.Parse(chValue.Split('#')[4]), Passport.Current.FxtCompanyId);
            return View(picture);
        }
        [HttpPost]
        public ActionResult PictureEdit(LNK_H_Photo lnkPhoto)
        {
            try
            {
                lnkPhoto.Valid = 1;    //数据库里没有设置valid默认值为1
                lnkPhoto.SaveDate = DateTime.Now;
                lnkPhoto.SaveUser = Passport.Current.ID;

                _businessStore.UpdateBusinessStorePhoto(lnkPhoto, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.修改, "", "", "修改商铺图片", RequestHelper.GetIP());

                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStore/PictureEdit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        //图片上传
        [HttpPost]
        public ActionResult UploadPicture(long projectId, long buildingId, long houseId, long houseTenantId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/Business/BusinessStore/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/" + houseId + "/" + houseTenantId + "/";

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
                LogHelper.WriteLog("Business/BusinessStore/UploadPicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        //删除商铺图片
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
                    var result = _businessStore.GetBusinessStorePhoto(int.Parse(array[1]), int.Parse(array[0]));
                    _businessStore.DeleteBusinessStorePhoto(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商铺, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商铺图片", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessStore/DeletePicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }
        #endregion

        #region  帮助程序

        [NonAction]
        private void BindDropDownList(int? subCode, int? areaId, long? projectId)
        {
            this.ViewBag.BizName = GetDictById(SYS_Code_Dict._经营业态大类);
            this.ViewBag.RentTypeName = GetDictById(SYS_Code_Dict._租金方式);
            this.ViewBag.BizTypeName = GetDictById(SYS_Code_Dict._消费定位);
            this.ViewBag.BizSubName = GetDictBySubCode(subCode ?? -1);
            this.ViewBag.TenantName = GetCompany();
            this.ViewBag.AreaName = GetAreaName();
            this.ViewBag.SubAreaBizName = SubAreaBizList(areaId ?? -1);
            this.ViewBag.ProjectName = ProjectBizList(areaId ?? -1);
            this.ViewBag.BuildingName = BuildingBizList(projectId ?? -1);
        }

        [NonAction]
        public IEnumerable<SelectListItem> SubAreaBizList(int areaId)
        {
            var query = _businessCircle.GetSubAreaBizByAreaId(areaId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Value = "0",
                Text = "--请选择--"
            });
            query.ToList().ForEach(m => result.Add(new SelectListItem
            {
                Value = m.SubAreaId.ToString(),
                Text = m.SubAreaName
            }));
            return result;
        }

        [NonAction]
        public IEnumerable<SelectListItem> ProjectBizList(int areaId)
        {
            var query = _businessStreet.GetProjectBizs(Passport.Current.CityId, Passport.Current.FxtCompanyId, areaId);
            var result = new List<SelectListItem>();
            query.ToList().ForEach(m => result.Add(new SelectListItem
            {
                Value = m.ProjectId.ToString(),
                Text = m.ProjectName + "(" + m.AreaName + ")"
            }));
            return result;
        }

        [NonAction]
        public IEnumerable<SelectListItem> BuildingBizList(long projectId)
        {
            var query = _businessBuilding.GetBusinessBuildings(projectId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var result = new List<SelectListItem>();
            query.ToList().ForEach(m => result.Add(new SelectListItem
            {
                Value = m.BuildingId.ToString(),
                Text = m.BuildingName
            }));
            return result;
        }

        //根据行政区id获取商业圈
        [HttpGet]
        public JsonResult GetSubAreaBiz(int areaId)
        {
            return Json(SubAreaBizList(areaId), JsonRequestBehavior.AllowGet);
        }

        //根据行政区id获取商业街
        [HttpGet]
        public JsonResult GetBusinessStreet(int areaId)
        {
            return Json(ProjectBizList(areaId), JsonRequestBehavior.AllowGet);
        }
        //根据商业街ID获取商业楼栋
        [HttpGet]
        public JsonResult GetBusinessBuilding(int projectId)
        {
            return Json(BuildingBizList(projectId), JsonRequestBehavior.AllowGet);
        }

        //行政区列表
        [NonAction]
        private List<SelectListItem> GetAreaName()
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m => areaResult.Add(
                new SelectListItem { Value = m.AreaId.ToString(), Text = m.AreaName }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            //this.ViewBag.areaResult = areaResult;
            return areaResult;
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

        //获取该城市的公司
        [NonAction]
        private IEnumerable<SelectListItem> GetCompany()
        {
            var company = _company.GetCompany_like("", Passport.Current.CityId);
            var companyResult = new List<SelectListItem>();
            company.ToList().ForEach(m =>
                companyResult.Add(
                new SelectListItem { Value = m.CompanyId.ToString(), Text = m.ChineseName }
                ));
            companyResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return companyResult;
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

        [NonAction]
        private IEnumerable<SelectListItem> GetDictBySubCode(int subCode)
        {
            var casetype = _dropDownList.GetDictBySubCode(subCode);
            var casetypeResult = new List<SelectListItem>();
            casetype.ToList().ForEach(m =>
                casetypeResult.Add(
                new SelectListItem { Value = m.code.ToString(), Text = m.codename }
                ));
            casetypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return casetypeResult;
        }

        #endregion
    }
}
