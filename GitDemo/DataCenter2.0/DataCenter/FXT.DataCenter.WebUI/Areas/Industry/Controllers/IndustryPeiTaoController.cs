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
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.Industry.Controllers
{
    [Authorize]
    public class IndustryPeiTaoController : BaseController
    {
        private readonly IIndustryProject _industryProject;
        private readonly IIndustryPeiTao _industryPeiTao;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IImportTask _importTask;
        private readonly ICompany _company;
        public IndustryPeiTaoController(ILog log, IDropDownList dropDownList, IIndustryProject industryProject, IIndustryPeiTao industryPeiTao, IImportTask importTask, ICompany company)
        {
            this._log = log;
            this._dropDownList = dropDownList;
            this._industryProject = industryProject;
            this._industryPeiTao = industryPeiTao;
            this._importTask = importTask;
            this._company = company;
        }
        //
        // GET: /Industry/IndustryPeiTao/

        public ActionResult Index(DatPeiTaoIndustry peitao, int? pageIndex)
        {
            this.ViewBag.ProjectId = peitao.ProjectId;
            this.ViewBag.FxtCompanyId = _industryProject.GetProjectNameById(peitao.ProjectId, Passport.Current.FxtCompanyId).FxtCompanyId;
            this.ViewBag.ProjectName = _industryProject.GetProjectNameById(peitao.ProjectId, Passport.Current.FxtCompanyId).ProjectName;

            BindViewData();

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.工业数据分类.工业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return View(new List<DatPeiTaoIndustry>().ToPagedList(1, 30));
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            peitao.FxtCompanyId = Passport.Current.FxtCompanyId;
            peitao.CityId = Passport.Current.CityId;

            var pageSize = 30;
            int totalCount;
            var result = _industryPeiTao.GetIndustryPeiTaos(peitao, pageIndex ?? 1, pageSize, out totalCount, self).AsQueryable();

            var peiTaoResult = new PagedList<DatPeiTaoIndustry>(result, pageIndex ?? 1, pageSize, totalCount);

            return View("Index", peiTaoResult);
        }

        #region 编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
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

            this.ViewBag.ProjectId = splitArray[1];
            var result = _industryPeiTao.GetPeiTaoById(int.Parse(splitArray[2]), int.Parse(splitArray[0]));
            BindDropDownList();
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(DatPeiTaoIndustry peitao)
        {
            try
            {
                peitao.SaveDate = DateTime.Now;
                peitao.SaveUser = Passport.Current.ID;

                //增加新增租户商家
                if (_company.GetCompany_office(peitao.TenantName, Passport.Current.CityId).FirstOrDefault() == null)
                {
                    var company = new DAT_Company();
                    company.ChineseName = peitao.TenantName;
                    company.CreateDate = DateTime.Now;
                    company.CityId = Passport.Current.CityId;
                    company.FxtCompanyId = Passport.Current.FxtCompanyId;
                    _company.AddCompany(company);
                }
                peitao.TenantID = _company.GetCompany_office(peitao.TenantName, Passport.Current.CityId).FirstOrDefault().CompanyId;
                _industryPeiTao.UpdateIndustryPeiTao(peitao, Passport.Current.FxtCompanyId);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业项目配套, SYS_Code_Dict.操作.修改, peitao.PeiTaoID.ToString(), peitao.PeiTaoName, "修改工业项目配套", RequestHelper.GetIP());

                return RedirectToAction("Index", new { projectId = peitao.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryPeitao/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }
        }

        [HttpPost]
        public JsonResult IsExistIndustryPeiTao(long peiTaoID, string peiTaoName, long projectId)
        {
            return Json(_industryPeiTao.IsExistIndustryPeiTao(peiTaoID, peiTaoName, projectId, Passport.Current.CityId, Passport.Current.FxtCompanyId));
        }

        #endregion

        #region 新增
        [HttpGet]
        public ActionResult Create(int projectId)
        {
            this.ViewBag.ProjectId = projectId;
            BindDropDownList();
            return View("Edit");
        }
        [HttpPost]
        public ActionResult Create(DatPeiTaoIndustry peitao)
        {
            try
            {
                peitao.CityId = Passport.Current.CityId;
                peitao.FxtCompanyId = Passport.Current.FxtCompanyId;
                peitao.CreateDate = DateTime.Now;
                peitao.Creators = Passport.Current.ID;

                //增加新增租户商家
                if (_company.GetCompany_office(peitao.TenantName, Passport.Current.CityId).FirstOrDefault() == null)
                {
                    var company = new DAT_Company();
                    company.ChineseName = peitao.TenantName;
                    company.CreateDate = DateTime.Now;
                    company.CityId = Passport.Current.CityId;
                    company.FxtCompanyId = Passport.Current.FxtCompanyId;
                    _company.AddCompany(company);
                }
                peitao.TenantID = _company.GetCompany_office(peitao.TenantName, Passport.Current.CityId).FirstOrDefault().CompanyId;
                _industryPeiTao.AddIndustryPeiTao(peitao);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业项目配套, SYS_Code_Dict.操作.新增, "", "", "新增工业项目配套", RequestHelper.GetIP());

                return RedirectToAction("Index", new { projectId = peitao.ProjectId });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryPeitao/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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
                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    if (self)
                    {
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[2]);
                            continue;
                        }
                    }
                    var result = _industryPeiTao.GetPeiTaoById(int.Parse(array[2]), int.Parse(array[0]));
                    result.SaveDate = DateTime.Now;
                    result.SaveUser = Passport.Current.ID;
                    _industryPeiTao.DeleteIndustryPeiTao(result, Passport.Current.FxtCompanyId);
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业项目配套, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除工业项目配套", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryPeitao/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        #endregion

        #region 导出
        [HttpGet]
        public ActionResult Export(long ProjectId, int? PeiTaoCode, string PeiTaoName)
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

            var peitao = new DatPeiTaoIndustry
            {
                FxtCompanyId = Passport.Current.FxtCompanyId,
                CityId = Passport.Current.CityId,
                ProjectId = ProjectId,
                PeiTaoCode = PeiTaoCode ?? 0,
                PeiTaoName = PeiTaoName,
            };

            int totalCount;
            var result = _industryPeiTao.GetIndustryPeiTaos(peitao, 1, int.MaxValue, out totalCount, self);

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_工业_项目配套_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业项目配套, SYS_Code_Dict.操作.导出, "", "", "导出工业项目配套", RequestHelper.GetIP());

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
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.工业项目配套, Passport.Current.CityId, Passport.Current.FxtCompanyId);
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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.工业项目配套, SYS_Code_Dict.操作.导入, "", "", "导入工业项目配套", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId, taskNameHiddenValue, "IndustryPeiTao");
                    try { client.Close(); }
                    catch { client.Abort(); }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Industry/IndustryPeitao/UploadFile", RequestHelper.GetIP(), Passport.Current.ID,
                    Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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
        public ActionResult DeleteIndustryPeiTaoImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Industry/IndustryPeiTao/DeleteIndustryPeiTaoImportRecord", RequestHelper.GetIP(), Passport.Current.ID,
                        Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }

        #endregion

        #region 辅助方法
        private void BindViewData()
        {
            ViewData.Add("PeiTaoCode", new SelectList(GetDictById(SYS_Code_Dict._办公商务配套), "Value", "Text", "-1"));
        }

        private void BindDropDownList()
        {
            this.ViewBag.PeiTaoCodeName = GetDictById(SYS_Code_Dict._办公商务配套);
        }

        //根据ID查找相应值
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

        //自动匹配商家信息
        [HttpGet]
        public JsonResult TenantNameSelect()
        {
            var result = _company.GetCompany_office(null, Passport.Current.CityId);

            var data = new List<dynamic>();
            foreach (var item in result)
            {
                data.Add(new { name = item.ChineseName, id = item.CompanyId.ToString() });
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
