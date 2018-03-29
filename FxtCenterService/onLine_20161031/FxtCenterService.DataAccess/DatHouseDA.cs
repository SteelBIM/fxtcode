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
    public class DatHouseDA : Base
    {

        public static int Add(DATHouse model)
        {
            string tableName = Base.GetEntityTable<DATHouse>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetPrimaryKeyIsIdentify(false);
            }
            else
            {
                //model.SetPrimaryKeyIsIdentify(true);
            }
            return InsertFromEntity<DATHouse>(model);
        }

        public static int Update(DATHouse model)
        {
            string tableName = Base.GetEntityTable<DATHouse>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetPrimaryKey<DATHouse>(new string[] { "houseid", "fxtcompanyid" });
                model.SetIgnoreFields(new string[] { "createtime" });

            }
            else
            {
                model.SetPrimaryKey<DATHouse>(new string[] { "houseid" });
                model.SetIgnoreFields(new string[] { "createtime" });

            }
            return UpdateFromEntity<DATHouse>(model);
        }
        /// <summary>
        /// 获取单元或楼层列表(过滤删除的)
        /// </summary>
        /// <param name="buildingid">楼栋ID</param>
        /// <param name="files">字段 FloorNo/UnitNo </param>
        /// <param name="ordertype">排序方式 desc/asc </param> 
        /// <returns></returns>
        public static DataSet GetHouseFileListWithSub(int cityId, long buildingId, string files, string ordertype, int fxtcompanyid, int typecode)
        {
            files = files == "floorno" ? files : "unitno";
            ordertype = ordertype == "desc" ? ordertype : "asc";
            string sql = SQL.Project.FloorOrUnitDropDownList;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return new DataSet();
            string houseTable = city.HouseTable;
            SqlCommand cmd = new SqlCommand();
            try
            {
                sql += string.Format(" order by {0} {1}", files, ordertype); ;
                sql = sql.Replace("@table_house", houseTable);
                sql = sql.Replace("@filed", files);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
                cmd.CommandText = sql;
                return ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取房号下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<DATHouseOrderBy> GetHouseDropDownList(SearchBase search, long buildingId, int floorno)
        {
            string sql = SQL.Project.HouseDropDownList;
            List<SqlParameter> parameters = new List<SqlParameter>();
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<DATHouseOrderBy>();
            string houseTable = city.HouseTable;
            SqlCommand cmd = new SqlCommand();
            sql = sql.Replace("@table_house", houseTable);
            parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@floorno", floorno, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));

            List<DATHouseOrderBy> result = new List<DATHouseOrderBy>();
            result = ExecuteToEntityList<DATHouseOrderBy>(sql, System.Data.CommandType.Text, parameters);
            return result;
        }
        /// <summary>
        /// 获取房号下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<DATHouseOrderBy> GetHouseDropDownList_MCAS(SearchBase search, long buildingId, int floorno, string key, string param, string serialno)
        {
            string sql = SQL.Project.HouseDropDownList_MCAS;
            List<SqlParameter> parameters = new List<SqlParameter>();
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<DATHouseOrderBy>();
            string houseTable = city.HouseTable;
            SqlCommand cmd = new SqlCommand();
            sql = sql.Replace("@table_house", houseTable);

            if (floorno > 0)
            {
                sql = sql.Replace("@floornowhere", "AND floorno = @floorno");
                parameters.Add(SqlHelper.GetSqlParameter("@floorno", floorno, SqlDbType.Int));
            }
            else
            {
                sql = sql.Replace("@floornowhere", "");
            }

            parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar, 100));
            parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 100));

            parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));

            //string strwhere = string.Empty;
            //if (!string.IsNullOrEmpty(key))
            //{
            //    strwhere += condition;
            //    parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar, 100));
            //}
            //if (!string.IsNullOrEmpty(param))
            //{
            //    parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 100));
            //}

            //sql += strwhere;
            List<DATHouseOrderBy> result = new List<DATHouseOrderBy>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //BaseDAEntity entity = new BaseDAEntity();
            result = ExecuteToEntityList<DATHouseOrderBy>(sql, System.Data.CommandType.Text, parameters);
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            //LogHelper.Info("房号sql执行时间：" + ts2.TotalMilliseconds + "ms.Guid:" + guid);

            //Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
            //{
            //    sqltime = (int)ts2.TotalMilliseconds,
            //    functionname = "housedropdownlistmcas",
            //    addtime = DateTime.Now,
            //    code = serialno,
            //    sqlconnetiontime = entity.SqlConnectionTime,
            //    sqlexecutetime = entity.SqlExecuteTime
            //}));

            return result;
        }
        /// <summary>
        /// 获取房号下拉列表(Mariadb)
        /// zhoub 20160826
        /// </summary>
        /// <param name="search"></param>
        /// <param name="buildingId"></param>
        /// <param name="floorno"></param>
        /// <param name="key"></param>
        /// <param name="param"></param>
        /// <param name="serialno"></param>
        /// <returns></returns>
        public static List<DATHouseOrderBy> GetHouseDropDownList_MCAS_Mariadb(SearchBase search, long buildingId, int floorno, string key, string param, string serialno)
        {
            Stopwatch swAll = new Stopwatch();
            swAll.Start();
            string sql = SQL.Project.Mariadb_HouseDropDownList_MCAS;
            List<MySqlParameter> sqlParam = new List<MySqlParameter>();
            CityTable city = CityTableDA.GetMariadbCityTable(search.CityId);
            if (null == city)
            {
                return new List<DATHouseOrderBy>();
            }
            string houseTable = city.HouseTable;
            sql = sql.Replace("?table_house", houseTable);

            if (floorno > 0)
            {
                sql = sql.Replace("?floornowhere", "AND floorno = ?floorno");
                sqlParam.Add(Dapper.GetMySqlParameter("?floorno", floorno, MySqlDbType.Int32));
            }
            else
            {
                sql = sql.Replace("?floornowhere", "");
            }

            sqlParam.Add(Dapper.GetMySqlParameter("?key", key, MySqlDbType.VarChar));
            sqlParam.Add(Dapper.GetMySqlParameter("?param", param, MySqlDbType.VarChar));
            sqlParam.Add(Dapper.GetMySqlParameter("?buildingid", buildingId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?cityid", search.CityId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?fxtcompanyid", search.FxtCompanyId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?typecode", search.SysTypeCode, MySqlDbType.Int32));

            Stopwatch swConn = new Stopwatch();
            swConn.Start();
            using (var conn = Dapper.MySqlConnection())
            {
                swConn.Stop();
                Stopwatch swExecute = new Stopwatch();
                swExecute.Start();
                DataSet set = MySqlHelper.ExecuteDataset(conn, sql, sqlParam.ToArray());
                List<DATHouseOrderBy> result = Dapper.DataTableToList<DATHouseOrderBy>(set.Tables[0]);
                swExecute.Stop();
                swAll.Stop();
                Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
                    {
                        sqltime = (int)swAll.Elapsed.TotalMilliseconds,
                        functionname = "housedropdownlistmcas",
                        code = serialno,
                        time = "mysql",
                        addtime = DateTime.Now,
                        sqlconnetiontime = (int)swConn.Elapsed.TotalMilliseconds,
                        sqlexecutetime = (int)swExecute.Elapsed.TotalMilliseconds
                    }));
                return result;
            }
        }

        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数
        /// </summary>
        public static DataSet GetAutoFloorNoList(SearchBase search, int buildingid, string key)
        {
            try
            {
                string sql = SQL.AutoPrice.FloorNoList;
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                if (null == city)
                    return new DataSet();
                string houseTable = city.HouseTable;
                SqlCommand cmd = new SqlCommand();
                sql = sql.Replace("@table_house", houseTable);
                //if (!string.IsNullOrEmpty(key))
                //{
                //    sql = sql.Replace("@key", " and floorno like @floorno ");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@strKey", key + "%", SqlDbType.NVarChar));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@param", "%" + key + "%", SqlDbType.NVarChar));
                //}
                //else
                //{
                //    sql = sql.Replace("@key", "");
                //}
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingid, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.FxtCompanyId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                search.OrderBy = " (case when a.FloorNo like @strKey then 0 else 1 end) asc ,floorno";
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteDataSet(cmd);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数forMCAS
        /// </summary>
        public static DataSet GetAutoFloorNoList_MCAS(SearchBase search, int buildingid, string key, string param, string serialno)
        {
            try
            {
                string sql = SQL.AutoPrice.FloorNoList_MCAS;
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                string houseTable = city.HouseTable;
                SqlCommand cmd = new SqlCommand();
                sql = sql.Replace("@table_house", houseTable);
                //if (!string.IsNullOrEmpty(key))
                //{
                //sql = sql.Replace("@key", condition);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@strkey", key, SqlDbType.NVarChar, 100));
                //}
                //else
                //{
                //sql = sql.Replace("@key", "");
                //}
                //if (!string.IsNullOrEmpty(param))
                //{
                
                //}

                //楼层模糊查询
                if (param != "%%")
                {
                    sql = sql.Replace("@floornoparam", " and a.floorno like @param ");
                    cmd.Parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 100));
                }
                else
                {
                    sql = sql.Replace("@floornoparam", "");
                }              

                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingid, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.FxtCompanyId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                search.OrderBy = " (case when a.FloorNo like @strKey then 0 else 1 end) asc ,floorno";
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;

                var result = ExecuteDataSet(cmd);


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数forMCAS(Mariadb)
        /// zhoub 20160826
        /// </summary>
        /// <param name="search"></param>
        /// <param name="buildingid"></param>
        /// <param name="key"></param>
        /// <param name="param"></param>
        /// <param name="serialno"></param>
        /// <returns></returns>
        public static DataSet GetAutoFloorNoList_MCAS_Mariadb(SearchBase search, int buildingid, string key, string param, string serialno)
        {
            Stopwatch swAll = new Stopwatch();
            swAll.Start();

            string sql = SQL.AutoPrice.Mariadb_FloorNoList_MCAS;
            CityTable city = CityTableDA.GetMariadbCityTable(search.CityId);
            if (null == city)
                return null;
            string houseTable = city.HouseTable;
            sql = sql.Replace("?table_house", houseTable);
            List<MySqlParameter> sqlParam = new List<MySqlParameter>();
            sqlParam.Add(Dapper.GetMySqlParameter("?strkey", key, MySqlDbType.VarChar));
            sqlParam.Add(Dapper.GetMySqlParameter("?param", param, MySqlDbType.VarBinary));
            sqlParam.Add(Dapper.GetMySqlParameter("?buildingid", buildingid, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?cityid", search.CityId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?fxtcompanyid", search.FxtCompanyId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?typecode", search.SysTypeCode, MySqlDbType.Int32));

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
                        functionname = "housefloorlistmcas",
                        code = serialno,
                        time = "mysql",
                        addtime = DateTime.Now,
                        sqlconnetiontime = (int)swConn.Elapsed.TotalMilliseconds,
                        sqlexecutetime = (int)swExecute.Elapsed.TotalMilliseconds
                    }));
                return set;
            }
        }

        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数forMCAS
        /// </summary>
        public static DataSet GetAutoFloorNoList_OUT(SearchBase search, int buildingid, string key, string param)
        {
            try
            {
                string sql = SQL.AutoPrice.FloorNoList_OUT;
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                if (null == city) return new DataSet();
                string houseTable = city.HouseTable;
                SqlCommand cmd = new SqlCommand();
                sql = sql.Replace("@table_house", houseTable);
                //if (!string.IsNullOrEmpty(key))
                //{
                //    sql = sql.Replace("@key", condition);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@strkey", key, SqlDbType.NVarChar));
                //}
                //else
                //{
                //    sql = sql.Replace("@key", "");
                //}
                //if (!string.IsNullOrEmpty(param))
                //{
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 100));
                //}
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingid, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.FxtCompanyId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                search.OrderBy = " (case when c.FloorNo like @strKey then 0 else 1 end) asc ,floorno";
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取房号列表
        /// </summary>
        public static List<DATHouse> GetAutoHouseListList(SearchBase search, int buildingid, int? floorno, string key)
        {
            try
            {
                string sql = SQL.AutoPrice.HouseList;
                CityTable city = CityTableDA.GetCityTable(search.CityId);
                if (null == city)
                    return new List<DATHouse>();
                string buildingTable = city.BuildingTable;
                string houseTable = city.HouseTable;
                List<SqlParameter> param = new List<SqlParameter>();
                sql = sql.Replace("@table_building", buildingTable);
                sql = sql.Replace("@table_house", houseTable);
                if (!string.IsNullOrEmpty(key))
                {
                    sql = sql.Replace("@key", " and housename like @housename ");
                    param.Add(SqlHelper.GetSqlParameter("@housename", "%" + key + "%", SqlDbType.NVarChar));
                }
                else
                {
                    sql = sql.Replace("@key", "");
                }

                if (floorno.HasValue)
                {
                    sql = sql.Replace("@floorno", " and floorno  = " + floorno + " ");
                }
                else
                {
                    sql = sql.Replace("@floorno", " ");
                }

                param.Add(SqlHelper.GetSqlParameter("@buildingid", buildingid, SqlDbType.Int));
                param.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                param.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                param.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
                //param.Add(SqlHelper.GetSqlParameter("@floorno", floorno, SqlDbType.Int));
                search.OrderBy = " isevalue desc,houseid";
                sql = HandleSQL(search, sql);

                return ExecuteToEntityList<DATHouse>(sql, CommandType.Text, param);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 得到云估价数据
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="Type">0：正常询价，1：只要均价</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="QueryTypeCode">询价目的[1]</param>
        /// <returns>Tables[0]:询价结果，Tables[1]:案例统计，Tables[2]:案例明细，Tables[3]:按天统计案例均价</returns>
        public static DataSet GetEValueByProjectId(int CityId, int ProjectId, int BuildingId, int HouseId, int FXTCompanyId, int Type,
            int CompanyId, string UserId, double BuildingArea, string StartDate, string EndDate, int QueryTypeCode, int qid, int sysTypeCode, int subhousetype, double subhousearea, double subhouseavgprice, double subhousetotalprice)
        {

            string casid = null;
            try
            {
                string strsql = "[dbo].[procGetEValueByPId]";
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CityId", CityId));
                cmd.Parameters.Add(new SqlParameter("@PorjectId", ProjectId));
                cmd.Parameters.Add(new SqlParameter("@BuildingId", BuildingId));
                cmd.Parameters.Add(new SqlParameter("@HouseId", HouseId));
                cmd.Parameters.Add(new SqlParameter("@FXTCompanyId", FXTCompanyId));
                cmd.Parameters.Add(new SqlParameter("@EType", Type));//返回数据类型，0：正常询价，1：只要均价
                cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));//25,客户公司ID
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                cmd.Parameters.Add(new SqlParameter("@BuildingArea", BuildingArea));
                cmd.Parameters.Add(new SqlParameter("@sysTypeCode", sysTypeCode));//默认1003001
                cmd.Parameters.Add(new SqlParameter("@QueryPurposeCode", QueryTypeCode));//询价目的 1004001
                if (subhousetype > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseType", subhousetype));
                }
                if (subhousearea > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseArea", subhousearea));
                }
                if (subhouseavgprice > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseUnitPrice", subhouseavgprice));
                }
                if (subhousetotalprice > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseTotalPrice", subhousetotalprice));
                }
                if (!string.IsNullOrEmpty(StartDate))
                    cmd.Parameters.Add(new SqlParameter("@DateStart", StartDate));
                if (!string.IsNullOrEmpty(EndDate))
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", EndDate));
                if (!string.IsNullOrEmpty(casid))
                    cmd.Parameters.Add(new SqlParameter("@CASId", casid));
                if (qid != 0)
                    cmd.Parameters.Add(new SqlParameter("@QId", qid));
                DataSet ds = ExecuteDataSet(cmd);
                if (ds != null && ds.Tables.Count > 4 && Type == 0)
                {//删除多余的表             
                    int l = ds.Tables.Count - 4;
                    for (int r = 0; r < l; r++)
                        ds.Tables.RemoveAt(0);
                }
                if (ds != null && ds.Tables.Count > 1 && Type == 1)
                {//删除多余的表             
                    int l = ds.Tables.Count - 1;
                    for (int r = 0; r < l; r++)
                        ds.Tables.RemoveAt(0);
                }
                return ds;
            }
            catch (SqlException ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取房号信息
        /// 创建人:曾智磊,日期:2014-06-30
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="buildingId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="houseName"></param>
        /// <returns></returns>
        public static DATHouse GetHouseByName(int cityId, int buildingId, int fxtCompanyId, string houseName, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.HouseInfoGetByName;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_house", city.HouseTable);
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@housename", houseName, SqlDbType.VarChar));
            return ExecuteToEntity<DATHouse>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 根据获取房号信息
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="buildingId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public static DATHouse GetHouseById(int cityId, int buildingId, int fxtCompanyId, int houseId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.HouseInfoGetById;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_house", city.HouseTable);
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@houseid", houseId, SqlDbType.Int));
            return ExecuteToEntity<DATHouse>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 自动估价，这个方法将不往数据中心插入自动估价结果，所有的结果都会返回来
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="Type">0：正常询价，1：只要均价</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="QueryTypeCode">询价目的[1]</param>
        /// <returns>Tables[0]:询价结果，Tables[1]:案例统计，Tables[2]:案例明细，Tables[3]:按天统计案例均价</returns>
        public static DataSet GetCASEValueByPId(int CityId, int ProjectId, int BuildingId, int HouseId, int FXTCompanyId, int Type,
            int CompanyId, string UserId, double BuildingArea, string StartDate, string EndDate, int QueryTypeCode, int qid, int sysTypeCode, int subhousetype, double subhousearea, double subhouseavgprice, double subhousetotalprice)
        {

            string casid = null;
            try
            {
                string strsql = "[dbo].[procCASGetEValueByPId]";
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CityId", CityId));
                cmd.Parameters.Add(new SqlParameter("@PorjectId", ProjectId));
                cmd.Parameters.Add(new SqlParameter("@BuildingId", BuildingId));
                cmd.Parameters.Add(new SqlParameter("@HouseId", HouseId));
                cmd.Parameters.Add(new SqlParameter("@FXTCompanyId", FXTCompanyId));
                cmd.Parameters.Add(new SqlParameter("@EType", Type));//返回数据类型，0：正常询价，1：只要均价
                cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));//25,客户公司ID
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                cmd.Parameters.Add(new SqlParameter("@BuildingArea", BuildingArea));
                cmd.Parameters.Add(new SqlParameter("@sysTypeCode", sysTypeCode));//默认1003001
                cmd.Parameters.Add(new SqlParameter("@QueryPurposeCode", QueryTypeCode));//询价目的 1004001
                if (subhousetype > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseType", subhousetype));
                }
                if (subhousearea > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseArea", subhousearea));
                }
                if (subhouseavgprice > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseUnitPrice", subhouseavgprice));
                }
                if (subhousetotalprice > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@SubHouseTotalPrice", subhousetotalprice));
                }
                if (!string.IsNullOrEmpty(StartDate))
                    cmd.Parameters.Add(new SqlParameter("@DateStart", StartDate));
                if (!string.IsNullOrEmpty(EndDate))
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", EndDate));
                if (!string.IsNullOrEmpty(casid))
                    cmd.Parameters.Add(new SqlParameter("@CASId", casid));
                if (qid != 0)
                    cmd.Parameters.Add(new SqlParameter("@QId", qid));
                DataSet ds = ExecuteDataSet(cmd);
                if (ds != null && ds.Tables.Count > 4 && Type == 0)
                {//删除多余的表             
                    int l = ds.Tables.Count - 4;
                    for (int r = 0; r < l; r++)
                        ds.Tables.RemoveAt(0);
                }
                if (ds != null && ds.Tables.Count > 1 && Type == 1)
                {//删除多余的表             
                    int l = ds.Tables.Count - 1;
                    for (int r = 0; r < l; r++)
                        ds.Tables.RemoveAt(0);
                }
                return ds;
            }
            catch (SqlException ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }
        }


        public static DataSet GetHouseDetailInfo(int houseid, SearchBase search)
        {
            string strSql = SQL.Project.HouseDetailInfo;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            strSql = strSql.Replace("@housetable", "fxtproject." + city.HouseTable);

            SqlCommand cmd = new SqlCommand(strSql);
            cmd.Parameters.Add(new SqlParameter("@CityId", search.CityId));
            cmd.Parameters.Add(new SqlParameter("@HouseId", houseid));
            cmd.Parameters.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            cmd.Parameters.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            cmd.CommandText = strSql;

            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }
    }
}
