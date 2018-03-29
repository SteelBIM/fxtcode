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
    public class SYSProvinceDA : Base
    {
        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<SYSProvince> GetSYSProvinceList(SearchBase search)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.ProvinceList;
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<SYSProvince>(sql, System.Data.CommandType.Text, parameters);
        }

    }
}
