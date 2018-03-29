using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Common
{
    public static class CommonHelp
    {
        public static void CopyValueTo(this object t, object to)
        {
            if (to == null)
            {
                to = new object();
            }
            if (t == null)
            {
                to = null;
                return;
            }
            PropertyInfo[] propertys1 = t.GetType().GetProperties();
            PropertyInfo[] propertys2 = to.GetType().GetProperties();
            for (int i = 0; i < propertys1.Length; i++)
            {
                PropertyInfo property1 = propertys1[i];
                string strName = property1.PropertyType.Name;
                if (strName == "Nullable`1")
                {
                    strName = property1.PropertyType.GetGenericArguments()[0].Name;
                }
                strName = strName.Trim();
                if (strName != "Decimal"
                    && strName != "Int32"
                    && strName != "Int64"
                     && strName != "DateTime"
                    && strName != "String")
                {
                    continue;
                }
                PropertyInfo property2 = propertys2[i];
                object propertyValue = property1.GetValue(t, null);
                if (propertyValue != null)
                {
                    property2.SetValue(to, propertyValue, null);
                }
                else
                {
                    property2.SetValue(to, null, null);
                }
            }
        }
        public static void CopyValueTo(this List<object> t, List<object> to)
        {
            if (t == null || to == null)
            {
                return;
            }
            if (t.Count != to.Count)
            {
                return;
            }
            for (int i = 0; i < t.Count; i++)
            {
                t[i].CopyValueTo(to[i]);
            }
        }
        public static string GetPropertyName(Expression<Func<案例信息>> expr)
        {
            var name = ((MemberExpression)expr.Body).Member.Name;
            return name;
        }
        /// <summary>
        /// 计算月份,默认上个月最后一天
        /// </summary>
        /// <param name="months">今天之前第几个月,默认上个月</param>
        /// <param name="format">转换格式,默认yyyy-MM-dd</param>
        /// <returns>返回格式化后的信息</returns>
        public static string GetDateTimeMoths(string datetime = null, int months = 0, string format = "yyyy-MM-dd")
        {
            DateTime lastMonth = DateTime.Now.AddMonths(-1);//得到上一个月10.5
            if (!string.IsNullOrEmpty(datetime))
                lastMonth = Convert.ToDateTime(datetime).AddMonths(-1);
            DateTime currentDateTime = lastMonth.AddDays(1 - lastMonth.Day).AddMonths(1)
                .AddDays(-1).AddMonths(months);
            return currentDateTime.ToString(format);
        }

    }
}
