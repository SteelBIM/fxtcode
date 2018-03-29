using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Bll;

namespace FxtSpider.NewHouseSource.重庆网上房地产
{
    public class 重庆_楼栋
    {
        public string 楼栋名称
        {
            get;
            set;
        }
        public string 楼栋ID
        {
            get;
            set;
        }
 
    }
    public class 重庆
    {
        public static string Escape(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append((Char.IsLetterOrDigit(c)
                || c == '-' || c == '_' || c == '\\'
                || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
            }
            return sb.ToString();
        }
        public void start()
        {
            RegexInfo 项目JSON信息正则 = new RegexInfo("(\\{[^\\}]*\\})", "$1");
            RegexInfo 项目ID正则 = new RegexInfo("'PARENTPROJID':'([^']*)'", "$1");
            RegexInfo 项目行政区正则 = new RegexInfo("'F_SITE':'([^']*)'", "$1");
            RegexInfo 楼栋JSON信息正则 = new RegexInfo("(\\{[^\\}]*\\})", "$1");
            RegexInfo 楼栋ID正则 = new RegexInfo("'BUILDID':'([^']*)'", "$1");
            RegexInfo 楼栋名称正则 = new RegexInfo("'F_BLOCK':'([^']*)'", "$1");
            RegexInfo 楼盘名项目名正则 = new RegexInfo("\"projectName\":\"([^\"]*)\"", "$1");
            RegexInfo 坐落地址正则 = new RegexInfo("\"location\":\"([^\"]*)\"", "$1");
            Dictionary<string, RegexInfo> 正则集合字典 = new Dictionary<string, RegexInfo>();
            正则集合字典.Add("项目JSON信息", 项目JSON信息正则);
            Dictionary<string, RegexInfo> 正则集合字典_楼栋内页 = new Dictionary<string, RegexInfo>();
            正则集合字典_楼栋内页.Add("楼盘名项目名", 楼盘名项目名正则);
            正则集合字典_楼栋内页.Add("坐落地址", 坐落地址正则);
            string NowEncoding = "utf-8";
            string 项目列表页URL分页参数 = "http://www.cq315house.com/315web/webservice/GetMyData913.ashx?projectname=&kfs=&projectaddr=&pagesize=10&pageindex={0}&presalecert=";
            string 项目详细页URL参数 = "http://www.cq315house.com/315web/webservice/GetMyData112.ashx?projectId={0}&type=1";
            string 楼栋详细页面URL参数 = "http://www.cq315house.com/315web/HtmlPage/ShowRooms.htm?block={0}&buildingid={1}";
            string 楼栋详细页面URL参数1 = "http://www.cq315house.com/315web/webservice/GetBuildingInfo.ashx?buildingId={0}";

            string 楼盘名 = "";//
            string 所属县区 = "";//
            string 地址 = "";//
            string 预售许可证号_房产证号 = "";
            string 楼栋编号 = "";
            string 单元 = "";//unitnumber:
            string 名义层 = "";//flr:
            string 房号 = "";//flr:+x:
            string 跃式 = "";//rn:
            string 套内面积 = "";//iArea:
            string 建筑面积 = "";//bArea:
            string 使用用途 = "";//use:            
            string 户型 = "";//rType:
            string 拟售单价_套内 = "";//nsjg:
            string 拟售单价_建面 = "";//nsjmjg:
            string 建筑结构 = "";//stru
            string 签约状况 = "";//F_ISONLINESIGN:(0:未签约)
            string 房屋状态 = "";//F_ISOWNERSHIP:(0:可售)

            for (long i = 1; i > 0; i++)
            {
                //**********项目列表页信息(获取项目链接)************//
                Dictionary<string, List<string>> 项目列表页结果 = SpiderHelp.GetHtmlByRegex(
                    string.Format(项目列表页URL分页参数, i), "utf-8", 正则集合字典,null,0);
                List<string> 项目JSON信息List = 项目列表页结果["项目JSON信息"];
                foreach (string 项目json in 项目JSON信息List)
                {
                    List<string> 项目IDList = SpiderHelp.GetStrByRegexByIndex(项目json, 项目ID正则);
                    List<string> 项目行政区List = SpiderHelp.GetStrByRegexByIndex(项目json, 项目行政区正则);
                    string 项目ID = 项目IDList.Count > 0 ? 项目IDList[0] : "";
                    所属县区 = 项目行政区List.Count > 0 ? 项目行政区List[0] : "";
                    //*************项目详细页面信息(获取楼栋链接)*************//
                    正则集合字典.Add("楼栋JSON信息", 楼栋JSON信息正则);
                    string 项目详细URL = string.Format(项目详细页URL参数, 项目ID);
                    Dictionary<string, List<string>> 楼栋列表页结果 = SpiderHelp.GetHtmlByRegex(
                        项目详细URL, "utf-8", 正则集合字典,null,0);
                    List<string> 楼栋JSON信息List = 楼栋列表页结果["楼栋JSON信息"];
                    foreach (string 楼栋json in 楼栋JSON信息List)
                    {
                        List<重庆_楼栋> 楼栋链接信息 = new List<重庆_楼栋>();
                        List<string> 楼栋IDList = SpiderHelp.GetStrByRegexByIndex(楼栋json, 楼栋ID正则);
                        List<string> 楼栋名称List = SpiderHelp.GetStrByRegexByIndex(楼栋json, 楼栋名称正则);
                       
                        string 楼栋ID = 楼栋IDList.Count > 0 ? 楼栋IDList[0] : "";
                        string 楼栋名称 = 楼栋名称List.Count > 0 ? 楼栋名称List[0] : "";
                        if (!string.IsNullOrEmpty(楼栋ID) && !string.IsNullOrEmpty(楼栋名称))
                        {
                            string[] 楼栋ID数组 = 楼栋ID.Split(',');
                            string[] 楼栋名称数组 = 楼栋名称.Split(',');
                            for (int j=0;j<楼栋ID数组.Length;j++)
                            {
                                if (j >= 楼栋名称数组.Length)
                                {
                                    break;
                                }
                                if (!string.IsNullOrEmpty(楼栋ID数组[j]))
                                {
                                    重庆_楼栋 obj = new 重庆_楼栋();
                                    obj.楼栋名称 = 楼栋名称数组[j];
                                    obj.楼栋ID = 楼栋ID数组[j];
                                    楼栋链接信息.Add(obj);
                                }
                            }
                        }
                        foreach (重庆_楼栋 楼栋obj in 楼栋链接信息)
                        {
                            //*************楼栋详细页面信息(获取房号信息)*************//
                            string 楼栋URL = string.Format(楼栋详细页面URL参数1, 楼栋obj.楼栋ID);
                            Dictionary<string, List<string>> 楼栋内页信息List = SpiderHelp.GetHtmlByRegex(楼栋URL, "utf-8", 正则集合字典_楼栋内页,null,0);
                            楼盘名 = 楼栋内页信息List["楼盘名项目名"].Count > 0 ? 楼栋内页信息List["楼盘名项目名"][0] : "";
                            地址 = 楼栋内页信息List["坐落地址"].Count > 0 ? 楼栋内页信息List["坐落地址"][0] : "";

                        }
                       
                    }
                }
            }
        }
    }
}
