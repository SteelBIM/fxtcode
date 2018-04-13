using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.DAL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.IDAL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.IBSLearnReport;
using Kingsun.SynchronousStudy.Common;
using ServiceStack.Text;

namespace Kingsun.IBS.BLL.IBSLearningReport
{
    public class IBSInitUserVideoBLL : IIBSInitUserVideoBLL
    {
        private IIBSInitUserVideoDAL uvDal = new IBSInitUserVideoDAL();
        private IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private int SubjectID = 3;
        private int ModelType = 1;

        public string datetime = ConfigurationManager.AppSettings["StudyReportInitTime"];

        public bool RemoveRedis()
        {
            #region 删除redis

            using (var Redis = RedisManager.GetClient(0))
            {
                List<string> tudyReport_ModuleTitleList = Redis.SearchKeys("Rds_StudyReport_*");
                foreach (var item in tudyReport_ModuleTitleList)
                {
                    Redis.Remove(item);
                }
                Redis.Dispose();

            }
            #endregion

            return true;
        }



        #region 初始化学习报告记录
        /// <summary>
        /// 初始化用户配音班级和书本报告记录数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool InitializeUserVideoInfo(string connectionstring)
        {
            try
            {


                int totalcount = 0;
                string sql1 =
                    string.Format(@"SELECT count(*) FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                                                          ", datetime);
                DataSet ds1 = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    totalcount = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString()) + 10000;
                }

                int pageCount = 100000;
                int pageSize = totalcount / pageCount + ((totalcount % pageCount == 0) ? 0 : 1);
                for (int i = 0; i < pageSize; i++)
                {
                    string sql = string.Format(@"SELECT  a.ID ,
                                                    a.UserID ,
                                                    a.VideoFileID ,
                                                    a.CreateTime ,
                                                    a.VersionID ,
                                                    a.BookID ,
                                                    a.VideoNumber ,
                                                    a.VideoImageAddress ,
                                                    a.IsEnableOss ,
                                                    a.DubTimes ,
                                                    a.TotalScore
                                            FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY a.ID ASC ) num ,
                                                                a.ID ,
                                                                d.UserID ,
                                                                d.VideoFileID ,
                                                                d.CreateTime ,
                                                                a.VersionID ,
                                                                d.BookID ,
                                                                d.VideoNumber ,
                                                                a.VideoImageAddress ,
                                                                a.IsEnableOss ,
                                                                d.DubTimes ,
                                                                d.TotalScore
                                                      FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                            AND CreateTime<'{2}'
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
                                                    ) a
                                            WHERE   a.num BETWEEN {0} AND {1}", (pageCount * i) + 1, pageCount * (i + 1), datetime);
                    DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                    List<IBSInitUserVideoDAL.UserVideoInfo> uvi = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.UserVideoInfo>(ds, 0);

