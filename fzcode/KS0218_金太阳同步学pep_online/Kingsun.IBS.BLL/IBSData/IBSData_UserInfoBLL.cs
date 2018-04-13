using Kingsun.IBS.BLL.FZUUMS_Relation2;
using Kingsun.IBS.BLL.RelationService;
using Kingsun.IBS.DAL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.Model.IBS;
using Kingsun.SynchronousStudy.Models;
using User = Kingsun.IBS.BLL.FZUUMS_UserService.User;

namespace Kingsun.IBS.BLL
{
    public class IBSData_UserInfoBLL : CommonBLL, IIBSData_UserInfoBLL
    {
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
        static readonly BaseManagementOther _bmBaseDB = new BaseManagementOther();
        static RelationService.RelationService relationservice = new RelationService.RelationService();
        static FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        static MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();

        static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        static IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();
        static IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        static IBSUserDAL userDAL = new IBSUserDAL();

        public Model.IBS_UserInfo GetUserInfoByUserId(int UserId)
        {
            //从redis取数据
            IBS_UserInfo user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserId.ToString());//用户信息
            //如果没有
            if (user == null)
            {
                //从MOD+DB取数据
                user = BuildUserInfoByUserId(UserId);
                if (user != null)
                {
                    //新增IBS_UserOtherID
                    if (!string.IsNullOrEmpty(user.TelePhone))
                    {
                        //手机号
                        IBS_UserOtherID telephone = new IBS_UserOtherID();
                        telephone.UserIDOther = user.TelePhone;
                        telephone.Type = 1;
                        telephone.UserID = user.UserID;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.TelePhone + "_" + 1, telephone);
                    }
                    if (!string.IsNullOrEmpty(user.UserName))
                    {
                        //账号
                        IBS_UserOtherID userName = new IBS_UserOtherID();
                        userName.UserIDOther = user.UserName;
                        userName.Type = 2;
                        userName.UserID = user.UserID;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserName + "_" + 2, userName);

                    }


