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
    public partial class 三六五淘房 : System.Web.UI.Page
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
            Regex reg = new Regex("<div class=\"item\">(?<ProjectName>.*?)</div>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {                
                ProjectName = mcs[0].Groups["ProjectName"].Value.Replace(" ","");
                if (ProjectName.Substring(0, 1) == "<")
                {
                    reg = new Regex(">(?<ProjectName>.*?)<");
                    mcs = reg.Matches(ProjectName);
                    if (mcs.Count >= 1)
                    {
                        ProjectName = mcs[0].Groups["ProjectName"].Value;
                    }
                }
                else
                {
                    ProjectName = ProjectName.Substring(0, ProjectName.IndexOf("<"));
                }
            }

            reg = new Regex("<div class=\"item\">.*?<div class=\"item\">(?<HouseInfo>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                if (HouseInfo.Contains("厅"))
                {
                    HouseTypeName = HouseInfo.Substring(HouseInfo.IndexOf("厅") - 3, 4);
                }
                if (HouseInfo.Contains("年"))
                {
                    BuildingDate = HouseInfo.Substring(HouseInfo.IndexOf("年") - 4, 4);
                }

                reg = new Regex("</span>(?<HouseList>.*?)<span");
                mcs = reg.Matches(HouseInfo);
                if (mcs.Count >= 1)
                {
                    string HouseList="";
                    for (int i = 0; i < mcs.Count; i++)
                    {
                        HouseList=mcs[i].Groups["HouseList"].Value;
                        if (HouseList.Contains("东") || HouseList.Contains("南") || HouseList.Contains("西") || HouseList.Contains("北"))
                        {
                            FrontName = HouseList;
                        }
                        else if (HouseList.Contains("装") || HouseList.Contains("毛坯"))
                        {
                            ZhuangXiu = HouseList;
                        }
                        else if (HouseList.Contains("层") || HouseList.Contains("/"))
                        {
                            FloorNumber = HouseList.Substring(0, HouseList.IndexOf("/"));
                            TotalFloor = HouseList.Substring(HouseList.IndexOf("/") + 1).Replace("层","");
                        }
                    }
                }
            }

            reg = new Regex("<div class=\"acreage\">(?<BuildingArea>.*?)平米");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                BuildingArea = mcs[0].Groups["BuildingArea"].Value;
            }

            reg = new Regex("<div class=\"price\"> <span class=\"number\">(?<TotalPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<div class=\"unitPrice\"><span class=\"number\">(?<UnitPrice>.*?)</span>元");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<span class=\"time\">(?<CaseDate>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                CaseDate = mcs[0].Groups["CaseDate"].Value;
                if (CaseDate.Replace(" ","").Length > 10)
                {
                    CaseDate = CaseDate.Replace(" ", "").Substring(0, 10);
                }
            }

            reg = new Regex("<div class=\"picBox\">.*?href=\"(?<SourceLink>.*?)\"");
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