using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    public static class WorkItemManager
    {
        public const string KeyName1 = "索引维护";
        public const string KeyName2 = "日志维护";
        public const string KeyName3 = "批量更新";
        /// <summary>
        /// 根据多个工作名称查询
        /// </summary>
        /// <param name="names"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static List<SysData_WorkItem> GetWorkItemByNames(string[] names, DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            if (names == null || names.Length < 1)
            {
                return new List<SysData_WorkItem>();
            }
            StringBuilder sb = new StringBuilder();
            foreach (string str in names)
            {
                sb.Append("'").Append(str).Append("',");
            }
            string sql = "select  * from SysData_WorkItem with(nolock) where WorkName in (" + sb.ToString().TrimEnd(',') + ")";
            List<SysData_WorkItem> obj = dc.DB.ExecuteQuery<SysData_WorkItem>(sql).ToList();
            return obj;
        }
        /// <summary>
        /// 监测是否可以爬取
        /// </summary>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static bool CheckPassSpider(DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            List<SysData_WorkItem> list = GetWorkItemByNames(new string[] { KeyName1, KeyName2, KeyName3 }, dc);
            if (list == null || list.Count<1)
            {
                return true;
            }
            List<SysData_WorkItem> list2 = list.Where(obj => obj.IsExec == false).ToList();
            //所有工作已经停止
            if (list2.Count() == list.Count())
            {
                return true;
            }
            return false;
        }

        public static bool SetAllStop(int val)
        {
            DataClass dc = new DataClass();
            string sql = "update SysData_WorkItem with(rowlock) set IsExec=" + val;
            dc.DB.ExecuteCommand(sql);
            return true;
        }
    }
}
