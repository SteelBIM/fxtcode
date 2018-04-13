using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kingsun.PSO
{
    /// <summary>
    /// PSO层使用的Cookie操作类，封装Cookie操作
    /// </summary>
    public class PSOCookie
    {
        HttpContext context;
        HttpCookie clientCookie;
        string cookiename = ConfigurationManager.AppSettings["cookieName"] as string;
        public DateTime ExpiresTime
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数，初始化HttpContext,设置默认过期时间1天

        /// </summary>
        /// <param name="paraContext">客户端HttpContext</param>
        public PSOCookie(HttpContext httpContext)
        {
            context = httpContext;
            this.ExpiresTime = DateTime.Now.AddDays(1);
        }

        /// <summary>
        /// 添加用户信息到Cookie中
        /// </summary>
        /// <param name="cookieInfo"></param>
        public void AddUserCookie(ClientUserinfo cookieInfo)
        {
            clientCookie = new HttpCookie(cookiename);
            clientCookie.Expires = this.ExpiresTime;
            //加密保存
            clientCookie["UserID"] = xxtea.Encrypt(cookieInfo.UserID);
            clientCookie["UserName"] = HttpUtility.UrlEncode(cookieInfo.UserName, Encoding.UTF8);
            clientCookie["UserNum"] = cookieInfo.UserNumber;

            context.Response.Cookies.Add(clientCookie);
        }

        /// <summary>
        /// 添加用户信息到Cookie中
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="userNum"></param>
        public void AddUserCookie(string userID, string userName, string userNum)
        {
            ClientUserinfo cookieInfo = new ClientUserinfo(userID, userName, userNum);
            AddUserCookie(cookieInfo);
        }

        /// <summary>
        /// 删除cookie中的用户信息
        /// </summary>
        public void DeleteUserCookie()
        {
            clientCookie = new HttpCookie(cookiename);
            clientCookie.Expires = DateTime.Now.AddMonths(-1);
            clientCookie["UserID"] = string.Empty;
            clientCookie["UserName"] = string.Empty;
            clientCookie["UserNum"] = string.Empty;
            context.Response.Cookies.Add(clientCookie);
            //HttpCookie ck = new HttpCookie("UserInfo");
            //ck.Expires = DateTime.Now.AddMonths(-1);
            //ck["UserID"] = string.Empty;
            //ck["UserName"] = string.Empty;
            //ck["UserNum"] = string.Empty;
            //context.Response.Cookies.Add(ck);
        }

        public void DeleteAllUserCookie()
        {
            clientCookie = new HttpCookie(cookiename);
            clientCookie.Expires = DateTime.Now.AddMonths(-1);
            clientCookie["UserID"] = string.Empty;
            clientCookie["UserName"] = string.Empty;
            clientCookie["UserNum"] = string.Empty;
            context.Response.Cookies.Add(clientCookie);
            HttpCookie ck = new HttpCookie("UserInfo");
            ck.Expires = DateTime.Now.AddMonths(-1);
            ck["UserID"] = string.Empty;
            ck["UserName"] = string.Empty;
            ck["UserNum"] = string.Empty;
            context.Response.Cookies.Add(ck);
        }


        /// <summary>
        /// 获取cookie中的用户ID,用户名和用户在线编号
        /// </summary>
        /// <returns>登录用户实体类</returns>
        public ClientUserinfo GetCookieUserInfo()
        {
            //客户端Cookie是否存在
            HttpCookie cookie = context.Request.Cookies[cookiename];

            if (cookie != null)
            {

                ClientUserinfo userCookieInfo = new ClientUserinfo();

                //解密用户信息返回
                userCookieInfo.UserID = xxtea.Decrypt(cookie["UserID"]);
                userCookieInfo.UserName = HttpUtility.UrlDecode(cookie["UserName"]);
                userCookieInfo.UserNumber = cookie["UserNum"];
                //userCookieInfo.UserAreaInfo = HttpUtility.UrlDecode(cookie["AreaCodesStr"]);
                if (string.IsNullOrEmpty(cookie["Depth"]))
                {
                    userCookieInfo.Depth = cookie["Depth"];
                }
                return userCookieInfo;
            }
            else//不存在返回空引用
            {
                return null;
            }
        }
    }
}
