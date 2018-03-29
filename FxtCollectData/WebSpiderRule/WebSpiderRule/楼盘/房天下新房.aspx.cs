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
    public partial class 房天下新房 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string content = "";


            content = content.Replace("\t", "").Replace("\n", "").Replace("'", "");

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
            Regex reg = new Regex("alt=\"(?<ProjectName>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }

            reg = new Regex("<div class=\"nhouse_price\">(?<UnitPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value.Replace("<span>","").Replace(" ","");
            }
            else
            {
                reg = new Regex("<h5><span >(?<UnitPrice>.*?)</span>");
                mcs = reg.Matches(content);
                if (mcs.Count == 1)
                {
                    UnitPrice = mcs[0].Groups["UnitPrice"].Value.Replace(" ", "");
                }
            }            

            reg = new Regex("出售房源：.*?>(?<SaleNum>.*?)套");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                SaleNum = mcs[0].Groups["SaleNum"].Value.Replace(" ", "");
            }

            reg = new Regex("出租房源：.*?>(?<RentNum>.*?)套");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                RentNum = mcs[0].Groups["RentNum"].Value.Replace(" ", "");
            }

            reg = new Regex("title=\"(?<Address>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                Address = mcs[0].Groups["Address"].Value.Replace(" ", "");
            }
            else
            {
                reg = new Regex("地址：<a >(?<Address>.*?)</a>");
                mcs = reg.Matches(content);
                if (mcs.Count == 1)
                {
                    Address = mcs[0].Groups["Address"].Value.Replace(" ", "");
                }
            }

            reg = new Regex("href=\"(?<Url>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                Url = mcs[0].Groups["Url"].Value.Replace(" ", "");
            }

            content = ProjectId + "','" + ProjectName + "','" + UnitPrice + "','" + UnitRentPrice + "','" + UnitPriceRise + "','" + SaleNum
                   + "','" + RentNum + "','" + Address + "','" + Url;


        }
    }
}