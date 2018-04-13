using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.PSO;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.Web.ApplicationManagement
{
    public partial class APPManagement : System.Web.UI.Page
    {
        public string menuList = "";
        public ClientUserinfo UserInfo = new ClientUserinfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo = CheckLogin.Check(HttpContext.Current, ref menuList);
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        private void DataBind()
        {
            string where = " 1=1 ";
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
                "Get_TB_APPManagement", ps);
            AspNetPager1.RecordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            rpApp.DataSource = ds.Tables[0];
            rpApp.DataBind();
        }


        protected void rpApp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "down")
            {
                Response.Redirect("../ApplicationManagement/ApplicationManage.aspx?VersionID=" + e.CommandArgument);
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;

            if (hfVersionID.Value == "" || hfVersionID.Value == "0")
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('请选择合适的版本！');</script>");
                return;
            }
            sql = string.Format(@"SELECT COUNT(1) FROM dbo.TB_APPManagement WHERE VersionID={0}", hfVersionID.Value);
            if (ParseInt(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('版本已存在！');</script>");
            }
            else
            {
                sql = string.Format(@"INSERT INTO dbo.TB_APPManagement
          (ID, VersionName , VersionID ,CreatePerson)  VALUES  ('{3}','{0}',{1},'{2}')", hfVersionName.Value, hfVersionID.Value, UserInfo.UserName, Guid.NewGuid());

                int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);

                if (i > 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('添加成功！');CloseDiv('MyDiv','fade');</script>");
                    DataBind();
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('添加失败！');</script>");
                }
            }

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