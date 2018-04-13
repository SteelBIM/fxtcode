using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class MarketBookCatalogController : ControllerBase
    {
        //
        // GET: /Tbx/ProductCata/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCatalogsJson(int parentId)
        {
            var classifies = this.TbxService.GetMarketBookCatalogs(parentId);
            return Json(new { total = classifies.Count, rows = classifies });
        }

        private void UpdateCatalogsByBookId(int marketBookID)
        {
            var book = this.TbxService.GetMarketBookByID(marketBookID);
            var response = this.IBSService.GetCatalogsFromDb(book.MODBookID);
            var catas = response.Data as List<V_MarketBookCatalog>;

            ImportCatalog(marketBookID, catas);
        }

        public ActionResult UpdateCatalogCover(int catalogId, string coverUrl)
        {
            this.TbxService.UpdateCatalogCover(catalogId, coverUrl);
            return Json(KingResponse.GetResponse(""));
        }

        void ImportCatalog(int bookId, List<V_MarketBookCatalog> catalogs, int parentCatalogId = 0)
        {
            if (catalogs != null)
            {
                foreach (var c in catalogs)
                {
                    var dataModel = c.ToJson().ToObject<MarketBookCatalog>();
                    dataModel.MarketBookID = bookId;
                    dataModel.ParentCatalogID = parentCatalogId;
                    dataModel.MarketBookID = bookId;

                    var existC = this.TbxService.GetMarketBookCatalogByModId(c.MODBookCatalogID.Value);
                    var cKey = 0;//目录主键
                    if (existC != null)
                    {
                        cKey = existC.MarketBookCatalogID;
                        dataModel.MarketBookCatalogID = existC.MarketBookCatalogID;
                        dataModel.IsShow = existC.IsShow;
                        this.TbxService.UpdateMarketBookCatalog(dataModel);
                    }
                    else
                    {
                        cKey = this.TbxService.AddMarketBookCatalog(dataModel);
                    }
                    if (c.V_MarketBookCatalogs != null && c.V_MarketBookCatalogs.Any())
                    {
                        ImportCatalog(bookId, c.V_MarketBookCatalogs, cKey);//递归导入
                    }
                }
            }

        }

        public ActionResult GetCatalogsByBookId(int marketBookID, bool loadModCata = false)
        {
            if (loadModCata)
            {
                UpdateCatalogsByBookId(marketBookID);//先更新
            }


            var classifies = this.TbxService.GetMarketBookCatalogsList(o => o.MarketBookID == marketBookID && o.ParentCatalogID == 0).ToList();
            return Json(new { total = classifies.Count, rows = classifies });
        }
        public ActionResult GetCatalog(int MarketBookCatalogID)
        {
            var classifies = this.TbxService.GetMarketBookCatalogsList(o => o.MarketBookCatalogID == MarketBookCatalogID);
            return Json(classifies.FirstOrDefault());
        }

        public JsonResult UpdateCatalogStatus(int marketBookCatalogID, int status)
        {
            try
            {
                this.TbxService.UpdateCatalogStatus(marketBookCatalogID, status);
            }
            catch (Exception ex)
            {
                return Json(KingResponse.GetErrorResponse(ex.Message + ex.StackTrace));
            }
            return Json(KingResponse.GetResponse(""));
        }

        /// <summary>
        /// 保存分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Submit()
        {
            try
            {
                MarketBookCatalog MarketBookCatalog = new MarketBookCatalog();
                this.TryUpdateModel(MarketBookCatalog);
                if (MarketBookCatalog.MarketBookCatalogID > 0)
                {
                    this.TbxService.UpdateMarketBookCatalog(MarketBookCatalog);
                }
                else
                {
                    MarketBookCatalog.CreateDate = DateTime.Now;
                    this.TbxService.AddMarketBookCatalog(MarketBookCatalog);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ex.StackTrace);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteMarketBookCatalog(int id)
        {
            var response = this.TbxService.DeleteMarketBookCatalog(id);

            return Json(response);
        }

        ///// <summary>
        ///// 根据子id获取全名
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public string GetFullName(int id, string name)
        //{
        //    name = "";
        //    this.TbxService.GetFullName(id, ref name);
        //    return name;
        //}


    }
}
