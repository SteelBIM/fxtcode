using System;
using System.Text;
using System.Net;
using System.IO;

namespace CAS.Common
{
    /// <summary>
    /// 获取缓存token
    /// </summary>
    /// <param name="getCache">true:获取缓存，false，获取数据库</param>
    /// <returns></returns>
    public delegate string GetWXToken(bool getCache);
    /// <summary>
    /// 更新缓存token
    /// </summary>
    /// <returns></returns>
    public delegate void UpdateWXToken(string token);
    public class WXMassage
    {
        /// <summary>
        /// 根据公司id，产品code获取web地址
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="systypecode"></param>
        /// <returns></returns>
        private static string PostData(string url, string text)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            MemoryStream memory = new MemoryStream();


            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(text);
            request.ContentLength = postdata.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            return result;
        }


        /// <summary>
        /// 调用API GET数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string APIGet(string url)
        {
              HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "GET";

                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string result = reader.ReadToEnd();
                return result;
        }
        /// <summary>
        /// /***gjbapi项目引用**/
        /// 微信推送文本消息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="msgContent"></param>
        /// <param name="getToken">委托，获取缓存或者数据库存的token</param>
        /// <param name="upToken">委托，更新缓存和数据库存的token</param>
        /// <returns></returns>
        public static string SendMassage(string appid, string appsecret,string msgContent,string wxopenid,GetWXToken getToken,UpdateWXToken upToken) 
        {
            try
            {
                LogHelper.Info(string.Format("获取到token开始--appid={0}, appsecret={1},msgContent={2},wxopenid={3}",appid, appsecret, msgContent, wxopenid));
                //是否有读过数据库
                bool isRedDB = false;
                //请求token方法
                Func<string> gtoken = delegate()
                {
                    if (isRedDB)//如果读过数据库则重新请求
                    {
                        string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
                        string resultJson = APIGet(url);
                        AccessToken access = JSONHelper.JSONToObject<AccessToken>(resultJson);
                        return access.access_token;
                    }
                    else
                    {
                        isRedDB = true;
                        return getToken(false);
                    }
                };
                //获取token
                string accessToken = getToken(true);
                //如果缓存没有就读数据库
                if (string.IsNullOrEmpty(accessToken))
                {
                    isRedDB = true;
                    accessToken = getToken(false);
                    upToken(accessToken);
                }
                //如果数据库和缓存都没有就重新请求
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = gtoken();
                    upToken(accessToken);
                }
                //发送消息方法
                Func<MessageModel> send = delegate()
                {
                    string senturl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + accessToken;
                    string text = MassageTemplete.Text(msgContent, wxopenid);
                    string _result = PostData(senturl, text);
                    MessageModel _msgModel = JSONHelper.JSONToObject<MessageModel>(_result);
                    return _msgModel;
                };
                MessageModel msgModel = send();
                //如果token失效，重新请求token，为确保出现误差，重新请求3次
                if (msgModel.errcode == "40001")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        accessToken = gtoken();
                        LogHelper.Info(string.Format("获取到token第{0}次--token={1},appid={2}, appsecret={3},msgContent={4},wxopenid={5}", i, accessToken, appid, appsecret, msgContent, wxopenid));
                        upToken(accessToken);
                        msgModel = send();
                        if (msgModel.errcode != "40001")
                        {
                            break;
                        }
                    }
                }
                LogHelper.Info(string.Format("获取到token成功--token={0},appid={1}, appsecret={2},msgContent={3},wxopenid={4}", accessToken, appid, appsecret, msgContent, wxopenid));
                string result = msgModel.errmsg;
                if (msgModel.errcode != "0")
                {
                    LogHelper.Info(string.Format("微信消息推送失败：info:{0},appid:{1},appsecret:{4},wxopenid:{2},msgContent:{3}",
                        JSONHelper.ObjectToJSON(msgModel), appid, wxopenid, msgContent, appsecret));
                }

                LogHelper.Info(string.Format("获取到token结束--appid={0}, appsecret={1},msgContent={2},wxopenid={3}", appid, appsecret, msgContent, wxopenid));
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Info(string.Format("微信消息推送error：appid:{0},appsecret:{3},wxopenid:{1},msgContent:{2},错误原因:{3}",
                    appid, wxopenid, msgContent, appsecret, ex.Message));
                return "error";
            }
            
        }
    }

    public class MassageTemplete 
    {
        public static string Text(string content,string wxopenid) 
        {
            string result = "{\"touser\":\""+wxopenid+"\",\"msgtype\":\"text\",\"text\":{\"content\":\""+content+"\"}}";
            return result;
        }
    }

    public class AccessToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }

    public class MessageModel 
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }
}
