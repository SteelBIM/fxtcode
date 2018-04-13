using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.Common;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo
{
    public partial class LrStuStudyInfo : System.Web.UI.Page
    {


        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        public string UserId;
        public string BookId;
        public string FTitleId;
        public string Classid;
        public string ImgUrl;
        public string FilesUrl = WebConfigurationManager.AppSettings["getFiles"];

        protected void Page_Load(object sender, EventArgs e)
        {
            UserId = Request.QueryString["UserID"];
            FTitleId = Request.QueryString["fTitleId"];
            BookId = Request.QueryString["BookID"];
            Classid = Request.QueryString["ClassID"];

            if (!IsPostBack)
            {
                DataBind();
            }

        }

        public override void DataBind()
        {
            string fId = "";
            string sId = "";
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(FTitleId))
            {
                if (FTitleId.Split('_').Length > 0)
                {
                    fId = FTitleId.Split('_')[0];
                    sId = FTitleId.Split('_')[1];
                }
                else
                {
                    fId = FTitleId;
                }
            }

            string where = " 1=1 AND a.BookID IS NOT NULL  ";
            if (!string.IsNullOrEmpty(Classid))
            {
                var classinfo = classBLL.GetClassUserRelationByClassId(Classid);
                if (classinfo != null)
                {
                    where += " AND b.ClassShortID = '" + classinfo.ClassNum + "' ";
                }

            }

            if (!string.IsNullOrEmpty(UserId))
            {
                where += " AND a.UserID = '" + UserId + "' ";
            }

            if (!string.IsNullOrEmpty(BookId))
            {
                where += " AND a.BookID = '" + BookId + "' ";

                if (string.IsNullOrEmpty(FTitleId))
                {
                    string sql = @"SELECT FirstTitleID,SecondTitleID FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE BookID=" + BookId +
                                 " ORDER BY SecondTitleID asc";
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        fId = ds.Tables[0].Rows[0]["FirstTitleID"].ToString();
                        sId = ds.Tables[0].Rows[0]["SecondTitleID"].ToString();
                    }
                }
            }

            string strSql = string.Format(@"SELECT BookName,FirstTitle,SecondTitle FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE  FirstTitleID='{0}' AND SecondTitleID='{1}'", fId, sId);
            DataSet dts = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);

            if (dts.Tables[0].Rows.Count > 0)
            {
                string fTitle = dts.Tables[0].Rows[0]["FirstTitle"].ToString().Length > 9 ? dts.Tables[0].Rows[0]["FirstTitle"].ToString().Substring(0, 9) + "..." : dts.Tables[0].Rows[0]["FirstTitle"].ToString();
                string sTitle = dts.Tables[0].Rows[0]["SecondTitle"].ToString().Length > 9 ? dts.Tables[0].Rows[0]["SecondTitle"].ToString().Substring(0, 9) + "..." : dts.Tables[0].Rows[0]["SecondTitle"].ToString();

                ModuleName.InnerText = fTitle + "/" + sTitle;
                string jg = dts.Tables[0].Rows[0]["BookName"].ToString();

                JuniorGrade.InnerText = jg.Substring(jg.Length - 5);
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

            DataSet newds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_TB_WechatStuVideoInfo", ps);

            if (newds.Tables[0].Rows.Count > 0)
            {
                string name =
                        (newds.Tables[0].Rows[0]["TrueName"].ToString() == ""
                            ? newds.Tables[0].Rows[0]["UserName"].ToString()
                            : newds.Tables[0].Rows[0]["TrueName"].ToString()) == ""
                            ? newds.Tables[0].Rows[0]["NickName"].ToString()
                            : (newds.Tables[0].Rows[0]["TrueName"].ToString() == ""
                                ? newds.Tables[0].Rows[0]["UserName"].ToString()
                                : newds.Tables[0].Rows[0]["TrueName"].ToString());

                if (newds.Tables[0].Rows[0]["UserImage"].ToString() != "" && newds.Tables[0].Rows[0]["UserImage"].ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    ImgUrl = FilesUrl + "?FileID=" + newds.Tables[0].Rows[0]["UserImage"] + "&view=true";
                }
                else
                {
                    ImgUrl = "../images/defaultImg.png";
                }
                //userImg1.ImageUrl = Newds.Tables[0].Rows[0]["UserImage"].ToString() == "00000000-0000-0000-0000-000000000000" ? "../images/defaultImg.png" : FilesURL + "?FileID=" + Newds.Tables[0].Rows[0]["UserImage"] + "&view=true";
                // ImgUrl = Newds.Tables[0].Rows[0]["UserImage"].ToString() == "00000000-0000-0000-0000-000000000000" ? "../images/defaultImg.png" : FilesURL + "?FileID=" + Newds.Tables[0].Rows[0]["UserImage"] + "&view=true";
                UserName.InnerText = name;
            }

            sb.Append("<ul>");

            for (int i = 0; i < newds.Tables[0].Rows.Count; i++)
            {
                //string userImg = FilesURL + "?FileID=" + Newds.Tables[0].Rows[i]["UserImage"] + "&view=true";

                if (i == 0)
                {
                    sb.Append("<li><span>" + newds.Tables[0].Rows[i]["FirstModular"] + "</span><b>" + newds.Tables[0].Rows[i]["VideoTitle"]);
                    sb.Append("<p class=\"ci_num ci_num1\">" + newds.Tables[0].Rows[i]["TotalScore"] + "分<a class=\"audio\" href=\"../../../Share.aspx?userID=" + newds.Tables[0].Rows[i]["UserID"] + "&VideoFileID=" + newds.Tables[0].Rows[i]["VideoFileID"] + "\"></a></p></b>");
                }
                else
                {
                    if (newds.Tables[0].Rows[i - 1]["UserID"].ToString() == newds.Tables[0].Rows[i]["UserID"].ToString())
                    {
                        if (newds.Tables[0].Rows[i - 1]["VideoNumber"].ToString() != newds.Tables[0].Rows[i]["VideoNumber"].ToString())
                        {
                            sb.Append("<b>" + newds.Tables[0].Rows[i]["VideoTitle"] + "<p class=\"ci_num ci_num1\">" + newds.Tables[0].Rows[i]["TotalScore"] + "分<a class=\"audio\"  href=\"../../../Share.aspx?userID=" + newds.Tables[0].Rows[i]["UserID"] + "&VideoFileID=" + newds.Tables[0].Rows[i]["VideoFileID"] + "\"></a></p></b>");
                        }
                    }
                    else
                    {
                        sb.Append("</li>");
                        sb.Append("<li><span>" + newds.Tables[0].Rows[i]["FirstModular"] + "</span><b>" + newds.Tables[0].Rows[i]["VideoTitle"]);
                        sb.Append("<p class=\"ci_num ci_num1\">" + newds.Tables[0].Rows[i]["TotalScore"] + "分<a class=\"audio\" href=\"../../../Share.aspx?userID=" + newds.Tables[0].Rows[i]["UserID"] + "&VideoFileID=" + newds.Tables[0].Rows[i]["VideoFileID"] + "\"></a></p></b>");
                    }
                }
            }
            sb.Append("</li>");
            sb.Append("</ul>");

            content.InnerHtml = sb.ToString();
        }
    }
}