                    if (user.UserType == (int)UserTypeEnum.Teacher)
                    {
                        if (!string.IsNullOrEmpty(user.UserNum))
                        {
                            //账号
                            IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                            teachInvm.UserIDOther = user.UserNum;
                            teachInvm.Type = 3;
                            teachInvm.UserID = user.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserNum + "_" + 3, teachInvm);
                        }
                        else
                        {
                            var invnum = relationservice.SelectOrAddUserInvNumByUserId(user.UserID.ToString());
                            if (!string.IsNullOrEmpty(invnum) && invnum.isNumberic1())
                            {
                                IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                                teachInvm.UserIDOther = invnum;
                                teachInvm.Type = 3;
                                teachInvm.UserID = user.UserID;
                                user.UserNum = invnum;
                                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserNum + "_" + 3, teachInvm);

                            }
                        }
                    }
                    //存入redis
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserId.ToString(), user);
                }

            }

            if (user != null)
            {
                user.TrueName = string.IsNullOrEmpty(user.TrueName) ? "暂未填写" : user.TrueName;
            }

            return user;

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
                    ClassSchDetail csDetail = new ClassSchDetail();
                    //班级信息
                    var classInfo = classBLL.GetClassUserRelationByClassId(a.ClassID.ToUpper());
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
                                csDetail.SubjectName = StringEnumHelper.GetStringValue<SubjectEnum>(csDetail.SubjectID);
                            }
                            else
                            {
                                csDetail.SubjectName = "";
                            }

                        }
                        //学校信息
                        csDetail.SchID = a.SchID;
                        if (a.SchID > 0)
                        {
                            var schInfo = schBLL.GetSchClassRelationBySchlId(a.SchID);
                            if (schInfo != null)
                            {
                                csDetail.SchName = schInfo.SchName;
                            }
                            else
                            {
                                csDetail.SchName = "";
                            }

                        }
                        else
                        {
                            csDetail.SchName = "";
                        }
                        //年级
                        csDetail.GradeID = a.GradeID;
                        csDetail.GradeName = a.GradeID.GetGradeName();

                        //区域信息
                        csDetail.AreaID = a.AreaID;
                        if (a.AreaID > 0)
                        {
                            var areainfo = areaBLL.GetAreaSchRelationByAreaId(a.AreaID);
                            if (areainfo != null) csDetail.AreaName = areainfo.AreaName;
                        }
                        tbxUserInfo.ClassSchDetailList.Add(csDetail);
                    }

                });
            }
            return tbxUserInfo;

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
                        var dbUser = userDAL.SelectSearch(a => a.UserID == user.UserID).FirstOrDefault();
                        if (dbUser != null)
                        {
                            dbUser.TrueName = TrueName;
                            userDAL.Update(dbUser);
                        }
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), user);

                    }

                }
            }
            else
            {
                user.UserType = (int)UserTypeEnum.Teacher;
                User moduser = new User();
                moduser.UserID = user.UserID.ToString();
                moduser.UserType = user.UserType;
                moduser.TrueName = TrueName;
                var res = userService.UpdateUserInfo2(AppID, moduser);

                var invnum = relationservice.SelectOrAddUserInvNumByUserId(user.UserID.ToString());
                if (!string.IsNullOrEmpty(invnum) && invnum.isNumberic1())
                {
                    IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                    teachInvm.UserIDOther = invnum;
                    teachInvm.Type = 3;
                    teachInvm.UserID = user.UserID;
                    user.UserNum = invnum;
                    hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserNum + "_" + 3, teachInvm);

                }
                user.TrueName = TrueName;
                var dbUser = userDAL.SelectSearch(a => a.UserID == user.UserID).FirstOrDefault();
                if (dbUser != null)
                {
                    dbUser.TrueName = TrueName;
                    userDAL.Update(dbUser);
                }
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), user);
            }
            return user;
        }

        public ClassInfoByUserID GetClassInfoByUserID(int UserId)
        {
            var user = GetUserInfoByUserId(UserId);
            if (user != null)
            {
                if (user.ClassSchList.Count > 0)
                {
                    var userClass = user.ClassSchList.FirstOrDefault();
                    if (userClass != null)
                    {
                        var classinfo = classBLL.GetClassUserRelationByClassId(userClass.ClassID);
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
                }
            }
            return null;
        }

        public KingResponse TBXLoginByPhone(string AppId, string Telephone, int UserType, TB_UserInfoExtend extend,
            string appNum)
        {
            var user = GetUserInfoByUserOtherID(Telephone, 1);
            KingResponse result = new KingResponse();
            if (user != null)
            {
                var tbuser = userDAL.SelectSearch(a => a.TelePhone == Telephone).FirstOrDefault();
                if (tbuser != null)
                {
                    tbuser.AppId = appNum;
                    userDAL.Update(tbuser);
                }
                user.AppID = appNum;
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), user);
            }
            else
            {

                user = new IBS_UserInfo();
                user.UserID = Math.Abs(Guid.NewGuid().GetHashCode());
                user.TelePhone = Telephone;
                user.UserPwd = Utils.Number(6,false).MD5Password();
                user.UserType = UserType;
                user.IsUser = 1;
                user.IsEnableOss = 0;
                user.UserImage = "00000000-0000-0000-0000-000000000000";
                user.UserName = "TBX" + Telephone;
                user.Regdate = DateTime.Now;
                user.TrueName = "";
                user.AppID = appNum;
                user.isLogState = "1";


                User modUser = new User();
                modUser.UserID = user.UserID.ToString();
                modUser.UserName = user.UserName;
                modUser.UserType = user.UserType;
                modUser.TrueName = user.TrueName;
                modUser.Telephone = user.TelePhone;
                modUser.State = 0;
                modUser.RegDate = user.Regdate;
                modUser.PassWord = user.UserPwd;
                modUser.LoginNum = 1;
                modUser.LastLoginDate = DateTime.Now;
                var returninfo = userService.CBSSAddUserInfo(modUser);
                if (returninfo != null)
                {
                    if (returninfo.Success)
                    {
                        var tbuser = userDAL.SelectSearch(a => a.TelePhone == Telephone).FirstOrDefault();
                        if (tbuser == null)
                        {
                            tbuser = new Tb_UserInfo();
                            tbuser.UserID = Convert.ToInt32(user.UserID);
                            tbuser.UserImage = user.UserImage;
                            tbuser.TrueName = user.TrueName;
                            tbuser.UserName = user.UserName;
                            tbuser.UserRoles = 0;
                            tbuser.IsEnableOss = user.IsEnableOss;
                            tbuser.isLogState = user.isLogState;
                            tbuser.IsUser = user.IsUser;
                            tbuser.NickName = user.TrueName;
                            tbuser.TelePhone = Telephone;
                            tbuser.CreateTime = user.Regdate;
                            tbuser.AppId = user.AppID;
                            userDAL.Insert(tbuser);
                        }
                        else
                        {
                            tbuser.AppId = user.AppID;
                            userDAL.Insert(tbuser);
                        }

                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), user);

                        //新增IBS_UserOtherID
                        if (!string.IsNullOrEmpty(user.TelePhone))
                        {
                            //手机号
                            IBS_UserOtherID telephone = new IBS_UserOtherID();
                            telephone.UserIDOther = user.TelePhone;
                            telephone.Type = 1;
                            telephone.UserID = user.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.TelePhone + "_" + 1, telephone);
                        }
                        if (!string.IsNullOrEmpty(user.UserName))
                        {
                            //账号
                            IBS_UserOtherID userName = new IBS_UserOtherID();
                            userName.UserIDOther = user.UserName;
                            userName.Type = 2;
                            userName.UserID = user.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserName + "_" + 2, userName);

                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMsg = "注册失败！" + returninfo.ErrorMsg;
                        result.Data = "";
                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorMsg = "注册失败！";
                    result.Data = "";
                    return result;
                }
                if (!string.IsNullOrEmpty(extend.EquipmentID))
                {
                    var ex = _bmBaseDB.Search<TB_UserInfoExtend>("UserID='" + user.UserID + "'");
                    TB_UserInfoExtend ext = null;
                    if (ex != null)
                    {
                        ext = ex.FirstOrDefault();
                    }
                    if (ext == null)
                    {
                        ext = new TB_UserInfoExtend();
                        ext.UserID = user.UserID.ToString();
                        ext.EquipmentID = extend.EquipmentID;
                        ext.DeviceType = extend.DeviceType;
                        ext.IPAddress = extend.IPAddress;
                        ext.CreateDate = DateTime.Now;
                        _bmBaseDB.Insert<TB_UserInfoExtend>(ext);
                    }
                    else
                    {
                        ext.UserID = user.UserID.ToString();
                        ext.EquipmentID = extend.EquipmentID;
                        ext.DeviceType = extend.DeviceType;
                        ext.IPAddress = extend.IPAddress;
                        ext.CreateDate = DateTime.Now;
                        _bmBaseDB.Update<TB_UserInfoExtend>(ext);
                    }

                }
               
            }
            result.Success = true;
            result.ErrorMsg = "";
            string classNum = "";
            var classinfo = user.ClassSchList.FirstOrDefault();
            if (classinfo != null)
            {
                var cl = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                if (cl != null)
                {
                    classNum = cl.ClassNum;
                }
            }
            result.Data = user.UserID + "|" + user.UserName + "|" + classNum;

            return result;
        }

        /// <summary>
        /// 通过其他内容获取用户信息
        /// </summary>
        /// <param name="UserOtherID"></param>
        /// <param name="Type">TelePhone=1,UserName=2,TchInvNum=3</param>
        /// <returns></returns>
        public Model.IBS_UserInfo GetUserInfoByUserOtherID(string UserOtherID, int Type)
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
                        var userByTelephone = userService.GetUserInfoByTelephone("", UserOtherID);
                        if (userByTelephone != null)
                        {
                            userOther.UserID = Convert.ToInt32(userByTelephone.UserID);
                            userOther.UserIDOther = UserOtherID;
                            userOther.Type = 1;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                            user = GetUserInfoByUserId(Convert.ToInt32(userOther.UserID));
                        }
                        break;
                    case 2://UserName
                        var userByName = userService.GetUserInfoByName("", UserOtherID);
                        if (userByName != null)
                        {
                            userOther.UserID = Convert.ToInt32(userByName.UserID);
                            userOther.UserIDOther = UserOtherID;
                            userOther.Type = 2;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserOtherID + "_" + Type, userOther);
                            user = GetUserInfoByUserId(Convert.ToInt32(userOther.UserID));
                        }
                        break;
                    case 3://TchInvNum
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
                        break;
                }

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
                        User userByTelephone = userService.GetUserInfoByTelephone("", UserOtherID);
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
                        User userByName = userService.GetUserInfoByName("", UserOtherID);
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
        public string GetBookData(string Stage, string Grade, string Subject, string Edition, string Booklet)
        {
            var result = metadataService.GetBookData(Stage, Grade, Subject, Edition, Booklet);
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
        public KingResponse TBXLoginByPhone(string AppId, string Telephone, int UserType)
        {
            var user = GetUserInfoByUserOtherID(Telephone, 1);
            KingResponse result = new KingResponse();
            string classNum = "";
            if (user == null)
            {
                user = new IBS_UserInfo();
                user.UserID = Math.Abs(Guid.NewGuid().GetHashCode());
                user.TelePhone = Telephone;
                user.UserPwd = Utils.Number(6, false).MD5Password();
                user.UserType = UserType;
                user.IsUser =  1;
                user.IsEnableOss = 0;
                user.UserImage = "00000000-0000-0000-0000-000000000000";
                user.UserName = "TBX" + Telephone;
                user.Regdate = DateTime.Now;
                user.TrueName = "";
                user.AppID ="";
                user.isLogState = "1";


                User modUser = new User();
                modUser.UserID = user.UserID.ToString();
                modUser.UserName = user.UserName;
                modUser.UserType = user.UserType;
                modUser.TrueName = user.TrueName;
                modUser.Telephone = user.TelePhone;
                modUser.State = 0;
                modUser.RegDate = user.Regdate;
                modUser.PassWord = user.UserPwd;
                modUser.LoginNum = 1;
                modUser.LastLoginDate = DateTime.Now;
                var returninfo=userService.CBSSAddUserInfo(modUser);
                if (returninfo != null)
                {
                    if (returninfo.Success)
                    {
                        var tbuser = userDAL.SelectSearch(a => a.TelePhone == Telephone).FirstOrDefault();
                        if (tbuser == null)
                        {
                            tbuser = new Tb_UserInfo();
                            tbuser.UserID = Convert.ToInt32(user.UserID);
                            tbuser.UserImage = user.UserImage;
                            tbuser.TrueName = user.TrueName;
                            tbuser.UserName = user.UserName;
                            tbuser.UserRoles = 0;
                            tbuser.IsEnableOss = user.IsEnableOss;
                            tbuser.isLogState = user.isLogState;
                            tbuser.IsUser = user.IsUser;
                            tbuser.NickName = user.TrueName;
                            tbuser.TelePhone = Telephone;
                            tbuser.CreateTime = user.Regdate;
                            tbuser.AppId = user.AppID;
                            userDAL.Insert(tbuser);
                        }

                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), user);

                        //新增IBS_UserOtherID
                        if (!string.IsNullOrEmpty(user.TelePhone))
                        {
                            //手机号
                            IBS_UserOtherID telephone = new IBS_UserOtherID();
                            telephone.UserIDOther = user.TelePhone;
                            telephone.Type = 1;
                            telephone.UserID = user.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.TelePhone + "_" + 1, telephone);
                        }
                        if (!string.IsNullOrEmpty(user.UserName))
                        {
                            //账号
                            IBS_UserOtherID userName = new IBS_UserOtherID();
                            userName.UserIDOther = user.UserName;
                            userName.Type = 2;
                            userName.UserID = user.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", user.UserName + "_" + 2, userName);

                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMsg = "注册失败！请于管理员联系。";
                        result.Data = "";
                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorMsg = "注册失败！请于管理员联系。";
                    result.Data = "";
                    return result;
                }
              

            }

            result.Success = true;
            result.ErrorMsg = "";
            var classinfo = user.ClassSchList.FirstOrDefault();
            if (classinfo != null)
            {
                var cl = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                if (cl != null)
                {
                    classNum = cl.ClassNum;
                }
            }
            result.Data = user.UserID + "|" + user.UserName + "|" + classNum;
            return result;

            /*var returninfo = userService.TBXLoginByPhone(AppId, Telephone, UserType);
            KingResponse result = new KingResponse();
            if (returninfo.Success)
            {
                string[] strs = returninfo.Data.ToString().Split('|');
                string userid = strs[0];
                var user = GetUserInfoByUserId(userid.ToInt());
            }
            result.Success = returninfo.Success;
            result.ErrorMsg = returninfo.ErrorMsg;
            result.Data = returninfo.Data;
            return result;*/
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
        public KingResponse AppLogin(string UserName, string Password, string Model, string AppId, string Mode)
        {
            KingResponse result = new KingResponse();
            IBS_UserInfo user = null;
            if (CommonHelper.IsValidPhone(UserName))
            {
                user = GetUserInfoByUserOtherID(UserName, 1);
            }
            else
            {
                user = GetUserInfoByUserOtherID(UserName, 2);
            }
            if (user != null)
            {
                if (Password.MD5Password().Equals(user.UserPwd))
                {
                    result.Success = true;
                    result.ErrorMsg = "";
                    if (string.IsNullOrEmpty(user.UserNum))
                    {
                        result.Data = user.UserID;
                    }
                    else
                    {
                        result.Data = user.UserID + "|" + user.UserNum;
                    }
                }
                else
                {
                    result = new KingResponse();
                    result.Success = false;
                    result.ErrorMsg = "密码错误！";
                    result.Data = "";
                }
            }
            return result;


            /*  KingResponse result = new KingResponse();
              var res = userService.AppLogin(UserName, Password, Model, AppId, Mode);
              if (res.Success)
              {
                  string[] strs = res.Data.ToString().Split('|');
                  int userID = strs[0].ToInt();
                  var user = GetUserInfoByUserId(userID);
              }
              result.Success = res.Success;
              result.ErrorMsg = res.ErrorMsg;
              result.Data = res.Data;
              return result;*/
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
        public KingResponse AppLogin(string UserName, string Password, string Model, string AppId, string Mode, TB_UserInfoExtend extend)
        {

            KingResponse result = new KingResponse();
            IBS_UserInfo user = null;
            if (CommonHelper.IsValidPhone(UserName))
            {
                user = GetUserInfoByUserOtherID(UserName, 1);
                if (user == null)
                {
                    user = GetUserInfoByUserOtherID(UserName, 2);
                }
            }
            else
            {
                user = GetUserInfoByUserOtherID(UserName, 2);
            }
            if (user != null)
            {
                if (Password.MD5Password().Equals(user.UserPwd))
                {
                    result.Success = true;
                    result.ErrorMsg = "";
                    if (user.UserNum == null)
                    {
                        user.UserNum = "";
                    }
                    result.Data = user.UserID + "|" + user.UserNum;
                    if (!string.IsNullOrEmpty(extend.EquipmentID))
                    {
                        var ex = _bmBaseDB.Search<TB_UserInfoExtend>("UserID='" + user.UserID + "'");
                        TB_UserInfoExtend ext = null;
                        if (ex != null)
                        {
                            ext = ex.FirstOrDefault();
                        }
                        if (ext == null)
                        {
                            ext = new TB_UserInfoExtend();
                            ext.UserID = user.UserID.ToString();
                            ext.EquipmentID = extend.EquipmentID;
                            ext.DeviceType = extend.DeviceType;
                            ext.IPAddress = extend.IPAddress;
                            ext.CreateDate = DateTime.Now;
                            _bmBaseDB.Insert<TB_UserInfoExtend>(ext);
                        }
                        else
                        {
                            ext.UserID = user.UserID.ToString();
                            ext.EquipmentID = extend.EquipmentID;
                            ext.DeviceType = extend.DeviceType;
                            ext.IPAddress = extend.IPAddress;
                            ext.CreateDate = DateTime.Now;
                            _bmBaseDB.Update<TB_UserInfoExtend>(ext);
                        }

                    }
                }
                else
                {
                    result = new KingResponse();
                    result.Success = false;
                    result.ErrorMsg = "密码错误！";
                    result.Data = "";
                }
            }
            return result;

            /*
            KingResponse result = new KingResponse();
            var res = userService.AppLogin(UserName, Password, Model, AppId, Mode);
            if (res.Success)
            {
                string[] strs = res.Data.ToString().Split('|');
                int userID = strs[0].ToInt();
                var user = GetUserInfoByUserId(userID);
                if (!string.IsNullOrEmpty(extend.EquipmentID))
                {
                    TB_UserInfoExtend uie = new TB_UserInfoExtend();
                    IList<TB_UserInfoExtend> ext = _bmBaseDB.Search<TB_UserInfoExtend>("UserID='" + strs[0] + "'");
                    if (ext == null)
                    {
                        uie.UserID = user.UserID.ToString();
                        uie.EquipmentID = extend.EquipmentID;
                        uie.DeviceType = extend.DeviceType;
                        uie.IPAddress = extend.IPAddress;
                        uie.CreateDate = DateTime.Now;
                        _bmBaseDB.Insert<TB_UserInfoExtend>(uie);
                    }
                    else
                    {
                        uie.UserID = user.UserID.ToString();
                        uie.EquipmentID = extend.EquipmentID;
                        uie.DeviceType = extend.DeviceType;
                        uie.IPAddress = extend.IPAddress;
                        uie.CreateDate = DateTime.Now;
                        _bmBaseDB.Update<TB_UserInfoExtend>(uie);
                    }
                }
            }
            result.Success = res.Success;
            result.ErrorMsg = res.ErrorMsg;
            result.Data = res.Data;
            return result;*/
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public KingResponse AppRegister2(string AppId, string UserName, string Password, int Type)
        {
            var rinfo = userService.AppRegister2(AppId, UserName, Password, Type);
            if (rinfo.Success)
            {
                string[] strs = rinfo.Data.ToString().Split('|');
                string userid = strs[0];
                var user = GetUserInfoByUserId(userid.ToInt());
            }
            KingResponse result = new KingResponse();
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
        public KingResponse AppModifyPassWord(string AppId, string UserId, string Password, string OldPassword)
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
            KingResponse result = new KingResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }


        public KingResponse AppResetPassWord(string AppId, string UserId, string Password)
        {
            var rinfo = userService.AppResetPassWord(AppId, UserId, Password);
            if (rinfo.Success)
            {
                var user = GetUserInfoByUserId(Convert.ToInt32(UserId));
                if (user != null)
                {
                    user.UserPwd = Password.MD5Password();
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserId, user);
                }
            }
            KingResponse result = new KingResponse();
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
        public KingResponse AppLoginOut(string AppId, string UserNum, string UserId)
        {
            var rinfo = userService.AppLoginOut(AppId, UserNum, UserId);
            KingResponse result = new KingResponse();
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
        public string LoginOut(string AppId, string UserId, out string Message)
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
        public KingResponse VerifyUserPhone(string Telephone, string UserId)
        {
            var rinfo = userService.VerifyUserPhone(Telephone, UserId);
            KingResponse result = new KingResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }

        public SynchronousStudy.Common.UserStateEnum CheckLoginUserState(string UserId, string UserNum, string AppId, string ClientIP)
        {
            UserStateEnum result;
            var rinfo = userService.CheckLoginUserState(UserId, UserNum, AppId, ClientIP);
            result = (UserStateEnum)Enum.Parse(typeof(UserStateEnum), rinfo.ToString());
            return result;

        }

        /// <summary>
        /// APP查询登陆状态
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public KingResponse AppCheckUserState(string AppId, string UserId)
        {
            var rinfo = userService.AppCheckUserState(AppId, UserId);
            KingResponse result = new KingResponse();
            result.Success = rinfo.Success;
            result.ErrorMsg = rinfo.ErrorMsg;
            result.Data = rinfo.Data;
            return result;
        }
        public KingResponse ModifyUserSchool(string AppID, string UserID, int SchID, string SchName, string UserName)
        {
            var rinfo = userService.ModifyUserSchool(AppID, UserID, SchID, SchName, UserName);
            KingResponse result = new KingResponse();
            if (rinfo.Success)
            {
                var user = GetUserInfoByUserId(Convert.ToInt32(UserID));
                if (SchID > 0)
                {
                    user.SchoolID = SchID;
                    user.SchoolName = SchName;
                }
                user.TrueName = string.IsNullOrEmpty(UserName) ? user.TrueName : UserName;
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserID, user);
            }
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
            List<PowerList> repowerList = null;
            var powerlist = userService.GetUserPowerList(UserId, appId);
            if (powerlist != null)
            {
                repowerList = new List<PowerList>();
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
            string re = userService.GetUserPowerID(UserId, AppId);
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
        public KingResponse AppUpdateUserInfo(string AppId, int UserId, string TrueName)
        {
            KingResponse result = new KingResponse();
            try
            {
                User moduUser = new User();
                moduUser.UserID = UserId.ToString();
                moduUser.TrueName = TrueName;

                var res = userService.UpdateUserInfo2(AppId, moduUser);

                result.Success = res.Success;
                result.ErrorMsg = res.ErrorMsg;
                result.Data = res.Data;
                if (res.Success)
                {
                    var user = GetUserInfoByUserId(UserId);
                    if (user != null)
                    {
                        //修改用户数据
                        user.TrueName = string.IsNullOrEmpty(TrueName) ? user.TrueName : TrueName;
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserId.ToString(), user);
                        //同时修改IBS_ClassUserRelation中用户名称
                        user.ClassSchList.ForEach(a =>
                        {
                            var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID.ToUpper());
                            if (classinfo != null)
                            {
                                if (user.UserType == (int)UserTypeEnum.Teacher)
                                {
                                    var tch = classinfo.ClassTchList.FirstOrDefault(x => x.TchID == user.UserID);
                                    if (tch != null)
                                    {
                                        tch.TchName = user.TrueName;
                                    }
                                }
                                else if (user.UserType == (int)UserTypeEnum.Student)
                                {
                                    var stu = classinfo.ClassStuList.FirstOrDefault(x => x.StuID == user.UserID);
                                    if (stu != null)
                                    {
                                        stu.StuName = user.TrueName;
                                    }
                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                            }
                        });

                    }


                    //DB操作
                    var dbUser = userDAL.SelectSearch(a => a.UserID == UserId).FirstOrDefault();
                    if (dbUser != null)
                    {
                        dbUser.TrueName = string.IsNullOrEmpty(TrueName) ? dbUser.TrueName : TrueName;
                        userDAL.Update(dbUser);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "AppUpdateUserInfo接口出错,UserId=" + UserId);
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

        /// <summary>
        /// 修改登陆状态 和身份
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public KingResponse UpdateUserInfoNoOnly(string AppId, IBS_UserInfo user)
        {
            KingResponse result = null;
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
                        var ibsuser = GetUserInfoByUserId(user.UserID.ToIntOrZero());
                        ibsuser.UserType = user.UserType;
                        ibsuser.isLogState = user.isLogState;
                        //DB操作
                        var dbUser = userDAL.SelectSearch(a => a.UserID == user.UserID).FirstOrDefault();
                        if (dbUser != null)
                        {
                            dbUser.isLogState = user.isLogState;
                            userDAL.Update(dbUser);
                        }
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", user.UserID.ToString(), ibsuser);

                    }
                    result = new KingResponse();
                    result.Success = returnInfo.Success;
                    result.Data = returnInfo.Data;
                    result.ErrorMsg = returnInfo.ErrorMsg;
                }
            }
            return result;
        }


        public bool Update2(TBX_UserInfo userInfo)
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
                    var dbuser = userDAL.SelectSearch(a => a.UserID == userInfo.iBS_UserInfo.UserID).FirstOrDefault();
                    if (dbuser != null)
                    {
                        Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                        var re = userDAL.Update(dbuser);
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
                        dbuser.TelePhone = userInfo.iBS_UserInfo.TelePhone;
                        dbuser.UserName = userInfo.iBS_UserInfo.UserName;
                        var re = userDAL.Insert(dbuser);
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
                        if (!string.IsNullOrEmpty(userInfo.iBS_UserInfo.TelePhone) && userInfo.iBS_UserInfo.TelePhone != ibsUser.TelePhone)
                        {
                            //移除旧手机号映射
                            hashRedis.Remove("IBS_UserOtherID", ibsUser.TelePhone + "_" + 1);

                            IBS_UserOtherID telephone = new IBS_UserOtherID();
                            telephone.UserIDOther = userInfo.iBS_UserInfo.TelePhone;
                            telephone.UserID = ibsUser.UserID;
                            telephone.Type = 1;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", userInfo.iBS_UserInfo.TelePhone + "_" + 1, telephone);
                        }
                        ibsUser.UserID = userInfo.iBS_UserInfo.UserID;
                        ibsUser.UserType = userInfo.iBS_UserInfo.UserType > 0 ? ibsUser.UserType : userInfo.iBS_UserInfo.UserType;
                        ibsUser.TrueName = string.IsNullOrEmpty(userInfo.iBS_UserInfo.TrueName) ? ibsUser.TrueName : userInfo.iBS_UserInfo.TrueName;
                        ibsUser.UserImage = string.IsNullOrEmpty(userInfo.iBS_UserInfo.UserImage) ? ibsUser.UserImage : userInfo.iBS_UserInfo.UserImage;
                        ibsUser.IsEnableOss = userInfo.iBS_UserInfo.IsEnableOss;


                        if (ibsUser.UserType == (int)UserTypeEnum.Teacher)
                        {
                            ibsUser.ClassSchList.ForEach(a =>
                            {
                                var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);

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
                                var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
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
                    }

                    #endregion
                    result = true;
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "IBS用户Update接口异常，UserInfo=" + JsonHelper.EncodeJson(userInfo));
                result = false;
            }
            return result;
        }
        public bool Update(TBX_UserInfo userInfo)
        {
            var result = false;
            try
            {
                #region 调用MOD接口
                FZUUMS_UserService.User modAddUser = new FZUUMS_UserService.User();
                modAddUser.UserID = userInfo.iBS_UserInfo.UserID.ToString();
                modAddUser.UserType = userInfo.iBS_UserInfo.UserType;
                modAddUser.TrueName = userInfo.iBS_UserInfo.TrueName;
                var returninfo = userService.UpdateUserInfo2("", modAddUser);
                if (!returninfo.Success)
                {
                    result = false;
                    return result;
                }
                #endregion

                #region IBS本地数据库
                //DB操作
                var dbuser = userDAL.SelectSearch(a => a.UserID == userInfo.iBS_UserInfo.UserID).FirstOrDefault();
                if (dbuser != null)
                {
                    Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                    dbuser.BookID = userInfo.BookID > 0 ? userInfo.BookID : dbuser.BookID;
                    var re = userDAL.Update(dbuser);
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
                    dbuser.UserName = userInfo.iBS_UserInfo.UserName;
                    dbuser.TelePhone = userInfo.iBS_UserInfo.TelePhone;
                    dbuser.BookID = userInfo.BookID > 0 ? userInfo.BookID : dbuser.BookID;
                    var re = userDAL.Insert(dbuser);
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

                    if (!string.IsNullOrEmpty(userInfo.iBS_UserInfo.TelePhone) && userInfo.iBS_UserInfo.TelePhone != ibsUser.TelePhone)
                    {
                        //移除旧手机号映射
                        hashRedis.Remove("IBS_UserOtherID", ibsUser.TelePhone + "_" + 1);

                        IBS_UserOtherID telephone = new IBS_UserOtherID();
                        telephone.UserIDOther = userInfo.iBS_UserInfo.TelePhone;
                        telephone.UserID = ibsUser.UserID;
                        telephone.Type = 1;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", userInfo.iBS_UserInfo.TelePhone + "_" + 1, telephone);
                    }

                    ibsUser.UserID = userInfo.iBS_UserInfo.UserID;
                    ibsUser.UserType = userInfo.iBS_UserInfo.UserType > 0 ? ibsUser.UserType : userInfo.iBS_UserInfo.UserType;
                    ibsUser.TrueName = string.IsNullOrEmpty(userInfo.iBS_UserInfo.TrueName) ? ibsUser.TrueName : userInfo.iBS_UserInfo.TrueName;

                    if (ibsUser.UserType == (int)UserTypeEnum.Teacher)
                    {
                        ibsUser.ClassSchList.ForEach(a =>
                        {
                            var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);

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
                            var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
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
                }

                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "IBS用户Update接口异常，UserInfo=" + JsonHelper.EncodeJson(userInfo));
                result = false;
            }
            return result;
        }

        public KingResponse UpdateUserInfo(string userId, string trueName, string SchID, string SchName, int[] subjectid, string[] subjectName)
        {
            KingResponse result = null;
            var returninfo = relationservice.UpdateUserInfo(userId, trueName, SchID, SchName, subjectid, subjectName);
            if (returninfo != null)
            {
                if (returninfo.Success)
                {
                    var ibsuser = GetUserInfoByUserId(Convert.ToInt32(userId));
                    var user = userDAL.SelectSearch(a => a.UserID == Convert.ToInt32(userId)).FirstOrDefault();
                    if (user != null)
                    {
                        user.TrueName = trueName;
                        userDAL.Update(user);
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
                                var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);

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
                                var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
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
                result = new KingResponse();
                result.Success = returninfo.Success;
                result.Data = returninfo.Data;
                result.ErrorMsg = returninfo.ErrorMsg;
            }
            return result;
        }
        /// <summary>
        /// 初始新增 修改用户信息外还有学校ID 学科 区域等
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public KingResponse UpdateUserAndOtherInfo(TBX_UserInfo userInfo)
        {
            KingResponse result = null;
            try
            {
                result=new KingResponse();
                #region 调用MOD接口
                FZUUMS_UserService.User modAddUser = new FZUUMS_UserService.User();
                modAddUser.UserID = userInfo.iBS_UserInfo.UserID.ToString();
                modAddUser.UserType = userInfo.iBS_UserInfo.UserType;
                modAddUser.TrueName = userInfo.iBS_UserInfo.TrueName;
                modAddUser.RegDate = DateTime.Now;
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
                result.Success = returninfo.Success;
                result.ErrorMsg = returninfo.ErrorMsg;
                result.Data = returninfo.Data;
                if (!returninfo.Success)
                {
                    return result;
                }
                #endregion

                #region IBS本地数据库
                //DB操作
                var dbuser = userDAL.SelectSearch(a => a.UserID == userInfo.iBS_UserInfo.UserID).FirstOrDefault();
                if (dbuser != null)
                {
                    Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                    var re = userDAL.Update(dbuser);
                    if (!re)
                    {
                        result.Success = false;
                        result.ErrorMsg = "更新用户信息失败！";
                        return result;
                    }
                }
                else
                {
                    dbuser = new Tb_UserInfo();
                    Base2TbSetValue(userInfo.iBS_UserInfo, dbuser);
                    dbuser.UserName = userInfo.iBS_UserInfo.UserName;
                    dbuser.TelePhone = userInfo.iBS_UserInfo.TelePhone;
                    dbuser.CreateTime = DateTime.Now;
                    var re = userDAL.Insert(dbuser);
                    if (re != null)
                    {
                        result.Success = false;
                        result.ErrorMsg = "更新用户信息失败！";
                        return result;
                    }
                }
                #endregion
                //IBSRedis
                var ibsUser = GetUserInfoByUserId(Convert.ToInt32(userInfo.iBS_UserInfo.UserID));
                if (ibsUser != null)
                {


                    ibsUser.UserID = userInfo.iBS_UserInfo.UserID;
                    ibsUser.UserType = userInfo.iBS_UserInfo.UserType;
                    ibsUser.TrueName = userInfo.iBS_UserInfo.TrueName ?? ibsUser.TrueName;
                    ibsUser.Regdate = userInfo.iBS_UserInfo.Regdate ?? ibsUser.Regdate;
                    ibsUser.SchoolID = userInfo.iBS_UserInfo.SchoolID ?? ibsUser.SchoolID;
                    ibsUser.SchoolName = userInfo.iBS_UserInfo.SchoolName ?? ibsUser.SchoolName;

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

                    //若手机号变更  新增手机号映射关系 
                    if (!string.IsNullOrEmpty(userInfo.iBS_UserInfo.TelePhone))
                    {
                        //移除旧手机号映射
                        hashRedis.Remove("IBS_UserOtherID", ibsUser.TelePhone + "_" + 1);

                        IBS_UserOtherID telephone = new IBS_UserOtherID();
                        telephone.UserIDOther = userInfo.iBS_UserInfo.TelePhone;
                        telephone.UserID = ibsUser.UserID;
                        telephone.Type = 1;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", userInfo.iBS_UserInfo.TelePhone + "_" + 1, telephone);
                    }

                    if (ibsUser.UserType == (int)UserTypeEnum.Teacher)
                    {
                        if (string.IsNullOrEmpty(ibsUser.UserNum))
                        {
                            var invnum = relationservice.SelectOrAddUserInvNumByUserId(ibsUser.UserID.ToString());
                            if (!string.IsNullOrEmpty(invnum) && invnum.isNumberic1())
                            {
                                IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                                teachInvm.UserIDOther = invnum;
                                teachInvm.Type = 3;
                                teachInvm.UserID = ibsUser.UserID;
                                ibsUser.UserNum = invnum;
                                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", ibsUser.UserNum + "_" + 3, teachInvm);

                            }
                        }


                        ibsUser.ClassSchList.ForEach(a =>
                        {
                            var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);

                            if (classinfo != null)
                            {
                                var stu = classinfo.ClassTchList.FirstOrDefault(x => x.TchID == ibsUser.UserID);
                                if (stu != null)
                                {
                                    stu.TchName = ibsUser.TrueName;
                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                            }
                        });
                    }
                    else
                    {
                        ibsUser.ClassSchList.ForEach(a =>
                        {
                            var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                            if (classinfo != null)
                            {
                                var stu = classinfo.ClassStuList.FirstOrDefault(x => x.StuID == ibsUser.UserID);
                                if (stu != null)
                                {
                                    stu.StuName = ibsUser.TrueName;
                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                            }
                        });
                    }
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", userInfo.iBS_UserInfo.UserID.ToString(), ibsUser);
                }
        #endregion
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "IBS用户Update接口异常，UserInfo=" + JsonHelper.EncodeJson(userInfo));
                result.Success = false;
                result.ErrorMsg = "接口异常！";
            }
            return result;
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
        /// <summary>
        /// 数据复制
        /// </summary>
        /// <param name="baseUser">IBS用户信息</param>
        /// <param name="tbUser">TB用户信息</param>
        public void Base2TbSetValue(IBS_UserInfo baseUser, Tb_UserInfo tbUser)
        {
            tbUser.UserID = Convert.ToInt32(baseUser.UserID);
            if (baseUser.UserImage != null && baseUser.UserImage != "00000000-0000-0000-0000-000000000000")
            {
                tbUser.UserImage = baseUser.UserImage;
            }
            tbUser.TrueName = baseUser.TrueName ?? tbUser.TrueName;
            tbUser.UserRoles = baseUser.UserRoles > 0 ? baseUser.UserRoles : tbUser.UserRoles;
            tbUser.NickName = baseUser.TrueName ?? tbUser.TrueName;
            tbUser.IsUser = baseUser.IsUser;
            if (baseUser.Regdate != null)
            {
                tbUser.CreateTime = baseUser.Regdate;
            }
            tbUser.isLogState = string.IsNullOrEmpty(baseUser.isLogState) ? "0" : baseUser.isLogState;
            tbUser.IsEnableOss = baseUser.IsEnableOss;
        }

        #region TBX增删改查
        public Tb_UserInfo Search(Expression<Func<Tb_UserInfo, bool>> where)
        {
            return userDAL.SelectSearch(where).FirstOrDefault();
        }

        public bool Insert(Tb_UserInfo info)
        {
            var result = false;
            var re = userDAL.Insert(info);
            if (re != null)
            {
                result = true;
            }
            return result;
        }
        #endregion

        public List<Tb_UserInfo> GetUserList(Expression<Func<Tb_UserInfo, bool>> where, int takeCount, string orderby = "")
        {
            List<Tb_UserInfo> result = null;
            var re = userDAL.SelectSearch(where, takeCount, orderby);
            if (re != null)
            {
                result = re.ToList();
            }
            return result;

        }



        public List<VailUserInfo> GetVailUserInfoList(int type, int count)
        {
            string sql = "";
            if (type == 0)
            {
                sql =
                    string.Format(
                        @"select TOP {0} ui.UserID, isnull(Versions,'') as Versions,ui.CreateTime,CASE WHEN ui.AppId='0a94ceaf-8747-4266-bc05-ed8ae2e7e410' THEN 3
																			WHEN ui.AppId='1548d0a3-ca8e-4702-9c2c-f0ba0cacd385' THEN 24
																			WHEN ui.AppId='241ea176-fce7-4bd7-a65f-a7978aac1cd2' THEN 21
																			WHEN ui.AppId='37ca795d-42a6-4117-84f3-f4f856e03c62' THEN 39
																			WHEN ui.AppId='41efcd18-ad8c-4585-8b6c-e7b61f49914c' THEN 57
																			WHEN ui.AppId='43716a9b-7ade-4137-bdc4-6362c9e1c999' THEN 25
																			WHEN ui.AppId='5373bbc9-49d4-47df-b5b5-ae196dc23d6d' THEN 1
																			WHEN ui.AppId='64a8de22-cea0-4026-ab36-5a70f94dd6e4' THEN 10
																			WHEN ui.AppId='6f0de2c0-a12d-4d78-8927-edd01a100274' THEN 22
																			WHEN ui.AppId='8170b2bf-82a8-4c2d-9458-ae9d43cac5e3' THEN 27
																			WHEN ui.AppId='9426808e-da8e-488c-9827-b082c19b62a7' THEN 5
																			WHEN ui.AppId='f0a9e1a7-b4cf-4a37-8fd1-932a66070afa' THEN 30
																			ELSE 0 END AS VersionID,
                        isnull(DownloadChannel,0)as DownloadChannel, ISNULL( IsValidUser,0)as IsValidUser,ISNULL( ValidUserTime,'2000-01-01')as ValidUserTime 
                        from TB_UserInfo ui left join Tb_UserDetails ud on ui.UserID=ud.UserId", count);
            }
            else
            {
                sql =
                    string.Format(
                        @"select TOP {0} ui.UserID,'' as Versions,ui.CreateTime,CASE WHEN ui.AppId='0a94ceaf-8747-4266-bc05-ed8ae2e7e410' THEN 3
																			WHEN ui.AppId='1548d0a3-ca8e-4702-9c2c-f0ba0cacd385' THEN 24
																			WHEN ui.AppId='241ea176-fce7-4bd7-a65f-a7978aac1cd2' THEN 21
																			WHEN ui.AppId='37ca795d-42a6-4117-84f3-f4f856e03c62' THEN 39
																			WHEN ui.AppId='41efcd18-ad8c-4585-8b6c-e7b61f49914c' THEN 57
																			WHEN ui.AppId='43716a9b-7ade-4137-bdc4-6362c9e1c999' THEN 25
																			WHEN ui.AppId='5373bbc9-49d4-47df-b5b5-ae196dc23d6d' THEN 1
																			WHEN ui.AppId='64a8de22-cea0-4026-ab36-5a70f94dd6e4' THEN 10
																			WHEN ui.AppId='6f0de2c0-a12d-4d78-8927-edd01a100274' THEN 22
																			WHEN ui.AppId='8170b2bf-82a8-4c2d-9458-ae9d43cac5e3' THEN 27
																			WHEN ui.AppId='9426808e-da8e-488c-9827-b082c19b62a7' THEN 5
																			WHEN ui.AppId='f0a9e1a7-b4cf-4a37-8fd1-932a66070afa' THEN 30
																			ELSE 0 END AS VersionID,
                        0as DownloadChannel,0as IsValidUser,'2000-01-01'as ValidUserTime 
                        from TB_UserInfo ui  WHERE CONVERT(VARCHAR,ui.CreateTime,11)=CONVERT(VARCHAR,DATEADD(DAY,-1,GETDATE()),11)
                        UNION ALL
                        SELECT  ui.UserID, isnull(Versions,'') as Versions,ui.CreateTime,CASE WHEN ui.AppId='0a94ceaf-8747-4266-bc05-ed8ae2e7e410' THEN 3
																			WHEN ui.AppId='1548d0a3-ca8e-4702-9c2c-f0ba0cacd385' THEN 24
																			WHEN ui.AppId='241ea176-fce7-4bd7-a65f-a7978aac1cd2' THEN 21
																			WHEN ui.AppId='37ca795d-42a6-4117-84f3-f4f856e03c62' THEN 39
																			WHEN ui.AppId='41efcd18-ad8c-4585-8b6c-e7b61f49914c' THEN 57
																			WHEN ui.AppId='43716a9b-7ade-4137-bdc4-6362c9e1c999' THEN 25
																			WHEN ui.AppId='5373bbc9-49d4-47df-b5b5-ae196dc23d6d' THEN 1
																			WHEN ui.AppId='64a8de22-cea0-4026-ab36-5a70f94dd6e4' THEN 10
																			WHEN ui.AppId='6f0de2c0-a12d-4d78-8927-edd01a100274' THEN 22
																			WHEN ui.AppId='8170b2bf-82a8-4c2d-9458-ae9d43cac5e3' THEN 27
																			WHEN ui.AppId='9426808e-da8e-488c-9827-b082c19b62a7' THEN 5
																			WHEN ui.AppId='f0a9e1a7-b4cf-4a37-8fd1-932a66070afa' THEN 30
																			ELSE 0 END AS VersionID,
                        isnull(DownloadChannel,0)as DownloadChannel, ISNULL( IsValidUser,0)as IsValidUser,ISNULL( ValidUserTime,'2000-01-01')as ValidUserTime 
                        from TB_UserInfo ui left join Tb_UserDetails ud on ui.UserID=ud.UserId WHERE CONVERT(VARCHAR,ud.ValidUserTime,11)=CONVERT(VARCHAR,DATEADD(DAY,-1,GETDATE()),11)",count);

            }
            DataSet ds = _bmBaseDB.ExecuteSql(sql);
           List<VailUserInfo> list = Extension.Convert2Object<VailUserInfo>(ds.Tables[0]);
            if (list != null)
            {
                list.ForEach(a =>
                {
                    try
                    {
                        var user = GetUserInfoByUserId(a.UserID);
                        if (user != null)
                        {
                            var classinfo=user.ClassSchList.FirstOrDefault();
                            if (classinfo != null)
                            {
                                a.SchoolID = classinfo.SchID;
                                a.AreaID = classinfo.AreaID;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "GetCFUserInfoList报错！Data=" + a.ToJson());
                    }
                });
            }
            return list;

        }



        public List<VailUserInfo> GetVailUserInfoListByCondition(string startDate, string endDate)
        {
            string sql =
                string.Format(
                    @"select  ui.UserID,'' as Versions,ui.CreateTime,CASE WHEN ui.AppId='0a94ceaf-8747-4266-bc05-ed8ae2e7e410' THEN 3
																			WHEN ui.AppId='1548d0a3-ca8e-4702-9c2c-f0ba0cacd385' THEN 24
																			WHEN ui.AppId='241ea176-fce7-4bd7-a65f-a7978aac1cd2' THEN 21
																			WHEN ui.AppId='37ca795d-42a6-4117-84f3-f4f856e03c62' THEN 39
																			WHEN ui.AppId='41efcd18-ad8c-4585-8b6c-e7b61f49914c' THEN 57
																			WHEN ui.AppId='43716a9b-7ade-4137-bdc4-6362c9e1c999' THEN 25
																			WHEN ui.AppId='5373bbc9-49d4-47df-b5b5-ae196dc23d6d' THEN 1
																			WHEN ui.AppId='64a8de22-cea0-4026-ab36-5a70f94dd6e4' THEN 10
																			WHEN ui.AppId='6f0de2c0-a12d-4d78-8927-edd01a100274' THEN 22
																			WHEN ui.AppId='8170b2bf-82a8-4c2d-9458-ae9d43cac5e3' THEN 27
																			WHEN ui.AppId='9426808e-da8e-488c-9827-b082c19b62a7' THEN 5
																			WHEN ui.AppId='f0a9e1a7-b4cf-4a37-8fd1-932a66070afa' THEN 30
																			ELSE 0 END AS VersionID,
                        ui.TelePhone
                        from TB_UserInfo ui  WHERE ui.CreateTime>='{0}' and ui.CreateTime<'{1}' order by ui.CreateTime"
                        , startDate, endDate);
            DataSet ds = _bmBaseDB.ExecuteSql(sql);
            List<VailUserInfo> list = Extension.Convert2Object<VailUserInfo>(ds.Tables[0]);
            if (list != null)
            {
                list.ForEach(a =>
                {
                    try
                    {
                        var user = GetUserInfoByUserId(a.UserID);
                        if (user != null)
                        {
                            var classinfo = user.ClassSchList.FirstOrDefault();
                            if (classinfo != null)
                            {
                                a.SchoolID = classinfo.SchID;
                                a.AreaID = classinfo.AreaID;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "GetCFUserInfoList报错！Data=" + a.ToJson());
                    }
                });
            }
            return list;
        }

        public List<VailUserInfo> GetVailUserInfoListByConditionTBX(string startDate, string endDate)
        {
            string sql =
                string.Format(
                    @"select  ui.UserID,'' as Versions,ui.CreateTime,CASE WHEN ui.AppId='0a94ceaf-8747-4266-bc05-ed8ae2e7e410' THEN 3
																			WHEN ui.AppId='1548d0a3-ca8e-4702-9c2c-f0ba0cacd385' THEN 24
																			WHEN ui.AppId='241ea176-fce7-4bd7-a65f-a7978aac1cd2' THEN 21
																			WHEN ui.AppId='37ca795d-42a6-4117-84f3-f4f856e03c62' THEN 39
																			WHEN ui.AppId='41efcd18-ad8c-4585-8b6c-e7b61f49914c' THEN 57
																			WHEN ui.AppId='43716a9b-7ade-4137-bdc4-6362c9e1c999' THEN 25
																			WHEN ui.AppId='5373bbc9-49d4-47df-b5b5-ae196dc23d6d' THEN 1
																			WHEN ui.AppId='64a8de22-cea0-4026-ab36-5a70f94dd6e4' THEN 10
																			WHEN ui.AppId='6f0de2c0-a12d-4d78-8927-edd01a100274' THEN 22
																			WHEN ui.AppId='8170b2bf-82a8-4c2d-9458-ae9d43cac5e3' THEN 27
																			WHEN ui.AppId='9426808e-da8e-488c-9827-b082c19b62a7' THEN 5
																			WHEN ui.AppId='f0a9e1a7-b4cf-4a37-8fd1-932a66070afa' THEN 30
																			ELSE 0 END AS VersionID,
                        ui.TelePhone
                        from TB_UserInfo ui  WHERE ui.CreateTime>='{0}' and ui.CreateTime<'{1}' and ui.appID is not null order by ui.CreateTime"
                        , startDate, endDate);
            DataSet ds = _bmBaseDB.ExecuteSql(sql);
            List<VailUserInfo> list = Extension.Convert2Object<VailUserInfo>(ds.Tables[0]);
            if (list != null)
            {
                list.ForEach(a =>
                {
                    try
                    {
                        var user = GetUserInfoByUserId(a.UserID);
                        if (user != null)
                        {
                            var classinfo = user.ClassSchList.FirstOrDefault();
                            if (classinfo != null)
                            {
                                a.SchoolID = classinfo.SchID;
                                a.AreaID = classinfo.AreaID;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "GetCFUserInfoList报错！Data=" + a.ToJson());
                    }
                });
            }
            return list;
        }

    }
}
