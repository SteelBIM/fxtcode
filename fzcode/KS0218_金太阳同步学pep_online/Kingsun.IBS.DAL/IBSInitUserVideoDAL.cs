using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.IDAL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.IBSLearnReport;
using Kingsun.SynchronousStudy.Common;
using ServiceStack.Text;

namespace Kingsun.IBS.DAL
{
    public class IBSInitUserVideoDAL : IIBSInitUserVideoDAL
    {
        private RedisHashOtherHelper redisOther = new RedisHashOtherHelper();
        private int SubjectID = 3;
        private int ModelType = 1;
        public string datetime = ConfigurationManager.AppSettings["StudyReportInitTime"];

        /// <summary>
        /// 初始化Rds_StudyReport_ModuleTitle 用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// < s></ s>
        /// 
        #region  初始化学习报告记录
        public bool InitializeRdsStudyReportModuleTitle(string connectionstring)
        {
            try
            {
                string sql = string.Format(@"SELECT  a.ID ,
                                                d.UserID ,
                                                d.VideoFileID ,
                                                d.CreateTime ,
                                                a.VersionID ,
                                                d.BookID ,
                                                d.VideoNumber ,
                                                a.VideoImageAddress ,
                                                a.IsEnableOss ,
                                                d.DubTimes ,
                                                d.TotalScore ,
                                                c.FirstTitleID ,
                                                c.FirstTitle ,
                                                c.SecondTitleID ,
                                                c.SecondTitle ,
                                                c.FirstModularID ,
                                                c.FirstModular
                                        FROM    ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
                                                            COUNT(UserID) DubTimes ,
                                                            MIN(VideoFileID) VideoFileID ,
                                                            BookID ,
                                                            VideoNumber ,
                                                            UserID ,
                                                            CONVERT(VARCHAR(10), CreateTime, 120) CreateTime
                                                  FROM      [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                                                  WHERE     BookID IS NOT NULL
                                                            AND BookID <> 0
                                                            AND VideoNumber <> 0
                                                            AND CreateTime<'{0}'
                                                  GROUP BY  BookID ,
                                                            VideoNumber ,
                                                            UserID ,
                                                            CONVERT(VARCHAR(10), CreateTime, 120)
                                                ) d
                                                LEFT JOIN FZ_InterestDubbing.dbo.TB_UserVideoDetails a ON a.UserID = d.UserID
                                                                                                      AND a.BookID = d.BookID
                                                                                                      AND a.VideoNumber = d.VideoNumber
                                                                                                      AND CONVERT(VARCHAR(10), a.CreateTime, 120) = d.CreateTime
                                                                                                      AND a.VideoFileID = d.VideoFileID
                                                LEFT        JOIN FZ_InterestDubbing.dbo.TB_VideoDetails c ON a.BookID = c.BookID
                                                                                                      AND a.VideoNumber = c.VideoNumber", datetime);
                DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                List<UserVideoInfo> uvi = JsonHelper.DataSetToIList<UserVideoInfo>(ds, 0);

                using (var Redis = RedisManager.GetClient(0))
                {
                    foreach (var info in uvi)
                    {
                        Rds_StudyReport_ModuleTitle learningRecord = new Rds_StudyReport_ModuleTitle();
                        #region Rds_StudyReport_ModuleTitle

                        string MKey = info.UserID + "_" + SubjectID + "_" + ModelType;
                        try
                        {
                            string value = Redis.GetValueFromHash("Rds_StudyReport_ModuleTitle_" + info.UserID.ToString().Substring(0, 2), MKey);

                            if (string.IsNullOrEmpty(value))
                            {
                                learningRecord = new Rds_StudyReport_ModuleTitle();
                                learningRecord.UserID = info.UserID;
                                learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                                {
                                    BookID = info.BookID,
                                    VideoNumber = info.VideoNumber,
                                    VideoID = info.ID.ToString(),
                                    BestScore = info.TotalScore,
                                    CreateTime = info.CreateTime,
                                    FirstTitleID = info.FirstTitleID,
                                    SecondTitleID = info.SecondTitleID,
                                    FirstModularID = info.FirstModularID,
                                    DubbingNum = 1
                                });
                            }
                            else
                            {
                                learningRecord = JsonSerializer.DeserializeFromString<Rds_StudyReport_ModuleTitle>(value);
                                if (learningRecord.detail.Count > 0)
                                {
                                    List<Rds_StudyReport_BookDetail> bt = learningRecord.detail.Where(i => i.BookID == info.BookID && i.VideoNumber == info.VideoNumber).ToList();
                                    if (bt.Count <= 0)
                                    {
                                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                        {
                                            BookID = info.BookID,
                                            VideoNumber = info.VideoNumber,
                                            VideoID = info.ID.ToString(),
                                            BestScore = info.TotalScore,
                                            CreateTime = info.CreateTime,
                                            FirstTitleID = info.FirstTitleID,
                                            SecondTitleID = info.SecondTitleID,
                                            FirstModularID = info.FirstModularID,
                                            DubbingNum = 1
                                        });
                                    }
                                    else
                                    {
                                        bt.ForEach(a =>
                                        {
                                            a.DubbingNum += 1;
                                            if (info.TotalScore > a.BestScore)
                                            {
                                                a.BookID = info.BookID;
                                                a.VideoNumber = info.VideoNumber;
                                                a.VideoID = info.ID.ToString();
                                                a.BestScore = info.TotalScore;
                                                a.CreateTime = info.CreateTime;
                                                a.FirstTitleID = info.FirstTitleID;
                                                a.SecondTitleID = info.SecondTitleID;
                                                a.FirstModularID = info.FirstModularID;
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    if (info.FirstTitleID != null)
                                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                        {
                                            BookID = info.BookID,
                                            VideoNumber = info.VideoNumber,
                                            VideoID = info.ID.ToString(),
                                            BestScore = info.TotalScore,
                                            CreateTime = info.CreateTime,
                                            FirstTitleID = info.FirstTitleID,
                                            SecondTitleID = info.SecondTitleID,
                                            FirstModularID = info.FirstModularID,
                                            DubbingNum = 1
                                        });
                                }
                            }
                            var ve = JsonSerializer.SerializeToString<Rds_StudyReport_ModuleTitle>(learningRecord);//该方法字段值为null时会丢失这个字段
                            Redis.SetEntryInHash("Rds_StudyReport_ModuleTitle_" + info.UserID.ToString().Substring(0, 2), MKey, ve);
                        }
                        catch (Exception ex)
                        {
                            Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + MKey);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeRdsStudyReportModuleTitle");
            }
            return true;
        }

        public bool initStudyReportBookCatalogues(string connectionstring)
        {

            string strsql = string.Format(@"SELECT  a.BookID ,
                                                a.VideoNumber ,
                                                a.VideoImageAddress ,
                                                a.IsEnableOss ,
                                                c.VideoTitle ,
                                                c.FirstTitleID ,
                                                c.FirstTitle,
                                                c.SecondTitleID,
                                                c.SecondTitle
                                        FROM    FZ_InterestDubbing.dbo.TB_UserVideoDetails a
                                                RIGHT JOIN ( SELECT BookID ,
                                                                    VideoNumber ,
                                                                    MAX(VideoImageAddress) VideoImageAddress
                                                                FROM   FZ_InterestDubbing.dbo.TB_UserVideoDetails
                                                                WHERE  BookID IS NOT NULL
                                                                    AND BookID <> 0
                                                                    AND VideoNumber <> 0
                                                                    AND CreateTime<'{0}' 
                                                                GROUP BY BookID ,
                                                                    VideoNumber
                                                            ) b ON b.BookID = a.BookID
                                                                    AND b.VideoImageAddress = a.VideoImageAddress
                                                                    AND b.VideoNumber = a.VideoNumber
                                                 JOIN FZ_InterestDubbing.dbo.TB_VideoDetails c ON c.BookID = b.BookID
                                                                                                        AND c.VideoNumber = b.VideoNumber
                                                                                                        where a.CreateTime<'{0}' 
                                                                                                        group by a.BookID ,
                                                a.VideoNumber ,
                                                a.VideoImageAddress ,
                                                a.IsEnableOss ,
                                                c.VideoTitle ,
                                                c.FirstTitleID ,
                                                c.FirstTitle,
                                                c.SecondTitleID,
                                                c.SecondTitle", datetime);
            DataSet dsBookCatalogues = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, strsql);
            List<BookCatalogues> BCList = JsonHelper.DataSetToIList<BookCatalogues>(dsBookCatalogues, 0);

            using (var Redis = RedisManager.GetClient(0))
            {
                foreach (var item in BCList)
                {

                    var value = Redis.GetValueFromHash("Rds_StudyReport_BookCatalogues_" + item.BookID, item.FirstTitleID + "_" + item.SecondTitleID + "_" + ModelType);
                    Rds_StudyReport_BookCatalogues_BookID rdsBookCatalogues;

                    if (string.IsNullOrEmpty(value))
                    {
                        List<Rds_StudyReport_BookCatalogues_Video> rdsLiost =
                            new List<Rds_StudyReport_BookCatalogues_Video>();
                        rdsLiost.Add(new Rds_StudyReport_BookCatalogues_Video()
                        {

                            VideoImageAddress = item.VideoImageAddress,
                            IsEnableOss = item.IsEnableOss,
                            VideoTitle = item.VideoTitle,
                            VideoNumber = item.VideoNumber
                        });
                        rdsBookCatalogues = new Rds_StudyReport_BookCatalogues_BookID()
                        {
                            FirstTitleID = item.FirstTitleID,
                            FirstTitle = item.FirstTitle,
                            SecondTitleID = item.SecondTitleID,
                            SecondTitle = item.SecondTitle,
                            FirstModularID = item.FirstModularID,
                            FirstModular = item.FirstModular,
                            Videos = rdsLiost
                        };


                    }
                    else
                    {
                        rdsBookCatalogues = JsonSerializer.DeserializeFromString<Rds_StudyReport_BookCatalogues_BookID>(value);

                        rdsBookCatalogues.FirstTitleID = item.FirstTitleID;
                        rdsBookCatalogues.FirstTitle = item.FirstTitle;
                        rdsBookCatalogues.SecondTitleID = item.SecondTitleID;
                        rdsBookCatalogues.SecondTitle = item.SecondTitle;
                        rdsBookCatalogues.FirstModularID = item.FirstModularID;
                        rdsBookCatalogues.FirstModular = item.FirstModular;
                        var val = rdsBookCatalogues.Videos.FirstOrDefault(a => a.VideoNumber == item.VideoNumber);
                        if (val == null)
                        {
                            rdsBookCatalogues.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoImageAddress = item.VideoImageAddress,
                                IsEnableOss = item.IsEnableOss,
                                VideoTitle = item.VideoTitle,
                                VideoNumber = item.VideoNumber
                            });
                        }

                    }
                    var ve = JsonSerializer.SerializeToString<Rds_StudyReport_BookCatalogues_BookID>(rdsBookCatalogues);//该方法字段值为null时会丢失这个字段
                    Redis.SetEntryInHash("Rds_StudyReport_BookCatalogues_" + item.BookID, item.FirstTitleID + "_" + item.SecondTitleID + "_" + ModelType, ve);

                }
            }

            return true;
        }
        #endregion
        #region 初始化同步之后的学习报告记录
        public bool TodayInitializeRdsStudyReportModuleTitle(string connectionstring)
        {
            try
            {
                string sql = string.Format(@"SELECT  a.ID ,
                                                d.UserID ,
                                                d.VideoFileID ,
                                                d.CreateTime ,
                                                a.VersionID ,
                                                d.BookID ,
                                                d.VideoNumber ,
                                                a.VideoImageAddress ,
                                                a.IsEnableOss ,
                                                d.DubTimes ,
                                                d.TotalScore ,
                                                c.FirstTitleID ,
                                                c.FirstTitle ,
                                                c.SecondTitleID ,
                                                c.SecondTitle ,
                                                c.FirstModularID ,
                                                c.FirstModular
                                        FROM    ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
                                                            COUNT(UserID) DubTimes ,
                                                            MIN(VideoFileID) VideoFileID ,
                                                            BookID ,
                                                            VideoNumber ,
                                                            UserID ,
                                                            CONVERT(VARCHAR(10), CreateTime, 120) CreateTime
                                                  FROM      [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                                                  WHERE     BookID IS NOT NULL
                                                            AND BookID <> 0
                                                            AND VideoNumber <> 0
                                                            AND CreateTime>='{0}'
                                                  GROUP BY  BookID ,
                                                            VideoNumber ,
                                                            UserID ,
                                                            CONVERT(VARCHAR(10), CreateTime, 120)
                                                ) d
                                                LEFT JOIN FZ_InterestDubbing.dbo.TB_UserVideoDetails a ON a.UserID = d.UserID
                                                                                                      AND a.BookID = d.BookID
                                                                                                      AND a.VideoNumber = d.VideoNumber
                                                                                                      AND CONVERT(VARCHAR(10), a.CreateTime, 120) = d.CreateTime
                                                                                                      AND a.VideoFileID = d.VideoFileID
                                                LEFT        JOIN FZ_InterestDubbing.dbo.TB_VideoDetails c ON a.BookID = c.BookID
                                                                                                      AND a.VideoNumber = c.VideoNumber", datetime);
                DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                List<UserVideoInfo> uvi = JsonHelper.DataSetToIList<UserVideoInfo>(ds, 0);

                using (var Redis = RedisManager.GetClient(0))
                {
                    foreach (var info in uvi)
                    {
                        Rds_StudyReport_ModuleTitle learningRecord = new Rds_StudyReport_ModuleTitle();
                        #region Rds_StudyReport_ModuleTitle

                        string MKey = info.UserID + "_" + SubjectID + "_" + ModelType;
                        try
                        {
                            string value = Redis.GetValueFromHash("Rds_StudyReport_ModuleTitle_" + info.UserID.ToString().Substring(0, 2), MKey);

                            if (string.IsNullOrEmpty(value))
                            {
                                learningRecord = new Rds_StudyReport_ModuleTitle();
                                learningRecord.UserID = info.UserID;
                                learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                                {
                                    BookID = info.BookID,
                                    VideoNumber = info.VideoNumber,
                                    VideoID = info.ID.ToString(),
                                    BestScore = info.TotalScore,
                                    CreateTime = info.CreateTime,
                                    FirstTitleID = info.FirstTitleID,
                                    SecondTitleID = info.SecondTitleID,
                                    FirstModularID = info.FirstModularID,
                                    DubbingNum = 1
                                });
                            }
                            else
                            {
                                learningRecord = JsonSerializer.DeserializeFromString<Rds_StudyReport_ModuleTitle>(value);
                                if (learningRecord.detail.Count > 0)
                                {
                                    List<Rds_StudyReport_BookDetail> bt = learningRecord.detail.Where(i => i.BookID == info.BookID && i.VideoNumber == info.VideoNumber).ToList();
                                    if (bt.Count <= 0)
                                    {
                                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                        {
                                            BookID = info.BookID,
                                            VideoNumber = info.VideoNumber,
                                            VideoID = info.ID.ToString(),
                                            BestScore = info.TotalScore,
                                            CreateTime = info.CreateTime,
                                            FirstTitleID = info.FirstTitleID,
                                            SecondTitleID = info.SecondTitleID,
                                            FirstModularID = info.FirstModularID,
                                            DubbingNum = 1
                                        });
                                    }
                                    else
                                    {
                                        bt.ForEach(a =>
                                        {
                                            a.DubbingNum += 1;
                                            if (info.TotalScore > a.BestScore)
                                            {
                                                a.BookID = info.BookID;
                                                a.VideoNumber = info.VideoNumber;
                                                a.VideoID = info.ID.ToString();
                                                a.BestScore = info.TotalScore;
                                                a.CreateTime = info.CreateTime;
                                                a.FirstTitleID = info.FirstTitleID;
                                                a.SecondTitleID = info.SecondTitleID;
                                                a.FirstModularID = info.FirstModularID;
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    if (info.FirstTitleID != null)
                                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail
                                        {
                                            BookID = info.BookID,
                                            VideoNumber = info.VideoNumber,
                                            VideoID = info.ID.ToString(),
                                            BestScore = info.TotalScore,
                                            CreateTime = info.CreateTime,
                                            FirstTitleID = info.FirstTitleID,
                                            SecondTitleID = info.SecondTitleID,
                                            FirstModularID = info.FirstModularID,
                                            DubbingNum = 1
                                        });
                                }
                            }
                            var ve = JsonSerializer.SerializeToString<Rds_StudyReport_ModuleTitle>(learningRecord);//该方法字段值为null时会丢失这个字段
                            Redis.SetEntryInHash("Rds_StudyReport_ModuleTitle_" + info.UserID.ToString().Substring(0, 2), MKey, ve);
                        }
                        catch (Exception ex)
                        {
                            Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + MKey);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeRdsStudyReportModuleTitle");
            }
            return true;
        }

        public bool TodayinitStudyReportBookCatalogues(string connectionstring)
        {

            string strsql = string.Format(@"SELECT  a.BookID ,
                                                a.VideoNumber ,
                                                a.VideoImageAddress ,
                                                a.IsEnableOss ,
                                                c.VideoTitle ,
                                                c.FirstTitleID ,
                                                c.FirstTitle,
                                                c.SecondTitleID,
                                                c.SecondTitle
                                        FROM    FZ_InterestDubbing.dbo.TB_UserVideoDetails a
                                                RIGHT JOIN ( SELECT BookID ,
                                                                    VideoNumber ,
                                                                    MAX(VideoImageAddress) VideoImageAddress
                                                                FROM   FZ_InterestDubbing.dbo.TB_UserVideoDetails
                                                                WHERE  BookID IS NOT NULL
                                                                    AND BookID <> 0
                                                                    AND VideoNumber <> 0
                                                                    AND CreateTime>='{0}' 
                                                                GROUP BY BookID ,
                                                                    VideoNumber
                                                            ) b ON b.BookID = a.BookID
                                                                    AND b.VideoImageAddress = a.VideoImageAddress
                                                                    AND b.VideoNumber = a.VideoNumber
                                                 JOIN FZ_InterestDubbing.dbo.TB_VideoDetails c ON c.BookID = b.BookID
                                                                                                        AND c.VideoNumber = b.VideoNumber
                                                                                                        where a.CreateTime>='{0}'
                                                                                                        group by a.BookID ,
                                                a.VideoNumber ,
                                                a.VideoImageAddress ,
                                                a.IsEnableOss ,
                                                c.VideoTitle ,
                                                c.FirstTitleID ,
                                                c.FirstTitle,
                                                c.SecondTitleID,
                                                c.SecondTitle", datetime);
            DataSet dsBookCatalogues = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, strsql);
            List<BookCatalogues> BCList = JsonHelper.DataSetToIList<BookCatalogues>(dsBookCatalogues, 0);

            using (var Redis = RedisManager.GetClient(0))
            {
                foreach (var item in BCList)
                {
                    var value = Redis.GetValueFromHash("Rds_StudyReport_BookCatalogues_" + item.BookID, item.FirstTitleID + "_" + item.SecondTitleID + "_" + ModelType);
                    Rds_StudyReport_BookCatalogues_BookID rdsBookCatalogues;

                    if (string.IsNullOrEmpty(value))
                    {
                        List<Rds_StudyReport_BookCatalogues_Video> rdsLiost =
                            new List<Rds_StudyReport_BookCatalogues_Video>();
                        rdsLiost.Add(new Rds_StudyReport_BookCatalogues_Video()
                        {

                            VideoImageAddress = item.VideoImageAddress,
                            IsEnableOss = item.IsEnableOss,
                            VideoTitle = item.VideoTitle,
                            VideoNumber = item.VideoNumber
                        });
                        rdsBookCatalogues = new Rds_StudyReport_BookCatalogues_BookID()
                        {
                            FirstTitleID = item.FirstTitleID,
                            FirstTitle = item.FirstTitle,
                            SecondTitleID = item.SecondTitleID,
                            SecondTitle = item.SecondTitle,
                            FirstModularID = item.FirstModularID,
                            FirstModular = item.FirstModular,
                            Videos = rdsLiost
                        };


                    }
                    else
                    {
                        rdsBookCatalogues = JsonSerializer.DeserializeFromString<Rds_StudyReport_BookCatalogues_BookID>(value);

                        rdsBookCatalogues.FirstTitleID = item.FirstTitleID;
                        rdsBookCatalogues.FirstTitle = item.FirstTitle;
                        rdsBookCatalogues.SecondTitleID = item.SecondTitleID;
                        rdsBookCatalogues.SecondTitle = item.SecondTitle;
                        rdsBookCatalogues.FirstModularID = item.FirstModularID;
                        rdsBookCatalogues.FirstModular = item.FirstModular;
                        var val = rdsBookCatalogues.Videos.FirstOrDefault(a => a.VideoNumber == item.VideoNumber);
                        if (val == null)
                        {
                            rdsBookCatalogues.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoImageAddress = item.VideoImageAddress,
                                IsEnableOss = item.IsEnableOss,
                                VideoTitle = item.VideoTitle,
                                VideoNumber = item.VideoNumber
                            });
                        }

                    }
                    var ve = JsonSerializer.SerializeToString<Rds_StudyReport_BookCatalogues_BookID>(rdsBookCatalogues);//该方法字段值为null时会丢失这个字段
                    Redis.SetEntryInHash("Rds_StudyReport_BookCatalogues_" + item.BookID, item.FirstTitleID + "_" + item.SecondTitleID + "_" + ModelType, ve);

                }
            }

            return true;
        }
        #endregion
        public class BookCatalogues
        {
            public int BookID { get; set; }
            public int FirstTitleID { get; set; }
            public string FirstTitle { get; set; }
            public int SecondTitleID { get; set; }
            public string SecondTitle { get; set; }
            public int FirstModularID { get; set; }
            public string FirstModular { get; set; }
            public int VideoNumber { get; set; }
            public string VideoImageAddress { get; set; }
            public int IsEnableOss { get; set; }
            public string VideoTitle { get; set; }
        }

        public class BookInfo
        {
            public int BookID { get; set; }
            public int FirstTitleID { get; set; }
            public string FirstTitle { get; set; }
            public int SecondTitleID { get; set; }
            public string SecondTitle { get; set; }
            public int VideoNumber { get; set; }
        }

        public class ClassList
        {
            public string ClassShortID { get; set; }
            public string ClassName { get; set; }
        }

        public class UserVideoInfo
        {
            public int ID { get; set; }
            public int UserID { get; set; }
            public string VideoFileID { get; set; }
            public string CreateTime { get; set; }
            public int VersionID { get; set; }
            public int BookID { get; set; }
            public int VideoNumber { get; set; }
            public string VideoImageAddress { get; set; }
            public int IsEnableOss { get; set; }
            public int DubTimes { get; set; }
            public double TotalScore { get; set; }
            public int? FirstTitleID { get; set; }
            public string FirstTitle { get; set; }
            public int? SecondTitleID { get; set; }
            public string SecondTitle { get; set; }
            public int? FirstModularID { get; set; }
            public string FirstModular { get; set; }
        }


    }
}
