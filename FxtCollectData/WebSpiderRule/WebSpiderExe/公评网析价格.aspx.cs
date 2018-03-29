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
    public partial class 公评网析价格 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sqlConnString = "server =192.168.0.5;DataBase = FxtData_Case;User Id = fxtbase_user;Password =base*cn.com;connect timeout=500";
                DataSet  ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(sqlConnString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.Connection = conn;
                    da.SelectCommand.CommandText = "select ProjectId from New_Project_House_Price WITH(NOLOCK) where SourceName='公评网' and (Remark is null or Remark='') and CityId=3  group by ProjectId ";
                    //da.SelectCommand.CommandType = commandType;
                    //if (para != null)
                    //{
                    //    da.SelectCommand.Parameters.AddRange(para);
                    //}                    
                    conn.Open();
                    da.Fill(ds);
                }
                string ProjectId = "";                
                for(int i=0;i<ds.Tables[0].Rows.Count;i++)
                {
                    ProjectId = ds.Tables[0].Rows[i][0].ToString();

                    byte[] byteArray = Encoding.UTF8.GetBytes("houseGuid=" + ProjectId);
                    HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create("http://wx.17gp.com/Inquiry/QueryHouseByGuid");
                    objWebRequest.Method = "POST";
                    objWebRequest.Host = "www.17gp.com";
                    //objWebRequest.Connection = "keep-alive";
                    objWebRequest.ContentLength = byteArray.Length;
                    objWebRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                    objWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    objWebRequest.Referer = "http://wx.17gp.com/Inquiry/Details?houseGuid=" + ProjectId;
                    Stream newStream = objWebRequest.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
                    newStream.Close();

                    HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string content = sr.ReadToEnd();// 返回的数据

                    Response.Write(content);
                    return;

                    string Remark = "";
                    if (content.Contains("price"))
                    {
                        Regex reg = new Regex("\"price\":(?<price>.*?),.*?quality\":\"(?<quality>.*?)\"");
                        MatchCollection mcs = reg.Matches(content);
                        if (mcs.Count >= 1)
                        { 
                            for (int j = 0; j < mcs.Count; j++)
                            {
                                Remark = Remark+ mcs[j].Groups["price"].Value + "(" + mcs[j].Groups["quality"].Value + ")";                                
                            }

                            var strSql = @"update New_Project_House_Price set Remark='" + Remark + "' where ProjectId='" + ProjectId + "'";

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
                        }

                    }
                    else
                    {
                        continue;
                    }
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