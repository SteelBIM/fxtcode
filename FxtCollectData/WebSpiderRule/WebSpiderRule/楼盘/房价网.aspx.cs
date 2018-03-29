using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSpiderRule.楼盘
{
    public partial class 房价网 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string content = "";


            content = content.Replace("\t", "").Replace("\n", "");

            string ProjectId = "";
            string ProjectName = "";
            string UnitPrice = "";
            string UnitRentPrice = "";
            string UnitPriceRise = "";
            string SaleNum = "0";
            string RentNum = "0";
            string Address = "";
            string Url = "";

            System.Text.StringBuilder reslut = new System.Text.StringBuilder();
            Regex reg = new Regex("name=\"entity.districtName\" value=\"(?<ProjectName>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("name=\"entity.curAvgPrice\" value=\"(?<UnitPrice>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("环比上周<span.*?>(?<UnitPriceRise>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                UnitPriceRise = mcs[0].Groups["UnitPriceRise"].Value;               
            }

            reg = new Regex("href=\"(?<Url>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                Url = response.Url;
            }

            content = ProjectId + "','" + ProjectName + "','" + UnitPrice + "','" + UnitRentPrice + "','" + UnitPriceRise + "','" + SaleNum
                   + "','" + RentNum + "','" + Address + "','" + Url;
        


        }
    }
}