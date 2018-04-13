using Kingsun.SpokenBroadcas.BLL;
using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement
{

    public partial class CourseList : System.Web.UI.Page
    {
        CourseBLL courseBll = new CourseBLL();
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string Action = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActionInit();
            }
        }
        private void ActionInit()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数 
                {
                    Action = Request.QueryString["Action"].Trim().ToLower();//去掉空格并变小写 
                }
                else
                {
                    return;
                }
                switch (Action)
                {
                    case "query":
                        query();
                        break;
                    case "updatestate":
                        UpdateState();
                        break;
                    case "getcoursemodel":
                        GetCourseModel();
                        break;
                    case "updatecourseimg":
                        UpdateCourseImg();
                        break;
                    case "updatecourse":
                        UpdateCourse();
                        break;
                    case "addcourse":
                        AddCourse();
                        break;
                    case "delcourse":
                        DelCourse();
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            Response.End();
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        public void DelCourse()
        {
            try
            {
                string CourseID = Request["ID"];
                List<string> list = new List<string>();
                int CoursePeriodTimeCount = Convert.ToInt32(Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteScalar(AppSetting.SpokenConnectionString, CommandType.Text, "   select ISNULL(COUNT(1),0) from  Tb_CoursePeriodTime where CoursePeriodID in(select ID from Tb_CoursePeriod where CourseID='" + CourseID + "')"));
                if (CoursePeriodTimeCount >= 1)
                {
                    list.Add(string.Format("delete Tb_CoursePeriodTime where CoursePeriodID in(select ID from Tb_CoursePeriod where CourseID='{0}')", CourseID)); 
                }
                int CoursePeriodCount = Convert.ToInt32(Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteScalar(AppSetting.SpokenConnectionString, CommandType.Text, "   select ISNULL(COUNT(1),0)from Tb_CoursePeriod where CourseID='" + CourseID + "'"));
                if (CoursePeriodCount >= 1)
                {
                    list.Add(string.Format("delete Tb_CoursePeriod where CourseID='{0}'", CourseID));
                }
                list.Add(string.Format("delete  Tb_Course where ID='{0}'", CourseID));
                bool flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'删除成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'删除失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'删除异常失败'}");
            }
        }
        public void AddCourse()
        {
            try
            {
                string txtName = Request["txtName"].Replace("'", "\"");
                string selStatus = Request["selStatus"];
                string selType = Request["selType"];
                string txtGroups = Request["txtGroups"];
                string txtAheadMinutes = Request["txtAheadMinutes"];
                string txtNum = Request["txtNum"];
                string txtOpenDate = Request["txtOpenDate"];
                string txtSummary = Request["txtSummary"].Replace("'", "\"");
                string ImgUrl = Request["ImgUrl"];
                string txtStudioUrl = Request["txtStudioUrl"].ToString().Trim();
                string txtStudioCommand = Request["txtStudioCommand"].ToString().Trim();
                string sql = string.Format(@" insert into Tb_Course(ID,Name,Image,BigImage,Type,Summary,Groups,Num,AheadMinutes,OpenDate,Status,StudioUrl,StudioCommand)
                values((select MAX(ID)+1 from Tb_Course),'{0}','{1}','{2}','{3}','{4}','{5}','{6}',(select top 1 AheadMinutes from dbo.Tb_GlobalSet),'{7}','{8}','{9}','{10}')", txtName, ImgUrl, ImgUrl, selType, txtSummary, txtGroups, txtNum, txtOpenDate, selStatus, txtStudioUrl, txtStudioCommand);
                int flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                if (flag > 0)
                {
                    Response.Write("{'state':'1','msg':'添加成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'添加失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'添加异常失败'}");
            }
        }
        /// <summary>
        /// 修改课程信息
        /// </summary>
        public void UpdateCourse()
        {
            try
            {
                string CourseID = Request["CourseID"];
                string txtName = Request["txtName"].Replace("'", "\"");
                string selType = Request["selType"];
                string txtGroups = Request["txtGroups"];
                string txtAheadMinutes = Request["txtAheadMinutes"];
                string txtNum = Request["txtNum"];
                string txtOpenDate = Request["txtOpenDate"];
                string txtSummary = Request["txtSummary"].Replace("'", "\"");
                string txtStudioUrl = Request["txtStudioUrl"].ToString().Trim();
                string txtStudioCommand = Request["txtStudioCommand"].ToString().Trim();
                string sql = string.Format(@"update Tb_Course set Name='{1}',Type='{2}',Summary='{3}',Groups='{4}',Num='{5}',AheadMinutes='{6}',OpenDate='{7}',StudioUrl='{8}',StudioCommand='{9}' where ID='{0}'", CourseID,
                    txtName, selType, txtSummary, txtGroups, txtNum, txtAheadMinutes, txtOpenDate, txtStudioUrl, txtStudioCommand);
                int flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                if (flag > 0)
                {
                    Response.Write("{'state':'1','msg':'修改成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'修改失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("");
            }
        }
        /// <summary>
        /// 修改课程图片
        /// </summary>
        public void UpdateCourseImg()
        {
            try
            {
                string ID = Request["ID"];
                string ImgUrl = Request["ImgUrl"];
                bool flag = courseBll.UpdateCourseImg(ID, ImgUrl);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'修改成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'修改失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'修改失败'}");
            }
        }
        /// <summary>
        /// 根据ID修改Tb_Course课程表课程状态
        /// </summary>
        public void UpdateState()
        {
            try
            {
                string ID = Request["ID"];
                string State = Request["State"];
                bool flag = courseBll.UpdateCourseState(ID, State);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'修改成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'修改失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'修改失败'}");
            }
        }
        /// <summary>
        /// 根据ID获取课程信息
        /// </summary>
        public void GetCourseModel()
        {
            try
            {
                string ID = Request.Form["ID"];
                IList<Tb_Course> courseList = courseBll.GetCourseList("ID=" + ID);
                string json = JsonHelper.EncodeJson(courseList);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("");
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void query()
        {
            try
            {
                string strWhere = "";
                int totalcount = 0;
                IList<Tb_Course> courseList = new List<Tb_Course>();
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = courseList, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj1));
                }
                int pageindex = int.Parse(Request.Form["page"].ToString());
                int pagesize = int.Parse(Request.Form["rows"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = "1=1";
                }

                courseList = courseBll.GetCourseList(strWhere);
                if (courseList == null)
                {
                    courseList = new List<Tb_Course>();
                }
                else
                {
                    totalcount = courseList.Count;
                    IList<Tb_Course> removelist = new List<Tb_Course>();
                    for (int i = 0; i < courseList.Count; i++)
                    {
                        if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                        {
                            removelist.Add(courseList[i]);
                        }
                    }
                    if (removelist != null && removelist.Count > 0)
                    {
                        for (int i = 0; i < removelist.Count; i++)
                        {
                            courseList.Remove(removelist[i]);
                        }
                    }
                }
                var obj = new { rows = courseList, total = totalcount };
                Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
    }
}