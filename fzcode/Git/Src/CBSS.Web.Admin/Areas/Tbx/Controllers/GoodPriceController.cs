using CBSS.Framework.Contract;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class GoodPriceController : ControllerBase
    {
        //
        // GET: /Tbx/GoodPrice/
         
        public ActionResult Index()
        {
            if (CheckActionName("Good_GoodPrice") == false)
            {
                return Redirect("~/Account/Auth/Login");
            } 
            GoodID = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"]);//GoodID 
            ViewData["GoodName"] = this.TbxService.GetGood(GoodID);//商品名称
            return View();
        }
        static int GoodID;
        public JsonResult GetGoodPricePage(int pagesize, int pageindex)
        { 
            GoodPriceRequest request = new GoodPriceRequest();
            request.GoodID = GoodID;
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize; 
            int total = 0;
            var list = TbxService.GetGoodPriceList(out total, request);
            return Json(new { total = total, rows = list });
        }
        public ActionResult Create()
        { 
            var model = new GoodPrice();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            int GoodID = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"]);//GoodID
            var model = new GoodPrice();
            model.CreateUser = 0;
            this.TryUpdateModel<GoodPrice>(model);
            try
            {
                model.GoodID = GoodID;
                this.TbxService.SaveGoodPrice(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View("Edit", model);
            }
            return this.RefreshParent();
        }
        public ActionResult Edit(int id)
        {
            var model = this.TbxService.GetGoodPrice(id); 
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.TbxService.GetGoodPrice(id);
            this.TryUpdateModel<GoodPrice>(model);
            this.TbxService.SaveGoodPrice(model);
            return this.RefreshParent();
        }
        /// <summary>
        /// 根据GoodID删除策略
        /// </summary>
        /// <param name="Price"></param>
        /// <returns></returns>
        public JsonResult DelGoodPriceByGoodPriceID(int GoodPriceID)
        {
            try
            {
                int flag = TbxService.DelGoodPriceByGoodPriceID(GoodPriceID);
                if (flag == 1)
                {
                    return Json("删除成功");
                }
                else
                {
                    return Json("删除失败");
                }
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return Json("删除失败");
            }
        }
    }
}
