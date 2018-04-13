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

namespace Kingsun.SynchronousStudy.Web.MessageManagement
{
    public partial class MessagePush : System.Web.UI.Page
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
            if (txtTitle.Text.Trim() != "")
            {
                where += "   AND MessageTitle >= '" + String2Json(txtTitle.Text.Trim()) + "' ";
            }
            if (txtStartDate.Value != "" && txtEndDate.Value != "")
            {
                where += "  AND CreateDate >= '" + String2Json(txtStartDate.Value) + "' AND CreateDate<='" + String2Json(txtEndDate.Value) + "' ";
            }
            else if (txtStartDate.Value != "")
            {
                where += "   AND CreateDate >= '" + String2Json(txtStartDate.Value) + "' ";
            }
            else if (txtEndDate.Value != "")
            {
                where += "  AND CreateDate<='" + String2Json(txtEndDate.Value) + "' ";
            }

            if (txtLoginStart.Value != "" && txtLoginEnd.Value != "")
            {
                where += "  AND EndTime >= '" + String2Json(txtLoginStart.Value) + "' AND EndTime<='" + String2Json(txtLoginEnd.Value) + "' ";
            }
            else if (txtStartDate.Value != "")
            {
                where += "   AND EndTime >= '" + String2Json(txtLoginStart.Value) + "' ";
            }
            else if (txtEndDate.Value != "")
            {
                where += "  AND EndTime<='" + String2Json(txtLoginEnd.Value) + "' ";
            }

            SqlParameter[] ps =
            {
                new SqlParameter("@PageIndex", SqlDbType.Int),
                new SqlParameter("@PageCount", SqlDbType.Int),
                new SqlParameter("@Where", SqlDbType.VarChar)
            };
            ps[0].Value = AspNetPager1.CurrentPageIndex;
            ps[1].Value = AspNetPager1.PageSize;
            ps[2].Value = where;

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_TB_MessagePush", ps);

            if (ds.Tables[0].Rows.Count > 0)
            {
                AspNetPager1.RecordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

                rpMessagePush.DataSource = ds.Tables[0];
                rpMessagePush.DataBind();
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('查询不到匹配的数据！');</script>");
                return;
            }
        }

        protected void rpMessagePush_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Serach":
                    Response.Redirect("../MessageManagement/AddMessagePush.aspx?ID=" + e.CommandArgument + "&Type=Serach", true);
                    break;
                case "Updata":
                    Response.Redirect("../MessageManagement/AddMessagePush.aspx?ID=" + e.CommandArgument + "&Type=Updata", true);
                    break;
                case "Delete":
                    string sql = string.Format(@"DELETE dbo.TB_MessagePush WHERE ID='{0}'", e.CommandArgument);
                    if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('删除成功！');</script>");
                        BindData();
                    }
                    break;
                case "State":
                    UpdateState(e);
                    break;
            }
        }

        private void UpdateState(RepeaterCommandEventArgs e)
        {
            string[] s = e.CommandArgument.ToString().Split(',');
            string state = "0";
            if (s[1] == "0")
            {
                state = "1";
            }
            string sql = string.Format(@"  UPDATE dbo.TB_MessagePush SET State={1} WHERE ID={0}", s[0], state);
            if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
            {
                if (s[1] == "0")
                {
                    ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('审核成功！');</script>");
                    BindData();
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('禁用成功！');</script>");
                    BindData();
                }

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