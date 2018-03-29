using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FxtDataAcquisition.Common
{
    public static class CommonUtility
    {
        /// <summary>
        /// 复制实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CopyEntity<T>(this T obj)
        {
            if (obj == null)
            {
                return obj;
            }
            T result = System.Activator.CreateInstance<T>();
            PropertyInfo[] ps = obj.GetType().GetProperties();
            foreach (PropertyInfo p in ps)
            {
                var property = result.GetType().GetProperties()
                         .Where(pInfo => pInfo.Name.ToLower().Equals(p.Name.ToLower())).FirstOrDefault();
                if (property != null)
                {
                    property.SetValue(result, p.GetValue(obj, null), null);
                }
            }
            return result;
        }
        public static object valueType(Type t, object value, bool isDecode = false)
        {
            string strName = t.Name;
            bool existsNull = false;
            if (t.Name == "Nullable`1")
            {
                existsNull = true;
                strName = t.GetGenericArguments()[0].Name;
            }
            if (string.IsNullOrEmpty(Convert.ToString(value)))
            {
                if (existsNull)
                {
                    return null;
                }
                else
                {
                    value = null;
                }
            }

            if (isDecode)
            {
                value = Convert.ToString(value).DecodeField();
            }

            switch (strName.Trim())
            {
                case "Decimal":
                    decimal d = decimal.Zero;
                    decimal.TryParse(value.ToString(), out d);
                    value = d;
                    break;
                case "Int32":
                    int i = 0;
                    int.TryParse(value.ToString(), out i);
                    value = i;
                    break;
                case "Int64":
                    long i2 = 0;
                    long.TryParse(value.ToString(), out i2);
                    value = i2;
                    break;
                case "Float":
                    value = float.Parse(Convert.ToString(value));
                    break;
                case "DateTime":
                    value = Convert.ToDateTime(value);
                    break;
                case "Double":
                    value = Convert.ToDouble(value);
                    break;
                case "Bool":
                    value = Convert.ToBoolean(value);
                    break;
                case "String":
                    value = Convert.ToString(value);
                    break;
                case "Array":
                    value = (Array)value;
                    break;
                default:
                    value = value as object;
                    break;
            }
            return value;
        }

        //取配置文件appsetting值
        public static string GetConfigSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 产生随机字符串，用于客户端随机命名
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRndString(int len)
        {
            string s = Guid.NewGuid().ToString().Replace("-", "");
            return s.Substring(0, len > s.Length ? s.Length : len);
        }
    }
}
