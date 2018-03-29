using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Data;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    /// <summary>
    /// 房号系数差设置
    /// </summary>
    [Authorize]
    public class HouseRatioController : BaseController
    {
        private readonly IImportTask _importTask;
        private readonly IFloorPrice _floorPrice;
        private readonly ICodePrice _codePrice;
        private readonly IDAT_House _dat_house;
        private readonly ILog _log;

        public HouseRatioController(IImportTask importTask, IFloorPrice floorPrice, ICodePrice codePrice, IDAT_House dat_house, ILog log)
        {
            _importTask = importTask;
            _floorPrice = floorPrice;
            _codePrice = codePrice;
            _dat_house = dat_house;
            _log = log;
        }

        public ActionResult Index(int? pageIndex, int? fpPageIndex)
        {
            ViewBag.IsExport = "admin@" + (Passport.Current.UserName.Split('@')[1]) == Passport.Current.UserName ? true : false;
            if (fpPageIndex.HasValue)
            {
                string index = TempData["fpPageIndex"] == null ? null : TempData["fpPageIndex"].ToString();
                if (!string.IsNullOrEmpty(index))
                {
                    if (index != fpPageIndex.Value.ToString())
                    {
                        ViewBag.fpPageIndex = fpPageIndex;
                    }
                }
                TempData["fpPageIndex"] = fpPageIndex;
            }
            else
            {
                TempData["fpPageIndex"] = null;
            }

            int cid = Passport.Current.CityId;
            int fid = Passport.Current.FxtCompanyId;
            //导入任务
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.住宅房号系数差, cid, fid);
            var projectResult = result.ToPagedList(pageIndex ?? 1, 30);
            //楼层差
            ViewBag.FloorPrices = _floorPrice.FindAll(cid, fid).ToPagedList(fpPageIndex ?? 1, 30);
            //朝向
            ViewBag.FrontCode = _codePrice.FindAllByTypeCode(1033001, cid, fid);
            //景观
            ViewBag.UnitPrice = _codePrice.FindAllByTypeCode(1033002, cid, fid);
            //通风采光
            ViewBag.VDCode = _codePrice.FindAllByTypeCode(1033006, cid, fid);
            //装修
            ViewBag.FitmentCode = _codePrice.FindAllByTypeCode(1033004, cid, fid);
            //面积段
            ViewBag.BuildingAreaCode = _codePrice.FindAllByTypeCode(1033005, cid, fid);

            return View(projectResult);
        }

        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult ImportHouseRatio(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/Excel/HouseRatio/UploadFile/" + Passport.Current.FxtCompanyId.ToString());
            if (null != file)
            {
                try
                {
                    //获得文件名
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    SaveFile(file, folder, filename);

                    // 异步调用WCF服务
                    var filePath = Path.Combine(folder, filename);
                    var cityId = Passport.Current.CityId;
                    var fxtCompanyId = Passport.Current.FxtCompanyId;
                    var userId = Passport.Current.ID;
                    Task.Factory.StartNew(() =>
                    {
                        var client = new ExcelUploadServices.ExcelUploadClient();
                        client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                            taskNameHiddenValue, "HouseRatio");
                        try { client.Close(); }
                        catch { client.Abort(); }

                    });

                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/HouseRatio/ImportHouseRatio", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                    return base.Back("操作失败！");
                }
            }
            return RedirectToAction("Index");
        }

        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出全部)]
        public ActionResult ExportHouseRatio()
        {
            try
            {
                //获取导出数据
                int cityid = Passport.Current.CityId;
                int fxtcompanyid = Passport.Current.FxtCompanyId;
                DataTable table = _floorPrice.ExportFloorCode(cityid, fxtcompanyid);
                table.TableName = "楼层差修正汇总";
                DataTable table1 = _codePrice.ExportFrontCode(1033001, cityid, fxtcompanyid);
                table1.TableName = "朝向修正系数";
                DataTable table2 = _codePrice.ExportSightCode(1033002, cityid, fxtcompanyid);
                table2.TableName = "景观修正系数";
                DataTable table3 = _codePrice.ExportVDCode(1033006, cityid, fxtcompanyid);
                table3.TableName = "通风采光修正系数";
                DataTable table4 = _codePrice.ExportFitmentCode(1033004, cityid, fxtcompanyid);
                table4.TableName = "装修修正系数";
                DataTable table5 = _codePrice.ExportBuildingAreaCode(1033005, cityid, fxtcompanyid);
                table5.TableName = "面积段修正系数";
                DataSet ds = new DataSet();
                ds.Tables.Add(table.Copy());
                ds.Tables.Add(table1.Copy());
                ds.Tables.Add(table2.Copy());
                ds.Tables.Add(table3.Copy());
                ds.Tables.Add(table4.Copy());
                ds.Tables.Add(table5.Copy());

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.导出, "", "", "导出房号系数", RequestHelper.GetIP());

                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                curContext.Response.AddHeader("content-disposition",
                                                 "attachment;filename*=UTF-8''" +
                                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_房号系数差_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                curContext.Response.Charset = "UTF-8";

                MemoryStream ms = ExcelHandle.RenderToExcel(ds);
                return new FileContentResult(ms.GetBuffer(), "application/vnd.ms-excel");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/HouseRatio/ExportHouseRatio", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        public ActionResult SetHouseRatio(int key)
        {
            try
            {
                int sussnum; int count;
                _dat_house.SetHouseRatio(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.UserName, key, 0, "", out sussnum, out count);
                return Json(new { result = false, msg = "成功设置" + sussnum + "条房号系数！" + ((count - sussnum) > 0 ? "有" + (count - sussnum) + "条房号系数没有更新，可能原因是该房号的楼层差系数为空。" : "") });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/HouseRatio/SetHouseRatio", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "设置失败！" });
            }
        }

        //删除房号系数导入记录
        public ActionResult DeleteHouseRatioImportRecord(List<string> ids)
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
                LogHelper.WriteLog("House/HouseRatio/DeleteHouseRatioImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
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

        [HttpPost]
        public ActionResult DeletePrice(List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return Json(new { result = false, msg = "删除失败！" });
                }
                var count = 0;
                if (ids.Contains("楼层差修正汇总"))
                {
                    count += _floorPrice.DeleteFloorPrice(Passport.Current.CityId, Passport.Current.FxtCompanyId);
                }
                if (ids.Contains("朝向修正系数"))
                {
                    count += _codePrice.DeleteCodePrice(Passport.Current.CityId, Passport.Current.FxtCompanyId, 1033001);
                }
                if (ids.Contains("景观修正系数"))
                {
                    count += _codePrice.DeleteCodePrice(Passport.Current.CityId, Passport.Current.FxtCompanyId, 1033002);
                }
                if (ids.Contains("通风采光修正系数"))
                {
                    count += _codePrice.DeleteCodePrice(Passport.Current.CityId, Passport.Current.FxtCompanyId, 1033006);
                }
                if (ids.Contains("装修修正系数"))
                {
                    count += _codePrice.DeleteCodePrice(Passport.Current.CityId, Passport.Current.FxtCompanyId, 1033004);
                }
                if (ids.Contains("面积段修正系数"))
                {
                    count += _codePrice.DeleteCodePrice(Passport.Current.CityId, Passport.Current.FxtCompanyId, 1033005);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.删除, "", "", "删除房号修正系数表", RequestHelper.GetIP());
                return Json(new { result = true, msg = "已删除" + count + "条数据。" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/HouseRatio/DeleteHouseRatioImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
    }
}
