
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.IBSLearnReport;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL
{
    public class IBSData_ClassUserRelationBLL : CommonBLL, IIBSData_ClassUserRelationBLL
    {
        //IBS reids
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
        static IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();
        /// <summary>
        /// 班级变更后学习报告修改队列
        /// </summary>
        static RedisListHelper redisList = new RedisListHelper();


        static RelationService.RelationService relationservice = new RelationService.RelationService();
        static FZUUMS_Relation2.FZUUMS_Relation2SoapClient relation2Client = new FZUUMS_Relation2.FZUUMS_Relation2SoapClient();
        static MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();



        static private IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();

        /// <summary>
        /// 通过ID获取班级信息
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public IBS_ClassUserRelation GetClassUserRelationByClassId(string classId)
        {
            var classUser = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", classId.ToUpper());
            if (classUser == null)
            {
                classUser = BuildClassInfoByClassId(classId.ToUpper());
                if (classUser != null)
                {
                    //新增IBS_ClassOtherID数据
                    if (!string.IsNullOrEmpty(classUser.ClassNum))
                    {
                        IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                        classnum.ClassIDOther = classUser.ClassNum;
                        classnum.ClassID = classUser.ClassID.ToUpper();
                        classnum.Type = 1;
                        hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classUser.ClassNum + "_" + 1, classnum);
                    }
                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", classId.ToUpper(), classUser);
                }

            }

            if (classUser != null)
            {
                classUser.ClassStuList.ForEach(a =>
                    {
                        a.StuName = string.IsNullOrEmpty(a.StuName) ? "暂未填写" : a.StuName;
                    });
                classUser.ClassTchList.ForEach(a =>
                {
                    a.TchName=string.IsNullOrEmpty(a.TchName)?"暂未填写":a.TchName;
                });
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
            var classUser = GetClassUserRelationByClassId(ClassId.ToUpper());
            if (classUser != null)
            {
                result.iBS_ClassUserRelation = classUser;
                if (classUser.SchID > 0)
                {
                    var schinfo = schBLL.GetSchClassRelationBySchlId(classUser.SchID);
                    if (schinfo != null)
                    {
                        result.SchName = schinfo.SchName;
                    }
                }
            }
            else
            {
                return null;
            }

            return result;
        }



        /// <summary>
        /// 通过其他获取班级信息
        /// </summary>
        /// <param name="classOtherId"></param>
        /// <param name="type">查找类型（ClassID=0,ClassNum=1）</param>
        /// <returns></returns>
        public Model.IBS_ClassUserRelation GetClassUserRelationByClassOtherId(string classOtherId, int type)
        {
            IBS_ClassUserRelation classUser = null;
            var classOther = hashRedis.Get<IBS_ClassOtherID>("IBS_ClassOtherID", classOtherId + "_" + type);
            if (classOther != null)
            {
                classUser = GetClassUserRelationByClassId(classOther.ClassID.ToUpper());
            }
            else
            {
                classOther = new IBS_ClassOtherID();
                switch (type)
                {
                    case 1://ClassNum
                        var classByClassNum = relation2Client.GetClassInfoByNum(classOtherId);
                        if (classByClassNum != null)
                        {
                            classOther.ClassID = classByClassNum.ID.ToString().ToUpper();
                            classOther.ClassIDOther = classOtherId;
                            classOther.Type = 1;
                            hashRedis.Set<IBS_ClassOtherID>("IBS_ClassOtherID", classOtherId + "_" + type, classOther);
                            classUser = GetClassUserRelationByClassId(classOther.ClassID.ToUpper());
                        }
                        break;
                }

            }
            return classUser;
        }

        public KingResponse GetUserClassInfoList(string userIds)
        {
            KingResponse result = new KingResponse();
            var ri = relation2Client.GetUserClassInfoList(userIds);
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
        /// <param name="operatorUserId"></param>
        /// <returns></returns>

        public string Add(IBS_ClassUserRelation classInfo, int operatorUserId)
        {
            var result = "";
            //从MOD新接口添加数据
            var returnInfo = relationservice.AddClassInCPoint(classInfo.SchID.ToString(), classInfo.ClassName, operatorUserId.ToString());
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


        /// <summary>
        /// 创建班级
        /// </summary>
        /// <param name="classInfo"></param>
        /// <param name="operatorUserId"></param>
        /// <returns></returns>

        public string CFAdd(IBS_ClassUserRelation classInfo, int operatorUserId)
        {
            var result = "";
            //从MOD新接口添加数据
            var returnInfo = relation2Client.CFCreateClass(classInfo.SchID.ToString(), classInfo.ClassName, operatorUserId.ToString());
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
        public KingResponse AddUserToClass(UserClassData data)
        {
            KingResponse result = new KingResponse();
            try
            {
                if (data.UserType == UserTypeEnum.Teacher)
                {
                    var returnInfo = relation2Client.BindClassTea(Guid.Parse(data.ClassID.ToUpper()), data.UserID.ToString(), data.SchoolId, Convert.ToInt32(data.SubjectID));
                    if (returnInfo)
                    {
                        TeacherBindClass(data);

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

                        AddReportingKey(data);
                        result.Success = returnInfo.Success;
                        result.ErrorMsg = returnInfo.ErrorMsg;
                        result.Data = returnInfo.Data;
                        if (returnInfo.Success)
                        {
                            StudentBindClass(data);
                            result.Success = returnInfo.Success;
                            result.ErrorMsg = returnInfo.ErrorMsg;
                            result.Data = returnInfo.Data;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "调用绑定关系接口异常!");
                result.Success = false;
                result.ErrorMsg = ex.Message;
                result.Data = "";
            }

            return result;
        }

        private void StudentBindClass(UserClassData data)
        {
            var user = userBLL.GetUserInfoByUserId(data.UserID);
            if (user != null)
            {
                ClassSch cs = new ClassSch();
                cs.ClassID = data.ClassID.ToUpper();

                var re = relationservice.GetClassInfoByID(data.ClassID.ToUpper());
                if (re != null)
                {
                    if (re.GradeID != null) cs.GradeID = (int)re.GradeID;
                    if (re.SchoolID != null && re.SchoolID > 0)
                    {
                        var res = metadataService.GetSchoolInfo((int)re.SchoolID);
                        if (!string.IsNullOrEmpty(res) && res.Split('|')[0] != "错误")
                        {
                            cs.SchID = (int)re.SchoolID;
                            MOD_SchoolInfoModel schoolInfo = JsonHelper.DecodeJson<MOD_SchoolInfoModel>(res);
                            cs.AreaID = Convert.ToInt32(schoolInfo.DistrictID);
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
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);
            }

            //修改班级教师list
            var classinfo = GetClassUserRelationByClassId(data.ClassID);
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
                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
            }
           
        }

        private static void AddReportingKey(UserClassData data)
        {
            StudentClassRelationKey key = new StudentClassRelationKey();
            key.UserID = data.UserID;
            key.ClassID = data.ClassID;
            key.type = 1;
            redisList.RPush("StudentClassRelationKey", key.ToJson());
        }

        private void TeacherBindClass(UserClassData data)
        {
            //修改用户信息表的关联list
            var user = userBLL.GetUserInfoByUserId(data.UserID);
            if (user != null)
            {
                ClassSch cs = new ClassSch();
                cs.SchID = data.SchoolId;
                cs.ClassID = data.ClassID.ToUpper();
                cs.SubjectID = Convert.ToInt32(data.SubjectID);
                var re = relationservice.GetClassInfoByID(data.ClassID.ToUpper());
                if (re != null)
                {
                    if (re.GradeID != null) cs.GradeID = (int)re.GradeID;
                }

                var res = metadataService.GetSchoolInfo(data.SchoolId);

                if (!string.IsNullOrEmpty(res) && res.Split('|')[0] != "错误")
                {
                    MOD_SchoolInfoModel schoolInfo = JsonHelper.DecodeJson<MOD_SchoolInfoModel>(res);
                    cs.AreaID = Convert.ToInt32(schoolInfo.DistrictID);
                }

                var isExists = user.ClassSchList.FirstOrDefault(x => x.ClassID.ToUpper() == cs.ClassID.ToUpper());
                if (isExists == null)
                {
                    user.ClassSchList.Add(cs);
                }
                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);
            }


            //修改班级教师list
            var classinfo = GetClassUserRelationByClassId(data.ClassID.ToUpper());
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
                            ts.SubjectName = StringEnumHelper.GetStringValue<SubjectEnum>(ts.SubjectID);
                        }
                        else
                        {
                            ts.SubjectID = 3;
                            ts.SubjectName = StringEnumHelper.GetStringValue<SubjectEnum>(ts.SubjectID);
                        }
                    }
                    else
                    {
                        ts.SubjectID = 3;
                        ts.SubjectName = StringEnumHelper.GetStringValue<SubjectEnum>(ts.SubjectID);
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
        }

        /// <summary>
        /// 解除绑定关系
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public KingResponse UnBindClass(UserClassData data)
        {
            KingResponse result = new KingResponse();
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
                            var user = userBLL.GetUserInfoByUserId(data.UserID);
                            if (user != null)
                            {
                                user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);
                            }



                            //修改班级教师list
                            var classinfo = GetClassUserRelationByClassId(data.ClassID.ToUpper());
                            if (classinfo != null)
                            {
                                classinfo.ClassTchList.RemoveAll(a => a.TchID == data.UserID);
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                            }

                            result.Data = returnInfo.Data;
                            result.Success = returnInfo.Success;
                            result.ErrorMsg = returnInfo.ErrorMsg;
                        }
                        else
                        {
                            if (returnInfo.ErrorMsg.Equals("错误:该用户没有关系记录"))
                            {
                                //修改用户信息表的关联list
                                var user = userBLL.GetUserInfoByUserId(data.UserID);
                                if (user != null)
                                {
                                    user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);
                                }
                                //修改班级教师list
                                //修改班级教师list
                                var classinfo = GetClassUserRelationByClassId(data.ClassID.ToUpper());
                                if (classinfo != null)
                                {
                                    classinfo.ClassTchList.RemoveAll(a => a.TchID == data.UserID);
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                                }
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
                            StudentClassRelationKey key = new StudentClassRelationKey();
                            key.UserID = data.UserID;
                            key.ClassID = data.ClassID;
                            key.type = 2;
                            redisList.RPush("StudentClassRelationKey", key.ToJson());

                            //修改用户信息表的关联list
                            var user = userBLL.GetUserInfoByUserId(data.UserID);
                            if (user != null)
                            {
                                user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                                hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);
                            }


                            //修改班级教师list
                            var classinfo = GetClassUserRelationByClassId(data.ClassID.ToUpper());
                            if (classinfo != null)
                            {
                                classinfo.ClassTchList.RemoveAll(a => a.TchID == data.UserID);
                                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                            }
                            result.Data = returnInfo.Data;
                            result.Success = returnInfo.Success;
                            result.ErrorMsg = returnInfo.ErrorMsg;
                        }
                        else
                        {
                            if (returnInfo.ErrorMsg.Equals("错误：该用户没有关系记录"))
                            {
                                //修改用户信息表的关联list
                                var user = userBLL.GetUserInfoByUserId(data.UserID);
                                if (user != null)
                                {
                                    user.ClassSchList.RemoveAll(a => a.ClassID.ToUpper() == data.ClassID.ToUpper());
                                    hashRedis.Set<IBS_UserInfo>("IBS_UserInfo", data.UserID.ToString(), user);
                                }


                                //修改班级教师list
                                var classinfo = GetClassUserRelationByClassId(data.ClassID.ToUpper());
                                if (classinfo != null)
                                {
                                    classinfo.ClassTchList.RemoveAll(a => a.TchID == data.UserID);
                                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", data.ClassID.ToUpper(), classinfo);
                                }
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
                Log4Net.LogHelper.Error(ex, "调用解除绑定关系接口异常!");
                result.Data = "";
                result.Success = false;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }
        #endregion


        public string UnBindClassByClassId(string classId)
        {
            var returnInfo = relationservice.UnBindClassByClassId(classId.ToUpper());
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
        /// <param name="classNum"></param>
        /// <param name="stuCount"></param>
        /// <returns></returns>
        public List<UserClass> GetUserClassRelationByNum(string classNum, out List<StudentCount> stuCount)
        {
            var classinfo = GetClassUserRelationByClassOtherId(classNum, 1);
            List<UserClass> userClass = new List<UserClass>();
            stuCount = new List<StudentCount>();
            if (classinfo != null)
            {
                StudentCount sc = new StudentCount();
                sc.ClassNum = classinfo.ClassNum;
                sc.StuCount = classinfo.ClassStuList.Count;
                stuCount.Add(sc);
                classinfo.ClassStuList.ForEach(a =>
                {
                    UserClass c = new UserClass();
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
                    UserClass c = new UserClass();
                    c.ClassID = classinfo.ClassID.ToUpper();
                    c.ClassNum = classinfo.ClassNum;
                    userClass.Add(c);
                }

            }

            return userClass;
        }
    }
}
