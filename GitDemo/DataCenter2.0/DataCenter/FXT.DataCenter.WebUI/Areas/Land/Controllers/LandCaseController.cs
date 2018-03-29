using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using System.IO;

using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.WebUI.Areas.Land.Controllers
{
    //[DataCenterAuthorize(SYS_Code_Dict.土地数据分类.土地案例数据)]
    [Authorize]
    public class LandCaseController : BaseController
    {

        private readonly ILandCase _landCase;
        private readonly IDropDownList _dropDownList;
        private readonly ILog _log;
        private readonly IImportTask _importTask;
        private readonly IDAT_Land _datLand;

        public LandCaseController(ILandCase landCase, IDropDownList dropDownList, ILog log, IMenu menu, IImportTask importTask, IDAT_Land datLand)
        {
            this._importTask = importTask;
            this._landCase = landCase;
            this._dropDownList = dropDownList;
            this._log = log;
            this._datLand = datLand;
            ViewBag.title = "土地案例数据";

        }

        public ActionResult Index(DAT_CaseLand landCase, int? searchId)
        {
            BindViewData(landCase);
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View(new List<DAT_CaseLand>().ToPagedList(1, 30));
            if (operate == (int)PermissionLevel.All) self = false;


            landCase.fxtcompanyid = Passport.Current.FxtCompanyId;
            landCase.cityid = Passport.Current.CityId;
            var pageIndex = landCase.pageIndex == 0 ? 1 : landCase.pageIndex;
            var pageSize = 30; var totalCount = 0;

            var result = searchId != null
                ? _landCase.GetLandCaseByCaseId((int)searchId, Passport.Current.FxtCompanyId, Passport.Current.CityId)
                : _landCase.GetLandCases(landCase, pageIndex, pageSize, out totalCount, self);
            // var landCaseResult = result.ToPagedList(pageIndex, 30);
            var landCaseResult = new PagedList<DAT_CaseLand>(result, pageIndex, pageSize, totalCount);

            return View("Index", landCaseResult);
        }

        //删除案例
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.删除自己)]
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

                    _landCase.DeleteLandCase(int.Parse(array[1]));
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地案例, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除土地案例", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/LandCase/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        public JsonResult GetSubArea(int? areaid)
        {
            return Json(GetSubAreaName(areaid ?? 0), JsonRequestBehavior.AllowGet);
        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindDropDownList();
            return View();
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(DAT_CaseLand model)
        {
            try
            {
                var temp = "";
                if (model.Description != null && model.Description.Any())
                    temp = string.Join(",", model.Description);

                model.landpurposedesc = temp;
                model.cityid = Passport.Current.CityId;
                model.fxtcompanyid = Passport.Current.FxtCompanyId;
                model.createdate = DateTime.Now;
                model.creator = Passport.Current.ID;
                _landCase.AddLandCase(model);

                //如果宗地号不在土地基础数据库，那么就添加
                if (LandNos().Count(m => m.Contains(model.landno)) == 0)
                {
                    var datLand = new DAT_Land
                    {
                        fxtcompanyid = Passport.Current.FxtCompanyId,
                        cityid = Passport.Current.CityId,
                        areaid = model.areaid,
                        subareaid = model.subareaid,
                        landno = model.landno,
                        address = model.landaddress,
                        planpurpose = model.landpurposedesc,
                        landarea = model.landarea,
                        createdate = DateTime.Now
                    };

                    _datLand.AddDAT_Land(datLand);
                }


                //插入日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地案例, SYS_Code_Dict.操作.新增, "", "", "新增土地案例", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/LandCase/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        //案例编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var split = id.Split('#');

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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
                if (int.Parse(split[0]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }

            var landCase = _landCase.GetLandCaseByCaseId(int.Parse(split[1]), int.Parse(split[0]), Passport.Current.CityId).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(landCase.landpurposedesc))
            {
                var array = landCase.landpurposedesc.Split(',');
                BindDropDownList(array.ToList(), landCase.areaid ?? 0);
            }
            else
            {
                BindDropDownList(subAreaId: landCase.areaid ?? 0);
            }

            return View("Create", landCase);
        }
        //编辑保存
        [HttpPost]
        //[DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.修改自己)]
        public ActionResult Edit(DAT_CaseLand model)
        {

            try
            {
                var temp = string.Empty;
                if (model.Description != null)
                {
                    temp = string.Join(",", model.Description);
                }

                model.landpurposedesc = temp;
                model.savedatetime = DateTime.Now;
                model.saveuser = Passport.Current.ID;
                _landCase.UpdateLandCase(model);


                //如果宗地号不在土地基础数据库，那么就添加
                if (LandNos().Count(m => m.Contains(model.landno)) == 0)
                {
                    var datLand = new DAT_Land
                    {
                        fxtcompanyid = Passport.Current.FxtCompanyId,
                        cityid = Passport.Current.CityId,
                        areaid = model.areaid,
                        subareaid = model.subareaid,
                        landno = model.landno,
                        address = model.landaddress,
                        planpurpose = model.landpurposedesc,
                        landarea = model.landarea,
                        createdate = DateTime.Now
                    };

                    _datLand.AddDAT_Land(datLand);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地案例, SYS_Code_Dict.操作.修改, model.caseid.ToString(), "", "修改土地案例", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/LandCase/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        //exce导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.土地案例, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            var taskList = result.ToPagedList(1, 30);

            return View(taskList);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());

            if (null == file) return this.Back("操作失败！");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地案例, SYS_Code_Dict.操作.导入, "", "", "导入土地案例", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "LandCase");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/LandCase/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

        }

        //删除土地案例导入记录
        public ActionResult DeleteLandCaseImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteLandCaseImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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

        //excel导出
        [HttpGet]
        public ActionResult Export(DAT_CaseLand model)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            int totalCount;
            var result = _landCase.GetLandCases(model, 1, 1000000, out totalCount, self);

            //操作日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地案例, SYS_Code_Dict.操作.导出, "", "", "导出土地案例", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_土地_案例数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.GetBuffer(), "application/vnd.ms-excel");
            }
        }

        //统计
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地案例数据, SYS_Code_Dict.页面权限.统计)]
        public ActionResult Statistics(string startDate, string endDate, string dataType)
        {
            //startDate = string.IsNullOrWhiteSpace(startDate) ? "" : startDate;
            this.ViewBag.startDate = startDate;
            //endDate = string.IsNullOrWhiteSpace(endDate) ? "" : endDate;
            this.ViewBag.endDate = endDate;

            var areaNameList = new List<string>();
            var areaList = new List<decimal>();
            var CJData = new List<LandCaseStatisticDTO>();

            var areaName = _dropDownList.GetAreaName(Passport.Current.CityId);
            areaName.ToList().ForEach(m =>
            {
                var result = _landCase.GetStatisticsData(startDate, endDate, dataType, m.AreaId, Passport.Current.FxtCompanyId, Passport.Current.CityId).FirstOrDefault();

                areaList.Add(result == null ? 0 : result.CJMJ);
                areaNameList.Add(m.AreaName);
                if (result != null)
                {
                    CJData.Add(result);
                }
            });

            this.ViewBag.areaNameList = "'" + string.Join("','", areaNameList) + "'";
            this.ViewBag.areaList = string.Join(",", areaList);
            this.ViewBag.CJData = CJData;
            return View();
        }


        public JsonResult GetLandNo()
        {
            var landNos = LandNos();
            return Json(landNos.Select(m => new { landNo = m }).Distinct(), JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private IEnumerable<string> LandNos()
        {
            return _datLand.GetLandNo(Passport.Current.CityId, Passport.Current.FxtCompanyId);
        }

        #region 帮助程序
        [NonAction]
        private void BindViewData(DAT_CaseLand landCase)
        {
            ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text", landCase.areaid));
            ViewData.Add("subareaid", new SelectList(GetSubAreaName(landCase.areaid ?? 0), "Value", "Text", landCase.subareaid));
            ViewData.Add("landpurposecode", new SelectList(GetLandUse(), "Value", "Text", landCase.landpurposecode));
            ViewData.Add("bargaintypecode", new SelectList(GetDictById(SYS_Code_Dict._买卖方式), "Value", "Text", landCase.bargaintypecode));
        }
        [NonAction]
        private void BindDropDownList(ICollection<string> purposeDescList = null, int subAreaId = 0)
        {
            var areaName = GetAreaName();
            var subAreaName = GetSubAreaName(subAreaId);
            var bargainType = GetDictById(SYS_Code_Dict._买卖方式);
            //bargainType.RemoveAt(0);
            var landUse = GetLandUse();
            //landUse.RemoveAt(0);
            var bargainState = GetDictById(SYS_Code_Dict._成交状态);
            //bargainState.RemoveAt(0);
            var developDegree = GetDictById(SYS_Code_Dict._土地开发情况);
            //developDegree.RemoveAt(0);
            var landSource = GetDictById(SYS_Code_Dict._土地来源);
            //landSource.RemoveAt(0);

            this.ViewBag.AreaName = areaName; //行政区
            this.ViewBag.SubAreaName = subAreaName;//片区
            this.ViewBag.BargainType = bargainType;//买卖方式
            this.ViewBag.LandUse = landUse;//土地用途
            this.ViewBag.BargainState = bargainState;//成交状态
            this.ViewBag.DevelopDegree = developDegree;//开发情况
            this.ViewBag.LandSource = landSource;//土地来源
            //规划用途
            this.ViewBag.LandPlanUse = GetLandPlanUse(purposeDescList ?? new List<string>());
            this.ViewBag.UseTypeCodeName = GetDictById(SYS_Code_Dict._土地所有权);
            this.ViewBag.LandClassName = GetDictById(SYS_Code_Dict._土地等级);
        }

        //行政区列表
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
        //片区列表
        [NonAction]
        private List<SelectListItem> GetSubAreaName(int areaid)
        {
            var subArea = _dropDownList.GetSubAreaName(areaid);
            var subAreaResult = new List<SelectListItem>();
            subArea.ToList().ForEach(m =>
                subAreaResult.Add(
                new SelectListItem { Value = m.SubAreaId.ToString(), Text = m.SubAreaName }
                ));
            subAreaResult.Insert(0, new SelectListItem { Value = "-1", Text = "全部" });
            return subAreaResult;
        }

        //土地用途
        [NonAction]
        private List<SelectListItem> GetLandUse()
        {
            var landUse = _dropDownList.GetLandPurpose(SYS_Code_Dict._土地用途);
            var landUseResult = new List<SelectListItem>();
            landUse.ToList().ForEach(m =>
                landUseResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            landUseResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return landUseResult;
        }

        //规划用途
        [NonAction]
        private List<SelectListItem> GetLandPlanUse(ICollection<string> selected)
        {
            var landUse = _dropDownList.GetLandPurpose(SYS_Code_Dict._土地规划用途);
            var landUseResult = new List<SelectListItem>();
            landUse.ToList().ForEach(m =>
                landUseResult.Add(
                new SelectListItem { Value = m.CodeName.ToString(), Text = m.CodeName, Selected = selected.Contains(m.CodeName) }
                ));
            return landUseResult;
        }


        //买卖方式 3004,成交状态 3003，土地开发情况 3005
        [NonAction]
        private List<SelectListItem> GetDictById(int id)
        {
            var bargainType = _dropDownList.GetDictById(id);
            var bargainTypeResult = new List<SelectListItem>();
            bargainType.ToList().ForEach(m =>
                bargainTypeResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            bargainTypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return bargainTypeResult;
        }

        #endregion
    }
}
