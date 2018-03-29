using System;
using System.Data;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using System.Linq;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class BusinessCase : IBusinessCase
    {

        public IQueryable<Dat_Case_Biz> GetCaseBizs(Dat_Case_Biz caseBiz, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(caseBiz.CityId, caseBiz.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = caseBiz.FxtCompanyId.ToString();

            if (self) comId = caseBiz.FxtCompanyId.ToString();

            var strSql = @"
SELECT cb.*
	,a.AreaName
	,sa.SubAreaName
	,c.CodeName AS CaseTypeName
	,c1.CodeName AS RentTypeName
	,c2.CodeName AS FitmentName
	,CASE WHEN cb.HouseType = 1119001 THEN '住宅底商'
		WHEN cb.HouseType = 1110001 THEN '商业街商铺'
		WHEN cb.HouseType = 1107002 THEN '临街门面'
		WHEN cb.HouseType = 1119002 THEN '写字楼配套'
		WHEN cb.HouseType = 1118004 THEN '购物中心/百货'
		WHEN cb.HouseType = 1118006 THEN '宾馆酒店'
		WHEN cb.HouseType = 1124001 THEN '旅游点商铺'
		WHEN cb.HouseType = 1118002 THEN '主题卖场'
		WHEN cb.HouseType = 1118011 THEN '其他'
		END AS HouseTypeName
	,REPLACE(REPLACE(REPLACE((
					SELECT CodeName + ','
					FROM FxtDataCenter.dbo.SYS_Code c4
					WHERE ('|' + REPLACE(cb.BizCode, ',', '|') + '|') LIKE '%|' + CONVERT(NVARCHAR(128), c4.Code) + '|%'
					ORDER BY CASE 
							WHEN c4.CodeName = '其他'
								THEN 1
							ELSE 0
							END
					FOR XML PATH('')
					) + ',', ',,', ''), '专业服务-', ''), '体验式服务-', '') AS BizCodeName
FROM FxtData_Biz.dbo.Dat_Case_Biz cb WITH (NOLOCK)
LEFT JOIN fxtdatacenter.dbo.SYS_Area a WITH (NOLOCK) ON cb.AreaId = a.AreaId
LEFT JOIN fxtdatacenter.dbo.SYS_SubArea_Biz sa WITH (NOLOCK) ON cb.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON c.Code = cb.CaseTypeCode
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON c1.Code = cb.RentTypeCode
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON c2.Code = cb.Fitment
WHERE cb.valid = 1
	AND cb.CityId = @CityId
	AND cb.FxtCompanyId IN (" + comId + ")";

            if (!new[] { 0, -1 }.Contains(caseBiz.AreaId)) strSql += " and cb.AreaId = @AreaId";
            if (!new[] { 0, -1 }.Contains(caseBiz.SubAreaId)) strSql += " and cb.SubAreaId = @SubAreaId";
            if (!string.IsNullOrEmpty(caseBiz.ProjectName)) strSql += " and cb.ProjectName like @ProjectName";
            if (caseBiz.CaseDateStart != default(DateTime)) strSql += " and cb.CaseDate >= @CaseDateStart";
            if (caseBiz.CaseDateEnd != default(DateTime)) strSql += " and cb.CaseDate-1 < @CaseDateEnd";
            if (!new[] { 0, -1 }.Contains(caseBiz.CaseTypeCode)) strSql += " and cb.CaseTypeCode = @CaseTypeCode";
            if (caseBiz.BuildingAreaFrom != default(decimal)) strSql += " and cb.BuildingArea >=@BuildingAreaFrom";
            if (caseBiz.BuildingAreaTo != default(decimal)) strSql += " and cb.BuildingArea <= @BuildingAreaTo";
            if (!new[] { 0, -1 }.Contains(caseBiz.RentTypeCode ?? -1)) strSql += " and cb.RentTypeCode = @RentTypeCode";
            if (caseBiz.UnitPriceFrom != default(decimal)) strSql += " and cb.UnitPrice >=@UnitPriceFrom";
            if (caseBiz.UnitPriceTo != default(decimal)) strSql += " and cb.UnitPrice <= @UnitPriceTo";

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.id desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";
            
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                totalCount = conn.Query<int>(totalCountSql, new { CityId = caseBiz.CityId, AreaId = caseBiz.AreaId, SubAreaId = caseBiz.SubAreaId, ProjectName = "%" + caseBiz.ProjectName + "%", CaseDateStart = caseBiz.CaseDateStart, CaseDateEnd = caseBiz.CaseDateEnd, CaseTypeCode = caseBiz.CaseTypeCode, BuildingAreaFrom = caseBiz.BuildingAreaFrom, BuildingAreaTo = caseBiz.BuildingAreaTo, RentTypeCode = caseBiz.RentTypeCode, UnitPriceFrom = caseBiz.UnitPriceFrom, UnitPriceTo = caseBiz.UnitPriceTo }).FirstOrDefault();
                return conn.Query<Dat_Case_Biz>(pagenatedSql, new { CityId = caseBiz.CityId, AreaId = caseBiz.AreaId, SubAreaId = caseBiz.SubAreaId, ProjectName = "%" + caseBiz.ProjectName + "%", CaseDateStart = caseBiz.CaseDateStart, CaseDateEnd = caseBiz.CaseDateEnd, CaseTypeCode = caseBiz.CaseTypeCode, BuildingAreaFrom = caseBiz.BuildingAreaFrom, BuildingAreaTo = caseBiz.BuildingAreaTo, RentTypeCode = caseBiz.RentTypeCode, UnitPriceFrom = caseBiz.UnitPriceFrom, UnitPriceTo = caseBiz.UnitPriceTo }).AsQueryable();
            }
        }

        public Dat_Case_Biz GetCaseBizById(int id)
        {
            var strSql = @"
SELECT cb.*
	,a.AreaName
	,sa.SubAreaName
	,c.CodeName AS CaseTypeName
	,c1.CodeName AS RentTypeName
FROM FxtData_Biz.dbo.Dat_Case_Biz cb WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON cb.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sa WITH (NOLOCK) ON cb.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON c.Code = cb.CaseTypeCode
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON c1.Code = cb.RentTypeCode
WHERE cb.id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<Dat_Case_Biz>(strSql, new { id }).FirstOrDefault();
            }
        }

        public int AddCaseBiz(Dat_Case_Biz caseBiz)
        {
            var strSql = @"insert into FxtData_Biz.dbo.Dat_Case_Biz (cityid,areaid,subareaid,address,projectid,buildingid,houseid,projectname,buildingname,housename,BuildingArea,UnitPrice,totalprice,casetypecode,renttypecode,casedate,rentrate,sourcename,sourcelink,sourcephone,fxtcompanyid,creator,HouseType,BizCode,FloorNo,TotalFloor,Fitment,ManagerPrice,AgencyCompany,Agent,AgencyTel) 
values(@cityid,@areaid,@subareaid,@address,@projectid,@buildingid,@houseid,@projectname,@buildingname,@housename,@BuildingArea,@UnitPrice,@totalprice,@casetypecode,@renttypecode,@casedate,@rentrate,@sourcename,@sourcelink,@sourcephone,@fxtcompanyid,@creator,@HouseType,@BizCode,@FloorNo,@TotalFloor,@Fitment,@ManagerPrice,@AgencyCompany,@Agent,@AgencyTel)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, caseBiz);
            }

        }

        public int UpdateCaseBiz(Dat_Case_Biz caseBiz)
        {
            var strSql = @"update FxtData_Biz.dbo.Dat_Case_Biz with(rowlock)  set areaid = @areaid, subareaid = @subareaid, address = @address, projectid = @projectid,buildingid = @buildingid,houseid = @houseid,projectname = @projectname,buildingname = @buildingname,housename = @housename,BuildingArea = @BuildingArea,UnitPrice = @UnitPrice,totalprice = @totalprice,casetypecode = @casetypecode,renttypecode = @renttypecode,casedate = @casedate,rentrate = @rentrate,sourcename = @sourcename,sourcelink = @sourcelink,sourcephone = @sourcephone,savedatetime = @savedatetime,saveuser = @saveuser,HouseType = @HouseType,BizCode = @BizCode,FloorNo = @FloorNo,TotalFloor = @TotalFloor,Fitment = @Fitment,ManagerPrice = @ManagerPrice,AgencyCompany = @AgencyCompany,Agent = @Agent,AgencyTel = @AgencyTel
where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, caseBiz);
            }
        }

        public int DeleteCaseBiz(int id)
        {
            var strSql = @"update FxtData_Biz.dbo.Dat_Case_Biz with(rowlock)  set valid = 0 where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { id });
            }
        }

        public int DeleteSameProjectCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo)
        {

            var dateFrom = caseDateFrom.ToString("yyyy-MM-dd") + " 00:00:00";
            var dateTo = caseDateTo.ToString("yyyy-MM-dd") + " 23:59:59";

            var strSql = @"UPDATE FxtData_Biz.dbo.Dat_Case_Biz
                                SET Valid = 0
                                WHERE CityId = " + cityId + @" AND FXTCompanyId = " + fxtCompanyId + @" AND CaseDate BETWEEN '" + dateFrom + @"' AND '" + dateTo + @"'
	                                AND Valid = 1
	                                AND id NOT IN (
		                                SELECT max(id) id
		                                FROM FxtData_Biz.dbo.Dat_Case_Biz
		                                WHERE CityId = " + cityId + @" AND FXTCompanyId = " + fxtCompanyId + @"  AND CaseDate BETWEEN '" + dateFrom + @"' AND '" + dateTo + @"' AND Valid = 1
		                                GROUP BY AreaId,ProjectName,BuildingArea,UnitPrice,CaseTypeCode
		                                )";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Execute(strSql, commandTimeout: 300);
            }

        }

        #region 公共

        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[CaseTable],s.BizCaseCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),FxtDataCenter.dbo.[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var query = conn.Query<AccessTable>(sql, new { cityid, fxtcompanyid }).FirstOrDefault();
                ptable = query == null ? "" : query.ProjectTable;
                ctable = query == null ? "" : query.CaseTable;
                btable = query == null ? "" : query.BuildingTable;
                comId = query == null ? "" : query.BizCaseCompanyId;
            }

        }

        private class AccessTable
        {
            public string ProjectTable { get; set; }
            public string CaseTable { get; set; }
            public string BuildingTable { get; set; }
            public string BizCaseCompanyId { get; set; }
        }

        #endregion
    }
}
