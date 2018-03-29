using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtUserCenterService.Entity;
using FxtUserCenterService.Logic;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// user_add 的摘要说明
    /// </summary>
    public class user_add : HttpHandlerBase
    {

        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "username", "companyid", "userpwd", "emailstr", "mobile", "wxopenid" })) return;
            string result = "";
            string username = GetRequest("username");
            int companyid = StringHelper.TryGetInt(GetRequest("companyid"));
            string userpwd = GetRequest("userpwd");
            string emailstr = GetRequest("emailstr");
            string mobile = GetRequest("mobile");
            string wxopenid = GetRequest("wxopenid");
            try
            {
                UserInfo user = new UserInfo();
                user.companyid = companyid;
                user.username = username;
                user.userpwd = userpwd;
                user.emailstr = emailstr;
                user.mobile = mobile;
                user.wxopenid = wxopenid;
                int add = UserBL.Add(user);
                if (add > 0) result = GetJson(1, "新增用户成功");
                else result = GetJson(1, "新增用户失败");
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