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
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.WebUI.Areas.Business.Controllers
{
    [Authorize]
    public class BusinessCaseController : BaseController
    {
        private readonly IBusinessCase _businessCase;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;
        private readonly IBusinessCircle _businessCircle;
        private readonly IBusinessStreet _businessStreet;
        private readonly IImportTask _importTask;
        private readonly IDat_Building_Biz _businessBuilding;
        private readonly IDat_House_Biz _businessHouse;

        public BusinessCaseController(IBusinessCase businessCase, ILog log, IDropDownList dropDownList, IBusinessCircle businessCircle, IBusinessStreet businessStreet, IImportTask importTask, IDat_Building_Biz businessBuilding, IDat_House_Biz businessHouse)
        {
            this._businessCase = businessCase;
            this._log = log;
            this._dropDownList = dropDownList;
            this._businessCircle = businessCircle;
            this._businessStreet = businessStreet;
            this._importTask = importTask;
            this._businessBuilding = businessBuilding;
            this._businessHouse = businessHouse;
        }

        public ActionResult Index(Dat_Case_Biz caseBiz, int? pageIndex)
        {
            BindDropDownList(-1);

            if (caseBiz.CaseDateStart == default(DateTime) || caseBiz.CaseDateEnd == default(DateTime))
            {
                return View();
            }

            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            caseBiz.CityId = Passport.Current.CityId;
            caseBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
            int pageSize = 30;
            int totalCount = 0;
            var result = _businessCase.GetCaseBizs(caseBiz, pageIndex ?? 1, pageSize, out totalCount, self);
            var businessCaseResult = new PagedList<Dat_Case_Biz>(result, pageIndex ?? 1, pageSize, totalCount);
            return View("Index", businessCaseResult);

        }

        //删除
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.删除自己)]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                //return this.Back("请选择要删除的数据！");
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

                    _businessCase.DeleteCaseBiz(int.Parse(array[1]));
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除商业案例", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCase/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        //删除重复案例(暂定：操作者具有‘删除全部’的权限，才能进行当前操作)
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.删除自己)]
        public ActionResult DeleteSameCase(string time)
        {
            try
            {
                DateTime caseDateFrom = DateTime.Now, caseDateTo = DateTime.Now;

                switch (time)
                {
                    case "0": //近一个月
                        caseDateFrom = DateTime.Now.AddMonths(-1);
                        caseDateTo = DateTime.Now;
                        break;
                    case "1"://近三个月
                        caseDateFrom = DateTime.Now.AddMonths(-3);
                        caseDateTo = DateTime.Now;
                        break;
                    case "2"://近半年
                        caseDateFrom = DateTime.Now.AddMonths(-6);
                        caseDateTo = DateTime.Now;
                        break;
                    case "3"://近一年
                        caseDateFrom = DateTime.Now.AddYears(-1);
                        caseDateTo = DateTime.Now;
                        break;
                }

               var result = _businessCase.DeleteSameProjectCase(Passport.Current.FxtCompanyId, Passport.Current.CityId, caseDateFrom, caseDateTo);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.操作.删除, "", "", "删除重复商业案例", RequestHelper.GetIP());
                return base.Back("已删除" + result + "条案例数据。");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCase/DeleteSameCase", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }

        }
        
        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            BindDropDownList(-1);
            return View();
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(Dat_Case_Biz caseBiz)
        {
            try
            {
                caseBiz.CityId = Passport.Current.CityId;
                caseBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                caseBiz.CreateTime = DateTime.Now;
                caseBiz.Creator = Passport.Current.ID;
                caseBiz.BuildingId = _businessBuilding.GetBuildIdByName(caseBiz.ProjectId, caseBiz.BuildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (caseBiz.BuildingId > 0)
                {
                    caseBiz.HouseId = _businessHouse.GetHouseId(caseBiz.BuildingId, caseBiz.HouseName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                }
                if (caseBiz.BizCodeId != null && caseBiz.BizCodeId.Any())
                {
                    caseBiz.BizCodeId.ForEach(m => caseBiz.BizCode += m + ",");
                    caseBiz.BizCode = caseBiz.BizCode.TrimEnd(',');
                }
                _businessCase.AddCaseBiz(caseBiz);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.操作.新增, caseBiz.Id.ToString(), "", "新增商业案例", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCase/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }

            return base.RefreshParent();
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var projectCase = _businessCase.GetCaseBizById(int.Parse(splitArray[1]));
            this.ViewBag.BizCode = projectCase.BizCode;

            BindDropDownList(projectCase.AreaId);
            return View("Create", projectCase);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(Dat_Case_Biz caseBiz)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }

                if (caseBiz.FxtCompanyId != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，您不能修改其他公司数据！");
                }

                caseBiz.SaveDateTime = DateTime.Now;
                caseBiz.SaveUser = Passport.Current.ID;
                caseBiz.BuildingId = _businessBuilding.GetBuildIdByName(caseBiz.ProjectId, caseBiz.BuildingName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                if (caseBiz.BuildingId > 0)
                {
                    caseBiz.HouseId = _businessHouse.GetHouseId(caseBiz.BuildingId, caseBiz.HouseName, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                }
                if (caseBiz.BizCodeId != null && caseBiz.BizCodeId.Any())
                {
                    caseBiz.BizCodeId.ForEach(m => caseBiz.BizCode += m + ",");
                    caseBiz.BizCode = caseBiz.BizCode.TrimEnd(',');
                }
                _businessCase.UpdateCaseBiz(caseBiz);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.操作.修改, caseBiz.Id.ToString(), "", "修改商业案例", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCase/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
            return this.RefreshParent();
        }

        //导出
        public ActionResult Export(DateTime caseDateStart, DateTime caseDateEnd)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return base.AuthorizeWarning("对不起，您没有导出权限！");
            if (operate == (int)PermissionLevel.All) self = false;


            var caseBiz = new Dat_Case_Biz
            {
                CaseDateStart = caseDateStart,
                CaseDateEnd = caseDateEnd,
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId
            };

            int totalCount;
            var result = _businessCase.GetCaseBizs(caseBiz, 1, int.MaxValue, out totalCount, self);

            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业案例, SYS_Code_Dict.操作.导出, "", "", "导出商业案例", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_商业案例_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }
        
        //删除导入记录
        public ActionResult DeleteBusinessCaseImportRecord(List<string> ids)
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
                LogHelper.WriteLog("Business/BusinessCase/DeleteBusinessCaseImportRecord", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }
        }
        
        //exce导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile()
        {
            var result = _importTask.GetTask(SYS_Code_Dict.批量导入类型.商业案例, Passport.Current.CityId, Passport.Current.FxtCompanyId);

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
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商业房号, SYS_Code_Dict.操作.导入, "", "", "导入商业案例", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "BusinessCase");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });

                return this.RedirectToAction("UploadFile");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCase/UploadFile", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
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


        #region 帮助程序

        //下拉列表数据绑定
        private void BindDropDownList(int areaId)
        {
            this.ViewBag.AreaName = GetAreaName();
            this.ViewBag.SubAreaName = GetSubAreaNamesByAreaId(areaId);
            this.ViewBag.CaseTypeName = GetDictById(SYS_Code_Dict._案例类型);
            this.ViewBag.RentTypeName = GetDictById(SYS_Code_Dict._租金方式);
            this.ViewBag.FitmentName = GetDictById(SYS_Code_Dict._装修情况);
        }

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

        //根据行政区ID获取所有片区
        [NonAction]
        private IEnumerable<SelectListItem> GetSubAreaNamesByAreaId(int areaId)
        {
            var list = new List<SelectListItem>();
            var subAreaNames = _businessCircle.GetSubAreaBizByAreaId(areaId, Passport.Current.CityId, Passport.Current.FxtCompanyId);

            subAreaNames.ToList().ForEach(m => list.Add(
                new SelectListItem
                {
                    Value = m.SubAreaId.ToString(),
                    Text = m.SubAreaName,
                }
                ));

            list.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return list;
        }

        [HttpPost]
        public ActionResult GetSubAreaName(int areaId)
        {
            var result = GetSubAreaNamesByAreaId(areaId);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetProjectBizAddress(int projectId)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.All) self = false;

            var projects = _businessStreet.GetProjectBizs(Passport.Current.CityId, Passport.Current.FxtCompanyId, projectId: projectId, self: self);
            var result = projects.FirstOrDefault().Address;
            return Json(projectId > 0 ? result : "");
        }

        //自动匹配商家信息
        [HttpGet]
        public JsonResult ProjectNameSelect(int areaId, int subAreaId)
        {
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业案例数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.All) self = false;

            var projects = _businessStreet.GetProjectBizs(Passport.Current.CityId, Passport.Current.FxtCompanyId, areaId, subAreaId, self: self);
            var data = new List<dynamic>();
            foreach (var item in projects)
            {
                data.Add(new { name = item.ProjectName, id = item.ProjectId, address = item.Address });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
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
            casetypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return casetypeResult;
        }

        #endregion

    }
}
