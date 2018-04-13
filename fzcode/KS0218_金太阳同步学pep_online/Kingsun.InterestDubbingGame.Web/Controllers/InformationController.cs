using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.InterestDubbingGame.BLL;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.InterestDubbingGame.Web.Controllers
{
    public class InformationController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /Information/ 
        static RedisHashHelper redis = new RedisHashHelper();
        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        public ActionResult Index()
        {
            string UserID = "";
            string TelePhone = "";
            string AppID = ""; int VersionID = 0; string VersionName = "";
            if (Request["UserID"] == null || Request["UserID"] == "")
            {
                Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
                return View();
            }
            else
            {
                UserID = Request["UserID"];
            }
            if (Request["AppID"] == null || Request["AppID"] == "")
            {
                Response.Write("<script>alert('参数错误！AppID不能为空！')</script>");
                return View();
            }
            else
            {
                AppID = Request["AppID"];
            }
            try
            {
                //根据AppID获取版本信息
                string sql = string.Format(" select *from dbo.TB_APPManagement where ID='{0}'", AppID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    VersionID = Convert.ToInt32(dt.Rows[0]["VersionID"]);
                    VersionName = dt.Rows[0]["VersionName"].ToString();
                }


                //RelationService.tb_Class classInfo = relationService.GetClassInfoByStuID(UserID);//学生Id找班级信息 
                //读取本地TB_UserClassRelation表
                string TrueName = "";
                string ClassShortID = "";
                string ClassName = "";
                int? GradeID = 0;
                int? SchoolID = 0;
                TB_InterestDubbingGame_UserInfo userInfo = null;
                var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(UserID));
                if (user != null) 
                {
                    TrueName = user.iBS_UserInfo.TrueName;
                    TelePhone = user.iBS_UserInfo.TelePhone;
                    if (user.ClassSchDetailList!=null&&user.ClassSchDetailList.Count > 0) 
                    {
                        var classinfo = classBLL.GetClassUserRelationByClassId(user.ClassSchDetailList[0].ClassID);
                        if (classinfo != null)
                        {
                            ClassShortID = classinfo.ClassNum.ToString();
                            ClassName = classinfo.ClassName;
                            GradeID = classinfo.GradeID;
                            SchoolID = classinfo.SchID;
                        } 
                    }
                    userInfo = new TB_InterestDubbingGame_UserInfo()
                    {
                        UserID = UserID,
                        UserName = user.iBS_UserInfo.TrueName,
                        ContactPhone = TelePhone,
                        VersionID = VersionID,
                        VersionName = VersionName,
                        SignUpTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        SchoolID = SchoolID,//uumsUser.SchoolID,
                        SchoolName = "",//uumsUser.SchoolName,
                        GradeID = GradeID,
                        GradeName = GetGradeName(GradeID),
                        ClassID = ClassShortID,//classInfo.ID,
                        ClassName = ClassName,// classInfo.ClassName,
                        TeacherID = "",//uumsTeacher.UserID,
                        TeacherName = "",//uumsTeacher.TrueName,
                        CreateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                }
                ViewData["userInfoMation"] = userInfo;
                bool flag = redis.Set<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo_2", userInfo.UserID + "_", userInfo);
                if (!flag)
                {
                    string msg = string.Format("TB_InterestDubbingGame_UserInfo_2的key={0},value='{1}'", userInfo.UserID, userInfo == null ? "" : "userInfo为null");
                    log.Info(msg);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return View();
        }
        public static string GetGradeName(int? GradeID)
        {
            string GradeName = "";
            switch (GradeID)
            {
                case 1:
                    GradeName = "学前";
                    break;
                case 2:
                    GradeName = "一年级";
                    break;
                case 3:
                    GradeName = "二年级";
                    break;
                case 4:
                    GradeName = "三年级";
                    break;
                case 5:
                    GradeName = "四年级";
                    break;
                case 6:
                    GradeName = "五年级";
                    break;
                case 7:
                    GradeName = "六年级";
                    break;
                case 8:
                    GradeName = "七年级";
                    break;
                case 9:
                    GradeName = "八年级";
                    break;
                case 10:
                    GradeName = "九年级";
                    break;
                default:
                    GradeName = "其它";
                    break;
            }
            return GradeName;
        }
        /// <summary>
        /// 提交报名信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult AddApply(string UserID, string TrueName, string Telephone)
        {
            try
            {
                UserID = PublicHelp.DelSQLStr(UserID);
                TrueName = PublicHelp.DelSQLStr(TrueName);
                Telephone = PublicHelp.DelSQLStr(Telephone);
                if (string.IsNullOrEmpty(TrueName))
                {
                    return Json(new { Success = false, Data = "", Msg = "请输入姓名" });
                }
                else if (string.IsNullOrEmpty(Telephone))
                {
                    return Json(new { Success = false, Data = "", Msg = "请输入电话号码" });
                }
                TB_InterestDubbingGame_UserInfo userInfo = redis.Get<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo_2", UserID + "_");
                if (userInfo == null)
                {
                    string uInfo = "userInfo等于null";
                    string msg = string.Format("提交失败了.TB_InterestDubbingGame_UserInfo的key={0},value='{1}'", "key为空", uInfo);
                    log.Info(msg);
                    return Json(new { Success = false, Data = "", Msg = "提交失败了,请稍后再试！" });
                }
                else
                {
                    var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(UserID));
                    if (user != null) 
                    {
                        if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                        {
                            userInfo.ClassID = user.ClassSchDetailList[0].ClassID;
                            userInfo.ClassName = user.ClassSchDetailList[0].ClassName;
                        }
                        else 
                        {
                            return Json(new { Success = false, Data = "", Msg = "暂未加入班级！" });
                        }
                    }
                 
                }
                //更新姓名
                #region 更新姓名
                try
                {
                    if (userInfo.UserName != TrueName)
                    {
                        userInfo.UserName = TrueName;
                        //更新真实姓名到ums 
                        var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userInfo.UserID));
                        if (user != null)
                        {
                            if (user.ClassSchList != null && user.ClassSchList.Count > 0) 
                            {
                                userInfo.SchoolID = user.ClassSchList[0].SchID;
                            }
                           
                        }
                        TBX_UserInfo model = new TBX_UserInfo();
                        model.iBS_UserInfo.UserID = Convert.ToInt32(UserID);
                        model.iBS_UserInfo.TrueName = TrueName;
                        var re=userBLL.Update(model);
                        if (!re)
                        {
                            string msg = string.Format("提交报名时，更新ums用户信息失败，UserID='{0}',TrueName='{1}'", UserID, TrueName);
                            log.Info(msg);
                            return Json(new { Success = false, Data = "", Msg = "" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("error", ex);
                }
                #endregion
                userInfo.ContactPhone = Telephone;
                bool flag = redis.Set<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", userInfo.UserID, userInfo);
                if (flag)
                {
                    redis.Remove("TB_InterestDubbingGame_UserInfo_2", userInfo.UserID + "_");
                    //判断比赛时间是否到了
                    DateTime? FirstGameStartTime;//初赛开始时间
                    DateTime? FirstGameEndTime;//初赛结束时间
                    List<TB_InterestDubbingGame_MatchTime> matchTime = new TB_InterestDubbingGame_MatchTimeBLL().GetList().Take(1).ToList();
                    if (matchTime != null && matchTime.Count > 0)
                    {
                        FirstGameStartTime = matchTime[0].FirstGameStartTime;
                        FirstGameEndTime = matchTime[0].FirstGameEndTime;
                        if (DateTime.Now >= FirstGameStartTime && DateTime.Now <= FirstGameEndTime)//比赛已开始
                        {
                            return Json(new { Success = true, Data = "", Msg = "1" });//比赛已开始
                        }
                    }
                    return Json(new { Success = true, Data = userInfo, Msg = "" });
                }
                else
                {
                    return Json(new { Success = false, Data = "", Msg = "提交失败了" });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常！" });
            }
        }
    }
}
