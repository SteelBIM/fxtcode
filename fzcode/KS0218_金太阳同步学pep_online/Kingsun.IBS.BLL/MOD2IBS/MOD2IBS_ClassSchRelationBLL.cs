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
    public class MOD2IBS_ClassSchRelationBLL : IIBS_MOD_ClassSchRelationBLL
    {
        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper();
        static RedisHashHelper hashRedis = new RedisHashHelper();
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper();

        RelationService.IRelationService relationservice = new RelationService.RelationServiceClient();
        FZUUMS_Relation2.FZUUMS_Relation2SoapClient relation2Client = new FZUUMS_Relation2.FZUUMS_Relation2SoapClient();
        FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();
        public bool Change(string ClassID)
        {
            bool result = false;
            try
            {
                ///
                var oldClass = hashRedis.Get<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassID);
                var newClass = BuildClassInfoByClassId(ClassID);

                //旧学校信息移除变更的班级
                var oldSchInfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", oldClass.SchID.ToString());
                oldSchInfo.SchClassList.Remove(oldSchInfo.SchClassList.Where(a => a.ClassID == oldClass.ClassID).First());
                hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", oldClass.SchID.ToString(), oldSchInfo);

                //新学校信息新增变更班级
                var newSchInfo = hashRedis.Get<IBS_SchClassRelation>("IBS_SchClassRelation", newClass.SchID.ToString());
                SchClassS schClass = new SchClassS();
                schClass.ClassID = newClass.ClassID;
                schClass.ClassName = newClass.ClassName;
                schClass.GradeID = newClass.GradeID;
                schClass.GradeName = newClass.GradeName;
                newSchInfo.SchClassList.Add(schClass);
                hashRedis.Set<IBS_SchClassRelation>("IBS_SchClassRelation", newClass.SchID.ToString(), oldSchInfo);

                //获取最新的数据 修改redis
                hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", ClassID, newClass);

                result = true;
            }
            catch (Exception ex) 
            {
                Log4Net.LogHelper.Error(ex, "班级学校变动Change接口异常，ClassID=" + ClassID);
                //新增到异常处理队列
                MOD2IBS_PushDataException excep = new MOD2IBS_PushDataException();
                excep.Type = "";
                excep.Json = ClassID;
                excep.ErrorMessage = "班级学校变动Change接口异常，ClassID=" + ClassID;
                listRedis.LPush("MOD2IBS_PushDataException", excep.ToJson());

                result = false;

            }
            return result;
        }

        #region 构建数据
        /// <summary>
        /// 构建MOD班级数据
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public Model.IBS_ClassUserRelation BuildClassInfoByClassId(string classId)
        {
            IBS_ClassUserRelation classUser = new IBS_ClassUserRelation();
            //获取Mod班级信息
            var modClassUser = relationservice.GetClassInfoByID(classId);
            //获取班级里面学生信息
            var modUserList = relationservice.GetStudentListByClassId(classId);
            //获取班级教师列表
            var modTeacher = relation2Client.GetTeaByClassID(classId);

            //班级学生关系数据处理
            List<ClassStuS> classStuList = new List<ClassStuS>();
            if (modUserList != null)
            {
                var model = modUserList.ToList();
                model.ForEach(a =>
                {
                    ClassStuS classStu = new ClassStuS();
                    classStu.StuID = Convert.ToInt32(a.UserID);
                    classStu.StuName = a.TrueName;
                    classStu.UserImage = "";//数据库查询UserImage
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
                    classTea.TchName = teaInfo.TrueName;//再次请求教师名称

                    classTea.SubjectID =(int)a.SubjectID;
                    classTea.SubjectName = teaInfo.SubjectName;//再次请求学科名称

                    classTea.UserImage = "";//本地数据库获取
                    classTeaList.Add(classTea);
                });
            }

            //赋值
            if (modClassUser != null)
            {
                classUser.ClassID = modClassUser.ID.ToString();
                classUser.ClassName = modClassUser.ClassName;
                classUser.ClassNum = Convert.ToInt32(modClassUser.ClassNum);//string?
                classUser.GradeID = (int)modClassUser.GradeID;
                classUser.GradeName = Extension.GetGradeName(modClassUser.GradeID);
                classUser.SchID = (int)modClassUser.SchoolID;
                classUser.ClassStuList = classStuList;
                classUser.ClassTchList = classTeaList;
            }
            return classUser;
        }
        #endregion 
    }
}
