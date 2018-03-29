using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;
namespace FXT.DataCenter.WebUI.Areas.Land.Controllers
{
    //[DataCenterAuthorize(SYS_Code_Dict.土地数据分类.土地基础数据)]
    [Authorize]
    public class LandController : BaseController
    {
        public IDAT_Land _land;
        public IDropDownList _dropDownList;
        public IDAT_Company _com;
        public IMenu _menu;
        public ILog _log;
        private readonly IImportTask _importTask;
        public LandController(IDAT_Land land, IDropDownList city, IDAT_Company com, IMenu menu, ILog log, IImportTask importTask)
        {
            this._land = land;
            this._dropDownList = city;
            this._com = com;
            this._menu = menu;
            this._log = log;
            this._importTask = importTask;
            ViewBag.title = "土地基础数据";

        }
        public LandController() { }

        public ActionResult Index(DAT_Land model)
        {
            ViewBag.CityId = Passport.Current.CityId;
            model.fxtcompanyid = Passport.Current.FxtCompanyId;
            model.cityid = Passport.Current.CityId;
            BindDropDow(Passport.Current.CityId, model.areaid ?? -1);
            #region 判断操作权限是查看自己还是查看全部
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View(new Webdiyer.WebControls.Mvc.PagedList<DAT_Land>(new List<DAT_Land>().AsQueryable(), model.pageIndex, model.pageSize, 0));
            }
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion
            int totalCount;
            var result = _land.GetAllLandInfo(model, model.pageIndex, model.pageSize, out totalCount, self).AsQueryable();
            //var landList = result.ToPagedList(model.pageIndex, 30);
            var landList = new PagedList<DAT_Land>(result, model.pageIndex, model.pageSize, totalCount);
            return View(landList);
        }

