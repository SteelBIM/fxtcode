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
    public partial class CourseCount : System.Web.UI.Page
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
                    case "excel":
                        excel();
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
        /// Excel导出
        /// </summary>
        public void excel()
        {
            try
            {
                List<CourseListModel> courseList = GetList();
                //创建Dt
                DataTable dtNew = new DataTable("courseDt");
                dtNew.Columns.Add("课程ID"); 
                dtNew.Columns.Add("课程名称");
                dtNew.Columns.Add("课程预约人数");
                dtNew.Columns.Add("课程上课人数");
                dtNew.Columns.Add("课时ID");
                dtNew.Columns.Add("课时名称");
                dtNew.Columns.Add("课时上课时间");
                dtNew.Columns.Add("课时预约人数");
                dtNew.Columns.Add("课时上课人数"); 
                foreach (var item in courseList)
                {
                    DataRow dr = dtNew.NewRow();
                    dr["课程ID"] = item.CourseID;
                    dr["课程名称"] = item.CourseName;
                    dr["课程预约人数"] = item.CourseAppointNum;
                    dr["课程上课人数"] = item.CourseCount;
                    dr["课时ID"] = item.CoursePeriodID;
                    dr["课时名称"] = item.CoursePeriodName;
                    dr["课时上课时间"] = item.StartTime;
                    dr["课时预约人数"] = item.CoursePeriodAppointNum;
                    dr["课时上课人数"] = item.CoursePeriodCount;
                    dtNew.Rows.Add(dr);
                } 
                MemoryStream s = dtNew.ToExcel() as MemoryStream;
                if (s != null)
                {
                    byte[] excel = s.ToArray();
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename=课程统计列表.xlsx"));
                    Response.AddHeader("Content-Length", excel.Length.ToString());
                    Response.BinaryWrite(excel);
                    s.Close();
                    Response.Flush();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void query()
        {
            try
            {
                int totalcount = 0;
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = new List<CourseListModel>(), total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj1));
                }
                int pageindex = int.Parse(Request.Form["page"].ToString());
                int pagesize = int.Parse(Request.Form["rows"].ToString());
                List<CourseListModel> courseList = GetList();
                if (courseList == null)
                {
                    courseList = new List<CourseListModel>();
                }
                else
                {
                    totalcount = courseList.Count;
                    List<CourseListModel> removelist = new List<CourseListModel>();
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
        /// 获取课程统计列表
        /// </summary>
        /// <returns></returns>
        public List<CourseListModel> GetList()
        {
            try
            {
                string strWhere = "";

                List<CourseListModel> listModel = new List<CourseListModel>();
                List<CourseListModel> courseList = new List<CourseListModel>();

                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = "1=1";
                }
                DataSet set = courseBll.GetCourseCount(strWhere);
                listModel = DataSetToIList<CourseListModel>(set, 0);
                var list = listModel.GroupBy(a => a.CourseID).Select(g => new { CourseID = g.Key, Count = g.Sum(t => t.CoursePeriodCount) });
                Dictionary<int, int> dic = new Dictionary<int, int>();
                foreach (var item in list)
                {
                    dic.Add(item.CourseID, item.Count);
                }
                foreach (CourseListModel item in listModel)
                {
                    foreach (var m in dic)
                    {
                        if (m.Key.Equals(item.CourseID))
                        {
                            item.CourseCount = m.Value;
                            courseList.Add(item);
                            break;
                        }
                    }
                }
                return courseList;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
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
    public class CourseListModel
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        /// <summary>
        /// 课时预约人数
        /// </summary>
        public int CourseAppointNum { get; set; }
        /// <summary>
        /// 课程上课人数
        /// </summary>
        public int CourseCount { get; set; }
        public int CoursePeriodID { get; set; }
        public string CoursePeriodName { get; set; }
        public DateTime StartTime { get; set; }
        public int CoursePeriodTimeID { get; set; }
        /// <summary>
        /// 课时预约人数
        /// </summary>
        public int CoursePeriodAppointNum { get; set; }
        /// <summary>
        /// 课时上课人数
        /// </summary>
        public int CoursePeriodCount { get; set; }
    }
}