using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// productfeedback 的摘要说明
    /// 产品反馈 kevin
    /// </summary>
    public class productfeedback : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "title", "content" })) return;
            string result = "";
            try {
                MailHelper.SendEmail(WebCommon.GetConfigSetting("recivemail")
                    , WebCommon.GetConfigSetting("sendmail")
                    , GetRequest("title")
                    , GetRequest("content")
                    , WebCommon.GetConfigSetting("mailaccount")
                    , WebCommon.GetConfigSetting("mailpassword")
                    , WebCommon.GetConfigSetting("smtpserver")
                    , StringHelper.TryGetInt(WebCommon.GetConfigSetting("smtpport")));
                result = GetJson(1, "发送成功");
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