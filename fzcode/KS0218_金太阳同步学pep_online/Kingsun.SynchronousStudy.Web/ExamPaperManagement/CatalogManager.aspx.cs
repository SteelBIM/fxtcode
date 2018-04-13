using System;
using System.Collections.Generic;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class CatalogManager : BasePage
    {
        public IList<QTb_Edition> listEdition = new List<QTb_Edition>();
        public IList<V_Book> listBook = new List<V_Book>();
        private CatalogBLL catalogBLL = new CatalogBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            listEdition = new EditionBLL().Search("ParentID=0 and IsRemove=0", "ParentID,EditionID");
            listBook = new BookBLL().GetVBookList("IsRemove=0", "EditionID,SubjectID,GradeID,BookReel,BookID");
            if (!IsPostBack)
            {
                ActionInit();
            }
        }

        private void ActionInit()
        {
            string action = "";
            string editionid = "", gradeid = "", bookreel = "", catalogid = "";
            if (!string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数 
            {
                action = Request.QueryString["Action"].Trim().ToLower();//去掉空格并变小写 
            }
            else
            {
                return;
            }
            switch (action)
            {
                case "query":
                    string strWhere = "";
                    if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        strWhere = Request.QueryString["queryStr"];
                    }
                    strWhere = string.IsNullOrEmpty(strWhere) ? " 1=1 " : strWhere;
                    int pageindex = int.Parse(Request.Form["page"].ToString());
                    int pagesize = int.Parse(Request.Form["rows"].ToString());
                    int totalcount, totalpage;
                    IList<V_Catalog> courseList = catalogBLL.GetCatalogPageList(pageindex, pagesize, strWhere + " and CatalogID is not NULL and IsRemove=0", "EditionID,SubjectID,GradeID,BookReel,BookID,Sort", 1, out totalcount, out totalpage);
                    var obj = new { rows = courseList, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "getbook":
                    editionid = Request.Form["EditionID"];
                    gradeid = Request.Form["GradeID"];
                    bookreel = Request.Form["BookReel"];
                    if (!string.IsNullOrEmpty(editionid))
                    {
                        strWhere = " IsRemove=0 ";
                        if (editionid != "0")
                        {
                            strWhere += " and EditionID=" + editionid;
                        }
                        if (gradeid != "0")
                        {
                            strWhere += " and GradeID=" + gradeid;
                        }
                        if (bookreel != "0")
                        {
                            strWhere += " and BookReel=" + bookreel;
                        }
                        listBook = new BookBLL().GetVBookList(strWhere, "EditionID,SubjectID,GradeID,BookReel,BookID");
                        WriteResult(new { BookList = listBook});
                    }
                    else
                    {
                        WriteErrorResult("未获取到版本ID");
                    }
                    break;
                case "changesort":
                    catalogid = Request.Form["CatalogID"];
                    string sort = Request.Form["Sort"];
                    QTb_Catalog catalog = catalogBLL.GetCatalog(Convert.ToInt32(catalogid));
                    if (catalog == null)
                    {
                        WriteErrorResult("目录信息不存在");
                    }
                    else
                    {
                        catalog.Sort = int.Parse(sort);
                        if (catalogBLL.UpdateCatalog(catalog))
                        {
                            WriteResult("");
                        }
                        else
                        {
                            WriteErrorResult("更新排序失败！");
                        }
                    }
                    break;
                case "updatecatalogname":
                    catalogid = Request.Form["CatalogID"];
                    string catalogName = Request.Form["CatalogName"];
                    catalog = catalogBLL.GetCatalog(Convert.ToInt32(catalogid));
                    if (catalog == null)
                    {
                        WriteErrorResult("目录信息不存在");
                    }
                    else
                    {
                        if (catalog.CatalogName == catalogName)
                        {
                            WriteErrorResult("没有变动，无需更新！");
                        }
                        else
                        {
                            catalog.CatalogName = catalogName;
                            if (catalogBLL.UpdateCatalog(catalog))
                            {
                                WriteResult("");
                            }
                            else
                            {
                                WriteErrorResult("更新目录名失败！");
                            }
                        }
                    }
                    break;
                case "removeques":
                    catalogid = Request.Form["CatalogID"];
                    if (string.IsNullOrEmpty(catalogid))
                    {
                        WriteErrorResult("目录信息不存在");
                    }
                    else
                    {
                        if (catalogBLL.RemoveCatalogQuestions(Convert.ToInt32(catalogid)))
                        {
                            WriteResult("");
                        }
                        else
                        {
                            WriteErrorResult("移除失败！");
                        }
                    }
                    break;
                default:
                    Response.Write("");
                    Response.End();
                    break;
            }
        }
    }
}