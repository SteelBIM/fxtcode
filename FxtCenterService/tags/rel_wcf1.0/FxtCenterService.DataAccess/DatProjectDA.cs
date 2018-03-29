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
                model.SetPrimaryKeyIsIdentify(true);
            }
            return InsertFromEntity<DATProject>(model);
        }

        public static int Update(DATProject model)
        {
            string tableName = Base.GetEntityTable<DATProject>();
            if (tableName.IndexOf("_sub") > 0)
            {
                model.SetIgnoreFields(new string[] { "fxtcompanyid", "createtime" });
                model.SetPrimaryKeyIsIdentify(false);
            }
            else
            {
                model.SetIgnoreFields(new string[] { "fxt_companyid", "createtime" });
                model.SetPrimaryKeyIsIdentify(true);
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
        public static List<DATProject> GetDATProjectList(SearchBase search, string key, int areaid, int buildingtypecode, int purposecode)
        {
            string sql = SQL.Project.DATProjectList;
            List<SqlParameter> parameters = new List<SqlParameter>();
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<DATProject>();
            sql = sql.Replace("@table_dat_project", city.ProjectTable);
            sql = sql.Replace("@fxtcompanyid", search.FxtCompanyId.ToString());
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
        public static DATProject GetProjectInfoByName(int cityId, int areaId, int fxtCompanyId, string projectname)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectInfoGetByName;
            CityTable city = CityTableDA.GetCityTable(cityId);
            if (null == city)
                return null;
            sql = sql.Replace("@dat_project", city.ProjectTable);
            sql = sql.Replace("@cityid", cityId.ToString());
            sql = sql.Replace("@fxtcompanyid", fxtCompanyId.ToString());
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
        /// 获得数据中心的楼盘信息
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoById(int cityid,int projectid,int fxtcompanyid) 
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
            return ExecuteToEntity<DATProject>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获得数据中心的楼盘图片，没有联合附表
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static List<LNKPPhoto> GetProjectPhotoById(int cityid, int projectid, int fxtcompanyid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.ProjectPhoto;
            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int)); ;

            return ExecuteToEntityList<LNKPPhoto>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取楼盘下拉列表  
        /// </summary>
        public static DataTable GetProjectDropDownList(SearchBase search, string condition, string strkey, string param)
        {
            string sql = SQL.Project.ProjectDropDownList;
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
            sql = sql.Replace("@where", condition);
            sql = sql.Replace("@top", topSql);
            SqlCommand cmd = new SqlCommand();
            sql = HandleSQL(search, sql); ;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CityId", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@strKey", strkey, SqlDbType.NVarChar, 81));
            if (!string.IsNullOrEmpty(param))
            {
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@param", param, SqlDbType.NVarChar, 82));
            }
            cmd.CommandText = sql;
            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 楼盘案例
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DataTable GetProjectCase(SearchBase search,int projectid,int fxtcompanyid,int cityid)
        {
            string sql = SQL.Project.ProjectCase;
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new DataTable();
            string projectTable = cityTb.ProjectTable;
            string caseTable = cityTb.casetable;
            SqlCommand cmd = new SqlCommand();

            sql = sql.Replace("@table_project",projectTable);
            sql = sql.Replace("@table_case", caseTable);
            sql = HandleSQL(search, sql);
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid",projectid,SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", search.DateBegin, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", search.DateEnd, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
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
        public static List<DATProject> GetSearchProjectListByKey(SearchBase search,int fxtcompanyid,int cityid ,string key) 
        {
            string sql = SQL.AutoPrice.ProjectList;
            List<SqlParameter> param = new List<SqlParameter>();
            CityTable cityTb = CityTableDA.GetCityTable(search.CityId);
            if (null == cityTb)
                return new List<DATProject>();
            sql = sql.Replace("@table_project",cityTb.ProjectTable);
            search.OrderBy = " isevalue desc";
            sql = HandleSQL(search,sql);
            string address = string.Format("%{0}%", key);
            param.Add(SqlHelper.GetSqlParameter("@cityid",cityid,SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
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
                sqlhead = "declare @casemonth int select @casemonth=casemonth from sys_city with(nolock) where cityid = @cityid \n";
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
            param.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            return ExecuteToEntity<DATProject>(sql, CommandType.Text, param);
        }
    }
}
