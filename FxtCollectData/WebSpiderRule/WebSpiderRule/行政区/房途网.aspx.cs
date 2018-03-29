using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSpiderRule.行政区
{
    public partial class 房途网 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://chongqing.qfang.com/sale/1079587");
            //WebResponse response = request.GetResponse();
            //Stream resStream = response.GetResponseStream();
            //StreamReader sr = new StreamReader(resStream);
            //string content = sr.ReadToEnd();

            //Regex reg = new Regex("<li class=\"list-img clearfix\"(?<content>.*?)</li>");
            //MatchCollection mcs = reg.Matches(content);
            //if (mcs.Count >= 1)
            //{
            //    content = mcs[0].Groups["content"].Value;
            //}

            string content = "";

            content = content.Replace("\t", "").Replace("\n", "").Replace("'", "");

            string ProjectName = "";
            string ProjectName2 = "";

            string AreaName = "";

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

            Regex reg = new Regex("href=\"(?<SourceLink>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                SourceLink = response.Url.Substring(0, response.Url.IndexOf("/es")) + mcs[0].Groups["SourceLink"].Value;
                if (SourceLink.Length > 500) SourceLink = "";
            }

            reg = new Regex("<div class=\"adds\">.*?title=\"(?<ProjectName>.*?)\"");
            mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;

                reg = new Regex("<a class=\"ml20 C000\".*?>(?<AreaName>.*?)</a>");
                mcs = reg.Matches(content);
                if (mcs.Count == 1)
                {
                    AreaName = mcs[0].Groups["AreaName"].Value;
                }
            }
            else//详情页
            {
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(SourceLink);
                request2.Timeout = 30000;
                request2.Method = "GET";
                request2.ContentType = "application/octet-stream";
                request2.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                StreamReader streamReader = new StreamReader(response2.GetResponseStream());
                string strResult = streamReader.ReadToEnd();
                streamReader.Dispose();
                response2.Close();

                reg = new Regex("housetitle:(?<ProjectName>.*?),");
                mcs = reg.Matches(strResult);
                if (mcs.Count == 1)
                {
                    ProjectName = mcs[0].Groups["ProjectName"].Value.Replace("'", "");
                    if (ProjectName.Length > 50) ProjectName = ProjectName.Substring(0, 50).Replace("'", "");
                }

                reg = new Regex("<li>区域：<a.*?class=\"C000\">(?<AreaName>.*?)</a>");
                mcs = reg.Matches(strResult);
                if (mcs.Count == 1)
                {
                    AreaName = mcs[0].Groups["AreaName"].Value;
                }

            }            

            reg = new Regex("<span class=\"fontS30\">(?<TotalPrice>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            reg = new Regex("<span class=\"fontS30\">.*?<div>(?<UnitPrice>.*?)元/㎡");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value;
            }

            reg = new Regex("<div class=\"qita\">(?<HouseInfo>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                reg = new Regex("<span.*?>(?<HouseList>.*?)</span>");
                mcs = reg.Matches(HouseInfo);
                if (mcs.Count >= 1)
                {
                    string HouseList = "";
                    for (int i = 0; i < mcs.Count; i++)
                    {
                        HouseList = mcs[i].Groups["HouseList"].Value;
                        if (HouseList.Contains("㎡"))
                        {
                            BuildingArea = HouseList.Replace("㎡", "");
                        }
                        else if (HouseList.Contains("厅"))
                        {
                            HouseTypeName = HouseList;
                        }
                        else if (HouseList.Contains("装") || HouseList.Contains("毛坯"))
                        {
                            ZhuangXiu = HouseList;
                        }
                        else if (HouseList.Contains("年建造"))
                        {
                            BuildingDate = HouseList.Replace("年建造", "");
                        }
                    }
                }
            }

            reg = new Regex("<div class=\"C999\">.*?<span>(?<CaseDate>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                CaseDate = mcs[0].Groups["CaseDate"].Value;
            }

            reg = new Regex("<div class=\"C999\">.*?<span>(?<CaseDate>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                CaseDate = mcs[0].Groups["CaseDate"].Value;
            }

            if (response.Url.Contains("dr76"))
            {
                FrontName = "东";
            }
            else if (response.Url.Contains("dr127"))
            {
                FrontName = "南";
            }
            else if (response.Url.Contains("dr77"))
            {
                FrontName = "西";
            }
            else if (response.Url.Contains("dr128"))
            {
                FrontName = "北";
            }
            else if (response.Url.Contains("dr66"))
            {
                FrontName = "东南";
            }
            else if (response.Url.Contains("dr129"))
            {
                FrontName = "东西";
            }
            else if (response.Url.Contains("dr131"))
            {
                FrontName = "东北";
            }
            else if (response.Url.Contains("dr126"))
            {
                FrontName = "南北";
            }
            else if (response.Url.Contains("dr130"))
            {
                FrontName = "西南";
            }
            else if (response.Url.Contains("dr132"))
            {
                FrontName = "西北";
            }
            else if (response.Url.Contains("dr325"))
            {
                FrontName = "";
            }
            

            content = ProjectName + "','" + ProjectName2 + "','" + AreaName + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;
    

        }
    }
}