using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ObjectHelper”的 XML 注释
    public static class ObjectHelper
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ObjectHelper”的 XML 注释
    {

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ObjectHelper.As<T>(object)”的 XML 注释
        public static T As<T>(this object o)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ObjectHelper.As<T>(object)”的 XML 注释
        {
            T result = default(T);

            if (o != null)
            {
                try
                {
                    if (o is IConvertible)
                    {
                        result = (T)Convert.ChangeType(o, typeof(T));
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException(string.Format("无法将类型\"{0}\"转换为\"{1}\",值:{2}", o.GetType().FullName, typeof(T).FullName, (o != null) ? o.ToString() : "null"), ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 不同对象之间的深拷贝，最好属性名一样
        /// </summary>
        /// <typeparam name="T">源对象类型</typeparam>
        /// <typeparam name="F">目的对象类型</typeparam>
        /// <param name="original">源对象</param>
        /// <returns>目的对象</returns>
        public static F DeepCopy<T, F>(T original)
        {
            var json = original.ToJson();
            var result = json.FromJson<F>();
            return result;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ObjectHelper.DeepCopy<T, F>(T, F)”的 XML 注释
        public static void DeepCopy<T, F>(T original, F desination)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ObjectHelper.DeepCopy<T, F>(T, F)”的 XML 注释
        {
            desination = DeepCopy<T, F>(original);
        }

    }

}
