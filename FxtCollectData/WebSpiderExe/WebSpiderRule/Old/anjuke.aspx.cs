using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test20160519
{
    public partial class anjuke : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://chongqing.qfang.com/sale/1079587");
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            string content = sr.ReadToEnd();

            

            

            //Regex reg = new Regex("<li class=\"list-img clearfix\"(?<content>.*?)</li>");
            //MatchCollection mcs = reg.Matches(content);
            //if (mcs.Count >= 1)
            //{
            //    content = mcs[0].Groups["content"].Value;
            //}

            content = content.Replace("\t", "").Replace("\n", "").Replace("'", "");

            string ProjectName = "";
            string ProjectName2 = "";

            string HouseTypeName = "";
            string BuildingArea = "";
            string FrontName = "";

            string FloorNumber = "0";
            string TotalFloor = "0";
            string BuildingDate = "";

            string ZhuangXiu = "";
            string CaseDate = "";

            string UnitPrice = "";
            string TotalPrice = "";

            string SourceLink = "";

            System.Text.StringBuilder reslut = new System.Text.StringBuilder();
            Regex reg = new Regex("<span class=\"comm-address\" title=\"(?<ProjectName>.*?)&nbsp;");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }
            if (string.IsNullOrEmpty(ProjectName))
            {
                reg = new Regex("title=\"(?<ProjectName>.*?)\"");
                mcs = reg.Matches(content);
                if (mcs.Count >= 1)
                {
                    ProjectName = mcs[0].Groups["ProjectName"].Value;
                    if (ProjectName.Length > 50) ProjectName = ProjectName.Substring(0, 50);
                }
            }

            reg = new Regex("<strong>(?<TotalPrice>.*?)</strong>万");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<div class=\"details-item\">(?<HouserInfo>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                reg = new Regex("<span>(?<HouserList>.*?)</span>");
                mcs = reg.Matches(mcs[0].Groups["HouserInfo"].Value);
                for (int i = 0; i < mcs.Count; i++)
                {
                    if (mcs[i].Value.Contains("平方米"))
                    {
                        BuildingArea = mcs[i].Groups["HouserList"].Value.Replace("平方米", "");
                    }
                    else if (mcs[i].Value.Contains("厅"))
                    {
                        HouseTypeName = mcs[i].Groups["HouserList"].Value;
                    }
                    else if (mcs[i].Value.Contains("元/m²"))
                    {
                        UnitPrice = mcs[i].Groups["HouserList"].Value.Replace("元/m²", "");
                    }
                    else if (mcs[i].Value.Contains("共") && mcs[i].Value.Contains("层"))
                    {
                        TotalFloor = mcs[i].Groups["HouserList"].Value.Substring(mcs[i].Groups["HouserList"].Value.IndexOf("共") + 1).Replace("层", "").Replace(")", "");
                    }
                    else if (mcs[i].Value.Contains("/") && mcs[i].Value.Contains("层"))
                    {
                        TotalFloor = mcs[i].Groups["HouserList"].Value.Substring(mcs[i].Groups["HouserList"].Value.IndexOf("/") + 1).Replace("层", "").Replace(")", "");
                    }
                    else if (mcs[i].Value.Contains("年建造"))
                    {
                        BuildingDate = mcs[i].Groups["HouserList"].Value.Replace("年建造", "");
                    }
                }
            }

            CaseDate = DateTime.Now.ToString("yyyy-MM-dd");

            reg = new Regex("href=\"(?<SourceLink>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                SourceLink = mcs[0].Groups["SourceLink"].Value;
                if (SourceLink.Length > 500) SourceLink = "";
            }

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
        
        }
    }
}