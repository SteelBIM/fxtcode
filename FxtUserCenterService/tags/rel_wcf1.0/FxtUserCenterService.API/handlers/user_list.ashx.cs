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
    /// user_list 的摘要说明
    /// </summary>
    public class user_list : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequestAfterLogin()) return;
            if (!CheckMustRequest(new string[] { "type" })) return;
            string result = "";
            string type = GetRequest("type");
            int companyid = StringHelper.TryGetInt(GetRequest("companyid"));
            string companycode = GetRequest("companycode");
            string[] userids = GetRequest("username").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            switch (type)
            {
                case "list"://获取用户列表 caoq 2014-03-04
                    List<UserCheck> list = UserBL.GetUserList(search, companyid, companycode, userids);
                    result = GetJson(list);
                    break;
            }
            context.Response.Write(result);
        }

    }
}