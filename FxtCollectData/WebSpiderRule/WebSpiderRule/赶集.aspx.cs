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
    public partial class 赶集 : System.Web.UI.Page
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

            Regex reg = new Regex("href=\"(?<SourceLink>.*?)\"");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {
                SourceLink = response.Url.Substring(0, response.Url.IndexOf("/fang5")) + mcs[0].Groups["SourceLink"].Value;
                if (SourceLink.Length > 500) SourceLink = "";
            }

            reg = new Regex("class=\"adds\".*?>(?<ProjectName>.*?)</a>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                ProjectName = mcs[0].Groups["ProjectName"].Value;
            }
            else
            {
                reg = new Regex("<i class=\"ico-general\"></i>(?<ProjectName>.*?)</span>");
                mcs = reg.Matches(content);
                if (mcs.Count == 1)
                {
                    ProjectName = mcs[0].Groups["ProjectName"].Value.Replace(" ", "");
                }
            }

            if (ProjectName.Contains("..."))
            {
                try
                {
                    System.Threading.Thread.Sleep(5000);

                    HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(SourceLink);
                    request2.Method = "GET";
                    request2.ContentType = "application/octet-stream";
                    request2.Headers.Set("Pragma", "no-cache");
                    request2.Timeout = 500;
                    HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                    StreamReader streamReader = new StreamReader(response2.GetResponseStream());
                    string strResult = streamReader.ReadToEnd().Replace("\t", "").Replace("\n", "").Replace("\r", "");
                    streamReader.Dispose();
                    response2.Close();

                    reg = new Regex("</i>区：</span>.*?title=\"(?<ProjectName>.*?)\"");
                    mcs = reg.Matches(strResult);
                    if (mcs.Count == 1)
                    {
                        ProjectName = mcs[0].Groups["ProjectName"].Value;
                    }
                    else
                    {
                        reg = new Regex("区：</span>(?<ProjectName>.*?)<");
                        mcs = reg.Matches(strResult);
                        if (mcs.Count == 1)
                        {
                            ProjectName = mcs[0].Groups["ProjectName"].Value.Replace(" ", "");
                        }
                    }
                }
                catch { }
            }

            reg = new Regex("<span class=\"js-huxing\">(?<HouseTypeName>.*?)</span>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                HouseTypeName = mcs[0].Groups["HouseTypeName"].Value;
            }

            reg = new Regex("<span class=\"js-huxing\">(?<HouseInfo>.*?)</div>");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                string HouseInfo = mcs[0].Groups["HouseInfo"].Value;
                if (HouseInfo.Contains("共"))
                {
                    reg = new Regex("共(?<TotalFloor>.*?)层");
                    mcs = reg.Matches(HouseInfo);
                    if (mcs.Count == 1)
                    {
                        TotalFloor = mcs[0].Groups["TotalFloor"].Value;
                    }
                }
                if (HouseInfo.Contains("更新"))
                {
                    reg = new Regex("</i>(?<CaseDate>.*?)更新");
                    mcs = reg.Matches(HouseInfo.Substring(HouseInfo.IndexOf("更新") - 20, 22));
                    if (mcs.Count == 1)
                    {
                        CaseDate = mcs[0].Groups["CaseDate"].Value;
                    }
                }

                reg = new Regex(@"[\u4e00-\u9fa5]+");
                mcs = reg.Matches(HouseInfo);
                for (int i = 0; i < mcs.Count; i++)
                {
                    if (mcs[i].Value.Contains("装修") || mcs[i].Value.Contains("毛坯"))
                    {
                        ZhuangXiu = mcs[i].Value;
                    }
                    else if (mcs[i].Value.Contains("东") || mcs[i].Value.Contains("南") || mcs[i].Value.Contains("西") || mcs[i].Value.Contains("北"))
                    {
                        FrontName = mcs[i].Value;
                    }
                }
            }

            reg = new Regex("<span class=\"js-area\">(?<BuildingArea>.*?)㎡");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                BuildingArea = mcs[0].Groups["BuildingArea"].Value;
            }

            reg = new Regex("㎡</span>&nbsp;(?<UnitPrice>.*?)元/㎡");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                UnitPrice = mcs[0].Groups["UnitPrice"].Value.Replace("(", "");
            }

            reg = new Regex("<em class=\"sale-price js-price\">(?<TotalPrice>.*?)</em>万");
            mcs = reg.Matches(content);
            if (mcs.Count == 1)
            {
                TotalPrice = mcs[0].Groups["TotalPrice"].Value;
            }

            content = ProjectName + "','" + ProjectName2 + "','" + HouseTypeName + "','" + BuildingArea + "','" + FrontName + "','" + FloorNumber
                    + "','" + TotalFloor + "','" + BuildingDate + "','" + ZhuangXiu + "','" + CaseDate + "','" + UnitPrice + "','" + TotalPrice + "','" + SourceLink;

        }
    }
}