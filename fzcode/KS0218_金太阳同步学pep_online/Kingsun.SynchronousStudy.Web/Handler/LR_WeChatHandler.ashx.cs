using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Web.RelationService;
using log4net;

namespace Kingsun.SynchronousStudy.Web.Handler
{
    /// <summary>
    /// LR_WeChatHandler 的摘要说明
    /// </summary>
    public class LR_WeChatHandler : IHttpHandler
    {
        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string FoundationDatabase = WebConfigurationManager.AppSettings["FounDation"]; //基础数据库
        string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        public string FilesURL = WebConfigurationManager.AppSettings["getFiles"];

        //public static Dictionary<string, RelationService.tb_Relation[]> dicTeastu; //班级学生列表键值对
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string queryKey = context.Request["queryKey"].ToLower() ?? "";
            switch (queryKey)
            {
                case "queryclasslist": //通过老师ID获取班级列表
                    QueryClassList(context);
                    break;
                case "getmoduleinfo":
                    GetModuleInfo(context);
                    break;
                case "querylearningreport": //获取班级学习数据
                    QueryLearningReport(context);
                    break;
                case "getvideodetailscount":
                    GetVideoDetailsCount(context);
                    break;
                case "getclassstustudyinfo":
                    GetClassStuStudyInfo(context);
                    break;
                case "getreporttime":
                    GetReportTime(context);
                    break;
                case "getclassinfobyclassid":
                    GetClassInfoByClassId(context);
                    break;
                case "getgradeinfo":
                    GetGradeInfo(context);
                    break;
                case "getmoduleinfobybookid":
                    GetModuleInfoByBookID(context);
                    break;
                default:
                    context.Response.Write("{\"Result\":\"false\",\"msg\":\"\",\"data\":\"\"}");
                    break;
            }
        }

