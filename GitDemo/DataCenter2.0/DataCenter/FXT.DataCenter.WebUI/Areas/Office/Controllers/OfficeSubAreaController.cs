using System;
using System.Collections.Generic;
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

namespace FXT.DataCenter.WebUI.Areas.Office.Controllers
{
    [Authorize]
    public class OfficeSubAreaController : BaseController
    {
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IOfficeSubArea _officeSubArea;
        private readonly IOfficeProject _officeProject;
        private readonly IImportTask _importTask;
        public OfficeSubAreaController(ILog log, IDropDownList dropDownList, IOfficeSubArea officeSubArea, IOfficeProject officeProject, IImportTask importTask)
        {
            this._log = log;
            this._dropDownList = dropDownList;
            this._officeSubArea = officeSubArea;
            this._officeProject = officeProject;
            this._importTask = importTask;
        }
        //
        // GET: /Office/OfficeSubArea/

        public ActionResult Index(SYS_SubArea_Office officeSubArea, int? pageIndex, bool? isExternalRequest)
        {
            BindViewData(-1);

            officeSubArea.FxtCompanyId = Passport.Current.FxtCompanyId;
            officeSubArea.CityId = Passport.Current.CityId;

            var pageSize = 30;
            int totalCount;
            var result = _officeSubArea.GetSubAreas(officeSubArea, pageIndex ?? 1, pageSize, out totalCount).AsQueryable();

            var projectOfficeResult = new PagedList<SYS_SubArea_Office>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", projectOfficeResult);
        }

        #region 编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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

