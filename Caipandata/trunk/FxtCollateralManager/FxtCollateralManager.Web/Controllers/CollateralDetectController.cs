using CAS.Common.MVC4;
using FxtCollateralManager.Common;
using FxtCollateralManager.Common.FxtAPI;
using FxtCommonLibrary.LibraryUtils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*作者:李晓东
 *摘要:新建 押品检测 控制器
 * 
 * **/
namespace FxtCollateralManager.Web.Controllers
{
    public class CollateralDetectController : BaseController
    {
        //
        // GET: /CollateralDetect/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //地图
        public ActionResult GetPCACount(int cid,
            string wuyetype, string jianzhutype, string niandaitype,
            string daikuantype, string mianjitype, string nianlingtype,
            int type)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("pId", 0);
                obj.Add("cId", cid);
                obj.Add("aId", 0);
                obj.Add("houseType", wuyetype);
                obj.Add("buildingType", jianzhutype);
                obj.Add("buildingDate", niandaitype);
                obj.Add("loanAmount", daikuantype);
                obj.Add("buildingArea", mianjitype);
                obj.Add("age", nianlingtype);
                obj.Add("type", type);
                obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _CollateralCountByPCA, Utils.Serialize(obj)));
            }
        }
        [HttpPost]
        //图形统计
        public ActionResult GetCFCount(string requirement,string start,string end)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("pId", 0);
                obj.Add("cId", 0);
                obj.Add("aId", 0);
                obj.Add("requirement", requirement);
                obj.Add("start", start);
                obj.Add("end", end);
                obj.Add("type",0);
                obj.Add("cityarrid", CookieHelper.Get("cityarrid"));
                obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetCollateralClassification, Utils.Serialize(obj)));
            }
        }
        [HttpPost]
        //明细查询
        public ActionResult GetCollDetials(string wuyetype, string jianzhutype, string niandaitype,
            string daikuantype, string mianjitype, string nianlingtype,
            int projectid, int companyid, string start, string end,
            int pageIndex,int pageSize)
        {

            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("pId", 0);
                obj.Add("cId", 0);
                obj.Add("aId", 0);
                obj.Add("houseType", wuyetype);
                obj.Add("buildingType", jianzhutype);
                obj.Add("buildingDate", niandaitype);
                obj.Add("loanAmount", daikuantype);
                obj.Add("buildingArea", mianjitype);
                obj.Add("age", nianlingtype);
                obj.Add("projectid", projectid);
                obj.Add("companyid", companyid);
                obj.Add("start", start);
                obj.Add("end", end);
                obj.Add("pageIndex", pageIndex);
                obj.Add("pageSize", pageSize);
                obj.Add("cityarrid", CookieHelper.Get("cityarrid"));
                obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetDetials, Utils.Serialize(obj)));
            }
        }
        //模糊搜索已匹配楼盘
        [HttpPost]
        public ActionResult Search(int cityid, string q)
        { 
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("cId", cityid);
                obj.Add("name", q);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetProjectByDataCollateral, Utils.Serialize(obj)));
            }            
        }

        //模糊搜索已匹配开发商
        [HttpPost]
        public ActionResult CompanySearch(int cityid, string q)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("cId", cityid);
                obj.Add("name", q);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetCompanyByDataCollateral, Utils.Serialize(obj)));
            }
        }

        //获得CodeList
        [HttpPost]
        public ActionResult GetCode(int id)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("id", id);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "D", _GetSYSCodeByID, Utils.Serialize(obj)));
            }
        }

        //获得单个Code
        [HttpPost]
        public ActionResult GetCodeByCode(int code)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("code", code);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "D", _GetSYSCodeByCode, Utils.Serialize(obj)));
            }
        }
    }
}
