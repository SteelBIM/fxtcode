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
    public partial class ClassUserCount : System.Web.UI.Page
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
                string strWhere = "";
                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = "and 1=1";
                }
                DataTable dt = courseBll.GetClassUserCount(strWhere).Tables[0];
                dt.Columns[0].ColumnName = "用户ID";
                dt.Columns[1].ColumnName = "用户名";
                dt.Columns[2].ColumnName = "真实姓名";
                dt.Columns[3].ColumnName = "联系方式";
                dt.Columns[4].ColumnName = "课程ID";
                dt.Columns[5].ColumnName = "课程名称";
                dt.Columns[6].ColumnName = "课时ID";
                dt.Columns[7].ColumnName = "课时名称";
                dt.Columns[8].ColumnName = "课时价格";
                dt.Columns[9].ColumnName = "上课时间";
                dt.Columns[10].ColumnName = "最早进入教室时间";
                dt.Columns[11].ColumnName = "最晚退出教室时间";
                dt.Columns[12].ColumnName = "退出次数";
                MemoryStream s = dt.ToExcel() as MemoryStream;
                if (s != null)
                {
                    byte[] excel = s.ToArray();
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename=上课用户列表.xlsx"));
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
                string strWhere = "";
                int totalcount = 0;
                List<Tb_UserLearn> listModel = new List<Tb_UserLearn>();
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = listModel, total = totalcount };
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
                    strWhere = " 1=1";
                }
                DataSet set = courseBll.GetClassUserCount(strWhere);
                listModel = DataSetToIList<Tb_UserLearn>(set, 0);
                if (listModel == null)
                {
                    listModel = new List<Tb_UserLearn>();
                }
                else
                {
                    totalcount = listModel.Count;
                    List<Tb_UserLearn> removelist = new List<Tb_UserLearn>();
                    for (int i = 0; i < listModel.Count; i++)
                    {
                        if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                        {
                            removelist.Add(listModel[i]);
                        }
                    }
                    if (removelist != null && removelist.Count > 0)
                    {
                        for (int i = 0; i < removelist.Count; i++)
                        {
                            listModel.Remove(removelist[i]);
                        }
                    }
                }
                var obj = new { rows = listModel, total = totalcount };
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
    class ClassUserCountModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string TelePhone { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int CoursePeriodID { get; set; }
        public string CoursePeriodName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal NewPrice { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public string CourseStartTime { get; set; }
        /// <summary>
        /// 最早进入教室时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 最晚退出教室时间
        /// </summary>
        public string EndTime { get; set; }
        public int OutTimes { get; set; }
    }
}