                    using (var Redis = RedisManager.GetClient(0))
                    {

                        foreach (var item in uvi)
                        {


                            try
                            {
                                ClassInfoByUserID classinfo = userBLL.GetClassInfoByUserID(item.UserID);
                                if (classinfo != null)
                                {
                                   
                                    #region Rds_StudyReport_Class
                                    string CKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType;
                                    string classValue = Redis.GetValueFromHash("Rds_StudyReport_Class", CKey);

                                    if (string.IsNullOrEmpty(classValue))
                                    {
                                        Rds_StudyReport_Class rdsBook = new Rds_StudyReport_Class()
                                        {
                                            ClassID = classinfo.ClassNum,
                                            Flag = 0,
                                            SubjectID = SubjectID,
                                            ModuleType = ModelType
                                        };
                                        var ve = JsonSerializer.SerializeToString<Rds_StudyReport_Class>(rdsBook);//该方法字段值为null时会丢失这个字段
                                        Redis.SetEntryInHash("Rds_StudyReport_Class", CKey, ve);
                                    }
                                    #endregion
                                }

                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "错误：初始化Rds_StudyReport_Class和Rds_StudyReport_Book失败，数据记录为" + item.ToJson());
                            }
                        }
                        Redis.Dispose();
                    }
                    Log4Net.LogHelper.Info("第" + i + "次初始化Rds_StudyReport_Class和Rds_StudyReport_Book成功！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeUserVideoInfo");
                return false;
            }
            return true;
        }


        public bool InitializeBook(string connectionstring)
        {
            try
            {


                int totalcount = 0;
                string sql1 =
                    string.Format(@"SELECT count(*) FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                                                          ", datetime);
                DataSet ds1 = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    totalcount = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString()) + 10000;
                }

                int pageCount = 100000;
                int pageSize = totalcount / pageCount + ((totalcount % pageCount == 0) ? 0 : 1);
                for (int i = 0; i < pageSize; i++)
                {
                    string sql = string.Format(@"SELECT  a.ID ,
                                                    a.UserID ,
                                                    a.VideoFileID ,
                                                    a.CreateTime ,
                                                    a.VersionID ,
                                                    a.BookID ,
                                                    a.VideoNumber ,
                                                    a.VideoImageAddress ,
                                                    a.IsEnableOss ,
                                                    a.DubTimes ,
                                                    a.TotalScore
                                            FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY a.ID ASC ) num ,
                                                                a.ID ,
                                                                d.UserID ,
                                                                d.VideoFileID ,
                                                                d.CreateTime ,
                                                                a.VersionID ,
                                                                d.BookID ,
                                                                d.VideoNumber ,
                                                                a.VideoImageAddress ,
                                                                a.IsEnableOss ,
                                                                d.DubTimes ,
                                                                d.TotalScore
                                                      FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                            AND CreateTime<'{2}'
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
                                                    ) a
                                            WHERE   a.num BETWEEN {0} AND {1}", (pageCount * i) + 1, pageCount * (i + 1), datetime);
                    DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                    List<IBSInitUserVideoDAL.UserVideoInfo> uvi = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.UserVideoInfo>(ds, 0);

                    using (var Redis = RedisManager.GetClient(0))
                    {

                        foreach (var item in uvi)
                        {


                            try
                            {
                                ClassInfoByUserID classinfo = userBLL.GetClassInfoByUserID(item.UserID);
                                if (classinfo != null)
                                {
                                    #region Rds_StudyReport_Book
                                    string BKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType + "_" + item.BookID;
                                    string value = Redis.GetValueFromHash("Rds_StudyReport_Book", BKey);

                                    if (string.IsNullOrEmpty(value))
                                    {
                                        Rds_StudyReport_Book rdsBook = new Rds_StudyReport_Book()
                                        {
                                            BookID = item.BookID,
                                            ClassID = classinfo.ClassNum,
                                            Flag = 0,
                                            SubjectID = SubjectID,
                                            ModuleType = ModelType
                                        };
                                        var ve = JsonSerializer.SerializeToString<Rds_StudyReport_Book>(rdsBook);
                                        //该方法字段值为null时会丢失这个字段
                                        Redis.SetEntryInHash("Rds_StudyReport_Book", BKey, ve);
                                    }
                                    #endregion
                                }

                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "错误：初始化Rds_StudyReport_Class和Rds_StudyReport_Book失败，数据记录为" + item.ToJson());
                            }
                        }
                        Redis.Dispose();
                    }
                    Log4Net.LogHelper.Info("第" + i + "次初始化Rds_StudyReport_Class和Rds_StudyReport_Book成功！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeUserVideoInfo");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化Rds_StudyReport_Module 用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool InitializeRdsStudyReportModule(string connectionstring)
        {
            try
            {
                string sqls = string.Format(@"SELECT  BookID ,
                                                    FirstTitleID ,
                                                    FirstTitle ,
                                                    SecondTitleID ,
                                                    SecondTitle ,
                                                    VideoNumber
                                            FROM    FZ_InterestDubbing.dbo.TB_VideoDetails
                                            WHERE   BookID IS NOT NULL
                                                    AND BookID <> 0
                                            GROUP BY BookID ,
                                                    FirstTitleID ,
                                                    FirstTitle ,
                                                    SecondTitleID ,
                                                    SecondTitle ,
                                                    VideoNumber");
                DataSet dsBookInfo = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sqls);
                List<IBSInitUserVideoDAL.BookInfo> BInfo = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.BookInfo>(dsBookInfo, 0);

                int totalcount = 0;
                string sql1 =
                    string.Format(@"SELECT count(*) FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                                                          AND a.VideoFileID = d.VideoFileID", datetime);
                DataSet ds1 = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    totalcount = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString()) + 10000;
                }

                int pageCount = 100000;
                int pageSize = totalcount / pageCount + ((totalcount % pageCount == 0) ? 0 : 1);
                for (int i = 0; i < pageSize; i++)
                {
                    string sql = string.Format(@"SELECT  a.ID ,
                                                    a.UserID ,
                                                    a.VideoFileID ,
                                                    a.CreateTime ,
                                                    a.VersionID ,
                                                    a.BookID ,
                                                    a.VideoNumber ,
                                                    a.VideoImageAddress ,
                                                    a.IsEnableOss ,
                                                    a.DubTimes ,
                                                    a.TotalScore
                                            FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY a.ID DESC, ID DESC ) num ,
                                                                a.ID ,
                                                                d.UserID ,
                                                                d.VideoFileID ,
                                                                d.CreateTime ,
                                                                a.VersionID ,
                                                                d.BookID ,
                                                                d.VideoNumber ,
                                                                a.VideoImageAddress ,
                                                                a.IsEnableOss ,
                                                                d.DubTimes ,
                                                                d.TotalScore
                                                      FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                            AND CreateTime<'{2}'
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
                                                    ) a
                                            WHERE   a.num BETWEEN {0} AND {1}", (pageCount * i) + 1, pageCount * (i + 1), datetime);
                    DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                    List<IBSInitUserVideoDAL.UserVideoInfo> uvi = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.UserVideoInfo>(ds, 0);

                    using (var Redis = RedisManager.GetClient(0))
                    {

                        foreach (var item in uvi)
                        {
                            try
                            {
                                ClassInfoByUserID classinfo = userBLL.GetClassInfoByUserID(item.UserID);
                                if (classinfo != null)
                                {
                                    List<IBSInitUserVideoDAL.BookInfo> bkInfo = BInfo.Where(x => x.BookID == item.BookID && x.VideoNumber == item.VideoNumber).ToList();
                                    if (bkInfo.Count > 0)
                                    {
                                        foreach (var bi in bkInfo)
                                        {
                                            if (bi.SecondTitleID > 0)
                                            {
                                                string MKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType +
                                                              "_" + bi.BookID + "_" + bi.FirstTitleID + bi.SecondTitleID;
                                                string value = Redis.GetValueFromHash("Rds_StudyReport_Module", MKey);

                                                if (string.IsNullOrEmpty(value))
                                                {
                                                    Rds_StudyReport_Module rdsBook = new Rds_StudyReport_Module()
                                                    {
                                                        FirstTitleID = bi.FirstTitleID,
                                                        FirstTitle = bi.FirstTitle,
                                                        SecondTitleID = bi.SecondTitleID,
                                                        SecondTitle = bi.SecondTitle,
                                                        ClassID = classinfo.ClassNum,
                                                        BookID = bi.BookID,
                                                        Flag = 0,
                                                        SubjectID = SubjectID,
                                                        ModuleType = ModelType
                                                    };
                                                    var ve =
                                                        JsonSerializer.SerializeToString<Rds_StudyReport_Module>(rdsBook);
                                                        //该方法字段值为null时会丢失这个字段
                                                    Redis.SetEntryInHash("Rds_StudyReport_Module", MKey, ve);
                                                }
                                            }
                                            else
                                            {
                                                string MKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType +
                                                              "_" + bi.BookID + "_" + bi.FirstTitleID;
                                                string value = Redis.GetValueFromHash("Rds_StudyReport_Module", MKey);

                                                if (string.IsNullOrEmpty(value))
                                                {
                                                    Rds_StudyReport_Module rdsBook = new Rds_StudyReport_Module()
                                                    {
                                                        FirstTitleID = bi.FirstTitleID,
                                                        FirstTitle = bi.FirstTitle,
                                                        SecondTitleID = bi.SecondTitleID,
                                                        SecondTitle = bi.SecondTitle,
                                                        ClassID = classinfo.ClassNum,
                                                        BookID = bi.BookID,
                                                        Flag = 0,
                                                        SubjectID = SubjectID,
                                                        ModuleType = ModelType
                                                    };
                                                    var ve =
                                                        JsonSerializer.SerializeToString<Rds_StudyReport_Module>(rdsBook);
                                                    //该方法字段值为null时会丢失这个字段
                                                    Redis.SetEntryInHash("Rds_StudyReport_Module", MKey, ve);
                                                }
                                            }
                                           
                                         
                                              
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "错误：初始化Rds_StudyReport_Module失败，数据记录为" + item.ToJson());
                            }
                        }
                        Redis.Dispose();
                    }
                    Log4Net.LogHelper.Info("第" + i + "次初始化Rds_StudyReport_Module成功！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeRdsStudyReportModule");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化Rds_StudyReport_ModuleTitle 用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool InitializeRdsStudyReportModuleTitle(string connectionstring)
        {
            return uvDal.InitializeRdsStudyReportModuleTitle(connectionstring);
        }



        public void initStudyReportBookCatalogues(string connectionstring)
        {
            uvDal.initStudyReportBookCatalogues(connectionstring);
        }
        #endregion

        #region 初始化同步之后的学习报告记录
        /// <summary>
        /// 初始化用户配音班级和书本报告记录数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool TodayInitializeUserVideoInfo(string connectionstring)
        {
            try
            {
                int totalcount = 0;
                string sql1 =
                    string.Format(@"SELECT count(*) FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                                                          ", datetime);
                DataSet ds1 = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    totalcount = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString()) + 10000;
                }

                int pageCount = 100000;
                int pageSize = totalcount / pageCount + ((totalcount % pageCount == 0) ? 0 : 1);
                for (int i = 0; i < pageSize; i++)
                {
                    string sql = string.Format(@"SELECT  a.ID ,
                                                    a.UserID ,
                                                    a.VideoFileID ,
                                                    a.CreateTime ,
                                                    a.VersionID ,
                                                    a.BookID ,
                                                    a.VideoNumber ,
                                                    a.VideoImageAddress ,
                                                    a.IsEnableOss ,
                                                    a.DubTimes ,
                                                    a.TotalScore
                                            FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY a.ID ASC ) num ,
                                                                a.ID ,
                                                                d.UserID ,
                                                                d.VideoFileID ,
                                                                d.CreateTime ,
                                                                a.VersionID ,
                                                                d.BookID ,
                                                                d.VideoNumber ,
                                                                a.VideoImageAddress ,
                                                                a.IsEnableOss ,
                                                                d.DubTimes ,
                                                                d.TotalScore
                                                      FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                            AND CreateTime>='{2}'
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
                                                    ) a
                                            WHERE   a.num BETWEEN {0} AND {1}", (pageCount * i) + 1, pageCount * (i + 1), datetime);
                    DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                    List<IBSInitUserVideoDAL.UserVideoInfo> uvi = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.UserVideoInfo>(ds, 0);

                    using (var Redis = RedisManager.GetClient(0))
                    {

                        foreach (var item in uvi)
                        {


                            try
                            {
                                ClassInfoByUserID classinfo = userBLL.GetClassInfoByUserID(item.UserID);
                                if (classinfo != null)
                                {
                                    #region Rds_StudyReport_Book
                                    string BKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType + "_" + item.BookID;
                                    string value = Redis.GetValueFromHash("Rds_StudyReport_Book", BKey);

                                    if (string.IsNullOrEmpty(value))
                                    {
                                        Rds_StudyReport_Book rdsBook = new Rds_StudyReport_Book()
                                        {
                                            BookID = item.BookID,
                                            ClassID = classinfo.ClassNum,
                                            Flag = 0,
                                            SubjectID = SubjectID,
                                            ModuleType = ModelType
                                        };
                                        var ve = JsonSerializer.SerializeToString<Rds_StudyReport_Book>(rdsBook);
                                        //该方法字段值为null时会丢失这个字段
                                        Redis.SetEntryInHash("Rds_StudyReport_Book", BKey, ve);
                                    }
                                    #endregion

                                    #region Rds_StudyReport_Class
                                    string CKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType;
                                    string classValue = Redis.GetValueFromHash("Rds_StudyReport_Class", CKey);

                                    if (string.IsNullOrEmpty(classValue))
                                    {
                                        Rds_StudyReport_Class rdsBook = new Rds_StudyReport_Class()
                                        {
                                            ClassID = classinfo.ClassNum,
                                            Flag = 0,
                                            SubjectID = SubjectID,
                                            ModuleType = ModelType
                                        };
                                        var ve = JsonSerializer.SerializeToString<Rds_StudyReport_Class>(rdsBook);//该方法字段值为null时会丢失这个字段
                                        Redis.SetEntryInHash("Rds_StudyReport_Class", CKey, ve);
                                    }
                                    #endregion
                                }

                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "错误：初始化Rds_StudyReport_Class和Rds_StudyReport_Book失败，数据记录为" + item.ToJson());
                            }
                        }
                        Redis.Dispose();
                    }
                    Log4Net.LogHelper.Info("第" + i + "次初始化Rds_StudyReport_Class和Rds_StudyReport_Book成功！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeUserVideoInfo");
                return false;
            }
            return true;
        }


        public bool TodayInitializeBook(string connectionstring)
        {
            try
            {


                int totalcount = 0;
                string sql1 =
                    string.Format(@"SELECT count(*) FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                                                          ", datetime);
                DataSet ds1 = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    totalcount = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString()) + 10000;
                }

                int pageCount = 100000;
                int pageSize = totalcount / pageCount + ((totalcount % pageCount == 0) ? 0 : 1);
                for (int i = 0; i < pageSize; i++)
                {
                    string sql = string.Format(@"SELECT  a.ID ,
                                                    a.UserID ,
                                                    a.VideoFileID ,
                                                    a.CreateTime ,
                                                    a.VersionID ,
                                                    a.BookID ,
                                                    a.VideoNumber ,
                                                    a.VideoImageAddress ,
                                                    a.IsEnableOss ,
                                                    a.DubTimes ,
                                                    a.TotalScore
                                            FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY a.ID ASC ) num ,
                                                                a.ID ,
                                                                d.UserID ,
                                                                d.VideoFileID ,
                                                                d.CreateTime ,
                                                                a.VersionID ,
                                                                d.BookID ,
                                                                d.VideoNumber ,
                                                                a.VideoImageAddress ,
                                                                a.IsEnableOss ,
                                                                d.DubTimes ,
                                                                d.TotalScore
                                                      FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                            AND CreateTime>='{2}'
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
                                                    ) a
                                            WHERE   a.num BETWEEN {0} AND {1}", (pageCount * i) + 1, pageCount * (i + 1), datetime);
                    DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                    List<IBSInitUserVideoDAL.UserVideoInfo> uvi = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.UserVideoInfo>(ds, 0);

                    using (var Redis = RedisManager.GetClient(0))
                    {

                        foreach (var item in uvi)
                        {


                            try
                            {
                                ClassInfoByUserID classinfo = userBLL.GetClassInfoByUserID(item.UserID);
                                if (classinfo != null)
                                {
                                    #region Rds_StudyReport_Book
                                    string BKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType + "_" + item.BookID;
                                    string value = Redis.GetValueFromHash("Rds_StudyReport_Book", BKey);

                                    if (string.IsNullOrEmpty(value))
                                    {
                                        Rds_StudyReport_Book rdsBook = new Rds_StudyReport_Book()
                                        {
                                            BookID = item.BookID,
                                            ClassID = classinfo.ClassNum,
                                            Flag = 0,
                                            SubjectID = SubjectID,
                                            ModuleType = ModelType
                                        };
                                        var ve = JsonSerializer.SerializeToString<Rds_StudyReport_Book>(rdsBook);
                                        //该方法字段值为null时会丢失这个字段
                                        Redis.SetEntryInHash("Rds_StudyReport_Book", BKey, ve);
                                    }
                                    #endregion
                                }

                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "错误：初始化Rds_StudyReport_Class和Rds_StudyReport_Book失败，数据记录为" + item.ToJson());
                            }
                        }
                        Redis.Dispose();
                    }
                    Log4Net.LogHelper.Info("第" + i + "次初始化Rds_StudyReport_Class和Rds_StudyReport_Book成功！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeUserVideoInfo");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 初始化Rds_StudyReport_Module 用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool TodayInitializeRdsStudyReportModule(string connectionstring)
        {
            try
            {
                string sqls = string.Format(@"SELECT  BookID ,
                                                    FirstTitleID ,
                                                    FirstTitle ,
                                                    SecondTitleID ,
                                                    SecondTitle ,
                                                    VideoNumber
                                            FROM    FZ_InterestDubbing.dbo.TB_VideoDetails
                                            WHERE   BookID IS NOT NULL
                                                    AND BookID <> 0
                                            GROUP BY BookID ,
                                                    FirstTitleID ,
                                                    FirstTitle ,
                                                    SecondTitleID ,
                                                    SecondTitle ,
                                                    VideoNumber");
                DataSet dsBookInfo = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sqls);
                List<IBSInitUserVideoDAL.BookInfo> BInfo = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.BookInfo>(dsBookInfo, 0);

                int totalcount = 0;
                string sql1 =
                    string.Format(@"SELECT count(*) FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                                                          AND a.VideoFileID = d.VideoFileID", datetime);
                DataSet ds1 = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    totalcount = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString()) + 10000;
                }

                int pageCount = 100000;
                int pageSize = totalcount / pageCount + ((totalcount % pageCount == 0) ? 0 : 1);
                for (int i = 0; i < pageSize; i++)
                {
                    string sql = string.Format(@"SELECT  a.ID ,
                                                    a.UserID ,
                                                    a.VideoFileID ,
                                                    a.CreateTime ,
                                                    a.VersionID ,
                                                    a.BookID ,
                                                    a.VideoNumber ,
                                                    a.VideoImageAddress ,
                                                    a.IsEnableOss ,
                                                    a.DubTimes ,
                                                    a.TotalScore
                                            FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY a.ID DESC, ID DESC ) num ,
                                                                a.ID ,
                                                                d.UserID ,
                                                                d.VideoFileID ,
                                                                d.CreateTime ,
                                                                a.VersionID ,
                                                                d.BookID ,
                                                                d.VideoNumber ,
                                                                a.VideoImageAddress ,
                                                                a.IsEnableOss ,
                                                                d.DubTimes ,
                                                                d.TotalScore
                                                      FROM      ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
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
                                                                            AND CreateTime>='{2}'
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
                                                    ) a
                                            WHERE   a.num BETWEEN {0} AND {1}", (pageCount * i) + 1, pageCount * (i + 1), datetime);
                    DataSet ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql);
                    List<IBSInitUserVideoDAL.UserVideoInfo> uvi = JsonHelper.DataSetToIList<IBSInitUserVideoDAL.UserVideoInfo>(ds, 0);

                    using (var Redis = RedisManager.GetClient(0))
                    {

                        foreach (var item in uvi)
                        {
                            try
                            {
                                ClassInfoByUserID classinfo = userBLL.GetClassInfoByUserID(item.UserID);
                                if (classinfo != null)
                                {
                                    List<IBSInitUserVideoDAL.BookInfo> bkInfo = BInfo.Where(x => x.BookID == item.BookID && x.VideoNumber == item.VideoNumber).ToList();
                                    if (bkInfo.Count > 0)
                                    {
                                        foreach (var bi in bkInfo)
                                        {
                                            if (bi.SecondTitleID > 0)
                                            {
                                                string MKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType +
                                                              "_" + bi.BookID + "_" + bi.FirstTitleID + bi.SecondTitleID;
                                                string value = Redis.GetValueFromHash("Rds_StudyReport_Module", MKey);

                                                if (string.IsNullOrEmpty(value))
                                                {
                                                    Rds_StudyReport_Module rdsBook = new Rds_StudyReport_Module()
                                                    {
                                                        FirstTitleID = bi.FirstTitleID,
                                                        FirstTitle = bi.FirstTitle,
                                                        SecondTitleID = bi.SecondTitleID,
                                                        SecondTitle = bi.SecondTitle,
                                                        ClassID = classinfo.ClassNum,
                                                        BookID = bi.BookID,
                                                        Flag = 0,
                                                        SubjectID = SubjectID,
                                                        ModuleType = ModelType
                                                    };
                                                    var ve =
                                                        JsonSerializer.SerializeToString<Rds_StudyReport_Module>(rdsBook);
                                                    //该方法字段值为null时会丢失这个字段
                                                    Redis.SetEntryInHash("Rds_StudyReport_Module", MKey, ve);
                                                }
                                            }
                                            else
                                            {
                                                string MKey = classinfo.ClassNum + "_" + SubjectID + "_" + ModelType +
                                                              "_" + bi.BookID + "_" + bi.FirstTitleID;
                                                string value = Redis.GetValueFromHash("Rds_StudyReport_Module", MKey);

                                                if (string.IsNullOrEmpty(value))
                                                {
                                                    Rds_StudyReport_Module rdsBook = new Rds_StudyReport_Module()
                                                    {
                                                        FirstTitleID = bi.FirstTitleID,
                                                        FirstTitle = bi.FirstTitle,
                                                        SecondTitleID = bi.SecondTitleID,
                                                        SecondTitle = bi.SecondTitle,
                                                        ClassID = classinfo.ClassNum,
                                                        BookID = bi.BookID,
                                                        Flag = 0,
                                                        SubjectID = SubjectID,
                                                        ModuleType = ModelType
                                                    };
                                                    var ve =
                                                        JsonSerializer.SerializeToString<Rds_StudyReport_Module>(rdsBook);
                                                    //该方法字段值为null时会丢失这个字段
                                                    Redis.SetEntryInHash("Rds_StudyReport_Module", MKey, ve);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "错误：初始化Rds_StudyReport_Module失败，数据记录为" + item.ToJson());
                            }
                        }
                        Redis.Dispose();
                    }
                    Log4Net.LogHelper.Info("第" + i + "次初始化Rds_StudyReport_Module成功！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InitializeRdsStudyReportModule");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化Rds_StudyReport_ModuleTitle 用户配音数据
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool TodayInitializeRdsStudyReportModuleTitle(string connectionstring)
        {
            return uvDal.TodayInitializeRdsStudyReportModuleTitle(connectionstring);
        }



        public void TodayinitStudyReportBookCatalogues(string connectionstring)
        {
            uvDal.TodayinitStudyReportBookCatalogues(connectionstring);
        }
        #endregion
    }
}
