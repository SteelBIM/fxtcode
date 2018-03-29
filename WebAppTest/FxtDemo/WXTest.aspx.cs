using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Web.Script.Serialization;
using System.Configuration;

namespace WebAppTest
{
    public partial class WXTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = "{\"access_token\":\"WymMgbwSI8Gjn3oQASXr0AZ2-wS1cWL3z1qjaECyQfIiXmRU9KcMYC1fVxDP6WmoAZJWfdMq25LX2KRoBrCVeIbPdyg9nZIgEh8UXhEYOGKmlGTpBhkSeCffCnR2OWvLEX6UddA4egOjh-w6hZw5fg\",\"expires_in\":7200}";
            JavaScriptSerializer json = new JavaScriptSerializer();
            AccessToken access = json.Deserialize<AccessToken>(str);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string type = DropDownList1.SelectedValue;
            string clickKey = DropDownList2.SelectedValue;
            string Content = string.Empty;

            if (type == "text")
            {
                Content = string.Format(StaticText.textMsg, DropDownList3.SelectedValue, ConvertDateTimeInt(DateTime.Now), sendTb.Text, DropDownList4.SelectedValue);
            }
            else if (type == "CLICK" || type == "subscribe" || type == "unsubscribe")
            {
                Content = string.Format(StaticText.eventMsg, DropDownList3.SelectedValue, ConvertDateTimeInt(DateTime.Now), type, clickKey, DropDownList4.SelectedValue);
            }
            else if (type =="location")
            {
                Content = string.Format(StaticText.locationMsg, DropDownList3.SelectedValue, ConvertDateTimeInt(DateTime.Now), DropDownList4.SelectedValue);
            }
            else if (type == "image")
            {
                Content = string.Format(StaticText.imgMsg, DropDownList3.SelectedValue, ConvertDateTimeInt(DateTime.Now), sendTb.Text, DropDownList4.SelectedValue);
            }

            string config =ConfigurationManager.AppSettings["fxtwxoa"];

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(config);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            MemoryStream memory = new MemoryStream();


            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(Content);
            request.ContentLength = postdata.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postdata, 0, postdata.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            acceptTb.Text = ConvertXMLToDataSet(content);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        private string GetAccess_Token(string url) 
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            MemoryStream memory = new MemoryStream();


            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(sendTb.Text);
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

        private string SendMessage(string url,string text)
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

        private static string ConvertXMLToDataSet(string xmlData)
        {
            string result = "";
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                DataTable dt = new DataTable();
                stream = new StringReader(xmlData);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                dt = xmlDS.Tables[0];
                if (dt.Rows[0]["MsgType"].ToString() =="text")
                {
                    result = dt.Rows[0]["Content"].ToString();
                }
                else if (dt.Rows[0]["MsgType"].ToString() =="news")
                {

                    for (int j = 0; j < xmlDS.Tables[2].Rows.Count; j++)
                    {

                        result += "Title:\n" + xmlDS.Tables[2].Rows[j]["Title"].ToString() + "\n Description:" +
                                xmlDS.Tables[2].Rows[j]["Description"].ToString() + "\n PicUrl:\n" +
                                xmlDS.Tables[2].Rows[j]["PicUrl"].ToString() + "\n Url:\n" +
                                xmlDS.Tables[2].Rows[j]["Url"].ToString();
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                string strTest = ex.Message;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return result;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx06993b306880fac5&secret=9afb1c46b1f26efb557cd6dd1261fd72";
            string access_token = GetAccess_Token(url);

            url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access_token;
            SendMessage(url,sendTb.Text);
            //string jsonStr = WXRequest.RequestUrl(url);
            //JsonObject newobj = new JsonObject(jsonStr);
            //string access_token = "";
            //try
            //{
            //    access_token = newobj["access_token"].Value;
            //}
            //catch (Exception)
            //{

            //    this.labmsg.Text = "错误编码:" + newobj["errcode"].Value + ",信息:" + newobj["errmsg"].Value;
            //    return;
            //}
            //if (!string.IsNullOrEmpty(access_token))
            //{

            //    string data = txtContent.Text.Trim();
            //    string posturl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token;
            //    string postStr = WXRequest.RequestPostUrl(posturl, data);
            //    JsonObject newpostobj = new JsonObject(postStr);
            //    if (newpostobj["errcode"].Value == "0")
            //    {
            //        this.labmsg.Text = "菜单生成成功!";
            //        return;
            //    }
            //    else
            //    {
            //        this.labmsg.Text = "错误编码:" + newpostobj["errcode"].Value + ",信息:" + newpostobj["errmsg"].Value;
            //        return;
            //    }

            //}
        }
    }

    public class StaticText
    {
        public static string textMsg = @" <xml>
                                         <ToUserName><![CDATA[{3}]]></ToUserName>
                                         <FromUserName><![CDATA[{0}]]></FromUserName> 
                                         <CreateTime>{1}</CreateTime>
                                         <MsgType><![CDATA[text]]></MsgType>
                                         <Content><![CDATA[{2}]]></Content>
                                         <MsgId>1234567890123456</MsgId>
                                         </xml>";
        public static string imgMsg = @"<xml>
                                     <ToUserName><![CDATA[{3}]]></ToUserName>
                                         <FromUserName><![CDATA[{0}]]></FromUserName> 
                                     <CreateTime>{1}</CreateTime>
                                     <MsgType><![CDATA[image]]></MsgType>
                                     <PicUrl><![CDATA[{2}]]></PicUrl>
                                     <MsgId>1234567890123456</MsgId>
                                     </xml>";
        public static string eventMsg = @"<xml><ToUserName><![CDATA[{4}]]></ToUserName>
                                         <FromUserName><![CDATA[{0}]]></FromUserName> 
                                        <CreateTime>{1}</CreateTime>
                                        <MsgType><![CDATA[event]]></MsgType>
                                        <Event><![CDATA[{2}]]></Event>
                                        <EventKey><![CDATA[{3}]]></EventKey>
                                        </xml>";
        public static string locationMsg = @"<xml><ToUserName><![CDATA[{2}]]></ToUserName>
                                         <FromUserName><![CDATA[{0}]]></FromUserName> 
                                        <CreateTime>{1}</CreateTime>
                                        <MsgType><![CDATA[location]]></MsgType>
                                        <Location_X>23.134521</Location_X>
                                        <Location_Y>113.358803</Location_Y>
                                        <Scale>20</Scale>
                                        <Label><![CDATA[位置信息]]></Label>
                                        <MsgId>1234567890123456</MsgId>
                                        </xml>";
    }


    public class AccessToken 
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
}