using FluentScheduler;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.FS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace Kingsun.Fs.Jobs
{
    public class VailUserJob : IJob
    {
        public void Execute()
        {
            lock (TimedTask._vaillock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("计算有效用户开始");
                    VailUser();
                    Log4Net.LogHelper.Info("计算有效用户结束");
                }
            }
        }
        public  void VailUser()
        {
            RedisHashHelper hase = new RedisHashHelper();
            DateTime StartTime = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            DateTime EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            List<UserLogin> userloginlist = hase.GetAll<UserLogin>("UserLoginRecord").Where(ul=>ul.LoginType==1 && ul.CreateTime>=StartTime && ul.CreateTime<EndTime).ToList();//用户登录注册记录(取昨天的注册用户)
            
            string kingsun_basedb = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunBaseDBConnectionStr"].ConnectionString;
            string operationtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #region 注册用户每天添加到用户信息子表  定义为非有效用户
            if (userloginlist != null && userloginlist.Count > 0)
            {
                string loginsqls = "";
                foreach (UserLogin row in userloginlist)
                    loginsqls += "insert into Tb_UserDetails (UserId,AppId,Versions,DownloadChannel,IsValidUser,ValidUserTime) values(" + row.UserId + ",'" + row.AppId + "','" + row.Versions + "'," + row.DownloadChannel + ",0,'" + operationtime + "');";
                if (!string.IsNullOrEmpty(loginsqls))
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery(kingsun_basedb, CommandType.Text, loginsqls);
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Info("添加注册用户至用户信息子表失败！" + loginsqls+ex.Message);
                    }
                }
            }
            #endregion 

            List<UsageDetails> numberlist = new List<UsageDetails>();//使用次数
            DateTime FirstTime = Convert.ToDateTime("2018-02-01");  //有效用户从2018-02-01 开始计算
            DateTime ContrastTime = DateTime.Now;

            #region 所有用户使用记录
            for (DateTime time = FirstTime; time < ContrastTime; time = time.AddDays(1))
            {
                string tablename = "UserAppRecord_" + time.ToString("yyyy-MM-dd");
                List<UsageDetails> daynumberlist = hase.GetAll<UsageDetails>(tablename);
                numberlist.AddRange(daynumberlist);
            }
            #endregion 


            //一年内加入班级的非有效用户  IsValidUser为0时  不是有效用户（执行修改）   IsValidUser为2时无记录（执行添加）
            string str = string.Format(@"select ui.UserID,ISNULL(IsValidUser,2) as IsValidUser  from Tb_UserInfo ui join TB_UserClassRelation ucr on ui.UserID=ucr.UserID  left join Tb_UserDetails ud on ui.UserID=ud.UserId
                                         where datediff(YEAR,ui.CreateTime,ucr.CreateDate)<1  and (IsValidUser is null or IsValidUser=0)  group by ui.UserID,IsValidUser order by UserID ");
          
            DataSet dsclassuser = SqlHelper.ExecuteDataset(kingsun_basedb, CommandType.Text, str);
            if (dsclassuser != null && dsclassuser.Tables.Count>0)
            {
                DataTable classusertable = dsclassuser.Tables[0];
                if (classusertable != null && classusertable.Rows.Count > 0)
                {
                    //使用次大于3次的用户
                    //var UserUsageNumberList = numberlist.GroupBy(n => n.UserId).Select(g => (new { UserId = g.Key, UsageNumber = g.Sum(item => item.UsageNumber) })).Where(u => u.UsageNumber >= 3).OrderBy(n => n.UserId).ToList();
                  
                    //使用次大于3天次的用户
                    var UserUsageNumberList = numberlist.GroupBy(n => n.UserId).Select(g => (new { UserId = g.Key, UsageNumber = g.Count() })).Where(u => u.UsageNumber >= 3).OrderBy(n => n.UserId).ToList();
                
                    if (UserUsageNumberList != null && UserUsageNumberList.Count > 0)
                    {
                        List<Tb_UserDetails> validuserlist = new List<Tb_UserDetails>();//有效用户集合

                        #region 循环使用次数大于3次以上的用户
                        foreach (var row in UserUsageNumberList)
                        {
                            #region 循环已加入班级的用户
                            if (classusertable != null && classusertable.Rows.Count > 0)
                            {
                                for (int i = classusertable.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (row.UserId == classusertable.Rows[i]["UserId"] )
                                    {
                                        validuserlist.Add(new Tb_UserDetails() { UserId = Convert.ToInt32(classusertable.Rows[i]["UserId"]), IsValidUser = Convert.ToInt32(classusertable.Rows[i]["IsValidUser"]) });
                                        classusertable.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            #endregion 
                        }
                        #endregion 
                     
                        #region 添加修改有效用户数据  
                        if (validuserlist != null && validuserlist.Count > 0)
                        {
                            string insertsqls = "";
                            foreach (Tb_UserDetails row in validuserlist)
                            {

                                #region 添加有效用户记录表语句 
                                string AppId = "";
                                string Versions = "";
                                int DownloadChannel = 0;
                                int IsValidUser = 1;
                                List<UsageDetails> usagenumberlist = numberlist.Where(ud => ud.UserId == row.UserId.ToString()).ToList();
                                if (usagenumberlist != null && usagenumberlist.Count > 0)
                                {
                                    AppId = usagenumberlist[0].AppId;
                                    Versions = usagenumberlist[0].Versions;
                                    DownloadChannel = usagenumberlist[0].DownloadChannel;
                                }

                                if (row.IsValidUser == 2)//没有有效用户记录
                                {
                                    insertsqls += "insert into Tb_UserDetails (UserId,AppId,Versions,DownloadChannel,IsValidUser,ValidUserTime) values(" + row.UserId + ",'" + AppId + "','" + Versions + "'," + DownloadChannel + "," + IsValidUser + ",'" + operationtime + "');";
                                }
                                else
                                {
                                    insertsqls += "update Tb_UserDetails set  IsValidUser=1,ValidUserTime='" + operationtime + "' where UserId=" + row.UserId+";"; 
                                }
                                #endregion 

                            }
                          
                            if (!string.IsNullOrEmpty(insertsqls))
                            {
                                try
                                {
                                    SqlHelper.ExecuteNonQuery(kingsun_basedb, CommandType.Text, insertsqls);
                                }
                                catch (Exception ex)
                                {
                                    Log4Net.LogHelper.Info("添加或者更新用户信息子表失败！" + insertsqls + ex.Message);
                                }
                            }
                        }
                        #endregion 

                    }
                }
            }



        }


    }
}