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
    public partial class 我爱我家 : System.Web.UI.Page
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

            reg = new Regex("<h3>(?<UnitPrice>.*?)<em>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<em class=\"num-rent\">(?<UnitPriceRise>.*?)</em>");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                UnitPriceRise = mcs[0].Groups["UnitPriceRise"].Value;
                if (UnitPriceRise.Contains("icon-up"))
                {
                    UnitPriceRise = "上升" + UnitPriceRise.Replace("<span class=\"icon-up\"></span>", "");
                }
                else if (UnitPriceRise.Contains("icon-down"))
                {
                    UnitPriceRise = "下降" + UnitPriceRise.Replace("<span class=\"icon-down\"></span>", "");
                }
            }

            reg = new Regex("<span class=\"num-change\">(?<SaleNum>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                SaleNum = mcs[0].Groups["SaleNum"].Value;
            }

            reg = new Regex("<span class=\"num-rent\">(?<RentNum>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                RentNum = mcs[0].Groups["RentNum"].Value;
            }


            reg = new Regex("href=\"(?<Url>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                Url = response.Url.Substring(0, response.Url.IndexOf(".cn") + 3) + mcs[0].Groups["Url"].Value.Replace(" ", "");
            }

            content = ProjectId + "','" + ProjectName + "','" + UnitPrice + "','" + UnitRentPrice + "','" + UnitPriceRise + "','" + SaleNum
                   + "','" + RentNum + "','" + Address + "','" + Url;
        


        }
    }
}