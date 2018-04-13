using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Common;
using System;
using System.Collections.Generic;
//using Kingsun.SunnyTask.BLL;
//using Kingsun.SunnyTask.Model;
//using Kingsun.SunnyTask.Common;

namespace Kingsun.SunnyTask.Web.QuestionModels
{
    public partial class M1 : BasePage
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
            string newBackUrl = "";
            switch (action)
            {
                case "GetQuestionInfo":
                    if (!string.IsNullOrEmpty(Request.Form["AccessType"]))
                    {
                        //IList<QuestionSet> listQues = new List<QuestionSet>();
                        //IList<Tb_StudentAnswer> saList = new List<Tb_StudentAnswer>();
                        switch (Request.Form["AccessType"])
                        {
                            case "1"://布置作业——作业预览
                            case "2"://作业篮——作业预览
                            case "5"://错题集
                                if (!string.IsNullOrEmpty(Request.Form["QuestionID"]))
                                {
                                    //listQues = questionBLL.GetStuQuestion("", Request.Form["QuestionID"].ToString(), 0);
                                    //listQues = questionBLL.GetScreenQuestion("", Request.Form["QuestionID"], Request.Form["ParentID"], 0);
                                    //if (listQues == null || listQues.Count == 0)
                                    //{
                                    //    WriteErrorResult("没有获取到作业信息！");
                                    //}
                                    //else
                                    //{
                                    //    WriteResult(JsonHelper.EncodeJson(new { QuestionInfo = listQues[0], AnswerList = saList }));
                                    //}
                                }
                                else
                                {
                                    WriteErrorResult("没有获取到题目信息！");
                                }
                                break;
                            case "4"://作业报告——错题重做
                                if (!string.IsNullOrEmpty(Request.Form["QuestionID"]) && !string.IsNullOrEmpty(Request.Form["StuTaskID"]))
                                {
                                    //listQues = questionBLL.GetStuQuestion("", Request.Form["QuestionID"].ToString(), 0);
                                    //listQues = questionBLL.GetScreenQuestion(Request.Form["StuTaskID"].ToString(), Request.Form["QuestionID"].ToString(), Request.Form["ParentID"], 0);
                                    //if (listQues == null || listQues.Count == 0)
                                    //{
                                    //    WriteErrorResult("没有获取到作业信息！");
                                    //}
                                    //else
                                    //{
                                    //    WriteResult(JsonHelper.EncodeJson(new { QuestionInfo = listQues[0], AnswerList = saList }));
                                    //}
                                }
                                else
                                {
                                    WriteErrorResult("没有获取到题目信息！");
                                }
                                break;
                            case "3"://做作业
                                if (!string.IsNullOrEmpty(Request.Form["QuestionID"]))
                                {
                                    string where =string.Format( "questionId='{0}'" ,Request.Form["QuestionID"]);
                                    var questions = questionBLL.GetVQuestionList(where, "sort");
                                  //  var question = questionBLL.GetQuestionInfo(Request.Form["QuestionID"]);
                                    WriteResult(JsonHelper.EncodeJson(new { QuestionInfo = questions[0] }));
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
                case "UploadAudioFile":
                    if (!string.IsNullOrEmpty(Request.Form["BackUrl"]))
                    {
                        // WriteResult(Common.UploadAudio.UploadAudioFile(Request.Form["BackUrl"].ToString(), CurrentUserInfo.UserID));
                        WriteResult(Request.Form["BackUrl"]);
                    }
                    else
                    {
                        WriteErrorResult("保存失败，请重试！");
                    }
                    break;
                case "SaveStuWrongQue":
                    if (!string.IsNullOrEmpty(Request.Form["IsRight"])
                        && !string.IsNullOrEmpty(Request.Form["StuTaskID"])
                        && !string.IsNullOrEmpty(Request.Form["QuestionID"]))
                    {
                        //Tb_StuWrongQue stuWrongQue = new Tb_StuWrongQue();
                        //stuWrongQue.StuTaskID = Request.Form["StuTaskID"];
                        //stuWrongQue.QuestionID = Request.Form["QuestionID"];
                        //stuWrongQue.QuestionNumber = Request.Form["QuestionID"];
                        //stuWrongQue.ErrorDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //if (wqBLL.SaveStuWrongQue(stuWrongQue, Convert.ToInt32(Request.Form["IsRight"])))
                        //{
                        //    WriteResult("");
                        //}
                        //else
                        //{
                        //    WriteErrorResult("保存失败，请重试！");
                        //}
                    }
                    else
                    {
                        WriteErrorResult("获取参数失败，请重试！");
                    }
                    break;
                //新增跟读记录
                case "InsertReadRecord":
                    if (!string.IsNullOrEmpty(Request.Form["StuScore"])
                        && !string.IsNullOrEmpty(Request.Form["SpendTime"])
                        && !string.IsNullOrEmpty(Request.Form["BackUrl"])
                        && !string.IsNullOrEmpty(Request.Form["StuTaskID"])
                        && !string.IsNullOrEmpty(Request.Form["QuestionID"])
                        && !string.IsNullOrEmpty(Request.Form["ParentID"])
                        && !string.IsNullOrEmpty(Request.Form["Round"]))
                    {
                        //Tb_ReadRecord rr = new Tb_ReadRecord();
                        //rr.ReadRecordID = Guid.NewGuid().ToString();
                        //rr.StuID = CurrentUserInfo.UserID;
                        //rr.StuTaskID = Request.Form["StuTaskID"].ToString();
                        //rr.ParentID = Request.Form["ParentID"].ToString();
                        //rr.QuestionID = Request.Form["QuestionID"].ToString();
                        //rr.SpendTime = Convert.ToDecimal(Request.Form["SpendTime"].ToString());
                        //rr.BackUrl = Request.Form["BackUrl"].ToString();
                        //rr.StuAnswer = Common.UploadAudio.UploadAudioFile(Request.Form["BackUrl"].ToString(), CurrentUserInfo.UserID);
                        //rr.StuScore = Convert.ToDecimal(Request.Form["StuScore"].ToString());
                        //rr.Round = Convert.ToInt32(Request.Form["Round"].ToString());
                        //rr.ReadDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //rr.ReadSystem = "Web";
                        //IList<Tb_ReadRecord> rList = new List<Tb_ReadRecord>();
                        //rList.Add(rr);
                        //if (rrBLL.AddReadRecord(rList))
                        //{
                        //    WriteResult(rr);
                        //}
                        //else
                        //{
                        //    WriteErrorResult("保存失败，请重试！");
                        //}
                    }
                    break;
                case "AddStuAnswer":
                    if (!string.IsNullOrEmpty(Request.Form["NewBackUrl"]))
                    {
                        //newBackUrl = Common.UploadAudio.UploadAudioFile(Request.Form["NewBackUrl"].ToString(), CurrentUserInfo.UserID);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request.Form["BackUrl"]))
                        {
                            //newBackUrl = Common.UploadAudio.UploadAudioFile(Request.Form["BackUrl"].ToString(), CurrentUserInfo.UserID);
                        }
                        else
                        {
                            WriteErrorResult("参数缺失，请重试！");
                        }
                    }

                    if (!string.IsNullOrEmpty(Request.Form["StuScore"])
                        && !string.IsNullOrEmpty(Request.Form["SpendTime"])
                        && !string.IsNullOrEmpty(Request.Form["StuTaskID"])
                        && !string.IsNullOrEmpty(Request.Form["QuestionID"])
                        && !string.IsNullOrEmpty(Request.Form["ParentID"])
                        && !string.IsNullOrEmpty(Request.Form["ParentScore"]))
                    {
                        //Tb_StudentAnswer sa = new Tb_StudentAnswer();
                        //sa.StuAnswerID = Guid.NewGuid().ToString();
                        //sa.StudentID = CurrentUserInfo.UserID;
                        //sa.StuTaskID = Request.Form["StuTaskID"].ToString();
                        //sa.QuestionID = Request.Form["QuestionID"].ToString();
                        //sa.ParentID = Request.Form["ParentID"].ToString();
                        //sa.SpendTime = Convert.ToDecimal(Request.Form["SpendTime"].ToString());
                        //if (!string.IsNullOrEmpty(Request.Form["Answer"]))
                        //{
                        //    sa.Answer = Request.Form["Answer"];
                        //}else{
                        //    sa.Answer = newBackUrl;
                        //}
                        //sa.Remark = Request.Form["BackUrl"].ToString();
                        //sa.StuScore = Convert.ToDecimal(Request.Form["StuScore"].ToString());
                        //sa.AnswerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //sa.IsRight = Convert.ToDecimal(Request.Form["StuScore"].ToString()) < 60 ? 0 : 1;
                        //sa.AnswerSystem = "Web";

                        //Tb_StudentAnswer parentSA = new Tb_StudentAnswer();
                        //parentSA.QuestionID = Request.Form["ParentID"].ToString();
                        //parentSA.StuScore = Convert.ToDecimal(Request.Form["ParentScore"].ToString());
                        //parentSA.SpendTime = Convert.ToDecimal(Request.Form["SpendTime"].ToString());
                        //parentSA.IsRight = Convert.ToInt32(Request.Form["ParentIsRight"].ToString());
                        //parentSA.AnswerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //parentSA.AnswerSystem = "Web";
                        //parentSA.StudentID = CurrentUserInfo.UserID;
                        //parentSA.StuTaskID = Request.Form["StuTaskID"].ToString();
                        //IList<Tb_StudentAnswer> saList = new List<Tb_StudentAnswer>();
                        //saList.Add(sa);
                        //saList.Add(parentSA);
                        ////if (new StudentAnswerBLL().NewSaveStuAnswer(saList))
                        ////{
                        ////    WriteResult(new { Answer = sa, NewBackUrl = newBackUrl });
                        ////}
                        //if (new StudentAnswerBLL().NewWaySaveStuAnswer(sa,parentSA))
                        //{
                        //    WriteResult(new { Answer = sa, NewBackUrl = newBackUrl });
                        //}
                        //else {
                        //    WriteResult(newBackUrl, "保存失败，请重试！");
                        //}
                    }
                    break;
                case "UpdateStuAnswer":
                    //转化录音文件
                    if (!string.IsNullOrEmpty(Request.Form["NewBackUrl"]))
                    {
                        //newBackUrl = Common.UploadAudio.UploadAudioFile(Request.Form["NewBackUrl"].ToString(), CurrentUserInfo.UserID);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request.Form["BackUrl"]))
                        {
                            //newBackUrl = Common.UploadAudio.UploadAudioFile(Request.Form["BackUrl"].ToString(), CurrentUserInfo.UserID);
                        }
                        else
                        {
                            WriteErrorResult("参数缺失，请重试！");
                        }
                    }

                    if (!string.IsNullOrEmpty(Request.Form["StuScore"])
                        && !string.IsNullOrEmpty(Request.Form["SpendTime"])
                        && !string.IsNullOrEmpty(Request.Form["StuTaskID"])
                        && !string.IsNullOrEmpty(Request.Form["QuestionID"])
                        && !string.IsNullOrEmpty(Request.Form["ParentID"])
                        && !string.IsNullOrEmpty(Request.Form["ParentScore"]))
                    {
                        //Tb_StudentAnswer sa = new Tb_StudentAnswer();
                        ////sa.StuAnswerID = Request.Form["StuAnswerID"].ToString();
                        ////sa.StudentID = CurrentUserInfo.UserID;
                        //sa.StuTaskID = Request.Form["StuTaskID"].ToString();
                        //sa.QuestionID = Request.Form["QuestionID"].ToString();
                        ////sa.ParentID = Request.Form["ParentID"].ToString();
                        //sa.SpendTime = Convert.ToDecimal(Request.Form["SpendTime"].ToString());
                        //if (!string.IsNullOrEmpty(Request.Form["Answer"]))
                        //{
                        //    sa.Answer = Request.Form["Answer"];
                        //}
                        //else
                        //{
                        //    sa.Answer = newBackUrl;
                        //}
                        //sa.Remark = Request.Form["BackUrl"].ToString();
                        //sa.StuScore = Convert.ToDecimal(Request.Form["StuScore"].ToString());
                        //sa.AnswerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //sa.IsRight = Convert.ToDecimal(Request.Form["StuScore"].ToString()) < 60 ? 0 : 1;
                        //sa.AnswerSystem = "Web";

                        //Tb_StudentAnswer pSA = new Tb_StudentAnswer();
                        //pSA.StuScore = Convert.ToDecimal(Request.Form["ParentScore"].ToString());
                        //pSA.SpendTime = Convert.ToDecimal(Request.Form["parentAddTime"].ToString());
                        //pSA.IsRight = Convert.ToInt32(Request.Form["ParentIsRight"].ToString());
                        //pSA.AnswerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //pSA.AnswerSystem = "Web";
                        //pSA.StuTaskID = Request.Form["StuTaskID"].ToString();
                        //pSA.QuestionID = Request.Form["ParentID"].ToString();
                        //IList<Tb_StudentAnswer> saList = new List<Tb_StudentAnswer>();
                        //saList.Add(sa);
                        ////saList.Add(pSA);
                        ////if (new StudentAnswerBLL().UpdateReadStuAnswer(saList))
                        ////{
                        ////    WriteResult(new { Answer = sa, NewBackUrl = newBackUrl });
                        ////}
                        //if (new StudentAnswerBLL().NewWaySaveStuAnswer(sa,pSA))
                        //{
                        //    WriteResult(new { Answer = sa, NewBackUrl = newBackUrl });
                        //}
                        //else {
                        //    WriteResult(newBackUrl, "保存失败，请重试！");
                        //}
                    }
                    else
                    {
                        WriteResult(newBackUrl, "参数缺失，请重试！");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}