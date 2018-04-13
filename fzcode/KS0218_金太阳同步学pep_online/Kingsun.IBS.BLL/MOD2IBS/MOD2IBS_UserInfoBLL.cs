
using Kingsun.IBS.BLL.FZUUMS_Relation2;
using Kingsun.IBS.DAL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.IBLL.IBS2MOD;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL.IBS2MOD
{
    public class MOD2IBS_UserInfoBLL : IIBS_MOD_UserInfoBLL
    {
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
        //消息队列
        static RedisListOtherHelper listRedis = new RedisListOtherHelper();

        RelationService.RelationService relationservice = new RelationService.RelationService();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();

        IBSUserDAL userDAL = new IBSUserDAL();
        /// IBSUserDALsummary>
        /// 新增用户接口
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool Add(IBS_UserInfo UserInfo)
        {
            try
            {
                var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString());

                if (user != null)
                {
                    hashRedis.Remove("IBS_UserOtherID", user.UserName+"_"+2);
                    hashRedis.Remove("IBS_UserOtherID", user.UserNum+"_"+3);
                    hashRedis.Remove("IBS_UserOtherID", user.TelePhone+"_"+1);

                    //若用户身份变更需要做班级变更
                    if (user.UserType != UserInfo.UserType) 
                    {
                        user.ClassSchList = new List<ClassSch>();
                    }
                }
                else
                {
                    user = new IBS_UserInfo();
                }
                user.UserID = UserInfo.UserID;
                if(user.TrueName!=UserInfo.TrueName)
                {
                   
                    user.ClassSchList.ForEach(x =>
                    {
                        var classinfo = classBLL.GetClassUserRelationByClassId(x.ClassID);
                        if (classinfo != null)
                        {
                            if (user.UserType == (int) UserTypeEnum.Teacher)
                            {
                                var tch = classinfo.ClassTchList.FirstOrDefault(y => y.TchID == user.UserID);
                                if (tch != null)
                                {
                                    tch.TchName = UserInfo.TrueName;
                                }
                            }
                            else
                            {
                                var tch = classinfo.ClassStuList.FirstOrDefault(y => y.StuID == user.UserID);
                                if (tch != null)
                                {
                                    tch.StuName = UserInfo.TrueName;
                                }
                            }
                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", x.ClassID.ToUpper(), classinfo);
                        }

                    });
                }
                user.TrueName = UserInfo.TrueName;

                user.UserType = UserInfo.UserType;
                user.UserPwd = UserInfo.UserPwd;
                user.UserNum = UserInfo.UserNum;
                user.UserName = UserInfo.UserName;
                user.TelePhone = UserInfo.TelePhone;
                user.Regdate = UserInfo.Regdate;
                if (UserInfo.SchoolID != null)
                {
                    user.SchoolID = UserInfo.SchoolID;
                    user.SchoolName = UserInfo.SchoolName;
                }
               
               

                //DB操作
                var tbUser = userDAL.SelectSearch(a => a.UserID == UserInfo.UserID).FirstOrDefault();

                if (tbUser != null)
                {
                    tbUser.UserID = Convert.ToInt32(user.UserID);
                    tbUser.UserName = user.UserName;
                    tbUser.TrueName = user.TrueName;
                    tbUser.UserImage = user.UserImage;
                    tbUser.UserRoles = user.UserRoles;
                    tbUser.NickName = user.TrueName;
                    tbUser.TelePhone = user.TelePhone;
                    tbUser.CreateTime = UserInfo.Regdate;
                    userDAL.Update(tbUser);
                }
                else
                {
                    //DB操作
                    tbUser = new Tb_UserInfo();
                    tbUser.UserID = Convert.ToInt32(user.UserID);
                    tbUser.UserName = user.UserName;
                    tbUser.TrueName = user.TrueName;
                    tbUser.UserRoles = user.UserRoles;
                    tbUser.NickName = user.TrueName;
                    tbUser.TelePhone = user.TelePhone;
                    tbUser.UserImage = string.IsNullOrEmpty(user.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.UserImage;
                    tbUser.isLogState = user.isLogState ?? "0";
                    tbUser.IsEnableOss =user.IsEnableOss;
                    tbUser.IsUser = 1;
                    tbUser.CreateTime = UserInfo.Regdate;
                    userDAL.Insert(tbUser);
                }
                user.UserImage = tbUser.UserImage;
                if (tbUser.IsEnableOss != null) user.IsEnableOss = (int)tbUser.IsEnableOss;
                user.isLogState = string.IsNullOrEmpty(tbUser.isLogState) ? "0" : tbUser.isLogState;
                user.IsUser = tbUser.IsUser ?? 1;
                user.AppID = tbUser.AppId;
                
                //新增IBS_UserOtherID
                if (!string.IsNullOrEmpty(UserInfo.TelePhone))
                {
                    //手机号
                    IBS_UserOtherID telephone = new IBS_UserOtherID();
                    telephone.UserIDOther = UserInfo.TelePhone;
                    telephone.Type = 1;
                    telephone.UserID = UserInfo.UserID;
                    hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserInfo.TelePhone+"_"+1, telephone);
                }
                if (!string.IsNullOrEmpty(UserInfo.UserName))
                {
                    //账号
                    IBS_UserOtherID userName = new IBS_UserOtherID();
                    userName.UserIDOther = UserInfo.UserName;
                    userName.Type = 2;
                    userName.UserID = UserInfo.UserID;
                    hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserInfo.UserName+"_"+2, userName);

                }


                if (UserInfo.UserType == (int)UserTypeEnum.Teacher)
                {
                    if (!string.IsNullOrEmpty(UserInfo.UserNum))
                    {
                        //账号
                        IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                        teachInvm.UserIDOther = UserInfo.UserNum;
                        teachInvm.Type = 3;
                        teachInvm.UserID = UserInfo.UserID;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserInfo.UserNum + "_" + 3, teachInvm);
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
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString(), user);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "新增用户接口异常,UserInfo=" + UserInfo.ToJson());
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 4;
                excep.ChangeType = 2;
                excep.Json = UserInfo.ToJson();
                excep.ErrorMessage = "新增用户接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());
                return false;
            }

        }
        public bool Update(IBS_UserInfo UserInfo)
        {
            var re = false;
            try
            {
                var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString());
                if (user != null)
                {
                    hashRedis.Remove("IBS_UserOtherID", user.UserName+"_"+2);
                    hashRedis.Remove("IBS_UserOtherID", user.UserNum+"_"+3);
                    hashRedis.Remove("IBS_UserOtherID", user.TelePhone+"_"+1);

                    //若用户身份变更需要做班级变更
                    if (user.UserType != UserInfo.UserType)
                    {
                        user.ClassSchList = new List<ClassSch>();
                    }

                }
                else 
                {
                    user = new IBS_UserInfo();
                }


                user.UserID = UserInfo.UserID;
                if (user.TrueName != UserInfo.TrueName)
                {

                    user.ClassSchList.ForEach(x =>
                    {
                        var classinfo = classBLL.GetClassUserRelationByClassId(x.ClassID);
                        if (classinfo != null)
                        {
                            if (user.UserType == (int)UserTypeEnum.Teacher)
                            {
                                var tch = classinfo.ClassTchList.FirstOrDefault(y => y.TchID == user.UserID);
                                if (tch != null)
                                {
                                    tch.TchName = UserInfo.TrueName;
                                }
                            }
                            else
                            {
                                var tch = classinfo.ClassStuList.FirstOrDefault(y => y.StuID == user.UserID);
                                if (tch != null)
                                {
                                    tch.StuName = UserInfo.TrueName;
                                }
                            }
                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", x.ClassID.ToUpper(), classinfo);
                        }

                    });
                }
                user.TrueName = UserInfo.TrueName;
                user.UserType = UserInfo.UserType;
                user.UserPwd = UserInfo.UserPwd;
                user.UserNum = UserInfo.UserNum;
                user.UserName = UserInfo.UserName;
                user.TelePhone = UserInfo.TelePhone;
                user.Regdate = UserInfo.Regdate;
                user.State = UserInfo.State;
                if (UserInfo.SchoolID != null)
                {
                    user.SchoolID = UserInfo.SchoolID;
                    user.SchoolName = UserInfo.SchoolName;
                }
               

              

                //DB操作
                var tbUser = userDAL.SelectSearch(a => a.UserID == UserInfo.UserID).FirstOrDefault();

                if (tbUser != null)
                {
                    tbUser.UserID = Convert.ToInt32(user.UserID);
                    tbUser.UserName = user.UserName;
                    tbUser.TrueName = user.TrueName;
                    tbUser.UserRoles = user.UserRoles;
                    tbUser.NickName = user.TrueName;
                    tbUser.TelePhone = user.TelePhone;
                    tbUser.CreateTime = UserInfo.Regdate;
                    userDAL.Update(tbUser);
                }
                else 
                {
                    //DB操作
                    tbUser = new Tb_UserInfo();
                    tbUser.UserID = Convert.ToInt32(user.UserID);
                    tbUser.UserName = user.UserName;
                    tbUser.TrueName = user.TrueName;
                    tbUser.UserRoles = user.UserRoles;
                    tbUser.NickName = user.TrueName;
                    tbUser.TelePhone = user.TelePhone;
                    tbUser.UserImage = string.IsNullOrEmpty(user.UserImage)? "00000000-0000-0000-0000-000000000000" : user.UserImage;
                    tbUser.CreateTime = UserInfo.Regdate;
                    tbUser.isLogState = "0";
                    tbUser.IsEnableOss = 0;
                    tbUser.IsUser = 1;
                    userDAL.Insert(tbUser);
                    
                }
                user.UserImage = tbUser.UserImage;
                user.IsEnableOss = tbUser.IsEnableOss??0;
                user.isLogState = string.IsNullOrEmpty(tbUser.isLogState) ? "0" : tbUser.isLogState;
                user.IsUser =tbUser.IsUser ?? 1;
                user.AppID = tbUser.AppId;

                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString(), user);
                

                //新增IBS_UserOtherID
                if (!string.IsNullOrEmpty(UserInfo.TelePhone))
                {
                    //手机号
                    IBS_UserOtherID telephone = new IBS_UserOtherID();
                    telephone.UserIDOther = UserInfo.TelePhone;
                    telephone.Type = 1;
                    telephone.UserID = UserInfo.UserID;
                    hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserInfo.TelePhone+"_"+1, telephone);
                }
                if (!string.IsNullOrEmpty(UserInfo.UserName))
                {
                    //账号
                    IBS_UserOtherID userName = new IBS_UserOtherID();
                    userName.UserIDOther = UserInfo.UserName;
                    userName.Type = 2;
                    userName.UserID = UserInfo.UserID;
                    hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserInfo.UserName+"_"+2, userName);

                }


                if (UserInfo.UserType == (int)UserTypeEnum.Teacher)
                {
                    if (!string.IsNullOrEmpty(UserInfo.UserNum))
                    {
                        //账号
                        IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                        teachInvm.UserIDOther = UserInfo.UserNum;
                        teachInvm.Type = 3;
                        teachInvm.UserID = UserInfo.UserID;
                        hashRedis.Set<IBS_UserOtherID>("IBS_UserOtherID", UserInfo.UserNum + "_" + 3, teachInvm);
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
                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString(), user);
                }
                re = true;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "Update接口异常，UserInfo=" + UserInfo.ToJson());
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 4;
                excep.ChangeType = 1;
                excep.Json = UserInfo.ToJson();
                excep.ErrorMessage = "Update接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());
                re=false;
            }
            
            return re;
        }

        public bool Delete(IBS_UserInfo UserInfo)
        {
            var re = false;
            try
            {
                var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", UserInfo.UserID.ToString());
                if (user != null) 
                {
                    //删除IBS_UserOtherID数据
                    hashRedis.Remove("IBS_UserOtherID", user.UserName + "_" + 2);
                    hashRedis.Remove("IBS_UserOtherID", user.UserNum + "_" + 3);
                    hashRedis.Remove("IBS_UserOtherID", user.TelePhone + "_" + 1);

                    //移除IBS_ClassUserRelation数据
                    user.ClassSchList.ForEach(a =>
                    {
                        var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper());
                        if (classinfo != null)
                        {
                            if (user.UserType == (int)UserTypeEnum.Teacher)
                            {
                                classinfo.ClassTchList.RemoveAll(x => x.TchID == user.UserID);

                            }
                            else if (user.UserType == (int)UserTypeEnum.Student)
                            {
                                classinfo.ClassStuList.RemoveAll(x => x.StuID == user.UserID);
                            }
                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), classinfo);
                        }

                    });

                    //删除IBS_UserInfo数据
                    hashRedis.Remove("IBS_UserInfo", UserInfo.UserID.ToString());
                }
                //DB操作
                var tbUser = userDAL.SelectSearch(a => a.UserID == UserInfo.UserID).FirstOrDefault();
                if (tbUser != null)
                {
                    re = userDAL.Delete(tbUser.ID);
                }
                
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "Del接口异常,UserInfo=" + UserInfo.ToJson());
                //新增到异常处理消息队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.DataType = 4;
                excep.ChangeType = 3;
                excep.Json = UserInfo.ToJson();
                excep.ErrorMessage = "Del接口异常！" + ex.Message;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());
                re= false;
            }
            return re;
        }

    }
}
