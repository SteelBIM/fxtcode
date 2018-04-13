using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo
{
    public partial class LR_ClassLRByUnit : System.Web.UI.Page
    {
        public string provs_data; //级联第一级数据
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            string userId = Request.QueryString["UserID"];//Request.QueryString["UserID"]; // "1028292797"
            string GradeID = Request.QueryString["GradeID"];
            //string bookId = "23_263";
            provs_data = BookInfo(userId, GradeID);

        }

        private static string BookInfo(string userId, string GradeID)
        {
            StringBuilder json = new StringBuilder();
            string sql = string.Format(@"SELECT VersionID FROM dbo.TB_UserEditionInfo WHERE UserID='{0}'", userId);
            string versionId = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql).ToString();
            if (!string.IsNullOrEmpty(versionId))
            {
                sql = string.Format(@"SELECT JuniorGrade,GradeID,TeachingBooks,BreelID,BookID  FROM dbo.TB_CurriculumManage WHERE   EditionID='{0}' AND GradeID='{1}' AND State=1 ORDER BY GradeID,BreelID", versionId, GradeID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json.Append("[");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        json.Append("{\"name\":\"" + ds.Tables[0].Rows[i]["JuniorGrade"] + ds.Tables[0].Rows[i]["TeachingBooks"]
                            + "\",\"id\":\"" + ds.Tables[0].Rows[i]["GradeID"]
                            + ds.Tables[0].Rows[i]["BreelID"] + "_" + ds.Tables[0].Rows[i]["BookID"] + "\"},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                }
            }
            return json.ToString();
        }
    }
}