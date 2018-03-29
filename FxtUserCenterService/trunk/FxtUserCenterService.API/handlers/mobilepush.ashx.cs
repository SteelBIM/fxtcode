using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtUserCenterService.Entity;
using FxtUserCenterService.Logic;
using CAS.Entity.DBEntity;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using IOSPush;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// mobilepush 的摘要说明
    /// 手机推送API hell
    /// </summary>
    public class mobilepush : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "type" })) return;
            string result = "";
            string username = GetRequest("username");
            int companyid = StringHelper.TryGetInt(GetRequest("companyid"));
            long entrustid = StringHelper.TryGetLong(GetRequest("entrustid"));
            string type = GetRequest("type");
            List<DatMobilePush> lstpush = null;
            DatMobilePush mobilepush = null;
            try
            {
                switch (type)
                {
                    case "bind":
                        if (!CheckMustRequest(new string[] { "username","producttypecode", "splatype" })) return;
                        int producttypecode = StringHelper.TryGetInt(GetRequest("producttypecode"));
                        string phshuserid = GetRequest("phshuserid");
                        string channelid = GetRequest("channelid");
                        mobilepush = new DatMobilePush();
                        mobilepush.username = GetRequest("username");
                        mobilepush.producttypecode = producttypecode;
                        mobilepush.phshuserid = phshuserid == null ? "" : phshuserid.Trim();
                        mobilepush.channelid = channelid == null ? "" : channelid.Trim();
                        mobilepush.splatype = GetRequest("splatype");
                        lstpush = MobilePushBL.GetCheckUser(GetRequest("username"),producttypecode);
                        if (lstpush != null && lstpush.Count > 0)
                        {
                            if (((lstpush.FirstOrDefault().andphshuserid != "" &&lstpush.FirstOrDefault().andphshuserid != mobilepush.phshuserid))
                            || ((lstpush.FirstOrDefault().iosphshuserid != "" &&lstpush.FirstOrDefault().phshuserid != mobilepush.phshuserid)))
                            {
                                //推送下线消息
                                if (mobilepush.splatype == "ios")
                                {
                                    result = BaiduPush.PushSend.Send("退出", "退出", lstpush.FirstOrDefault().channelid, lstpush.FirstOrDefault().andphshuserid, -1);
                                }
                                else if (!string.IsNullOrEmpty(lstpush.FirstOrDefault().iosphshuserid)) //android
                                {
                                    string cearpath = "yck.pl2";
                                    if (producttypecode == (int)EnumHelper.Codes.SysTypeCodeCMB_Bank)
                                    {
                                        cearpath = "yfk.pl2";
                                    }
                                    PushNotificationForIos.pushNotifications(lstpush.FirstOrDefault().iosphshuserid, "退出", cearpath);
                                }
                            }
                        }
                        int bindid = MobilePushBL.Bind(mobilepush);
                        if (bindid > 0) result = GetJson(1, "绑定成功");
                        else result = GetJson(0, "绑定失败");
                        break;
                    case "send":
                        string tag = (string.IsNullOrEmpty(GetRequest("tag")) ? "" :GetRequest("tag"));
                        //是否按组推送
                        if (tag == "")
                        {
                            if (!CheckMustRequest(new string[] { "username", "neirong", "entrustid", "producttypecode" })) return;
                            lstpush = MobilePushBL.GetCheckUser(GetRequest("username"), StringHelper.TryGetInt(GetRequest("producttypecode")));
                            if (lstpush != null && lstpush.Count > 0)
                            {
                                DatMobilePush item = lstpush.FirstOrDefault();//获取推送绑定信息
                                if (!string.IsNullOrEmpty(item.andphshuserid))
                                {
                                    string pushresult = BaiduPush.PushSend.Send(GetRequest("title"), GetRequest("neirong"), item.channelid, item.andphshuserid, entrustid);
                                    if (pushresult.IndexOf("Response:")!=-1)
                                    {
                                        pushresult = pushresult.Substring(pushresult.IndexOf("Response:"));
                                    }
                                    JObject pushobj=JObject.Parse(pushresult);
                                    string pusmes = pushobj["response_params"].ToString();
                                    pushobj = JObject.Parse(pusmes);
                                    if (!string.IsNullOrEmpty(pushobj["success_amount"].ToString()) &&
                                        StringHelper.TryGetInt(pushobj["success_amount"].ToString())>0)
                                    {
                                        result = GetJson(1);
                                    }
                                    else
                                    {
                                        result = GetJson(-1, "推送失败");
                                    }
                                }
                                else if (!string.IsNullOrEmpty(item.iosphshuserid))
                                {
                                    PushNotificationForIos.pushNotifications(item.iosphshuserid, GetRequest("neirong"), "aps_developer_identity.p12");
                                    result = GetJson(1);
                                }
                            }
                            else
                            {
                                result = GetJson(0, "没有找到绑定信息");
                            }
                        }
                        else
                        {
                            //按组推送
                            if (!CheckMustRequest(new string[] { "neirong" })) return;
                            result = BaiduPush.PushSend.Send(GetRequest("title"),GetRequest("neirong"), "", "", entrustid, tag);
                            result = GetJson(1, result);
                        }
                        break;
                    case "exit":
                        mobilepush = new DatMobilePush();
                        mobilepush.username = GetRequest("username");
                        mobilepush.channelid = GetRequest("channelid");
                        mobilepush.phshuserid = GetRequest("phshuserid");
                        mobilepush.splatype = GetRequest("splatype");
                        mobilepush.producttypecode = StringHelper.TryGetInt(GetRequest("producttypecode"));
                        int clearid = MobilePushBL.Bind(mobilepush);
                        if (clearid > 0) result = GetJson(1, "退出成功");
                        else result = GetJson(0, "退出失败");
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = GetJson(ex);
            }
            context.Response.Write(result);
        }

    }
}