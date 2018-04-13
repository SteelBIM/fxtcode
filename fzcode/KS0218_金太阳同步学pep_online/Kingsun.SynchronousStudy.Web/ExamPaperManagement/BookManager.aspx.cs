using System;
using System.Collections.Generic;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Web.ExamPaperManagement;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class BookManager : BasePage
    {
        public IList<QTb_Edition> listEdition = new List<QTb_Edition>();
        protected void Page_Load(object sender, EventArgs e)
        {
            listEdition = new EditionBLL().Search("ParentID=0 and IsRemove=0", "ParentID,EditionID");
            if (!IsPostBack)
            {
                ActionInit();
            }
        }

        private void ActionInit()
        {
            string action = "";
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
                    IList<V_Book> courseList = new BookBLL().GetBookPageList(pageindex, pagesize, strWhere + " and IsRemove=0", "EditionID,SubjectID,GradeID,BookReel,BookID", 1, out totalcount, out totalpage);
                    var obj = new { rows = courseList, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                default:
                    Response.Write("");
                    Response.End();
                    break;
            }
        }
    }
}