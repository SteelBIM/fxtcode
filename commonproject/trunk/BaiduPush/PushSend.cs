using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace BaiduPush
{
    /// <summary>
    /// Android IOS手机设备消息推送
    /// </summary>
    public class PushSend
    {
        private static string yck_sk = "4XU8LBnUERd0hGgzamnbEegWaNo7mfXB";
        private static string yck_ak = "A35NrU7lF5BA5KKGIGW8EbSG";
        private static string yfk_sk = "XQnl5CK9fTGk94zLttugl9i8nGi4ymiU";
        private static string yfk_ak = "jmfhQD6RLLi16DrZaE2wkluv";

        /// <summary>
        /// 消息推送
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="ChannelId">渠道Id</param>
        /// <param name="andphshuserid">推送用户</param>
        /// <param name="entrustid">自定义内容,为空或者json字符串</param>
        /// <param name="stype">区分云查勘，风控宝</param>
        /// <param name="issucess">是否推送成功</param>
        /// <param name="tag">组Id</param>
        /// <returns></returns>
        public static string Send(string title, string text
            , string ChannelId, string andphshuserid, string entrustid, string stype, ref int issucess, string tag = "")
        {
            BaiduPush Bpush = null; 
            String apiKey ="";
            if (stype == "yck")
            {
                Bpush=new BaiduPush("POST", yck_sk);
                apiKey = yck_ak;
            }
            else if (stype == "yfk")
            {
                Bpush = new BaiduPush("POST", yfk_sk);
                apiKey = yfk_ak;
            }
            String messages = "";
            String method = (tag == "") ? "push_msg" : "fetch_tag";
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            uint device_type = 3;
            uint unixTime = (uint)ts.TotalSeconds;
            uint message_type;
            string messageksy = "xxxxxx";
            //默认为通知notification 
            message_type = 1;
            //如果为透传消息passmsg
            if (!string.IsNullOrEmpty(entrustid))
            {
                try
                {
                    JObject cutomerobj = JObject.Parse(entrustid);
                    if (cutomerobj != null && cutomerobj["pushtype"] != null
                        && cutomerobj["pushtype"].ToString() == "passmsg")
                    {
                        message_type = 0;
                    }
                }
                catch (Exception ex)
                {
                    message_type = 1;
                }
            }
            BaiduPushNotification notification = new BaiduPushNotification();
            notification.title = title;
            notification.description = text;
            notification.user_confirm = 1;
            notification.open_type = 2;
            notification.notification_basic_style = 4;
            notification.notification_builder_id = 0;
            notification.pkg_content = "";
            notification.custom_content = entrustid;
            messages = notification.getJsonString();
            PushOptions pOpts;
            //如果是按组推送
            if (!string.IsNullOrEmpty(tag))
            {
                pOpts = new PushOptions(method, apiKey, tag, device_type, messages, messageksy, unixTime);
            }
            else
            {
                pOpts = new PushOptions(method, apiKey, andphshuserid, ChannelId, device_type, messages, messageksy,
unixTime);
            }
            pOpts.message_type = message_type;
            string pushresult = Bpush.PushMessage(pOpts),checkoushresult="";
            try
            {
                checkoushresult = pushresult;
                if (checkoushresult.IndexOf("Response:") != -1)
                {
                    checkoushresult = checkoushresult.Substring(checkoushresult.IndexOf("Response:") + "Response:".Length);
                }
                JObject pushobj = JObject.Parse(checkoushresult);
                string pusmes = pushobj["response_params"].ToString();
                pushobj = JObject.Parse(pusmes);
                if (!string.IsNullOrEmpty(pushobj["success_amount"].ToString()) &&
                     Int32.Parse(pushobj["success_amount"].ToString()) > 0)
                {
                    issucess = 1;
                }
            }
            catch (Exception ex)
            {
            }
            return pushresult;
        }




    }
}