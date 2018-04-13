    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using Kingsun.SynchronousStudy.Common;
    using Kingsun.SynchronousStudy.Common.Base;
    using Kingsun.SynchronousStudy.Models;
    using Kingsun.IBS.Model;
    using Kingsun.IBS.Model.IBSLearnReport;
    using ServiceStack.Text;

    namespace Kingsun.SynchronousStudy.DAL
    {
        public class NewLearningReportDAL
        {
            static RedisHashHelper hashRedis = new RedisHashHelper();
            readonly BaseManagement _bm = new BaseManagement();
            readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
            readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];
            private int ModelType = 1;//redis数据类型(1:趣配音,2:单元测试，3：说说看)
            private int Subject = 3;//学科（3：英语）

            /// <summary>
            /// 根据版本id获取版本
            /// </summary>
            /// <param name="appId"></param>
            /// <returns></returns>
            public string GetVersionIdByAppId(string appId)
            {
                string VersionID = "";
                string sql = @"SELECT VersionID FROM dbo.TB_APPManagement WHERE ID='" + appId + "'";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        VersionID = ds.Tables[0].Rows[0]["VersionID"].ToString();
                    }
                }
                return VersionID;
            }

            /// <summary>
            /// 根据老师ID查询班级信息
            /// </summary>
            /// <returns></returns>
            public HttpResponseMessage GetClassInfoByTeacherId_bak(string appId, List<UserClass> userClass, List<ClassInfoList> classList)
            {
                string versionId = GetVersionIdByAppId(appId);
                string sql = string.Format(@"  SELECT  a.UserID  
                                                    FROM    dbo.TB_UserStudyDeatilsLite3 a
                                                    WHERE    a.VersionID='{0}'", versionId);

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<ClassStudyNum> userids = new List<ClassStudyNum>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ClassStudyNum num = new ClassStudyNum();
                        num.num = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString());
                        userids.Add(num);
                    }
                }
                var csn = (from user in userids
                           join uc in userClass on user.num equals uc.UserID into x
                           from uc in x.DefaultIfEmpty((new UserClass { UserID = 0, ClassNum = "", ClassID = "" }))
                           where user.num == uc.UserID
                           group new { user.num, uc.UserID } by new { uc.ClassNum } into g
                           select new ClassStudyNum
                           {
                               ClassId = g.Key.ClassNum,
                               num = g.Count()
                           }).ToList<ClassStudyNum>();

                List<ClassInfoList> returnCinfoList = ClassOrder(classList);//排序后的班级新
                ClassInfoList list;
                List<ClassInfoList> classinfoList = new List<ClassInfoList>();
                foreach (var item in returnCinfoList)
                {
                    list = new ClassInfoList();
                    IList<ClassStudyNum> cil = csn.Where(i => i.ClassId.ToString() == item.Id).ToList();
                    list.ClassName = item.ClassName;
                    if (cil.Count == 0)
                    {
                        list.Id = item.Id;
                        list.IsStudy = false;
                    }
                    else
                    {
                        foreach (var ci in cil)
                        {
                            list.Id = ci.ClassId;
                            list.IsStudy = true;
                        }
                    }
                    classinfoList.Add(list);
                }
                object obj = new { ClassList = classinfoList.OrderByDescending(i => i.IsStudy).ThenBy(i => i.ClassName) };//返回信息
                return JsonHelper.GetResult(obj, "操作成功");

            }

            /// <summary>
            /// 根据老师ID查询班级信息
            /// </summary>
            /// <returns></returns>
            public HttpResponseMessage GetClassInfoByTeacherId(List<ClassInfoList> classList)
            {
                List<ClassInfoList> returnCinfoList = ClassOrder(classList);//排序后的班级新

                object obj = new { ClassList = returnCinfoList.OrderByDescending(i => i.ClassName) };//返回信息
                return JsonHelper.GetResult(obj, "操作成功");

            }

            /// <summary>
            /// 班级排序
            /// </summary>
            /// <param name="list"></param>
            private List<ClassInfoList> ClassOrder(List<ClassInfoList> list)
            {
                List<ClassInfoList> classList = list;
                List<ClassInfoList> returnList = new List<ClassInfoList>();
                if (classList != null && classList.Count > 0)
                {
                    string[] gradeArr = { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" };
                    for (int i = 0, length = gradeArr.Length; i < length; i++)
                    {
                        returnList.AddRange(classList.Where(t => t.ClassName.IndexOf(gradeArr[i], StringComparison.Ordinal) > -1));
                    }
                }
                return returnList;
            }

            /// <summary>
            /// 根据班级Id查询年级学习人数
            /// </summary>
            /// <param name="appId">版本ID</param>
            /// <param name="classId">班级ID</param>
            /// <returns></returns>
            public HttpResponseMessage GetJuniorGradeNumByClassId_bak(string appId, string classId)
            {
                string sql = string.Format(@" SELECT BookID ,
                                                    JuniorGrade ,
                                                    TeachingBooks ,
                                                    MAX(StudentStudyCount) StudentStudyCount
                                             FROM   ( SELECT    BookID ,
                                                                JuniorGrade ,
                                                                TeachingBooks ,
                                                                StudentStudyCount
                                                      FROM      TB_UserStudyCurriculum a
                                                                LEFT JOIN dbo.TB_APPManagement b ON a.EditionID = b.VersionID
                                                      WHERE     a.StudentStudyCount > 0
                                                                AND a.ClassShortID = '{0}'
                                                                AND b.ID = '{1}'
                                                      UNION
                                                      SELECT    BookID ,
                                                                JuniorGrade ,
                                                                TeachingBooks ,
                                                                '0' AS StudentStudyCount
                                                      FROM      dbo.TB_CurriculumManage a
                                                                LEFT JOIN dbo.TB_APPManagement b ON a.EditionID = b.VersionID
                                                      WHERE     [State] = 1
                                                                AND b.ID = '{1}'
                                                    ) a
                                             GROUP BY a.BookID ,
                                                    a.JuniorGrade ,
                                                    a.TeachingBooks", classId, appId);

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<JuniorGradeInfoList> listobj = new List<JuniorGradeInfoList>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JuniorGradeInfoList obj;
                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["StudentStudyCount"]) > 0)
                    {
                        int bookId = Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"]);
                        string bookName = ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"].ToString();
                        obj = new JuniorGradeInfoList { BookId = bookId, BookName = bookName, IsStudy = true };
                    }
                    else
                    {
                        int bookId = Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"]);
                        string bookName = ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"].ToString();
                        obj = new JuniorGradeInfoList { BookId = bookId, BookName = bookName, IsStudy = false };
                    }
                    listobj.Add(obj);
                }
                if (listobj.Count > 0)
                {
                    return JsonHelper.GetResult(CurriculumOrder(listobj), "操作成功");
                }
                else
                {
                    return JsonHelper.GetErrorResult("数据不存在！");
                }
            }

            /// <summary>
            /// 根据班级Id查询年级学习人数
            /// </summary>
            /// <param name="appId">版本ID</param>
            /// <param name="classId">班级ID</param>
            /// <returns></returns>
            public HttpResponseMessage GetJuniorGradeNumByClassId(string appId, string classId)
            {
                string sql = string.Format(@" SELECT  a.BookID ,
                                                        a.JuniorGrade ,
                                                        a.TeachingBooks
                                                FROM    dbo.TB_CurriculumManage a
                                                        LEFT JOIN TB_APPManagement b ON a.EditionID = b.VersionID
                                                WHERE   b.ID = '{0}'
                                                GROUP BY a.BookID ,
                                                        a.JuniorGrade ,
                                                        a.TeachingBooks", appId);

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<JuniorGradeInfoList> listobj = new List<JuniorGradeInfoList>();

                using (var Redis = RedisManager.GetClient(0))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        JuniorGradeInfoList obj = new JuniorGradeInfoList();
                        try
                        {
                            string value = Redis.GetValueFromHash("Rds_StudyReport_Book", classId + "_" + Subject + "_" + ModelType + "_" + ds.Tables[0].Rows[i]["BookID"]);
                            if (!string.IsNullOrEmpty(value))
                            {
                                int bookId = Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"]);
                                string bookName = ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"].ToString();
                                obj = new JuniorGradeInfoList { BookId = bookId, BookName = bookName, IsStudy = true };
                            }
                            else
                            {
                                int bookId = Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"]);
                                string bookName = ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"].ToString();
                                obj = new JuniorGradeInfoList { BookId = bookId, BookName = bookName, IsStudy = false };
                            }
                            Rds_StudyReport_Book study = JsonHelper.DecodeJson<Rds_StudyReport_Book>(value);
                            if (study != null)
                            {
                                study.Flag = 1;
                                try
                                {
                                    string ve = JsonSerializer.SerializeToString<Rds_StudyReport_Book>(study);
                                    Redis.SetEntryInHash("Rds_StudyReport_Book", classId + "_" + Subject + "_" + ModelType + "_" + ds.Tables[0].Rows[i]["BookID"], ve);
                                }
                                catch (Exception ex)
                                {
                                    Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_Book|pairs为：" + classId + "_" + Subject + "_" + ModelType + "_" + ds.Tables[0].Rows[i]["BookID"]);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_Class|Key为：" + classId + "_" + Subject + "_" + ModelType + "_" + ds.Tables[0].Rows[i]["BookID"]);
                        }

                        listobj.Add(obj);
                    }
                }

                if (listobj.Count > 0)
                {
                    return JsonHelper.GetResult(CurriculumOrder(listobj), "操作成功");
                }
                else
                {
                    return JsonHelper.GetErrorResult("数据不存在！");
                }
            }


            /// <summary>
            /// 年级排序
            /// </summary>
            /// <param name="list"></param>
            private List<JuniorGradeInfoList> CurriculumOrder(List<JuniorGradeInfoList> list)
            {
                List<JuniorGradeInfoList> returnList = new List<JuniorGradeInfoList>();
                if (list != null && list.Count > 0)
                {
                    string[] gradeArr = { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" };
                    for (int i = 0, length = gradeArr.Length; i < length; i++)
                    {
                        returnList.AddRange(list.Where(t => t.BookName.IndexOf(gradeArr[i], StringComparison.Ordinal) > -1));
                    }
                }
                return returnList;
            }

            /// <summary>
            /// 根据书籍Id和班级Id查询单元学习人数
            /// </summary>
            /// <param name="bookId">书籍ID</param>
            /// <param name="classId">班级ID</param>
            /// <param name="pageNumber">分页页码</param>
            /// <param name="appId"></param>
            /// <returns></returns>
            public HttpResponseMessage GetUnitLearningByBookId_bak(string bookId, string classId, int pageNumber, List<UserClass> userClass)
            {
                string sql = string.Format(@"SELECT  MAX(BookID) BookID,
                                                    FirstTitleID ,
                                                    MAX(FirstTitle) FirstTitle,
                                                    SecondTitleID ,
                                                    MAX(SecondTitle) SecondTitle
                                            FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails]
                                            WHERE   BookID IS NOT NULL
                                                    AND BookID='{0}'
                                            GROUP BY FirstTitleID ,
                                                    SecondTitleID", bookId);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<TB_VideoDetails> videoDetail = JsonHelper.DataSetToIList<TB_VideoDetails>(ds, 0);

                string strSql = string.Format(@"SELECT  
                                                        a.BookID ,
                                                        a.VideoNumber ,
                                                        a.BookName ,
                                                        a.FirstTitleID ,
                                                        a.FirstTitle ,
                                                        a.SecondTitleID ,
                                                        a.SecondTitle ,
                                                        a.VideoTitle
                                                FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails] a
                                                WHERE   a.BookID IS NOT NULL
                                                        AND a.BookID = '{0}'", bookId);
                DataSet dsList = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);
                List<UnitLearningList> ulList = JsonHelper.DataSetToIList<UnitLearningList>(dsList, 0);


                string strSql1 = string.Format(@"SELECT  a.UserID ,
                                                                            BookID ,
                                                                            VideoNumber
                                                                    FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                                                    WHERE   BookID IS NOT NULL
                                                                            AND BookID <> 0
                                                                            AND VideoNumber <> 0
                                                                            AND CreateTime < CONVERT(VARCHAR(10), GETDATE(), 120)
                                                                            AND a.BookID = '{0}'", bookId);
                DataSet dsList1 = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql1);
                List<UnitLearningList> ulList1 = JsonHelper.DataSetToIList<UnitLearningList>(dsList1, 0);

                var relist = (from a in ulList1
                              join b in userClass on a.UserID equals b.UserID into userid
                              from b in userid.DefaultIfEmpty(new UserClass { UserID = 0, ClassNum = "", ClassID = "" })
                              where a.UserID == b.UserID
                              group new { a.UserID } by new { a.VideoNumber, a.BookID, a.UserID } into g
                              select new UnitLearningList
                              {
                                  BookID = g.Key.BookID,
                                  VideoNumber = g.Key.VideoNumber,
                                  StudentStudyCount = (from a in ulList1
                                                       where a.BookID == g.Key.BookID
                                                       where a.VideoNumber == g.Key.VideoNumber
                                                       where a.UserID == g.Key.UserID
                                                       select a.UserID).Distinct().Count()
                              }).ToList<UnitLearningList>();


                ulList = (from a in ulList
                          join b in relist on new { a.BookID, a.VideoNumber } equals new { b.BookID, b.VideoNumber } into x
                          from b in x.DefaultIfEmpty()
                          select new UnitLearningList
                          {
                              FirstTitleID = a.FirstTitleID,
                              SecondTitleID = a.SecondTitleID,
                              StudentStudyCount = b == null ? 0 : b.StudentStudyCount,
                              BookID = a.BookID
                          }).Distinct().ToList<UnitLearningList>();

                List<UnitLearningInfoList> unit = new List<UnitLearningInfoList>();
                UnitLearningInfoList ull;
                foreach (var item in videoDetail)
                {
                    ull = new UnitLearningInfoList();
                    List<UnitLearningList> unitLearning = ulList.Where(i => i.FirstTitleID == item.FirstTitleID && i.SecondTitleID == item.SecondTitleID && i.BookID == item.BookID && i.StudentStudyCount > 0).ToList();
                    string modName = ""; string uniName = ""; string muName = "";
                    modName = item.FirstTitle.Trim().Length >= 9 ? item.FirstTitle.Substring(0, 9).Trim() + "..." : item.FirstTitle.Trim();
                    uniName = item.SecondTitle.Trim().Length >= 9 ? item.SecondTitle.Substring(0, 9).Trim() + "..." : item.SecondTitle.Trim();
                    if (modName != "" && uniName != "") { muName = modName + "/" + uniName; }
                    if (modName != "" && uniName == "") { muName = modName; }

                    if (unitLearning.Count != 0)
                    {
                        if (!unitLearning.Any(list => list.StudentStudyCount > 0)) continue;
                        ull.FirstTitleID = item.FirstTitleID ?? 0;
                        ull.SecondTitleID = item.SecondTitleID ?? 0;
                        ull.Catalague = muName;
                        ull.IsStudy = true;
                        unit.Add(ull);
                    }
                    else
                    {
                        ull.Catalague = muName;
                        ull.FirstTitleID = item.FirstTitleID ?? 0;
                        ull.SecondTitleID = item.SecondTitleID ?? 0;
                        ull.IsStudy = false;
                        unit.Add(ull);
                    }
                }

                unit = unit.Skip(pageNumber * 10).Take(10).ToList();

                return JsonHelper.GetResult(unit, "操作成功");
            }

            /// <summary>
            /// 根据书籍Id和班级Id查询单元学习人数
            /// </summary>
            /// <param name="bookId">书籍ID</param>
            /// <param name="classId">班级ID</param>
            /// <param name="pageNumber">分页页码</param>
            /// <param name="appId"></param>
            /// <returns></returns>
            public HttpResponseMessage GetUnitLearningByBookId(string bookId, string classId, int pageNumber, List<UserClass> userClass)
            {
                string sql = string.Format(@"SELECT  BookID ,
                                            FirstTitleID ,
                                            FirstTitle ,
                                            SecondTitleID ,
                                            SecondTitle
                                    FROM    FZ_InterestDubbing.dbo.TB_VideoDetails
                                    WHERE   BookID IS NOT NULL
                                            AND BookID <> 0
                                            AND BookID = '{0}'
                                    GROUP BY BookID ,
                                            FirstTitleID ,
                                            FirstTitle ,
                                            SecondTitleID ,
                                            SecondTitle", bookId);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds == null)
                {
                    return JsonHelper.GetErrorResult("无目录数据！");
                }
                List<VideoDetail> vdl = JsonHelper.DataSetToIList<VideoDetail>(ds, 0);

                List<UnitLearningInfoList> unit = new List<UnitLearningInfoList>();
                UnitLearningInfoList ull;

                using (var Redis = RedisManager.GetClient(0))
                {
                    foreach (var item in vdl)
                    {
                        ull = new UnitLearningInfoList();
                        string modName = "";
                        string uniName = "";
                        string muName = "";
                        modName = item.FirstTitle.Trim().Length >= 9
                            ? item.FirstTitle.Substring(0, 9).Trim() + "..."
                            : item.FirstTitle.Trim();
                        if (item.SecondTitle != null)
                        {
                            uniName = item.SecondTitle.Trim().Length >= 9
                            ? item.SecondTitle.Substring(0, 9).Trim() + "..."
                            : item.SecondTitle.Trim();
                        }
                        if (modName != "" && uniName != "")
                        {
                            muName = modName + "/" + uniName;
                        }
                        if (modName != "" && uniName == "")
                        {
                            muName = modName;
                        }
                        string Mkey = "";
                        if (item.SecondTitleID > 0)
                        {
                            Mkey = classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + item.FirstTitleID + item.SecondTitleID;
                        }
                        else
                        {
                            Mkey = classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + item.FirstTitleID;
                        }
                        string value = Redis.GetValueFromHash("Rds_StudyReport_Module", Mkey);
                        if (!string.IsNullOrEmpty(value))
                        {
                            ull.FirstTitleID = item.FirstTitleID;
                            ull.SecondTitleID = item.SecondTitleID;
                            ull.Catalague = muName;
                            ull.IsStudy = true;
                            unit.Add(ull);
                        }
                        else
                        {
                            ull.Catalague = muName;
                            ull.FirstTitleID = item.FirstTitleID;
                            ull.SecondTitleID = item.SecondTitleID;
                            ull.IsStudy = false;
                            unit.Add(ull);
                        }
                    }
                }
                if (unit.Count <= 0)
                {
                    return JsonHelper.GetErrorResult("暂无数据！" + vdl.Count);
                }
                else
                {
                    unit = unit.OrderBy(i => i.FirstTitleID).ThenBy(i => i.SecondTitleID).Skip(pageNumber * 10).Take(10).ToList();
                    return JsonHelper.GetResult(unit, "操作成功");
                }
            }

            /// <summary>
            /// 根据班级Id、册别、目录统计趣配音学习情况
            /// </summary>
            /// <param name="bookId">书籍id</param>
            /// <param name="classId">班级ID</param>
            /// <param name="firstTitleId">单元ID</param>
            /// <param name="secondTitleId">模块ID</param>
            /// <param name="appId"></param>
            /// <returns></returns>
            public HttpResponseMessage GetVideoDetailsByModuleId_bak(string bookId, List<UserClass> userClass, string firstTitleId, string secondTitleId, int count)
            {
                string where = "";
                string userids = "";
                userClass.ForEach(a =>
                {

                    userids += a.UserID + ",";
                });
                userids = userids.Substring(0, userids.Length - 1);
                if (secondTitleId != "0" || string.IsNullOrEmpty(secondTitleId))
                {
                    where = string.Format(" AND a.FirstTitleID = '{0}'  AND a.SecondTitleID = '{1}' ", firstTitleId, secondTitleId);
                }
                else
                {
                    where = string.Format(" AND a.FirstTitleID = '{0}' ", firstTitleId);
                }
                string sql = string.Format(@"SELECT  DISTINCT
                                                    b.StudentStudyCount ,
                                                    a.BookID ,
                                                    a.VideoNumber ,
                                                    a.BookName ,
                                                    a.FirstTitleID ,
                                                    a.FirstTitle ,
                                                    a.SecondTitleID ,
                                                    a.SecondTitle ,
                                                    a.VideoTitle ,
                                                    a.FirstModularId as ModuleId ,
                                                    a.firstModular as ModuleName
                                            FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails] a
                                                    LEFT JOIN ( SELECT  COUNT(DISTINCT a.UserID) StudentStudyCount ,
                                                                        BookID ,
                                                                        VideoNumber
                                                                FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                                                WHERE   BookID IS NOT NULL
                                                                        AND BookID <> 0
                                                                        AND VideoNumber <> 0
                                                                        AND CreateTime < CONVERT(VARCHAR(10), GETDATE(), 120)
                                                                        AND a.BookID = '{0}'
                                                                        AND a.UserID in({1})
                                                                GROUP BY BookID ,
                                                                        VideoNumber
                                                              ) b ON a.BookID = b.BookID
                                                                     AND a.VideoNumber = b.VideoNumber
                                            WHERE   a.BookID IS NOT NULL
                                                    AND a.BookID = '{0}'  {2}", bookId, userids, where);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<VideoDetailsList> vdl = JsonHelper.DataSetToIList<VideoDetailsList>(ds, 0);


                int classStudentCount = count;
                List<VideoDetailsList> videoList = vdl.Select(item => new VideoDetailsList
                {
                    ClassStudentCount = classStudentCount,
                    StudentStudyCount = item.StudentStudyCount,
                    ModuleId = item.ModuleId,
                    ModuleName = item.ModuleName,
                    BookID = item.BookID,
                    VideoNumber = item.VideoNumber,
                    VideoTitle = item.VideoTitle
                }).ToList();

                return JsonHelper.GetResult(videoList.OrderBy(i => i.VideoNumber), "操作成功");
            }


            /// <summary>
            /// 根据班级Id、册别、目录统计趣配音学习情况
            /// </summary>
            /// <param name="bookId">书籍id</param>
            /// <param name="classId">班级ID</param>
            /// <param name="firstTitleId">单元ID</param>
            /// <param name="secondTitleId">模块ID</param>
            /// <param name="appId"></param>
            /// <returns></returns>
            public HttpResponseMessage GetVideoDetailsByModuleId(string bookId, string classId, string firstTitleId, string secondTitleId, IBS_ClassUserRelation userClassList)
            {
                string where = "";
                if (secondTitleId != "0" || string.IsNullOrEmpty(secondTitleId))
                {
                    where = string.Format(" AND FirstTitleID = '{0}'  AND SecondTitleID = '{1}' ", firstTitleId, secondTitleId);
                }
                else
                {
                    where = string.Format(" AND FirstTitleID = '{0}' ", firstTitleId);
                }

                string sql = string.Format(@"SELECT  BookID ,
                                                    FirstTitleID ,
                                                    FirstTitle ,
                                                    SecondTitleID ,
                                                    SecondTitle ,
                                                    FirstModularID ,
                                                    FirstModular ,
                                                    VideoNumber ,
                                                    VideoTitle
                                            FROM    [FZ_InterestDubbing].dbo.TB_VideoDetails
                                            WHERE   BookID = '{0}'  {1}", bookId, where);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<VideoDetail> vdl = JsonHelper.DataSetToIList<VideoDetail>(ds, 0);


                List<VideoDetailsList> videoList = new List<VideoDetailsList>();

                using (var Redis = RedisManager.GetClient(0))
                {
                    foreach (var item in vdl)
                    {
                        int cl = 0;
                        try
                        {
                            foreach (var stu in userClassList.ClassStuList)
                            {
                                string value = Redis.GetValueFromHash("Rds_StudyReport_ModuleTitle_" + stu.StuID.ToString().Substring(0, 2), stu.StuID + "_" + Subject + "_" + ModelType);
                                if (!string.IsNullOrEmpty(value))
                                {
                                    Rds_StudyReport_ModuleTitle study = JsonHelper.DecodeJson<Rds_StudyReport_ModuleTitle>(value);
                                    foreach (var userinfo in study.detail)
                                    {
                                        if (userinfo.BookID == bookId.ToInt() && userinfo.VideoNumber == item.VideoNumber)
                                        {
                                            cl++;
                                        }
                                    }
                                }
                            }

                            VideoDetailsList vlist = new VideoDetailsList()
                            {
                                ClassStudentCount = userClassList.ClassStuList.Count,
                                StudentStudyCount = cl,
                                ModuleId = Convert.ToInt32(item.FirstModularID),
                                ModuleName = item.FirstModular,
                                BookID = bookId.ToInt(),
                                VideoNumber = Convert.ToInt32(item.VideoNumber),
                                VideoTitle = item.VideoTitle
                            };
                            videoList.Add(vlist);

                        }
                        catch (Exception ex)
                        {
                            Log4Net.LogHelper.Error(ex, "错误：HashID为：Rds_StudyReport_ModuleTitle|Key为：" + classId + "_" + Subject + "_" + ModelType + "_" + bookId + "_" + item.VideoNumber);
                        }
                    }
                }

                return JsonHelper.GetResult(videoList.OrderBy(i => i.VideoNumber), "操作成功");
            }

            /// <summary>
            /// 根据书籍ID,班级ID获取班级详细学习情况
            /// </summary>
            /// <param name="classId">班级ID</param>
            /// <param name="bookId">书籍ID</param>
            /// <param name="videoNumber">视频序号</param>
            /// <param name="pageNumber">分页代码</param>
            /// <returns></returns>
            public HttpResponseMessage GetClassStudyDetailsByClassId_bak(string classId, string bookId, int videoNumber, int pageNumber, List<UserClass> userClass)
            {
                List<UInfo> stulist = GetStuListByClassShortId(classId, userClass);//匹配学生
                if (stulist == null || stulist.Count == 0) { return JsonHelper.GetErrorResult("该班级没有学生"); }

                string where = " BookID=" + bookId + " and  VideoNumber=" + videoNumber;

                IList<TB_UserStudyDeatilsLite3> tempuslist = _bm.Search<TB_UserStudyDeatilsLite3>(where);
                //if (tempuslist == null || tempuslist.Count == 0) { return JsonHelper.GetErrorResult("该模块单元下没有配音的学生"); }
                string sql = string.Format(@"SELECT TOP 1 VideoImageAddress,IsEnableOss FROM  [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] WHERE {0} ORDER BY CreateTime DESC", where);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<UserVideosInfo> uservideoinfo = new List<UserVideosInfo>();
                UserVideosInfo uvi;
                double count = 0;
                double maxScore = 0;
                int sort = 1;
                double minScore = 100;
                string ImgUrl = "";
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ImgUrl = ds.Tables[0].Rows[0]["IsEnableOss"].ToString() != "0"
                            ? _getOssFilesUrl + ds.Tables[0].Rows[0]["VideoImageAddress"]
                            : _getFilesUrl + "?FileID=" + ds.Tables[0].Rows[0]["VideoImageAddress"];
                    }
                }
                int cl = 0;

                foreach (var item in stulist)
                {
                    uvi = new UserVideosInfo();
                    if (tempuslist != null)
                    {
                        IList<TB_UserStudyDeatilsLite3> uslist = tempuslist.OrderBy(i => i.CreateTime).Where(i => i.UserID.ToString() == item.UserID).Take(1).ToList();

                        if (uslist.Count == 0)
                        {
                            uvi.UserId = Convert.ToInt32(item.UserID);
                            uvi.UserImg = item.UserImg;
                            uvi.UserName = item.UserName;
                            //minScore = 0;
                            uvi.Score = "0.0";
                            uvi.IsStudy = false;
                        }
                        else
                        {
                            cl++;
                            foreach (var usItem in uslist)
                            {
                                if (Convert.ToDouble(usItem.TotalScore) > maxScore)
                                {
                                    maxScore = Convert.ToDouble(usItem.TotalScore.ToString("0.0"));
                                }
                                if (Convert.ToDouble(usItem.TotalScore) <= minScore)
                                {
                                    minScore = Convert.ToDouble(usItem.TotalScore.ToString("0.0"));
                                }
                                count += Convert.ToDouble(usItem.TotalScore.ToString("0.0"));

                                if (!string.IsNullOrEmpty(usItem.TrueName))
                                {
                                    uvi.UserName = usItem.TrueName;
                                }
                                else
                                {
                                    uvi.UserName = "暂未填写";
                                }

                                string imgUrl = usItem.IsEnableOss != 0 ? _getOssFilesUrl + usItem.UserImage : _getFilesUrl + "?FileID=" + usItem.UserImage;
                                uvi.UserId = usItem.UserID;//返回UserId
                                if (usItem.UserImage != null) uvi.UserImg = imgUrl;

                                // uvi.Sort = sort++;
                                uvi.DubTimes = usItem.DubTimes;
                                uvi.VedioId = usItem.id;
                                uvi.VedioName = usItem.VideoTitle;
                                uvi.IsStudy = true;
                                if (usItem.CreateTime != null) uvi.CreateTime = usItem.CreateTime.Value.ToString("yyyy.MM.dd");
                                uvi.Score = usItem.TotalScore.ToString("0.0");
                            }
                        }
                    }
                    else
                    {
                        uvi.UserId = Convert.ToInt32(item.UserID);
                        uvi.UserImg = item.UserImg;
                        uvi.UserName = item.UserName;
                        uvi.Score = "0.0";
                        uvi.IsStudy = false;
                    }

                    uservideoinfo.Add(uvi);
                }
                if (cl == 0)
                {
                    minScore = 0;
                }
                //以时间为单位，降序排列
                uservideoinfo = uservideoinfo.OrderByDescending(i => i.IsStudy).ThenByDescending(i => Convert.ToDouble(i.Score)).Skip(pageNumber * 10).Take(10).ToList();
                object obj =
                    new
                    {
                        AverageScore = count <= 0 ? "0" : (count / cl).ToString("0.0"),
                        HighestScore = maxScore,
                        LowestScore = minScore,
                        ImgUrl = ImgUrl,
                        Students = uservideoinfo
                    };
                return JsonHelper.GetResult(obj, "操作成功");//返回信息 
            }


            /// <summary>
            /// 根据书籍ID,班级ID获取班级详细学习情况
            /// </summary>
            /// <param name="classId">班级ID</param>
            /// <param name="bookId">书籍ID</param>
            /// <param name="videoNumber">视频序号</param>
            /// <param name="pageNumber">分页代码</param>
            /// <returns></returns>
            public HttpResponseMessage GetClassStudyDetailsByClassId(string classId, string bookId, int videoNumber, int pageNumber, IBS_ClassUserRelation userClass)
            {
                double count = 0;
                double maxScore = 0;
                int sort = 1;
                double minScore = 100;
                int cl = 0;
                string ImgUrl = "";
                UserVideosInfo uvi;
                List<UserVideosInfo> uservideoinfo = new List<UserVideosInfo>();
                if (userClass != null)
                {
                    foreach (var item in userClass.ClassStuList)
                    {
                        Rds_StudyReport_ModuleTitle module = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + item.StuID.ToString().Substring(0, 2), item.StuID + "_" + Subject + "_" + ModelType);
                        uvi = new UserVideosInfo();
                        if (module != null)
                        {
                            Rds_StudyReport_BookDetail rdsBookDetail = module.detail.FirstOrDefault(i => i.BookID == bookId.ToInt() && i.VideoNumber == videoNumber);
                            if (rdsBookDetail != null)
                            {
                                Rds_StudyReport_BookCatalogues_BookID BookCatalogues = hashRedis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + bookId, rdsBookDetail.FirstTitleID + "_" + rdsBookDetail.SecondTitleID + "_" + ModelType);
                                if (BookCatalogues != null)
                                {
                                    foreach (var vi in BookCatalogues.Videos)
                                    {
                                        if (vi.VideoNumber == videoNumber)
                                        {
                                            ImgUrl = vi.IsEnableOss != 0
                                                ? _getOssFilesUrl + vi.VideoImageAddress
                                                : _getFilesUrl + "?FileID=" + vi.VideoImageAddress;
                                        }
                                    }
                                }
                                rdsBookDetail.BestScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                                cl++;
                                if (Convert.ToDouble(rdsBookDetail.BestScore) > maxScore)
                                {
                                    maxScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                                }
                                if (Convert.ToDouble(rdsBookDetail.BestScore) <= minScore)
                                {
                                    minScore = rdsBookDetail.BestScore.CutDoubleWithN(1);
                                }
                                count += Convert.ToDouble(rdsBookDetail.BestScore.ToString("0.0"));

                                if (!string.IsNullOrEmpty(item.StuName))
                                {
                                    uvi.UserName = item.StuName;
                                }
                                else
                                {
                                    uvi.UserName = "暂未填写";
                                }

                                string imgUrl = item.IsEnableOss != 0 ? _getOssFilesUrl + item.UserImage : _getFilesUrl + "?FileID=" + item.UserImage;
                                uvi.UserId = (int)item.StuID;//返回UserId
                                if (item.UserImage != null) uvi.UserImg = imgUrl;

                                uvi.DubTimes = rdsBookDetail.DubbingNum;
                                uvi.VedioId = Convert.ToInt32(rdsBookDetail.VideoID);
                                uvi.IsStudy = true;
                                uvi.CreateTime = rdsBookDetail.CreateTime;
                                uvi.Score = rdsBookDetail.BestScore.ToString("0.0");
                            }
                            else
                            {
                                uvi.UserId = Convert.ToInt32(item.StuID);
                                uvi.UserImg = item.IsEnableOss != 0
                                                ? _getOssFilesUrl + item.UserImage
                                                : _getFilesUrl + "?FileID=" + item.UserImage;
                                uvi.UserName = item.StuName;
                                uvi.Score = "0.0";
                                uvi.IsStudy = false;
                            }
                        }
                        else
                        {
                            uvi.UserId = Convert.ToInt32(item.StuID);
                            uvi.UserImg = item.IsEnableOss != 0
                                              ? _getOssFilesUrl + item.UserImage
                                              : _getFilesUrl + "?FileID=" + item.UserImage;
                            uvi.UserName = item.StuName;
                            uvi.Score = "0.0";
                            uvi.IsStudy = false;
                        }
                        uservideoinfo.Add(uvi);
                    }
                }
                if (cl == 0)
                {
                    minScore = 0;
                }
                //以时间为单位，降序排列
                uservideoinfo = uservideoinfo.OrderByDescending(i => i.IsStudy).ThenByDescending(i => Convert.ToDouble(i.Score)).Skip(pageNumber * 10).Take(10).ToList();
                object obj =
                    new
                    {
                        AverageScore = count <= 0 ? "0" : (count / cl).ToString("0.0"),
                        HighestScore = maxScore,
                        LowestScore = minScore,
                        ImgUrl = ImgUrl,
                        Students = uservideoinfo
                    };
                return JsonHelper.GetResult(obj, "操作成功");//返回信息 

            }

            /// <summary>
            /// 通过班级ID查询班级下的所有学生
            /// </summary>
            /// <param name="classId">班级ID</param>
            /// <returns></returns>
            private List<UInfo> GetStuListByClassShortId(string classId, List<UserClass> userClass)
            {
                List<UInfo> stuList = new List<UInfo>();
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
                        UInfo ui = new UInfo
                        {
                            UserID = ds.Tables[0].Rows[i]["UserID"].ToString(),
                            UserImg = ds.Tables[0].Rows[i]["IsEnableOss"].ToString() != "0" ? _getOssFilesUrl + ds.Tables[0].Rows[i]["UserImage"] : _getFilesUrl + "?FileID=" + ds.Tables[0].Rows[i]["UserImage"]
                        };
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TrueName"].ToString()))
                        {
                            ui.UserName = ds.Tables[0].Rows[i]["TrueName"].ToString();
                        }
                        else
                        {
                            ui.UserName = "暂未填写";
                        }
                        ui.IsEnableOss = Convert.ToInt32(ds.Tables[0].Rows[i]["IsEnableOss"].ToString());
                        stuList.Add(ui);
                    }
                }
                if (userClass.Count > 0)
                {
                    stuList = (from u in userClass
                               join b in stuList on u.UserID.ToString() equals b.UserID into userid
                               from b in userid.DefaultIfEmpty()
                               where u.UserID == Convert.ToInt32(b.UserID)
                               select new UInfo
                               {
                                   UserID = u.UserID.ToString(),
                                   IsEnableOss = b == null ? 0 : b.IsEnableOss,
                                   TrueName = b == null ? "" : b.TrueName,
                                   UserImg = b == null ? "" : b.UserImg,
                                   UserName = b == null ? "" : b.UserName,
                               }).ToList<UInfo>();
                }

                return stuList.OrderBy(i => i.UserID).ToList();
            }
        }
    }
