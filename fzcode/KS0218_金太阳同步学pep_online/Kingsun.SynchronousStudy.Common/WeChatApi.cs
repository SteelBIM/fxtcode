using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.Web.WeChat;

namespace Kingsun.SynchronousStudy.Common
{
    public class WeChatApi
    {
        private static string WXappid = System.Configuration.ConfigurationManager.AppSettings["WXappid"];
        private static string WXsecret = System.Configuration.ConfigurationManager.AppSettings["WXsecret"];

        /// <summary>
        /// 获取JsApi权限配置的数组/四个参数
        /// </summary>
        /// <param name="pageurl">当前页面链接（绝对路径）</param>
        /// <returns></returns>
        public static string GetWxJsApi(string pageurl)
        {
            //生成签名的时间戳
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-01-01 00:00:00");
            string timestamp = ts.TotalSeconds.ToString().Split('.')[0];
            //生成签名的随机串
            string nonceStr = "test";
            //微信access_token，用于获取微信jsapi_ticket
            string token = GetAccessToken(WXappid, WXsecret);
            //微信jsapi_ticket
            string ticket = GetTicket(token);
            //Request.Url.AbsoluteUri;
            //对所有待签名参数按照字段名的ASCII 码从小到大排序（字典序）后，使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串
            string str = "jsapi_ticket=" + ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + pageurl;
            //签名,使用SHA1生成
            string signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();

            return "{\"appId\":\"" + WXappid + "\", \"timestamp\":" + timestamp + ",\"nonceStr\":\"" + nonceStr + "\",\"signature\":\"" + signature + "\"}";
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
            string jsonresult = HttpGet(ticketUrl, "UTF-8");
            WX_Ticket wxTicket = Jss.Deserialize<WX_Ticket>(jsonresult);
            return wxTicket.ticket;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string AppID, string AppSecret)
        {
            //JavaScriptSerializer Jss = new JavaScriptSerializer();
            //string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret);
            //string respText = HttpGet(url, "UTF-8");
            //Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            //string accessToken = respDic["access_token"].ToString();
            //return accessToken;
            string accessToken = SettingHelper.GetSettingByAccessToken();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                accessToken = SettingHelper.GetAccessToken();
            }
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                accessToken = "";
            }
            return accessToken;
        }

        /// <summary>
        /// 用code换取openid 此方法一般是不获取用户昵称时候使用
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="Appsecret"></param>
        /// <param name="Code">回调页面带的code参数</param>
        /// <returns>微信用户唯一标识openid</returns>
        public static string CodeGetOpenid(string Appid, string Appsecret, string Code)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Appid, Appsecret, Code);
            string ReText = Common.WebRequestPostOrGet(url, "");//post/get方法获取信息 
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
            if (!DicText.ContainsKey("openid"))
                return "";
            return DicText["openid"].ToString();
        }

        /// <summary>
        /// HttpGET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="encode">编码方式：GB2312/UTF-8</param>
        /// <returns>字符串</returns>
        private static string HttpGet(string url, string encode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=" + encode;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encode));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 通过微信API获取jsapi_ticket得到的JSON反序列化后的实体
        /// </summary>
        public class WX_Ticket
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public string expires_in { get; set; }
        }
    }
}
