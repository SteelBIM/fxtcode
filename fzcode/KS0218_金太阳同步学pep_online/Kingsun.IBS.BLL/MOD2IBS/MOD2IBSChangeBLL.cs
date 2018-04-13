using Kingsun.IBS.BLL.IBS2MOD;
using Kingsun.IBS.DAL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.IBLL.IBS_MOD;
using Kingsun.IBS.IBLL.IBS2MOD;
using Kingsun.IBS.IDAL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL.MOD2IBS
{
    /// <summary>
    /// MOD2IBS数据变动业务类
    /// </summary>
    public class MOD2IBSChangeBLL : IMOD2IBSChangeBLL
    {
        //消息队列
        static RedisListOtherHelper listRedis = new RedisListOtherHelper();
        static RedisHashOtherHelper hashRedis=new RedisHashOtherHelper();
        private string connectMODDB = ConfigurationManager.AppSettings["connectMODDB"];

        IIBS_MOD_UserInfoBLL userBLL = new MOD2IBS_UserInfoBLL();
        IIBS_MOD_UserClassRelationBLL userClassBLL = new MOD2IBS_UserClassRelationBLL();
        IIBS_MOD_ClassInfoBLL classBLL = new MOD2IBS_ClassInfoBLL();
        IIBS_MOD_SchInfoBLL schBLL = new MOD2IBS_SchInfoBLL();
        IIBS_MOD_AreaInfoBLL areaBLL = new MOD2IBS_AreaInfoBLL();

        IIBSData_UserInfoBLL userinfoBLL=new IBSData_UserInfoBLL();


        /// <summary>
        /// 正常业务库
        /// </summary>
        public BaseManagement bm = new BaseManagement();

        /// <summary>
        /// BaseDB总库
        /// </summary>
        public BaseManagementOther bm1 = new BaseManagementOther();
        IProcDal proc = new ProcDal();
        public void Change()
        {
            
            var listCount = listRedis.Count("MODIBSListKey");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            int Changetype = 0;
            int DataType = 0;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("MODIBSListKey");
                try
                {
                    if (!string.IsNullOrEmpty(model))
                    {
                        MOD_PushData data = JsonHelper.DecodeJson<MOD_PushData>(model);
                        DataType = data.DataType;
                        Changetype = data.ChangeType;
                        //数据类型,1=账号信息（暂不用）,2=班级信息,3=学校信息,4=用户信息,5=区域信息,6=课程信息,7=用户班级关系,8=班级学校关系,9=学校区域关系
                        switch (data.DataType)
                        {
                            case 2:
                                var classInfo = JsonHelper.DecodeJson<IBS_ClassUserRelation>(data.Data);
                                if (classInfo != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            classBLL.Update(classInfo);
                                            break;
                                        case 2:
                                            classBLL.Add(classInfo);
                                            break;
                                        case 3:
                                            classBLL.Delete(classInfo);
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                var schInfo = JsonHelper.DecodeJson<IBS_SchClassRelation>(data.Data);
                                if (schInfo != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            schBLL.Update(schInfo);
                                            break;
                                        case 2:
                                            schBLL.Add(schInfo);
                                            break;
                                        case 3:
                                            schBLL.Delete(schInfo);
                                            break;
                                    }
                                }
                                break;
                            case 4:
                                var user = JsonHelper.DecodeJson<IBS_UserInfo>(data.Data);
                                if (user != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            userBLL.Update(user);
                                            break;
                                        case 2:
                                            userBLL.Add(user);
                                            break;
                                        case 3:
                                            userBLL.Delete(user);
                                            break;
                                    }
                                }
                                break;
                            case 5:
                                var areaInfo = JsonHelper.DecodeJson<IBS_AreaSchRelation>(data.Data);
                                if (areaInfo != null)
                                {
                                    switch (data.ChangeType)
                                    {
                                        case 1:
                                            areaBLL.Update(areaInfo);
                                            break;
                                        case 2:
                                            areaBLL.Add(areaInfo);
                                            break;
                                        case 3:
                                            areaBLL.Delete(areaInfo);
                                            break;
                                    }
                                }
                                break;
                            case 7:
                                var userClass = JsonHelper.DecodeJson<IBS_UserInfo>(data.Data);
                                //0=查询,1=更新,2=增加,3=删除
                                switch (data.ChangeType)
                                {
                                    case 2:
                                        userClassBLL.ClassUserAdd(userClass);
                                        break;
                                    case 3:
                                        userClassBLL.ClassUserRemove(userClass);
                                        break;

                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "MOD同步IBS同步异常,异常数据：" + model);
                    MOD2IBS_PushDataException exc = new MOD2IBS_PushDataException();
                    exc.ErrorMessage = ex.Message;
                    exc.Json = model;
                    exc.ChangeType = Changetype;
                    exc.DataType = DataType;
                    listRedis.LPush("MOD2IBS_PushDataException", exc.ToJson());
                }

            }
        }

        public void CreateProcTable()
        {
           /* proc_GetUserStudyCurriculum();
            proc_GetUserStudyDetailsLite3();*/
          //  proc_GetUserStudyDirectory();
          //  proc_GetUserStudyReport();
        }


        public void MOD2IBSFirstCreateData()
        {
            MOD2IBSUserInfo();
            MOD2IBSClassInfo();
            MOD2IBSAreaInfo();
            MOD2IBSSchInfo();
        }

        /// <summary>
        ///同步用户数据到IBS
        /// </summary>
        public void MOD2IBSSchInfo()
        {
            Log4Net.LogHelper.Info("同步学校信息到IBS");

            try
            {
                string sql = @"select * from dbo.MOD2IBS_temparea a 
                               left join dbo.MOD2IBS_tempClassUser b on a.schoolId=b.schoolId
                               WHERE a.schoolid<>0";
                DataSet ds = bm1.ExecuteSql(sql);
                List<MOD2IBS_Temparea> list = Extension.Convert2Object<MOD2IBS_Temparea>(ds.Tables[0]);
                Log4Net.LogHelper.Info("同步学校信息条数：" + list.Count);
                if (list.Count > 0)
                {
                    using (var Redis = RedisOtherManager.GetClient(0))
                    {
                        list.ForEach(ax =>
                        {
                            var schid = 0;
                            try
                            {
                                if (ax.SchoolID != null)
                                {
                                    schid = (int)ax.SchoolID;
                                    IBS_SchClassRelation schInfo = null;
                                    string value = Redis.GetValueFromHash("IBS_SchClassRelation", ax.SchoolID.ToString());
                                    if (string.IsNullOrEmpty(value))
                                    {
                                        schInfo = default(IBS_SchClassRelation);
                                    }
                                    else
                                    {
                                        schInfo = value.toObject<IBS_SchClassRelation>();
                                    }
                                    if (schInfo == null)
                                    {
                                        schInfo = new IBS_SchClassRelation();
                                    }
                                    schInfo.SchID = (int)ax.SchoolID;
                                    schInfo.SchName = ax.SchoolName;
                                    schInfo.AreaID = ax.AreaID == null ? 0 : (int)ax.AreaID;
                                    if (ax.ClassID != null && !string.IsNullOrEmpty(ax.ClassID.ToString()))
                                    {
                                        SchClassS cs = new SchClassS();
                                        cs.ClassID = ax.ClassID.ToString().ToUpper();
                                        cs.ClassName = ax.ClassName;
                                        cs.GradeID = ax.GradeID == null ? 0 : (int)ax.GradeID;
                                        cs.GradeName = cs.GradeID.GetGradeName();
                                        if (schInfo.SchClassList.FirstOrDefault(a => a.ClassID.ToUpper() == ax.ClassID.ToString().ToUpper()) == null)
                                        {
                                            schInfo.SchClassList.Add(cs);
                                        }

                                    }
                                    Redis.SetEntryInHash("IBS_SchClassRelation", ax.SchoolID.ToString(), schInfo.ToJson());
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "同步学校信息异常！" + schid);
                            }
                        });
                        Log4Net.LogHelper.Info("学校信息同步全部完成！一共写入" + list.Count + "条");
                        Redis.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步学校信息异常！");
            }
        }
        /// <summary>
        ///同步区域数据到IBS
        /// </summary>
        public void MOD2IBSAreaInfo()
        {

            Log4Net.LogHelper.Info("同步区域信息到IBS");
            try
            {
                string sql = @"select * from dbo.MOD2IBS_temparea";
                DataSet ds = bm1.ExecuteSql(sql);
                List<MOD2IBS_Temparea> list = Extension.Convert2Object<MOD2IBS_Temparea>(ds.Tables[0]);
                Log4Net.LogHelper.Info("同步区域信息条数：" + list.Count);
                if (list.Count > 0)
                {
                    using (var Redis = RedisOtherManager.GetClient(0))
                    {
                        var count = 0;
                        list.ForEach(ax =>
                        {
                            var areaid = 0;
                            try
                            {
                                if (ax.AreaID != null)
                                {
                                    areaid = (int)ax.AreaID;
                                    IBS_AreaSchRelation areaInfo = null;
                                    string value = Redis.GetValueFromHash("IBS_AreaSchRelation", ax.AreaID.ToString());
                                    if (string.IsNullOrEmpty(value))
                                    {
                                        areaInfo = default(IBS_AreaSchRelation);
                                    }
                                    else
                                    {
                                        areaInfo = value.toObject<IBS_AreaSchRelation>();
                                    }
                                    if (areaInfo == null)
                                    {
                                        areaInfo = new IBS_AreaSchRelation();
                                    }
                                    areaInfo.AreaID = (int)ax.AreaID;
                                    areaInfo.AreaName = ax.AreaName;
                                    if (ax.SchoolID != null && ax.SchoolID > 0)
                                    {
                                        AreaSchS arsch = new AreaSchS();
                                        arsch.SchD = (int)ax.SchoolID;
                                        arsch.SchName = ax.SchoolName;
                                        if (areaInfo.AreaSchList.FirstOrDefault(a => a.SchD == ax.SchoolID) == null)
                                        {
                                            areaInfo.AreaSchList.Add(arsch);
                                        }

                                    }
                                    Redis.SetEntryInHash("IBS_AreaSchRelation", ax.AreaID.ToString(), areaInfo.ToJson());
                                    count += 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "同步区域信息异常！" + areaid);
                            }

                        });
                        Log4Net.LogHelper.Info("区域信息同步全部完成！一共写入" + count + "条");
                        Redis.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步区域信息异常！");
            }
        }
        /// <summary>
        ///同步班级数据到IBS
        /// </summary>
        public void MOD2IBSClassInfo()
        {
            Log4Net.LogHelper.Info("同步班级信息到IBS");

            try
            {
                string sql = @"select * from dbo.MOD2IBS_tempClassUser a left join dbo.[tb_userinfo] b on a.userid=b.userid ";
                DataSet ds = bm1.ExecuteSql(sql);

                List<MOD2IBS_TempClassUser> list = Extension.Convert2Object<MOD2IBS_TempClassUser>(ds.Tables[0]);

                string sql1 = @"select Classid,UserType,Gradeid,subjectid,a.userid,a.TrueName,b.UserImage,b.IsEnableOss from dbo.MOD2IBS_tempUser a left join dbo.[Tb_UserInfo] b on a.Userid=b.UserID
                                where a.classid is not null;";
                DataSet ds1 = bm1.ExecuteSql(sql1);

                List<MOD2IBS_TempStu> list1 = Extension.Convert2Object<MOD2IBS_TempStu>(ds1.Tables[0]);
                Log4Net.LogHelper.Info("同步班级信息条数：" + list.Count);

                if (list.Count > 0)
                {
                    using (var Redis = RedisOtherManager.GetClient(0))
                    {
                        list.ForEach(ax =>
                        {
                            var classid = "";
                            try
                            {
                                classid = ax.ClassID.ToString();
                                IBS_ClassUserRelation classinfo = null;
                                string value = Redis.GetValueFromHash("IBS_ClassUserRelation", ax.ClassID.ToString().ToUpper());
                                if (string.IsNullOrEmpty(value))
                                {
                                    classinfo = default(IBS_ClassUserRelation);
                                }
                                else
                                {
                                    classinfo = value.toObject<IBS_ClassUserRelation>();
                                }
                                if (classinfo == null)
                                {
                                    classinfo = new IBS_ClassUserRelation();
                                }
                                classinfo.ClassID = ax.ClassID.ToString().ToUpper();
                                classinfo.ClassName = ax.ClassName;
                                classinfo.ClassNum = ax.ClassNum;
                                classinfo.CreateDate = DateTime.Now;
                                classinfo.SchID = ax.SchoolID == null ? 0 : (int)ax.SchoolID;
                                classinfo.GradeID = ax.GradeID == null ? 0 : (int)ax.GradeID;
                                classinfo.GradeName = classinfo.GradeID.GetGradeName();


                                if (!string.IsNullOrEmpty(ax.ClassNum))
                                {
                                    IBS_ClassOtherID classnum = new IBS_ClassOtherID();
                                    classnum.ClassIDOther = ax.ClassNum.ToString();
                                    classnum.ClassID = ax.ClassID.ToString().ToUpper();
                                    classnum.Type = 1;
                                    Redis.SetEntryInHash("IBS_ClassOtherID", ax.ClassNum + "_" + 1, classnum.ToJson());
                                }

                                if (ax.userID != null)
                                {
                                    ClassTchS ts = new ClassTchS();
                                    ts.TchID = long.Parse(ax.userID);
                                    ts.TchName = ax.TrueName ?? "";
                                    ts.SubjectID = ax.subjectID ?? 0;
                                    ts.SubjectName = ts.SubjectID > 0 ? StringEnumHelper.GetStringValue<SubjectEnum>(ts.SubjectID) : "";
                                    ts.UserImage = ax.UserImage ?? "00000000-0000-0000-0000-000000000000";
                                    ts.IsEnableOss = ax.IsEnableOss ?? 0;
                                    if (classinfo.ClassTchList.FirstOrDefault(a => a.TchID == Convert.ToInt32(ax.userID)) == null)
                                    {
                                        classinfo.ClassTchList.Add(ts);
                                    }

                                }
                                var listuser = list1.Where(a => a.ClassID == ax.ClassID);
                                {
                                    var userlist = listuser.ToList();
                                    if (userlist.Count > 0)
                                    {
                                        userlist.ForEach(x =>
                                        {
                                            if (x.UserType == (int)UserTypeEnum.Teacher)
                                            {
                                                ClassTchS ts = new ClassTchS();
                                                ts.TchID = Convert.ToInt32(x.UserID);
                                                ts.TchName = x.TrueName ?? "";
                                                ts.SubjectID = x.SubjectID ?? 0;
                                                ts.SubjectName = x.SubjectID > 0 ? StringEnumHelper.GetStringValue<SubjectEnum>(ts.SubjectID) : "";
                                                ts.UserImage = x.UserImage ?? "00000000-0000-0000-0000-000000000000";
                                                ts.IsEnableOss = (int)x.IsEnableOss;
                                                if (classinfo.ClassTchList.FirstOrDefault(a => a.TchID == Convert.ToInt32(x.UserID)) == null)
                                                {
                                                    classinfo.ClassTchList.Add(ts);
                                                }
                                            }
                                            else if (x.UserType == (int)UserTypeEnum.Student)
                                            {
                                                ClassStuS ss = new ClassStuS();
                                                ss.StuID = Convert.ToInt32(x.UserID);
                                                ss.StuName = x.TrueName ?? "";
                                                ss.UserImage = x.UserImage ?? "00000000-0000-0000-0000-000000000000";
                                                ss.IsEnableOss = (int)x.IsEnableOss;
                                                if (classinfo.ClassStuList.FirstOrDefault(a => a.StuID == Convert.ToInt32(x.UserID)) == null)
                                                {
                                                    classinfo.ClassStuList.Add(ss);
                                                }
                                            }
                                        });
                                    }


                                }
                                Redis.SetEntryInHash("IBS_ClassUserRelation", ax.ClassID.ToString().ToUpper(), classinfo.ToJson());
                            }
                            catch (Exception ex)
                            {
                                Log4Net.LogHelper.Error(ex, "同步班级信息异常！" + classid);

                            }

                        });
                        Log4Net.LogHelper.Info("IBS_ClassUserRelation写入成功!");
                        Redis.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步班级信息异常！");
            }
        }
        /// <summary>
        ///同步班学校数据到IBS
        /// </summary>
        public void MOD2IBSUserInfo()
        {
            Log4Net.LogHelper.Info("同步用户数据到IBS");

            try
            {
                string sql = @"SELECT b.UserID
                            ,a.UserNum
                            ,b.UserName
                            ,a.UserPwd
                            ,b.AppId,
                            case when b.IsEnableOss is null then 0
                            else b.IsEnableOss end IsEnableOss,
                            case when b.IsUser is null then 1
                            else b.IsUser end IsUser,
                            case when b.UserImage is null then '00000000-0000-0000-0000-000000000000'
                            else b.UserImage end UserImage,
                            case when b.UserRoles is null then 0
                            else b.UserRoles end UserRoles,
                            case when b.isLogState is null then '0'
                            else b.isLogState end isLogState
                            ,a.UserType
                            ,b.TrueName
                            ,a.SchoolID
                            ,a.SchoolName
                            ,b.TelePhone
                            ,b.CreateTime Regdate 
                            ,a.ClassID
                            ,a.ClassName
                            ,a.ClassNum
                            ,a.GradeID
                            ,a.SubjectID
                            ,a.SchID
                            ,a.SchName
                            ,a.AreaID
                            FROM dbo.[Tb_UserInfo] b 
                            left join MOD2IBS_tempUser a  on a.Userid=b.UserID ";
                DataSet ds = bm1.ExecuteSql(sql);

                List<MOD2IBS_TempStu> list = Extension.Convert2Object<MOD2IBS_TempStu>(ds.Tables[0]);
                Log4Net.LogHelper.Info("同步用户信息条数：" + list.Count);
                if (list.Count > 0)
                {
                    // 2.分页数据信息
                    int totalSize = list.Count; // 总记录数
                    int pageSize = 50000; // 每页N条
                    int totalPage = totalSize / pageSize; // 共N页

                    if (totalSize % pageSize != 0)
                    {
                        totalPage += 1;
                        if (totalSize < pageSize)
                        {
                            pageSize = list.Count;
                        }
                    }

                    // 临时List
                    for (int pageNum = 1; pageNum < totalPage + 1; pageNum++)
                    {
                        int starNum = (pageNum - 1) * pageSize;
                        int endNum = pageNum * pageSize > totalSize ? (totalSize) : pageNum * pageSize;
                        var temList = list.Skip(pageNum * pageSize).Take(pageSize).ToList();
                        using (var Redis = RedisOtherManager.GetClient(0))
                        {


                            temList.ForEach(ax =>
                            {
                                var userid = 0;
                                try
                                {
                                    userid = ax.UserID;
                                    IBS_UserInfo user ;
                                    string value = Redis.GetValueFromHash("IBS_UserInfo", ax.UserID.ToString());
                                    if (string.IsNullOrEmpty(value))
                                    {
                                        user = default(IBS_UserInfo);
                                    }
                                    else
                                    {
                                        user = value.toObject<IBS_UserInfo>();
                                    }
                                    if (user == null)
                                    {
                                        user = new IBS_UserInfo();
                                    }
                                    user.UserID = Convert.ToInt32(ax.UserID);
                                    user.TrueName = ax.TrueName;
                                    user.UserImage = ax.UserImage;
                                    user.UserName = ax.UserName;
                                    user.UserNum = ax.UserNum == null ? "" : ax.UserNum.ToString();
                                    user.UserPwd = ax.UserPwd;
                                    user.UserRoles = ax.UserRoles;
                                    user.UserType = ax.UserType ?? 0;
                                    user.TelePhone = ax.TelePhone;
                                    user.Regdate = ax.Regdate;
                                    user.IsUser = ax.IsUser;
                                    user.SchoolID = ax.SchoolID;
                                    user.SchoolName = ax.SchoolName;
                                    if(ax.SchoolID!=null&&ax.SchoolID>0)
                                    {
                                        user.SchoolID=ax.SchoolID;
                                        user.SchoolName=ax.SchoolName;
                                    }
                                    user.isLogState = ax.isLogState;
                                    user.IsEnableOss = ax.IsEnableOss;
                                    user.AppID = ax.AppID;

                                    //新增IBS_UserOtherID
                                    if (!string.IsNullOrEmpty(user.TelePhone))
                                    {
                                        //手机号
                                        IBS_UserOtherID telephone = new IBS_UserOtherID();
                                        telephone.UserIDOther = user.TelePhone;
                                        telephone.Type = 1;
                                        telephone.UserID = user.UserID;
                                        Redis.SetEntryInHash("IBS_UserOtherID", user.TelePhone + "_" + 1, telephone.ToJson());
                                    }
                                    if (!string.IsNullOrEmpty(user.UserName))
                                    {
                                        //账号
                                        IBS_UserOtherID userName = new IBS_UserOtherID();
                                        userName.UserIDOther = user.UserName;
                                        userName.Type = 2;
                                        userName.UserID = user.UserID;
                                        Redis.SetEntryInHash("IBS_UserOtherID", user.UserName + "_" + 2, userName.ToJson());

                                    }


                                    if (user.UserType == (int)UserTypeEnum.Teacher)
                                    {
                                        if (!string.IsNullOrEmpty(user.UserNum))
                                        {
                                            //账号
                                            IBS_UserOtherID teachInvm = new IBS_UserOtherID();
                                            teachInvm.UserIDOther = user.UserNum.ToString();
                                            teachInvm.Type = 3;
                                            teachInvm.UserID = user.UserID;
                                            Redis.SetEntryInHash("IBS_UserOtherID", user.UserNum + "_" + 3, teachInvm.ToJson());
                                        }
                                    }

                                    if (ax.ClassID != null && !string.IsNullOrEmpty(ax.ClassID.ToString()))
                                    {
                                        ClassSch sc = new ClassSch();
                                        sc.ClassID = ax.ClassID.ToString().ToUpper();
                                        sc.GradeID = ax.GradeID ?? 0;
                                        sc.SchID = ax.SchID ?? 0;
                                        sc.SubjectID = ax.SubjectID ?? 0;
                                        sc.AreaID = ax.AreaID ?? 0;
                                        if (user.ClassSchList.FirstOrDefault(a => a.ClassID.ToUpper() == sc.ClassID.ToUpper()) == null)
                                        {
                                            user.ClassSchList.Add(sc);
                                        }

                                    }
                                    Redis.SetEntryInHash("IBS_UserInfo", ax.UserID.ToString(), user.ToJson());
                                }
                                catch (Exception ex)
                                {
                                    Log4Net.LogHelper.Error(ex, "同步用户信息异常！" + userid);
                                }



                            });
                            Log4Net.LogHelper.Info("第" + pageNum + "次用户信息同步完成！一共写入" + temList.Count + "条");
                            Redis.Dispose();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Log4Net.LogHelper.Error(ex, "同步用户信息异常！");
            }
        }


        static IBSUserDAL userDAL = new IBSUserDAL();
        /// <summary>
        /// IBS修复redis用户表名称为空的
        /// </summary>
        public void IBSRepairTrueName()
        {
            try
            {
                string sql = String.Format("SELECT UserID,TrueName FROM modUser_Temp WHERE TrueName IS NOT NULL AND TrueName <>''");
                DataSet ds = bm1.ExecuteSql(sql);
                List<MOD_User> list = Extension.Convert2Object<MOD_User>(ds.Tables[0]);
                Log4Net.LogHelper.Info("IBS修复条数：" + list.Count);
                list.ForEach(a =>
                {
                    using (var Redis = RedisOtherManager.GetClient(0))
                    {
                        IBS_UserInfo user;
                        string value = Redis.GetValueFromHash("IBS_UserInfo", a.UserID.ToString());
                        if (string.IsNullOrEmpty(value))
                        {
                            user = default(IBS_UserInfo);
                        }
                        else
                        {
                            user = value.toObject<IBS_UserInfo>();
                        }
                        if (user != null)
                        {
                            user.TrueName = a.TrueName;
                            Redis.SetEntryInHash("IBS_UserInfo", a.UserID.ToString(), user.ToJson());
                        }
                        //DB操作
                        var tbUser = userDAL.SelectSearch(x => x.UserID == Convert.ToInt32(a.UserID)).FirstOrDefault();

                        if (tbUser != null)
                        {
                            if (!tbUser.TrueName.Equals(a.TrueName))
                            {
                                tbUser.NickName = a.TrueName;
                                tbUser.TrueName = a.TrueName;
                                userDAL.Update(tbUser);
                            }

                        }
                       
                    }
                });
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "修复异常！");
            }

        }

        public void IBSRepairClassUserTrueName()
        {
            try
            {
                var classUser = hashRedis.GetAll<IBS_ClassUserRelation>("IBS_ClassUserRelation");
                classUser.ForEach(a =>
                {
                    a.ClassStuList.ForEach(x =>
                    {
                        if (string.IsNullOrEmpty(x.StuName))
                        {
                            var user = userinfoBLL.GetUserInfoByUserId(Convert.ToInt32(x.StuID));
                            if (user != null)
                            {
                                if (!user.TrueName.Equals("暂未填写"))
                                {
                                    x.StuName = user.TrueName;
                                }
                            }
                        }
                    });

                    a.ClassTchList.ForEach(y =>
                    {
                        if (string.IsNullOrEmpty(y.TchName))
                        {
                            var user = userinfoBLL.GetUserInfoByUserId(Convert.ToInt32(y.TchID));
                            if (user != null)
                            {
                                if (!user.TrueName.Equals("暂未填写"))
                                {
                                    y.TchName = user.TrueName;
                                }
                            }
                        }
                    });
                    hashRedis.Set<IBS_ClassUserRelation>("IBS_ClassUserRelation", a.ClassID.ToUpper(), a);
                });
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "修复班级信息异常！");
            }
            
        }


    }
}
