using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class RepairController : Controller
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        RedisHashOtherHelper redis=new RedisHashOtherHelper();
        //
        // GET: /Repair/
        public HttpResponseMessage RepairIBSClassUserInfo([FromBody] KingRequest request)
        {
            string classID = request.Data;
            if (classID.IsNullOrEmpty())
            {
                return ObjectToJson.GetErrorResult("参数不能为空！");
            }
            var classinfo=classBLL.BuildClassInfoByClassId(classID.ToUpper());
            if (classinfo != null)
            {
                redis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", classID.ToUpper(), classinfo);
            }
           return  ObjectToJson.GetResult("更新成功！");
        }


        public HttpResponseMessage RepairIBSUserInfo([FromBody] KingRequest request)
        {
            string UserID = request.Data;
            if (UserID.IsNullOrEmpty())
            {
                return ObjectToJson.GetErrorResult("参数不能为空！");
            }
            int userid = Convert.ToInt32(UserID);
            var userinfo = userBLL.BuildUserInfoByUserId(userid);
            if (userinfo != null)
            {
                redis.Set<IBS_UserInfo>("IBS_UserInfo", userid.ToString(), userinfo);
            }
            return ObjectToJson.GetResult("更新成功！");
        }
	}
}