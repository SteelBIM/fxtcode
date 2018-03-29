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
    public partial class 中原地产 : System.Web.UI.Page
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
            Regex reg = new Regex("<p class=\"f14 f000 mb_10\">(?<ProjectInfo>.*?)</p>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string ProjectInfo = mcs[0].Groups["ProjectInfo"].Value;
                reg = new Regex(">(?<ProjectList>.*?)<");
                mcs = reg.Matches(ProjectInfo);
                if (mcs.Count == 5)
                {
                    ProjectName = mcs[0].Groups["ProjectList"].Value;
                    HouseTypeName = mcs[2].Groups["ProjectList"].Value;
                    BuildingArea = mcs[4].Groups["ProjectList"].Value.Replace("平", "");
                }
                else if (mcs.Count >= 1)
                {
                    ProjectName = mcs[0].Groups["ProjectList"].Value;
                }
            }

            reg = new Regex("<p class=\"f7b mb_10\">(?<HouseInfo>.*?)</p>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                if (HouseInfo.Contains("年"))
                {
                    BuildingDate = HouseInfo.Substring(HouseInfo.IndexOf("年") - 4, 4);
                }

                reg = new Regex(@"[\u4e00-\u9fa5]+");
                mcs = reg.Matches(HouseInfo);
                string HouseList = "";
                for (int i = 0; i < mcs.Count; i++)
                {
                    HouseList = mcs[i].Value;
                    if (HouseList.Contains("东") || HouseList.Contains("南") || HouseList.Contains("西") || HouseList.Contains("北"))
                    {
                        FrontName = HouseList;
                    }
                    else if (HouseList.Contains("层"))
                    {
                        FloorNumber = HouseList;
                    }
                    else if (HouseList.Contains("装") || HouseList.Contains("毛坯"))
                    {
                        ZhuangXiu = HouseList;
                    }
                }
            }

            reg = new Regex("<p class=\'price-nub cRed\'>(?<TotalPrice>.*?)万</p>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<p class=\"f14 f000 mb_15 fsm\">(?<UnitPrice>.*?)元/平");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<p class=\"f7b mb_15\">(?<AreaName>.*?)-");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                AreaName = mcs[0].Groups["AreaName"].Value.Replace(" ","");
            }

            CaseDate = DateTime.Now.ToString("yyyy-MM-dd");

            //reg = new Regex("href=\"/ershoufang(?<SourceLink>.*?)\"");
            //mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //{
            //    SourceLink = response.Url.Substring(0, response.Url.IndexOf("/ershoufang")) + "/ershoufang" + mcs[0].Groups["SourceLink"].Value;
            //    if (SourceLink.Length > 500) SourceLink = "";
            //}

            content = ProjectName + "','" + ProjectName2 + "','" + AreaName + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
        

        }
    }
}