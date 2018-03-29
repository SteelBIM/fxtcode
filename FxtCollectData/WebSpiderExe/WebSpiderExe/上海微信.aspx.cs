using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSpiderExe
{
    public partial class 上海微信 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sqlConnString = "server =192.168.0.5;DataBase = FxtData_Case;User Id = fxtbase_user;Password =base*cn.com;connect timeout=500";
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(sqlConnString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.Connection = conn;
                    da.SelectCommand.CommandText = "select ProjectId from New_Project_House_Price WITH(NOLOCK) where SourceName='微信'and UnitPrice='' order by Id ";
                    //da.SelectCommand.CommandType = commandType;
                    //if (para != null)
                    //{
                    //    da.SelectCommand.Parameters.AddRange(para);
                    //}                    
                    conn.Open();
                    da.Fill(ds);
                }
                string ProjectId = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ProjectId = ds.Tables[0].Rows[i][0].ToString();

                    string strURL = "http://wx.surea.com/api/Viss/GetProjectInfo?projectId=" + ProjectId;
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                    
                    if (i % 2 == 0)
                    {
                        request.Headers.Add("Cookie", "ASP.NET_SessionId=zjxiwksxy2l3hgpv4gfqq4at");
                    }
                    else
                    {
                        request.Headers.Add("Cookie", "ASP.NET_SessionId=olpimau1b5pno3024ghv0dzh");
                    }
                    
                    request.Method = "get";
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                    System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string content = myreader.ReadToEnd();

                    string UnitPrice = "";
                    if (content.Contains("ProjectPrice"))
                    {
                        Regex reg = new Regex("ProjectPrice\":(?<UnitPrice>.*?),");
                        MatchCollection mcs = reg.Matches(content);
                        if (mcs.Count >= 1)
                        {
                            UnitPrice = mcs[0].Groups["UnitPrice"].Value;
                        }                        
                    }
                    else
                    {
                        Response.Write(content);
                        return;
                    }

                    var strSql = @"update New_Project_House_Price set UnitPrice='" + UnitPrice + "' where ProjectId='" + ProjectId + "'";

                    using (SqlConnection conn = new SqlConnection(sqlConnString))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = conn;
                        try
                        {
                            command.CommandText = strSql;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    System.Threading.Thread.Sleep(10 * 1000); 
                }

                Response.Write("结束");

            }
            catch (Exception ee)
            {
                throw ee;
            }

        }
    }
}