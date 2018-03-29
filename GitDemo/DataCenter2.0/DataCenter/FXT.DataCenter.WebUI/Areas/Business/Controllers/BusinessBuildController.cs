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
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.Business.Controllers
{
    [Authorize]
    public class BusinessBuildController : BaseController
    {
        private readonly IBusinessStreet _project;
        private readonly IDat_Building_Biz _build;
        private readonly IDat_Floor_Biz _floor;
        private readonly IDat_House_Biz _house;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IBusinessBuildingPhoto _linkBPhoto;
        private readonly IImportTask _importTask;

        public BusinessBuildController(ILog log, IBusinessStreet project, IDat_Building_Biz build, IDat_Floor_Biz floor, IDat_House_Biz house, IDropDownList dropDownList, IBusinessBuildingPhoto linkBPhoto, IImportTask importTask)
        {
            this._project = project;
            this._log = log;
            this._build = build;
            this._floor = floor;
            this._house = house;
            this._dropDownList = dropDownList;
            this._linkBPhoto = linkBPhoto;
            this._importTask = importTask;
        }

        #region 商业楼栋
        public ActionResult Index(Dat_Building_Biz build, int? pageIndex)
        {
            BindViewData();
            build.ProjectId = build.ProjectId;
            build.CityId = Passport.Current.CityId;
            build.FxtCompanyId = Passport.Current.FxtCompanyId;
            ViewBag.projectId = build.ProjectId;
            ViewBag.SubAreaId = build.SubAreaId;
            #region 权限验证
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion
            var result = _build.GetDat_Building_BizList(build, self).ToPagedList(pageIndex ?? 1, 30);
            return View(result);
        }

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create(int projectId)
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            ViewBag.projectId = projectId;
            BindViewData();
            return View("EditBuild", new Dat_Building_Biz());
        }
        [HttpPost]
        public ActionResult Create(Dat_Building_Biz build)
        {
            try
            {
                var buildlist = _build.GetBusinessBuildings((long)build.ProjectId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                var isexistbuildlist = (
                     from c in buildlist
                     where c.BuildingName == build.BuildingName
                     select c).ToList();
                if (isexistbuildlist != null && isexistbuildlist.Count > 0)
                {
                    return this.Back("该楼栋已存在！");
                }

                build.CityId = Passport.Current.CityId;
                build.FxtCompanyId = Passport.Current.FxtCompanyId;
                build.Weight = build.Weight.HasValue? build.Weight : 1; 
                int result = _build.AddDat_Building_Biz(build);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.新增, "", "", "商业楼栋", RequestHelper.GetIP());
                return RedirectToAction("Index", new { ProjectId = build.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        // 编辑
        [HttpGet]
        public ActionResult EditBuild(string id)
        {
            var splitArray = id.Split('#');

            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

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
                if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }
            #endregion
            int buildingId = int.Parse(splitArray[1]), cityId = Passport.Current.CityId, fxtCompanyId = Passport.Current.FxtCompanyId;
            var result = _build.GetDat_Building_BizById(buildingId, cityId, fxtCompanyId);
            BindViewData(result.AreaId);
            ViewBag.projectId = result.ProjectId;
            return View(result);
        }

        [HttpPost]
        public ActionResult EditBuild(Dat_Building_Biz build)
        {
            try
            {
                var builddetal = _build.GetDat_Building_BizById((int)build.BuildingId,Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (builddetal.BuildingName != build.BuildingName) //编辑时楼栋名字有改动
                {
                    var buildlist = _build.GetBusinessBuildings((long)build.ProjectId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                    var isexistbuildlist = (
                         from c in buildlist
                         where c.BuildingName == build.BuildingName
                         select c).ToList();
                    if (isexistbuildlist != null && isexistbuildlist.Count > 0)
                    {
                        return this.Back("该楼栋已存在！");
                    }
                }                

                build.CityId = Passport.Current.CityId;
                //build.FxtCompanyId = Passport.Current.FxtCompanyId;
                build.SaveUser = Passport.Current.UserName;
                build.SaveDateTime = DateTime.Now;
                build.Weight = build.Weight.HasValue ? build.Weight : 1;
                int result = _build.UpdateDat_Building_Biz(build, Passport.Current.FxtCompanyId);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.修改, build.BuildingId.ToString(), build.BuildingName, "修改商业楼栋", RequestHelper.GetIP());
                return RedirectToAction("Index", new { ProjectId = build.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/EditBuild", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        // 删除
        [HttpPost]
        public ActionResult DeleteBuild(List<string> ids)
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
                    _build.DeleteDat_Building_Biz(int.Parse(array[1]), Passport.Current.UserName, Passport.Current.CityId, int.Parse(array[0]), Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业楼栋", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/DeleteBuild", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        // 导出
        public ActionResult ExportBuild(Dat_Building_Biz build)
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
            build.CityId = Passport.Current.CityId;
            build.FxtCompanyId = Passport.Current.FxtCompanyId;
            var result = _build.GetDat_Building_BizList(build, self);

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.导出, "", "", "导出商业楼栋", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_楼栋数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        // 导入
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.商业楼栋信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.导入, "", "", "导入商业楼栋", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "BusinessBuilding");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/UploadFile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        //删除导入记录
        public ActionResult DeleteBusinessBuildImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Business/BusinessBuild/DeleteBusinessBuildImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        // 商业楼栋复制：projectId：商业街ID；buildId：商业楼栋ID；buildNameTo：目标楼栋名称
        [HttpPost]
        public ActionResult BuildCopy(int projectId, int buildId, string buildNameTo)
        {
            string msg, userName = Passport.Current.UserName;
            int cityId = Passport.Current.CityId, fxtCompanyId = Passport.Current.FxtCompanyId;
            try
            {
                //原始楼栋下的楼层列表
                var floorList = _floor.GetDat_Floor_BizByBuildId(buildId, Passport.Current.CityId, Passport.Current.FxtCompanyId);

                var flag = _build.CheckBuild(projectId, buildNameTo, cityId, fxtCompanyId);
                if (flag == null)//不存在该楼栋
                {
                    var build = _build.GetDat_Building_BizById(buildId, cityId, fxtCompanyId);
                    build.BuildingName = buildNameTo;
                    int newbuildId = _build.AddDat_Building_Biz(build);
                    CopyFloorHouse(floorList, newbuildId, out msg);

                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.新增, buildId.ToString(), buildNameTo, "商业楼栋复制", RequestHelper.GetIP());
                    return Json(msg);
                }
                else//存在该楼栋
                {
                    //目标楼栋下的楼层列表
                    var floorLostTo = _floor.GetDat_Floor_BizByBuildId(Convert.ToInt32(flag.BuildingId), Passport.Current.CityId, Passport.Current.FxtCompanyId);
                    if (CheckedFloor(floorList, floorLostTo, out msg))
                    {
                        return Json(msg);
                    }
                    else
                    {
                        CopyFloorHouse(floorList, Convert.ToInt32(flag.BuildingId), out msg);
                        _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.修改, buildId.ToString(), buildNameTo, "商业楼栋复制", RequestHelper.GetIP());
                        return Json(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/BuildCopy", RequestHelper.GetIP(), Passport.Current.ID,
                  Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        // 检查楼层的物理层和实际层是否存在：floorList：原始楼栋下的楼层列表；floorLostTo：目标楼栋下的楼层列表
        private bool CheckedFloor(IEnumerable<Dat_Floor_Biz> floorList, IEnumerable<Dat_Floor_Biz> floorLostTo, out string msg)
        {
            bool flag = false;
            msg = "";
            foreach (var list in floorList)
            {
                foreach (var listTo in floorLostTo)
                {
                    //物理层唯一
                    if (list.FloorNo == listTo.FloorNo)
                    {
                        flag = true;
                        msg = "已经存在相同的物理层";
                        break;
                    }
                    //实际层唯一
                    if (list.FloorNum == listTo.FloorNum)
                    {
                        flag = true;
                        msg = "已经存在相同的实际层";
                        break;
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
            return flag;
        }

        // 复制楼层和房号
        private void CopyFloorHouse(IEnumerable<Dat_Floor_Biz> floorList, int newbuildId, out string msg)
        {
            try
            {
                if (newbuildId <= 0)
                {
                    msg = "楼栋复制失败";
                }
                else
                {
                    foreach (var floor in floorList)
                    {
                        floor.BuildingId = newbuildId;
                        int newfloorId = _floor.AddDat_Floor_Biz(floor);
                        var houseList = _house.GetDat_House_BizByFloorId(floor.FloorId, floor.CityId, floor.FxtCompanyId);
                        foreach (var house in houseList)
                        {
                            house.FloorId = newfloorId;
                            house.BuildingId = newbuildId;
                            _house.AddDat_House_Biz(house);
                        }
                    }
                    msg = "";
                }
            }
            catch
            {

                msg = "网络异常";
            }
        }
        #endregion
        #region 商业楼栋图片

        public ActionResult BuildPhoto(int projectId, int buildingId, int? pageIndex)
        {
            SaveVal(projectId, buildingId);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            var model = new LNK_B_Photo
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                BuildingId = buildingId
            };
            var photolist = _linkBPhoto.GetLNK_B_PhotoList(model, self).ToPagedList(pageIndex ?? 1, 30);
            return View(photolist);
        }

        //新建图片
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult CreatePhoto(string Id)
        {
            string projectId = Id.Split('#')[0], buildingId = Id.Split('#')[1];
            SaveVal(int.Parse(projectId), int.Parse(buildingId));
            ViewBag.PhotoTypeName = GetPhotoTypeName();
            return View();
        }

        [HttpPost]
        public ActionResult CreatePhoto()
        {
            var isSavedSuccessfully = true;
            var count = 0;

            var projectId = Request["projectId"];
            var buildingId = Request["buildingId"];
            var photoTypeCode = Request["photoTypeCode"];

            if (string.IsNullOrEmpty(buildingId) || string.IsNullOrEmpty(photoTypeCode))
            {
                return Json(new { Result = false, Count = count });
            }

            try
            {
                var virtualPath = "/ProjectPic/Business/BusinessBuilding/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";

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
                            var bp = new LNK_B_Photo()
                            {
                                BuildingId = int.Parse(buildingId),
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                SaveDate = DateTime.Now,
                                SaveUser = Passport.Current.UserName,
                                Valid = 1,
                            };
                            _linkBPhoto.AddLNK_B_Photo(bp);
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
                            var bp = new LNK_B_Photo()
                            {
                                BuildingId = int.Parse(buildingId),
                                CityId = Passport.Current.CityId,
                                FxtCompanyId = Passport.Current.FxtCompanyId,
                                Path = virtualFilePath,
                                PhotoDate = DateTime.Now,
                                PhotoName = fileName,
                                PhotoTypeCode = int.Parse(photoTypeCode),
                                SaveDate = DateTime.Now,
                                SaveUser = Passport.Current.UserName,
                                Valid = 1,
                            };
                            _linkBPhoto.AddLNK_B_Photo(bp);
                            count++;
                        }
                    }
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.新增, "", "", "新增商业楼栋图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/PictureCreate", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        //编辑楼栋图片
        [HttpGet]
        public ActionResult EdityPhoto(string chValue)
        {
            string projectId = chValue.Split('#')[0], buildingId = chValue.Split('#')[1];
            SaveVal(int.Parse(projectId), int.Parse(buildingId));
            ViewBag.photoType = GetPhotoTypeName();
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
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
            var photo = _linkBPhoto.GetLNK_B_PhotoById(int.Parse(chValue.Split('#')[2]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
            return View("EdityPhoto", photo);
        }

        [HttpPost]
        public ActionResult EdityPhoto(LNK_B_Photo photo)
        {
            try
            {
                var obj = new LNK_B_Photo
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
                int result = _linkBPhoto.UpdateLNK_B_Photo(obj, Passport.Current.FxtCompanyId);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.修改, "", "", "修改商业楼栋图片", RequestHelper.GetIP());
                if (result > 0)
                    return RefreshParent();
                else
                    return this.Back("更新失败");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/EdityPhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
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
                var filePath = "/ProjectPic/Business/BusinessBuilding/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";

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
                LogHelper.WriteLog("Business/BusinessBuild/UploadPicture", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        //删除楼栋图片
        [HttpPost]
        public ActionResult DeleteBPhoto(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                //return this.Back("请选择要删除的数据！");
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {
                ids.ForEach(m =>
                {
                    _linkBPhoto.DeleteLNK_B_Photo(Convert.ToInt32(m.Split('#')[1]), Passport.Current.CityId, Convert.ToInt32(m.Split('#')[0]), Passport.Current.UserName, Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId);
                });
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业楼栋图片", RequestHelper.GetIP());
                return Json(new { result = true, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessBuild/DeleteBPhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        // 获取楼层数量
        public int GetFloorCount(int projectId, int buildingId)
        {
            return _floor.GetFloorCount(projectId, buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
        }
        #endregion

        #region 辅助方法

        [HttpPost]
        public ActionResult GetSubAreaList(int areaId)
        {
            var result = _build.GetSubAreaBizByAreaId(areaId);
            if (result != null && result.Count() > 0)
                return Json(new { flag = true, data = result });
            else
                return Json(new { flag = false });

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

        [NonAction]
        private void BindViewData(int areaId = -1)
        {
            ViewBag.AreaList = GetAreaName();
            ViewBag.CorrelationTypeName = BindDropDown(SYS_Code_Dict._商圈关联度);
            ViewBag.StructureName = BindDropDown(SYS_Code_Dict._建筑结构);
            ViewBag.BuildingTypeName = BindDropDown(SYS_Code_Dict._建筑类型);
            ViewBag.RentSaleTypeName = BindDropDown(SYS_Code_Dict._经营方式);
            ViewBag.LiftFitmentName = BindDropDown(SYS_Code_Dict._客厅装修);
            ViewBag.PublicFitmentName = BindDropDown(SYS_Code_Dict._客厅装修);
            ViewBag.WallFitmentName = BindDropDown(SYS_Code_Dict._外墙装修);
            ViewBag.TrafficTypeName = BindDropDown(SYS_Code_Dict._交通便捷度);
            ViewBag.ParkingLevelName = BindDropDown(SYS_Code_Dict._交通便捷度);
            ViewBag.AirConditionTypeName = BindDropDown(SYS_Code_Dict._空调系统类型);
            ViewBag.BizCutOffeName = BindDropDown(SYS_Code_Dict._商业阻隔);
            ViewBag.BuildingBizTypeName = BindDropDown(SYS_Code_Dict._楼栋商业类型);
            ViewBag.BizTypeName = BindDropDown(SYS_Code_Dict._商业细分类型);
            ViewBag.ProRoadName = BindDropDown(SYS_Code_Dict._临路类型);
            ViewBag.FlowsName = BindDropDown(SYS_Code_Dict._人流量);
            ViewBag.CustomerTypeName = BindDropDown(SYS_Code_Dict._消费定位);
            //ViewData.Add("PhotoTypeCode", new SelectList(GetPhotoTypeName(), "Value", "Text", photoTypeCode));
        }

        [NonAction]
        private List<SelectListItem> GetAreaName()
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

        [NonAction]
        private List<SelectListItem> GetPhotoTypeName()
        {
            var area = _dropDownList.GetDictById(SYS_Code_Dict._图片类型);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return areaResult;
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
            Result.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return Result;
        }

        [NonAction]
        private void SaveVal(int projectId, int buildingId)
        {
            ViewBag.projectId = projectId;
            ViewBag.buildingId = buildingId;
        }
        #endregion

        //商业楼栋自定义导出
        [HttpGet]
        public ActionResult SelfDefineExport()
        {
            BindViewData();
            return View();
        }

        [HttpPost]
        public ActionResult SelfDefineExport(Dat_Building_Biz buildingBiz, List<string> building)
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

                buildingBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                buildingBiz.CityId = Passport.Current.CityId;
                var dt = _build.BuildingSelfDefineExport(buildingBiz, building, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
                if (dt != null && dt.Rows.Count > 0)
                {
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.导出, "", "", "自定义导出商业楼栋", RequestHelper.GetIP());

                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "商业_自定义导出楼栋_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
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
                LogHelper.WriteLog("Business/BusinessBuild/SelfDefineExport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }
    }
}
