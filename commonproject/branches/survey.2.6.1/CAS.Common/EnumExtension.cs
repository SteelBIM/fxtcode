using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace CAS.Common
{
    public static class EnumExtension
    {
        /// <summary>
        /// 遍历获取枚举的所有值封装到键值对字典 "key_1"="df"
        /// 是要此方法时枚举上需定义Description属性
        /// 创建人：rock,20150209
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Dictionary<string,string> GetEnumDesToDictionaryJson<T>(string dataType = "int")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                string enumDes = GetEnumDescription((Enum)(object)((T)item), false);
                if (enumDes == null)
                {
                    continue;
                }
                string enumValue = Convert.ToString((object)item);
                if (dataType == "int")
                {
                    enumValue = Convert.ToInt32((object)item).ToString();
                }
                result["key_" + enumValue] = enumDes;
            }
            return result;
        }
        /// <summary>
        /// 遍历获取枚举的所有值封装到json [{name:"df",value:"232"}]
        /// 是要此方法时枚举上需定义Description属性
        /// 创建人：rock,20150209
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="dataType">value的数据类型，string、int</typeparam>
        /// <returns></returns>
        public static string GetEnumDesJoinNameListToJson<T>(string dataType = "string")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                string enumName = GetEnumDescription((Enum)(object)((T)item), false);
                if (enumName == null)
                {
                    continue;
                }
                string enumValue = Convert.ToString((object)item);
                if (dataType == "int")
                {
                    enumValue = Convert.ToInt32((object)item).ToString();
                }
                sb.Append("{\"name\":\"").Append(enumName).Append("\"");
                sb.Append(",\"value\":\"").Append(enumValue).Append("\"},");
            }
            string result = sb.ToString().TrimEnd(',') + "]";
            return result;
        }
        public static string Description<TEnum>(this TEnum EnumValue) where TEnum : struct
        {
            return GetEnumDescription((Enum)(object)((TEnum)EnumValue));            
        }
        /// <summary>
        /// 获取枚举中的描述
        /// 创建人：曾智磊，20150107
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="EnumValue"></param>
        /// <param name="nowValue"></param>
        /// <returns></returns>
        public static string Description<TEnum>(this TEnum EnumValue, int nowValue) where TEnum : struct
        {
            string result = "";
            //Type week = typeof(TEnum);
            //Array Arrays = Enum.GetValues(week);
            //for (int i = 0; i < Arrays.LongLength; i++)
            //{
            //    Enum val = (Enum)Arrays.GetValue(i);
            //}
            foreach (TEnum en in (TEnum[])System.Enum.GetValues(typeof(TEnum)))
            {
                if ((int)(object)en == nowValue)
                {
                    result = GetEnumDescription((Enum)(object)((TEnum)en));
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 根据当前枚举获取Description属性值
        /// 修改人：rock,20150209,添加参数isReturnName
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isReturnName">当当前枚举没有定义Description属性时是否返回当前枚举名称</param>
        /// <returns></returns>
        private static string GetEnumDescription(Enum value, bool isReturnName = true)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                if (isReturnName)
                {
                    return value.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
