using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Kingsun.ExamPaper.Wechat.Models;

namespace Kingsun.ExamPaper.Wechat.api
{
    /// <summary>
    /// 基础接口
    /// </summary>
    public class BasicApi
    {
        private static readonly string WXappid = System.Configuration.ConfigurationManager.AppSettings["WXappid"];
        private static readonly string WXsecret = System.Configuration.ConfigurationManager.AppSettings["WXsecret"];
        public static string SessionAccessToken = "SessionAccessToken";//access_token缓存 其他接口的通行证
        public static string SessionTicket = "SessionTicket";//jsapi_ticket缓存 

        public BasicApi() { }

        #region 获取access_token缓存
        public static string GetTokenSession(string AppID, string AppSecret)
        {
            string TokenSession = "";

            if (System.Web.HttpContext.Current.Session[SessionAccessToken] == null)
            {
                TokenSession = AddTokenSession(AppID, AppSecret);
            }
            else
            {
                TokenSession = System.Web.HttpContext.Current.Session[SessionAccessToken].ToString();
            }

            return TokenSession;
        }

        /// <summary>
        /// 添加AccessToken缓存
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string AddTokenSession(string AppID, string AppSecret)
        {
            //获取AccessToken
            string AccessToken = GetAccessToken(AppID, AppSecret);
            HttpContext.Current.Session[SessionAccessToken] = AccessToken;
            HttpContext.Current.Session.Timeout = 7200;
            return AccessToken;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string AppID, string AppSecret)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string respText = CommonMethod.WebRequestPostOrGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret), "");
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            string accessToken = respDic["access_token"].ToString();
            return accessToken;
        }
        #endregion

        #region  获取jsapi_ticket缓存
        /// <summary>
        /// 获取jsapi_ticket缓存
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetTicketSession(string token)
        {
            string TicketSession = "";

            if (System.Web.HttpContext.Current.Session[SessionTicket] == null)
            {
                TicketSession = AddTicketSession(token);
            }
            else
            {
                TicketSession = System.Web.HttpContext.Current.Session[SessionTicket].ToString();
            }

            return TicketSession;
        }

        /// <summary>
        /// 添加jsapi_ticket缓存
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string AddTicketSession(string token)
        {
            //获取AccessToken
            string Ticket = GetTicket(token);
            HttpContext.Current.Session[SessionTicket] = Ticket;
            HttpContext.Current.Session.Timeout = 7200;
            return Ticket;
        }

        /// <summary>
        /// 获取微信jsapi_ticket
        /// </summary>
        /// <param name="token">access_token</param>
        /// <returns>jsapi_ticket</returns>
        public static string GetTicket(string token)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string ticketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=jsapi";
            string jsonresult = CommonMethod.WebRequestPostOrGet(ticketUrl, "UTF-8");
            WX_Ticket wxTicket = Jss.Deserialize<WX_Ticket>(jsonresult);
            return wxTicket.ticket;
        }
        #endregion

        //测试
        public string GetToken()
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string respText = CommonMethod.WebRequestPostOrGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", WXappid, WXsecret), "");
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            //string accessToken = respDic["access_token"].ToString();
            if (respDic != null)
            {
                return respDic.Aggregate("",
                    (current, item) => current + ("【Key:" + item.Key + "Values" + item.Value + "】"));

            }
            else
            {
                return "无数据";
            }
        }

        /// <summary>
        /// 获取JsApi权限配置的数组/四个参数
        /// </summary>
        /// <param name="pageurl">当前页面链接（绝对路径）</param>
        /// <returns></returns>
        public string GetWxJsApi(string pageurl)
        {
            //生成签名的时间戳
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-01-01 00:00:00");
            string timestamp = ts.TotalSeconds.ToString().Split('.')[0];
            //生成签名的随机串
            string nonceStr = "test";
            //微信access_token，用于获取微信jsapi_ticket
            string token = GetTokenSession(WXappid, WXsecret);
            //微信jsapi_ticket
            string ticket = GetTicketSession(token);
            //Request.Url.AbsoluteUri;
            //对所有待签名参数按照字段名的ASCII 码从小到大排序（字典序）后，使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串
            string str = "jsapi_ticket=" + ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + pageurl;
            //签名,使用SHA1生成
            string signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();

            return "{\"appId\":\"" + WXappid + "\", \"timestamp\":" + timestamp + ",\"nonceStr\":\"" + nonceStr + "\",\"signature\":\"" + signature + "\",\"token\":\"" + token + "\",\"ticket\":\"" + ticket + "\",\"pageurl\":\"" + pageurl + "\"}";
        }

    }
}