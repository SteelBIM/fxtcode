using CBSS.Cfgmanager.Contract.DataModel;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Web.Admin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Cfgmanager.Controllers
{
    public class DictItemController : AdminControllerBase
    {
        //
        // GET: /Cfgmanager/DictItem/

        public ActionResult Index(ApiFunctionRequest request)
        {
            int totalcount = 0;
            var list = new PagedList<Sys_DictItem>(this.CfgmanagerService.GetAllSys_DictItem(out totalcount, request), request.PageIndex, PageSize, totalcount);
            ViewData["SystemCode"] = new SelectList(WebControl.GetSelectList(typeof(SystemCodeEnum)), "Value", "Text", request.SystemCode);
            return View(list);
        }

        //
        // GET: /Cfgmanager/DictItem/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Cfgmanager/DictItem/Create

        public ActionResult Create()
        {
            ViewData["selSystemCode"] = new SelectList(WebControl.GetSelectList(typeof(SystemCodeEnum)), "Value", "Text");
            ViewData["selConfigType"] = new SelectList(WebControl.GetSelectList(typeof(ConfigTypeEnum)), "Value", "Text");
            var model = new Sys_DictItem();
            return View("Edit", model);
        }

        //
        // POST: /Cfgmanager/DictItem/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            int selSystemCode = Convert.ToInt32(Request["selSystemCode"]);
            int selConfigType = Convert.ToInt32(Request["selConfigType"]);

            var model =new Sys_DictItem();
            model.CreateUser = 0;
            this.TryUpdateModel<Sys_DictItem>(model);
            try
            {
                model.SystemCode = selSystemCode;
                model.ConfigType = selConfigType;
                model.CreateDate = DateTime.Now;
                this.CfgmanagerService.SaveDictItem(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name,e.Message);
                return View("Edit", model);
            }
            return this.RefreshParent() ;
        }

        //
        // GET: /Cfgmanager/DictItem/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.CfgmanagerService.GetDictItem(id);
            ViewData["selSystemCode"] = new SelectList(WebControl.GetSelectList(typeof(SystemCodeEnum)), "Value", "Text", model.SystemCode);
            ViewData["selConfigType"] = new SelectList(WebControl.GetSelectList(typeof(ConfigTypeEnum)), "Value", "Text", model.ConfigType);
            return View(model);
        }

        //
        // POST: /Cfgmanager/DictItem/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            int selSystemCode = Convert.ToInt32(Request["selSystemCode"]);
            int selConfigType = Convert.ToInt32(Request["selConfigType"]);

            var model = this.CfgmanagerService.GetDictItem(id);
            this.TryUpdateModel<Sys_DictItem>(model);
            model.SystemCode = selSystemCode;
            model.ConfigType = selConfigType;
            model.CreateDate = DateTime.Now;
            this.CfgmanagerService.SaveDictItem(model);
            return this.RefreshParent();
        }

        //
        // GET: /Cfgmanager/DictItem/Delete/5
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
            {
                return RedirectToAction("Index");
            }
            bool flag = this.CfgmanagerService.DeleteDictItem(ids);
            return RedirectToAction("Index");
        }
    }
}
