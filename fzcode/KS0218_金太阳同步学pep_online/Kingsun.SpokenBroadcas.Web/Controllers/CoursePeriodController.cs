using Kingsun.SpokenBroadcas.BLL;
using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.SpokenBroadcas.Web.Controllers
{
    public class CoursePeriodController : Controller
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /CoursePeriod/
        CourseBLL bll = new CourseBLL();
        UserBLL userBll = new UserBLL();
        BaseManagement bm = new BaseManagement();
        public ActionResult Index()
        {
            try
            {
                int UserID = 0;
                int CourseID = 0;
                if (Request["UserID"] != null)
                {
                    UserID = Convert.ToInt32(Request["UserID"]);
                }
                else
                {
                    Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
                    return View();
                }
                if (Request["CourseID"] == null || Request["CourseID"] == "")
                {
                    Response.Write("<script>alert('参数错误！CourseID不能为空！')</script>");
                    return View();
                }
                else
                {
                    CourseID = Convert.ToInt32(Request["CourseID"]);
                }
                IList<Tb_Course> courseList = bll.GetCourseList("ID=" + CourseID + " and Status=1");
                if (courseList != null && courseList.Count > 0)
                {
                    ViewData["courseList"] = courseList;
                    DataTable dt = bll.GetCoursePeriodByCourseID(CourseID).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<CoursePeriodListModel> returnList = new List<CoursePeriodListModel>();
                        string CoursePeriodState = "";
                        DateTime strStartTime;
                        foreach (DataRow row in dt.Rows)
                        {
                            CoursePeriodState = bll.GetCoursePeriodState(UserID, Convert.ToInt32(row["ID"]));
                            if (row["StartTime"] == null || row["StartTime"].ToString() == "")
                            {
                                strStartTime = DateTime.Now;
                            }
                            else
                            {
                                strStartTime = Convert.ToDateTime(row["StartTime"]);
                            }
                            returnList.Add(new CoursePeriodListModel()
                            {
                                ID = Convert.ToInt32(row["ID"]),
                                CourseID = CourseID,
                                UserID = UserID,
                                Name = row["Name"].ToString(),
                                Image = row["Image"].ToString(),
                                Price = Convert.ToDouble(row["Price"]),
                                NewPrice = Convert.ToDouble(row["NewPrice"]),
                                AppointNumShow = Convert.ToInt32(row["AppointNumShow"]),
                                StartTime = strStartTime,
                                CoursePeriodState = CoursePeriodState
                            });
                        }
                        returnList = returnList.OrderByDescending(a => a.CoursePeriodState != "已结束").ToList();
                        ViewData["returnList"] = returnList;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return View();
        }
        /// <summary>
        /// 根据课时ID获取课时时间列表
        /// </summary>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public JsonResult GetCoursePeriodTime(int CoursePeriodID, int UserID)
        {
            try
            {
                DataSet set = bll.GetCoursePeriodTime(CoursePeriodID, UserID);
                string json = JsonConvert.SerializeObject(set.Tables[0]);
                return Json(new { Success = true, Data = json, Msg = "" });
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "当前操作发生异常" });
            }
        }
        /// <summary>
        ///  根据UserID和课时时间ID得到CoursePeriodTime状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public JsonResult GetCoursePeriodTimeState(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                string CoursePeriodTimeState = "";
                CoursePeriodTimeState = bll.GetCoursePeriodTimeState(UserID, CoursePeriodID, CoursePeriodTimeID);
                if (CoursePeriodTimeState == "")
                {
                    return Json(new { Success = false, Data = "预约失败", Msg = "预约失败" });
                }
                else
                {
                    return Json(new { Success = true, Data = CoursePeriodTimeState, Msg = CoursePeriodTimeState });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常" });
            }
        }
        /// <summary>
        /// 提交用户预约信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public JsonResult CommitUserAppoint(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                bool flag = bll.CommitUserAppoint(UserID, CoursePeriodID, CoursePeriodTimeID);
                if (flag)
                {
                    DataTable dt = bll.GetCoursePeriodPrice(CoursePeriodID).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string json = JsonConvert.SerializeObject(dt);
                        return Json(new { Success = true, Data = json, Msg = "预约成功" });//预约成功
                    }
                    else
                    {
                        return Json(new { Success = false, Data = "", Msg = "预约失败" });
                    }
                }
                else
                {
                    return Json(new { Success = false, Data = "", Msg = "预约失败" });
                }

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "当前操作发生异常" });
            }
        }
        /// <summary>
        /// 根据UserID和CoursePeriodTimeID更新预约表状态为1
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public JsonResult UpdateUserAppointState(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                string CoursePeriodTimeState = bll.GetCoursePeriodTimeState(UserID, CoursePeriodID, CoursePeriodTimeID);
                if (CoursePeriodTimeState != "可预约")
                {
                    return Json(new { Success = false, Data = "", Msg = "修改失败" });
                }
                bool flag = bll.UpdateUserAppointState(UserID, CoursePeriodID, CoursePeriodTimeID);
                if (flag)
                {
                    return Json(new { Success = true, Data = "", Msg = "修改成功" });
                }
                else
                {
                    return Json(new { Success = false, Data = "", Msg = "修改失败" });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "当前操作发生异常" });
            }
        }
        /// <summary>
        /// 根据UserID和课时ID获取已预约的信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodID"></param>
        /// <returns></returns>
        public JsonResult GetAlreadyAppointInfo(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                DataTable dt = bll.GetAlreadyAppointInfo(UserID, CoursePeriodID, CoursePeriodTimeID).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string json = JsonConvert.SerializeObject(dt);
                    return Json(new { Success = true, Data = json, Msg = "成功" });
                }
                else
                {
                    return Json(new { Success = false, Data = "", Msg = "无数据" });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "当前操作发生异常" });
            }
        }
        /// <summary>
        /// 根据UserID返回用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult GetUserInfo(string UserID)
        {
            try
            {
                if (string.IsNullOrEmpty(UserID))
                {
                    return Json(new { Success = false, Data = "", Msg = "参数错误！UserID不能为空！" });
                }
                IList<Tb_UserInfo> userList = userBll.GetUserInfo("UserID='" + UserID + "'");
                if (userList == null) //不存在
                {
                    DataTable dtUser = SqlHelper.ExecuteDataset(AppSetting.SyncConnectionString, CommandType.Text, string.Format(" select top 1*from ITSV_Base.[FZ_SynchronousStudy].dbo[TB_UserInfo] where UserID='{0}'", UserID)).Tables[0];
                    // bm.SyncExecuteSql(string.Format(" select top 1*from TB_UserInfo where UserID='{0}'", UserID)).Tables[0];   
                    if (dtUser != null && dtUser.Rows.Count > 0)
                    {
                        Tb_UserInfo model = null;
                        foreach (DataRow row in dtUser.Rows)
                        {
                            model = new Tb_UserInfo()
                            {
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
                        userList = userBll.GetUserInfo("UserID='" + UserID + "'");
                    }
                    else
                    {
                        log.Error(string.Format("同步用户信息失败，UserID={0} 的用户不存在", UserID));
                        return Json(new { Success = false, Data = "", Msg = "当前用户不存在" });
                    }
                }
                string json = JsonConvert.SerializeObject(userList);
                return Json(new { Success = true, Data = json, Msg = "" });
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常" });
            }
        }
        /// <summary>
        /// 根据CoursePeriodID和UserID获取直播间信息
        /// </summary>
        /// <param name="CoursePeriodID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult GetStudioInfo(string UserID, string CoursePeriodID, string CoursePeriodTimeID)
        {
            try
            {
                string CoursePeriodState = bll.IsEnterRoom(Convert.ToInt32(UserID), Convert.ToInt32(CoursePeriodTimeID));
                if (CoursePeriodState == "进入教室")
                {
                    DataTable dt = bll.GetStudioInfo(UserID, CoursePeriodID).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string json = JsonConvert.SerializeObject(dt);
                        return Json(new { Success = true, Data = json, Msg = "成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Data = "", Msg = "'该课时暂未开播！'" });
                    }
                }
                else
                {
                    return Json(new { Success = false, Data = "", Msg = CoursePeriodState });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "当前操作发生异常" });
            }
        }
        UserLearnBLL userLearnBll = new UserLearnBLL();
        /// <summary>
        /// 退出教室
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public JsonResult OutRoom(int UserID, int CoursePeriodTimeID)
        {
            try
            {
                IList<Tb_UserLearn> list = userLearnBll.GetUserLearn("UserID='" + UserID + "' and CoursePeriodTimeID='" + CoursePeriodTimeID + "'");
                if (list != null && list.Count > 0)//存在
                {
                    string sql = string.Format(@"update Tb_UserLearn set OutTimes=OutTimes+1,EndTime='{2}' where UserID='{0}' and CoursePeriodTimeID='{1}'", UserID, CoursePeriodTimeID, DateTime.Now);
                    int flagOutTimes = SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                    if (flagOutTimes > 0)
                    {
                        return Json(new { Success = true, Data = "", Msg = "退出成功" });
                    }
                    else
                    {
                        string info = string.Format("退出教室记录退出次数和时间，方法：OutRoom，参数：UserID={0}，CoursePeriodTimeID={1}。", UserID, CoursePeriodTimeID);
                        log.Error(info);
                        return Json(new { Success = false, Data = "", Msg = "退出失败" });
                    }
                }
                else//不存在，则添加一条学习记录
                {
                    IList<Tb_UserInfo> userModel = userBll.GetUserInfo("UserID='" + UserID + "'");
                    DataTable dt = userLearnBll.GetCoursePeriodByCoursePeriodTimeID(CoursePeriodTimeID).Tables[0];
                    if (userModel == null || dt == null || dt.Rows.Count <= 0)
                    {
                        string info = string.Format("添加用户学习信息失败(Tb_UserInfo和课时信息为空)，方法：OutRoom，参数：UserID={0}，CoursePeriodTimeID={1}。", UserID, CoursePeriodTimeID);
                        log.Info(info);
                        return Json(new { Success = false, Data = "", Msg = "添加失败" });
                    }
                    Tb_UserLearn model = new Tb_UserLearn()
                    {
                        ID = 0,
                        UserID = UserID,
                        CoursePeriodTimeID = CoursePeriodTimeID,
                        OutTimes = 1,
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now,
                        CreateTime = DateTime.Now,
                        UserName = userModel[0].UserName,
                        TrueName = userModel[0].TrueName,
                        TelePhone = userModel[0].TelePhone,
                        CourseID = Convert.ToInt32(dt.Rows[0]["CourseID"]),
                        CourseName = dt.Rows[0]["CourseName"].ToString(),
                        CoursePeriodID = Convert.ToInt32(dt.Rows[0]["CoursePeriodID"]),
                        CoursePeriodName = dt.Rows[0]["CoursePeriodName"].ToString(),
                        NewPrice = Decimal.Parse(dt.Rows[0]["NewPrice"].ToString()),
                        CourseStartTime = dt.Rows[0]["CourseStartTime"].ToString()
                    };
                    bool flag = userLearnBll.AddUserLearn(model);
                    if (flag)
                    {
                        return Json(new { Success = true, Data = "", Msg = "添加成功" });
                    }
                    else
                    {
                        string info = string.Format("退出教室，记录不存在则添加一条学习记录，方法：OutRoom，参数：UserID={0}，CoursePeriodTimeID={1}。", UserID, CoursePeriodTimeID);
                        log.Error(info);
                        return Json(new { Success = false, Data = "", Msg = "添加失败" });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "当前操作发生异常" });
            }
        }
        /// <summary>
        /// 添加用户学习信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <param name="OutTimes"></param>
        /// <returns></returns>
        public JsonResult AddUserLearn(int UserID, int CoursePeriodTimeID)
        {
            try
            {
                IList<Tb_UserLearn> list = userLearnBll.GetUserLearn("UserID='" + UserID + "' and CoursePeriodTimeID='" + CoursePeriodTimeID + "'");
                if (list != null && list.Count > 0)//存在
                {
                    //string sql = string.Format(@"update Tb_UserLearn set OutTimes=OutTimes+1 where UserID='{0}' and CoursePeriodTimeID='{1}'", UserID, CoursePeriodTimeID);
                    //int flagOutTimes = SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                    //if (flagOutTimes > 0)
                    //{
                    //    return Json(new { Success = true, Data = "", Msg = "添加成功" });
                    //}
                    //else
                    //{
                    //    string info = string.Format("添加用户学习信息失败(学习记录已存在情况)，方法：AddUserLearn，参数：UserID={0}，CoursePeriodTimeID={1}。", UserID, CoursePeriodTimeID);
                    //    log.Info(info);
                    //    return Json(new { Success = false, Data = "", Msg = "添加失败" });
                    //}
                    return Json(new { Success = true, Data = "", Msg = "添加成功" });
                }
                IList<Tb_UserInfo> userModel = userBll.GetUserInfo("UserID='" + UserID + "'");
                DataTable dt = userLearnBll.GetCoursePeriodByCoursePeriodTimeID(CoursePeriodTimeID).Tables[0];
                if (userModel == null || dt == null || dt.Rows.Count <= 0)
                {
                    string info = string.Format("添加用户学习信息失败(Tb_UserInfo和课时信息为空)，方法：AddUserLearn，参数：UserID={0}，CoursePeriodTimeID={1}。", UserID, CoursePeriodTimeID);
                    log.Info(info);
                    return Json(new { Success = false, Data = "", Msg = "添加失败" });
                }
                Tb_UserLearn model = new Tb_UserLearn()
                {
                    ID = 0,
                    UserID = UserID,
                    CoursePeriodTimeID = CoursePeriodTimeID,
                    OutTimes = 0,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UserName = userModel[0].UserName,
                    TrueName = userModel[0].TrueName,
                    TelePhone = userModel[0].TelePhone,
                    CourseID = Convert.ToInt32(dt.Rows[0]["CourseID"]),
                    CourseName = dt.Rows[0]["CourseName"].ToString(),
                    CoursePeriodID = Convert.ToInt32(dt.Rows[0]["CoursePeriodID"]),
                    CoursePeriodName = dt.Rows[0]["CoursePeriodName"].ToString(),
                    NewPrice =Decimal.Parse(dt.Rows[0]["NewPrice"].ToString()),
                    CourseStartTime = dt.Rows[0]["CourseStartTime"].ToString()
                };
                bool flag = userLearnBll.AddUserLearn(model);
                if (flag)
                {
                    return Json(new { Success = true, Data = "", Msg = "添加成功" });
                }
                else
                {
                    string info = string.Format("添加用户学习信息失败，方法：AddUserLearn，参数：UserID={0}，CoursePeriodTimeID={1}。", UserID, CoursePeriodTimeID);
                    log.Error(info);
                    return Json(new { Success = false, Data = "", Msg = "添加失败" });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "当前操作发生异常" });
            }
        }
    }

    public class CoursePeriodListModel
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public double NewPrice { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public int AppointNum { get; set; }
        /// <summary>
        /// 显示预约人数
        /// </summary>
        public int AppointNumShow { get; set; }
        public DateTime StartTime { get; set; }
        public string CoursePeriodState { get; set; }
    }
}
