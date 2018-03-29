using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using FxtCenterService.DataAccess.SQL;
using CAS.Common;

namespace FxtCenterService.DataAccess
{
    public class Base : BaseDA
    {
        /// <summary>
        /// 继承CAS.SQL.SQLName语句类，直接返回SQL语句
        /// </summary>
        public class SQL : SQLName { }

        //public const string RETURN_VALUE_PARAMETER_NAME = "@ReturnValue";

        /// <summary>
        /// 返回分页、排序处理后的SQL
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string HandleSQL(SearchBase search, string sql)
        {
            sql += search.Where;
            if (search.Page)
            {
                return search.PageSelect(sql) + search.PageWhere + " order by " + search.OrderBy;
            }
            else
            {
                if (!string.IsNullOrEmpty(search.OrderBy))
                    return sql + " order by " + search.OrderBy;
                else
                    return sql;
            }
        }

        /// <summary>
        /// 返回Union数据集分页、排序处理后的SQL
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string HandleSQL(SearchBase search)
        {
            if (search.Page)
            {
                return search.PageSql;
            }
            else
            {
                if (!string.IsNullOrEmpty(search.OrderBy))
                    return string.Format("SELECT * FROM({0}) ORDER BY {1}", search.Sql, search.OrderBy);
                else
                    return search.Sql;
            }

        }

        /// <summary>
        /// 是否分页
        /// zhoub 20160825
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string HandleMySQL(SearchBase search, string sql)
        {
            if (search.Page)
            {
                return sql + " limit " + (search.PageIndex - 1) * search.PageRecords+","+search.PageRecords;
            }
            else
            {
                return sql + " limit 0,"+search.PageRecords;
            }
        }
    }
}
