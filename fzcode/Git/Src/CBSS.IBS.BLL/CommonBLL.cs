﻿
using CBSS.Framework.Redis;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.DAL;

namespace CBSS.IBS.BLL
{
    public class CommonBLL
    {
        Repository re = new Repository("IBS2");
        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper("IBS");
        static RedisHashHelper hashRedis = new RedisHashHelper("IBS");
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper("IBS");

        RelationService.RelationService relationservice = new RelationService.RelationService();
        FZUUMS_Relation2.FZUUMS_Relation2SoapClient relation2Client = new FZUUMS_Relation2.FZUUMS_Relation2SoapClient();
        FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();

        #region 组装数据
        /// <summary>
        /// 从MOD和DB中组装用户数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IBS_UserInfo BuildUserInfoByUserId(int userId)
        {
            IBS_UserInfo user = new IBS_UserInfo();

            //从MOD获取用户信息
            var modUser = userService.GetUserInfoByID("", userId.ToString());
            if (modUser == null)
            {
                return null;
            }
            List<ClassSch> cs = new List<ClassSch>();
            //DB获取数据
            var tbuser = re.SelectSearch<Tb_UserInfo>(a => a.UserID == userId).FirstOrDefault();

            if (tbuser == null)
            {
                tbuser = new Tb_UserInfo();
                tbuser.UserID = Convert.ToInt32(modUser.UserID);
                tbuser.UserImage = "00000000-0000-0000-0000-000000000000";
                tbuser.TrueName = modUser.TrueName;
                tbuser.UserName = modUser.UserName;
                tbuser.UserRoles = 0;
                tbuser.IsEnableOss = 0;
                tbuser.isLogState = "0";
                tbuser.IsUser = 1;
                tbuser.NickName = modUser.TrueName;
                tbuser.TelePhone = modUser.Telephone;
                tbuser.CreateTime = modUser.RegDate;
                tbuser.AppId = modUser.AppID;
                re.Insert(tbuser);
            }
            user.UserImage = tbuser.UserImage;
            user.isLogState = tbuser.isLogState;
            user.AppID = tbuser.AppId;
            user.IsUser = tbuser.IsUser == null ? 1 : (int)tbuser.IsUser;
            user.IsEnableOss = (int)tbuser.IsEnableOss;
            user.SchoolID = modUser.SchoolID;
            user.SchoolName = modUser.SchoolName;

            if (modUser.UserType == (int)UserTypeEnum.Teacher)
            {

                //从MOD获取班级信息
                FZUUMS_Relation2.tb_Class[] classList = relation2Client.GetClassByUserID("", userId.ToString());
                if (classList != null)
                {
                    List<FZUUMS_Relation2.tb_Class> ri = new List<FZUUMS_Relation2.tb_Class>(classList);
                    ri.ForEach(a =>
                    {
                        ClassSch s = new ClassSch();
                        s.ClassID = a.ID.ToString().ToUpper();
                        s.GradeID = (int)a.GradeID;
                        s.SchID = (int)a.SchoolID;
                        string schoolInfo = metadataService.GetSchoolInfo(Convert.ToInt32(a.SchoolID));
                        if (!string.IsNullOrEmpty(schoolInfo) && schoolInfo.Split('|')[0] != "错误")
                        {
                            MOD_SchoolInfoModel sm = schoolInfo.FromJson<MOD_SchoolInfoModel>();
                            s.AreaID = Convert.ToInt32(sm.DistrictID);
                        }
                        cs.Add(s);
                    });
                }

            }
            else
            {
                var stuClass = relation2Client.GetClassInfoByStuID(userId.ToString());
                if (stuClass != null)
                {
                    ClassSch s = new ClassSch();
                    s.ClassID = stuClass.ID.ToString().ToUpper();
                    s.GradeID = (int)stuClass.GradeID;
                    s.SchID = (int)stuClass.SchoolID;
                    string schoolInfo = metadataService.GetSchoolInfo(Convert.ToInt32(stuClass.SchoolID));
                    if (!string.IsNullOrEmpty(schoolInfo) && schoolInfo.Split('|')[0] != "错误")
                    {
                        MOD_SchoolInfoModel sm = schoolInfo.FromJson<MOD_SchoolInfoModel>();
                        s.AreaID = Convert.ToInt32(sm.DistrictID);
                    }
                    cs.Add(s);
                }
            }

            var userByTchInvNum = relationservice.GetUserTelInviTationByUserIdOrInvNum("", userId.ToString());
            if (userByTchInvNum != null)
            {
                var Inv = userByTchInvNum.FirstOrDefault();
                user.UserNum = Inv.InvitationNum;
            }
            if (modUser != null)
            {
                user.UserID = userId;
                user.UserName = modUser.UserName;
                user.UserPwd = modUser.PassWord;
                user.UserType = (int)modUser.UserType;
                user.TrueName = modUser.TrueName;
                user.TelePhone = modUser.Telephone;
                user.Regdate = (DateTime)modUser.RegDate;
                user.ClassSchList = cs;
            }
            return user;
        }

