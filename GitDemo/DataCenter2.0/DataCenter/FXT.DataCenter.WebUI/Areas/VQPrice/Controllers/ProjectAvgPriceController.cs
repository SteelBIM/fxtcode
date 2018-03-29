using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.Infrastructure.IoC.Binder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.VQPrice.Controllers
{
    [Authorize]
    public class ProjectAvgPriceController : BaseController
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;
        private static readonly IProjectAvgPriceServices Services = new StandardKernel(new ProjectAvgPriceModule()).Get<IProjectAvgPriceServices>();

        public ActionResult Index(int? pageIndex, string projectName, string useMonthStr, int IsPrices = -1)
        {
            int totalCount, pageSize = 30, currIndex = pageIndex ?? 1;
            //判断权限
            int operate;
            Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View(new List<Dat_ProjectAvg>().ToPagedList(pageIndex ?? 0, pageSize));

            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }

            DateTime useMonth = string.IsNullOrWhiteSpace(useMonthStr) ? Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")) : Convert.ToDateTime(useMonthStr);
            //VQ模块维护，使用房讯通角色共享数据。
            //var datProjectAvg = new Dat_ProjectAvg { ProjectName = projectName, CityID = Passport.Current.CityId, FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), UseMonth = useMonth, IsPrices = IsPrices };
            var data = Services.GetProjectAvgPrices(projectName, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, useMonth, IsPrices, currIndex, pageSize, out totalCount);
            var model = new PagedList<Dat_ProjectAvg>(data, currIndex, pageSize, totalCount);
            return View(model);
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(int projectId, string UseMonthStr)
        {
            var result = new Dat_ProjectAvg();
            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }

            DateTime UseMonth = string.IsNullOrWhiteSpace(UseMonthStr) || UseMonthStr == "0001-01-01" ? Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")) : Convert.ToDateTime(UseMonthStr);
            //VQ模块维护，使用房讯通角色共享数据。
            result = Services.GetProjectHistoryAvgPriceByProjectid(projectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, UseMonth);

            var resultBe = Services.GetProjectHistoryAvgPriceByProjectid(projectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, UseMonth.AddMonths(-1));
            result.ProjectAvgPriceBe = resultBe.ProjectAvgPrice;

            if (string.IsNullOrWhiteSpace(UseMonthStr))
            {
                Dat_ProjectAvg model = new Dat_ProjectAvg()
                {
                    AreaId = result.AreaId,
                    AreaName = result.AreaName,
                    CityID = result.CityID,
                    FxtCompanyId = result.FxtCompanyId,
                    ProjectId = result.ProjectId,
                    ProjectName = result.ProjectName,
                    SubAreaId = result.SubAreaId
                };
                return View(model);
            }
            else
            {
                return View(result);
            }
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(Dat_ProjectAvg datProjectAvg)
        {
            var result = new Dat_ProjectAvg();
            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return this.RefreshParent();
            }
            try
            {
                datProjectAvg.ProjectGained = datProjectAvg.ProjectGained != null && datProjectAvg.ProjectGained != 0 ? datProjectAvg.ProjectGained / 100 : null;
                if (datProjectAvg.ProjectGained == null)
                {
                    var proHistoryAvgBe = Services.GetProjectHistoryAvgPriceByProjectid((int)datProjectAvg.ProjectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, datProjectAvg.UseMonth.AddMonths(-1));
                    if (proHistoryAvgBe != null && proHistoryAvgBe.projectavgid > 0)
                    {
                        decimal p = datProjectAvg.ProjectAvgPrice == null ? 0 : (decimal)datProjectAvg.ProjectAvgPrice;
                        decimal pb = proHistoryAvgBe.ProjectAvgPrice == null ? 0 : (decimal)proHistoryAvgBe.ProjectAvgPrice;
                        datProjectAvg.ProjectGained = datProjectAvg.ProjectAvgPrice == null || proHistoryAvgBe.ProjectAvgPrice == null ? null : (decimal?)Math.Round((p - pb) / pb, 4);
                    }
                }

                //History表里先更新一次
                var proHistoryAvg = Services.GetProjectHistoryAvgPriceByProjectid((int)datProjectAvg.ProjectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, datProjectAvg.UseMonth);
                if (proHistoryAvg != null && proHistoryAvg.projectavgid > 0)
                {
                    //判断有没有修改权限
                    int operate;
                    Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                    if (operate == (int)PermissionLevel.None)
                    {
                        return base.Back("对不起，您没有修改权限！");
                    }

                    datProjectAvg.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                    datProjectAvg.UpdateDate = DateTime.Now;
                    datProjectAvg.UpdateUser = Passport.Current.UserName;
                    datProjectAvg.isevalue = 1;
                    Services.UpdateProjectHistoryAvg(datProjectAvg);
                    Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅案例均价, SYS_Code_Dict.操作.修改, "", "", "修改住宅案例历史均价", RequestHelper.GetIP());
                }
                else
                {
                    //判断有没有新增权限
                    int operate;
                    Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.新增, SYS_Code_Dict.页面权限.新增, out operate);
                    if (operate == (int)PermissionLevel.None)
                    {
                        return base.Back("对不起，您没有新增权限！");
                    }

                    datProjectAvg.CityID = Passport.Current.CityId;
                    datProjectAvg.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                    datProjectAvg.CreateDate = DateTime.Now;
                    datProjectAvg.UpdateDate = DateTime.Now;
                    datProjectAvg.UpdateUser = Passport.Current.UserName;
                    datProjectAvg.casemaxprice = 0;
                    datProjectAvg.caseminprice = 0;
                    datProjectAvg.casecount = 0;
                    datProjectAvg.isevalue = 1;
                    Services.AddProjectHistoryAvg(datProjectAvg);
                    Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅案例均价, SYS_Code_Dict.操作.新增, "", "", "新增住宅案例历史均价", RequestHelper.GetIP());
                }

                //更新即时表
                if (datProjectAvg.UseMonth == Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")))
                {
                    var proAvg = Services.GetProjectAvgPriceByProjectid((int)datProjectAvg.ProjectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, datProjectAvg.UseMonth);
                    if (proAvg != null && proAvg.projectavgid > 0)
                    {
                        //判断有没有修改权限
                        int operate;
                        Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                        if (operate == (int)PermissionLevel.None)
                        {
                            return base.Back("对不起，您没有修改权限！");
                        }

                        datProjectAvg.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        datProjectAvg.UpdateDate = DateTime.Now;
                        datProjectAvg.UpdateUser = Passport.Current.UserName;
                        datProjectAvg.isevalue = 1;
                        int r = Services.UpdateProjectAvg(datProjectAvg);
                        //操作日志
                        Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅案例均价, SYS_Code_Dict.操作.修改, "", "", "修改住宅案例均价", RequestHelper.GetIP());
                    }
                    else
                    {
                        //判断有没有新增权限
                        int operate;
                        Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.新增, SYS_Code_Dict.页面权限.新增, out operate);
                        if (operate == (int)PermissionLevel.None)
                        {
                            return base.Back("对不起，您没有新增权限！");
                        }

                        //VQ模块维护，使用房讯通角色共享数据。
                        datProjectAvg.CityID = Passport.Current.CityId;
                        datProjectAvg.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        datProjectAvg.CreateDate = DateTime.Now;
                        datProjectAvg.UpdateDate = DateTime.Now;
                        datProjectAvg.UpdateUser = Passport.Current.UserName;
                        datProjectAvg.casemaxprice = 0;
                        datProjectAvg.caseminprice = 0;
                        datProjectAvg.casecount = 0;
                        datProjectAvg.isevalue = 1;
                        int r = Services.AddProjectAvg(datProjectAvg);
                        //操作日志
                        Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅案例均价, SYS_Code_Dict.操作.新增, "", "", "新增住宅案例均价", RequestHelper.GetIP());
                    }
                }

                //修改均价后，更新后一个月的涨跌幅
                var proHistoryAvgAf = Services.GetProjectHistoryAvgPriceByProjectid((int)datProjectAvg.ProjectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, datProjectAvg.UseMonth.AddMonths(1));
                if (proHistoryAvgAf != null && proHistoryAvgAf.projectavgid > 0)
                {
                    decimal p = datProjectAvg.ProjectAvgPrice == null ? 0 : (decimal)datProjectAvg.ProjectAvgPrice;
                    decimal pa = proHistoryAvgAf.ProjectAvgPrice == null ? 0 : (decimal)proHistoryAvgAf.ProjectAvgPrice;
                    proHistoryAvgAf.ProjectGained = datProjectAvg.ProjectAvgPrice == null || proHistoryAvgAf.ProjectAvgPrice == null ? null : (decimal?)Math.Round((pa - p) / p, 4);
                    proHistoryAvgAf.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                    proHistoryAvgAf.UpdateDate = DateTime.Now;
                    proHistoryAvgAf.UpdateUser = Passport.Current.UserName;
                    Services.UpdateProjectHistoryAvg(proHistoryAvgAf);
                }
                //更新即时表
                if ((datProjectAvg.UseMonth).AddMonths(1) == Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")))
                {
                    var proAvgAf = Services.GetProjectAvgPriceByProjectid((int)datProjectAvg.ProjectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, datProjectAvg.UseMonth.AddMonths(1));
                    if (proAvgAf != null && proAvgAf.projectavgid > 0)
                    {
                        decimal p = datProjectAvg.ProjectAvgPrice == null ? 0 : (decimal)datProjectAvg.ProjectAvgPrice;
                        decimal pa = proAvgAf.ProjectAvgPrice == null ? 0 : (decimal)proAvgAf.ProjectAvgPrice;
                        proAvgAf.ProjectGained = datProjectAvg.ProjectAvgPrice == null || proAvgAf.ProjectAvgPrice == null ? null : (decimal?)Math.Round((pa - p) / p, 4);
                        proAvgAf.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        proAvgAf.UpdateDate = DateTime.Now;
                        proAvgAf.UpdateUser = Passport.Current.UserName;
                        int r = Services.UpdateProjectAvg(proAvgAf);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("VQPrice/ProjectAvgPrice/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        [HttpGet]
        public ActionResult ProjectHistoryAvgIndex(int? pageIndex, int projectid, string projectname, string useMonthStr)
        {
            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }

            int totalCount, pageSize = 30, currIndex = pageIndex ?? 1;
            var result = Services.GetProjectHistoryAvgPrices(projectid, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, (DateTime?)TryParseHelper.StrToDateTime(useMonthStr), currIndex, pageSize, out totalCount).OrderByDescending(m => m.UseMonth);
            var model = new PagedList<Dat_ProjectAvg>(result, currIndex, pageSize, totalCount);

            this.ViewBag.ProjectId = projectid;
            this.ViewBag.ProjectName = projectname;
            return View(model);
        }

        //导出
        public ActionResult ExportProjectAvgPrice(string projectName, string useMonthStr, int IsPrices = -1)
        {
            int operate;
            Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }

            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return null;
            }

            DateTime useMonth = string.IsNullOrWhiteSpace(useMonthStr) ? Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")) : Convert.ToDateTime(useMonthStr);
            //VQ模块维护，使用房讯通角色共享数据。
            var data = Services.ExportProjectAvgPrice(Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, projectName, useMonth, IsPrices);

            //操作日志
            Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅案例均价, SYS_Code_Dict.操作.导出, "", "", "导出VQ住宅案例均价", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_VQ住宅案例均价_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(data.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //导出历史
        public ActionResult ExportProjectHistoryAvgPrice(int projectid, string useMonthStr)
        {
            int operate;
            Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }

            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return null;
            }
            //VQ模块维护，使用房讯通角色共享数据。
            var data = Services.ExportProjectHistoryAvgPrice(Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, projectid, (DateTime?)TryParseHelper.StrToDateTime(useMonthStr)).OrderByDescending(m => m.UseMonth);

            //操作日志
            Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅案例均价, SYS_Code_Dict.操作.导出, "", "", "导出VQ住宅单个楼盘案例历史均价", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_VQ住宅案例均价_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(data.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //确认价格
        public ActionResult IsEvaluePrices(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { result = false, msg = "操作失败！" });
            }
            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return Json(new { result = false, msg = "操作失败！" });
            }
            try
            {
                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    int projectid = int.Parse(array[0]);
                    DateTime usemonth = DateTime.Parse(array[1]);
                    //历史表
                    var result = Services.GetProjectHistoryAvgPriceByProjectid(projectid, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, usemonth);
                    if (result != null && result.projectavgid > 0 && result.ProjectAvgPrice > 0)
                    {
                        //判断有没有修改权限
                        int operate;
                        Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                        if (operate == (int)PermissionLevel.None)
                        {
                            failList.Add(array[0]);
                            continue;
                        }
                        result.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                        result.UpdateDate = DateTime.Now;
                        result.UpdateUser = Passport.Current.UserName;
                        result.isevalue = 1;
                        Services.UpdateProjectHistoryAvg(result);
                    }
                    else
                    {
                        failList.Add(array[0]);
                        continue;
                    }
                    //判断即时表
                    if (usemonth == Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")))
                    {
                        var proAvg = Services.GetProjectAvgPriceByProjectid(projectid, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, usemonth);
                        if (proAvg != null && proAvg.projectavgid > 0 && proAvg.ProjectAvgPrice > 0)
                        {
                            //判断有没有修改权限
                            int operate;
                            Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                            if (operate == (int)PermissionLevel.None)
                            {
                                failList.Add(array[0]);
                                continue;
                            }
                            proAvg.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                            proAvg.UpdateDate = DateTime.Now;
                            proAvg.UpdateUser = Passport.Current.UserName;
                            proAvg.isevalue = 1;
                            Services.UpdateProjectAvg(proAvg);
                        }
                        else
                        {
                            failList.Add(array[0]);
                            continue;
                        }
                    }
                }
                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                return this.Back("操作失败！");
            }
        }

        #region 导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.VQ模块分类.VQ住宅案例均价, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = Services.GetTask(SYS_Code_Dict.批量导入类型.VQ住宅案例均价, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            var projectResult = result.ToPagedList(pageIndex ?? 1, 30);
            return View(projectResult);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string taskNameHiddenValue)
        {
            var folder = Server.MapPath("~/tempfiles/datacenter/" + Passport.Current.FxtCompanyId);

            if (null == file) return this.RedirectToAction("UploadFile");
            try
            {
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                SaveFile(file, folder, filename);

                //操作日志
                Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅案例均价, SYS_Code_Dict.操作.导入, "", "", "导入VQ案例均价", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "ProjectAvgPrice");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("VQPrice/ProjectAvgPrice/Uploadfile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
            return this.RedirectToAction("UploadFile");
        }

        //删除导入记录
        public ActionResult DeleteImportRecord(List<string> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return Json(new { result = false, msg = "删除失败！" });
                }

                ids.ForEach(m => Services.DeleteTask(Int64.Parse(m)));

                return Json(new { result = true, msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("VQPrice/ProjectWeightRevised/DeleteImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
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
