using CBSS.Core.Utility;
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
    public class AppVersionController : ControllerBase
    {
        //
        // GET: /Tbx/AppVersion/

        public ActionResult Index()
        {
            if (CheckActionName("App_AppVersion") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text");
            AppID = Request.RequestContext.RouteData.Values["id"].ToString();//AppID
            return View();
        }
        static string AppID = "";
        public JsonResult GetAppVersionPage(int pagesize, int pageindex, string AppType)
        {
            AppVersionRequest request = new AppVersionRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.AppType = Convert.ToInt32(AppType);
            request.AppID = AppID;
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text", request.AppType);
            int total = 0;
            var list = TbxService.GetAppVersionList(out total, request);
            return Json(new { total = total, rows = list });
        }
        public ActionResult Create()
        {
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text");
            ViewData["AppID"] = new SelectList(this.TbxService.GetAppListByStatus(), "AppID", "AppName");//应用名称
            //ViewData["AppVersionUpdateType"] = new SelectList(WebControl.GetSelectList(typeof(AppVersionUpdateTypeEnum)), "Value", "Text");//应用版本号更新类型
            var model = new AppVersion();
            return View("Edit", model);
        }
        /// <summary>
        /// 提交新建
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text");
            //ViewData["AppVersionUpdateType"] = new SelectList(WebControl.GetSelectList(typeof(AppVersionUpdateTypeEnum)), "Value", "Text");//应用版本号更新类型
            var model = new AppVersion();
            model.CreateUser = 0;
            this.TryUpdateModel<AppVersion>(model);
            try
            {
                model.AppID = AppID;
                model.Status = 2;
                model.AppVersionUpdateType = "1";
                if (model.AppType < 0)
                {
                    ModelState.AddModelError(model.AppID, "请选择应用类型!");
                    return View("Edit", model);
                }
                int flag = this.TbxService.SaveAppVersion(model);
                if (flag == 2)
                {
                    ModelState.AddModelError(model.AppID, "App版本号不能重名!");
                    return View("Edit", model);
                }
                else if (flag == 0)
                {
                    ModelState.AddModelError(model.AppID, "操作失败！请稍后再试!");
                    return View("Edit", model);
                }
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                //ViewData["AppType"] = new SelectList(this.AppService.GetAppListByStatus(), "AppID", "AppName");
                return View("Edit", model);
            }
            string content = string.Format("<script>parent.location.href='{0}'</script>", "/Tbx/AppVersion/Index/" + AppID + "?AppType=" + model.AppType);
            return this.Content(content);
        }
        public ActionResult Edit(int id)
        {
            var model = this.TbxService.GetAppVersion(id);
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text", model.AppType);
            ViewData["AppID"] = new SelectList(this.TbxService.GetAppListByStatus(), "AppID", "AppName", model.AppID);
            //ViewData["AppVersionUpdateType"] = new SelectList(WebControl.GetSelectList(typeof(AppVersionUpdateTypeEnum)), "Value", "Text", model.AppVersionUpdateType);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            ViewData["AppType"] = new SelectList(WebControl.GetSelectList(typeof(AppTypeEnum)), "Value", "Text");
            //ViewData["AppVersionUpdateType"] = new SelectList(WebControl.GetSelectList(typeof(AppVersionUpdateTypeEnum)), "Value", "Text");//应用版本号更新类型
            var model = this.TbxService.GetAppVersion(id);
            this.TryUpdateModel<AppVersion>(model);
            model.AppVersionUpdateType = "1";
            if (model.AppType < 0)
            {
                ModelState.AddModelError(model.AppID, "请选择应用类型!");
                return View("Edit", model);
            }
            int flag = this.TbxService.SaveAppVersion(model);
            if (flag == 2)
            {
                ModelState.AddModelError(model.AppID, "App版本号不能重名!");
                return View("Edit", model);
            }
            else if (flag == 0)
            {
                ModelState.AddModelError(model.AppID, "操作失败！请稍后再试!");
                return View("Edit", model);
            }
            return this.RefreshParent();
        }
        /// <summary>
        /// 修改版本状态
        /// </summary>
        /// <param name="AppVersionID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public JsonResult UpdateStatus(int AppVersionID, int Status)
        {
            try
            {
                var model = this.TbxService.GetAppVersion(AppVersionID);
                if (model != null)
                {
                    model.Status = Status;
                    this.TbxService.SaveAppVersion(model);
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch
            {
                return Json(false);
            }
        }

        /// <summary>
        /// 根据AppVersionID删除应用版本
        /// </summary>
        /// <param name="AppVersionID"></param>
        /// <returns></returns>
        public JsonResult DelAppVersion(int AppVersionID)
        {
            try
            {
                var list = TbxService.GetAppVersion(a => a.AppVersionID == AppVersionID && a.Status == 1);
                if (list.Count() > 0)
                {
                    return Json("该版本已启用不能删除！");
                }
                bool flag = TbxService.DelAppVersion(AppVersionID);
                if (flag)
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
