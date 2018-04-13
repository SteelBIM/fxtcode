using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using log4net;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonalCenterController : ApiController
    {
        private PersonalCenterBLL pc = new PersonalCenterBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();
        string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();
        /// <summary>
        /// 根据地区ID获取对应地区的学校信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSchoolInfo([FromBody] KingRequest request)
        {
            ReceiveParameter submitData = JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            if (submitData == null)
            {
                JsonHelper.GetErrorResult(101, ErrorMsgCode.ErrorDic[101]);//"参数错误！"
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Version))
            {
                JsonHelper.GetErrorResult(116, ErrorMsgCode.ErrorDic[116]);//"版本号不能为空！");
            }
            if (submitData != null && submitData.AreaID <= 0)
            {
                JsonHelper.GetErrorResult(102, ErrorMsgCode.ErrorDic[102]);//"地区ID不能为空！");
            }
            HttpResponseMessage htm = new HttpResponseMessage();
            switch (submitData.Version)
            {
                case "V1":
                    var areaInfo = areaBLL.GetAreaSchRelationByAreaId(submitData.AreaID);
                    List<SchoolInfo> sc = new List<SchoolInfo>();
                    areaInfo.AreaSchList.ForEach(a =>
                    {
                        SchoolInfo sch = new SchoolInfo();
                        sch.ID = a.SchD;
                        sch.SchoolName = a.SchName;
                        sch.DistrictID = submitData.AreaID;
                        sc.Add(sch);
                    });
                    htm = JsonHelper.GetResult(sc);
                    break;
                default:
                    htm = JsonHelper.GetErrorResult(115, ErrorMsgCode.ErrorDic[115]);//"所在地区没有学校信息！"
                    break;
            }
            return htm;
        }

        /// <summary>
        /// 添加教师信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddTeacherInfo([FromBody] KingRequest request)
        {
            ReceiveParameter submitData = JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            #region 参数验证
            if (submitData == null)
            {
                JsonHelper.GetErrorResult(101, ErrorMsgCode.ErrorDic[101]);//"参数错误！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Version))
            {
                JsonHelper.GetErrorResult(116, ErrorMsgCode.ErrorDic[116]);//"版本号不能为空！");
            }
            if (submitData != null && submitData.UserID <= 0)
            {
                JsonHelper.GetErrorResult(104, ErrorMsgCode.ErrorDic[104]);//"用户ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.UserName))
            {
                JsonHelper.GetErrorResult(105, ErrorMsgCode.ErrorDic[105]);//"用户名不能为空！");
            }
            if (submitData != null && submitData.AreaID <= 0)
            {
                JsonHelper.GetErrorResult(102, ErrorMsgCode.ErrorDic[102]);//"地区ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Area))
            {
                JsonHelper.GetErrorResult(103, ErrorMsgCode.ErrorDic[103]);//"地区不能为空！");
            }
            if (submitData != null && submitData.CityID <= 0)
            {
                JsonHelper.GetErrorResult(106, ErrorMsgCode.ErrorDic[106]);//"城市ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.City))
            {
                JsonHelper.GetErrorResult(107, ErrorMsgCode.ErrorDic[107]);//"城市不能为空！");
            }
            if (submitData != null && submitData.ProvinceID <= 0)
            {
                JsonHelper.GetErrorResult(108, ErrorMsgCode.ErrorDic[108]);//"省份ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Province))
            {
                JsonHelper.GetErrorResult(109, ErrorMsgCode.ErrorDic[109]);//"省份不能为空！");
            }
            if (submitData != null && submitData.SubjectID <= 0)
            {
                JsonHelper.GetErrorResult(110, ErrorMsgCode.ErrorDic[110]);//"学科ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Subject))
            {
                JsonHelper.GetErrorResult(111, ErrorMsgCode.ErrorDic[111]);//"学科不能为空！");
            }
            if (submitData != null && submitData.SchoolID <= 0)
            {
                JsonHelper.GetErrorResult(112, ErrorMsgCode.ErrorDic[112]);//"学校ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.SchoolName))
            {
                JsonHelper.GetErrorResult(113, ErrorMsgCode.ErrorDic[113]);//"学校不能为空！");
            }
            #endregion

            TBX_UserInfo info = new TBX_UserInfo
            {

                ClassSchDetailList = new List<ClassSchDetail>()
                {
                    new ClassSchDetail(){
                            AreaID=submitData.AreaID,
                            SchID=submitData.SchoolID,
                            SubjectID=submitData.SubjectID,
                            SchName=submitData.SchoolName,
                            SubjectName=submitData.Subject,
                        }
                },
                iBS_UserInfo = new IBS_UserInfo()
                {
                    UserID = submitData.UserID,
                    UserName = submitData.UserName,
                    SchoolID = submitData.SchoolID,
                    SchoolName = submitData.SchoolName
                },
                ProvinceID = submitData.ProvinceID,
                Province = submitData.Province,
                CityID = submitData.CityID,
                City = submitData.City,
            };
            HttpResponseMessage htm;
            switch (submitData.Version)
            {
                case "V1":
                    htm = pc.AddTeacherInfo(info, AppID);
                    break;
                default:
                    htm = JsonHelper.GetErrorResult(201, ErrorMsgCode.ErrorDic[201]);//"信息不存在！");
                    break;
            }
            return htm;
        }



        /// <summary>
        /// 添加教师信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AddTeacherInfoTest()
        {
            string s = "{\"UserID\":\"1056759904\",\"UserName\":\"信逗的\",\"ProvinceID\":\"110000000\",\"Province\":\"北京市\",\"CityID\":\"110100000\",\"City\":\"市辖区\",\"AreaID\":\"110101000\",\"Area\":\"东城区\",\"SubjectID\":\"3\",\"Subject\":\"英语\",\"SchoolID\":\"73628\",\"SchoolName\":\"测试学校\",\"Version\":\"V1\"}";
            ReceiveParameter submitData = JsonHelper.DecodeJson<ReceiveParameter>(s);
            #region 参数验证
            if (submitData == null)
            {
                JsonHelper.GetErrorResult(101, ErrorMsgCode.ErrorDic[101]);//"参数错误！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Version))
            {
                JsonHelper.GetErrorResult(116, ErrorMsgCode.ErrorDic[116]);//"版本号不能为空！");
            }
            if (submitData != null && submitData.UserID <= 0)
            {
                JsonHelper.GetErrorResult(104, ErrorMsgCode.ErrorDic[104]);//"用户ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.UserName))
            {
                JsonHelper.GetErrorResult(105, ErrorMsgCode.ErrorDic[105]);//"用户名不能为空！");
            }
            if (submitData != null && submitData.AreaID <= 0)
            {
                JsonHelper.GetErrorResult(102, ErrorMsgCode.ErrorDic[102]);//"地区ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Area))
            {
                JsonHelper.GetErrorResult(103, ErrorMsgCode.ErrorDic[103]);//"地区不能为空！");
            }
            if (submitData != null && submitData.CityID <= 0)
            {
                JsonHelper.GetErrorResult(106, ErrorMsgCode.ErrorDic[106]);//"城市ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.City))
            {
                JsonHelper.GetErrorResult(107, ErrorMsgCode.ErrorDic[107]);//"城市不能为空！");
            }
            if (submitData != null && submitData.ProvinceID <= 0)
            {
                JsonHelper.GetErrorResult(108, ErrorMsgCode.ErrorDic[108]);//"省份ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Province))
            {
                JsonHelper.GetErrorResult(109, ErrorMsgCode.ErrorDic[109]);//"省份不能为空！");
            }
            if (submitData != null && submitData.SubjectID <= 0)
            {
                JsonHelper.GetErrorResult(110, ErrorMsgCode.ErrorDic[110]);//"学科ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Subject))
            {
                JsonHelper.GetErrorResult(111, ErrorMsgCode.ErrorDic[111]);//"学科不能为空！");
            }
            if (submitData != null && submitData.SchoolID <= 0)
            {
                JsonHelper.GetErrorResult(112, ErrorMsgCode.ErrorDic[112]);//"学校ID不能为空！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.SchoolName))
            {
                JsonHelper.GetErrorResult(113, ErrorMsgCode.ErrorDic[113]);//"学校不能为空！");
            }
            #endregion

            TBX_UserInfo info = new TBX_UserInfo
            {

                ClassSchDetailList = new List<ClassSchDetail>()
                {
                    new ClassSchDetail(){
                            AreaID=submitData.AreaID,
                            SchID=submitData.SchoolID,
                            SubjectID=submitData.SubjectID,
                            SchName=submitData.SchoolName,
                            SubjectName=submitData.Subject,
                        }
                },
                iBS_UserInfo = new IBS_UserInfo()
                {
                    UserID = submitData.UserID,
                    UserName = submitData.UserName,
                    SchoolID = submitData.SchoolID,
                    SchoolName = submitData.SchoolName
                },
                ProvinceID = submitData.ProvinceID,
                Province = submitData.Province,
                CityID = submitData.CityID,
                City = submitData.City,
            };
            HttpResponseMessage htm;
            switch (submitData.Version)
            {
                case "V1":
                    htm = pc.AddTeacherInfo(info, AppID);
                    break;
                default:
                    htm = JsonHelper.GetErrorResult(201, ErrorMsgCode.ErrorDic[201]);//"信息不存在！");
                    break;
            }
            return htm;
        }

        /// <summary>
        /// 获取个人中心订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetOrderListByUserId([FromBody] KingRequest request)
        {
            ReceiveParameter submitData = JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            if (submitData == null)
            {
                JsonHelper.GetErrorResult(101, ErrorMsgCode.ErrorDic[101]);//,"参数错误！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Version))
            {
                JsonHelper.GetErrorResult(116, ErrorMsgCode.ErrorDic[116]);//"版本号不能为空！");
            }
            if (submitData != null && submitData.UserID <= 0)
            {
                JsonHelper.GetErrorResult(104, ErrorMsgCode.ErrorDic[104]);//"用户ID不能为空！");
            }
            HttpResponseMessage htm = new HttpResponseMessage();
            switch (submitData.Version)
            {
                case "V1":
                    htm = pc.GetOrderListByUserId(submitData.UserID.ToString());
                    break;
                default:
                    htm = JsonHelper.GetErrorResult(114, ErrorMsgCode.ErrorDic[114]);//"无订单信息！");
                    break;
            }
            return htm;
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserInfoByUserID([FromBody] KingRequest request)
        {
            ReceiveParameter submitData = JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            if (submitData == null)
            {
                JsonHelper.GetErrorResult(101, ErrorMsgCode.ErrorDic[101]);//,"参数错误！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Version))
            {
                JsonHelper.GetErrorResult(116, ErrorMsgCode.ErrorDic[116]);//"版本号不能为空！");
            }
            if (submitData != null && submitData.UserID <= 0)
            {
                JsonHelper.GetErrorResult(104, ErrorMsgCode.ErrorDic[104]);//"用户ID不能为空！");
            }
            HttpResponseMessage htm = new HttpResponseMessage();
            UserCenter uc = new UserCenter();
            switch (submitData.Version)
            {
                case "V1":
                    var user = userBLL.GetUserAllInfoByUserId(submitData.UserID);
                    if (user != null)
                    {
                        uc.UserType = user.iBS_UserInfo.UserType;
                        uc.UserID = user.iBS_UserInfo.UserID.ToString();
                        uc.TrueName = string.IsNullOrEmpty(user.iBS_UserInfo.TrueName) ? "暂未填写" : user.iBS_UserInfo.TrueName;
                        uc.Telephone = user.iBS_UserInfo.TelePhone;
                        var clsinfo = user.ClassSchDetailList.FirstOrDefault();
                        if (user.iBS_UserInfo.UserType == (int)UserTypeEnum.Teacher)
                        {
                            uc.SchoolID = user.iBS_UserInfo.SchoolID;
                            uc.SchoolName = user.iBS_UserInfo.SchoolName;
                            uc.ClassListNum = 0;
                            uc.InvNum = user.iBS_UserInfo.UserNum;
                            if (clsinfo != null)
                            {
                                uc.ClassListNum = user.ClassSchDetailList.Count;
                            }

                        }
                        else
                        {
                            if (clsinfo != null)
                            {
                                uc.ClassID = clsinfo.ClassID;
                                uc.ClassName = clsinfo.ClassName;
                                uc.SchoolID = clsinfo.SchID;
                                uc.SchoolName = clsinfo.SchName;
                            }
                            else
                            {
                                uc.ClassID = "";
                                uc.ClassName = "";
                                uc.SchoolID = 0;
                                uc.SchoolName = "";
                            }
                        }
                        htm = JsonHelper.GetResult(uc);
                    }
                    else
                    {
                        htm = JsonHelper.GetErrorResult(201, ErrorMsgCode.ErrorDic[201]);
                    }
                    break;
                default:
                    htm = JsonHelper.GetErrorResult(201, ErrorMsgCode.ErrorDic[201]);//"无订单信息！");
                    break;
            }
            return htm;
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserInfoByUserIDTest()
        {
            ReceiveParameter submitData = new ReceiveParameter();//JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            submitData.UserID = 1676744993;// 2064256946;
            submitData.Version = "V1";
            if (submitData == null)
            {
                JsonHelper.GetErrorResult(101, ErrorMsgCode.ErrorDic[101]);//,"参数错误！");
            }
            if (submitData != null && string.IsNullOrEmpty(submitData.Version))
            {
                JsonHelper.GetErrorResult(116, ErrorMsgCode.ErrorDic[116]);//"版本号不能为空！");
            }
            if (submitData != null && submitData.UserID <= 0)
            {
                JsonHelper.GetErrorResult(104, ErrorMsgCode.ErrorDic[104]);//"用户ID不能为空！");
            }
            HttpResponseMessage htm = new HttpResponseMessage();
            UserCenter uc = new UserCenter();
            switch (submitData.Version)
            {
                case "V1":
                    var user = userBLL.GetUserAllInfoByUserId(submitData.UserID);
                    if (user != null)
                    {
                        uc.UserType = user.iBS_UserInfo.UserType;
                        uc.UserID = user.iBS_UserInfo.UserID.ToString();
                        uc.TrueName = user.iBS_UserInfo.TrueName;
                        uc.Telephone = user.iBS_UserInfo.TelePhone;
                        var clsinfo = user.ClassSchDetailList.FirstOrDefault();
                        if (user.iBS_UserInfo.UserType == (int)UserTypeEnum.Teacher)
                        {
                            uc.SchoolID = user.iBS_UserInfo.SchoolID;
                            uc.SchoolName = user.iBS_UserInfo.SchoolName;
                            uc.ClassListNum = 0;
                            uc.InvNum = user.iBS_UserInfo.UserNum;
                            if (clsinfo != null)
                            {
                                uc.ClassListNum = user.ClassSchDetailList.Count;
                            }

                        }
                        else
                        {
                            if (clsinfo != null)
                            {
                                uc.ClassID = clsinfo.ClassID;
                                uc.ClassName = clsinfo.ClassName;
                                uc.SchoolID = clsinfo.SchID;
                                uc.SchoolName = clsinfo.SchName;
                            }
                            else
                            {
                                uc.ClassID = "";
                                uc.ClassName = "";
                                uc.SchoolID = 0;
                                uc.SchoolName = "";
                            }
                        }
                        htm = JsonHelper.GetResult(uc);
                    }
                    else
                    {
                        htm = JsonHelper.GetErrorResult(201, ErrorMsgCode.ErrorDic[201]);
                    }
                    break;
                default:
                    htm = JsonHelper.GetErrorResult(201, ErrorMsgCode.ErrorDic[201]);//"无订单信息！");
                    break;
            }
            return htm;
        }

        /// <summary>
        /// 添加教师信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AddTeacherInfo()
        {
            ReceiveParameter submitData = new ReceiveParameter();//JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            submitData.Version = "V1";
            submitData.UserID = 1289666394;
            submitData.UserName = "夜月行";
            submitData.SubjectID = 3;
            submitData.Subject = "学科";
            submitData.ProvinceID = 150000000;
            submitData.Province = "内蒙古自治区";
            submitData.CityID = 150100000;
            submitData.City = "呼和浩特市";
            submitData.AreaID = 150123000;
            submitData.Area = "和林格尔县";
            submitData.SchoolID = 535701;
            submitData.SchoolName = "和林格尔县城关镇第六小学";
            TBX_UserInfo info = new TBX_UserInfo
            {
                iBS_UserInfo = new IBS_UserInfo()
                {
                    UserID = submitData.UserID,
                    UserName = submitData.UserName,
                    ClassSchList = new List<ClassSch>() 
                    {
                        new ClassSch()
                        {
                        AreaID = submitData.AreaID,
                        SubjectID = submitData.SubjectID,
                        SchID = submitData.SchoolID,
                        }
                        
                    }
                },
                ClassSchDetailList = new List<ClassSchDetail>()
                {
                    new ClassSchDetail()
                    {
                        AreaID = submitData.AreaID,
                        AreaName = submitData.Area,
                        SubjectID = submitData.SubjectID,
                        SubjectName = submitData.Subject,
                        SchID = submitData.SchoolID,
                        SchName = submitData.SchoolName
                    }
                },
                ProvinceID = submitData.ProvinceID,
                Province = submitData.Province,
                CityID = submitData.CityID,
                City = submitData.City,


            };
            HttpResponseMessage htm;
            switch (submitData.Version.ToUpper())
            {
                case "V1":
                    htm = pc.AddTeacherInfo(info, AppID);
                    break;
                default:
                    htm = JsonHelper.GetErrorResult(115, ErrorMsgCode.ErrorDic[115]);//"信息不存在！");
                    break;
            }
            return htm;
        }

        [HttpGet]
        public HttpResponseMessage GetSchoolInfoTest()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ReceiveParameter submitData = new ReceiveParameter();//JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            submitData.AreaID = 442000001;
            submitData.Version = "V1";
            if (submitData == null)
            {
                JsonHelper.GetErrorResult("参数错误！");
            }
            if (submitData != null && submitData.AreaID <= 0)
            {
                JsonHelper.GetErrorResult("地区ID不能为空！");
            }
            var areaInfo = areaBLL.GetAreaSchRelationByAreaId(submitData.AreaID);
            List<SchoolInfo> sc = new List<SchoolInfo>();
            areaInfo.AreaSchList.ForEach(a =>
            {
                SchoolInfo sch = new SchoolInfo();
                sch.ID = a.SchD;
                sch.SchoolName = a.SchName;
                sch.DistrictID = submitData.AreaID;
                sc.Add(sch);
            });
            return JsonHelper.GetResult(sc, "");
        }

        /// <summary>
        /// 获取个人中心订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOrderListByUserIdTest(string AppID, int UserID, string Version)
        {
            ReceiveParameter submitData = new ReceiveParameter();//JsonHelper.DecodeJson<ReceiveParameter>(request.Data);
            submitData.UserID = UserID;
            submitData.Version = Version;
            if (submitData == null)
            {
                JsonHelper.GetErrorResult("参数错误！");
            }
            if (submitData != null && submitData.UserID <= 0)
            {
                JsonHelper.GetErrorResult("用户ID不能为空！");
            }
            HttpResponseMessage htm = new HttpResponseMessage();
            switch (submitData.Version)
            {
                case "V1":
                    htm = pc.GetOrderListByUserId(submitData.UserID.ToString());
                    break;
                default:
                    htm = JsonHelper.GetErrorResult("无订单信息！");
                    break;
            }
            return htm;
        }

        public class ReceiveParameter
        {
            public string Version { get; set; }
            public int AreaID { get; set; }
            public int UserID { get; set; }
            public string UserName { get; set; }
            public int SubjectID { get; set; }
            public string Subject { get; set; }
            public int ProvinceID { get; set; }
            public string Province { get; set; }
            public int CityID { get; set; }
            public string City { get; set; }
            public string Area { get; set; }
            public int SchoolID { get; set; }
            public string SchoolName { get; set; }
        }

        public class ss
        {
            public SchoolInfo[] cs { get; set; }
        }

        public class SchoolInfo
        {
            public int ID { get; set; }
            public string SchoolName { get; set; }
            public int DistrictID { get; set; }
        }

        public class UserCenter
        {
            public string UserID { get; set; }
            public string TrueName { get; set; }
            public int? UserType { get; set; }
            public int? SchoolID { get; set; }
            public string SchoolName { get; set; }
            public string ClassID { get; set; }
            public string ClassName { get; set; }
            public string InvNum { get; set; }
            public string Telephone { get; set; }

            public int? SubjectID { get; set; }

            public int? ClassListNum { get; set; }

        }
    }
}