using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSpiderExe
{
    public class ProjectInfo
    {
        public string AreaName { get; set; }
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }
        public string ProjectAddress { get; set; }
        public string BudingName { get; set; }
        public string BudingArea { get; set; }
        public string BudingNum { get; set; }
        public string BudingFloor { get; set; }
        public string HouseName { get; set; }
    }

    

    public partial class 导出房价网 : System.Web.UI.Page
    {

        public List<ProjectInfo> projectlist = new List<ProjectInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {

            foreach (ProjectInfo info in projectlist)
            {
                string listvalue = info.AreaName + "\t" + info.ProjectName + "\t" + info.CompanyName + "\t" + info.ProjectAddress + "\t"
                            + info.BudingName + "\t" + info.BudingArea + "\t" + info.BudingNum + "\t" + info.BudingFloor + "\t" + info.HouseName + "\n";
                Console.WriteLine(listvalue.ToString());
                FileStream aFile = new FileStream("D:\\www_szfcweb_com.txt", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(aFile);
                sw.Write(listvalue.ToString());
                sw.Close();
            }


            string sqlConnString = "server =192.168.0.5;DataBase = FxtData_Case;User Id = fxtbase_user;Password =base*cn.com;connect timeout=50000";
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(sqlConnString))
            {

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandText = @"select ProjectName,LEFT(b.Url,charindex('/ershoufang',b.Url))+'autoComplete/suggest?cat=district&__ajax=1&keyword=' as Url
                                                    from (
	                                                    select * from 
	                                                    (
		                                                    select CityId,ProjectName from New_Project_House_Price with(nolock) where Id<7652538 and len(ProjectName)>2
		                                                    union all
		                                                    select CityId,left(ProjectName,2) as ProjectName from New_Project_House_Price with(nolock) where Id<7652538 and len(ProjectName)>4
		                                                    union all
		                                                    select CityId,left(ProjectName,4) as ProjectName from New_Project_House_Price with(nolock) where Id<7652538 and len(ProjectName)>6
	                                                    )d
	                                                    group by CityId,ProjectName
                                                    )a
                                                    left join
                                                    (
	                                                    select CityID,MIN(Url) as Url from Spider_Task_Website_Pages with(nolock)
		                                                    where SoureName='房价网' and Url!=''
		                                                    group by CityID
                                                    )b on a.CityId=b.CityID 
                                                    where b.Url is not null and a.ProjectName!=''
                                                    order by a.CityId,ProjectName
                                                    ";

                conn.Open();
                da.Fill(ds);
            }
            string listvalue = "";
            //for (int i = 240233; i < ds.Tables[0].Rows.Count; i++)
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                listvalue = listvalue + ds.Tables[0].Rows[i][1].ToString() + UrlEncode(ds.Tables[0].Rows[i][0].ToString()) + "\r\n";
            }
            Console.WriteLine(listvalue.ToString());
            FileStream aFile = new FileStream("D:\\房价网1.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
            sw.Write(listvalue.ToString());
            sw.Close();
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}