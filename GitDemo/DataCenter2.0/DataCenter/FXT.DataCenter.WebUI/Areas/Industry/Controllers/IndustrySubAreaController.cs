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

namespace FXT.DataCenter.WebUI.Areas.Industry.Controllers
{
    [Authorize]
    public class IndustrySubAreaController : BaseController
    {
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IImportTask _importTask;
        private readonly IIndustrySubArea _industrySubArea;
        private readonly IIndustryProject _industryProject;
        public IndustrySubAreaController(ILog log, IDropDownList dropDownList, IImportTask importTask, IIndustrySubArea industrySubArea, IIndustryProject industryProject)
        {
            this._log = log;
            this._dropDownList = dropDownList;
            this._importTask = importTask;
            this._industrySubArea = industrySubArea;
            this._industryProject = industryProject;
        }
        //
        // GET: /Industry/IndustrySubArea/

        public ActionResult Index(SYS_SubArea_Industry subArea, int? pageIndex)
        {
            BindViewData(-1);

            subArea.FxtCompanyId = Passport.Current.FxtCompanyId;
            subArea.CityId = Passport.Current.CityId;

            var pageSize = 30;
            int totalCount;
            var result = _industrySubArea.GetSubAreas(subArea, pageIndex ?? 1, pageSize, out totalCount).AsQueryable();
            var subAreaList = new PagedList<SYS_SubArea_Industry>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", subAreaList);
        }

        #region 编辑
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

            var result = _industrySubArea.GetSubAreaById(int.Parse(splitArray[1]));
            BindViewData(result.AreaId);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(SYS_SubArea_Industry subArea)
        {
            try
            {
                subArea.SaveDate = DateTime.Now;
                subArea.SaveUser = Passport.Current.ID;

                _industrySubArea.UpdateSubArea(subArea);

                //SYS_SubArea_Industry_Coordinate
                var subAreaIndustryCoordinateList = new List<SYS_SubArea_Industry_Coordinate>();
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                if (!string.IsNullOrWhiteSpace(subArea.LngOrLat))
                {
                    var xyList = subArea.LngOrLat.Split('|');
                    if (xyList.Length > 0)
                    {
                        xyList.ToList().ForEach(m => subAreaIndustryCoordinateList.Add(
                            new SYS_SubArea_Industry_Coordinate
                            {
                                SubAreaId = subArea.SubAreaId,
                                AreaId = subArea.AreaId,
                                CityID = cityId,
                                FxtCompanyID = fxtCompanyId,
                                X = Convert.ToDecimal(m.Split(',')[0]),
                                Y = Convert.ToDecimal(m.Split(',')[1])
                            }));
                    }

                    var list = _industrySubArea.GetSubAreaIndustryCoordinate(subArea.SubAreaId, subArea.AreaId, Passport.Current.FxtCompanyId);
                    if (list == 0)
                    {
                        subAreaIndustryCoordinateList.ForEach(p => _industrySubArea.AddSubAreaIndustryCoordinate(p));
                    }
                    else
                    {
                        subAreaIndustryCoordinateList.ForEach(p => _industrySubArea.UpdateSubAreaIndustryCoordinate(subArea.SubAreaId, subArea.AreaId, Passport.Current.FxtCompanyId));
                        subAreaIndustryCoordinateList.ForEach(p => _industrySubArea.AddSubAreaIndustryCoordinate(p));
                    }
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商务中心, SYS_Code_Dict.操作.修改, "", "", "修改商务中心", RequestHelper.GetIP());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustrySubArea/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        [HttpPost]
        public JsonResult IsExistSubAreaIndustry(int areaId, int subAreaId, string subAreaName)
        {
            return Json(_industrySubArea.IsExistSubAreaIndustry(areaId, subAreaId, subAreaName, Passport.Current.FxtCompanyId));
        }
        #endregion

        #region 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            BindViewData(-1);
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(SYS_SubArea_Industry subArea)
        {
            try
            {
                subArea.FxtCompanyId = Passport.Current.FxtCompanyId;
                subArea.CreateDate = DateTime.Now;
                subArea.Creators = Passport.Current.ID;

                var subAreaId = _industrySubArea.AddSubArea(subArea);

                var subAreaIndustryCoordinateList = new List<SYS_SubArea_Industry_Coordinate>();
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                if (!string.IsNullOrWhiteSpace(subArea.LngOrLat))
                {
                    var xyList = subArea.LngOrLat.Split('|');
                    if (xyList.Length > 0)
                    {
                        xyList.ToList().ForEach(m => subAreaIndustryCoordinateList.Add(
                            new SYS_SubArea_Industry_Coordinate
                            {
                                SubAreaId = subAreaId,
                                AreaId = subArea.AreaId,
                                CityID = cityId,
                                FxtCompanyID = fxtCompanyId,
                                X = Convert.ToDecimal(m.Split(',')[0]),
                                Y = Convert.ToDecimal(m.Split(',')[1])
                            }));
                    }
                    subAreaIndustryCoordinateList.ForEach(p => _industrySubArea.AddSubAreaIndustryCoordinate(p));
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业片区, SYS_Code_Dict.操作.新增, "", "", "新增工业片区", RequestHelper.GetIP());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustrySubArea/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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
                    if (_industrySubArea.CanDelete(int.Parse(array[1]), int.Parse(array[0])) > 0)
                    {
                        failList.Add(array[1]);
                        continue;
                    }
                    var result = _industrySubArea.GetSubAreaById(int.Parse(array[1]));
                    _industrySubArea.DeleteSubArea(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商务中心, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商务中心", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustrySubArea/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 导出
        [HttpGet]
        public ActionResult Export(int? areaid, string SubAreaName)
        {
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }

            var subArea = new SYS_SubArea_Industry
            {
                FxtCompanyId = Passport.Current.FxtCompanyId,
                CityId = Passport.Current.CityId,
                AreaId = areaid ?? 0,
                SubAreaName = SubAreaName,
            };

            int totalCount;
            var result = _industrySubArea.GetSubAreas(subArea, 1, int.MaxValue, out totalCount);

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_工业_片区数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业片区, SYS_Code_Dict.操作.导出, "", "", "导出工业片区", RequestHelper.GetIP());

            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }
        #endregion

        #region 导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.工业片区, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业片区, SYS_Code_Dict.操作.导入, "", "", "导入工业片区", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "IndustrySubArea");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustrySubArea/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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
        public ActionResult DeleteIndustrySubAreaImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Industry/IndustrySubArea/DeleteIndustrySubAreaImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 统计
        [HttpGet]
        public JsonResult IndustryProjectCount(int subAreaId)
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

            var result = _industryProject.GetProjectCounts(subAreaId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.统计)]
        public ActionResult Statistic(int? areaId, int? pageIndex)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View(new List<SYS_SubArea_Industry>().ToPagedList(1, 30));
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text"));
            var statistic = _industrySubArea.GetSubAreaIndustryStatistic(areaId ?? -1, Passport.Current.FxtCompanyId, Passport.Current.CityId, self);

            var pageSize = 10;
            var result = statistic.ToPagedList(pageIndex ?? 1, pageSize);
            return View(result);

        }
        #endregion

        private void BindViewData(int areaId)
        {
            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text", areaId));
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
    }
}
