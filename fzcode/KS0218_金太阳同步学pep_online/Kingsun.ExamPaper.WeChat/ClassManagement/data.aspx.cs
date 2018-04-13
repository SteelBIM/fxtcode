using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;

namespace Kingsun.ExamPaper.WeChat.ClassManagement
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private BaseManagement bm = new BaseManagement();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        private void DataBind()
        {
            string where = " 1=1 ";

            //            string sql = string.Format(@"SELECT  [ID]
            //                                                ,[OpenID]
            //                                                ,[TelePhone]
            //                                                ,[UserID]
            //                                                ,[CreateDate]
            //                                            FROM [dbo].[TB_UserOpenID] WHERE {0} AND ID BETWEEN {1} AND {2}", where, AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize);

            //            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            IList<TB_UserOpenID> user = bm.SearchAll<TB_UserOpenID>();
            if (user != null)
            {
                DataSet dt = ToDataSet(user);
                AspNetPager1.RecordCount = dt.Tables[0].Rows.Count;
                rpApp.DataSource = dt.Tables[0];
                rpApp.DataBind();
            }
          
        }

        protected void rpApp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "dele")
            {
                string sql = string.Format(@"DELETE FROM [dbo].[TB_UserOpenID] WHERE OpenID='{0}'", e.CommandArgument);
                if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('删除成功！');</script>");
                    DataBind();
                }
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataBind();
        }

        //public static DataSet ToDataSet<T>(IList<T> list)
        //{
        //    return ToDataSet<T>(list);
        //}

        /// <summary>   
        /// 集合装换DataSet   
        /// </summary>   
        /// <param name="list">集合</param>  
        /// <returns></returns>     
        /// 2008-8-1 22:08 HPDV2806   
        public static DataSet ToDataSet(IList<TB_UserOpenID> p_List)
        {
            DataSet result = new DataSet();
            DataTable _DataTable = new DataTable();
            if (p_List.Count > 0)
            {
                PropertyInfo[] propertys = p_List[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    _DataTable.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < p_List.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(p_List[i], null);
                        tempList.Add(obj);
                    }

                    object[] array = tempList.ToArray();
                    _DataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(_DataTable);
            return result;

        }
    }
}