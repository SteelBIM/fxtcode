using Kingsun.SpokenBroadcas.BLL;
using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.SpokenBroadcas.Web.Controllers
{
    public class IndexController : Controller
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CourseBLL bll = new CourseBLL();
        BaseManagement bm = new BaseManagement();
        UserBLL userBll = new UserBLL();
        //
        // GET: /Index/ 
        public ActionResult Index()
        {
            try
            {
                string UserID = "";
                if (Request["UserID"] == null || Request["UserID"] == "")
                {
                    Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
                    return View();
                }
                else
                {
                    UserID = Request["UserID"];
                }
                SyncUserInfo(UserID);
                ViewData["CourseList"] = GetCourseInfo();
                ViewData["MyCoursePeriodList"]= GetMyCoursePeriod(UserID);
                ViewData["ReMoveCloud"] = @"removecloud();";
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return View();
        }
        /// <summary>
        /// 获取我的课时
        /// </summary>
        /// <returns></returns>
        public List<MyCoursePeriodModel> GetMyCoursePeriod(string UserID)
        {
            try
            {
                List<MyCoursePeriodModel> returnList = new List<MyCoursePeriodModel>();
                DataTable dt = bll.GetMyCoursePeriodUserID(UserID).Tables[0];
                returnList = ObjectExtend.ToList<MyCoursePeriodModel>(dt);
                if (returnList.Count>0)
                {
                    returnList = returnList.OrderBy(a => a.CoursePeriodState).ToList();
                    return returnList;
                } 
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据类型绑定课程信息(0：电影课，1：绘本课)
        /// </summary>
        public List<CourseListModel> GetCourseInfo()
        {
            try
            {
                int pageNumber = 0;
                IList<Tb_Course> courseList = bll.GetCourseList(" Status=1", "Groups ");
                List<CourseListModel> returnList = new List<CourseListModel>();
                if (courseList != null && courseList.Count > 0)
                {
                    DataTable dt = null;
                    foreach (var item in courseList)
                    {
                        dt = bll.GetGourseScheduleCourseState(item.ID).Tables[0];
                        returnList.Add(new CourseListModel()
                        {
                            ID = item.ID,
                            Type = item.Type,
                            Name = item.Name,
                            BigImage = item.BigImage,
                            Groups = item.Groups,
                            Num = item.Num,
                            AppointNum = item.AppointNumShow,
                            QuestionName = Convert.ToInt32(dt.Rows[0]["YCount"]),
                            CourseState = dt.Rows[0]["CourseState"].ToString()
                        });
                    }
                    //returnList = returnList.Skip(pageNumber * 10).Take(10).ToList();
                    returnList = returnList.OrderByDescending(a => a.CourseState).ToList();
                    return returnList;
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 同步当前用户信息
        /// </summary>
        /// <param name="UserID"></param>
        public void SyncUserInfo(string UserID)
        {
            try
            {
                IList<Tb_UserInfo> userList = userBll.GetUserInfo("UserID='" + UserID + "'");
                if (userList == null) //不存在
                {
                    DataTable dtUser = SqlHelper.ExecuteDataset(AppSetting.SyncConnectionString, CommandType.Text, string.Format(" select top 1*from ITSV_Base.[FZ_SynchronousStudy].dbo.[TB_UserInfo] where UserID='{0}'", UserID)).Tables[0];
                    if (dtUser != null && dtUser.Rows.Count > 0)
                    {
                        Tb_UserInfo model = null;
                        foreach (DataRow row in dtUser.Rows)
                        {
                            model = new Tb_UserInfo()
                            {

                                ID = 1,
                                UserID = Convert.ToInt32(row["UserID"]),
                                UserName = row["UserName"] == null ? "" : row["UserName"].ToString(),
                                NickName = row["NickName"] == null ? "" : row["NickName"].ToString(),
                                TrueName = row["TrueName"] == null ? "" : row["TrueName"].ToString(),
                                UserImage = row["UserImage"] == null ? "" : row["UserImage"].ToString(),
                                UserRoles = Convert.ToInt32(string.IsNullOrEmpty(row["UserRoles"] == null ? "" : row["UserRoles"].ToString()) ? 0 : row["UserRoles"]),
                                TelePhone = row["TelePhone"] == null ? "" : row["TelePhone"].ToString(),
                                isFirstLog = 0,
                                CreateTime = DateTime.Now
                            };
                        }
                        userBll.AddUserInfo(model);
                    }
                    else
                    {
                        log.Error(string.Format("同步用户信息失败，UserID={0} 的用户不存在", UserID));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
    }
    /// <summary>
    /// 我的课时
    /// </summary>
    public class MyCoursePeriodModel
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public int CoursePeriodID { get; set; }
        public int CoursePeriodTimeID { get; set; }
        public int Type { get; set; }
        public string BigImage { get; set; }
        public string AppointCoursePeriodName { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime StartTime { get; set; }
        public int AheadMinutes { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int TimeLong { get; set; }
        /// <summary>
        /// 课时状态
        /// </summary>
        public string CoursePeriodState { get; set; }
    }
    public class CourseListModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string BigImage { get; set; }
        /// <summary>
        /// 课程使用人群
        /// </summary>
        public string Groups { get; set; }
        /// <summary>
        /// 课时数
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 已上课时数
        /// </summary>
        public int QuestionName { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public int AppointNum { get; set; }
        /// <summary>
        /// 课程状态
        /// </summary>
        public string CourseState { get; set; }
    }
}
