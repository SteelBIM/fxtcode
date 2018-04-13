using Kingsun.InterestDubbingGame.BLL;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Kingsun.InterestDubbingGame.Web.Controllers
{
    public class InterestDubbingGameMatchController : Controller
    {
        TB_InterestDubbingGame_MatchBLL matchbll = new TB_InterestDubbingGame_MatchBLL();
        static RedisSortedSetHelper sortedSet = new RedisSortedSetHelper();
        static RedisHashHelper hashRedis = new RedisHashHelper();
        public string WXappid = System.Configuration.ConfigurationManager.AppSettings["WXappid"];
        public string WXsecret = System.Configuration.ConfigurationManager.AppSettings["WXsecret"];
        public string WxDomainName = System.Configuration.ConfigurationManager.AppSettings["WxDomainName"];
        public JsonResult Index()
        {
            try
            {
                Log4Net.LogHelper.Info("返回");
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "返回出错");
            }
            return Json(GetResult(""));
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gradeRange"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public JsonResult UserSchoolRankList(string userId, string gradeRange, int pageIndex, int pageSize, string username = "")
        {
            KingResponse response = new KingResponse();
            try
            {
                var result = new UserSchoolRankReponse();
                if (string.IsNullOrWhiteSpace(username))
                {
                    result = matchbll.GetAllUserSchoolRankList(userId, gradeRange, pageIndex, pageSize);
                }
                else
                {
                    result = matchbll.SearchUserSchoolRankList(userId, gradeRange, username, pageIndex, pageSize);
                }
                response.Data = result;
                response.Success = true;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "获取排行榜出错");

                return Json(KingResponse.GetErrorResponseMessage("获取排行榜出错"));
            }
        }

        public JsonResult GetUserScore(string userId, string voterId)
        {
            KingResponse response = new KingResponse();
            try
            {
                response = matchbll.GetUserScore(userId, voterId);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "获取分数出现异常");
                response.ErrorMsg = "获取分数出现异常.";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Vote(string voterId, string userId)
        {
            KingResponse response = new KingResponse();
            try
            {
                if (string.IsNullOrWhiteSpace(voterId))
                {
                    response.ErrorMsg = "未获取到投票人信息";
                }
                else if (userId == voterId)
                {
                    response.ErrorMsg = "不能给自己投票";
                }
                else
                {
                    response = matchbll.Vote(voterId, userId);
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "投票出现异常");
                response.ErrorMsg = "投票出现异常.";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserContentRecords(string userId)
        {
            KingResponse response = new KingResponse();
            try
            {
                response = matchbll.GetUserContentRecord(userId);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "获取记录出现异常");
                response.ErrorMsg = "获取记录出现异常.";
            }
            return Json(response);
        }


        public ActionResult UserGameMatchDetail_View(string userId, string voterId = "", int type = 0)
        {
            ViewBag.Score = matchbll.GetUserScore(userId, voterId);
          
            string link = string.Format(@"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", WXappid, "http://"+WxDomainName+"/iisstart.htm?userId=" + userId);
            ViewBag.Link = link;

            return View();
        }

        public ActionResult GetOpenID(string code, string userId)
        {
            Log4Net.LogHelper.Info("通过code" + code + "获取openid");
            WebClient wc = new WebClient();
            string jsApiUrl = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", WXappid, WXsecret, code);
            string openIdHash = "Redis_WXOpenID";
            string key = WXappid + "_" + WXsecret + "_" + code;

            if (hashRedis.Get(openIdHash, key) != null)
            {
                Response.Cookies[key].Value = hashRedis.Get(openIdHash, key);//用于判断前端有没有openid            

                return RedirectToAction("UserGameMatchDetail_View", new { voterId = hashRedis.Get(openIdHash, key), userId = userId, type = 3, openIdkey = key });
            }
            string json = wc.DownloadString(jsApiUrl);
            Log4Net.LogHelper.Info("返回openId json:" + json);
            Log4Net.LogHelper.Info("openid cache:" + hashRedis.Get(openIdHash, key));
            JavaScriptSerializer convert = new JavaScriptSerializer();
            var response = convert.Deserialize<WeChatResponse>(json);
            hashRedis.Set(openIdHash, key, response.openid);
            Response.Cookies[key].Value = hashRedis.Get(openIdHash, key);

            return RedirectToAction("UserGameMatchDetail_View", new { voterId = response.openid, userId = userId, type = 3, openIdkey = key });
        }



        //public string GetWeChatConfigJson(string pageurl)
        //{
        //    //生成签名的时间戳
        //    TimeSpan ts = DateTime.Now - DateTime.Parse("1970-01-01 00:00:00");
        //    string timestamp = ts.TotalSeconds.ToString().Split('.')[0];
        //    //生成签名的随机串
        //    string nonceStr = "test";
        //    //微信access_token，用于获取微信jsapi_ticket
        //    string token = GetAccessToken(WXappid, WXsecret);
        //    //微信jsapi_ticket
        //    string ticket = GetTicket(token);
        //    //Request.Url.AbsoluteUri;
        //    //对所有待签名参数按照字段名的ASCII 码从小到大排序（字典序）后，使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串
        //    string str = "jsapi_ticket=" + ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + pageurl;
        //    //签名,使用SHA1生成
        //    string signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();

        //    return "{\"appId\":\"" + WXappid + "\", \"timestamp\":" + timestamp + ",\"nonceStr\":\"" + nonceStr + "\",\"signature\":\"" + signature + "\",\"token\":\"" + token + "\",\"ticket\":\"" + ticket + "\"}";
        //}

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        //public string GetAccessToken(string AppID, string AppSecret)
        //{
        //    return "Yq2Do6qu7VDwQvwU3zLIAgPxOIEngeL2x04KaaNTJxwQ74gP88G8CW-oVmIvXBfwy7ZhQxyQ-Fg0y_ITJI2w3zmDx3beb8CaqhYv3S7acaBESBBeRZ0JIIgs9ec6qfhYBSSeAHAQHH";
        //}

        /// <summary>
        /// 获取微信jsapi_ticket
        /// </summary>
        /// <param name="token">access_token</param>
        /// <returns>jsapi_ticket</returns>
        //public static string GetTicket(string token)
        //{
        //    JavaScriptSerializer Jss = new JavaScriptSerializer();
        //    string ticketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=jsapi";
        //    string jsonresult = HttpGet(ticketUrl, "UTF-8");
        //    WX_Ticket wxTicket = Jss.Deserialize<WX_Ticket>(jsonresult);
        //    return wxTicket.ticket;
        //}

        /// <summary>
        /// HttpGET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="encode">编码方式：GB2312/UTF-8</param>
        /// <returns>字符串</returns>
        //private static string HttpGet(string url, string encode)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "GET";
        //    request.ContentType = "text/html;charset=" + encode;
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream myResponseStream = response.GetResponseStream();
        //    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encode));
        //    string retString = myStreamReader.ReadToEnd();
        //    myStreamReader.Close();
        //    myResponseStream.Close();

        //    return retString;
        //}
        /// </summary>
        public class WX_Ticket
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public string expires_in { get; set; }
        }

        public class WeChatApiClass
        {
            public string appId { get; set; }
            public string timestamp { get; set; }
            public string nonceStr { get; set; }
            public string signature { get; set; }
            public string ticket { get; set; }
            public string token { get; set; }

        }

        public class WeChatResponse
        {
            public string access_token { get; set; }
            public string openid { get; set; }
        }


        private ApiResponse GetErrorResult(string message)
        {
            return new ApiResponse
            {
                Success = false,
                data = null,
                Message = message
            };
        }

        private ApiResponse GetResult(object Data, string message = "")
        {

            return new ApiResponse
            {
                Success = true,
                data = Data,
                Message = message
            };
        }


    }
}
