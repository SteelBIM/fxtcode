using System;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.IoC.Binder;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Ninject;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Text;
using System.Net;
using System.Configuration;

namespace FXT.DataCenter.WebUI.Areas.VQPrice.Controllers
{
    [Authorize]
    public class HouseWeightRevisedController : BaseController
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;
        private static readonly IHouseWeightRevisedServices Services = new StandardKernel(new HouseWeightRevisedModule()).Get<IHouseWeightRevisedServices>();

        public ActionResult Index(string buildingName, int projectId, int buildingId, string houseName, int? pageIndex, int type = -1)
        {
            #region 查看自己或查看全部
            var self = true; int operate;
            Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅基准房价, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;
            #endregion

            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId;
            this.ViewBag.buildingName = buildingName;

            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }

            int totalCount, pageSize = 30, currIndex = pageIndex ?? 1;
            //VQ模块维护，使用房讯通角色共享数据。
            //var datWeightHouse = new DatWeightHouse { ProjectId = projectId, BuildingId = buildingId, HouseName = houseName, CityId = Passport.Current.CityId, FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Type = type };

            var data = Services.GetWeightHouses(projectId, buildingId, houseName, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode, type, currIndex, pageSize, out totalCount, self);
            var model = new PagedList<DatWeightHouse>(data, currIndex, pageSize, totalCount);
            return View(model);
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(int houseId, int projectId, int buildingId)
        {
            var result = new DatWeightHouse();
            var cp = FxtUserCenterService_GetFPInfo(Convert.ToInt32(ConfigurationHelper.FxtCompanyId), Passport.Current.CityId, 1003036, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View(result);
            }
            //VQ模块维护，使用房讯通角色共享数据。
            result = Services.GetWeightHouse(projectId, buildingId, houseId, Passport.Current.CityId, cp.ParentShowDataCompanyId, cp.ParentProductTypeCode);

            this.ViewBag.projectId = projectId;
            this.ViewBag.buildingId = buildingId;
            this.ViewBag.buildingName = "";
            return View(result);
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(DatWeightHouse datWeightHouse)
        {
            try
            {
                int operate;
                Permission.Check(SYS_Code_Dict.VQ模块分类.VQ住宅基准房价, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.Back("对不起，您没有修改权限！");
                }
                if (datWeightHouse.Id > 0)
                {
                    datWeightHouse.UpdateDate = DateTime.Now;
                    datWeightHouse.UpdateUser = Passport.Current.UserName;
                    datWeightHouse.EvaluationCompanyId = Passport.Current.FxtCompanyId;
                    Services.UpdateWeightHouse(datWeightHouse);

                    //操作日志
                    Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅基准房价, SYS_Code_Dict.操作.修改, "", "", "修改VQ房号修正系数", RequestHelper.GetIP());
                }
                else
                {
                    //VQ模块维护，使用房讯通角色共享数据。
                    datWeightHouse.CityId = Passport.Current.CityId;
                    datWeightHouse.FxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);
                    datWeightHouse.UpdateDate = DateTime.Now;
                    datWeightHouse.UpdateUser = Passport.Current.UserName;
                    datWeightHouse.EvaluationCompanyId = Passport.Current.FxtCompanyId;
                    datWeightHouse.Weight = datWeightHouse.Weight ?? 1;

                    Services.AddWeightHouse(datWeightHouse);
                    //操作日志
                    Services.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.VQ住宅基准房价, SYS_Code_Dict.操作.新增, "", "", "新增VQ房号修正系数", RequestHelper.GetIP());
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("VQPrice/HouseWeightRevised/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.RefreshParent("操作失败！");
            }
            return this.RefreshParent();
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
