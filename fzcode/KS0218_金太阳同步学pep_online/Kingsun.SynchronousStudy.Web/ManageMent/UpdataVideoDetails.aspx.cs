using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.ManageMent
{
    public partial class UpdataVideoDetails : System.Web.UI.Page
    {
        public string id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request.QueryString["ID"];
            if (!IsPostBack)
            {
                BindDate();
            }
        }

        private void BindDate()
        {
            string sql = string.Format(@"SELECT *  FROM [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] Where id='{0}'", id);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDialogueText.Text = ds.Tables[0].Rows[0]["DialogueText"].ToString();
                txtStartTime.Text = ds.Tables[0].Rows[0]["StartTime"].ToString();
                txtEndTime.Text = ds.Tables[0].Rows[0]["EndTime"].ToString();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtDialogueText.Text.Trim() == "")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('对白不能为空！');</script>");
                return;
            }
            if (txtStartTime.Text.Trim() == "0")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('开始时间不能为空！');</script>");
                return;
            }
            if (txtEndTime.Text.Trim() == "")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('结束时间不能为空！');</script>");
                return;
            }

            string sql = "";

            sql = string.Format(@"UPDATE [FZ_InterestDubbing].[dbo].[TB_VideoDialogue]
                                       SET [DialogueText] = '{0}',[StartTime] = '{1}',[EndTime] = '{2}' WHERE id='{3}'",
                                        txtDialogueText.Text, txtStartTime.Text, txtEndTime.Text, id);


            if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('保存成功！');</script>");
                Response.Redirect("../MessageManagement/MessagePush.aspx", true);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("../ManageMent/VideoDetailsList.aspx", true);
        }
    }
}