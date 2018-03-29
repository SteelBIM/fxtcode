using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using CAS.Common;
using CAS.Entity;
using CAS.Entity.DBEntity;
using FxtUserCenterService.Logic;
using FxtUserCenterService.Entity;
using IOSPush;
using System.Xml;
using System.Collections;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Web;

namespace FxtUserCenterService.Actualize.Impl
{
    /// <summary>
    /// 个推
    /// </summary>
    public partial class Implement
    {
        /// <summary>
        /// 个推绑定 如果存在此用户的绑定信息且值不同则推送退出消息
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData MobileGeTuiBind(string sinfo, string info)
        {
            WCFJsonData result = null;
            try
            {
                var uInfo = JObject.Parse(info)["uinfo"];
                var appInfo = JObject.Parse(info)["appinfo"];
                var funInfo = JObject.Parse(info)["funinfo"];
                var username = uInfo["username"].ToString();
                //long entrustid = StringHelper.TryGetLong(funInfo["entrustid"].ToString());
                int producttypecode = StringHelper.TryGetInt(appInfo["systypecode"].ToString());
                if (producttypecode <= 0)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "缺少必传参数producttypecode");
                }
                string stype = appInfo["stype"].ToString();
                string phshuserid = funInfo["phshuserid"].ToString();
                string channelid = funInfo["channelid"].ToString();
                string pushappid = (funInfo["pushappid"] == null) ? "" : funInfo["pushappid"].ToString();
                var splatype = appInfo["splatype"].ToString();
                int sendsuccess = 0;

                DatMobilePush mobilepush = null;
                List<DatMobilePush> lstpush = null;
                mobilepush = new DatMobilePush();
                mobilepush.username = username;
                mobilepush.producttypecode = producttypecode;
                mobilepush.phshuserid = phshuserid == null ? "" : phshuserid.Trim();
                mobilepush.channelid = channelid == null ? "" : channelid.Trim();
                mobilepush.splatype = splatype;
                mobilepush.token = (uInfo["token"] == null) ? "" : uInfo["token"].ToString();
                mobilepush.pushappid = pushappid;

