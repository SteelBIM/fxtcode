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
using System.Web.Script.Serialization;

namespace CAS.Common
{
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
        /// 微信推送文本消息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public static string SendMassage(string appid, string appsecret,string msgContent,string wxopenid) 
        {
            string url =string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}",appid,appsecret);
            string result = PostData(url,"");
            AccessToken access =JSONHelper.JSONToObject<AccessToken>(result);
            string senturl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access.access_token;
            string text = MassageTemplete.Text(msgContent, wxopenid);
            result = PostData(senturl, text);
            MessageModel  msgModel =JSONHelper.JSONToObject<MessageModel>(result);
            result = msgModel.errmsg;
            return result;
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
