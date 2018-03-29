using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Land.Controllers
{
    //[DataCenterAuthorize(SYS_Code_Dict.土地数据分类.土地基准地价)]
    [Authorize]
    public class LandBasePriceController : BaseController
    {
        private readonly IDAT_Land_BasePrice la;
        private readonly IDropDownList _dropDownList;
        private readonly IMenu _menu;
        private readonly IImportTask _importTask;
        private readonly ILog _log;
        public LandBasePriceController(IDAT_Land_BasePrice land, IDropDownList drop, IMenu menu, ILog log, IImportTask importTaks)
        {
            this.la = land;
            this._dropDownList = drop;
            this._menu = menu;
            this._log = log;
            this._importTask = importTaks;
            ViewBag.title = "土地基准地价";
        }

        public ActionResult Index(DAT_Land_BasePrice mode)
        {
            mode.fxtcompanyid = Passport.Current.FxtCompanyId;
            mode.cityid = Passport.Current.CityId;
            this.BindViewData(mode);
            #region 判断操作权限是查看自己还是查看全部
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地基准地价, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View(new Webdiyer.WebControls.Mvc.PagedList<DAT_Land_BasePrice>(new List<DAT_Land_BasePrice>().AsQueryable(), mode.pageIndex, mode.pageSize, 0));
            }
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion
            int totalCount;
            var result = la.GetLand_BasePrice(mode, mode.pageIndex, mode.pageSize, out totalCount, self).AsQueryable();
            var landList = new Webdiyer.WebControls.Mvc.PagedList<DAT_Land_BasePrice>(result, mode.pageIndex, mode.pageSize, totalCount);
            return View(landList);
        }

        public ActionResult Edity(string id)
        {
            var splitArray = id.Split('#');
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.土地数据分类.土地基准地价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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
            DAT_Land_BasePrice obj = new DAT_Land_BasePrice();
            BindViewData(); GetLandUse();
            ViewBag.cityId = Passport.Current.CityId;
            if (int.Parse(splitArray[1]) > 0)
            {
                obj = la.GetAllLand_BasePriceByLandId(int.Parse(splitArray[1]), Passport.Current.CityId, Passport.Current.FxtCompanyId);
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Edity(DAT_Land_BasePrice obj)
        {
            try
            {
                if (obj.id == 0)
                {
                    obj.fxtcompanyid = Passport.Current.FxtCompanyId;
                    int reslut = la.AddLand_BasePrice(obj);
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID,
                   SYS_Code_Dict.功能模块.土地基准地价, SYS_Code_Dict.操作.新增, reslut.ToString(), "", "新增土地基准地价,", RequestHelper.GetIP());

                }
                else
                {
                    int reslut = la.UpdateLand_BasePrice(obj);
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID,
                  SYS_Code_Dict.功能模块.土地基准地价, SYS_Code_Dict.操作.修改, obj.id.ToString(), "", "修改土地基准地价", RequestHelper.GetIP());

                }
                return this.RefreshParent();
            }
            catch (Exception ex)
            {

                //异常日志
                LogHelper.WriteLog("Land/LandBasePrice/Edity", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败");
            }
        }

        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "删除失败！" });
            }

            DAT_Land_BasePrice mode = new DAT_Land_BasePrice();
            mode.cityid = Passport.Current.CityId;
            mode.fxtcompanyid = Passport.Current.FxtCompanyId;
            try
            {
                #region 判断操作权限是删除自己还是删除全部
                int operate;
                var self = true;
                Permission.Check(SYS_Code_Dict.土地数据分类.土地基准地价, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
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
                    la.DeleteLand_BasePrice(int.Parse(array[1]));
                }
                _log.InsertLog(
                          Passport.Current.CityId,
                          Passport.Current.FxtCompanyId,
                          Passport.Current.ID,
                          Passport.Current.ID,
                          SYS_Code_Dict.功能模块.土地基准地价,
                          SYS_Code_Dict.操作.删除,
                          string.Join(",", ids),
                          "",
                          "删除土地基准地价",
                          RequestHelper.GetIP());
                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/LandBasePrice/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败");
            }

        }

        //exce导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.土地数据分类.土地基准地价, SYS_Code_Dict.页面权限.导入)]
        public ActionResult ImportLand()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.土地信息, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            var taskList = result.ToPagedList(1, 30);
            return View(taskList);
        }

        [HttpPost]
        public ActionResult ImportLand(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId.ToString());
            if (null == file) return this.RedirectToAction("ImportLand");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地基准地价, SYS_Code_Dict.操作.导入, "", "", "导入土地基准地价", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "LandPrice");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });

                return this.RedirectToAction("ImportLand");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Land/LandBasePrice/ImportLand", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //导出
        public ActionResult ExcelImport(DAT_Land_BasePrice model)
        {
            try
            {
                model.fxtcompanyid = Passport.Current.FxtCompanyId;
                model.cityid = Passport.Current.CityId;
                #region 判断操作权限是查看自己还是查看全部

                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.土地数据分类.土地基准地价, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有导出权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                #endregion
                var dt = la.GetLand_BasePriceExeclImport(model, self);
                #region header 信息
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                curContext.Response.AddHeader("content-disposition",
                                                 "attachment;filename*=UTF-8''" +
                                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_土地_基准地价_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xls");
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                curContext.Response.Charset = "UTF-8";
                #endregion

                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.土地基准地价, SYS_Code_Dict.操作.导出, "", "", "导出土地基础信息", RequestHelper.GetIP());

                using (var ms = ExcelHandle.ListToExcel(dt.ToList()))
                {
                    return new FileContentResult(ms.GetBuffer(), "application/vnd.ms-excel");
                }
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteLog("Land/LandBasePrice/ExcelImport", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败");
            }
        }

        [NonAction]
        private bool SaveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
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

        [NonAction]
        private List<DAT_Land_BasePrice> ConvertExcelToList(DataTable dt)
        {
            List<DAT_Land_BasePrice> list = new List<DAT_Land_BasePrice>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DAT_Land_BasePrice la = new DAT_Land_BasePrice();
                //此处要改begin
                la.fxtcompanyid = Passport.Current.FxtCompanyId;
                la.cityid = Passport.Current.CityId;
                //此处要改end
                la.purposecode = dt.Rows[i][0].ToString() == "" ? -1 : GetCodeByName(SYS_Code_Dict._土地用途, dt.Rows[i][0].ToString());
                la.landclass = dt.Rows[i][1].ToString() == "" ? -1 : Sys_TypeCodeOrName.GetLandClassCode(dt.Rows[i][1].ToString());
                var landunitprice_avg = dt.Rows[i][2].ToString();
                if (!string.IsNullOrEmpty(landunitprice_avg) && TryParseHelper.StrToDecimal(landunitprice_avg) != null)
                {
                    la.landunitprice_avg = (decimal?)TryParseHelper.StrToDecimal(landunitprice_avg);
                }
                var landunitprice_min = dt.Rows[i][3].ToString();
                if (!string.IsNullOrEmpty(landunitprice_min) && TryParseHelper.StrToDecimal(landunitprice_min) != null)
                {
                    la.landunitprice_min = (decimal?)TryParseHelper.StrToDecimal(landunitprice_min);
                }
                var landunitprice_max = dt.Rows[i][4].ToString();
                if (!string.IsNullOrEmpty(landunitprice_max) && TryParseHelper.StrToDecimal(landunitprice_max) != null)
                {
                    la.landunitprice_max = (decimal?)TryParseHelper.StrToDecimal(landunitprice_max);
                }
                var buildingunitprice_avg = dt.Rows[i][5].ToString();
                if (!string.IsNullOrEmpty(buildingunitprice_avg) && TryParseHelper.StrToDecimal(buildingunitprice_avg) != null)
                {
                    la.buildingunitprice_avg = (decimal?)TryParseHelper.StrToDecimal(buildingunitprice_avg);
                }
                var buildingunitprice_min = dt.Rows[i][6].ToString();
                if (!string.IsNullOrEmpty(buildingunitprice_min) && TryParseHelper.StrToDecimal(buildingunitprice_min) != null)
                {
                    la.buildingunitprice_min = (decimal?)TryParseHelper.StrToDecimal(buildingunitprice_min);
                }
                var buildingunitprice_max = dt.Rows[i][7].ToString();
                if (!string.IsNullOrEmpty(buildingunitprice_max) && TryParseHelper.StrToDecimal(buildingunitprice_max) != null)
                {
                    la.buildingunitprice_max = (decimal?)TryParseHelper.StrToDecimal(buildingunitprice_max);
                }
                var pricedate = dt.Rows[i][8].ToString();
                if (!string.IsNullOrEmpty(pricedate) && TryParseHelper.StrToDateTime(pricedate) != null)
                {
                    la.pricedate = (DateTime?)TryParseHelper.StrToDateTime(pricedate);
                }
                else
                {
                    la.pricedate = (DateTime)SqlDateTime.MinValue;
                }
                //la.valid = dt.Rows[i][9] == null ? 0 : Convert.ToInt32(dt.Rows[i][9]);
                list.Add(la);
            }
            return list;
        }

        [NonAction]
        private void BindViewData()
        {
            ViewData.Add("cityid", new SelectList(GetCityNameName(), "Value", "Text", Passport.Current.CityId));
            ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text", Passport.Current.CityId));
        }
        [NonAction]
        private void BindViewData(DAT_Land_BasePrice mode)
        {
            ViewData.Add("cityid", new SelectList(GetCityNameName(), "Value", "Text", Passport.Current.CityId));
            ViewData.Add("areaid", new SelectList(GetAreaName(), "Value", "Text", Passport.Current.CityId));
            var selectedValues = new List<string>();
            if (!string.IsNullOrWhiteSpace(mode.opValue))
            {
                selectedValues = mode.opValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Cast<string>().ToList();
            }
            ViewData.Add("purposecode", new MultiSelectList(GetLandUse(), "Value", "Text", selectedValues));
        }

        [NonAction]
        private int GetCodeByName(int id, string name)
        {
            var syscode = _dropDownList.GetDictById(id).FirstOrDefault(m => m.CodeName == name.Trim());
            return syscode == null ? -1 : syscode.Code;
        }

        private List<SelectListItem> GetAreaName()
        {
            var city = _dropDownList.GetAreaName(Passport.Current.CityId);
            var cityResult = new List<SelectListItem>();
            city.ToList().ForEach(m =>
                cityResult.Add(
                new SelectListItem { Value = m.AreaId.ToString(), Text = m.AreaName }
                ));
            cityResult.Insert(0, new SelectListItem { Value = "-1", Text = "全部" });
            ViewBag.arealist = cityResult;
            return cityResult;
        }
        //城市列表
        [NonAction]
        private List<SelectListItem> GetCityNameName()
        {
            var city = _dropDownList.GetCityName(0, Passport.Current.CityId);
            var cityResult = new List<SelectListItem>();
            city.ToList().ForEach(m =>
                cityResult.Add(
                new SelectListItem { Value = m.cityid.ToString(), Text = m.CityName }
                ));
            //cityResult.Insert(0, new SelectListItem { Value = "-1", Text = "全省" });
            ViewBag.citylist = cityResult;
            return cityResult;
        }
        //土地用途
        [NonAction]
        private List<SelectListItem> GetLandUse()
        {
            var landUse = _dropDownList.GetLandPurpose(1001);
            var landUseResult = new List<SelectListItem>();
            landUse.ToList().ForEach(m =>
                landUseResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            landUseResult.Insert(0, new SelectListItem { Value = "-1", Text = "全部" });
            ViewBag.landcodelist = landUseResult;
            return landUseResult;
        }
    }
}
