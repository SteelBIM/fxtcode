using CBSS.Core.Config;
using CBSS.Core.Log;
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
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{
    public class AppModuleController : ControllerBase
    {
        //应用选模块管理
        // GET: /Tbx/AppModule/
        public static string AppID = "";
        public ActionResult Index()
        {
            if (CheckActionName("App_AppModule") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            AppID = Request.RequestContext.RouteData.Values["id"] != null ? Request.RequestContext.RouteData.Values["id"].ToString() : "";//AppID
            return View();
        }
        #region 书籍和分类排序
        public JsonResult GetAppModuleSortPage(int SortType, int pagesize, int pageindex, string MarketBookName)
        {
            MarketBookRequest request = new MarketBookRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.AppID = AppID;
            request.MarketBookName = MarketBookName;
            int total = 0;
            List<ModuleManage> list = GetModuleManageSortList(SortType, request, out total);
            return Json(new { total = total, rows = list });
        }
        public List<ModuleManage> GetModuleManageSortList(int SortType, MarketBookRequest request, out int totalcount)
        {
            MarketClassifyList = TbxService.GetMarketClassifyList(a => 1 == 1);
            List<ModuleManage> list = new List<ModuleManage>();
            IEnumerable<v_AppModule> MarketBookList = TbxService.GetMarketBookByAppIDSortList(SortType, out totalcount, request);
            if (MarketBookList != null && MarketBookList.Count() > 0)
            {
                foreach (var item in MarketBookList)
                {
                    var model = MarketClassifyList.Where(a => a.MarketClassifyID == item.MarketClassifyId).FirstOrDefault();
                    ModuleManage m = new ModuleManage();
                    List<string> moduleStr = new List<string>();
                    IEnumerable<Module> moduleList = TbxService.GetModuleList(a => a.MarketBookID == item.MarketBookID);
                    foreach (var row in moduleList)
                    {
                        moduleStr.Add(row.ModuleName);
                    }
                    m.MarketBookID = item.MarketBookID;
                    m.MarketBookClass = new TreeData().GetMarketClassName(item.MarketClassifyId, MarketClassifyList.ToList());
                    m.MarketBookName = m.MarketBookClass.Replace("_", "") + item.MarketBookName;
                    m.ModuleStr = string.Join(",", moduleStr);
                    m.MarketClassifyID = item.MarketClassifyId;
                    m.BookSort = item.BookSort;
                    m.ClassifySort = item.ClassifySort;
                    list.Add(m);
                }
            }
            return list;
        }
        #endregion

        public JsonResult GetAppModulePage(int pagesize, int pageindex, string MarketBookName)
        {
            int MarketClassifyId = Request["MarketClassifyId"] != null ? Convert.ToInt32(Request["MarketClassifyId"]) : 0;
            MarketBookRequest request = new MarketBookRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.AppID = AppID;
            request.MarketClassifyId = MarketClassifyId;
            request.MarketBookName = MarketBookName;
            int total = 0;
            List<ModuleManage> list = GetModuleManageList(request, out total);
            return Json(new { total = total, rows = list });
        }
        public List<ModuleManage> GetModuleManageList(MarketBookRequest request, out int totalcount)
        {
            MarketClassifyList = TbxService.GetMarketClassifyList(a => 1 == 1);
            List<ModuleManage> list = new List<ModuleManage>();
            //递归获取书籍分类ID集合
            if (request.MarketClassifyId != 0 && request.MarketClassifyId != null)
            {
                MarketClassifyIdList = GetMarketClassifyIdList(request.MarketClassifyId);
                MarketClassifyIdList.Add(Convert.ToInt32(request.MarketClassifyId));
                if (MarketClassifyIdList != null && MarketClassifyIdList.Count() > 0)
                {
                    request.MarketClassifyIdList = "MarketClassifyId in(" + string.Join(",", MarketClassifyIdList) + ")";
                }
            }

            IEnumerable<v_AppModule> MarketBookList = TbxService.GetMarketBookByAppIDList(out totalcount, request);
            if (MarketBookList != null && MarketBookList.Count() > 0)
            {
                foreach (var item in MarketBookList)
                {
                    var model = MarketClassifyList.Where(a => a.MarketClassifyID == item.MarketClassifyId).FirstOrDefault();
                    ModuleManage m = new ModuleManage();
                    List<string> moduleStr = new List<string>();
                    IEnumerable<Module> moduleList = TbxService.GetModuleList(a => a.MarketBookID == item.MarketBookID);
                    foreach (var row in moduleList)
                    {
                        moduleStr.Add(row.ModuleName);
                    }
                    m.MarketBookID = item.MarketBookID;
                    m.MarketBookClass = new TreeData().GetMarketClassName(item.MarketClassifyId, MarketClassifyList.ToList());
                    m.MarketBookName = string.IsNullOrEmpty(item.MarketBookName) ? item.MODBookName : m.MarketBookClass.Replace("_", "") + item.MarketBookName;
                    m.ModuleStr = string.Join(",", moduleStr);
                    m.MarketClassifyID = item.MarketClassifyId;
                    m.BookSort = item.BookSort;
                    m.ClassifySort = item.ClassifySort;
                    list.Add(m);
                }
            }
            return list;
        }
        IEnumerable<MarketClassify> MarketClassifyList = null;
        List<int> MarketClassifyIdList = new List<int>();
        /// <summary>
        /// 递归获取书籍分类ID集合
        /// </summary>
        /// <param name="MarketClassifyId"></param>
        /// <returns></returns>
        public List<int> GetMarketClassifyIdList(int? MarketClassifyId)
        {
            MarketClassify marketList = MarketClassifyList.Where(a => a.ParentId == MarketClassifyId).FirstOrDefault();
            if (marketList != null)
            {
                MarketClassifyIdList.Add(marketList.MarketClassifyID);
                GetMarketClassifyIdList(marketList.MarketClassifyID);
            }
            return MarketClassifyIdList;
        }
        public ActionResult Create()
        {
            var model = new Module();
            return View("Edit", model);
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
            return this.RefreshParent();
        }
        /// <summary>
        /// 书籍MarketBookID
        /// </summary>
        /// <param name="id">MarketBookID</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            string AppID = Request["AppID"] == null ? "0" : Request["AppID"];
            var app = TbxService.GetApp(AppID);
            if (app != null)
            {
                //应用名称 
                ViewData["AppName"] = app.AppName;
            }
            var MarketBookModel = TbxService.GetMarketBook(id);
            //课本名称 
            string MarketBookNameStr = "";
            if (MarketBookModel != null)
            {
                MarketBookNameStr = new TreeData().GetMarketClassName(MarketBookModel.MarketClassifyId, TbxService.GetMarketClassifyList(a => 1 == 1).ToList()).Replace("_", "") + MarketBookModel.MarketBookName;
                if (string.IsNullOrEmpty(MarketBookNameStr))
                {
                    MarketBookNameStr = MarketBookModel.MODBookName;
                }
            }
            ViewData["MarketBookName"] = MarketBookNameStr;
            //模块信息
            ViewData["MarketBookTree"] = GetTreeInfo(id);
            return View();
        }
        /// <summary>
        /// 编辑书籍和分类排序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditSort(int id)
        {
            string AppID = Request["AppID"] == null ? "0" : Request["AppID"];
            int MarketBookID = id;
            int MarketClassifyID = string.IsNullOrEmpty(Request["MarketClassifyID"]) ? 0 : Convert.ToInt32(Request["MarketClassifyID"]);
            var BookSortModel = TbxService.GetAppMarketBookList(a => a.AppID == AppID && a.MarketBookID == MarketBookID && a.MarketClassifyID == MarketClassifyID).First();
            if (BookSortModel != null)
            {
                ViewData["BookSort"] = BookSortModel.Sort;
            }
            var ClassifySortModel = TbxService.GetAppMarketClassifyList(a => a.AppID == AppID && a.MarketClassifyID == MarketClassifyID).First();
            if (ClassifySortModel != null)
            {
                ViewData["ClassifySort"] = ClassifySortModel.Sort;
            }
            ViewData["MarketBookID"] = MarketBookID;
            ViewData["MarketClassifyID"] = MarketClassifyID;
            ViewData["AppID"] = AppID;
            return View();
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSort(FormCollection collection)
        {
            AppID = Request["AppID"] == null ? "0" : Request["AppID"];
            Regex reg = new Regex(@"^([0-9]\d*)$");
            if (!reg.IsMatch(Request["BookSort"].ToString()))
            {
                return this.RefreshParent("排序号只能输入大于零的正整数！");
            }
            if (!reg.IsMatch(Request["ClassifySort"].ToString()))
            {
                return this.RefreshParent("排序号只能输入大于零的正整数！");
            }
            int BookSort = Convert.ToInt32(Request["BookSort"]);
            int ClassifySort = Convert.ToInt32(Request["ClassifySort"]);
            int MarketBookID = Convert.ToInt32(Request["MarketBookID"]);
            int MarketClassifyID = Convert.ToInt32(Request["MarketClassifyID"]);
            bool flag = TbxService.UpdateBookClassifySort(AppID, MarketBookID, MarketClassifyID, BookSort, ClassifySort);
            if (flag)
            {
                return this.RefreshParent("操作成功！");
            }
            else
            {
                return this.RefreshParent("操作失败！");
            }
        }
        /// <summary>
        /// 批量配置目录模块
        /// </summary>
        /// <param name="id">MarketBookID</param>
        /// <returns></returns>
        public ActionResult BatchEdit(int id)
        {
            string AppID = Request["AppID"] == null ? "0" : Request["AppID"];
            var app = TbxService.GetApp(AppID);
            if (app != null)
            {
                //应用名称 
                ViewData["AppName"] = app.AppName;
            }
            var MarketBookModel = TbxService.GetMarketBook(id);
            //课本名称 
            string MarketBookNameStr = "";
            if (MarketBookModel != null)
            {
                MarketBookNameStr = new TreeData().GetMarketClassName(MarketBookModel.MarketClassifyId, TbxService.GetMarketClassifyList(a => 1 == 1).ToList()).Replace("_", "") + MarketBookModel.MarketBookName;
                if (string.IsNullOrEmpty(MarketBookNameStr))
                {
                    MarketBookNameStr = MarketBookModel.MODBookName;
                }
            }
            ViewData["MarketBookName"] = MarketBookNameStr;
            //模块信息
            ViewData["MarketBookTree"] = GetTreeInfo(id);
            return View();
        }
        /// <summary>
        /// 获取目录
        /// </summary> 
        /// <returns></returns>
        public string GetTreeInfo(int MarketBookID)
        {
            string strJson = "";
            try
            {
                List<TreeModel> list = new List<TreeModel>();
                IEnumerable<MarketBookCatalog> marketBookCatalog = TbxService.GetMarketBookCatalogsList(a => a.MarketBookID == MarketBookID);
                if (marketBookCatalog != null && marketBookCatalog.Count() > 0)
                {
                    list = GetMarketClassify(0, marketBookCatalog, MarketBookID);
                }
                strJson = JsonConvertHelper.ToJson<List<TreeModel>>(list);
            }
            catch (Exception)
            {

                throw;
            }
            return strJson;
        }

        /// <summary>
        /// 递归获取
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="marketClassifys"></param>
        /// <returns></returns>
        public List<TreeModel> GetMarketClassify(int? ParentId, IEnumerable<MarketBookCatalog> marketClassifys, int? MarketBookID)
        {
            List<TreeModel> listTreeNodes = new List<TreeModel>();
            IEnumerable<MarketBookCatalog> marketBookCatalog = marketClassifys.Where(a => a.ParentCatalogID == ParentId);
            if (marketBookCatalog != null && marketBookCatalog.Count() > 0)
            {
                foreach (var item in marketBookCatalog)
                {
                    listTreeNodes.Add(new TreeModel()
                    {
                        name = item.MarketBookCatalogName,
                        checkboxValue = item.MarketBookCatalogID.ToString(),
                        Checked = false,
                        children = GetMarketClassify(item.MarketBookCatalogID, marketClassifys, MarketBookID)
                    });
                }
            }
            return listTreeNodes;
        }


        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            //var model = this.TbxService.GetModule(id);
            //this.TryUpdateModel<Module>(model);
            //this.TbxService.SaveModule(model);

            return this.RefreshParent();
        }
        /// <summary>
        /// 获取应用匹配书籍目录模块表
        /// </summary>
        /// <param name="MarketBookID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAppBookCatalogModuleItem(int MarketBookID, int MarketBookCatalogID, string AppID)
        {
            try
            {
                DataTable dt = TbxService.GetAppBookCatalogModuleItemList(MarketBookID, MarketBookCatalogID, AppID);
                string json = JsonConvertHelper.DataTableToJson(dt);
                JsonResult jr = Json(json);
                return jr;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "AppModule获取应用匹配书籍目录模块表GetAppBookCatalogModuleItem", ex);
                return Json("");
            }
        }
        /// <summary>
        /// 获取图片库
        /// </summary>
        /// <returns></returns>
        public JsonResult GetModelImgLibrary(int ModuleID)
        {
            try
            {
                DataTable dt = TbxService.GetModelImgLibraryList(ModuleID);
                string json = JsonConvertHelper.DataTableToJson(dt);
                JsonResult jr = Json(json);
                return jr;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "AppModule获取图片库GetModelImgLibrary", ex);
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult SaveAppBookCatalogModuleItem(string obj)
        {
            try
            {
                AppBookCatalogModuleItem model = JsonConvertHelper.ToObject<AppBookCatalogModuleItem>(obj);
                AppBookCatalogModuleItem m = TbxService.GetAppBookCatalogModuleItemList(a => a.AppBookCatalogModuleItemID == model.AppBookCatalogModuleItemID).SingleOrDefault();
                if (m != null)
                {
                    m.ModuleName = model.ModuleName;
                    m.BeforeBuyingImg = model.BeforeBuyingImg;
                    m.BuyLaterImg = model.BuyLaterImg;
                    m.BeforeBuyingClickImg = model.BeforeBuyingClickImg;
                    m.BuyLaterClickImg = model.BuyLaterClickImg;
                    m.Sort = model.Sort;
                    bool flag = TbxService.SaveAppBookCatalogModuleItem(m);
                    return Json(flag);
                }
                else
                {
                    bool flag = TbxService.SaveAppBookCatalogModuleItem(model);
                    return Json(flag);
                }

            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "SaveAppBookCatalogModuleItem", ex);
                return Json(false);
            }
        }
        /// <summary>
        /// 批量配置目录模块
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="MarketBookID"></param>
        /// <param name="MarketBookCatalogIDs"></param>
        /// <param name="ModuleIDs"></param>
        /// <returns></returns>
        public JsonResult BatchSaveMarketBookCatalogModule(string AppID, int MarketBookID, string MarketBookCatalogIDs, string ModuleIDs, string ModuleDataList)
        {
            try
            {
                List<ModuleData> ListModuleData = JsonConvertHelper.ToObject<List<ModuleData>>(ModuleDataList);
                MarketBookCatalogIDs = MarketBookCatalogIDs.TrimEnd(',');
                string[] strMarketBookCatalogIDs = string.IsNullOrEmpty(MarketBookCatalogIDs) ? new string[] { } : MarketBookCatalogIDs.Split(',');//所选课本目录集合 
                ModuleIDs = ModuleIDs.TrimEnd(',');
                string[] strModuleIDs = string.IsNullOrEmpty(ModuleIDs) ? new string[] { } : ModuleIDs.Split(',');//所选模块集合 
                List<Module> listModule = TbxService.GetModule(a => a.MarketBookID == MarketBookID).ToList();//所有模块集合
                Guid GuidAppID = Guid.Parse(AppID);
                if (listModule != null)
                {
                    //bool flag = TbxService.DelAppBookCatalogModuleItem(a => a.AppID == GuidAppID); 
                    //if (strModuleIDs.Length == 0)
                    //{
                    //    return Json(true);
                    //}
                    var AppBookCatalogModuleItemList = TbxService.GetAppBookCatalogModuleItemList(a => a.AppID == GuidAppID);
                    List<AppBookCatalogModuleItem> list = new List<AppBookCatalogModuleItem>();
                    for (int i = 0; i < strMarketBookCatalogIDs.Length; i++)
                    {
                        int MarketBookCatalogID = Convert.ToInt32(strMarketBookCatalogIDs[i]);
                        //根据APPID和目录修改AppBookCatalogModuleItem的状态为0
                        bool StatusFlag = TbxService.UpdateAppBookCatalogModuleItemStatus(AppID, MarketBookCatalogID);
                        foreach (var item in listModule)
                        {
                            var CatModuleList = AppBookCatalogModuleItemList.Where(a => a.MarketBookCatalogID == MarketBookCatalogID && a.ModuleID == item.ModuleID);
                            //TbxService.GetAppBookCatalogModuleItemList(a => a.AppID == GuidAppID && a.MarketBookCatalogID == MarketBookCatalogID && a.ModuleID == item.ModuleID);
                            var CatModule = CatModuleList != null && CatModuleList.Count() > 0 ? CatModuleList.First() : null;
                            if (CatModule != null)
                            {
                                //选中的模块
                                var ShowModel = ListModuleData.Where(a => a.ModuleID == item.ModuleID).FirstOrDefault();
                                if (ShowModel != null)
                                {
                                    CatModule.ModuleName = ShowModel.ModuleNameShow;
                                    CatModule.BeforeBuyingImg = ShowModel.BeforeBuyingImg;
                                    CatModule.BuyLaterImg = ShowModel.BuyLaterImg;
                                    CatModule.Sort = ShowModel.Sort;
                                    CatModule.Status = 1;
                                    CatModule.BeforeBuyingClickImg = ShowModel.BeforeBuyingClickImg;
                                    CatModule.BuyLaterClickImg = ShowModel.BuyLaterClickImg;
                                    bool flag = TbxService.UpdateAppBookCatalogModuleItem(CatModule);
                                }
                            }
                            else
                            {
                                //新增
                                AppBookCatalogModuleItem model = new AppBookCatalogModuleItem();
                                model.AppID = Guid.Parse(AppID);
                                model.MarketBookCatalogID = Convert.ToInt32(strMarketBookCatalogIDs[i]);
                                model.ModuleID = item.ModuleID;
                                var ShowModel = ListModuleData.Where(a => a.ModuleID == item.ModuleID);
                                if (ShowModel != null && ShowModel.Count() > 0)
                                {
                                    var sModel = ShowModel.First();
                                    model.ModuleName = sModel.ModuleNameShow;
                                    model.BeforeBuyingImg = sModel.BeforeBuyingImg;
                                    model.BuyLaterImg = sModel.BuyLaterImg;
                                    model.Status = 1;
                                    model.Sort = sModel.Sort;
                                    model.BeforeBuyingClickImg = sModel.BeforeBuyingClickImg;
                                    model.BuyLaterClickImg = sModel.BuyLaterClickImg;
                                    list.Add(model);
                                }
                            }
                        }
                    }
                    if (list != null && list.Count > 0)
                    {
                        bool BatchFlag = TbxService.BatchSaveAppBookCatalogModuleItem(list);
                        return Json(BatchFlag);
                    }
                }
                else
                {
                    return Json(false);
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "批量配置目录模块BatchSaveMarketBookCatalogModule", ex);
                return Json(false);
            }
        }
        /// <summary>
        /// 批量保存模块
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult BatchSaveAppBookCatalogModuleItem(string AppID, int MarketBookID, int MarketBookCatalogID, string ModuleIDs)
        {
            try
            {
                ModuleIDs = ModuleIDs.TrimEnd(',');
                string[] strModuleIDs = ModuleIDs.Split(',');

                List<Module> listModule = TbxService.GetModule(a => a.MarketBookID == MarketBookID).ToList();
                Guid GuidAppID = Guid.Parse(AppID);
                //List<AppBookCatalogModuleItem> listModuleItem = TbxService.GetAppBookCatalogModuleItemList(a => a.MarketBookCatalogID == MarketBookCatalogID && a.AppID == GuidAppID).ToList();
                if (listModule != null)
                {
                    foreach (var item in listModule)
                    {
                        var CatModule = TbxService.GetAppBookCatalogModuleItemList(a => a.MarketBookCatalogID == MarketBookCatalogID && a.ModuleID == item.ModuleID && a.AppID == GuidAppID).SingleOrDefault();
                        if (CatModule != null && strModuleIDs.Contains(item.ModuleID.ToString()))
                        {
                            //修改Status状态为1
                            CatModule.Status = 1;
                            bool flag = TbxService.UpdateAppBookCatalogModuleItem(CatModule);
                        }
                        else if (CatModule != null && !strModuleIDs.Contains(item.ModuleID.ToString()))
                        {
                            //修改Status状态为0
                            CatModule.Status = 0;
                            bool flag = TbxService.UpdateAppBookCatalogModuleItem(CatModule);
                        }
                        else if (CatModule == null && strModuleIDs.Contains(item.ModuleID.ToString()))
                        {
                            //新增
                            AppBookCatalogModuleItem model = new AppBookCatalogModuleItem();
                            model.AppID = Guid.Parse(AppID);
                            model.MarketBookCatalogID = MarketBookCatalogID;
                            model.ModuleID = item.ModuleID;
                            model.ModuleName = item.ModuleName;
                            model.BeforeBuyingImg = "";//item.ModuleImg;
                            model.BuyLaterImg = "";//item.ModuleImg;
                            model.Status = 1;
                            bool flag = TbxService.SaveAppBookCatalogModuleItem(model);
                        }
                    }
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "批量保存模块BatchSaveAppBookCatalogModuleItem", ex);
                return Json(false);
            }
        }
        /// <summary>
        /// 批量配置某本书的目录模块
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="MarketBookID"></param>
        /// <returns></returns>
        public JsonResult BatchConfigBookDicModule(string AppID, int MarketBookID)
        {
            bool flag = BatchConfigDicModule(AppID, MarketBookID);
            return Json(flag);
        }
        /// <summary>
        /// 根据AppID和MarketBookID批量配置某本书的目录模块
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="MarketBookID"></param>
        /// <returns></returns>
        public bool BatchConfigDicModule(string AppID, int MarketBookID)
        {
            try
            {
                Guid GuidAppID = Guid.Parse(AppID);
                List<AppBookCatalogModuleItem> list = new List<AppBookCatalogModuleItem>();
                IEnumerable<MarketBookCatalog> marketBookCatalogList = TbxService.GetMarketBookCatalogsList(a => a.MarketBookID == MarketBookID);
                IEnumerable<Module> moduleList = TbxService.GetModule(a => a.MarketBookID == MarketBookID);
                if (marketBookCatalogList != null && marketBookCatalogList.Count() > 0 && moduleList != null && moduleList.Count() > 0)
                {
                    var AppBookCatalogModuleItemList = TbxService.GetAppBookCatalogModuleItemList(a => a.AppID == GuidAppID);
                    foreach (var catalog in marketBookCatalogList)
                    {
                        foreach (var module in moduleList)
                        {
                            var CatModuleList = AppBookCatalogModuleItemList.Where(a => a.MarketBookCatalogID == catalog.MarketBookCatalogID && a.ModuleID == module.ModuleID);
                            if (CatModuleList != null && CatModuleList.Count() > 0)
                            {
                                var CatModule = CatModuleList.Count() > 1 ? CatModuleList.Where(a => a.Status == 1).FirstOrDefault() : CatModuleList.FirstOrDefault();
                                CatModule.Status = 1;
                                bool flag = TbxService.UpdateAppBookCatalogModuleItem(CatModule);
                            }
                            else
                            {
                                list.Add(new AppBookCatalogModuleItem()
                                {
                                    AppID = GuidAppID,
                                    MarketBookCatalogID = catalog.MarketBookCatalogID,
                                    ModuleID = module.ModuleID,
                                    ModuleName = module.ModuleName,
                                    BeforeBuyingImg = "",// module.ModuleImg,
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
                Log4NetHelper.Debug(LoggerType.WebExceptionLog, "根据AppID和MarketBookID批量配置某本书的目录模块BatchConfigDicModule", ex);
                return false;
            }
        }
    }
    /// <summary>
    /// 前台显示模块信息集合
    /// </summary>
    [Serializable]
    public class ModuleData
    {
        public int ModuleID { get; set; }
        public string ModuleNameShow { get; set; }
        public string BeforeBuyingImg { get; set; }
        public string BuyLaterImg { get; set; }
        public int Sort { get; set; }
        public string BeforeBuyingClickImg { get; set; }
        public string BuyLaterClickImg { get; set; }
    }
}
