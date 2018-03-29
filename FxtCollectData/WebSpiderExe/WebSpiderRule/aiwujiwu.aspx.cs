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
    public partial class aiwujiwu : System.Web.UI.Page
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
            Regex reg = new Regex("<span class=\"house-title highLight\">(?<ProjectList>.*?)</a>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                reg = new Regex("<span class=\"\">(?<Project>.*?)</span");
                mcs = reg.Matches(mcs[0].Groups["ProjectList"].Value);
                if (mcs.Count >= 3)
                {
                    ProjectName = mcs[0].Groups["Project"].Value;
                    HouseTypeName = mcs[1].Groups["Project"].Value;
                    BuildingArea = mcs[2].Groups["Project"].Value.Replace("m²", "");
                }
            }

            reg = new Regex("<span class=\"total-text\">(?<TotalPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<div class=\"unit-price\">(?<UnitPrice>.*?)</div> ");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                try
                {
                    if (mcs[0].Groups["UnitPrice"].Value.Contains("万"))
                    {
                        UnitPrice = (float.Parse(mcs[0].Groups["UnitPrice"].Value.Replace("万/平", "")) * 10000).ToString();
                    }
                    else
                    {
                        UnitPrice = mcs[0].Groups["UnitPrice"].Value.Replace("元/平", "");
                    }

                }
                catch
                {
                    UnitPrice = mcs[0].Groups["UnitPrice"].Value;
                }
            }

            reg = new Regex("<span class=\"item house-floor\">(?<TotalFloor>.*?)层</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                if (mcs[0].Groups["TotalFloor"].Value.Contains("/"))
                {
                    TotalFloor = mcs[0].Groups["TotalFloor"].Value.Substring(mcs[0].Groups["TotalFloor"].Value.IndexOf("/") + 1).Replace(" ", "");
                }
            }

            reg = new Regex("<span class=\"item ventilation\">(?<FrontName>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                FrontName = mcs[0].Groups["FrontName"].Value;
            }

            reg = new Regex("<span class=\"item\">(?<BuildingDate>.*?)年建造</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                BuildingDate = mcs[0].Groups["BuildingDate"].Value;
            }

            CaseDate = DateTime.Now.ToString("yyyy-MM-dd");

            //reg = new Regex("<a class=\"list-house need-cut\" href=\"(?<SourceLink>.*?)\"");
            //mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //{
            //    SourceLink = response.Url.Substring(0, response.Url.IndexOf("/sale") ) + mcs[0].Groups["SourceLink"].Value;
            //    if (SourceLink.Length > 500) SourceLink = "";
            //}

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;


        }
    }
}