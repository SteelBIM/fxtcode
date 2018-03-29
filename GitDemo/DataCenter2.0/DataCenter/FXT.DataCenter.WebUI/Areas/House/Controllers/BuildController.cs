using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.QueryObjects;
using FXT.DataCenter.Domain.Services;
using System.IO;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;
using System.Configuration;
using FXT.DataCenter.Domain.Models.FxtProject;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class BuildController : BaseController
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;
        private readonly IDropDownList _dropDownList;
        private readonly IDAT_Project _project;
        private readonly IDAT_Building _build;
        private readonly IDAT_House _house;
        private readonly ILog _log;
        private readonly IBuildingWeightRevised _buildingWeight;
        private readonly IShare _share;
        public BuildController(IDropDownList drop, IDAT_Project proj, IDAT_Building build, IDAT_House house, ILog log, IBuildingWeightRevised buildingWeight, IShare share)
        {
            this._dropDownList = drop;
            this._project = proj;
            this._build = build;
            this._house = house;
            this._log = log;
            this._buildingWeight = buildingWeight;
            this._share = share;
        }

        public ActionResult Index(BuildStatiParam parame)
        {
            var cp = FxtUserCenterService_GetFPInfo(Passport.Current.FxtCompanyId, Passport.Current.CityId, Passport.Current.ProductTypeCode, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }

            ViewBag.ProjectId = parame.ProjectId;
            //判断是否有导出权限
            //var ret = _share.IsExport(Passport.Current.CityId, Passport.Current.FxtCompanyId);
            ViewBag.IsExport = (cp.IsExportHose == 1);
            //判断是否有更新VQ房号系数权限
            bool isvqret = false; int parentProductTypeCode = 0;

            //int parentShowDataCompanyId = 0, parentProductTypeCode = 0;
            //_share.GetVQDataParent(Passport.Current.CityId, Convert.ToInt32(ConfigurationHelper.FxtCompanyId), out parentShowDataCompanyId, out parentProductTypeCode);

            var cpvq = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cpvq != null && Passport.Current.FxtCompanyId == cpvq.ParentShowDataCompanyId && Passport.Current.CityId != 147)
            {
                isvqret = true;
                parentProductTypeCode = cpvq.ParentProductTypeCode;
            }
            ViewBag.isvqret = isvqret;
            ViewBag.parentProductTypeCode = parentProductTypeCode;

            //判断操作权限是查看自己还是查看全部
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View(new List<BuildStatiParam>().ToPagedList(parame.pageindex ?? 0, 30));
            if (operate == (int)PermissionLevel.All) self = false;

            var result = _build.GetBuildingInfo(Passport.Current.CityId, parame, Passport.Current.FxtCompanyId);
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                }
                else
                {
                    result = result.Where(t => t.Creator == Passport.Current.UserName).AsQueryable();
                }
            }

            var r = OrderByBuildingName(result.ToList());
            var builList = r.ToPagedList(parame.pageindex ?? 0, 30);

            ViewBag.projectPara = Passport.Current.FxtCompanyId + "#" + parame.ProjectId;
            ViewBag.projectName = _build.GetProjectNameById(parame.ProjectId.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId).projectname;
            ViewBag.areaID = _build.GetProjectNameById(parame.ProjectId.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId).areaid;
            ViewBag.areaName = _build.GetProjectNameById(parame.ProjectId.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId).AreaName;

            return View(builList);
        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create(string projectId)
        {
            var splitArray = projectId.Split('#');

            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            ViewBag.projectPara = projectId;
            ViewBag.ProjectId = int.Parse(splitArray[1]);

            var project = _build.GetProjectNameById(splitArray[1], Passport.Current.CityId, Passport.Current.FxtCompanyId);
            ViewBag.areaid = project == null ? -1 : project.areaid;
            ViewBag.areaname = project == null ? "" : project.AreaName;
            ViewBag.projectName = project == null ? "" : project.projectname;
            ViewBag.bAvgPrice = project == null ? 0 : project.averageprice;

            var result = new DAT_Building()
            {
                projectid = project.projectid,
                cityid = project.cityid,
                ProjectName = project.projectname,
                ProAveragePrice = project.averageprice,
            };
            return View("Edity", result);
        }

        //新增状态时保存
        [HttpPost]
        public ActionResult Create(DAT_Building dat)
        {
            try
            {
                dat.cityid = Passport.Current.CityId;
                dat.creator = Passport.Current.ID;
                dat.fxtcompanyid = Passport.Current.FxtCompanyId;

                var buildlist = _build.GetBuildingInfo(Passport.Current.CityId, dat.projectid, Passport.Current.FxtCompanyId);
                var isexistbuildlist = (
                     from c in buildlist
                     where c.buildingname == dat.buildingname
                     select c).ToList();
                if (isexistbuildlist != null && isexistbuildlist.Count > 0)
                {
                    return this.Back("该楼栋已存在！");
                }

                var result = _build.AddBuild(dat);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.新增, result.ToString(), dat.buildingname, "新增楼栋", RequestHelper.GetIP());

                return this.Redirect(dat.projectid.ToString(), result > 0 ? "新增楼栋成功" : (result == -2 ? "已有重名楼栋" : "新增楼栋失败"), "index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/Edity", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        [HttpGet]
        public ActionResult GetBuildingNameList(int projectId)
        {
            var data = _build.GetBuildNameList(Passport.Current.CityId, projectId, Passport.Current.FxtCompanyId).ToList();
            return Json(data.Select(m => new { name = m.buildingname, id = m.buildingname }), JsonRequestBehavior.AllowGet);
        }

        //编辑
        [HttpGet]
        public ActionResult Edity(string projectId, int? buildingid, decimal? pavgPrice)
        {
            var splitArray = projectId.Split('#');

            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标
            ViewBag.projectPara = projectId;
            var projectid = int.Parse(splitArray[1]);
            ViewBag.ProjectId = projectid;
            //ViewBag.BuildNameList = _build.GetBuildNameList(Passport.Current.CityId, int.Parse(splitArray[1]), Passport.Current.FxtCompanyId).ToList();
            var result = _build.GetBuildingInfo(Passport.Current.CityId, projectid, Passport.Current.FxtCompanyId, buildingid ?? 0).FirstOrDefault();

            var project = _build.GetProjectNameById(projectid.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var projectName = project == null ? "" : project.projectname;
            var averagePrice = project == null ? 0 : project.averageprice;

            ViewBag.areaid = project == null ? -1 : project.areaid;
            ViewBag.areaname = project == null ? "" : project.AreaName;
            ViewBag.projectName = project == null ? "" : project.projectname;
            ViewBag.buildName = result == null ? "" : result.buildingname;

            result.ProjectName = projectName;
            result.ProAveragePrice = averagePrice;

            return View(result);
        }

        [HttpPost]
        public ActionResult Edity(DAT_Building dat)
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
            var b = _build.GetBuildingInfo(Passport.Current.CityId, dat.projectid, Passport.Current.FxtCompanyId, dat.buildingid).FirstOrDefault();
            if (self)//self && b.creator != Passport.Current.UserName
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && b.fxtcompanyid != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    return base.Back("对不起，该条数据您没有修改权限！");
                }
                if (Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && b.creator != Passport.Current.UserName)
                {
                    return base.Back("对不起，该条数据您没有修改权限！");
                }                
            }

            try
            {
                var builddetal = _build.GetBuildingInfo(Passport.Current.CityId, dat.projectid, Passport.Current.FxtCompanyId, dat.buildingid).FirstOrDefault();
                if (builddetal.buildingname != dat.buildingname) //编辑时楼栋名字有改动
                {
                    var buildlist = _build.GetBuildingInfo(Passport.Current.CityId, dat.projectid, Passport.Current.FxtCompanyId);
                    var isexistbuildlist = (
                         from c in buildlist
                         where c.buildingname == dat.buildingname
                         select c).ToList();
                    if (isexistbuildlist != null && isexistbuildlist.Count > 0)
                    {
                        return this.Back("该楼栋已存在！");
                    }
                }

                dat.cityid = Passport.Current.CityId;
                dat.saveuser = Passport.Current.ID;

                var result = _build.ModifyBuilding(dat, Passport.Current.FxtCompanyId);
                _log.InsertLog(Passport.Current.CityId, dat.fxtcompanyid, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.修改, dat.buildingid.ToString(), dat.buildingname, "修改楼栋", RequestHelper.GetIP());

                return result > 0 ? this.Redirect(dat.projectid.ToString(), "修改楼栋成功", "index") : this.Back("修改楼栋失败");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/Edity", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //删除
        public ActionResult DeleteBuild(List<string> ids, int projectId)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }
            var cp = FxtUserCenterService_GetFPInfo(Passport.Current.FxtCompanyId, Passport.Current.CityId, Passport.Current.ProductTypeCode, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return Json(new { result = false, msg = "删除失败！" });
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
                    int buildingid = int.Parse(array[0]);
                    int fxtcompanyid = int.Parse(array[2]);
                    var b = _build.GetBuildingInfo(Passport.Current.CityId, projectId, Passport.Current.FxtCompanyId, buildingid).FirstOrDefault();
                    if (self)//self && b.creator != Passport.Current.UserName
                    {
                        if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && b.fxtcompanyid != Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                        {
                            failList.Add(array[0]);
                            continue;
                        }
                        if (Passport.Current.FxtCompanyId != Convert.ToInt32(ConfigurationHelper.FxtCompanyId) && b.creator != Passport.Current.UserName)
                        {
                            failList.Add(array[0]);
                            continue;
                        }
                    }
                    //删除楼栋
                    _build.DeleteBuilding(buildingid, Passport.Current.CityId, fxtcompanyid, Passport.Current.ID, Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId, (int)cp.IsDeleteTrue);
                }

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除楼栋", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/DeleteBuild", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //获取楼栋图片
        [HttpGet]
        public ActionResult BuildPhoto(BaseParams para, int projectid, int buildId)
        {
            ViewBag.projectName = _build.GetProjectNameById(projectid.ToString(), Passport.Current.CityId, Passport.Current.FxtCompanyId).projectname;
            var building = _build.GetBuildingInfo(Passport.Current.CityId, projectid, Passport.Current.FxtCompanyId, buildId).FirstOrDefault();
            ViewBag.buildName = building == null ? "" : building.buildingname;

            ViewBag.projectPara = Passport.Current.FxtCompanyId + "#" + projectid;
            ViewBag.projectId = projectid;
            ViewBag.buildId = buildId;
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;

            var photo = _project.GetBuildingPhotoList(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectid, buildId, 0).OrderByDescending(m => m.id);
            var photoResult = photo.ToPagedList(para.pageIndex, 10);
            return View(photoResult);
        }

        //批量新增
        [HttpGet]
        public ActionResult BatchAddPicture(int projectId, int buildingId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.BuildingId = buildingId;
            ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型_住宅);

            return View();
        }

        //批量新增保存
        [HttpPost]
        public ActionResult BatchAddPictureSave()
        {
            var isSavedSuccessfully = true;
            var count = 0;

            var projectId = Request["projectId"];
            var buildingId = Request["buildingId"];
            var photoTypeCode = Request["photoTypeCode"];

            if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(photoTypeCode))
            {
                return Json(new { Result = false, Count = count });
            }
            try
            {
                var virtualPath = "/ProjectPic/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";

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
                            _project.AddProjectPhoto(Passport.Current.CityId, Passport.Current.FxtCompanyId, int.Parse(projectId), int.Parse(photoTypeCode), virtualFilePath, fileName, int.Parse(buildingId));
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
                            _project.AddProjectPhoto(Passport.Current.CityId, Passport.Current.FxtCompanyId, int.Parse(projectId), int.Parse(photoTypeCode), virtualFilePath, fileName, int.Parse(buildingId));
                            count++;
                        }
                    }
                }
                //var virtualPath = "/ProjectPic/" + Passport.Current.CityId + "/" + projectId + "/" + buildingId + "/";
                ////var directoryPath = Server.MapPath(virtualPath);

                ////if (!Directory.Exists(directoryPath))
                ////    Directory.CreateDirectory(directoryPath);

                ////var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                ////var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);

                //foreach (string f in Request.Files)
                //{
                //    HttpPostedFileBase file = Request.Files[f];

                //    if (file != null && file.ContentLength > 0)
                //    {
                //        var fileExtension = Path.GetExtension(file.FileName);//扩展名 如：.jpg
                //        var fileName = file.FileName.Substring(0, file.FileName.IndexOf('.'));//扩展名前的名称
                //        var fileNewName = new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

                //        var virtualFilePath = Path.Combine(virtualPath, fileNewName);//服务器虚拟路径
                //        //var physicalFilePath = Server.MapPath(virtualFilePath);//服务器物理路径
                //        ////保存图片
                //        //file.SaveAs(physicalFilePath);
                //        ////保存缩略图
                //        //var thumbnailPath = physicalFilePath.Insert(physicalFilePath.LastIndexOf(".", StringComparison.Ordinal), "_t");
                //        //ImageHandler.MakeThumbnail(physicalFilePath, thumbnailPath, width, height, "H");

                //        //修改OSS存储
                //        var fs = file.InputStream;
                //        StreamContent content = new StreamContent(fs);
                //        var result = OssHelper.UpFileAsync(content, virtualFilePath);
                //        //保存该条图片信息到数据库
                //        _project.AddProjectPhoto(Passport.Current.CityId, Passport.Current.FxtCompanyId, int.Parse(projectId), int.Parse(photoTypeCode), virtualFilePath, fileName, int.Parse(buildingId));
                //        count++;
                //    }
                //}
                //日志 
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.新增, buildingId.ToString(), "", "新增楼栋项目图片", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/BatchAddPictureSave", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                isSavedSuccessfully = false;
            }
            return Json(new { Result = isSavedSuccessfully, Count = count });
        }

        //编辑楼栋项目图片
        [HttpGet]
        public ActionResult EdityBuildPhoto(int projectid, int buildId, int photoId)
        {
            ViewBag.PhotoTypeName = GetDictById(SYS_Code_Dict._图片类型_住宅);
            ViewBag.IsOss = ConfigurationHelper.IsOss;
            ViewBag.OssImgServer = ConfigurationHelper.OssImgServer;
            var photoList = _project.GetBuildingPhotoList(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectid, buildId, photoId).FirstOrDefault();
            return View(photoList);
        }

        [HttpPost]
        public ActionResult EdityBuildPhoto(LNK_P_Photo photo)
        {
            try
            {
                _project.UpdataProjectPhoto(photo.id, photo.phototypecode ?? 0, photo.path, photo.photoname, Passport.Current.CityId, photo.fxtcompanyid, Passport.Current.FxtCompanyId);
                // 操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.修改, photo.id.ToString(), photo.photoname, "修改楼栋图片", RequestHelper.GetIP());
                return this.RefreshParent("修改楼栋图片成功");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/EdityBuildPhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //楼栋图片上传
        public ActionResult BuildPicUp(int projectid, int buildId)
        {
            var upImg = Request.Files["Filedata"];
            if (upImg == null) return Json(new { result = false, msg = "上传的图片失败" });
            if (upImg.ContentLength > 524288) return Json(new { result = false, msg = "上传的图片大小不能超过512K" });

            try
            {
                var filePath = "/ProjectPic/" + Passport.Current.CityId + "/" + projectid + "/" + buildId + "/";//原图路径

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
                //var fileExtension = Path.GetExtension(upImg.FileName);
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
                LogHelper.WriteLog("House/Build/BuildPicUp", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "上传的图片失败" });
            }
        }

        //删除楼盘项目图片
        public ActionResult DeletePhoto(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "请选择要删除数据！" });
            }
            try
            {
                ids.ForEach(photoId => _project.DeleteProjectPhoto(int.Parse(photoId.Split('#')[0]), Passport.Current.CityId, int.Parse(photoId.Split('#')[3]), Passport.Current.FxtCompanyId));
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除楼栋图片", RequestHelper.GetIP());

                return Json(new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/DeletePhoto", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "操作失败！" });
            }
        }

        //楼栋复制
        [HttpPost]
        public ActionResult BuildCopy(int projectId, int buildId, string buildName, string buildNameTo)
        {
            string msg = "";
            try
            {
                if (buildName.Trim() == buildNameTo.Trim())
                {
                    msg = "目标楼栋名称和原始楼栋名称一致";
                    return Json(msg);
                }
                msg = _build.BuildCopy(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectId, buildId, buildName, buildNameTo, Passport.Current.ID);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.新增, buildId.ToString(), buildName + "复制到" + buildNameTo, "楼栋复制", RequestHelper.GetIP());
                return Json(msg);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/BuildCopy", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //导出
        public ActionResult ExportBuild(BuildStatiParam parame)
        {
            //判断操作权限是查看自己还是查看全部
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

            ViewBag.ProjectId = parame.ProjectId;
            var result = _build.GetBuildingInfo(Passport.Current.CityId, parame, Passport.Current.FxtCompanyId);
            if (self)
            {
                if (Passport.Current.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                {
                    result = result.Where(t => t.FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId)).AsQueryable();
                }
                else
                {
                    result = result.Where(t => t.Creator == Passport.Current.UserName).AsQueryable();
                }
            }

            if (result.Count() > 0)
            {
                #region header 信息
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                curContext.Response.AddHeader("content-disposition",
                                                 "attachment;filename*=UTF-8''" +
                                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_楼栋基础信息_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                curContext.Response.Charset = "UTF-8";
                #endregion
                try
                {
                    //操作日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.楼栋, SYS_Code_Dict.操作.导出, "", "", "导出楼栋基础信息", RequestHelper.GetIP());

                    using (var ms = ExcelHandle.ListToExcel(result.ToList()))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/Build/ExportBuild", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                    return this.Back("操作失败！");
                }
            }
            return base.AuthorizeWarning("对不起，没有需要导出的数据！");
        }

        //弹出窗口并跳转到指定的页面
        public ContentResult Redirect(string paras, string alert = null, string action = null)
        {
            var script = string.Format("<script>{0};{1};</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')", string.IsNullOrEmpty(action) ? string.Empty : "location.href='" + Url.Action(action, new { projectId = paras }) + "'");
            return this.Content(script);
        }

        //获取楼栋数量
        public int GetBuildCount(int cityId, int fxtcompanyId, int projectId)
        {
            return _build.GetBuildCount(cityId, fxtcompanyId, projectId);
        }

        //获取房号数量
        public int GetHouseCount(int cityId, int fxtcompanyId, int buildingId)
        {
            return _build.GetHouseCount(cityId, fxtcompanyId, buildingId);
        }

        [HttpPost]
        public JsonResult BatchSetEvalue(int projectId, string buildingName, int purposeCode, int buildingTypeCode, int isEvalue)
        {
            var building = new DAT_Building
            {
                projectid = projectId,
                buildingname = buildingName,
                purposecode = purposeCode,
                buildingtypecode = buildingTypeCode,
                isevalue = isEvalue,
                cityid = Passport.Current.CityId,
                fxtcompanyid = Passport.Current.FxtCompanyId
            };

            try
            {
                var result = _build.BatchSetEvalue(building);
                return Json(true);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Build/BatchSetEvalue", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(false);
            }
        }

        public ActionResult SetHouseRatio(int projectid, List<string> ids)
        {
            try
            {
                string idstring = string.Empty;
                if (ids != null)
                {
                    foreach (string id in ids)
                    {
                        var array = id.Split('#');
                        idstring += array[0] + ",";
                    }
                }
                idstring = idstring.TrimEnd(',');

                int sussnum; int count;
                _house.SetHouseRatio(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName, 0, projectid, idstring, out sussnum, out count);
                return Json(new { result = true, success = sussnum, msg = "成功设置" + sussnum + "条房号系数！" + ((count - sussnum) > 0 ? "有" + (count - sussnum) + "条房号系数没有更新，可能原因是该房号的楼层差系数为空。" : "") });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/HouseRatio/SetHouseRatio", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, success = 0, msg = "设置失败！" });
            }
        }

        public ActionResult SetVQHouseRatio(int projectid, int parentProductTypeCode)
        {
            try
            {
                if (!(projectid > 0 && parentProductTypeCode > 0))
                {
                    return Json(new { result = false, msg = "设置失败！" });
                }
                _house.SetVQHouseRatio(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName, projectid, parentProductTypeCode);
                return Json(new { result = true, msg = "成功设置房号VQ系数！" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/HouseRatio/SetVQHouseRatio", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "设置失败！" });
            }
        }

        #region 辅助函数
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

        private List<BuildStatiParam> OrderByBuildingName(List<BuildStatiParam> buildinglist)
        {
            foreach (var building in buildinglist)
            {
                building.orderbyName = building.BuildingName.Replace("号楼", "").Replace("栋", "").Replace("幢", "").Replace("座", "");

                string re = Regex.Match(building.orderbyName, "(?<数字>\\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["数字"].Value;
                if (!string.IsNullOrWhiteSpace(re) && Int32.Parse(re) > 0)
                {
                    building.orderby = Int32.Parse(re);
                    building.orderbyName = Regex.Replace(building.orderbyName, "(?<数字>\\d+)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }

                building.orderbyName = building.orderbyName.Replace("甲", "1甲");
                building.orderbyName = building.orderbyName.Replace("乙", "2乙");
                building.orderbyName = building.orderbyName.Replace("丙", "3丙");
                building.orderbyName = building.orderbyName.Replace("丁", "4丁");
                building.orderbyName = building.orderbyName.Replace("一", "5一");
                building.orderbyName = building.orderbyName.Replace("二", "6二");
                building.orderbyName = building.orderbyName.Replace("三", "7三");
                building.orderbyName = building.orderbyName.Replace("四", "8四");
                building.orderbyName = building.orderbyName.Replace("五", "9五");
            }

            return buildinglist.OrderBy(m => (int?)TryParseHelper.StrToInt32(m.orderbyName) == null ? int.MaxValue : (int?)TryParseHelper.StrToInt32(m.orderbyName)).ThenBy(t => t.orderbyName).ThenBy(t => t.orderby).ToList();
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

                        cp.IsDeleteTrue = IsDeleteTrue;
                        cp.IsExportHose = IsExportHose;
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
