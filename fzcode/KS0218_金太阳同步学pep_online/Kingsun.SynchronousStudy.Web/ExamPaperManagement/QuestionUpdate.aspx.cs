using System;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class QuestionUpdate : BasePage
    {
        public Tb_QuestionInfo questionInfo = new Tb_QuestionInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["QuestionID"]))
            {
                questionInfo = new QuestionBLL().GetQuestionInfo(Request.QueryString["QuestionID"]);
            }
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
                case "savequestion":
                    string questionID = Request.Form["QuestionID"];  
                    string questionTitle = Server.HtmlDecode(Request.Form["QuestionTitle"]);
                    string questionModel = Server.HtmlDecode(Request.Form["QuestionModel"]);
                    string questionContent = Server.HtmlDecode(Request.Form["QuestionContent"]);
                    string secondContent = Server.HtmlDecode(Request.Form["SecondContent"]);
                    string mp3Url = Server.HtmlDecode(Request.Form["Mp3Url"]);
                    string imgUrl = Server.HtmlDecode(Request.Form["ImgUrl"]);
                    if (!string.IsNullOrEmpty(questionID))
                    {
                        Tb_QuestionInfo qInfo = new QuestionBLL().GetQuestionInfo(questionID);
                        qInfo.QuestionTitle = questionTitle;
                        qInfo.QuestionModel = questionModel;
                        qInfo.QuestionContent = questionContent;
                        qInfo.SecondContent = secondContent;
                        qInfo.Mp3Url = mp3Url;
                        qInfo.ImgUrl = imgUrl;
                        if (new QuestionBLL().UpdateQuestionInfo(qInfo))
                        {
                            WriteResult("");
                        }
                        else
                        {
                            WriteErrorResult("保存失败！");
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