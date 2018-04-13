using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.IDAL;
using Kingsun.IBS.Model.IBSLearnReport;
using Kingsun.SynchronousStudy.Common;
using ServiceStack.Text;

namespace Kingsun.IBS.DAL
{
    public class IBSInitStuCatalogDAL : IIBSInitStuCatalogDAL
    {
        private int SubjectID = 3;
        private int ModelType = 2;
        public string datetime = ConfigurationManager.AppSettings["StudyReportInitTime"];

        /// <summary>
        /// 初始化Rds_StudyReport_ModuleTitle
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool InitializeUserInfo(string connectionstring)
        {
            string sql = string.Format(@"SELECT  a.StuID ,
                                                a.StuCatID ,
                                                a.CatalogID ,
                                                a.TotalScore ,
                                                a.BestTotalScore ,
                                                CONVERT(VARCHAR(10), a.DoDate, 120) DoDate,
                                                a.AnswerNum ,
                                                c.CatalogName ,
                                                c.BookID
                                        FROM    FZ_Exampaper.dbo.Tb_StuCatalog a
                                                JOIN FZ_Exampaper.dbo.QTb_Catalog c ON a.CatalogID = c.CatalogID
                                                where a.DoDate<'{0}'", datetime);
            DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
            List<StuCatalog> uvi = JsonHelper.DataSetToIList<StuCatalog>(ds, 0);

            using (var Redis = RedisManager.GetClient(0))
            {
                foreach (var info in uvi)
                {
                    Rds_StudyReport_ModuleTitle learningRecord = new Rds_StudyReport_ModuleTitle();

                    #region Rds_StudyReport_ModuleTitle

                    string MKey = info.StuID + "_" + SubjectID + "_" + ModelType;
                    try
                    {
                        string value = Redis.GetValueFromHash("Rds_StudyReport_ModuleTitle_" + info.StuID.Substring(0, 2), MKey);
                        //用户没有记录
                        if (string.IsNullOrEmpty(value))
                        {
                            learningRecord = new Rds_StudyReport_ModuleTitle();
                            learningRecord.UserID = info.StuID.ToInt();
                            learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                            {
                                BookID = info.BookID,
                                VideoNumber = info.CatalogID,
                                VideoID = info.StuCatID,
                                BestScore = (float)info.BestTotalScore,
                                CreateTime = info.DoDate,
                                DubbingNum = info.AnswerNum
                            });
                        }
                        else
                        {
                            learningRecord = JsonSerializer.DeserializeFromString<Rds_StudyReport_ModuleTitle>(value);
                            //用户有做题记录
                            if (learningRecord.detail.Count > 0)
                            {
                                //用户没有这套试卷做题记录
                                List<Rds_StudyReport_BookDetail> bt = learningRecord.detail.Where(i => i.BookID == info.BookID && i.VideoNumber == info.CatalogID).ToList();
                                if (bt.Count <= 0)
                                {
                                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                    {
                                        BookID = info.BookID,
                                        VideoNumber = info.CatalogID,
                                        VideoID = info.StuCatID,
                                        BestScore = (float)info.BestTotalScore,
                                        CreateTime = info.DoDate,
                                        DubbingNum = info.AnswerNum
                                    });
                                }
                                else
                                {
                                    //用户有做题记录，是否最高分
                                    bt.ForEach(a =>
                                    {
                                        //a.DubbingNum += 1;
                                        if (info.BestTotalScore > decimal.Parse(a.BestScore.ToString()))
                                        {
                                            a.BookID = info.BookID;
                                            a.VideoNumber = info.CatalogID;
                                            a.VideoID = info.StuCatID;
                                            a.BestScore = (float)info.BestTotalScore;
                                            a.CreateTime = info.DoDate;
                                            a.DubbingNum = info.AnswerNum;
                                        }
                                    });
                                }
                            }
                            else
                            {
                                //用户没有做题记录
                                learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                {
                                    BookID = info.BookID,
                                    DubbingNum = 1,
                                    BestScore = (float)info.TotalScore,
                                    VideoID = info.StuCatID,
                                    CreateTime = info.DoDate
                                });
                            }
                        }

                        if (learningRecord.detail == null)
                        {
                            Log4Net.LogHelper.Error("learningRecord值为null,info=" + info.ToJson());
                        }
                        else if (learningRecord.detail[0] == null)
                        {
                            Log4Net.LogHelper.Error("learningRecord值为null,info=" + info.ToJson() + ",learningRecord="+learningRecord.ToJson());
                        }
                        var ve = JsonSerializer.SerializeToString<Rds_StudyReport_ModuleTitle>(learningRecord);
                        //该方法字段值为null时会丢失这个字段
                        Redis.SetEntryInHash("Rds_StudyReport_ModuleTitle_" + info.StuID.ToString().Substring(0, 2), MKey, ve);
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + MKey);
                    }

