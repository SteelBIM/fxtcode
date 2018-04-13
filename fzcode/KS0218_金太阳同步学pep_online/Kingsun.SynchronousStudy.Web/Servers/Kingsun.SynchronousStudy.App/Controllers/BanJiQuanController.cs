using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class BanJiQuanController : ApiController
    {
        readonly BaseManagement _bm = new BaseManagement();
        string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

        IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();

        /// <summary>
        /// 根据用户ID获取用户信息、用户班级列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserInfo([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            UserInfo submitData = JsonHelper.DecodeJson<UserInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为空");
            }

            //根据邀请码查询用户全部信息
            var user = userBLL.GetUserALLInfoByUserOtherID(submitData.UserID, 3);

            //如果邀请码获取不到，通过手机号获取
            if (user == null)
            {
                user = userBLL.GetUserALLInfoByUserOtherID(submitData.UserID, 1);
            }

            if (user != null)
            {
                    List<string> classlist = new List<string>();
                    List<ClassInfo> returnlist = new List<ClassInfo>();
                    user.ClassSchDetailList.ForEach(a =>
                    {
                        if (a.SubjectID == (int)SubjectEnum.English)
                        {
                            classlist.Add(a.ClassID);
                        }
                    });

                    UserInfo returnuser = new UserInfo();
                    returnuser.UserID = user.iBS_UserInfo.UserID.ToString();
                    returnuser.UserName = string.IsNullOrEmpty(user.iBS_UserInfo.TrueName)?"暂未填写":user.iBS_UserInfo.UserName;
                    returnuser.TrueName = string.IsNullOrEmpty(user.iBS_UserInfo.TrueName) ? "暂未填写" : user.iBS_UserInfo.TrueName;
                    if (submitData.IsEnableOss == 0)
                    {
                        returnuser.HeadImage = _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(user.iBS_UserInfo.UserImage)? "00000000-0000-0000-0000-000000000000":user.iBS_UserInfo.UserImage);
                    }
                    else
                    {
                        returnuser.HeadImage = user.iBS_UserInfo.IsEnableOss != 0 ? _getOssFilesUrl + user.iBS_UserInfo.UserImage : _getFilesUrl + "?FileID=" + user.iBS_UserInfo.UserImage;
                    }

                    classlist.ForEach(a =>
                    {
                        string cID = a;
                        var clas = classBLL.GetClassUserRelationByClassId(cID);
                        if (clas != null)
                        {
                            ClassInfo classInfo = new ClassInfo();
                            classInfo.StudentNum = 0;
                            classInfo.ID = cID;
                            classInfo.ClassNum = clas.ClassNum.ToString();
                            classInfo.ClassName = clas.ClassName;
                            classInfo.SchoolID = clas.SchID;
                            returnlist.Add(classInfo);
                        }

                    });
                    object obj = new { User = returnuser, ClassList = returnlist };
                    return ObjectToJson.GetResult(obj, "");
            }
            else
            {

                UserInfo reuser = new UserInfo();
                List<ClassInfo> returnList = new List<ClassInfo>();
                //可能传入的非邀请码而是ClassNum
                var classinfo = classBLL.GetClassUserRelationByClassOtherId(submitData.UserID, 1);
                if (classinfo != null)
                {
                    var clastch = classinfo.ClassTchList.FirstOrDefault(a => a.SubjectID == 3);
                    if (clastch != null)
                    {

                        reuser.UserID = clastch.TchID.ToString();
                        reuser.TrueName = clastch.TchName;
                        reuser.HeadImage = clastch.IsEnableOss != 0 ? _getOssFilesUrl + clastch.UserImage : _getFilesUrl + "?FileID=" + clastch.UserImage;
                    }
                    ClassInfo reclass = new ClassInfo();
                    reclass.StudentNum = 0;
                    reclass.ID = classinfo.ClassID;
                    reclass.ClassNum = classinfo.ClassNum;
                    reclass.ClassName = classinfo.ClassName;
                    reclass.SchoolID = classinfo.SchID;
                    returnList.Add(reclass);
                    object obj = new { User = reuser, ClassList = returnList };
                    return ObjectToJson.GetResult(obj, "");
                }
                else
                {
                    return ObjectToJson.GetErrorResult("邀请码错误！");
                }

            }
            return ObjectToJson.GetResult("");
        }

        /// <summary>
        /// 根据用户ID获取用户信息、用户班级列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserInfoTest()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            UserInfo submitData = new UserInfo();//JsonHelper.DecodeJson<UserInfo>(request.Data);
            //if (submitData == null)
            //{
            //    return ObjectToJson.GetErrorResult("当前信息为空");
            //}
            //if (string.IsNullOrEmpty(submitData.UserID))
            //{
            //    return ObjectToJson.GetErrorResult("用户ID不能为空");
            //}
            submitData.UserID = "15112369730";
            submitData.IsEnableOss = 1;
            //根据邀请码查询用户全部信息
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为空");
            }

            //根据邀请码查询用户全部信息
            var user = userBLL.GetUserALLInfoByUserOtherID(submitData.UserID, 3);

            //如果邀请码获取不到，通过手机号获取
            if (user == null)
            {
                user = userBLL.GetUserALLInfoByUserOtherID(submitData.UserID, 1);
            }

            if (user != null)
            {
                
                    List<string> classlist = new List<string>();
                    List<ClassInfo> returnlist = new List<ClassInfo>();
                    user.ClassSchDetailList.ForEach(a =>
                    {
                       classlist.Add(a.ClassID);
                    });

                    UserInfo returnuser = new UserInfo();
                    returnuser.UserID = user.iBS_UserInfo.UserID.ToString();
                    returnuser.UserName = string.IsNullOrEmpty(user.iBS_UserInfo.TrueName) ? "暂未填写" : user.iBS_UserInfo.UserName;
                    returnuser.TrueName = string.IsNullOrEmpty(user.iBS_UserInfo.TrueName) ? "暂未填写" : user.iBS_UserInfo.TrueName;
                    if (string.IsNullOrEmpty(user.iBS_UserInfo.TrueName))
                    {
                        returnuser.UserName = "暂未填写";
                    }
                    if (submitData.IsEnableOss == 0)
                    {
                        returnuser.HeadImage = _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(user.iBS_UserInfo.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.iBS_UserInfo.UserImage);
                    }
                    else
                    {
                        returnuser.HeadImage = user.iBS_UserInfo.IsEnableOss != 0 ? _getOssFilesUrl + user.iBS_UserInfo.UserImage : _getFilesUrl + "?FileID=" + user.iBS_UserInfo.UserImage;
                    }

                    classlist.ForEach(a =>
                    {
                        string cID = a;
                        var clas = classBLL.GetClassUserRelationByClassId(cID);
                        if (clas != null)
                        {
                            ClassInfo classInfo = new ClassInfo();
                            classInfo.StudentNum = 0;
                            classInfo.ID = cID;
                            classInfo.ClassNum = clas.ClassNum.ToString();
                            classInfo.ClassName = clas.ClassName;
                            classInfo.SchoolID = clas.SchID;
                            returnlist.Add(classInfo);
                        }

                    });
                    object obj = new { User = returnuser, ClassList = returnlist };
                    return ObjectToJson.GetResult(obj, "");
            }
            else
            {
                UserInfo reuser = new UserInfo();
                List<ClassInfo> returnList = new List<ClassInfo>();
                //可能传入的非邀请码而是ClassNum
                var classinfo = classBLL.GetClassUserRelationByClassOtherId(submitData.UserID, 1);
                if (classinfo != null)
                {
                    var clastch = classinfo.ClassTchList.FirstOrDefault(a => a.SubjectID == 3);
                    if (clastch != null)
                    {

                        reuser.UserID = clastch.TchID.ToString();
                        reuser.TrueName = string.IsNullOrEmpty(clastch.TchName) ? "暂未填写" : clastch.TchName;
                        reuser.HeadImage = clastch.IsEnableOss != 0 ? _getOssFilesUrl + clastch.UserImage : _getFilesUrl + "?FileID=" + clastch.UserImage;
                    }
                    ClassInfo reclass = new ClassInfo();
                    reclass.StudentNum = 0;
                    reclass.ID = classinfo.ClassID;
                    reclass.ClassNum = classinfo.ClassNum.ToString();
                    reclass.ClassName = classinfo.ClassName;
                    reclass.SchoolID = classinfo.SchID;
                    returnList.Add(reclass);
                    object obj = new { User = reuser, ClassList = returnList };
                    return ObjectToJson.GetResult(obj, "");
                }
                else
                {
                    return ObjectToJson.GetErrorResult("邀请码错误！");
                }

            }
            return ObjectToJson.GetResult("");
        }

        /// <summary>
        /// 学生绑定班级
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage StudentAddClass([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            AddInfo submitData = JsonHelper.DecodeJson<AddInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.StudentID) || string.IsNullOrEmpty(submitData.ClassID))
            {
                return ObjectToJson.GetErrorResult("学生ID、班级ID不能为空");
            }

            if (!string.IsNullOrEmpty(submitData.TrueName))
            {
                try
                {
                    var result = userBLL.AppUpdateUserInfo("", Convert.ToInt32(submitData.StudentID), submitData.TrueName);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error("修改用户信息失败！");
                }
            }

            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.StudentID));
            if (user != null)
            {
                if (user.ClassSchList.Count > 0 && user.ClassSchList != null)
                {
                    var clss = user.ClassSchList.FirstOrDefault();

                    if (clss != null)
                    {
                        UserClassData data = new UserClassData();
                        data.ClassID = clss.ClassID;
                        data.UserID = Convert.ToInt32(submitData.StudentID);
                        data.UserType = UserTypeEnum.Student;
                        var result = classBLL.UnBindClass(data);
                        if (result != null)
                        {
                            if (result.Success)
                            {
                                UserClassData data1 = new UserClassData();
                                data1.ClassID = submitData.ClassID;
                                data1.UserID = Convert.ToInt32(submitData.StudentID);
                                data1.UserType = UserTypeEnum.Student;
                                data1.Type = ModRelationTypeEnum.StuClass;
                                data1.flag = BCPointEnum.Cpoint;
                                var res = classBLL.AddUserToClass(data1);
                                if (res == null || !res.Success)
                                {
                                    ////return ObjectToJson.GetErrorResult("绑定失败！" + res.ErrorMsg);
                                    return ObjectToJson.GetErrorResult("加入失败，请重试！");
                                }
                            }
                            else
                            {
                                return ObjectToJson.GetResult("解除绑定失败！" + result.ErrorMsg);
                            }

                            return ObjectToJson.GetResult("");
                        }
                        else
                        {
                            return ObjectToJson.GetErrorResult("解绑失败！");
                        }
                    }

                }
                else
                {
                    UserClassData data1 = new UserClassData();
                    data1.ClassID = submitData.ClassID;
                    data1.UserID = Convert.ToInt32(submitData.StudentID);
                    data1.UserType = UserTypeEnum.Student;
                    data1.Type = ModRelationTypeEnum.StuClass;
                    data1.flag = BCPointEnum.Cpoint;
                    var res = classBLL.AddUserToClass(data1);
                    Log4Net.LogHelper.Info("绑定：UserID=" + data1.UserID + ",ClassID=" + data1.ClassID + ",Type=" + (int)data1.Type);
                    if (res == null || !res.Success)
                    {
                        return ObjectToJson.GetErrorResult("绑定失败！" + res.ErrorMsg);

                    }
                }
                return ObjectToJson.GetResult("");

            }
            else
            {
                return ObjectToJson.GetErrorResult("学生信息不存在");
            }

        }

        /// <summary>
        /// 通过班级ID获取学生列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetStuList([FromBody]KingRequest request)
        {
            AddInfo submitData = JsonHelper.DecodeJson<AddInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.StudentID))
            {
                return ObjectToJson.GetErrorResult("学生ID不能为空");
            }

            var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(submitData.StudentID));
            if (user != null)
            {
                //NickName trueName
                ReturnInfo ri = new ReturnInfo();



                if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                {
                    var tc = user.ClassSchDetailList.FirstOrDefault();
                    if (tc != null)
                    {
                        ri.ClassName = tc.ClassName;
                        var stulist = classBLL.GetClassUserRelationALLInfoByClassId(tc.ClassID);
                        if (stulist != null)
                        {
                            var teainfo = stulist.iBS_ClassUserRelation.ClassTchList.Where(a => a.SubjectID == 3).FirstOrDefault();
                            if (teainfo != null)
                            {
                                ri.TeaHeadImage = teainfo.UserImage;
                                ri.TeaID = teainfo.TchID.ToString();
                                ri.TeaName = string.IsNullOrEmpty(teainfo.TchName)?"暂未填写":teainfo.TchName;
                                ri.TeaNickName = string.IsNullOrEmpty(teainfo.TchName) ? "暂未填写" : teainfo.TchName;
                                ri.TeaTrueName = string.IsNullOrEmpty(teainfo.TchName) ? "暂未填写" : teainfo.TchName;
                                if (teainfo.IsEnableOss == 0)
                                {
                                    ri.TeaHeadImage = _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(teainfo.UserImage)? "00000000-0000-0000-0000-000000000000":teainfo.UserImage);
                                }
                                else
                                {
                                    ri.TeaHeadImage = teainfo.IsEnableOss != 0 ? _getOssFilesUrl + teainfo.UserImage
                                                    : _getFilesUrl + "?FileID=" + teainfo.UserImage;
                                }
                            }

                            List<StuList> stu = new List<StuList>();
                            if (stulist.iBS_ClassUserRelation.ClassStuList != null && stulist.iBS_ClassUserRelation.ClassStuList.Count > 0)
                            {
                                stulist.iBS_ClassUserRelation.ClassStuList.ForEach(a =>
                                {
                                    string UserName = "";
                                    var stuUser=userBLL.GetUserInfoByUserId(a.StuID.ToIntOrZero());
                                    if (stuUser != null)
                                    {
                                        UserName = string.IsNullOrEmpty(stuUser.UserName) ? "暂未填写" : stuUser.UserName;
                                    }
                                    string Image = "";
                                    if (a.IsEnableOss == 0)
                                    {
                                        Image = _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(a.UserImage) ? "00000000-0000-0000-0000-000000000000":a.UserImage);
                                    }
                                    else
                                    {
                                        Image = a.IsEnableOss != 0 ? _getOssFilesUrl + a.UserImage
                                                        : _getFilesUrl + "?FileID=" + a.UserImage;
                                    }
                                    stu.Add(new StuList
                                    {


                                        HeadImage = Image,
                                        NickName = string.IsNullOrEmpty(a.StuName) ? "暂未填写" : a.StuName,
                                        UserID = a.StuID.ToString(),
                                        UserName = string.IsNullOrEmpty(a.StuName) ? "暂未填写" : UserName,
                                        TrueName = string.IsNullOrEmpty(a.StuName) ? "暂未填写" : a.StuName
                                    });
                                });
                                ri.StuList = stu.ToArray();
                                return ObjectToJson.GetResult(ri, "");
                            }
                        }
                        return ObjectToJson.GetResult(ri, "");
                    }

                }
                return ObjectToJson.GetResult(ri, "");
            }
            else
            {
                return ObjectToJson.GetErrorResult("找不到用户");
            }
        }

        /// <summary>
        /// 通过班级ID获取学生列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetStuListTest()
        {
            AddInfo submitData = new AddInfo();//JsonHelper.DecodeJson<AddInfo>(request.Data);
            submitData.StudentID = "1817301596";
            submitData.IsEnableOss = 1;
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.StudentID))
            {
                return ObjectToJson.GetErrorResult("学生ID不能为空");
            }
            var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(submitData.StudentID));
            if (user != null)
            {

                //NickName trueName
                ReturnInfo ri = new ReturnInfo();
                ri.TeaHeadImage = user.iBS_UserInfo.UserImage;
                ri.TeaID = user.iBS_UserInfo.UserID.ToString();
                ri.TeaName = user.iBS_UserInfo.TrueName;
                ri.TeaNickName = user.iBS_UserInfo.TrueName;
                ri.TeaTrueName = user.iBS_UserInfo.TrueName;
                if (submitData.IsEnableOss == 0)
                {
                    ri.TeaHeadImage = user.iBS_UserInfo.UserImage;
                }
                else
                {
                    ri.TeaHeadImage = user.iBS_UserInfo.IsEnableOss != 0 ? _getOssFilesUrl + user.iBS_UserInfo.UserImage
                                    : _getFilesUrl + "?FileID=" + user.iBS_UserInfo.UserImage;
                }

                if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                {
                    var tc = user.ClassSchDetailList.FirstOrDefault();
                    if (tc != null)
                    {
                        ri.ClassName = tc.ClassName;
                        var stulist = classBLL.GetClassUserRelationALLInfoByClassId(tc.ClassID);
                        List<StuList> stu = new List<StuList>();
                        if (stulist.iBS_ClassUserRelation.ClassStuList != null)
                        {
                            stulist.iBS_ClassUserRelation.ClassStuList.ForEach(a =>
                            {
                                stu.Add(new StuList
                                {
                                    HeadImage = a.IsEnableOss == 0 ? a.UserImage : _getOssFilesUrl + a.UserImage,
                                    NickName = a.StuName,
                                    UserID = a.StuID.ToString(),
                                    UserName = a.StuName,
                                    TrueName = a.StuName
                                });
                            });
                            ri.StuList = stu.ToArray();
                            return ObjectToJson.GetResult(ri, "");
                        }
                        else
                        {
                            return ObjectToJson.GetErrorResult("班级信息为空");
                        }

                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("学生未绑定班级");
                    }

                }
                else
                {
                    return ObjectToJson.GetErrorResult("学生未绑定班级");
                }
            }
            else
            {
                return ObjectToJson.GetErrorResult("找不到用户");
            }
        }

        /// <summary>
        /// 通过用户ID获取用户是否是第一次登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetIsLogState([FromBody]KingRequest request)
        {
            Tb_UserInfo submitData = JsonHelper.DecodeJson<Tb_UserInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID.ToString()))
            {
                return ObjectToJson.GetErrorResult("学生ID不能为空");
            }
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
            if (user != null)
            {
                UType tp = new UType();
                tp.IsLogState = user.isLogState;
                tp.UserType = user.UserType.ToString();
                if (tp.UserType == ((int)UserTypeEnum.Teacher).ToString())
                {
                    if (user.ClassSchList != null)
                    {
                        tp.ClassNum = user.ClassSchList.Count();
                    }
                    else
                    {
                        tp.ClassNum = 0;
                    }

                }
                switch (user.isLogState)
                {
                    case "0":
                        tp.IsLogState = "0";
                        return ObjectToJson.GetResult(tp);
                    case "1":
                        tp.IsLogState = "1";
                        return ObjectToJson.GetResult(tp);

                    default:
                        return ObjectToJson.GetErrorResult("班级信息为空");
                }

            }
            else
            {
                return ObjectToJson.GetErrorResult("班级信息为空");
            }
        }

        /// <summary>
        /// 通过用户ID获取用户是否是第一次登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetIsLogStateTest()
        {
            string s = "{\"UserID\":\"1056759904\"}";
            Tb_UserInfo submitData = JsonHelper.DecodeJson<Tb_UserInfo>(s);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID.ToString()))
            {
                return ObjectToJson.GetErrorResult("学生ID不能为空");
            }
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
            if (user != null)
            {
                UType tp = new UType();
                tp.IsLogState = user.isLogState;
                tp.UserType = user.UserType.ToString();
                if (tp.UserType == ((int)UserTypeEnum.Teacher).ToString())
                {
                    if (user.ClassSchList != null)
                    {
                        tp.ClassNum = user.ClassSchList.Count();
                    }
                    else
                    {
                        tp.ClassNum = 0;
                    }

                }
                switch (user.isLogState)
                {
                    case "0":
                        tp.IsLogState = "0";
                        return ObjectToJson.GetResult(tp);
                    case "1":
                        tp.IsLogState = "1";
                        return ObjectToJson.GetResult(tp);

                    default:
                        return ObjectToJson.GetErrorResult("班级信息为空");
                }

            }
            else
            {
                return ObjectToJson.GetErrorResult("班级信息为空");
            }
        }
        /// <summary>
        /// 修改用户登录状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateIsLogState([FromBody]KingRequest request)
        {
            Tb_UserInfo submitData = JsonHelper.DecodeJson<Tb_UserInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID.ToString()))
            {
                return ObjectToJson.GetErrorResult("学生ID不能为空");
            }

            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
            if (user != null)
            {
                user.isLogState = "1";
                TBX_UserInfo db = new TBX_UserInfo();
                db.iBS_UserInfo = user;
                user.UserType = (int)UserTypeEnum.Student;

                var result = userBLL.UpdateUserInfoNoOnly(AppID, user);
                if (result.Success)
                {
                    return ObjectToJson.GetResult("信息修改成功");
                }
                else
                {
                    return ObjectToJson.GetErrorResult("身份修改失败!" + result.ErrorMsg);
                }

            }
            else
            {
                return ObjectToJson.GetErrorResult("学生信息为空");
            }

        }

        /// <summary>
        /// 修改用户登录状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public void UpdateIsLogStateTest()
        {
            Tb_UserInfo submitData=new Tb_UserInfo();
            submitData.TrueName = "何明11221";
            submitData.TelePhone = "13760350119";
            var user = userBLL.GetUserInfoByPhone(submitData.TelePhone, submitData.TrueName);
           
        }

        /// <summary>
        /// 通过班级ID查询班级下的所有学生
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        //        public static List<UInfo> GetStuListByClassShortID(string useridList, int IsEnableOss)
        //        {
        //            IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        //            string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        //            string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

        //            List<UInfo> StuList = new List<UInfo>();
        //            string sql = string.Format(@" SELECT UserID ,
        //                                                UserName ,
        //                                                NickName ,
        //                                                UserImage ,
        //                                                TrueName ,
        //                                                TelePhone,
        //                                                IsEnableOss,
        //                                                isLogState,
        //                                                IsUser
        //                                        FROM tb_userinfo WHERE UserID IN ({0})  AND IsUser=1 ", useridList);

        //            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
        //            if (ds.Tables.Count == 0) { return null; }

        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    UInfo ui = new UInfo();
        //                    ui.UserID = ds.Tables[0].Rows[i]["UserID"].ToString();
        //                    string UIMG = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UserImage"].ToString())
        //                         ? "00000000-0000-0000-0000-000000000000"
        //                         : ds.Tables[0].Rows[i]["UserImage"].ToString();
        //                    if (IsEnableOss == 0)
        //                    {
        //                        ui.UserImg = UIMG;
        //                    }
        //                    else
        //                    {
        //                        ui.UserImg = ds.Tables[0].Rows[i]["IsEnableOss"].ToString() != "0" ? _getOssFilesUrl + UIMG : _getFilesUrl + "?FileID=" + UIMG;
        //                    }
        //                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TrueName"].ToString()))
        //                    {
        //                        ui.UserName = ds.Tables[0].Rows[i]["TrueName"].ToString();
        //                    }
        //                    else
        //                    {
        //                        ui.UserName = "暂未填写";
        //                    }
        //                    ui.IsEnableOss = Convert.ToInt32(ds.Tables[0].Rows[i]["IsEnableOss"].ToString());
        //                    StuList.Add(ui);
        //                }
        //            }

        //            return StuList;
        //        }
    }

    public class UserInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
        public string HeadImage { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class UType
    {
        public int ClassNum { get; set; }
        public string UserType { get; set; }
        public string IsLogState { get; set; }
    }

    //班级信息
    public class ClassInfo
    {
        public string ID { get; set; }
        public string ClassNum { get; set; }
        public string ClassName { get; set; }
        public int StudentNum { get; set; }
        public int? SchoolID { get; set; }
    }

    public class AddInfo
    {
        public string StudentID { get; set; }
        public string ClassID { get; set; }
        public int IsEnableOss { get; set; }
        public string TrueName { get; set; }
    }

    public class ReturnInfo
    {
        public string TeaID { get; set; }
        public string TeaName { get; set; }
        public string TeaNickName { get; set; }
        public string TeaTrueName { get; set; }
        public string TeaHeadImage { get; set; }
        public string ClassName { get; set; }
        // public List<UserInfo> StuList { get; set; }
        public StuList[] StuList { get; set; }
    }

    public class Data
    {
        public TeaInfo TeaInfo { get; set; }
        public StuList[] StuList { get; set; }
    }

    public class TeaInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
        public string HeadImage { get; set; }
        public string ClassName { get; set; }
        public string SchoolID { get; set; }
        public string SchoolName { get; set; }

    }

    public class StuList
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
        public string HeadImage { get; set; }
    }
}
