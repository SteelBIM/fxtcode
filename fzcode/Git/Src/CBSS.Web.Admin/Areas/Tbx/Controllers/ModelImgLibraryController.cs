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
    public class ModelImgLibraryController : ControllerBase
    {
        //模型图片库
        // GET: /Tbx/ModelImgLibrary/

        public ActionResult Index()
        { 
            if (CheckActionName("Model_ImgLibrary") == false)
            {
                return Redirect("~/Account/Auth/Login");
            } 
            return View();
        }

        public JsonResult GetModelImgLibraryPage(int pagesize, int pageindex,int ModelID)
        { 
            ViewData["ModelName"] = this.TbxService.GetModel(ModelID);//模型名称  
            ModelImgLibraryRequest request = new ModelImgLibraryRequest();
            request.ModelID = ModelID;
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            int total = 0;
            var list = TbxService.GetModelImgLibraryList(out total, request);
            return Json(new { total = total, rows = list });
        }
        static int ModelID=0;
        public ActionResult Create()
        {
            ModelID = Convert.ToInt32(Request["ModelID"]);//ModelID
            ViewData["ModelName"] = this.TbxService.GetModel(ModelID);//模型名称 
            ViewData["ImgType"] = new SelectList(WebControl.GetSelectList(typeof(ImgTypeEnum)), "Value", "Text");
            var model = new ModelImgLibrary();
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
            ViewData["ModelName"] = this.TbxService.GetModel(ModelID);//模型名称 
            ViewData["ImgType"] = new SelectList(WebControl.GetSelectList(typeof(ImgTypeEnum)), "Value", "Text");

            var model = new ModelImgLibrary();
            model.ModelID = ModelID;
            model.CreateUser = 0;
            this.TryUpdateModel<ModelImgLibrary>(model);
            try
            {
                int flag = this.TbxService.SaveModelImgLibrary(model);
                if (flag == 1)
                {
                    return this.RefreshParent("操作成功！");
                }
                else if (flag == 2)
                {
                    ModelState.AddModelError(ModelID.ToString(), "图片名称重名");
                }
                else
                {
                    return this.RefreshParent("操作失败！");
                }
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
            }
            return View("Edit", model);
        }
        /// <summary>
        /// 编辑图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var model = this.TbxService.GetModelImgLibrary(id);
            ViewData["ModelName"] = this.TbxService.GetModel(model.ModelID);//模型名称 
            ViewData["ImgType"] = new SelectList(WebControl.GetSelectList(typeof(ImgTypeEnum)), "Value", "Text", model.ImgType);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.TbxService.GetModelImgLibrary(id);
            ViewData["ModelName"] = this.TbxService.GetModel(model.ModelID);//模型名称 
            ViewData["ImgType"] = new SelectList(WebControl.GetSelectList(typeof(ImgTypeEnum)), "Value", "Text");
            this.TryUpdateModel<ModelImgLibrary>(model);
            try
            {
                int flag = this.TbxService.SaveModelImgLibrary(model);
                if (flag == 1)
                {
                    return this.RefreshParent("操作成功！");
                }
                else if (flag == 2)
                {
                    ModelState.AddModelError(id.ToString(), "图片名称重复");
                }
                else
                {
                    return this.RefreshParent("操作失败！");
                }
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
            }
            return View("Edit", model);
        }
        
        public int Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                List<int> listid = new List<int>();
                foreach (string row in ids.Split(','))
                    listid.Add(int.Parse(row.ToString()));

                if (listid != null && listid.Count > 0)
                    this.TbxService.DeleteModelImgLibrary(listid);
            }
            return 1;
        }
    }
}
