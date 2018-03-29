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
    public partial class souhu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string content = "<p class=\"items_attr2\">开间1厅<span class=\"s_line\"></span>1/1&nbsp;层<span class=\"s_line\"></span>2000年建造</p>";

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://cd.esf.focus.cn/sale/balixiaoqu/p1/");
            //request.Timeout = 10000;
            //WebResponse response = request.GetResponse();
            //Stream resStream = response.GetResponseStream();
            //StreamReader sr = new StreamReader(resStream);
            //content = sr.ReadToEnd();

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
            Regex reg = new Regex("<a href=\"/xiaoqu/.*?/\" target=\"_blank\">(?<ProjectName>.*?)</a>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<p class=\"items_attr2\">(?<HouseInfo>.*?)</p>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                if (HouseInfo.Contains("厅"))
                {
                    HouseTypeName = HouseInfo.Substring(HouseInfo.IndexOf("厅") - 3, 4);
                }
                if (HouseInfo.Contains("层"))
                {
                    reg = new Regex("</span>(?<Floor>.*?)&nbsp;");
                    mcs = reg.Matches(HouseInfo.Substring(HouseInfo.IndexOf("层") - 20, 20));
                    if (mcs.Count == 1)
                    {
                        FloorNumber = mcs[0].Groups["Floor"].Value.Substring(0, mcs[0].Groups["Floor"].Value.IndexOf("/"));
                        TotalFloor = mcs[0].Groups["Floor"].Value.Substring(mcs[0].Groups["Floor"].Value.IndexOf("/") + 1);
                    }
                }
                if (HouseInfo.Contains("年"))
                {
                    reg = new Regex("</span>(?<BuildingDate>.*?)年");
                    mcs = reg.Matches(HouseInfo.Substring(HouseInfo.IndexOf("年建造") - 20));
                    if (mcs.Count == 1)
                    {
                        BuildingDate = mcs[0].Groups["BuildingDate"].Value;
                    }
                }
            }

            reg = new Regex("<span class=\"items_m2\">(?<BuildingArea>.*?)平米");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                BuildingArea = mcs[0].Groups["BuildingArea"].Value;
            }

            reg = new Regex("<em class=\"em1\"><i>(?<TotalPrice>.*?)</i>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<em class=\"em2\">(?<UnitPrice>.*?)元/平</em>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            //reg = new Regex("<a class=\"items_l\" href=\"(?<SourceLink>.*?)\"");
            //mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //{
            //    SourceLink = response.Url.Substring(0, response.Url.IndexOf("/sale/")) + mcs[0].Groups["SourceLink"].Value;
            //    if (SourceLink.Length > 500) SourceLink = "";
            //}

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;






            //return ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
            //        + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;


        }
    }
}