        #region
        /// <summary>
        /// 通过老师ID获取班级列表
        /// </summary>
        /// <param name="context"></param>
        private void QueryClassList(HttpContext context)
        {
            string userID = context.Request.Form["UserID"].ToString();
            try
            {
                //页面初始加载班级 
                #region
                RelationService.RelationService relationService = new RelationService.RelationService();
                ReturnInfo classList = relationService.GetTeacherClassInfoByUserId(userID);
                List<ClassInfo> returnList = new List<ClassInfo>();

                ClassInfo cInfo = new ClassInfo();
                if (classList != null)
                {
                    if (classList.Data != null)
                    {
                        CSinfo csinfo = JsonHelper.DecodeJson<CSinfo>(classList.Data.ToString());
                        foreach (var item in csinfo.ClassInfo)
                        {
                            cInfo = new ClassInfo();
                            cInfo.StudentNum = 0;
                            cInfo.Id = item.Id;
                            cInfo.ClassNum = item.ClassNum;
                            cInfo.ClassName = item.ClassName;
                            cInfo.SchoolId = item.SchoolId;
                            cInfo.GradeId = item.GradeId;
                            //班级学生信息
                            //RelationService.tb_Relation[] studentList = relationService.GetStuListByClassID(item.ID.ToString());
                            //if (studentList != null)
                            //{
                            //    cInfo.StudentNum = studentList.Length;
                            //}
                            returnList.Add(cInfo);
                        }
                        returnList = new WeChatHandler().ClassOrder(returnList);
                        var obj = new { Success = classList.Success, ClassList = returnList, Msg = classList.ErrorMsg };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    else
                    {
                        var obj = new { Success = classList.Success, Msg = classList.ErrorMsg };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                }
                else
                {
                    var obj = new { Success = false };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
                #endregion
            }
            catch (Exception ex)
            {
                var obj = new { Success = false, Msg = ex.Message };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            context.Response.End();
        }

        /// <summary>
        /// 查询教师所有班级的学习数据--根据教师ID
        /// </summary>
        /// <param name="context"></param>
        private void QueryLearningReport(HttpContext context)
        {
            string userID = context.Request.Form["UserID"]; //登录教师ID
            string ClassID = context.Request.Form["ClassID"]; //所选班级IDs
            string EditionID = context.Request.Form["EditionID"]; //版本ID
            string Time = context.Request.Form["Time"]; //版本ID

            if (string.IsNullOrEmpty(EditionID))
            {
                var obj = new { Success = false, Msg = "BookID为空" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            StringBuilder CatalogInfo = new StringBuilder();
            StringBuilder userList = new StringBuilder();
            StringBuilder json = new StringBuilder();
            string BookID = "";
            string GradeName = "";
            RelationService.RelationService rs = new RelationService.RelationService();
            int classStuNum = 0;
            //查询班级下的学生
            User[] user = rs.GetStudentListByClassId(ClassID);
            if (user != null && user.Length > 0)
            {
                classStuNum = user.Length;
                foreach (User m in user)
                {
                    userList.Append("'" + m.UserID + "',");
                }
                userList.Remove(userList.Length - 1, 1);
            }

            tb_Class classInfo = rs.GetClassInfoByID(ClassID);
            if (classInfo != null)
            {
                string sql = string.Format(@"SELECT  TextbookVersion ,
                                                            [EditionID] ,
                                                            [JuniorGrade] ,
                                                            [GradeID] ,
                                                            [TeachingBooks] ,
                                                            [BreelID] ,
                                                            [BookID]
                                                    FROM    [TB_CurriculumManage] WHERE EditionID='{0}' AND GradeID='{1}'", EditionID, classInfo.GradeID);

                DataSet dsBookid = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (dsBookid != null)
                {
                    if (dsBookid.Tables[0].Rows.Count > 0)
                    {
                        BookID = dsBookid.Tables[0].Rows[1]["BookID"].ToString();
                        GradeName = dsBookid.Tables[0].Rows[1]["JuniorGrade"].ToString() + dsBookid.Tables[0].Rows[1]["TeachingBooks"];
                    }
                }
            }

            string sqls = string.Format(@"SELECT FirstTitileID,FirstTitle,SecondTitleID,SecondTitle FROM dbo.TB_ModuleConfiguration WHERE State=0 AND BookID='{0}'", BookID);
            DataSet da = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqls);
            if (da.Tables[0].Rows.Count > 0)
            {
                json.Append("[");
                for (int i = 0; i < da.Tables[0].Rows.Count; i++)
                {
                    if (da.Tables[0].Rows[i]["SecondTitleID"] == null || da.Tables[0].Rows[i]["SecondTitleID"].ToString() == "")
                    {
                        json.Append("{\"id\":\"" + da.Tables[0].Rows[i]["FirstTitileID"] + "\",\"name\":\"" + da.Tables[0].Rows[i]["FirstTitle"] + "\"},");
                    }
                    else
                    {
                        string fTitle = da.Tables[0].Rows[i]["FirstTitle"].ToString().Length > 9 ? da.Tables[0].Rows[i]["FirstTitle"].ToString().Substring(0, 9) + "..." : da.Tables[0].Rows[i]["FirstTitle"].ToString();
                        string sTitle = da.Tables[0].Rows[i]["SecondTitle"].ToString().Length > 9 ? da.Tables[0].Rows[i]["SecondTitle"].ToString().Substring(0, 9) + "..." : da.Tables[0].Rows[i]["SecondTitle"].ToString();
                        json.Append("{\"id\":\"" + da.Tables[0].Rows[i]["FirstTitileID"] + "_" + da.Tables[0].Rows[i]["SecondTitleID"] + "\",\"name\":\"" + fTitle + "/" + sTitle + "\"},");
                    }
                }
                json.Remove(json.Length - 1, 1);
                json.Append("]");

                string where = "";
                if (da.Tables[0].Rows[0]["SecondTitleID"] == null || da.Tables[0].Rows[0]["SecondTitleID"].ToString() == "")
                {
                    where = " b.FirstTitleID='" + da.Tables[0].Rows[0]["FirstTitileID"] + "'";
                }
                else
                {
                    where = " b.SecondTitleID='" + da.Tables[0].Rows[0]["SecondTitleID"] + "'";
                }

                string strsql = string.Format(@"SELECT  COUNT(DISTINCT
                                                        ( a.UserID ) ) AS stuNum
                                                FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                                        LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDetails] b ON a.BookID = b.BookID
                                                                                           AND b.VideoNumber = a.VideoNumber
                                                WHERE   a.UserID IN ( {0})
                                                        AND a.BookID = '{1}'
                                                        AND a.CreateTime BETWEEN '{2} 00:00:00'
                                                                         AND     '{2} 23:59:59'
                                                        AND a.State = 1 AND {3} ", userList, BookID, Time, where);

                int stuNum = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, strsql));

                var obj = new { Success = true, ModuleConfig = json.ToString(), classStuNum = classStuNum, stuNum = stuNum, BookID = BookID, GradeName = GradeName };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            else
            {
                var obj = new { Success = false, Msg = "无数据" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }

        /// <summary>
        /// 获取班级下用户趣配音完成度
        /// </summary>
        /// <param name="context"></param>
        private void GetVideoDetailsCount(HttpContext context)
        {
            string ClassID = context.Request.Form["ClassID"].ToString();  //班级ID
            string DTime = context.Request.Form["time"].ToString();         //查询时间
            string UnitId = context.Request.Form["UnitId"].ToString();  //UnitId
            try
            {
                if (ClassID != string.Empty && DTime != string.Empty)
                {
                    #region 读取数据
                    RelationService.RelationService relationService = new RelationService.RelationService();
                    IList<User> studentList = relationService.GetStudentListByClassId(ClassID);
                    if (studentList != null && studentList.Count > 0)
                    {
                        List<int> lsStuid = new List<int>();
                        #region 获取班级下学生IDs
                        foreach (User m in studentList)
                        {
                            lsStuid.Add(Convert.ToInt32(m.UserID));
                        }
                        #endregion

                        #region 根据学生ID查询完成数量
                        LR_LearningReportBLL lrBLL = new LR_LearningReportBLL();
                        DataTable tempTb = lrBLL.GetVideoDetailsCountByClass(lsStuid, DTime, UnitId);
                        if (tempTb != null && tempTb.Rows.Count > 0)
                        {
                            var obj = new { Success = true, Data = tempTb.Rows.Count.ToString() };//tempTb.Rows[0]["Count"].ToString() 
                            context.Response.Write(JsonHelper.EncodeJson(obj));
                        }
                        else
                        {
                            //未获取到数据
                            var obj = new { Success = true, Data = 0, Msg = "获取不到计和数据，返回0" };
                            context.Response.Write(JsonHelper.EncodeJson(obj));
                        }
                        #endregion
                    }
                    else
                    {
                        //未获取到数据--学生列表
                        var obj = new { Success = true, Data = 0, Msg = "获取不到学生列表数据，返回0" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                    }
                    #endregion
                }
                else
                {
                    var obj = new { Success = false, Msg = "参数错误" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                }
            }
            catch (Exception ex)
            {
                var obj = new { Success = false, Msg = ex.Message };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            //context.Response.Write(JsonHelper.EncodeJson(""));
            context.Response.End();
        }

        /// <summary>
        /// 获取班级下用户趣配音数据
        /// </summary>
        /// <param name="context"></param>
        private void GetClassStuStudyInfo(HttpContext context)
        {
            string classid = context.Request.Form["ClassID"].ToString();  //班级ID
            string bookId = context.Request.Form["BookID"].ToString();         //查询时间
            string fTitleId = context.Request.Form["fid"].ToString();  //UnitId
            string Time = context.Request.Form["Times"].ToString();

            try
            {
                string fId = "";
                string sId = "";
                StringBuilder sb = new StringBuilder();
                int num = 1;
                string cid = "";

                if (!string.IsNullOrEmpty(fTitleId))
                {
                    if (fTitleId.Split('_').Length > 0)
                    {
                        fId = fTitleId.Split('_')[0];
                        sId = fTitleId.Split('_')[1];
                    }
                    else
                    {
                        fId = fTitleId;
                    }
                }

                RelationService.RelationService rl = new RelationService.RelationService();
                tb_Class tc = rl.GetClassInfoByID(classid);
                if (tc != null)
                {
                    cid = tc.ClassNum;
                }
                string where = " 1=1 AND a.BookID IS NOT NULL AND e.VersionID<>0 AND e.VersionID='21' AND e.State=1  ";
                if (!string.IsNullOrEmpty(cid))
                {
                    where += " AND b.ClassShortID = '" + cid + "' ";
                }
                if (!string.IsNullOrEmpty(bookId))
                {
                    where += " AND a.BookID = '" + bookId + "' ";

                    if (string.IsNullOrEmpty(fTitleId))
                    {
                        string sql = @"SELECT FirstTitleID,SecondTitleID FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE BookID=" + bookId + " ORDER BY SecondTitleID asc";
                        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            fId = ds.Tables[0].Rows[0]["FirstTitleID"].ToString();
                            sId = ds.Tables[0].Rows[0]["SecondTitleID"].ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Time))
                {
                    where += " AND  e.CreateTime  between '" + Convert.ToDateTime(Time).ToString("yyyy-MM-dd 00:00:00") +
                             "' and '"
                             + Convert.ToDateTime(Time).ToString("yyyy-MM-dd 23:59:59") + "' ";
                }

                if (!string.IsNullOrEmpty(sId))
                {
                    where += " AND c.SecondTitleID = '" + sId + "' ";
                }
                else if (!string.IsNullOrEmpty(fId))
                {
                    where += " AND c.FirstTitleID = '" + fId + "' ";
                }


                SqlParameter[] ps =
                {
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageCount", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar)
                };
                ps[0].Value = 1;//AspNetPager1.CurrentPageIndex;
                ps[1].Value = 5000;//AspNetPager1.PageSize;
                ps[2].Value = where;

                DataSet Newds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_TB_WechatStuVideoInfo", ps);
                sb.Append("<ul>");

                for (int i = 0; i < Newds.Tables[0].Rows.Count; i++)
                {
                    string name =
                            (Newds.Tables[0].Rows[i]["TrueName"].ToString() == ""
                                ? Newds.Tables[0].Rows[i]["UserName"].ToString()
                                : Newds.Tables[0].Rows[i]["TrueName"].ToString()) == ""
                                ? Newds.Tables[0].Rows[i]["NickName"].ToString()
                                : (Newds.Tables[0].Rows[i]["TrueName"].ToString() == ""
                                    ? Newds.Tables[0].Rows[i]["UserName"].ToString()
                                    : Newds.Tables[0].Rows[i]["TrueName"].ToString());



                    string userImg = Newds.Tables[0].Rows[i]["UserImage"].ToString() == "00000000-0000-0000-0000-000000000000" ? "/WeChat/Page/images/defaultImg.png" : FilesURL + "?FileID=" + Newds.Tables[0].Rows[i]["UserImage"] + "&view=true";

                    if (i == 0)
                    {
                        sb.Append("<li><span id=\"" + Newds.Tables[0].Rows[i]["UserID"].ToString() + "\" class=\"userinfo\"><img src=\"" + userImg + "\" alt=\"\"/>" + name + "</span><b>" + Newds.Tables[0].Rows[i]["VideoTitle"]);
                        sb.Append("<p class=\"ci_num\">" + Newds.Tables[0].Rows[i]["TotalScore"] + "分<a class=\"audio\" href=\"../../../Share.aspx?userID=" + Newds.Tables[0].Rows[i]["UserID"] + "&VideoFileID=" + Newds.Tables[0].Rows[i]["VideoFileID"] + "\"></a></p></b>");
                    }
                    else
                    {
                        if (Newds.Tables[0].Rows[i - 1]["UserID"].ToString() == Newds.Tables[0].Rows[i]["UserID"].ToString())
                        {
                            if (Newds.Tables[0].Rows[i - 1]["VideoNumber"].ToString() != Newds.Tables[0].Rows[i]["VideoNumber"].ToString())
                            {
                                sb.Append("<b>" + Newds.Tables[0].Rows[i]["VideoTitle"] + "<p class=\"ci_num\">" + Newds.Tables[0].Rows[i]["TotalScore"] + "分<a class=\"audio\"  href=\"../../../Share.aspx?userID=" + Newds.Tables[0].Rows[i]["UserID"] + "&VideoFileID=" + Newds.Tables[0].Rows[i]["VideoFileID"] + "\"></a></p></b>");
                            }
                        }
                        else
                        {
                            sb.Append("</li>");
                            sb.Append("<li><span id=\"" + Newds.Tables[0].Rows[i]["UserID"].ToString() + "\" class=\"userinfo\"><img src=\"" + userImg + "\" alt=\"\"/>" + name + "</span><b>" + Newds.Tables[0].Rows[i]["VideoTitle"]);
                            sb.Append("<p class=\"ci_num\">" + Newds.Tables[0].Rows[i]["TotalScore"] + "分<a class=\"audio\" href=\"../../../Share.aspx?userID=" + Newds.Tables[0].Rows[i]["UserID"] + "&VideoFileID=" + Newds.Tables[0].Rows[i]["VideoFileID"] + "\"></a></p></b>");
                        }
                    }
                }
                sb.Append("</li>");
                sb.Append("</ul>");
                var obj = new { Success = true, data = sb.ToString() };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                var obj = new { Success = false, Msg = ex.Message };
                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            //context.Response.Write(JsonHelper.EncodeJson(""));
            context.Response.End();
        }

        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <param name="context"></param>
        private void GetModuleInfo(HttpContext context)
        {
            string bookId = context.Request.Form["BookID"].ToString();
            StringBuilder json = new StringBuilder();
            string bId = "";
            if (!string.IsNullOrEmpty(bookId))
            {
                bId = bookId.Split('_')[1];
            }
            try
            {
                string sql = string.Format(@"SELECT FirstTitileID,FirstTitle,SecondTitleID,SecondTitle FROM dbo.TB_ModuleConfiguration WHERE State=0 AND BookID='{0}'", bId);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json.Append("[");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["SecondTitleID"] == null || ds.Tables[0].Rows[i]["SecondTitleID"].ToString() == "")
                        {
                            json.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["FirstTitileID"] + "\",\"name\":\"" + ds.Tables[0].Rows[i]["FirstTitle"] + "\"},");
                        }
                        else
                        {
                            string fTitle = ds.Tables[0].Rows[i]["FirstTitle"].ToString().Length > 9 ? ds.Tables[0].Rows[i]["FirstTitle"].ToString().Substring(0, 9) + "..." : ds.Tables[0].Rows[i]["FirstTitle"].ToString();
                            string sTitle = ds.Tables[0].Rows[i]["SecondTitle"].ToString().Length > 9 ? ds.Tables[0].Rows[i]["SecondTitle"].ToString().Substring(0, 9) + "..." : ds.Tables[0].Rows[i]["SecondTitle"].ToString();
                            json.Append("{\"id\":\"" + ds.Tables[0].Rows[i]["FirstTitileID"] + "_" + ds.Tables[0].Rows[i]["SecondTitleID"] + "\",\"name\":\"" + fTitle + "/" + sTitle + "\"},");
                        }
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }

            var obj = new { Success = true, data = json.ToString() };
            context.Response.Write(JsonHelper.EncodeJson(obj));
            context.Response.End();
        }

        /// <summary>
        /// 获取学习报告时间
        /// </summary>
        /// <param name="context"></param>
        private void GetReportTime(HttpContext context)
        {

            RelationService.RelationService rs = new RelationService.RelationService();
            string UserID = context.Request.Form["UserID"];
            string EditionID = context.Request.Form["EditionID"];
            string ClassID = context.Request.Form["ClassID"];
            string json = "";
            try
            {
                if (string.IsNullOrEmpty(ClassID))
                {
                    //查询到教师班级列表
                    ReturnInfo classList = rs.GetTeacherClassInfoByUserId(UserID);
                    if (classList.Success)
                    {
                        CSinfo csinfo = JsonHelper.DecodeJson<CSinfo>(classList.Data.ToString());

                        json = GetDayInfo(csinfo.ClassInfo[0].Id, EditionID);

                        if (!string.IsNullOrEmpty(json))
                        {
                            var obj = new { Success = true, data = json };
                            context.Response.Write(JsonHelper.EncodeJson(obj));
                            context.Response.End();
                        }
                        else
                        {
                            var obj = new { Success = false, Msg = "无数据" };
                            context.Response.Write(JsonHelper.EncodeJson(obj));
                            context.Response.End();
                        }
                    }
                }
                else
                {
                    json = GetDayInfo(ClassID, EditionID);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var obj = new { Success = true, data = json };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                        context.Response.End();
                    }
                    else
                    {
                        var obj = new { Success = false, Msg = "无数据" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                        context.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }

        /// <summary>
        /// 获取有学习报告的日期
        /// </summary>
        /// <param name="ClassID"></param>
        /// <param name="EditionID"></param>
        /// <returns></returns>
        private string GetDayInfo(string ClassID, string EditionID)
        {
            StringBuilder json = new StringBuilder();
            StringBuilder userList = new StringBuilder();
            RelationService.RelationService rs = new RelationService.RelationService();
            //查询班级下的学生
            User[] user = rs.GetStudentListByClassId(ClassID);
            if (user != null && user.Length > 0)
            {
                foreach (User m in user)
                {
                    userList.Append("'" + m.UserID + "',");
                }
                userList.Remove(userList.Length - 1, 1);

                string sql = string.Format(@"SELECT  DISTINCT
                                                    ( CONVERT(NVARCHAR(10), CreateTime, 120) ) AS CreateTime
                                            FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] WHERE UserID IN ({0}) AND VersionID='{1}'
                                            GROUP BY CreateTime", userList, EditionID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json.Append("[");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        json.Append("{\"Time\":\"" + DateTime.Parse(ds.Tables[0].Rows[i]["CreateTime"].ToString()).ToString("yyyy-M-d") + "\"},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                }
            }
            return json.ToString();
        }


        /// <summary>
        /// 根据班级ID获取班级信息
        /// </summary>
        /// <param name="context"></param>
        private void GetClassInfoByClassId(HttpContext context)
        {
            RelationService.RelationService rs = new RelationService.RelationService();
            string ClassID = context.Request.Form["ClassID"];
            string EditionID = context.Request.Form["EditionID"];
            StringBuilder json = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(ClassID))
                {
                    //查询到教师班级列表
                    tb_Class classInfo = rs.GetClassInfoByID(ClassID);
                    if (classInfo != null)
                    {
                        string sql = string.Format(@"SELECT  TextbookVersion ,
                                                            [EditionID] ,
                                                            [JuniorGrade] ,
                                                            [GradeID] ,
                                                            [TeachingBooks] ,
                                                            [BreelID] ,
                                                            [BookID]
                                                    FROM    [TB_CurriculumManage] WHERE EditionID='{0}' AND GradeID='{1}'", EditionID, classInfo.GradeID);

                        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                json.Append("[");
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    json.Append("{\"BookID\":\"" + ds.Tables[0].Rows[i]["BookID"] + "\",\"GradeName\":\"" + ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"] + "\"},");
                                }
                                json.Remove(json.Length - 1, 1);
                                json.Append("]");
                            }
                            var obj = new { Success = true, data = json.ToString() };
                            context.Response.Write(JsonHelper.EncodeJson(obj));
                            context.Response.End();
                        }

                    }
                    else
                    {
                        var obj = new { Success = false, Msg = "无班级数据" };
                        context.Response.Write(JsonHelper.EncodeJson(obj));
                        context.Response.End();
                    }
                }
                else
                {
                    var obj = new { Success = false, Msg = "班级ID为空" };
                    context.Response.Write(JsonHelper.EncodeJson(obj));
                    context.Response.End();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }


        /// <summary>
        /// 获取有学习报告的书籍
        /// </summary>
        /// <param name="ClassID"></param>
        /// <param name="EditionID"></param>
        /// <returns></returns>
        private void GetGradeInfo(HttpContext context)
        {
            string ClassID = context.Request.Form["ClassID"];
            string EditionID = context.Request.Form["EditionID"];
            StringBuilder json = new StringBuilder();
            StringBuilder userList = new StringBuilder();
            RelationService.RelationService rs = new RelationService.RelationService();

            //查询班级下的学生
            User[] user = rs.GetStudentListByClassId(ClassID);
            if (user != null && user.Length > 0)
            {
                foreach (User m in user)
                {
                    userList.Append("'" + m.UserID + "',");
                }
                userList.Remove(userList.Length - 1, 1);

                string sql = string.Format(@"SELECT  a.BookID ,
                                                        a.JuniorGrade ,
                                                        a.TeachingBooks
                                                FROM    ( SELECT  DISTINCT
                                                                    ( BookID )
                                                          FROM      [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                                                          WHERE     UserID IN ({0} )
                                                                    AND BookID IN ( SELECT  BookID
                                                                                    FROM    dbo.TB_CurriculumManage
                                                                                    WHERE   EditionID = '{1}' )
                                                          GROUP BY  BookID
                                                        ) t
                                                        LEFT JOIN dbo.TB_CurriculumManage a ON t.BookID = a.BookID", userList, EditionID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json.Append("[");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        json.Append("{\"BookID\":\"" + ds.Tables[0].Rows[i]["BookID"] + "\",\"GradeName\":\"" + ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"] + "\"},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                }
                var obj = new { Success = true, data = json.ToString() };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }

        /// <summary>
        /// 根据书籍ID获取目录信息
        /// </summary>
        /// <param name="ClassID"></param>
        /// <param name="EditionID"></param>
        /// <returns></returns>
        private void GetModuleInfoByBookID(HttpContext context)
        {
            string ClassID = context.Request.Form["ClassID"]; //所选班级IDs
            string Time = context.Request.Form["Time"]; //版本ID
            string BookID = context.Request.Form["BookID"];
            string Fid = context.Request.Form["Fid"];

            StringBuilder userList = new StringBuilder();
            RelationService.RelationService rs = new RelationService.RelationService();
            if (string.IsNullOrEmpty(Fid))
            {
                var obj = new { Success = false, Msg = "目录ID不能为空！" };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
            string where = "";
            string FirstTitileID = "";

            if (Fid.Split('_').Length > 0)
            {
                where = " b.SecondTitleID=" + Fid.Split('_')[1];
            }
            else
            {
                where = " b.FirstTitileID=" + Fid.Split('_')[0];
            }

            //查询班级下的学生
            User[] user = rs.GetStudentListByClassId(ClassID);
            if (user != null && user.Length > 0)
            {
                foreach (User m in user)
                {
                    userList.Append("'" + m.UserID + "',");
                }
                userList.Remove(userList.Length - 1, 1);

                string strsql = string.Format(@"SELECT  COUNT(DISTINCT
                                                        ( a.UserID ) ) AS stuNum
                                                FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                                        LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDetails] b ON a.BookID = b.BookID
                                                                                           AND b.VideoNumber = a.VideoNumber
                                                WHERE   a.UserID IN ( {0})
                                                        AND a.BookID = '{1}'
                                                        AND a.CreateTime BETWEEN '{2} 00:00:00'
                                                                         AND     '{2} 23:59:59'
                                                        AND a.State = 1 AND {3} ", userList, BookID, Time, where);

                int stuNum = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, strsql));

                var obj = new { Success = true, stuNum = stuNum, classStuNum = userList.Length, };
                context.Response.Write(JsonHelper.EncodeJson(obj));
                context.Response.End();
            }
        }



        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}