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
    public partial class 我爱我家成交 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("http://hz.5i5j.com/exchange/getdeals?communityId=127220&page=4&communityname=%E4%BF%9D%E5%88%A9%E4%B8%9C%E6%B9%BE");
            request2.Method = "GET";
            request2.ContentType = "application/octet-stream";
            request2.Headers.Add("x-requested-with", "XMLHttpRequest");
            request2.Timeout = 500;
            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
            StreamReader streamReader = new StreamReader(response2.GetResponseStream());
            string strResult = streamReader.ReadToEnd();
            streamReader.Dispose();
            response2.Close();

            content = strResult.Replace("\t", "").Replace("\n", "").Replace("", "");

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
            Regex reg = new Regex("alt=\"(?<ProjectName>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<b>(?<HouseTypeName>.*?)</b>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                HouseTypeName = mcs[0].Groups["HouseTypeName"].Value;
            }

            reg = new Regex("<span class=\"small-font\">(?<HouseInfo>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                if (HouseInfo.Contains("/"))
                {
                    FloorNumber = HouseInfo.Substring(0, HouseInfo.IndexOf("/"));
                    TotalFloor = HouseInfo.Substring(HouseInfo.IndexOf("/") + 1, 3).Replace("&", "").Replace("n", "");
                }

                reg = new Regex(@"[\u4e00-\u9fa5]+");
                mcs = reg.Matches(HouseInfo);
                for (int i = 0; i < mcs.Count; i++)
                {
                    if (mcs[i].Value.Contains("装") || mcs[i].Value.Contains("毛坯"))
                    {
                        ZhuangXiu = mcs[i].Value;
                    }
                    else if (mcs[i].Value.Contains("东") || mcs[i].Value.Contains("南") || mcs[i].Value.Contains("西") || mcs[i].Value.Contains("北"))
                    {
                        FrontName = mcs[i].Value.Replace("朝", "");
                    }
                }
            }

            reg = new Regex("<li class=\"w1\">(?<BuildingArea>.*?)㎡");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                BuildingArea = mcs[0].Groups["BuildingArea"].Value;
            }

            reg = new Regex("<li class=\"w2 tel-text-red\">(?<CaseDate>.*?)</li>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                CaseDate = mcs[0].Groups["CaseDate"].Value;
            }

            reg = new Regex("<li class=\"w1\">.*?<li class=\"w1\">(?<TotalPrice>.*?)万元");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<li class=\"w2 tel-text-red\">.*?<li class=\"w2 tel-text-red\">(?<UnitPrice>.*?)元");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }            

            reg = new Regex("href=\"(?<SourceLink>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                SourceLink = response.Url;
                if (SourceLink.Length > 500) SourceLink = "";
            }

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
    
        }
    }
}