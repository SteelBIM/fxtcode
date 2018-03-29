using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;
using CAS.Entity.DBEntity;
using IOSPush;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {
        //对外方法名：mp_func_1 参数名：username ,entrustid,producttypecode,phshuserid,channelid,splatype
        public WCFJsonData MobilePushBind(string sinfo, string info)
        {
            return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "该功能已删除");
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
            WCFJsonData result = null;
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
                if (((lstpush.FirstOrDefault().andphshuserid != "" && lstpush.FirstOrDefault().andphshuserid != mobilepush.phshuserid))
                || ((lstpush.FirstOrDefault().iosphshuserid != "" && lstpush.FirstOrDefault().phshuserid != mobilepush.phshuserid)))
                {
                    string poushresult = "", exitmes = "{\"pushtype\": \"exit\",\"data\": { \"key\": \"您的账号在另一设备登陆\"} }";
                    DatPushMessage pushrecord = new DatPushMessage();
                    pushrecord.id = lstpush.FirstOrDefault().id;
                    //推送下线消息
                    if (mobilepush.splatype.ToLower() == "ios")
                    {
                        poushresult = BaiduPush.PushSend.Send("退出", "您的账号在另一设备登陆", lstpush.FirstOrDefault().channelid, lstpush.FirstOrDefault().andphshuserid, exitmes, stype, ref sendsuccess);
                        result = JSONHelper.GetWcfJson(poushresult, (int)EnumHelper.Status.Success, (sendsuccess == 1) ? "推送下线消息成功" : "推送下线消息失败");
                        pushrecord.title = "推送下线消息给Android" + lstpush.FirstOrDefault().andphshuserid;
                        pushrecord.result = poushresult;
                    }
                    else if (!string.IsNullOrEmpty(lstpush.FirstOrDefault().iosphshuserid)) //android
                    {
                        string cearpath = "yck.p12";
                        if (stype != "yfk")
                        {
                            cearpath = "yfk.p12";
                        }
                        try
                        {
                            poushresult = PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, "退出", cearpath, ref sendsuccess, exitmes);
                        }
                        catch (Exception ex)
                        {
                            poushresult = ex.Message;
                            LogHelper.Error(ex);
                        }
                        pushrecord.title = "推送下线消息给IOS" + lstpush.FirstOrDefault().iosphshuserid;
                        pushrecord.result = poushresult;
                    }

                    if (!string.IsNullOrEmpty(pushrecord.result))
                        DatPushMessageBL.Add(pushrecord);
                }
            }
            int bindid = MobilePushBL.Bind(mobilepush);
            if (bindid > 0) result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "绑定成功");
            else result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "绑定失败");
            return result;

        }
        //对外方法名：mp_func_2 参数名：tag ,username,title,neirong,producttypecode,entrustid
        public WCFJsonData MobilePushSend(string sinfo, string info)
        {
            return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "该功能已删除");
            var uInfo = JObject.Parse(info)["uinfo"];
            var appInfo = JObject.Parse(info)["appinfo"];
            var funInfo = JObject.Parse(info)["funinfo"];
            string tag = (funInfo["tag"] == null) ? "" : funInfo["tag"].ToString();
            string username = (funInfo["username"] == null) ? "" : funInfo["username"].ToString();//消息推送用户并非为登录用户，从funInfo里面获取 caoq 2014-4-8
            string title = (funInfo["title"] == null) ? "" : funInfo["title"].ToString();
            string neirong = (funInfo["neirong"] == null) ? "" : funInfo["neirong"].ToString();
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
            WCFJsonData result = null;
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
                        pushresult = BaiduPush.PushSend.Send(title, neirong, item.channelid, item.andphshuserid, entrustid, stype, ref sendsuccess);
                        pushrecord.createdate = DateTime.Now;
                        pushrecord.pushid = item.id;
                        pushrecord.title = title;
                        pushrecord.neirong = neirong;
                        pushrecord.result = pushresult;

                        if (!string.IsNullOrEmpty(pushrecord.result))
                            DatPushMessageBL.Add(pushrecord);
                        if (sendsuccess == 1)
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
                        string cearpath = "yck.pl2";
                        if (stype != "yfk")
                        {
                            cearpath = "yfk.pl2";
                        }
                        try
                        {
                            pushresult = PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, neirong, cearpath, ref sendsuccess, entrustid);
                        }
                        catch (Exception ex)
                        {
                            pushresult = ex.Message;
                            LogHelper.Error(ex);
                        }
                        pushrecord.createdate = DateTime.Now;
                        pushrecord.pushid = item.id;
                        pushrecord.title = title;
                        pushrecord.neirong = neirong;
                        pushrecord.result = pushresult;

                        if (!string.IsNullOrEmpty(pushrecord.result))
                            DatPushMessageBL.Add(pushrecord);
                        result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
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
            return result;
        }
        //对外方法名：mp_func_3 参数名：username,channelid,phshuserid,splatype,producttypecode
        public WCFJsonData MobilePushEdit(string sinfo, string info)
        {
            return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "该功能已删除");
            var uInfo = JObject.Parse(info)["uinfo"];
            var appInfo = JObject.Parse(info)["appinfo"];
            var funInfo = JObject.Parse(info)["funinfo"];
            var username = uInfo["username"].ToString();
            string channelid = funInfo["channelid"].ToString();
            string phshuserid = funInfo["phshuserid"].ToString();
            var splatype = appInfo["splatype"].ToString();
            int producttypecode = StringHelper.TryGetInt(appInfo["systypecode"].ToString());
            if (producttypecode <= 0)
            {
                return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "缺少必传参数producttypecode");
            }
            WCFJsonData result = null;
            DatMobilePush mobilepush = null;
            mobilepush = new DatMobilePush();
            mobilepush.username = username;
            mobilepush.channelid = channelid;
            mobilepush.phshuserid = phshuserid;
            mobilepush.splatype = splatype;
            mobilepush.producttypecode = producttypecode;
            int clearid = MobilePushBL.Bind(mobilepush);
            if (clearid > 0)
            {
                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "退出成功");
            }
            else
            {
                result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "退出失败");
            }
            return result;
        }

    }
}
