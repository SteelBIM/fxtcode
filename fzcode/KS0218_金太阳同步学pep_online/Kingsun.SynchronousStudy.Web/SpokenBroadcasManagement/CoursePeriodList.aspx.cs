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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement
{
    public partial class CoursePeriodList : System.Web.UI.Page
    {
        CourseBLL courseBll = new CourseBLL();
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string Action = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.hiddenCourseID.Value = Request.QueryString["CourseID"];
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
                    case "updatestatus":
                        UpdateStatus();
                        break;
                    case "getcourseperiodmodel":
                        GetCoursePeriodModel();
                        break;
                    case "updatecourseperiodimg":
                        UpdateCoursePeriodImg();
                        break;
                    case "getcourseperiod":
                        GetCoursePeriod();
                        break;
                    case "getcourseperiodtime":
                        GetCoursePeriodTime();
                        break;
                    case "updatecourseperiod":
                        UpdateCoursePeriod();
                        break;
                    case "addcourseperiod":
                        AddCoursePeriod();
                        break;
                    case "addcourseperiodtime":
                        AddCoursePeriodTime();
                        break;
                    case "delcourseperiodtime":
                        DelCoursePeriodTime();
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
        /// 删除课时
        /// </summary>
        public void DelCoursePeriodTime()
        {
            try
            {
                string CoursePeriodTimeID = Request["CoursePeriodTimeID"];
                string CoursePeriodID = Request["CoursePeriodID"];
                int Count = Convert.ToInt32(Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteScalar(AppSetting.SpokenConnectionString, CommandType.Text, "select  isnull(COUNT(1),0)Count from dbo.Tb_CoursePeriod a left join Tb_CoursePeriodTime b on a.ID=b.CoursePeriodID where CoursePeriodID='" + CoursePeriodID + "'"));
                if (Count <= 1)//不存在课时时间了，则删除课时
                {
                    List<string> list = new List<string>();
                    if (CoursePeriodTimeID == "0")
                    {
                        list.Add(string.Format("delete Tb_CoursePeriod where ID='{0}'", CoursePeriodID));
                    }
                    else
                    {
                        list.Add(string.Format("delete Tb_CoursePeriod where ID='{0}'", CoursePeriodID));
                        list.Add(string.Format("delete Tb_CoursePeriodTime where ID='{0}'", CoursePeriodTimeID)); 
                    }
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
                else
                {
                    int count = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, " delete Tb_CoursePeriodTime where ID='" + CoursePeriodTimeID + "'");
                    if (count > 0)
                    {
                        Response.Write("{'state':'1','msg':'删除成功'}");
                    }
                    else
                    {
                        Response.Write("{'state':'0','msg':'删除失败'}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'删除异常失败'}");
            }
        }
        /// <summary>
        /// 添加课时时间信息
        /// </summary>
        public void AddCoursePeriodTime()
        {
            try
            {
                string CoursePeriodID = Request["CoursePeriodID"];
                //课时时间信息
                string txtAddStartTime1 = Request["txtAddStartTime1"];
                string txtAddEndTime1 = Request["txtAddEndTime1"];
                string txtAddTeacherType1 = Request["txtAddTeacherType1"];
                string txtAddStartTime2 = Request["txtAddStartTime2"];
                string txtAddEndTime2 = Request["txtAddEndTime2"];
                string txtAddTeacherType2 = Request["txtAddTeacherType2"];
                string txtAddStartTime3 = Request["txtAddStartTime3"];
                string txtAddEndTime3 = Request["txtAddEndTime3"];
                string txtAddTeacherType3 = Request["txtAddTeacherType3"];
                List<string> list = new List<string>();
                if (txtAddStartTime1 != "" && txtAddEndTime1 != "")
                {
                    list.Add(string.Format(@" insert into Tb_CoursePeriodTime(CoursePeriodID,LimitNum,StartTime,EndTime,TeacherType) 
                    values('{0}',(select top 1 LimitNum from dbo.Tb_GlobalSet),'{1}','{2}','{3}')", CoursePeriodID, txtAddStartTime1, txtAddEndTime1, txtAddTeacherType1));
                }
                if (txtAddStartTime2 != "" && txtAddEndTime2 != "")
                {
                    list.Add(string.Format(@" insert into Tb_CoursePeriodTime(CoursePeriodID,LimitNum,StartTime,EndTime,TeacherType) 
                    values('{0}',(select top 1 LimitNum from dbo.Tb_GlobalSet),'{1}','{2}','{3}')", CoursePeriodID, txtAddStartTime2, txtAddEndTime2, txtAddTeacherType2));
                }
                if (txtAddStartTime3 != "" && txtAddEndTime3 != "")
                {
                    list.Add(string.Format(@" insert into Tb_CoursePeriodTime(CoursePeriodID,LimitNum,StartTime,EndTime,TeacherType) 
                    values('{0}',(select top 1 LimitNum from dbo.Tb_GlobalSet),'{1}','{2}','{3}')", CoursePeriodID, txtAddStartTime3, txtAddEndTime3, txtAddTeacherType3));
                }
                if (list.Count > 0)
                {
                    list.Add(string.Format(" update Tb_CoursePeriod set LimitNum+=(select top 1 LimitNum*{1} from dbo.Tb_GlobalSet) where ID='{0}'", CoursePeriodID, list.Count));
                }
                bool flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                if (flag)
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
        /// 新增课时信息
        /// </summary>
        public void AddCoursePeriod()
        {
            try
            {
                string CourseID = Request["CourseID"];
                //课时信息
                string txtName = Request["txtName"].Replace("'", "\"");
                string selStatus = Request["selStatus"];
                string selType = Request["selType"];
                string txtPrice = Request["txtPrice"];
                string txtNewPrice = Request["txtNewPrice"];
                string txtSummary = Request["txtSummary"].Replace("'", "\"");
                string ImgUrl = Request["ImgUrl"];
                //课时时间信息
                string txtAddStartTime1 = Request["txtAddStartTime1"];
                string txtAddEndTime1 = Request["txtAddEndTime1"];
                string txtAddTeacherType1 = Request["txtAddTeacherType1"];
                string txtAddStartTime2 = Request["txtAddStartTime2"];
                string txtAddEndTime2 = Request["txtAddEndTime2"];
                string txtAddTeacherType2 = Request["txtAddTeacherType2"];
                string txtAddStartTime3 = Request["txtAddStartTime3"];
                string txtAddEndTime3 = Request["txtAddEndTime3"];
                string txtAddTeacherType3 = Request["txtAddTeacherType3"];

                string sql_coursePeriod = string.Format(@" insert into Tb_CoursePeriod(ID,CourseID,Name,Summary,Price,NewPrice,Image,BigImage,AheadMinutes,LimitNum,Status)
                values((select MAX(ID)+1 from Tb_CoursePeriod),'{0}','{1}','{2}','{3}','{4}','{5}','{6}',
                        (select top 1 AheadMinutes from dbo.Tb_GlobalSet),(select top 1 LimitNum from dbo.Tb_GlobalSet),'{7}');SELECT MAX(ID)from Tb_CoursePeriod", CourseID, txtName, txtSummary, txtPrice, txtNewPrice, ImgUrl, ImgUrl, selStatus);
                object CoursePeriodID = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteScalar(AppSetting.SpokenConnectionString, CommandType.Text, sql_coursePeriod);
                if (CoursePeriodID == null)
                {
                    CoursePeriodID = "";
                    Response.Write("{'state':'0','msg':'添加失败'}");
                }
                else
                {
                    if (CoursePeriodID.ToString() == "")
                    {
                        CoursePeriodID = "";
                        Response.Write("{'state':'0','msg':'添加失败'}");
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        if (txtAddStartTime1 != "" && txtAddEndTime1 != "")
                        {
                            list.Add(string.Format(@" insert into Tb_CoursePeriodTime(CoursePeriodID,LimitNum,StartTime,EndTime,TeacherType) 
                    values('{0}',(select top 1 LimitNum from dbo.Tb_GlobalSet),'{1}','{2}','{3}')", CoursePeriodID, txtAddStartTime1, txtAddEndTime1, txtAddTeacherType1));
                        }
                        if (txtAddStartTime2 != "" && txtAddEndTime2 != "")
                        {
                            list.Add(string.Format(@" insert into Tb_CoursePeriodTime(CoursePeriodID,LimitNum,StartTime,EndTime,TeacherType) 
                    values('{0}',(select top 1 LimitNum from dbo.Tb_GlobalSet),'{1}','{2}','{3}')", CoursePeriodID, txtAddStartTime2, txtAddEndTime2, txtAddTeacherType2));
                        }
                        if (txtAddStartTime3 != "" && txtAddEndTime3 != "")
                        {
                            list.Add(string.Format(@" insert into Tb_CoursePeriodTime(CoursePeriodID,LimitNum,StartTime,EndTime,TeacherType) 
                    values('{0}',(select top 1 LimitNum from dbo.Tb_GlobalSet),'{1}','{2}','{3}')", CoursePeriodID, txtAddStartTime3, txtAddEndTime3, txtAddTeacherType3));
                        }
                        if (list.Count > 0)
                        {
                            list.Add(string.Format(" update Tb_CoursePeriod set LimitNum=(select top 1 LimitNum*{1} from dbo.Tb_GlobalSet) where ID='{0}'", CoursePeriodID, list.Count));
                        }
                        bool flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                        if (flag)
                        {
                            Response.Write("{'state':'1','msg':'添加成功'}");
                        }
                        else
                        {
                            Response.Write("{'state':'0','msg':'添加失败'}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'添加异常失败'}");
            }
        }
        /// <summary>
        /// 修改课时和课时时间表
        /// </summary>
        public void UpdateCoursePeriod()
        {
            try
            {
                string CoursePeriodID = Request["CoursePeriodID"];
                string txtName = Request["txtName"].Replace("'", "\"");
                string txtPrice = Request["txtPrice"];
                string txtNewPrice = Request["txtNewPrice"];
                //string txtAheadMinutes = Request["txtAheadMinutes"];
                //string txtLimitNum = Request["txtLimitNum"];
                string txtSummary = Request["txtSummary"].Replace("'", "\"");
                //课时信息
                string CoursePeriodTimeID = Request["CoursePeriodTimeID"];
                //string txtTimeLimitNum = Request["txtTimeLimitNum"];
                string txtStartTime = Request["txtStartTime"];
                string txtEndTime = Request["txtEndTime"];
                string txtTeacherType = Request["txtTeacherType"];
                string sql_CourseProid = string.Format(@"update Tb_CoursePeriod set Name='{1}',Summary='{2}',Price='{3}',NewPrice='{4}'  where ID='{0}'",
                    CoursePeriodID, txtName, txtSummary, txtPrice, txtNewPrice);
                string sql_CoursePeriodTime = string.Format("update Tb_CoursePeriodTime set StartTime='{1}',EndTime='{2}',TeacherType='{3}' where ID='{0}'",
                    CoursePeriodTimeID, txtStartTime, txtEndTime, txtTeacherType);
                List<string> list = new List<string>();
                list.Add(sql_CourseProid);
                list.Add(sql_CoursePeriodTime);
                bool flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
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
        /// 修改课程图片
        /// </summary>
        public void UpdateCoursePeriodImg()
        {
            try
            {
                string ID = Request["ID"];
                string ImgUrl = Request["ImgUrl"];
                bool flag = courseBll.UpdateCoursePeriodImg(ID, ImgUrl);
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
        /// 根据ID修改Tb_CoursePeriod课时表课时状态
        /// </summary>
        public void UpdateStatus()
        {
            try
            {
                string ID = Request["ID"];
                string State = Request["State"];
                bool flag = courseBll.UpdateCoursePeriodStatus(ID, State);
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
        /// 获取课时信息
        /// </summary>
        public void GetCoursePeriod()
        {
            try
            {
                string CoursePeriodID = Request.Form["CoursePeriodID"];
                IList<Tb_CoursePeriod> courseList = courseBll.GetCoursePeriodList("ID=" + CoursePeriodID);
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
        /// 获取课时时间信息
        /// </summary>
        public void GetCoursePeriodTime()
        {
            try
            {
                string CoursePeriodTimeID = Request.Form["CoursePeriodTimeID"];
                IList<Tb_CoursePeriodTime> courseList = courseBll.GetCoursePeriodTimeList("ID=" + CoursePeriodTimeID);
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
        /// 根据ID获取课时信息
        /// </summary>
        public void GetCoursePeriodModel()
        {
            try
            {
                string ID = Request.Form["ID"];
                IList<Tb_CoursePeriod> courseList = courseBll.GetCoursePeriodList("ID=" + ID);
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
                List<CoursePeriodModel> courseList = new List<CoursePeriodModel>();
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
                DataSet set = courseBll.GetCoursePeriodDataSet(strWhere);
                courseList = DataSetToIList<CoursePeriodModel>(set, 0);
                if (courseList == null)
                {
                    courseList = new List<CoursePeriodModel>();
                }
                else
                {
                    totalcount = courseList.Count;
                    List<CoursePeriodModel> removelist = new List<CoursePeriodModel>();
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
        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }

    public class CoursePeriodModel
    {
        /// <summary>
        /// 课时时间ID
        /// </summary>
        public int CoursePeriodTimeID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StudioUrl { get; set; }
        public string StudioCommand { get; set; }
        /// <summary>
        /// 课时ID
        /// </summary>
        public int CoursePeriodID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal NewPrice { get; set; }
        public string BigImage { get; set; }
        /// <summary>
        /// 课时状态
        /// </summary>
        public int Status { get; set; }
    }
}