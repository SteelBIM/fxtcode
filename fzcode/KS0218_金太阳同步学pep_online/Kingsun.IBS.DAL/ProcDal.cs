using Kingsun.IBS.IDAL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.TBX;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.DAL
{
    public class ProcDal : IProcDal
    {
        public BaseManagement bm = new BaseManagement();
        public List<StudyCurriculum> proc_GetUserStudyCurriculum()
        {
            List<StudyCurriculum> stuCurriculum = new List<StudyCurriculum>();
        
                string sql = @"SELECT  a.ID ID ,
                      b.UserID,
                        a.CourseCategory CourseCategory ,
                        a.SubjectID ,
                        a.TextbookVersion TextbookVersion ,
                        a.EditionID ,
                        JuniorGrade JuniorGrade ,
                        a.GradeID ,
                        a.TeachingBooks TeachingBooks ,
                        a.BreelID ,
                        a.BookID ,
                        CONVERT(VARCHAR(10), CreateTime, 120) CreateTime
                FROM    TB_CurriculumManage a
                        LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] b ON a.BookID = b.BookID
                WHERE   a.BookID <> 0
                        AND b.CreateTime >(Convert(varchar(100),dateadd(day,-1,GETDATE()),23)+' 23:59:00')
                        AND b.CreateTime <=(Convert(varchar(100),GETDATE(),23)+' 23:59:00')";
             DataSet ds=bm.ExecuteSql(sql);
            /*List<StudyCurriculum> stuCurriculum = JsonHelper.DataSetToIList<StudyCurriculum>(ds, 0);*/
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    StudyCurriculum sr = new StudyCurriculum();
                    sr.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                    sr.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString());
                    sr.CreateTime = DateTime.Parse(ds.Tables[0].Rows[i]["CreateTime"].ToString());
                    sr.BookID = ds.Tables[0].Rows[i]["BookID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"].ToString());
                    sr.BreelID = ds.Tables[0].Rows[i]["BreelID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["BreelID"].ToString());
                    sr.EditionID = ds.Tables[0].Rows[i]["EditionID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["EditionID"].ToString());
                    sr.GradeID = ds.Tables[0].Rows[i]["GradeID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["GradeID"].ToString());
                    sr.SubjectID = ds.Tables[0].Rows[i]["SubjectID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["SubjectID"].ToString());
                    sr.CourseCategory = ds.Tables[0].Rows[i]["CourseCategory"].ToString();
                    sr.JuniorGrade = ds.Tables[0].Rows[i]["JuniorGrade"].ToString();
                    sr.TeachingBooks = ds.Tables[0].Rows[i]["TeachingBooks"].ToString();
                    sr.TextbookVersion = ds.Tables[0].Rows[i]["TextbookVersion"].ToString();
                    stuCurriculum.Add(sr);
                }
            }
            return stuCurriculum;
        }


        public string proc_GetUserStudyDetailsLite3(List<UserClass> userClass)
        {
            List<StudyDetailsLite3> stuDetailsLite3 = new List<StudyDetailsLite3>();
            int maxUpdateID = 0;
            int maxTableID = 0;
            string sql = @"SELECT TOP 1
                                    MAX(ID) ID 
                             FROM   [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                             GROUP BY BookID ,
                                    VideoNumber ,
                                    CONVERT(VARCHAR(10), CreateTime, 120)ORDER BY ID DESC";
            DataSet ds = bm.ExecuteSql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                maxUpdateID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"].ToString());
            }
            string sql1 = @" SELECT  MAX(id) ID
                            FROM    TB_UserStudyDeatilsLite3";
            DataSet ds1 = bm.ExecuteSql(sql1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                maxTableID = Convert.ToInt32(ds1.Tables[0].Rows[0]["ID"].ToString());
            }
            if (maxTableID < maxUpdateID)
            {
                    string sql2 = string.Format(@"SELECT d.ID ,
                                d.CreateTime ,
                                a.BookID ,
                                a.VideoNumber ,
                                a.TotalScore ,
                                a.UserID ,
                                d.VideoFileID ,
                                b.VideoTitle ,
                                b.FirstModularID ,
                                b.FirstModular ,
                                b.FirstTitleID ,
                                b.FirstTitle ,
                                b.SecondTitleID ,
                                b.SecondTitle ,
                                c.NickName ,
                                c.TrueName ,
                                c.UserName ,
                                c.UserImage ,
                                c.IsEnableOss ,
                                a.DubTimes ,
                                d.VersionID
                        FROM    ( SELECT    MAX(DISTINCT TotalScore) TotalScore ,
                                            COUNT(UserID) DubTimes ,
                                            BookID ,
                                            VideoNumber ,
                                            UserID
                                  FROM      [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                                  WHERE     BookID IS NOT NULL
                                            AND BookID <> 0
                                            AND VideoNumber <> 0
                                            AND CreateTime > DATEADD(MONTH, -6,
                                                              GETDATE())
                                  GROUP BY  BookID ,
                                            VideoNumber ,
                                            UserID
                                ) a
                                LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDetails] b ON a.BookID = b.BookID
                                                              AND a.VideoNumber = b.VideoNumber
                                LEFT JOIN Tb_UserInfo c ON a.UserID = c.UserID
                                LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] d ON a.BookID = d.BookID
                                                              AND a.TotalScore = d.TotalScore
                                                              AND a.VideoNumber = d.VideoNumber
                                                              AND a.UserID = d.UserID
                        WHERE   d.ID > {0}
                                AND c.IsUser = 1
                                AND d.CreateTime >(Convert(varchar(100),dateadd(day,-1,GETDATE()),23)+' 23:59:00'
                                AND d.CreateTime <=(Convert(varchar(100),GETDATE(),23)+' 23:59:00')
                        ORDER BY d.ID DESC", maxTableID);
                 DataSet ds2 = bm.ExecuteSql(sql2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        StudyDetailsLite3 sr = new StudyDetailsLite3();
                        sr.ID = Convert.ToInt32(ds2.Tables[0].Rows[i]["ID"].ToString());
                        sr.UserID = Convert.ToInt32(ds2.Tables[0].Rows[i]["UserID"].ToString());
                        sr.CreateTime = DateTime.Parse(ds2.Tables[0].Rows[i]["CreateTime"].ToString());
                        sr.BookID = ds2.Tables[0].Rows[i]["BookID"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["BookID"].ToString());
                        sr.FirstModularID = ds2.Tables[0].Rows[i]["FirstModularID"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["FirstModularID"].ToString());
                        sr.FirstTitleID = ds2.Tables[0].Rows[i]["FirstTitleID"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["FirstTitleID"].ToString());
                        sr.SecondTitleID = ds2.Tables[0].Rows[i]["SecondTitleID"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["SecondTitleID"].ToString());
                        sr.VideoNumber = ds2.Tables[0].Rows[i]["VideoNumber"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["VideoNumber"].ToString());
                        sr.SecondTitle = ds2.Tables[0].Rows[i]["SecondTitle"].ToString();
                        sr.FirstModular = ds2.Tables[0].Rows[i]["FirstModular"].ToString();
                        sr.FirstTitle = ds2.Tables[0].Rows[i]["FirstTitle"].ToString();
                        sr.VideoFileID = ds2.Tables[0].Rows[i]["VideoFileID"].ToString();
                        sr.VideoTitle = ds2.Tables[0].Rows[i]["VideoTitle"].ToString();

                        sr.TotalScore = (float)(ds2.Tables[0].Rows[i]["TotalScore"] is DBNull ? 0.0 : float.Parse(ds2.Tables[0].Rows[i]["TotalScore"].ToString()));
                        sr.NickName = ds2.Tables[0].Rows[i]["NickName"].ToString();
                        sr.TrueName = ds2.Tables[0].Rows[i]["TrueName"].ToString();
                        sr.UserName = ds2.Tables[0].Rows[i]["UserName"].ToString();
                        sr.UserImage = ds2.Tables[0].Rows[i]["UserImage"].ToString();
                        sr.IsEnableOss = ds2.Tables[0].Rows[i]["IsEnableOss"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["IsEnableOss"].ToString());
                        sr.DubTimes = ds2.Tables[0].Rows[i]["DubTimes"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["DubTimes"].ToString());
                        sr.VersionID = ds2.Tables[0].Rows[i]["VersionID"] is DBNull ? 0 : Convert.ToInt32(ds2.Tables[0].Rows[i]["VersionID"].ToString());
                        stuDetailsLite3.Add(sr);
                    }
                }

                List<TB_UserStudyDeatilsLite3_Day> query = (from c in stuDetailsLite3
                                                        join o in userClass on c.UserID equals o.UserID into userid
                                                        from o in userid.DefaultIfEmpty(new UserClass { UserID = 0, ClassNum = "", ClassID = "" })
                                                        select new TB_UserStudyDeatilsLite3_Day
                                                        {
                                                            id = c.ID,
                                                            CreateTime = c.CreateTime,
                                                            BookID = c.BookID,
                                                            FirstModularID = c.FirstModularID,
                                                            FirstTitleID = c.FirstTitleID,
                                                            SecondTitleID = c.SecondTitleID,
                                                            VideoNumber = c.VideoNumber,
                                                            SecondTitle = c.SecondTitle,
                                                            FirstModular = c.FirstModular,
                                                            FirstTitle = c.FirstTitle,
                                                            VideoFileID = c.VideoFileID,
                                                            NickName = c.NickName,
                                                            TrueName = c.TrueName,
                                                            UserName = c.UserName,
                                                            UserImage = c.UserImage,
                                                            IsEnableOss = c.IsEnableOss,
                                                            DubTimes = c.DubTimes,
                                                            VersionID = c.VersionID,
                                                        }).Distinct().OrderByDescending(a => a.id).ToList<TB_UserStudyDeatilsLite3_Day>();

                if (query != null && query.Count > 0)
                {
                    string result = CreateTB_UserStudyDeatilsLite3(query);
                    return result;
                }
            }

            return "无需同步";

        }

       /* public List<StudyDirectory> proc_GetUserStudyDirectory()
        {
            List<StudyDirectory> stuDirectory = new List<StudyDirectory>();
            string sql = @"SELECT  a.ID,
                        CONVERT(VARCHAR(10), a.CreateTime, 120) CreateTime ,
                        a.UserID,
                        a.BookID ,
                        a.VideoNumber VideoNumber ,
                        b.FirstModularID ,
                        b.FirstModular  ,
                        b.FirstTitleID ,
                        b.FirstTitle FirstTitle ,
                        b.SecondTitleID ,
                        b.SecondTitle SecondTitle 
                FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                        LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDetails] b ON a.BookID = b.BookID
                                                       AND a.VideoNumber = b.VideoNumber
                WHERE   b.FirstTitleID IS NOT NULL
                        AND b.FirstModularID IS NOT NULL
                        AND a.BookID <> 0
                        AND a.VideoNumber <> 0
                        AND a.CreateTime < CONVERT(VARCHAR(10), GETDATE(), 120)";
            DataSet ds = bm.ExecuteSql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    StudyDirectory sr = new StudyDirectory();
                    sr.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                    sr.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString());
                    sr.CreateTime = DateTime.Parse(ds.Tables[0].Rows[i]["CreateTime"].ToString());
                    sr.BookID = ds.Tables[0].Rows[i]["BookID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"].ToString());
                    sr.FirstModularID = ds.Tables[0].Rows[i]["FirstModularID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["FirstModularID"].ToString());
                    sr.FirstTitleID = ds.Tables[0].Rows[i]["FirstTitleID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["FirstTitleID"].ToString());
                    sr.SecondTitleID = ds.Tables[0].Rows[i]["SecondTitleID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["SecondTitleID"].ToString());
                    sr.VideoNumber = ds.Tables[0].Rows[i]["VideoNumber"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["VideoNumber"].ToString());
                    sr.SecondTitle = ds.Tables[0].Rows[i]["SecondTitle"].ToString();
                    sr.FirstModular = ds.Tables[0].Rows[i]["FirstModular"].ToString();
                    sr.FirstTitle = ds.Tables[0].Rows[i]["FirstTitle"].ToString();
                    stuDirectory.Add(sr);
                }
            }
            return stuDirectory;
        }*/

        public List<StudyReport> proc_GetUserStudyReport()
        {
            List<StudyReport> stuReport = new List<StudyReport>();
            string sql = @"SELECT  ID,CreateTime,UserID,VersionID  FROM    TB_UserVideoDetails";
            DataSet ds = bm.ExecuteSql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    StudyReport sr = new StudyReport();
                    sr.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                    sr.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString());
                    sr.CreateTime = DateTime.Parse(ds.Tables[0].Rows[i]["CreateTime"].ToString());
                    sr.VersionID = ds.Tables[0].Rows[i]["VersionID"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["VersionID"].ToString());
                    stuReport.Add(sr);
                }
            }
            return stuReport;
        }

        public string CreateTB_UserStudyDeatilsLite3(List<TB_UserStudyDeatilsLite3_Day> stuCurriculum)
        {
            //如果存在表则truncate表  没有则创建表
            string sql = @"IF NOT EXISTS ( SELECT  *
                            FROM    sys.objects
                            WHERE   object_id = OBJECT_ID(N'[dbo].[TB_UserStudyDeatilsLite3_Day]') )
                            BEGIN   
                            CREATE TABLE [dbo].[TB_UserStudyDeatilsLite3_Day](
                        	[id] [int] NULL,
                        	[CreateTime] [datetime] NULL,
                        	[BookID] [int] NULL,
                        	[VideoNumber] [int] NULL,
                        	[TotalScore] [float] NULL,
                        	[UserID] [int] NULL,
                        	[VideoFileID] [varchar](150) NULL,
                        	[VideoTitle] [varchar](500) NULL,
                        	[FirstModularID] [int] NULL,
                        	[FirstModular] [varchar](500) NULL,
                        	[FirstTitleID] [int] NULL,
                        	[FirstTitle] [varchar](500) NULL,
                        	[SecondTitleID] [int] NULL,
                        	[SecondTitle] [varchar](500) NULL,
                        	[NickName] [varchar](50) NULL,
                        	[TrueName] [varchar](50) NULL,
                        	[UserName] [varchar](50) NULL,
                        	[UserImage] [nvarchar](150) NULL,
                        	[IsEnableOss] [int] NULL,
                        	[DubTimes] [int] NULL,
                        	[VersionID] [int] NULL
                            );    
                    END;
                    ELSE
                     BEGIN
                        TRUNCATE TABLE TB_UserStudyDeatilsLite3_Day;
                    END";
            int susscesCount = 0;
            int failCount = 0;
            List<int> ids = new List<int>();
            stuCurriculum.ForEach(a =>
            {
                var result = bm.Insert<TB_UserStudyDeatilsLite3_Day>(a);
                if (result)
                {
                    susscesCount += 1;
                }
                else
                {
                    failCount += 1;
                    ids.Add((int)a.id);
                }
            });
            return "CreateTB_UserStudyReport成功" + susscesCount + "个,失败" + failCount + "个,ID=" + string.Join(",", ids);
        }

        public string CreateTB_UserStudyReport(List<TB_UserStudyReport> stuCurriculum)
        {
            bm.ExecuteSql("TRUNCATE TABLE TB_UserStudyReport;");

            int susscesCount = 0;
            int failCount = 0;
            List<int> ids = new List<int>();
            stuCurriculum.ForEach(a =>
            {
                var result = bm.Insert<TB_UserStudyReport>(a);
                if (result)
                {
                    susscesCount += 1;
                }
                else
                {
                    failCount += 1;
                    ids.Add((int)a.id);
                }
            });
            return "CreateTB_UserStudyReport成功" + susscesCount + "个,失败" + failCount + "个,ID=" + string.Join(",", ids);
        }

        public string CreateTB_UserStudyDirectory(List<TB_UserStudyDirectory> stuCurriculum)
        {
            bm.ExecuteSql("TRUNCATE TABLE TB_UserStudyDirectory;");
            int susscesCount = 0;
            int failCount = 0;
            List<int> ids = new List<int>();
            stuCurriculum.ForEach(a =>
            {
                var result = bm.Insert<TB_UserStudyDirectory>(a);
                if (result)
                {
                    susscesCount += 1;
                }
                else
                {
                    failCount += 1;
                    ids.Add((int)a.id);
                }
            });
            return "CreateTB_UserStudyDirectory成功" + susscesCount + "个,失败" + failCount + "个,ID=" + string.Join(",", ids);
        }

        public string CreateTB_UserStudyCurriculum(List<TB_UserStudyCurriculum_Day> stuCurriculum)
        {
            string sql = @"IF NOT EXISTS ( SELECT  *
                        FROM    sys.objects
                        WHERE   object_id = OBJECT_ID(N'[dbo].[TB_UserStudyCurriculum_Day]') )
                        BEGIN   
                            CREATE TABLE [dbo].[TB_UserStudyCurriculum_Day](
                     	[id] [int] NULL,
                     	[StudentStudyCount] [int] NULL,
                     	[CourseCategory] [varchar](100) NULL,
                     	[SubjectID] [int] NULL,
                     	[TextbookVersion] [varchar](100) NULL,
                     	[EditionID] [int] NULL,
                     	[JuniorGrade] [varchar](100) NULL,
                     	[GradeID] [int] NULL,
                     	[TeachingBooks] [varchar](100) NULL,
                     	[BreelID] [int] NULL,
                     	[ClassShortID] [nvarchar](50) NULL,
                     	[BookID] [int] NULL,
                     	[CreateTime] [datetime] NULL
                        );    
                    END;
                    ELSE
                     BEGIN
                        TRUNCATE TABLE TB_UserStudyCurriculum_Day;
                    END";
            int susscesCount = 0;
            int failCount = 0;
            List<int> ids = new List<int>();
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("StudentStudyCount");
            dt.Columns.Add("CourseCategory");
            dt.Columns.Add("SubjectID");
            dt.Columns.Add("TextbookVersion");
            dt.Columns.Add("EditionID");
            dt.Columns.Add("GradeID");
            dt.Columns.Add("JuniorGrade");
            dt.Columns.Add("TeachingBooks");
            dt.Columns.Add("BreelID");
            dt.Columns.Add("ClassShortID");
            dt.Columns.Add("BookID");
            dt.Columns.Add("CreateTime");

            if (stuCurriculum.Count>0)
            {
               stuCurriculum.ForEach(a=>{
                        DataRow dr = dt.NewRow();
                       dr["id"]=a.id;
                       dr["StudentStudyCount"]=a.StudentStudyCount;
                       dr["CourseCategory"]=a.CourseCategory;
                       dr["SubjectID"]=a.SubjectID;
                       dr["TextbookVersion"]=a.TextbookVersion;
                       dr["EditionID"]=a.EditionID;
                       dr["GradeID"]=a.GradeID;
                       dr["JuniorGrade"]=a.JuniorGrade;
                       dr["TeachingBooks"]=a.TeachingBooks;
                       dr["BreelID"]=a.BreelID;
                       dr["ClassShortID"]=a.ClassShortID;
                       dr["BookID"]=a.BookID;
                       dr["CreateTime"]=a.CreateTime;
                       dt.Rows.Add(dr);
                   });     
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count
            };
            List<DbParameter> list = new List<DbParameter>();
            //执行存储过程
            bm.ExecuteProcedure("", list);
            return "";
        }

    }
}
