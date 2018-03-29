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
    public partial class 房价网析楼盘 : System.Web.UI.Page
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
                    da.SelectCommand.CommandText = @"select * from (
	                    select ProjectName from New_Project_House_Price with(nolock) where CityID=3 
	                    union all
	                    select left(ProjectName,3) from New_Project_House_Price with(nolock) where CityID=3 
                    )a
                    group by ProjectName";
                
                    conn.Open();
                    da.Fill(ds);
                }
                string ProjectNameList = "";                
                for(int i=0;i<ds.Tables[0].Rows.Count;i++)
                {
                    ProjectNameList = ds.Tables[0].Rows[i][0].ToString();

                    byte[] byteArray = Encoding.UTF8.GetBytes("__ajax=1&keyword=" + ProjectNameList);
                    HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create("http://tj.fangjia.com/autoComplete/suggest?cat=district");
                    objWebRequest.Method = "POST";
                    objWebRequest.Host = "tj.fangjia.com";
                    objWebRequest.ContentLength = byteArray.Length;
                    objWebRequest.Referer = "http://tj.fangjia.com/zoushi/";
                    Stream newStream = objWebRequest.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
                    newStream.Close();

                    HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string content = sr.ReadToEnd();// 返回的数据

                    string ProjectName = "";
                    string AreaName = "";
                    if (content.Contains("price"))
                    {
                        Regex reg = new Regex("\"price\":(?<price>.*?),.*?quality\":\"(?<quality>.*?)\"");
                        MatchCollection mcs = reg.Matches(content);
                        if (mcs.Count >= 1)
                        { 
                            for (int j = 0; j < mcs.Count; j++)
                            {
                                AreaName = ProjectName + mcs[j].Groups["price"].Value + "(" + mcs[j].Groups["quality"].Value + ")";                                
                            }

                            var strSql = @"insert into New_Project_House_Price(
                                            CityId, ProjectId, ProjectName, AreaName, UnitPrice, UnitRentPrice, UnitPriceRise, SaleNum, RentNum, 
                                            SourceName, Address, Url, Remark) values
                                            ('3','','" + ProjectName + "',,'" + AreaName + "','','','','','')";

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

            }
            catch (Exception ee)
            {
                throw ee;
            }

        }
    }
}