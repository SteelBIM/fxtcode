using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class GoodController : ControllerBase
    {
        //商品
        // GET: /Tbx/Good/
        public ActionResult Index()
        {
            if (CheckActionName("Good_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            ViewBag.Good_GoodPrice = action.Good_GoodPrice;
            ViewBag.Good_GoodModuleItem = action.Good_GoodModuleItem;
            ViewData["GoodWay"] = new SelectList(WebControl.GetSelectList(typeof(GoodWayEnum)), "Value", "Text", "请选择");
            return View();
        }
        public JsonResult GetGoodPage(int pagesize, int pageindex,string GoodWay,string GoodName)
        {
            GoodRequest request = new GoodRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.GoodWay =string.IsNullOrEmpty(GoodWay)?0:Convert.ToInt32(GoodWay);
            request.GoodName = GoodName;
            int total = 0;
            var list = TbxService.GetGoodList(out total, request);
            return Json(new { total = total, rows = list });
        } 
        public ActionResult Create()
        {
            ViewData["GoodWay"] = new SelectList(WebControl.GetSelectList(typeof(GoodWayEnum)), "Value", "Text");
            var model = new Good();
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Good();
            model.CreateUser = 0;
            this.TryUpdateModel<Good>(model);
            try
            {
                int flag = this.TbxService.SaveGood(model);
                if (flag == 2)
                {
                    ViewData["GoodWay"] = new SelectList(WebControl.GetSelectList(typeof(GoodWayEnum)), "Value", "Text");
                    ModelState.AddModelError(model.GoodID.ToString(), "策略名称不能重复!");
                    return View("Edit", model);
                }
                else if (flag == 0)
                {
                    return this.RefreshParent("操作失败！");
                }
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
            var model = this.TbxService.GetGood(id);
            ViewData["GoodWay"] = new SelectList(WebControl.GetSelectList(typeof(GoodWayEnum)), "Value", "Text", model.GoodWay);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.TbxService.GetGood(id);
            this.TryUpdateModel<Good>(model); 
            try
            {
                int flag = this.TbxService.SaveGood(model);
                if (flag == 2)
                {
                    ViewData["GoodWay"] = new SelectList(WebControl.GetSelectList(typeof(GoodWayEnum)), "Value", "Text");
                    ModelState.AddModelError(model.GoodID.ToString(), "策略名称不能重复!");
                    return View("Edit", model);
                }
                else if (flag == 0)
                {
                    return this.RefreshParent("操作失败！");
                }
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View("Edit", model);
            }
            return this.RefreshParent();
        }
        /// <summary>
        /// 根据GoodID删除策略
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public int DelGoodByGoodID(int GoodID)
        {
            try
            {
               return TbxService.DelGoodByGoodID(GoodID);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return 2;
            }
        }
    }
}
