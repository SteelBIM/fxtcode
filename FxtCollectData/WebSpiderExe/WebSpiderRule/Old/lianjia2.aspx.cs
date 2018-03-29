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
    public partial class lianjia2 : System.Web.UI.Page
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
            Regex reg = new Regex("data-el=\"region\">(?<ProjectName>.*?)</a>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<div class=\"totalPrice\"><span>(?<TotalPrice>.*?)</span>万");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<span>单价(?<UnitPrice>.*?)元/平米");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("(共(?<TotalFloor>.*?)层)");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalFloor = mcs[0].Groups["TotalFloor"].Value;
            }

            reg = new Regex("data-el=\"region\">(?<HouseInfo>.*?)<a");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value.Replace(" ","");
                if (HouseInfo.Contains("厅"))
                {
                    HouseTypeName = HouseInfo.Substring(HouseInfo.IndexOf("厅") - 3, 4);
                }
                if (HouseInfo.Contains("平米"))
                {
                    reg = new Regex("厅(?<BuildingArea>.*?)平米");
                    mcs = reg.Matches(HouseInfo);
                    if (mcs.Count == 1)
                    {
                        BuildingArea = mcs[0].Groups["BuildingArea"].Value.Replace("|", "").Replace(" ", "");
                    }
                }
                if (HouseInfo.Contains("年建"))
                {
                    BuildingDate = HouseInfo.Substring(HouseInfo.IndexOf("年建") - 4, 4);
                }

                reg = new Regex(@"[\u4e00-\u9fa5]+");
                mcs = reg.Matches(HouseInfo);
                for (int i = 0; i < mcs.Count; i++)
                {
                    if (mcs[i].Value.Contains("东") || mcs[i].Value.Contains("南") || mcs[i].Value.Contains("西") || mcs[i].Value.Contains("北"))
                    {
                        FrontName = mcs[i].Value.Replace(" ", "");
                    }
                    else if (mcs[i].Value.Contains("装") || mcs[i].Value.Contains("毛坯"))
                    {
                        ZhuangXiu = mcs[i].Value.Replace(" ", "");
                    }
                }
            } 

            reg = new Regex("<div class=\"followInfo\">(?<CaseDate>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                if (mcs[0].Groups["CaseDate"].Value.Contains("带看") && mcs[0].Groups["CaseDate"].Value.Contains("发布"))
                {
                    CaseDate = mcs[0].Groups["CaseDate"].Value.Substring(mcs[0].Groups["CaseDate"].Value.IndexOf("带看")+2, mcs[0].Groups["CaseDate"].Value.IndexOf("发布") - mcs[0].Groups["CaseDate"].Value.IndexOf("带看")).Replace(" ", "").Replace("/", "");
                }
                else if (mcs[0].Groups["CaseDate"].Value.Contains("发布"))
                {
                    CaseDate = mcs[0].Groups["CaseDate"].Value.Substring(mcs[0].Groups["CaseDate"].Value.IndexOf("发布") - 7, 7).Replace(" ", "").Replace("/", "");
                }
            }

            reg = new Regex("<div class=\"title\"><a href=\"(?<SourceLink>.*?)\"");
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