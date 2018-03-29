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

namespace FxtCenterService.DataAccess
{
    public class SYSAreaDA : Base
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYSArea> GetSYSAreaList(SearchBase search, string areaid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.AreaList;
            if (search.CityId > 0)
            {
                sql += " and cityid=" + search.CityId;
            }
            if (!string.IsNullOrEmpty(areaid) && areaid.Trim()!="0")
            {
                sql += " and areaid in(" + areaid + ")";
            }
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<SYSArea>(sql, System.Data.CommandType.Text, parameters);
        }
        /// 获取省份列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定城市ID集合</param>
        /// <returns></returns>
        public static List<SYSProvince> GetProvinceList()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.ProvinceList;
            return ExecuteToEntityList<SYSProvince>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityList(int provinceid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.CityList;
            if (provinceid > 0)
            {
                sql += " and ProvinceId=" + provinceid;
            }
            return ExecuteToEntityList<SYSCity>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 获取区域
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="areaid">指定区域ID</param>
        /// <returns></returns>
        public static SYSArea GetSYSAreaById(int areaid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.AreaList;
            sql += " and areaid =" + areaid.ToString();
            return ExecuteToEntity<SYSArea>(sql, System.Data.CommandType.Text, parameters);
        }

    }
}
