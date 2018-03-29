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
        public static List<SYSArea> GetSYSAreaList(SearchBase search, string areaid, string zipcode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.AreaList;
            if (search.CityId > 0)
            {
                sql += " and cityid=" + search.CityId;
            }
            if (!string.IsNullOrEmpty(areaid) && areaid.Trim() != "0")
            {
                sql += " and areaid in(" + areaid + ")";
            }
            if (!string.IsNullOrEmpty(zipcode) && zipcode.Trim() != "0")
            {
                sql += " and zipcode in(" + zipcode + ")";
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
        public static List<SYSCity> GetSYSCityList(int provinceid, int zipcode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.CityList;
            if (provinceid > 0)
            {
                parameters.Add(new SqlParameter("@provinceid", provinceid));
                sql += " and ProvinceId= @provinceid";
            }
            if (zipcode > 0)
            {
                parameters.Add(new SqlParameter("@zipcode", zipcode));
                sql += " and zipcode= @zipcode";
            }
            return ExecuteToEntityList<SYSCity>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取城市列表（根据省份zipcode）
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid">指定区域ID集合</param>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityListByPZipCode(int zipcode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.CityListByProvinceZipCode;

            if (zipcode > 0)
            {
                parameters.Add(new SqlParameter("@zipcode", zipcode));
                sql += " and p.zipcode = @zipcode";
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

        /// <summary>
        /// 获取片区列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cityId">指定城市ID</param>
        /// <param name="areaid">指定区域ID</param>
        /// <returns></returns>
        public static List<SYSSubArea> GetSubAreaList(SearchBase search, int areaId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.SubAreaList;
            if (areaId > 0)
            {
                sql += " where sa.AreaId = " + areaId;
            }
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<SYSSubArea>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取商业片区列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cityId">指定城市ID</param>
        /// <param name="areaid">指定区域ID</param>
        /// <returns></returns>
        public static List<SYSSubAreaBiz> GetSubAreaListBiz(SearchBase search, int areaId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.SubAreaBizList;
            sql += "and sa.FxtCompanyId = " + search.FxtCompanyId;
            if (search.CityId > 0)
            {
                sql += " and a.CityId = " + search.CityId;
            }
            if (areaId > 0)
            {
                sql += " and sa.AreaId = " + areaId;
            }
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<SYSSubAreaBiz>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取办公片区列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cityId">指定城市ID</param>
        /// <param name="areaid">指定区域ID</param>
        /// <returns></returns>
        public static List<SYSSubAreaOffice> GetSubAreaListOffice(SearchBase search, int areaId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.SubAreaOfficeList;
            sql += "and sa.FxtCompanyId = " + search.FxtCompanyId;
            if (search.CityId > 0)
            {
                sql += " and a.CityId = " + search.CityId;
            }
            if (areaId > 0)
            {
                sql += " and sa.AreaId = " + areaId;
            }
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<SYSSubAreaOffice>(sql, System.Data.CommandType.Text, parameters);
        }

        ///// <summary>
        ///// 获取公司开通的城市列表
        ///// </summary>
        ///// <returns></returns>
        //public static List<SYSCity> GetSYSCityListByCompany(string signname, int productcode)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    string sql = SQL.CityArea.CityListByCompany;
        //    parameters.Add(new SqlParameter("@signname", signname));
        //    parameters.Add(new SqlParameter("@productcode", productcode));

        //    return ExecuteToEntityList<SYSCity>(sql, System.Data.CommandType.Text, parameters);
        //}

        /// <summary>
        /// 根据城市ID获取城市信息
        /// </summary>
        /// <param name="cityIDs"></param>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityListByID(List<int> cityIDs)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < cityIDs.Count; i++)
			{
                parameters.Add(new SqlParameter("@cityID"+i,cityIDs[i]));
            } 
            string sql = string.Format("select * from FxtDataCenter.dbo.SYS_City where CityId in ({0});",string.Join(",",parameters.Select(o=>o.ParameterName)));
            return ExecuteToEntityList<SYSCity>(sql, System.Data.CommandType.Text, parameters);
        }
    }
}
