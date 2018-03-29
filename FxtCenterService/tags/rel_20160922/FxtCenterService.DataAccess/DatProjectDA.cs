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
using CAS.Entity.FxtProject;
using System.Diagnostics;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FxtCenterService.DataAccess
{
    public class DatProjectDA : Base
    {
        public static int Add(DATProject model)
        {
            string tableName = Base.GetEntityTable<DATProject>();
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
            return InsertFromEntity<DATProject>(model);
        }

        public static int Update(DATProject model)
        {
            string tableName = Base.GetEntityTable<DATProject>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetPrimaryKey<DATProject>(new string[] { "projectid", "fxt_companyid" });
                model.SetIgnoreFields(new string[] { "fxtcompanyid", "createtime" });

            }
            else
            {
                model.SetPrimaryKey<DATProject>(new string[] { "projectid" });
                model.SetIgnoreFields(new string[] { "fxt_companyid", "createtime" });

                //model.SetPrimaryKeyIsIdentify(true);
            }
            return UpdateFromEntity<DATProject>(model);
        }

        public static int Delete(int id)
        {
            int result = 0;
            string tableName = Base.GetEntityTable<DATProject>();
            if (tableName.IndexOf("_sub") > 0)
            {
                throw new Exception("子表包含多个主键，不能用一个主键字段删除记录");
            }
            else
            {
                result = DeleteByPrimaryKey<DATProject>(id);
            }
            return result;
        }

        public static int Delete(int[] ids)
        {
            int result = 0;
            string tableName = Base.GetEntityTable<DATProject>();
            if (tableName.IndexOf("_sub") > 0)
            {
                throw new Exception("子表包含多个主键，不能用一个主键字段删除记录");
            }
            else
            {
                result = DeleteByPrimaryKeyArray<DATProject>(ids);
            }
            return result;
        }

        //实体存在多个主键删除
        public static int Delete(DATProject model)
        {
            string tableName = Base.GetEntityTable<DATProject>();
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
            return DeleteByPrimaryKey<DATProject>(model);
        }
        /// <summary>
        /// 新增照片
        /// 创建人:曾智磊,日期:2014-07-07
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddPhoto(LNKPPhoto model)
        {
            string tableName = Base.GetEntityTable<LNKPPhoto>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetPrimaryKeyIsIdentify(false);
            }
            else
            {
                //model.SetPrimaryKeyIsIdentify(true);
            }
            return InsertFromEntity<LNKPPhoto>(model);
        }


        public static DATProject GetDATProjectByPK(int id)
        {
            DATProject model = new DATProject();
            model.projectid = id;
            string tableName = Base.GetEntityTable<DATProject>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model = null;
                throw new Exception("子表包含多个主键，不能用一个主键字段查询记录");
            }
            else
            {
                model.SetIgnoreFields(new string[] { "fxt_companyid" });
                model.SetPrimaryKeyIsIdentify(true);
                model = ExecuteToEntityByEntity<DATProject>(model);
            }
            return model;
        }

        //实体存在多个主键查询
        public static DATProject GetDATProjectByPK(DATProject model)
        {
            string tableName = Base.GetEntityTable<DATProject>();
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
            return ExecuteToEntityByEntity<DATProject>(model);
        }

        /// <summary>
        ///  获取楼盘列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="key"></param>
        /// <param name="areaid"></param>
        /// <param name="buildingtypecode"></param>
        /// <param name="purposecode"></param>
        /// <returns></returns>
        public static List<DATProject> GetDATProjectList(SearchBase search, string key, int areaid, int subareaid, int buildingtypecode, int purposecode)
        {
            string sql = SQL.Project.DATProjectList;
            List<SqlParameter> parameters = new List<SqlParameter>();
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<DATProject>();
            sql = sql.Replace("@table_dat_project", city.ProjectTable);
            sql = sql.Replace("@fxtcompanyid", search.FxtCompanyId.ToString());
            sql = sql.Replace("@typecode", search.SysTypeCode.ToString());
            string strwhere = "";
            if (!string.IsNullOrEmpty(key))
            {
                strwhere += " and ProjectName like @key escape '$'";
                parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar, 100));
            }
            if (search.CityId > 0)
            {
                strwhere += " and CityId = @CityId";
                parameters.Add(SqlHelper.GetSqlParameter("@CityId", search.CityId, SqlDbType.Int));
            }
            if (areaid > 0)
            {
                strwhere += " and AreaId = @AreaId";
                parameters.Add(SqlHelper.GetSqlParameter("@AreaId", areaid, SqlDbType.Int));
            }
            if (subareaid > 0)
            {
                strwhere += " and SubAreaId = @SubAreaId";
                parameters.Add(SqlHelper.GetSqlParameter("@SubAreaId", subareaid, SqlDbType.Int));
            }
            if (buildingtypecode > 0)
            {
                strwhere += " and BuildingTypeCode = @BuildingTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@BuildingTypeCode", buildingtypecode, SqlDbType.Int));
            }
            if (purposecode > 0)
            {
                strwhere += " and PurposeCode = @PurposeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@PurposeCode", purposecode, SqlDbType.Int));
            }
            sql = sql.Replace("@valid", " and valid=1");
            sql = sql.Replace("@where", strwhere);
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据楼盘名称，获取楼盘信息(取子表数据和主表未修改数据)
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectname"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoByName(int cityId, int areaId, int fxtCompanyId, string projectname, string typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectInfoGetByName;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_project", city.ProjectTable);
            sql = sql.Replace("@cityid", cityId.ToString());
            sql = sql.Replace("@fxtcompanyid", fxtCompanyId.ToString());
            sql = sql.Replace("@typecode", typecode.ToString());
            parameters.Add(SqlHelper.GetSqlParameter("@projectname", projectname, SqlDbType.VarChar));

            string strWhere = "";
            if (areaId > 0)
            {
                strWhere = " and AreaId=" + areaId;
            }
            sql = sql.Replace("@where", strWhere);

            return ExecuteToEntity<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据多个楼盘名称，获取楼盘信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectnames"></param>
        /// <returns></returns>
        public static List<DATProject> GetProjectInfoByNames(int cityId, int areaId, int fxtCompanyId, string[] projectnames, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectInfoGetByNames;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            sql = sql.Replace("@dat_project", city.ProjectTable);
            string _projectnames = "''";
            if (projectnames != null && projectnames.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in projectnames)
                {
                    sb.Append("'").Append(str).Append("',");
                }
                _projectnames = sb.ToString().TrimEnd(',');
            }
            sql = sql.Replace("@projectnames", _projectnames);

            string strWhere = "";
            if (areaId > 0)
            {
                strWhere = " and AreaId=" + areaId;
            }
            sql = sql.Replace("@where", strWhere);

            return ExecuteToEntityList<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 获得数据中心的楼盘信息
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoById(int cityid, int projectid, int fxtcompanyid, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectDetailInfo;
            CityTable city = CityTableDA.GetCityTable(cityid);
            if (null == city)
                return null;
            sql = sql.Replace("@projecttable", city.ProjectTable);

            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            return ExecuteToEntity<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获得数据中心的楼盘图片，没有联合附表
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static List<LNKPPhoto> GetProjectPhotoById(int cityid, int projectid, int fxtcompanyid, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectPhoto;
            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));

            return ExecuteToEntityList<LNKPPhoto>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获得数据中心的楼盘图片，没有联合附表forMCAS
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static List<LNKPPhoto> GetProjectPhotoById_MCAS(int cityid, int projectid, int buildingid, int fxtcompanyid, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectPhoto_MCAS;
            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingid, SqlDbType.Int));

            return ExecuteToEntityList<LNKPPhoto>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取楼盘下拉列表  
        /// </summary>
        public static DataTable GetProjectDropDownList(SearchBase search, string strkey, string param)
        {
            string sql = SQL.Project.ProjectDropDownList;
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new DataTable();
            string projectTable = cityTb.ProjectTable;
            string buildingTable = cityTb.BuildingTable;
            string houseTable = cityTb.HouseTable;
            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";

            if (search.FxtCompanyId == 177)
            {
                sql = sql.Replace("@addlike", " or [Address] like '%" + param + "%'");
            }
            else
            {
                sql = sql.Replace("@addlike", "");
            }

            sql = sql.Replace("@table_project", projectTable);
            sql = sql.Replace("@table_building", buildingTable);
            sql = sql.Replace("@table_house", houseTable);
            sql = sql.Replace("@top", topSql);
            SqlCommand cmd = new SqlCommand();
            sql = HandleSQL(search, sql); ;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CityId", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@strKey", strkey, SqlDbType.NVarChar, 81));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 82));
            cmd.CommandText = sql;

            //调试
            if (search.FxtCompanyId == 6)
            {
                LogHelper.Info(sql);
                LogHelper.Info("CityId:" + search.CityId + ",fxtcompanyid:" + search.FxtCompanyId + ",typecode:" + search.SysTypeCode + ",strKey:" + strkey + ",param" + param);
            }

            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 获取楼盘下拉列表forMCAS
        /// </summary>
        public static DataTable GetProjectDropDownList_MCAS(SearchBase search, string condition, string strkey, string buildingName, string param, string serialno, int priceorderby = 0)
        {
            string sql = SQL.Project.ProjectDropDownList_MCAS;
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new DataTable();
            string projectTable = cityTb.ProjectTable;
            string buildingTable = cityTb.BuildingTable;
            string houseTable = cityTb.HouseTable;
            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@table_project", projectTable);
            sql = sql.Replace("@table_building", buildingTable);
            sql = sql.Replace("@table_house", houseTable);
            sql = sql.Replace("@dat_weightproject", cityTb.weightproject);
            sql = sql.Replace("@dat_projectavg", cityTb.projectavgtable);
            //sql = sql.Replace("@where", condition);
            sql = sql.Replace("@top", topSql);

            SqlCommand cmd = new SqlCommand();
            //行政区
            if (search.AreaId > 0)
            {
                sql = sql.Replace("@areawhere", " and areaid = @areaid ");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", search.AreaId, SqlDbType.Int));
            }
            else
            {
                sql = sql.Replace("@areawhere", "");
            }

            //价格排序
            //if (priceorderby == 1)
            //{
            //    sql = sql.Replace("@priceorderby", " avgprice asc,");
            //    sql = sql.Replace("@pricewhere", " and avgprice > 0 and avgprice is not null ");

            //}
            //else if (priceorderby == 2)
            //{
            //    sql = sql.Replace("@priceorderby", " avgprice desc,");
            //    sql = sql.Replace("@pricewhere", " and avgprice > 0 and avgprice is not null ");
            //}
            //else
            //{
            //    sql = sql.Replace("@priceorderby", " ");
            //    sql = sql.Replace("@pricewhere", " ");
            //}

            if (!string.IsNullOrEmpty(buildingName))
            {
                string buildingSql = @"select Projectid
                                                from @buildingTable b with(nolock)
                                                where not exists (
        			                                        select BuildingId from @buildingTable_sub bb where bb.BuildingId = b.BuildingId
        				                                        and b.fxtcompanyid = @FxtCompanyId
        			                                        )
                                                and b.fxtcompanyid in (
        		                                        select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
        		                                        where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
        		                                        )
                                                and b.CityID = @CityId
                                                and b.valid = 1
                                                and b.BuildingName like @buildingname or b.OtherName like @buildingname
                                                union
                                                select Projectid
                                                from @buildingTable_sub bb with(nolock)
                                                where bb.Fxt_CompanyId  = @FxtCompanyId
                                                and bb.CityID = @CityId
                                                and bb.valid = 1
                                                and bb.BuildingName like @buildingname or bb.OtherName like @buildingname";

                buildingSql = buildingSql.Replace("@buildingTable", buildingTable);

                sql = sql.Replace("@buildingwhere", "and t.projectid in( " + buildingSql + " ) ");

                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingname", "%" + buildingName + "%", SqlDbType.NVarChar));
            }
            else
            {
                sql = sql.Replace("@buildingwhere", "");
            }
            if (!string.IsNullOrEmpty(param))
            {
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 82));
            }

            sql = HandleSQL(search, sql);
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CityId", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@strKey", strkey, SqlDbType.NVarChar, 81));
            cmd.CommandText = sql;

            //LogHelper.Info("search.AreaId=" + search.AreaId);
            //LogHelper.Info("search.AreaId=" + search.CityId);
            //LogHelper.Info("search.AreaId=" + search.FxtCompanyId);
            //LogHelper.Info("search.AreaId=" + search.SysTypeCode);
            //LogHelper.Info("search.AreaId=" + strkey);
            //LogHelper.Info(sql);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //BaseDAEntity entity = new BaseDAEntity();
            var result = ExecuteDataSet(cmd).Tables[0];
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;

            ////LogHelper.Info("楼盘sql执行时间：" + ts2.TotalMilliseconds + "ms.Guid:" + guid);
            //Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
            //{
            //    sqltime = (int)ts2.TotalMilliseconds,
            //    functionname = "projectdropdownlistmcas",
            //    code = serialno,
            //    addtime = DateTime.Now,
            //    sqlconnetiontime = entity.SqlConnectionTime,
            //    sqlexecutetime = entity.SqlExecuteTime
            //}));

            return result;
        }

        /// <summary>
        /// 获取楼盘下拉列表forMCAS(查询Mariadb库)
        /// zhoub 20160825
        /// </summary>
        /// <param name="search"></param>
        /// <param name="condition"></param>
        /// <param name="strkey"></param>
        /// <param name="buildingName"></param>
        /// <param name="param"></param>
        /// <param name="serialno"></param>
        /// <param name="priceorderby"></param>
        /// <returns></returns>
        public static DataTable GetProjectDropDownList_MCAS_Mariadb(SearchBase search, string condition, string strkey, string buildingName, string param, string serialno, int priceorderby = 0)
        {
            Stopwatch swAll = new Stopwatch();
            swAll.Start();
            string sql = SQL.Project.Mariadb_ProjectDropDownList_MCAS;
            search.PageRecords = search.Top > 0 ? search.Top : search.PageRecords;
            sql = HandleMySQL(search, sql);

            List<MySqlParameter> sqlParam = new List<MySqlParameter>();
            //行政区
            if (search.AreaId > 0)
            {
                sql = sql.Replace("?areawhere", " and areaid = ?areaid ");
                sqlParam.Add(Dapper.GetMySqlParameter("?areaid", search.AreaId, MySqlDbType.Int32));
            }
            else
            {
                sql = sql.Replace("?areawhere", "");
            }

            if (!string.IsNullOrEmpty(buildingName))
            {
                string buildingSql = @"SELECT Projectid
                                                FROM base_building b 
                                                WHERE NOT EXISTS (
        			                                        SELECT BuildingId FROM base_building_sub bb WHERE bb.BuildingId = b.BuildingId
        				                                        AND b.fxtcompanyid = ?FxtCompanyId
        			                                        )
        									    AND CONCAT(',',(SELECT showcompanyid FROM privi_company_show_data
                                                    WHERE fxtcompanyid = ?fxtcompanyid
                                                    AND cityid = ?cityid
                                                    AND typecode = ?fxtcompanyid),',') LIKE CONCAT('%,',b.fxtcompanyid,',%')
                                                AND b.CityID = ?CityId
                                                AND b.valid = 1
                                                AND b.BuildingName LIKE ?buildingname OR b.OtherName LIKE ?buildingname
                                                UNION all
                                                SELECT Projectid
                                                FROM base_building_sub bb 
                                                WHERE bb.fxtcompanyid  = ?FxtCompanyId
                                                AND bb.CityID = ?CityId
                                                AND bb.valid = 1
                                                AND bb.BuildingName LIKE ?buildingname OR bb.OtherName LIKE ?buildingname";

                sql = sql.Replace("?buildingwhere", "and t.projectid in( " + buildingSql + " ) ");
                sqlParam.Add(Dapper.GetMySqlParameter("?buildingname", "%" + buildingName + "%", MySqlDbType.VarChar));
            }
            else
            {
                sql = sql.Replace("?buildingwhere", "");
            }
            if (!string.IsNullOrEmpty(param))
            {
                sqlParam.Add(Dapper.GetMySqlParameter("?param", param, MySqlDbType.VarChar));
            }

            sqlParam.Add(Dapper.GetMySqlParameter("?CityId", search.CityId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?fxtcompanyid", search.FxtCompanyId, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?typecode", search.SysTypeCode, MySqlDbType.Int32));
            sqlParam.Add(Dapper.GetMySqlParameter("?strKey", strkey, MySqlDbType.VarChar));

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
                        functionname = "projectdropdownlistmcas",
                        code = serialno,
                        time = "mysql",
                        addtime = DateTime.Now,
                        sqlconnetiontime = (int)swConn.Elapsed.TotalMilliseconds,
                        sqlexecutetime = (int)swExecute.Elapsed.TotalMilliseconds
                    }));
                return set.Tables[0];
            }
        }

        /// <summary>
        /// 获取楼盘下拉列表forMCAS(查询Mariadb库)
        /// zhoub 20160825
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetProjectDropDownList_MCAS_Mariadb(SearchBase search, string condition, string strkey, string buildingName, string param, string serialno, int priceorderby = 0)
        //{
        //    Stopwatch swAll = new Stopwatch();
        //    swAll.Start();
        //    Stopwatch swConn = new Stopwatch();
        //    swConn.Start();
        //    strkey = strkey.Replace("%", "");
        //    param = param.Replace("%", "");
        //    DataTable dt = new DataTable();
        //    using (var conn = Dapper.MySqlConnection())
        //    {
        //        swConn.Stop();
        //        Stopwatch swExecute = new Stopwatch();
        //        swExecute.Start();
        //        DataSet projectSet = Dapper.GetBaseProject(conn, search.CityId, search.FxtCompanyId, search.SysTypeCode);
        //        DataSet basePhotoSet = Dapper.GetBasePhoto(conn, search.CityId, search.FxtCompanyId, search.SysTypeCode);
        //        DataSet sysareaSet = Dapper.GetSysarea(conn);

        //        var quary = from pl in projectSet.Tables[0].AsEnumerable() select pl;
        //        //行政区
        //        if (search.AreaId > 0)
        //        {
        //            quary = from pl in quary where pl.Field<int>("areaid") == search.AreaId select pl;
        //        }

        //        if (!string.IsNullOrEmpty(param))
        //        {
        //            quary = from pl in quary where pl.Field<string>("projectname").Contains(param) || pl.Field<string>("othername").Contains(param) || pl.Field<string>("PinYin").Contains(param) || pl.Field<string>("PinYinAll").Contains(param) || pl.Field<string>("address").Contains(param) select pl;
        //        }

        //        var result = from pro in quary
        //                     from pho in basePhotoSet.Tables[0].AsEnumerable() 
        //                     //from area in sysareaSet.Tables[0].AsEnumerable()
        //                     where pro.Field<int>("projectid") == pho.Field<int>("projectid") && pro.Field<int>("cityid") == pho.Field<int>("cityid") //&& pro.Field<int>("areaid") == area.Field<int>("areaid")
        //                     select new
        //                         {
        //                             cityid = pro.Field<int>("cityid"),
        //                             projectid = pro.Field<int>("projectid"),
        //                             projectname = (string.IsNullOrEmpty(pro.Field<string>("projectname")) ? "" : pro.Field<string>("projectname")),
        //                             othername = (string.IsNullOrEmpty(pro.Field<string>("othername")) ? "" : pro.Field<string>("othername")),
        //                             areaid = pro.Field<int>("areaid"),
        //                             subareaid = (string.IsNullOrEmpty(pro.Field<int>("subareaid").ToString()) ? 0 : pro.Field<int>("subareaid")),
        //                             address = (string.IsNullOrEmpty(pro.Field<string>("address")) ? "" : pro.Field<string>("address")),
        //                             //isevalue = (pro.Field<int?>("isevalue")>0 ? 0 : pro.Field<int>("isevalue")),
        //                             //usableyear = (string.IsNullOrEmpty(pro.Field<int>("usableyear").ToString()) ? 0 : pro.Field<int>("usableyear")),
        //                             ////areaname = (string.IsNullOrEmpty(area.Field<string>("areaname")) ? "" : area.Field<string>("areaname")),
        //                             //buildingtotal = (string.IsNullOrEmpty(pro.Field<int>("buildingnum").ToString()) ? 0 : pro.Field<int>("buildingnum")),
        //                             //housetotal = (string.IsNullOrEmpty(pro.Field<int>("totalnum").ToString()) ? 0 : pro.Field<int>("totalnum")),
        //                             //x = (string.IsNullOrEmpty(pro.Field<double>("x").ToString()) ? 0 : pro.Field<double>("x")),
        //                             //y = (string.IsNullOrEmpty(pro.Field<double>("y").ToString()) ? 0 : pro.Field<double>("y")),
        //                             //photocnt = (string.IsNullOrEmpty(pho.Field<int>("photocnt").ToString()) ? 0 : pho.Field<int>("photocnt")),
        //                             avgprice = 0,
        //                             ordernumber = (pro.Field<string>("projectname").StartsWith(strkey) ? 0 : 1)
        //                         };

        //        result = result.OrderBy(or => or.ordernumber).OrderByDescending(or => or.projectid);
        //        //string s = result.ToJson();
        //        dt = Dapper.ToDataTable(result);
        //        swExecute.Stop();
        //        swAll.Stop();
        //        Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
        //        {
        //            sqltime = (int)swAll.Elapsed.TotalMilliseconds,
        //            functionname = "projectdropdownlistmcas",
        //            code = serialno,
        //            time = "mysql",
        //            addtime = DateTime.Now,
        //            sqlconnetiontime = (int)swConn.Elapsed.TotalMilliseconds,
        //            sqlexecutetime = (int)swExecute.Elapsed.TotalMilliseconds
        //        }));
        //    }
        //    return dt;
        //}

        /// <summary>
        /// 获取楼盘下拉列表forMCAS
        /// </summary>
        public static List<DATProject> GetProjectDropDownList_MCAS2(SearchBase search, string condition, string strkey, string buildingName, string param, string serialno)
        {
            string sql = SQL.Project.ProjectDropDownList_MCAS;
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);

            string projectTable = cityTb.ProjectTable;
            string buildingTable = cityTb.BuildingTable;
            string houseTable = cityTb.HouseTable;
            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@table_project", projectTable);
            sql = sql.Replace("@table_building", buildingTable);
            sql = sql.Replace("@table_house", houseTable);
            sql = sql.Replace("@dat_weightproject", cityTb.weightproject);
            sql = sql.Replace("@dat_projectavg", cityTb.projectavgtable);
            //sql = sql.Replace("@where", condition);
            sql = sql.Replace("@top", topSql);

            List<SqlParameter> sqlParam = new List<SqlParameter>();
            //行政区
            if (search.AreaId > 0)
            {
                sql = sql.Replace("@areawhere", " and areaid = @areaid ");
                sqlParam.Add(SqlHelper.GetSqlParameter("@areaid", search.AreaId, SqlDbType.Int));
            }
            else
            {
                sql = sql.Replace("@areawhere", "");
            }

            sql = sql.Replace("@buildingwhere", "");

            if (!string.IsNullOrEmpty(param))
            {
                sqlParam.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 82));
            }

            sql = HandleSQL(search, sql);
            sqlParam.Add(SqlHelper.GetSqlParameter("@CityId", search.CityId, SqlDbType.Int));
            sqlParam.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            sqlParam.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            sqlParam.Add(SqlHelper.GetSqlParameter("@strKey", strkey, SqlDbType.NVarChar, 81));

            Stopwatch sw = new Stopwatch();
            sw.Start();

            //BaseDAEntity entity = new BaseDAEntity();
            var result = ExecuteToEntityList<DATProject>(sql, CommandType.Text, sqlParam);
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            //LogHelper.Info("楼盘sql执行时间：" + ts2.TotalMilliseconds + "ms.Guid:" + guid);
            int time = (int)ts2.TotalMilliseconds;
            //if (time > 2000)
            //{
            //new Task(DatProjectDA.AddExecuteTimeLog, new ExecuteTimeLog()
            //{
            //    sqltime = (int)ts2.TotalMilliseconds,
            //    functionname = "projectdropdownlistmcas",
            //    addtime = DateTime.Now,
            //    code = serialno,
            //    sqlconnetiontime = entity.SqlConnectionTime,
            //    sqlexecutetime = entity.SqlExecuteTime
            //}).Start();
            //ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
            //{
            //    sqltime = (int)ts2.TotalMilliseconds,
            //    functionname = "projectdropdownlistmcas",
            //    addtime = DateTime.Now,
            //    ident = guid,
            //    sqlconnetiontime = entity.SqlConnectionTime,
            //    sqlexecutetime = entity.SqlExecuteTime
            //});
            //}

            return result;
        }

        public static void AddExecuteTimeLog(object olog)
        {
            ExecuteTimeLog log = olog as ExecuteTimeLog;
            ExecuteTimeLogDA.Add09(log);
        }

        /// <summary>
        /// 楼盘案例
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DataTable GetProjectCase(SearchBase search, int projectid, int fxtcompanyid, int cityid)
        {
            string sql = SQL.Project.ProjectCase;
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new DataTable();
            string projectTable = cityTb.ProjectTable;
            string caseTable = cityTb.casetable;
            SqlCommand cmd = new SqlCommand();

            sql = sql.Replace("@table_project", projectTable);
            sql = sql.Replace("@table_case", caseTable);
            sql = HandleSQL(search, sql);
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", search.DateBegin, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", search.DateEnd, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 楼盘案例forMCAS
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DataTable GetProjectCase_MCAS(SearchBase search, int projectid, int fxtcompanyid, int cityid)
        {
            string sql = SQL.Project.ProjectCase_MCAS;
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new DataTable();
            string projectTable = cityTb.ProjectTable;
            string caseTable = cityTb.casetable;
            SqlCommand cmd = new SqlCommand();

            sql = sql.Replace("@table_project", projectTable);
            sql = sql.Replace("@table_case", caseTable);
            sql = HandleSQL(search, sql);
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", search.DateBegin, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", search.DateEnd, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            return ExecuteDataSet(cmd).Tables[0];
        }
        /// <summary>
        /// 获取自动估价楼盘信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<DATProject> GetSearchProjectListByKey(SearchBase search, int fxtcompanyid, int cityid, string key)
        {
            string sql = SQL.AutoPrice.ProjectList;
            List<SqlParameter> param = new List<SqlParameter>();
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new List<DATProject>();
            sql = sql.Replace("@table_project", cityTb.ProjectTable);
            search.OrderBy = " isevalue desc";
            sql = HandleSQL(search, sql);
            string address = string.Format("%{0}%", key);
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@key", address, SqlDbType.NVarChar));
            param.Add(SqlHelper.GetSqlParameter("@address", address, SqlDbType.NVarChar));
            return ExecuteToEntityList<DATProject>(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 获取自动估价楼盘详细信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DATProject GetProjectDetailsByProjectid(SearchBase search, int fxtcompanyid, int cityid, int projectid)
        {
            string sql = SQL.AutoPrice.ProjectDetail,
                sqlhead = "declare @casemonth int select @casemonth=casemonth from FxtDataCenter.dbo.SYS_City with(nolock) where cityid = @cityid \n";
            List<SqlParameter> param = new List<SqlParameter>();
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return null;
            sql = sql.Replace("@table_project", cityTb.ProjectTable);
            sql = sql.Replace("@casetable", cityTb.casetable);
            search.OrderBy = " isevalue desc,casecnt desc,projectid desc";
            sql = HandleSQL(search, sql);
            sql = sqlhead + sql;
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            return ExecuteToEntity<DATProject>(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 楼盘详细：楼盘ID，楼盘名，区域名，是否可估，停车位，管理费，地址，区域id,竣工时间，开发商，物业管理  
        /// hody,暂为易房保
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoDetailsByProjectid(SearchBase search, int fxtcompanyid, int cityid, int projectid)
        {
            string sql = SQL.ProjectInfo.ProjectDetails;

            List<SqlParameter> param = new List<SqlParameter>();
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return null;
            sql = sql.Replace("@table_project", cityTb.ProjectTable);
            search.OrderBy = " isevalue desc,projectid desc";
            sql = HandleSQL(search, sql);
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            return ExecuteToEntity<DATProject>(sql, CommandType.Text, param);
        }
        /// <summary>
        /// 根据公司ID和楼盘ID在子表中查询数据
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DATProject GetProjectSubByProjectIdAndCompanyId(int fxtCompanyId, int cityId, int projectId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectSubInfoByProjIdAndComId;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_project", city.ProjectTable);
            sql = sql.Replace("@projectid", projectId.ToString());
            sql = sql.Replace("@cityid", cityId.ToString());
            sql = sql.Replace("@fxtcompanyid", fxtCompanyId.ToString());
            return ExecuteToEntity<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 根据公司ID和楼盘ID在主表中查询数据
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static DATProject GetProjectParentByProjectIdAndCompanyId(int fxtCompanyId, int cityId, int projectId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectParentInfoByProjIdAndComId;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_project", city.ProjectTable);
            sql = sql.Replace("@projectid", projectId.ToString());
            sql = sql.Replace("@cityid", cityId.ToString());
            sql = sql.Replace("@fxtcompanyid", fxtCompanyId.ToString());
            return ExecuteToEntity<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据楼盘ID获取楼盘信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoByProjectId(int cityId, int fxtCompanyId, int projectId, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectInfoGetById;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_project", city.ProjectTable);
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
            return ExecuteToEntity<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取楼盘附属房屋信息forMCAS kujj20150714
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMCASProjectSubHouse(int projectid, long buildingid, long houseid, int cityid, int fxtcompanyid, int systypecode)
        {
            string sql = SQL.Project.ProjectSubHouse_MCAS;
            CityTable cityTb = CityTableDA.GetCityTable(cityid);
            SqlCommand cmd = new SqlCommand();

            sql = sql.Replace("@projecttable", cityTb.ProjectTable);
            sql = sql.Replace("@projecttable_sub", cityTb.ProjectTable + "_sub");
            sql = sql.Replace("@buildingtable", cityTb.BuildingTable);
            sql = sql.Replace("@buildingtable_sub", cityTb.BuildingTable + "_sub");
            sql = sql.Replace("@housetable", cityTb.HouseTable);
            sql = sql.Replace("@housetable_sub", cityTb.HouseTable + "_sub");
            sql = sql.Replace("@subhouseprice", cityTb.SubHousePriceTable);


            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingid, SqlDbType.BigInt));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@houseid", houseid, SqlDbType.BigInt));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", systypecode, SqlDbType.Int));
            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 获取楼盘附属房屋价格 tanql20150908
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProjectSubHouse(int projectid, SearchBase search)
        {
            CityTable city = CityTableDA.GetCityTable(search.CityId);

            string sql = @"select s.SubHouseType,s.SubHouseUnitPrice,c.CodeName as SubHouseTypeName
                            from FXTProject.@subhouseprice s 
                            left join FxtDataCenter.dbo.SYS_Code c 
                            on c.Code = s.SubHouseType 
                            where ProjectId = @projectid and CityId = @cityid and 
                            FxtCompanyId = @fxtcompanyid ";
            sql = sql.Replace("@subhouseprice", city.SubHousePriceTable);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            return ExecuteDataSet(cmd).Tables[0];
        }
        /// <summary>
        /// 获取装修单价列表 tanql20150909
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static DataTable GetFitmentPriceList(SearchBase search)
        {
            string sql = @"select f.FitmentPrice,f.FitmentCode from fxtproject.dbo.sys_FitmentPrice f where CityId = @cityid and 
                            FxtCompanyId = @fxtcompanyid ";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 获取楼盘详细(包含codeName) tanql20150911
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static DataSet GetProjectDetailInfo(int projectId, SearchBase search)
        {
            string strSql = SQL.Project.ProjectDetailInfoContainCodeName;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            strSql = strSql.Replace("@projecttable", "fxtproject." + city.ProjectTable);
            string where = " and ProjectId = " + projectId;
            strSql = strSql.Replace("@projectid", where);

            SqlCommand cmd = new SqlCommand(strSql);
            cmd.Parameters.Add(new SqlParameter("@CityId", search.CityId));
            cmd.Parameters.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            cmd.Parameters.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            //cmd.Parameters.Add(new SqlParameter("@ProjectId", projectId));
            cmd.CommandText = strSql;
            //LogHelper.Info("楼盘详细 CityId:" + search.CityId + ";FxtCompanyId:" + search.FxtCompanyId + ";TypeCode:" + search.SysTypeCode + ";ProjectId:" + projectId);
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }

        /// <summary>
        /// 获取楼盘详细(包含codeName) tanql20150911
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<DATProject> GetProjectDetail(string[] pIds, SearchBase search)
        {
            string strSql = SQL.Project.ProjectDetailInfoContainCodeName;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            strSql = strSql.Replace("@projecttable", "fxtproject." + city.ProjectTable);

            List<SqlParameter> paramsList = new List<SqlParameter>();
            paramsList.Add(new SqlParameter("@CityId", search.CityId));
            paramsList.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            paramsList.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            string where = string.Empty;
            if (pIds != null && pIds.Length > 0)
            {
                where = " and ProjectId in (" + string.Join(",", pIds) + ")";
            }
            strSql = strSql.Replace("@projectid", where);

            List<DATProject> entity = Base.ExecuteToEntityList<DATProject>(strSql, CommandType.Text, paramsList);
            return entity;
        }

        public static DatProjectTotal GetProjectBuildingHouseTotal(int projectId, SearchBase search)
        {
            string strSql = SQL.Project.BuildingAndHouseTotalByProjectId;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            strSql = strSql.Replace("@projecttable", "fxtproject." + city.ProjectTable);
            strSql = strSql.Replace("@buildingtable", "fxtproject." + city.BuildingTable);
            strSql = strSql.Replace("@housetable", "fxtproject." + city.HouseTable);

            List<SqlParameter> paramsList = new List<SqlParameter>();
            paramsList.Add(new SqlParameter("@CityId", search.CityId));
            paramsList.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            paramsList.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            paramsList.Add(new SqlParameter("@projectid", projectId));
            DatProjectTotal entity = ExecuteToEntity<DatProjectTotal>(strSql, CommandType.Text, paramsList);
            return entity;
        }

        /// <summary>
        /// 获取关联楼盘
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="projectId"></param>
        /// <param name="distance">关联距离（米）</param>
        public static List<RelProject> GetRelProject(SearchBase search, int projectId, int distance, decimal x, decimal y)
        {
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            string sql = SQL.Project.RelProjectList;
            sql = sql.Replace("@projecttable", "fxtproject." + city.ProjectTable);
            List<SqlParameter> paramsList = new List<SqlParameter>();
            paramsList.Add(new SqlParameter("@CityId", search.CityId));
            paramsList.Add(new SqlParameter("@AreaId", search.AreaId));
            paramsList.Add(new SqlParameter("@ProjectId", projectId));
            paramsList.Add(new SqlParameter("@Distance", distance));
            paramsList.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            paramsList.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            paramsList.Add(new SqlParameter("@x", x));
            paramsList.Add(new SqlParameter("@y", y));
            List<RelProject> entity = ExecuteToEntityList<RelProject>(sql, CommandType.Text, paramsList);
            return entity;
        }

        /// <summary>
        /// 获取楼盘数量
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static int GetProjectCountByCityId(SearchBase search)
        {
            CityTable city = CityTableDA.GetCityTable(search.CityId);

            string sql = SQL.Project.ProjectCount;

            sql = sql.Replace("@projecttable", "fxtproject." + city.ProjectTable);

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = sql;

            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));

            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));

            int result = (int)ExecuteScalar(cmd);

            return result;
        }

        /// <summary>
        /// 楼盘列表
        /// </summary>
        /// <returns></returns>
        public static List<DATProject> GetProjectList(SearchBase search)
        {
            string sql = SQL.Project.ProjectList;

            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);

            sql = sql.Replace("@table_project", cityTb.ProjectTable);

            List<SqlParameter> sqlParam = new List<SqlParameter>();

            //行政区
            if (search.AreaId > 0)
            {
                sql = sql.Replace("@areawhere", " and areaid = @areaid ");

                sqlParam.Add(SqlHelper.GetSqlParameter("@areaid", search.AreaId, SqlDbType.Int));
            }
            else
            {
                sql = sql.Replace("@areawhere", "");
            }

            string key = "" + search.Key + "%";

            string param = "%" + search.Key + "%";

            sqlParam.Add(SqlHelper.GetSqlParameter("@CityId", search.CityId, SqlDbType.Int));

            sqlParam.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));

            sqlParam.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));

            sqlParam.Add(SqlHelper.GetSqlParameter("@strKey", key, SqlDbType.NVarChar, 81));

            sqlParam.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 82));

            search.OrderBy = " (case when [ProjectName] like @strKey then 0 else 1 end) asc";

            sql = HandleSQL(search, sql);

            var result = ExecuteToEntityList<DATProject>(sql, CommandType.Text, sqlParam);

            return result;
        }
    }
}
