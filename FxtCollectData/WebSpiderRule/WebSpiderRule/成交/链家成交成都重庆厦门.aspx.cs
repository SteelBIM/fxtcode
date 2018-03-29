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
    public partial class 链家成交成都重庆厦门 : System.Web.UI.Page
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
            Regex reg = new Regex("target=\"_blank\">(?<ProjectName>.*?)</a>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                string[] sArray = Regex.Split(mcs[0].Groups["ProjectName"].Value, " ", RegexOptions.IgnoreCase);
                if (sArray.Length == 3)
                {
                    ProjectName = sArray[0];
                    HouseTypeName = sArray[1];
                    BuildingArea = sArray[2].Replace("平米", "");
                }
            }

            reg = new Regex("<div class=\"div-cun\">.*?<div class=\"div-cun\">.*?<div class=\"div-cun\">(?<TotalPrice>.*?)<span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<div class=\"div-cun\">.*?<div class=\"div-cun\">(?<UnitPrice>.*?)<span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<div class=\"con\">(?<HouseInfo>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string[] sArray = Regex.Split(mcs[0].Groups["HouseInfo"].Value, "/", RegexOptions.IgnoreCase);
                if (sArray.Length == 3)
                {
                    FrontName = sArray[0];
                    TotalFloor = sArray[1].Substring(sArray[1].IndexOf("(共")+2, sArray[1].IndexOf("层)") - sArray[1].IndexOf("(共")-2);
                    BuildingDate = sArray[2].Replace(" ","").Substring(0,4);
                }
            }

            reg = new Regex("<div class=\"div-cun\">(?<CaseDate>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                CaseDate = mcs[0].Groups["CaseDate"].Value;
            }

            reg = new Regex("href=\"(?<SourceLink>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                SourceLink = mcs[0].Groups["SourceLink"].Value;
                if (SourceLink.Length > 500) SourceLink = "";
            }

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
        
        }
    }
}