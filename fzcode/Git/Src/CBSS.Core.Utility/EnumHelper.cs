using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
      
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDesc<T>(object enumValue)
        {
            try
            {
                var t = typeof(T);
                DescriptionAttribute[] attrs = (DescriptionAttribute[])t.GetField(GetEnumName<T>(enumValue)).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return ((attrs.Length > 0) ? attrs[0].Description : GetEnumName<T>(enumValue));
            }
            catch
            {
                return "NA";
            }
        }
       
        /// <summary>
        /// 根据枚举值获取特性
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(EnumTitleAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            EnumTitleAttribute descriptionAttribute = (EnumTitleAttribute)objs[0];
            return descriptionAttribute.Title;
        }
        /// <summary>
        /// 获取枚举类型名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static string GetEnumName<T>(object enumValue)
        {
            try
            {
                return Enum.GetName(typeof(T), enumValue);
            }
            catch
            {
                return "NA";
            }
        }
        /// <summary>
        /// 转换如："enum1,enum2,enum3"字符串到枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">枚举字符串</param>
        /// <returns></returns>
        public static T Parse<T>(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default(T);
            else
                return (T)Enum.Parse(typeof(T), obj);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumHelper.TryParse<T>(string, T)”的 XML 注释
        public static T TryParse<T>(string obj, T defT = default(T))
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumHelper.TryParse<T>(string, T)”的 XML 注释
        {
            try
            {
                return Parse<T>(obj);
            }
            catch
            {
                return defT;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumHelper.ENUM_TITLE_SEPARATOR”的 XML 注释
        public static readonly string ENUM_TITLE_SEPARATOR = ",";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumHelper.ENUM_TITLE_SEPARATOR”的 XML 注释
#pragma warning disable CS1573 // 参数“language”在“EnumHelper.GetEnumTitle(Enum, Enum)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 根据枚举值，返回描述字符串
        /// 如果多选枚举，返回以","分割的字符串
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumTitle(Enum e, Enum language = null)
#pragma warning restore CS1573 // 参数“language”在“EnumHelper.GetEnumTitle(Enum, Enum)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            if (e == null)
            {
                return "";
            }
            string[] valueArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type type = e.GetType();
            string ret = "";
            foreach (string enumValue in valueArray)
            {
                System.Reflection.FieldInfo fi = type.GetField(enumValue.Trim());
                if (fi == null)
                    continue;
                EnumTitleAttribute[] attrs = fi.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                if (attrs != null && attrs.Length > 0 && attrs[0].IsDisplay)
                {
                    ret += attrs[0].Title + ENUM_TITLE_SEPARATOR;
                }
            }
            return ret.TrimEnd(ENUM_TITLE_SEPARATOR.ToArray());
        }

#pragma warning disable CS1573 // 参数“language”在“EnumHelper.GetAllEnumTitle(Enum, Enum)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 根据枚举值，返回描述字符串
        /// 如果多选枚举，返回以","分割的字符串
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetAllEnumTitle(Enum e, Enum language = null)
#pragma warning restore CS1573 // 参数“language”在“EnumHelper.GetAllEnumTitle(Enum, Enum)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            if (e == null)
            {
                return "";
            }
            string[] valueArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type type = e.GetType();
            string ret = "";
            foreach (string enumValue in valueArray)
            {
                System.Reflection.FieldInfo fi = type.GetField(enumValue.Trim());
                if (fi == null)
                    continue;
                EnumTitleAttribute[] attrs = fi.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    ret += attrs[0].Title + ENUM_TITLE_SEPARATOR;
                }
            }
            return ret.TrimEnd(ENUM_TITLE_SEPARATOR.ToArray());
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetEnumTitleAttribute(Enum, Enum)”的 XML 注释
        public static EnumTitleAttribute GetEnumTitleAttribute(Enum e, Enum language = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetEnumTitleAttribute(Enum, Enum)”的 XML 注释
        {
            if (e == null)
            {
                return null;
            }
            string[] valueArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type type = e.GetType();
            EnumTitleAttribute ret = null;
            foreach (string enumValue in valueArray)
            {
                System.Reflection.FieldInfo fi = type.GetField(enumValue.Trim());
                if (fi == null)
                    continue;
                EnumTitleAttribute[] attrs = fi.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    ret = attrs[0];
                    break;
                }
            }
            return ret;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetDayOfWeekTitle(DayOfWeek, Enum)”的 XML 注释
        public static string GetDayOfWeekTitle(DayOfWeek day, Enum language = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetDayOfWeekTitle(DayOfWeek, Enum)”的 XML 注释
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return "周一";
                case DayOfWeek.Tuesday:
                    return "周二";
                case DayOfWeek.Wednesday:
                    return "周三";
                case DayOfWeek.Thursday:
                    return "周四";
                case DayOfWeek.Friday:
                    return "周五";
                case DayOfWeek.Saturday:
                    return "周六";
                case DayOfWeek.Sunday:
                    return "周日";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 返回键值对，建为枚举的EnumTitle中指定的名称和近义词名称，值为枚举项
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Dictionary<string, T> GetTitleAndSynonyms<T>(Enum language = null) where T : struct
        {
            Dictionary<string, T> ret = new Dictionary<string, T>();
            //枚举值
            Array arrEnumValue = typeof(T).GetEnumValues();
            foreach (object enumValue in arrEnumValue)
            {
                System.Reflection.FieldInfo fi = typeof(T).GetField(enumValue.ToString());
                if (fi == null)
                {
                    continue;
                }

                EnumTitleAttribute[] arrEnumTitleAttr = fi.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                if (arrEnumTitleAttr == null || arrEnumTitleAttr.Length < 1 || !arrEnumTitleAttr[0].IsDisplay)
                {
                    continue;
                }

                if (!ret.ContainsKey(arrEnumTitleAttr[0].Title))
                {
                    ret.Add(arrEnumTitleAttr[0].Title, (T)enumValue);
                }

                if (arrEnumTitleAttr[0].Synonyms == null || arrEnumTitleAttr[0].Synonyms.Length < 1)
                {
                    continue;
                }

                foreach (string s in arrEnumTitleAttr[0].Synonyms)
                {
                    if (!ret.ContainsKey(s))
                    {
                        ret.Add(s, (T)enumValue);
                    }
                }
            }//using
            return ret;
        }

        /// <summary>
        /// 根据枚举获取包含所有所有值和描述的哈希表，其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <returns></returns>
        public static Dictionary<T, string> GetItemList<T>(Enum language = null) where T : struct
        {
            return GetItemValueList<T, T>(false, language);
        }

        /// <summary>
        /// 根据枚举获取包含所有所有值和描述的哈希表，其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <returns></returns>
        public static Dictionary<T, string> GetAllItemList<T>(Enum language = null) where T : struct
        {
            return GetItemValueList<T, T>(true, language);
        }

        /// <summary>
        /// 获取枚举所有项的标题,其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetItemValueList<T>(Enum language = null) where T : struct
        {
            return GetItemValueList<T, int>(false, language);
        }

#pragma warning disable CS1712 // 类型参数“TKey”在“EnumHelper.GetItemValueList<T, TKey>(bool, Enum)”的 XML 注释中没有匹配的 typeparam 标记(但其他类型参数有)
        /// <summary>
        /// 获取枚举所有项的标题,其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="isAll">是否生成“全部”项</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static Dictionary<TKey, string> GetItemValueList<T, TKey>(bool isAll, Enum language = null) where T : struct
#pragma warning restore CS1712 // 类型参数“TKey”在“EnumHelper.GetItemValueList<T, TKey>(bool, Enum)”的 XML 注释中没有匹配的 typeparam 标记(但其他类型参数有)
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("参数必须是枚举！");
            }
            Dictionary<TKey, string> ret = new Dictionary<TKey, string>();

            var titles = EnumHelper.GetItemAttributeList<T>().OrderBy(t => t.Value.Order);
            foreach (var t in titles)
            {
                if (!isAll && (!t.Value.IsDisplay || t.Key.ToString() == "None"))
                    continue;

                if (t.Key.ToString() == "None" && isAll)
                {
                    ret.Add((TKey)(object)t.Key, "全部");
                }
                else
                {
                    if (!string.IsNullOrEmpty(t.Value.Title))
                        ret.Add((TKey)(object)t.Key, t.Value.Title);
                }
            }

            return ret;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetItemKeyList<T>(Enum)”的 XML 注释
        public static List<T> GetItemKeyList<T>(Enum language = null) where T : struct
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetItemKeyList<T>(Enum)”的 XML 注释
        {
            List<T> list = new List<T>();
            Array array = typeof(T).GetEnumValues();
            foreach (object t in array)
            {
                list.Add((T)t);
            }
            return list;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetItemAttributeList<T>(Enum)”的 XML 注释
        public static Dictionary<T, EnumTitleAttribute> GetItemAttributeList<T>(Enum language = null) where T : struct
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumHelper.GetItemAttributeList<T>(Enum)”的 XML 注释
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("参数必须是枚举！");
            }
            Dictionary<T, EnumTitleAttribute> ret = new Dictionary<T, EnumTitleAttribute>();

            Array array = typeof(T).GetEnumValues();
            foreach (object t in array)
            {
                EnumTitleAttribute att = GetEnumTitleAttribute(t as Enum, language);
                if (att != null)
                    ret.Add((T)t, att);
            }

            return ret;
        }

#pragma warning disable CS1572 // XML 注释中有“isAll”的 param 标记，但是没有该名称的参数
#pragma warning disable CS1712 // 类型参数“TKey”在“EnumHelper.GetAllItemValueList<T, TKey>(Enum)”的 XML 注释中没有匹配的 typeparam 标记(但其他类型参数有)
        /// <summary>
        /// 获取枚举所有项的标题,其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="isAll">是否生成“全部”项</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        public static Dictionary<TKey, string> GetAllItemValueList<T, TKey>(Enum language = null) where T : struct
#pragma warning restore CS1712 // 类型参数“TKey”在“EnumHelper.GetAllItemValueList<T, TKey>(Enum)”的 XML 注释中没有匹配的 typeparam 标记(但其他类型参数有)
#pragma warning restore CS1572 // XML 注释中有“isAll”的 param 标记，但是没有该名称的参数
        {
            return GetItemValueList<T, TKey>(true, language);
        }


        /// <summary>
        /// 获取一个枚举的键值对形式
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="exceptTypes">排除的枚举</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumDictionary<TEnum>(IEnumerable<TEnum> exceptTypes = null) where TEnum : struct
        {
            var dic = GetItemList<TEnum>();

            Dictionary<int, string> dicNew = new Dictionary<int, string>();
            foreach (var d in dic)
            {
                if (exceptTypes != null && exceptTypes.Contains(d.Key))
                {
                    continue;
                }
                dicNew.Add(d.Key.GetHashCode(), d.Value);
            }
            return dicNew;
        }

        /// <summary>
        /// 根据枚举成员获取DescriptionAttribute的属性Description
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetDescription(object o)
        {
            //获取枚举的Type类型对象
            Type t = o.GetType();

            //获取枚举的所有字段
            FieldInfo[] ms = t.GetFields();

            //遍历所有枚举的所有字段
            foreach (FieldInfo f in ms)
            {
                if (f.Name != o.ToString())
                {
                    continue;
                }
                FieldInfo fi = o.GetType().GetField(o.ToString());
                try
                {
                    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return (attributes != null && attributes.Length > 0) ? attributes[0].Description : o.ToString();
                }
                catch
                {
                    return "(Unknow)";
                }
            }

            //如果没有找到自定义属性，直接返回属性项的名称
            return o.ToString();
        }
    }



#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute”的 XML 注释
    public class EnumTitleAttribute : Attribute
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute”的 XML 注释
    {
        private bool _IsDisplay = true;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.EnumTitleAttribute(string, params string[])”的 XML 注释
        public EnumTitleAttribute(string title, params string[] synonyms)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.EnumTitleAttribute(string, params string[])”的 XML 注释
        {
            Title = title;
            Synonyms = synonyms;
            Order = int.MaxValue;
        }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.IsDisplay”的 XML 注释
        public bool IsDisplay { get { return _IsDisplay; } set { _IsDisplay = value; } }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.IsDisplay”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Title”的 XML 注释
        public string Title { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Title”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Description”的 XML 注释
        public string Description { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Description”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Letter”的 XML 注释
        public string Letter { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Letter”的 XML 注释
        /// <summary>
        /// 近义词
        /// </summary>
        public string[] Synonyms { get; set; }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Category”的 XML 注释
        public int Category { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Category”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Order”的 XML 注释
        public int Order { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“EnumTitleAttribute.Order”的 XML 注释
    }

}
