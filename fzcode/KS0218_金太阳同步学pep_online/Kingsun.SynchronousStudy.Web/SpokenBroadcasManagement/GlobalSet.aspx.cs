using Kingsun.SpokenBroadcas.Common;
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
    public partial class GlobalSet : System.Web.UI.Page
    {
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
                    case "getglobalsetmodel":
                        GetGlobalSetModel();
                        break;
                    case "updateglobalset":
                        UpdateGlobalSet();
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
        /// 查询
        /// </summary>
        public void query()
        {
            try
            {
                string strWhere = "";
                int totalcount = 0;
                List<GlobalSetModel> listModel = new List<GlobalSetModel>();
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
                DataSet set = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteDataset(AppSetting.SpokenConnectionString, CommandType.Text, string.Format("select * from dbo.Tb_GlobalSet where 1=1 {0}", strWhere));
                listModel = DataSetToIList<GlobalSetModel>(set, 0);
                if (listModel == null)
                {
                    listModel = new List<GlobalSetModel>();
                }
                else
                {
                    totalcount = listModel.Count;
                    List<GlobalSetModel> removelist = new List<GlobalSetModel>();
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
        /// 根据ID获取GlobalSet信息
        /// </summary>
        public void GetGlobalSetModel()
        {
            try
            {
                string ID = Request["ID"];
                DataSet set = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteDataset(AppSetting.SpokenConnectionString, CommandType.Text, string.Format("select * from dbo.Tb_GlobalSet where 1=1 and ID='{0}'", ID));
                List<GlobalSetModel> listModel = DataSetToIList<GlobalSetModel>(set, 0);
                if (listModel == null)
                {
                    listModel = new List<GlobalSetModel>();
                }
                string json = JsonHelper.EncodeJson(listModel);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        /// <summary>
        /// 修改GlobalSet信息
        /// </summary>
        public void UpdateGlobalSet()
        {
            try
            {
                string ID = Request["ID"];
                string txtAheadMinutes = Request["txtAheadMinutes"];
                string txtLimitNum = Request["txtLimitNum"];
                List<string> list = new List<string>();
                string upGlobalSet = string.Format(" update Tb_GlobalSet set AheadMinutes='{1}',LimitNum='{2}' where ID='{0}'", ID, txtAheadMinutes, txtLimitNum);
                list.Add(upGlobalSet);
                //修改分钟
                string upCourseAhead = string.Format("update Tb_Course set AheadMinutes='{0}'", txtAheadMinutes);
                list.Add(upCourseAhead);
                string upCoursePeriodAhead = string.Format("update Tb_CoursePeriod set AheadMinutes='{0}'", txtAheadMinutes);
                list.Add(upCoursePeriodAhead);
                //修改限制人数
                string upCoursePeriodLimit = string.Format("update Tb_CoursePeriodTime set LimitNum='{0}'", txtLimitNum);
                list.Add(upCoursePeriodLimit);
                bool flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                if (flag)
                {
                    DataTable dt = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteDataset(AppSetting.SpokenConnectionString, CommandType.Text, "select CoursePeriodID,SUM(LimitNum)LimitNum from dbo.Tb_CoursePeriodTime group by CoursePeriodID").Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        list = new List<string>();
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(string.Format(" update Tb_CoursePeriod set LimitNum='{1}' where ID='{0}'", row["CoursePeriodID"], row["LimitNum"]));
                        }
                        flag = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQueryTransaction(AppSetting.SpokenConnectionString, list);
                        if (flag)
                        {
                            Response.Write("{'state':'1','msg':'修改成功'}");
                        }
                        else
                        {
                            Response.Write("{'state':'0','msg':'修改失败'}");
                        }
                    }
                    else
                    {
                        Response.Write("{'state':'1','msg':'修改成功'}");
                    }
                }
                else
                {
                    Response.Write("{'state':'0','msg':'修改失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'修改异常失败'}");
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
    class GlobalSetModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 设置提前进入时间(分钟)
        /// </summary>
        public int AheadMinutes { get; set; }
        /// <summary>
        /// 人数限制
        /// </summary>
        public int LimitNum { get; set; }
    }
}