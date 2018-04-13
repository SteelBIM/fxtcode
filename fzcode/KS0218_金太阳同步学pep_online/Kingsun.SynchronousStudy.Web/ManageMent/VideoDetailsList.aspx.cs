using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.ManageMent
{
    public partial class VideoDetailsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(txtDialogueText.Text.Trim()))
            {
                where += " AND a.DialogueText LIKE '%" + String2Json(txtDialogueText.Text.Trim()) + "%'";
            }
            SqlParameter[] ps =
            {
                new SqlParameter("@PageIndex", SqlDbType.Int),
                new SqlParameter("@PageSize", SqlDbType.Int),
                new SqlParameter("@Where", SqlDbType.VarChar)
            };
            ps[0].Value = AspNetPager1.CurrentPageIndex;
            ps[1].Value = AspNetPager1.PageSize;
            ps[2].Value = where;

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_TB_VideoDialogue", ps);
            if (ds.Tables[0].Rows.Count > 0)
            {
                AspNetPager1.RecordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

                rpVideoDialogue.DataSource = ds.Tables[0];
                rpVideoDialogue.DataBind();
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('查询不到匹配的数据！');</script>");
                return;
            }
            
        }

        protected void rpVideoDialogue_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Updata":
                    Response.Redirect("../ManageMent/UpdataVideoDetails.aspx?ID=" + e.CommandArgument, true);
                    break;
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary> 
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '+':
                        sb.Append("\\n"); break;
                    case '\'':
                        sb.Append("\'\'"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
    }
}