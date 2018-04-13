
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;


namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper”的 XML 注释
    public static class JsonConvertHelper
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson<T>(T)”的 XML 注释
        public static string ToJson<T>(this T objectToSerialize)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson<T>(T)”的 XML 注释
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.FromJson<T>(string)”的 XML 注释
        public static T FromJson<T>(this string objectToDeserialize)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.FromJson<T>(string)”的 XML 注释
        {
            return JsonConvert.DeserializeObject<T>(objectToDeserialize);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.JSONStringToList<T>(string)”的 XML 注释
        public static List<T> JSONStringToList<T>(this string JsonStr)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.JSONStringToList<T>(string)”的 XML 注释
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            return Serializer.Deserialize<List<T>>(JsonStr);
        }
      

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson(string)”的 XML 注释
        public static object ToJson(this string Json)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson(string)”的 XML 注释
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson(object)”的 XML 注释
        public static string ToJson(this object obj)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson(object)”的 XML 注释
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.DataTableToJson(DataTable)”的 XML 注释
        public static string DataTableToJson(this DataTable table)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.DataTableToJson(DataTable)”的 XML 注释
        {
            var JsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
            }
            return JsonString.ToString();
        } 
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson(object, string)”的 XML 注释
        public static string ToJson(this object obj, string datetimeformats)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJson(object, string)”的 XML 注释
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToObject<T>(string)”的 XML 注释
        public static T ToObject<T>(this string Json)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToObject<T>(string)”的 XML 注释
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToList<T>(string)”的 XML 注释
        public static List<T> ToList<T>(this string Json)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToList<T>(string)”的 XML 注释
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToTable(string)”的 XML 注释
        public static DataTable ToTable(this string Json)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToTable(string)”的 XML 注释
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJObject(string)”的 XML 注释
        public static JObject ToJObject(this string Json)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“JsonConvertHelper.ToJObject(string)”的 XML 注释
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }
}
