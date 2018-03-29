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
using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.IoC.Binder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using Newtonsoft.Json;
using Ninject;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.WebUI.Areas.VQPrice.Controllers
{
    [Authorize]
    public class ProjectWeightRevisedController : BaseController
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;
        private static readonly IProjectWeightRevisedServices Services = new StandardKernel(new ProjectWeightRevisedModule()).Get<IProjectWeightRevisedServices>();

        public ActionResult Index(int? pageIndex, string projectName, int type = -1)
        {
            bool self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅基准房价, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            int totalCount, pageSize = 30, currIndex = pageIndex ?? 1;

            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }

            //VQ模块维护，使用房讯通角色共享数据。
            //var datWeightProject = new DatWeightProject { ProjectName = projectName, CityId = Passport.Current.CityId, FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Type = type };
            var data = Services.GetWeightProjects(projectName, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, type, currIndex, pageSize, out totalCount, self);
            var model = new PagedList<DatWeightProject>(data, currIndex, pageSize, totalCount);
            return View(model);
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(int projectId)
        {
            var result = new DatWeightProject();
            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View(result);
            }
            //VQ模块维护，使用房讯通角色共享数据。
            result = Services.GetWeightProject(projectId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode);
            ViewBag.select2ProjectId = result.ProjectId;
            ViewBag.select2ProjectName = result.ProjectName;
            return View(result);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(DatWeightProject datWeightProject)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅基准房价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.Back("对不起，您没有修改权限！");
                }

                //存在，更新即可；不存在，不存在则把数据补入价格表中
                if (datWeightProject.Id > 0)
                {
                    datWeightProject.UpdateDate = DateTime.Now;
                    datWeightProject.UpdateUser = Passport.Current.UserName;
                    datWeightProject.EvaluationCompanyId = Passport.Current.FxtCompanyId;
                    Services.UpdateWeightProject(datWeightProject);
                    //操作日志
                    Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅基准房价, SYS_Code_Dict.操作.修改, "", "", "修改住宅基准房价", RequestHelper.GetIP());
                }
                else
                {
                    //VQ模块维护，使用房讯通角色共享数据。
                    datWeightProject.CityId = Passport.Current.CityId;
                    datWeightProject.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                    datWeightProject.UpdateDate = DateTime.Now;
                    datWeightProject.UpdateUser = Passport.Current.UserName;
                    datWeightProject.EvaluationCompanyId = Passport.Current.FxtCompanyId;
                    datWeightProject.LowLayerWeight = datWeightProject.LowLayerWeight ?? 1;
                    datWeightProject.MultiLayerWeight = datWeightProject.MultiLayerWeight ?? 1;
                    datWeightProject.SmallHighLayerWeight = datWeightProject.SmallHighLayerWeight ?? 1;
                    datWeightProject.HighLayerWeight = datWeightProject.HighLayerWeight ?? 1;

                    Services.AddWeightProject(datWeightProject);
                    //操作日志
                    Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅基准房价, SYS_Code_Dict.操作.新增, "", "", "新增住宅基准房价", RequestHelper.GetIP());
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("VQPrice/ProjectWeightRevised/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
        }

        //导出
        public ActionResult ExportNotUpdatedAvgPrice(string projectName, int type = -1)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅基准房价, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return null;
            }
            //VQ模块维护，使用房讯通角色共享数据。
            //var datWeightProject = new DatWeightProject { CityId = Passport.Current.CityId, FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), ProjectName = projectName, Type = type };
            var data = Services.GetNotUpdatedAvrPriceProjects(projectName, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, type, self);

            //操作日志
            Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅基准房价, SYS_Code_Dict.操作.导出, "", "", "导出住宅基准房价", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_VQ住宅基准房价_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(data.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //楼盘导入
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.VQ模块分类.VQ住宅基准房价, SYS_Code_Dict.页面权限.导入)]
        public ActionResult UploadFile(int? pageIndex)
        {
            var result = Services.GetTask(SYS_Code_Dict.批量导入类型.VQ住宅基准房价, Passport.Current.CityId, Passport.Current.FxtCompanyId);

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
                Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅基准房价, SYS_Code_Dict.操作.导入, "", "", "导入VQ基准房价", RequestHelper.GetIP());

                // 异步调用WCF服务
                var filePath = Path.Combine(folder, filename);
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                var userId = Passport.Current.ID;
                Task.Factory.StartNew(() =>
                {
                    var client = new ExcelUploadServices.ExcelUploadClient();
                    client.StartAsync(cityId, fxtCompanyId, filePath, userId,
                        taskNameHiddenValue, "ProjectWeightRevised");
                    try { client.Close(); }
                    catch { client.Abort(); }

                });

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("VQPrice/ProjectWeightRevised/Uploadfile", RequestHelper.GetIP(), Passport.Current.ID,
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
