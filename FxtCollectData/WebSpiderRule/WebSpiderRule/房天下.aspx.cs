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
    public partial class 房天下 : System.Web.UI.Page
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
            Regex reg = new Regex("<p class=\"mt10\"><a.*?title=\"(?<ProjectName>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<div class=\"area alignR\"><p>(?<BuildingArea>.*?)㎡</p>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                BuildingArea = mcs[0].Groups["BuildingArea"].Value;
            }

            reg = new Regex("<span class=\"price\">(?<TotalPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<p class=\"danjia alignR mt5\">(?<UnitPrice>.*?)元");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<p class=\"mt12\">(?<HouseInfo>.*?)</p>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                if (HouseInfo.Contains("厅"))
                {
                    HouseTypeName = HouseInfo.Substring(HouseInfo.IndexOf("厅") - 3, 4);
                }
                if (HouseInfo.Contains("建筑年代"))
                {
                    BuildingDate = HouseInfo.Substring(HouseInfo.IndexOf("建筑年代") + 5, 4);
                }
                if (HouseInfo.Contains("向"))
                {
                    FrontName = HouseInfo.Substring(HouseInfo.IndexOf("向") - 3, 3).Replace(">", "").Replace("n", "");
                }
                reg = new Regex("共(?<TotalFloor>.*?)层");
                mcs = reg.Matches(content);
                if (mcs.Count == 1)
                {
                    TotalFloor = mcs[0].Groups["TotalFloor"].Value;
                }                
            }

            reg = new Regex("<span class=\"ml10 gray9\">(?<CaseDate>.*?)更新");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                CaseDate = mcs[0].Groups["CaseDate"].Value;
            }

            //reg = new Regex("<p class=\"title\"><a href=\"(?<SourceLink>.*?)\"");
            //mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //{
            //    SourceLink = response.Url.Substring(0, response.Url.IndexOf("com") + 3) + mcs[0].Groups["SourceLink"].Value;
            //    if (SourceLink.Length > 500) SourceLink = "";
            //}

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
        

        }
    }
}