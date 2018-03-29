using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using CAS.Common;
using System.Collections;

namespace FxtCenterService.DataAccess
{
    /// <summary>
    /// 读取Mariadb_FXTConnectionString
    /// </summary>
    public static class Dapper
    {
        private static readonly string connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["Mariadb_FXTConnectionString"]);//默认数据库
        private static readonly int cacheMinute = 2;

        public static MySqlConnection MySqlConnection(string connString = null)
        {
            var connection = string.IsNullOrEmpty(connString) ? new MySqlConnection(connectionString) : new MySqlConnection(connString);

            connection.Open();
            return connection;
        }

        public static MySqlParameter GetMySqlParameter(string key, object value, MySqlDbType type)
        {
            MySqlParameter param = new MySqlParameter(key, type);
            param.Value = value;
            return param;
        }

        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// zhoub 20160826
        /// </summary>  
        public static List<T> DataTableToList<T>(DataTable table) where T : new()
        {
            List<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (!item.PropertyType.IsGenericType)
                        {
                            //非泛型
                            item.SetValue(entity, DBNull.Value == row[item.Name] ? null : Convert.ChangeType(row[item.Name], item.PropertyType), null);
                        }
                        else
                        {
                            //泛型Nullable<>
                            Type genericTypeDefinition = item.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                item.SetValue(entity, DBNull.Value == row[item.Name] ? null : Convert.ChangeType(row[item.Name], Nullable.GetUnderlyingType(item.PropertyType)), null);
                            }
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }


        #region  临时存放


        /// <summary>
        /// 楼盘数据获取（已缓存）
        /// zhoub 20160902
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="typecode"></param>
        /// <returns></returns>
        public static DataSet GetBaseProject(MySqlConnection conn, int cityid, int fxtcompanyid, int typecode)
        {
            DataSet set = new DataSet();
            string key = "GetBaseProject_" + cityid + "_" + fxtcompanyid + "_" + typecode;
            if (CacheHelper.Contains<DataSet>(key))
            {
                set = CacheHelper.Get<DataSet>(key);
            }
            else
            {
                string sql = "SELECT cityid,projectid,projectname,othername,areaid,subareaid,address,isevaluate AS isevalue,usableyear,buildingnumber AS buildingnum,totalnumber AS totalnum,X,Y,pinyin,pinyinall FROM base_project p WHERE 1 = 1 AND p.cityid = " + cityid + " AND NOT EXISTS(SELECT ps.projectid FROM base_project_sub ps WHERE ps.projectid = p.projectid AND ps.fxtcompanyid = " + fxtcompanyid + " AND ps.cityid =p.cityid) AND valid = 1 AND CONCAT(',',(SELECT showcompanyid FROM privi_company_show_data WHERE fxtcompanyid = " + fxtcompanyid + " AND cityid = " + cityid + " AND typecode = " + typecode + "),',') LIKE CONCAT('%,',p.fxtcompanyid,',%') UNION SELECT cityid,projectid,projectname,othername,areaid,subareaid,address,isevaluate AS isevalue,usableyear,buildingnumber AS buildingnum,totalnumber AS totalnum,X,Y,pinyin,pinyinall FROM base_project_sub p WHERE 1 = 1 AND p.cityid = " + cityid + " AND p.fxtcompanyid = " + fxtcompanyid + " AND valid = 1";
                set = MySqlHelper.ExecuteDataset(conn, sql);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    CacheHelper.Set<DataSet>(key, set, cacheMinute);
                }
            }
            return set;
        }


        /// <summary>
        /// 图片数据获取（已缓存）
        /// zhoub 20160902
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="typecode"></param>
        /// <returns></returns>
        public static DataSet GetBasePhoto(MySqlConnection conn, int cityid, int fxtcompanyid, int typecode)
        {
            DataSet set = new DataSet();
            string key = "GetBasePhoto_" + cityid + "_" + fxtcompanyid + "_" + typecode;
            if (CacheHelper.Contains<DataSet>(key))
            {
                set = CacheHelper.Get<DataSet>(key);
            }
            else
            {
                string sql = "SELECT projectid,cityid,COUNT(*) AS photocnt FROM (SELECT id,projectid,cityid FROM base_lnk_p_photo p WHERE 1 = 1 AND NOT EXISTS (SELECT id FROM base_lnk_p_photo_sub ps WHERE ps.id = p.id AND ps.cityid = " + cityid + " AND ps.fxtcompanyid = " + fxtcompanyid + ") AND p.valid = 1 AND p.cityid = " + cityid + " AND CONCAT(',',(SELECT showcompanyid FROM privi_company_show_data WHERE fxtcompanyid = " + fxtcompanyid + " AND cityid = " + cityid + " AND typecode = " + typecode + "),',') LIKE CONCAT('%,',p.fxtcompanyid,',%') AND p.phototypecode LIKE '2009%' UNION SELECT id,projectid,cityid FROM base_lnk_p_photo_sub p WHERE 1 = 1 AND p.valid = 1 AND p.cityid = " + cityid + " AND p.fxtcompanyid = " + fxtcompanyid + " AND p.phototypecode LIKE '2009%') t GROUP BY projectid,cityid";
                set = MySqlHelper.ExecuteDataset(conn, sql);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    CacheHelper.Set<DataSet>(key, set, cacheMinute);
                }
            }
            return set;
        }

        /// <summary>
        /// 区域数据获取（已缓存）
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="typecode"></param>
        /// <returns></returns>
        public static DataSet GetSysarea(MySqlConnection conn)
        {
            DataSet set = new DataSet();
            string key = "GetSysarea_All";
            if (CacheHelper.Contains<DataSet>(key))
            {
                set = CacheHelper.Get<DataSet>(key);
            }
            else
            {
                string sql = "SELECT * FROM sys_area";
                set = MySqlHelper.ExecuteDataset(conn, sql);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    CacheHelper.Set<DataSet>(key, set, cacheMinute);
                }
            }
            return set;
        }

        /// <summary>  
        /// 转换为一个DataTable  
        /// zhoub 20160902
        /// </summary>  
        /// <typeparam name="TResult"></typeparam>  
        ///<param name="value"></param>  
        /// <returns></returns>  
        public static DataTable ToDataTable(IEnumerable list)
        {
            //创建属性的集合  
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口  
            Type type = list.AsQueryable().ElementType;
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列  
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例  
                DataRow row = dt.NewRow();
                //给row 赋值  
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable  
                dt.Rows.Add(row);
            }
            return dt;
        }


        #endregion
    }
}
