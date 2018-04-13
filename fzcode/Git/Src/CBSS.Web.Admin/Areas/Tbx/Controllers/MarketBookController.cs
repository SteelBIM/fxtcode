using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.BLL;
using CBSS.Framework.Contract.API;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class MarketBookController : ControllerBase
    {
        //模型管理
        // GET: /Tbx/Model/

        public ActionResult Index()
        {
            if (CheckActionName("MarketBook_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            #region 控制操作权限
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            ViewBag.MarketBook_SyncModBook = action.MarketBook_SyncModBook;
            ViewBag.MarketBook_Catalogs = action.MarketBook_Catalogs;
            #endregion
            return View();
        }

        public JsonResult GetMarketBookPage(int pagesize, int pageindex, int MarketClassifyId, string MarketBookName)
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
                    item.MarketClassifyName = treedata.GetMarketClassName(item.MarketClassifyId, MarketClassifyList);
            }
            return Json(new { total = totalcount, rows = list });
        }

        public JsonResult GetMODBooksByMarketClassify(int classifyID, int stage)
        {
            var classifies = this.TbxService.GetMarketClassifyList(o => o.MarketClassifyID == classifyID);
            if (classifies.Any())
            {
                var classify = classifies.First();
                var response = this.IBSService.GetBooksFromIbsDb(classify.MODID.HasValue ? classify.MODID.Value : 0, stage);
                return Json(response);
            }
            return Json(APIResponse.GetErrorResponse(ErrorCodeEnum.未找到课本));
        }
        public ActionResult Create()
        {
            return View("Edit", new MarketBook());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            var model = new MarketBook();
            model.CreateUser = 0;
            this.TryUpdateModel<MarketBook>(model);
            try
            {
                this.TbxService.AddMarketBook(model);
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
            ViewData["MarketClassifyId"] = new List<SelectListItem>();

            var model = this.TbxService.GetMarketBookByID(id);

            var classifies = this.TbxService.GetMarketClassifyList(o => o.MarketClassifyID == model.MarketClassifyId).FirstOrDefault();
            if (classifies != null)
                ViewBag.MarketClassifyName = string.IsNullOrWhiteSpace(classifies.MarketClassifyName) ? classifies.ModClassifyName : classifies.MarketClassifyName;

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {

            //上传资源模板
            var model = new MarketBook();
            this.TryUpdateModel<MarketBook>(model);
            try
            {
                if (model.MarketBookID > 0)
                {
                    this.TbxService.UpdateMarketBook(model);
                }
                else
                {
                    this.TbxService.AddMarketBook(model);
                }

            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (BusinessException e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
                return View("Edit", model);
            }
            return this.RefreshParent();
        }

        // POST: /Account/Role/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var response = this.TbxService.DeleteMarketBook(id);
            return Json(response);
        }

        public ActionResult GetSubjectList()
        {
            return Json(this.IBSService.GetSubjectList());
        }

        public ActionResult GetVersionList(string subject, int? stage)
        {
            List<V_MarketClassify> list = new List<V_MarketClassify>();
            if (stage.HasValue)
            {
                return Json(this.IBSService.GetVersionList(new List<string> { subject }, stage.Value));
            }
            else
            {
                int[] stages = new int[] { 2, 4, 8 };
                foreach (var s in stages)
                {
                    var modList = this.IBSService.GetVersionList(new List<string> { subject }, s);
                    foreach (var m in modList)
                    {
                        if (list.Any(o => o.MODID == m.MODID)) continue;
                        list.Add(m);
                    }
                }
            }
            return Json(list);
        }

        public ActionResult GetBookList(string subject, long? ver, int stage)
        {
            return Json(this.IBSService.GetBookList(new List<string> { subject }, new List<long?> { ver }, stage));
        }
    }
}
