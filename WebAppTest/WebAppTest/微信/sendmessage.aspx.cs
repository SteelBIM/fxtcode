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
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret="+appSecret+"";
            string result = GetAccess_Token(url);
            string senturl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + result;
            string text = "{\"touser\":\"" + txtwxopenid.Text+ "\",\"msgtype\":\"text\",\"text\":{\"content\":\"预评完成：【房地产评估】“w”编号为房讯通估字(G)(2014)第06011号，评估总值:500.00万元，总税费:5000.00元，总净值:499.50万元。------------------请知会:某某 联系方式（在微信“服务记录”中查看其它记录）\"}}";
            string wxopenid = SendMessage(senturl, text);
            JavaScriptSerializer json = new JavaScriptSerializer();
            MessageModel msg = json.Deserialize<MessageModel>(wxopenid);
            if (msg.errmsg=="ok")
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
            request.Method = "POST";

            MemoryStream memory = new MemoryStream();


            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes("");
            request.ContentLength = postdata.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

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
}