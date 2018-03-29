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
    public partial class qfang2 : System.Web.UI.Page
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
            Regex reg = new Regex("<a target=\"_blank\" key=\"showKeyword\" href=.*?>(?<ProjectName>.*?)</a>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<span class=\"sale-price\">(?<TotalPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }


            reg = new Regex("<span class=\"sale-unit\">万</span><p>(?<UnitPrice>.*?)元/平米");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<p class=\"house-about clearfix\">(?<HouseInfo>.*?)</p>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                if (HouseInfo.Contains("厅"))
                {
                    HouseTypeName = HouseInfo.Substring(HouseInfo.IndexOf("厅") - 3, 4);
                }
                if (HouseInfo.Contains("平米"))
                {
                    string BuildingAreaInfo = HouseInfo.Substring(HouseInfo.IndexOf("平米") - 20, 22);
                    reg = new Regex("<span>(?<BuildingArea>.*?)平");
                    mcs = reg.Matches(BuildingAreaInfo);
                    if (mcs.Count == 1)
                    {
                        BuildingArea = mcs[0].Groups["BuildingArea"].Value;
                    }
                }
                if (HouseInfo.Contains("层/"))
                {
                    TotalFloor = HouseInfo.Substring(HouseInfo.IndexOf("层/") + 2, 3).Replace("层", "").Replace("<", "");
                }
                if (HouseInfo.Contains("年建"))
                {
                    BuildingDate = HouseInfo.Substring(HouseInfo.IndexOf("年建") -4, 4);
                }
                reg = new Regex(@"[\u4e00-\u9fa5]+");
                mcs = reg.Matches(HouseInfo);
                for (int i = 0; i < mcs.Count; i++)
                {
                    if (mcs[i].Value.Contains("装修") || mcs[i].Value.Contains("毛坯"))
                    {
                        ZhuangXiu = mcs[i].Value;
                    }
                }
            }

            CaseDate = DateTime.Now.ToString("yyyy-MM-dd");

            //reg = new Regex("<a key=\"showKeyword\" class=\"showKeyword\" target=\"_blank\" href=\"(?<SourceLink>.*?)\"");
            //mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //{
            //    SourceLink = response.Url.Substring(0, response.Url.IndexOf("sale") - 1) + mcs[0].Groups["SourceLink"].Value;
            //    if (SourceLink.Length > 500) SourceLink = "";
            //}

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;

        }
    }
}