                    #endregion
                }
            }
            return true;
        }


        /// <summary>
        /// 初始化同步之后的学习报告
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool TodayInitializeUserInfo(string connectionstring)
        {
            string sql = string.Format(@"SELECT  a.StuID ,
                                                a.StuCatID ,
                                                a.CatalogID ,
                                                a.TotalScore ,
                                                a.BestTotalScore ,
                                                CONVERT(VARCHAR(10), a.DoDate, 120) DoDate,
                                                a.AnswerNum ,
                                                c.CatalogName ,
                                                c.BookID
                                        FROM    FZ_Exampaper.dbo.Tb_StuCatalog a
                                                JOIN FZ_Exampaper.dbo.QTb_Catalog c ON a.CatalogID = c.CatalogID
                                                where a.DoDate>='{0}'", datetime);
            DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
            List<StuCatalog> uvi = JsonHelper.DataSetToIList<StuCatalog>(ds, 0);

            using (var Redis = RedisManager.GetClient(0))
            {
                foreach (var info in uvi)
                {
                    Rds_StudyReport_ModuleTitle learningRecord = new Rds_StudyReport_ModuleTitle();

                    #region Rds_StudyReport_ModuleTitle

                    string MKey = info.StuID + "_" + SubjectID + "_" + ModelType;
                    try
                    {
                        string value = Redis.GetValueFromHash("Rds_StudyReport_ModuleTitle_" + info.StuID.Substring(0, 2), MKey);

                        if (string.IsNullOrEmpty(value))
                        {
                            learningRecord = new Rds_StudyReport_ModuleTitle();
                            learningRecord.UserID = info.StuID.ToInt();
                            learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                            {
                                BookID = info.BookID,
                                VideoNumber = info.CatalogID,
                                VideoID = info.StuCatID,
                                BestScore = (float)info.BestTotalScore,
                                CreateTime = info.DoDate,
                                DubbingNum = info.AnswerNum
                            });
                        }
                        else
                        {
                            learningRecord = JsonSerializer.DeserializeFromString<Rds_StudyReport_ModuleTitle>(value);
                            if (learningRecord.detail.Count > 0)
                            {
                                List<Rds_StudyReport_BookDetail> bt = learningRecord.detail.Where(i => i.BookID == info.BookID && i.VideoNumber == info.CatalogID).ToList();
                                if (bt.Count <= 0)
                                {
                                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                    {
                                        BookID = info.BookID,
                                        VideoNumber = info.CatalogID,
                                        VideoID = info.StuCatID,
                                        BestScore = (float)info.BestTotalScore,
                                        CreateTime = info.DoDate,
                                        DubbingNum = info.AnswerNum
                                    });
                                }
                                else
                                {
                                    bt.ForEach(a =>
                                    {
                                        //a.DubbingNum += 1;
                                        if (info.BestTotalScore > decimal.Parse(a.BestScore.ToString()))
                                        {
                                            a.BookID = info.BookID;
                                            a.VideoNumber = info.CatalogID;
                                            a.VideoID = info.StuCatID;
                                            a.BestScore = (float)info.BestTotalScore;
                                            a.CreateTime = info.DoDate;
                                            a.DubbingNum = info.AnswerNum;
                                        }
                                    });
                                }
                            }
                            else
                            {
                                learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                {
                                    BookID = info.BookID,
                                    DubbingNum = 1,
                                    BestScore = (float)info.TotalScore,
                                    VideoID = info.StuCatID,
                                    CreateTime = info.DoDate
                                });
                            }
                        }
                        var ve = JsonSerializer.SerializeToString<Rds_StudyReport_ModuleTitle>(learningRecord);
                        //该方法字段值为null时会丢失这个字段
                        Redis.SetEntryInHash("Rds_StudyReport_ModuleTitle_" + info.StuID.ToString().Substring(0, 2), MKey, ve);
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + MKey);
                    }

                    #endregion
                }
            }
            return true;
        }
    }

    public class ClassList
    {
        public string ClassShortID { get; set; }
        public string ClassName { get; set; }
    }

    public class StuCatalog
    {
        public string StuID { get; set; }
        public string StuCatID { get; set; }
        public int CatalogID { get; set; }
        public decimal TotalScore { get; set; }
        public decimal BestTotalScore { get; set; }
        public string DoDate { get; set; }
        public int AnswerNum { get; set; }
        public string ClassShortID { get; set; }
        public string ClassName { get; set; }
        public string CatalogName { get; set; }
        public int BookID { get; set; }
    }
}
