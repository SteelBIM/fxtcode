using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DataTableHelper<T>”的 XML 注释
    public class DataTableHelper<T> where T : new()  // 此处一定要加上new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DataTableHelper<T>”的 XML 注释
    {

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DataTableHelper<T>.ConvertToModel(DataTable)”的 XML 注释
        public static IList<T> ConvertToModel(DataTable dt)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DataTableHelper<T>.ConvertToModel(DataTable)”的 XML 注释
        {

            IList<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    }
}
