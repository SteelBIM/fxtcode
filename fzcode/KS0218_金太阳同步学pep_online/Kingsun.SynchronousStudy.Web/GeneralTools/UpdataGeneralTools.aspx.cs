using Kingsun.SynchronousStudy.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.GeneralTools
{
    public partial class UpdataGeneralTools : Page
    {
        public int BookId;
        public string BookName;
        protected void Page_Load(object sender, EventArgs e)
        {
            BookId = Convert.ToInt32(Request.QueryString["BookID"]);
            if (!IsPostBack)
            {
                //DataBind();
                AspNetPager1.CurrentPageIndex = 1;
            }
        }

        protected void DataBind()
        {
            string where = " 1=1 ";
            SqlParameter[] ps =
            {
                new SqlParameter("@PageIndex",SqlDbType.Int),
                new SqlParameter("@PageCount",SqlDbType.Int),
                new SqlParameter("@Where",SqlDbType.VarChar)
            };
            ps[0].Value = AspNetPager1.CurrentPageIndex;
            ps[1].Value = AspNetPager1.PageSize;
            ps[2].Value = where;

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_TB_VideoDetails", ps);
            AspNetPager1.RecordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            rpModule.DataSource = ds.Tables[0];
            rpModule.DataBind();
        }

        protected void rpModule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "down")
            {
                Response.Redirect("");
            }
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            DataBind();
        }
    }
}