        /// <summary>
        /// 构建MOD班级数据
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public IBS_ClassUserRelation BuildClassInfoByClassId(string classId)
        {
            IBS_ClassUserRelation classUser = new IBS_ClassUserRelation();
            //获取Mod班级信息
            var modClassUser = relationservice.GetClassInfoByID(classId.ToUpper());
            if (modClassUser == null)
            {
                return null;
            }
            //获取班级里面学生信息
            var modUserList = relationservice.GetStudentListByClassId(classId.ToUpper());
            //获取班级教师列表
            var modTeacher = relation2Client.GetTeaByClassID(classId.ToUpper());

            //班级学生关系数据处理
            List<ClassStuS> classStuList = new List<ClassStuS>();
            if (modUserList != null)
            {
                var model = modUserList.ToList();
                model.ForEach(a =>
                {
                    ClassStuS classStu = new ClassStuS();
                    var user = re.SelectSearch<Tb_UserInfo>(x => x.UserID == Convert.ToInt32(a.UserID)).FirstOrDefault();
                    if (user != null)
                    {
                        classStu.UserImage = user.UserImage;//数据库查询UserImage
                        classStu.IsEnableOss = (int)user.IsEnableOss;
                    }
                    classStu.StuID = Convert.ToInt32(a.UserID);
                    classStu.StuName = a.TrueName;
                    classStuList.Add(classStu);
                });
            }

            //班级教师关系数据处理
            List<ClassTchS> classTeaList = new List<ClassTchS>();
            if (modTeacher != null)
            {
                var teachList = modTeacher.ToList();
                teachList.ForEach(a =>
                {
                    ClassTchS classTea = new ClassTchS();
                    classTea.TchID = Convert.ToInt32(a.UserID);

                    var teaInfo = userService.GetUserInfoByID("", a.UserID);
                    if (teaInfo != null)
                    {
                        classTea.TchName = teaInfo.TrueName;//再次请求教师名称

                        classTea.SubjectID = (int)a.SubjectID;
                        classTea.SubjectName = teaInfo.SubjectName;//再次请求学科名称
                    }
                    classTea.UserImage = "";//本地数据库获取
                    classTeaList.Add(classTea);
                });
            }

            //赋值
            if (modClassUser != null)
            {
                classUser.ClassID = modClassUser.ID.ToString().ToUpper();
                classUser.ClassName = modClassUser.ClassName;
                classUser.ClassNum =modClassUser.ClassNum;//string?
                classUser.GradeID = (int)modClassUser.GradeID;
                classUser.GradeName = modClassUser.GradeID.GetGradeName();
                classUser.SchID = (int)modClassUser.SchoolID;
                classUser.ClassStuList = classStuList;
                classUser.ClassTchList = classTeaList;
            }
            return classUser;
        }


        /// <summary>
        /// 构建MOD学校信息
        /// </summary>
        /// <param name="schId"></param>
        /// <returns></returns>
        public IBS_SchClassRelation BuildSchoolInfoBySchoolId(int schId)
        {
            IBS_SchClassRelation schClass = new IBS_SchClassRelation();
            //获取学校信息
            var modSchInfo = metadataService.GetSchoolInfo(schId);
            List<SchClassS> schClassList = new List<SchClassS>();
            //获取学校班级列表
            var modClassList = relation2Client.GetSchoolClass(schId);
            if (modClassList != null)
            {
                var classList = modClassList.ToList();
                classList.ForEach(a =>
                {
                    SchClassS sc = new SchClassS();
                    sc.ClassID = a.ID.ToString().ToUpper();
                    sc.ClassName = a.ClassName;
                    sc.GradeID = (int)a.GradeID;
                    sc.GradeName = a.GradeID.GetGradeName();
                    schClassList.Add(sc);
                });
            }
            if (!string.IsNullOrEmpty(modSchInfo) && modSchInfo.Split('|')[0] != "错误")
            {
                MOD_SchoolInfoModel schoolInfo = modSchInfo.FromJson<MOD_SchoolInfoModel>();
                schClass.SchID = schId;
                schClass.SchName = schoolInfo.SchoolName;
                schClass.AreaID = Convert.ToInt32(schoolInfo.DistrictID);
            }
            else
            {
                return null;
            }
            schClass.SchClassList = schClassList;

            //Redis操作

            return schClass;

        }

        /// <summary>
        /// 构建MOD学校信息
        /// </summary>
        /// <param name="schId"></param>
        /// <returns></returns>
        public IBS_AreaSchRelation BuildAreaInfoByAreaId(int AreaId)
        {
            IBS_AreaSchRelation areaSchRelation = new IBS_AreaSchRelation();
            var modAreaSchJson = metadataService.GetAreaInfo(AreaId);
            if (!string.IsNullOrEmpty(modAreaSchJson) && modAreaSchJson.Split('|')[0] != "错误")
            {
                MOD_AreaInfoModel modAreaSch = modAreaSchJson.FromJson<MOD_AreaInfoModel>();

                areaSchRelation.AreaID = Convert.ToInt32(modAreaSch.ID);
                areaSchRelation.AreaName = modAreaSch.CodeName;
            }
            else
            {
                return null;
            }
            var modschInfoJson = metadataService.GetSchoolData(AreaId.ToString(), "", "");
            if (!string.IsNullOrEmpty(modschInfoJson) && modschInfoJson.Split('|')[0] != "错误")
            {
                List<MOD_SchoolInfoModel> modSchInfo = modschInfoJson.FromJson<List<MOD_SchoolInfoModel>>();
                modSchInfo.ForEach(a =>
                {
                    AreaSchS areaSch = new AreaSchS();
                    areaSch.SchD = a.ID;
                    areaSch.SchName = a.SchoolName;
                    areaSchRelation.AreaSchList.Add(areaSch);
                });
            }
            return areaSchRelation;
        }
        #endregion
    }
}