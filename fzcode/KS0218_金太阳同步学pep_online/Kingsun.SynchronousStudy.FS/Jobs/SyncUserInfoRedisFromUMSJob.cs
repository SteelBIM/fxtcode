using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class SyncUserInfoRedisFromUMSJob:IJob
    {
        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        static RedisHashHelper redis = new RedisHashHelper();
        public void Execute()
        {
            lock (TimedTask._lock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("趣配音报名补全用户信息SyncUserInfoRedisFromUMS开始");
                    SyncUserInfoRedisFromUMS();
                    Log4Net.LogHelper.Info("趣配音报名补全用户信息SyncUserInfoRedisFromUMS结束");
                }
            }
        }

        /// <summary>
        /// 从Redis(Redis_InterestDubbingGame_UserInfo)获取未同步完成的用户，将MOD信息同步到Redis（UserImage、SchoolID、SchoolName、GradeID、GradeName、TeacherID、TeacherName、AreaID、AreaName）
        /// </summary>
        public static void SyncUserInfoRedisFromUMS()
        {
            try
            {
                string connect_com = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
                string kingsun_qpy = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_qpy"].ConnectionString;
                List<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo> list = redis.GetAll<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo");
                foreach (var item in list)
                {
                    //读取本地用户表的头像信息

                    if (item.State == 0)
                    {
                        #region 通过Umms获取用户信息
                        try
                        {
                            //读取ums  
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
                                            item.GradeName = classinfo.GradeID.GetGradeName();
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
                            Log4Net.LogHelper.Error(string.Format("SyncUserInfoRedisFromUMS中UserID={0},同步异常", item.UserID));
                        }
                        #endregion

                    }
                    //同步数据到Redis
                    redis.Set<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", item.UserID, item);
                    //同步数据到DB
                    string sqlUpdate = string.Format(@"update TB_InterestDubbingGame_UserInfo set SchoolID='{1}',SchoolName='{2}',GradeID='{3}',GradeName='{4}',
ClassID='{5}',ClassName='{6}', AreaID='{7}',AreaName='{8}',UserName='{9}',State=1 where UserID='{0}'", item.UserID, item.SchoolID, item.SchoolName, item.GradeID, item.GradeName, item.ClassID, item.ClassName,
                                                                                  item.AreaID, item.AreaName, item.UserName);
                    if (SqlHelper.ExecuteNonQuery(kingsun_qpy, CommandType.Text, sqlUpdate) <= 0)
                    {
                        Log4Net.LogHelper.Info("SyncUserInfoRedisFromUMS趣配音同步数据到DB失败，UserID=" + item.UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "SyncUserInfoRedisFromUMS");
            }
        }
    }
}