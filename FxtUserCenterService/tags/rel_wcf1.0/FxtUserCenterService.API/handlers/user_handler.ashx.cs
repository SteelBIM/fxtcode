using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FxtUserCenterService.Logic;
using FxtUserCenterService.Entity;
using CAS.Common;
using CAS.Entity;

namespace FxtUserCenterService.API.handlers
{
    /// <summary>
    /// user_check 的摘要说明
    /// 检查登录用户有效 kevin 2013-4-2
    /// </summary>
    public class user_handler : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "username", "companyid", "type" })) return;
            string result = "";
            string username = GetRequest("username");
            int companyid = StringHelper.TryGetInt(GetRequest("companyid"));
            string userpwd = GetRequest("userpwd");
            string emailstr = GetRequest("emailstr");
            string mobile = GetRequest("mobile");
            string wxopenid = GetRequest("wxopenid");
            string truename = GetRequest("truename");
            string type = GetRequest("type");
            try
            {
                FxtUserCenterService.Entity.UserInfo user = new FxtUserCenterService.Entity.UserInfo();
                switch (type)
                {
                    case "add":
                        user.companyid = companyid;
                        user.username = username;
                        user.userpwd = userpwd;
                        user.emailstr = emailstr;
                        user.mobile = mobile;
                        user.wxopenid = wxopenid;
                        user.truename = truename;
                        int add = UserBL.Add(user);
                        if (add > 0) result = GetJson(1, "新增用户成功");
                        else result = GetJson(1, "新增用户失败");
                        break;
                    case "edit":
                        UserCheck chkuser = UserBL.GetFindUser(username);
                        if (chkuser == null) result = GetJson(0, "用户名不存在");
                        else
                            user.companyid = companyid;
                        user.username = username;
                        //user.userpwd = userpwd;
                        user.emailstr = emailstr;
                        user.mobile = mobile;
                        user.wxopenid = wxopenid;
                        user.truename = truename;
                        int edit = UserBL.EditUser(user);
                        if (edit > 0) result = GetJson(1, "修改用户信息成功");
                        else result = GetJson(1, "修改用户信息失败");
                        break;
                    case "delete":
                        int status = UserBL.Delete(username, companyid);
                        if (status > 0) result = GetJson(1, "删除用户成功");
                        else result = GetJson(1, "删除用户失败");
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