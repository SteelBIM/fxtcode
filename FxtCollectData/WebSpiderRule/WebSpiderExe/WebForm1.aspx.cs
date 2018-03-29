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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                for (double i = 105.10369; i < 110.0145; i=i+0.01)
                {
                    for (double j = 28.1369; j < 32.12638; j=j+0.01)
                    {
                        string postData = "maxLevel=3&minLng=" + i.ToString() + "&maxLng=" + (i + 0.01).ToString() + "&minLat=" + j.ToString() + "&maxLat=" + (j + 0.01).ToString(); // 要发放的数据 
                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                        HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create("http://www.17gp.com/Inquiry/GetAreaHouseName/");
                        objWebRequest.Method = "POST";
                        objWebRequest.Host = "www.17gp.com";
                        //objWebRequest.Connection = "keep-alive";
                        objWebRequest.ContentLength = byteArray.Length;
                        objWebRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                        objWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        objWebRequest.Referer = "http://www.17gp.com/Inquiry/Index";
                        Stream newStream = objWebRequest.GetRequestStream();
                        newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
                        newStream.Close();

                        HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
                        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        string content = sr.ReadToEnd();// 返回的数据
                        if (content.Contains("Propertyguid"))
                        {
                            content = content.Substring(10).Replace("\t", "").Replace("\n", "");
                        }
                        else
                        {
                            continue;
                        }


                        //////////////////*******************获取内容*************************///////////////
                        string ProjectInfo = "";
                        string ProjectId = "";
                        string ProjectName = "";
                        string UnitPrice = "";

                        System.Text.StringBuilder reslut = new System.Text.StringBuilder();
                        Regex reg = new Regex("{(?<ProjectInfo>.*?)}");
                        MatchCollection mcs = reg.Matches(content);
                        for (int num = 0; num < mcs.Count; num++)
                        {
                            ProjectInfo = mcs[num].Groups["ProjectInfo"].Value;

                            Regex reg2 = new Regex("Propertyguid\":\"(?<ProjectId>.*?)\",\"Propertyname\":\"(?<ProjectName>.*?)\".*?Price\":\"(?<UnitPrice>.*?)\"");
                            MatchCollection mcs2 = reg2.Matches(ProjectInfo);
                            if (mcs.Count == 1)
                            {
                                ProjectId = mcs2[0].Groups["ProjectId"].Value;
                                ProjectName = mcs2[0].Groups["ProjectName"].Value;
                                UnitPrice = mcs2[0].Groups["UnitPrice"].Value;
                            }

                            var strSql =
                                @"insert into New_Project_House_Price(
                                    CityId, ProjectId, ProjectName, AreaName, UnitPrice, UnitRentPrice, UnitPriceRise, SaleNum, RentNum, SourceName, Address, Url)
                                values(4, '" + ProjectId + "', '" + ProjectName + "', '', '" + UnitPrice + "', '', '', '', '',  '公评网', '', '" + postData + "');";

                            string sqlConnString = "server =192.168.0.5;DataBase = FxtData_Case;User Id = fxtbase_user;Password =base*cn.com;connect timeout=500";
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
                        //////////////////*******************获取内容*************************///////////////


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