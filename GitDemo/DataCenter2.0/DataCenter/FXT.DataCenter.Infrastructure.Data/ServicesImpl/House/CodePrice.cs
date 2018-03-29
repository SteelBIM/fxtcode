using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using FXT.DataCenter.Domain.Services;
using System.Data.SqlClient;


namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class CodePrice : ICodePrice
    {
        public IQueryable<sys_CodePrice> FindAll(int cityId, int fxtCompanyId)
        {
            string str_sql = @"
SELECT ID, CityID, Code, CodeName, Price, PurposeCode, SubCode, TypeCode, fxtcompanyid FROM FXTProject.dbo.sys_CodePrice
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var result = con.Query<sys_CodePrice>(str_sql, new { CityID = cityId, FxtCompanyId = fxtCompanyId }).AsQueryable();
                return result;
            }
        }

        public sys_CodePrice FindByCodeName(string codeName, int cityId, int fxtCompanyId)
        {
            string str_sql = @"
SELECT ID, CityID, Code, CodeName, Price, PurposeCode, SubCode, TypeCode, fxtcompanyid FROM FXTProject.dbo.sys_CodePrice
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId and codeName = @codeName";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var houserList = con.Query<sys_CodePrice>(str_sql, new { CityID = cityId, FxtCompanyId = fxtCompanyId, codeName = codeName }).FirstOrDefault();
                return houserList;
            }
        }

        public IQueryable<sys_CodePrice> FindAllByTypeCode(int typeCode, int cityId, int fxtCompanyId)
        {
            string str_sql = @"
SELECT ID
	,CityID
	,Code
	,CodeName
	,CONVERT(numeric(18,2),Price) as Price
	,PurposeCode
	,SubCode
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where cp.SubCode = c.Code) as SubCodeName
	,TypeCode
	,fxtcompanyid
FROM FXTProject.dbo.sys_CodePrice cp
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId and typeCode = @typeCode";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var houserList = con.Query<sys_CodePrice>(str_sql, new { CityID = cityId, FxtCompanyId = fxtCompanyId, typeCode = typeCode }).AsQueryable();
                return houserList;
            }
        }

        public DataTable ExportFrontCode(int typeCode, int cityId, int fxtCompanyId)
        {
            List<SqlParameter> paramet = new List<SqlParameter>();
            string str_sql = @"
SELECT CodeName as [朝向]
	,CONVERT(numeric(18,2),Price) as [修正系数_百分比]
FROM FXTProject.dbo.sys_CodePrice cp
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId and typeCode = @typeCode";
            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                paramet.Add(new SqlParameter("@CityID", cityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", fxtCompanyId));
                paramet.Add(new SqlParameter("@typeCode", typeCode));

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var houserList = DBHelperSql.ExecuteDataTable(str_sql, param);
                return houserList;
            }
        }

        public DataTable ExportSightCode(int typeCode, int cityId, int fxtCompanyId)
        {
            List<SqlParameter> paramet = new List<SqlParameter>();
            string str_sql = @"
SELECT CodeName as [景观]
	,CONVERT(numeric(18,2),Price) as [修正系数_百分比]
FROM FXTProject.dbo.sys_CodePrice cp
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId and typeCode = @typeCode";
            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                paramet.Add(new SqlParameter("@CityID", cityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", fxtCompanyId));
                paramet.Add(new SqlParameter("@typeCode", typeCode));

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var houserList = DBHelperSql.ExecuteDataTable(str_sql, param);
                return houserList;
            }
        }

        public DataTable ExportVDCode(int typeCode, int cityId, int fxtCompanyId)
        {
            List<SqlParameter> paramet = new List<SqlParameter>();
            string str_sql = @"
SELECT CodeName as [通风采光]
	,CONVERT(numeric(18,2),Price) as [修正系数_百分比]
FROM FXTProject.dbo.sys_CodePrice cp
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId and typeCode = @typeCode";
            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                paramet.Add(new SqlParameter("@CityID", cityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", fxtCompanyId));
                paramet.Add(new SqlParameter("@typeCode", typeCode));

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var houserList = DBHelperSql.ExecuteDataTable(str_sql, param);
                return houserList;
            }
        }

        public DataTable ExportFitmentCode(int typeCode, int cityId, int fxtCompanyId)
        {
            List<SqlParameter> paramet = new List<SqlParameter>();
            string str_sql = @"
SELECT CodeName as [装修]
	,CONVERT(numeric(18,2),Price) as [修正系数_百分比]
FROM FXTProject.dbo.sys_CodePrice cp
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId and typeCode = @typeCode";
            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                paramet.Add(new SqlParameter("@CityID", cityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", fxtCompanyId));
                paramet.Add(new SqlParameter("@typeCode", typeCode));

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var houserList = DBHelperSql.ExecuteDataTable(str_sql, param);
                return houserList;
            }
        }

        public DataTable ExportBuildingAreaCode(int typeCode, int cityId, int fxtCompanyId)
        {
            List<SqlParameter> paramet = new List<SqlParameter>();
            string str_sql = @"
SELECT CodeName as [建筑类型]
    ,(select codename from fxtdatacenter.dbo.sys_code c with(nolock) where c.code = cp.subcode) as [面积段]
	,CONVERT(numeric(18,2),Price) as [修正系数_百分比]
FROM FXTProject.dbo.sys_CodePrice cp
WHERE CityID = @CityID and fxtcompanyid = @FxtCompanyId and typeCode = @typeCode";
            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                paramet.Add(new SqlParameter("@CityID", cityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", fxtCompanyId));
                paramet.Add(new SqlParameter("@typeCode", typeCode));

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var houserList = DBHelperSql.ExecuteDataTable(str_sql, param);
                return houserList;
            }
        }

        public int UpdateCodePrice(int codePriceId, string price)
        {
            string str_sql = @"UPDATE FXTProject.dbo.sys_CodePrice SET Price = @Price WHERE id = @id";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Execute(str_sql, new { Price = price, id = codePriceId });
            }
        }

        public int AddCodePrice(sys_CodePrice model)
        {
            string str_sql = @"
insert into FXTProject.dbo.sys_CodePrice(CityID,Code,CodeName,Price,PurposeCode,SubCode,TypeCode,fxtcompanyid)
values(@CityID,@Code,@CodeName,@Price,@PurposeCode,@SubCode,@TypeCode,@fxtcompanyid)";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Execute(str_sql, new
                {
                    CityID = model.cityid,
                    Code = model.code,
                    CodeName = model.codename,
                    Price = model.price,
                    PurposeCode = model.purposecode,
                    SubCode = model.subcode,
                    TypeCode = model.typecode,
                    fxtcompanyid = model.fxtcompanyid
                });
            }
        }

        public int DeleteCodePrice(int cityid, int fxtcompanyid, int TypeCode)
        {
            string str_sql = @"
delete from FXTProject.dbo.sys_CodePrice
where CityID = @CityID
and fxtcompanyid = @fxtcompanyid
and TypeCode = @TypeCode";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Execute(str_sql, new { CityID = cityid, fxtcompanyid = fxtcompanyid, TypeCode = TypeCode });
            }
        }

    }
}
