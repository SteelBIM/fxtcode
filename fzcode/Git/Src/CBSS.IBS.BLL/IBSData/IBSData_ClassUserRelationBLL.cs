using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.IBS.Contract;
using CBSS.Framework.Redis;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract;
using CBSS.Core.Log;
using CBSS.IBS.BLL.RelationService;
using CBSS.IBS.IBLL;

namespace CBSS.IBS.BLL
{
    public partial class IBSService : CommonBLL,IIBSService
    {

        /// <summary>
        /// 通过ID获取班级信息
        /// </summary>
        /// <param name="ClassId"></param>
        /// <returns></returns>
        public IBS_ClassUserRelation GetClassUserRelationByClassId(string ClassId)
        {
            var classUser = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassId);
            if (classUser == null)
            {
                classUser = BuildClassInfoByClassId(ClassId);
                if (classUser != null)
                { 
                    //新增IBS_ClassOtherID数据
                    if (!string.IsNullOrEmpty(classUser.ClassNum))
                    {
                        IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                        classnum.ClassIDOther = classUser.ClassNum.ToString();
                        classnum.ClassID = classUser.ClassID.ToUpper();
                        classnum.Type = 1;
                        hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classUser.ClassNum.ToString() + "_" + 1, classnum);
                    }
                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassId.ToUpper(), classUser);
                }
                
            }
            return classUser;
        }


        /// <summary>
        /// 获取班级用户关系+学校名称
        /// </summary>
        /// <param name="ClassId">ClassId</param>
        /// <returns></returns>
        public TBX_ClassUserRelation GetClassUserRelationALLInfoByClassId(string ClassId)
        {
            TBX_ClassUserRelation result = new TBX_ClassUserRelation();
            var classUser = GetClassUserRelationByClassId(ClassId);
            result.iBS_ClassUserRelation = classUser;
            if (classUser.SchID > 0)
            {
                var modSchInfo = metadataService.GetSchoolInfo(classUser.SchID);
                if (!string.IsNullOrEmpty(modSchInfo) && modSchInfo.Split('|')[0] != "错误")
                {
                    MOD_SchoolInfoModel schoolInfo = modSchInfo.FromJson<MOD_SchoolInfoModel>();
                    result.SchName = schoolInfo.SchoolName;
                }
            }
            return result;
        }

        

       /// <summary>
       /// 通过其他获取班级信息
       /// </summary>
       /// <param name="ClassOtherId"></param>
        /// <param name="Type">查找类型（ClassID=0,ClassNum=1）</param>
       /// <returns></returns>
        public IBS_ClassUserRelation GetClassUserRelationByClassOtherId(string ClassOtherId, int Type)
        {
            IBS_ClassUserRelation classUser=null;
            var classOther = hashRedis.Get<IBS_ClassOtherID>("IBS_ClassOtherID", ClassOtherId + "_" + Type);
            if (classOther != null)
            {
                classUser = GetClassUserRelationByClassId(classOther.ClassID);
            }
            else
            {
                classOther = new IBS_ClassOtherID();
                switch (Type)
                {
                    case 1://ClassNum
                        var classByClassNum = relation2Client.GetClassInfoByNum(ClassOtherId);
                        if (classByClassNum != null) 
                        {
                            classOther.ClassID = classByClassNum.ID.ToString();
                            classOther.ClassIDOther = ClassOtherId;
                            classOther.Type = 1;
                            hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", ClassOtherId + "_" + Type, classOther);
                            classUser = GetClassUserRelationByClassId(classOther.ClassID);
                        }
                        break;
                }

            }
            return classUser;
        }

        public APIResponse GetUserClassInfoList(string userIds) 
        {
            APIResponse result = new APIResponse();
            var ri =relation2Client.GetUserClassInfoList(userIds);
            result.Success = ri.Success;
            result.ErrorMsg = ri.ErrorMsg;
            result.Data = ri.Data;
            return result;
        }

        #region 增删改
        /// <summary>
        /// 创建班级
        /// </summary>
        /// <param name="classInfo"></param>
        /// <param name="OperatorUserId"></param>
        /// <returns></returns>
        
        public string Add(IBS_ClassUserRelation classInfo,int OperatorUserId)
        {
            var result = "";
            //从MOD新接口添加数据
            var returnInfo = relationservice.AddClassInCPoint(classInfo.SchID.ToString(), classInfo.ClassName, OperatorUserId.ToString());
            if (returnInfo.Success)
            {
                var classid = "";
                if (returnInfo.Data.ToString().Split('|')[0].IsGUID())
                {
                    classid = returnInfo.Data.ToString().Split('|')[0];
                }
                else if (returnInfo.Data.ToString().Split('|')[1].IsGUID())
                {
                    classid = returnInfo.Data.ToString().Split('|')[1];
                }
                var clsssinfo = GetClassUserRelationByClassId(classid.ToUpper());

                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", classid.ToUpper(), clsssinfo);
                result = classid;
            }
            return result;
        }

        public bool Update(IBS_ClassUserRelation classInfo)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string classId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 绑定关系（学生或者老师）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIResponse AddUserToClass(UserClassData data)
        {
            APIResponse result = new APIResponse();
            try
            {
                if (data.UserType == UserTypeEnum.Teacher)
                {
                    var returnInfo = relation2Client.BindClassTea(Guid.Parse(data.ClassID.ToUpper()), data.UserID.ToString(), data.SchoolId, Convert.ToInt32(data.SubjectID));
                    if (returnInfo)
                    {
                        //修改用户信息表的关联list
                        var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString());
                        if (user != null)
                        {
                            ClassSch cs = new ClassSch();
                            cs.SchID = data.SchoolId;
                            cs.ClassID = data.ClassID.ToUpper();
                            cs.SubjectID = Convert.ToInt32(data.SubjectID);
                            var re = GetClassUserRelationByClassId(data.ClassID.ToUpper());
                            if (re != null)
                            {
                                cs.GradeID = re.GradeID;
                            }
                            if (data.SchoolId > 0)
                            {
                                var res = GetSchClassRelationBySchlId(data.SchoolId);
                                if (res != null)
                                {
                                    cs.AreaID = res.AreaID;
                                }
                            }
                            var isExists = user.ClassSchList.FirstOrDefault(x => x.ClassID.ToUpper() == cs.ClassID.ToUpper());
                            if (isExists == null)
                            {
                                user.ClassSchList.Add(cs);
                            }
                        }
                        else
                        {
                            user = BuildUserInfoByUserId(data.UserID);
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
                                    if (!string.IsNullOrEmpty(invnum))
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
                        }

                        hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);


                        //修改班级教师list
                        var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper());
                        if (classinfo != null)
                        {
                            ClassTchS ts = new ClassTchS();
                            RelationService.tb_UserClass[] teaInfo = relationservice.GetUserClassByUserId(data.UserID.ToString());
                            if (teaInfo != null)
                            {
                                List<RelationService.tb_UserClass> userclass = new List<RelationService.tb_UserClass>(teaInfo);
                                var tea = userclass.FirstOrDefault(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                                if (tea != null)
                                {
                                    if (tea.SubjectID != null && tea.SubjectID > 0)
                                    {
                                        ts.SubjectID = (int)tea.SubjectID;
                                        switch (ts.SubjectID)
                                        {
                                            case 1:
                                                ts.SubjectName = "语文";
                                                break;
                                            case 2:
                                                ts.SubjectName = "数学";
                                                break;
                                            case 3:
                                                ts.SubjectName = "英语";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        ts.SubjectID = 3;
                                        ts.SubjectName = "英语";
                                    }

                                }
                                else
                                {
                                    ts.SubjectID = 3;
                                    ts.SubjectName = "英语";
                                }
                            }
                            ts.TchID = data.UserID;
                            ts.TchName = user.TrueName;
                            ts.UserImage = user.UserImage;
                            var isExistUser = classinfo.ClassTchList.FirstOrDefault(x => x.TchID == ts.TchID);
                            if (isExistUser == null)
                            {
                                classinfo.ClassTchList.Add(ts);
                            }
                        }
                        else
                        {
                            classinfo = BuildClassInfoByClassId(data.ClassID.ToUpper());
                            //新增IBS_ClassOtherID数据
                            if (!string.IsNullOrEmpty(classinfo.ClassNum))
                            {
                                IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                classnum.ClassIDOther = classinfo.ClassNum;
                                classnum.ClassID = classinfo.ClassID.ToUpper();
                                classnum.Type = 1;
                                hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classinfo.ClassNum + "_" + 1, classnum);
                            }
                        }

                        hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                        result.Success = true;
                        result.ErrorMsg = "";
                        result.Data = "";
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMsg = "绑定失败";
                        result.Data = "";
                    }
                }
                else
                {
                    var returnInfo = relationservice.AddStudentClass(data.UserID.ToString(), data.ClassID.ToUpper(), data.UserID.ToString(), ((int)data.Type).ToString(), data.message, ((int)data.flag).ToString());
                    if (returnInfo != null)
                    {

                        //StudentClassRelationKey key = new StudentClassRelationKey();
                        //key.UserID = data.UserID;
                        //key.ClassID = data.ClassID;
                        //key.type = 1;
                        //redisList.RPush("StudentClassRelationKey", key.ToJson());
                        result.Success = returnInfo.Success;
                        result.ErrorMsg = returnInfo.ErrorMsg;
                        result.Data = returnInfo.Data;
                        if (returnInfo.Success)
                        {
                            //修改用户信息表的关联list
                            var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString());
                            if (user != null)
                            {
                                ClassSch cs = new ClassSch();
                                cs.ClassID = data.ClassID.ToUpper();

                                var re = GetClassUserRelationByClassId(data.ClassID.ToUpper());
                                if (re != null)
                                {
                                    cs.GradeID = (int)re.GradeID;
                                    if (re.SchID > 0)
                                    {
                                        var res = GetSchClassRelationBySchlId(re.SchID);
                                        if (res!=null)
                                        {
                                            cs.SchID = (int)re.SchID;
                                            cs.AreaID =res.AreaID;
                                        }
                                    }
                                    else
                                    {
                                        cs.SchID = 0;
                                    }
                                }
                                else
                                {
                                    cs.SchID = 0;
                                }




                                var isExists = user.ClassSchList.FirstOrDefault(a => a.ClassID.ToUpper() == cs.ClassID.ToUpper());
                                if (isExists == null)
                                {
                                    user.ClassSchList.Add(cs);

                                }
                            }
                            else
                            {
                                user = BuildUserInfoByUserId(data.UserID);
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
                                        if (!string.IsNullOrEmpty(invnum))
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
                            }


                            hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);


                            //修改班级教师list
                            var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper());
                            if (classinfo != null)
                            {
                                ClassStuS css = new ClassStuS();
                                css.StuID = data.UserID;
                                css.StuName = user.TrueName;
                                css.UserImage = user.UserImage;
                                css.IsEnableOss = user.IsEnableOss;
                                var isExistClass = classinfo.ClassStuList.FirstOrDefault(a => a.StuID == css.StuID);
                                if (isExistClass == null)
                                {
                                    classinfo.ClassStuList.Add(css);
                                }

                            }
                            else
                            {
                                classinfo = BuildClassInfoByClassId(data.ClassID.ToUpper());
                                if (!string.IsNullOrEmpty(classinfo.ClassNum))
                                {
                                    IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                    classnum.ClassIDOther = classinfo.ClassNum;
                                    classnum.ClassID = classinfo.ClassID.ToUpper();
                                    classnum.Type = 1;
                                    hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classinfo.ClassNum + "_" + 1, classnum);
                                }

                            }

                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                            result.Success = returnInfo.Success;
                            result.ErrorMsg = returnInfo.ErrorMsg;
                            result.Data = returnInfo.Data;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "调用绑定关系接口异常", ex);
                result.Success = false;
                result.ErrorMsg = ex.Message;
                result.Data = "";
            }

            return result;
        }

        /// <summary>
        /// 解除绑定关系
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIResponse UnBindClass(UserClassData data)
        {
            APIResponse result = new APIResponse();
            try
            {
                if (data.UserType == UserTypeEnum.Teacher)
                {
                    var returnInfo = relationservice.UnBindClassTea(data.ClassID.ToUpper(), data.UserID.ToString(), data.SubjectID);
                    if (returnInfo != null)
                    {
                        result.Data = returnInfo.Data;
                        result.Success = returnInfo.Success;
                        result.ErrorMsg = returnInfo.ErrorMsg;
                        if (returnInfo.Success)
                        {
                            //修改用户信息表的关联list
                            var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString());
                            if (user != null)
                            {
                                user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                            }
                            else
                            {
                                user = BuildUserInfoByUserId(data.UserID);
                                if (user != null)
                                {
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
                                            if (!string.IsNullOrEmpty(invnum))
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
                                }
                            }
                            hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);


                            //修改班级教师list
                            var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper());

                            if (classinfo != null)
                            {
                                classinfo.ClassTchList.RemoveAll(a => a.TchID == data.UserID);

                            }
                            else
                            {
                                classinfo = BuildClassInfoByClassId(data.ClassID.ToUpper());
                                if (classinfo != null)
                                {
                                    if (!string.IsNullOrEmpty(classinfo.ClassNum))
                                    {
                                        IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                        classnum.ClassIDOther = classinfo.ClassNum;
                                        classnum.ClassID = classinfo.ClassID.ToUpper();
                                        classnum.Type = 1;
                                        hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classinfo.ClassNum + "_" + 1, classnum);

                                    }
                                }

                            }
                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                            result.Data = returnInfo.Data;
                            result.Success = returnInfo.Success;
                            result.ErrorMsg = returnInfo.ErrorMsg;
                        }
                        else
                        {
                            if (returnInfo.ErrorMsg.Equals("错误:该用户没有关系记录"))
                            {
                                //修改用户信息表的关联list
                                var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString());
                                if (user != null)
                                {
                                    user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                                }
                                else
                                {
                                    user = BuildUserInfoByUserId(data.UserID);
                                    if (user != null)
                                    {
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
                                                if (!string.IsNullOrEmpty(invnum))
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
                                    }
                                }
                                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);


                                //修改班级教师list
                                var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper());

                                if (classinfo != null)
                                {
                                    classinfo.ClassTchList.RemoveAll(a => a.TchID == data.UserID);

                                }
                                else
                                {
                                    classinfo = BuildClassInfoByClassId(data.ClassID.ToUpper());
                                    if (classinfo != null)
                                    {
                                        if (!string.IsNullOrEmpty(classinfo.ClassNum))
                                        {

                                            IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                            classnum.ClassIDOther = classinfo.ClassNum;
                                            classnum.ClassID = classinfo.ClassID.ToUpper();
                                            classnum.Type = 1;
                                            hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classinfo.ClassNum + "_" + 1, classnum);

                                        }
                                    }

                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                                result.Data = returnInfo.Data;
                                result.Success = true;
                                result.ErrorMsg = returnInfo.ErrorMsg;
                            }
                        }
                    }
                }
                else
                {
                    var returnInfo = relationservice.UnBindClassStu(data.UserID.ToString(), data.ClassID.ToUpper());
                    if (returnInfo != null)
                    {
                        result.Data = returnInfo.Data;
                        result.Success = returnInfo.Success;
                        result.ErrorMsg = returnInfo.ErrorMsg;
                        if (returnInfo.Success)
                        {
                            //StudentClassRelationKey key = new StudentClassRelationKey();
                            //key.UserID = data.UserID;
                            //key.ClassID = data.ClassID;
                            //key.type = 2;
                            //redisList.RPush("StudentClassRelationKey", key.ToJson());
                            //修改用户信息表的关联list
                            var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString());
                            if (user != null)
                            {
                                user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                            }
                            else
                            {
                                user = BuildUserInfoByUserId(data.UserID);
                                if (user != null)
                                {
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
                                            if (!string.IsNullOrEmpty(invnum))
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
                                }
                            }

                            hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);


                            //修改班级教师list
                            var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper());
                            if (classinfo != null)
                            {
                                classinfo.ClassStuList.RemoveAll(a => a.StuID == data.UserID);

                            }
                            else
                            {
                                classinfo = BuildClassInfoByClassId(data.ClassID.ToUpper());
                                if (classinfo != null)
                                {
                                    if (!string.IsNullOrEmpty(classinfo.ClassNum))
                                    {

                                        IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                        classnum.ClassIDOther = classinfo.ClassNum;
                                        classnum.ClassID = classinfo.ClassID.ToUpper();
                                        classnum.Type = 1;
                                        hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classinfo.ClassNum + "_" + 1, classnum);

                                    }
                                }

                            }
                            hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                            result.Data = returnInfo.Data;
                            result.Success = returnInfo.Success;
                            result.ErrorMsg = returnInfo.ErrorMsg;
                        }
                        else
                        {
                            if (returnInfo.ErrorMsg.Equals("错误：该用户没有关系记录"))
                            {
                                //修改用户信息表的关联list
                                var user = hashRedis.Get<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString());
                                if (user != null)
                                {
                                    user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                                }
                                else
                                {
                                    user = BuildUserInfoByUserId(data.UserID);
                                    if (user != null)
                                    {
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
                                                if (!string.IsNullOrEmpty(invnum))
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
                                    }
                                }

                                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);


                                //修改班级教师list
                                var classinfo = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper());
                                if (classinfo != null)
                                {
                                    classinfo.ClassStuList.RemoveAll(a => a.StuID == data.UserID);

                                }
                                else
                                {
                                    classinfo = BuildClassInfoByClassId(data.ClassID.ToUpper());
                                    if (classinfo != null)
                                    {
                                        if (!string.IsNullOrEmpty(classinfo.ClassNum))
                                        {

                                            IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                            classnum.ClassIDOther = classinfo.ClassNum;
                                            classnum.ClassID = classinfo.ClassID.ToUpper();
                                            classnum.Type = 1;
                                            hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classinfo.ClassNum + "_" + 1, classnum);

                                        }
                                    }

                                }
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                                result.Data = returnInfo.Data;
                                result.Success = true;
                                result.ErrorMsg = returnInfo.ErrorMsg;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "调用解除绑定关系接口异常", ex);
                result.Data = "";
                result.Success = false;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }
        #endregion


        public string UnBindClassByClassId(string classId) 
        {
            var returnInfo = relationservice.UnBindClassByClassId(classId);
            return returnInfo;
        }

        public bool Add(IBS_ClassUserRelation classInfo)
        {
            throw new NotImplementedException();
        }

        public List<IBS_ClassUserRelation> SearchALL() 
        {
            return hashRedis.GetAll<IBS_ClassUserRelation>("IBS_ClassUserRelation");
        }

       
        /// <summary>
        /// 通过班级编码获取班级与学生关联List
        /// </summary>
        /// <param name="ClassNum"></param>
        /// <param name="stuCount"></param>
        /// <returns></returns>
        public List<TBX_UserClass> GetUserClassRelationByNum(string ClassNum, out List<TBX_StudentCount> stuCount)
        {
            var classinfo = GetClassUserRelationByClassOtherId(ClassNum, 1);
            List<TBX_UserClass> userClass = new List<TBX_UserClass>();
            stuCount = new List<TBX_StudentCount>();
            if (classinfo != null)
            {
                TBX_StudentCount sc = new TBX_StudentCount();
                sc.ClassNum = classinfo.ClassNum;
                sc.StuCount = classinfo.ClassStuList.Count;
                stuCount.Add(sc);
                classinfo.ClassStuList.ForEach(a =>
                {
                    TBX_UserClass c = new TBX_UserClass();
                    c.ClassID = classinfo.ClassID.ToUpper();
                    c.ClassNum = classinfo.ClassNum;
                    c.UserID = Convert.ToInt32(a.StuID);
                    if (userClass.FirstOrDefault(y => y.UserID == c.UserID) == null)
                    {
                        userClass.Add(c);
                    }
                });
                if (classinfo.ClassStuList.Count == 0)
                {
                    TBX_UserClass c = new TBX_UserClass();
                    c.ClassID = classinfo.ClassID.ToUpper();
                    c.ClassNum = classinfo.ClassNum;
                    userClass.Add(c);
                }

            }
            return userClass;
        }
    }
}
