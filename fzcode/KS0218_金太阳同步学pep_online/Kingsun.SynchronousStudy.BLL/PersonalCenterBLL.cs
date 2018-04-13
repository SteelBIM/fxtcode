using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.Model;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.BLL
{
    public class PersonalCenterBLL
    {
        PersonalCenterDAL pc = new PersonalCenterDAL();

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        /// <summary>
        /// 添加教师信息
        /// </summary>
        /// <param name="tInfo"></param>
        /// <returns></returns>
        public HttpResponseMessage AddTeacherInfo(TBX_UserInfo tInfo, string AppID)
        {
            tInfo.iBS_UserInfo.UserType = (int)UserTypeEnum.Teacher;
            var result = userBLL.UpdateUserAndOtherInfo(tInfo);
            bool res1 = false;
            if (!result.Success)
            {
                return JsonHelper.GetErrorResult(301, result.ErrorMsg);//"新增失败"
            }
            var otherInfo = tInfo.ClassSchDetailList.FirstOrDefault();
            if (otherInfo != null)
            {
                var re = userBLL.ModifyUserSchool(AppID, tInfo.iBS_UserInfo.UserID.ToString(), otherInfo.SchID, otherInfo.SchName, tInfo.iBS_UserInfo.UserName);
                if (!re.Success)
                {
                    return JsonHelper.GetErrorResult(301, re.ErrorMsg);//"新增失败"
                }
                res1 = userBLL.UpdateSubjectListByUserID(tInfo.iBS_UserInfo.UserID.ToString(), new[] { otherInfo.SubjectID }, new[] { otherInfo.SchName });
                if (!res1)
                {
                    return JsonHelper.GetErrorResult(301, "");//"新增失败"
                }
            }
            return JsonHelper.GetResult("新增成功");
            

        }

        /// <summary>
        /// 获取个人中心订单列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetOrderListByUserId(string UserId)
        {
            return pc.GetOrderListByUserId(UserId);
        }
    }
}
