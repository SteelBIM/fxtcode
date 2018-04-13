using CBSS.Framework.Redis;
using CBSS.Core.Utility;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CBSS.Framework.Contract;
using CBSS.IBS.BLL.RelationService;
using CBSS.Framework.Contract.API;
using CBSS.Framework.DAL;
using CBSS.Core.Log;
using CBSS.IBS.IBLL;

namespace CBSS.IBS.BLL
{
    public partial class IBSService : CommonBLL, IIBSService
    {

        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper("IBS");
        static RedisHashHelper hashRedis = new RedisHashHelper("IBS");

        RelationService.RelationService relationservice = new RelationService.RelationService();
        FZUUMS_Relation2.FZUUMS_Relation2SoapClient relation2Client = new FZUUMS_Relation2.FZUUMS_Relation2SoapClient();
        FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();

        

        public IBS_UserInfo GetUserInfoByUserId(int UserId)
        {
            IBS_UserInfo user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserId.ToString());//用户信息
            //如果没有
            if (user == null)
            {
                //从MOD+DB取数据
                user = BuildUserInfoByUserId(UserId);
                if (user != null)
                {
                    //新增手机号映射
                    CreateRdsTelephone(user);
                    //新增用户名映射
                    CreateRdsUserName(user);
                    //新增教师邀请码映射
                    CreateRdsInvm(user);
                    //存入redis
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserId.ToString(), user);
                }

            }
            return user;
        }

        //新增教师邀请码映射
        private void CreateRdsInvm(IBS_UserInfo user)
        {
            if (user.UserType == (int)UserTypeEnum.Teacher)
            {
                if (!string.IsNullOrEmpty(user.UserNum))
                {
                    //账号
                    IBS_UserOtherID teachInvm = new IBS_UserOtherID
                    {
                        UserIDOther = user.UserNum.ToString(),
                        Type = 3,
                        UserID = user.UserID
                    };
                    hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserNum + "_" + 3, teachInvm);
                }
                else
                {
                    InvokeMODGetInvNum(user);
                }
            }
        }

        /// <summary>
        /// 调用MOD获取教师邀请码
        /// </summary>
        /// <param name="user"></param>
        private void InvokeMODGetInvNum(IBS_UserInfo user)
        {
            var invnum = relationservice.SelectOrAddUserInvNumByUserId(user.UserID.ToString());
            if (!string.IsNullOrEmpty(invnum))
            {
                IBS_UserOtherID teachInvm = new IBS_UserOtherID
                {
                    UserIDOther = invnum,
                    Type = 3,
                    UserID = user.UserID
                };
                user.UserNum = invnum;
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserNum + "_" + 3, teachInvm);

            }
        }

        //新增用户名映射
        private void CreateRdsUserName(IBS_UserInfo user)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                //账号
                IBS_UserOtherID userName = new IBS_UserOtherID
                {
                    UserIDOther = user.UserName,
                    Type = 2,
                    UserID = user.UserID
                };
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserName + "_" + 2, userName);

            }
        }
        //新增手机号映射
        private void CreateRdsTelephone(IBS_UserInfo user)
        {
            //新增IBS_UserOtherID
            if (!string.IsNullOrEmpty(user.TelePhone))
            {
                //手机号
                IBS_UserOtherID telephone = new IBS_UserOtherID
                {
                    UserIDOther = user.TelePhone,
                    Type = 1,
                    UserID = user.UserID
                };
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.TelePhone + "_" + 1, telephone);
            }
        }

        /// <summary>
        /// 返回用户所有信息（针对TBX）
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public TBX_UserInfo GetUserAllInfoByUserId(int UserId)
        {
            TBX_UserInfo tbxUserInfo = null;
            var user = GetUserInfoByUserId(UserId);
            if (user != null)
            {
                tbxUserInfo = new TBX_UserInfo();
                tbxUserInfo.iBS_UserInfo = user;

                user.ClassSchList.ForEach(a =>
                {
                    BuildClassInfoByMod(a, tbxUserInfo, user);

                });
            }
            return tbxUserInfo;

        }

        private void BuildClassInfoByMod(ClassSch a, TBX_UserInfo tbxUserInfo, IBS_UserInfo user)
        {
            ClassSchDetail csDetail = new ClassSchDetail();
            //班级信息
            var classInfo = relationservice.GetClassInfoByID(a.ClassID.ToUpper());
            if (classInfo != null)
            {
                csDetail.ClassID = a.ClassID.ToUpper();
                csDetail.ClassName = classInfo.ClassName;
                if (user.UserType == (int)UserTypeEnum.Teacher)
                {
                    //学科
                    csDetail.SubjectID = a.SubjectID;
                    if (csDetail.SubjectID > 0)
                    {

                        csDetail.SubjectName = EnumHelper.GetEnumDesc<OldSubjectTypeEnum>(Convert.ToInt32(csDetail.SubjectID));
                    }
                    else
                    {
                        csDetail.SubjectName = "";
                    }

                }
                //学校信息
                csDetail.SchID = a.SchID;
                //通过Mod学校ID获取学校名称
                InvokeModGetSchInfo(a.SchID, csDetail);
                //年级
                csDetail.GradeID = a.GradeID;
                csDetail.GradeName = a.GradeID.GetGradeName();

                //区域信息
                csDetail.AreaID = a.AreaID;
                InvokeModGetAreaInfo(a.AreaID, csDetail);
                tbxUserInfo.ClassSchDetailList.Add(csDetail);
            }
        }

        private void InvokeModGetAreaInfo(int AreaId, ClassSchDetail csDetail)
        {
            var areaInfo = metadataService.GetAreaInfo(AreaId);
            if (!string.IsNullOrEmpty(areaInfo) && areaInfo.Split('|')[0] != "错误")
            {
                MOD_AreaInfoModel arm = areaInfo.FromJson<MOD_AreaInfoModel>();
                csDetail.AreaName = arm.CodeName;
            }
        }

        private void InvokeModGetSchInfo(int SchId, ClassSchDetail csDetail)
        {
            var schInfo = metadataService.GetSchoolInfo(SchId);
            if (!string.IsNullOrEmpty(schInfo) && schInfo.Split('|')[0] != "错误")
            {
                MOD_SchoolInfoModel sm = schInfo.FromJson<MOD_SchoolInfoModel>();
                csDetail.SchName = sm.SchoolName;
            }
        }


        /// <summary>
        /// 通过手机号确定一个用户 并修改名称和改为教师身份
        /// </summary>
        /// <param name="MobilePhone"></param>
        /// <param name="TrueName"></param>
        /// <returns></returns>
        public IBS_UserInfo GetUserInfoByPhone(string MobilePhone, string TrueName, string AppID = "KS021801")
        {
            var user = GetUserInfoByUserOtherID(MobilePhone, 1);
            if (user == null)
            {
                var reinfo = TBXLoginByPhone(AppID, MobilePhone, (int)UserTypeEnum.Teacher);
                if (reinfo.Success)
                {
                    string[] strs = reinfo.Data.ToString().Split('|');
                    string userid = strs[0];
                    user = GetUserInfoByUserId(userid.ToInt());
                    var res = userService.AppUpdateUserInfo(AppID, userid, TrueName, 0, "", "", "");
                    if (res)
                    {
                        user.TrueName = TrueName;
                        var dbUser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == user.UserID).FirstOrDefault();
                        if (dbUser != null)
                        {
                            dbUser.TrueName = TrueName;
                            repository2.Update(dbUser);
                        }
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), user);

                    }

                }
            }
            else
            {
                user.UserType = (int)UserTypeEnum.Teacher;
                FZUUMS_UserService.User moduser = new FZUUMS_UserService.User();
                moduser.UserID = user.UserID.ToString();
                moduser.UserType = user.UserType;
                moduser.TrueName = TrueName;
                var res = userService.UpdateUserInfo2(AppID, moduser);

                InvokeMODGetInvNum(user);
                user.TrueName = TrueName;
                var dbUser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == user.UserID).FirstOrDefault();
                if (dbUser != null)
                {
                    dbUser.TrueName = TrueName;
                    repository2.Update(dbUser);
                }
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), user);
            }
            return user;
        }


        public ClassInfoByUserID GetClassInfoByUserID(int UserId)
        {
            var user = GetUserInfoByUserId(UserId);
            if (user == null||user.ClassSchList.Count==0)
            {
                return null;
            }
            var userClass = user.ClassSchList.FirstOrDefault();
            if (userClass == null) return null;
            var classinfo = GetClassUserRelationByClassId(userClass.ClassID);
            if (classinfo != null)
            {
                ClassInfoByUserID clas = new ClassInfoByUserID();
                clas.UserID = UserId;
                clas.ClassID = classinfo.ClassID;
                clas.ClassNum = classinfo.ClassNum;
                return clas;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过其他内容获取用户信息
        /// </summary>
        /// <param name="UserOtherID"></param>
        /// <param name="Type">TelePhone=1,UserName=2,TchInvNum=3</param>
        /// <returns></returns>
        public IBS_UserInfo GetUserInfoByUserOtherID(string UserOtherID, int Type)
        {
            IBS_UserInfo user = null;
            var userOther = hashRedis.Get<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type);
            if (userOther != null)
            {
                user = GetUserInfoByUserId(Convert.ToInt32(userOther.UserID));
            }
            else
            {
                userOther = new IBS_UserOtherID();
                switch (Type)
                {
                    case 1://TelePhone
                        user = InvokeModGetUserInfoByTelephone(UserOtherID, Type, user, userOther);
                        break;
                    case 2://UserName
                        user = InvokeModGetUserInfoByUserName(UserOtherID, Type, user, userOther);
                        break;
                    case 3://TchInvNum
                        user = InvokeModGetUserInfoByInvNum(UserOtherID, Type, user, userOther);
                        break;
                }

            }
            return user;
        }

        /// <summary>
        /// 通过邀请码调用MOD获取用户信息
        /// </summary>
        /// <param name="UserOtherID"></param>
        /// <param name="Type"></param>
        /// <param name="user"></param>
        /// <param name="userOther"></param>
        /// <returns></returns>
        private IBS_UserInfo InvokeModGetUserInfoByInvNum(string UserOtherID, int Type, IBS_UserInfo user, IBS_UserOtherID userOther)
        {
            var userByTchInvNum = relationservice.GetUserTelInviTationByUserIdOrInvNum(UserOtherID, "");
            if (userByTchInvNum != null)
            {
                var Inv = userByTchInvNum.FirstOrDefault();
                userOther.UserID = Convert.ToInt32(Inv.UserID);
                userOther.UserIDOther = UserOtherID;
                userOther.Type = 2;
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                user = GetUserInfoByUserId(Convert.ToInt32(userOther.UserID));
            }

            return user;
        }
        /// <summary>
        /// 通过用户名调用MOD获取用户信息
        /// </summary>
        /// <param name="UserOtherID"></param>
        /// <param name="Type"></param>
        /// <param name="user"></param>
        /// <param name="userOther"></param>
        /// <returns></returns>
        private IBS_UserInfo InvokeModGetUserInfoByUserName(string UserOtherID, int Type, IBS_UserInfo user, IBS_UserOtherID userOther)
        {
            var userByName = userService.GetUserInfoByName("", UserOtherID);
            if (userByName != null)
            {
                userOther.UserID = Convert.ToInt32(userByName.UserID);
                userOther.UserIDOther = UserOtherID;
                userOther.Type = 2;
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                user = GetUserInfoByUserId(Convert.ToInt32(userOther.UserID));
            }

            return user;
        }
        /// <summary>
        /// 通过手机号调用MOD获取用户信息
        /// </summary>
        /// <param name="UserOtherID"></param>
        /// <param name="Type"></param>
        /// <param name="user"></param>
        /// <param name="userOther"></param>
        /// <returns></returns>
        private IBS_UserInfo InvokeModGetUserInfoByTelephone(string UserOtherID, int Type, IBS_UserInfo user, IBS_UserOtherID userOther)
        {
            var userByTelephone = userService.GetUserInfoByTelephone("", UserOtherID);
            if (userByTelephone != null)
            {
                userOther.UserID = Convert.ToInt32(userByTelephone.UserID);
                userOther.UserIDOther = UserOtherID;
                userOther.Type = 1;
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                user = GetUserInfoByUserId(Convert.ToInt32(userOther.UserID));
            }

            return user;
        }

        /// <summary>
        /// 通过其他内容获取用户信息
        /// </summary>
        /// <param name="UserOtherID"></param>
        /// <param name="Type">TelePhone=1,UserName=2,TchInvNum=3</param>
        /// <returns></returns>
        public TBX_UserInfo GetUserALLInfoByUserOtherID(string UserOtherID, int Type)
        {
            TBX_UserInfo user = null;
            var userOther = hashRedis.Get<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type);
            if (userOther != null)
            {
                user = GetUserAllInfoByUserId(Convert.ToInt32(userOther.UserID));
            }
            else
            {
                userOther = new IBS_UserOtherID();
                switch (Type)
                {
                    case 1://TelePhone
                        FZUUMS_UserService.User userByTelephone = userService.GetUserInfoByTelephone("", UserOtherID);
                        if (userByTelephone != null)
                        {
                            userOther.UserID = Convert.ToInt32(userByTelephone.UserID);
                            userOther.UserIDOther = UserOtherID;
                            userOther.Type = 1;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                            user = GetUserAllInfoByUserId(Convert.ToInt32(userOther.UserID));
                        }
                        break;
                    case 2://UserName
                        FZUUMS_UserService.User userByName = userService.GetUserInfoByName("", UserOtherID);
                        if (userByName != null)
                        {
                            userOther.UserID = Convert.ToInt32(userByName.UserID);
                            userOther.UserIDOther = UserOtherID;
                            userOther.Type = 2;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                            user = GetUserAllInfoByUserId(Convert.ToInt32(userOther.UserID));
                        }
                        break;
                    case 3://TchInvNum
                        Tb_UserTelInviTation[] userByTchInvNum = relationservice.GetUserTelInviTationByUserIdOrInvNum(UserOtherID, "");
                        if (userByTchInvNum != null)
                        {
                            var Inv = userByTchInvNum.FirstOrDefault();
                            userOther.UserID = Convert.ToInt32(Inv.UserID);
                            userOther.UserIDOther = UserOtherID;
                            userOther.Type = 2;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                            user = GetUserAllInfoByUserId(Convert.ToInt32(userOther.UserID));
                        }
                        break;
                }

            }
            return user;
        }

        /// <summary>
        /// 书本信息
        /// </summary>
        /// <param name="Stage"></param>
        /// <param name="Grade"></param>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        /// <param name="Booklet"></param>
        /// <returns></returns>
        public string GetBookData(string Stage,string Grade,string Subject,string Edition,string Booklet)
        {
            var result=metadataService.GetBookData(Stage, Grade, Subject, Edition, Booklet);
            return result;
        }

        /// <summary>
        /// 查询所有班级学生
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public string GetAllClassStudentSum(string[] arr) 
        {
            var result = relationservice.GetAllClassStudentSum(arr);
            return result;
        }
        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Telephone"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public APIResponse TBXLoginByPhone(string AppId, string Telephone, int UserType)
        {
            var returninfo = userService.TBXLoginByPhone(AppId, Telephone, UserType);
            APIResponse result = new APIResponse();
            if (returninfo.Success)
            {
                
                string[] strs = returninfo.Data.ToString().Split('|');
                string userid = strs[0];
                var ibsuser = GetUserInfoByUserId(userid.ToInt());
            }
            result.Success = returninfo.Success;
            result.ErrorMsg = returninfo.ErrorMsg;
            result.Data = returninfo.Data;
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppId">同步学APP</param>
        /// <param name="Telephone"></param>
        /// <param name="UserType"></param>
        /// <param name="extend"></param>
        /// <param name="appNum">设备APP</param>
        /// <returns></returns>
        public APIResponse TBXLoginByPhone(string AppId, string Telephone, int UserType,TB_UserInfoExtend extend,string appNum)
        {
            var returninfo = userService.TBXLoginByPhone(AppId, Telephone, UserType);
            APIResponse result = new APIResponse();
            if (returninfo.Success)
            {
                string[] strs = returninfo.Data.ToString().Split('|');
                string userid = strs[0];
                var user = GetUserInfoByUserId(userid.ToInt());
                user.AppID = appNum;
                var dbUser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == user.UserID).FirstOrDefault();
                if (dbUser != null)
                {
                    dbUser.AppId = appNum;
                    repository2.Update(dbUser);
                }
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userid, user);
                //新增设备记录
                CreateUserInfoExtend(extend, userid);
            }
            result.Success = returninfo.Success;
            result.ErrorMsg = returninfo.ErrorMsg;
            result.Data = returninfo.Data;
            return result;
        }

        private void CreateUserInfoExtend(TB_UserInfoExtend extend,string userid)
        {
            if (!string.IsNullOrEmpty(extend.EquipmentID))
            {
                var ext = repository2.SelectSearch<TB_UserInfoExtend>(a => a.UserID == userid).FirstOrDefault();
                if (ext == null)
                {
                    ext = new TB_UserInfoExtend();
                    ext.UserID = userid;
                    ext.EquipmentID = extend.EquipmentID;
                    ext.DeviceType = extend.DeviceType;
                    ext.IPAddress = extend.IPAddress;
                    ext.CreateDate = DateTime.Now;
                    repository2.Insert<TB_UserInfoExtend>(ext);
                }
                else
                {
                    ext.EquipmentID = extend.EquipmentID;
                    ext.DeviceType = extend.DeviceType;
                    ext.IPAddress = extend.IPAddress;
                    ext.CreateDate = DateTime.Now;
                    repository2.CustomIgnoreUpdate<TB_UserInfoExtend>(o => o.UserID, ext);
                }
            }
        }

        /// <summary>
        /// 手机登陆同时注册  若注册新增注册记录
        /// </summary>
        /// <param name="AppId">同步学APP</param>
        /// <param name="Telephone"></param>
        /// <param name="UserType"></param>
        /// <param name="extend"></param>
        /// <param name="appNum">设备APP</param>
        /// <returns></returns>
        public APIResponse TBXLoginByPhone(string AppId, string Telephone, int UserType, TB_UserInfoExtend extend, string appNum,string AppChannelID, string AppVersionNumber)
        {
            var user = GetUserALLInfoByUserOtherID(Telephone, 1);
            var returninfo = userService.TBXLoginByPhone(AppId, Telephone, UserType);
            APIResponse result = new APIResponse();
            if (returninfo.Success)
            {
                string[] strs = returninfo.Data.ToString().Split('|');
                string userid = strs[0];
                if (user == null)
                {
                    //若查询用户数据为空则为注册
                    var record = hashRedis.Get<Rds_UserLoginRecords>("Rds_UserLoginRecords", userid);
                    if (record != null)
                    {
                        var userrecord = record.records.FirstOrDefault(a => a.Status == 1);
                        if (userrecord == null)
                        {
                            record.records.Add(new Rds_UserLoginRecord()
                            {
                                AppChannelID = AppChannelID,
                                CreateData = DateTime.Now,
                                AppVersionNumber= AppVersionNumber,
                                UserID = long.Parse(userid),
                                Status = 1
                            });
                        }
                    }
                    else
                    {
                        record = new Rds_UserLoginRecords();
                        record.records.Add(new Rds_UserLoginRecord()
                        {
                            AppChannelID = AppChannelID,
                            CreateData = DateTime.Now,
                            AppVersionNumber = AppVersionNumber,
                            UserID = long.Parse(userid),
                            Status = 1
                        });
                    }
                    hashRedis.Set<Rds_UserLoginRecords>("Rds_UserLoginRecords", userid, record);
                }
                var ibsuser = GetUserInfoByUserId(userid.ToInt());
                ibsuser.AppID = appNum;
                var dbUser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == ibsuser.UserID).FirstOrDefault();
                if (dbUser != null)
                {
                    dbUser.AppId = appNum;
                    repository2.Update(dbUser);
                }
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userid, ibsuser);
                CreateUserInfoExtend(extend,userid);
            }
            result.Success = returninfo.Success;
            result.ErrorMsg = returninfo.ErrorMsg;
            result.Data = returninfo.Data;
            return result;
        }

        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Model"></param>
        /// <param name="AppId"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public APIResponse AppLogin(string UserName, string Password, string Model, string AppId, string Mode)
        {
            APIResponse result = new APIResponse();
            var res = userService.AppLogin(UserName, Password, Model, AppId, Mode);
            if (res.Success)
            {
                string[] strs = res.Data.ToString().Split('|');
                var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", strs[0]);
                if (user == null)
                {
                    user = BuildUserInfoByUserId(strs[0].ToInt());
                    //新增手机号映射
                    CreateRdsTelephone(user);
                    //新增用户名映射
                    CreateRdsUserName(user);
                    //新增邀请码映射
                    CreateRdsInvm(user);

                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", strs[0], user);

                    var dbUser = repository2.SelectSearch<Tb_UserInfo>(a=>a.UserID== user.UserID).FirstOrDefault();
                    if (dbUser == null)
                    {
                        dbUser = new Tb_UserInfo();
                        dbUser.UserID = Convert.ToInt32(user.UserID);
                        dbUser.TrueName = user.TrueName;
                        dbUser.UserImage = "00000000-0000-0000-0000-000000000000";
                        dbUser.UserName = user.UserName;
                        dbUser.UserRoles = 0;
                        dbUser.NickName = user.TrueName;
                        dbUser.IsUser = 1;
                        dbUser.isLogState = "0";
                        dbUser.IsEnableOss = 0;
                        dbUser.CreateTime = user.Regdate;
                        dbUser.AppId = AppId;
                        repository2.Insert(dbUser);
                    }
                    else
                    {
                        dbUser.UserID = Convert.ToInt32(user.UserID);
                        dbUser.TrueName = user.TrueName;
                        dbUser.UserName = user.UserName;
                        dbUser.NickName = user.TrueName;
                        dbUser.CreateTime = user.Regdate;
                        dbUser.AppId = AppId;
                        repository2.Update(dbUser);
                    }
                }
            }
            result.Success = res.Success;
            result.ErrorMsg = res.ErrorMsg;
            result.Data = res.Data;
            return result;
        }
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
        /// <returns></returns>
        public APIResponse AppLogin(string UserName, string Password, string Model, string AppId, string Mode,TB_UserInfoExtend extend,string AppNum)
        {
            APIResponse result = new APIResponse();
            var res = userService.AppLogin(UserName, Password, Model, AppId, Mode);
            if (res.Success)
            {
                string[] strs = res.Data.ToString().Split('|');
                var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", strs[0]);
                if (user == null)
                {
                    user = BuildUserInfoByUserId(strs[0].ToInt());
                    user.AppID = AppNum;
                    //新增手机号映射
                    CreateRdsTelephone(user);
                    //新增用户名映射
                    CreateRdsUserName(user);
                    //新增邀请码映射
                    CreateRdsInvm(user);

                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", strs[0], user);

                    var dbUser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == user.UserID).FirstOrDefault();
                    if (dbUser == null)
                    {
                        dbUser = new Tb_UserInfo();
                        dbUser.UserID = Convert.ToInt32(user.UserID);
                        dbUser.TrueName = user.TrueName;
                        dbUser.UserImage = "00000000-0000-0000-0000-000000000000";
                        dbUser.UserName = user.UserName;
                        dbUser.UserRoles = 0;
                        dbUser.NickName = user.TrueName;
                        dbUser.IsUser = 1;
                        dbUser.isLogState = "0";
                        dbUser.IsEnableOss = 0;
                        dbUser.CreateTime = user.Regdate;
                        dbUser.AppId = AppNum;
                        repository2.Insert(dbUser);
                    }
                    else
                    {
                        dbUser.UserID = Convert.ToInt32(user.UserID);
                        dbUser.TrueName = user.TrueName;
                        dbUser.UserName = user.UserName;
                        dbUser.NickName = user.TrueName;
                        dbUser.CreateTime = user.Regdate;
                        dbUser.AppId = AppNum;
                        repository2.Update(dbUser);
                    }

                    CreateUserInfoExtend(extend,strs[0]);


                }
            }
            result.Success = res.Success;
            result.ErrorMsg = res.ErrorMsg;
            result.Data = res.Data;
            return result;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public APIResponse AppRegister2(string AppId, string UserName, string Password, int Type)
        {
            var rinfo = userService.AppRegister2(AppId, UserName, Password, Type);
            if (rinfo.Success)
            {
                string[] strs = rinfo.Data.ToString().Split('|');
                string userid = strs[0];
                var user = GetUserInfoByUserId(userid.ToInt());
            }
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        } 
        /// <summary>
          /// 注册
          /// </summary>
          /// <param name="AppId"></param>
          /// <param name="UserName"></param>
          /// <param name="Password"></param>
          /// <param name="Type"></param>
          /// <returns></returns>
        public APIResponse AppRegister2(string AppId, string UserName, string Password, int Type,string AppChannelID, string AppVersionNumber)
        {
            var rinfo = userService.AppRegister2(AppId, UserName, Password, Type);
            if (rinfo.Success)
            {
                string[] strs = rinfo.Data.ToString().Split('|');

                string userid = strs[0];
                //若查询用户数据为空则为注册
                var record = hashRedis.Get<Rds_UserLoginRecords>("Rds_UseAppRecords", userid);
                if (record != null)
                {
                    var userrecord = record.records.FirstOrDefault(a => a.Status == 1);
                    if (userrecord == null)
                    {
                        record.records.Add(new Rds_UserLoginRecord()
                        {
                            AppChannelID = AppChannelID,
                            CreateData = DateTime.Now,
                            AppVersionNumber= AppVersionNumber,
                            UserID = long.Parse(userid),
                            Status = 1
                        });
                    }
                }
                else
                {
                    record = new Rds_UserLoginRecords();
                    record.records.Add(new Rds_UserLoginRecord()
                    {
                        AppChannelID = AppChannelID,
                        CreateData = DateTime.Now,
                        AppVersionNumber = AppVersionNumber,
                        UserID = long.Parse(userid),
                        Status = 1
                    });
                }
                var user = GetUserInfoByUserId(userid.ToInt());
            }
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        public APIResponse AppModifyPassWord(string AppId, string UserId, string Password, string OldPassword)
        {
            var rinfo = userService.AppModifyPassWord(AppId, UserId, Password, OldPassword);
            if (rinfo.Success)
            {
                var user = GetUserInfoByUserId(Convert.ToInt32(UserId));
                if (user != null)
                {
                    user.UserPwd = Password;
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserId, user);
                }
            }
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        public APIResponse AppResetPassWord(string AppId, string UserId, string Password)
        {
            var rinfo = userService.AppResetPassWord(AppId, UserId, Password);
            if (rinfo.Success)
            {
                var user = GetUserInfoByUserId(Convert.ToInt32(UserId));
                if (user != null)
                {
                    user.UserPwd = Password;
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserId, user);
                }
            }
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }
        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserNum"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public APIResponse AppLoginOut(string AppId, string UserNum, string UserId)
        {
            var rinfo = userService.AppLoginOut(AppId, UserNum, UserId);
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }
        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserNum"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string LoginOut(string AppId, string UserId,out string Message)
        {
            var rinfo = userService.LoginOut(AppId, UserId, out Message);
            return rinfo;
        }
        /// <summary>
        /// 使用手机号成为某一用户唯一的,使用接口前业务系统需要验证用户手机号 
        /// </summary>
        /// <param name="Telephone"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public APIResponse VerifyUserPhone(string Telephone, string UserId)
        {
            var rinfo = userService.VerifyUserPhone(Telephone, UserId);
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }

        public UserStateEnum CheckLoginUserState(string UserId, string UserNum, string AppId, string ClientIP)
        {
            UserStateEnum result;
            var rinfo=userService.CheckLoginUserState(UserId, UserNum, AppId, ClientIP);
            result = (UserStateEnum)Enum.Parse(typeof(UserStateEnum), rinfo.ToString());
            return result;
            
        }

        /// <summary>
        /// APP查询登陆状态
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public APIResponse AppCheckUserState(string AppId, string UserId)
        {
            var rinfo = userService.AppCheckUserState(AppId, UserId);
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }
        public APIResponse ModifyUserSchool(string AppID, string UserID, int SchID, string SchName, string UserName)
        {
            var rinfo = userService.ModifyUserSchool(AppID, UserID, SchID, SchName, UserName);
            APIResponse result = new APIResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<PowerList> GetUserPowerList(string UserId, string appId)
        {
            List<PowerList> repowerList=null;
            var powerlist = userService.GetUserPowerList(UserId, appId);
            if (powerlist != null) 
            {
                repowerList= new List<PowerList>();
                foreach (var a in powerlist) 
                {
                    PowerList p = new PowerList();
                    p.AppID = a.AppID;
                    p.AppName = a.AppName;
                    p.Code = a.Code;
                    p.Depth = a.Depth;
                    p.FunctionID = a.FunctionID;
                    p.funID = a.funID;
                    p.GID = a.GID;
                    p.GroupName = a.GroupName;
                    p.IsShow = a.IsShow;
                    p.LinkUrl = a.LinkUrl;
                    p.ManagerURL = a.ManagerURL;
                    p.Name = a.Name;
                    p.Order = a.Order;
                    p.ParentID = a.ParentID;
                    p.State = a.State;
                }
            }
            return repowerList;
        }

        /// <summary>
        /// 获取powerID
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="AppId"></param>
        /// <returns></returns>
        public string GetUserPowerID(string UserId, string AppId) 
        {
            string re=userService.GetUserPowerID(UserId, AppId);
            return re;
        }
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
        public APIResponse AppUpdateUserInfo(string AppId, int UserId, string TrueName)
        {
            APIResponse result = new APIResponse();
            try
            {
                FZUUMS_UserService.User moduUser = new FZUUMS_UserService.User();
                moduUser.UserID = UserId.ToString();
                moduUser.TrueName = TrueName;

                var res = userService.UpdateUserInfo2(AppId, moduUser);
                result.Success = res.Success;
                result.ErrorMsg = res.ErrorMsg;
                result.Data = res.Data;
                if (res.Success)
                {
                    var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserId.ToString());
                    if (user != null)
                    {
                        //修改用户数据
                        user.TrueName = string.IsNullOrEmpty(TrueName) ? user.TrueName : TrueName;
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserId.ToString(), user);
                        //同时修改IBS_ClassUserRelation中用户名称
                        user.ClassSchList.ForEach(a => 
                        {
                            var classinfo=hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID);
                            if (classinfo != null) 
                            {
                                if (user.UserType == (int)UserTypeEnum.Teacher) 
                                {
                                        classinfo.ClassTchList.ForEach(x => 
                                        {
                                            if (x.TchID == user.UserID) 
                                            {
                                                x.TchName = user.TrueName;
                                            }
                                        }) ;
                                }
                                else if (user.UserType == (int)UserTypeEnum.Student) 
                                {
                                    classinfo.ClassStuList.ForEach(x =>
                                    {
                                        if (x.StuID == user.UserID)
                                        {
                                            x.StuName = user.TrueName;
                                        }
                                    });
                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID, classinfo);
                            }
                        });

                    }


                    //DB操作
                    var dbUser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == UserId).FirstOrDefault();
                    if (dbUser != null)
                    {
                        dbUser.TrueName = string.IsNullOrEmpty(TrueName) ? dbUser.TrueName : TrueName;
                        repository2.Update(dbUser);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "AppUpdateUserInfo接口出错,UserId=" + UserId, ex);
                result.Success = false;
                result.ErrorMsg = "AppUpdateUserInfo接口异常";
            }

            return result;
        }
        #region 增删改

        public bool Add(TBX_UserInfo userInfo)
        {
            var result = false;
            //try
            //{
            //    #region 调用MOD接口
            //    FZUUMS_UserService.User modAddUser = new FZUUMS_UserService.User();
            //    modAddUser.UserName = userInfo.iBS_UserInfo.UserName;
            //    modAddUser.UserType = userInfo.iBS_UserInfo.UserType;
            //    modAddUser.TrueName = userInfo.iBS_UserInfo.TrueName;
            //    modAddUser.Telephone = userInfo.iBS_UserInfo.TelePhone;
            //    modAddUser.RegDate = DateTime.Now;
            //    modAddUser.PassWord = userInfo.iBS_UserInfo.UserPwd;
            //    var user = userService.InserUserSingle("", modAddUser);
            //    if (user == null)
            //    {
            //        result = false;
            //        return result;
            //    }
            //    #endregion

            //    #region IBS本地数据更新
            //    //IBSRedis
            //    var ibsuser = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", userInfo.iBS_UserInfo.UserID.ToString());
            //    if (ibsuser != null)
            //    {
            //        hashRedis.Remove("IBS_UserOtherID", ibsuser.UserName);
            //        hashRedis.Remove("IBS_UserOtherID", ibsuser.UserNum);
            //        hashRedis.Remove("IBS_UserOtherID", ibsuser.TelePhone);

            //        //若用户身份变更需要做班级变更

            //    }
            //    else
            //    {
            //        ibsuser = new IBS_UserInfo();
            //    }
            //    ibsuser.UserID = userInfo.iBS_UserInfo.UserID;
            //    ibsuser.TrueName = userInfo.iBS_UserInfo.TrueName;
            //    ibsuser.UserType = userInfo.iBS_UserInfo.UserType;
            //    ibsuser.UserRoles = userInfo.iBS_UserInfo.UserRoles;
            //    ibsuser.UserPwd = userInfo.iBS_UserInfo.UserPwd;
            //    ibsuser.UserNum = userInfo.iBS_UserInfo.UserNum;
            //    ibsuser.UserName = userInfo.iBS_UserInfo.UserName;
            //    ibsuser.TelePhone = userInfo.iBS_UserInfo.TelePhone;
            //    ibsuser.Regdate = userInfo.iBS_UserInfo.Regdate;
            //    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userInfo.iBS_UserInfo.UserID.ToString(), ibsuser);

            //    //DB操作
            //    var dbuser = userDAL.SelectSearch(a => a.UserID == Convert.ToInt32(user.UserID)).FirstOrDefault();
            //    if (dbuser != null)
            //    {
            //        Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
            //        userDAL.Update(dbuser);
            //    }
            //    else
            //    {
            //        dbuser = new Tb_UserInfo();
            //        Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
            //        userDAL.Insert(dbuser);
            //    }
            //    #endregion
            //    result = true;
            //}
            //catch (Exception ex)
            //{
            //    Log4Net.LogHelper.Error(ex, "IBS用户Add接口异常，UserInfo=" + JsonHelper.EncodeJson(userInfo));
            //    result = false;
            //}
            return result;

        }

        public APIResponse UpdateUserInfoNoOnly(string AppId, IBS_UserInfo user)
        {
            APIResponse result = null;
            if (user != null)
            {
                FZUUMS_UserService.User moduser = new FZUUMS_UserService.User();
                moduser.UserID = user.UserID.ToString();
                moduser.UserType = user.UserType;
                var returnInfo = userService.UpdateUserInfoNoOnly(AppId, moduser);
                if (returnInfo != null)
                {
                    if (returnInfo.Success)
                    {
                        var ibsuser = GetUserInfoByUserId(user.UserID.ToInt());
                        ibsuser.UserType = user.UserType;
                        ibsuser.isLogState = user.isLogState;
                        //DB操作
                        var dbUser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == user.UserID).FirstOrDefault();
                        if (dbUser != null)
                        {
                            dbUser.isLogState = user.isLogState;
                            repository2.Update(dbUser);
                        }
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), ibsuser);

                    }
                    result = new APIResponse();
                    result.Success = returnInfo.Success;
                    result.Data = returnInfo.Data;
                    result.ErrorMsg = returnInfo.ErrorMsg;
                }
            }
            return result;
        }


        public bool Update(TBX_UserInfo userInfo)
        {

            var result = false;
            try
            {
                #region 调用MOD接口
                var returninfo = userService.AppUpdateUserInfo("", userInfo.iBS_UserInfo.UserID.ToString(), userInfo.iBS_UserInfo.TrueName, userInfo.iBS_UserInfo.UserRoles, "", "", "");
                #endregion

                if (returninfo)
                {
                    #region IBS本地数据库
                    //DB操作
                    var dbuser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == userInfo.iBS_UserInfo.UserID).FirstOrDefault();
                    if (dbuser != null)
                    {
                        Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                        var re = repository2.Update(dbuser);
                        if (!re)
                        {
                            result = false;
                            return result;
                        }
                    }
                    else
                    {
                        dbuser = new Tb_UserInfo();
                        Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                        var re = repository2.Insert(dbuser);
                        if (re != null)
                        {
                            result = false;
                            return result;
                        }
                    }

                    //IBSRedis
                    var ibsUser = GetUserInfoByUserId(Convert.ToInt32(userInfo.iBS_UserInfo.UserID));
                    if (ibsUser != null)
                    {

                        //若手机号变更  新增手机号映射关系 
                        RemoveAndCreateRdsTelephone(userInfo, ibsUser);

                        ibsUser.UserID = userInfo.iBS_UserInfo.UserID;
                        ibsUser.UserName = userInfo.iBS_UserInfo.UserName ?? ibsUser.UserName;
                        ibsUser.UserType = userInfo.iBS_UserInfo.UserType > 0 ? ibsUser.UserType : userInfo.iBS_UserInfo.UserType;
                        ibsUser.TrueName = userInfo.iBS_UserInfo.TrueName ?? ibsUser.TrueName;
                        ibsUser.TelePhone = userInfo.iBS_UserInfo.TelePhone ?? ibsUser.TelePhone;
                        ibsUser.UserPwd = userInfo.iBS_UserInfo.UserPwd ?? ibsUser.UserPwd;
                        ibsUser.UserImage = userInfo.iBS_UserInfo.UserImage ?? ibsUser.UserImage;
                        ibsUser.IsEnableOss = userInfo.iBS_UserInfo.IsEnableOss;


                        if (ibsUser.UserType == (int)UserTypeEnum.Teacher)
                        {
                            ibsUser.ClassSchList.ForEach(a =>
                            {
                                var classinfo = GetClassUserRelationByClassId(a.ClassID);

                                if (classinfo != null)
                                {
                                    var tch = classinfo.ClassTchList.FirstOrDefault(x => x.TchID == ibsUser.UserID);
                                    if (tch != null)
                                    {
                                        tch.TchName = ibsUser.TrueName;
                                        tch.UserImage = ibsUser.UserImage;
                                        tch.IsEnableOss = ibsUser.IsEnableOss;
                                    }
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                                }
                            });
                        }
                        else
                        {
                            ibsUser.ClassSchList.ForEach(a =>
                            {
                                var classinfo = GetClassUserRelationByClassId(a.ClassID);
                                if (classinfo != null)
                                {
                                    var stu = classinfo.ClassStuList.FirstOrDefault(x => x.StuID == ibsUser.UserID);
                                    if (stu != null)
                                    {
                                        stu.StuName = ibsUser.TrueName;
                                        stu.UserImage = ibsUser.UserImage;
                                        stu.IsEnableOss = ibsUser.IsEnableOss;
                                    }
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                                }
                            });
                        }


                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userInfo.iBS_UserInfo.UserID.ToString(), ibsUser);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    #endregion
                   
                }

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "IBS用户Update2接口异常，UserInfo=" + userInfo.ToJson(), ex);
                result= false;
            }
            return result;
        }


     /*   public APIResponse Update(TBX_UserInfo userInfo)
        {
            var result = new APIResponse();
            try
            {
                #region 调用MOD接口
                FZUUMS_UserService.User modAddUser = new FZUUMS_UserService.User();
                if (userInfo.iBS_UserInfo.UserID < 0)
                {
                    result.Success = false;
                    result.ErrorMsg = "UserID不能为空！";
                    return result;
                }
                modAddUser.UserID = userInfo.iBS_UserInfo.UserID.ToString();
                if (!string.IsNullOrEmpty(userInfo.iBS_UserInfo.UserName))
                {
                    modAddUser.UserName = userInfo.iBS_UserInfo.UserName;
                }
                if (userInfo.iBS_UserInfo.UserType>0)
                {
                    modAddUser.UserType = userInfo.iBS_UserInfo.UserType;
                }
                if (!string.IsNullOrEmpty(userInfo.iBS_UserInfo.TrueName))
                {
                    modAddUser.TrueName = userInfo.iBS_UserInfo.TrueName;
                }
                if (!string.IsNullOrEmpty(userInfo.iBS_UserInfo.TelePhone))
                {
                    modAddUser.Telephone = userInfo.iBS_UserInfo.TelePhone;
                }
                if (!string.IsNullOrEmpty(userInfo.iBS_UserInfo.UserPwd))
                {
                    modAddUser.PassWord = userInfo.iBS_UserInfo.UserPwd;
                }
                var returninfo = userService.UpdateUserInfo2("", modAddUser);
                #endregion
                result.Success = returninfo.Success;
                result.ErrorMsg = returninfo.ErrorMsg;
                result.Data = returninfo.Data;
                if (returninfo.Success)
                {
                    #region IBS本地数据库
                    //DB操作
                   var dbuser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == userInfo.iBS_UserInfo.UserID).FirstOrDefault();
                if (dbuser != null)
                {
                    Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                    repository2.Update(dbuser);
                   
                }
                else
                {
                    dbuser = new Tb_UserInfo();
                    Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                    repository2.Insert(dbuser);
                }
                    #endregion
                    //IBSRedis
                    var ibsUser =GetUserInfoByUserId(Convert.ToInt32(userInfo.iBS_UserInfo.UserID));
                if(ibsUser!=null)
                {
                    
                    
                    ibsUser.UserID = userInfo.iBS_UserInfo.UserID;
                    ibsUser.UserName = userInfo.iBS_UserInfo.UserName?? ibsUser.UserName;
                    ibsUser.UserType = userInfo.iBS_UserInfo.UserType;
                    ibsUser.TrueName = userInfo.iBS_UserInfo.TrueName?? ibsUser.TrueName;
                    ibsUser.TelePhone = userInfo.iBS_UserInfo.TelePhone?? ibsUser.TelePhone;
                    ibsUser.Regdate = DateTime.Now;
                    ibsUser.UserPwd = userInfo.iBS_UserInfo.UserPwd;

                    //若手机号变更  新增手机号映射关系 
                    if (string.IsNullOrEmpty(userInfo.iBS_UserInfo.TelePhone))
                    {
                        //移除旧手机号映射
                        hashRedis.Remove("IBS_UserOtherID", ibsUser.TelePhone + "_" + 1);

                        IBS_UserOtherID telephone = new IBS_UserOtherID();
                        telephone.UserIDOther = userInfo.iBS_UserInfo.TelePhone;
                        telephone.UserID = ibsUser.UserID;
                        telephone.Type = 1;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", userInfo.iBS_UserInfo.TelePhone + "_" + 1, telephone);
                    }
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userInfo.iBS_UserInfo.UserID.ToString(), ibsUser);

                    ibsUser.UserID = userInfo.iBS_UserInfo.UserID;
                    ibsUser.UserName = userInfo.iBS_UserInfo.UserName;
                    ibsUser.UserType = userInfo.iBS_UserInfo.UserType;
                    ibsUser.TrueName = userInfo.iBS_UserInfo.TrueName;
                    ibsUser.TelePhone = userInfo.iBS_UserInfo.TelePhone;
                    ibsUser.Regdate = DateTime.Now;
                    ibsUser.UserPwd = userInfo.iBS_UserInfo.UserPwd;
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userInfo.iBS_UserInfo.UserID.ToString(), ibsUser);
                }

                   
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "IBS用户Update接口异常，UserInfo=" + userInfo.ToJson(), ex);
                result.Success = false;
                result.ErrorMsg = "IBS用户Update接口异常";
            }
            return result;
        }
        */
        public APIResponse UpdateUserInfo(string userId, string trueName, string SchID, string SchName, int[] subjectid, string[] subjectName) 
        {
            APIResponse result = null;
            var returninfo = relationservice.UpdateUserInfo(userId, trueName, SchID, SchName, subjectid, subjectName);
            if (returninfo != null)
            {
                if (returninfo.Success)
                {
                    var ibsuser = GetUserInfoByUserId(Convert.ToInt32(userId));
                    var user = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == Convert.ToInt32(userId)).FirstOrDefault();
                    if (user != null)
                    {
                        user.TrueName = trueName;
                        repository2.Update(user);
                    }

                    if (ibsuser != null)
                    {
                        if (!string.IsNullOrEmpty(SchID))
                        {
                            ibsuser.SchoolID = Convert.ToInt32(SchID);
                            ibsuser.SchoolName = SchName;
                        }
                        ibsuser.TrueName = trueName;

                        //修改学生或老师的名称
                        if (ibsuser.UserType == (int)UserTypeEnum.Teacher)
                        {
                            ibsuser.ClassSchList.ForEach(a =>
                            {
                                var classinfo = GetClassUserRelationByClassId(a.ClassID);

                                if (classinfo != null)
                                {
                                    var stu = classinfo.ClassTchList.FirstOrDefault(x => x.TchID == ibsuser.UserID);
                                    if (stu != null)
                                    {
                                        stu.TchName = ibsuser.TrueName;
                                    }
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                                }
                            });
                        }
                        else
                        {
                            ibsuser.ClassSchList.ForEach(a =>
                            {
                                var classinfo =GetClassUserRelationByClassId(a.ClassID);
                                if (classinfo != null)
                                {
                                    var stu = classinfo.ClassStuList.FirstOrDefault(x => x.StuID == ibsuser.UserID);
                                    if (stu != null)
                                    {
                                        stu.StuName = ibsuser.TrueName;
                                    }
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                                }
                            });
                        }
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userId, ibsuser);
                    }
                }
                result = new APIResponse();
                result.Success = returninfo.Success;
                result.Data = returninfo.Data;
                result.ErrorMsg = returninfo.ErrorMsg;
            }
            return result;
        }
        /// <summary>
        /// 初始新增
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateUserAndOtherInfo(TBX_UserInfo userInfo)
        {
            var result = false;
            try
            {
                #region 调用MOD接口
                FZUUMS_UserService.User modAddUser = new FZUUMS_UserService.User();
                modAddUser.UserID = userInfo.iBS_UserInfo.UserID.ToString();
                modAddUser.UserName = userInfo.iBS_UserInfo.UserName;
                modAddUser.UserType = userInfo.iBS_UserInfo.UserType;
                modAddUser.TrueName = userInfo.iBS_UserInfo.TrueName;
                modAddUser.Telephone = userInfo.iBS_UserInfo.TelePhone;
                modAddUser.RegDate = DateTime.Now;
                modAddUser.PassWord = userInfo.iBS_UserInfo.UserPwd;
                var otherInfo = userInfo.ClassSchDetailList.FirstOrDefault();
                if (otherInfo != null)
                {
                    modAddUser.AeraID = otherInfo.AreaID.ToString();
                    modAddUser.SchoolID = otherInfo.SchID;
                    modAddUser.SchoolName = otherInfo.SchName;
                    modAddUser.SubjectID = otherInfo.SubjectID;
                    modAddUser.SubjectName = otherInfo.SubjectName;
                }
                modAddUser.CityID = userInfo.CityID;
                modAddUser.ProID = userInfo.ProvinceID;

                var returninfo = userService.UpdateUserInfo2("", modAddUser);
                if (!returninfo.Success)
                {
                    result = false;
                    return result;
                }
                #endregion

                #region IBS本地数据库
                //DB操作
                var dbuser = repository2.SelectSearch<Tb_UserInfo>(a => a.UserID == userInfo.iBS_UserInfo.UserID).FirstOrDefault();
                if (dbuser != null)
                {
                    Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                    var re = repository2.Update(dbuser);
                    if (!re)
                    {
                        result = false;
                        return result;
                    }
                }
                else
                {
                    dbuser = new Tb_UserInfo();
                    Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                    dbuser.CreateTime = DateTime.Now;
                    var re = repository2.Insert(dbuser);
                    if (re!=null)
                    {
                        result = false;
                        return result;
                    }
                }
                #endregion

                #region IBSRedis
                //IBSRedis
                var ibsUser = GetUserInfoByUserId(Convert.ToInt32(userInfo.iBS_UserInfo.UserID));
                if (ibsUser != null)
                {


                    ibsUser.UserID = userInfo.iBS_UserInfo.UserID;
                    ibsUser.UserName = userInfo.iBS_UserInfo.UserName ?? ibsUser.UserName;
                    ibsUser.UserType = userInfo.iBS_UserInfo.UserType;
                    ibsUser.TrueName = userInfo.iBS_UserInfo.TrueName ?? ibsUser.TrueName;
                    ibsUser.TelePhone = userInfo.iBS_UserInfo.TelePhone ?? ibsUser.TelePhone;
                    ibsUser.Regdate = userInfo.iBS_UserInfo.Regdate ?? ibsUser.Regdate;
                    ibsUser.UserPwd = userInfo.iBS_UserInfo.UserPwd ?? ibsUser.UserPwd;
                    ibsUser.SchoolID = userInfo.iBS_UserInfo.SchoolID ?? ibsUser.SchoolID;
                    ibsUser.SchoolName = userInfo.iBS_UserInfo.SchoolName ?? ibsUser.SchoolName;

                    //若手机号变更  先移除再新增新增手机号映射关系
                    RemoveAndCreateRdsUserName(userInfo, ibsUser);

                    //若手机号变更  新增手机号映射关系 
                    RemoveAndCreateRdsTelephone(userInfo, ibsUser);
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userInfo.iBS_UserInfo.UserID.ToString(), ibsUser);
                }
                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "IBS用户Update接口异常，UserInfo=" + userInfo.ToJson(), ex);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 若手机号变更  先移除旧映射再新增手机号映射关系 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ibsUser"></param>
        private static void RemoveAndCreateRdsTelephone(TBX_UserInfo userInfo, IBS_UserInfo ibsUser)
        {
            if (string.IsNullOrEmpty(userInfo.iBS_UserInfo.TelePhone))
            {
                //移除旧手机号映射
                hashRedis.Remove("IBS_UserOtherID", ibsUser.TelePhone + "_" + 1);

                IBS_UserOtherID telephone = new IBS_UserOtherID();
                telephone.UserIDOther = userInfo.iBS_UserInfo.TelePhone;
                telephone.UserID = ibsUser.UserID;
                telephone.Type = 1;
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", userInfo.iBS_UserInfo.TelePhone + "_" + 1, telephone);
            }
        }

        /// <summary>
        /// 若用户名变更  先移除旧映射再新增新增用户名映射关系 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ibsUser"></param>
        private static void RemoveAndCreateRdsUserName(TBX_UserInfo userInfo, IBS_UserInfo ibsUser)
        {
            //若手机号变更  新增手机号映射关系 
            if (userInfo.iBS_UserInfo.UserName != null && ibsUser.UserName != userInfo.iBS_UserInfo.UserName)
            {
                //移除UserName映射
                hashRedis.Remove("IBS_UserOtherID", ibsUser.UserName + "_" + 2);

                IBS_UserOtherID username = new IBS_UserOtherID();
                username.UserIDOther = userInfo.iBS_UserInfo.UserName;
                username.UserID = ibsUser.UserID;
                username.Type = 2;
                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", userInfo.iBS_UserInfo.UserName + "_" + 2, username);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="str"></param>
        /// <param name="str1"></param>
        /// <returns></returns>
        public bool UpdateSubjectListByUserID(string UserID, int[] SubjectIds, string[] SubjectNames)
        {
            var l = userService.UpdateSubjectListByUserID(UserID, SubjectIds, SubjectNames);
            return l;
        }
        public bool Delete(int userId)
        {
            return true;
        }
        #endregion
        /// <summary>
        /// 数据复制
        /// </summary>
        /// <param name="baseUser">IBS用户信息</param>
        /// <param name="tbUser">TB用户信息</param>
        public void Base2TbSetValue(IBS_UserInfo baseUser, Tb_UserInfo tbUser)
        {
            tbUser.UserID = Convert.ToInt32(baseUser.UserID);
            tbUser.UserName = baseUser.UserName ?? tbUser.UserName;
            if (baseUser.UserImage != null && baseUser.UserImage != "00000000-0000-0000-0000-000000000000")
            {
                tbUser.UserImage = baseUser.UserImage;
            }
            tbUser.TrueName = baseUser.TrueName ?? tbUser.TrueName;
            tbUser.UserRoles = baseUser.UserRoles > 0 ? baseUser.UserRoles : tbUser.UserRoles;
            tbUser.TelePhone = baseUser.TelePhone ?? tbUser.TelePhone;
            tbUser.NickName = baseUser.TrueName ?? tbUser.TrueName;
            tbUser.IsUser = baseUser.IsUser==0?1: baseUser.IsUser;
            if (baseUser.Regdate != null)
            {
                tbUser.CreateTime = baseUser.Regdate;
            }
            tbUser.isLogState = string.IsNullOrEmpty(baseUser.isLogState) ? "0" : baseUser.isLogState;
            tbUser.IsEnableOss = baseUser.IsEnableOss;
        }

        #region TBX增删改查
        public Tb_UserInfo Search(Expression<Func<Tb_UserInfo,bool>> where) 
        {
            return repository2.SelectSearch(where).FirstOrDefault();
        }

        public bool Insert(Tb_UserInfo info)
        {
            var result = false;
            var re = repository2.Insert(info);
            if (re != null)
            {
                result = true;
            }
            return result;
        }
        #endregion
        
        public List<Tb_UserInfo> GetUserList(Expression<Func<Tb_UserInfo,bool>> where,string orderby="")
        {
            List<Tb_UserInfo> result=null;
            var re = repository2.SelectSearch(where, 1, orderby);
            if (re != null) 
            {
                result = re.ToList();
            }
            return result;
            
        }

        /// <summary>
        /// 添加教师信息
        /// </summary>
        /// <param name="tInfo"></param>
        /// <returns></returns>
        public APIResponse AddTeacherInfo(TBX_UserInfo tInfo, string AppID)
        {
            APIResponse response = new APIResponse();
            tInfo.iBS_UserInfo.UserType = (int)UserTypeEnum.Teacher;
            bool result =UpdateUserAndOtherInfo(tInfo);
            bool res1 = false;

            var otherInfo = tInfo.ClassSchDetailList.FirstOrDefault();
            if (otherInfo != null)
            {
                var re = ModifyUserSchool(AppID, tInfo.iBS_UserInfo.UserID.ToString(), otherInfo.SchID, otherInfo.SchName, tInfo.iBS_UserInfo.UserName);
                if (!re.Success)
                {
                    response.Success = false;
                    response.ErrorCode = 301;
                    response.ErrorMsg = "新增失败";
                }
                res1 =UpdateSubjectListByUserID(tInfo.iBS_UserInfo.UserID.ToString(), new[] { otherInfo.SubjectID }, new[] { otherInfo.SchName });
            }
            if (result && res1)
            {
                response.Success = true;
                response.Data = "新增成功！"; 
            }
            else
            {
                response.Success = false;
                response.ErrorCode = 301;
                response.ErrorMsg = "新增失败";
            }
            return response;
        }



    }
}
