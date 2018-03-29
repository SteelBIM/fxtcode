using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {

        //对外方法名：pfb_func_1 参数名：title,content
        public WCFJsonData ProductFeedBack(string sinfo, string info)
        {
            var func = JObject.Parse(info)["funinfo"];

            var title = func["title"].ToString();
            var content = func["content"].ToString();

            MailHelper.SendEmail(WebCommon.GetConfigSetting("recivemail")
                   , WebCommon.GetConfigSetting("sendmail")
                   , title
                   , content
                   , WebCommon.GetConfigSetting("mailaccount")
                   , WebCommon.GetConfigSetting("mailpassword")
                   , WebCommon.GetConfigSetting("smtpserver")
                   , StringHelper.TryGetInt(WebCommon.GetConfigSetting("smtpport")));
            return JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "成功");
        }
    }
}
