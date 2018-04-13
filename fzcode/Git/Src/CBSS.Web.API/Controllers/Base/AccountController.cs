using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using CBSS.IBS.IBLL;
using CBSS.Tbx.IBLL;
using CourseActivate.Web.API.Model;
using CourseActivate.Web.API.SMSService;
using CBSS.Framework.Redis;
using CBSS.Framework.Contract;
using CBSS.Tbx.BLL;
using CBSS.Framework.Contract.Enums;
using CourseActivate.Web.API.Filter;

namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 账号
    /// </summary>
    public partial class BaseController
    {
        static string _appId = XMLHelper.GetAppSetting("AppID");
        static string _getOssFilesUrl = XMLHelper.GetAppSetting("getOssFiles");
        static string _getFilesUrl = XMLHelper.GetAppSetting("getFiles");

        //------------------以下登陆注册相关接口-------------------
        /// <summary>
        /// 1 获取验证码 √ 
        /// </summary>
        /// <returns></returns>
        public static APIResponse SendCodeMessage(string inputStr)
        {
            #region 必填参数验证
            Telephone input;
            var verifyResult = tbxService.VerifyParam<Telephone>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            #endregion

            return tbxService.SendTelephoneCode(input.telephone);
        }

        /// <summary>
        /// 1.1 验证验证码 √
        /// </summary>
        /// <returns></returns>
        public static APIResponse DecidePhoneCode(string inputStr)
        {
            #region 必填参数验证
            TelephoneAndCode input;
            var verifyResult = tbxService.VerifyParam<TelephoneAndCode>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            #endregion

            return tbxService.DecidePhoneCode(input.telephone, input.code);
        }

        /// <summary>
        /// 2 手机号登陆 √  
        /// </summary>
        /// <returns></returns>
        public static APIResponse LoginByPhone(string inputStr)
        {
            MobileLoginInfo input;
            var verifyResult = tbxService.VerifyParam<MobileLoginInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            //验证码验证
            var code = redis.Get<Tb_PhoneCode>("UserPhoneCode", input.telephone);
            if (code != null)
            {
                if (code.Code != input.code)
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.请输入有效的验证码);
                }
            }
            else
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.请输入有效的验证码);
            }
            redis.Remove("UserPhoneCode", input.telephone);

            TB_UserInfoExtend ue = new TB_UserInfoExtend();
            ue.CreateDate = DateTime.Now;
            ue.DeviceType = input.deviceType;
            ue.EquipmentID = input.equipmentID;
            ue.IPAddress = input.ipAddress;

            //手机号登陆
            APIResponse realationNum = ibsService.TBXLoginByPhone(_appId, input.telephone, 1, ue, input.appID, input.appChannelID, input.versionNumber);
            if (realationNum.Success)
            {
                string[] strs = realationNum.Data.ToString().Split('|');
                int userid = Convert.ToInt32(strs[0]);
                string userName = strs[1];
                string className = strs[2];

                //根据用户id，添加到Redis缓存中
                tbxService.SetValidUserRecord(string.IsNullOrEmpty(strs[0]) ? "0" : strs[0]);

                Rds_UserLoginRecord loginRecord = new Rds_UserLoginRecord();
                loginRecord.UserID = userid;
                loginRecord.Status = 0;
                loginRecord.AppChannelID = input.appChannelID;
                loginRecord.CreateData = DateTime.Now;
                loginRecord.AppID = input.appID;
                loginRecord.AppVersionNumber = input.versionNumber;

                tbxService.SetUserLoginRecord(loginRecord);

                var userInfo = ibsService.GetUserInfoByUserId(userid);

                userInfo.Token = tbxService.AddToken(strs[0], input.equipmentID);

                return APIResponse.GetResponse(new { userInfo = userInfo });
            }
            else
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.登录失败);
            }
        }

        /// <summary>
        /// 3 重置密码 √
        /// </summary>
        /// <returns></returns>
        public static APIResponse ModifyPassWord(string inputStr)
        {
            ModifyPassWordInfo input;
            var verifyResult = tbxService.VerifyParam<ModifyPassWordInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            //验证用户是否存在
            var userInfo = ibsService.GetUserInfoByUserOtherID(input.telephone, 1);
            if (userInfo == null)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.用户不存在);
            }

            var rinfo = ibsService.AppResetPassWord(_appId, userInfo.UserID.ToString(), input.password);
            if (rinfo.Success)
            {
                return APIResponse.GetResponse();
            }
            else
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.操作失败);
            }
        }

        /// <summary>
        /// 4 账号登陆 √ 
        /// </summary>
        /// <returns></returns>
        public static APIResponse Login(string inputStr)
        {
            LoginInfo input;
            var verifyResult = tbxService.VerifyParam<LoginInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            TB_UserInfoExtend ue = new TB_UserInfoExtend();
            ue.CreateDate = DateTime.Now;
            ue.DeviceType = input.deviceType;
            ue.EquipmentID = input.equipmentID;
            ue.IPAddress = input.ipAddress;

            var rinfo = ibsService.AppLogin(input.userName.Trim(), input.passWord, input.machineCode, _appId, input.machineModel, ue, input.appID);

            if (rinfo.Success)
            {
                string[] strs = rinfo.Data.ToString().Split('|');

                var userInfo = ibsService.GetUserInfoByUserId(Convert.ToInt32(strs[0]));
                tbxService.SetValidUserRecord(userInfo.UserID.ToString());
                tbxService.RemoveOnlineUser(userInfo.UserID.ToString());

                Rds_UserLoginRecord loginRecord = new Rds_UserLoginRecord();
                loginRecord.UserID = userInfo.UserID;
                loginRecord.Status = 0;
                loginRecord.AppChannelID = input.appChannelID;
                loginRecord.CreateData = DateTime.Now;
                loginRecord.AppID = input.appID;
                loginRecord.AppVersionNumber = input.versionNumber;
                tbxService.SetUserLoginRecord(loginRecord);

                userInfo.Token = tbxService.AddToken(strs[0], input.equipmentID);

                if (userInfo != null)
                {
                    return APIResponse.GetResponse(new { userInfo = userInfo });
                }
                return APIResponse.GetErrorResponse(ErrorCodeEnum.登录失败);
            }
            else
            {
                return APIResponse.GetErrorResponse(rinfo.ErrorMsg);
            }
        }



        /// <summary>
        /// 6 根据ID获取用户信息 √
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetUserInfoByUserID(string inputStr)
        {
            UserID input;
            var verifyResult = tbxService.VerifyParam<UserID>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var res = ibsService.GetUserAllInfoByUserId(Convert.ToInt32(input.userID));
            if (res != null)
            {
                res.iBS_UserInfo.UserImage = res.iBS_UserInfo.IsEnableOss != 0 ? _getOssFilesUrl + res.iBS_UserInfo.UserImage : res.iBS_UserInfo.UserImage;
            }
            else
            {
                return APIResponse.GetErrorResponse("账户不存在");
            }
            return APIResponse.GetResponse(res);
        }


        //------------------以下完善资料相关接口-------------------
        /// <summary>
        /// 7 保存老师资料 △
        /// </summary>
        /// <returns></returns>
        public static APIResponse AddTeacherInfo(string inputStr)
        {
            CourseActivate.Web.API.Model.TeacherInfo input;
            var verifyResult = tbxService.VerifyParam<CourseActivate.Web.API.Model.TeacherInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            TBX_UserInfo info = new TBX_UserInfo();
            info.ProvinceID = input.provinceID;
            info.Province = input.province;
            info.CityID = input.cityID;
            info.City = input.city;
            info.iBS_UserInfo.UserID = input.userID;
            info.iBS_UserInfo.UserName = input.userName;
            info.ClassSchDetailList.Add(new ClassSchDetail
            {
                AreaID = input.areaID,
                AreaName = input.area,
                SubjectID = input.subjectID,
                SubjectName = input.subject,
                SchID = input.schoolID,
                SchName = input.schoolName
            });

            return ibsService.AddTeacherInfo(info, _appId);
        }

        /// <summary>
        /// 8 更新用户头像昵称 √
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse UpdateUsers(string inputStr)
        {
            UserImage input;
            var verifyResult = tbxService.VerifyParam<UserImage>(inputStr, out input, new List<string> { "nickName" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            if (!string.IsNullOrEmpty(input.nickName))
            {
                //特殊字符验证
                if (tbxService.ContainsBadChar(input.nickName))
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误);
                }
            }

            TBX_UserInfo userinfo = new TBX_UserInfo();
            userinfo.iBS_UserInfo.UserID = long.Parse(input.userID);
            userinfo.iBS_UserInfo.TrueName = input.nickName;
            userinfo.iBS_UserInfo.UserImage = input.userImage;
            userinfo.iBS_UserInfo.IsEnableOss = input.isEnableOss;
            var result = ibsService.Update(userinfo);
            if (result)
            {
                return APIResponse.GetResponse();
            }
            else
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.操作失败);
            }
        }





        //------------------新增补充接口--------------------------
        /// <summary>
        /// 12 检查手机号是否已经注册 √
        /// </summary>
        [HttpPost]
        public static APIResponse CheckPhone(string inputStr)
        {
            Telephone input;
            var verifyResult = tbxService.VerifyParam<Telephone>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var userInfo = ibsService.GetUserInfoByUserOtherID(input.telephone, 1);

            if (userInfo == null)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.用户不存在);
            }
            else
            {
                return APIResponse.GetResponse();
            }
        }

        /// <summary>
        /// 13 用户注销登陆 △
        /// </summary>
        [HttpPost]
        public static APIResponse AppLoginOut(string inputStr)
        {
            UserNumAndUserID input;
            var verifyResult = tbxService.VerifyParam<UserNumAndUserID>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var res = ibsService.AppLoginOut(_appId, input.userNum, input.userID);

            if (res.Success)
            {
                return APIResponse.GetResponse();
            }
            else
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.操作失败);
            }
        }

        /// <summary>
        /// 13 用户注销登陆 △
        /// </summary>
        [HttpPost]
        public static APIResponse GetAreaInfo(string inputStr)
        {
            AreaInfo input;
            var verifyResult = tbxService.VerifyParam<AreaInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            var res = ibsService.GetAreaInfo(input.ID);

            if (res != null)
            {
                return APIResponse.GetResponse(res);
            }
            else
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.操作失败);
            }
        }


        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="context"></param>
        [HttpPost]
        public static APIResponse UpdateUserInfo(string inputStr)
        {
            var submitData = inputStr.ToObject<UserInfoRequest>();
            try
            {
                if (submitData.TrueName.filterSql())
                {
                    var obj = new { Success = false, Msg = "有SQL攻击嫌疑，请停止操作。" };
                    return APIResponse.GetErrorResponse(obj.ToJson());
                }
                else
                {
                    int[] subjectids = { 3 };
                    string[] subjectName = { "英语" };


                    var result = ibsService.UpdateUserInfo(submitData.UserID.ToString(), submitData.TrueName, submitData.SchoolID, submitData.SchoolName, subjectids, subjectName);

                    if (result.Success)
                    {
                        return APIResponse.GetResponse("");
                    }
                    else
                    {
                        return APIResponse.GetErrorResponse(result.ErrorMsg);
                    }
                }
            }
            catch (Exception)
            {
                return APIResponse.GetErrorResponse("");
            }
        }



        /// <summary>
        /// 通过老师ID
        /// </summary>
        /// <param name="context"></param>
        public static APIResponse QueryClassList(string inputStr)
        {
            var submitData = inputStr.ToObject<UserInfoRequest>();
            try
            {

                var user = ibsService.GetUserInfoByUserId(submitData.UserID);
                List<ClassInfo> returnList = new List<ClassInfo>();

                if (user != null)
                {
                    if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                    {
                        user.ClassSchList.ForEach(a =>
                        {
                            var classinfo = ibsService.GetClassUserRelationByClassId(a.ClassID);
                            if (classinfo != null)
                            {
                                ClassInfo cInfo = new ClassInfo
                                {
                                    StudentNum = classinfo.ClassStuList.Count,
                                    Id = classinfo.ClassID,
                                    ClassNum = classinfo.ClassNum.ToString(),
                                    ClassName = classinfo.ClassName,
                                    SchoolId = classinfo.SchID
                                };
                                returnList.Add(cInfo);
                            }
                        }
                        );
                        List<ClassInfo> list = new List<ClassInfo>();
                        if (returnList != null && returnList.Count > 0)
                        {
                            string[] gradeArr = { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" };

                            for (int i = 0, length = gradeArr.Length; i < length; i++)
                            {
                                list.AddRange(returnList.Where(t => t.ClassName.IndexOf(gradeArr[i], StringComparison.Ordinal) > -1));
                            }
                        }

                        returnList = list;
                        return APIResponse.GetResponse(returnList.ToJson());
                    }
                    else
                    {
                        return APIResponse.GetErrorResponse("");
                    }
                }
                else
                {
                    return APIResponse.GetErrorResponse("");
                }
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ex.Message);
            }
        }


        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="context"></param>
        public static APIResponse GetUserName(string inputStr)
        {
            var submitData = inputStr.ToObject<UserInfoRequest>();

            try
            {
                var user = ibsService.GetUserInfoByUserId(submitData.UserID);
                if (user != null)
                {
                    TB_UUMSUser userInfo = new TB_UUMSUser();

                    if (!string.IsNullOrEmpty(user.TrueName))
                    {
                        userInfo.TrueName = user.TrueName;
                    }
                    userInfo.AvatarUrl = user.IsEnableOss != 0 ? _getOssFilesUrl + user.UserImage : _getFilesUrl + "?FileID=" + user.UserImage;
                    userInfo.SchoolID = user.SchoolID;
                    userInfo.SchoolName = user.SchoolName;
                    App app = tbxService.GetApp(submitData.APPID.ToString());

                    string personId = string.IsNullOrEmpty(user.UserNum) ? "您尚未认证，请加添加微信号（tbx010）进行验证" : user.UserNum;
                    userInfo.PersonID = personId;

                    var obj = new { Success = true, UserInfo = userInfo, VersionName = app == null?"": app.AppName, IsEnableOss = user.IsEnableOss };
                    return APIResponse.GetResponse(obj.ToJson());
                }
                else
                {
                    return APIResponse.GetErrorResponse("");
                }
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ex.Message);
            }
        }


        /// <summary>
        /// 删除老师绑定班级
        /// </summary>
        /// <param name="context"></param>
        public static APIResponse DeleteClassList(string inputStr)
        {
            var submitData = inputStr.ToObject<UserInfoRequest>();
            bool flag = false;

            try
            {
                var user = ibsService.GetUserInfoByUserId(submitData.UserID);
                if (user != null)
                {
                    if (submitData.IDStr.IndexOf(',') > 0)
                    {
                        List<string> idArr = new List<string>(submitData.IDStr.Split(','));
                        idArr.ForEach(a =>
                        {
                            var ss = ibsService.GetClassUserRelationByClassId(a);
                            if (ss != null)
                            {
                                if (ss.ClassStuList.Count > 0)
                                {
                                    throw new Exception("不能删除有学生的班级");
                                }
                                UserClassData data = new UserClassData();
                                data.ClassID = a;
                                data.UserID = submitData.UserID;
                                data.SubjectID = submitData.SubjectID;
                                if (user.UserType == 12)
                                {

                                    data.UserType = UserTypeEnum.Teacher;
                                    var result = ibsService.UnBindClass(data);
                                    if (result == null || !result.Success)
                                    {
                                        throw new Exception(result == null ? "" : result.ErrorMsg);
                                    }

                                    ibsService.UnBindClassByClassId(data.ClassID);
                                    flag = true;
                                }
                                else if (user.UserType == 26)
                                {
                                    data.UserType = UserTypeEnum.Student;
                                    var result = ibsService.UnBindClass(data);
                                    if (result == null || !result.Success)
                                    {
                                        throw new Exception(result == null ? "" : result.ErrorMsg);
                                    }
                                    ibsService.UnBindClassByClassId(data.ClassID);
                                    flag = true;
                                }
                            }
                        });


                    }
                    else
                    {
                        var ss = ibsService.GetClassUserRelationByClassId(submitData.IDStr);
                        if (ss != null)
                        {
                            if (ss.ClassStuList.Count > 0)
                            {
                                return APIResponse.GetErrorResponse("不能删除有学生的班级");
                            }
                            UserClassData data = new UserClassData();
                            data.ClassID = submitData.IDStr;
                            data.UserID = Convert.ToInt32(submitData.UserID);
                            data.SubjectID = submitData.SubjectID;
                            if (user.UserType == 12)
                            {

                                data.UserType = UserTypeEnum.Teacher;
                                var result = ibsService.UnBindClass(data);
                                if (result == null || !result.Success)
                                {
                                    return APIResponse.GetErrorResponse(result == null ? "" : result.ErrorMsg);
                                }
                                ibsService.UnBindClassByClassId(data.ClassID);
                                flag = true;
                            }
                            else if (user.UserType == 26)
                            {
                                data.UserType = UserTypeEnum.Student;
                                var result = ibsService.UnBindClass(data);
                                if (result == null || !result.Success)
                                {
                                    return APIResponse.GetErrorResponse(result == null ? "" : result.ErrorMsg);
                                }
                                ibsService.UnBindClassByClassId(data.ClassID);
                                flag = true;
                            }
                        }
                        else
                        {
                            return APIResponse.GetErrorResponse("不存在此班级");
                        }

                    }
                    int count = 0;
                    user = ibsService.GetUserInfoByUserId(submitData.UserID);
                    if (user != null)
                    {
                        count = user.ClassSchList.Count;
                    }
                    var obj = new { Success = flag, Count = count };
                    return APIResponse.GetResponse(obj.ToJson());
                }
                else
                {
                    return APIResponse.GetErrorResponse("");
                }
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ex.Message);
            }
          


        }


        /// <summary>
        /// 通过手机号查询老师班级
        /// </summary>
        /// <param name="context"></param>
        private static APIResponse GetClassByTelePhone(string inputStr)
        {
            var submitData = inputStr.ToObject<ClassByTelephone>();
            if (string.IsNullOrEmpty(submitData.code))
            {
                return APIResponse.GetErrorResponse("验证码不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.telephone) || submitData.telephone == "undefined")
            {
                return APIResponse.GetErrorResponse("手机不能为空！");
            }

            if (tbxService.DecidePhoneCode(submitData.telephone, submitData.code).Success)
            {
                var serviceInfo = ibsService.GetUserInfoByUserOtherID(submitData.telephone, 1);
                if (serviceInfo != null)
                {
                    var obj = new { Success = true, UserID = serviceInfo.UserID, UserType = serviceInfo.UserType };
                    return APIResponse.GetResponse(obj.ToJson());
                }
                else
                {
                    var returnInfo = ibsService.TBXLoginByPhone("", submitData.telephone, submitData.Type);
                    string userId = returnInfo.Data.ToString().Split('|')[0];
                    string userName = returnInfo.Data.ToString().Split('|')[1];
                    var userInfo = ibsService.GetUserInfoByUserId(Convert.ToInt32(userId));
                    //PSO.UUMSService.User info = UUMSService.GetUserInfoByID(AppID, userID);
                    if (userInfo == null)
                    {
                        Tb_UserInfo user = new Tb_UserInfo
                        {
                            UserID = string.IsNullOrEmpty(userId.ToString()) ? 0 : Convert.ToInt32(userId.ToString()),
                            UserName = userName,
                            IsUser = 1,
                            IsEnableOss = 0,
                            isLogState = "0"
                        };
                        ibsService.Insert(user);
                    }

                   // SessionHelper.SetSession("phone", telephone);
                    if (returnInfo.Data != null)
                    {
                        var obj = new { Success = true, UserID = userId, UserType = userInfo.UserType };
                        return APIResponse.GetResponse(obj.ToJson());
                    }
                    else
                    {
                        var obj = new { Success = false, UserID = userId };
                        return APIResponse.GetErrorResponse(obj.ToJson());
                    }
                }
            }
            else
            {
                var obj = new { Success = false, Msg = "验证码错误！" };
                return APIResponse.GetErrorResponse(obj.ToJson());
            }
            
        }


        /// <summary>
        /// 学生绑定班级
        /// </summary>
        /// <param name="context"></param>
        public static APIResponse StudentBindClass(string inputStr)
        {
            var submitData = inputStr.ToObject<UserInfoRequest>();
            var userInfo = ibsService.GetUserInfoByUserId(Convert.ToInt32(submitData.StudentID));
            if (userInfo != null)
            {
                if (userInfo.ClassSchList.Count > 0)
                {
                    userInfo.ClassSchList.ForEach(a =>
                    {
                        UserClassData data = new UserClassData();
                        data.UserID = Convert.ToInt32(submitData.StudentID);
                        data.ClassID = submitData.ClassID;

                        data.UserType = UserTypeEnum.Student;
                        data.Type = ModRelationTypeEnum.StuClass;
                        data.message = "";
                        data.flag = BCPointEnum.Cpoint;
                        var flag = ibsService.UnBindClass(data);
                        if (flag != null)
                        {
                            if (flag.Success)
                            {
                                flag = ibsService.AddUserToClass(data);
                                if (flag == null || !flag.Success)
                                {
                                    throw new Exception(flag == null ? "绑定失败！" : flag.ErrorMsg);
                                }
                            }
                            else
                            {
                                throw new Exception(flag == null ? "绑定失败！" : flag.ErrorMsg);
                            }
                            
                        }
                        else
                        {
                            throw new Exception(flag == null ? "绑定失败！" : flag.ErrorMsg);
                        }
                    });
                    return APIResponse.GetResponse("");
                }
                else
                {
                    UserClassData data = new UserClassData();
                    data.UserID = Convert.ToInt32(submitData.StudentID);
                    data.ClassID = submitData.ClassID;

                    data.UserType = UserTypeEnum.Student;
                    data.Type = ModRelationTypeEnum.StuClass;
                    data.message = "";
                    data.flag = BCPointEnum.Cpoint;
                    var flag = ibsService.AddUserToClass(data);
                    if (flag == null || !flag.Success)
                    {
                        return APIResponse.GetErrorResponse(flag == null ? "绑定失败！" : flag.ErrorMsg);
                    }
                    return APIResponse.GetResponse("");
                }
            }
            else
            {
                var obj = new { Success = false, Msg = "" };
                return APIResponse.GetErrorResponse("您还不是本校学生！");
            }
        }

        /// <summary>
        /// 获取用户身份
        /// </summary>
        /// <param name="context"></param>
        private static APIResponse GetUserTypeById(string inputStr)
        {
            var submitData = inputStr.ToObject<UserInfoRequest>();

            var user = ibsService.GetUserInfoByUserId(submitData.UserID);

            if (user != null)
            {

                var obj = new { Success = true, UserType = user.UserType, ClassList = true };
                return APIResponse.GetResponse(obj.ToJson());
            }
            else
            {
                return APIResponse.GetErrorResponse("用户身份不存在！");
            }
        }

    }
}

