using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Transactions;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.SynchronousStudy.Common;
using JsonHelper = Kingsun.ExamPaper.Common.JsonHelper;

namespace Kingsun.ExamPaper.BLL
{
    public class StuAnswerBLL : BaseManagement
    {

        private RedisListHelper ibsredisList = new RedisListHelper();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        /// <summary>
        /// 一键提交上传成绩答案
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <param name="totalScore"></param>
        /// <param name="listCSA"></param>
        /// <returns></returns>
        public bool UploadStuAnswerList(string userid, int catalogid, float totalScore, IList<Custom_StuAnswer> listCSA)
        {
            Guid StuCatID = new Guid();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"declare @StuCatID varchar(50);declare @IsBest int;set @StuCatID='';set @IsBest=0;
                select @StuCatID=StuCatID,@IsBest=(case when BestTotalScore<{2} then 1 else 0 end) 
                from Tb_StuCatalog 
                where StuID='{0}' and CatalogID={1}
                -----用户这套试卷有做题记录-------
                if @StuCatID<>'' 
                begin	
	                ----最佳成绩----
	                if @IsBest=1 
	                begin
		                -------------更新最佳成绩-----------------
		                update Tb_StuCatalog with(rowlock) 
		                set TotalScore={2},DoDate='{3}',BestTotalScore=(case when @IsBest=1 then {2} else BestTotalScore end) 
		                where StuCatID=@StuCatID; 
		
		                -----删除之前记录-------
		                delete Tb_StuAnswer
		                where StuID='{0}' and CatalogID={1}
		
		                -----插入做题记录-------
		                " + BatchInsertSQL(listCSA, userid, catalogid) + @"		
	                end
                end
                else 
                begin 
	                set @StuCatID=cast(newid() as varchar(50));	
	                ----------插入成绩------------
	                insert into Tb_StuCatalog
	                (
		                StuCatID,StuID,CatalogID,TotalScore,BestTotalScore,DoDate
	                ) 

	                values(
		                @StuCatID,'{0}',{1},{2},{2},'{3}'
	                )
	                ----------插入做题记录------------
	                " + BatchInsertSQL(listCSA, userid, catalogid) + @"
	                -----试卷做题次数+1-------
                    -----显示做题次数0-500,显示做题次数+1----------
                    -----显示做题次数500-1000,显示做题次数+1/3------
                    -----显示做题次数1000-2000,显示做题次数+1/5------
                    -----显示做题次数2000以上,显示做题次数+1/10------
	                update QTb_Catalog 
						set AnswerNum=(case when AnswerNum is null then 0 else AnswerNum end)+1 ,
						AnswerNumShow=(
                            case when {4}=1 then AnswerNumShow+1
	                        when AnswerNumShow is null then 1
	                        when AnswerNumShow<500 then AnswerNumShow+1
	                        when AnswerNumShow<1000 then AnswerNumShow+1/3
	                        when AnswerNumShow<2000 then AnswerNumShow+1/5
	                        else AnswerNumShow+1/10 END)
	                where CatalogID={1};	
                end;
                -----用户试卷做题次数+1-------                
                update Tb_StuCatalog 
                    set AnswerNum=(case when AnswerNum is null then 0 else AnswerNum end)+1
                where StuID='{0}' and CatalogID={1}; ", userid, catalogid, totalScore, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), isNewUser(userid));

            bool isSuccess = ExcuteSqlWithTran(sb.ToString());
            if (isSuccess)
            {
                string CatalogName = "";
                string BookID = "";
                string SCatID = "";
                string AnswerNum = "";
                string strSql = string.Format(@"SELECT  [StuCatID],[AnswerNum]
                                                FROM    [dbo].[Tb_StuCatalog]
                                                WHERE   StuID = '{0}'
                                                        AND CatalogID = '{1}'", userid, catalogid);
                DataSet dsCat = Kingsun.ExamPaper.Common.SqlHelper.ExecuteDataset(Kingsun.ExamPaper.Common.SqlHelper.ConnectionString, CommandType.Text, strSql);
                List<tbStuCatalog> catId = JsonHelper.DataSetToIList<tbStuCatalog>(dsCat, 0);

                if (catId.Count > 0)
                {
                    tbStuCatalog vinfo = catId.FirstOrDefault();
                    if (vinfo != null) SCatID = vinfo.StuCatID.ToString();
                    if (vinfo != null) AnswerNum = vinfo.AnswerNum.ToString();
                }

                string sql = string.Format(@"SELECT CatalogID,CatalogName,BookID FROM dbo.QTb_Catalog WHERE CatalogID='{0}'", catalogid);
                DataSet ds = Kingsun.ExamPaper.Common.SqlHelper.ExecuteDataset(Kingsun.ExamPaper.Common.SqlHelper.ConnectionString, CommandType.Text, sql);


                List<CatalogInfo> vi = JsonHelper.DataSetToIList<CatalogInfo>(ds, 0);
                if (vi.Count > 0)
                {
                    CatalogInfo vinfo = vi.FirstOrDefault();
                    if (vinfo != null)
                    {
                        CatalogName = vinfo.CatalogName;
                        BookID = vinfo.BookID.ToString();
                    }
                }

                RedisVideoInfo rvi = new RedisVideoInfo
                {
                    VideoID = SCatID,
                    VideoNumber = catalogid.ToString(),
                    UserId = userid,
                    TotalScore = totalScore.ToString(),
                    ModuleType = "2",
                    BookId = BookID,
                    VideoTitle = CatalogName,
                    DubbingNum = AnswerNum,
                    CreateTime = DateTime.Now.ToShortDateString()
                };
                ibsredisList.LPush("LearningReportQueue", JsonHelper.DeepEncodeJson(rvi));
            }
            return isSuccess;
        }


        /// <summary>
        /// 一键提交上传成绩答案
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <param name="totalScore"></param>
        /// <param name="listCSA"></param>
        /// <returns></returns>
        public bool UploadStuAnswerList(string userid, int catalogid, float totalScore, List<Custom_StuAnswer> listCSA,string connectionStr)
        {
            Guid StuCatID = new Guid();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"declare @StuCatID varchar(50);declare @IsBest int;set @StuCatID='';set @IsBest=0;
                select @StuCatID=StuCatID,@IsBest=(case when BestTotalScore<{2} then 1 else 0 end) 
                from   FZ_Exampaper.[dbo].Tb_StuCatalog 
                where StuID='{0}' and CatalogID={1}
                -----用户这套试卷有做题记录-------
                if @StuCatID<>'' 
                begin	
	                ----最佳成绩----
	                if @IsBest=1 
	                begin
		                -------------更新最佳成绩-----------------
		                update   FZ_Exampaper.[dbo].Tb_StuCatalog with(rowlock) 
		                set TotalScore={2},DoDate='{3}',BestTotalScore=(case when @IsBest=1 then {2} else BestTotalScore end) 
		                where StuCatID=@StuCatID; 
		
		                -----删除之前记录-------
		                delete   FZ_Exampaper.[dbo].Tb_StuAnswer
		                where StuID='{0}' and CatalogID={1}
		
		                -----插入做题记录-------
		                " + BatchInsertSQL(listCSA, userid, catalogid) + @"		
	                end
                end
                else 
                begin 
	                set @StuCatID=cast(newid() as varchar(50));	
	                ----------插入成绩------------
	                insert into   FZ_Exampaper.[dbo].Tb_StuCatalog
	                (
		                StuCatID,StuID,CatalogID,TotalScore,BestTotalScore,DoDate
	                ) 

	                values(
		                @StuCatID,'{0}',{1},{2},{2},'{3}'
	                )
	                ----------插入做题记录------------
	                " + BatchInsertSQL(listCSA, userid, catalogid) + @"
	                -----试卷做题次数+1-------
                    -----显示做题次数0-500,显示做题次数+1----------
                    -----显示做题次数500-1000,显示做题次数+1/3------
                    -----显示做题次数1000-2000,显示做题次数+1/5------
                    -----显示做题次数2000以上,显示做题次数+1/10------
	                update   FZ_Exampaper.[dbo].QTb_Catalog 
						set AnswerNum=(case when AnswerNum is null then 0 else AnswerNum end)+1 ,
						AnswerNumShow=(
                            case when {4}=1 then AnswerNumShow+1
	                        when AnswerNumShow is null then 1
	                        when AnswerNumShow<500 then AnswerNumShow+1
	                        when AnswerNumShow<1000 then AnswerNumShow+1/3
	                        when AnswerNumShow<2000 then AnswerNumShow+1/5
	                        else AnswerNumShow+1/10 END)
	                where CatalogID={1};	
                end;
                -----用户试卷做题次数+1-------                
                update   FZ_Exampaper.[dbo].Tb_StuCatalog 
                    set AnswerNum=(case when AnswerNum is null then 0 else AnswerNum end)+1
                where StuID='{0}' and CatalogID={1}; ", userid, catalogid, totalScore, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), isNewUser(userid));
            string sql=Kingsun.ExamPaper.Common.SqlHelper.ExcuteSqlWithTran(sb.ToString());
            bool isSuccess = Kingsun.ExamPaper.Common.SqlHelper.ExecuteNonQuery(connectionStr, CommandType.Text, sql) > 0;
            if (isSuccess)
            {
                string CatalogName = "";
                string BookID = "";
                string SCatID = "";
                string AnswerNum = "";
                string strSql = string.Format(@"SELECT  [StuCatID],[AnswerNum]
                                                FROM    FZ_Exampaper.[dbo].[Tb_StuCatalog]
                                                WHERE   StuID = '{0}'
                                                        AND CatalogID = '{1}'", userid, catalogid);
                DataSet dsCat = Kingsun.ExamPaper.Common.SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, strSql);
                List<tbStuCatalog> catId = JsonHelper.DataSetToIList<tbStuCatalog>(dsCat, 0);

                if (catId.Count > 0)
                {
                    tbStuCatalog vinfo = catId.FirstOrDefault();
                    if (vinfo != null) SCatID = vinfo.StuCatID.ToString();
                    if (vinfo != null) AnswerNum = vinfo.AnswerNum.ToString();
                }

                string sql1 = string.Format(@"SELECT CatalogID,CatalogName,BookID FROM FZ_Exampaper.dbo.QTb_Catalog WHERE CatalogID='{0}'", catalogid);
                DataSet ds = Kingsun.ExamPaper.Common.SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, sql1);


                List<CatalogInfo> vi = JsonHelper.DataSetToIList<CatalogInfo>(ds, 0);
                if (vi.Count > 0)
                {
                    CatalogInfo vinfo = vi.FirstOrDefault();
                    if (vinfo != null)
                    {
                        CatalogName = vinfo.CatalogName;
                        BookID = vinfo.BookID.ToString();
                    }
                }

                RedisVideoInfo rvi = new RedisVideoInfo
                {
                    VideoID = SCatID,
                    VideoNumber = catalogid.ToString(),
                    UserId = userid,
                    TotalScore = totalScore.ToString(),
                    ModuleType = "2",
                    BookId = BookID,
                    VideoTitle = CatalogName,
                    DubbingNum = AnswerNum,
                    CreateTime = DateTime.Now.ToShortDateString()
                };
                ibsredisList.LPush("LearningReportQueue", JsonHelper.DeepEncodeJson(rvi));
            }
            return isSuccess;
        }
        /// <summary>
        /// 判断是否为新老用户（2017-06-01后为新用户）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private int isNewUser(string userid)
        {
            int isnew = 0;
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userid));
            if (user != null)
            {
                isnew = user.Regdate > Convert.ToDateTime("2017-06-01") ? 1 : 0;
                return isnew;
            }
            return 0;
        }

        /// <summary>
        /// 批量插入答案语句
        /// </summary>
        /// <param name="listCSA"></param>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        private StringBuilder BatchInsertSQL(IList<Custom_StuAnswer> listCSA, string userid, int catalogid)
        {
            StringBuilder sbI = new StringBuilder();
            for (int i = 0; i < listCSA.Count; i++)
            {
                sbI.AppendFormat(@"insert into FZ_Exampaper.[dbo].Tb_StuAnswer
	                (
		                StuAnswerID,StuCatID,StuID,CatalogID,QuestionID,ParentID,Answer,IsRight,BestAnswer,BestIsRight,BestScore
	                ) 
	                values
	                (
                      cast(newid() as varchar(50)),@StuCatID,'{0}',{1},'{2}','{3}','{4}','{5}','{4}','{5}','{6}'
	                );",
                userid, //{0}
                catalogid, //{1}
                listCSA[i].QuestionID, //{2}
                string.IsNullOrEmpty(listCSA[i].ParentID) ? "NULL" : listCSA[i].ParentID,//{3}
                (string.IsNullOrEmpty(listCSA[i].Answer) ? "" : listCSA[i].Answer.Replace("'", "''")), //{4}
                listCSA[i].IsRight, //{5}
                listCSA[i].Score);//{6}
            }
            return sbI;
        }
        /// <summary>
        /// 批量更新答案语句
        /// </summary>
        /// <param name="listCSA"></param>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        private StringBuilder BatchUpdateSQL(IList<Custom_StuAnswer> listCSA, string userid, int catalogid)
        {
            StringBuilder sbU = new StringBuilder();
            sbU.Append("if @IsBest=0 begin ");//最佳成绩不变时
            for (int i = 0; i < listCSA.Count; i++)
            {
                sbU.Append(string.Format("if exists(select 1 from Tb_StuAnswer where StuCatID=@StuCatID and QuestionID='{2}') "
                    + "begin update Tb_StuAnswer set Answer='{4}',IsRight={5},Score={6} where StuCatID=@StuCatID and QuestionID='{2}' end "
                    + "else begin insert into Tb_StuAnswer(StuAnswerID,StuCatID,StuID,CatalogID,QuestionID,ParentID,Answer,IsRight,Score,BestAnswer,BestIsRight,BestScore) "
                    + "values(cast(newid() as varchar(50)),@StuCatID,'{0}',{1},'{2}',{3},'{4}',{5},{6},'{4}',{5},{6}) end ", userid, catalogid, listCSA[i].QuestionID,
                    (string.IsNullOrEmpty(listCSA[i].ParentID) ? "NULL" : ("'" + listCSA[i].ParentID + "'")), (string.IsNullOrEmpty(listCSA[i].Answer) ? "" : listCSA[i].Answer),
                    listCSA[i].IsRight, listCSA[i].Score));
            }
            sbU.Append(" end else begin ");//当前成绩为最佳成绩时
            for (int i = 0; i < listCSA.Count; i++)
            {
                sbU.Append(string.Format("if exists(select 1 from Tb_StuAnswer where StuCatID=@StuCatID and QuestionID='{2}') "
                    + "begin update Tb_StuAnswer set Answer='{4}',IsRight={5},Score={6},BestAnswer='{4}',BestIsRight={5},BestScore={6} where StuCatID=@StuCatID and QuestionID='{2}' end "
                    + "else begin insert into Tb_StuAnswer(StuAnswerID,StuCatID,StuID,CatalogID,QuestionID,ParentID,Answer,IsRight,Score,BestAnswer,BestIsRight,BestScore) "
                    + "values(cast(newid() as varchar(50)),@StuCatID,'{0}',{1},'{2}',{3},'{4}',{5},{6},'{4}',{5},{6}) end ", userid, catalogid, listCSA[i].QuestionID,
                    (string.IsNullOrEmpty(listCSA[i].ParentID) ? "NULL" : ("'" + listCSA[i].ParentID + "'")), (string.IsNullOrEmpty(listCSA[i].Answer) ? "" : listCSA[i].Answer),
                    listCSA[i].IsRight, listCSA[i].Score));
            }
            sbU.Append(" end ");
            return sbU;
        }

        public string SubmitStuBestAnswer(List<ExamPaper.Model.Tb_StuAnswer> answers, string userId)
        {
            int totalCount, totalPages;
            //先更新最佳成绩:
            answers.ForEach(o =>
            {
                var q = Select<Tb_QuestionInfo>(o.QuestionID);
                o.Score = o.Score / 100M * q.Score;
                o.BestScore = o.Score;
            });
            var currentScore = answers.Sum(o => o.BestScore.Value) / answers.Count;
            var catalogID = answers[0].CatalogID;

            using (TransactionScope scope = new TransactionScope())//事务
            {
                string bestScoreSql = string.Format(" StuID='{0}' and CatalogID='{1}'", userId, catalogID);
                var stuCatalog = Search<Tb_StuCatalog>(bestScoreSql);

                if (stuCatalog != null && stuCatalog.Any())
                {
                    var best = stuCatalog[0];
                    if (best.BestTotalScore <= currentScore)//当前分数更高,update
                    {
                        best.BestTotalScore = currentScore;
                        best.TotalScore = currentScore;
                        best.DoDate = DateTime.Now.Date;
                        var success = Update<Tb_StuCatalog>(best);
                        if (success)
                        {
                            string deleteAnswers = string.Format("delete from Tb_StuAnswer where StuCatID='{0}'", best.StuCatID);
                            ExecuteSql(deleteAnswers);
                            foreach (var o in answers)
                            {
                                o.StuAnswerID = Guid.NewGuid().ToString();
                                o.StuCatID = best.StuCatID;
                                o.StuID = userId;
                                o.IsRight = 1;
                                o.BestIsRight = 1;

                                if (!Insert(o))
                                {
                                    return "保存学生答案出错";
                                };//插入
                            }

                        }
                        else
                        {
                            return "插入学生做题目录失败";
                        }
                    }
                }
                else//不存在做题成绩,insert
                {
                    Tb_StuCatalog newStuCata = new Tb_StuCatalog
                    {
                        StuCatID = Guid.NewGuid().ToString(),
                        BestTotalScore = currentScore,
                        CatalogID = catalogID,
                        DoDate = DateTime.Now.Date,
                        TotalScore = currentScore,
                        StuID = userId
                    };
                    var success = Insert<Tb_StuCatalog>(newStuCata);
                    if (success)
                    {
                        foreach (var o in answers)
                        {
                            o.StuAnswerID = Guid.NewGuid().ToString();
                            o.StuCatID = newStuCata.StuCatID;
                            o.StuID = userId;
                            o.IsRight = 1;
                            o.BestIsRight = 1;
                            if (!Insert(o))//插入
                            {
                                return "保存学生答案出错";
                            }
                        }

                    }
                    else
                    {
                        return "插入学生做题目录失败";
                    }
                }
                scope.Complete();
                return "提交成功";
            }

        }

        public string SubmitStuDailyBestAnswer(List<ExamPaper.Model.Tb_StuAnswer_Month> answers, string userId)
        {
            int totalCount, totalPages;
            //先更新最佳成绩:
            answers.ForEach(o =>
            {
                var q = Select<Tb_QuestionInfo>(o.QuestionID);
                o.Score = o.Score / 100M * q.Score;
                o.BestScore = o.Score;
            });
            var currentScore = answers.Sum(o => o.BestScore.Value) / answers.Count;
            var catalogID = answers[0].CatalogID;
            using (TransactionScope scope = new TransactionScope())//事务
            {
                string bestScoreSql = string.Format(" StuID='{0}' and CatalogID='{1}' and DoDate>='{2}' and DoDate<'{3}'", userId, catalogID, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                var stuCatalog = Search<Tb_StuCatalog_Month>(bestScoreSql);
                if (stuCatalog != null && stuCatalog.Any())
                {
                    var best = stuCatalog[0];
                    if (best.BestTotalScore <= currentScore)//当前分数更高,update
                    {
                        best.BestTotalScore = currentScore;
                        best.DoDate = DateTime.Now.Date;
                        best.TotalScore = currentScore;
                        var success = Update<Tb_StuCatalog_Month>(best);
                        if (success)
                        {
                            string deleteAnswers = string.Format("delete from Tb_StuAnswer_Month where StuCatID='{0}'", best.StuCatID);
                            ExecuteSql(deleteAnswers);
                            foreach (var o in answers)
                            {
                                o.StuAnswerID = Guid.NewGuid().ToString();
                                o.StuCatID = best.StuCatID;
                                o.StuID = userId;
                                o.IsRight = 1;
                                o.BestIsRight = 1;
                                o.DoDate = best.DoDate;
                                if (!Insert(o))
                                {
                                    return "保存学生答案出错";
                                };//插入
                            }

                        }
                        else
                        {
                            return "插入学生做题目录失败";
                        }
                    }
                }
                else//不存在做题成绩,insert
                {
                    Tb_StuCatalog_Month newStuCata = new Tb_StuCatalog_Month
                    {
                        StuCatID = Guid.NewGuid().ToString(),
                        BestTotalScore = currentScore,
                        CatalogID = catalogID,
                        DoDate = DateTime.Now.Date,
                        TotalScore = currentScore,
                        StuID = userId
                    };
                    var success = Insert<Tb_StuCatalog_Month>(newStuCata);
                    if (success)
                    {
                        foreach (var o in answers)
                        {
                            o.StuAnswerID = Guid.NewGuid().ToString();
                            o.StuCatID = newStuCata.StuCatID;
                            o.StuID = userId;
                            o.IsRight = 1;
                            o.BestIsRight = 1;
                            o.DoDate = newStuCata.DoDate;
                            if (!Insert(o))//插入
                            {
                                return "保存学生答案出错";
                            }
                        }
                    }
                    else
                    {
                        return "插入学生做题目录失败";
                    }
                }
                scope.Complete();
                return "提交成功";
            }

        }

    }

    public class UserInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public DateTime RegDate { get; set; }
    }

}
