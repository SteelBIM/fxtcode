using System;
using System.Linq;
using System.Reflection;

namespace CAS.Common
{
    public static class ObjectHelper
    {

        /// <summary>
        /// 利用反射，动态获取一个 Property 的值（一般用于获得匿名类型的实例的值）
        /// </summary>
        /// <typeparam name="T">期望返回的类型</typeparam>
        /// <param name="obj">要获得的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>是否成功，如果成功，则返回等效的值，否则返回默认值</returns>
        public static bool GetPropertyValue<T>(this object obj, string propertyName, out T returnValue)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            //DisplayAttribute[] attrs = (DisplayAttribute[])property.GetCustomAttributes(typeof(DisplayAttribute),
            //    false);
           
            if (property == null)
            {
                returnValue = default(T);
                return false;
            }
            object value = property.GetValue(obj, null);
            if (value is T)
            {
                returnValue = (T)value;
                return true;
            }
            else
            {
                returnValue = (T)value;
                //returnValue = default(T);
                return false;
            }
        }

        /// <summary>
        /// 直接获取属性对应值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyValue<T>(this object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            //DisplayAttribute[] attrs = (DisplayAttribute[])property.GetCustomAttributes(typeof(DisplayAttribute),
            //    false);

            if (property == null)
            {
                return string.Empty;
            }
            object value = property.GetValue(obj, null);
            return Convert.ToString(value);
        }
        /// <summary>
        /// 直接获取属性对应值,可设置传入属性的特点 ,如 忽略大小写BindingFlags.IgnoreCase 等
        /// 潘锦发  20151222
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="flag">属性标志特点</param>
        /// <returns></returns>
        public static string GetPropertyValue<T>(this object obj, string propertyName,BindingFlags flag)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName, flag);
            if (property == null)
            {
                return string.Empty;
            }
            object value = property.GetValue(obj, null);
            return Convert.ToString(value);
        }

        /// <summary>
        /// 利用反射，动态获取一个 Property 的值（一般用于获得匿名类型的实例的值）
        /// </summary>
        /// <typeparam name="T">期望返回的类型</typeparam>
        /// <param name="obj">要获得的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>是否成功，如果成功，则返回等效的值，否则返回默认值</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyName, T defaultValue)
        {
            T returnValue;
            return GetPropertyValue<T>(obj, propertyName, out returnValue) ? returnValue : defaultValue;
        }

        /// <summary>
        /// 利用反射，动态设置一个 Property 的值（一般用于设置具有 public setter 类型的实例的值）
        /// </summary>
        /// <param name="obj">要设置的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">要设置的值</param>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);

            if (property != null)
            {
                property.SetValue(obj, value, null);
            }
        }

        /// <summary>
        /// 复制实体,创建人：rock,20150312
        /// 20161206 rock 不限制参数类型改为object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CopyEntity<T>(this object obj)
        {
            if (obj == null)
            {
                return default(T);
            }
            T result = System.Activator.CreateInstance<T>();
            PropertyInfo[] ps = obj.GetType().GetProperties();
            foreach (PropertyInfo p in ps)
            {
                var property = result.GetType().GetProperties()
                         .Where(pInfo => pInfo.Name.ToLower().Equals(p.Name.ToLower())).FirstOrDefault();
                if (property != null)
                {
                    if (property.CanWrite)/*判断属性是否可写是否有set*/
                    {
                        property.SetValue(result, p.GetValue(obj, null), null);
                    }
                }
            }
            return result;
        }
    }
}
