using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using CAS.Entity.DBEntity;
using MySql.Data.MySqlClient;

namespace FxtCenterService.DataAccess
{
    public class CityTableDA : Base
    {
        /// <summary>
        /// 取对应的城市表名
        /// </summary>
        /// <returns></returns>
        public static CityTable GetCityTable(int cityid)
        {
            CityTable result = null;
            List<CityTable> list = GlobleCache.CenterDBCityTable.Get();
            if (null == list || 0 == list.Count)
            {
                //从数据库中取数据并缓存
                list = GetSYSCityTableList(new SearchBase(), 0);
                GlobleCache.CenterDBCityTable.Add(list);
            }
            result = list.FirstOrDefault(t => t.cityid == cityid);
            return result;
        }

        /// <summary>
        /// 获取Mariadb城市对应表名
        /// zhoub 20160826
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static CityTable GetMariadbCityTable(int cityid)
        {
            CityTable result = null;
            List<CityTable> list = GlobleCache.Mariadb_CenterDBCityTable.Get();
            if (null == list || 0 == list.Count)
            {
                //从数据库中取数据并缓存
                list = GetMariadbSYSCityTableList();
                GlobleCache.Mariadb_CenterDBCityTable.Add(list);
            }
            result = list.FirstOrDefault(t => t.cityid == cityid);
            return result;
        }

        public static int Add(CityTable model)
        {
            GlobleCache.CenterDBCityTable.Reset();
            return InsertFromEntity<CityTable>(model);
        }
        public static int Update(CityTable model)
        {
            GlobleCache.CenterDBCityTable.Reset();
            return UpdateFromEntity<CityTable>(model);
        }
        public static int Delete(int id)
        {
            GlobleCache.CenterDBCityTable.Reset();
            return DeleteByPrimaryKey<CityTable>(id);
        }
        public static CityTable GetSYSCityTableByPK(int id)
        {
            return ExecuteToEntityByPrimaryKey<CityTable>(id);
        }
        public static List<CityTable> GetSYSCityTableList(SearchBase search, int cityId)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			if (0 < cityId)
			{
				search.Where += " and cityId = " + cityId.ToString();
			}
            string sql = SQL.Configuration.SYSCityTableList;
			sql = HandleSQL(search, sql);
            return ExecuteToEntityList<CityTable>(sql, System.Data.CommandType.Text, parameters);
		}

        /// <summary>
        /// 城市数据获取(Mariadb)
        /// zhoub 20160826
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static List<CityTable> GetMariadbSYSCityTableList()
        {
            string sql = "SELECT * FROM sys_city_table WHERE 1=1";
            using (var conn = Dapper.MySqlConnection())
            {
                DataSet set = MySqlHelper.ExecuteDataset(conn, sql);
                List<CityTable> result = Dapper.DataTableToList<CityTable>(set.Tables[0]);
                return result;
            }
        }
    }
}
