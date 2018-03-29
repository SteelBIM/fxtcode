using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace CAS.Common.MVC4
{
    public class DataTableHelper
    {
        public static List<T> TableToList<T>(DataTable dt) where T:new()
        {
            // 定义集合 
            List<T> ts = new List<T>();
            // 获得此模型的类型 
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性 
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    // 检查DataTable是否包含此列 
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter 
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
        /// <summary>
        /// 将实体数组转换为DataTable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitis"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public static System.Data.DataTable ConvertEntityToDataTable<TEntity>(TEntity[] entitis, string fieldNames) where TEntity : class
        {
            string[] fieldNameArray = fieldNames.Split(char.Parse(","));
            System.Data.DataTable table = new System.Data.DataTable();
            if ((fieldNameArray != null) && (entitis != null) && (entitis.Length > 0))
            {
                Type type = typeof(TEntity);
                Dictionary<DataColumn, PropertyInfo> propDict = new Dictionary<DataColumn, PropertyInfo>();
                int index = 0;
                foreach (var fieldName in fieldNameArray)
                {
                    var prop = type.GetProperty(fieldName);
                    if ((prop != null) && (prop.CanRead))
                    {
                        if ((prop.PropertyType.IsGenericType) && (prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            propDict.Add(table.Columns.Add(fieldName, Nullable.GetUnderlyingType(prop.PropertyType)), prop);
                        }
                        else
                        {
                            propDict.Add(table.Columns.Add(fieldName, prop.PropertyType), prop);
                        }
                    }
                    else
                    {
                        propDict.Add(table.Columns.Add(fieldName), null);
                    }
                    index++;
                }

                foreach (TEntity entity in entitis)
                {
                    DataRow row = table.NewRow();
                    foreach (DataColumn column in table.Columns)
                    {
                        try
                        {
                            object value = propDict[column].GetValue(entity, null);
                            if (value == null)
                            {
                                row[column] = DBNull.Value;
                            }
                            else
                            {
                                row[column] = value;
                            }
                        }
                        catch (Exception err)
                        {
                            throw new Exception(string.Format("Row:{1}\tField:{0}\r\n" + err.Message, propDict[column].Name, table.Rows.Count), err);
                        }
                    }
                    table.Rows.Add(row);
                }
            }

            return table;
        } 
    }
}
