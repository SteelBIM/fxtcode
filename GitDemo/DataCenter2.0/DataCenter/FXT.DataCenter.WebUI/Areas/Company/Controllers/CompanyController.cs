using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
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

namespace FXT.DataCenter.WebUI.Areas.Company.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {

        private readonly ICompany _company;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;

        public CompanyController(ICompany company, ILog log, IDropDownList dropDownList)
        {
            this._company = company;
            this._log = log;
            this._dropDownList = dropDownList;
        }

        //
        // GET: /Company/Company/

        public ActionResult Index(string name, int? pageIndex)
        {
            var companys = _company.GetCompany_like(name, Passport.Current.CityId);
            var model = companys.ToPagedList(pageIndex ?? 1, 10);
            return View(model);
        }

        //删除
        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            try
            {
                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');

                    if (TryParseHelper.StrToInt32(array[0],-1) != Passport.Current.FxtCompanyId)
                    {
                        failList.Add(array[1]);
                        continue;
                    }

                    _company.DeleteCompany(int.Parse(array[1]));
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.公司, SYS_Code_Dict.操作.删除, "", "", "删除公司", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Company/Company/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "操作失败！" });
            }

        }

        //创建
        [HttpGet]
        public ActionResult Create()
        {
            BindView();
            return View("Edit", new DAT_Company());
        }
        //创建保存
        [HttpPost]
        public ActionResult Create(DAT_Company dc)
        {
            try
            {
                dc.CreateDate = DateTime.Now;
                dc.ChineseName = dc.ChineseName.Trim();
                dc.CityId = Passport.Current.CityId;
                dc.FxtCompanyId = Passport.Current.FxtCompanyId;
                _company.AddCompany(dc);

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.公司, SYS_Code_Dict.操作.新增, "", "", "新增公司", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Company/Company/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.RefreshParent("操作失败！");
            }

            return base.RefreshParent();
        }

        //下拉框数据绑定
        public void BindView(int proId = -1, int cityid = -1, int companytypecode = -1, int natureCode = -1, int industryCode = -1, int subIndustryCode = -1, int standingCode = -1)
        {
            ViewData.Add("Province", new SelectList(GetProvince(), "Value", "Text", proId));
            ViewData.Add("CityId", new SelectList(GetCityByProId(proId), "Value", "Text", cityid));
            ViewData.Add("CompanyTypeCode", new SelectList(GetDictById(SYS_Code_Dict._公司类型), "Value", "Text", companytypecode));
            ViewData.Add("NatureCode", new SelectList(GetDictById(SYS_Code_Dict._公司性质), "Value", "Text", natureCode));
            ViewData.Add("IndustryCode", new SelectList(GetDictById(SYS_Code_Dict._行业大类), "Value", "Text", industryCode));
            ViewData.Add("SubIndustryCode", new SelectList(GetDictById(SYS_Code_Dict._行业小类), "Value", "Text", subIndustryCode));
            ViewData.Add("StandingCode", new SelectList(GetDictById(SYS_Code_Dict._行业地位), "Value", "Text", standingCode));

        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');
            if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
            {
                return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
            }

            var wp = _company.GetCompanyById(int.Parse(splitArray[1]));
            var city = _dropDownList.GetCityByCityId(wp.CityId ?? -1).FirstOrDefault();
            var proid = city == null ? -1 : city.ProvinceId;
            BindView(proid, wp.CityId ?? -1, wp.CompanyTypeCode ?? -1, wp.NatureCode ?? -1, wp.IndustryCode ?? -1, wp.SubIndustryCode ?? -1, wp.StandingCode ?? -1);

            this.ViewBag.companyId = wp.GroupId;
            this.ViewBag.chineseName = wp.GroupName;

            return View(wp);
        }
        //编辑保存
        [HttpPost]
        public ActionResult Edit(DAT_Company dc)
        {
            try
            {
                dc.CreateDate = DateTime.Now;
                dc.ChineseName = dc.ChineseName.Trim();
                _company.UpdateCompany(dc);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.公司, SYS_Code_Dict.操作.修改, "", "", "修改公司", RequestHelper.GetIP());

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Company/Company/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }

            return this.RefreshParent();
        }

        //导出
        [HttpGet]
        public ActionResult Export(string name)
        {
            var companys = _company.GetCompany_like(name, Passport.Current.CityId);
            companys.ToList().ForEach(m =>
            {
                m.GroupName = GetCompanyTypeName(m.GroupId ?? -1);
                m.NatureName = GetNameByCode(m.NatureCode ?? -1);
                m.IndustryName = GetNameByCode(m.IndustryCode ?? -1);
                m.SubIndustryName = GetNameByCode(m.SubIndustryCode ?? -1);
                m.StandingName = GetNameByCode(m.StandingCode ?? -1);
            });
                        
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商家企业信息_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            try
            {
                //操作日志
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.公司, SYS_Code_Dict.操作.导出, "", "", "导出商家企业信息", RequestHelper.GetIP());

                using (var ms = ExcelHandle.ListToExcel(companys.ToList()))
                {
                    return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Company/Export", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //导入
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                var folder = Server.MapPath("~/Excel/Company/UploadFile/" + Passport.Current.FxtCompanyId);
                //获得文件名
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                var isSuccess = SaveFile(file, folder, filename);

                if (isSuccess)
                {
                    string filePath = Path.Combine(folder, filename);
                    var excelHelper = new ExcelHandle(filePath);
                    var data = excelHelper.ExcelToDataTable("Sheet1", true);

                    List<DAT_Company> dcList;
                    DataToList(data, out dcList);

                   // dcList.ForEach(m => _company.AddCompany(m));

                    foreach (var item in dcList)
                    {
                        var count = _company.GetCompany_office(item.ChineseName, item.CityId ?? -1).Count();
                        if (count > 0) continue;
                        _company.AddCompany(item);
                    }

                }
            }
            catch (Exception)
            {
                return this.Back("操作失败");
            }

            return this.RefreshParent();
        }

        [NonAction]
        private static bool SaveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
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
        [NonAction]
        private void DataToList(DataTable dt, out List<DAT_Company> dcList)
        {


            if (dt.Rows.Count > 0)
            {

                var result = new List<DAT_Company>();
                var cityid = Passport.Current.CityId;

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var dc = new DAT_Company
                    {
                        ChineseName = dt.Rows[i][0].ToString(),
                        EnglishName = dt.Rows[i][1].ToString(),
                        CompanyTypeCode = GetCodeByName(dt.Rows[i][2].ToString(), SYS_Code_Dict._公司类型),
                        Address = dt.Rows[i][3].ToString(),
                        Telephone = dt.Rows[i][4].ToString(),
                        Fax = dt.Rows[i][5].ToString(),
                        Website = dt.Rows[i][6].ToString(),
                        COtherName = dt.Rows[i][7].ToString(),
                        Brand = dt.Rows[i][8].ToString(),
                        FromCountry = dt.Rows[i][9].ToString(),
                        FromCity = dt.Rows[i][10].ToString(),
                        NatureCode = GetCodeByName(dt.Rows[i][11].ToString(), SYS_Code_Dict._公司性质),
                        IndustryCode = GetCodeByName(dt.Rows[i][12].ToString(), SYS_Code_Dict._行业大类),
                        SubIndustryCode = GetCodeByName(dt.Rows[i][13].ToString(), SYS_Code_Dict._行业小类),
                        ScaleCode = (int?)TryParseHelper.StrToInt32(dt.Rows[i][14].ToString()),
                        RegistCapital = (int?)TryParseHelper.StrToInt32(dt.Rows[i][15].ToString()),
                        StandingCode = GetCodeByName(dt.Rows[i][16].ToString(), SYS_Code_Dict._行业地位)
                    };
                    var query = _company.GetCompany_like(dt.Rows[i][17].ToString(), Passport.Current.CityId).FirstOrDefault();
                    dc.GroupId = query == null ? -1 : query.CompanyId;
                    dc.CityId = cityid;
                    dc.FxtCompanyId = Passport.Current.FxtCompanyId;
                    dc.CreateDate = DateTime.Now;
                    result.Add(dc);

                }
                dcList = result;
            }
            else
            {
                dcList = new List<DAT_Company>();
            }
        }

        //统计
        public ActionResult Statistics(int? companyid, string companyname)
        {
            if (companyid == null) return View();
            var count = _company.CompanyStatistcs((int)companyid).Split(',');

            dynamic model = new ExpandoObject();
            model.CompanyName = companyname;
            model.LandCount = count[0];
            model.ProjectCount = count[1];

            //this.ViewBag.Statistics = new { CompanyName = companyname, LandCount = count[0], ProjectCount = count[1] };
            this.ViewBag.Statistics = model;
            return View();
        }



        #region 帮助城市

        [HttpGet]
        public JsonResult GetSubIndustry(int code)
        {
            var subCode = _dropDownList.GetDictBySubCode(code);
            return Json(subCode.Select(m=>new{Value = m.code,Text = m.codename}),JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CompanyIsExit(string chineseName, int companyId)
        {
            var result = _company.CompanyIsExit(chineseName,companyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        private int GetCodeByName(string name,int typeId)
        {
            return _dropDownList.GetCodeByName(name,typeId);
        }

        [NonAction]
        private string GetNameByCode(int code)
        {
            return _dropDownList.GetNameByCode(code);
        }

        [NonAction]
        public string GetCompanyTypeName(int companyid)
        {
            var query = _company.GetCompanyById(companyid);
            return query == null ? null : query.ChineseName;
        }

        [HttpGet]
        public JsonResult GetGroup(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return Json(null, JsonRequestBehavior.AllowGet);

            var result = _company.GetCompany_like(key, Passport.Current.CityId);
            return Json(result.Select(m => new { id = m.CompanyId, text = m.ChineseName }).ToList().Distinct(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCity(int? proId)
        {
            var result = GetCityByProId(proId ?? -1);
            return Json(result.Select(m => new
            {
                m.Value,
                m.Text
            }), JsonRequestBehavior.AllowGet);
        }

        //excel 导出头部信息
        readonly Action<string> _excelExportHeader = m =>
        {
            var curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             HttpUtility.UrlEncode(m, System.Text.Encoding.GetEncoding("UTF-8")) + DateTime.Now.ToShortDateString()+".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

        };

        //获取所有省份
        private IEnumerable<SelectListItem> GetProvince()
        {
            var provinces = _dropDownList.GetProvince();
            var listItems = new List<SelectListItem>();
            provinces.ToList().ForEach(m => listItems.Add(
                new SelectListItem
                {

                    Value = m.ProvinceId.ToString(),
                    Text = m.ProvinceName

                }));
            listItems.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return listItems;
        }

        //根据省份获取城市
        [NonAction]
        private IEnumerable<SelectListItem> GetCityByProId(int proId)
        {
            var citys = _dropDownList.GetCityByProId(proId);
            var listItems = new List<SelectListItem>();
            citys.ToList().ForEach(m => listItems.Add(
                new SelectListItem
                {
                    Value = m.CityId.ToString(),
                    Text = m.CityName

                }));
            return listItems;
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

        #endregion

    }
}
