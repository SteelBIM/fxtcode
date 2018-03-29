using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using CAS.Entity;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;
namespace FxtCenterService.API
{
    /// <summary>
    /// 一般处理程序基类
    /// 所有一般处理程序，都要继承IRequiresSessionState接口以读取session
    /// 除登录处理外，都需要判断登录状态，如未登录，统一返回提示。
    /// </summary>
    public class HttpHandlerBase : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public HttpHandlerBase()
        {
            InitData();
            CheckLogin();
        }
        public virtual void ProcessRequest(HttpContext context)
        {

        }

        private bool CheckLogin()
        {
            return true;
        }

        /// <summary>
        /// 初始化参数，组装searchbase
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public SearchBase InitData()
        {
            search = new SearchBase();
            HttpContext context = HttpContext.Current;
            try
            {
                string strcode = GetRequest("strcode");
                if (!string.IsNullOrEmpty(strcode))
                {
                    search.StrCode = strcode;
                }
                string strdate = GetRequest("strdate");
                if (!string.IsNullOrEmpty(strdate))
                {
                    search.StrDate = HttpUtility.UrlDecode(strdate, Encoding.UTF8);
                }
                //公用参数            
                string cityid = GetRequest("cityid");
                if (!string.IsNullOrEmpty(cityid))
                {
                    search.CityId = StringHelper.TryGetInt(cityid);
                }
                string areaid = GetRequest("areaid");
                if (!string.IsNullOrEmpty(areaid))
                {
                    search.AreaId = StringHelper.TryGetInt(areaid);
                }
                string companyid = GetRequest("companyid");
                if (!string.IsNullOrEmpty(companyid))
                {
                    search.CompanyId = StringHelper.TryGetInt(companyid);
                }
                string departmentid = GetRequest("departmentid");
                if (!string.IsNullOrEmpty(departmentid))
                {
                    search.DepartmentId = StringHelper.TryGetInt(departmentid);
                }
                string fxtcompanyid = GetRequest("fxtcompanyid");
                if (!string.IsNullOrEmpty(fxtcompanyid))
                {
                    search.FxtCompanyId = StringHelper.TryGetInt(fxtcompanyid);
                }
                string systypecode = GetRequest("systypecode");
                if (!string.IsNullOrEmpty(systypecode))
                {
                    search.SysTypeCode = StringHelper.TryGetInt(systypecode);
                }
                string userid = GetRequest("userid");
                if (!string.IsNullOrEmpty(userid))
                {
                    search.UserId = StringHelper.TryGetInt(userid);
                }
                string username = GetRequest("username");
                if (!string.IsNullOrEmpty(username))
                {
                    search.UserName = username;
                }
                //分页参数
                string pageindex = GetRequest("pageindex");
                search.PageIndex = StringHelper.TryGetInt(pageindex);
                if (!string.IsNullOrEmpty(pageindex))
                {
                    search.Page = true;
                    string pagerecords = GetRequest("pagerecords");
                    string pagerecords1 = GetRequest("pagerecords");
                    if (!string.IsNullOrEmpty(pagerecords))
                        search.PageRecords = StringHelper.TryGetInt(pagerecords);
                }
                //排序参数
                string sortname = GetRequest("sortname");
                if (!string.IsNullOrEmpty(sortname))
                    search.OrderBy = sortname;

                string sortorder = GetRequest("sortorder");
                if (!string.IsNullOrEmpty(sortorder))
                    search.OrderBy += " " + sortorder;
                if (!string.IsNullOrEmpty(GetRequest("orderby"))) {
                    search.OrderBy = GetRequest("orderby");
                }
                search.Top = StringHelper.TryGetInt(GetRequest("top"));
                //查询条件参数
                string query = GetRequest("query");
                if (!string.IsNullOrEmpty(query))
                    search.Where = query;
                //查询条件参数
                string key = GetRequest("key");
                if (!string.IsNullOrEmpty(key))
                    search.Key = key;
                //查勘类型
                int FxtCenterServicetypecode = StringHelper.TryGetInt(GetRequest("SurveyTypeCode"));
                search.SurveyTypeCode = FxtCenterServicetypecode;
                return search;
            }
            catch (Exception ex)
            {
                context.Response.Write(JSONHelper.GetJson(null, 0, ex.Message, ex));
                return null;
            }
        }

