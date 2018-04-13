using CBSS.Framework.Contract;
using CBSS.Framework.Redis;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract;
using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Core.Log;

namespace CBSS.IBS.BLL
{
    public class MOD2IBS_UserClassRelationBLL :CommonBLL, IIBS_MOD_UserClassRelationBLL
    {
        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper("IBS");
        static RedisHashHelper hashRedis = new RedisHashHelper("IBS");

        //消息队列
        static RedisListHelper listRedis = new RedisListHelper("IBS");

        RelationService.RelationService relationservice = new RelationService.RelationService();
        FZUUMS_Relation2.FZUUMS_Relation2SoapClient relation2Client = new FZUUMS_Relation2.FZUUMS_Relation2SoapClient();
        FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();



        /// <summary>
        /// 数据变动
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ClassUserRemove(IBS_UserInfo UserInfo)
        {
            bool result = false;
            ///
            try
            {
                var oldUser = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString());
                if (oldUser != null)
                {

                    UserInfo.ClassSchList.ForEach(a =>
                    {
                        //先移除关联班级与用户的关系
                        var ClassInfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper());
                        if (ClassInfo != null)
                        {
                            if (oldUser.UserType == (int)UserTypeEnum.Teacher)
                            {
                                ClassInfo.ClassTchList.RemoveAll(ax => ax.TchID == oldUser.UserID);

                            }
                            else if (oldUser.UserType == (int)UserTypeEnum.Student)
                            {
                                ClassInfo.ClassStuList.RemoveAll(ax => ax.StuID == oldUser.UserID);

                            }
                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), ClassInfo);
                        }
                        else
                        {
                            ClassInfo = BuildClassInfoByClassId(a.ClassID);
                            if (ClassInfo != null)
                            {
                                //新增IBS_ClassOtherID数据
                                if (!string.IsNullOrEmpty(ClassInfo.ClassNum))
                                {
                                    IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                    classnum.ClassIDOther = ClassInfo.ClassNum.ToString();
                                    classnum.ClassID = ClassInfo.ClassID.ToUpper();
                                    classnum.Type = 1;
                                    hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", ClassInfo.ClassNum.ToString() + "_" + 1, classnum);
                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), ClassInfo);
                            }
                        }
                        oldUser.ClassSchList.RemoveAll(ax => ax.ClassID == a.ClassID);

                    }
                        );
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString(), oldUser);
                    result = true;
                }
                else
                {
                    oldUser = BuildUserInfoByUserId(Convert.ToInt32(UserInfo.UserID));
                    if (oldUser != null)
                    {
                        //新增IBS_UserOtherID
                        if (!string.IsNullOrEmpty(oldUser.TelePhone))
                        {
                            //手机号
                            IBS_UserOtherID telephone = new IBS_UserOtherID();
                            telephone.UserIDOther = oldUser.TelePhone;
                            telephone.Type = 1;
                            telephone.UserID = oldUser.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", oldUser.TelePhone + "_" + 1, telephone);
                        }
                        if (!string.IsNullOrEmpty(oldUser.UserName))
                        {
                            //账号
                            IBS_UserOtherID userName = new IBS_UserOtherID();
                            userName.UserIDOther = oldUser.UserName;
                            userName.Type = 2;
                            userName.UserID = oldUser.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", oldUser.UserName + "_" + 2, userName);

                        }
                        if (oldUser.UserType == (int)UserTypeEnum.Teacher)
                        {
                            if (!string.IsNullOrEmpty(oldUser.UserNum.ToString()))
                            {
                                //账号
                                IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                                teachInvm.UserIDOther = oldUser.UserNum.ToString();
                                teachInvm.Type = 3;
                                teachInvm.UserID = oldUser.UserID;
                                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", oldUser.UserNum + "_" + 3, teachInvm);
                            }
                            else
                            {
                                var invnum = relationservice.SelectOrAddUserInvNumByUserId(oldUser.UserID.ToString());
                                if (!string.IsNullOrEmpty(invnum))
                                {
                                    IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                                    teachInvm.UserIDOther = invnum;
                                    teachInvm.Type = 3;
                                    teachInvm.UserID = oldUser.UserID;
                                    oldUser.UserNum = invnum;
                                    hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", oldUser.UserNum + "_" + 3, teachInvm);

                                }
                            }
                        }

                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", oldUser.UserID.ToString(), oldUser);

                        UserInfo.ClassSchList.ForEach(a =>
                        {
                            //先移除关联班级与用户的关系
                            var ClassInfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper());
                            if (ClassInfo != null)
                            {
                                if (oldUser.UserType == (int)UserTypeEnum.Teacher)
                                {
                                    ClassInfo.ClassTchList.RemoveAll(ax => ax.TchID == oldUser.UserID);
                                }
                                else if (oldUser.UserType == (int)UserTypeEnum.Student)
                                {

                                    ClassInfo.ClassStuList.RemoveAll(ax => ax.StuID == oldUser.UserID);

                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), ClassInfo);
                            }
                            else
                            {
                                ClassInfo = BuildClassInfoByClassId(a.ClassID.ToUpper());
                                if (ClassInfo != null)
                                {
                                    //新增IBS_ClassOtherID数据
                                    if (!string.IsNullOrEmpty(ClassInfo.ClassNum))
                                    {
                                        IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                        classnum.ClassIDOther = ClassInfo.ClassNum.ToString();
                                        classnum.ClassID = ClassInfo.ClassID.ToUpper();
                                        classnum.Type = 1;
                                        hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", ClassInfo.ClassNum.ToString() + "_" + 1, classnum);
                                    }
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), ClassInfo);
                                }
                            }
                        }
                       );

                    }
                    result = true;
                }
            }
            catch (Exception ex) 
            {

                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "ClassUserRemove接口异常，UserInfo=" + UserInfo.ToJson(),ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 7;
                excep.ChangeType = 3;
                excep.Json = UserInfo.ToJson();
                excep.ErrorMessage = "ClassUserRemove接口异常!" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());
                result=false;
            }
           

            return result;
        }
        /// <summary>
        /// 数据变动
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ClassUserAdd(IBS_UserInfo UserInfo)
        {
            bool result = false;
            ///
            try
            {
                var newUser = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString());
                if (newUser != null)
                {
                    //新数据新增newUser数据
                    UserInfo.ClassSchList.ForEach(a =>
                    {
                        var newClassInfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper());
                        if (newClassInfo != null)
                        {

                            if (newUser.UserType == (int)UserTypeEnum.Teacher)
                            {
                                var isExist = newClassInfo.ClassTchList.FirstOrDefault(x => x.TchID == UserInfo.UserID);
                                if (isExist == null)
                                {
                                    ClassTchS tch = new ClassTchS();
                                    tch.TchID = newUser.UserID;
                                    tch.TchName = newUser.TrueName;
                                    tch.SubjectID = a.SubjectID == 0 ? 3 : a.SubjectID;
                                    tch.SubjectName = EnumHelper.GetEnumDesc<OldSubjectTypeEnum>(Convert.ToInt32(a.SubjectID));
                                    tch.UserImage = newUser.UserImage;
                                    tch.IsEnableOss = newUser.IsEnableOss;
                                    newClassInfo.ClassTchList.Add(tch);
                                }

                            }
                            else
                            {
                                var isExist = newClassInfo.ClassStuList.FirstOrDefault(x => x.StuID == UserInfo.UserID);
                                if (isExist == null)
                                {
                                    ClassStuS stu = new ClassStuS();
                                    stu.StuID = newUser.UserID;
                                    stu.StuName = newUser.TrueName;
                                    stu.IsEnableOss = newUser.IsEnableOss;
                                    stu.UserImage = newUser.UserImage;
                                    newClassInfo.ClassStuList.Add(stu);
                                }
                            }

                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), newClassInfo);
                        }
                        else
                        {
                            newClassInfo = BuildClassInfoByClassId(a.ClassID.ToUpper());
                            if (newClassInfo != null)
                            {
                                //新增IBS_ClassOtherID数据
                                if (!string.IsNullOrEmpty(newClassInfo.ClassNum))
                                {
                                    IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                    classnum.ClassIDOther = newClassInfo.ClassNum.ToString();
                                    classnum.ClassID = newClassInfo.ClassID.ToUpper();
                                    classnum.Type = 1;
                                    hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", newClassInfo.ClassNum.ToString() + "_" + 1, classnum);
                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), newClassInfo);
                            }
                        }
                        var isExistUser = newUser.ClassSchList.FirstOrDefault(x => x.ClassID.ToUpper() == a.ClassID.ToUpper());
                        if (isExistUser == null)
                        {
                            newUser.ClassSchList.Add(a);
                        }
                    });

                    //将老数据用新数据替换
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString(), newUser);
                }
                else
                {
                    newUser = BuildUserInfoByUserId(Convert.ToInt32(UserInfo.UserID));
                    //新增IBS_UserOtherID
                    if (!string.IsNullOrEmpty(newUser.TelePhone))
                    {
                        //手机号
                        IBS_UserOtherID telephone = new IBS_UserOtherID();
                        telephone.UserIDOther = newUser.TelePhone;
                        telephone.Type = 1;
                        telephone.UserID = newUser.UserID;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", newUser.TelePhone + "_" + 1, telephone);
                    }
                    if (!string.IsNullOrEmpty(newUser.UserName))
                    {
                        //账号
                        IBS_UserOtherID userName = new IBS_UserOtherID();
                        userName.UserIDOther = newUser.UserName;
                        userName.Type = 2;
                        userName.UserID = newUser.UserID;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", newUser.UserName + "_" + 2, userName);

                    }


                    if (newUser.UserType == (int)UserTypeEnum.Teacher)
                    {
                        if (!string.IsNullOrEmpty(newUser.UserNum))
                        {
                            //账号
                            IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                            teachInvm.UserIDOther = newUser.UserNum.ToString();
                            teachInvm.Type = 3;
                            teachInvm.UserID = newUser.UserID;
                            hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", newUser.UserNum + "_" + 3, teachInvm);
                        }
                        else
                        {
                            var invnum = relationservice.SelectOrAddUserInvNumByUserId(newUser.UserID.ToString());
                            if (!string.IsNullOrEmpty(invnum))
                            {
                                IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                                teachInvm.UserIDOther = invnum;
                                teachInvm.Type = 3;
                                teachInvm.UserID = newUser.UserID;
                                newUser.UserNum = invnum;
                                hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", newUser.UserNum + "_" + 3, teachInvm);

                            }
                        }
                    }

                    if (newUser != null)
                    {
                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString(), newUser);
                        //新数据新增newUser数据
                        UserInfo.ClassSchList.ForEach(a =>
                        {
                            var newClassInfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper());
                            if (newClassInfo != null)
                            {

                                if (newUser.UserType == (int)UserTypeEnum.Teacher)
                                {
                                    var isExist = newClassInfo.ClassTchList.FirstOrDefault(x => x.TchID == UserInfo.UserID);
                                    if (isExist == null)
                                    {
                                        ClassTchS tch = new ClassTchS();
                                        tch.TchID = newUser.UserID;
                                        tch.TchName = newUser.TrueName;
                                        tch.SubjectName = "";
                                        tch.SubjectID = a.SubjectID;
                                        tch.UserImage = newUser.UserImage;
                                        tch.IsEnableOss = newUser.IsEnableOss;
                                        newClassInfo.ClassTchList.Add(tch);
                                    }

                                }
                                else
                                {
                                    var isExist = newClassInfo.ClassStuList.FirstOrDefault(x => x.StuID == UserInfo.UserID);
                                    if (isExist == null)
                                    {
                                        ClassStuS stu = new ClassStuS();
                                        stu.StuID = newUser.UserID;
                                        stu.StuName = newUser.TrueName;
                                        stu.IsEnableOss = newUser.IsEnableOss;
                                        stu.UserImage = newUser.UserImage;
                                        newClassInfo.ClassStuList.Add(stu);
                                    }
                                }

                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), newClassInfo);
                            }
                            else
                            {
                                newClassInfo = BuildClassInfoByClassId(a.ClassID.ToUpper());
                                if (newClassInfo != null)
                                {
                                    //新增IBS_ClassOtherID数据
                                    if (!string.IsNullOrEmpty(newClassInfo.ClassNum))
                                    {
                                        IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                        classnum.ClassIDOther = newClassInfo.ClassNum.ToString();
                                        classnum.ClassID = newClassInfo.ClassID.ToUpper();
                                        classnum.Type = 1;
                                        hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", newClassInfo.ClassNum.ToString() + "_" + 1, classnum);
                                    }
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), newClassInfo);
                                }
                            }
                        });
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "ClassUserAdd接口异常，UserInfo=" + UserInfo.ToJson(), ex);
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 7;
                excep.ChangeType = 2;
                excep.Json = UserInfo.ToJson();
                excep.ErrorMessage = "ClassUserAdd接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());
                result = false;
            }


            return result;
        }
    }
}
