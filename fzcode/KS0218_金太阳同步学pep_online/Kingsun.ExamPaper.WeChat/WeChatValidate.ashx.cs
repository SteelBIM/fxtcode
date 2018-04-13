using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using Kingsun.ExamPaper.Wechat.api;
using Kingsun.ExamPaper.Wechat.Models;

namespace Kingsun.ExamPaper.WeChat
{
    /// <summary>
    /// WeChatValidate 的摘要说明
    /// </summary>
    public class WeChatValidate : IHttpHandler, IRequiresSessionState
    {
        readonly string _url = System.Configuration.ConfigurationManager.AppSettings["WeChatAddress"];
        readonly string _wxAppId = System.Configuration.ConfigurationManager.AppSettings["WXappid"];
        readonly string _wxsecret = System.Configuration.ConfigurationManager.AppSettings["WXsecret"];
        readonly CreateMenu _cm = new CreateMenu();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.HttpMethod.ToLower() == "post")
            {

                //回复消息的时候也需要验证消息，这个很多开发者没有注意这个，存在安全隐患  
                //微信中 谁都可以获取信息 所以 关系不大 对于普通用户 但是对于某些涉及到验证信息的开发非常有必要 
                if (CheckSignature())
                {
                    //var data = "{\"button\": [{\"name\":\"学习报告\", \"type\": \"view\",\"url\": \"#\"},{\"name\":\"班级管理\", \"type\": \"view\",\"url\": \"http://" + _url + "/ExWechat/ClassManagement/SignIn.aspx\"},{\"name\":\"敬请期待\", \"type\": \"view\",\"url\": \"#\"}]}";
                    //string i = _cm.MenuCreate(data, _wxAppId, _wxsecret);
                    //接收消息
                    ReceiveXml();
                }
                else
                {
                    HttpContext.Current.Response.Write("消息并非来自微信");
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                CheckWechat();
            }
        }

        #region 验证微信签名
        /// <summary>
        /// 返回随机数表示验证成功
        /// </summary>
        private void CheckWechat()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["echoStr"]))
            {
                var data = "{\"button\": [{\"name\":\"班级管理\", \"type\": \"view\",\"url\": \"http://" + _url + "/ExWechat/ClassManagement/SignIn.aspx\"},{\"name\":\"学习报告\", \"type\": \"view\",\"url\": \"http://" + _url + "/ExWechat/StudyReport/SignIn.aspx\"},{\"name\":\"修改\", \"type\": \"view\",\"url\": \"http://" + _url + "/ExWechat/ClassManagement/SelectRole.aspx\"}]}";
                string i = _cm.MenuCreate(data, _wxAppId, _wxsecret);
                HttpContext.Current.Response.Write("【】" + i);
                HttpContext.Current.Response.End();
            }
            string echoStr = HttpContext.Current.Request.QueryString["echoStr"];
            if (CheckSignature())
            {
                HttpContext.Current.Response.Write(echoStr);
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <returns></returns>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        private bool CheckSignature()
        {
            string access_token = "ClientWechat";

            string signature = HttpContext.Current.Request.QueryString["signature"].ToString();
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"].ToString();
            string nonce = HttpContext.Current.Request.QueryString["nonce"].ToString();
            string[] arrTmp = { access_token, timestamp, nonce };
            Array.Sort(arrTmp);     //字典排序
            string tmpStr = string.Join("", arrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");

            if (tmpStr != null && tmpStr.ToLower() == signature.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 接收消息
        /// <summary>
        /// 接收微信发送的XML消息并且解析
        /// </summary>
        private void ReceiveXml()
        {
            Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
            byte[] requestByte = new byte[requestStream.Length];
            requestStream.Read(requestByte, 0, (int)requestStream.Length);
            string requestStr = Encoding.UTF8.GetString(requestByte);

            if (!string.IsNullOrEmpty(requestStr))
            {
                //封装请求类
                XmlDocument requestDocXml = new XmlDocument();
                requestDocXml.LoadXml(requestStr);
                XmlElement rootElement = requestDocXml.DocumentElement;
                WxXmlModel wxXmlModel = new WxXmlModel
                {
                    ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText,
                    FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText,
                    CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText,
                    MsgType = rootElement.SelectSingleNode("MsgType").InnerText
                };

                switch (wxXmlModel.MsgType)
                {
                    case "text"://文本
                        wxXmlModel.Content = rootElement.SelectSingleNode("Content").InnerText;
                        break;
                    case "image"://图片
                        wxXmlModel.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                        break;
                    case "event"://事件
                        wxXmlModel.Event = rootElement.SelectSingleNode("Event").InnerText;
                        if (wxXmlModel.Event != "TEMPLATESENDJOBFINISH")//关注类型
                        {
                            wxXmlModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                        }
                        break;
                    default:
                        break;
                }

                ResponseXML(wxXmlModel);//回复消息
            }
        }
        #endregion

        #region 回复消息
        private void ResponseXML(WxXmlModel WxXmlModel)
        {
            QrCodeApi QrCodeApi = new QrCodeApi();
            string XML = "";
            switch (WxXmlModel.MsgType)
            {
                case "text"://文本回复
                    XML = ResponseMessage.GetText(WxXmlModel.FromUserName, WxXmlModel.ToUserName, WxXmlModel.Content);
                    break;
                case "event":
                    switch (WxXmlModel.Event)
                    {
                        case "subscribe":
                            if (string.IsNullOrEmpty(WxXmlModel.EventKey))
                            {
                                XML = ResponseMessage.GetText(WxXmlModel.FromUserName, WxXmlModel.ToUserName, "关注成功");
                            }
                            else
                            {
                                XML = ResponseMessage.SubScanQrcode(WxXmlModel.FromUserName, WxXmlModel.ToUserName, WxXmlModel.EventKey);//扫描带参数二维码先关注后推送事件
                            }
                            break;
                        case "SCAN":
                            XML = ResponseMessage.ScanQrcode(WxXmlModel.FromUserName, WxXmlModel.ToUserName, WxXmlModel.EventKey);//扫描带参数二维码已关注 直接推送事件
                            break;
                        case "unsubscribe":
                            XML = ResponseMessage.GetText(WxXmlModel.FromUserName, WxXmlModel.ToUserName, "取消关注成功");
                            break;
                    }
                    break;
                default://默认回复
                    break;
            }
            HttpContext.Current.Response.Write(XML);
            HttpContext.Current.Response.End();
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}