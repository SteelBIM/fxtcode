using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CDI.Utils
{
    /// <summary>
    /// 数据转换工具类
    /// </summary>
    public class ConvUtils<T> where T : new()
    {

        /// <summary>  
        /// 利用反射和泛型  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static List<T> ConvertToList(DataTable dt)
        {

            // 定义集合  
            List<T> ts = new List<T>();

            // 获得此模型的类型  
            Type type = typeof(T);
            //定义一个临时变量  
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性  
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性  
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量  
                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter  
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出  
                        //取值  
                        object value = dr[tempName];

                        //如果非空，则赋给对象的属性  
                        if (value != DBNull.Value)
                        {
                            if (pi.PropertyType == typeof(DateTime))
                            {
                                pi.SetValue(t, DateTime.Parse(Convert.ToString(value)), null);
                            }
                            else if (pi.PropertyType == typeof(int?))
                            {
                                pi.SetValue(t, Convert.ToInt32(value), null);
                            }
                            else if (pi.PropertyType == typeof(string))
                            {
                                pi.SetValue(t, Convert.ToString(value), null);
                            }
                            else if (pi.PropertyType == typeof(decimal?))
                            {
                                pi.SetValue(t, Convert.ToDecimal(value), null);
                            }
                            else
                            {
                                pi.SetValue(t, value, null);
                            }
                        }

                    }
                }
                //对象添加到泛型集合中  
                ts.Add(t);
            }

            return ts;

        }

        /// <summary>
        /// Array To List
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> ArrayConvertToList(T[] t)
        {
            List<T> lst_tmp = new List<T>();
            foreach (T item in t)
            {
                lst_tmp.Add(item);
            }
            return lst_tmp;
        }

    }
}
