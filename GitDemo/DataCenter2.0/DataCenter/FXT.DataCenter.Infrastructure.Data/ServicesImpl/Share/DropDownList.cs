using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class DropDownList : IDropDownList
    {
        /// <summary>
        /// 根据城市ID获取城市名称列表
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public IList<CompanyProduct_Module> GetCityNameByCityID(int cityId)
        {
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = "select CityId,cityName,ProvinceId from [FXTDataCenter].[dbo].SYS_City with(nolock) where CityId=@cityId";
            SqlParameter parameter = new SqlParameter("@cityId", SqlDbType.Int);
            parameter.Value = cityId;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            DataSet ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }
        /// <summary>
        /// 获取城市名称列表
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public IList<CompanyProduct_Module> GetCityName(int provinceId, int cityId)
        {
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            DataSet ds = null;
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = "select CityId,cityName,ProvinceId from [FXTDataCenter].[dbo].SYS_City with(nolock) where 1=1";
            List<SqlParameter> par = new List<SqlParameter>();
            if (provinceId > 0)
            {
                sql += "  and ProvinceId=@provinceId";
                par.Add(new SqlParameter("@provinceId", provinceId));
            }
            if (cityId > 0)
            {
                sql += "  and CityId=@CityId";
                par.Add(new SqlParameter("@CityId", cityId));
            }
            SqlParameter[] parameter = par.ToArray();
            ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }

        /// <summary>
        /// 获取行政区名称列表
        /// </summary>
        /// <returns></returns>
        public IList<CompanyProduct_Module> GetAreaName(int cityid)
        {
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = "select AreaId, AreaName, CityId, ConstructionCount, GIS_ID, AreaPlacePicName, OldId, X, Y, XYScale from [FXTDataCenter].[dbo].SYS_Area with(nolock)";
            DataSet ds = null;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            if (cityid > 0)
            {

                sql += " where CityId = @CityId";
                SqlParameter parameter = new SqlParameter("@CityId", SqlDbType.Int);
                parameter.Value = cityid;
                ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            }
            else
            {
                ds = DBHelperSql.ExecuteDataSet(sql);
            }
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }
        /// <summary>
        /// 获取片区区名称列表
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public IList<CompanyProduct_Module> GetSubAreaName(int areaId)
        {
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = @"select SubAreaId, SubAreaName, AreaId, ConstructionCount, GIS_ID, RegionPlacePicName, OldId, X, Y, XYScale from [FXTDataCenter].[dbo].SYS_SubArea with(nolock)";
            DataSet ds = null;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            //if (areaId >= 0)
            //{

            sql += " where AreaId = @AreaId";
            SqlParameter parameter = new SqlParameter("@AreaId", SqlDbType.Int);
            parameter.Value = areaId;
            ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            //}
            //else
            //{
            //    ds = DBHelperSql.ExecuteDataSet(sql);
            //}
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }
        /// <summary>
        /// 根据省ID获得省名称
        /// </summary>
        /// <param name="ProvinceId">省ID</param>
        /// <returns></returns>
        public IList<CompanyProduct_Module> GetProNameByProId(int ProvinceId)
        {
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = "select ProvinceId,ProvinceName,Alias from FXTProject.dbo.SYS_Province with(nolock) where ProvinceId=@ProvinceId ";
            SqlParameter parameter = new SqlParameter("@ProvinceId", SqlDbType.Int);
            parameter.Value = ProvinceId;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
            DataSet ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }

        /// <summary>
        /// 获取土地用途
        /// </summary>
        /// <param name="code">土地用途code</param>
        /// <returns></returns>
        public IList<CompanyProduct_Module> GetLandPurpose(int code = 1001)
        {
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = "select ID, Code, CodeName, CodeType, Remark, SubCode from [FXTDataCenter].[dbo].sys_Code with(nolock) where ID=@code";
            SqlParameter parameter = new SqlParameter("@code", SqlDbType.Int);
            parameter.Value = code;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            DataSet ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }
        /// <summary>
        /// 根据城市Id,产品Code获取公司名称
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="companyTypeCode">产品Code</param>
        /// <returns></returns>
        public IList<CompanyProduct_Module> GetCompanyName(int cityId, int companyTypeCode)
        {
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = "select CompanyId, ChineseName, EnglishName, CompanyTypeCode, CityId, Address, Telephone, Fax, Website, CreateDate, Valid from [FXTDataCenter].[dbo].DAT_Company with(nolock) where Valid=1";
            List<SqlParameter> paramenter = new List<SqlParameter>();
            if (cityId > 0)
            {
                sql += " and CityId=@CityId";
                paramenter.Add(new SqlParameter("@CityId", cityId));
            }
            if (companyTypeCode > 0)
            {
                sql += " and CompanyTypeCode=@CompanyTypeCode";
                paramenter.Add(new SqlParameter("@CompanyTypeCode", companyTypeCode));
            }
            SqlParameter[] pa = paramenter.ToArray();
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            DataSet ds = DBHelperSql.ExecuteDataSet(sql, pa);
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }
        /// <summary>
        /// 根据城市Id,产品Code获取公司名称
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="companyTypeCode">产品Code</param>
        /// <returns></returns>
        public string GetCompanyName(int cityId, int companyTypeCode, string split)
        {
            string info = "";
            string sql = "select CompanyId, ChineseName, EnglishName, CompanyTypeCode, CityId, Address, Telephone, Fax, Website, CreateDate, Valid from [FXTDataCenter].[dbo].DAT_Company with(nolock) where Valid=1 and CityId=@CityId ";
            SqlParameter[] par = { 
                                 new SqlParameter("@CityId",cityId),
                                 new SqlParameter("@CompanyTypeCode",companyTypeCode)
                                 };
            //DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter_Role;
            //DataSet ds = DBHelperSql.ExecuteDataSet(sql, par);
            //var reslut = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);

            IQueryable<CompanyProduct_Module> result;
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                result = conn.Query<CompanyProduct_Module>(sql, new { CityId = cityId, CompanyTypeCode = companyTypeCode }).AsQueryable();
            }

            if (result.Any())
            {
                foreach (var item in result)
                {
                    info += item.ChineseName + split;
                }
            }
            if (!string.IsNullOrEmpty(info))
            {
                info = info.Substring(0, info.Length - 1);
            }
            return info;
        }

        public IList<CompanyProduct_Module> GetDictById(int id)
        {
            IList<CompanyProduct_Module> list = new List<CompanyProduct_Module>();
            string sql = "select ID, Code, CodeName, CodeType, Remark, SubCode from [FXTDataCenter].[dbo].sys_Code with(nolock) where ID=@id order by remark asc";
            SqlParameter parameter = new SqlParameter("@id", SqlDbType.Int);
            parameter.Value = id;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            DataSet ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            list = ModelConvertHelper<CompanyProduct_Module>.ConvertToModel(ds.Tables[0]);
            return list;
        }

        public IList<SYS_Code> GetDictBySubCode(int code)
        {
            IList<SYS_Code> list = new List<SYS_Code>();
            string sql = "select ID, Code, CodeName, CodeType, Remark, SubCode from [FXTDataCenter].[dbo].sys_Code with(nolock) where id=(select subcode from [FXTDataCenter].[dbo].sys_Code with(nolock) where code = @subCode )";
            SqlParameter parameter = new SqlParameter("@subCode", SqlDbType.Int);
            parameter.Value = code;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            DataSet ds = DBHelperSql.ExecuteDataSet(sql, parameter);
            list = ModelConvertHelper<SYS_Code>.ConvertToModel(ds.Tables[0]);
            return list;
        }

        public int GetCodeByName(string name,params int[] typeId)
        {
            var ids = string.Join(",",typeId);
            var sql = "select code from FxtDataCenter.dbo.sys_Code with(nolock) where codename=@codename and id in (" + ids + ")";
            SqlParameter[] parameter =
            {
                new SqlParameter("@codename", SqlDbType.NVarChar)
            };
            parameter[0].Value = name;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            DataSet ds = DBHelperSql.ExecuteDataSet(sql, parameter);

            if (ds.Tables[0].Rows.Count == 0) return -1;

            string code = ds.Tables[0].Rows[0][0].ToString();
            if (ds.Tables[0].Rows.Count == 0 || code == "2000002" || code == "2000001")
            {
                return -1;
            }

            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 根据行政区名称获取行政区Id
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetAreaIdByName(int cityId, string name)
        {
            var strSql = "select AreaId from [FXTDataCenter].[dbo].SYS_Area with(nolock) where AreaName = @AreaName and CityId=@cityId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<int>(strSql, new { AreaName = name, cityId }).FirstOrDefault();
                return query == 0 ? -1 : query;
            }
        }
        /// <summary>
        /// 根据行政区Id获取行政区名称
        /// 刘晓博
        /// 2014-09-20
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAreaNameByAreaId(int areaId)
        {
            string strSql = "select AreaName from [FXTDataCenter].[dbo].SYS_Area with(nolock) where AreaId = @AreaId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<string>(strSql, new { AreaId = areaId });
                return query == null ? "" : query.FirstOrDefault();
            }
        }

        public int GetSubAreaIdByName(string name,int areaId)
        {
            string strSql = "select SubAreaId from [FXTDataCenter].[dbo].SYS_SubArea with(nolock) where SubAreaName = @SubAreaName and areaId = @areaId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<int>(strSql, new { SubAreaName = name, areaId });
                return query == null ? -1 : query.FirstOrDefault();
            }
        }
        /// <summary>
        /// 根据片区Id获取片区名称
        /// </summary>
        /// <param name="subAreaId"></param>
        /// <returns></returns>
        public string GetSubAreaNameBySubAreaId(int subAreaId)
        {
            string strSql = "select SubAreaName from [FXTDataCenter].[dbo].SYS_SubArea with(nolock) where SubAreaId = @SubAreaId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<string>(strSql, new { SubAreaId = subAreaId });
                return query == null ? "" : query.FirstOrDefault();
            }
        }

        public IQueryable<SYS_Province> GetProvince()
        {
            string strSql = "select * from sys_province";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<SYS_Province>(strSql).AsQueryable();
            }
        }

        public IQueryable<SYS_City> GetCityByProId(int proId)
        {
            string strSql = "select * from [FXTDataCenter].[dbo].sys_city where ProvinceId=@proId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_City>(strSql, new { proId }).AsQueryable();
            }
        }


        public IQueryable<SYS_City> GetCityByCityId(int cityId)
        {
            string strSql = "select * from [FXTDataCenter].[dbo].sys_city where cityid = @cityId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_City>(strSql, new { cityId }).AsQueryable();
            }
        }


        public string GetNameByCode(int code)
        {
            string sql = "select codeName from FxtDataCenter.dbo.sys_Code with(nolock) where code=@code";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<string>(sql, new { code }).AsQueryable();
                return query.FirstOrDefault();
            }
        }


        public int GetCityIdByName(string name)
        {
            string sql = "select CityId from FxtDataCenter.dbo.SYS_City with(nolock) where CityName like @cityName";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<int>(sql, new { cityName = "%" + name + "%" }).AsEnumerable();
                return query == null ? -1 : query.FirstOrDefault();
            }
        }

        public int GetAreaLineIdByName(string name)
        {
            var sql = @"select AreaLineId from FxtDataCenter.dbo.SYS_AreaLine with(nolock) where AreaLineName = @AreaLineName";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<int>(sql, new { AreaLineName = name }).AsEnumerable();
                return query == null ? -1 : query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据商圈名称获取商圈ID
        /// 刘晓博
        /// 2014-09-12
        /// </summary>
        /// <param name="subAreaName"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public int GetBizSubAreaIdByName(string subAreaName,int areaId)
        {
            string strSql = "select SubAreaId from [FXTDataCenter].[dbo].SYS_SubArea_Biz with(nolock) where SubAreaName = @SubAreaName and areaId = @areaId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<int>(strSql, new { SubAreaName = subAreaName,areaId }).AsQueryable();
                return query.FirstOrDefault();
            }
        }
        /// <summary>
        /// 根据商圈ID获取商圈名称
        /// 刘晓博
        /// 2014-09-19
        /// </summary>
        /// <param name="subAreaName"></param>
        /// <returns></returns>
        public string GetBizSubAreaNameBySubAreaId(int subAreaId)
        {
            string strSql = "select SubAreaName from [FXTDataCenter].[dbo].SYS_SubArea_Biz with(nolock) where SubAreaId = @SubAreaId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<string>(strSql, new { SubAreaId = subAreaId }).AsQueryable();
                return query.FirstOrDefault();
            }
        }

        public IQueryable<Dat_Building_Biz> GetBusinessBuilding(long projectId)
        {
            var strSql = "select BuildingId,BuildingName from FxtData_Biz.dbo.Dat_Building_Biz where ProjectId = @projectId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<Dat_Building_Biz>(strSql, new { projectId }).AsQueryable();
            }
        }


        public IQueryable<SYS_Area> GetAreaIds(int cityId)
        {
            var strSql = "select AreaId,AreaName from [FXTDataCenter].[dbo].SYS_Area with(nolock) where CityId=@cityId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Area>(strSql, new {cityId}).AsQueryable();
            }
        }


        public IQueryable<SYS_Code> GetCodes()
        {
            var sql = "select Code,CodeName,id from FxtDataCenter.dbo.sys_Code with(nolock)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Code>(sql).AsQueryable();
            }
        }
    }
}
