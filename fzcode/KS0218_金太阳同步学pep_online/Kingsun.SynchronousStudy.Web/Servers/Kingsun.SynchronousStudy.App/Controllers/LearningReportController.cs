using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using System.Configuration;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.DB;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;


namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class LearningReportController : ApiController
    {
        BaseManagement bm = new BaseManagement();
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

        private static IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        private static IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();

        /// <summary>
        /// 通过班级ID查询班级下的所有学生 [SelfID--作为班级ID,OtherID--作为学生ID]
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        private List<string> GetStuListByClassID(string classId)
        {
            List<string> StuList = new List<string>();
            var classinfo = classBLL.GetClassUserRelationByClassId(classId);
            if (classinfo != null)
            {
                if (classinfo.ClassStuList != null && classinfo.ClassStuList.Count > 0)
                {
                    classinfo.ClassStuList.ForEach(a =>
                    {
                        if (!StuList.Contains(a.StuID.ToString()))
                        {
                            StuList.Add(a.StuID.ToString());
                        }
                    });
                }
            }
            return StuList;
        }


        /// <summary>
        /// 通过老师Id获取班级信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserClassByUserId(string UserId)
        {
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(UserId));
            if (user != null)
            {
                List<CInfo> classList = new List<CInfo>();
                if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                {
                    user.ClassSchList.ForEach(a =>
                    {
                        var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                        classList.Add(new CInfo
                        {
                            ID = classinfo.ClassNum.ToString(),
                            ClassName = classinfo.ClassName//将班级Id和班级名称添加到集合中
                        });
                    });
                }
                List<CInfo> returnCinfoList = ClassOrder(classList);//排序后的班级新
                object obj = new { ClassList = returnCinfoList };//返回信息
                return ObjectToJson.GetResult(obj, "操作成功");
            }
            else
            {
                return ObjectToJson.GetErrorResult("班级不存在");
            }
        }


        /// <summary>
        /// 班级排序
        /// </summary>
        /// <param name="list"></param>
        public List<CInfo> ClassOrder(List<CInfo> list)
        {
            List<CInfo> classList = list;
            List<CInfo> returnList = new List<CInfo>();
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
        /// 通过班级Id统计学习报告人数(分页)
        /// </summary>
        /// <param name="ClassId"></param>
        /// <returns></returns> 
        [HttpGet]
        public HttpResponseMessage GetLearReportListByClassId(string classId, int pageNumber, string appId)
        {
            if (pageNumber < 0) { return ObjectToJson.GetErrorResult("页码不对,必须是>=0的正整数"); }

            string sql = string.Format(@"SELECT VersionID FROM dbo.TB_APPManagement WHERE ID='{0}'", appId);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            string where = " 1=1 ";
            if (ds.Tables[0].Rows.Count > 0)
            {
                where += " AND versionid='" + ds.Tables[0].Rows[0]["VersionID"] + "'";
            }
            where += " AND ClassID='" + classId + "'";
            IList<TB_UserStudyReport> usc = bm.Search<TB_UserStudyReport>(where);
            List<LearnReport> learnReportList = new List<LearnReport>();
            if (usc != null && usc.Count > 0)
            {
                learnReportList.AddRange(usc.Select(item => new LearnReport
                {
                    ClassID = item.ClassID.ToString(),
                    ExpirationDate = Convert.ToDateTime(item.CreateTime.ToString()).ToString("yyyy-MM-dd"),
                    StudyCount = item.ClassStudentCount,
                    StudyStudentCount = item.StudentStudyCount
                }));
                //以时间为单位，降序排列
                learnReportList.Sort((h1, h2) => string.Compare(h2.ExpirationDate, h1.ExpirationDate, StringComparison.Ordinal));

                learnReportList = learnReportList.Skip(pageNumber * 10).Take(10).ToList();
                object obj = new { LearnReport = learnReportList, Date = DateTime.Now.ToString("yyyy-MM-dd") };
                return ObjectToJson.GetResult(obj, "操作成功");
            }
            else
            {
                return ObjectToJson.GetErrorResult("该班级下没有学习报告信息");
            }

        }

        /// <summary>
        /// 根据班级Id、截止日期、册别统计趣配音学生总数
        /// </summary>
        /// <param name="ClassId">班级Id</param>
        /// <param name="ExpirationDate">截止日期</param>
        /// <param name="BookId">册别</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDubbingStudentCount(string ClassId, string ExpirationDate, int BookId, int pageNumber)
        {
            if (pageNumber < 0) { return ObjectToJson.GetErrorResult("页码不对,必须是>=0的正整数"); }
            IList<TB_UserStudyDirectory> usd = bm.Search<TB_UserStudyDirectory>(" ClassID=" + ClassId + " AND CreateTime='" + ExpirationDate + "' AND BookID=" + BookId + " ORDER BY VideoNumber");
            List<StuDubbingList> StuDubbingList = new List<StuDubbingList>();

            if (usd != null && usd.Count > 0)
            {
                foreach (var item in usd)
                {
                    string ModName = ""; string UniName = ""; string MUName = "";
                    ModName = item.FirstTitle.Trim().Length >= 9 ? item.FirstTitle.Substring(0, 9).Trim() + "..." : item.FirstTitle.Trim();
                    UniName = item.SecondTitle.Trim().Length >= 9 ? item.SecondTitle.Substring(0, 9).Trim() + "..." : item.SecondTitle.Trim();
                    if (ModName != "" && UniName != "") { MUName = ModName + "/" + UniName; }
                    if (ModName != "" && UniName == "") { MUName = ModName; }

                    List<StudentPeiYin> Moduleslist = new List<StudentPeiYin>();
                    Moduleslist.Add(new StudentPeiYin
                    {
                        ModuleId = item.FirstModularID,
                        ModuleName = item.FirstModular,
                        StudyStudentCount = item.StudentStudyCount,
                        StudentCount = item.ClassStudentCount,
                        ExpirationDate = ExpirationDate
                    });

                    StuDubbingList.Add(new StuDubbingList
                    {
                        FirstTitleID = item.FirstTitleID,
                        SecondTitleID = item.SecondTitleID,
                        Catalague = MUName,
                        Modules = Moduleslist
                    });
                }
                StuDubbingList = StuDubbingList.Skip(pageNumber * 10).Take(10).ToList();
                return ObjectToJson.GetResult(StuDubbingList, "操作成功");
            }
            else
            {
                return ObjectToJson.GetErrorResult("无数据信息");
            }
        }

        /// <summary>
        /// 根据班级Id、模块Id、单元ID获取某模块的学习情况
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="firstTitleId"></param>
        /// <param name="secondTitleId"></param>
        /// <param name="expirationDate"></param>
        /// <param name="bookId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDubbingDetailById(string classId, int firstTitleId, int secondTitleId, string expirationDate, int pageNumber)
        {
            if (pageNumber < 0) { return ObjectToJson.GetErrorResult("页码不对,必须是>=0的正整数"); }

            //string classId, int firstTitleId, int secondTitleId, string expirationDate, int bookId, int pageNumber
            //string classId = "eec92b23-5091-4fa7-8f6c-fa7a176c2252";
            //string expirationDate = "2017-03-13";
            //int firstTitleId = 285986;
            //int secondTitleId = 285987;
            //int pageNumber = 1;

            string where = "";
            List<UInfo> stulist = OldGetStuListByClassShortID(classId);//匹配学生
            if (stulist == null || stulist.Count == 0) { return ObjectToJson.GetErrorResult("该班级没有学生"); }

            if (secondTitleId != 0)
            {
                where = " FirstTitleID=" + firstTitleId + " and  SecondTitleID=" + secondTitleId;
                //+ " and CreateTime<='" + expirationDate + " 23:59:59'";
            }
            else
            {
                where = " FirstTitleID=" + firstTitleId; //+ " and CreateTime<='" + expirationDate + " 23:59:59'";
            }

            IList<TB_UserStudyDeatilsLite3> tempuslist = bm.Search<TB_UserStudyDeatilsLite3>(where);
            if (tempuslist == null || tempuslist.Count == 0) { return ObjectToJson.GetErrorResult("该模块单元下没有配音的学生"); }

            // List<object> listobj = new List<object>(); //object obj = null; string userImg = ""; string userName = ""; DateTime createTime;
            List<UserVideosInfo> uservideoinfo = new List<UserVideosInfo>();
            UserVideosInfo uvi;
            foreach (var item in stulist)
            {
                uvi = new UserVideosInfo();
                Vedios video;
                List<Vedios> videos = new List<Vedios>();

                IList<TB_UserStudyDeatilsLite3> uslist = tempuslist.Where(i => i.UserID.ToString() == item.UserID).ToList();
                if (uslist.Count == 0)
                {
                    uvi.UserId = Convert.ToInt32(item.UserID);
                    uvi.UserImg = item.UserImg;
                    uvi.UserName = item.UserName;
                    uvi.vedios = videos;
                }
                else
                {
                    foreach (var usItem in uslist)
                    {
                        uvi.UserId = usItem.UserID;//返回UserId
                        if (usItem.UserImage != null) uvi.UserImg = usItem.UserImage;
                        if (usItem.CreateTime != null) uvi.CreateTime = usItem.CreateTime.Value.ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(usItem.NickName))
                        {
                            uvi.UserName = usItem.NickName;
                        }
                        else if (!string.IsNullOrEmpty(usItem.TrueName))
                        {
                            uvi.UserName = usItem.TrueName;
                        }
                        else if (!string.IsNullOrEmpty(usItem.UserName))
                        {
                            uvi.UserName = usItem.UserName;
                        }

                        video = new Vedios();
                        video.VedioId = usItem.id;
                        video.VedioName = usItem.VideoTitle;
                        if (usItem.CreateTime != null) video.CreateTime = usItem.CreateTime.Value.ToString("yyyy.MM.dd");
                        if (usItem.TotalScore != null) video.Score = usItem.TotalScore.ToString();
                        videos.Add(video);
                        uvi.vedios = videos;
                        //obj = new { UserName = userName, CreateTime = createTime, UserImg = userImg, Vedios = video };
                        //listobj.Add(obj);
                    }
                }
                uservideoinfo.Add(uvi);
            }
            //以时间为单位，降序排列
            uservideoinfo.Sort((h1, h2) => string.Compare(h2.CreateTime, h1.CreateTime, StringComparison.Ordinal));

            uservideoinfo = uservideoinfo.Skip(pageNumber * 10).Take(10).ToList();

            //if (uservideoinfo.Count == 0) { return ObjectToJson.GetErrorResult("该模块单元下没有匹配的趣配音信息"); }
            return ObjectToJson.GetResult(uservideoinfo, "操作成功");//返回信息 
        }


        /// <summary>
        /// 获取某一天某班级下课程册次学习情况
        /// </summary>
        /// <param name="ClassId"></param>
        /// <param name="ExpirationDate"></param>
        /// <param name="BookId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetStuCourseStudyCountByCondition(string appId, string ClassID, string ExpirationDate)
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
                                                            AND a.CreateTime = '{0}'
                                                            AND a.ClassShortID = '{1}'
                                                            AND b.ID = '{2}'
                                                  UNION
                                                  SELECT    BookID ,
                                                            JuniorGrade ,
                                                            TeachingBooks ,
                                                            '0' AS StudentStudyCount
                                                  FROM      dbo.TB_CurriculumManage a
                                                            LEFT JOIN dbo.TB_APPManagement b ON a.EditionID = b.VersionID
                                                  WHERE     [State] = 1
                                                            AND b.ID = '{2}'
                                                ) a
                                         GROUP BY a.BookID ,
                                                a.JuniorGrade ,
                                                a.TeachingBooks", ExpirationDate, ClassID, appId);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            List<Curriculum> Listobj = new List<Curriculum>();
            Curriculum obj;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[i]["StudentStudyCount"]) > 0)
                {
                    int bookId = Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"]);
                    string BookName = ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"].ToString();
                    obj = new Curriculum { CourseID = bookId, CourseName = BookName, IsStudy = true, ExpirationDate = ExpirationDate };
                }
                else
                {
                    int bookId = Convert.ToInt32(ds.Tables[0].Rows[i]["BookID"]);
                    string BookName = ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"].ToString();
                    obj = new Curriculum { CourseID = bookId, CourseName = BookName, IsStudy = false, ExpirationDate = ExpirationDate };
                }
                Listobj.Add(obj);
            }
            if (Listobj.Count > 0)
            {
                return ObjectToJson.GetResult(CurriculumOrder(Listobj), "操作成功");
            }
            else
            {
                return ObjectToJson.GetErrorResult("数据不存在！");
            }

        }


        /// <summary>
        /// 获取模块下视频标题
        /// </summary>
        /// <param name="ClassId"></param>
        /// <param name="ExpirationDate"></param>
        /// <param name="BookId"></param> 
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetVideoTitleByFirstTitleId([FromBody]KingRequest request)
        {
            GetVideoJson submitData = JsonHelper.DecodeJson<GetVideoJson>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.ClassId))
            {
                return ObjectToJson.GetErrorResult("班级ID不能为空！");
            }
            if (submitData.FirstTitleID < 0)
            {
                return ObjectToJson.GetErrorResult("标题不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.ExpirationDate))
            {
                return ObjectToJson.GetErrorResult("截止日期不能为空！");
            }

            string where = "";
            if (submitData.SecondTitleID != 0)
            {
                where = " FirstTitleID=" + submitData.FirstTitleID + " and  SecondTitleID=" + submitData.SecondTitleID;
            }
            else
            {
                where = " FirstTitleID=" + submitData.FirstTitleID;
            }

            IList<TB_GetVideoTitleInfo> tv = bm.Search<TB_GetVideoTitleInfo>(where);
            List<reVideoJson> list = tv.Select(item => new reVideoJson
            {
                VedioTitle = item.VideoTitle,
                ImgUrl = item.IsEnableOss != 0 ? _getOssFilesUrl + item.VideoImageAddress : _getFilesUrl + "?FileID=" + item.VideoImageAddress,
                VideoNumber = item.VideoNumber,
                BookId = item.BookID,
                ExpirationDate = submitData.ExpirationDate,
                ClassId = submitData.ClassId
            }).ToList();
            if (list.Count > 0)
            {
                return ObjectToJson.GetResult(list, "操作成功");
            }
            else
            {
                return ObjectToJson.GetErrorResult("版本ID不能小于0！");
            }
        }

        /// <summary>
        /// 根据班级Id、模块Id、单元ID获取某模块的学习情况(新)
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="firstTitleId"></param>
        /// <param name="secondTitleId"></param>
        /// <param name="expirationDate"></param>
        /// <param name="bookId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage NewGetDubbingDetailById([FromBody]KingRequest request)
        {
            GetVideoJson submitData = JsonHelper.DecodeJson<GetVideoJson>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.ClassId))
            {
                return ObjectToJson.GetErrorResult("班级ID不能为空！");
            }
            //if (submitData.BookId < 0)
            //{
            //    return ObjectToJson.GetErrorResult("书籍Id不能为空！");
            //}
            //if (submitData.VideoNumber < 0)
            //{
            //    return ObjectToJson.GetErrorResult("视频序号不能为空！");
            //}
            if (string.IsNullOrEmpty(submitData.ExpirationDate))
            {
                return ObjectToJson.GetErrorResult("截止日期不能为空！");
            }
            if (submitData.PageNumber < 0)
            {
                return ObjectToJson.GetErrorResult("页码不能小于0！");
            }

            List<UInfo> stulist = GetStuListByClassShortID(submitData.ClassId);//匹配学生
            if (stulist == null || stulist.Count == 0) { return ObjectToJson.GetErrorResult("该班级没有学生"); }
            string where = "";
            if (submitData.SecondTitleID != 0)
            {
                where = " FirstTitleID=" + submitData.FirstTitleID + " and  SecondTitleID=" + submitData.SecondTitleID;
                //+ " and CreateTime<='" + expirationDate + " 23:59:59'";
            }
            else
            {
                where = " FirstTitleID=" + submitData.FirstTitleID; //+ " and CreateTime<='" + expirationDate + " 23:59:59'";
            }

            IList<TB_UserStudyDeatilsLite3> tempuslist = bm.Search<TB_UserStudyDeatilsLite3>(where);
            if (tempuslist == null || tempuslist.Count == 0) { return ObjectToJson.GetErrorResult("该模块单元下没有配音的学生"); }

            // List<object> listobj = new List<object>(); //object obj = null; string userImg = ""; string userName = ""; DateTime createTime;
            List<UserVideosInfo> uservideoinfo = new List<UserVideosInfo>();
            UserVideosInfo uvi;
            foreach (var item in stulist)
            {
                uvi = new UserVideosInfo();
                Vedios video;
                List<Vedios> videos = new List<Vedios>();

                IList<TB_UserStudyDeatilsLite3> uslist = tempuslist.Where(i => i.UserID.ToString() == item.UserID).ToList();
                if (uslist.Count == 0)
                {

                    uvi.UserId = Convert.ToInt32(item.UserID);
                    uvi.UserImg = item.UserImg; ;
                    uvi.UserName = item.UserName;
                    uvi.vedios = videos;
                }
                else
                {
                    foreach (var usItem in uslist)
                    {
                        string imgUrl = usItem.IsEnableOss != 0
                              ? _getOssFilesUrl + usItem.UserImage
                              : _getFilesUrl + "?FileID=" + usItem.UserImage;
                        uvi.UserId = usItem.UserID;//返回UserId
                        if (usItem.UserImage != null) uvi.UserImg = imgUrl;
                        if (usItem.CreateTime != null) uvi.CreateTime = usItem.CreateTime.Value.ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(usItem.NickName))
                        {
                            uvi.UserName = usItem.NickName;
                        }
                        else if (!string.IsNullOrEmpty(usItem.TrueName))
                        {
                            uvi.UserName = usItem.TrueName;
                        }
                        else if (!string.IsNullOrEmpty(usItem.UserName))
                        {
                            uvi.UserName = usItem.UserName;
                        }

                        video = new Vedios
                        {
                            VedioId = usItem.id,
                            VedioName = usItem.VideoTitle
                        };
                        if (usItem.CreateTime != null) video.CreateTime = usItem.CreateTime.Value.ToString("yyyy.MM.dd");
                        video.Score = usItem.TotalScore.ToString("0.0");
                        videos.Add(video);
                        uvi.vedios = videos;
                    }
                }
                uservideoinfo.Add(uvi);
            }
            //以时间为单位，降序排列
            uservideoinfo.Sort((h1, h2) => string.Compare(h2.CreateTime, h1.CreateTime, StringComparison.Ordinal));

            uservideoinfo = uservideoinfo.Skip(submitData.PageNumber * 10).Take(10).ToList();
            //if (uservideoinfo.Count == 0) { return ObjectToJson.GetErrorResult("该模块单元下没有匹配的趣配音信息"); }
            return ObjectToJson.GetResult(uservideoinfo, "操作成功");//返回信息 
        }

        /// <summary>
        /// 根据班级Id、模块Id、单元ID获取某模块的学习情况(新)
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="firstTitleId"></param>
        /// <param name="secondTitleId"></param>
        /// <param name="expirationDate"></param>
        /// <param name="bookId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage NewGetDubbingDetailByIdTest()
        {
            GetVideoJson submitData = new GetVideoJson(); //JsonHelper.DecodeJson<GetVideoJson>(request.Data);
            //if (submitData == null)
            //{
            //    return ObjectToJson.GetErrorResult("当前信息为空");
            //}
            //if (string.IsNullOrEmpty(submitData.ClassId))
            //{
            //    return ObjectToJson.GetErrorResult("班级ID不能为空！");
            //}
            //if (submitData.BookId < 0)
            //{
            //    return ObjectToJson.GetErrorResult("书籍Id不能为空！");
            //}
            //if (submitData.VideoNumber < 0)
            //{
            //    return ObjectToJson.GetErrorResult("视频序号不能为空！");
            //}
            //if (string.IsNullOrEmpty(submitData.ExpirationDate))
            //{
            //    return ObjectToJson.GetErrorResult("截止日期不能为空！");
            //}
            //if (submitData.PageNumber < 0)
            //{
            //    return ObjectToJson.GetErrorResult("页码不能小于0！");
            //}
            //{"ClassId":"37675411","pageNumber":"0","ExpirationDate":"2017-04-05","BookId":"2","FirstTitleID":"277654","SecondTitleID":"277655"}
            submitData.ClassId = "08397946";
            submitData.PageNumber = 1;
            submitData.ExpirationDate = "2017-02-23";
            submitData.BookId = 170;
            submitData.FirstTitleID = 279516;
            submitData.SecondTitleID = 279517;


            List<UInfo> stulist = GetStuListByClassShortID(submitData.ClassId);//匹配学生
            if (stulist == null || stulist.Count == 0) { return ObjectToJson.GetErrorResult("该班级没有学生"); }
            string where = "";
            if (submitData.SecondTitleID != 0)
            {
                where = " FirstTitleID=" + submitData.FirstTitleID + " and  SecondTitleID=" + submitData.SecondTitleID;
                //+ " and CreateTime<='" + expirationDate + " 23:59:59'";
            }
            else
            {
                where = " FirstTitleID=" + submitData.FirstTitleID; //+ " and CreateTime<='" + expirationDate + " 23:59:59'";
            }

            IList<TB_UserStudyDeatilsLite3> tempuslist = bm.Search<TB_UserStudyDeatilsLite3>(where);
            if (tempuslist == null || tempuslist.Count == 0) { return ObjectToJson.GetErrorResult("该模块单元下没有配音的学生"); }

            // List<object> listobj = new List<object>(); //object obj = null; string userImg = ""; string userName = ""; DateTime createTime;
            List<UserVideosInfo> uservideoinfo = new List<UserVideosInfo>();
            UserVideosInfo uvi;
            foreach (var item in stulist)
            {
                uvi = new UserVideosInfo();
                Vedios video;
                List<Vedios> videos = new List<Vedios>();

                IList<TB_UserStudyDeatilsLite3> uslist = tempuslist.Where(i => i.UserID.ToString() == item.UserID).ToList();
                if (uslist.Count == 0)
                {

                    uvi.UserId = Convert.ToInt32(item.UserID);
                    uvi.UserImg = item.UserImg; ;
                    uvi.UserName = item.UserName;
                    uvi.vedios = videos;
                }
                else
                {
                    foreach (var usItem in uslist)
                    {
                        string imgUrl = usItem.IsEnableOss != 0
                              ? _getOssFilesUrl + usItem.UserImage
                              : _getFilesUrl + "?FileID=" + usItem.UserImage;
                        uvi.UserId = usItem.UserID;//返回UserId
                        if (usItem.UserImage != null) uvi.UserImg = imgUrl;
                        if (usItem.CreateTime != null) uvi.CreateTime = usItem.CreateTime.Value.ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(usItem.NickName))
                        {
                            uvi.UserName = usItem.NickName;
                        }
                        else if (!string.IsNullOrEmpty(usItem.TrueName))
                        {
                            uvi.UserName = usItem.TrueName;
                        }
                        else if (!string.IsNullOrEmpty(usItem.UserName))
                        {
                            uvi.UserName = usItem.UserName;
                        }

                        video = new Vedios
                        {
                            VedioId = usItem.id,
                            VedioName = usItem.VideoTitle
                        };
                        if (usItem.CreateTime != null) video.CreateTime = usItem.CreateTime.Value.ToString("yyyy.MM.dd");
                        video.Score = usItem.TotalScore.ToString("0.0");
                        videos.Add(video);
                        uvi.vedios = videos;
                    }
                }
                uservideoinfo.Add(uvi);
            }
            //以时间为单位，降序排列
            uservideoinfo.Sort((h1, h2) => string.Compare(h2.CreateTime, h1.CreateTime, StringComparison.Ordinal));

            uservideoinfo = uservideoinfo.Skip(submitData.PageNumber * 10).Take(10).ToList();
            //if (uservideoinfo.Count == 0) { return ObjectToJson.GetErrorResult("该模块单元下没有匹配的趣配音信息"); }
            return ObjectToJson.GetResult(uservideoinfo, "操作成功");//返回信息 
        }

        /// <summary>
        /// 年级排序
        /// </summary>
        /// <param name="list"></param>
        public List<Curriculum> CurriculumOrder(List<Curriculum> list)
        {
            List<Curriculum> classList = list;
            List<Curriculum> returnList = new List<Curriculum>();
            if (classList != null && classList.Count > 0)
            {
                string[] gradeArr = { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" };
                for (int i = 0, length = gradeArr.Length; i < length; i++)
                {
                    returnList.AddRange(classList.Where(t => t.CourseName.IndexOf(gradeArr[i], StringComparison.Ordinal) > -1));
                }
            }
            return returnList;
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }


        /// <summary>
        /// 通过班级ID查询班级下的所有学生
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        private static List<UInfo> GetStuListByClassShortID(string classId)
        {
            string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
            string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

            List<UInfo> StuList = new List<UInfo>();

            var uclass = classBLL.GetClassUserRelationByClassId(classId);
            if (uclass != null)
            {
                if (uclass.ClassStuList != null && uclass.ClassStuList.Count > 0)
                {
                    uclass.ClassStuList.ForEach(a =>
                    {
                        UInfo ui = new UInfo();
                        ui.UserID = a.StuID.ToString();
                        ui.UserImg = a.IsEnableOss != 0 ? _getOssFilesUrl + a.UserImage : _getFilesUrl + "?FileID=" + a.UserImage;
                        if (!string.IsNullOrEmpty(a.StuName))
                        {
                            ui.UserName = a.StuName;
                        }
                        else
                        {
                            ui.UserName = "暂未填写";
                        }
                        ui.IsEnableOss = Convert.ToInt32(a.IsEnableOss);
                        StuList.Add(ui);
                    });
                }
            }

            return StuList.OrderBy(i => i.UserID).ToList();
        }

        /// <summary>
        /// 通过班级ID查询班级下的所有学生
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        private static List<UInfo> OldGetStuListByClassShortID(string classId)
        {
            List<UInfo> StuList = new List<UInfo>();
            string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
            string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

            var uclass = classBLL.GetClassUserRelationByClassId(classId);
            if (uclass != null)
            {
                if (uclass.ClassStuList != null && uclass.ClassStuList.Count > 0)
                {
                    uclass.ClassStuList.ForEach(a =>
                    {
                        UInfo ui = new UInfo();
                        ui.UserID = a.StuID.ToString();
                        ui.UserImg = a.IsEnableOss != 0 ? _getOssFilesUrl + a.UserImage : _getFilesUrl + "?FileID=" + a.UserImage;
                        if (!string.IsNullOrEmpty(a.StuName))
                        {
                            ui.UserName = a.StuName;
                        }
                        else
                        {
                            ui.UserName = "暂未填写";
                        }
                        ui.IsEnableOss = Convert.ToInt32(a.IsEnableOss);
                        StuList.Add(ui);
                    });
                }
            }
            return StuList;
        }
    }

    public class GetVideoJson
    {
        public string ClassId { get; set; }
        public int FirstTitleID { get; set; }
        public int SecondTitleID { get; set; }
        public string ExpirationDate { get; set; }
        public int PageNumber { get; set; }
        public int BookId { get; set; }
        public int VideoNumber { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class reVideoJson
    {
        public string VedioTitle { get; set; }
        public string ImgUrl { get; set; }
        public int? VideoNumber { get; set; }
        public int? BookId { get; set; }
        public string ExpirationDate { get; set; }
        public int IsEnableOss { get; set; }
        public string ClassId { get; set; }
    }

    public class UInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class Curriculum
    {

        public int CourseID { get; set; }

        public string CourseName { get; set; }

        public bool IsStudy { get; set; }

        public string ExpirationDate { get; set; }
    }

    public class CInfo
    {
        public string ID { get; set; }

        public string ClassName { get; set; }
    }

    public class LearnReport
    {
        public string ClassID { get; set; }

        public string ExpirationDate { get; set; }

        public int? StudyStudentCount { get; set; }

        public int? StudyCount { get; set; }
    }

    public class UserVideosInfo
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string CreateTime { get; set; }
        public string UserImg { get; set; }

        public List<Vedios> vedios { get; set; }
    }

    public class Vedios
    {
        public int VedioId { get; set; }
        public string VedioName { get; set; }
        public string Score { get; set; }
        public string CreateTime { get; set; }
    }

    public class StudentPeiYin
    {
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? StudyStudentCount { get; set; }
        public int? StudentCount { get; set; }
        public string ExpirationDate { get; set; }

    }

    public class StuDubbingList
    {
        public string Catalague { get; set; }
        public int? FirstTitleID { get; set; }
        public int? SecondTitleID { get; set; }
        public List<StudentPeiYin> Modules { get; set; }
    }
}