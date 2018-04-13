using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using System.Transactions;
namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class MarketClassifyController : ControllerBase
    {
        //
        // GET: /Tbx/ProductCata/
        public ActionResult Index()
        {
            if (CheckActionName("MarketClassify_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            #region 控制操作权限
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del; 
            ViewBag.MarketClassify_SyncModClassify = action.MarketClassify_SyncModClassify;
            #endregion
            return View();
        }

        public ActionResult GetClassifiesJson(int parentId)
        {
            var classifies = this.TbxService.GetMarketClassifies(parentId);
            classifies.ForEach(o => o.MarketClassifyName = string.IsNullOrWhiteSpace(o.MarketClassifyName) ? o.ModClassifyName : o.MarketClassifyName);
            return Json(new { total = classifies.Count, rows = classifies });
        }

        public ActionResult GetClassfiy(int marketClassifyID)
        {
            var classifies = this.TbxService.GetMarketClassifyList(o => o.MarketClassifyID == marketClassifyID);
            return Json(classifies.FirstOrDefault());
        }

        /// <summary>
        /// 保存分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Submit()
        {
            try
            {
                MarketClassify classify = new MarketClassify();
                this.TryUpdateModel(classify);
                if (classify.MarketClassifyID > 0)
                {
                    this.TbxService.UpdateClassify(classify);
                }
                else
                {
                    classify.CreateDate = DateTime.Now;
                    this.TbxService.AddClassify(classify);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ex.StackTrace);
            }
            return RedirectToAction("Index/");
        }

        public ActionResult DeleteClassify(int id)
        {
            var response = this.TbxService.DeleteClassify(id);

            return Json(response);
        }

        /// <summary>
        /// 根据子id获取全名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFullName(int id, string name)
        {
            name = "";
            this.TbxService.GetFullName(id, ref name);
            return name;
        }

        public JsonResult GetClassifiesAndBooks()
        {
            var reponse = this.IBSService.GetBookSubjects();
            var subjects = reponse.Data as List<MarketClassify>;
            try
            {
                //using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
                //{
                foreach (var subject in subjects)//更新科目
                {
                    #region 更新科目
                    var exist = this.TbxService.GetMarketClassifyList(o => o.MODID == subject.MODID).FirstOrDefault();
                    int key = 0;
                    if (exist != null)
                    {//已存在，更新
                        key = exist.MarketClassifyID;
                        subject.MarketClassifyID = exist.MarketClassifyID;
                        subject.MarketClassifyName = exist.MarketClassifyName;//自定义名称不变

                        this.TbxService.UpdateClassify(subject);
                    }
                    else
                    {
                        //  key = this.TbxService.AddClassify(subject);
                    }
                    #endregion
                    if (key > 0)//科目更新成功，更新版本和书籍
                    {
                        int[] stages = new int[] { 2, 4, 8 };
                        foreach (var stage in stages)
                        {
                            var verBookResponse = this.IBSService.GetBooks(new List<string> { subject.MarketClassifyName }, stage);
                            var versions = verBookResponse.Data as List<V_MarketClassify>;
                            //转datamodel进行导入
                            var dataModelVersions = versions.ToJson().ToObject<List<MarketClassify>>();
                            foreach (var v in dataModelVersions)//更新版本
                            {
                                #region 更新版本
                                v.ParentId = key;
                                var existV = this.TbxService.GetMarketClassifyList(o => o.MODID == v.MODID).FirstOrDefault();
                                int vKey = 0;//记录版本主键
                                if (existV != null)
                                {
                                    vKey = existV.MarketClassifyID;
                                    v.MarketClassifyID = existV.MarketClassifyID;
                                    v.MarketClassifyName = existV.MarketClassifyName;
                                    this.TbxService.UpdateClassify(v);
                                }
                                else
                                {
                                    // vKey = this.TbxService.AddClassify(v);
                                }
                                #endregion
                                var v_version = versions.First(o => o.MODID == v.MODID);

                                var dataModelBooks = v_version.MarketBooks.ToJson().ToObject<List<MarketBook>>();
                                foreach (var book in dataModelBooks)//更新书籍
                                {
                                    #region 更新书籍
                                    var existB = this.TbxService.GetMarketBookByModID(book.MODBookID);
                                    var bKey = 0;//书本主键
                                    book.MarketClassifyId = vKey;//书籍的版本
                                    if (existB != null)
                                    {
                                        bKey = existB.MarketBookID;
                                        book.MarketBookID = existB.MarketBookID;
                                        //书名和封面保留原来的
                                        book.MarketBookCover = existB.MarketBookCover;
                                        book.MarketBookName = existB.MarketBookName;

                                        this.TbxService.UpdateMarketBook(book);
                                    }
                                    else
                                    {
                                        //  bKey = this.TbxService.AddMarketBook(book);
                                    }
                                    //导入目录
                                    //var bookCatalogs = this.IBSService.GetCatalogs(book.MODBookID,2).Data as List<V_MarketBookCatalog>;
                                    //ImportCatalog(bKey, bookCatalogs);
                                    #endregion
                                }
                            }
                        }
                    }
                }
                // scope.Complete();
                //  }
            }
            catch (Exception ex)
            {
                return Json(KingResponse.GetErrorResponse(ex.Message));
            }

            return Json(KingResponse.GetResponse("success"), JsonRequestBehavior.AllowGet);
        }

        public void ImportCatalog(int bookId, List<V_MarketBookCatalog> catalogs, int parentCatalogId = 0)
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
    }
}
