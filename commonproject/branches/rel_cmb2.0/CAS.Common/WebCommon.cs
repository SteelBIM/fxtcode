using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.UI.WebControls;
using CAS.Entity.DBEntity;
using System.Collections;
using System.Net;
using System.Threading;
using CAS.Entity;
using System.Reflection;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.IO;
using System.Drawing.Imaging;
using CAS.Entity.SurveyEntity;

namespace CAS.Common
{
    public class WebCommon
    {
        /// <summary>
        /// 数据中心API kevin
        /// </summary>
        public static string FxtDataCenterService
        {
            get
            {
                return GetConfigSetting("fxtdatacenterservice");
            }
        }

        /// <summary>
        /// 用户中心API kevin
        /// </summary>
        public static string FxtUserCenterService
        {
            get
            {
                return GetConfigSetting("fxtusercenterservice");
            }
        }

        /// <summary>
        /// 查勘中心API caoq
        /// </summary>
        public static string FxtSurveyCenterService
        {
            get
            {
                return GetConfigSetting("fxtsurveycenterservice");
            }
        }



        /// <summary>
        /// 根据公司id，产品code获取web地址
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="systypecode"></param>
        /// <returns></returns>
        public static string GetWebUrlByCompanyId(int companyid, int systypecode)
        {
            string msg = null;
            UserCheck user = WebCommon.FxtUserCenterService_GetCompanyProductByCompanyIdAndProductTypeCode(companyid, systypecode, out msg);
            if (user != null)
            {
                msg = user.weburl;
            }
            return msg;
        }

