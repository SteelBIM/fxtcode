using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kingsun.InterestDubbingGame.Common;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Kingsun.IBS.Model;

namespace Kingsun.InterestDubbingGame.DAL
{
    public class InterestDubbingGameMatchApiDAL : InterestDubbingBaseManagement
    {
        RedisHashHelper redis = new RedisHashHelper();
        public string FilePath = WebConfigurationManager.AppSettings["FilePath"];
        readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];


        /// <summary>
        /// 从redis获取用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public Redis_InterestDubbingGame_UserInfo GetUserInfo(string UserID)
        {
            Redis_InterestDubbingGame_UserInfo IDG_Userinfo = redis.Get<Redis_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", UserID);
            return IDG_Userinfo;
        }

        /// <summary>
        /// 获取课本剧资源
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public DataSet GetBookDramatList(string groupName)
        {
            string sql = string.Format(@"SELECT  a.[ID] ,
                                                a.[VideoTitle] ,
                                                a.[VideoNumber] ,
                                                a.[MuteVideo] ,
                                                a.[CompleteVideo] ,
                                                a.[BackgroundAudio] ,
                                                a.[VideoCover] ,
                                                a.[VideoDesc] ,
                                                a.[VideoDifficulty] ,
                                                a.[GradeName] ,
                                                a.[GroupName] ,
                                                a.[CreateDate] ,
                                                b.[ID] 'Did' ,
                                                b.[VideoNumber] 'VNumber' ,
                                                b.[DialogueNumber] ,
                                                b.[DialogueText] ,
                                                b.[StartTime] ,
                                                b.[EndTime] 
                                        FROM    dbo.QTB_IDG_BookDrama a
                                                LEFT JOIN dbo.QTB_IDG_BookDialogue b ON b.VideoNumber = a.VideoNumber
                                                WHERE a.GroupName='{0}'", groupName);

            return SqlHelper.ExecuteDataset(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
        }

        /// <summary>
        /// 获取故事朗读资源
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public DataSet GetStoryReadList(string groupName)
        {
            string sql = string.Format(@"SELECT TOP 5
                                                ID ,
                                                Title ,
                                                OriginalText ,
                                                EvaluationContent ,
                                                SerialNumber ,
                                                GradeName ,
                                                GroupName ,
                                                CreateDate
                                        FROM    QTB_IDG_StoryRead
                                        WHERE   GroupName = '{0}'
                                        ORDER BY NEWID()", groupName);

            return SqlHelper.ExecuteDataset(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
        }

        /// <summary>
        /// 插入学生成绩
        /// </summary>
        /// <param name="idgUserContent"></param>
        /// <returns></returns>
        public int InsertUserContentsRecord(TB_InterestDubbingGame_UserContentsRecord idgUserContent)
        {
            string sql = string.Format(@"INSERT INTO dbo.TB_InterestDubbingGame_UserContentsRecord
                                                    ( UserID ,
                                                      DubbingTitle ,
                                                      DubbingFilePath ,
                                                      DubbingScore ,
                                                      Type ,
                                                      CreateDate ,
                                                      VideoID
                                                    )
                                            VALUES  ( {0} , -- UserID - int
                                                      N'{1}' , -- DubbingTitle - nvarchar(80)
                                                      N'{2}' , -- DubbingFilePath - nvarchar(100)
                                                      {3} , -- DubbingScore - float
                                                      {4} , -- Type - int
                                                      GETDATE() , -- CreateDate - datetime
                                                      {5}  -- VideoID - int
                                                    )", idgUserContent.UserID, idgUserContent.DubbingTitle.Replace("\\", ""), FilePath + idgUserContent.DubbingFilePath, idgUserContent.DubbingScore, idgUserContent.Type, idgUserContent.VideoID);
            int i = SqlHelper.ExecuteNonQuery(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
            return i;
        }

        /// <summary>
        /// 获取比赛报告
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetGameScore(string classId, int pageNumber,List<UserClass> userclass)
        {
            List<IDGUserInfo> userids = GetStuListByClassShortId(classId, userclass);
            //List<string> uid = userids.Select(item => item.UserID).ToList();
            //List<IDGUserScore> ListIDGUS = redis.GetValues<IDGUserScore>("Redis_InterestDubbingGame_UserTotalScore", uid);
            //List<IDGUserInfo> ListIDGUserInfo = redis.GetValues<IDGUserInfo>("TB_InterestDubbingGame_UserInfo", uid);
            List<IDGGameScoreInfoList> GameScore = new List<IDGGameScoreInfoList>();
            IDGGameScoreInfoList IDGui;
            double maxScore = 0;
            double minScore = 220;
            double count = 0;
            int cl = 0;

            foreach (var em in userids)
            {
                IDGui = new IDGGameScoreInfoList();
                IDGUserScore us = redis.Get<IDGUserScore>("Redis_InterestDubbingGame_UserTotalScore", em.UserID.ToString());
                if (us != null)
                {
                    if (us.StoryReadScore > 0 && us.BookPlayScore > 0)
                    {

                        cl++;
                        if (us.TotalScore > maxScore)
                        {
                            maxScore = Convert.ToDouble(us.TotalScore.ToString("0.0"));
                        }
                        if (us.TotalScore <= minScore)
                        {
                            minScore = Convert.ToDouble(us.TotalScore.ToString("0.0"));
                        }
                        count += Convert.ToDouble(us.TotalScore.ToString("0.0"));

                        IDGui.TotalScore = us.TotalScore;
                        IDGui.IsStudy = true;
                    }
                    else
                    {
                        IDGui.TotalScore = 0.00;
                        IDGui.IsStudy = false;
                    }
                }
                else
                {
                    IDGui.TotalScore = 0.00;
                    IDGui.IsStudy = false;
                }

                //IDGUserInfo ui = redis.Get<IDGUserInfo>("TB_InterestDubbingGame_UserInfo", em.UserID);
                //if (ui != null)
                //{
                IDGui.UserName = em.UserName;
                IDGui.UserImg = em.UserImage;
                IDGui.CreateTime = (DateTime)em.CreateTime;
                // }
                #region 弃用for循环
                //foreach (var item in ListIDGUS)
                //{
                //    if (item != null)
                //    {
                //        if (em.UserID == item.UserID.ToString())
                //        {
                //            cl++;
                //            if (item.TotalScore > maxScore)
                //            {
                //                maxScore = Convert.ToDouble(item.TotalScore.ToString("0.0"));
                //            }
                //            if (item.TotalScore <= minScore)
                //            {
                //                minScore = Convert.ToDouble(item.TotalScore.ToString("0.0"));
                //            }
                //            count += Convert.ToDouble(item.TotalScore.ToString("0.0"));

                //            IDGui.TotalScore = item.TotalScore;
                //            IDGui.IsStudy = true;
                //        }
                //    }
                //    else
                //    {
                //        IDGui.TotalScore = 0.00;
                //        IDGui.IsStudy = false;
                //    }
                //}

                //foreach (var ui in ListIDGUserInfo)
                //{
                //    if (ui != null)
                //    {
                //        if (em.UserID == ui.UserID.ToString())
                //        {
                //            IDGui.UserID = ui.UserID;
                //            IDGui.UserName = ui.TrueName;
                //            IDGui.UserImg = ui.UserImg;
                //            IDGui.CreateTime = ui.CreateTime;
                //        }
                //    }
                //}
                #endregion
                IDGui.UserID = Convert.ToInt32(em.UserID);
                GameScore.Add(IDGui);
            }

            if (cl == 0)
            {
                minScore = 0;
            }
            //以时间为单位，降序排列
            GameScore = GameScore.OrderByDescending(i => i.TotalScore).ThenByDescending(i => i.IsStudy).Skip(pageNumber * 20).Take(20).ToList();
            object obj =
                new
                {
                    AverageScore = count <= 0 ? "0" : (count / cl).ToString("0.0"),
                    StudyStudentCount = cl,
                    StudentCount = userids.Count,
                    HighestScore = maxScore,
                    LowestScore = minScore,
                    Students = GameScore
                };
            return JsonHelper.GetResult(obj, "操作成功");//返回信息 

        }


        /// <summary>
        /// 通过班级ID查询班级下的所有学生
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        private List<IDGUserInfo> GetStuListByClassShortId(string classId, List<UserClass> userClass)
        {
            List<IDGUserInfo> stuList = new List<IDGUserInfo>();

         

            string sql = string.Format(@" SELECT b.TrueName ,
                                                b.UserName ,
                                                b.UserID ,
                                                b.NickName ,
                                                b.UserImage,
                                                b.IsEnableOss
                                                from ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo b where b.IsUser=1 ");
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables.Count == 0) { return null; }

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    IDGUserInfo ui = new IDGUserInfo
                    {
                        UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString()),
                        UserImage = ds.Tables[0].Rows[i]["IsEnableOss"].ToString() != "0" ? _getOssFilesUrl + ds.Tables[0].Rows[i]["UserImage"] : _getFilesUrl + "?FileID=" + ds.Tables[0].Rows[i]["UserImage"],
                        UserName = ds.Tables[0].Rows[i]["TrueName"].ToString(),
                        CreateTime = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CreateTime"].ToString()) ? Convert.ToDateTime("1900-01-01 00:00:00.0000") : Convert.ToDateTime(ds.Tables[0].Rows[i]["CreateTime"].ToString())
                    };
                    stuList.Add(ui);
                }
            }
            if (userClass.Count > 0)
            {
                DateTime? TimeNull = null;
                stuList = (from u in userClass
                           join b in stuList on u.UserID equals b.UserID into userid
                           from b in userid.DefaultIfEmpty()
                           where u.UserID==b.UserID
                           select new IDGUserInfo
                           {
                               UserID = u.UserID,
                               CreateTime = b == null ? TimeNull : b.CreateTime,
                               UserImage = b == null ? "" : b.UserImage,
                               UserName = b == null ? "" : b.UserName
                           }).ToList<IDGUserInfo>();
            }

            return stuList.OrderBy(i => i.UserID).ToList();
        }

        /// <summary>
        /// 获取用户课本剧数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUserBookDramaInfo(int userId)
        {
            string sql = string.Format(@"SELECT  UserID ,
                                                DubbingTitle ,
                                                DubbingFilePath ,
                                                DubbingScore ,
                                                Type ,
                                                CreateDate ,
                                                VideoID
                                        FROM    TB_InterestDubbingGame_UserContentsRecord
                                        WHERE   Type = 0
                                                AND UserID = {0}", userId);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
            if (ds == null) return JsonHelper.GetResult("用户故事朗读数据不存在！"); //返回信息 
            if (ds.Tables[0].Rows.Count > 0)
            {
                return JsonHelper.GetResult(JsonHelper.DataSetToIList<TB_InterestDubbingGame_UserContentsRecord>(ds, 0), "操作成功"); ;
            }
            return JsonHelper.GetErrorResult("数据不存在！");//返回信息 
        }

        /// <summary>
        /// 获取用户故事朗读数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUserStoryReadInfo(int userId)
        {
            string sql = string.Format(@"SELECT  a.UserID ,
                                                a.DubbingTitle ,
                                                a.DubbingFilePath ,
                                                a.DubbingScore ,
                                                a.CreateDate ,
                                                a.VideoID ,
                                                b.EvaluationContent,
                                                b.OriginalText
                                        FROM    dbo.TB_InterestDubbingGame_UserContentsRecord a
                                                LEFT JOIN dbo.QTB_IDG_StoryRead b ON a.VideoID = b.ID
                                        WHERE   a.Type = 1
                                                AND a.UserID = {0}", userId);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
            if (ds == null) return JsonHelper.GetResult("用户故事朗读数据不存在！"); //返回信息 
            if (ds.Tables[0].Rows.Count > 0)
            {
                return JsonHelper.GetResult(JsonHelper.DataSetToIList<UserStoryInfo>(ds, 0), "操作成功"); ;
            }
            return JsonHelper.GetErrorResult("数据不存在！");//返回信息 
        }

    }
}