            var result = _officeSubArea.GetSubAreaById(int.Parse(splitArray[1]));
            BindDropDownList(result.AreaId, Passport.Current.FxtCompanyId);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(SYS_SubArea_Office officeSubArea)
        {
            try
            {
                officeSubArea.SaveDate = DateTime.Now;
                officeSubArea.SaveUser = Passport.Current.ID;

                _officeSubArea.UpdateSubArea(officeSubArea);

                //SYS_SubArea_Office_Coordinate
                var subAreaOfficeCoordinateList = new List<SYS_SubArea_Office_Coordinate>();
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                if (!string.IsNullOrWhiteSpace(officeSubArea.LngOrLat))
                {
                    var xyList = officeSubArea.LngOrLat.Split('|');
                    if (xyList.Length > 0)
                    {
                        xyList.ToList().ForEach(m => subAreaOfficeCoordinateList.Add(
                            new SYS_SubArea_Office_Coordinate
                            {
                                SubAreaId = officeSubArea.SubAreaId,
                                AreaId = officeSubArea.AreaId,
                                CityID = cityId,
                                FxtCompanyID = fxtCompanyId,
                                X = Convert.ToDecimal(m.Split(',')[0]),
                                Y = Convert.ToDecimal(m.Split(',')[1])
                            }));

                    }

                    var list = _officeSubArea.GetSubAreaOfficeCoordinate(officeSubArea.SubAreaId, officeSubArea.AreaId, Passport.Current.FxtCompanyId);
                    if (list == 0)
                    {
                        subAreaOfficeCoordinateList.ForEach(p => _officeSubArea.AddSubAreaOfficeCoordinate(p));
                    }
                    else
                    {
                        subAreaOfficeCoordinateList.ForEach(p => _officeSubArea.UpdateSubAreaOfficeCoordinate(officeSubArea.SubAreaId, officeSubArea.AreaId, Passport.Current.FxtCompanyId));
                        subAreaOfficeCoordinateList.ForEach(p => _officeSubArea.AddSubAreaOfficeCoordinate(p));
                    }
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商务中心, SYS_Code_Dict.操作.修改, "", "", "修改商务中心", RequestHelper.GetIP());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeSubArea/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        [HttpPost]
        public JsonResult IsExistSubAreaOffice(int areaId, int subAreaId, string subAreaName)
        {
            return Json(_officeSubArea.IsExistSubAreaOffice(areaId, subAreaId, subAreaName, Passport.Current.FxtCompanyId));
        }

        private void BindDropDownList(int areaId, int fxtCompanyId)
        {
            this.ViewBag.AreaName = GetAreaName();
            this.ViewBag.TypeCodeName = GetDictById(SYS_Code_Dict._办公区域等级);
        }
        #endregion

        #region 新增

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.新增)]
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
        public ActionResult Create(SYS_SubArea_Office officeSubArea)
        {
            try
            {
                officeSubArea.FxtCompanyId = Passport.Current.FxtCompanyId;
                officeSubArea.CreateDate = DateTime.Now;
                officeSubArea.Creators = Passport.Current.ID;

                var subAreaId = _officeSubArea.AddSubArea(officeSubArea);

                var subAreaOfficeCoordinateList = new List<SYS_SubArea_Office_Coordinate>();
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                if (!string.IsNullOrWhiteSpace(officeSubArea.LngOrLat))
                {
                    var xyList = officeSubArea.LngOrLat.Split('|');
                    if (xyList.Length > 0)
                    {
                        xyList.ToList().ForEach(m => subAreaOfficeCoordinateList.Add(
                            new SYS_SubArea_Office_Coordinate
                            {
                                SubAreaId = subAreaId,
                                AreaId = officeSubArea.AreaId,
                                CityID = cityId,
                                FxtCompanyID = fxtCompanyId,
                                X = Convert.ToDecimal(m.Split(',')[0]),
                                Y = Convert.ToDecimal(m.Split(',')[1])
                            }));

                    }
                    subAreaOfficeCoordinateList.ForEach(p => _officeSubArea.AddSubAreaOfficeCoordinate(p));
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商务中心, SYS_Code_Dict.操作.新增, "", "", "新增办公商务中心", RequestHelper.GetIP());

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeSubArea/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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
                Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                var failList = new List<string>();
                var deleteList = new List<string>();
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
                    if (_officeSubArea.CanDelete(int.Parse(array[1]), int.Parse(array[0])) > 0)
                    {
                        failList.Add(array[1]);
                        continue;
                    }
                    var result = _officeSubArea.GetSubAreaById(int.Parse(array[1]));
                    _officeSubArea.DeleteSubArea(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商务中心, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商务中心", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeSubArea/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 导出
        [HttpGet]
        public ActionResult Export(int? areaid, int? TypeCode, string SubAreaName)
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

            var subArea = new SYS_SubArea_Office
            {
                FxtCompanyId = Passport.Current.FxtCompanyId,
                CityId = Passport.Current.CityId,
                AreaId = areaid ?? 0,
                SubAreaName = SubAreaName,
                TypeCode = TypeCode ?? 0,
            };

            int totalCount;
            var result = _officeSubArea.GetSubAreas(subArea, 1, int.MaxValue, out totalCount);

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_办公_商务中心_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商务中心, SYS_Code_Dict.操作.导出, "", "", "导出办公商务中心", RequestHelper.GetIP());

            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        #endregion

        #region 导入

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.办公商务中心, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID,
                    Passport.Current.ID, SYS_Code_Dict.功能模块.商务中心, SYS_Code_Dict.操作.导入, "", "", "导入办公商务中心",
                    RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "OfficeSubArea");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Office/OfficeSubArea/UploadFile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("UploadFile");
        }
        /// <summary>
        /// 导入文件保存
        /// </summary>
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

        //根据CodeName获取Code
        private int GetCodeByName(string name)
        {
            return _dropDownList.GetCodeByName(name);
        }

        //删除导入记录
        public ActionResult DeleteOfficeSubAreaImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Office/OfficeSubArea/DeleteOfficeSubAreaImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 统计
        [HttpGet]
        public JsonResult OfficeProjectCount(int subAreaId)
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

            var result = _officeProject.GetProjectCounts(subAreaId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.统计)]
        public ActionResult Statistic(int? areaId, int? pageIndex)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.办公数据分类.办公基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View(new List<SYS_SubArea_Office>().ToPagedList(1, 30));
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text"));
            var statistic = _officeSubArea.GetSubAreaOfficeStatistic(areaId ?? -1, Passport.Current.FxtCompanyId, Passport.Current.CityId, self);

            var pageSize = 10;
            var result = statistic.ToPagedList(pageIndex ?? 1, pageSize);
            return View(result);

        }
        #endregion
        private void BindViewData(int areaId)
        {
            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text", areaId));
            ViewData.Add("TypeCode", new SelectList(GetDictById(SYS_Code_Dict._办公区域等级), "Value", "Text", "-1"));
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
        //根据code的ID查找code
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

    }
}
