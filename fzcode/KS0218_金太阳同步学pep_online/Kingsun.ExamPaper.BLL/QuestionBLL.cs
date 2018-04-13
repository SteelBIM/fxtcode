using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace Kingsun.ExamPaper.BLL
{
    public class QuestionBLL
    {
        public IList<V_Question> GetVQuestionList(string strWhere, string orderby = "")
        {
            return new QuestionDAL().GetVQuestionList(strWhere, orderby);
        }

        /// <summary>
        /// 获取题目信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="catalogid">目录ID</param>
        /// <param name="isdo">0:查看，1:做题</param>
        /// <returns></returns>
        public IList<Custom_Question> GetQuestionList(string userid, int catalogid, int isdo)
        {
            DataSet ds = new QuestionDAL().GetQuestionList(userid, catalogid, isdo);
            if (ds != null && ds.Tables.Count >0)
            {
                IList<Custom_Question> listQue = new List<Custom_Question>();
                DataTable dtQue = ds.Tables[0];//题目
                DataTable dtBlank = ds.Tables[1];//填空答案
                DataTable dtSel = ds.Tables[2];//填空答案
                DataTable dtStuAnswer = ds.Tables.Count > 3 ? ds.Tables[3] : null;//学生答案
                //01.获取题目
                for (int i = 0; i < dtQue.Rows.Count; i++)
                {
                    DataRow drQue = dtQue.Rows[i];
                    Custom_Question cusQue = new Custom_Question();
                    cusQue.QuestionID = drQue["QuestionID"].ToString();
                    cusQue.QuestionTitle = drQue["QuestionTitle"].ToString();
                    cusQue.QuestionModel = drQue["QuestionModel"].ToString();
                    cusQue.QuestionContent = drQue["QuestionContent"].ToString();
                    cusQue.SecondContent = drQue["SecondContent"].ToString();
                    cusQue.Mp3Url = drQue["Mp3Url"].ToString();
                    cusQue.ImgUrl = drQue["ImgUrl"].ToString();
                    cusQue.ParentID = drQue["ParentID"].ToString();
                    cusQue.CatalogID = Convert.ToInt32(drQue["CatalogID"].ToString());
                    cusQue.Sort = Convert.ToInt32(drQue["Sort"].ToString());
                    cusQue.MinQueCount = Convert.ToInt32(drQue["MinQueCount"].ToString());
                    cusQue.Score = float.Parse(drQue["Score"].ToString());
                    cusQue.BlankAnswer=new List<Tb_BlankAnswer>();
                    if (dtBlank != null && dtBlank.Rows.Count > 0)
                    {
                        //02.获取填空题答案
                        DataRow[] drBlank = dtBlank.Select("QuestionID='" + cusQue.QuestionID + "'");
                        for (int j = 0; j < drBlank.Length; j++)
                        {
                            cusQue.BlankAnswer.Add(new Tb_BlankAnswer
                            {
                                QuestionID = cusQue.QuestionID,
                                Answer = drBlank[j]["Answer"].ToString(),
                                AnswerType = Convert.ToInt32(drBlank[j]["AnswerType"].ToString())
                            });
                        }
                    }
                    if (dtSel != null && dtSel.Rows.Count > 0)
                    {
                        cusQue.SelectList = new List<Tb_SelectItem>();
                        //03.获取选择题答案
                        DataRow[] drSel = dtSel.Select("QuestionID='" + cusQue.QuestionID + "'", "Sort");
                        for (int j = 0; j < drSel.Length; j++)
                        {
                            cusQue.SelectList.Add(new Tb_SelectItem
                            {
                                QuestionID = cusQue.QuestionID,
                                SelectItem = drSel[j]["SelectItem"].ToString(),
                                ImgUrl = drSel[j]["ImgUrl"].ToString(),
                                Sort = Convert.ToInt32(drSel[j]["Sort"].ToString()),
                                IsAnswer = Convert.ToInt32(drSel[j]["IsAnswer"].ToString())
                            });
                        }
                    }

                        if (dtStuAnswer != null && dtStuAnswer.Rows.Count > 0)
                        {
                            //04.获取学生答案
                            DataRow[] drSA = dtStuAnswer.Select("QuestionID='" + cusQue.QuestionID + "'");
                            if (drSA.Length > 0)
                            {
                                cusQue.StuAnswer = new Tb_StuAnswer
                                {
                                    StuAnswerID = drSA[0]["StuAnswerID"].ToString(),
                                    StuCatID = drSA[0]["StuCatID"].ToString(),
                                    StuID = userid,
                                    CatalogID = catalogid,
                                    QuestionID = cusQue.QuestionID,
                                    ParentID = cusQue.ParentID,
                                    Answer = drSA[0]["BestAnswer"].ToString(),
                                    IsRight = Convert.ToInt32(drSA[0]["BestIsRight"].ToString()),
                                    Score = decimal.Parse(drSA[0]["BestScore"].ToString()),
                                    BestAnswer = drSA[0]["BestAnswer"].ToString(),
                                    BestIsRight = Convert.ToInt32(drSA[0]["BestIsRight"].ToString()),
                                    BestScore = decimal.Parse(drSA[0]["BestScore"].ToString())
                                };
                            }
                        }
                    

                   
                    listQue.Add(cusQue);
                }
                return listQue;
            }
            else
            {
                return new List<Custom_Question>();
            }
        }

        /// <summary>
        /// 分页查询题目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public IList<Tb_QuestionInfo> GetQuestionPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            IList<Tb_QuestionInfo> list = new QuestionDAL().GetQuestionPageList(pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
            return list == null ? (new List<Tb_QuestionInfo>()) : list;
        }

        /// <summary>
        /// 分页查询题目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public IList<V_Question> GetVQuestionPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            IList<V_Question> list = new QuestionDAL().GetVQuestionPageList(pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
            return list == null ? (new List<V_Question>()) : list;
        }

        public Tb_QuestionInfo GetQuestionInfo(string questionid)
        {
            return new QuestionDAL().GetQuestion(questionid);
        }

        public bool UpdateQuestionInfo(Tb_QuestionInfo qInfo)
        {
            return new QuestionDAL().UpdateQuestion(qInfo);
        }
    }
}
