using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSpiderExe
{
    public partial class 中国土地市场 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string URL="";

                string 行政区 = "";
                string 项目名称 = "";
                string 项目位置 = "";
                string 面积 = "";
                string 成交价格 = "";
                string 上限 = "";
                string 下限 = "";
                string 合同签订日期 = "";

                for (int i = 0; i < 1000; i++)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes("__VIEWSTATE=%2FwEPDwUJNjkzNzgyNTU4D2QWAmYPZBYIZg9kFgICAQ9kFgJmDxYCHgdWaXNpYmxlaGQCAQ9kFgICAQ8WAh4Fc3R5bGUFIEJBQ0tHUk9VTkQtQ09MT1I6I2YzZjVmNztDT0xPUjo7ZAICD2QWAgIBD2QWAmYPZBYCZg9kFgJmD2QWBGYPZBYCZg9kFgJmD2QWAmYPZBYCZg9kFgJmDxYEHwEFIENPTE9SOiNEM0QzRDM7QkFDS0dST1VORC1DT0xPUjo7HwBoFgJmD2QWAgIBD2QWAmYPDxYCHgRUZXh0ZWRkAgEPZBYCZg9kFgJmD2QWAmYPZBYEZg9kFgJmDxYEHwEFhwFDT0xPUjojRDNEM0QzO0JBQ0tHUk9VTkQtQ09MT1I6O0JBQ0tHUk9VTkQtSU1BR0U6dXJsKGh0dHA6Ly93d3cubGFuZGNoaW5hLmNvbS9Vc2VyL2RlZmF1bHQvVXBsb2FkL3N5c0ZyYW1lSW1nL3hfdGRzY3dfc3lfamhnZ18wMDAuZ2lmKTseBmhlaWdodAUBMxYCZg9kFgICAQ9kFgJmDw8WAh8CZWRkAgIPZBYCZg9kFgJmD2QWAmYPZBYCZg9kFgJmD2QWAmYPZBYEZg9kFgJmDxYEHwEFIENPTE9SOiNEM0QzRDM7QkFDS0dST1VORC1DT0xPUjo7HwBoFgJmD2QWAgIBD2QWAmYPDxYCHwJlZGQCAg9kFgJmD2QWBGYPZBYCZg9kFgJmD2QWAmYPZBYCZg9kFgJmD2QWAmYPFgQfAQUgQ09MT1I6I0QzRDNEMztCQUNLR1JPVU5ELUNPTE9SOjsfAGgWAmYPZBYCAgEPZBYCZg8PFgIfAmVkZAICD2QWBGYPZBYCZg9kFgJmD2QWAmYPZBYCAgEPZBYCZg8WBB8BBYYBQ09MT1I6I0QzRDNEMztCQUNLR1JPVU5ELUNPTE9SOjtCQUNLR1JPVU5ELUlNQUdFOnVybChodHRwOi8vd3d3LmxhbmRjaGluYS5jb20vVXNlci9kZWZhdWx0L1VwbG9hZC9zeXNGcmFtZUltZy94X3Rkc2N3X3p5X2pnZ2dfMDEuZ2lmKTsfAwUCNDYWAmYPZBYCAgEPZBYCZg8PFgIfAmVkZAIBD2QWAmYPZBYCZg9kFgJmD2QWAgIBD2QWAmYPFgQfAQUgQ09MT1I6I0QzRDNEMztCQUNLR1JPVU5ELUNPTE9SOjsfAGgWAmYPZBYCAgEPZBYCZg8PFgIfAmVkZAIDD2QWAgIDDxYEHglpbm5lcmh0bWwF%2FQY8cCBhbGlnbj0iY2VudGVyIj48c3BhbiBzdHlsZT0iZm9udC1zaXplOiB4LXNtYWxsIj4mbmJzcDs8YnIgLz4NCiZuYnNwOzxhIHRhcmdldD0iX3NlbGYiIGhyZWY9Imh0dHA6Ly93d3cubGFuZGNoaW5hLmNvbS8iPjxpbWcgYm9yZGVyPSIwIiBhbHQ9IiIgd2lkdGg9IjI2MCIgaGVpZ2h0PSI2MSIgc3JjPSIvVXNlci9kZWZhdWx0L1VwbG9hZC9mY2svaW1hZ2UvdGRzY3dfbG9nZS5wbmciIC8%2BPC9hPiZuYnNwOzxiciAvPg0KJm5ic3A7PHNwYW4gc3R5bGU9ImNvbG9yOiAjZmZmZmZmIj5Db3B5cmlnaHQgMjAwOC0yMDE0IERSQ25ldC4gQWxsIFJpZ2h0cyBSZXNlcnZlZCZuYnNwOyZuYnNwOyZuYnNwOyA8c2NyaXB0IHR5cGU9InRleHQvamF2YXNjcmlwdCI%2BDQp2YXIgX2JkaG1Qcm90b2NvbCA9ICgoImh0dHBzOiIgPT0gZG9jdW1lbnQubG9jYXRpb24ucHJvdG9jb2wpID8gIiBodHRwczovLyIgOiAiIGh0dHA6Ly8iKTsNCmRvY3VtZW50LndyaXRlKHVuZXNjYXBlKCIlM0NzY3JpcHQgc3JjPSciICsgX2JkaG1Qcm90b2NvbCArICJobS5iYWlkdS5jb20vaC5qcyUzRjgzODUzODU5YzcyNDdjNWIwM2I1Mjc4OTQ2MjJkM2ZhJyB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnJTNFJTNDL3NjcmlwdCUzRSIpKTsNCjwvc2NyaXB0PiZuYnNwOzxiciAvPg0K54mI5p2D5omA5pyJJm5ic3A7IOS4reWbveWcn%2BWcsOW4guWcuue9kSZuYnNwOyZuYnNwO%2BaKgOacr%2BaUr%2BaMgTrmtZnmsZ%2Foh7vlloTnp5HmioDmnInpmZDlhazlj7gmbmJzcDvkupHlnLDnvZE8YnIgLz4NCuWkh%2BahiOWPtzog5LqsSUNQ5aSHMDkwNzQ5OTLlj7cg5Lqs5YWs572R5a6J5aSHMTEwMTAyMDAwNjY2KDIpJm5ic3A7PGJyIC8%2BDQo8L3NwYW4%2BJm5ic3A7Jm5ic3A7Jm5ic3A7PGJyIC8%2BDQombmJzcDs8L3NwYW4%2BPC9wPh8BBWRCQUNLR1JPVU5ELUlNQUdFOnVybChodHRwOi8vd3d3LmxhbmRjaGluYS5jb20vVXNlci9kZWZhdWx0L1VwbG9hZC9zeXNGcmFtZUltZy94X3Rkc2N3MjAxM195d18xLmpwZyk7ZGSSWU%2FsPKNSytLnkS4icjnGqQCw6EfSi13%2B6v2DEyVusQ%3D%3D&__EVENTVALIDATION=%2FwEWAgK51vnrAgLN3cj%2FBG6upSbr8AvbHjCxfLVlkQpcqjWfGNfu2hITfByVCPcl&hidComName=default&TAB_QuerySubmitConditionData=&TAB_QuerySubmitOrderData=&TAB_RowButtonActionControl=&TAB_QuerySubmitPagerData=" + i + "&TAB_QuerySubmitSortData=");
                    HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create("http://www.landchina.com/default.aspx?tabid=263&ComName=default");
                    objWebRequest.Method = "POST";
                    objWebRequest.Host = "www.landchina.com";
                    //objWebRequest.Connection = "keep-alive";
                    objWebRequest.ContentLength = byteArray.Length;
                    objWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    objWebRequest.ContentType = "application/x-www-form-urlencoded";
                    objWebRequest.Referer = "http://www.landchina.com/default.aspx?tabid=263&ComName=default";
                    Stream newStream = objWebRequest.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
                    newStream.Close();

                    HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                    string content = sr.ReadToEnd().Replace("\t", "").Replace("\n", "").Replace("\r", "");// 返回的数据

                    Regex reg = new Regex("href=\"default.aspx(?<URL>.*?)\"");
                    MatchCollection mcs = reg.Matches(content);
                    if (mcs.Count >= 1)
                    {
                        for (int j = 0; j < mcs.Count; j++)
                        {
                            URL = "http://www.landchina.com/default.aspx" + mcs[j].Groups["URL"].Value;

                            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(URL);
                            request.Method = "get";
                            response = (System.Net.HttpWebResponse)request.GetResponse();
                            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.Default);
                            content = myreader.ReadToEnd().Replace("\t", "").Replace("\n", "");

                            Regex reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f1_r1_c2_ctrl\">(?<行政区>.*?)</span>");
                            MatchCollection mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                行政区 = mcs2[j].Groups["行政区"].Value;
                            }
                            reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f1_r17_c2_ctrl\">(?<项目名称>.*?)</span>");
                            mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                项目名称 = mcs2[j].Groups["项目名称"].Value;
                            }
                            reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f1_r16_c2_ctrl\">(?<项目位置>.*?)</span>");
                            mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                项目位置 = mcs2[j].Groups["项目位置"].Value;
                            }
                            reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f1_r2_c2_ctrl\">(?<面积>.*?)</span>");
                            mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                面积 = mcs2[j].Groups["面积"].Value;
                            }
                            reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f1_r20_c4_ctrl\">(?<成交价格>.*?)</span>");
                            mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                成交价格 = mcs2[j].Groups["成交价格"].Value;
                            }
                            reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f2_r1_c4_ctrl\">(?<上限>.*?)</span>");
                            mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                上限 = mcs2[j].Groups["上限"].Value;
                            }
                            reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f2_r1_c2_ctrl\">(?<下限>.*?)</span>");
                            mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                下限 = mcs2[j].Groups["下限"].Value;
                            }
                            reg2 = new Regex("id=\"mainModuleContainer_1855_1856_ctl00_ctl00_p1_f1_r14_c4_ctrl\">(?<合同签订日期>.*?)</span>");
                            mcs2 = reg2.Matches(content);
                            if (mcs2.Count >= 1)
                            {
                                合同签订日期 = mcs2[j].Groups["合同签订日期"].Value;
                            }

                            var strSql = @"insert into [www.landchina.com386](行政区, 项目名称, 项目位置, 面积, 成交价格, 上限, 下限, 合同签订日期, URL) values('"
                                         + 行政区 + "','" + 项目名称 + "','" + 项目位置 + "','" + 面积 + "','" + 成交价格 + "','" + 上限 + "','" + 下限 + "','" + 合同签订日期 + "','" + URL + "')";

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