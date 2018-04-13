using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.ExamPaper.Common;
using SqlHelper = Kingsun.SynchronousStudy.Common.SqlHelper;

namespace Kingsun.ExamPaper.BLL
{
    public class StuCatalogBLL
    {
        private BaseManagement bm = new BaseManagement();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        private RedisListHelper ibsredisList = new RedisListHelper();
        static RedisHashHelper redis = new RedisHashHelper();

        string appAreaVersion
        {
            get
            {
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("AppAreaVersion")))
                {
                    throw new Exception("未获取到地区版本appAreaVersion");
                }

                return System.Configuration.ConfigurationManager.AppSettings.Get("AppAreaVersion");
            }
        }

        public void SubmitModelInit(SubmitModel2 submitModel, string Connection)
        {

            if (string.IsNullOrEmpty(submitModel.UserID) || !submitModel.CatalogID.HasValue ||
                submitModel.AnswerList == null || submitModel.AnswerList.Count == 0)
            {
                return;
            }


            //构建报告页接口(GetStuCatalog)的redis数据

            #region

            Custom_StuCatalog custom_StuCatalog = redis.Get<Custom_StuCatalog>(
                appAreaVersion + "_Exampaper_Custom_StuCatalog_" + submitModel.UserID.Substring(0, 2),
                submitModel.UserID + "_" + submitModel.CatalogID.Value);

            if (custom_StuCatalog == null)
            {
                custom_StuCatalog = new Custom_StuCatalog();

            }

            //构造题目和答案
            var list = redis.Get<IList<Custom_Question>>(appAreaVersion + "_Exampaper_Custom_Question",
                submitModel.CatalogID.ToString()); //题目列表

            if (list == null)
            {
                list = GetQuestionList("", submitModel.CatalogID.Value, 1, Connection);
                if (list == null || !list.Any())
                {
                    return;
                }
                else
                {
                    redis.Set(appAreaVersion + "_Exampaper_Custom_Question", submitModel.CatalogID.Value.ToString(),
                        list);
                }
            }

            var questionList = new List<Custom_PQ>();
            foreach (var a in submitModel.AnswerList)
            {
                var q = list.FirstOrDefault(o => o.QuestionID == a.QuestionID);
                if (q == null)
                {
                    return;
                }

                //if (string.IsNullOrWhiteSpace(q.ParentID))
                //{
                questionList.Add(new Custom_PQ
                {
                    QuestionID = a.QuestionID,
                    Score = a.Score,
                    QuestionModel = q.QuestionModel,
                    QuestionTitle = q.QuestionTitle,
                    Sort = q.Sort,
                    ParentID = a.ParentID,
                    BestAnswer = a.Answer,
                    BestScore = a.Score.Value,
                    BestIsRight = a.IsRight,
                });
                //}
            }

            custom_StuCatalog.BestTotalScore = custom_StuCatalog.BestTotalScore < submitModel.TotalScore.Value
                ? submitModel.TotalScore.Value
                : custom_StuCatalog.BestTotalScore;
            if (custom_StuCatalog.BestTotalScore <= submitModel.TotalScore.Value) //只存最优记录
            {
                custom_StuCatalog.QuestionList = questionList;
                custom_StuCatalog.TotalScore = submitModel.TotalScore.Value;
            }

            AsyncCustom_StuCatalog(submitModel);

            redis.Set<Custom_StuCatalog>(
                appAreaVersion + "_Exampaper_Custom_StuCatalog_" + submitModel.UserID.Substring(0, 2),
                submitModel.UserID + "_" + submitModel.CatalogID.Value, custom_StuCatalog);

            //var update = new StuAnswerBLL().UploadStuAnswerList(submitModel.UserID, submitModel.CatalogID.Value, submitModel.TotalScore.Value, submitModel.AnswerList);

            #endregion

        }

        /// <summary>
        /// 异步redis写入报告
        /// </summary>
        /// <param name="agrs"></param>
        public bool AsyncCustom_StuCatalog(object agrs)
        {
            SubmitModel2 submitModel = null;
            try
            {
                if (agrs is SubmitModel2)
                {
                    submitModel = (SubmitModel2) agrs;

                    var sm = redis.Get<SubmitModel2>(
                        appAreaVersion + "_Exampaper_SubmitModel_" + submitModel.UserID.Substring(0, 2),
                        submitModel.UserID + "_" + submitModel.CatalogID);
                    if (sm == null)
                    {
                        return redis.Set<SubmitModel2>(
                            appAreaVersion + "_Exampaper_SubmitModel_" + submitModel.UserID.Substring(0, 2),
                            submitModel.UserID + "_" + submitModel.CatalogID, submitModel);
                    }
                    else
                    {
                        sm.AnswerList = submitModel.AnswerList;
                        sm.DoDate = DateTime.Now.Date;
                        sm.TotalScore = submitModel.TotalScore;

                        return redis.Set<SubmitModel2>(
                            appAreaVersion + "_Exampaper_SubmitModel_" + submitModel.UserID.Substring(0, 2),
                            submitModel.UserID + "_" + submitModel.CatalogID, sm);
                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("异步写入redis报告UploadStuAnswerList, UserID=" + submitModel.UserID +
                                        "\t CatalogID=" + submitModel.CatalogID + "\t异常：" + ex.Message);

                return false;
            }
        }

        public string GetAllStuAnswer1(string connectionStr)
        {
            try
            {
                string stuCataSql = @"SELECT  [StuCatID]
                  ,[StuID] as UserID
                  ,[CatalogID]     
                  ,[BestTotalScore] as TotalScore
                  ,[DoDate]     
                FROM FZ_Exampaper.[dbo].[Tb_StuCatalog]";
                var cataDs = SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, stuCataSql);
                List<SubmitModel2> stuCataList =
                    Kingsun.SynchronousStudy.Common.JsonHelper.DataTableToList<SubmitModel2>(cataDs.Tables[0]);

                string answerSql = @"SELECT  [StuAnswerID]
                  ,[StuCatID]
                  ,[StuID]
                  ,[CatalogID]
                  ,[QuestionID]
                  ,[ParentID]  
                  ,[BestAnswer] as Answer
                  ,[BestIsRight] as IsRight
                  ,[BestScore] as Score
                 FROM FZ_Exampaper.[dbo].[Tb_StuAnswer]";

                var answerDs = SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, answerSql);
                List<Custom_StuAnswer2> answerList =
                    Kingsun.SynchronousStudy.Common.JsonHelper.DataTableToList<Custom_StuAnswer2>(answerDs.Tables[0]);

                if (stuCataList.Count > 0)
                {
                    Log4Net.LogHelper.Info("Catalog目录数:" + stuCataList.Count);
                }

                foreach (var cata in stuCataList)
                {
                    cata.AnswerList = answerList.Where(o => o.StuCatID == cata.StuCatID).ToList();
                    SubmitModelInit(cata, connectionStr);
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex);
                return ex.Message;
            }

            return "success";
        }


        public string GetAllStuAnswer(string connectionStr)
        {
            try
            {
                string stuCataSql = @"SELECT  [StuCatID]
                  ,[StuID] as UserID
                  ,[CatalogID]     
                  ,[BestTotalScore] as TotalScore
                  ,[DoDate]     
                FROM FZ_Exampaper.[dbo].[Tb_StuCatalog]";
                var cataDs = SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, stuCataSql);
                List<SubmitModel2> stuCataList =
                Kingsun.SynchronousStudy.Common.JsonHelper.DataTableToList<SubmitModel2>(cataDs.Tables[0]);

                if (stuCataList.Count > 0)
                {
                    Log4Net.LogHelper.Info("Catalog目录数:" + stuCataList.Count);
                }

                foreach (var cata in stuCataList)
                {
                    cata.AnswerList = GetAnswerList(cata.StuCatID, connectionStr);
                    SubmitModelInit(cata, connectionStr);
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex);
                return ex.Message;
            }

            return "success";
        }

        public List<Custom_StuAnswer2> GetAnswerList(string stuCatID, string ConnectionStr)
        {

            if (string.IsNullOrEmpty(stuCatID))
            {
                return new List<Custom_StuAnswer2>();
            }

            string answerSql = @"SELECT  [StuAnswerID]
                  ,[StuCatID]
                  ,[StuID]
                  ,[CatalogID]
                  ,[QuestionID]
                  ,[ParentID]  
                  ,[BestAnswer] as Answer
                  ,[BestIsRight] as IsRight
                  ,[BestScore] as Score
                 FROM FZ_Exampaper.[dbo].[Tb_StuAnswer] where StuCatID='stuCatID'";

            var answerDs = SqlHelper.ExecuteDataset(ConnectionStr, CommandType.Text, answerSql);
            List<Custom_StuAnswer2> answerList =Kingsun.SynchronousStudy.Common.JsonHelper.DataTableToList<Custom_StuAnswer2>(answerDs.Tables[0]);
            return answerList;
        }


    /// <summary>
        /// 获取题目信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="catalogid">目录ID</param>
        /// <param name="isdo">0:查看，1:做题</param>
        /// <returns></returns>
        public IList<Custom_Question> GetQuestionList(string userid, int catalogid, int isdo,string ConnectionStr)
        {

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
            param[1] = new SqlParameter("@CatalogID", SqlDbType.Int);
            param[2] = new SqlParameter("@IsDo", SqlDbType.Int);

            param[0].Value = userid;
            param[1].Value = catalogid;
            param[2].Value = isdo;
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStr, CommandType.StoredProcedure, "FZ_Exampaper.dbo.Proc_GetQuestionList", param);
            if (ds != null && ds.Tables.Count > 0)
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
                    cusQue.BlankAnswer = new List<Tb_BlankAnswer>();
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
        /// 获取成绩报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public Custom_StuCatalog GetStuCatalog(string userid, int catalogid)
        {
            DataSet ds = new StuCatalogDAL().GetStuCatalog(userid, catalogid);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                Custom_StuCatalog cusSC = new Custom_StuCatalog();
                cusSC.TotalScore = decimal.Parse(dt.Rows[0]["TotalScore"].ToString());
                cusSC.BestTotalScore = decimal.Parse(dt.Rows[0]["BestTotalScore"].ToString());
                cusSC.DoDate = DateTime.Parse(dt.Rows[0]["DoDate"].ToString());
                cusSC.QuestionList = new List<Custom_PQ>();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[1].Rows[i];
                    cusSC.QuestionList.Add(new Custom_PQ
                    {
                        QuestionID = dr["QuestionID"].ToString(),
                        QuestionTitle = dr["QuestionTitle"].ToString(),
                        Sort = int.Parse(dr["Sort"].ToString()),
                        BestScore = string.IsNullOrEmpty(dr["BestScore"].ToString()) ? 0 : decimal.Parse(dr["BestScore"].ToString()),
                        BestAnswer = dr["BestAnswer"].ToString(),
                        BestIsRight = string.IsNullOrEmpty(dr["BestIsRight"].ToString()) ? 0 : Convert.ToInt32(dr["BestIsRight"].ToString()),
                        ParentID = dr["ParentID"].ToString(),
                        QuestionModel = dr["QuestionModel"].ToString()
                    });
                }
                return cusSC;
            }
            else
            {
                return new Custom_StuCatalog();
            }
        }

        /// <summary>
        /// 获取某天成绩报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public Custom_StuCatalogList GetDoDateStuCatalog(string userid, DateTime DoDate)
        {
            Custom_StuCatalogList cussclist = new Custom_StuCatalogList();
            DataSet ds = new StuCatalogDAL().GetDoDateStuCatalog(userid, DoDate);
            if (ds != null && ds.Tables.Count > 0)
            {
                List<StuCatalog> cusSClist = new List<StuCatalog>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = ds.Tables[0];
                    StuCatalog cusSC = new StuCatalog();
                    cusSC.CatalogID = int.Parse(dt.Rows[i]["CatalogID"].ToString());
                    cusSC.CatalogName = dt.Rows[i]["CatalogName"].ToString();
                    cusSC.ParentID = int.Parse(dt.Rows[i]["ParentID"].ToString());
                    cusSC.ParentCatalogName = dt.Rows[i]["ParentCatalogName"].ToString();
                    cusSC.TotalScore = float.Parse(dt.Rows[i]["TotalScore"].ToString());
                    cusSC.BestTotalScore = float.Parse(dt.Rows[i]["BestTotalScore"].ToString());
                    cusSC.DoDate = DateTime.Parse(dt.Rows[i]["DoDate"].ToString());
                    cusSC.QuestionList = new List<PQ>();
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        DataRow dr = ds.Tables[1].Rows[j];
                        if (dt.Rows[i]["StuCatID"].ToString() == dr["StuCatID"].ToString())
                        {
                            cusSC.QuestionList.Add(new PQ
                                                 {
                                                     CatalogID = int.Parse(dt.Rows[i]["CatalogID"].ToString()),
                                                     CatalogName = dr["CatalogName"].ToString(),
                                                     QuestionID = dr["QuestionID"].ToString(),
                                                     QuestionTitle = dr["QuestionTitle"].ToString(),
                                                     Sort = int.Parse(dr["Sort"].ToString()),
                                                     BestScore = string.IsNullOrEmpty(dr["BestScore"].ToString()) ? 0 : float.Parse(dr["BestScore"].ToString()),
                                                     BestAnswer = dr["BestAnswer"].ToString(),
                                                     BestIsRight = string.IsNullOrEmpty(dr["BestIsRight"].ToString()) ? 0 : Convert.ToInt32(dr["BestIsRight"].ToString()),
                                                     QuestionModel = dr["QuestionModel"].ToString()
                                                 });
                        }

                    }
                    cusSClist.Add(cusSC);
                }
                cussclist.StuCatalogList = cusSClist;
            }
            return cussclist;
        }

        /// <summary>
        /// 获取班级圈周成绩报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public Custom_StuCatalogListWeek GetClassStuCatalogWeek(string UserID)
        {
            Custom_StuCatalogListWeek cussclist = new Custom_StuCatalogListWeek();
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(UserID));
            if (user != null)
            {
                if (user.ClassSchList.Count > 0)
                {
                    StringBuilder UserIDs = new StringBuilder();
                    var classinfo = classBLL.GetClassUserRelationByClassId(user.ClassSchList[0].ClassID);
                    if (classinfo != null)
                    {
                        classinfo.ClassStuList.ForEach(a =>
                        {
                            UserIDs.Append(a.StuID + ",");
                        });
                        UserIDs.Remove(UserIDs.Length - 1, 1);

                        DataSet ds = new StuCatalogDAL().GetClassStuCatalogWeek(UserIDs.ToString());
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            List<StuCatalogWeek> cusSClist = new List<StuCatalogWeek>();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                StuCatalogWeek week = new StuCatalogWeek();
                                week.UserID = dr["StuID"].ToString();
                                week.BestScore = int.Parse(dr["BestScore"].ToString());
                                week.LastBestScore = int.Parse(dr["LastBestScore"].ToString());

                                var userinfo = userBLL.GetUserInfoByUserId(Convert.ToInt32(week.UserID));
                                if (userinfo != null)
                                {
                                    if (!string.IsNullOrEmpty(userinfo.TrueName))
                                    {
                                        week.TrueName = userinfo.TrueName;
                                    }
                                    else
                                    {
                                        week.TrueName = userinfo.UserName;
                                    }
                                }
                                cusSClist.Add(week);
                            }
                            cussclist.StuCatalogWeek = cusSClist;
                        }
                    }
                }
            }
            return cussclist;
        }

        /// <summary>
        /// 获取班级最佳成绩报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public Custom_StuCatalogList GetClassStuCatalog(string ClassID, int? CatalogID, int isBest)
        {
            Custom_StuCatalogList cussclist = new Custom_StuCatalogList();
            var classinfo = classBLL.GetClassUserRelationByClassId(ClassID);
            if (classinfo != null)
            {
                StringBuilder UserIDs = new StringBuilder();
                if (classinfo.ClassStuList.Count > 0)
                {
                    classinfo.ClassStuList.ForEach(a =>
                    {
                        UserIDs.Append(a.StuID + ",");
                    });
                    UserIDs = UserIDs.Remove(UserIDs.Length - 1, 1);
                }
                DataSet ds = new StuCatalogDAL().GetClassStuCatalog(UserIDs.ToString(), CatalogID, isBest);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<StuCatalog> cusSClist = new List<StuCatalog>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataTable dt = ds.Tables[0];
                        StuCatalog cusSC = new StuCatalog();
                        cusSC.TotalScore = float.Parse(dt.Rows[0]["TotalScore"].ToString());
                        cusSC.BestTotalScore = float.Parse(dt.Rows[0]["BestTotalScore"].ToString());
                        cusSC.DoDate = DateTime.Parse(dt.Rows[0]["DoDate"].ToString());
                        cusSC.UserID = dt.Rows[0]["StuID"].ToString();
                        var userinfo = userBLL.GetUserInfoByUserId(Convert.ToInt32(cusSC.UserID));
                        if (userinfo != null)
                        {
                            if (!string.IsNullOrEmpty(userinfo.TrueName))
                            {
                                cusSC.TrueName = userinfo.TrueName;
                            }
                            else
                            {
                                cusSC.TrueName = userinfo.UserName;
                            }

                        }
                        cusSC.QuestionList = new List<PQ>();
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            DataRow dr = ds.Tables[1].Rows[j];
                            if (dt.Rows[0]["StuID"].ToString() == cusSC.UserID)
                            {
                                cusSC.QuestionList.Add(new PQ
                                {
                                    CatalogID = int.Parse(dr["CatalogID"].ToString()),
                                    CatalogName = dr["CatalogName"].ToString(),
                                    QuestionID = dr["QuestionID"].ToString(),
                                    QuestionTitle = dr["QuestionTitle"].ToString(),
                                    Sort = int.Parse(dr["Sort"].ToString()),
                                    BestScore = string.IsNullOrEmpty(dr["BestScore"].ToString()) ? 0 : float.Parse(dr["BestScore"].ToString()),
                                    BestAnswer = dr["BestAnswer"].ToString(),
                                    BestIsRight = string.IsNullOrEmpty(dr["BestIsRight"].ToString()) ? 0 : Convert.ToInt32(dr["BestIsRight"].ToString()),
                                    QuestionModel = dr["QuestionModel"].ToString()
                                });
                            }
                        }
                        cusSClist.Add(cusSC);
                    }
                    cussclist.StuCatalogList = cusSClist;
                }
            }
            return cussclist;
        }

        public IList<Tb_StuCatalog> GetStuCatalogList(int[] catalogIds, string userId)
        {
            var catas = string.Join(",", catalogIds);
            return new StuCatalogDAL().Search<Tb_StuCatalog>(string.Format("stuid={0}  and  catalogid in ({1})", userId, catas));
        }
    }

    /// <summary>
    /// 批量提交答案原型
    /// </summary>
    public class SubmitModel2
    {
        public string StuCatID { get; set; }
        public string UserID { get; set; }
        public int? CatalogID { get; set; }
        public decimal? TotalScore { get; set; }

        public DateTime? DoDate { get; set; }//hlw加
        public IList<Custom_StuAnswer2> AnswerList { get; set; }
    }

    /// <summary>
    /// 自定义上传的答案
    /// </summary>
    public class Custom_StuAnswer2
    {
        public string StuCatID { get; set; }
        public string QuestionID { get; set; }
        public string ParentID { get; set; }
        public string Answer { get; set; }
        public decimal? Score { get; set; }
        public int IsRight { get; set; }
        public int CatalogID { get; set; }
    }
}
