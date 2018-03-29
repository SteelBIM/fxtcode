using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test20160519
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        public const string API_Datacenter2 = "https://api.fxtcn.com/wdc2/dc/active";
        public const string SignName2 = "70A6A39A-4823-4B94-B834-EA13780FCB34";
        public const string AppId2 = "1003108";
        public const string AppPwd2 = "1087460282";
        public const string AppKey2 = "8551905482";
        protected void Page_Load(object sender, EventArgs e)
        {
            string iProjectName = "长春苑";
            List<ProjectNetName> projectcaselist = GetProjectNetNameByCity(2,0);

            var v = from c in projectcaselist
                    where c.netname != null && c.netname != "" && iProjectName.Contains(c.netname)
                    select c;
            List<ProjectNetName> iprojectcaselist = v.Distinct(new item_collection_DistinctBy_item1()).ToList();
        }

        public class item_collection_DistinctBy_item1 : IEqualityComparer<ProjectNetName>
        {
            public bool Equals(ProjectNetName x, ProjectNetName y)
            {
                if (x.netname == y.netname && x.projectnameid == y.projectnameid && x.netareaname == y.netareaname)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(ProjectNetName obj)
            {
                return 0;
            }
        }

        public class ProjectNetName
        {
            public int projectnameid { get; set; }
            public string netareaname { get; set; }
            public string netname { get; set; }
        }

        private List<ProjectNetName> GetProjectNetNameByCity(int CityID, int pageindex)
        {
            List<ProjectNetName> projectcase = new List<ProjectNetName>();
            JSONHelper.JsonData data = new JSONHelper.JsonData();

            data.sinfo = new SecurityInfo(SignName2, AppId2, AppPwd2, AppKey2);
            data.sinfo.functionname = "projectmatchlist";// "splist";"allplist"
            data.info.appinfo = new { splatype = "win", systypecode = "1003002" };
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
                string list = APIPostBack(API_Datacenter2, str, "application/json");
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(list);
                if (rtn.returntype > 0)
                {
                    //获取楼盘列表
                    string strdata = rtn.data.ToString();
                    projectcase = JSONHelper.JSONStringToList<ProjectNetName>(strdata);
                    if (projectcase != null && projectcase.Count() > 0)
                    {
                        var v = from c in projectcase
                                where c.netname != null
                                orderby c.netname.Length descending
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

        public static string APIPostBack(string url, string posts, string contentType)
        {
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
    }
}