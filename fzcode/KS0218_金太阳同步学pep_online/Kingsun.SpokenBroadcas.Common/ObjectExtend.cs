using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Web.Script.Serialization;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace Kingsun.SpokenBroadcas.Common
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 字符串转Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToIntOrZero(this object obj)
        {
            int temp = 0;
            try
            {
                if (!int.TryParse(obj.ToString(), out temp))
                {
                    temp = 0;
                }
            }
            catch
            {

            }
            return temp;
        }
        /// <summary>
        /// 字符串转Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToIntOrMaxValue(this object obj)
        {
            int temp = int.MaxValue;
            try
            {
                if (!int.TryParse(obj.ToString(), out temp))
                {
                    temp = int.MaxValue;
                }
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToIntOrMinValue(this object obj)
        {
            int temp = int.MinValue;
            try
            {
                if (!int.TryParse(obj.ToString(), out temp))
                {
                    temp = int.MinValue;
                }
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Float
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float ToFloatOrZero(this object obj)
        {
            float temp = 0;
            try
            {
                float.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }
        /// <summary>
        /// 字符串转float
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float ToFloatOrMaxValue(this object obj)
        {
            float temp = float.MaxValue;
            try
            {
                float.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转float
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float ToFloatOrMinValue(this object obj)
        {
            float temp = float.MinValue;
            try
            {
                float.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDoubleOrZero(this object obj)
        {
            double temp = 0;
            try
            {
                double.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDoubleOrMaxValue(this object obj)
        {
            double temp = double.MaxValue;
            try
            {
                double.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDoubleOrMinValue(this object obj)
        {
            double temp = double.MinValue;
            try
            {
                double.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int64
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int64 ToInt64OrZero(this object obj)
        {
            Int64 temp = 0;
            try
            {
                Int64.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int64
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int64 ToInt64OrMaxValue(this object obj)
        {
            Int64 temp = Int64.MaxValue;
            try
            {
                Int64.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int64
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int64 ToInt64OrMinValue(this object obj)
        {
            Int64 temp = Int64.MinValue;
            try
            {
                Int64.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int16
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int16 ToInt16OrZero(this object obj)
        {
            Int16 temp = 0;
            try
            {
                Int16.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int16
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int16 ToInt16OrMaxValue(this object obj)
        {
            Int16 temp = Int16.MaxValue;
            try
            {
                Int16.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int16
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int16 ToInt16OrMinValue(this object obj)
        {
            Int16 temp = Int16.MinValue;
            try
            {
                Int16.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimalOrZero(this object obj)
        {
            decimal temp = 0;
            try
            {
                decimal.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimalOrMaxValue(this object obj)
        {
            decimal temp = decimal.MaxValue;
            try
            {
                decimal.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimalOrMinValue(this object obj)
        {
            decimal temp = decimal.MinValue;
            try
            {
                decimal.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeOrMaxValue(this object obj)
        {
            DateTime temp = DateTime.MaxValue;
            try
            {
                DateTime.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeOrMinValue(this object obj)
        {
            DateTime temp = DateTime.MinValue;
            try
            {
                DateTime.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Guid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Guid ToGuidOrEmpty(this object obj)
        {
            Guid temp = Guid.Empty;
            try
            {
                Guid.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBoolOrFalse(this object obj)
        {
            bool temp = false;
            try
            {
                bool.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }
        /// <summary>
        /// 字符串转Bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBoolOrTrue(this object obj)
        {
            bool temp = true;
            try
            {
                bool.TryParse(obj.ToString(), out temp);
            }
            catch
            {

            }
            return temp;
        }
        public static string ToJoinStringAsDigit(this IList<int> obj)
        {
            string returnValue = null;
            if (obj != null && obj.Count > 0)
            {
                foreach (int item in obj)
                {
                    if (returnValue != null)
                        returnValue += ",";
                    returnValue += item;
                }
            }
            return returnValue;
        }
        public static string ToJoinStringAsDigit(this IList<Guid> obj)
        {
            string returnValue = null;
            if (obj != null && obj.Count > 0)
            {
                foreach (Guid item in obj)
                {
                    if (returnValue != null)
                        returnValue += ",";
                    returnValue += string.Format("'{0}'", item.ToString());
                }
            }
            return returnValue;
        }
        public static string ToJoinString(this IList<int> obj)
        {
            string returnValue = null;
            if (obj != null && obj.Count > 0)
            {
                foreach (int item in obj)
                {
                    if (returnValue != null)
                        returnValue += ",";
                    returnValue += "'" + item + "'";
                }
            }
            return returnValue;
        }
        public static string ToJoinString(this IList<Guid> obj)
        {
            string returnValue = null;
            if (obj != null && obj.Count > 0)
            {
                foreach (Guid item in obj)
                {
                    if (returnValue != null)
                        returnValue += ",";
                    returnValue += "'" + item + "'";
                }
            }
            return returnValue;
        }
        public static IList<int> ToIntList(this string obj)
        {
            List<int> list = new List<int>();
            if (String.IsNullOrEmpty(obj))
                return null;
            foreach (string item in obj.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                int temp = item.ToIntOrMaxValue();
                if (temp != int.MaxValue)
                    list.Add(temp);
            }
            return list;
        }
        public static IList<Guid> ToGuidList(this string obj)
        {
            List<Guid> list = new List<Guid>();
            if (String.IsNullOrEmpty(obj))
                return null;
            foreach (string item in obj.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                Guid temp = Guid.Empty;

                if (Guid.TryParse(item, out temp))
                    list.Add(temp);
            }
            return list;
        }
        public static IList<int> ToIntList(this string obj, string splitString)
        {
            List<int> list = new List<int>();
            if (String.IsNullOrEmpty(obj))
                return null;
            foreach (string item in obj.Split(new string[] { splitString }, StringSplitOptions.RemoveEmptyEntries))
            {
                int temp = item.ToIntOrMaxValue();
                if (temp != int.MaxValue)
                    list.Add(temp);
            }
            return list;
        }

        public static IList<Guid> ToGuidList(this string obj, string splitString)
        {
            List<Guid> list = new List<Guid>();
            if (String.IsNullOrEmpty(obj))
                return null;
            foreach (string item in obj.Split(new string[] { splitString }, StringSplitOptions.RemoveEmptyEntries))
            {
                Guid temp = Guid.Empty;
                Guid.TryParse(item, out temp);
                if (temp != Guid.Empty)
                    list.Add(temp);
            }
            return list;
        }

        public static List<T> ToList<T>(this DataTable obj) where T : new()
        {
            List<T> list = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            try
            {
                foreach (DataRow row in obj.Rows)
                {
                    T local = Activator.CreateInstance<T>();
                    foreach (PropertyInfo info in properties)
                    {
                        if (obj.Columns.Contains(info.Name))
                        {
                            if (row[obj.Columns[info.Name]] != DBNull.Value)
                            {
                                info.SetValue(local, row[obj.Columns[info.Name]], null);
                            }
                            else
                            {
                                info.SetValue(local, null, null);
                            }
                        }
                    }
                    list.Add(local);
                }
            }
            catch (Exception)
            {

            }
            return list;
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        result.Columns.Add(pi.Name);
                    }
                    else
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }




        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        /// <returns></returns>
        public static bool EqualsEx(this string text1, string text2)
        {
            return string.Equals(text1, text2, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 判断是否包含字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsEx(this string text, string value)
        {
            return text.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// 判断是否以指定字符串开头
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StartWithEx(this string text, string value)
        {
            return text.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 判断是否以指定字符串结尾
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EndWithEx(this string text, string value)
        {
            return text.EndsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 判断字符串是否空
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Datatable转Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary);
            }
            return javaScriptSerializer.Serialize(arrayList);
        }

        /// <summary>
        /// Datatable转List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList ToList(this DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary);
            }
            return arrayList;
        }


        /// <summary>
        /// Datatable转Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Stream ToExcel(this DataTable dt)
        {
            XSSFWorkbook workbook = null;
            MemoryStream ms = new MemoryStream();
            ISheet sheet = null;
            XSSFRow headerRow = null;
            try
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet();
                headerRow = (XSSFRow)sheet.CreateRow(0);
                foreach (DataColumn column in dt.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dt.Columns)
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    ++rowIndex;
                }
                for (int i = 0; i <= dt.Columns.Count; ++i)
                    sheet.AutoSizeColumn(i);
                workbook.Write(ms);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ms.Close();
                sheet = null;
                headerRow = null;
                workbook = null;
            }
            return ms;
        }

        /// <summary>
        /// Datatable转Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>

        //private void DownloadExcel(DataTable dt, string reportName)
        //{
        //    Stream s = RenderDataTableToExcel(dt);
        //    if (s != null)
        //    {
        //        MemoryStream ms = resultStream.result as MemoryStream;
        //        Response.AddHeader("Content-Disposition", string.Format("attachment;filename=" + HttpUtility.UrlEncode(reportName) + DateTime.Now.ToString("yyyyMMdd") + ".xlsx"));
        //        Response.AddHeader("Content-Length", ms.ToArray().Length.ToString());
        //        Response.BinaryWrite(ms.ToArray());
        //        Response.Flush();
        //        ms.Close();
        //        ms.Dispose();
        //    }
        //    else
        //        Response.Write("出错，无法下载！");
        //}
    }
}