        //删除
        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }
            try
            {

                #region 判断操作权限是删除自己还是删除全部
                int operate;
                //PermissionLevelCheck.Check(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                var self = true;
                Permission.Check(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                var failList = new List<string>();
                #endregion
                foreach (var item in ids)
                {
                    var array = item.Split('|');
                    if (self)
                    {
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                    }
                    _land.DeleteDAT_Land(int.Parse(array[1]));
                }
                //ids.ForEach(m => { _land.DeleteDAT_Land(int.Parse(m.Split('|')[1])); });
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地基础信息, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除土地基础信息", RequestHelper.GetIP());
                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/Land/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败");
            }
        }

        //编辑
        //[DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Edity(string id)
        {
            var splitArray = id.Split('#');
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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

            DAT_Land land = new DAT_Land();
            BindDropDownList();
            if (int.Parse(splitArray[1]) > 0)
            {
                land = _land.GetAllLandByLandId(int.Parse(splitArray[1]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (land != null)
                {
                    if (!string.IsNullOrEmpty(land.planpurpose))
                    {
                        var array = land.planpurpose.Split(',');
                        if (array != null && array.Count() > 0)
                            BindDropDownList(array.ToList());
                    }
                }
                //Session["landno"] = land.landno;

            }
            BindDropDow(Passport.Current.CityId, land.areaid ?? -1);
            return View(land);
        }

        [NonAction]
        private void BindDropDownList(ICollection<string> purposeDescList = null)
        {
            //规划用途
            this.ViewBag.LandPlanUse = GetLandPlanUse(purposeDescList ?? new List<string>());
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

        [HttpPost]
        public ActionResult Edity(DAT_Land mode)
        {
            mode.landownerid = GetDATCompanyIdByName(mode.LandOwnerName ?? "");
            mode.landuseid = GetDATCompanyIdByName(mode.LandUseName ?? "");
            mode.cityid = Passport.Current.CityId;
            mode.creator = Passport.Current.UserName;
            mode.planpurpose = mode.opValue;
            var temp = "";
            try
            {
                if (mode.Description != null && mode.Description.Any())
                    temp = string.Join(",", mode.Description);
                mode.opValue = temp;
                if (mode.landid == 0)
                {
                    mode.fxtcompanyid = Passport.Current.FxtCompanyId;
                    int reslut = _land.AddDAT_Land(mode);
                    //操作日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地基础信息, SYS_Code_Dict.操作.新增, reslut.ToString(), "", "新增土地基础信息", RequestHelper.GetIP());

                }
                else
                {
                    //if (Session["landno"].ToString() != mode.landno)
                    //{
                    //    if (ValidLandNo(mode.landno.Trim()))
                    //    {
                    //        return this.Back("该宗地号已存在");
                    //    }
                    //}
                    string msg = "";
                    int reslut = _land.UpdateDAT_Land(mode, Passport.Current.FxtCompanyId);
                    //if (reslut > 0) { msg = "更新成功"; return this.CloseThickbox(""); }
                    //else { msg = "更新失败"; return this.Thickbox("更新失败"); }
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地基础信息, SYS_Code_Dict.操作.修改, mode.landid.ToString(), "", "修改土地基础信息," + msg, RequestHelper.GetIP());

                }
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/Land/Edity", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败");
            }
        }

        //exce导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {

            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.土地信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var taskList = result.ToPagedList(pageIndex ?? 1, 30);
            return View(taskList);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {

            string folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId);

            try
            {
                if (file.ContentLength > 0 && file != null)
                {
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    var sResult = saveFile(file, folder, filename);
                    //操作日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地基础信息, SYS_Code_Dict.操作.导入, "", "", "导入土地基础信息", RequestHelper.GetIP());

                    // 异步调用WCF服务
                    var filePath = Path.Combine(folder, filename);
                    var cityId = Passport.Current.CityId;
                    var fxtCompanyId = Passport.Current.FxtCompanyId;
                    var userId = Passport.Current.ID;
                    Task.Factory.StartNew(() =>
                    {
                        var client = new ExcelUploadServices.ExcelUploadClient();
                        client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "LandInfo");
                        try { client.Close(); }
                        catch { client.Abort(); }
                    });

                }
                return RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteLog("Land/Land/ImportLand", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }



        }

        //删除土地导入记录
        public ActionResult DeleteLandImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/Project/DeleteProjectImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        //导出
        public ActionResult ExcelImport(DAT_Land model)
        {
            try
            {
                #region 判断操作权限是查看自己还是查看全部

                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.土地数据分类.土地基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                #endregion
                model.fxtcompanyid = Passport.Current.FxtCompanyId;
                model.cityid = Passport.Current.CityId;
                model.command = self;
                var dt = _land.GetAllLandInfoImport(model, self);

                #region header 信息
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                curContext.Response.AddHeader("content-disposition",
                                                 "attachment;filename*=UTF-8''" +
                                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_土地_土地基础数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                curContext.Response.Charset = "UTF-8";
                #endregion

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地基础信息, SYS_Code_Dict.操作.导出, "", "", "导出土地基础信息", RequestHelper.GetIP());

                using (var ms = ExcelHandle.ListToExcel(dt.ToList()))
                {
                    return new FileContentResult(ms.GetBuffer(), "application/vnd.ms-excel");
                }
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteLog("Land/Land/ExcelImport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent(ex.Message);
            }
        }

        //土地统计：条件:行政区，片区，用途，土地起始时间，土地结束时间，土地面积，土地使用者；统计出符合条件的土地量，面积总和
        public ActionResult Statistical()
        {
            return View();
        }

        public ActionResult BindCity(int cityId)
        {
            var cityList = _dropDownList.GetCityByCityId(cityId);
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "--全市--", Value = "-1" });
            foreach (var item in cityList)
            {
                items.Add(new SelectListItem { Text = item.CityName, Value = item.CityId.ToString() });

            }
            ViewBag.citylist = items;
            return Json(new { data = items });
        }

        public ActionResult BindArea(int cityId)
        {
            var areaList = _dropDownList.GetAreaName(cityId);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--全市--", Value = "-1" });
            foreach (var item in areaList)
            {
                items.Add(new SelectListItem { Text = item.AreaName, Value = item.AreaId.ToString() });

            }
            ViewBag.arealist = items;
            return Json(new { data = items });

        }

        public ActionResult BingSubArean(int areaId = 0)
        {
            var areaList = _dropDownList.GetSubAreaName(areaId);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--请选择--", Value = "-1" });
            foreach (var item in areaList)
            {
                items.Add(new SelectListItem { Text = item.SubAreaName, Value = item.SubAreaId.ToString() });

            }
            ViewBag.subarealist = items;
            return Json(new { data = items });
        }

        public ActionResult GetLandPurpose(int code = SYS_Code_Dict._土地用途)
        {
            var areaList = _dropDownList.GetLandPurpose(SYS_Code_Dict._土地用途);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--请选择--", Value = "" });
            foreach (var item in areaList)
            {
                items.Add(new SelectListItem { Text = item.CodeName, Value = item.CodeName });

            }
            ViewBag.landcodelist = items;
            return Json(new { data = items });
        }

        [HttpPost]
        public ActionResult BindCompan(string companypecode)
        {
            var companyList = _dropDownList.GetCompanyName(Passport.Current.CityId, 0);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--请选择--", Value = "-1" });
            foreach (var item in companyList)
            {
                items.Add(new SelectListItem { Text = item.ChineseName, Value = item.companyid.ToString() });

            }
            ViewBag.companyList = items;
            return Json(new { list = companyList });
        }

        [HttpPost]
        public ActionResult ValidLandNo(string landNo)
        {
            bool flg = _land.ValidLandNo(Passport.Current.CityId, Passport.Current.FxtCompanyId, landNo);
            return Json(flg);
        }

        [NonAction]
        private void BindDropDow(int cityId, int areaId)
        {
            BindCity(cityId);
            BindArea(Passport.Current.CityId);
            BingSubArean(areaId);
            GetLandPurpose();
            BindCompan("");

        }

        [NonAction]
        private int GetDATCompanyIdByName(string ChineseName)
        {
            int datCompandId = 0;
            if (!string.IsNullOrEmpty(ChineseName))
            {
                DAT_Company company = _com.GetDAT_CompanyInfo(ChineseName);
                if (company != null)
                {
                    datCompandId = company.CompanyId;
                }
                else
                {
                    //UserCheck u = UserBase.CurrentUser;
                    //int newId = _com.AddDAT_Compandy(ChineseName, u.producttypecode, u.cityid);
                    var newId = _com.AddDAT_Compandy(ChineseName, Passport.Current.ProductTypeCode, Passport.Current.CityId.ToString(), Passport.Current.FxtCompanyId);
                    datCompandId = newId;
                }
            }
            return datCompandId;
        }
        [NonAction]
        public List<DAT_Land> ConvertExcelToList(DataTable dt)
        {
            List<DAT_Land> list = new List<DAT_Land>();

            //UserCheck user=UserBase.CurrentUser;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DAT_Land la = new DAT_Land();
                la.fxtcompanyid = Passport.Current.FxtCompanyId;
                la.cityid = Passport.Current.CityId;
                la.areaid = GetAreaId(dt.Rows[i][0].ToString());
                la.subareaid = GetSubAreaId(dt.Rows[i][1].ToString());
                la.landno = dt.Rows[i][2] == null ? "" : dt.Rows[i][2].ToString();
                la.fieldno = dt.Rows[i][3] == null ? "" : dt.Rows[i][3].ToString();
                la.mapno = dt.Rows[i][4] == null ? "" : dt.Rows[i][4].ToString();
                la.landname = dt.Rows[i][5] == null ? "" : dt.Rows[i][5].ToString();
                la.address = dt.Rows[i][6] == null ? "" : dt.Rows[i][6].ToString();
                la.landtypecode = dt.Rows[i][7] == null ? 0 : Sys_TypeCodeOrName.GetLandTypeCode(dt.Rows[i][7].ToString());
                la.usetypecode = dt.Rows[i][8] == null ? 0 : Sys_TypeCodeOrName.GetUseTypeCode(dt.Rows[i][8].ToString());
                la.startdate = dt.Rows[i][9].ToString() == "" ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dt.Rows[i][9]);
                la.enddate = dt.Rows[i][10].ToString() == "" ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dt.Rows[i][10]);
                la.useyear = dt.Rows[i][11].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[i][11]);
                la.planpurpose = dt.Rows[i][12].ToString() == "" ? "" : Sys_TypeCodeOrName.GetPurposeCode(dt.Rows[i][12].ToString()).ToString();
                la.factpurpose = dt.Rows[i][13].ToString() == "" ? "" : Sys_TypeCodeOrName.GetPurposeCode(dt.Rows[i][13].ToString()).ToString();
                la.landarea = dt.Rows[i][14].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][14]);
                la.buildingarea = dt.Rows[i][15].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][15]);
                la.cubagerate = dt.Rows[i][16].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][16]);
                la.maxcubagerate = dt.Rows[i][17].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][17]);
                la.mincubagerate = dt.Rows[i][18].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][18]);
                la.coverage = dt.Rows[i][19].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][19]);
                la.maxcoverage = dt.Rows[i][20].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][20]);
                la.greenrage = dt.Rows[i][21].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][21]);
                la.mingreenrage = dt.Rows[i][22].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][22]);
                la.landshapecode = dt.Rows[i][23].ToString() == "" ? 0 : Sys_TypeCodeOrName.GetLandShapeCode(dt.Rows[i][23].ToString());
                la.developmentcode = dt.Rows[i][24].ToString() == "" ? 0 : Sys_TypeCodeOrName.GetDevelopmentCode(dt.Rows[i][24].ToString());
                la.landusestatus = dt.Rows[i][25].ToString() == "" ? "" : dt.Rows[i][25].ToString();
                la.landclass = dt.Rows[i][26].ToString() == "" ? 0 : Sys_TypeCodeOrName.GetLandClassCode(dt.Rows[i][26].ToString());
                la.heightlimited = dt.Rows[i][27].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[i][27]);
                la.planlimited = dt.Rows[i][28].ToString() == "" ? "" : dt.Rows[i][28].ToString();
                la.east = dt.Rows[i][29].ToString() == "" ? "" : dt.Rows[i][29].ToString();
                la.west = dt.Rows[i][30].ToString() == "" ? "" : dt.Rows[i][30].ToString();
                la.south = dt.Rows[i][31].ToString() == "" ? "" : dt.Rows[i][31].ToString();
                la.north = dt.Rows[i][32].ToString() == "" ? "" : dt.Rows[i][32].ToString();
                if (dt.Rows[i][33] != null && dt.Rows[i][33].ToString() != "")
                {
                    if (_com.GetDAT_CompanyInfo(dt.Rows[i][33].ToString()) == null)
                    {
                        la.landownerid = _com.AddDAT_Compandy(dt.Rows[i][33].ToString(), Passport.Current.ProductTypeCode, Passport.Current.CityId.ToString(), Passport.Current.FxtCompanyId);
                    }
                    else
                    {
                        la.landownerid = _com.GetDAT_CompanyInfo(dt.Rows[i][33].ToString()).CompanyId;
                    }
                }
                else
                {
                    la.landownerid = 0;
                }
                if (dt.Rows[i][34] != null && dt.Rows[i][34].ToString() != "")
                {
                    if (_com.GetDAT_CompanyInfo(dt.Rows[i][34].ToString()) == null)
                    {
                        la.landuseid = _com.AddDAT_Compandy(dt.Rows[i][34].ToString(), Passport.Current.ProductTypeCode, Passport.Current.CityId.ToString(), Passport.Current.FxtCompanyId);
                    }
                    else
                    {
                        la.landuseid = _com.GetDAT_CompanyInfo(dt.Rows[i][34].ToString()).CompanyId;
                    }
                }
                else
                {
                    la.landuseid = 0;
                }
                la.businesscenterdistance = dt.Rows[i][35].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][35]);
                la.traffic = dt.Rows[i][36].ToString() == "" ? "" : dt.Rows[i][36].ToString();
                la.infrastructure = dt.Rows[i][37].ToString() == "" ? "" : dt.Rows[i][37].ToString();
                la.publicservice = dt.Rows[i][38].ToString() == "" ? "" : dt.Rows[i][38].ToString();
                la.environmentcode = dt.Rows[i][39].ToString() == "" ? 0 : Sys_TypeCodeOrName.GetEnvironmentCode(dt.Rows[i][39].ToString());
                la.licencedate = dt.Rows[i][40].ToString() == "" ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dt.Rows[i][40]);
                la.landdetail = dt.Rows[i][41].ToString() == "" ? "" : dt.Rows[i][41].ToString();
                la.weight = dt.Rows[i][42].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][42]);
                la.coefficient = dt.Rows[i][43].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][43]);
                la.x = dt.Rows[i][44].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][44]);
                la.y = dt.Rows[i][45].ToString() == "" ? 0 : Convert.ToDecimal(dt.Rows[i][45]);
                la.xyscale = dt.Rows[i][46].ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[i][46]);
                //la.createdate = dt.Rows[i][47] != null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dt.Rows[i][47]);
                //la.creator = dt.Rows[i][48] .ToString() == "" ? "" : dt.Rows[i][48].ToString();
                //la.savedate = dt.Rows[i][49] .ToString() == "" ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dt.Rows[i][49]);
                //la.saveuser = dt.Rows[i][50] .ToString() == "" ? "" : dt.Rows[i][50].ToString();
                la.remark = dt.Rows[i][47].ToString() == "" ? "" : dt.Rows[i][47].ToString();
                //la.valid = dt.Rows[i][52] .ToString() == "" ? 0 : Convert.ToInt32(dt.Rows[i][52]);
                list.Add(la);
            }
            return list;
        }
        [NonAction]
        private int GetCodeByName(int id, string name)
        {
            var syscode = _dropDownList.GetDictById(id).Where(m => m.CodeName == name.Trim()).FirstOrDefault();
            return syscode == null ? -1 : syscode.Code;
        }
        [NonAction]
        public string GetCodeNameByCode(int id, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                if (code != "-1")
                {
                    IDropDownList list = new DropDownList();
                    if (code.Contains(","))
                    {
                        string codename = "";
                        string[] str = code.Split(',');
                        for (int i = 0; i < str.Length; i++)
                        {
                            codename += list.GetDictById(id).Where(m => m.Code == Convert.ToInt32(str[i])).FirstOrDefault().CodeName + ",";
                        }
                        return codename.Substring(0, codename.Length - 1);
                    }
                    else
                    {
                        var syscodeName = list.GetDictById(id).Where(m => m.Code == Convert.ToInt32(code)).FirstOrDefault();
                        return syscodeName == null ? "" : syscodeName.CodeName;
                    }
                }
            }
            return "";

        }

        [NonAction]
        private int GetAreaId(string areaName)
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId).Where(m => m.AreaName == areaName.Trim()).FirstOrDefault();
            return area == null ? -1 : area.AreaId;
        }

        [NonAction]
        private int GetSubAreaId(string subAreaName)
        {
            var area = _dropDownList.GetSubAreaName(Passport.Current.CityId).Where(m => m.SubAreaName == subAreaName.Trim()).FirstOrDefault();
            return area == null ? -1 : area.SubAreaId;
        }
        [NonAction]
        private bool saveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            try
            {
                postedFile.SaveAs(Path.Combine(filepath, saveName));
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }
    }
}
