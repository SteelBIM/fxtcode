using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSpiderRule
{
    public partial class 我爱我家 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://bj.5i5j.com/exchange/haidian/");
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            string content = sr.ReadToEnd();

            

            content = content.Replace("\t", "").Replace("\n", "").Replace("'", "");

            Regex reg = new Regex("<div class=\"list-info\">(?<content>.*?)</li>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                content = mcs[0].Groups["content"].Value;
            }

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
            reg = new Regex("<h3.*?>(?<ProjectName>.*?)&nbsp;");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<div class=\"list-info-r\">(?<Price>.*?)平");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string Price = mcs[0].Groups["Price"].Value;
                reg = new Regex("<h3>(?<TotalPrice>.*?)<em>万元");
                mcs = reg.Matches(Price);
                if (mcs.Count == 1)
                {
                    TotalPrice = mcs[0].Groups["TotalPrice"].Value;
                }

                reg = new Regex("<p>(?<UnitPrice>.*?)元");
                mcs = reg.Matches(Price);
                if (mcs.Count == 1)
                {
                    UnitPrice = mcs[0].Groups["UnitPrice"].Value;
                }
            }

            reg = new Regex("<li class=\"font-balck\">(?<HouserInfo>.*?)</li>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                reg = new Regex("<span>(?<HouserList>.*?)</span>");
                mcs = reg.Matches(mcs[0].Groups["HouserInfo"].Value);
                string HouserInfo = "";
                for (int i = 0; i < mcs.Count; i++)
                {
                    HouserInfo = mcs[i].Groups["HouserList"].Value;
                    if (HouserInfo.Contains("厅"))
                    {
                        HouseTypeName = HouserInfo;
                    }
                    else if (HouserInfo.Contains("平"))
                    {
                        BuildingArea = HouserInfo.Replace("平", "");
                    }
                    else if (HouserInfo.Contains("东") || HouserInfo.Contains("南") || HouserInfo.Contains("西") || HouserInfo.Contains("北"))
                    {
                        FrontName = HouserInfo;
                    }
                    else if (HouserInfo.Contains("层") && HouserInfo.Contains("/"))
                    {
                        TotalFloor = HouserInfo.Substring(HouserInfo.IndexOf("/") + 1).Replace("层", "");
                    }
                }
            }

            CaseDate = DateTime.Now.ToString("yyyy-MM-dd");

            //reg = new Regex("<h2><a href=\"(?<SourceLink>.*?)\"");
            //mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //{
            //    SourceLink = response.Url.Substring(0, response.Url.IndexOf("/exchange")) + mcs[0].Groups["SourceLink"].Value;
            //    if (SourceLink.Length > 500) SourceLink = "";
            //}

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
        

        }
    }
}