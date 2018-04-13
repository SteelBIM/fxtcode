using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class AppGoodItemController : ControllerBase
    {
        //
        // GET: /Tbx/AppGoodItem/

        public ActionResult Index()
        {
            string AppID =Request.RequestContext.RouteData.Values["id"].ToString();//AppID
            var goodList = this.TbxService.GetGoodList(a => 1 == 1);
            var appGoodItemList = this.TbxService.GetAppGoodItemList(a => a.AppID == AppID);
            if (goodList != null && goodList.Count() > 0)
            {
                List<v_AppGoodItem> list = new List<v_AppGoodItem>();
                foreach (var item in goodList)
                {
                    list.Add(new v_AppGoodItem()
                    {
                        GoodID = item.GoodID,
                        GoodName = item.GoodName,
                        GoodWay = item.GoodWay,
                        Status = item.Status,
                        CreateDate = item.CreateDate,
                        CreateUser = item.CreateUser,
                        isCheck = appGoodItemList.Where(a => a.GoodID == item.GoodID).Count() > 0 ? true : false
                    });
                }
                return View(list);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create()
        {
            var model = new Good();
            return View("Index",model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            return this.RefreshParent();
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            return this.RefreshParent();
        }
    }
}
