using Kingsun.SynchronousStudy.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.UserManagement
{
    public partial class UserStatisticsInfo : System.Web.UI.Page
    {
        public int UserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserId = Convert.ToInt32(Request.QueryString["UserId"]);
            if (!IsPostBack)
            {
                BindModular();

                BindVersion();

                DataBind();
            }
        }

        private void BindModular()
        {
            string sql = @"SELECT DISTINCT
                                [ModuleID] ,
                                [ModuleName]
                        FROM    [TB_ModuleSort]
                        WHERE   SuperiorID = FirstTitleID
                                OR SuperiorID = SecondTitleID";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            ddlModular.DataSource = ds.Tables[0];
            ddlModular.DataValueField = ds.Tables[0].Columns[0].ColumnName;
            ddlModular.DataTextField = ds.Tables[0].Columns[1].ColumnName;
            ddlModular.DataBind();
            ddlModular.Items.Insert(0, new ListItem("请选择", "0"));
        }

        private void BindVersion()
        {
            string sql = @"SELECT [VersionID]
                                  ,[VersionName]
                              FROM [TB_APPManagement]";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            ddlVersion.DataSource = ds.Tables[0];
            ddlVersion.DataValueField = ds.Tables[0].Columns[0].ColumnName;
            ddlVersion.DataTextField = ds.Tables[0].Columns[1].ColumnName;
            ddlVersion.DataBind();
            ddlVersion.Items.Insert(0, new ListItem("请选择", "0"));
        }

        public void DataBind()
        {
            string where = " 1=1 ";
            if (UserId > 0)
            {
                where += " AND a.UserID ='" + UserId + "'";
            }
            if (ddlModular.SelectedValue != "0")
            {
                where += " AND b.FirstModular like '%" + ddlModular.SelectedItem.Text + "%'";
            }
            if (ddlVersion.SelectedValue != "0")
            {
                where += " AND b.BookName like '%" + ddlVersion.SelectedItem.Text + "%' ";
            }
            if (txtStartDate.Value != "" && txtEndDate.Value != "")
            {
                where += " AND a.CreateTime >= '" + txtStartDate.Value + "' AND a.CreateTime<='" + txtEndDate.Value + "' ";
            }
            else if (txtStartDate.Value != "")
            {
                where += "   AND a.CreateTime >= '" + txtStartDate.Value + "' ";
            }
            else if (txtEndDate.Value != "")
            {
                where += "  AND a.CreateTime<='" + txtEndDate.Value + "' ";
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

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure,
                "Get_UserStatisticsInfo", ps);
            AspNetPager1.RecordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

            rpStatistics.DataSource = ds.Tables[0];
            rpStatistics.DataBind();

        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataBind();
        }
    }
}