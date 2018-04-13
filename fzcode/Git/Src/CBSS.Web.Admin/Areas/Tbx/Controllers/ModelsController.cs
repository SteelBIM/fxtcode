using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class ModelsController : ControllerBase
    {
        //
        // GET: /Tbx/MyModels/

        public ActionResult Index()
        {
            if (CheckActionName("Model_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            #region 控制操作权限
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            ViewBag.Down = action.Down;
            ViewBag.Model_ImgLibrary = action.Model_ImgLibrary;
            #endregion

            ViewData["FunctionID"] = new SelectList(WebControl.GetSelectList(typeof(ModelFunctionEnum)), "Value", "Text");
            ViewData["ModelSourceType"] = new SelectList(WebControl.GetSelectList(typeof(ModelResourceType)), "Value", "Text");
            ViewData["ModelType"] = new SelectList(WebControl.GetSelectList(typeof(ModelTypeEnum)), "Value", "Text");
            var model = new Model();
            return View(model);
        }

        /// <summary>
        /// 获取Model分页
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetModelJsonPage(int parentId, int pageNumber, int pageSize)
        {
            int totalcount = 0;
            var model = this.TbxService.GetModelByParentID(parentId, out totalcount, pageNumber, pageSize);
            return Json(new { total = totalcount, rows = model });
        }
        public ActionResult Create()
        {
            ViewData["FunctionID"] = new SelectList(WebControl.GetSelectList(typeof(ModelFunctionEnum)), "Value", "Text");
            ViewData["ModelSourceType"] = new SelectList(WebControl.GetSelectList(typeof(ModelResourceType)), "Value", "Text");
            ViewData["ModelType"] = new SelectList(WebControl.GetSelectList(typeof(ModelTypeEnum)), "Value", "Text");
            var model = new Model();
            return View("Edit", model);
        }
        /// <summary>
        /// 根据ParentId获取Model
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ActionResult GetModelJson(int parentId)
        {
            var model = this.TbxService.GetModelByParentID(parentId);
            return Json(new { total = model.Count(), rows = model });
        }
        public ActionResult GetModelByModelID(int ModelID)
        {
            var model = this.TbxService.GetModel(ModelID);
            return Json(model);
        }
        public ActionResult GetDelModelByModelID(int ModelID)
        {
            try
            {
                int flag = TbxService.DelModelByModelID(ModelID);
                if (flag == 0)
                {
                    return Json("已经生成具体模块的模型不能删除");
                }
                else if (flag == 1)
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
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(FormCollection collection)
        {
            //上传资源模板
            string RelativePath = "";
            HttpPostedFileBase uploadFile = Request.Files["ResourceTemplate"];
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                try
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(uploadFile.FileName);
                    string direction = Server.MapPath("/Upload/Resources/");
                    string destinationPath = Path.Combine(direction, fileName);
                    if (!Directory.Exists(direction))
                    {
                        Directory.CreateDirectory(direction);
                    }
                    uploadFile.SaveAs(destinationPath);
                    RelativePath = "/Upload/Resources/" + fileName;
                }
                catch
                {
                    RelativePath = "";
                }
            }
            var model = new Model();
            model.CreateUser = 0;
            string FunctionID = Request.Form["FunctionID"];
            this.TryUpdateModel<Model>(model);
            try
            {
                model.FunctionID = FunctionID;
                model.ResourceTemplate = RelativePath;
                this.TbxService.SaveModel(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.ViewBag.ParentModel = new SelectList(this.TbxService.GetModelList(), "ModelID", "ModelName");
                return View(model);
            }
            return RedirectToAction("Index/"); //return this.RefreshParent();
        }
        [HttpPost]
        public JsonResult CheckModelName(int ModelID, string ModelName)
        {
            try
            {
                bool flag = TbxService.CheckModelName(ModelID, ModelName);
                return Json(flag);
            }
            catch
            {
                return Json(false);
            }
        }
    }
}
