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
    public partial class 淘房屋 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://029.taofw.cn/");
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            string content = sr.ReadToEnd();

            //Regex reg = new Regex("<div class='ui-loupan-text'>(?<content>.*?)</div>");
            //MatchCollection mcs = reg.Matches(content);
            //if (mcs.Count >= 1)
            //{
            //    content = mcs[0].Groups["content"].Value;
            //}

            content="<div class='ui-loupan-text'><span class='ui-price ui-price-esf'><strong>32</strong>万<i>6957元/平米</i></span><h2><a title='万象国际公寓位置好楼层佳是你居家好去处' target='_blank' href='/house/view_house_311109.html' onclick=\"AddCompare('history0','311109','万象国际公寓 46㎡ 32万元',8,17)\">万象国际公寓位置好楼层佳是你居家好去处</a><u>多图</u></h2><p>雁塔&nbsp;&nbsp;电子城&nbsp;&nbsp;&nbsp;&nbsp;万象国际公寓</p><p><script>GetHouseTypeByID(0)</script>&nbsp;&nbsp;|&nbsp;&nbsp;1房1厅&nbsp;&nbsp;| &nbsp;&nbsp; 46㎡&nbsp;&nbsp;|&nbsp;&nbsp;<script>GetHousePropertyByID(1)</script> · <script>GetHouseFitmentByID(1)</script> · <script>GetHouseAspectByID(1)</script>向 · 楼层33/16 · 15年房</p><p>经纪人： 高海继&nbsp;&nbsp;&nbsp;&nbsp;18710859310</p><p>2016-08-15 更新</p></div>";
            
            content = content.Replace("\t", "").Replace("\n", ""); 

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
            Regex reg = new Regex("AddCompare(?<ProjectInfo>.*?)>");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                string[] ProjectInfo = mcs[0].Groups["ProjectInfo"].Value.Split(',');
                if (ProjectInfo.Length >= 3)
                {
                    string[] ProjectList = ProjectInfo[2].Split(' ');
                    if (ProjectInfo.Length >= 3)
                    {
                        ProjectName = ProjectList[0];
                        BuildingArea = ProjectList[1].Replace("㎡", "");
                        TotalPrice = ProjectList[2].Replace("万元", "");
                    }
                }                
            }            

            reg = new Regex("<strong>(?<TotalPrice>.*?)</strong>万");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("万<i>(?<UnitPrice>.*?)元");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("GetHouseFitmentByID(?<Fitment>.*?)<");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                try
                {
                    int Fitment = int.Parse(mcs[0].Groups["Fitment"].Value.Replace("(", "").Replace(")", ""));
                    switch (Fitment)
                    {
                        case 0:
                            ZhuangXiu = "毛坯";
                            break;
                        case 1:
                            ZhuangXiu = "简装";
                            break;
                        case 2:
                            ZhuangXiu = "精装";
                            break;
                        case 3:
                            ZhuangXiu = "豪华";
                            break;
                        case 4:
                            ZhuangXiu = "中装";
                            break;
                    }
                }
                catch { }                
            }

            reg = new Regex("GetHouseAspectByID(?<HouseAspect>.*?)<");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                try
                {
                    int HouseAspect = int.Parse(mcs[0].Groups["HouseAspect"].Value.Replace("(", "").Replace(")", ""));
                    switch (HouseAspect)
                    {
                        case 0:
                            FrontName = "南";
                            break;
                        case 1:
                            FrontName = "南北";
                            break;
                        case 2:
                            FrontName = "东南";
                            break;
                        case 3:
                            FrontName = "西南";
                            break;
                        case 4:
                            FrontName = "东";
                            break;
                        case 5:
                            FrontName = "东西";
                            break;
                        case 6:
                            FrontName = "东北";
                            break;
                        case 7:
                            FrontName = "西";
                            break;
                        case 8:
                            FrontName = "西北";
                            break;
                        case 9:
                            FrontName = "北";
                            break;
                    }
                }
                catch { }
            }

            if (content.Contains("厅"))
            {
                HouseTypeName = content.Substring(content.IndexOf("厅") - 3, 4);
            }

            reg = new Regex("向 · 楼层(?<Floor>.*?)·");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string Floor = mcs[0].Groups["Floor"].Value;
                if (Floor.Contains("/"))
                {
                    FloorNumber = Floor.Substring(Floor.IndexOf("/")+1);
                    TotalFloor = Floor.Substring(0,Floor.IndexOf("/"));
                }
            }

            if (content.Contains("更新"))
            {
                CaseDate = content.Substring(content.IndexOf("更新") - 11, 10);
            }

            //reg = new Regex("/house/(?<SourceLink>.*?).html");
            //mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //{
            //    SourceLink = response.Url.Substring(0, response.Url.IndexOf("/search")) + "/house/" + mcs[0].Groups["SourceLink"].Value + ".html";
            //    if (SourceLink.Length > 500) SourceLink = "";
            //}

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
        

        }
    }
}