using CAS.DataAccess.DA;
using CAS.Entity.BaseDAModels;
using FxtService.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FxtNHibernater.Data
{
    /// <summary>
    /// 针对SQL Server数据库操作的CURD实现
    /// 作者:李晓东
    /// 日期:2014.05.26
    /// Version:1.0
    /// </summary>
    public class MSSQLADODAL
    {
        string connectionkey = "default";
        static string defalutkey = "default";
        public MSSQLADODAL(string key = null)
        {
            connectionkey = key != null ? key.ToLower() : defalutkey;
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="commandText">SQL语句</param>
        /// <param name="countTable">分页获取总数的表名及条件</param>
        /// <returns></returns>
        public List<T> GetList<T>(string commandText, UtilityPager utilityPager = null, string countTable = null) where T : BaseTO, new()
        {
            MSSQLSqlHelper db = new MSSQLSqlHelper(connectionkey);
            List<T> list = new List<T>();
            //是否需要分页
            if (utilityPager != null)
            {
                string sqlcount = string.Format("select count(*) from {0}", countTable);
                utilityPager.Count = int.Parse(db.ExecuteScalar(sqlcount).ToString());
                commandText = PagingAll(commandText, utilityPager.PageIndex, utilityPager.PageSize);
            }
            using (SqlDataReader reader = db.ExecuteReader(commandText))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        T to = new T();
                        to.Initialize(reader);
                        list.Add(to);
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// 获取数据列表 带参数化
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="commandText">SQL语句</param>
        /// <param name="countTable">分页获取总数的表名及条件</param>
        /// <returns></returns>
        public List<T> GetList<T>(string commandText, UtilityPager utilityPager = null, string countTable = null,SqlParameter[] parmarr=null) where T : BaseTO, new()
        {
            MSSQLSqlHelper db = new MSSQLSqlHelper(connectionkey);
            List<T> list = new List<T>();
            //是否需要分页
            if (utilityPager != null)
            {
                string sqlcount = string.Format("select count(*) from {0}", countTable);
                utilityPager.Count = int.Parse(db.ExecuteScalar(sqlcount, CommandType.Text, parmarr).ToString());
                commandText = PagingAll(commandText, utilityPager.PageIndex, utilityPager.PageSize);
            }
            using (SqlDataReader reader = db.ExecuteReader(commandText, CommandType.Text, parmarr))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        T to = new T();
                        to.Initialize(reader);
                        list.Add(to);
                    }
                }
            }
            return list;
        }


        public List<T> GetList<T>(string commandText) where T : new()
        {
            MSSQLSqlHelper db = new MSSQLSqlHelper(connectionkey);
            List<T> list = new List<T>();
            using (SqlDataReader reader = db.ExecuteReader(commandText))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        T to = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string field = reader.GetName(i);
                            if (field.Equals("rownum"))
                                continue;
                            object value = reader.GetValue(i);
                            if (value != null && value != DBNull.Value)
                            {
                                var property = to.GetType().GetProperty(field);
                                bool IsNull = value == null || string.IsNullOrEmpty(value.ToString());
                                if (!property.PropertyType.IsGenericType)
                                {
                                    //非泛型
                                    property.SetValue(to,
                                         IsNull ? null : Convert.ChangeType(value, property.PropertyType),
                                        null);
                                }
                                else
                                {
                                    //泛型Nullable<>
                                    Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                                    if (genericTypeDefinition == typeof(Nullable<>))
                                    {
                                        property.SetValue(to, 
                                            IsNull ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType)),
                                            null);
                                    }
                                }
                            }
                        }
                        list.Add(to);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取数组数据列表
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public List<object> GetListObject(string commandText)
        {
            MSSQLSqlHelper db = new MSSQLSqlHelper(connectionkey);
            List<object> list = new List<object>();
            using (SqlDataReader reader = db.ExecuteReader(commandText))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.FieldCount > 1)
                        {
                            object[] obj = new object[reader.FieldCount];
                            reader.GetValues(obj);
                            list.Add(obj);
                        }
                        else
                        {
                            list.Add(reader.GetValue(0));
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public T GetModel<T>(string commandText) where T : new()
        {
            MSSQLSqlHelper db = new MSSQLSqlHelper(connectionkey);
            T to = default(T);

            using (SqlDataReader reader = db.ExecuteReader(commandText))
            {
                if (reader.HasRows)
                {
                    to = new T();
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string field = reader.GetName(i);
                            if (field.Equals("rownum"))
                                continue;
                            object value = reader.GetValue(i);
                            if (value != null && value != DBNull.Value)
                            {
                                var property = to.GetType().GetProperty(field);
                                bool IsNull = value == null || string.IsNullOrEmpty(value.ToString());
                                if (!property.PropertyType.IsGenericType)
                                {
                                    //非泛型
                                    property.SetValue(to,
                                         IsNull ? null : Convert.ChangeType(value, property.PropertyType),
                                        null);
                                }
                                else
                                {
                                    //泛型Nullable<>
                                    Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                                    if (genericTypeDefinition == typeof(Nullable<>))
                                    {
                                        property.SetValue(to,
                                            IsNull ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType)),
                                            null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return to;
        }

        /// <summary>
        /// 返回结果中第一行第一列结果
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public object GetUniqueResult(string commandText)
        {
            MSSQLSqlHelper db = new MSSQLSqlHelper(connectionkey);
            return db.ExecuteScalar(commandText);
        }
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public int CUD(string commandText, SqlParameter[] param = null)
        {
            MSSQLSqlHelper db = new MSSQLSqlHelper(connectionkey);
            return db.ExecuteNonQuery(commandText, System.Data.CommandType.Text, param);
        }

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="key">ConnectionString To Key(or Name)</param>
        public static void SetConnection(string key = null)
        {
            key = key == null ? defalutkey : key;
            CAS.DataAccess.BaseDAModels.SqlServerSet.CustomGetConnectionString = delegate(string connName)
            {
                //直接从config中获取
                return MSSQLSqlHelper.GetConfigurationManager(key);
            };
        }

        /// <summary>
        /// 组装新的分页语句
        /// </summary>
        /// <param name="Sql">原SQL语句</param>
        /// <param name="PageIndex">索引页</param>
        /// <param name="PageSize">获取一页个数</param>
        /// <returns></returns>
        public static string PagingAll(string Sql, int PageIndex, int PageSize)
        {
            string selectStr = string.Empty, orderStr = string.Empty;
            int rows = PageIndex * PageSize, starti = (PageIndex - 1) * PageSize, endi = rows;
            string val = string.Empty;
            selectStr = Sql.Trim().ToUpper();
            if (PageIndex > 1)
            {
                starti += 1;
                endi += 1;
            }
            if (selectStr.IndexOf("TOP") == -1)
            {
                selectStr = string.Format("SELECT TOP {0} {1}", rows, selectStr.Substring(6));
                //selectStr.IndexOf("SELECT"),selectStr.Length - selectStr.IndexOf("SELECT")
            }
            if (selectStr.IndexOf("ORDER BY") != -1)
            {
                orderStr = selectStr.Substring(selectStr.IndexOf("ORDER BY"), selectStr.Length - selectStr.IndexOf("ORDER BY"));
                if (selectStr.IndexOf("GROUP BY") != -1)
                    orderStr = string.Format("{0} {1}", orderStr,
                        selectStr.Substring(selectStr.IndexOf("GROUP BY")));

                selectStr = selectStr.Substring(0, selectStr.IndexOf("ORDER BY"));
            }
            if (selectStr.IndexOf("ORDER BY") == -1 && string.IsNullOrEmpty(orderStr))
            {
                val = string.Format(@"select * from (SELECT ROW_NUMBER() OVER(ORDER BY orderbyID DESC) AS AllowPagingId ,* FROM ( select *, 1 as orderbyID from ( {0} 
) as tbs1 ) as Tabl1 ) as table2 where AllowPagingId between {1} and {2}",
                    selectStr, starti, endi);
                //val = @"select * from (SELECT ROW_NUMBER() OVER(ORDER BY orderbyID DESC) AS AllowPagingId ,* FROM ( select *, 1 as orderbyID from ( "
                // + selectStr + @" ) as tbs1 ) as Tabl1 ) as table2 where AllowPagingId between " + ((PageIndex - 1) * PageSize).ToString() +
                //" and " + rows.ToString();
            }
            else
            {
                val = string.Format(@"select * from (SELECT ROW_NUMBER() OVER({0}) AS AllowPagingId ,* FROM 
( select * from ({1}) as tbs1 ) as Tabl1 ) as table2 where AllowPagingId between {2} and {3}",
                 orderStr, selectStr, starti, endi);
                //                val = @"select * from (SELECT ROW_NUMBER() OVER(" + orderStr + @") AS AllowPagingId ,* FROM ( select * from ( "
                //+ selectStr + @" ) as tbs1 ) as Tabl1 ) as table2 where AllowPagingId between " + ((PageIndex - 1) * PageSize).ToString() +
                //" and " + rows.ToString();
            }
            return val;
        }
    }
}
