using System;
using System.Collections.Generic;
using Kingsun.ExamPaper.BLL;
using System.Web.Services;
using Kingsun.ExamPaper.Model;
using System.Linq;
namespace Kingsun.SunnyTask.Web.QuestionModels
{
    public partial class DoQuestion : BasePage
    {
        //private StudentTaskBLL stuTaskBLL = new StudentTaskBLL();
        private QuestionBLL questionBLL = new QuestionBLL();
        private CatalogBLL catalogBLL = new CatalogBLL();
        private StuCatalogBLL stuCataBLL = new StuCatalogBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] == null)
            {
if (string.IsNullOrEmpty(Convert.ToString(Session["userid"])) && Convert.ToInt32(Session["FreeCata"]) != Convert.ToInt32(Request.QueryString["UnitCatalogId"]))//未登录
            {
                Response.Write("<script>parent.location.href='/QuestionModels/Login.aspx'</script>");
                Response.Cookies["returnUrl"].Value = Request.Url.AbsoluteUri;
            }
            }
            
          
            if (!IsPostBack)
            {               
                if (!string.IsNullOrEmpty(Request.QueryString["action"]))
                {
                    ActionPage(Request.QueryString["action"]);
                }
                GetCatalogByUniteCataID();
            }
        }
        public QTb_Catalog UnitCatalog { get; set; }
        public List<QTb_Catalog> Catalogs { get; set; }
        public List<Tb_StuCatalog> StuCatalogs { get; set; }
        private void GetCatalogByUniteCataID()
        {
            string sql = string.Format("parentId='{0}'", Request.QueryString["UnitCatalogId"]);//单元的子级目录
            var catalogs = catalogBLL.GetCatalogList(sql);
            if (catalogs == null) Catalogs = new List<QTb_Catalog>();
            else
            {
                Catalogs = catalogs.OrderBy(o=>o.Sort).ToList();
            }

            var stuCatalogs = stuCataBLL.GetStuCatalogList(Catalogs.Select(o => o.CatalogID.Value).ToArray(), Request.Cookies["UserId"].Value);
            if (stuCatalogs != null && stuCatalogs.Count > 0)
            {
                StuCatalogs = stuCatalogs.ToList();
            }
            else
            {
                StuCatalogs = new List<Tb_StuCatalog>();
            }

            UnitCatalog = catalogBLL.GetCatalogList("catalogid=" + Request.QueryString["UnitCatalogId"]).FirstOrDefault();
        }

        private void ActionPage(string action)
        {
            switch (action)
            {
                case "CheckIsUndo"://学生做做作业模式下，检验是否被撤回
                    WriteResult("");
                    break;
                case "CheckIsSubmit"://学生做作业模式下，校验作业是否提交
                    if (!string.IsNullOrEmpty(Request.Form["StuTaskID"]))
                    {
                        // Tb_StudentTask st = stuTaskBLL.GetStuTaskByID(Request.Form["StuTaskID"]);
                        //if (st != null && !string.IsNullOrEmpty(st.SubmitDate))
                        //{
                        //    WriteErrorResult("作业已提交！");
                        //}
                        //else
                        //{
                        //    WriteResult("");
                        //}
                    }
                    else
                    {
                        WriteErrorResult("获取作业信息失败！");
                    }
                    break;
                case "GetStuTaskQuestions":
                    if (!string.IsNullOrEmpty(Request.Form["catalogId"]))
                    {
                        string where = "  catalogId=" + Request.Form["catalogId"];
                        var questions = questionBLL.GetVQuestionList(where, "sort");
                        if (questions != null && questions.Count > 0)
                        {
                            WriteResult(questions);
                        }
                    }
                    else
                    {
                        WriteErrorResult("获取作业信息失败！");
                    }
                    break;
                case "GetPreviewQuestions":
                    if (!string.IsNullOrEmpty(Request.Form["QuestionID"]))
                    {
                        //IList<SimpleQuestionSet> listQues = questionBLL.GetQuestionListForWeb("", Request.Form["QuestionID"].ToString());
                        //if (listQues == null || listQues.Count == 0)
                        //{
                        //    WriteErrorResult("没有获取到作业信息！");
                        //}
                        //else
                        //{
                        //    WriteResult(listQues);
                        //}
                    }
                    else
                    {
                        WriteErrorResult("获取作业信息失败！");
                    }
                    break;
                case "GetStuWrongQuestions":
                    if (!string.IsNullOrEmpty(Request.Form["StuTaskID"]))
                    {
                        //IList<SimpleQuestionSet> listQues = questionBLL.GetStuWrongQueListForWeb(Request.Form["StuTaskID"].ToString(), Request.Form["TaskID"].ToString());

                        //IList<SimpleQuestionSet> listQues = questionBLL.GetStuWrongQueListForWeb(Request.Form["StuTaskID"].ToString(), Request.Form["TaskID"].ToString());
                        //if (listQues == null || listQues.Count == 0)
                        //{
                        //    WriteErrorResult("没有获取到错题信息！");
                        //}
                        //else
                        //{
                        //    WriteResult(listQues);
                        //}
                    }
                    else
                    {
                        WriteErrorResult("获取作业信息失败！");
                    }
                    break;
                case "GetWrongQueDo": //错题集做题，获取题目
                    if (!string.IsNullOrEmpty(Request.Form["QuestionID"]) || !string.IsNullOrEmpty(Request.Form["UnitID"]))
                    {
                        //string UnitID = Request.Form["UnitID"].ToString();//单元ID
                        //string WrongQueID = Request.Form["QuestionID"].ToString();//错题集中题目的ID
                        //WrongQueBLL wBLL = new WrongQueBLL();
                        //List<SimpleQuestionSet> lsQue = wBLL.GetWrongQueDo(CurrentUserInfo.UserID, UnitID, WrongQueID);
                        //if (lsQue == null || lsQue.Count == 0)
                        //{
                        //    WriteErrorResult("没有获取到错题信息！");
                        //}
                        //else
                        //{
                        //    WriteResult(lsQue);
                        //}
                    }
                    break;
                //查看作业是否完成
                case "CheckIsFinish":
                    if (!string.IsNullOrEmpty(Request.Form["StuTaskID"]) && !string.IsNullOrEmpty(Request.Form["TaskID"]))
                    {
                        if (checkIsUndo(Request.Form["StuTaskID"]))
                        {
                            WriteErrorResult("作业已撤回！");
                        }
                        else
                        {
                            //IList<SimpleQuestionSet> listQues = questionBLL.GetQueList(Request.Form["StuTaskID"].ToString(), Request.Form["TaskID"].ToString(), "");
                            //if (listQues == null || listQues.Count == 0)
                            //{
                            //    WriteErrorResult("没有获取到作业信息！");
                            //}
                            //else
                            //{
                            //    bool isFinish = true;
                            //    for (int i = listQues.Count - 1; i >= 0; i--)
                            //    {
                            //        if (listQues[i].IsDo == 0)
                            //        {
                            //            isFinish = false;
                            //            break;
                            //        }
                            //    }
                            //    WriteResult(isFinish);
                            //}
                        }
                    }
                    else
                    {
                        WriteErrorResult("获取作业信息失败！");
                    }
                    break;
                case "SubmitTask":
                    if (!string.IsNullOrEmpty(Request.Form["StuTaskID"]))
                    {
                        string msg = "";
                        if (checkIsUndo(Request.Form["StuTaskID"]))
                        {
                            msg = "作业已撤回！";
                        }
                        else
                        {
                            // stuTaskBLL.SubmitTask(Request.Form["StuTaskID"].ToString(), CurrentUserInfo.UserID, out msg);
                        }
                        if (!string.IsNullOrEmpty(msg))
                        {
                            WriteResult(msg);
                        }
                        else
                        {
                            WriteResult("");
                        }
                    }
                    else
                    {
                        WriteErrorResult("获取作业信息失败！");
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 检测是否被教师撤回
        /// </summary>
        /// <param name="stuTaskID"></param>
        /// <returns></returns>
        private bool checkIsUndo(string stuTaskID)
        {
            //V_StuTask st = stuTaskBLL.GetStuTask(stuTaskID);
            //if (st != null && st.TaskState.GetValueOrDefault() == 1)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            return true;
        }
    }
}