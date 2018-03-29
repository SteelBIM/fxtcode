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
using System.IO;
using System.Data;
using System.Threading.Tasks;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Areas.Industry.Models;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Industry.Controllers
{
    [Authorize]
    public class IndustryHouseController : BaseController
    {
        private readonly IDropDownList _dropDownList;
        private readonly ILog _log;
        private readonly IIndustryProject _industryProject;
        private readonly IIndustryBuilding _industryBuilding;
        private readonly IIndustryHouse _industryHouse;
        private readonly IImportTask _importTask;

        public IndustryHouseController(IDropDownList dropDownList, ILog log, IIndustryProject industryProject, IIndustryBuilding industryBuilding, IIndustryHouse industryHouse, IImportTask importTask)
        {
            this._dropDownList = dropDownList;
            this._log = log;
            this._industryProject = industryProject;
            this._industryBuilding = industryBuilding;
            this._industryHouse = industryHouse;
            this._importTask = importTask;
        }
        //
        // GET: /Industry/IndustryHouse/

        public ActionResult Index(int buildingId, int projectId)
        {
            BindDropDownList();

            this.ViewBag.ProjectId = projectId;
            this.ViewBag.ProjectFxtCompanyId = _industryProject.GetProjectNameById(projectId, Passport.Current.FxtCompanyId).FxtCompanyId;
            this.ViewBag.ProjectName = _industryProject.GetProjectNameById(projectId, Passport.Current.FxtCompanyId).ProjectName;
            this.ViewBag.BuildingId = buildingId;
            this.ViewBag.BuildingFxtCompanyId = _industryBuilding.GetIndustryBuilding(buildingId, Passport.Current.FxtCompanyId).FxtCompanyId;
            this.ViewBag.BuildingName = _industryBuilding.GetIndustryBuilding(buildingId, Passport.Current.FxtCompanyId).BuildingName;

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            //房号数据列表
            var houses = _industryHouse.GetIndustryHouses(projectId, buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            //室号数据列表
            var unitNos = _industryHouse.GetIndustryHouses_UnitNo(buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
            //楼层号数据列表
            var floorNos = _industryHouse.GetIndustryHouses_FloorNo(buildingId, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);

            var viewModel = BindViewModel(houses, unitNos, floorNos, buildingId, projectId);
            return View(viewModel);
        }

        #region 编辑
        //单条房号记录编辑
        [HttpGet]
        public ActionResult Edit(int houseId, int fxtcompanyId)
        {

            var self = true; int operate;
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
                if (fxtcompanyId != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }
            BindDropDownList();
            var house = _industryHouse.GetIndustryHouse(houseId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            return View(house);
        }

        //单条房号记录保存
        [HttpPost]
        public ActionResult Edit(DatHouseIndustry datHouse)
        {
            try
            {
                var house = _industryHouse.GetIndustryHouse(datHouse.HouseId, datHouse.CityId, datHouse.FxtCompanyId);
                if (house.FloorNum != datHouse.FloorNum)
                {
                    _industryHouse.UpdateFloorNum(datHouse.FloorNo, datHouse.FloorNum, datHouse.CityId, datHouse.FxtCompanyId, Passport.Current.ID);
                }

                datHouse.SaveUser = Passport.Current.ID;
                datHouse.SaveDateTime = DateTime.Now;

                _industryHouse.UpdateIndustryHouse(datHouse, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.修改, datHouse.HouseId.ToString(), datHouse.HouseName, "修改工业房号", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("Industry/IndustryHouse/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
            }
            return base.RefreshParent();
        }

        //保存
        [HttpPost]
        public JsonResult Save(IndustryHouseOperate ohp)
        {
            if (ohp == null)
            {
                return Json(false);
            }
            #region 新增

            if (ohp.AddHouse != null && ohp.AddHouse.Any())
            {
                int operate;
                Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.新增, SYS_Code_Dict.页面权限.新增, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    throw new Exception("无新增权限");
                }

                try
                {
                    foreach (var item in ohp.AddHouse)
                    {
                        var house = ConvertValue(item);
                        _industryHouse.AddIndustryHouse(house);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("Industry/IndustryHouse/Save", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                }
            }

            #endregion

            #region 修改
            if (ohp.UpdateHouse != null && ohp.UpdateHouse.Any())
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    throw new Exception("对不起，您没有修改权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                try
                {
                    foreach (var item in ohp.UpdateHouse)
                    {
                        var house = ConvertValue(item);
                        if (self)
                        {
                            if (house.FxtCompanyId != Passport.Current.FxtCompanyId)
                            {
                                continue;
                            }
                        }
                        _industryHouse.UpdateIndustryHouse(house, Passport.Current.FxtCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("Industry/IndustryHouse/Save", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                }
            }
            #endregion

            #region 删除
            if (ohp.DeleteHouse != null && ohp.DeleteHouse.Any())
            {
                var self = true; int operate;
                Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    throw new Exception("对不起，您没有删除权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                try
                {
                    foreach (var item in ohp.DeleteHouse)
                    {
                        var house = ConvertValue(item, true);
                        if (self)
                        {
                            if (house.FxtCompanyId != Passport.Current.FxtCompanyId)
                            {
                                continue;
                            }
                        }
                        house.SaveDateTime = DateTime.Now;
                        house.SaveUser = Passport.Current.ID;
                        _industryHouse.DeleteIndustryHouse(house, Passport.Current.FxtCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("Industry/IndustryHouse/Save", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                }
            }
            #endregion

            #region 批量修改实际层

            if (ohp.UpdateFloorNum == null || !ohp.UpdateFloorNum.Any()) return Json(true);

            foreach (var item in ohp.UpdateFloorNum)
            {
                _industryHouse.UpdateFloorNum(item.FloorNo, item.FloorNum, Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID);
            }

            #endregion

            return Json(true);
        }

        #endregion

        #region 导入
        //删除导入记录
        public ActionResult DeleteIndustryHouseImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Industry/IndustryHouse/DeleteIndustryHouseImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.工业房号信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业房号, SYS_Code_Dict.操作.导入, "", "", "导入工业房号", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "IndustryHouse");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryHouse/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
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

        #endregion

        #region 自定义导出
        [HttpGet]
        public ActionResult SelfDefineExport()
        {
            BindDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult SelfDefineExport(DatHouseIndustry datHouse, List<string> house)
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

                datHouse.FxtCompanyId = Passport.Current.FxtCompanyId;
                datHouse.CityId = Passport.Current.CityId;
                var dt = _industryHouse.HouseSelfDefineExport(datHouse, house, Passport.Current.CityId, Passport.Current.FxtCompanyId, self);
                if (dt != null && dt.Rows.Count > 0)
                {
                    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                    curContext.Response.AddHeader("content-disposition",
                                                     "attachment;filename*=UTF-8''" +
                                                     System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_工业_自定义导出房号_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                    curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    curContext.Response.Charset = "UTF-8";

                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业房号, SYS_Code_Dict.操作.导出, "", "", "自定义导出工业房号", RequestHelper.GetIP());

                    using (MemoryStream ms = ExcelHandle.RenderToExcel(dt))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }

                }
                return this.Back("温馨提示:没有您要导出的数据");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryHouse/SelfDefineExport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        #endregion

        #region 辅助方法
        [NonAction]
        private void BindDropDownList()
        {
            this.ViewBag.PurposeName = GetDictById(SYS_Code_Dict._居住用途);
            this.ViewBag.SJPurposeCodeName = GetDictById(SYS_Code_Dict._居住用途);
            this.ViewBag.FrontName = GetDictById(SYS_Code_Dict._朝向);
            this.ViewBag.SightName = GetDictById(SYS_Code_Dict._景观);
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetDictById(int id)
        {
            var casetype = _dropDownList.GetDictById(id);
            var casetypeResult = new List<SelectListItem>();
            casetype.ToList().ForEach(m =>
                casetypeResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            casetypeResult.Insert(0, new SelectListItem
            {
                Value = "-1",
                Text = "--请选择--",
            });
            return casetypeResult;
        }

        private IndustryHouse BindViewModel(DataTable houses, IQueryable<DatHouseIndustry> unitNos, IQueryable<int> floorNos, int buildingId = 0, int projectId = 0)
        {
            //楼盘均价,楼栋均价,价格系数说明
            string pAveragePrice = string.Empty,
                   bAveragePrice = string.Empty,
                   priceDetail = string.Empty;
            var proWeight = "1.00";//百分比
            var totalFloor = 0;

            if (houses.Rows.Count > 0)
            {
                var lastFloorNo = houses.Rows[houses.Rows.Count - 1]["FloorNo"];
                totalFloor = lastFloorNo == DBNull.Value ? 0 : Convert.ToInt32(lastFloorNo);

                pAveragePrice = houses.Rows[0]["PAvePrice"] == DBNull.Value ? "" : houses.Rows[0]["PAvePrice"].ToString();
                bAveragePrice = houses.Rows[0]["BAvePrice"] == DBNull.Value ? "" : houses.Rows[0]["PAvePrice"].ToString();
                priceDetail = houses.Rows[0]["PriceDetail"] == DBNull.Value ? "" : houses.Rows[0]["PriceDetail"].ToString();

            }
            if (!string.IsNullOrEmpty(pAveragePrice) && !string.IsNullOrEmpty(bAveragePrice))
            {
                proWeight = (Convert.ToDouble(bAveragePrice) / Convert.ToDouble(pAveragePrice) * 100).ToString("F2");
            }

            return new IndustryHouse
            {
                Houses = houses,
                UnitNos = unitNos,
                FloorNos = floorNos,
                ProjectId = projectId,
                ProjectName = _industryHouse.GetProjectName(buildingId, Passport.Current.FxtCompanyId),
                BuildingName = _industryHouse.GetBuildingName(buildingId, Passport.Current.FxtCompanyId),
                TotalFloor = totalFloor,
                BuildingId = buildingId,
                ProjectAvePrice = pAveragePrice,
                BuildingAvePrice = bAveragePrice,
                PriceDetail = priceDetail,
                ProWeight = proWeight,
                FxtCompanyId = Passport.Current.FxtCompanyId
            };
        }

        [NonAction]
        private DatHouseIndustry ConvertValue(IndustryHouseViewModel ofv, bool isdelete = false)
        {
            var datHouse = new DatHouseIndustry();

            if (!isdelete)
            {
                if (!string.IsNullOrEmpty(ofv.PurposeCode))
                {
                    datHouse.PurposeCode = _dropDownList.GetCodeByName(ofv.PurposeCode, SYS_Code_Dict._居住用途);
                }
                if (!string.IsNullOrEmpty(ofv.SJPurposeCode))
                {
                    datHouse.SJPurposeCode = _dropDownList.GetCodeByName(ofv.SJPurposeCode, SYS_Code_Dict._居住用途);
                }
                if (!string.IsNullOrEmpty(ofv.SightCode))
                {
                    datHouse.SightCode = _dropDownList.GetCodeByName(ofv.SightCode, SYS_Code_Dict._景观);
                }
                if (!string.IsNullOrEmpty(ofv.FrontCode))
                {
                    datHouse.FrontCode = _dropDownList.GetCodeByName(ofv.FrontCode, SYS_Code_Dict._朝向);
                }
                if (!string.IsNullOrEmpty(ofv.IsEValue))
                {
                    datHouse.IsEValue = IsEvalue(ofv.IsEValue);
                }
            }

            datHouse.HouseId = ofv.HouseId;
            datHouse.BuildingId = ofv.BuildingId;
            datHouse.ProjectId = ofv.ProjectId;
            datHouse.FloorNo = ofv.FloorNo;
            datHouse.FloorNum = ofv.FloorNum;
            datHouse.UnitNo = ofv.UnitNo;
            datHouse.HouseNo = ofv.HouseNo;
            datHouse.HouseName = ofv.HouseName;
            datHouse.BuildingArea = ofv.BuildingArea;
            datHouse.InnerBuildingArea = ofv.InnerBuildingArea;
            datHouse.UnitPrice = ofv.UnitPrice;
            datHouse.Weight = ofv.Weight;
            datHouse.CityId = Passport.Current.CityId;
            datHouse.FxtCompanyId = ofv.FxtCompanyId;
            datHouse.Valid = 1;

            return datHouse;
        }
        [NonAction]
        private static int IsEvalue(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return name == "是" ? 1 : 0;
            }
            return -1;
        }
        #endregion
    }
}
