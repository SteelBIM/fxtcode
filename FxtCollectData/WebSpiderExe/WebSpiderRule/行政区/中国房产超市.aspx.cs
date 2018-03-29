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
    public partial class 中国房产超市 : System.Web.UI.Page
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

            string AreaName = "";

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
            Regex reg = new Regex("class=\"w_l_2\">(?<ProjectName>.*?)</a>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<label for=\"\">#(?<AreaName>.*?)-");
            mcs = reg.Matches(content.Replace("[","#"));
            if (mcs.Count == 1)
            {
                AreaName = mcs[0].Groups["AreaName"].Value;
            }

            reg = new Regex("<div class=\"fl\">(?<BuildingArea>.*?)m");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                BuildingArea = mcs[0].Groups["BuildingArea"].Value;
            }

            reg = new Regex("<span class=\"f24 arial\">(?<TotalPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<div class=\"p2 tr\">(?<UnitPrice>.*?)元");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value.Replace("约", "").Replace(" ","");
            }

            reg = new Regex("<span class=\"w_c_1\">(?<HouseInfo>.*?)</label>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value.Replace("</span>","");
                if (HouseInfo.Contains("厅"))
                {
                    HouseTypeName = HouseInfo.Substring(HouseInfo.IndexOf("厅") - 3, 4);
                }

                string[] HouserList = HouseInfo.Split('|');
                foreach (string house in HouserList)
                {
                    if (house.Contains("/") && house.Contains("层"))
                    {
                        FloorNumber = house.Substring(0,house.IndexOf("/"));
                        TotalFloor = house.Substring(house.IndexOf("/") + 1).Replace("层","");
                    }                    
                    else if (house.Contains("装修") || house.Contains("毛坯"))
                    {
                        ZhuangXiu = house;
                    }
                    else if (house.Contains("年建"))
                    {
                        BuildingDate = house.Replace("年建","");
                    }
                    else if (house.Contains("东")||house.Contains("南")||house.Contains("西")||house.Contains("北"))
                    {
                        FrontName = house;
                    }
                }
            }

            reg = new Regex("class=\"w_c_5\">(?<CaseDate>.*?)</label>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
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

            content = ProjectName + "','" + ProjectName2 + "','" + AreaName + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
        

        }
    }
}