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
    public partial class xinlang_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://bj.esf.sina.com.cn/house/b3-a3/");
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            string content = sr.ReadToEnd();


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
            Regex reg = new Regex("class=\"mr20 txt-cut cmm-name\" title=\"(?<ProjectName>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<div class=\"house-info txt-cut\">(?<HouseInfo>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                reg = new Regex("<span class=\"mr20\">(?<House>.*?)</span>");
                mcs = reg.Matches(mcs[0].Groups["HouseInfo"].Value);
                for (int i = 0; i < mcs.Count; i++)
                {
                    if (mcs[i].Value.Contains("厅"))
                    {
                        HouseTypeName = mcs[i].Groups["House"].Value;
                    }
                    else if (mcs[i].Value.Contains("平米"))
                    {
                        BuildingArea = mcs[i].Groups["House"].Value.Replace("平米", "");
                    }
                    else if (mcs[i].Value.Contains("东") || mcs[i].Value.Contains("南") || mcs[i].Value.Contains("西") || mcs[i].Value.Contains("北"))
                    {
                        FrontName = mcs[i].Groups["House"].Value;
                    }
                }
            }

            reg = new Regex("<span class=\"sp1\">(?<HousePosition>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                reg = new Regex("</span><span>(?<Position>.*?)</span>");
                mcs = reg.Matches(mcs[0].Groups["HousePosition"].Value);
                for (int i = 0; i < mcs.Count; i++)
                {
                    if (mcs[i].Groups["Position"].Value.Contains("共") && mcs[i].Groups["Position"].Value.Contains("层"))
                    {
                        FloorNumber = "0";
                        TotalFloor = mcs[i].Groups["Position"].Value.Replace("共", "").Replace("层", "");
                    }
                    else if (mcs[i].Groups["Position"].Value.Contains("层") && mcs[i].Groups["Position"].Value.Contains("/"))
                    {
                        FloorNumber = mcs[i].Groups["Position"].Value.Substring(0, mcs[i].Groups["Position"].Value.IndexOf("/"));
                        TotalFloor = mcs[i].Groups["Position"].Value.Substring(mcs[i].Groups["Position"].Value.IndexOf("/") + 1).Replace("层", "");
                    }
                    else if (mcs[i].Value.Contains("年建造"))
                    {
                        BuildingDate = mcs[i].Groups["Position"].Value.Substring(0, mcs[i].Groups["Position"].Value.IndexOf("年建造"));
                    }
                    else
                    {
                        ZhuangXiu = mcs[i].Groups["Position"].Value;
                    }
                }
            }

            reg = new Regex("<span class=\"c-9 fs12\">(?<CaseDate>.*?)更新</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                CaseDate = mcs[0].Groups["CaseDate"].Value;
            }

            reg = new Regex("<span class=\"georgia\".*?>(?<TotalPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value.Replace(" ", "");
            }

            reg = new Regex("<div class=\"two\">(?<UnitPrice>.*?)元");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<a target=\"_blank\" link-clk=\"1\" href=\"(?<SourceLink>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                SourceLink = mcs[0].Groups["SourceLink"].Value;
            }

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
    
        }
    }
}