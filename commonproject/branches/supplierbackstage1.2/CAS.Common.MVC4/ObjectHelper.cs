using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace CAS.Common.MVC4
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
                returnValue = default(T);
                return false;
            }
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
    }
}
