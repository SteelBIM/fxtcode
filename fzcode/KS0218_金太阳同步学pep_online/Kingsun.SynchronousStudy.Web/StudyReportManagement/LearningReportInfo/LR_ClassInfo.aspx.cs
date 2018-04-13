using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.Web.RelationService;
using log4net;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo
{
    public partial class LR_ClassInfo : System.Web.UI.Page
    {
        public string provs_data;
        public string userId;
        public string ClassLongID;
        public string html;
        public static string classShortId;
        public string bookId;
        public string FilesURL = WebConfigurationManager.AppSettings["getFiles"];
        public string vid = "";
        public string Time;
        public string GradeID;
        public string bfId;
        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            userId = Request.QueryString["UserID"]; //"1695290833";//Request.QueryString["UserID"];//"1695290833";//
            bookId = Request.QueryString["BookID"];//"168";//Request.QueryString["BookID"];
            ClassLongID = Request.QueryString["ClassID"];
            Time = Request.QueryString["Times"];
            GradeID = Request.QueryString["GradeID"];
            bfId = Request.QueryString["fTitleId"];//年级与模块ID 用'|'分割
            //classShortId = "10091075";

            provs_data = BookInfo(userId);
            GetClassInfoByUserId();
        }

        private string BookInfo(string userId)
        {
            StringBuilder json = new StringBuilder();
            try
            {
                string sql = string.Format(@"SELECT VersionID FROM dbo.TB_UserEditionInfo WHERE UserID='{0}'", userId);
                DataSet data = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (data.Tables[0].Rows.Count > 0)
                {
                    vid = data.Tables[0].Rows[0]["VersionID"].ToString();
                    sql = string.Format(@"SELECT JuniorGrade,GradeID,TeachingBooks,BreelID,BookID  FROM dbo.TB_CurriculumManage WHERE   EditionID='{0}' AND GradeID='{1}' AND State=1 ORDER BY GradeID,BreelID", data.Tables[0].Rows[0]["VersionID"].ToString(), GradeID);
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        json.Append("[");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            json.Append("{\"name\":\"" + ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"] + "\",\"id\":\"" + ds.Tables[0].Rows[i]["GradeID"] + ds.Tables[0].Rows[i]["BreelID"] + "_" + ds.Tables[0].Rows[i]["BookID"] + "\"},");
                        }
                        json.Remove(json.Length - 1, 1);
                        json.Append("]");
                        BID.InnerText = ds.Tables[0].Rows[0]["BookID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }

            return json.ToString();
        }

        public void GetClassInfoByUserId()
        {
            try
            {
                RelationService.RelationService rl = new RelationService.RelationService();
                tb_Class tc = rl.GetClassInfoByID(ClassLongID);
                BulkCopyClassList(tc, rl);
                if (tc != null)
                {
                    classShortId = tc.ClassNum;
                }
                //User[] us = rl.GetStudentListByClassId(ClassID);
                DataBind(classShortId);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }

        public void DataBind(string classid)
        {
            string fId = "";
            string sId = "";
            StringBuilder sb = new StringBuilder();

            try
            {
                int num = 1;
                string where = string.Format(" 1=1 AND a.BookID IS NOT NULL AND e.VersionID<>0 AND e.VersionID='{0}' AND e.State=1  ", vid);
                if (!string.IsNullOrEmpty(classid))
                {
                    where += " AND b.ClassShortID = '" + classid + "' ";
                }
                if (!string.IsNullOrEmpty(bookId))
                {
                    where += " AND a.BookID = '" + bookId + "' ";

                    if (string.IsNullOrEmpty(fId))
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
                    STitleID.InnerText = sId;
                    FTitleID.InnerText = fId;
                    where += " AND c.SecondTitleID = '" + sId + "' ";
                }
                else if (!string.IsNullOrEmpty(fId))
                {
                    FTitleID.InnerText = fId;
                    where += " AND c.FirstTitleID = '" + fId + "' ";
                }


                SqlParameter[] ps =
                {
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageCount", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar)
                };
                ps[0].Value = 1;//AspNetPager1.CurrentPageIndex;
                ps[1].Value = 50000;//AspNetPager1.PageSize;
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



                    string userImg = Newds.Tables[0].Rows[i]["UserImage"].ToString() == "00000000-0000-0000-0000-000000000000" ? "../images/defaultImg.png" : FilesURL + "?FileID=" + Newds.Tables[0].Rows[i]["UserImage"] + "&view=true";

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
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            content.InnerHtml = sb.ToString();
        }

        /// <summary>
        /// 从SNS上获取班级学生信息。批量复制到本地数据表
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="rl"></param>
        private void BulkCopyClassList(tb_Class tc, RelationService.RelationService rl)
        {
            try
            {
                if (tc == null)
                {
                    //ClientScript.RegisterStartupScript(GetType(), "tishi",
                    //    "<script type=\"text/javascript\">alert('数据为空！');</script>");
                    return;
                }

                User[] user = rl.GetStuListByClassNum(tc.ClassNum);

                DataTable dtList = new DataTable();
                dtList.Columns.Add("ID");
                dtList.Columns.Add("ClassLongID", typeof(System.Data.SqlTypes.SqlGuid));
                dtList.Columns.Add("ClassShortID");
                dtList.Columns.Add("ClassName");
                dtList.Columns.Add("UserID");
                dtList.Columns.Add("CreateDate");
                dtList.Columns.Add("TrueName");
                dtList.Columns.Add("UserName");

                if (user == null)
                {
                    //ClientScript.RegisterStartupScript(GetType(), "tishi",
                    //    "<script type=\"text/javascript\">alert('用户数据为空');</script>");
                    return;
                }

                foreach (var item in user)
                {
                    DataRow dr = dtList.NewRow();
                    dr["ClassLongID"] = new Guid(tc.ID);
                    dr["ClassShortID"] = tc.ClassNum;
                    dr["ClassName"] = tc.ClassName;
                    dr["UserID"] = item.UserID;
                    dr["TrueName"] = item.TrueName;
                    dr["UserName"] = item.UserName;
                    dtList.Rows.Add(dr);
                }

                SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
                {
                    BulkCopyTimeout = 5000,
                    NotifyAfter = dtList.Rows.Count,
                };

                string strSql = "DELETE FROM dbo.[TB_UserClassRelation] WHERE ClassShortID=" + Convert.ToInt32(tc.ClassNum);
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

                try
                {
                    sbc.DestinationTableName = "TB_UserClassRelation";
                    sbc.WriteToServer(dtList); //此处报错
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(GetType(), "tishi",
                    //    "<script type=\"text/javascript\">alert('错误：" + ex.Message + "');</script>");
                }

                if (sbc.NotifyAfter <= 0)
                {
                    //ClientScript.RegisterStartupScript(GetType(), "tishi",
                    //    "<script type=\"text/javascript\">alert('插入" + sbc.NotifyAfter + "条数据！');</script>");
                }
                else
                {
                    //ClientScript.RegisterStartupScript(GetType(), "tishi",
                    //    "<script type=\"text/javascript\">alert('导入成功！');</script>");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataBind(classShortId);
        }
    }
}