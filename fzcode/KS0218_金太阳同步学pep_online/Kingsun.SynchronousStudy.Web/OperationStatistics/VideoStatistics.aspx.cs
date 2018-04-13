using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.OperationStatistics
{
    public partial class VideoStatistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVersion();

                DataBind();
            }
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
            string where = " 1=1 And a.BookID IS NOT NULL ";
            if (txtBookName.Text != "")
            {
                where += " AND a.BookName  like '%" + txtBookName.Text + "%' ";
            }
            if (ddlVersion.SelectedItem.Text != "请选择")
            {
                where += " AND a.BookName like '%" + ddlVersion.SelectedItem.Text + "%' ";
            }

            if (txtLoginStart.Value != "" && txtLoginEnd.Value != "")
            {
                where += "  AND b.CreateDate >= '" + txtLoginStart.Value + "' AND b.CreateDate<='" + txtLoginEnd.Value + "' ";
            }
            else if (txtLoginStart.Value != "")
            {
                where += "   AND b.CreateDate >= '" + txtLoginStart.Value + "' ";
            }
            else if (txtLoginEnd.Value != "")
            {
                where += "  AND b.CreateDate<='" + txtLoginEnd.Value + "' ";
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

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_VideoStatistics", ps);

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

            AspNetPager1.CurrentPageIndex = 1;
            DataBind();
        }

        /// <summary>
        /// 转换int型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            int reInt = -1;
            if (obj != null)
                int.TryParse(obj.ToString(), out reInt);
            return reInt;
        }
    }
}