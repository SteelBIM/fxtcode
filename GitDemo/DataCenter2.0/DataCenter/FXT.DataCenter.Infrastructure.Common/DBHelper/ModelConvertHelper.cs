using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace FXT.DataCenter.Infrastructure.Common.DBHelper
{
    /// <summary>
    /// 实体转换辅助类
    /// </summary>
    public class ModelConvertHelper<T> where T : new()
    {
        public static IList<T> ConvertToModel(DataTable dt)
        {
            // 定义集合
            IList<T> ts = new List<T>();

            // 获得此模型的公共属性
            PropertyInfo[] allPropertys = typeof(T).GetProperties();

            // 获得数据表中具有的属性
            IList<PropertyInfo> hasProperties = new List<PropertyInfo>();
            foreach (PropertyInfo pi in allPropertys)
            {
                // 判断此属性是否有Setter
                if (!pi.CanWrite) continue;

                // 判断此属性是否在查询结果中
                if (dt.Columns.Contains(pi.Name)) hasProperties.Add(pi);
            }

            foreach (DataRow dr in dt.Rows)
            {
                // 产生实例
                T t = new T();

                foreach (PropertyInfo pi in hasProperties)
                {
                    object value = dr[pi.Name];
                    if (value != DBNull.Value)
                        pi.SetValue(t, value, null);
                }

                // 把实例添加到集合
                ts.Add(t);
            }

            return ts;
        }
    }
}
