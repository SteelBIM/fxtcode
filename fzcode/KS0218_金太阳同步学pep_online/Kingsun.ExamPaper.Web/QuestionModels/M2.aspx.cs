using Kingsun.ExamPaper.BLL;
using System;
using System.Collections.Generic;


namespace Kingsun.SunnyTask.Web.QuestionModels
{
    public partial class M2 : BasePage
    {
        private QuestionBLL questionBLL = new QuestionBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (CurrentUserInfo == null)
            //{
            //    Response.Write("<script>window.location.replace('../Default.aspx');</script>");
            //    return;
            //}

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["action"]))
                {
                    ActionPage(Request.QueryString["action"]);
                }
            }
        }

        private void ActionPage(string action)
        {
            switch (action)
            {
                case "GetQuestionInfo":
                    if (!string.IsNullOrEmpty(Request.Form["AccessType"]))
                    {
                       // IList<QuestionSet> listQues = new List<QuestionSet>();
                        switch (Request.Form["AccessType"])
                        {
                            case "1"://布置作业——作业预览
                            case "2"://作业篮——作业预览
                            case "5"://错题集
                                if (!string.IsNullOrEmpty(Request.Form["QuestionID"]))
                                {
                                
                                //    listQues = questionBLL.GetScreenQuestion("", Request.Form["QuestionID"].ToString(), "", 0);
                                }
                                else
                                {
                                    WriteErrorResult("没有获取到题目信息！");
                                }
                                break;
                            case "4"://作业报告——错题重做
                                if (!string.IsNullOrEmpty(Request.Form["QuestionID"]) && !string.IsNullOrEmpty(Request.Form["StuTaskID"]))
                                {                                 
                                 //   listQues = questionBLL.GetScreenQuestion(Request.Form["StuTaskID"].ToString(), Request.Form["QuestionID"].ToString(), "", 0);
                                }
                                else
                                {
                                    WriteErrorResult("没有获取到题目信息！");
                                }
                                break;
                            case "3"://做作业
                                if (!string.IsNullOrEmpty(Request.Form["catalogId"]))
                                {
                                    string where = "  catalogId=" + Request.Form["catalogId"];
                                    var questions = questionBLL.GetVQuestionList(where, "sort");
                                    questions.Insert(0, new ExamPaper.Model.V_Question { QuestionID = Guid.NewGuid().ToString() });
                                    WriteResult(questions);
                                }
                                else
                                {
                                    WriteErrorResult("没有获取到作业信息！");
                                }
                                break;
                            default:
                                break;
                        }
                       
                    }
                    else
                    {
                        WriteErrorResult("获取访问来源失败！");
                    }
                    break;
                case "UploadAudioFile"://实时上传音频，获取存储路径                   
                    break;
                case "SaveStuWrongQue":
                    if (!string.IsNullOrEmpty(Request.Form["IsRight"])
                        && !string.IsNullOrEmpty(Request.Form["StuTaskID"])
                        && !string.IsNullOrEmpty(Request.Form["QuestionID"]))
                    {
                        
                    }
                    else
                    {
                        WriteErrorResult("获取参数失败，请重试！");
                    }
                    break;
                    //插入跟读记录
                case "InsertReadRecord":
                    if (!string.IsNullOrEmpty(Request.Form["FormData"]))
                    {
                        string allbackurl = "";
                        //ReadRecord readRecord = JsonHelper.DecodeJson<ReadRecord>(Request.Form["FormData"]);
                        //IList<Tb_ReadRecord> rrList = new List<Tb_ReadRecord>();
                        //Tb_ReadRecord parentRR = new Tb_ReadRecord();
                        //parentRR.ReadRecordID = Guid.NewGuid().ToString();
                        //parentRR.StuID = CurrentUserInfo.UserID;
                        //parentRR.StuTaskID = readRecord.StuTaskID;
                        //parentRR.QuestionID = readRecord.ParentID;
                        //parentRR.SpendTime = readRecord.SpendTime;
                        //parentRR.StuScore = readRecord.StuScore;
                        //parentRR.Round = readRecord.Round;
                        //parentRR.ReadDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //parentRR.ReadSystem = "Web";
                        //rrList.Add(parentRR);
                        //foreach (Tb_ReadRecord item in readRecord.ReadRecordList)
                        //{
                        //    Tb_ReadRecord rr = new Tb_ReadRecord();
                        //    rr.ReadRecordID = Guid.NewGuid().ToString();
                        //    rr.StuID = CurrentUserInfo.UserID;
                        //    rr.StuTaskID = readRecord.StuTaskID;
                        //    rr.ParentID = readRecord.ParentID;
                        //    rr.QuestionID = item.QuestionID;
                        //    rr.SpendTime = item.SpendTime;
                        //    rr.BackUrl = item.BackUrl;
                        //    rr.StuAnswer = Common.UploadAudio.UploadAudioFile(item.BackUrl, CurrentUserInfo.UserID);
                        //    allbackurl += ";" + rr.StuAnswer;
                        //    rr.StuScore = item.StuScore;
                        //    rr.Round = readRecord.Round;
                        //    rr.ReadDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //    rr.ReadSystem = "Web";
                        //    rrList.Add(rr);
                        //}
                        //if (rrBLL.AddReadRecord(rrList))
                        //{
                        //    WriteResult(allbackurl == "" ? "" : allbackurl.Substring(1));
                        //}
                        //else
                        //{
                        //    WriteErrorResult("保存失败，请重试！");
                        //}
                    }
                    break;
                case "AddStuAnswer":
                    //转化录音路径
                    string newbackurl = "";
                    
                    break;
                case "UpdateStuAnswer":
                    //转化录音路径
                 
                    break;
                case "UploadUrl":
                 
                    break;
                default:
                    break;
            }
        }
    }

    public class ReadRecord
    {
        public string StuTaskID { get; set; }
        public string ParentID { get; set; }
        public decimal StuScore { get; set; }
        public decimal SpendTime { get; set; }
        public int Round { get; set; }
       // public IList<Tb_ReadRecord> ReadRecordList { get; set; }
    }
}