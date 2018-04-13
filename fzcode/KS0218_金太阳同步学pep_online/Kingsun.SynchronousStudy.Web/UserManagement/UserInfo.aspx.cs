using System;
using System.Collections.Generic;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Kingsun.IBS.Model;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.Web.UserManagement
{
    public partial class UserInfo : System.Web.UI.Page
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }

            SqlDataAdapter sqldar = new SqlDataAdapter("SELECT ID,VersionName,VersionID FROM dbo.TB_APPManagement", SqlHelper.ConnectionString);
            sqldar.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            sqldar.Fill(ds, "Users"); 
            ddlVersion.DataSource = ds.Tables["Users"].DefaultView;
            ddlVersion.DataTextField = "VersionName";
            ddlVersion.DataValueField = "ID";
            ddlVersion.DataBind();
            ddlVersion.Items.Insert(0, new ListItem("-----所有数据-----", "0"));
            ddlVersion.Items.Insert(1, new ListItem("-----全部版本-----", "1"));
        }

        private void InitAction(string action)
        {
            switch (action)
            {
                case "queryuserlist":
                    int totalcount = 0;
                    IList<Tb_UserInfo> userlist = new List<Tb_UserInfo>();
                    if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                    {
                        var obj1 = new { rows = userlist, total = totalcount };
                        Response.Write(JsonHelper.EncodeJson(obj1));
                        Response.End();
                    }
                    int pageindex = int.Parse(Request.Form["page"]);
                    int pagesize = int.Parse(Request.Form["rows"]);
                    string where;
                    string orderBy="";
                    if (string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        orderBy = "CreateTime DESC";
                    }
                    else
                    {
                        orderBy = Request.QueryString["queryStr"] + "Desc";
                    }
                    userlist = userBLL.GetUserList(null,0,orderBy);
                    if (userlist == null)
                    {
                        userlist = new List<Tb_UserInfo>();
                    }
                    else
                    {
                        totalcount = userlist.Count;
                        IList<Tb_UserInfo> removelist = new List<Tb_UserInfo>();
                        for (int i = 0; i < userlist.Count; i++)
                        {
                            if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                            {
                                removelist.Add(userlist[i]);
                            }
                        }
                        if (removelist != null && removelist.Count > 0)
                        {
                            for (int i = 0; i < removelist.Count; i++)
                            {
                                userlist.Remove(removelist[i]);
                            }
                        }
                    }
                    var obj = new { rows = userlist, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                default:
                    break;
            }
        }
    }
}