        /// <summary>
        /// 检查时间合法的字符串参数
        /// </summary>
        /// <returns></returns>        
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
                posts += "&strcode=" + StringHelper.GetMd5("123" + dt.ToString() + "321");
            }
            //加上当前IP
            posts += "&sourceip=" + WebCommon.GetIPAddress();
            if (posts.StartsWith("http://") && posts.IndexOf("&") > 0 && posts.IndexOf("?") < 0)
            {
                posts = posts.Substring(0, posts.IndexOf("&")) + "?" + posts.Substring(posts.IndexOf("&") + 1); ;
            }
            return posts;
        }

        /// <summary>
        /// 调用API GET数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string APIGet(string url)
        {
            url = TimeString(url);
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            //这里url要组装安全标记等参数
            try
            {
                string result = client.DownloadString(url);
                client.Dispose();
                return result;
            }
            catch (Exception ex)
            {
#if DEBUG
                return ex.Message + "<br>源URL:" + url;
#else
                return "访问页面出错";
#endif
            }
        }

        /// <summary>
        /// 调用API POST数据，并不返回数据，不阻塞当前线程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        public static void APIPost(string url, string posts)
        {
            posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            client.UploadDataAsync(new Uri(url), "POST", postData);
        }

        /// <summary>
        /// 调用API POST数据，并返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static string APIPostBack(string url, string posts, bool check)
        {
            //检查参数，登录接口不需要
            if (check)
                posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            //找退出原因
            //LogHelper.Info(url + posts);
            WebClient client = new WebClient();

            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            byte[] responseData = null;
            string result = "";
            try
            {
                responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = JSONHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }
        /// <summary>
        /// 中心服务器检查用户的接口 kevin 2013-4-2
        /// </summary>
        /// <param name="username"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetUser(string username, int systypecode, out Exception msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            UserCheck usercheck = null;
            msg = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    string url = api + "handlers/user_check.ashx";
                    string str = APIPostBack(url, string.Format("username={0}&systypecode={1}", username, systypecode), true);
                    if (!string.IsNullOrEmpty(str))
                    {
                        JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                        if (rtn.returntype > 0)
                        {
                            usercheck = new UserCheck();
                            return JSONHelper.JSONToObject<UserCheck>(JSONHelper.ObjectToJSON(rtn.data));
                        }
                        else
                        {
                            msg = new Exception(rtn.returntext.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex;
            }
            return usercheck;
        }

        /// <summary>
        /// 新增中心用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int AddFxtUserCenter(string username, int compnayid)
        {
            int result = 0;
            string api = FxtUserCenterService;
            if (!string.IsNullOrEmpty(api))
            {
                string url = FxtUserCenterService + "handlers/user_handler.ashx";
                string str = APIPostBack(url, string.Format("username={0}&companyid={1}&type={2}", username, compnayid, "add"), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        result = rtn.returntype;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 删除中心数据库用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static bool DeleteFxtUserCenter(List<string> usernames, int companyid)
        {
            bool result = false; ;
            string api = FxtUserCenterService;
            if (!string.IsNullOrEmpty(api))
            {
                string url = FxtUserCenterService + "handlers/user_handler.ashx";
                foreach (string username in usernames)
                {
                    string str = APIPostBack(url, string.Format("username={0}&companyid={1}&type={2}", username, companyid, "delete"), true);
                    if (!string.IsNullOrEmpty(str))
                    {
                        JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                        if (rtn.returntype > 0)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据公司code获取公司信息
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyByCode(string companycode, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            UserCheck usercheck = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/company.ashx";
                string str = APIPostBack(url, string.Format("type={0}&companycode={1}", "companycode", companycode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        return JSONHelper.JSONToObject<UserCheck>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = rtn.returntext.ToString();
                    }
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 根据产品code查评估机构信息  
        /// hody
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static string FxtUserCenterService_GetCompanyInfoByProductCode(SearchBase search,int producttypecode, string key)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string str = string.Empty;

            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/company.ashx";
                str = APIPostBack(url, string.Format("type={0}&producttypecode={1}&key={2}&pageindex={3}&pagerecords={4}", "companysearch", producttypecode, key,search.PageIndex,search.PageRecords), true);
                
            }
            return str;
        }

        /// <summary>
        /// 根据公司id获取公司信息
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyByCompanyId(int companyid, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            UserCheck usercheck = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/company.ashx";
                string str = APIPostBack(url, string.Format("type={0}&companyid={1}", "companyid", companyid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        return JSONHelper.JSONToObject<UserCheck>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = rtn.returntext.ToString();
                    }
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 根据公司id获取公司信息
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static string FxtUserCenterService_EditCompany(string companycode,string wxid, string wxname)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string massage = string.Empty;
            UserCheck usercheck = null;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/company.ashx";
                string str = APIPostBack(url, string.Format("type={0}&companycode={3}&wxid={1}&wxname={2}", "editcompany", wxid, wxname, companycode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        return massage= "1";
                    }
                    else
                    {
                        massage = "-1";
                    }
                }
            }
            return massage;
        }

        /// <summary>
        /// 根据公司id和产品code获取信息(caoq 2013-7-12)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyProductByCompanyIdAndProductTypeCode(int companyid, int producttypecode, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            UserCheck usercheck = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/companyproduct.ashx";
                string str = APIPostBack(url, string.Format("type={0}&companyid={1}&producttypecode={2}", "companyproduct", companyid, producttypecode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        return JSONHelper.JSONToObject<UserCheck>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = rtn.returntext.ToString();
                    }
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 绑定用户手机推送号信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="producttypecode"></param>
        /// <param name="splatype"></param>
        /// <param name="phshuserid"></param>
        /// <param name="channelid"></param>
        /// <param name="msg"></param>
        public static string FxtUserCenterService_BindUserMobilePush(string username, int producttypecode, string splatype, string phshuserid, string channelid, out string msg)
        {
            string api = FxtUserCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/mobilepush.ashx";
                string str = APIPostBack(url, string.Format("type={0}&username={1}&producttypecode={2}&splatype={3}&phshuserid={4}&channelid={5}"
                    , "bind", username, producttypecode, splatype, phshuserid, channelid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        return msg = "1";
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return msg;
        }

        /// <summary>
        /// 清除用户绑定的手机推送号信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="producttypecode"></param>
        /// <param name="channelid">渠道Id</param>
        /// <param name="phshuserid">设备Id</param>
        /// <param name="splatype">手机平台类型</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtUserCenterService_ClearUserMobilePush(string username, int producttypecode,string channelid,string phshuserid,string splatype,out string msg)
        {
            string api = FxtUserCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/mobilepush.ashx";
                string str = APIPostBack(url, string.Format("type={0}&username={1}&producttypecode={2}&channelid={3}&phshuserid={4}&splatype={5}", "exit", username, producttypecode, channelid, phshuserid, splatype), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        return msg = "1";
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return msg;
        }
               
        /// <summary>
        /// 推送信息到手机
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="neirong">消息内容</param>
        /// <param name="entrustid"></param>
        /// <param name="systypecode">产品CODE</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int FxtUserCenterService_SendMesToMobile(string username,string neirong,long entrustid,int systypecode,out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;           
            msg = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    string url = api + "handlers/mobilepush.ashx";
                    string str = APIPostBack(url, string.Format("type={0}&username={1}&neirong={2}&entrustid={3}&producttypecode={4}", "send", username, neirong, entrustid,systypecode), true); 
                    if (!string.IsNullOrEmpty(str))
                    {
                        JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                        if (rtn.returntype > 0)
                        {
                            return 0;
                        }
                        else
                        {
                            msg =rtn.returntext.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return 0;
        }


        //取配置文件appsetting值
        public static string GetConfigSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        public static string GetIPAddress()
        {
            try
            {
                string forwarded = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string remote = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                string IP = GetIPAddress(forwarded, remote);
                return IP;
            }
            catch { return ""; }
        }

        /// <summary>
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址  
        /// </summary>
        /// <param name="forwarded">获得IP的参数</param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static string GetIPAddress(string forwarded, string remote)
        {
            //forwarded＝ HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
            //remote ＝ HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
            string result = String.Empty;
            result = forwarded;
            if (result != null && result != String.Empty)
            {
                //可能有代理  
                if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式  
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。  
                        result = result.Replace("  ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];        //找到不是内网的地址  
                            }
                        }
                    }
                    else if (IsIPAddress(result))  //代理即是IP格式  
                        return result;
                    else
                        result = null;        //代理中的内容  非IP，取IP  
                }
            }

            string IpAddress = (result != null && result != String.Empty) ? result : remote;

            if (null == result || result == String.Empty)
                result = remote;
            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;
            return result;
        }
        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

            return regex.IsMatch(str1);
        }

        /// <summary>
        /// 产生随机字符串，用于客户端随机命名
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRndString(int len)
        {
            string s = Guid.NewGuid().ToString().Replace("-", "");
            return s.Substring(0, len > s.Length ? s.Length : len);
        }

        /// <summary>
        /// 取随机数
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

        /*产生验证码*/
        public static string CreateCode(int codeLength)
        {

            string so = "1,2,3,4,5,6,7,8,9,0,A,B,C,D,G,H";//,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
            string[] strArr = so.Split(',');
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                code += strArr[rand.Next(0, strArr.Length)];
            }
            return code;
        }

        /*产生验证码*/
        public static string NumCode(int codeLength)
        {

            string so = "1,2,3,4,5,6,7,8,9,0";//,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
            string[] strArr = so.Split(',');
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                code += strArr[rand.Next(0, strArr.Length)];
            }
            return code;
        }

        /*产生验证图片*/
        public static System.IO.MemoryStream CreateImages(string code)
        {

            Bitmap image = new Bitmap(60, 20);
            Graphics g = Graphics.FromImage(image);
            WebColorConverter ww = new WebColorConverter();
            g.Clear((Color)ww.ConvertFromString("#FAE264"));

            Random random = new Random();
            //画图片的背景噪音线
            for (int i = 0; i < 12; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.LightGray), x1, y1, x2, y2);
            }
            Font font = new Font("Arial", 15, FontStyle.Bold | FontStyle.Italic);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
             new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Gray, 1.2f, true);
            g.DrawString(code, font, brush, 0, 0);

            //画图片的前景噪音点
            for (int i = 0; i < 10; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.White);
            }

            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

            g.Dispose();
            image.Dispose();

            return ms;
        }

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="url">消息服务器</param>
        /// <param name="cityid"></param>
        /// <param name="userid">发送者</param>
        /// <param name="touserid">接收者</param>
        /// <param name="msgid">消息id来自message表</param>
        /// <param name="tosystypecode">接收方系统类型</param>
        /// <param name="fxtcompanyid">运营方id</param>
        /// <param name="messagetype">消息类型</param>
        public static void SendMessage(string url, int cityid, string userid, string touserid, int msgid, int tosystypecode, int fxtcompanyid, int messagetype)
        {
            MessageBody msg = new MessageBody();
            msg.url = url;
            msg.userid = userid;
            msg.touserid = touserid;
            msg.cityid = cityid;
            msg.msgid = msgid;
            msg.tosystypecode = tosystypecode;
            msg.fxtcompanyid = fxtcompanyid;
            msg.messagetype = messagetype;
            Thread t = new Thread(WebCommon.SendMessageToServer);
            t.IsBackground = true;
            t.Start(msg);
        }
        //public static void SendMessage(string url, int msgid,DatMessage model)
        //{
        //    MessageBody msg = new MessageBody();
        //    msg.url = url;
        //    //msg.userid = model.senduserid;
        //    //msg.touserid = model.touserid;
        //    //msg.cityid = model.cityid;
        //    //msg.msgid = msgid;
        //    //msg.tosystypecode = 1003003;
        //    //msg.companyid = model.companyid;
        //    //msg.messagetype = model.messagetype;
        //    Thread t = new Thread(WebCommon.SendMessageToServer);
        //    t.IsBackground = true;
        //    t.Start(msg);            
        //}

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendMessage(MessageBody msg)
        {
            Thread t = new Thread(WebCommon.SendMessageToServer);
            t.IsBackground = true;
            t.Start(msg);
        }

        /// <summary>
        /// 发送消息到服务器，用线程是为了不阻塞主线程 kevin
        /// </summary>
        /// <param name="o"></param>
        public static void SendMessageToServer(object o)
        {
            if (o is MessageBody)
            {
                try
                {
                    MessageBody msg = (MessageBody)o;
                    string posts = string.Format("cityid={0}&userid={1}&touserid={2}&msgid={3}&tosystypecode={4}&fxtcompanyid={5}&messagetype={6}"
                        , msg.cityid, msg.userid, msg.touserid, msg.msgid, msg.tosystypecode, msg.fxtcompanyid, msg.messagetype);
                    byte[] postData = Encoding.UTF8.GetBytes(posts);
                    WebClient client = new WebClient();
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.Headers.Add("ContentLength", postData.Length.ToString());

                    client.UploadData(new Uri(msg.url), "POST", postData);
                }
                catch (Exception ex) { LogHelper.Error(ex); }
            }
        }

        public static string GetRequest(string key)
        {
            return GetRequest(key, string.Empty);
        }

        /// <summary>
        /// 取request参数，注意不能用request[key]，因为这样会包含cookie和servervariables
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetRequest(string key, string defVal)
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.QueryString[key] != null)
                return Request.QueryString[key];
            if (Request.Form[key] != null)
                return Request.Form[key];
            return defVal;
        }

        /// <summary>
        /// 从实体中获取数据，组装成字典，用于报告生成或其他 kevin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getDictFromModel<T>(T t)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Type type = t.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in infos)
            {
                dict.Add(info.Name.ToLower(), info.GetValue(t, null) == null ? "" : info.GetValue(t, null).ToString());
            }
            return dict;
        }

        /// <summary>
        /// 根据http请求自动生成实体
        /// </summary>        
        public static T InitModel<T>(HttpRequest request, T t, string preFix) where T : CAS.Entity.BaseDAModels.BaseTO
        {
            Type type = t.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            string[] requestKeys = request.QueryString.AllKeys
                                   .Concat(request.Form.AllKeys)
                                   .ToArray();

            foreach (string key in requestKeys)
            {
                foreach (PropertyInfo info in infos)
                {
                    if (preFix + info.Name.ToLower() == key.ToLower())
                    {
                        switch (info.PropertyType.FullName.Split(',')[0])
                        {
                            case "System.String":
                                info.SetValue(t, GetRequest(key), null);
                                break;
                            case "System.Int64":
                                info.SetValue(t, StringHelper.TryGetLong(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Int64":
                                info.SetValue(t, StringHelper.TryGetLong(GetRequest(key)), null);
                                break;
                            case "System.Int32":
                                info.SetValue(t, StringHelper.TryGetInt(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Int32":
                                info.SetValue(t, StringHelper.TryGetInt(GetRequest(key)), null);
                                break;
                            case "System.DateTime":
                                info.SetValue(t, StringHelper.TryGetDateTime(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.DateTime":
                                info.SetValue(t, StringHelper.TryGetDateTime(GetRequest(key)), null);
                                break;
                            case "System.Decimal":
                                info.SetValue(t, StringHelper.TryGetDecimal(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Decimal":
                                info.SetValue(t, StringHelper.TryGetDecimal(GetRequest(key)), null);
                                break;
                            case "System.Double":
                                info.SetValue(t, StringHelper.TryGetDouble(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Double":
                                info.SetValue(t, StringHelper.TryGetDouble(GetRequest(key)), null);
                                break;
                            case "System.Boolean":
                                info.SetValue(t, StringHelper.TryGetBool(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Boolean":
                                info.SetValue(t, StringHelper.TryGetBool(GetRequest(key)), null);
                                break;
                        }
                        break;
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// 根据http请求自动生成实体
        /// </summary>
        public static T InitModel<T>(HttpRequest request) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T t = new T();
            return InitModel<T>(request, t, null);
        }

        /// <summary>
        /// 根据指定字段前缀生成实体
        /// </summary>
        public static T InitModel<T>(HttpRequest request, string preFix) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T t = new T();
            return WebCommon.InitModel<T>(request, t, preFix);
        }

        /// <summary>
        /// 是否管理员
        /// </summary>
        /// <param name="username"></param>
        public static bool IsAdmin(string username)
        {
            if (!string.IsNullOrEmpty(username) && username.IndexOf('@') != -1)
                return username.Split('@')[0] == ConstCommon.Administrator;
            return false;
        }


        /// <summary>
        /// 创建二维码 kevin
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool CreateQrCode(string content, string filepath)
        {
            try
            {
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                //GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four), Brushes.Black, Brushes.White);  
                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Four));
                using (FileStream stream = new FileStream(filepath, FileMode.Create))
                {
                    gRender.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 数据反射给实体赋值 kevin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="tbname"></param>
        public static void getValues<T>(T model, Hashtable tbname)
        {
            Type type = model.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in infos)
            {
                if (tbname.ContainsKey(info.Name))
                {
                    if (!string.IsNullOrEmpty(tbname[info.Name].ToString()))
                    {
                        Type propertyType = info.PropertyType;
                        //Nullable的，要取出实际数据类型
                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = propertyType.GetGenericArguments()[0];
                        }
                        //值转换
                        object val = Convert.ChangeType(tbname[info.Name], propertyType);
                        info.SetValue(model, val, null);
                    }
                }
            }
        }

        /// <summary>
        /// 自定义字段处理 kevin 2013-3-14
        /// 供API和调度中心使用
        /// 1：获取查勘系统字段，赋值给实体
        /// 2：返回后面页面显示、EXCEL使用的hashtable
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customfields"></param>
        public static void GetSurveyFromCustomFields<T>(T model, string customfields, Dictionary<string, string> tbcname)
        {
            Hashtable tbname = new Hashtable();//匹配系统字段            
            List<mobileFields> groups = JSONHelper.JSONStringToList<mobileFields>(customfields);
            foreach (mobileFields item in groups)
            {
                //系统分类直接由手机返回给实体，不用解析 kevin
                if (item.n != "survey_bz" && item.n != "house_scal")
                {
                    string strs = JSONHelper.ObjectToJSON(item.v);
                    List<mobileFieldValues> fields = JSONHelper.JSONStringToList<mobileFieldValues>(strs);
                    foreach (mobileFieldValues field in fields)
                    {
                        bool sysfield = field.f > 0;
                        if (sysfield)
                            tbname.Add(field.n.ToLower(), field.s);//匹配系统字段,字段名称小写，与实体一致
                        string v = string.IsNullOrEmpty(field.s) ? "" : field.s;
                        if (tbcname.ContainsKey(field.c))//匹配EXCEL模板字段  
                            tbcname[field.c] = v;
                        else
                            tbcname.Add(field.c, v);
                    }
                }
            }
            //从数据反射给实体赋值 kevin
            getValues(model, tbname);
        }

        /// <summary>
        /// 从微信下载附件.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static string Download(string url,int companyid)
        {
            string imageUrl = string.Empty;
            // url = http://ww2.sinaimg.cn/bmiddle/43a39d58gw1e87bhe0nevg208c04nx6s.gif
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream reader = response.GetResponseStream();
              string file = "";
             string uploadroot = "upload";
             string uploadpath = companyid.ToString()+"/OA/" + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
             string savepath = HttpContext.Current.Server.MapPath("/" + uploadroot + "/" + uploadpath + "/");
             if (!Directory.Exists(savepath))
                 Directory.CreateDirectory(savepath);
             imageUrl = WebCommon.GetRndString(20) + ".jpg";
             file = savepath + imageUrl;
             imageUrl =  uploadroot + "/" + uploadpath + "/" + imageUrl;
             string thumfilepath = file.Replace(".jpg", "_wx.jpg");
             FileStream writer = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
             byte[] buff = new byte[512];
             int c = 0; //实际读取的字节数
             while ((c = reader.Read(buff, 0, buff.Length)) > 0)
             {
                 writer.Write(buff, 0, c);
             }
             writer.Close();
             writer.Dispose();
             reader.Close();
             reader.Dispose();
             response.Close();
             ImageUtil.MakeThumbnail(file, thumfilepath, 280, 160, CAS.Common.ImageUtil.ThumbnailCompressType.BaseOnProportion,"jpg");
            
            return imageUrl;
        }

        /// <summary>
        /// 写日志(用于跟踪)
        /// </summary>
        public static void WriteLog(string strMemo, string state)
        {
            string filename = HttpContext.Current.Server.MapPath("/logs/log.txt");
            if (!string.IsNullOrEmpty(state))
            {
                filename = HttpContext.Current.Server.MapPath("/logs/log" + state + ".txt");
            }
            StreamWriter sr = null;
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/logs/"));
            try
            {
                if (!File.Exists(filename))
                {
                    sr = new StreamWriter(filename, true);
                    sr.WriteLine(strMemo);
                }
                else
                {
                    sr = new StreamWriter(filename, true);
                    sr.WriteLine(strMemo);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

    }


    /// <summary>
    /// 消息主体
    /// </summary>
    public class MessageBody
    {
        public string url { get; set; }
        public string userid { get; set; }
        public string touserid { get; set; }
        public int cityid { get; set; }
        public int msgid { get; set; }
        public int tosystypecode { get; set; }
        public int fxtcompanyid { get; set; }
        public int companyid { get; set; }
        public int messagetype { get; set; }
    }
}
