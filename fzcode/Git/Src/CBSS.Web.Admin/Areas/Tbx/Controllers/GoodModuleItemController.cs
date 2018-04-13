using CBSS.Tbx.BLL;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class GoodModuleItemController : ControllerBase
    {
        //
        // GET: /Tbx/GoodModuleItem/
        static int GoodID;
        public ActionResult Index()
        {
            if (CheckActionName("Good_GoodModuleItem") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            GoodID = Request.RequestContext.RouteData.Values["id"] != null ? Convert.ToInt32(Request.RequestContext.RouteData.Values["id"].ToString()) : 0;//GoodID
            IEnumerable<GoodModuleItem> modulelist = TbxService.GetGoodModuleItem(GoodID);
            if (modulelist != null && modulelist.Count() > 0)
            {
                string modids = "";
                foreach (GoodModuleItem row in modulelist)
                    modids += row.ModuleID + ",";
                ViewBag.ModeleIds = "," + modids;
            }
            return View();
        }
        public JsonResult GetGoodModulePage(int MarketBookID)
        {
            TreeData treedata = new TreeData(); 
            if (MarketBookID > 0)
            {
                List<v_GoodModuleItem> dt = TbxService.GetGoodModuleByMarketClassifyId(GoodID, MarketBookID);
                if (dt != null && dt.Count() > 0)
                {
                    return Json(new { total = dt.Count(), rows = dt });
                }
            } 
            return Json("");
        }
        /// <summary>
        /// 根据GoodID删除对应的模块后再批量插入GoodModuleItem表
        /// </summary>
        /// <param name="MarketClassifyId"></param>
        /// <returns></returns>
        public JsonResult CommitGoodModule(string MarketClassifyId)
        {
            try
            {
                if (MarketClassifyId != "")
                {
                    MarketClassifyId = MarketClassifyId.TrimEnd(',').TrimStart(',');
                }
                else
                {
                    bool flag = TbxService.DelGoodModule(GoodID);
                    return Json(flag);
                }
                string[] strMarketClassifyId = MarketClassifyId.Split(',');
                if (strMarketClassifyId != null && strMarketClassifyId.Length > 0)
                {
                    List<GoodModuleItem> list = new List<GoodModuleItem>();
                    foreach (var item in strMarketClassifyId)
                    {
                        int ModuleID = 0;
                        if (item.Contains('|'))
                            ModuleID = Convert.ToInt32(item.Split('|')[1]);
                        else
                            ModuleID = Convert.ToInt32(item);
                        if (ModuleID > 0)
                        {
                            list.Add(new GoodModuleItem()
                            {
                                GoodID = GoodID,
                                ModuleID = ModuleID,
                                ModuleFunctionID = 0
                            });
                        }
                    }
                    if (list.Count() > 0)
                    {
                        bool flag = TbxService.SaveGoodModule(GoodID, list);
                        return Json(flag);
                    }
                }
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
