using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Logic;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// user_find 的摘要说明
    /// </summary>
    public class user_find : HttpHandlerBase
    {

        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            //sun 2013-12-27 查询用户
            if (!CheckMustRequest(new string[] { "username" })) return;
            string result = "";
            string username = GetRequest("username");
            string companyid = GetRequest("companyid");
            try
            {
                UserCheck chkuser = UserBL.GetFindUser(username);
                if (chkuser != null)
                {
                    if (chkuser.uservalid == 1)
                    {
                        if (chkuser.companyvalid == 1)
                        {
                            result = GetJson(chkuser);
                        }
                        else { result = GetJson(0, "机构无效"); }
                    }
                    else { result = GetJson(0, "用户名无效"); }
                }
                else { result = GetJson(0, "用户名不存在"); }
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