                lstpush = MobilePushBL.GetCheckUser(username, 0);
                if (lstpush != null && lstpush.Count > 0)
                {
                    if (mobilepush.splatype.ToLower() == "android")
                    {//android
                        string poushresult = "", exitmes = "{\"pushtype\": \"exit\",\"data\": { \"pushtext\": \"您的账号在另一android设备登录！\"} }";
                        DatPushMessage pushrecord = new DatPushMessage();
                        pushrecord.pushid = lstpush.FirstOrDefault().id;
                        //推送下线消息
                        if (lstpush.FirstOrDefault().andphshuserid != "" && lstpush.FirstOrDefault().andphshuserid != mobilepush.phshuserid)
                        {//上次登录为android 
                            //poushresult = BaiduPush.PushSend.Send("退出", "您的账号在另一设备登陆", lstpush.FirstOrDefault().channelid, lstpush.FirstOrDefault().andphshuserid, exitmes, stype, ref sendsuccess);
                            poushresult = PushMessageToSingle(lstpush.FirstOrDefault().andphshuserid, "退出提示", "您的账号在另一Android设备登录！", exitmes);
                            //{"taskId":"OSS-0722_8cml3GmV5H6wiTKFqRttb5","result":"ok","status":"successed_online"}

                            string pushresult = poushresult.ToLower();
                            if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                                sendsuccess = 1;
                            result = JSONHelper.GetWcfJson(poushresult, (int)EnumHelper.Status.Success, (sendsuccess == 1) ? "推送下线消息成功" : "推送下线消息失败");
                            pushrecord.title = "推送下线消息给Android" + lstpush.FirstOrDefault().andphshuserid;
                            pushrecord.result = poushresult;
                        }
                        else if (lstpush.FirstOrDefault().iosphshuserid != "")
                        {//上传登录为ios 
                            try
                            {
                                poushresult = IOSPushMessageToSingle(lstpush.FirstOrDefault().iosphshuserid, "退出", "您的账号在另一Android设备登陆", exitmes);

                                string pushresult = poushresult.ToLower();
                                if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                                    sendsuccess = 1;
                                result = JSONHelper.GetWcfJson(poushresult, (int)EnumHelper.Status.Success, (sendsuccess == 1) ? "推送下线消息成功" : "推送下线消息失败");
                                pushrecord.title = "推送下线消息给Android" + lstpush.FirstOrDefault().iosphshuserid;
                                pushrecord.result = poushresult;
                                //string cearpath = "yck.p12";
                                //if (stype != "yfk")
                                //{
                                //    cearpath = "yfk.p12";
                                //}
                                //try
                                //{
                                //    poushresult = PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, "退出", cearpath, ref sendsuccess, exitmes);
                                //}
                                //catch (Exception ex)
                                //{
                                //    poushresult = ex.Message;
                                //    LogHelper.Error(ex);
                                //}
                                //pushrecord.title = "推送下线消息给IOS" + lstpush.FirstOrDefault().iosphshuserid;
                                //pushrecord.result = poushresult;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (!string.IsNullOrEmpty(pushrecord.result))
                            DatPushMessageBL.Add(pushrecord);
                    }
                    else if (mobilepush.splatype.ToLower() == "ios")
                    { //ios
                        string poushresult = "", exitmes = "{\"pushtype\": \"exit\",\"data\": { \"pushtext\": \"您的账号在另一IOS设备登陆\"} }";
                        DatPushMessage pushrecord = new DatPushMessage();
                        pushrecord.pushid = lstpush.FirstOrDefault().id;
                        //推送下线消息
                        if (lstpush.FirstOrDefault().iosphshuserid != "" && lstpush.FirstOrDefault().iosphshuserid != mobilepush.phshuserid)
                        {//上次登录为ios 
                            try
                            {
                                poushresult = IOSPushMessageToSingle(lstpush.FirstOrDefault().iosphshuserid, "退出", "您的账号在另一IOS设备登陆", exitmes);

                                string pushresult = poushresult.ToLower();
                                if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                                    sendsuccess = 1;
                                result = JSONHelper.GetWcfJson(poushresult, (int)EnumHelper.Status.Success, (sendsuccess == 1) ? "推送下线消息成功" : "推送下线消息失败");
                                pushrecord.title = "推送下线消息给IOS" + lstpush.FirstOrDefault().iosphshuserid;
                                pushrecord.result = poushresult;
                                //string cearpath = "yck.p12";
                                //if (stype != "yfk")
                                //{
                                //    cearpath = "yfk.p12";
                                //}
                                //try
                                //{
                                //    poushresult = PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, "退出", cearpath, ref sendsuccess, exitmes);
                                //}
                                //catch (Exception ex)
                                //{
                                //    poushresult = ex.Message;
                                //    LogHelper.Error(ex);
                                //}
                                //pushrecord.title = "推送下线消息给IOS" + lstpush.FirstOrDefault().iosphshuserid;
                                //pushrecord.result = poushresult;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else if (lstpush.FirstOrDefault().andphshuserid != "")
                        {//上传登录为ios 
                            //poushresult = BaiduPush.PushSend.Send("退出", "您的账号在另一设备登陆", lstpush.FirstOrDefault().channelid, lstpush.FirstOrDefault().andphshuserid, exitmes, stype, ref sendsuccess);
                            poushresult = PushMessageToSingle(lstpush.FirstOrDefault().andphshuserid, "退出", "您的账号在另一IOS设备登陆", exitmes);

                            string pushresult = poushresult.ToLower();
                            if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                                sendsuccess = 1;
                            result = JSONHelper.GetWcfJson(poushresult, (int)EnumHelper.Status.Success, (sendsuccess == 1) ? "推送下线消息成功" : "推送下线消息失败");
                            pushrecord.title = "推送下线消息给Android" + lstpush.FirstOrDefault().andphshuserid;
                            pushrecord.result = poushresult;
                        }

                        if (!string.IsNullOrEmpty(pushrecord.result))
                            DatPushMessageBL.Add(pushrecord);
                    }
                }
                int bindid = MobilePushBL.Bind(mobilepush);
                if (bindid > 0) result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "绑定成功");
                else result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "绑定失败");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
            }
            return result;

        }
        /// <summary>
        /// 个推信息发送
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData MobileGeTuiSend(string sinfo, string info)
        {
            WCFJsonData result = null;
            try
            {
                var uInfo = JObject.Parse(info)["uinfo"];
                var appInfo = JObject.Parse(info)["appinfo"];
                var funInfo = JObject.Parse(info)["funinfo"];
                string tag = (funInfo["tag"] == null) ? "" : funInfo["tag"].ToString();
                string username = (funInfo["username"] == null) ? "" : funInfo["username"].ToString();//消息推送用户并非为登录用户，从funInfo里面获取 caoq 2014-4-8
                string title = (funInfo["title"] == null) ? "" : funInfo["title"].ToString();
                string neirong = (funInfo["neirong"] == null) ? "" : funInfo["neirong"].ToString();
                string content = (funInfo["content"] == null) ? "" : funInfo["content"].ToString();//透传内容
                int producttypecode = StringHelper.TryGetInt(appInfo["systypecode"].ToString());
                if (producttypecode <= 0)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "缺少必传参数producttypecode");
                }
                if (string.IsNullOrEmpty(username))
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "缺少必传参数funinfo:username");
                }
                string entrustid = (funInfo["entrustid"] == null) ? "" : funInfo["entrustid"].ToString();
                string stype = appInfo["stype"].ToString();
                List<DatMobilePush> lstpush = null;
                DatPushMessage pushrecord = new DatPushMessage();
                int sendsuccess = 0;
                if (tag == "")
                {
                    lstpush = MobilePushBL.GetCheckUser(username, 0);
                    if (lstpush != null && lstpush.Count > 0)
                    {
                        DatMobilePush item = lstpush.FirstOrDefault();//获取推送绑定信息
                        string pushresult = "";
                        if (!string.IsNullOrEmpty(item.andphshuserid))
                        {
                            //pushresult = BaiduPush.PushSend.Send(title, neirong, item.channelid, item.andphshuserid, entrustid, stype, ref sendsuccess);
                            pushresult = PushMessageToSingle(item.andphshuserid, title, neirong, content);
                            pushrecord.createdate = DateTime.Now;
                            pushrecord.pushid = item.id;
                            pushrecord.title = title;
                            pushrecord.neirong = neirong;
                            pushrecord.result = pushresult;
                            if (!string.IsNullOrEmpty(pushrecord.result))
                                DatPushMessageBL.Add(pushrecord);

                            pushresult = pushresult.ToLower();
                            if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                            {
                                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
                            }
                            else
                            {
                                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "推送失败");
                            }
                        }
                        else if (!string.IsNullOrEmpty(item.iosphshuserid))
                        {
                            pushresult = IOSPushMessageToSingle(item.iosphshuserid, title, neirong, content);
                            pushrecord.createdate = DateTime.Now;
                            pushrecord.pushid = item.id;
                            pushrecord.title = title;
                            pushrecord.neirong = neirong;
                            pushrecord.result = pushresult;
                            if (!string.IsNullOrEmpty(pushresult))
                                DatPushMessageBL.Add(pushrecord);
                            pushresult = pushresult.ToLower();
                            if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                            {
                                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
                            }
                            else
                            {
                                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "推送失败");
                            }

                            //string cearpath = "yck.pl2";
                            //if (stype != "yfk")
                            //{
                            //    cearpath = "yfk.pl2";
                            //}
                            //try
                            //{
                            //    pushresult = PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, neirong, cearpath, ref sendsuccess, entrustid);
                            //}
                            //catch (Exception ex)
                            //{
                            //    pushresult = ex.Message;
                            //    LogHelper.Error(ex);
                            //}
                            //pushrecord.createdate = DateTime.Now;
                            //pushrecord.pushid = item.id;
                            //pushrecord.title = title;
                            //pushrecord.neirong = neirong;
                            //pushrecord.result = pushresult;

                            //if (!string.IsNullOrEmpty(pushrecord.result))
                            //    DatPushMessageBL.Add(pushrecord);
                            //result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
                        }
                    }
                    else
                    {
                        result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "没有找到绑定信息");
                    }
                }
                else
                {
                    //按组推送
                    result = JSONHelper.GetWcfJson(BaiduPush.PushSend.Send(title, neirong, "", "", entrustid, stype, ref sendsuccess, tag), (int)EnumHelper.Status.Success, "成功");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 个推信息发送 不需要验证
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData GeTuiSend(string info)
        {
            WCFJsonData result = null;
            try
            {
                var funInfo = JObject.Parse(info);
                string username = (funInfo["username"] == null) ? "" : funInfo["username"].ToString();//消息推送用户并非为登录用户，从funInfo里面获取 caoq 2014-4-8
                string title = (funInfo["title"] == null) ? "" : funInfo["title"].ToString();
                string neirong = (funInfo["neirong"] == null) ? "" : funInfo["neirong"].ToString();
                string content = (funInfo["content"] == null) ? "" : funInfo["content"].ToString();//透传内容
                if (string.IsNullOrEmpty(username))
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "缺少必传参数username");
                }
                string entrustid = (funInfo["entrustid"] == null) ? "" : funInfo["entrustid"].ToString();
                string stype = funInfo["stype"].ToString();
                List<DatMobilePush> lstpush = null;
                DatPushMessage pushrecord = new DatPushMessage();
                lstpush = MobilePushBL.GetCheckUser(username, 0);
                if (lstpush != null && lstpush.Count > 0)
                {
                    DatMobilePush item = lstpush.FirstOrDefault();//获取推送绑定信息
                    string pushresult = "";
                    if (!string.IsNullOrEmpty(item.andphshuserid))
                    {
                        pushresult = PushMessageToSingle(item.andphshuserid, title, neirong, content);
                        pushrecord.createdate = DateTime.Now;
                        pushrecord.pushid = item.id;
                        pushrecord.title = title;
                        pushrecord.neirong = neirong;
                        pushrecord.result = pushresult;
                        if (!string.IsNullOrEmpty(pushresult))
                            DatPushMessageBL.Add(pushrecord);

                        pushresult = pushresult.ToLower();
                        if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                        {
                            result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
                        }
                        else
                        {
                            result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "推送失败");
                        }
                    }
                    else if (!string.IsNullOrEmpty(item.iosphshuserid))
                    {
                        pushresult = IOSPushMessageToSingle(item.iosphshuserid, title, neirong, content);
                        pushrecord.createdate = DateTime.Now;
                        pushrecord.pushid = item.id;
                        pushrecord.title = title;
                        pushrecord.neirong = neirong;
                        pushrecord.result = pushresult;
                        if (!string.IsNullOrEmpty(pushresult))
                            DatPushMessageBL.Add(pushrecord);
                        pushresult = pushresult.ToLower();
                        if (pushresult.Contains("successed_offline") || pushresult.Contains("successed_online") || pushresult.Contains("ok"))
                        {
                            result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
                        }
                        else
                        {
                            result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "推送失败");
                        }
                        //int sendsuccess = 0;
                        //string cearpath = "yck.pl2";
                        //if (stype != "yfk")
                        //{
                        //    cearpath = "yfk.pl2";
                        //}
                        //try
                        //{
                        //    pushresult = PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, neirong, cearpath, ref sendsuccess, entrustid);
                        //}
                        //catch (Exception ex)
                        //{
                        //    pushresult = ex.Message;
                        //    LogHelper.Error(ex);
                        //}
                        //pushrecord.createdate = DateTime.Now;
                        //pushrecord.pushid = item.id;
                        //pushrecord.title = title;
                        //pushrecord.neirong = neirong;
                        //pushrecord.result = pushresult;
                        //if (!string.IsNullOrEmpty(pushresult))
                        //    DatPushMessageBL.Add(pushrecord);
                        //result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
                    }
                }
                else
                {
                    result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "没有找到绑定信息");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 推送单对单 调用预处理文件
        /// </summary>
        /// <param name="andphshuserid"></param>
        /// <param name="title"></param>
        /// <param name="neirong"></param>
        /// <param name="tc"></param>
        /// <returns></returns>
        private string PushMessageToSingle(string andphshuserid, string title, string neirong, string content)
        {
            string getuiurl = System.Configuration.ConfigurationManager.AppSettings["getui"];
            if (!string.IsNullOrEmpty(getuiurl))
            {
                string gettokenurl = getuiurl + "?CLIENTID=" + andphshuserid + "&title=" + title + "&neirong=" + neirong + "&content=" + HttpUtility.UrlEncode(content);
                return GetRequestGet(gettokenurl);
            }
            else
            {
                LogHelper.Info("个推网站配置错误!");
                return "";
            }

        }
        /// <summary>
        /// 推送单对单 调用预处理文件
        /// </summary>
        /// <param name="andphshuserid"></param>
        /// <param name="title"></param>
        /// <param name="neirong"></param>
        /// <param name="tc"></param>
        /// <returns></returns>
        private string IOSPushMessageToSingle(string andphshuserid, string title, string neirong, string content)
        {
            string getuiurl = System.Configuration.ConfigurationManager.AppSettings["getui"];
            if (!string.IsNullOrEmpty(getuiurl))
            {
                string gettokenurl = getuiurl + "?CLIENTID=" + andphshuserid + "&title=" + title + "&type=ios"
                    + "&neirong=" + neirong + "&content=" + HttpUtility.UrlEncode(content);
                return GetRequestGet(gettokenurl);
            }
            else
            {
                LogHelper.Info("个推网站配置错误!");
                return "";
            }

        }
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetRequestGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            return content.Trim();
        }
    }
}
