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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetCityAreaDicCach(80);
        }

        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, TimeSpan.Zero);
        }

        public Dictionary<String, Int32> GetCityAreaDicCach(int CityID)
        {
            Dictionary<String, Int32> areas = new Dictionary<string, int>();
            try
            {
                string CacheKey = "arealist_" + CityID.ToString().Trim();
                var cachlist = GetCache(CacheKey);
                if (cachlist == null)
                {
                    List<Area> areaslist = GetAreas(CityID);
                    foreach (Area area in areaslist)
                    {
                        string name = area.areaname.Replace("区", "").Replace("市", "").Replace("开发区", "").Replace("县", "").Replace("高新区", "");
                        areas.Add(name, area.areaid);
                    }
                    SetCache(CacheKey, areas, System.DateTime.Now.AddDays(1));
                }
                else
                {
                    areas = (Dictionary<String, Int32>)cachlist;
                }
            }
            catch (Exception ex)
            {
            }
            return areas;
        }

        public const string Url_DataCenter = "https://data.yungujia.com";
        public const string API_Datacenter = "https://api.fxtcn.com/wdc/dc/active";
        public const string SignName = "70A6A39A-4823-4B94-B834-EA13780FCB34";

        private List<Area> GetAreas(int CityID)
        {
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(SignName, "1003104", "108746028", "855190548");
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
                string list = APIPostBack(API_Datacenter, str);
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

        public static string APIPostBack(string url, string posts)
        {
            return APIPostBack(url, posts, "application/json");
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

        public class Area
        {
            public int areaid { get; set; }
            public string areaname { get; set; }
        }
    }
}