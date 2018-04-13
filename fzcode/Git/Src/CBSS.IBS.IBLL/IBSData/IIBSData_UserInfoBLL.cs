
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.API;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.IBLL
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IIBSData_UserInfoBLL
    {
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        APIResponse AppResetPassWord(string AppId, string UserId, string Password);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        IBS_UserInfo GetUserInfoByUserId(int UserId);
        /// <summary>
        /// 获取用户全部信息（针对TBX）
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        TBX_UserInfo GetUserAllInfoByUserId(int UserId);

        /// <summary>
        /// 通过手机号/登陆账号/教师邀请码获取用户信息
        /// </summary>
        /// <param name="UserOtherID">手机号/登陆账号/教师邀请码</param>
        /// <param name="Type">类型（UserID=0,TelePhone=1,UserName=2,TchInvNum=3）</param>
        /// <returns></returns>
        IBS_UserInfo GetUserInfoByUserOtherID(string UserOtherID, int Type);
        /// <summary>
        /// 通过手机号/登陆账号/教师邀请码获取用户全部信息
        /// </summary>
        /// <param name="UserOtherID">手机号/登陆账号/教师邀请码</param>
        /// <param name="Type">类型（UserID=0,TelePhone=1,UserName=2,TchInvNum=3）</param>
        /// <returns></returns>
        TBX_UserInfo GetUserALLInfoByUserOtherID(string UserOtherID, int Type);
        /// <summary>
        /// 查询所有班级学生
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        string GetAllClassStudentSum(string[] arr);
        /// <summary>
        /// 获取书本信息
        /// </summary>
        /// <param name="Stage"></param>
        /// <param name="Grade"></param>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        /// <param name="Booklet"></param>
        /// <returns></returns>
        string GetBookData(string Stage, string Grade, string Subject, string Edition, string Booklet);
        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Telephone"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        APIResponse TBXLoginByPhone(string AppId, string Telephone, int UserType);

        APIResponse TBXLoginByPhone(string AppId, string Telephone, int UserType, TB_UserInfoExtend extend, string appNum);
        APIResponse TBXLoginByPhone(string AppId, string Telephone, int UserType, TB_UserInfoExtend extend, string appNum,string AppChannelID, string AppVersionNumber);
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        List<PowerList>  GetUserPowerList(string UserId,string appId);

        string GetUserPowerID(string UserId, string AppId);
        /// <summary>
        /// 检查登陆状态
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserNum"></param>
        /// <param name="AppId"></param>
        /// <param name="ClientIP"></param>
        UserStateEnum CheckLoginUserState(string UserId,string UserNum,string AppId,string ClientIP);

        APIResponse AppCheckUserState(string AppId, string UserId);
        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Model"></param>
        /// <param name="AppId"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        APIResponse AppLogin(string UserName, string Password, string Model, string AppId, string Mode);
        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Model"></param>
        /// <param name="AppId">设备APPID（调MOD接口）</param>
        /// <param name="Mode"></param>
        /// <param name="extend"></param>
        /// <param name="AppNum">同步学APPID（存入数据库）</param>
        APIResponse AppLogin(string UserName, string Password, string Model, string AppId, string Mode, TB_UserInfoExtend extend, string AppNum);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        APIResponse AppRegister2(string AppId, string UserName, string Password, int Type);

        APIResponse AppRegister2(string AppId, string UserName, string Password, int Type, string AppChannelID,string AppVersionNumber);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        APIResponse AppModifyPassWord(string AppId, string UserId, string Password, string OldPassword);
        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserNum"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        APIResponse AppLoginOut(string AppId, string UserNum, string UserId);
        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        string LoginOut(string AppId, string UserId, out string Message);
        /// <summary>
        /// 使用手机号成为某一用户唯一的,使用接口前业务系统需要验证用户手机号 
        /// </summary>
        /// <param name="Telephone"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        APIResponse VerifyUserPhone(string Telephone, string UserId);
        /// <summary>
        /// App修改用户信息真实姓名，电话，密码，角色，邮箱
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <param name="TrueName"></param>
        /// <param name="Userrole"></param>
        /// <param name="PassWord"></param>
        /// <param name="Phone"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        APIResponse AppUpdateUserInfo(string AppId, int UserId, string TrueName);


        APIResponse ModifyUserSchool(string AppID, string UserID, int SchID, string SchName, string UserName);

        bool UpdateSubjectListByUserID(string UserID, int[] str, string[] str1);
        APIResponse UpdateUserInfoNoOnly(string AppId, IBS_UserInfo user);

        APIResponse UpdateUserInfo(string userId, string trueName, string SchID, string SchName, int[] subjectid, string[] subjectName);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>                                          
        bool Add(TBX_UserInfo userInfo);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        //APIResponse Update(TBX_UserInfo userInfo);

        bool Update(TBX_UserInfo userInfo);
        /// <summary>
        /// 调用MOD同时修改关联关系
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool UpdateUserAndOtherInfo(TBX_UserInfo userInfo);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userId">AreaID</param>
        /// <returns></returns>
        bool Delete(int userId);
        /// <summary>
        /// 从MOD和TBXDB中组装用户数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IBS_UserInfo BuildUserInfoByUserId(int userId);
        /// <summary>
        /// TBXDB查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Tb_UserInfo Search(Expression<Func<Tb_UserInfo, bool>> where);
        /// <summary>
        /// TBXDB插入
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool Insert(Tb_UserInfo info);

        /// <summary>
        /// 根据条件查询用户列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Tb_UserInfo> GetUserList(Expression<Func<Tb_UserInfo, bool>> where, string orderby = "");

        APIResponse AddTeacherInfo(TBX_UserInfo tInfo, string AppID);
    }
}
