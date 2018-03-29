using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using CAS.Common;

namespace WebAppTest.微信
{
    public partial class sendmessage : System.Web.UI.Page
    {
        public delegate string Send();
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        public string TipError() 
        {
           return "发送失败";
        }

        public string TipSuccess()
        {
            return "发送成功";
        }

        private string RequestData(Send error,Send success) 
        {
            string appid = txtAppid.Text.TrimEnd(); string appSecret = txtAppSecret.Text.TrimEnd();
            string wxopenid = txtwxopenid.Text.TrimEnd();
            string text = "预评完成：【房地产评估】“w”编号为房讯通估字(G)(2014)第06011号，评估总值:500.00万元，总税费:5000.00元，总净值:499.50万元。------------------请知会:某某 联系方式（在微信“服务记录”中查看其它记录）";
            wxopenid = WXMassage.SendMassage(appid, appSecret, text, wxopenid);
            if (wxopenid == "ok")
            {
                return success();
            }
            else
            {
                return error();
            }

        }

        private string SendMessage(string url, string text)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            MemoryStream memory = new MemoryStream();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(text);
            request.ContentLength = postdata.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            return content;
        }

        private static string GetAccess_Token(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            //request.Method = "POST";

            //MemoryStream memory = new MemoryStream();
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            ////ASCIIEncoding encoding = new ASCIIEncoding();
            //byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes("");
            //request.ContentLength = postdata.Length;
            //Stream newStream = request.GetRequestStream();
            //newStream.Write(postdata, 0, postdata.Length);
            //newStream.Close();
            request.Method = "GET"; 

            //ASCIIEncoding encoding = new ASCIIEncoding();
            //byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes("");
            //request.ContentLength = postdata.Length;
            //Stream newStream = request.GetRequestStream();
            //newStream.Write(postdata, 0, postdata.Length);
            //newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            JavaScriptSerializer json = new JavaScriptSerializer();
            AccessToken access = json.Deserialize<AccessToken>(content);

            return access.access_token;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Send Error = new Send(TipError);
            Send Success = new Send(TipSuccess);
            Response.Write(RequestData(Error, Success));
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
        /// 微信推送文本消息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public static string SendMassage(string appid, string appsecret, string msgContent, string wxopenid)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
                string result = APIGet(url);
                AccessToken access = JSONHelper.JSONToObject<AccessToken>(result);
                string senturl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access.access_token;
                string text = MassageTemplete.Text(msgContent, wxopenid);
                result = PostData(senturl, text);
                MessageModel msgModel = JSONHelper.JSONToObject<MessageModel>(result);
                result = msgModel.errmsg;
                return result;
            }
            catch (Exception ex)
            {
                return "error";
            }

        }
    }
}