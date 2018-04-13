using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class SyncDubbingDataJob:IJob
    {
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
                    Log4Net.LogHelper.Info("SyncDubbingData趣配音报名同步开始");
                    SyncDubbingData();//趣配音报名同步
                    Log4Net.LogHelper.Info("SyncDubbingData趣配音报名同步结束");
                }
            }
        }
        public static void SyncDubbingData()
        {
            try
            {
                //string connect_com = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
                string kingsun_qpy = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_qpy"].ConnectionString;


                List<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo> gamelist = redis.GetAll<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo");
                if (gamelist != null && gamelist.Count > 0)
                {
                    Predicate<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo> dubbinguserinfo = delegate(Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo dubbinguser) { return dubbinguser.CreateTime > Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")); };
                    List<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo> dubbinguserlist = gamelist.FindAll(dubbinguserinfo);

                    if (dubbinguserlist != null && dubbinguserlist.Count > 0)
                    {
                        foreach (Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo row in dubbinguserlist)
                        {
                            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(kingsun_qpy, CommandType.Text, "select COUNT(1) from dbo.TB_InterestDubbingGame_UserInfo where UserID='" + row.UserID + "'"));
                            if (count <= 0)
                            {
                                //                                string sql = string.Format(@"insert into TB_InterestDubbingGame_UserInfo (UserID,UserName,ContactPhone,VersionID,VersionName,SignUpTime,SchoolID,SchoolName,GradeID,GradeName,ClassID,ClassName,TeacherID,TeacherName,CreateTime) 
                                //                                values(" + ReturnInt(row.UserID) + ",'" + ReturnString(row.UserName) + "','" + ReturnString(row.ContactPhone) + "','" + ReturnString(row.VersionID) + @"',
                                //                                '" + ReturnString(row.VersionName) + "','" + ReturnDateTime(row.SignUpTime) + "','" + ReturnString(row.SchoolID) + "','" + ReturnString(row.SchoolName) + @"',
                                //                                '" + ReturnString(row.GradeID) + "','" + ReturnString(row.GradeName) + "','" + ReturnString(row.ClassID) + "','" + ReturnString(row.ClassName) + @"',
                                //                                '" + ReturnString(row.TeacherID) + "','" + ReturnString(row.TeacherName) + "','" + ReturnDateTime(row.CreateTime) + "' )");
                                string sql = string.Format(@"insert into TB_InterestDubbingGame_UserInfo (UserID,UserName,ContactPhone,VersionID,VersionName,SignUpTime,SchoolID,SchoolName,
                                                   GradeID,GradeName,ClassID,ClassName,TeacherID,TeacherName,CreateTime) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',
                                                    '{9}','{10}','{11}','{12}','{13}','{14}')", row.UserID, row.UserName, row.ContactPhone, row.VersionID, row.VersionName,
                                                     row.SignUpTime, row.SchoolID, row.SchoolName, row.GradeID, row.GradeName, row.ClassID, row.ClassName, row.TeacherID, row.TeacherName
                                                     , row.CreateTime);
                                if (SqlHelper.ExecuteNonQuery(kingsun_qpy, CommandType.Text, sql) <= 0)
                                {
                                    Log4Net.LogHelper.Info("SyncDubbingData趣配音同步数据失败，同步用户出错：" + row.UserID + "|" + row.UserName + "|" + DateTime.Now);
                                }
                            }
                        }
                    }
                }
                Log4Net.LogHelper.Info("SyncDubbingData趣配音同步数据成功" + DateTime.Now);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "SyncDubbingData");
            }
        }
    }
}