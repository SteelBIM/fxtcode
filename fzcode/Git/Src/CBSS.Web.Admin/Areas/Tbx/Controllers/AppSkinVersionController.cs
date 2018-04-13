using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class AppSkinVersionController : ControllerBase
    {
        //
        // GET: /Tbx/AppSkinVersion/

        public ActionResult Index(AppVersionRequest req)
        {
            if (CheckActionName("App_AppSkinVersion") == false)
            {
                return Redirect("~/Account/Auth/Login");
            } 
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text");
            AppID = Request.RequestContext.RouteData.Values["id"].ToString();//AppID
            return View();
        }
        static string AppID = "";
        public JsonResult GetAppSkinVersionPage(int pagesize, int pageindex, int AppType)
        {
            AppVersionRequest request = new AppVersionRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.AppType = AppType;
            request.AppID = AppID;
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text", request.AppType);
            int total = 0;
            var list = TbxService.GetAppSkinVersionList(out total, request);
            return Json(new { total = total, rows = list });
        } 
        //
        // GET: /Tbx/AppSkinVersion/Create

        public ActionResult Create()
        {
            ViewData["AppID"] = new SelectList(this.TbxService.GetAppListByStatus(), "AppID", "AppName");//应用名称
            var model = new AppSkinVersion();
            ViewData["UpdateType"] = new SelectList(WebControl.GetSelectList(typeof(AppVersionUpdateTypeEnum)), "Value", "Text");//应用版本号更新类型
            return View("Edit", model);
        }

        //
        // POST: /Tbx/AppSkinVersion/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        { 
            var model = new AppSkinVersion();
            model.CreateUser = 0;
            this.TryUpdateModel<AppSkinVersion>(model);
            try
            {
                model.AppID = AppID;
                this.TbxService.SaveAppSkinVersion(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View("Edit", model);
            }
            return this.RefreshParent();
        }

        //
        // GET: /Tbx/AppSkinVersion/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.TbxService.GetAppSkinVersion(id);
            //ViewData["AppID"] = new SelectList(this.TbxService.GetAppListByStatus(), "AppID", "AppName", model.AppID);
            ViewData["UpdateType"] = new SelectList(WebControl.GetSelectList(typeof(AppVersionUpdateTypeEnum)), "Value", "Text", model.UpdateType);
            return View(model);
        }

        //
        // POST: /Tbx/AppSkinVersion/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.TbxService.GetAppSkinVersion(id);
            this.TryUpdateModel<AppSkinVersion>(model);
            this.TbxService.SaveAppSkinVersion(model);
            return this.RefreshParent();
        }

        //
        // GET: /Tbx/AppSkinVersion/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Tbx/AppSkinVersion/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
