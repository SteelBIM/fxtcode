using System;
using System.Collections.Generic;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class SelectItemManager : BasePage
    {
        public IList<Tb_SelectItem> listSelectItem = new List<Tb_SelectItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["QuestionID"]))
            {
                listSelectItem = new SelectItemBLL().GetSelectItemList(Request.QueryString["QuestionID"]);
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
                case "saveselectitem":
                    string questionID = Request.Form["QuestionID"];
                    int qSort = Convert.ToInt32(Request.Form["Sort"]);
                    string qItem = Request.Form["SelectItem"];
                    string imgUrl = Request.Form["ImgUrl"];
                    int isAnswer = Convert.ToInt32(Request.Form["IsAnswer"]);
                    if (!string.IsNullOrEmpty(questionID) || qSort > 0)
                    {
                        if (new SelectItemBLL().UpdateSelectItem(questionID, qSort, isAnswer, imgUrl, qItem))
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