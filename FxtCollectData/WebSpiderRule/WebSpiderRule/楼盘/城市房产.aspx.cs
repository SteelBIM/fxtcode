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
    public partial class 城市房产 : System.Web.UI.Page
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
            Regex reg = new Regex("title=\"(?<ProjectName>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("房价：<span.*?>(?<UnitPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("租金：<span.*?>(?<UnitPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitRentPrice = mcs[0].Groups["UnitRentPrice"].Value;
            }

            reg = new Regex("class=\"blue\">(?<SaleNum>.*?)</a>");
            mcs = reg.Matches(content);
            if (mcs.Count == 2)
            {
                SaleNum = mcs[0].Groups["SaleNum"].Value;
                RentNum = mcs[1].Groups["SaleNum"].Value;
            }

            reg = new Regex("<span class=\"mr\">(?<Address>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                Address = mcs[0].Groups["Address"].Value.Replace(" ", "");
            }

            //reg = new Regex("href=\"(?<Url>.*?)\"");
            //mcs = reg.Matches(content);
            //if (mcs.Count >= 1)
            //{
            //    Url = response.Url.Substring(0, response.Url.IndexOf(".cn") + 3) + mcs[0].Groups["Url"].Value.Replace(" ", "");
            //}

            content = ProjectId + "','" + ProjectName + "','" + UnitPrice + "','" + UnitRentPrice + "','" + UnitPriceRise + "','" + SaleNum
                   + "','" + RentNum + "','" + Address + "','" + Url;
        


        }
    }
}