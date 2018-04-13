using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Kingsun.SynchronousStudy.Common
{
    public static partial class Extension
    {


        public static string MD5Password(this string password)
        {
            if (string.IsNullOrEmpty(password)) 
            {
                return "";
            }
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").Substring(8, 16);

        }
        public static string OToJson<T>(this T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public static T FromJson<T>(this string objectToDeserialize)
        {
            return JsonConvert.DeserializeObject<T>(objectToDeserialize);
        }

        public static double CutDoubleWithN(this double d, int n)
        {
            string strDecimal = d.ToString();
            int index = strDecimal.IndexOf(".");
            if (index == -1 || strDecimal.Length < index + n + 1)
            {
                strDecimal = string.Format("{0:F" + n + "}", d);
            }
            else
            {
                int length = index;
                if (n != 0)
                {
                    length = index + n + 1;
                }
                strDecimal = strDecimal.Substring(0, length);
            }
            return Double.Parse(strDecimal);
        }  


        public static bool isNumberic1(this string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))//if(c<'0' || c>'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右
                    return false;
            }
            return true;
        }


        public static List<T> Convert2Object<T>(DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(ToEntity<T>(dr));
            }
            return list;
        }

        public static T ToEntity<T>(DataRow dr) where T : new()
        {
            T model = new T();
            foreach (PropertyInfo pInfo in model.GetType().GetProperties())
            {
                object val = GetValueByColumnName(dr, pInfo.Name);
                pInfo.SetValue(model, val, null);
            }
            return model;
        }

        private static object GetValueByColumnName(System.Data.DataRow dr, string columnName)
        {
            if (dr.Table.Columns.IndexOf(columnName) >= 0)
            {
                if (dr[columnName] == DBNull.Value)
                    return null;
                return dr[columnName];
            }
            return null;
        }
        public static bool IsGUID(this string str)
        {
            Match m = Regex.Match(str, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                //可以转换
                //Guid guid = new Guid(str);
                return true;
            }
            else
            {
                //不可转换
                return false;
            }
        }

        /// <summary>
        /// 小数转整数，类似四舍五入
        /// </summary>
        /// <param name="value">小数</param>
        /// <returns>整数</returns>
        public static int ToInt(this decimal value)
        {
            var decimalNum = value - (int)value;
            if (decimalNum >= 0.5m)
                return ((int)value) + 1;
            else
                return (int)value;
        }

        /// <summary>
        /// double转整数，类似四舍五入
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>整数</returns>
        public static int ToInt(this double value)
        {
            return ((decimal)value).ToInt();
        }


        /// <summary>
        /// double转整数，类似四舍五入
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>整数</returns>
        public static int ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            else 
            {
                return Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// 对象JSON序列化接口
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            return serializer.Serialize(obj);
        }
    

        /// <summary>
        /// 对象反序列化接口
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="json">序列化字符串</param>
        /// <returns></returns>
        public static T toObject<T>(this string json) where T : new()
        {
            T obj;
            if (!String.IsNullOrEmpty(json))
            {

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                obj = (T)serializer.Deserialize(json, typeof(T));

            }
            else
            {
                obj = default(T);
            }
            return obj;
        }


        public static string GetGradeName(this int? GradeID)
        {
            string GradeName = "";
            switch (GradeID)
            {
                case 1:
                    GradeName = "学前";
                    break;
                case 2:
                    GradeName = "一年级";
                    break;
                case 3:
                    GradeName = "二年级";
                    break;
                case 4:
                    GradeName = "三年级";
                    break;
                case 5:
                    GradeName = "四年级";
                    break;
                case 6:
                    GradeName = "五年级";
                    break;
                case 7:
                    GradeName = "六年级";
                    break;
                case 8:
                    GradeName = "七年级";
                    break;
                case 9:
                    GradeName = "八年级";
                    break;
                case 10:
                    GradeName = "九年级";
                    break;
                default:
                    GradeName = "其它";
                    break;
            }
            return GradeName;
        }
        public static string GetGradeName(this int GradeID)
        {
            string GradeName = "";
            switch (GradeID)
            {
                case 1:
                    GradeName = "学前";
                    break;
                case 2:
                    GradeName = "一年级";
                    break;
                case 3:
                    GradeName = "二年级";
                    break;
                case 4:
                    GradeName = "三年级";
                    break;
                case 5:
                    GradeName = "四年级";
                    break;
                case 6:
                    GradeName = "五年级";
                    break;
                case 7:
                    GradeName = "六年级";
                    break;
                case 8:
                    GradeName = "七年级";
                    break;
                case 9:
                    GradeName = "八年级";
                    break;
                case 10:
                    GradeName = "九年级";
                    break;
                default:
                    GradeName = "其它";
                    break;
            }
            return GradeName;
        }
    }
}
