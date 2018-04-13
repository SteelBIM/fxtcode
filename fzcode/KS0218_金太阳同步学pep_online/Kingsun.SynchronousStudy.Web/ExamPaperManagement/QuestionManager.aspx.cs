using System;
using System.Collections.Generic;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class ExamPaperManagementger : BasePage
    {
        public IList<QTb_Edition> listEdition = new List<QTb_Edition>();
        public IList<V_Book> listBook = new List<V_Book>();
        public IList<V_Catalog> listCatalog = new List<V_Catalog>();
        protected void Page_Load(object sender, EventArgs e)
        {
            listEdition = new EditionBLL().Search("ParentID=0 and IsRemove=0", "ParentID,EditionID");
            listBook = new BookBLL().GetVBookList("IsRemove=0", "EditionID,SubjectID,GradeID,BookReel,BookID");
            listCatalog = new CatalogBLL().GetVCatalogList("IsRemove=0", "BookID,Sort");
            if (!IsPostBack)
            {
                ActionInit();
            }
        }

        private void ActionInit()
        {
            string action = "";
            string questionid = "";
            string editionid = "", gradeid = "", bookreel = "", bookid = "";
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
                    IList<V_Question> questionList = new QuestionBLL().GetVQuestionPageList(pageindex, pagesize, strWhere, "EditionID,SubjectID,GradeID,BookReel,BookID,CatalogID,Sort", 1, out totalcount, out totalpage);
                    var obj = new { rows = questionList, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "getbookandcatalog":
                    editionid = Request.Form["EditionID"];
                    gradeid = Request.Form["GradeID"];
                    bookreel = Request.Form["BookReel"];
                    if (!string.IsNullOrEmpty(editionid))
                    {
                        strWhere=" IsRemove=0 ";
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
                        listCatalog = new CatalogBLL().GetVCatalogList(strWhere, "BookID,Sort");
                        WriteResult(new { BookList = listBook, CatalogList = listCatalog });
                    }
                    else
                    {
                        WriteErrorResult("未获取到版本ID");
                    }
                    break;
                case "getcatalog":
                    bookid = Request.Form["BookID"];
                    if (!string.IsNullOrEmpty(bookid))
                    {
                        strWhere = " IsRemove=0 ";
                        if (bookid != "0")
                        {
                            strWhere += " and BookID=" + bookid;
                        }
                        listCatalog = new CatalogBLL().GetVCatalogList(strWhere, "BookID,Sort");
                        WriteResult(new { CatalogList = listCatalog });
                    }
                    else
                    {
                        WriteErrorResult("未获取到教材ID");
                    }
                    break;
                case "changesort":
                    questionid = Request.Form["QuestionID"];
                    string sort = Request.Form["Sort"];
                    Tb_QuestionInfo qInfo = new QuestionBLL().GetQuestionInfo(questionid);
                    if (qInfo == null)
                    {
                        WriteErrorResult("题目信息不存在");
                    }
                    else
                    {
                        qInfo.Sort = int.Parse(sort);
                        if (new QuestionBLL().UpdateQuestionInfo(qInfo))
                        {
                            WriteResult("");
                        }
                        else
                        {
                            WriteErrorResult("更新排序失败！");
                        }
                    }
                    break;
                case "saveanswer":
                    questionid = Request.Form["QuestionID"];
                    if (!string.IsNullOrEmpty(questionid))
                    {
                        BlankAnswerBLL baBLL=new BlankAnswerBLL();
                        Tb_BlankAnswer ba = baBLL.GetBlankAnswer(questionid);
                        if (ba != null)
                        {
                            ba.Answer = Request.Form["Answer"];
                            if (baBLL.UpdateBlankAnswer(ba))
                            {
                                WriteResult("");
                            }
                            else
                            {
                                WriteErrorResult("保存失败");
                            }
                        }
                        else
                        {
                            WriteErrorResult("未查询到答案记录");
                        }
                    }
                    else
                    {
                        WriteErrorResult("未获取到题目ID");
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