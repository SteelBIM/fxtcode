using CAS.Common;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess
{
    public class SPDBDA : Base
    {
        public static DataTable GetProjectList(SearchBase search, string key)
        {
            string sql = SQL.Project.ProjectListForSPDB;
            SqlCommand cmd = new SqlCommand();
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new DataTable();
            string projectTable = cityTb.ProjectTable;
            sql = sql.Replace("@table_project", projectTable);
            if (!string.IsNullOrWhiteSpace(key))
            {
                sql = sql.Replace("$keylimit", "and ([PinYin] like @key or [ProjectName] like @key or [OtherName] like @key or [PinYinAll] like @key or [address] like @key)");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@key",string.Format("%{0}%",key), SqlDbType.NVarChar, 128));
            }
            else
            {
                sql = sql.Replace("$keylimit", string.Empty);
            }
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.CommandText = sql;
            return ExecuteDataSet(cmd).Tables[0];
        }

        public static DataTable GetProjectCaseList(SearchBase search,
            int projectid,
            decimal buildingarea,
            List<int> purposecodes,
            int casetypecode,
            int floorno,
            string houseno,
            int buildingtypecode,
            int housetypecode,
            DateTime datebegin,
            DateTime dateend)
        {
            string sql = SQL.Project.ProjectCaseListForSPDB;
            SqlCommand cmd = new SqlCommand();
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new DataTable();
            string projectTable = cityTb.ProjectTable;
            string caseTable = cityTb.casetable;
            sql = sql.Replace("@projecttable", projectTable);
            sql = sql.Replace("@table_case", caseTable);
            //所在楼层
            if(floorno > 0)
            {
                sql = sql.Replace("$floornolimit", "and c.FloorNumber = @floorno");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@floorno", floorno, SqlDbType.Int));
            }
            else
            {
                sql = sql.Replace("$floornolimit", string.Empty);
            }
            //楼房号
            if (!string.IsNullOrWhiteSpace(houseno))
	        {
                sql = sql.Replace("$housenolimit", "and c.BuildingName + c.HouseNo like @houseno");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@houseno", string.Format("%{0}%", houseno), SqlDbType.NVarChar));
	        }
            else
            {
                sql = sql.Replace("$housenolimit", string.Empty);
            }
            //建筑类型
            if (buildingtypecode>0)
            {
                sql = sql.Replace("$buildingtypecodelimit", "and c.BuildingTypeCode = @buildingtypecode");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));
            }
            else
            {
                sql = sql.Replace("$buildingtypecodelimit", string.Empty);
            }
            if (casetypecode == 0)
            {
                //成交价
                sql = sql.Replace("$casetypecodelimit", "3001002,3001005");
                //去除不需要显示的案例数据,详见文档
                sql = sql.Replace("$casetypecodeappend", "and (c.PurposeCode not in (1002005,1002006,1002007,1002008,1002014,1002015,1002016,1002017,1002018,1002019,1002020,1002022,1002027) or (c.PurposeCode in (1002005,1002006,1002007,1002008,1002027) and (c.buildingtypecode = 2003001 or c.buildingtypecode = -1 or c.buildingtypecode is null) and (c.TotalFloor <= 3 or c.TotalFloor is null)))");
            }
            else
            {
                //挂牌房源
                sql = sql.Replace("$casetypecodelimit", "3001001,3001003,3001004");
                sql = sql.Replace("$casetypecodeappend", "");
            }
            if (purposecodes.Count > 0)
	        {
                string purposecodeLimit = string.Format("and c.PurposeCode in ({0})", string.Join(",", purposecodes));
                sql = sql.Replace("$purposecodelimit", purposecodeLimit);
	        }
            else
            {
                sql = sql.Replace("$purposecodelimit", "");
            }
            if (housetypecode > 0)
            {
                sql = sql.Replace("$housetypecode", "and c.HouseTypeCode = @housetypecode");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@housetypecode", housetypecode, SqlDbType.Int));
            }
            else
            {
                sql = sql.Replace("$housetypecode", "");
            }
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@datebegin", datebegin, SqlDbType.DateTime));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@dateend", dateend, SqlDbType.DateTime));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid",projectid,SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingarea", buildingarea,SqlDbType.Decimal));
            //cmd.Parameters.Add(SqlHelper.GetSqlParameter("@casetypecode",casetypecode,SqlDbType.Int));
            cmd.CommandText = sql;
            return ExecuteDataSet(cmd).Tables[0];
        }
    }
}
