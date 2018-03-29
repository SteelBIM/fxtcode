using System;
using System.Collections.Generic;
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

namespace FXT.DataCenter.WebUI.Areas.Business.Controllers
{
    public class BusinessHouseController : BaseController
    {
        private readonly IBusinessStreet _project;
        private readonly IDat_Building_Biz _build;
        private readonly IDat_Floor_Biz _floor;
        private readonly IDat_House_Biz _house;
        private readonly ILog _log;
        private readonly IImportTask _importTask;
        private readonly IDropDownList _dropDownList;

        public BusinessHouseController(IBusinessStreet project, IDat_Building_Biz build, IDat_Floor_Biz florr, IDat_House_Biz house, ILog log, IDropDownList dropDownList, IImportTask importTask)
        {
            this._project = project;
            this._floor = florr;
            this._house = house;
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
            this._build = build;
        }

        public ActionResult Index(Dat_House_Biz house_biz, int? pageIndex)
        {
            SaveVal(house_biz.BuildingId, house_biz.FloorId, ViewBag.ProjectId = house_biz.ProjectId);
            house_biz.CityId = Passport.Current.CityId;
            house_biz.FxtCompanyId = Passport.Current.FxtCompanyId;
            #region 查看权限
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion
            var result = _house.GetDat_House_BizList(house_biz, self).ToPagedList(pageIndex ?? 1, 30);
            return View(result);
        }

        //新建
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create(string Id)
        {
            string buildingId = Id.Split('#')[0], floorId = Id.Split('#')[1], projectId = Id.Split('#')[2];
            SaveVal(buildingId, floorId, projectId);
            InitDropDown();
            var floor = new Dat_House_Biz
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId
            };
            return View("EdityHouse", floor);
        }

        [HttpPost]
        public ActionResult Create(Dat_House_Biz house)
        {
            try
            {
                house.CityId = Passport.Current.CityId;
                house.FxtCompanyId = Passport.Current.FxtCompanyId;
                house.Creator = house.SaveUser = Passport.Current.UserName;
                _house.AddDat_House_Biz(house);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业房号, SYS_Code_Dict.操作.新增, "", "", "商业房号", RequestHelper.GetIP());
                return RedirectToAction("Index", new { projectId = house.ProjectId, buildingId = house.BuildingId, floorId = house.FloorId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessHouse/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        //编辑
        [HttpGet]
        public ActionResult EdityHouse(string id)
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
                if (int.Parse(splitArray[3]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }
            #endregion
            int buildId = GetInt(splitArray[0]), floorId = GetInt(splitArray[1]), houseId = GetInt(splitArray[2]), cityId = Passport.Current.CityId, fxtCompanyId = Passport.Current.FxtCompanyId, projectId = GetInt(splitArray[4]);
            var result = _house.GetDat_House_BizById(houseId, cityId, fxtCompanyId);
            SaveVal(buildId, floorId, projectId);
            InitDropDown();
            return View(result);

        }

        [HttpPost]
        public ActionResult EdityHouse(Dat_House_Biz house)
        {
            try
            {
                house.CityId = Passport.Current.CityId;
                //house.FxtCompanyId = Passport.Current.FxtCompanyId;
                house.SaveDateTime = DateTime.Now;
                house.SaveUser = Passport.Current.UserName;
                int result = _house.UpdateDat_House_Biz(house, Passport.Current.FxtCompanyId);
                return RedirectToAction("Index", new { projectId = house.ProjectId, buildingId = house.BuildingId, floorId = house.FloorId });

            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Business/BusinessHouse/EdityHouse", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
        }

        //删除
        [HttpPost]
        public ActionResult DeleteHouse(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
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
                        if (GetInt(array[3]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[2]);
                            continue;
                        }
                    }
                    _house.DeleteDat_House_Biz(GetInt(array[2]), Passport.Current.UserName, Passport.Current.CityId, GetInt(array[3]), Passport.Current.ProductTypeCode, Passport.Current.FxtCompanyId);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业房号, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业房号", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessHouse/DeleteHouse", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //导出
        public ActionResult ExportHouse(Dat_House_Biz house)
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
            house.CityId = Passport.Current.CityId;
            house.FxtCompanyId = Passport.Current.FxtCompanyId;
            var result = _house.GetDat_House_BizList(house, self);


            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业楼栋, SYS_Code_Dict.操作.导出, "", "", "导出商业房号", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_房号数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.商业房号信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业房号, SYS_Code_Dict.操作.导入, "", "", "导入商业房号", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "BusinessHouse");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessHouse/UploadFile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        //删除导入记录
        public ActionResult DeleteBusinessHouseImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Business/BusinessHouse/DeleteBusinessHouseImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //商业房号自定义导出
        [HttpGet]
        public ActionResult SelfDefineExport()
        {
            ViewData.Add("AreaList", GetAreaName());
            return View();
        }

        [HttpPost]
        public ActionResult SelfDefineExport(Dat_House_Biz houseBiz, List<string> house)
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

                houseBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                houseBiz.CityId = Passport.Current.CityId;
                var dt = _house.HouseSelfDefineExport(houseBiz, house, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
                if (dt != null && dt.Rows.Count > 0)
                {
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_自定义导出房号_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";

                    MemoryStream ms = ExcelHandle.RenderToExcel(dt);
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业房号, SYS_Code_Dict.操作.导出, "", "", "自定义导出商业房号", RequestHelper.GetIP());

                    return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                }
                return this.Back("没有您要导出的数据");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessHouse/SelfDefineExport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //行政区列表
        [NonAction]
        private IEnumerable<SelectListItem> GetAreaName()
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId);
            var result = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                result.Add(
                new SelectListItem { Value = m.AreaId.ToString(), Text = m.AreaName }
                ));
            result.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            return result;
        }

        #region 房号辅助方法
        /// <summary>
        /// 保存楼栋Id和楼层Id
        /// 刘晓博
        /// 2014-09-22
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        [NonAction]
        private void SaveVal(object buildingId, object floorId, object projectId)
        {
            ViewBag.BuildingId = GetInt(buildingId);
            ViewBag.FloorId = GetInt(floorId);
            ViewBag.ProjectId = GetInt(projectId);
        }
        /// <summary>
        /// 初始化下拉菜单
        /// 刘晓博
        /// 2012-09-22
        /// </summary>
        [NonAction]
        private void InitDropDown()
        {
            ViewBag.PurposeCodeName = BindDropDown(SYS_Code_Dict._居住用途);
            ViewBag.FrontCodeName = BindDropDown(SYS_Code_Dict._朝向);
            ViewBag.ShapeName = BindDropDown(SYS_Code_Dict._平面形状);
            ViewBag.BizCutOffName = BindDropDown(SYS_Code_Dict._商业阻隔);
            ViewBag.BizHouseTypeName = BindDropDown(SYS_Code_Dict._商铺类型);
            ViewBag.BizHouseLocationName = BindDropDown(SYS_Code_Dict._商铺位置类型);
            ViewBag.FlowTypeName = BindDropDown(SYS_Code_Dict._人流量);
        }
        /// <summary>
        /// 绑定下拉
        /// </summary>
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
        /// <summary>
        /// ConvertToInt32
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 导入文件保存
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="filepath"></param>
        /// <param name="saveName"></param>
        /// <returns></returns>
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
        #endregion

    }
}
