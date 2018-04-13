using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace KingsSun.SpokenBroadcas.DAL
{
    public class CourseDAL : BaseManagement
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 根据条件获取课程信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_Course> GetCourseList(string strWhere, string orderby = "")
        {
            try
            {
                return Search<Tb_Course>(strWhere, orderby);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据CourseID获取时间进度和课程状态
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public DataSet GetGourseScheduleCourseState(int CourseID)
        {
            try
            {
                string sql = string.Format(@"   select ISNULL(sum(ysk),0)YCount,
                                                  case when ISNULL(sum(wsk),0)+ISNULL(sum(ysk),0)>=(select Num from Tb_Course where ID='{0}') and ISNULL(sum(wsk),0)=0 then 0 else 1 end CourseState 
                                                  from (
	                                                select 
	                                                case when maxTime<GETDATE()then COUNT(1) end ysk,
	                                                case when maxTime>GETDATE()then COUNT(1) end wsk from 
	                                                (
	                                                select CoursePeriodID,MAX(EndTime)maxTime 
		                                                from Tb_CoursePeriodTime where CoursePeriodID 
			                                                in(
			                                                select ID from Tb_CoursePeriod where CourseID='{0}' and Status=1 
			                                                ) 
	                                                group by CoursePeriodID
	                                                ) a  
	                                                group by maxTime
                                                ) aa", CourseID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据CourseID获取课时信息
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodByCourseID(int CourseID)
        {
            try
            {

                string sql = string.Format(@" select a.ID,a.Name,a.Image,a.Price,a.NewPrice,a.AppointNum,a.AppointNumShow,MIN(b.StartTime)StartTime 
 from Tb_CoursePeriod a left join Tb_CoursePeriodTime b on a.ID=b.CoursePeriodID where a.CourseID={0} and Status=1
 group by a.ID,a.Name,a.Image,a.Price,a.NewPrice,a.AppointNum,a.AppointNumShow order by StartTime ", CourseID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 获取课时状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID">课时ID</param>
        /// <returns></returns>
        public string GetCoursePeriodState(int UserID, int CoursePeriodID)
        {
            try
            {
                List<DbParameter> list = new List<DbParameter>();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@UserID", SqlDbType.Int);
                param[1] = new SqlParameter("@CoursePeriodID", SqlDbType.Int);
                param[0].Value = UserID;
                param[1].Value = CoursePeriodID;
                list.AddRange(param);
                DataSet set = ExecuteProcedure("proc_getCoursePeriodState", list);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    return set.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return "";
        }

        /// <summary>
        /// 根据课时ID获取课时时间列表
        /// </summary>
        /// <param name="CoursePeriodID">课时ID</param>
        /// <returns></returns>
        public DataSet GetCoursePeriodTime(int CoursePeriodID, int UserID)
        {
            try
            {
                string sql = string.Format(@"select   a.ID,StartTime,EndTime,TeacherType,LimitNum, COUNT(b.ID) ,
	  case when EndTime<GETDATE() then '已结束'  
	  when a.ID=b.CoursePeriodTimeID and UserID='{0}' then '已预约' 
	  when LimitNum-(select COUNT(b.ID) from Tb_UserAppoint where CoursePeriodID='{1}' and CoursePeriodTimeID=b.CoursePeriodTimeID)<=0  and state=1 then '已满' 
	  else '可预约' end CourseState
		 from Tb_CoursePeriodTime a left join 
			(
				select ID,State,UserID,CoursePeriodTimeID from Tb_UserAppoint 
				where CoursePeriodTimeID in(select ID from Tb_CoursePeriodTime 
				where  CoursePeriodID='{1}' )
			)
		 b on a.ID=b.CoursePeriodTimeID where CoursePeriodID='{1}' 
	  group by a.ID,LimitNum,StartTime,EndTime,TeacherType,state,CoursePeriodTimeID,UserID,CoursePeriodID
	  order by StartTime", UserID, CoursePeriodID);
                sql = string.Format(@"select ID,StartTime,EndTime,TeacherType,
		                 case when EndTime<GETDATE() then '已结束'  
		                 when UserID is not null then'已预约'
		                 when ApointNum>=LimitNum then '已满'
		                 else '可预约'
		                 end CourseState
	                from (	  
	                select ID,StartTime,EndTime,TeacherType,UserID,ApointNum,LimitNum  from (
		                select *  from Tb_CoursePeriodTime a left join 
		                (
			                select UserID,CoursePeriodTimeID from Tb_UserAppoint 
			                where CoursePeriodTimeID in(select ID from Tb_CoursePeriodTime where   UserID='{0}') and State=1
		                )
		                 b on a.ID=b.CoursePeriodTimeID where CoursePeriodID='{1}' 
	                )m
	                 left join 
	                (
		                select CoursePeriodTimeID,COUNT(1) ApointNum from Tb_UserAppoint where  State=1
					                group by CoursePeriodTimeID
	                ) n on m.ID=n.CoursePeriodTimeID
                )aa
                order by StartTime", UserID, CoursePeriodID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据UserID和课时时间ID得到CoursePeriodTime状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID">课时ID</param>
        /// <param name="CoursePeriodTimeID">课时时间ID</param>
        /// <returns></returns>
        public string GetCoursePeriodTimeState(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                //                string sql = string.Format(@"select  
                //	case when EndTime<GETDATE() then '已结束' 
                //	when LimitNum-COUNT(b.ID)=0 and state=1 then '已满'  
                //	when ( select COUNT(1) from Tb_UserAppoint where CoursePeriodTimeID ='{1}' and UserID='{0}' and State=1)>0 then '已预约'  
                //	else '可预约' end CourseState
                //from Tb_CoursePeriodTime a 
                //left join Tb_UserAppoint b on a.ID=b.CoursePeriodTimeID where a.ID='{1}'  
                //group by EndTime,LimitNum,UserID,CoursePeriodTimeID,state", UserID, CoursePeriodTimeID);

                string sql = string.Format(@" select  
	case when EndTime<GETDATE() then '已结束' 
	when (LimitNum-COUNT(b.ID)=0 and state=1) or(LimitNum-COUNT(b.ID)=0 and state=0 and DATEADD(MI,15,b.CreateTime)>GETDATE() and UserID <>'{0}')  then '已满'  --当前这个课时时间段预约已满
	when ( select COUNT(1) from Tb_CoursePeriod where ID='{1}' and AppointNum=LimitNum and Status=1)>0 then '课时已满'		--当前这个课时所有的时间预约已满
	when ( select COUNT(1) from Tb_UserAppoint where CoursePeriodTimeID ='{2}' and UserID='{0}' and State=1)>0 then '已预约'  
	else '可预约' end CourseState
from Tb_CoursePeriodTime a 
left join Tb_UserAppoint b on a.ID=b.CoursePeriodTimeID where a.ID='{2}'  
group by EndTime,LimitNum,UserID,CoursePeriodTimeID,state,b.CreateTime", UserID, CoursePeriodID, CoursePeriodTimeID);
                DataSet set = ExecuteSql(sql);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    return set.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return "";
        }
        /// <summary>
        /// 提交用户预约信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public bool CommitUserAppoint(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                string sql_UserAppoint = "";
                string sql_Exists = string.Format("select COUNT(1) from dbo.Tb_UserAppoint where UserID='{0}' and CoursePeriodTimeID='{1}' and State=0", UserID, CoursePeriodTimeID);
                string sql = string.Format(@"select c.Name CourseName,b.Name CoursePeriodName,a.StartTime,a.EndTime,b.NewPrice from dbo.Tb_CoursePeriodTime a 
                                            left join Tb_CoursePeriod b on a.CoursePeriodID=b.ID
                                            left join Tb_Course c on b.CourseID=c.ID
                                                where a.ID='{0}'  and c.Status=1 and b.Status=1 ", CoursePeriodTimeID);
                DataTable dt = ExecuteSql(sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string CourseName = dt.Rows[0]["CourseName"].ToString();
                    string CoursePeriodName = dt.Rows[0]["CoursePeriodName"].ToString();
                    string StartTime = dt.Rows[0]["StartTime"].ToString();
                    string EndTime = dt.Rows[0]["EndTime"].ToString();
                    string NewPrice = dt.Rows[0]["NewPrice"].ToString();
                    int HaveCount = Convert.ToInt32(SqlHelper.ExecuteScalar(AppSetting.ConnectionString, CommandType.Text, sql_Exists));
                    if (HaveCount > 0)//存在
                    {
                        sql_UserAppoint = string.Format(@"update dbo.Tb_UserAppoint set CreateTime=GETDATE(),AppointCourseName='{2}',AppointCoursePeriodName='{3}',AppointStartTime='{4}',AppointEndTime='{5}',AppointNewPrice='{6}'
                        where UserID='{0}' and CoursePeriodTimeID='{1}' and State=0", UserID, CoursePeriodTimeID, CourseName, CoursePeriodName, StartTime, EndTime, NewPrice);
                    }
                    else
                    {
                        sql_UserAppoint = string.Format(@" insert into Tb_UserAppoint(UserID,CoursePeriodTimeID,AppointCourseName,AppointCoursePeriodName,AppointStartTime,AppointEndTime,AppointNewPrice)
                    values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", UserID, CoursePeriodTimeID, CourseName, CoursePeriodName, StartTime, EndTime, NewPrice);
                    }
                    int count = SqlHelper.ExecuteNonQuery(AppSetting.ConnectionString, CommandType.Text, sql_UserAppoint);
                    return count > 0 ? true : false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return false;
        }
        /// <summary>
        /// 根据UserID和CoursePeriodTimeID更新预约表状态为1
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public bool UpdateUserAppointState(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                string sql_Course = string.Format(@"update Tb_CoursePeriod set AppointNum=AppointNum+1,AppointNumShow=
	                                            (
                                                    case when AppointNum<10 then AppointNumShow+9
                                                    when AppointNum<20 then AppointNumShow+8
                                                    when AppointNum<30 then AppointNumShow+3
                                                    else AppointNum+1 END
                                                ) where ID='{0}'", CoursePeriodID);
                string sql_CoursePeriod = string.Format(@"update Tb_Course set AppointNum=AppointNum+1,
	                                        AppointNumShow=(
		                                        select SUM(AppointNumShow) from Tb_CoursePeriod 
		                                        where CourseID in (
			                                        select CourseID from Tb_CoursePeriod  where ID='{0}'
		                                        )
	                                        )
                                        where ID=(select CourseID from Tb_CoursePeriod where ID='{0}')", CoursePeriodID);
                string sql_UserAppoint = string.Format(" update Tb_UserAppoint set State=1 where UserID='{0}' and CoursePeriodTimeID='{1}'  and State=0 and DATEADD(MI,15,CreateTime)>GETDATE()", UserID, CoursePeriodTimeID);
                List<string> list = new List<string>();
                list.Add(sql_Course);
                list.Add(sql_CoursePeriod);
                list.Add(sql_UserAppoint);
                return SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return false;
        }
        /// <summary>
        /// 根据课时ID获取该课时的价格
        /// </summary>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodPrice(string CoursePeriodID)
        {
            try
            {
                string sql = string.Format(" select * from  dbo.Tb_CoursePeriod where  ID='{0}' and Status=1 ", CoursePeriodID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }

        /// <summary>
        /// 根据UserID和课时ID CoursePeriodID得到预约成功信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public DataSet GetAppointSuccessInfo(string UserID, string CoursePeriodID)
        {
            try
            {
                string sql = string.Format(@" select (select TrueName from ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo where UserID='{0}')TrueName,Name,AheadMinutes from  dbo.Tb_CoursePeriod where  ID='{1}'", UserID, CoursePeriodID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据UserID和课时ID获取已预约的信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public DataSet GetAlreadyAppointInfo(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                string sql = string.Format(@"  select TrueName,c.AppointCoursePeriodName CourseName,c.AppointStartTime StartTime,a.AheadMinutes from Tb_CoursePeriod a left join Tb_CoursePeriodTime b 
 on a.ID=b.CoursePeriodID left join Tb_UserAppoint c on b.ID=c.CoursePeriodTimeID
 left join ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo] d on c.UserID=d.UserID  where a.ID='{1}' and c.UserID='{0}' and c.CoursePeriodTimeID='{2}' and  Status=1 and c.State=1 and EndTime>GETDATE() order by StartTime", UserID, CoursePeriodID, CoursePeriodTimeID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据CoursePeriodID和UserID获取直播间信息
        /// </summary>
        /// <param name="CoursePeriodID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetStudioInfo(string UserID, string CoursePeriodID)
        {
            try
            {
                //                string sql = string.Format(@"select a.* from dbo.Tb_CoursePeriodTime a left join Tb_UserAppoint b on a.ID=b.CoursePeriodTimeID 
                //      where CoursePeriodID='{1}' and b.State=1 and b.UserID='{0}'", UserID, CoursePeriodID);
                string sql = string.Format(@"select b.ID,d.StudioUrl,d.StudioCommand from Tb_UserAppoint a left join Tb_CoursePeriodTime b on a.CoursePeriodTimeID=b.ID
    left join Tb_CoursePeriod c on b.CoursePeriodID=c.ID 
    left join Tb_Course d on c.CourseID=d.ID
    where CoursePeriodID='{1}' and a.State=1 and a.UserID='{0}' and d.Status=1 and c.Status=1 ", UserID, CoursePeriodID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }

        ///////////////20170710新添加我的课时功能////////////////
        /// <summary>
        /// 根据UserID获取我的课时信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetMyCoursePeriodUserID(string UserID)
        {
            try
            {
                string sql = string.Format(@"select a.UserID,d.ID CourseID,c.ID CoursePeriodID,b.ID CoursePeriodTimeID,d.Type,c.BigImage,
a.AppointCoursePeriodName,a.AppointStartTime StartTime,a.AppointEndTime,c.AheadMinutes,a.CreateTime,DATEDIFF(MI,a.AppointStartTime,a.AppointEndTime)TimeLong,
case when a.AppointEndTime<GETDATE() then '已结束'
when DATEADD(MI,-c.AheadMinutes,a.AppointStartTime)<GETDATE()then '进入教室'
else '未开始' end CoursePeriodState
from Tb_UserAppoint a left join Tb_CoursePeriodTime b on a.CoursePeriodTimeID=b.ID
left join Tb_CoursePeriod c on b.CoursePeriodID=c.ID
left join Tb_Course d on c.CourseID=d.ID
 where a.UserID='{0}' and a.State=1 and c.Status=1 and d.Status=1
 group by a.UserID,d.ID ,c.ID ,b.ID ,d.Type,c.BigImage,
a.AppointCoursePeriodName,a.AppointStartTime,a.AppointEndTime,c.AheadMinutes,a.CreateTime
 order by a.AppointStartTime", UserID);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 判断预约的课时是否到达进入教室
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public string IsEnterRoom(int UserID, int CoursePeriodTimeID)
        {
            try
            {
                string sql = string.Format(@"select 
case when a.AppointEndTime<GETDATE() then '已结束'
when DATEADD(MI,-c.AheadMinutes,a.AppointStartTime)<GETDATE()then '进入教室'
else '未开始' end CoursePeriodState
from Tb_UserAppoint a left join Tb_CoursePeriodTime b on a.CoursePeriodTimeID=b.ID
left join Tb_CoursePeriod c on b.CoursePeriodID=c.ID
left join Tb_Course d on c.CourseID=d.ID
 where a.UserID='{0}' and a.CoursePeriodTimeID='{1}'
 and a.State=1 and c.Status=1 and d.Status=1
 group by   a.AppointStartTime,a.AppointEndTime,c.AheadMinutes 
 order by a.AppointStartTime", UserID, CoursePeriodTimeID);
                DataSet set = ExecuteSql(sql);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    return set.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return "";
        }

        #region 后台管理
        /// <summary>
        /// 根据ID修改Tb_Course课程表课程状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool UpdateCourseState(string ID, string State)
        {
            try
            {
                List<string> list = new List<string>();
                string sql = string.Format("update Tb_Course set Status='{1}' where ID='{0}'", ID, State);
                list.Add(sql);
                DataTable dt = SqlHelper.ExecuteDataset(AppSetting.SpokenConnectionString, CommandType.Text, string.Format(" select  *from Tb_CoursePeriod where CourseID='{0}'", ID)).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string sql_coursePeriod = string.Format("update Tb_CoursePeriod set Status='{1}'  where CourseID='{0}'", ID, State);
                    list.Add(sql_coursePeriod);
                }
                bool flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                return flag;
                //int count = SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                //return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 根据ID修改Tb_Course课程表课程图片
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ImgUrl"></param>
        /// <returns></returns>
        public bool UpdateCourseImg(string ID, string ImgUrl)
        {
            try
            {
                string sql = string.Format("update Tb_Course set Image='{1}',BigImage='{1}' where ID='{0}'", ID, ImgUrl);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 根据条件得到课时列表信息
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodDataSet(string strWhere)
        {
            try
            {
                string sql = string.Format(@"select b.ID CoursePeriodTimeID,b.StartTime,b.EndTime,a.ID CoursePeriodID,a.Name,a.Price,a.NewPrice,a.BigImage,a.Status
   from dbo.Tb_CoursePeriod a left join Tb_CoursePeriodTime b on a.ID=b.CoursePeriodID 
   where {0}", strWhere);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据ID修改课时状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdateCoursePeriodStatus(string ID, string Status)
        {
            try
            {
                List<string> list = new List<string>();
                string sql = string.Format("update Tb_CoursePeriod set Status='{1}' where ID='{0}'", ID, Status);
                list.Add(sql);
                if (Status == "1")
                {
                    string sql2 = string.Format("update Tb_Course set Status='{1}' where ID=(select CourseID from Tb_CoursePeriod where ID='{0}')", ID, Status);
                    list.Add(sql2);
                }
                bool flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                return flag;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }

        /// <summary>
        /// 根据条件获取Tb_CoursePeriod信息集合
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_CoursePeriod> GetCoursePeriodList(string strWhere, string orderby = "")
        {
            try
            {
                return Search<Tb_CoursePeriod>(strWhere, orderby);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据ID修改课时图片
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ImgUrl"></param>
        /// <returns></returns>
        public bool UpdateCoursePeriodImg(string ID, string ImgUrl)
        {
            try
            {
                string sql = string.Format("update Tb_CoursePeriod set Image='{1}',BigImage='{1}' where ID='{0}'", ID, ImgUrl);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 根据条件获取课时时间信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_CoursePeriodTime> GetCoursePeriodTimeList(string strWhere, string orderby = "")
        {
            try
            {
                return Search<Tb_CoursePeriodTime>(strWhere, orderby);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据条件查询课程统计
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetCourseCount(string strWhere)
        {
            try
            {
                string sql = string.Format(@" select tabA.CourseID,tabA.CourseName,tabA.CourseAppointNum,tabA.CoursePeriodID,tabA.CoursePeriodName,tabA.StartTime,CoursePeriodAppointNum,COUNT(e.ID)'CoursePeriodCount' from
(
   select aa.CourseID,aa.CourseName,aa.CourseAppointNum,aa.CoursePeriodID,aa.CoursePeriodName,aa.StartTime,aa.CoursePeriodTimeID,sum(aa.CoursePeriodAppointNum)CoursePeriodAppointNum 
   from 
   (
       select a.ID'CourseID',a.Name'CourseName',a.AppointNum'CourseAppointNum',b.ID'CoursePeriodID',b.Name'CoursePeriodName',c.StartTime,c.ID'CoursePeriodTimeID',
       case when d.State=1 then COUNT(c.ID) else 0 end'CoursePeriodAppointNum'
       from dbo.Tb_Course a left join Tb_CoursePeriod b on a.ID=b.CourseID
       left join Tb_CoursePeriodTime c on b.ID=c.CoursePeriodID 
       left join Tb_UserAppoint d on c.ID=d.CoursePeriodTimeID  
       where {0}-- b.Name='超级无敌掌门狗'  
       group by  a.ID,a.Name,a.AppointNum,b.ID,b.Name,c.StartTime,c.ID,d.State
   ) aa
   group by aa.CourseID,aa.CourseName,aa.CourseAppointNum,aa.CoursePeriodID,aa.CoursePeriodName,aa.StartTime,aa.CoursePeriodTimeID
) tabA left join Tb_UserLearn e on e.CoursePeriodTimeID=tabA.CoursePeriodTimeID
group by tabA.CourseID,tabA.CourseName,tabA.CourseAppointNum,tabA.CoursePeriodID,tabA.CoursePeriodName,tabA.StartTime,CoursePeriodAppointNum", strWhere);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据条件获取用户预约统计信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetUserAppointCount(string strWhere)
        {
            try
            {
                string sql = string.Format(@"   select b.UserID,b.UserName,b.TrueName,b.TelePhone,e.ID'CourseID',a.AppointCourseName'CourseName',d.ID'CoursePeriodID',a.AppointCoursePeriodName'CoursePeriodName',
                 CONVERT(varchar(100),a.CreateTime, 112)+' '+CONVERT(varchar(100),a.CreateTime, 24)'CreateTime',
                 CONVERT(varchar(100),a.AppointStartTime, 112)+' '+CONVERT(varchar(100),a.AppointStartTime, 24)+'-'+CONVERT(varchar(100),a.AppointEndTime, 24)'StartTime' ,a.AppointNewPrice NewPrice
                 from dbo.Tb_UserAppoint a left join ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo] b on a.UserID=b.UserID 
                 left join Tb_CoursePeriodTime c on c.ID=a.CoursePeriodTimeID 
                 left join Tb_CoursePeriod d on d.ID=c.CoursePeriodID
                 left join Tb_Course e on e.ID=d.CourseID 
                 where a.State=1 and d.Status=1 and e.Status=1 {0}  order by a.CreateTime desc", strWhere);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据条件获取上课用户统计信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetClassUserCount(string strWhere)
        {
            try
            {
                string sql = string.Format(@"select UserID,UserName,TrueName,TelePhone,CourseID,CourseName,CoursePeriodID,CoursePeriodName,NewPrice,CourseStartTime,StartTime,EndTime,OutTimes 
                                            from Tb_UserLearn where {0} order by CreateTime desc", strWhere);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 导入用户预约信息
        /// </summary>
        /// <returns></returns>
        public bool AddUserAppointInfo(string UserID, string UserName, string TrueName, string TelePhone, string CoursePeriodTimeID, string AppointTime, string CourseName, string CoursePeriodName, string StartTime, string EndTime, string NewPrice)
        {
            try
            {
                string sql_UserInfo = "";
                string sql_AppointInfo = "";
                DataTable dtUser = SqlHelper.ExecuteDataset(AppSetting.SpokenConnectionString, CommandType.Text, string.Format(" select top 1*from ITSV_Base.[FZ_SynchronousStudy].dbo.[TB_UserInfo] where UserID='{0}'", UserID)).Tables[0];
                if (dtUser != null && dtUser.Rows.Count > 0)//存在该用户，则修改
                {
                    sql_UserInfo = string.Format("update ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo] set UserName='{1}',TrueName='{2}',TelePhone='{3}' where UserID='{0}'", UserID, UserName, TrueName, TelePhone);
                }
                else
                {
                    sql_UserInfo = string.Format(@"insert into ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo](UserID,UserName,TrueName,TelePhone,UserRoles)values('{0}','{1}','{2}','{3}',0)", UserID, UserName, TrueName, TelePhone);

                }
                DataTable dtAppoint = SqlHelper.ExecuteDataset(AppSetting.SpokenConnectionString, CommandType.Text, string.Format("  select top 1 *from  dbo.Tb_UserAppoint where UserID='{0}' and CoursePeriodTimeID='{1}' and State=1", UserID, CoursePeriodTimeID)).Tables[0];
                if (dtAppoint != null && dtAppoint.Rows.Count > 0)//存在预约，则修改
                {
                    sql_AppointInfo = string.Format(@"update Tb_UserAppoint set CreateTime='{2}',AppointCourseName='{3}',AppointCoursePeriodName='{4}',AppointStartTime='{5}',AppointEndTime='{6}',AppointNewPrice='{7}' 
                    where UserID='{0}' and CoursePeriodTimeID='{1}' and State=1", UserID, CoursePeriodTimeID, AppointTime, CourseName, CoursePeriodName, StartTime, EndTime, NewPrice);
                }
                else
                {
                    sql_AppointInfo = string.Format(@"insert into Tb_UserAppoint(UserID,CoursePeriodTimeID,State,CreateTime,AppointCourseName,AppointCoursePeriodName,AppointStartTime,AppointEndTime,AppointNewPrice)
                    values('{0}','{1}',1,'{2}','{3}','{4}','{5}','{6}','{7}')", UserID, CoursePeriodTimeID, AppointTime, CourseName, CoursePeriodName, StartTime, EndTime, NewPrice);
                }
                string sql_Course = string.Format(@"update Tb_CoursePeriod set AppointNum=AppointNum+1,AppointNumShow=
	                                            (
                                                    case when AppointNum<10 then AppointNumShow+9
                                                    when AppointNum<20 then AppointNumShow+8
                                                    when AppointNum<30 then AppointNumShow+3
                                                    else AppointNum+1 END
                                                ) where ID=( select CoursePeriodID from Tb_CoursePeriodTime where ID='{0}')", CoursePeriodTimeID);
                string sql_CoursePeriod = string.Format(@"update Tb_Course set AppointNum=AppointNum+1,
	                                        AppointNumShow=(
		                                        select SUM(AppointNumShow) from Tb_CoursePeriod 
		                                        where CourseID in (
			                                        select CourseID from Tb_CoursePeriod  where ID=( select CoursePeriodID from Tb_CoursePeriodTime where ID='{0}')
		                                        )
	                                        )
                                        where ID=(select CourseID from Tb_CoursePeriod where ID=( select CoursePeriodID from Tb_CoursePeriodTime where ID='{0}'))", CoursePeriodTimeID);
                List<string> list = new List<string>();
                list.Add(sql_UserInfo);
                list.Add(sql_AppointInfo);
                list.Add(sql_Course);
                list.Add(sql_CoursePeriod);
                bool flag = SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                return flag;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 根据条件获取UserInfoTemp表信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetUserInfoTemp(string strWhere)
        {
            try
            {
                string sql = string.Format(@"select * from dbo.Tb_UserInfoTemp where {0} order by CreateTime desc", strWhere);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据条件获取预约用户查询课时列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet getCoursePeriodTimeSearch(string strWhere)
        {
            try
            {
                string sql = string.Format(@"select c.ID'CoursePeriodTimeID',a.Name'CourseName',a.Groups,b.Name'CoursePeriodName',c.StartTime,c.EndTime,b.NewPrice
                                          from Tb_Course a left join Tb_CoursePeriod b on a.ID=b.CourseID
                                         left join Tb_CoursePeriodTime c on b.ID=c.CoursePeriodID
                                         where {0}  order by c.CreateTime desc", strWhere);
                return ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        #endregion
    }
}
