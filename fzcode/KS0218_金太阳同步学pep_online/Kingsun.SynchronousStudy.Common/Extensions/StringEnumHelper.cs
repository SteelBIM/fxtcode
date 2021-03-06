﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public static class StringEnumHelper
    {
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        //public static string GetStringValue<TEnum>(this TEnum value) where TEnum : struct
        //{
        //    // Get the type
        //    Type type = value.GetType();

        //    // Get fieldinfo for this type
        //    FieldInfo fieldInfo = type.GetField(value.ToString());

        //    // Get the stringvalue attributes
        //    StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

        //    // Return the first if there was a match.
        //    return attribs.Length > 0 ? attribs[0].StringValue : null;
        //}

        public static string GetStringValue<TEnum>(this TEnum value) where TEnum : struct
        {
            return value.GetStringValue(0);
        }

        public static string GetStringValue<TEnum>(object value) where TEnum : struct
        {
            var result = default(string);
            TEnum enu = default(TEnum);
            if (Enum.TryParse(value.ToString(), out enu))
            {
                result = GetStringValue(enu);
            }

            return result;
        }

        public static string TryStringValue<TEnum>(object value) where TEnum : struct
        {
            var result = default(string);
            TEnum enu = default(TEnum);

            if (Enum.TryParse(value.ToString(), out enu))
            {
                result = GetStringValue(enu);
            }
            else
            {
                result = value.ToString();
            }

            return result;
        }

        public static string GetStringValue<TEnum>(this TEnum value, int index) where TEnum : struct
        {
            // Get the type
            var type = value.GetType();

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length >= 0 ? attribs.Single(a => a.SortIndex == index).StringValue : null;
        }

        public static IDictionary<string, int> ToDictionaryKeyValue<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var dict = new Dictionary<string, int>();
            var values = typeof(T).GetEnumValues();

            foreach (var value in values)
            {
                var stringValue = Enum.GetName(typeof(T), value);

                dict.Add(stringValue, (int)value);
            }

            return dict;
        }

        public static IDictionary<string, IDictionary<string, object>> ToDictionaryKeyDict<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var dict = new Dictionary<string, IDictionary<string, object>>();
            var names = typeof(T).GetEnumNames();

            foreach (var name in names)
            {
                var enumVal = (T)Enum.Parse(typeof(T), name);
                var keyDict = new Dictionary<string, object>();
                keyDict.Add("value", Convert.ToInt32(enumVal));
                keyDict.Add("desc", enumVal.GetStringValue());

                dict.Add(name, keyDict);
            }

            return dict;
        }

        public static IDictionary<string, string> ToDictionaryKeyString<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var dict = new Dictionary<string, string>();
            var names = typeof(T).GetEnumNames();

            foreach (var name in names)
            {
                var enumVal = (T)Enum.Parse(typeof(T), name);

                dict.Add(name, enumVal.GetStringValue());
            }

            return dict;
        }

        public static IDictionary<string, int> ToDictionaryNameValue<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var dict = new Dictionary<string, int>();
            var values = typeof(T).GetEnumValues();

            foreach (var value in values)
            {
                var enumType = (T)Enum.Parse(typeof(T), value.ToString());
                var stringValue = enumType.GetStringValue();

                dict.Add(stringValue, (int)value);
            }

            return dict;
        }

        public static IDictionary<int, string> ToDictionaryValueName<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var dict = new Dictionary<int, string>();

            var values = typeof(T).GetEnumValues();

            foreach (var value in values)
            {
                var enumType = (T)Enum.Parse(typeof(T), value.ToString());
                var stringValue = enumType.GetStringValue();

                dict.Add((int)value, stringValue);
            }

            return dict;
        }

        public static int? GetIntValue<TEnum>(string code) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            var values = typeof(TEnum).GetEnumValues();

            foreach (var value in values)
            {
                var stringValue = Enum.GetName(typeof(TEnum), value);
                if (stringValue.Equals(code, StringComparison.OrdinalIgnoreCase))
                {
                    return (int)value;
                }
            }
            return null;
        }
    }
}
