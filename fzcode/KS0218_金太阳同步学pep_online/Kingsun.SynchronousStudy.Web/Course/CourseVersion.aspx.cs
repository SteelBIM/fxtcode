using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.AppLibrary.Web.Course
{
    public partial class CourseVersion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = string.Format(@"SELECT ModularID,ModularName FROM dbo.TB_ModularManage WHERE State=1 AND SuperiorID=0");
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
            //中心思想就是将下拉列表的数据源绑定一个表(这里没有对表进行赋值)
            slModuleID.DataSource = dt.DefaultView;
            //设置DropDownList空间显示项对应的字段名,假设表里面有两列,一列绑定下拉列表的Text,另一列绑定Value
            slModuleID.DataValueField = dt.Columns[0].ColumnName;
            slModuleID.DataTextField = dt.Columns[1].ColumnName;
            slModuleID.DataBind();
        }
    }
}