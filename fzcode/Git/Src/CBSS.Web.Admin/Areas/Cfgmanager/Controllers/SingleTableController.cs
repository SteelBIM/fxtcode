using CBSS.Cfgmanager.BLL;
using CBSS.Cfgmanager.Contract;
using CBSS.Cfgmanager.Contract.ViewModel;
using CBSS.Cfgmanager.IBLL;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Cfgmanager.Controllers
{
    public class SingleTableController : ControllerBase
    {
        //
        // GET: /Cfgmanager/SingleTable/

        //
        // GET: /SingleTable/
        ICfgmanagerService dbConfigService = new CfgmanagerService();
        //public ActionResult Index()
        //{
        //    //string OutputDirectory = Server.MapPath("~/Web.config"); ;
        //    //for (int i = 0; i < 2; i++)
        //    //    OutputDirectory = OutputDirectory.Substring(0, OutputDirectory.LastIndexOf('\\'));
        //    //ViewBag.OutputDirectory = OutputDirectory;
        //    //ViewBag.UserName = "";

        //    return View();
        //}

        public ActionResult Index(string dbName, string keyword, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            if (CheckActionName("SingleTable_View") == false)
            {
                return Redirect("~/Account/Auth/Login");
            }
            ViewBag.Add = action.Add;
            ViewBag.SingleTable_FieldList = action.SingleTable_FieldList;
            if (string.IsNullOrEmpty(dbName))
            {
                return RedirectToAction("Index/", new { dbName = "Tbx" });
            }
            var data = dbConfigService.GetTableList(dbName, keyword);
            var model = new PagedList<DataBaseTableEntity>(data, pageIndex, pageSize);
            var dbs = dbConfigService.GetDBList();
            var listItem = new List<SelectListItem>();
            listItem = dbs.Select(o => new SelectListItem { Text = o.DbName }).ToList();
            ViewBag.DbList = listItem;

            return View(model);
        }

        //public ActionResult FieldListView()
        //{
        //    return View();

        //}

        public JsonResult GetDBList()
        {
            //   string xmlPath = Server.MapPath("~/Config/DaoConfig.xml");
            var dbs = dbConfigService.GetDBList();
            dbs.ForEach(o => o.ConnectString = "");
            return Json(dbs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetTableListJson(string dbName, string keyword)
        {
            var data = dbConfigService.GetTableList(dbName, keyword);
            return Json((data.ToJson()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTableFiledList(string dbName, string tableName, string filed = null)
        {
            var data = dbConfigService.GetTableFiledList(dbName, tableName);
            return Json((data.ToJson()), JsonRequestBehavior.AllowGet);
        }


        public ActionResult TableFiledListView(string dbName, string tableName, string filed = null)
        {
            var data = dbConfigService.GetTableFiledList(dbName, tableName);
            data.ForEach(o =>
            {
                if (o.datatype.Contains("char") && o.length == -1) {
                    o.datatype += "(max)";
                }
            });
            var model = new PagedList<DataBaseTableFieldEntity>(data, 1, int.MaxValue);
            return View(model);
        }

        public ActionResult AlterTableField(string dbName, string tableName, DataBaseTableFieldEntity entity)
        {
            bool success = false;
            entity.isnullable = entity.isnullable == "on" ? "true" : "false";
            entity.identity = entity.identity == "on" ? "true" : "false";
            entity.key = entity.key == "on" ? "true" : "false";
            if (string.IsNullOrWhiteSpace(entity.oldcolumn))
            {
                return AddTableColumn(dbName, tableName, entity); ;
            }
            string error = "";
            try
            {
                success = dbConfigService.AlterTableColumn(dbName, tableName, entity.ToJson());
                if (success)
                {
                    return Content("<script>window.onload=function(){window.parent.location.reload()}</script>");

                    //   Response.Write("<script>parent.window.location.href('" + Url.Action("TableFiledListView", new { dbName = dbName, tableName = tableName }) + "')</script>");
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(new { Success = success, Error = error });
        }

        public ActionResult AlterTableColumnView(DataBaseTableFieldEntity filedEntity)
        {
            return View(filedEntity);
        }

        public ActionResult AddTableColumn(string dbName, string tableName, DataBaseTableFieldEntity entity)
        {
            bool success = false;
            string error = "";
            try
            {
                success = dbConfigService.AddTableColumn(dbName, tableName, entity.ToJson());
                if (success)
                {
                    return Content("<script>window.onload=function(){window.parent.location.reload()}</script>");

                    //Response.Write("<script>parent.window.location.href('" + Url.Action("TableFiledListView", new { dbName = dbName, tableName = tableName }) + "')</script>");
                }
            }

            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(new { Success = success, Error = error });
        }
        protected virtual ActionResult ToJsonResult(object data)
        {
            return Content(data.ToJson());
        }

        public ActionResult TableManage()
        {
            return View();
        }

        public ActionResult AddTable(string dbName, string tableName, string des)
        {
            bool success = false;
            string error = "";
            try
            {
                success = dbConfigService.AddTable(dbName, tableName, des);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(new { Success = success, Error = error });
        }

        public ActionResult AddTableView()
        {
            var dbs = dbConfigService.GetDBList();
            var listItem = new List<SelectListItem>();
            listItem = dbs.Select(o => new SelectListItem { Text = o.DbName }).ToList();
            ViewBag.DbList = listItem;
            return View();
        }

        public ActionResult DropTableField(string dbName, string tableName, string field)
        {
            bool success = false;
            string error = "";
            try
            {
                success = dbConfigService.DropField(dbName, tableName, field);
                if (success)
                {
                    return RedirectToAction("TableFiledListView",new { dbName =dbName, tableName =tableName});
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return Content(string.Format("<script>alert({0})</script>",ex.Message + ex.StackTrace));
            }
            return Content("");
        }

        public ActionResult DropTable(string dbName, string tableName)
        {
            bool success = false;
            string error = "";
            try
            {
                success = dbConfigService.DropTable(dbName, tableName);
                if (success)
                {
                    return RedirectToAction("Index/", new { dbName = dbName });
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(new { Success = success, Error = error }, JsonRequestBehavior.AllowGet);
        }

    }
}