        /// <summary>
        /// 取随机数 kevin
        /// </summary>
        /// <returns></returns>
        public static int GetRandom()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            int rtn = BitConverter.ToInt32(bytes, 0);
            if (rtn < 0) rtn = 0 - rtn;
            return rtn;
        } 

        public SearchBase search = null;

        public string PicRootPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("/ProjectPic");
            }
        }

        public string GetRequest(string key)
        {
            return GetRequest(key, string.Empty);
        }

        /// <summary>
        /// 取request参数，注意不能用request[key]，因为这样会包含cookie和servervariables
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public string GetRequest(string key, string defVal)
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.QueryString[key] != null)
                return Request.QueryString[key];
            if (Request.Form[key] != null)
                return Request.Form[key];
            return defVal;
        }
        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="strDate">发送请求的时间</param>
        /// <param name="strCode">加密码</param>
        /// <returns>1：验证通过，0：时间误差过大，-1：加密码错误</returns>
        public int CheckSysCode()
        {
#if DEBUG
            return 1;
#endif
            HttpContext con = HttpContext.Current;
            string strDate = HttpUtility.UrlDecode(con.Request["strdate"].ToString(), Encoding.UTF8);
            string strCode = con.Request["strcode"].ToString();
            DateTime date = Convert.ToDateTime(strDate);
            long ltime = DateTime.Now.ToFileTime() - date.ToFileTime();
            if (ltime > 6000000000 || ltime < -6000000000)//误差超过10分钟
                return 0;
            if (strCode != GetSysCode(strDate))
                return -1;
            return 1;
        }
        /// <summary>
        /// 得到系统验证码
        /// </summary>
        /// <returns></returns>
        public string GetSysCode(string strCode)
        {
            //string strdate = DateTime.Now.ToShortDateString();
            return GetMd5("123" + strCode + "321");
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
        
        /// <summary>
        /// 检查必须传的参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool CheckMustRequest(string[] request)
        {
            HttpContext con = HttpContext.Current;
            Dictionary<string, string> errList = new Dictionary<string, string>();
            for (int i = 0; i < request.Length; i++)
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request[request[i]]))
                {
                    errList.Add(request[i], "必传参数");
                }
            }
            if (errList.Count > 0)
            {
#if DEBUG
                con.Response.Write(GetJson(null, 0, "必传参数缺失", errList));
#else
                con.Response.Write(GetJson(null, 0, "非法访问", null));
#endif
            }
            return errList.Count == 0;
        }

        /// <summary>
        /// 检查必须传的参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Dictionary<string, string> CheckRequest(string[] request)
        {
            Dictionary<string, string> errList = new Dictionary<string, string>();
            for (int i = 0; i < request.Length; i++)
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request[request[i]]))
                {
                    errList.Add(request[i], "必传参数");
                }
            }
            return errList;
        }
        //json返回
        public string GetJson()
        {
            return GetJson(null, 1, "", null);
        }
        //json返回
        public string GetJson(object obj)
        {
            if (obj != null && obj.GetType().Name.IndexOf("Exception") >= 0)
            {
                Exception ex = (Exception)obj;
                return GetJson(null, 0, ex.Message, ex);
            }
            else
                return GetJson(obj, 1, "", null);
        }
        //json返回
        public string GetJson(string message)
        {
            return GetJson(null, 1, message, null);
        }
        //json返回
        public string GetJson(object obj, string message)
        {
            return GetJson(obj, 1, message, null);
        }
        //json返回
        public string GetJson(int status, string message)
        {
            return GetJson(null, status, message, null);
        }
        //json返回
        public string GetJson(object obj, int status, string message)
        {
            return GetJson(obj, status, message, null);
        }
        //json返回
        public string GetJson(object obj, int status, string message, object debug)
        {
            string str = JSONHelper.GetJson(obj, status, message, debug);
            return str.Replace("\"null\"", "null");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// 把数据表组装成JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string DataTableToJSON(DataTable dt)
        {
            string json = "";            
            json = GetJson(JSONHelper.DataTableToList(dt), "");            
            return json;
        }

        /// <summary>
        /// 得到WCF需要的验证码
        /// </summary>
        /// <returns></returns>
        public static string GetCode(string strCode)
        {
            string strDay = DateTime.Now.ToString("yyyy-MM-dd");
            string strDayReverse = string.Empty;
            IEnumerable<char> iableReverse = strDay.Reverse();
            foreach (char chReverse in iableReverse)
            {
                strDayReverse += chReverse;
            }
            return GetMd5(string.Format("{0}{1}{2}", strDay, strCode, strDayReverse));
        }
    }
}