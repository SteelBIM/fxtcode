using CBSS.Core.Config;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.BLL;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class ModuleController : ControllerBase
    {
        //模块管理
        // GET: /Tbx/Module/

        public ActionResult Index(MarketBookRequest request)
        {
            if (CheckActionName("Module_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewBag.Edit = action.Edit;
         
            return View();
        }

        public JsonResult GetModulePage(int pagesize, int pageindex, int MarketClassifyId, string MarketBookName)
        {
            MarketBookRequest request = new MarketBookRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.MarketClassifyId = MarketClassifyId;
            request.MarketBookName = MarketBookName;

            TreeData treedata = new TreeData();
            List<MarketClassify> MarketClassifyList = TbxService.GetMarketClassifyList(a => 1 == 1).ToList();

            #region   获取书籍分类ID集合
            if (request.MarketClassifyId != null && request.MarketClassifyId > 0)
            {
                List<int> MarketClassifyIdList = treedata.GetMarketClassifyID(request.MarketClassifyId, MarketClassifyList);
                MarketClassifyIdList.Add(Convert.ToInt32(request.MarketClassifyId));

                if (MarketClassifyIdList != null && MarketClassifyIdList.Count() > 0)
                    request.MarketClassifyIdList = "MarketClassifyId in(" + string.Join(",", MarketClassifyIdList) + ")";
            }
            #endregion 

            int totalcount = 0;
            IEnumerable<v_MarketBookModule> list = TbxService.GetMarketBookList(out totalcount, request);

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    item.MarketClassifyName = treedata.GetMarketClassName(item.MarketClassifyId, MarketClassifyList).TrimEnd('_');
                    item.MarketBookName = string.IsNullOrEmpty(item.MarketBookName) ? item.MODBookName : item.MarketClassifyName + item.MarketBookName;
                }
            }

            return Json(new { total = totalcount, rows = list });
        }


        /// <summary>
        /// 加载书籍树形分类
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMarketClassify()
        {
            TreeData treedata = new TreeData();
            bool IsMarkBook = Request["IsMarkBook"].ToBool();
            return Json(treedata.GetMarketClassifyNode(0, TbxService.GetMarketClassifyList(t => t.MarketClassifyID > 0).ToList(), IsMarkBook));
        }

        public ActionResult Create()
        {
            string DefaultModuleName = bindDropDown();
            var model = new Module();
            model.ModuleName = DefaultModuleName;
            return View("Edit", model);
        }
        /// <summary>
        ///  绑定下拉初始化数据,返回书籍名字加模型名字
        /// </summary>
        /// <returns></returns>
        public string bindDropDown(int MarketBookID = 0)
        {
            //课本名称
            if (MarketBookID == 0)
            {
                MarketBookID = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"]);//MarketBookID
            }

            var MarketBookModel = TbxService.GetMarketBook(MarketBookID);
            string MarketBookNameStr = "";
            if (MarketBookModel != null)
            {
                if (!string.IsNullOrEmpty(MarketBookModel.MarketBookName))
                {
                    MarketBookNameStr = new TreeData().GetMarketClassName(MarketBookModel.MarketClassifyId, TbxService.GetMarketClassifyList(a => 1 == 1).ToList()).Replace("_", "") + MarketBookModel.MarketBookName;
                }
                else
                {
                    MarketBookNameStr = MarketBookModel.MODBookName;
                }
            }
            ViewData["MarketBookName"] = MarketBookNameStr;
            //模型名称
            IEnumerable<Model> modelList = this.TbxService.GetModelByParentID(0);
            ViewData["ModelIDShow"] = new SelectList(modelList, "ModelID", "ModelName");
            //试看条件
            ViewData["FreeType"] = new SelectList(WebControl.GetSelectList(typeof(FreeTypeEnum)), "Value", "Text");
            //资源访问方式
            ViewData["SourceAccessMode"] = new SelectList(WebControl.GetSelectList(typeof(SourceAccessModeEnum)), "Value", "Text");
            //MOD资源类型
            ViewData["MODSourceType"] = new SelectList(WebControl.GetSelectList(typeof(MODSourceTypeEnum)), "Value", "Text");


            ViewData["ParentModule"] = new SelectList(TbxService.GetModule(m => m.MarketBookID == MarketBookID && m.ParentID == 0), "ModuleID", "ModuleName");
            //if (MarketBookModel != null)
            //{
            //    return MarketBookNameStr + (modelList != null && modelList.Count() > 0 ? modelList.First().ModelName : "");
            //}
            return "";
        }
        public JsonResult GetModelByParentID(int ModelID)
        {
            IEnumerable<Model> modelList = this.TbxService.GetModelByParentID(ModelID);
            return Json(modelList);
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            //上传资源模板
            string RelativePath = "";
            HttpPostedFileBase uploadFile = Request.Files["SourceAddress"];
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                try
                {
                    string ext = Path.GetExtension(uploadFile.FileName);
                    if (ext == "xls" || ext == "xlsx")
                    {
                        string fileName = Guid.NewGuid() + ext;
                        string direction = Server.MapPath("/Upload/Resources/");
                        string destinationPath = Path.Combine(direction, fileName);
                        if (!Directory.Exists(direction))
                        {
                            Directory.CreateDirectory(direction);
                        }
                        uploadFile.SaveAs(destinationPath);
                        RelativePath = "/Upload/Resources/" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError(uploadFile.FileName, "资源模板格式不正确！");
                        bindDropDown();
                        return View("Edit");
                    }
                }
                catch
                {
                    RelativePath = "";
                }
            }
            int MarketBookID = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"]);//MarketBookID
            var model = new Module();
            model.CreateUser = 0;
            this.TryUpdateModel<Module>(model);
            try
            {
                model.SourceAddress = RelativePath;
                model.MarketBookID = MarketBookID;
                int flag = this.TbxService.SaveModule(model);
                if (flag == 1)
                {
                    return this.RefreshParent("操作成功！");
                }
                else if (flag == 2)
                {
                    ModelState.AddModelError(model.ModelID.ToString(), "模块名称不能重复！");
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
            bindDropDown();
            return View("Edit", model);
        }

        public ActionResult Edit(int id)
        {

            var m = TbxService.GetModule(id);
            int MarketBookID = Convert.ToInt32(Request["MarketBookID"]);
            var MarketBookModel = TbxService.GetMarketBook(MarketBookID);
            //课本名称 
            string MarketBookNameStr = "";
            if (MarketBookModel != null)
            {
                MarketBookNameStr = new TreeData().GetMarketClassName(MarketBookModel.MarketClassifyId, TbxService.GetMarketClassifyList(a => 1 == 1).ToList()).Replace("_", "") + MarketBookModel.MarketBookName;
            }
            ViewData["MarketBookName"] = MarketBookNameStr;
            //模型名称
            Model model = this.TbxService.GetModel(m.ModelID);
            if (model != null && model.ParentID == 0)//只有一级模型
            {
                ViewData["ModelIDShow"] = new SelectList(this.TbxService.GetModelByParentID(0), "ModelID", "ModelName", m.ModelID);
            }
            else
            {
                ViewData["ModelIDShow"] = new SelectList(this.TbxService.GetModelByParentID(0), "ModelID", "ModelName", model.ParentID);
                ViewData["selModelID"] = new SelectList(this.TbxService.GetModelByParentID(model.ParentID), "ModelID", "ModelName", m.ModelID);
            }

            //试看条件
            ViewData["FreeType"] = new SelectList(WebControl.GetSelectList(typeof(FreeTypeEnum)), "Value", "Text", m.FreeType);
            //资源访问方式
            ViewData["SourceAccessMode"] = new SelectList(WebControl.GetSelectList(typeof(SourceAccessModeEnum)), "Value", "Text", m.SourceAccessMode);
            //MOD资源类型
            ViewData["MODSourceType"] = new SelectList(WebControl.GetSelectList(typeof(MODSourceTypeEnum)), "Value", "Text", m.MODSourceType);

            ViewData["ParentModule"] = new SelectList(TbxService.GetModule(mo => mo.MarketBookID == MarketBookID && mo.ParentID == 0 && mo.ModuleID != id), "ModuleID", "ModuleName");

            return View(m);
        }

        public ActionResult Resources(int id)
        {
            var m = TbxService.GetModule(id);
            return View(m);
        }

        [HttpPost]
        public ActionResult Resources(int id, FormCollection collection)
        {
            var model = this.TbxService.GetModule(id);
            //上传资源模板
            string RelativePath = "";
            HttpPostedFileBase uploadFile = Request.Files["SourceAddress"];
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                try
                {
                    string filePath = Server.MapPath("~\\Upload\\Excel\\");
                    string filename = ExcelHelper.GetNewFileName(uploadFile.FileName, filePath, uploadFile);
                    ZipHelper.UnZip(filename, "");
                    string ext = Path.GetExtension(uploadFile.FileName);
                    if (ext == "xls" || ext == "xlsx")
                    {
                        string fileName = Guid.NewGuid() + ext;
                        string direction = Server.MapPath("/Upload/Resources/");
                        string destinationPath = Path.Combine(direction, fileName);
                        if (!Directory.Exists(direction))
                        {
                            Directory.CreateDirectory(direction);
                        }
                        uploadFile.SaveAs(destinationPath);
                        RelativePath = "/Upload/Resources/" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError(uploadFile.FileName, "资源模板格式不正确！");
                        bindDropDown();
                        return View("Edit");
                    }
                }
                catch
                {
                    RelativePath = "";
                }
            }
            return View("Edit", model);
        }





        static int MarketBookID;
        public ActionResult ModuleManage(ModuleManageRequest request = null)
        {
            #region 控制操作权限
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            #endregion
            //int MarketBookID = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"]);//MarketBookID
            //request.MarketBookID = MarketBookID;
            //int totalcount = 0;
            //var list = new PagedList<v_ModuleManage>(TbxService.GetModuleManage(out totalcount, request).ToList(), request.PageIndex, PageSize, totalcount);
            MarketBookID = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"]);//MarketBookID
         
            return View("EditModule");
        }
        public ActionResult GetModuleManageJsonPage(int pagesize, int pageindex)
        {
            #region 控制操作权限
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            #endregion
            ModuleManageRequest request = new ModuleManageRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.MarketBookID = MarketBookID;
            int total = 0;
            var list = TbxService.GetModuleManage(out total, request);
            return Json(new { total = total, rows = list });
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.TbxService.GetModule(id);
            this.TryUpdateModel<Module>(model);
            //上传资源模板
            string RelativePath = "";
            HttpPostedFileBase uploadFile = Request.Files["SourceAddress"];
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                try
                {
                    string ext = Path.GetExtension(uploadFile.FileName);
                    if (ext == ".xls" || ext == ".xlsx")
                    {
                        string fileName = Guid.NewGuid() + ext;
                        string direction = Server.MapPath("/Upload/Resources/");
                        string destinationPath = Path.Combine(direction, fileName);
                        if (!Directory.Exists(direction))
                        {
                            Directory.CreateDirectory(direction);
                        }
                        uploadFile.SaveAs(destinationPath);
                        RelativePath = "/Upload/Resources/" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError(uploadFile.FileName, "资源模板格式不正确！");
                        bindDropDown(model.MarketBookID);
                        return View("Edit");
                    }
                }
                catch
                {
                    RelativePath = "";
                }
                model.SourceAddress = RelativePath;
            }
            else
            {
                string SourceAddressText = Request["SourceAddress"];
                if (!string.IsNullOrEmpty(SourceAddressText))
                {
                    int index = SourceAddressText.LastIndexOf(".");
                    string ext = SourceAddressText.Substring(index, SourceAddressText.Length - index);
                    if (!(ext == ".xls" || ext == ".xlsx"))
                    {
                        ModelState.AddModelError(uploadFile.FileName, "资源模板格式不正确！");
                        bindDropDown(model.MarketBookID);
                        return View("Edit");
                    }
                }
            }
            if (model.SourceAccessMode == 1)
            {
                model.SourceAddress = "";
            }
            int flag = this.TbxService.SaveModule(model);
            if (flag == 1)
            {
                return this.RefreshParent("操作成功！");
            }
            else if (flag == 2)
            {
                ModelState.AddModelError(model.ModelID.ToString(), "模块名称不能重复！");
            }
            else
            {
                ModelState.AddModelError(model.ModelID.ToString(), "操作失败！请稍后再试！");
            }
            bindDropDown(model.MarketBookID);
            return View("Edit", model);
        }


        /// <summary>
        /// 上传图片获取签名验证
        /// </summary>
        /// <returns></returns>
        public ContentResult GetPostObjectSignature()
        {
            string accessKeyId = CachedConfigContext.Current.OSSConfig.accessKeyId;// "LTAIVaiUNugzG5Wm";
            string accessKeySecret = CachedConfigContext.Current.OSSConfig.accessKeySecret;// "lEWqqIK4xFKmtgc1ur3gskaiuH7IQC";
            string host = CachedConfigContext.Current.OSSConfig.host;// "http://fzyouke.oss-cn-shenzhen.aliyuncs.com";
            string bucketName = CachedConfigContext.Current.OSSConfig.bucketName;// "Test";
            Dictionary<string, object> dic = OSSHelper.GetPostObjectSignature(accessKeyId, accessKeySecret, host, bucketName);
            // string strJson = JsonConvert.SerializeObject(dic);
            string strJson = dic.ToJson();
            return Content(strJson);
        }
        /// <summary>
        /// 根据ModuleID删除模块信息
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public JsonResult DelModuleByModuleID(int ModuleID, int MarketBookID)
        {
            try
            {
                int flag = TbxService.DelModuleByModuleID(ModuleID, MarketBookID);
                if (flag == 0)
                {
                    return Json("已被引用的模块不能删除");
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
    }
}
