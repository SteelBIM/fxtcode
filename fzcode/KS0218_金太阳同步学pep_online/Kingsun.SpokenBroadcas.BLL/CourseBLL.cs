using KingsSun.SpokenBroadcas.DAL;
using Kingsun.SpokenBroadcas.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.BLL
{
    public class CourseBLL
    {
        CourseDAL dal = new CourseDAL();
        /// <summary>
        /// 根据条件获取课程信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_Course> GetCourseList(string strWhere, string orderby = "")
        {
            return dal.GetCourseList(strWhere, orderby);
        }
        /// <summary>
        /// 根据CourseID获取时间进度和课程状态
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public DataSet GetGourseScheduleCourseState(int CourseID)
        {
            return dal.GetGourseScheduleCourseState(CourseID);
        }
        /// <summary>
        /// 根据CourseID获取课时信息
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodByCourseID(int CourseID)
        {
            return dal.GetCoursePeriodByCourseID(CourseID);
        }
        /// <summary>
        /// 获取课时状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID">课时ID</param>
        /// <returns></returns>
        public string GetCoursePeriodState(int UserID, int CoursePeriodID)
        {
            return dal.GetCoursePeriodState(UserID, CoursePeriodID);
        }

        /// <summary>
        /// 根据课时ID获取课时时间列表
        /// </summary>
        /// <param name="CoursePeriodID">课时ID</param>
        /// <returns></returns>
        public DataSet GetCoursePeriodTime(int CoursePeriodID, int UserID)
        {
            return dal.GetCoursePeriodTime(CoursePeriodID, UserID);
        }
        /// <summary>
        /// 根据UserID和课时时间ID得到CoursePeriodTime状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID">课时时间ID</param>
        /// <returns></returns>
        public string GetCoursePeriodTimeState(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            return dal.GetCoursePeriodTimeState(UserID, CoursePeriodID, CoursePeriodTimeID);
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
            return dal.CommitUserAppoint(UserID, CoursePeriodID, CoursePeriodTimeID);
        }
        /// <summary>
        /// 根据课时ID获取该课时的价格
        /// </summary>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodPrice(string CoursePeriodID)
        {
            return dal.GetCoursePeriodPrice(CoursePeriodID);
        }
        /// <summary>
        /// 根据UserID和CoursePeriodTimeID更新预约表状态为1
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public bool UpdateUserAppointState(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            return dal.UpdateUserAppointState(UserID, CoursePeriodID, CoursePeriodTimeID);
        }
        /// <summary>
        /// 根据UserID和课时ID CoursePeriodID得到预约成功信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public DataSet GetAppointSuccessInfo(string UserID, string CoursePeriodID)
        {
            return dal.GetAppointSuccessInfo(UserID, CoursePeriodID);
        }
        /// <summary>
        /// 根据UserID和课时ID获取已预约的信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public DataSet GetAlreadyAppointInfo(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            return dal.GetAlreadyAppointInfo(UserID, CoursePeriodID, CoursePeriodTimeID);
        }
        /// <summary>
        /// 根据CoursePeriodID和UserID获取直播间信息
        /// </summary>
        /// <param name="CoursePeriodID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetStudioInfo(string UserID, string CoursePeriodID)
        {
            return dal.GetStudioInfo(UserID, CoursePeriodID);
        }
        /// <summary>
        /// 根据UserID获取我的课时信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetMyCoursePeriodUserID(string UserID)
        {
            return dal.GetMyCoursePeriodUserID(UserID);
        }
        /// <summary>
        /// 判断预约的课时是否到达进入教室
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public string IsEnterRoom(int UserID, int CoursePeriodTimeID)
        {
            return dal.IsEnterRoom(UserID, CoursePeriodTimeID);
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
            return dal.UpdateCourseState(ID, State);
        }
        /// <summary>
        /// 根据ID修改Tb_Course课程表课程图片
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ImgUrl"></param>
        /// <returns></returns>
        public bool UpdateCourseImg(string ID, string ImgUrl)
        {
            return dal.UpdateCourseImg(ID, ImgUrl);
        }
        /// <summary>
        /// 根据条件得到课时列表信息
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public DataSet GetCoursePeriodDataSet(string strWhere)
        {
            return dal.GetCoursePeriodDataSet(strWhere);
        }
        /// <summary>
        /// 根据条件获取Tb_CoursePeriod信息集合
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_CoursePeriod> GetCoursePeriodList(string strWhere, string orderby = "")
        {
            return dal.GetCoursePeriodList(strWhere, orderby);
        }
        /// <summary>
        /// 根据ID修改课时状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdateCoursePeriodStatus(string ID, string Status)
        {
            return dal.UpdateCoursePeriodStatus(ID, Status);
        }
        /// <summary>
        /// 根据ID修改课时图片
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ImgUrl"></param>
        /// <returns></returns>
        public bool UpdateCoursePeriodImg(string ID, string ImgUrl)
        {
            return dal.UpdateCoursePeriodImg(ID, ImgUrl);
        }
        /// <summary>
        /// 根据条件获取课时时间信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_CoursePeriodTime> GetCoursePeriodTimeList(string strWhere, string orderby = "")
        {
            return dal.GetCoursePeriodTimeList(strWhere, orderby);
        }
        /// <summary>
        /// 根据条件查询课程统计
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetCourseCount(string strWhere)
        {
            return dal.GetCourseCount(strWhere);
        }
        /// <summary>
        /// 根据条件获取用户预约统计信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetUserAppointCount(string strWhere)
        {
            return dal.GetUserAppointCount(strWhere);
        }
        /// <summary>
        /// 根据条件获取上课用户统计信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetClassUserCount(string strWhere)
        {
            return dal.GetClassUserCount(strWhere);
        }
        /// <summary>
        /// 导入用户预约信息
        /// </summary>
        /// <returns></returns>
        public bool AddUserAppointInfo(string UserID, string UserName, string TrueName, string TelePhone, string CoursePeriodTimeID, string AppointTime, string CourseName, string CoursePeriodName, string StartTime, string EndTime, string NewPrice)
        {
            return dal.AddUserAppointInfo(UserID, UserName, TrueName, TelePhone, CoursePeriodTimeID, AppointTime, CourseName, CoursePeriodName, StartTime, EndTime, NewPrice);
        }
        /// <summary>
        /// 根据条件获取UserInfoTemp表信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetUserInfoTemp(string strWhere)
        {
            return dal.GetUserInfoTemp(strWhere);
        }
        /// <summary>
        /// 根据条件获取预约用户查询课时列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet getCoursePeriodTimeSearch(string strWhere)
        {
            return dal.getCoursePeriodTimeSearch(strWhere);
        }
        #endregion
    }
}
