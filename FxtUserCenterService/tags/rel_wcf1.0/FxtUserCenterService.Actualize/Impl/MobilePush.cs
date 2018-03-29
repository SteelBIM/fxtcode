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
            var uInfo = JObject.Parse(info)["uinfo"];
            var appInfo = JObject.Parse(info)["appinfo"];
            var funInfo = JObject.Parse(info)["funinfo"];
            var username = uInfo["username"].ToString();
            //long entrustid = StringHelper.TryGetLong(funInfo["entrustid"].ToString());
            int producttypecode = StringHelper.TryGetInt(appInfo["systypecode"].ToString());
            string phshuserid = funInfo["phshuserid"].ToString();
            string channelid = funInfo["channelid"].ToString();
            string pushappid = funInfo["pushappid"].ToString();
            var splatype = appInfo["splatype"].ToString();
            WCFJsonData result = null;
            DatMobilePush mobilepush = null;
            List<DatMobilePush> lstpush = null;
            mobilepush = new DatMobilePush();
            mobilepush.username = username;
            mobilepush.producttypecode = producttypecode;
            mobilepush.phshuserid = phshuserid == null ? "" : phshuserid.Trim();
            mobilepush.channelid = channelid == null ? "" : channelid.Trim();
            mobilepush.splatype = splatype;
            mobilepush.pushappid = pushappid;
            lstpush = MobilePushBL.GetCheckUser(username, producttypecode);
            if (lstpush != null && lstpush.Count > 0)
            {
                if (((lstpush.FirstOrDefault().andphshuserid != "" && lstpush.FirstOrDefault().andphshuserid != mobilepush.phshuserid))
                || ((lstpush.FirstOrDefault().iosphshuserid != "" && lstpush.FirstOrDefault().phshuserid != mobilepush.phshuserid)))
                {
                    //推送下线消息
                    if (mobilepush.splatype.ToLower() == "ios")
                    {
                        result = JSONHelper.GetWcfJson(BaiduPush.PushSend.Send("退出", "退出", lstpush.FirstOrDefault().channelid, lstpush.FirstOrDefault().andphshuserid, -1), (int)EnumHelper.Status.Success, "成功");
                    }
                    else if (!string.IsNullOrEmpty(lstpush.FirstOrDefault().iosphshuserid)) //android
                    {
                        string cearpath = "yck.pl2";
                        if (producttypecode ==(int)EnumHelper.Codes.SysTypeCodeCMB_Bank)
                        {
                            cearpath = "yfk.pl2";
                        }
                        PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, "退出", cearpath);

                    }
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
            var uInfo = JObject.Parse(info)["uinfo"];
            var appInfo = JObject.Parse(info)["appinfo"];
            var funInfo = JObject.Parse(info)["funinfo"];

            string tag = (funInfo["tag"]==null) ? "" : funInfo["tag"].ToString();
            string username = funInfo["username"].ToString();//消息推送用户并非为登录用户，从funInfo里面获取 caoq 2014-4-8
            string title = (funInfo["title"] == null) ? "" : funInfo["title"].ToString();
            string neirong = funInfo["neirong"].ToString();
            string producttypecode = appInfo["systypecode"].ToString();
            long entrustid = StringHelper.TryGetLong(funInfo["entrustid"].ToString());
         
            WCFJsonData result = null;
            List<DatMobilePush> lstpush = null;

            if (tag == "")
            {

                lstpush = MobilePushBL.GetCheckUser(username, StringHelper.TryGetInt(producttypecode));
                if (lstpush != null && lstpush.Count > 0)
                {
                    DatMobilePush item = lstpush.FirstOrDefault();//获取推送绑定信息
                    if (!string.IsNullOrEmpty(item.andphshuserid))
                    {
                        string pushresult = BaiduPush.PushSend.Send(title, neirong, item.channelid, item.andphshuserid, entrustid);
                        if (pushresult.IndexOf("Response:") != -1)
                        {
                            pushresult = pushresult.Substring(pushresult.IndexOf("Response:") + "Response:".Length);
                        }
                        JObject pushobj = JObject.Parse(pushresult);
                        string pusmes = pushobj["response_params"].ToString();
                        pushobj = JObject.Parse(pusmes);
                        if (!string.IsNullOrEmpty(pushobj["success_amount"].ToString()) &&
                            StringHelper.TryGetInt(pushobj["success_amount"].ToString()) > 0)
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
                        if (producttypecode == ((int)EnumHelper.Codes.SysTypeCodeCMB_Bank).ToString())
                        {
                            cearpath = "yfk.pl2";
                        }
                        PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, neirong, cearpath);
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
                result = JSONHelper.GetWcfJson(BaiduPush.PushSend.Send(title, neirong, "", "", entrustid, tag),(int)EnumHelper.Status.Success, "成功");
                //result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
            }

            return result;

        }

        //对外方法名：mp_func_3 参数名：username,channelid,phshuserid,splatype,producttypecode
        public WCFJsonData MobilePushEdit(string sinfo, string info)
        {
            var uInfo = JObject.Parse(info)["uinfo"];
            var appInfo = JObject.Parse(info)["appinfo"];
            var funInfo = JObject.Parse(info)["funinfo"];

           
            var username = uInfo["username"].ToString();
            string channelid = funInfo["channelid"].ToString();
            string phshuserid = funInfo["phshuserid"].ToString();
            var splatype = appInfo["splatype"].ToString();
            string producttypecode = appInfo["systypecode"].ToString();
           

            WCFJsonData result = null;
            DatMobilePush mobilepush = null;
        
            mobilepush = new DatMobilePush();
            mobilepush.username = username;
            mobilepush.channelid = channelid;
            mobilepush.phshuserid = phshuserid;
            mobilepush.splatype = splatype;
            mobilepush.producttypecode = StringHelper.TryGetInt(producttypecode);
            int clearid = MobilePushBL.Bind(mobilepush);
            if (clearid > 0) result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "退出成功");
            else result = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "退出失败");

            return result;

        }
    }
}
