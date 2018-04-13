using CBSS.Account.Contract.ViewModel;
using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class AppController : ControllerBase
    {
        //
        // GET: /Tbx/App/

        public ActionResult Index()
        {
            if (CheckActionName("App_AppModule") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewBag.Add = action.Add;
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            ViewBag.App_AppVersion = action.App_AppVersion;
            ViewBag.App_AppModule = action.App_AppModule;
            ViewBag.App_Good = action.App_Good;
            ViewBag.App_AppSkinVersion = action.App_AppSkinVersion;
            return View();
        }
        public JsonResult GetAppPage(int pagesize, int pageindex)
        {
            AppRequest request = new AppRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            int total = 0;
            var list = TbxService.GetAppList(out total, request);
            return Json(new { total = total, rows = list });
        }
        public ActionResult Create()
        {
            var model = new App();
            //  ViewData["MarketBookTree"] =  GetTreeInfo(0).Replace("Checked", "checked");
            return View("Edit", model);
        }
        /// <summary>
        /// 获取市场分类
        /// </summary>
        /// <param name="selectState">0：创建，1：修改</param>
        /// <returns></returns>
        public string GetTreeInfo(string AppID)
        {
            string strJson = "";
            try
            {
                int selectState = 0;
                IEnumerable<AppMarketClassify> appMarketClassify = null;
                IEnumerable<AppMarketBook> appMarketBook = null;
                if (!string.IsNullOrEmpty(AppID))
                {
                    selectState = 1;
                    appMarketClassify = TbxService.GetMarketMarketClassifyList(AppID.ToUpper());
                    appMarketBook = TbxService.GetMarketMarketBookList(AppID.ToUpper());
                }

                List<TreeModel> list = new List<TreeModel>();
                IEnumerable<MarketClassify> marketClassifys = TbxService.GetMarketClassifyList(a => 1 == 1);
                if (marketClassifys != null && marketClassifys.Count() > 0)
                {
                    list = GetMarketClassify(0, marketClassifys, selectState, AppID, appMarketClassify, appMarketBook);
                }
                strJson = JsonConvertHelper.ToJson<List<TreeModel>>(list);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "App获取市场分类GetTreeInfo", ex);
                return "";
            }
            return strJson.Replace("Checked", "checked");
        }
        /// <summary>
        /// 递归获取
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="marketClassifys"></param>
        /// <returns></returns>
        public List<TreeModel> GetMarketClassify(int ParentId, IEnumerable<MarketClassify> marketClassifys, int selectState, string AppID, IEnumerable<AppMarketClassify> appMarketClassify, IEnumerable<AppMarketBook> appMarketBook)
        {
            List<TreeModel> listTreeNodes = new List<TreeModel>();
            IEnumerable<MarketClassify> marketClassify = marketClassifys.Where(a => a.ParentId == ParentId);
            if (marketClassify != null && marketClassify.Count() > 0)
            {
                foreach (var item in marketClassify)
                {
                    listTreeNodes.Add(new TreeModel()
                    {
                        name = string.IsNullOrEmpty(item.MarketClassifyName) ? item.ModClassifyName : item.MarketClassifyName,
                        checkboxValue = item.MarketClassifyID.ToString() + "|0|" + ParentId,
                        Checked = selectState == 0 ? false : appMarketClassify.Count<AppMarketClassify>(amc => amc.MarketClassifyID == item.MarketClassifyID) > 0,
                        children = GetMarketClassify(item.MarketClassifyID, marketClassifys, selectState, AppID, appMarketClassify, appMarketBook)
                    });
                }
            }
            else
            {
                List<MarketBook> marketBookList = TbxService.GetMarketBooks(ParentId);
                if (marketBookList != null && marketBookList.Count() > 0)
                {
                    foreach (var item in marketBookList)
                    {
                        listTreeNodes.Add(new TreeModel()
                        {
                            name = string.IsNullOrEmpty(item.MarketBookName)?item.MODBookName: item.MarketBookName,
                            checkboxValue = item.MarketBookID.ToString() + "|1|" + ParentId,
                            Checked = selectState == 0 ? false : appMarketBook.Count<AppMarketBook>(amk => amk.MarketBookID == item.MarketBookID) > 0
                        });
                    }
                }
            }
            return listTreeNodes;
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new App();
            model.CreateUser = 0;
            this.TryUpdateModel<App>(model);
            try
            {
                string MarketClassifyIDs = Request["MarketClassifyID"];
                if (string.IsNullOrEmpty(MarketClassifyIDs))
                {
                    this.ModelState.AddModelError(MarketClassifyIDs, "请选择课本信息！");
                    return View("Edit", model);
                }
                model.AppName = model.AppName.Trim();
                int flag = this.TbxService.SaveApp(model);
                if (flag == 2)
                {
                    ModelState.AddModelError(model.AppName, "App名称不能重复!");
                    return View("Edit", model);
                }
                else if (flag == 0)
                {
                    return this.RefreshParent("操作失败！");
                }
                //批量添加AppMarketBook表和AppMarketClassify表数据 
                bool IsFlag = BatchInsertInfo(MarketClassifyIDs, model);
                if (IsFlag)
                {
                    return this.RefreshParent("操作成功！");
                }
                else
                {
                    return this.RefreshParent("操作失败！");
                }
            }
            catch (BusinessException e)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "App创建Create", e);
                this.ModelState.AddModelError(e.Name, e.Message);
            }
            return View("Edit", model);
        }
        /// <summary>
        /// 批量添加AppMarketBook表和AppMarketClassify表数据
        /// </summary>
        /// <param name="MarketClassifyIDs"></param>
        /// <param name="model"></param>
        public bool BatchInsertInfo(string MarketClassifyIDs, App model)
        {
            bool flag = true;
            try
            {
                //当前ID 类型 分类or书籍 父级ID
                //2|0|0,12|0|2,14|1|12,
                if (!string.IsNullOrEmpty(MarketClassifyIDs))
                {

                    string[] MarketClassifyBooks = MarketClassifyIDs.TrimEnd(',').Split(',');
                    //批量添加AppMarketClassify表
                    List<AppMarketClassify> appMarketClassify = new List<AppMarketClassify>();
                    List<AppMarketBook> appMarketBooks = new List<AppMarketBook>();
                    foreach (var item in MarketClassifyBooks)
                    {
                        string[] classifybooks = item.TrimEnd('|').Split('|');
                        if (int.Parse(classifybooks[1]) == 0)
                        {
                            appMarketClassify.Add(new AppMarketClassify() { AppID = model.AppID, MarketClassifyID = Convert.ToInt32(classifybooks[0]), ParentId = Convert.ToInt32(classifybooks[2]) });
                        }
                        else if (int.Parse(classifybooks[1]) == 1)
                        {
                            appMarketBooks.Add(new AppMarketBook() { AppID = model.AppID, MarketBookID = Convert.ToInt32(classifybooks[0]), MarketClassifyID = Convert.ToInt32(classifybooks[2]) });
                        }
                    }

                    if (appMarketClassify != null && appMarketClassify.Count() > 0 && appMarketBooks != null && appMarketBooks.Count > 0)
                    {
                        flag = TbxService.AddAppMarketClassifyBatch(appMarketClassify);
                        flag = TbxService.AddAppMarketBookBatch(appMarketBooks);
                        if (flag == false)
                        {
                            return false;
                        }
                    }
                    //批量添加AppMarketBook应用选择书籍表
                    //if (marketBooks != null && marketBooks.Count() > 0)
                    //{
                    //    List<AppMarketBook> appMarketBook = new List<AppMarketBook>();
                    //    foreach (var item in marketBooks)
                    //    {
                    //        appMarketBook.Add(new AppMarketBook() { AppID = model.AppID, MarketBookID = item.MarketBookID });
                    //    }
                    //    if (appMarketBook != null && appMarketBook.Count() > 0)
                    //    {
                    //        flag = TbxService.AddAppMarketBookBatch(appMarketBook);
                    //        if (flag == false)
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //    //批量添加默认的目录配置模块
                    //    //flag = BatchConfigDicModule(marketBooks, model.AppID);
                    //    //if (flag == false)
                    //    //{
                    //    //    return false;
                    //    //}
                    //}

                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "App批量添加AppMarketBook表和AppMarketClassify表数据BatchInsertInfo", ex);
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 批量添加默认的目录配置模块
        /// </summary>
        /// <param name="listMarketBook"></param>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public bool BatchConfigDicModule(List<MarketBook> listMarketBook, string AppID)
        {
            try
            {
                List<AppBookCatalogModuleItem> list = new List<AppBookCatalogModuleItem>();
                foreach (var item in listMarketBook)
                {
                    IEnumerable<MarketBookCatalog> marketBookCatalogList = TbxService.GetMarketBookCatalogsList(a => a.MarketBookID == item.MarketBookID);
                    IEnumerable<Module> moduleList = TbxService.GetModule(a => a.MarketBookID == item.MarketBookID);
                    if (marketBookCatalogList != null && marketBookCatalogList.Count() > 0 && moduleList != null && moduleList.Count() > 0)
                    {
                        foreach (var catalog in marketBookCatalogList)
                        {
                            foreach (var module in moduleList)
                            {
                                list.Add(new AppBookCatalogModuleItem()
                                {
                                    AppID = Guid.Parse(AppID),
                                    MarketBookCatalogID = catalog.MarketBookCatalogID,
                                    ModuleID = module.ModuleID,
                                    ModuleName = module.ModuleName,
                                    BeforeBuyingImg ="",// module.ModuleImg,
                                    BuyLaterImg = "",//module.ModuleImg,
                                    Status = 1
                                });
                            }
                        }
                    }
                }
                if (list != null && list.Count > 0)
                {
                    return TbxService.BatchSaveAppBookCatalogModuleItem(list);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "App批量添加默认的目录配置模块BatchConfigDicModule", ex);
                return false;
            }
        }
        public ActionResult Edit(string id)
        {
            var model = this.TbxService.GetApp(id);
            ViewBag.AppID = model.AppID;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var model = this.TbxService.GetApp(id);
            this.TryUpdateModel<App>(model);
            try
            {
                string MarketClassifyIDs = Request["MarketClassifyID"];
                if (string.IsNullOrEmpty(MarketClassifyIDs))
                {
                    this.ModelState.AddModelError(model.AppID, "请选择课本信息！");
                    return View("Edit", model);
                }
                int flag = this.TbxService.SaveApp(model);
                if (flag == 2)
                {
                    ModelState.AddModelError(model.AppName, "App名称不能重复!");
                    return View("Edit", model);
                }
                else if (flag == 0)
                {
                    return this.RefreshParent("操作失败！");
                }
                //删除AppMarketBook表和AppMarketClassify表数据 
                bool delFlag = TbxService.DelAppMarketBookAppMarketClassify(model.AppID);
                if (delFlag)
                {
                    //批量添加AppMarketBook表和AppMarketClassify表数据
                    bool IsFlag = BatchInsertInfo(MarketClassifyIDs, model);
                    if (IsFlag)
                    {
                        return this.RefreshParent("操作成功！");
                    }
                    else
                    {
                        return this.RefreshParent("操作失败！");
                    }
                }
                else
                {
                    return this.RefreshParent("操作失败！");
                }
            }
            catch (BusinessException e)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "App编辑Edit", e);
                this.ModelState.AddModelError(e.Name, e.Message);
            }
            return View("Edit", model);
        }
        /// <summary>
        /// 根据AppID删除应用
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public JsonResult DelAppByAppID(string AppID)
        {
            try
            {
                int flag = TbxService.DelAppByAppID(AppID);
                if (flag == 0)
                {
                    return Json("该应用被引用不能删除");
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
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "App根据AppID删除应用DelAppByAppID", e);
                this.ModelState.AddModelError(e.Name, e.Message);
                return Json("删除失败");
            }
        }

        /// <summary>
        /// 配置商品策略
        /// </summary>
        /// <param name="id">AppID</param>
        /// <returns></returns>
        public ActionResult SelectGood(string id, FormCollection collection)
        {
            var goodList = this.TbxService.GetGoodList(a => 1 == 1);
            var appGoodItemList = this.TbxService.GetAppGoodItemList(a => a.AppID == id);
            string ckb = Request["ckb"];
            if (Request["state"] != null)//展示
            {
                if (goodList != null && goodList.Count() > 0)
                {
                    List<v_AppGoodItem> list = new List<v_AppGoodItem>();
                    foreach (var item in goodList)
                    {
                        list.Add(new v_AppGoodItem()
                        {
                            GoodID = item.GoodID,
                            GoodName = item.GoodName,
                            GoodWay = item.GoodWay,
                            Status = item.Status,
                            CreateDate = item.CreateDate,
                            CreateUser = item.CreateUser,
                            isCheck = appGoodItemList.Where(a => a.GoodID == item.GoodID).Count() > 0 ? true : false
                        });
                    }
                    return View("SelectGood", list);
                }
            }
            else//提交修改
            {
                if (string.IsNullOrEmpty(ckb))
                {
                    foreach (var item in goodList)
                    {
                        var goodModel = appGoodItemList.Where(a => a.GoodID == item.GoodID).SingleOrDefault(); 
                        if (goodModel != null)//删除
                        {
                            TbxService.DelAppGoodItem(goodModel.AppGoodItemID);
                        }
                    } 
                }
                else
                {
                    string[] GoodIDs = ckb.Split(',');
                    if (goodList != null && goodList.Count() > 0)
                    {
                        foreach (var item in goodList)
                        {
                            var goodModel = appGoodItemList.Where(a => a.GoodID == item.GoodID).SingleOrDefault();
                            if (goodModel == null && GoodIDs.Contains(item.GoodID.ToString()))//不存在,则新增
                            {
                                AppGoodItem appGoodItem = new AppGoodItem()
                                {
                                    AppID = id,
                                    GoodID = item.GoodID
                                };
                                TbxService.SaveAppGoodItem(appGoodItem);
                            }
                            else if (goodModel != null && !GoodIDs.Contains(item.GoodID.ToString()))//删除
                            {
                                TbxService.DelAppGoodItem(goodModel.AppGoodItemID);
                            }
                        }
                    }
                }
                return this.RefreshParent("操作成功！");
            }
            return View("SelectGood", null);
        }
    }
}
