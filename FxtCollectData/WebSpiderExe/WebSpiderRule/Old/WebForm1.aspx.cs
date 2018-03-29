using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test20160519
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public const string API_Datacenter2 = "https://api.fxtcn.com/wdc2/dc/active";
        public const string SignName2 = "70A6A39A-4823-4B94-B834-EA13780FCB34";
        public const string AppId2 = "1003108";
        public const string AppPwd2 = "1087460282";
        public const string AppKey2 = "8551905482";

        public const string API_Datacenter = "https://api.fxtcn.com/wdc/dc/active";
        public const string SignName = "70A6A39A-4823-4B94-B834-EA13780FCB34";
        public const string AppId = "1003104";
        public const string AppPwd = "108746028";
        public const string AppKey = "855190548";

        protected void Page_Load(object sender, EventArgs e)
        {
            string content = "9/23";
            //if (content.Length > 0 && content.Contains("/"))
            //{
            //    content = content.Substring(0, content.IndexOf("/"));
            //}



            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes("http://www.cnblogs.com/gengaixue/archive/2012/08/03/2621054.html"); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            string dddd=sb.ToString();

            //string ProjectName = "";
            //if(content.Contains("href="))
            //{
            //    Regex reg = new Regex("target=\"_blank\">(?<ProjectName>.*?)</a>");
            //    MatchCollection mcs = reg.Matches(content);
            //    if (mcs.Count >= 1)
            //    {
            //        ProjectName = mcs[0].Groups["ProjectName"].Value;
            //    }
            //}
            //else
            //{
            //    Regex reg = new Regex("<span class=\"\">(?<ProjectName>.*?)</span>");
            //    MatchCollection mcs = reg.Matches(content);
            //    if (mcs.Count >= 1)
            //    {
            //        ProjectName = mcs[0].Groups["ProjectName"].Value;
            //    }
            //}

            content = "| 4/6层| 高档装修| 2007年建| 朝南";
            string[] HouserList = content.Split('|');

            content = "?page=6'>6</a>?page=7'>7</a>?page=6'>6</a>?page=8'>8</a>?page=9'>9</a>";
            int pages = 1;
            Regex reg = new Regex("page=(?<page>.*?)'");
            MatchCollection mcs = reg.Matches(content);
            if (mcs.Count >= 1)
            {                
                for (int i = 0;i< mcs.Count;i++ ){
                    if (int.Parse(mcs[i].Groups["page"].Value) > pages)
                        pages = int.Parse(mcs[i].Groups["page"].Value);
                }
            }

            content = "西城-复兴门";
            if (content.Length > 0 && content.Contains("-"))
            {
                content = content.Substring(content.IndexOf("-")+1);
            }

            content = "|</span>3室2厅                                                <span class=\"line\">|<  ";
            if (content.Length > 0 && content.Contains("平米"))
            {
                content = content.Substring(content.IndexOf("平米") - 4, 4).Replace(">", "");
            }
            else
            {
                content = "";
            }

            //content = " <img alt=\"保利中惠悦城\" ";
            //Regex reg = new Regex("<img alt=\"(?<Name>.*?)\"");
            //MatchCollection mcs = reg.Matches(content);
            //if (mcs.Count == 1)
            //    content = mcs[0].Groups["Name"].Value;
            //else
            //    content = "";








            //content = "name: '锦园新世纪房价',";
            //Regex reg = new Regex(@"[\u4e00-\u9fa5]+");
            //MatchCollection mcs = reg.Matches(content);
            //if (mcs.Count >= 1)
            //    content = mcs[0].Value.Replace("房价", "");
            //else
            //    content = "";
            //return;


            return;
            string HouseTypeName = IntToCn("1室1卫").Replace("房", "室").Replace("二", "两");
            
            int AreaId = 2;
            string iProjectName = "小黄庄一区";
            List<ProjectCase> projectcaselist = GetCityPorjectCaseCach(1);
            List<ProjectCase> iprojectcaselist = new List<ProjectCase>();
            if (AreaId > 0) //存在区域
            {
                var v = from c in projectcaselist
                        where c.areaid == AreaId && c.projectname == iProjectName
                        select c;
                iprojectcaselist = v.ToList();
                if (iprojectcaselist != null && iprojectcaselist.Count > 0)
                {
                    if (iprojectcaselist.Count == 1)
                    {
                        //ProjectId = iprojectcaselist[0].projectid;
                        //ProjectName = iprojectcaselist[0].projectname;
                    }
                    else
                    {
                        //string log = "楼盘名称不确定:ID=" + houserorgin.ID + ",iProjectName=" + iProjectName;
                        //for (int i = 0; i < iprojectcaselist.Count; i++)
                        //{
                        //    log = log + "," + iprojectcaselist[i].projectname;
                        //}
                    }
                    return;
                }
            }



            //DateTime dt = DateTime.Parse("05-27 10:24");

            //HandlerDateCase("05-22 22:43","2016-01-01");

            //int AreaId = 0;
            //string AreaName = "海淀区";
            //GetOriginArea(1, ref AreaId, ref AreaName);

            //DownloadAreas();
            //GetProjectCaseByCity(120, 1);
            //GetProjectNetNameByCity(120, 1);

            //string iProjectName2 = "313附近靠近85203开始134附近156											";
            //if (iProjectName2.Contains("附近") && iProjectName2.Length - iProjectName2.IndexOf("附近") > 7)
            //{
            //    iProjectName2 = iProjectName2.Substring(0, iProjectName2.IndexOf("附近") - 5) + iProjectName2.Substring(iProjectName2.IndexOf("附近") + 2, iProjectName2.Length - iProjectName2.IndexOf("附近") - 2);
            //}




            //int CityID = 2;
            //double BuildingArea = 10;
            //if ((CityID == 1 || CityID == 2) && BuildingArea < 5)
            //{
                
            //}
            //else if (CityID != 1 && CityID != 2 && BuildingArea < 15)
            //{
               
            //}
        }

        private string IntToCn(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return "";
                var nums = Regex.Matches(str, @"\d");
                foreach (var item in nums)
                {
                    str = str.Replace(item.ToString(), NumToChar(Convert.ToInt16(item.ToString())));
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        private string NumToChar(int num)
        {
            switch (num)
            {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                case 0:
                    return "零";
                default:
                    return "";
            }
        }

        public void GetOriginArea(int CityID, ref int AreaId, ref string AreaName)
        {
            if (AreaId == 0 && string.IsNullOrEmpty(AreaName)) return;
            try
            {
                if (!string.IsNullOrEmpty(AreaName))
                {
                    AreaName = AreaName.Trim().Replace("区", "").Replace("市", "").Replace("开发区", "").Replace("县", "").Replace("高新区", "");
                    Dictionary<String, String> arealist = GetCityAreaDicByNameCach(CityID);
                    if (arealist.ContainsKey(AreaName))
                    {
                        string[] sArray = arealist[AreaName].Split(',');
                        AreaId = int.Parse(sArray[0]);
                        AreaName = sArray[1];
                    }
                    else
                    {
                        AreaId = 0;
                        AreaName = "";
                    }
                }
                else
                {
                    Dictionary<int, String> arealist = GetCityAreaDicByIdCach(CityID);
                    if (arealist.ContainsKey(AreaId))
                    {
                        AreaName = arealist[AreaId];
                    }
                }
            }
            catch (Exception ex)
            {                
                AreaId = 0;
                AreaName = "";
            }
        }

        public List<ProjectCase> GetCityPorjectCaseCach(int CityID)
        {
            List<ProjectCase> projectcaselist = new List<ProjectCase>();
            for (int pageindex = 1; pageindex < 90; pageindex++)
            {
                List<ProjectCase> projectcaselist2 = GetProjectCaseByCity(CityID, pageindex);
                projectcaselist.AddRange(projectcaselist2);
                if (projectcaselist2.Count() < 200)
                    break;
            }
            return projectcaselist;
        }

        private List<ProjectCase> GetProjectCaseByCity(int CityID, int pageindex)
        {
            List<ProjectCase> projectcase = new List<ProjectCase>();
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(SignName, AppId, AppPwd, AppKey);
            data.sinfo.functionname = "plist";// "splist";"allplist"
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302,
                cityid = CityID,
                key = "",
                pageindex = pageindex,
                pagerecords = 200
            };
            string str = data.GetJsonString();
            try
            {
                string list = APIPostBack(API_Datacenter, str, false, "application/json");
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(list);
                if (rtn.returntype > 0)
                {
                    //获取楼盘列表
                    string strdata = rtn.data.ToString();
                    projectcase = JSONHelper.JSONStringToList<ProjectCase>(strdata);
                    if (projectcase != null && projectcase.Count() > 0)
                    {
                        var v = from c in projectcase
                                orderby c.projectname.Length descending
                                select c;
                        projectcase = v.ToList();
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }
            return projectcase;
        }


        /// <summary>
        /// 根据城市获取区域
        /// </summary>
        /// <param name="CityID"></param>
        /// <returns></returns>

        public Dictionary<String, String> GetCityAreaDicByNameCach(int CityID)
        {
            Dictionary<String, String> areas = new Dictionary<string, String>();
            try
            {
                //LogHelper.SaveLog("开始重新获取GetCityAreaList");
                List<Area> areaslist = GetAreas(CityID);
                foreach (Area area in areaslist)
                {
                    string name = area.areaname.Replace("区", "").Replace("市", "").Replace("开发区", "").Replace("县", "").Replace("高新区", "");
                    areas.Add(name, area.areaid + "," + area.areaname);
                }
            }
            catch (Exception ex)
            {
            }
            return areas;
        }

        /// <summary>
        /// 根据城市获取区域
        /// </summary>
        /// <param name="CityID"></param>
        /// <returns></returns>

        public Dictionary<int, String> GetCityAreaDicByIdCach(int CityID)
        {
            Dictionary<int, String> areas = new Dictionary<int, String>();
            try
            {
                //LogHelper.SaveLog("开始重新获取GetCityAreaList");
                List<Area> areaslist = GetAreas(CityID);
                foreach (Area area in areaslist)
                {
                    string name = area.areaname.Replace("区", "").Replace("市", "").Replace("开发区", "").Replace("县", "").Replace("高新区", "");
                    areas.Add(area.areaid, area.areaname);
                }
            }
            catch (Exception ex)
            {
            }
            return areas;
        }

        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<List<T>>(JsonStr);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        private DateTime HandlerDateCase(string casedate, string createdate)
        {
            DateTime ret = DateTime.Parse(createdate);
            try
            {
                string icasedate = "";
                //////************************日期与时期间隔都存在，如2016/3/16 0:33:18(54天前更新)***********************************////////
                icasedate = casedate.Replace("\t", "").Replace("\n", "");
                if (icasedate.Contains("今天") || icasedate.Contains("今日") || icasedate.Contains("小时") || icasedate.Contains("分钟") || icasedate.Contains("刚"))
                {
                    icasedate = createdate;
                }
                else if (icasedate.Contains("（"))
                {
                    icasedate = icasedate.Substring(0, icasedate.IndexOf("（"));
                }
                else if (icasedate.Contains("("))
                {
                    icasedate = icasedate.Substring(0, icasedate.IndexOf("("));
                }

                ////////////////////////////////************************54天前更新***********************************////////////////////////////
                if (icasedate.Contains("前"))//**前更新类型
                {
                    int num = 0;
                    if (icasedate.Contains("年"))
                    {
                        num = (int)ParseCnToInt(icasedate.Substring(0, icasedate.IndexOf("年")));
                        ret = DateTime.Parse(createdate).AddYears(-num);
                    }
                    else if (icasedate.Contains("月"))
                    {
                        if (icasedate.Contains("个"))
                        {
                            num = (int)ParseCnToInt(icasedate.Substring(0, icasedate.IndexOf("个")));
                            ret = DateTime.Parse(createdate).AddMonths(-num);
                        }
                        else
                        {
                            num = (int)ParseCnToInt(icasedate.Substring(0, icasedate.IndexOf("月")));
                            ret = DateTime.Parse(createdate).AddMonths(-num);
                        }
                    }
                    else if (icasedate.Contains("天"))
                    {
                        num = (int)ParseCnToInt(icasedate.Substring(0, icasedate.IndexOf("天")));
                        ret = DateTime.Parse(createdate).AddDays(-num);
                    }
                    else if (icasedate.Contains("周"))
                    {
                        num = (int)ParseCnToInt(icasedate.Substring(0, icasedate.IndexOf("周"))) * 7;
                        ret = DateTime.Parse(createdate).AddDays(-num);
                    }
                }
                ////////////////////////////////************************2016/3/16 0:33:18***********************************////////////////////////////
                else
                {
                    try
                    {
                        ret = DateTime.Parse(icasedate);
                    }
                    catch
                    {
                        if (icasedate.Contains("-")) ret = DateTime.Parse(DateTime.Now.Year + "-" + icasedate);
                        else if (icasedate.Contains("/")) ret = DateTime.Parse(DateTime.Now.Year + "/" + icasedate);
                    }

                }
            }
            catch
            {
                
            }
            return ret;
        }

        

        public class ProjectNetName
        {
            public int projectnameid { get; set; }
            public string netareaname { get; set; }
            public string netname { get; set; }
        }

        private List<Area> GetAreas(int CityID)
        {
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(SignName, AppId, AppPwd, AppKey);
            data.sinfo.functionname = "garealist";
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302,
                cityid = CityID,
            };
            string str = data.GetJsonString();
            List<Area> areas = new List<Area>();
            try
            {
                string list = APIPostBack(API_Datacenter, str, false, "application/json");
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(list);
                if (rtn.returntype > 0)
                {
                    //获取行政区列表
                    string strdata = rtn.data.ToString();
                    areas = JSONHelper.JSONStringToList<Area>(strdata);
                    if (areas.Count <= 0)
                    {
                        
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }
            return areas;
        }

        public class Area
        {
            public int areaid { get; set; }
            public string areaname { get; set; }
        }

        private List<ProjectCase> GetProjectNetNameByCity(int CityID, int pageindex)
        {
            List<ProjectCase> projectcase = new List<ProjectCase>();
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(SignName, AppId, AppPwd, AppKey);
            data.sinfo.functionname = "plist";// "splist";"allplist"
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302,
                cityid = CityID,
                key = "",
                pageindex = pageindex,
                pagerecords = 200
            };
            string str = data.GetJsonString();
            try
            {
                string list = APIPostBack(API_Datacenter, str, false, "application/json");
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(list);
                if (rtn.returntype > 0)
                {
                    //获取楼盘列表
                    string strdata = rtn.data.ToString();
                    projectcase = JSONHelper.JSONStringToList<ProjectCase>(strdata);
                }
            }
            catch (Exception ex)
            {
            }
            return projectcase;
        }

        public class ProjectCase
        {
            public int projectid { get; set; }
            public string projectname { get; set; }
            public int areaid { get; set; }
        }

        private double StandardArea(string area)
        {
            double ret = 0;
            if (string.IsNullOrEmpty(area)) return 0;
            if (area.Contains("大") || area.Contains("小")) return 0;
            try
            {
                area = area.Trim().Replace("平方", "").Replace("平米", "").Replace("㎡", "");
                ret = double.Parse(area);
            }
            catch
            {
            }
            return ret;
        }

        private double StandardPrice(string price, int type)
        {
            double ret = 0;
            if (string.IsNullOrEmpty(price.Trim())) return 0;
            if (price.Contains("*") || price.Contains("面谈") || price.Contains("面议") || price.Contains("大") || price.Contains("小")) return 0;
            try
            {
                price = price.Trim().Replace("元", "").Replace("，", "").Replace(",", ""); //去掉“万”“，”“,”
                if (price.Contains("万"))
                {
                    ret = double.Parse(price.Substring(0, price.IndexOf("万"))) * 10000;
                }
                else if (type == 1)
                {
                    ret = double.Parse(price) * 10000;
                }
                else
                {
                    ret = double.Parse(price);
                }
            }
            catch
            {
            }
            return ret;
        }

        private long ParseCnToInt(string cnum)
        {
            cnum = cnum.Trim().Replace("第", "").Replace("层", "");
            try
            {
                return Convert.ToInt16(cnum);
            }
            catch (Exception)
            {
                cnum = Regex.Replace(cnum, "\\s+", "");
                long firstUnit = 1;//一级单位                  
                long secondUnit = 1;//二级单位   
                long tmpUnit = 1;//临时单位变量  
                long result = 0;//结果  
                for (int i = cnum.Length - 1; i > -1; --i)//从低到高位依次处理  
                {
                    tmpUnit = CharToUnit(cnum[i]);//取出此位对应的单位  
                    if (tmpUnit > firstUnit)//判断此位是数字还是单位  
                    {
                        firstUnit = tmpUnit;//是的话就赋值,以备下次循环使用  
                        secondUnit = 1;
                        if (i == 0)//处理如果是"十","十一"这样的开头的  
                        {
                            result += firstUnit * secondUnit;
                        }
                        continue;//结束本次循环  
                    }
                    else if (tmpUnit > secondUnit)
                    {
                        secondUnit = tmpUnit;
                        continue;
                    }
                    result += firstUnit * secondUnit * CharToNumber(cnum[i]);//如果是数字,则和单位想乘然后存到结果里  
                }
                return result;
            }
        }

        /// <summary>  
        /// 转换数字  
        /// </summary>  
        private long CharToNumber(char c)
        {
            switch (c)
            {
                case '一': return 1;
                case '二': return 2;
                case '三': return 3;
                case '四': return 4;
                case '五': return 5;
                case '六': return 6;
                case '七': return 7;
                case '八': return 8;
                case '九': return 9;
                case '零': return 0;
                default: return -1;
            }
        }

        private long CharToUnit(char c)
        {
            switch (c)
            {
                case '一': return 1;
                case '二': return 2;
                case '三': return 3;
                case '四': return 4;
                case '五': return 5;
                case '六': return 6;
                case '七': return 7;
                case '八': return 8;
                case '九': return 9;
                case '十': return 10;
                case '百': return 100;
                case '千': return 1000;
                case '万': return 10000;
                case '亿': return 100000000;
                default: return 1;
            }
        }

        private List<Area> DownloadAreas()
        {
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(SignName, "1003104", "108746028", "855190548");
            data.sinfo.functionname = "garealist";
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302,
                cityid = 6,
            };
            string str = data.GetJsonString();
            List<Area> areas = new List<Area>();
            try
            {
                string list = APIPostBack(API_Datacenter, str, false, "application/json");
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(list);
                if (rtn.returntype > 0)
                {
                    //获取行政区列表
                    string strdata = rtn.data.ToString();
                    areas = JSONHelper.JSONStringToList<Area>(strdata);
                }
            }
            catch (Exception ex)
            {                
            }
            return areas;
        }

        public static string APIPostBack(string url, string posts, bool check, string contentType)
        {
            //检查参数，登录接口不需要
            if (check)
                posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            //找退出原因
            //LogHelper.Info(url + posts);
            WebClient client = new WebClient();

            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            byte[] responseData = null;
            string result = "";
            try
            {
                responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }

        public static string TimeString(string posts)
        {
            DateTime dt = DateTime.Now;
            //这里改为数组，原来用indexof会引起相似名称的参数冲突 kevin
            string[] tmp = null;
            string tmpstr = posts;
            if (!posts.StartsWith("http://") && !posts.StartsWith("?") && !posts.StartsWith("&"))
            {
                tmpstr = "&" + posts;
            }
            tmp = tmpstr.Split(new char[] { '?', '&' });
            List<string> tmpkey = new List<string>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i].IndexOf("=") > 0)
                {
                    tmpkey.Add(tmp[i].Split('=')[0]);
                }
            }
            if (!tmpkey.Contains<string>("strdate"))
            {
                posts += "&strdate=" + HttpUtility.UrlEncode(dt.ToString());
            }
            if (!tmpkey.Contains<string>("strcode"))
            {
                posts += "&strcode=" + GetMd5("123" + dt.ToString() + "321");
            }
            //加上当前IP
            posts += "&sourceip=";
            if (posts.StartsWith("http://") && posts.IndexOf("&") > 0 && posts.IndexOf("?") < 0)
            {
                posts = posts.Substring(0, posts.IndexOf("&")) + "?" + posts.Substring(posts.IndexOf("&") + 1); ;
            }
            return posts;
        }

        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        public static string GetMd5(string strmd5)
        {
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }
    }
}