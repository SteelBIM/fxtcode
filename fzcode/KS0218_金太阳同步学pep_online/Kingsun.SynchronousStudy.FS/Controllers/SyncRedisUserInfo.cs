using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.FS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.Fs.Controllers
{
    public class SyncRedisUserInfoController : Controller
    {

        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        //
        // GET: /SyncRedisUserInfo/

        public ActionResult Index()
        {
            //SyncUserInfoRedisFromUMS();
            return View();
        }
        /// <summary>
        /// 手动同步redis数据到DB
        /// </summary>
        /// <returns></returns>
        public ActionResult HandSyncUserInfo()
        {
            try
            {
                int TotalCount = 0; int fail_count = 0;
                //string connect_com = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
                string kingsun_qpy = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_qpy"].ConnectionString;
                List<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo> gamelist = redis.GetAll<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo");
                if (gamelist != null && gamelist.Count > 0)
                {
                    TotalCount = gamelist.Count;
                    if (gamelist != null && gamelist.Count > 0)
                    {
                        foreach (Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo row in gamelist)
                        {
                            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(kingsun_qpy, CommandType.Text, "select COUNT(1) from dbo.TB_InterestDubbingGame_UserInfo where UserID='" + row.UserID + "'"));
                            if (count <= 0)
                            {
                                string sql = string.Format(@"insert into TB_InterestDubbingGame_UserInfo (UserID,UserName,ContactPhone,VersionID,VersionName,SignUpTime,SchoolID,SchoolName,
                                                   GradeID,GradeName,ClassID,ClassName,TeacherID,TeacherName,CreateTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',
                                                    '{9}','{10}','{11}','{12}','{13}','{14}')", row.UserID, row.UserName, row.ContactPhone, row.VersionID, row.VersionName,
                                                     row.SignUpTime, row.SchoolID, row.SchoolName, row.GradeID, row.GradeName, row.ClassID, row.ClassName, row.TeacherID, row.TeacherName
                                                     , row.CreateTime);
                                if (SqlHelper.ExecuteNonQuery(kingsun_qpy, CommandType.Text, sql) <= 0)
                                {
                                    fail_count++;
                                    Log4Net.LogHelper.Info("HandSyncUserInfo趣配音手动同步数据到DB失败，同步用户出错：" + row.UserID + "|" + row.UserName + "|" + DateTime.Now);
                                }
                            }
                        }
                    }
                }
                string result = string.Format("HandSyncUserInfo趣配音手动同步数据成功,总数量：{0} ,失败数量：{1} ", TotalCount, fail_count);
                Log4Net.LogHelper.Info(result + DateTime.Now);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "HandSyncUserInfo");
            }
            return View();
        }
        static RedisHashHelper redis = new RedisHashHelper();

        /// <summary>
        /// 手动同步数据到Redis和DB
        /// </summary>
        public void SyncUserInfoRedisFromUMS()
        {
            int count = 0; int success_count = 0; int fail_count = 0;
            try
            {
                string connect_com = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
                string kingsun_qpy = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_qpy"].ConnectionString;
                List<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo> list = redis.GetAll<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo");
                if (list != null && list.Count > 0)
                {
                    count = list.Count;
                    foreach (var item in list)
                    {
                        if (item.State == 0)
                        {
                            #region 通过Umms获取用户信息
                            try
                            {
                                var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(item.UserID));
                                if (user != null)
                                {
                                    item.UserImage = user.iBS_UserInfo.UserImage;
                                    if (string.IsNullOrEmpty(item.UserName))
                                    {
                                        item.UserName = user.iBS_UserInfo.TrueName;
                                    }
                                    if (user.ClassSchDetailList.Count > 0)
                                    {
                                        var classinfo = classBLL.GetClassUserRelationByClassId(user.ClassSchDetailList[0].ClassID);
                                        if (classinfo != null)
                                        {
                                            if (string.IsNullOrEmpty(item.ClassID))
                                            {
                                                item.ClassID = classinfo.ClassNum.ToString();
                                                item.ClassName = classinfo.ClassName;
                                            }
                                            if (string.IsNullOrEmpty(item.GradeID.ToString()) || item.GradeID == 0)
                                            {
                                                item.GradeID = classinfo.GradeID;
                                                item.GradeName = GetGradeName(classinfo.GradeID);
                                            }
                                            if (!string.IsNullOrEmpty(classinfo.SchID.ToString()))
                                            {
                                                item.SchoolID = classinfo.SchID;
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(user.ClassSchDetailList[0].SchID.ToString()))
                                            {
                                                item.SchoolID = user.ClassSchDetailList[0].SchID;
                                            }
                                            if (!string.IsNullOrEmpty(user.ClassSchDetailList[0].SchName))
                                            {
                                                item.SchoolName = user.ClassSchDetailList[0].SchName;
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(item.SchoolID.ToString()) && item.SchoolID != 0)
                                        {
                                            try
                                            {
                                                item.AreaName = user.ClassSchDetailList[0].AreaName;
                                                item.SchoolName = user.ClassSchDetailList[0].SchName;
                                            }
                                            catch (Exception ex)
                                            {
                                                Log4Net.LogHelper.Info(string.Format("SyncUserInfoRedisFromUMS中SchoolID={0},UserID={1},未找到区域ID和区域", item.SchoolID, item.UserID));
                                            }
                                        }
                                        else
                                        {
                                            Log4Net.LogHelper.Info(string.Format("SyncUserInfoRedisFromUMS中SchoolID={0},UserID={1},未找到区域ID和区域", item.SchoolID, item.UserID));
                                        }
                                    }

                                    item.State = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                fail_count++;
                                Log4Net.LogHelper.Error(ex, string.Format("UserID={0},同步异常", item.UserID));
                            }
                            #endregion
                        }
                        redis.Set<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", item.UserID, item);
                        //同步数据到DB
                        string sqlUpdate = string.Format(@"update TB_InterestDubbingGame_UserInfo set SchoolID='{1}',SchoolName='{2}',GradeID='{3}',GradeName='{4}',
ClassID='{5}',ClassName='{6}', AreaID='{7}',AreaName='{8}',State=1 where UserID='{0}'", item.UserID, item.SchoolID, item.SchoolName, item.GradeID, item.GradeName, item.ClassID, item.ClassName,
                                                                                      item.AreaID, item.AreaName);
                        if (SqlHelper.ExecuteNonQuery(kingsun_qpy, CommandType.Text, sqlUpdate) <= 0)
                        {
                            Log4Net.LogHelper.Info("SyncUserInfoRedisFromUMS趣配音同步数据到DB失败，UserID=" + item.UserID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                fail_count++;
                Log4Net.LogHelper.Error(ex);
            }
            string result = string.Format("总数量：{0} ,失败数量：{1} ", count, fail_count);
            ViewData["result"] = result;
        }
        /// <summary>
        /// 根据GradeID得到年级名称
        /// </summary>
        /// <param name="GradeID"></param>
        /// <returns></returns>
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
    }
}
