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
using System.Diagnostics;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FxtCenterService.DataAccess
{
    public class DatBuildingDA : Base
    {
        public static int Add(DATBuilding model)
        {
            string tableName = Base.GetEntityTable<DATBuilding>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetIgnoreFields(new string[] { "fxtcompanyid" });
                model.SetPrimaryKeyIsIdentify(false);
            }
            else
            {
                model.SetIgnoreFields(new string[] { "fxt_companyid" });
                //model.SetPrimaryKeyIsIdentify(true);
            }
            return InsertFromEntity<DATBuilding>(model);
        }

        public static int Update(DATBuilding model)
        {
            string tableName = Base.GetEntityTable<DATBuilding>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetPrimaryKey<DATBuilding>(new string[] { "buildingid", "fxt_companyid" });
                model.SetIgnoreFields(new string[] { "fxtcompanyid", "createtime" });
            }
            else
            {
                model.SetPrimaryKey<DATBuilding>(new string[] { "buildingid" });
                model.SetIgnoreFields(new string[] { "fxt_companyid", "createtime" });
                //model.SetPrimaryKeyIsIdentify(true);
            }
            return UpdateFromEntity<DATBuilding>(model);
        }

        public static int Delete(int id)
        {
            int result = 0;
            string tableName = Base.GetEntityTable<DATBuilding>();
            if (tableName.IndexOf("_sub") > 0)
            {
                throw new Exception("子表包含多个主键，不能用一个主键字段删除记录");
            }
            else
            {
                result = DeleteByPrimaryKey<DATBuilding>(id);
            }
            return result;
        }

        //实体存在多个主键删除
        public static int Delete(DATBuilding model)
        {
            string tableName = Base.GetEntityTable<DATBuilding>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetIgnoreFields(new string[] { "fxtcompanyid" });
                model.SetPrimaryKeyIsIdentify(false);
            }
            else
            {
                model.SetIgnoreFields(new string[] { "fxt_companyid" });
                model.SetPrimaryKeyIsIdentify(true);
            }
            return DeleteByPrimaryKey<DATBuilding>(model);
        }

        public static DATBuilding GetDATBuildingByPK(int id)
        {
            DATBuilding model = new DATBuilding();
            model.buildingid = id;
            string tableName = Base.GetEntityTable<DATBuilding>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model = null;
                throw new Exception("子表包含多个主键，不能用一个主键字段查询记录");
            }
            else
            {
                model.SetIgnoreFields(new string[] { "fxt_companyid" });
                model.SetPrimaryKeyIsIdentify(true);
                model = ExecuteToEntityByEntity<DATBuilding>(model);
            }
            return model;
        }
        //实体存在多个主键查询
        public static DATBuilding GetDATBuildingByPK(DATBuilding model)
        {
            string tableName = Base.GetEntityTable<DATBuilding>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetIgnoreFields(new string[] { "fxtcompanyid" });
                model.SetPrimaryKeyIsIdentify(false);
            }
            else
            {
                model.SetIgnoreFields(new string[] { "fxt_companyid" });
                model.SetPrimaryKeyIsIdentify(true);
            }
            return ExecuteToEntityByEntity<DATBuilding>(model);
        }

        /// <summary>
        /// 根据楼盘ID获取所有楼栋列表（不分页）
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<DATBuilding> GetDATBuildingList(SearchBase search, int projectId, string key)
        {
            string sql = SQL.Project.BuildingBaseList;
            List<SqlParameter> parameters = new List<SqlParameter>();
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<DATBuilding>();
            //sql = sql.Replace("@dat_building", CityTableDA.GetCityTable(search.CityId).BuildingTable);
            sql = sql.Replace("@table_dat_project", city.ProjectTable);
            sql = sql.Replace("@fxtcompanyid", search.FxtCompanyId.ToString());
            sql = sql.Replace("@typecode", search.SysTypeCode.ToString());
            sql = sql.Replace("@cityid", search.CityId.ToString());
            sql = sql.Replace("@projectid", projectId.ToString());
            string strwhere = "";
            if (!string.IsNullOrEmpty(key))
            {
                strwhere += " and BuildingName = @key";
                parameters.Add(SqlHelper.GetSqlParameter("@key", SQLFilterHelper.EscapeLikeString(key, "$"), SqlDbType.NVarChar, 100));
            }
            strwhere += " and valid=1";
            sql = sql.Replace("@where", strwhere);
            sql = sql.Replace("@valid", strwhere);
            return ExecuteToEntityList<DATBuilding>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据楼盘ID获取所有楼栋列表（不分页）
        /// </summary>
        public static List<DATBuilding> GetDATBuildingList(SearchBase search, int projectId, int cityid, string key)
        {
            string sql = SQL.Project.BuildingBaseList;
            List<SqlParameter> parameters = new List<SqlParameter>();
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<DATBuilding>();
            //sql = sql.Replace("@dat_building", CityTableDA.GetCityTable(search.CityId).BuildingTable);
            sql = sql.Replace("@table_dat_project", city.ProjectTable);
            sql = sql.Replace("@companyid", search.CompanyId.ToString());
            sql = sql.Replace("@typecode", search.SysTypeCode.ToString());
            if (cityid > 0)
                sql = sql.Replace("@cityid", cityid.ToString());
            else
                sql = sql.Replace("@cityid", search.CityId.ToString());
            sql = sql.Replace("@projectid", projectId.ToString());
            string strwhere = "";
            if (!string.IsNullOrEmpty(key))
            {
                strwhere += " and BuildingName = @key";
                parameters.Add(SqlHelper.GetSqlParameter("@key", SQLFilterHelper.EscapeLikeString(key, "$"), SqlDbType.NVarChar, 100));
            }
            strwhere += " and valid=1";
            sql = sql.Replace("@where", strwhere);
            sql = sql.Replace("@valid", strwhere);
            return ExecuteToEntityList<DATBuilding>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 楼栋-获取楼栋下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<DATBuildingOrderBy> GetBuildingBaseInfoList(SearchBase search, int projectId, int avgprice)
        {
            string sql = SQL.Project.DATBuildingList;
            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                if (null == city)
                    return new List<DATBuildingOrderBy>();
                string buildingTable = city.BuildingTable;
                string houseTable = city.HouseTable;
                sql = sql.Replace("@table_building", buildingTable);
                sql = sql.Replace("@table_house", houseTable);
                parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.CompanyId, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@avgprice", avgprice, SqlDbType.Int));
                List<DATBuildingOrderBy> result = new List<DATBuildingOrderBy>();
                result = ExecuteToEntityList<DATBuildingOrderBy>(sql, System.Data.CommandType.Text, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 楼栋-获取楼栋下拉列表
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBuildingBaseInfoListList(SearchBase search, int projectId, int avgprice)
        {
            SqlCommand command = new SqlCommand();

            string sql = SQL.Project.DATBuildingList;
            try
            {
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                if (null == city)
                    return null;
                string buildingTable = city.BuildingTable;
                string houseTable = city.HouseTable;
                sql = sql.Replace("@table_building", buildingTable);
                sql = sql.Replace("@table_house", houseTable);
                command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
                command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                command.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.CompanyId, SqlDbType.Int));
                command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                command.Parameters.Add(SqlHelper.GetSqlParameter("@avgprice", avgprice, SqlDbType.Int));

                search.OrderBy = !string.IsNullOrEmpty(search.OrderBy) ? search.OrderBy : " buildingid asc";
                sql = HandleSQL(search, sql);
                command.CommandText = sql;
                return ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 楼栋-获取楼栋下拉列表forMCAS
        /// </summary>
        /// <returns></returns>
        public static List<DATBuildingOrderBy> GetBuildingBaseInfoList_MCAS(SearchBase search, int projectId, int avgprice, string key, string param, string serialno)
        {
            string sql = SQL.Project.DATBuildingList_MCAS;
            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                if (null == city)
                    return new List<DATBuildingOrderBy>();
                string buildingTable = city.BuildingTable;
                string houseTable = city.HouseTable;
                sql = sql.Replace("@table_building", buildingTable);
                sql = sql.Replace("@table_house", houseTable);
                parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                parameters.Add(SqlHelper.GetSqlParameter("@avgprice", avgprice, SqlDbType.Int));

                //if (!string.IsNullOrEmpty(key))
                //{
                //    sql += condition;
                parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar, 100));
                //}
                //if (!string.IsNullOrEmpty(param))
                //{
                parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 100));
                //}

                List<DATBuildingOrderBy> result = new List<DATBuildingOrderBy>();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //BaseDAEntity entity = new BaseDAEntity();
                result = ExecuteToEntityList<DATBuildingOrderBy>(sql, System.Data.CommandType.Text, parameters);
                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                //LogHelper.Info("楼栋sql执行时间：" + ts2.TotalMilliseconds + "ms.Guid:" + guid);
                //LogHelper.Info("projectid：" + projectId + ",cityid:" + search.CityId + ",fxtcompanyid:" + search.FxtCompanyId + ",typecode:" + search.SysTypeCode + ",avgprice:" + avgprice + ",buildingTable:" + buildingTable + ",houseTable:" + houseTable);

                //Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
                //    {
                //        sqltime = (int)ts2.TotalMilliseconds,
                //        functionname = "buildingbaseinfolistmcas",
                //        addtime = DateTime.Now,
                //        code = serialno,
                //        sqlconnetiontime = entity.SqlConnectionTime,
                //        sqlexecutetime = entity.SqlExecuteTime
                //    }));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 楼栋-获取楼栋下拉列表forMCAS(Mariadb)
        /// zhoub 20160826
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectId"></param>
        /// <param name="avgprice"></param>
        /// <param name="key"></param>
        /// <param name="param"></param>
        /// <param name="serialno"></param>
        /// <returns></returns>
        public static List<DATBuildingOrderBy> GetBuildingBaseInfoList_MCAS_Mariadb(SearchBase search, int projectId, int avgprice, string key, string param, string serialno)
        {
            Stopwatch swAll = new Stopwatch();
            swAll.Start();

            string sql = SQL.Project.Mariadb_DATBuildingList_MCAS;
            List<MySqlParameter> sqlParam = new List<MySqlParameter>();

            sqlParam.Add(Dapper.GetMySqlParameter("?projectid", projectId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?cityid", search.CityId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?fxtcompanyid", search.FxtCompanyId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?typecode", search.SysTypeCode, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?avgprice", avgprice, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?key", key, MySqlDbType.VarChar));
            sqlParam.Add(Dapper.GetMySqlParameter("?param", param, MySqlDbType.VarChar));

            Stopwatch swConn = new Stopwatch();
            swConn.Start();
            using (var conn = Dapper.MySqlConnection())
            {
                swConn.Stop();
                Stopwatch swExecute = new Stopwatch();
                swExecute.Start();
                DataSet set = MySqlHelper.ExecuteDataset(conn, sql, sqlParam.ToArray());
                swExecute.Stop();
                swAll.Stop();

                Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
                {
                    sqltime = (int)swAll.Elapsed.TotalMilliseconds,
                    functionname = "buildingbaseinfolistmcas",
                    code = serialno,
                    time = "mysql",
                    addtime = DateTime.Now,
                    sqlconnetiontime = (int)swConn.Elapsed.TotalMilliseconds,
                    sqlexecutetime = (int)swExecute.Elapsed.TotalMilliseconds
                }));
                List<DATBuildingOrderBy> result = Dapper.DataTableToList<DATBuildingOrderBy>(set.Tables[0]);
                return result;
            }
        }

        /// <summary>
        /// 楼栋-获取楼栋下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<DATBuilding> GetAutoBuildingInfoList(SearchBase search, int projectId, string key)
        {
            string sql = SQL.AutoPrice.BuildingList;
            SqlCommand cmd = new SqlCommand();
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                if (null == city)
                    return new List<DATBuilding>();
                string buildingTable = city.BuildingTable;
                sql = sql.Replace("@table_building", buildingTable);
                if (!string.IsNullOrEmpty(key))
                {
                    sql = sql.Replace("@key", " and buildingname like @buildingname ");
                    param.Add(SqlHelper.GetSqlParameter("@buildingname", "%" + key + "%", SqlDbType.NVarChar));
                }
                else
                {
                    sql = sql.Replace("@key", "");
                }
                param.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
                param.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                param.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                param.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                search.OrderBy = " isevalue desc,buildingid";
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteToEntityList<DATBuilding>(sql, CommandType.Text, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取楼栋信息
        /// 创建人:曾智磊,日期:2014-06-30
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="buildingName"></param>
        /// <returns></returns>
        public static DATBuilding GetBuildingByName(int cityId, int projectId, int fxtCompanyId, string buildingName, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.BuildingInfoGetByName;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_building", city.BuildingTable);
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@buildingname", buildingName, SqlDbType.VarChar));

            return ExecuteToEntity<DATBuilding>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据ID获取楼栋信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public static DATBuilding GetBuildingById(int cityId, int projectId, int fxtCompanyId, int buildingId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.BuildingInfoGetById;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_building", city.BuildingTable);
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));


            return ExecuteToEntity<DATBuilding>(sql, System.Data.CommandType.Text, parameters);
        }

        public static DataSet GetBuildingDetailInfo(int buildingId, SearchBase search)
        {

            string strSql = SQL.Project.BuildingDetailInfo;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            strSql = strSql.Replace("@buildingtable", "fxtproject." + city.BuildingTable);

            SqlCommand cmd = new SqlCommand(strSql);
            cmd.Parameters.Add(new SqlParameter("@CityId", search.CityId));
            cmd.Parameters.Add(new SqlParameter("@BuildingId", buildingId));
            cmd.Parameters.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            cmd.Parameters.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            cmd.CommandText = strSql;
            DataSet ds = ExecuteDataSet(cmd);

            if (buildingId == 697997)
            {
                //LogHelper.Info("CityId:" + search.CityId + ",BuildingId:" + buildingId + ",FxtCompanyId:" + search.FxtCompanyId + ",TypeCode:" + search.SysTypeCode + ",buildingtable:" + city.BuildingTable);
                //LogHelper.Info("gbdinfo;" + strSql);
            }
            return ds;
        }

        public static List<DATBuilding> GetBuildingDetailInfoList(int projectId, SearchBase search)
        {
            string strSql = SQL.Project.BuildingDetailInfoList;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            strSql = strSql.Replace("@buildingtable", "fxtproject." + city.BuildingTable);

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlCommand cmd = new SqlCommand(strSql);
            parameters.Add(new SqlParameter("@CityId", search.CityId));
            parameters.Add(new SqlParameter("@ProjectId", projectId));
            parameters.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            parameters.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            cmd.CommandText = strSql;
            return ExecuteToEntityList<DATBuilding>(strSql, System.Data.CommandType.Text, parameters);
        }

        public static void AddExecuteTimeLog(object olog)
        {
            ExecuteTimeLog log = olog as ExecuteTimeLog;
            ExecuteTimeLogDA.Add09(log);
        }
    }
}
