using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Domain.Models.QueryObjects.House;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using Dapper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ProjectCase : IProjectCase
    {

        #region 查询

        public IQueryable<DAT_Case> GetProjectCase(ProjectCaseParams pcp)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(pcp.cityid, pcp.fxtcompanyid, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = pcp.fxtcompanyid.ToString();
            if (string.IsNullOrEmpty(casecomId)) casecomId = pcp.fxtcompanyid.ToString();
            if (ptable == "" || ctable == "")
            {
                return new List<DAT_Case>().AsQueryable();
            }
            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;
            //按条件筛选拼接
            string dhhyzd = pcp.fxtcompanyid != 178 ? "" : @",d.PlanPurpose as PlanPurpose_dhhy,d.WallType as WallType_dhhy,d.Window as Window_dhhy,d.UnitDoor as UnitDoor_dhhy,d.UnitWall as UnitWall_dhhy,d.Banister as Banister_dhhy,d.UnitTread as UnitTread_dhhy,d.Doors as Doors_dhhy,d.HeatingType as HeatingType_dhhy,d.HouseTypeDetail as HouseTypeDetail_dhhy,d.ZhuangXiuType as ZhuangXiuType_dhhy,d.JuJia as JuJia_dhhy,d.LandDetail as LandDetail_dhhy,d.LandArea as LandArea_dhhy,d.LandRight as LandRight_dhhy,d.LandOver as LandOver_dhhy,d.Creator as Creator_dhhy,d.CreateDate as CreateDate_dhhy,d.SiZhi as SiZhi_dhhy,d.BuildingRemark as BuildingRemark_dhhy,d.OutWall as OutWall_dhhy";
            string dhhysql = pcp.fxtcompanyid != 178 ? "" : @"inner join FXTProject.dbo.Dat_Case_hbh_dhhy d with(nolock) on c.CaseID = d.CaseId";

            string sql = @"
select 
	P.ProjectName
    ,c.CaseID,c.ProjectId,BuildingId,HouseId,c.CompanyId,CaseDate,c.PurposeCode,FloorNumber,BuildingName,HouseNo,c.BuildingArea,UsableArea,FrontCode,UnitPrice,MoneyUnitCode,SightCode,CaseTypeCode,StructureCode,c.BuildingTypeCode,HouseTypeCode,c.CreateDate,
    c8.TrueName Creator,c.Remark,TotalPrice,c.OldID,c.CityID,c.Valid,c.FXTCompanyId,TotalFloor,RemainYear,Depreciation,FitmentCode,SurveyId,c.SaveDateTime,c.SaveUser,SourceName,SourceLink,SourcePhone,c.BuildingDate,ZhuangXiu,SubHouse,PeiTao
    ,P.AreaID
    ,a.AreaName
	,c1.CodeName as CaseTypeName
	,c2.CodeName as PurposeName
	,c3.CodeName as MoneyUnitCodeName
	,c4.CodeName as FrontName
	,c5.CodeName as StructureName
	,c6.CodeName as BuildingTypeName
	,c7.CodeName as HouseTypeName
	,P.Creator as projectCreator
    ,P.FxtCompanyId as projectFxtCompanyId" + dhhyzd + @"
from (
	select 
    ProjectId,ProjectName,AreaID,Creator,FxtCompanyId
	from " + ptable + @" p with(nolock)
	where not exists(
		select ProjectId from " + ptable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = @cityid
		and ps.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + showcomId + @")
	union
	select 
    ProjectId,ProjectName,AreaID,Creator,Fxt_CompanyId
	from " + ptable + @"_sub ps with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
)P
inner join " + ctable + " c with(nolock) on P.ProjectId = c.ProjectId " + dhhysql + @"
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on c.CaseTypeCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on c.PurposeCode = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on c.MoneyUnitCode = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on c.FrontCode = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on c.StructureCode = c5.Code
left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) on c.BuildingTypeCode = c6.Code
left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) on c.HouseTypeCode = c7.Code
left join FxtUserCenter.dbo.UserInfo c8 with(nolock) on c.Creator = c8.UserName
where 1 = 1
and c.valid = 1
and c.CityID = @cityid
and c.FXTCompanyId in (" + casecomId + @")
and c.CaseDate between @casedateStart and @casedateEnd";

            var strWhere0 = string.Empty;
            if (pcp.areaid != -1 && pcp.areaid != 0)
                strWhere0 += " and p.AreaID = @areaid";
            if (!string.IsNullOrWhiteSpace(pcp.key))
                strWhere0 += " and p.ProjectName like '%'+@key+'%' ";
            if (pcp.purposeCode != -1)
                strWhere0 += " and c.PurposeCode = @purposeCode";
            if (pcp.caseTypeCode != -1)
                strWhere0 += " and c.CaseTypeCode =@caseTypeCode";
            if (pcp.buildingAreaFrom != null)
                strWhere0 += " and c.BuildingArea >= @buildingAreaFrom ";
            if (pcp.buildingAreaTo != null)
                strWhere0 += " and c.BuildingArea <=@buildingAreaTo  ";
            if (pcp.unitPriceFrom != null)
                strWhere0 += " and c.UnitPrice >= @unitPriceFrom  ";
            if (pcp.unitPriceTo != null)
                strWhere0 += " and c.UnitPrice <=  @unitPriceTo ";
            if (pcp.savedatetime != null)
                strWhere0 += " and c.savedatetime >=@savedatetime and c.savedatetime-1 < @savedatetime ";
            if (pcp.buildingTypeCode != -1)
                strWhere0 += " and c.BuildingTypeCode = @buildingTypeCode";

            sql += strWhere0;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Case>(sql, pcp).AsQueryable();
            }
        }

        public IQueryable<DAT_Project> GetProjectList(int fxtCompanyId, int cityId)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtCompanyId.ToString();
            if (string.IsNullOrEmpty(casecomId)) casecomId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_Project>().AsQueryable();
            }
            ptable = "FXTProject." + ptable;
            string sql = @"
SELECT P.ProjectId
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = p.AreaID) as AreaName
	,p.ProjectName
	,P.OtherName
	,P.PinYin
	,P.AreaId
	,P.SubAreaId
	,P.IsComplete
	,P.IsEValue
	,P.Creator
    ,P.FxtCompanyId
FROM " + ptable + @" P WITH (NOLOCK)
WHERE P.CityId = @cityId
	AND p.valid = 1
	AND p.FxtCompanyId IN (" + showcomId + @")
	AND NOT EXISTS (
		SELECT ProjectId
		FROM " + ptable + @"_sub ps WITH (NOLOCK)
		WHERE p.ProjectId = ps.ProjectId
			AND ps.Fxt_CompanyId = @fxtCompanyId
			AND ps.CityId = @cityId
		)
UNION
SELECT P.ProjectId
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = p.AreaID) as AreaName
	,p.ProjectName
	,P.OtherName
	,P.PinYin
	,P.AreaId
	,P.SubAreaId
	,P.IsComplete
	,P.IsEValue
	,P.Creator
    ,P.Fxt_CompanyId
FROM " + ptable + @"_sub P WITH (NOLOCK)
WHERE P.CityId = @cityId
	AND p.valid = 1
	AND p.Fxt_CompanyId = @fxtCompanyId
ORDER BY P.AreaId,p.ProjectName";

            //var sb = new StringBuilder();
            //sb.Append("select P.ProjectId,A.AreaName,p.ProjectName,P.OtherName,P.PinYin,P.AreaId,P.SubAreaId,P.IsComplete,P.IsEValue from " + ptable + " P with(nolock) ");
            //sb.Append(",FxtDataCenter.dbo.Sys_Area A with(nolock) where P.CityId=" + cityId.ToString() + " and A.AreaId = P.AreaId and p.valid=1 and A.CityId=" + cityId.ToString() + " and p.FxtCompanyId in(" + showcomId + ") and not exists ");
            //sb.Append("(select ProjectId from " + ptable + "_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=" + fxtCompanyId + " and ps.CityId=p.CityId) ");
            //sb.Append(" union ");
            //sb.Append(" select P.ProjectId,A.AreaName,p.ProjectName,P.OtherName,P.PinYin,P.AreaId,P.SubAreaId,P.IsComplete,P.IsEValue from " + ptable + "_sub P with(nolock) ");
            //sb.Append(",FxtDataCenter.dbo.Sys_Area A with(nolock) where P.CityId=" + cityId.ToString() + " and A.AreaId = P.AreaId and p.valid=1 and A.CityId=" + cityId.ToString());
            //sb.Append(" and p.Fxt_CompanyId=" + fxtCompanyId + " order by P.AreaId,p.ProjectName ");

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Project>(sql, new { fxtCompanyId, cityId }).AsQueryable();
            }
        }

        //public IQueryable<SYS_ProjectMatch> GetProjectNetName()
        //{
        //    var strSql = @"select * from dbo.SYS_ProjectMatch";
        //    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //    {
        //        return conn.Query<SYS_ProjectMatch>(strSql).AsQueryable();
        //    }
        //}

        public IQueryable<DAT_Building> GetBuildingList(int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(casecomId)) casecomId = fxtCompanyId.ToString();
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_Building>().AsQueryable();
            }

            string strSql = @"select b.BuildingId,b.BuildingName from " + btable + @" b with(nolock) where b.CityID = " + cityId + @" and b.valid = 1  and b.FxtCompanyId in(" + showcomId + @")  and
not exists(select BuildingId from " + btable + @"_sub b1 with(nolock) where b1.buildingId=b.buildingId and b1.valid=1 and b1.CityID =b.cityId and Fxt_CompanyId =" + fxtCompanyId + @")
union
select bs.BuildingId,bs.BuildingName from " + btable + "_sub bs where bs.valid = 1 and  bs.CityID = " + cityId + " and bs.Fxt_CompanyId=" + fxtCompanyId;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Building>(strSql).AsQueryable();
            }
        }

        public IQueryable<DAT_Case> GetProjectCaseById(int caseId, int fxtCompanyId, int cityId)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtCompanyId.ToString();
            if (string.IsNullOrEmpty(casecomId)) casecomId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_Case>().AsQueryable();
            }

            var strSql = @"select 
                                CaseID, c.ProjectId, BuildingId, HouseId, c.CompanyId, CaseDate, PurposeCode, FloorNumber, BuildingName, 
                                HouseNo, BuildingArea, UsableArea, FrontCode, UnitPrice, MoneyUnitCode, SightCode, CaseTypeCode, StructureCode, 
                                BuildingTypeCode, HouseTypeCode, c.CreateDate,u.TrueName Creator, Remark, TotalPrice, OldID, CityID, c.Valid, FXTCompanyId, 
                                TotalFloor, RemainYear, Depreciation, FitmentCode, SurveyId, SaveDateTime, SaveUser, SourceName, SourceLink, 
                                SourcePhone, AreaId, AreaName, BuildingDate, ZhuangXiu, SubHouse, PeiTao,p1.ProjectName 
                        from " + ctable + @" c with(nolock)
                        inner join (select p.ProjectId,p.ProjectName 
                            from " + ptable + @" p
                            where p.Valid = 1
                            and p.CityID= @cityId
                            and p.FxtCompanyId in (" + showcomId + @")
                            and not exists(select ProjectId from " + ptable + @"_sub p1 
                            where p.ProjectId = p1.ProjectId and p1.Valid=1 and p1.CityID=@cityId
                            and p1.Fxt_CompanyId =@fxtCompanyId)
                            union
                            select p.ProjectId,p.ProjectName 
                            from " + ptable + @"_sub p
                            where p.Valid = 1
                            and p.CityID= @cityId
                            and p.Fxt_CompanyId = @fxtCompanyId) p1 on p1.projectId = c.projectId
                        left join [FxtUserCenter].[dbo].[UserInfo] u
                        on c.Creator=u.UserName
                        where c.valid =1 and c.cityId=@cityId and c.fxtCompanyId in (" + casecomId + @") and c.caseid =@caseId ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Case>(strSql, new { caseId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<ProjectCase_WorkLoad> GetProjectCaseCount(string datefrom, string dateto, int cityid, int fxtcompanyid)
        {
            var strSql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable] FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock) where c.CityId=" + cityid;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var gobalTable = conn.Query<SYS_City_Table>(strSql).FirstOrDefault();

                var cTable = gobalTable == null ? "" : gobalTable.casetable;

                if (cTable == "") return new List<ProjectCase_WorkLoad>().AsQueryable();

                var sql = "select C.Creator as UserName,count(c.CaseId) Count from " + cTable + " C with(nolock) where 1 = 1 and C.createdate between @datefrom and @dateto and C.valid=1 and FXTCompanyId=@fxtcompanyid and c.CityId=@cityid  group by C.Creator";

                dateto = dateto + " 23:59:59";
                return conn.Query<ProjectCase_WorkLoad>(sql, new { datefrom, dateto, fxtcompanyid, cityid }).AsQueryable();

            }
        }

        public IQueryable<ProjectCase_Statist> GetProjectCaseCount(string casedateFrom, string casedateTo, int caseTypeCode, string condition, int count, int cityid, int fxtcompanyid)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyid.ToString();
            if (string.IsNullOrEmpty(casecomId)) casecomId = fxtcompanyid.ToString();
            if (ptable == "" || ctable == "")
            {
                return new List<ProjectCase_Statist>().AsQueryable();
            }
            //if (self)
            //{
            //    ComId = fxtcompanyid.ToString();
            //}
            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;
            var strWhere = caseTypeCode <= 0 ? " and CaseTypeCode in (3001001,3001002,3001003,3001004,3001005)" : " and CaseTypeCode = @caseTypeCode";
            //只计算，居住用途在 “普通住宅,非普通住宅,公寓,新式里弄,旧式里弄,商住,经济适用房”范围之内的。
            var strSql = @"
select
	ProjectId
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = T3.AreaID) as AreaName
	,ProjectName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = T3.PurposeCode) as PurposeCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = T3.pBuildingTypeCode) as pBuildingTypeCodeName
	,casecount_fxt
	,CONVERT(numeric(18,0),(case when totalbuildingarea_fxt > 0 then totalprice_fxt / totalbuildingarea_fxt else 0 end)) as caseprice_fxt
	,casecount_nfxt
	,CONVERT(numeric(18,0),(case when totalbuildingarea_nfxt > 0 then totalprice_nfxt / totalbuildingarea_nfxt else 0 end)) as caseprice_nfxt
	,casecount
	,CONVERT(numeric(18,0),(case when totalbuildingarea > 0 then totalprice / totalbuildingarea else 0 end)) as caseprice
	,LowerCount_fxt
	,CONVERT(numeric(18,0),(case when LowerBuildingArea_fxt > 0 then LowerPrice_fxt / LowerBuildingArea_fxt else 0 end)) as lowerprice_fxt
	,LowerCount_nfxt
	,CONVERT(numeric(18,0),(case when LowerBuildingArea_nfxt > 0 then LowerPrice_nfxt / LowerBuildingArea_nfxt else 0 end)) as lowerprice_nfxt
	,LowerCount
	,CONVERT(numeric(18,0),(case when LowerBuildingArea > 0 then LowerPrice / LowerBuildingArea else 0 end)) as lowerprice
	,MultiCount_fxt
	,CONVERT(numeric(18,0),(case when MultiBuildingArea_fxt > 0 then MultiPrice_fxt / MultiBuildingArea_fxt else 0 end)) as multiprice_fxt
	,MultiCount_nfxt
	,CONVERT(numeric(18,0),(case when MultiBuildingArea_nfxt > 0 then MultiPrice_nfxt / MultiBuildingArea_nfxt else 0 end)) as multiprice_nfxt
	,MultiCount
	,CONVERT(numeric(18,0),(case when MultiBuildingArea > 0 then MultiPrice / MultiBuildingArea else 0 end)) as multiprice
	,SmallHighCount_fxt
	,CONVERT(numeric(18,0),(case when SmallHighBuildingArea_fxt > 0 then SmallHighPrice_fxt / SmallHighBuildingArea_fxt else 0 end)) as smallhighprice_fxt
	,SmallHighCount_nfxt
	,CONVERT(numeric(18,0),(case when SmallHighBuildingArea_nfxt > 0 then SmallHighPrice_nfxt / SmallHighBuildingArea_nfxt else 0 end)) as smallhighprice_nfxt
	,SmallHighCount
	,CONVERT(numeric(18,0),(case when SmallHighBuildingArea > 0 then SmallHighPrice / SmallHighBuildingArea else 0 end)) as smallhighprice
	,HighCount_fxt
	,CONVERT(numeric(18,0),(case when HighBuildingArea_fxt > 0 then HighPrice_fxt / HighBuildingArea_fxt else 0 end)) as highprice_fxt
	,HighCount_nfxt
	,CONVERT(numeric(18,0),(case when HighBuildingArea_nfxt > 0 then HighPrice_nfxt / HighBuildingArea_nfxt else 0 end)) as highprice_nfxt
	,HighCount
	,CONVERT(numeric(18,0),(case when HighBuildingArea > 0 then HighPrice / HighBuildingArea else 0 end)) as highprice
	,NBuildingTypeCount
	,CONVERT(numeric(18,0),(case when NBuildingTypeBuildingArea > 0 then NBuildingTypePrice / NBuildingTypeBuildingArea else 0 end)) as nbuildingtypeprice
from (
	select
		ProjectId
		,ProjectName
		,AreaID
		,PurposeCode
		,pBuildingTypeCode
		,SUM(LowerCount_fxt + MultiCount_fxt + SmallHighCount_fxt + HighCount_fxt + NBuildingTypeCount_fxt) as casecount_fxt
		,SUM(LowerPrice_fxt + MultiPrice_fxt + SmallHighPrice_fxt + HighPrice_fxt + NBuildingTypePrice_fxt) as totalprice_fxt
		,SUM(LowerBuildingArea_fxt + MultiBuildingArea_fxt + SmallHighBuildingArea_fxt + HighBuildingArea_fxt + NBuildingTypeBuildingArea_fxt) as totalbuildingarea_fxt
		,SUM(LowerCount_nfxt + MultiCount_nfxt + SmallHighCount_nfxt + HighCount_nfxt + NBuildingTypeCount_nfxt) as casecount_nfxt
		,SUM(LowerPrice_nfxt + MultiPrice_nfxt + SmallHighPrice_nfxt + HighPrice_nfxt + NBuildingTypePrice_nfxt) as totalprice_nfxt
		,SUM(LowerBuildingArea_nfxt + MultiBuildingArea_nfxt + SmallHighBuildingArea_nfxt + HighBuildingArea_nfxt + NBuildingTypeBuildingArea_nfxt) as totalbuildingarea_nfxt
		,SUM(LowerCount_fxt + LowerCount_nfxt + MultiCount_fxt + MultiCount_nfxt + SmallHighCount_fxt + SmallHighCount_nfxt + HighCount_fxt + HighCount_nfxt + NBuildingTypeCount_fxt + NBuildingTypeCount_nfxt) as casecount
		,SUM(LowerPrice_fxt + LowerPrice_nfxt + MultiPrice_fxt + MultiPrice_nfxt + SmallHighPrice_fxt + SmallHighPrice_nfxt + HighPrice_fxt + HighPrice_nfxt + NBuildingTypePrice_fxt + NBuildingTypePrice_nfxt) as totalprice
		,SUM(LowerBuildingArea_fxt + LowerBuildingArea_nfxt + MultiBuildingArea_fxt + MultiBuildingArea_nfxt + SmallHighBuildingArea_fxt + SmallHighBuildingArea_nfxt + HighBuildingArea_fxt + HighBuildingArea_nfxt + NBuildingTypeBuildingArea_fxt + NBuildingTypeBuildingArea_nfxt) as totalbuildingarea
		,SUM(LowerCount_fxt) as LowerCount_fxt
		,SUM(LowerCount_nfxt) as LowerCount_nfxt
		,SUM(LowerCount_fxt + LowerCount_nfxt) as LowerCount
		,SUM(MultiCount_fxt) as MultiCount_fxt
		,SUM(MultiCount_nfxt) as MultiCount_nfxt
		,SUM(MultiCount_fxt + MultiCount_nfxt) as MultiCount
		,SUM(SmallHighCount_fxt) as SmallHighCount_fxt
		,SUM(SmallHighCount_nfxt) as SmallHighCount_nfxt
		,SUM(SmallHighCount_fxt + SmallHighCount_nfxt) as SmallHighCount
		,SUM(HighCount_fxt) as HighCount_fxt
		,SUM(HighCount_nfxt) as HighCount_nfxt
		,SUM(HighCount_fxt + HighCount_nfxt) as HighCount
		,SUM(NBuildingTypeCount_fxt) as NBuildingTypeCount_fxt
		,SUM(NBuildingTypeCount_nfxt) as NBuildingTypeCount_nfxt
		,SUM(NBuildingTypeCount_fxt + NBuildingTypeCount_nfxt) as NBuildingTypeCount
		,SUM(LowerPrice_fxt) as LowerPrice_fxt
		,SUM(LowerPrice_nfxt) as LowerPrice_nfxt
		,SUM(LowerPrice_fxt + LowerPrice_nfxt) as LowerPrice
		,SUM(MultiPrice_fxt) as MultiPrice_fxt
		,SUM(MultiPrice_nfxt) as MultiPrice_nfxt
		,SUM(MultiPrice_fxt + MultiPrice_nfxt) as MultiPrice
		,SUM(SmallHighPrice_fxt) as SmallHighPrice_fxt
		,SUM(SmallHighPrice_nfxt) as SmallHighPrice_nfxt
		,SUM(SmallHighPrice_fxt + SmallHighPrice_nfxt) as SmallHighPrice
		,SUM(HighPrice_fxt) as HighPrice_fxt
		,SUM(HighPrice_nfxt) as HighPrice_nfxt
		,SUM(HighPrice_fxt + HighPrice_nfxt) as HighPrice
		,SUM(NBuildingTypePrice_fxt) as NBuildingTypePrice_fxt
		,SUM(NBuildingTypePrice_nfxt) as NBuildingTypePrice_nfxt
		,SUM(NBuildingTypePrice_fxt + NBuildingTypePrice_nfxt) as NBuildingTypePrice
		,SUM(LowerBuildingArea_fxt) as LowerBuildingArea_fxt
		,SUM(LowerBuildingArea_nfxt) as LowerBuildingArea_nfxt
		,SUM(LowerBuildingArea_fxt + LowerBuildingArea_nfxt) as LowerBuildingArea
		,SUM(MultiBuildingArea_fxt) as MultiBuildingArea_fxt
		,SUM(MultiBuildingArea_nfxt) as MultiBuildingArea_nfxt
		,SUM(MultiBuildingArea_fxt + MultiBuildingArea_nfxt) as MultiBuildingArea
		,SUM(SmallHighBuildingArea_fxt) as SmallHighBuildingArea_fxt
		,SUM(SmallHighBuildingArea_nfxt) as SmallHighBuildingArea_nfxt
		,SUM(SmallHighBuildingArea_fxt + SmallHighBuildingArea_nfxt) as SmallHighBuildingArea
		,SUM(HighBuildingArea_fxt) as HighBuildingArea_fxt
		,SUM(HighBuildingArea_nfxt) as HighBuildingArea_nfxt
		,SUM(HighBuildingArea_fxt + HighBuildingArea_nfxt) as HighBuildingArea
		,SUM(NBuildingTypeBuildingArea_fxt) as NBuildingTypeBuildingArea_fxt
		,SUM(NBuildingTypeBuildingArea_nfxt) as NBuildingTypeBuildingArea_nfxt
		,SUM(NBuildingTypeBuildingArea_fxt + NBuildingTypeBuildingArea_nfxt) as NBuildingTypeBuildingArea
	from (
		select
			ProjectId
			,ProjectName
			,AreaID
			,PurposeCode
			,pBuildingTypeCode
			,case when cBuildingTypeCode = 2003001 and cFXTCompanyId = 25 then cBuildingTypeCodeNum else 0 end as LowerCount_fxt
			,case when cBuildingTypeCode = 2003001 and cFXTCompanyId = 25 then totalprice else 0 end as LowerPrice_fxt
			,case when cBuildingTypeCode = 2003001 and cFXTCompanyId = 25 then totalbuildingarea else 0 end as LowerBuildingArea_fxt
			,case when cBuildingTypeCode = 2003001 and cFXTCompanyId = 0 then cBuildingTypeCodeNum else 0 end as LowerCount_nfxt
			,case when cBuildingTypeCode = 2003001 and cFXTCompanyId = 0 then totalprice else 0 end as LowerPrice_nfxt
			,case when cBuildingTypeCode = 2003001 and cFXTCompanyId = 0 then totalbuildingarea else 0 end as LowerBuildingArea_nfxt
			,case when cBuildingTypeCode = 2003002 and cFXTCompanyId = 25 then cBuildingTypeCodeNum else 0 end as MultiCount_fxt
			,case when cBuildingTypeCode = 2003002 and cFXTCompanyId = 25 then totalprice else 0 end as MultiPrice_fxt
			,case when cBuildingTypeCode = 2003002 and cFXTCompanyId = 25 then totalbuildingarea else 0 end as MultiBuildingArea_fxt
			,case when cBuildingTypeCode = 2003002 and cFXTCompanyId = 0 then cBuildingTypeCodeNum else 0 end as MultiCount_nfxt
			,case when cBuildingTypeCode = 2003002 and cFXTCompanyId = 0 then totalprice else 0 end as MultiPrice_nfxt
			,case when cBuildingTypeCode = 2003002 and cFXTCompanyId = 0 then totalbuildingarea else 0 end as MultiBuildingArea_nfxt
			,case when cBuildingTypeCode = 2003003 and cFXTCompanyId = 25 then cBuildingTypeCodeNum else 0 end as SmallHighCount_fxt
			,case when cBuildingTypeCode = 2003003 and cFXTCompanyId = 25 then totalprice else 0 end as SmallHighPrice_fxt
			,case when cBuildingTypeCode = 2003003 and cFXTCompanyId = 25 then totalbuildingarea else 0 end as SmallHighBuildingArea_fxt	
			,case when cBuildingTypeCode = 2003003 and cFXTCompanyId = 0 then cBuildingTypeCodeNum else 0 end as SmallHighCount_nfxt
			,case when cBuildingTypeCode = 2003003 and cFXTCompanyId = 0 then totalprice else 0 end as SmallHighPrice_nfxt
			,case when cBuildingTypeCode = 2003003 and cFXTCompanyId = 0 then totalbuildingarea else 0 end as SmallHighBuildingArea_nfxt
			,case when cBuildingTypeCode = 2003004 and cFXTCompanyId = 25 then cBuildingTypeCodeNum else 0 end as HighCount_fxt
			,case when cBuildingTypeCode = 2003004 and cFXTCompanyId = 25 then totalprice else 0 end as HighPrice_fxt
			,case when cBuildingTypeCode = 2003004 and cFXTCompanyId = 25 then totalbuildingarea else 0 end as HighBuildingArea_fxt
			,case when cBuildingTypeCode = 2003004 and cFXTCompanyId = 0 then cBuildingTypeCodeNum else 0 end as HighCount_nfxt
			,case when cBuildingTypeCode = 2003004 and cFXTCompanyId = 0 then totalprice else 0 end as HighPrice_nfxt
			,case when cBuildingTypeCode = 2003004 and cFXTCompanyId = 0 then totalbuildingarea else 0 end as HighBuildingArea_nfxt
			,case when cBuildingTypeCode = 0 and cFXTCompanyId = 25 then cBuildingTypeCodeNum else 0 end as NBuildingTypeCount_fxt
			,case when cBuildingTypeCode = 0 and cFXTCompanyId = 25 then totalprice else 0 end as NBuildingTypePrice_fxt
			,case when cBuildingTypeCode = 0 and cFXTCompanyId = 25 then totalbuildingarea else 0 end as NBuildingTypeBuildingArea_fxt
			,case when cBuildingTypeCode = 0 and cFXTCompanyId = 0 then cBuildingTypeCodeNum else 0 end as NBuildingTypeCount_nfxt
			,case when cBuildingTypeCode = 0 and cFXTCompanyId = 0 then totalprice else 0 end as NBuildingTypePrice_nfxt
			,case when cBuildingTypeCode = 0 and cFXTCompanyId = 0 then totalbuildingarea else 0 end as NBuildingTypeBuildingArea_nfxt
		from (
			select 
				T.ProjectId
				,T.ProjectName
				,T.AreaID
				,T.PurposeCode
				,T.pBuildingTypeCode
				,T.cBuildingTypeCode
				,T.cFXTCompanyId
				,COUNT(CaseID) as cBuildingTypeCodeNum
				,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
				,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
			from (
				select 
					P.ProjectId
					,P.projectname
					,p.AreaID
					,p.PurposeCode
					,p.BuildingTypeCode as pBuildingTypeCode
					,c.CaseID
					,c.BuildingArea
					,c.UnitPrice
					,(case when c.BuildingTypeCode in (2003001,2003002,2003003,2003004) then c.BuildingTypeCode else 0 end) as cBuildingTypeCode
					,(case when c.FXTCompanyId = 25 then 25 when c.FXTCompanyId <> 25 then 0 else c.FXTCompanyId end) as cFXTCompanyId
				from (
					select ProjectId,ProjectName,PurposeCode,AreaID,BuildingTypeCode
					from " + ptable + @" p with(nolock)
					where not exists (
						select ProjectId from " + ptable + @"_sub ps with(nolock)
						where ps.ProjectId = p.ProjectId
						and ps.CityID = @cityid
						and ps.Fxt_CompanyId = @fxtcompanyid
					)
					and p.Valid = 1
					and p.CityID = @cityid
					and p.FxtCompanyId in (" + showcomId + @")
					union
					select ProjectId,ProjectName,PurposeCode,AreaID,BuildingTypeCode
					from " + ptable + @"_sub p with(nolock)
					where p.Valid = 1
					and p.CityID = @cityid
					and p.Fxt_CompanyId = @fxtcompanyid
				)P
				left join (
					select * from " + ctable + @" with(nolock)
					where Valid = 1
					and CityID = @cityid
					and FXTCompanyId in (" + casecomId + @")
					and CaseDate between @datefrom and @dateto
					and PurposeCode in (1002001,1002002,1002003,1002012,1002013,1002021,1002023)
                    " + strWhere + @"
				) c on P.ProjectId = c.ProjectId
				where 1 = 1
			)T
			group by T.ProjectId,T.ProjectName,T.AreaID,T.PurposeCode,T.pBuildingTypeCode,T.cBuildingTypeCode,T.cFXTCompanyId
		)T1
	)T2
	group by ProjectId
			,ProjectName
			,AreaID
			,PurposeCode
			,pBuildingTypeCode
)T3
where 1 = 1
and casecount " + condition + @" @count
order by casecount desc,AreaID,ProjectName";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var result = conn.Query<ProjectCase_Statist>(strSql, new { datefrom = Convert.ToDateTime(casedateFrom), dateto = Convert.ToDateTime(casedateTo), caseTypeCode, count, cityid, fxtcompanyid }).AsQueryable();
                return result;
            }
        }

        public IQueryable<ProjectCase_ProjectEValue> GetProjectEValueCount(string datefrom, string dateto, List<int> peareaname, string ProjectEValueProjectName, int cityid, int fxtcompanyid, string projectUEReason)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyid.ToString();
            if (string.IsNullOrEmpty(casecomId)) casecomId = fxtcompanyid.ToString();
            if (ptable == "" || ctable == "")
            {
                return new List<ProjectCase_ProjectEValue>().AsQueryable();
            }
            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;

            string where = string.Empty;
            if (peareaname != null && peareaname.Count > 0 && !peareaname.Contains(-1))
            {
                var areanames = string.Join(",", peareaname);
                where += " and AreaID in (" + areanames + ")";
            }
            if (!string.IsNullOrWhiteSpace(ProjectEValueProjectName)) 
            {
                where += " and ProjectName like @ProjectName "; 
            }
            if (projectUEReason == "1")
            {
                where += " and remark = '自身评估案例不足'";
            }
            else if (projectUEReason == "2")
            {
                where += " and remark = '案例不足'";
            }

            var strSql = @"
select * from (
	select
		P.CityID
		,P.AreaID
		,a.AreaName
		,P.ProjectId
		,P.ProjectName
		,(case when P.PIsEValue = 1 then '是' else '否' end) as PIsEValue
		,p.PWeight
		,ISNULL(P.Address,'') as [Address]
		,C.casecount
		,c.unitprice
		,c.cbcount
		,c.avgprice
		,c.avgPrice_other
		,c.evalue
		,(case when c.casecount >= 2 and c.casecount < 5 then '案例均价:' + CONVERT(nvarchar(10),c.unitprice) + '元'
			when c.casecount >= 5 then CONVERT(nvarchar(10),c.avgprice) + '元'
			else '--' end) as PEPrice
		,(case when ISNULL(casecount,0) < 5 then '案例不足'
			when casecount >= 5 and avgprice = 0 then '自身评估案例不足'
			else '' end) as remark
	from (
		select CityID,ProjectId,ProjectName,Address,AreaID,IsEValue as PIsEValue,Weight as PWeight from " + ptable + @" p with(nolock)
		where not exists(
			select ProjectId from " + ptable + @"_sub ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityID = @cityid
			and ps.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (
			select value from FXTProject.dbo.SplitToTable((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where TypeCode = 1003002 and CityID = @CityId and FxtCompanyId = @FXTCompanyId),',')
		)
		union
		select CityID,ProjectId,ProjectName,Address,AreaID,IsEValue as PIsEValue,Weight as PWeight from " + ptable + @"_sub ps with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
	)P
	left join (
		select
			CityID,ProjectId,casecount,unitprice
			,cbcount
			,convert(int,avgprice) as avgprice
			,convert(int,avgPrice_other) as avgPrice_other
			,convert(int,(case when cbcount >= 5 then convert(int,avgPrice)
				when cbcount < 5 and avgPrice > 0 and avgPrice_other > 0 then convert(int,avgPrice * 0.7 + avgPrice_other * 0.3,0)
				when cbcount < 5 and avgPrice > 0 and avgPrice_other <= 0 then convert(int,avgPrice,0)
				when cbcount < 5 and avgPrice <= 0 and avgPrice_other > 0 then convert(int,avgPrice_other,0)
				else 0 end)) as evalue
		from (
			select
				CityID,ProjectId,casecount
				,convert(int,totalprice / totalbuildingarea,0) as unitprice
				,cbcount
				,(case when ISNULL(cjRate,0) + ISNULL(bpRate,0) <> 1 then (cjTotalPrice + bpTotalPrice) / (cjBuildingarea + bpBuildingarea)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount > 0 and bpcount > 0 then cjTotalPrice / cjBuildingarea * ISNULL(cjRate,0) + bpTotalPrice / bpBuildingarea * ISNULL(bpRate,0)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount > 0 and bpcount <= 0 then cjTotalPrice / cjBuildingarea
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount <= 0 and bpcount > 0 then bpTotalPrice / bpBuildingarea
					else 0 end) as avgPrice
				,(case when ISNULL(cjRate,0) + ISNULL(bpRate,0) <> 1 then (cjTotalPrice_other + bpTotalPrice_other) / (cjBuildingarea_other + bpBuildingarea_other)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount_other > 0 and bpcount_other > 0 then cjTotalPrice_other / cjBuildingarea_other * ISNULL(cjRate,0) + bpTotalPrice_other / bpBuildingarea_other * ISNULL(bpRate,0)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount_other > 0 and bpcount_other <= 0 then cjTotalPrice_other / cjBuildingarea_other
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount_other <= 0 and bpcount_other > 0 then bpTotalPrice_other / bpBuildingarea_other
					else 0 end) as avgPrice_other
			from (
				select
					CityID
					,(select c.PriceCJ from FxtDataCenter.dbo.SYS_City c with(nolock) where CityID = @CityId) as cjRate
					,(select c.PriceBP from FxtDataCenter.dbo.SYS_City c with(nolock) where CityID = @CityId) as bpRate
					,ProjectId
					,SUM(1) as casecount
					,SUM(BuildingArea) as totalbuildingarea
					,SUM(BuildingArea * UnitPrice) as totalprice
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001,3001002,3001003) then 1 else 0 end) as cbcount
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001002,3001003) then 1 else 0 end) as cjcount
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea else 0 end) as cjBuildingarea
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea * UnitPrice else 0 end) as cjTotalPrice
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001) then 1 else 0 end) as bpcount
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea else 0 end) as bpBuildingarea
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea * UnitPrice else 0 end) as bpTotalPrice	
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001002,3001003) then 1 else 0 end) as cjcount_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea else 0 end) as cjBuildingarea_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea * UnitPrice else 0 end) as cjTotalPrice_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001001) then 1 else 0 end) as bpcount_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea else 0 end) as bpBuildingarea_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea * UnitPrice else 0 end) as bpTotalPrice_other
				from " + ctable + @"
				where 1 = 1
				and CityID = @CityId
				and UnitPrice > 0
				and BuildingArea > 0
				and PurposeCode in (1002001,1002002,1002003,1002004,1002010,1002011,1002012,1002013)
				and CaseTypeCode in (3001001,3001002,3001003)
				and Casedate BETWEEN @datefrom and @dateto
				and valid = 1
				and FXTCompanyId in(
						select value from FXTProject.dbo.SplitToTable((select CaseCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where TypeCode = 1003002 and CityID = @CityId and FxtCompanyId = @FXTCompanyId),',')
					)
				group by CityID,ProjectId
			)T
		)T
	)C on P.CityID = C.CityID and P.ProjectId = C.ProjectId
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on P.AreaID = a.AreaId
)T
where 1 = 1" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<ProjectCase_ProjectEValue>(strSql, new { datefrom = Convert.ToDateTime(datefrom), dateto = Convert.ToDateTime(dateto), peareaname, ProjectName = "%" + ProjectEValueProjectName + "%", cityid, fxtcompanyid }).AsQueryable();
            }
        }

        public IQueryable<ProjectCase_BuildingEValue> GetBuildingEValueCount(string datefrom, string dateto, List<int> peareaname, string BuildingEValueProjectName, int cityid, int fxtcompanyid, string buildingUEReason)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyid.ToString();
            if (string.IsNullOrEmpty(casecomId)) casecomId = fxtcompanyid.ToString();
            if (ptable == "" || ctable == "")
            {
                return new List<ProjectCase_BuildingEValue>().AsQueryable();
            }
            ptable = "FXTProject." + ptable;
            btable = "FXTProject." + btable;
            ctable = "FXTProject." + ctable;

            string where = string.Empty;
            if (peareaname != null && peareaname.Count > 0 && !peareaname.Contains(-1))
            {
                var areanames = string.Join(",", peareaname);
                where += " and AreaID in (" + areanames + ")";
            }
            if (!string.IsNullOrWhiteSpace(BuildingEValueProjectName)) 
            { 
                where += " and ProjectName like @ProjectName "; 
            }
            if (buildingUEReason == "1")
            {
                where += " and remark = '楼栋不可估'"; 
            }
            else if (buildingUEReason == "2")
            {
                where += " and remark = '案例不足'"; 
            }
            var strSql = @"
select * from (
	select
		P.CityID
		,P.AreaID
		,a.AreaName
		,P.ProjectId
		,P.ProjectName
		,P.PIsEValue
		,p.PWeight
		,ISNULL(P.Address,'') as [Address]
		,ISNULL(P.BuildingId,0) as BuildingId
		,ISNULL(P.BuildingName,'') as BuildingName
		,P.BIsEValue
		,P.BWeight
		,ISNULL(P.BEWeight,0) as BEWeight
		,ISNULL(C.casecount,0) as casecount
		,c.unitprice
		,c.cbcount
		,c.avgprice
		,c.avgPrice_other
		,c.evalue
		,ISNULL(CONVERT(int,evalue * BEWeight),0) as BPrice
		,(case when c.casecount >= 2 and c.casecount < 5 then '案例均价:' + CONVERT(nvarchar(10),c.unitprice) + '元'
			when c.casecount >= 5 then CONVERT(nvarchar(10),c.avgprice) + '元'
			else '--' end) as PEPrice
		,(case when c.casecount < 5 then '--'
			when c.casecount >= 5 and (BWeight = 0 or BIsEValue = 0 or BIsEValue is null) then '--'
			when c.casecount >= 5 and ISNULL(CONVERT(int,evalue * BEWeight),0) < 0 then '--'
			when c.casecount >= 5 and ISNULL(CONVERT(int,evalue * BEWeight),0) = 0 then convert(nvarchar(10),c.avgprice) + '元'
			else CONVERT(nvarchar(10),CONVERT(int,evalue * BEWeight)) + '元' end) as BEPrice
		,(case when ISNULL(casecount,0) < 5 then '案例不足'
			when BWeight = 0 or BIsEValue = 0 or BIsEValue is null then '楼栋不可估'
			else '' end) as remark
	from (
		select
			P.*
			,B.BuildingId
			,B.BuildingName
			,B.BIsEValue
			,B.BWeight
			,(case when B.BIsEValue is null or B.BWeight IS null OR B.BIsEValue * b.BWeight < 0 then 0 else B.BIsEValue * b.BWeight end) as BEWeight
		from (	
			select CityID,ProjectId,ProjectName,Address,AreaID,IsEValue as PIsEValue,Weight as PWeight from " + ptable + @" p with(nolock)
			where not exists(
				select ProjectId from " + ptable + @"_sub ps with(nolock)
				where ps.ProjectId = p.ProjectId
				and ps.CityID = @cityid
				and ps.Fxt_CompanyId = @fxtcompanyid
			)
			and Valid = 1
			and CityID = @cityid
			and FxtCompanyId in (
				select value from FXTProject.dbo.SplitToTable((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where TypeCode = 1003002 and CityID = @CityId and FxtCompanyId = @FXTCompanyId),',')
			)
			union
			select CityID,ProjectId,ProjectName,Address,AreaID,IsEValue as PIsEValue,Weight as PWeight from " + ptable + @"_sub ps with(nolock)
			where Valid = 1
			and CityID = @cityid
			and Fxt_CompanyId = @fxtcompanyid
		)P
		inner join (	
			select ProjectId,BuildingId,BuildingName,Weight as BWeight,IsEValue as BIsEValue from " + btable + @" b with(nolock)
			where not exists(
				select ProjectId from " + btable + @"_sub bs with(nolock)
				where bs.BuildingId = b.BuildingId
				and bs.CityID = @cityid
				and bs.Fxt_CompanyId = @fxtcompanyid
			)
			and Valid = 1
			and CityID = @cityid
			and FxtCompanyId in (
				select value from FXTProject.dbo.SplitToTable((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where TypeCode = 1003002 and CityID = @CityId and FxtCompanyId = @FXTCompanyId),',')
			)
			union
			select ProjectId,BuildingId,BuildingName,Weight,IsEValue from " + btable + @"_sub ps with(nolock)
			where Valid = 1
			and CityID = @cityid
			and Fxt_CompanyId = @fxtcompanyid
		)B on P.ProjectId = B.ProjectId
	)P
	left join (
		select
			CityID,ProjectId,casecount,unitprice
			,cbcount
			,convert(int,avgprice) as avgprice
			,convert(int,avgPrice_other) as avgPrice_other
			,convert(int,(case when cbcount >= 5 then convert(int,avgPrice)
				when cbcount < 5 and avgPrice > 0 and avgPrice_other > 0 then convert(int,avgPrice * 0.7 + avgPrice_other * 0.3,0)
				when cbcount < 5 and avgPrice > 0 and avgPrice_other <= 0 then convert(int,avgPrice,0)
				when cbcount < 5 and avgPrice <= 0 and avgPrice_other > 0 then convert(int,avgPrice_other,0)
				else 0 end)) as evalue
		from (
			select
				CityID,ProjectId,casecount
				,convert(int,totalprice / totalbuildingarea,0) as unitprice
				,cbcount
				,(case when ISNULL(cjRate,0) + ISNULL(bpRate,0) <> 1 then (cjTotalPrice + bpTotalPrice) / (cjBuildingarea + bpBuildingarea)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount > 0 and bpcount > 0 then cjTotalPrice / cjBuildingarea * ISNULL(cjRate,0) + bpTotalPrice / bpBuildingarea * ISNULL(bpRate,0)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount > 0 and bpcount <= 0 then cjTotalPrice / cjBuildingarea
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount <= 0 and bpcount > 0 then bpTotalPrice / bpBuildingarea
					else 0 end) as avgPrice
				,(case when ISNULL(cjRate,0) + ISNULL(bpRate,0) <> 1 then (cjTotalPrice_other + bpTotalPrice_other) / (cjBuildingarea_other + bpBuildingarea_other)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount_other > 0 and bpcount_other > 0 then cjTotalPrice_other / cjBuildingarea_other * ISNULL(cjRate,0) + bpTotalPrice_other / bpBuildingarea_other * ISNULL(bpRate,0)
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount_other > 0 and bpcount_other <= 0 then cjTotalPrice_other / cjBuildingarea_other
					when ISNULL(cjRate,0) + ISNULL(bpRate,0) = 1 and cjcount_other <= 0 and bpcount_other > 0 then bpTotalPrice_other / bpBuildingarea_other
					else 0 end) as avgPrice_other
			from (
				select
					CityID
					,(select c.PriceCJ from FxtDataCenter.dbo.SYS_City c with(nolock) where CityID = @CityId) as cjRate
					,(select c.PriceBP from FxtDataCenter.dbo.SYS_City c with(nolock) where CityID = @CityId) as bpRate
					,ProjectId
					,SUM(1) as casecount
					,SUM(BuildingArea) as totalbuildingarea
					,SUM(BuildingArea * UnitPrice) as totalprice
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001,3001002,3001003) then 1 else 0 end) as cbcount
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001002,3001003) then 1 else 0 end) as cjcount
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea else 0 end) as cjBuildingarea
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea * UnitPrice else 0 end) as cjTotalPrice
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001) then 1 else 0 end) as bpcount
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea else 0 end) as bpBuildingarea
					,SUM(case when FXTCompanyId = @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea * UnitPrice else 0 end) as bpTotalPrice	
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001002,3001003) then 1 else 0 end) as cjcount_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea else 0 end) as cjBuildingarea_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001002,3001003) then BuildingArea * UnitPrice else 0 end) as cjTotalPrice_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001001) then 1 else 0 end) as bpcount_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea else 0 end) as bpBuildingarea_other
					,SUM(case when FXTCompanyId <> @FXTCompanyId and CaseTypeCode in(3001001) then BuildingArea * UnitPrice else 0 end) as bpTotalPrice_other
				from " + ctable + @"
				where 1 = 1
				and CityID = @CityId
				and UnitPrice > 0
				and BuildingArea > 0
				and PurposeCode in (1002001,1002002,1002003,1002004,1002010,1002011,1002012,1002013)
				and CaseTypeCode in (3001001,3001002,3001003)
				and Casedate BETWEEN @datefrom and @dateto
				and valid = 1
				and FXTCompanyId in(
						select value from FXTProject.dbo.SplitToTable((select CaseCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where TypeCode = 1003002 and CityID = @CityId and FxtCompanyId = @FXTCompanyId),',')
					)
				group by CityID,ProjectId
			)T
		)T
	)C on P.CityID = C.CityID and P.ProjectId = C.ProjectId
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on P.AreaID = a.AreaId
)T
where 1 = 1" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<ProjectCase_BuildingEValue>(strSql, new { datefrom = Convert.ToDateTime(datefrom), dateto = Convert.ToDateTime(dateto), peareaname, ProjectName = "%" + BuildingEValueProjectName + "%", cityid, fxtcompanyid }).AsQueryable();
            }
        }

        #endregion

        #region 案例均价统计
        public DataTable GetProjectCaseAvePrice(ProjectCase_AvgPrice parameters)
        {
            //var avePrice = new List<ProjectCase_AvePrice>().AsQueryable();
            List<SqlParameter> paramet = new List<SqlParameter>();
            var avePrice = new DataTable();
            var cycle = parameters.groupcycle;
            var area = parameters.grouparea;

            parameters.casetypecode = parameters.casetypecode ?? new List<int>();
            parameters.buildingtypecode = parameters.buildingtypecode ?? new List<int>();
            parameters.purposecode = parameters.purposecode ?? new List<int>();

            string ptable, ctable, btable, casecomId, showcomId;
            Access(Convert.ToInt32(parameters.cityid), Convert.ToInt32(parameters.fxtcompanyid), out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = parameters.fxtcompanyid.ToString();
            if (string.IsNullOrEmpty(casecomId)) casecomId = parameters.fxtcompanyid.ToString();
            if (ptable == "" || ctable == "")
            {
                return null;
            }

            if (parameters.timeFrom == null || parameters.timeTo == null)
            {
                return null;
            }

            parameters.timeTo = parameters.timeTo.Value.AddMonths(1).AddDays(-1);
            var date = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + "~" + parameters.timeTo.Value.ToString("yyyy-MM-dd");

            string where = string.Empty;
            string cwhere = string.Empty;
            //行政区
            if (parameters.areaname != null && parameters.areaname.Count > 0 && !parameters.areaname.Contains(-1))
            {
                var areanames = string.Join(",", parameters.areaname);
                where += " and AreaId in (" + areanames + ")";
            }
            //案例类型  取案例
            if (parameters.casetypecode != null && parameters.casetypecode.Count > 0 && !parameters.casetypecode.Contains(-1))
            {
                var casetypecodes = string.Join(",", parameters.casetypecode);
                cwhere += " and c.CaseTypeCode in (" + casetypecodes + ")";
            }
            //建筑类型  取案例
            if (parameters.buildingtypecode != null && parameters.buildingtypecode.Count > 0 && !parameters.buildingtypecode.Contains(-1))
            {
                var buildingtypecodes = string.Join(",", parameters.buildingtypecode);
                cwhere += " and c.BuildingTypeCode in (" + buildingtypecodes + ")";
            }
            //用途    取案例
            if (parameters.purposecode != null && parameters.purposecode.Count > 0 && !parameters.purposecode.Contains(-1))
            {
                var purposecodes = string.Join(",", parameters.purposecode);
                cwhere += " and c.PurposeCode in (" + purposecodes + ")";
            }
            //建筑年代 取案例
            if (parameters.buildingdatecode != null && parameters.buildingdatecode.Count > 0 && !parameters.buildingdatecode.Contains(-1))
            {
                var buildingdatecode = string.Join(",", parameters.buildingdatecode);
                where += " and BuildingDateCode in (" + buildingdatecode + ")";
            }

            #region sql
            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;

            string sampleprojectwhere = parameters.sampleproject == "1" ? @"
		    inner join (
				select * from FXTProject.dbo.DAT_SampleProject with(nolock)
				where CityId = @cityid
				and FxtCompanyId = @fxtcompanyid
				and Valid = 1
		    )S on P.ProjectId = S.ProjectId and P.CityID = S.CityId" : "";

            string sql = @"
            select * from (
		        select
			        C.CaseID
			        ,C.CaseDate
			        ,C.CaseTypeCode
			        ,C.BuildingArea
			        ,C.UnitPrice
			        ,P.ProjectId
			        ,P.ProjectName
			        ,P.AreaID
			        ,case when P.SubAreaId > 0 then P.SubAreaId else 0 end as SubAreaId
			        ,CONVERT(varchar(7), C.CaseDate, 120) + '-01' as casemonth
                    ,P.EndDate as BuildingDate
                    ,C.PurposeCode
                    ,C.BuildingTypeCode
                    ,case when P.EndDate < '1990-01-01' then 8004001 
				        when P.EndDate >= '1990-01-01' and P.EndDate < '1995-01-01' then 8004002
				        when P.EndDate >= '1995-01-01' and P.EndDate < '2000-01-01' then 8004003
				        when P.EndDate >= '2000-01-01' and P.EndDate < '2005-01-01' then 8004004
				        when P.EndDate >= '2005-01-01' and P.EndDate < '2010-01-01' then 8004005
				        when P.EndDate >= '2010-01-01' then 8004006
				        else '' end as BuildingDateCode
		        from (
			        select 
				        ProjectId,ProjectName,AreaID,SubAreaId,EndDate
			        from " + ptable + @" p with(nolock)
			        where not exists(
				        select ProjectId from " + ptable + @"_sub ps with(nolock)
				        where ps.ProjectId = p.ProjectId
				        and ps.CityID = @cityid 
				        and ps.Fxt_CompanyId = @fxtcompanyid
			        )
			        and p.Valid = 1
			        and p.CityID = @cityid
			        and p.FxtCompanyId in (" + showcomId + @")
			        union
			        select 
				        ProjectId,ProjectName,AreaID,SubAreaId,EndDate
			        from " + ptable + @"_sub p with(nolock)
			        where p.Valid = 1
			        and p.CityID = @cityid
			        and p.Fxt_CompanyId = @fxtcompanyid 
		        )P
		        left join(
			        select * from " + ctable + @" c with(nolock)
			        where c.Valid = 1
			        and c.CityID = @cityid
			        and c.FXTCompanyId in (" + casecomId + @")
			        and c.CaseDate between @datefrom and @dateto " + cwhere + @"
		        )C on P.ProjectId = C.ProjectId"
                + sampleprojectwhere + @"
	        )T
	        where 1 = 1 " + where;

            //自关联，获取涨跌幅
            string sql1 = @"
        select * from (
		    select
			    C.CaseID
			    ,C.CaseDate
			    ,C.CaseTypeCode
			    ,C.BuildingArea
			    ,C.UnitPrice
			    ,P.ProjectId
			    ,P.ProjectName
			    ,P.AreaID
			    ,case when P.SubAreaId > 0 then P.SubAreaId else 0 end as SubAreaId
			    ,CONVERT(varchar(7), C.CaseDate, 120) + '-01' as casemonth
                ,P.EndDate as BuildingDate
                ,C.PurposeCode
                ,C.BuildingTypeCode
                ,case when P.EndDate < '1990-01-01' then 8004001 
				    when P.EndDate >= '1990-01-01' and P.EndDate < '1995-01-01' then 8004002
				    when P.EndDate >= '1995-01-01' and P.EndDate < '2000-01-01' then 8004003
				    when P.EndDate >= '2000-01-01' and P.EndDate < '2005-01-01' then 8004004
				    when P.EndDate >= '2005-01-01' and P.EndDate < '2010-01-01' then 8004005
				    when P.EndDate >= '2010-01-01' then 8004006
				    else '' end as BuildingDateCode
		    from (
			    select 
				    ProjectId,ProjectName,AreaID,SubAreaId,EndDate
			    from " + ptable + @" p with(nolock)
			    where not exists(
				    select ProjectId from " + ptable + @"_sub ps with(nolock)
				    where ps.ProjectId = p.ProjectId
				    and ps.CityID = @cityid 
				    and ps.Fxt_CompanyId = @fxtcompanyid
			    )
			    and p.Valid = 1
			    and p.CityID = @cityid
			    and p.FxtCompanyId in (" + showcomId + @") 
			    union
			    select 
				    ProjectId,ProjectName,AreaID,SubAreaId,EndDate
			    from " + ptable + @"_sub p with(nolock)
			    where p.Valid = 1
			    and p.CityID = @cityid
			    and p.Fxt_CompanyId = @fxtcompanyid 
		    )P
		    left join(
			    select * from " + ctable + @" c with(nolock)
			    where c.Valid = 1
			    and c.CityID = @cityid
			    and c.FXTCompanyId in (" + casecomId + @")
			    and c.CaseDate between DATEADD(mm,-1,@datefrom) and @dateto " + cwhere + @"
		    )C on P.ProjectId = C.ProjectId"
                + sampleprojectwhere + @"
	    )T
	    where 1 = 1 " + where;
            #endregion

            #region 默认
            if (string.IsNullOrWhiteSpace(cycle) && string.IsNullOrWhiteSpace(area))  //默认
            {
                sql = @"
select 
	'" + date + @"' as [案例时间]
	--,'' as [行政区]
	--,'' as [片区]
    --,0 as ProjectId
	--,'' as [楼盘名称]
    --,'' as [建筑年代]
	,num as [案例总数]
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as [均价]
    --,'' as [涨跌幅]
from (
	select 
		COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql + @"
	)T
)T";
                using (IDbConnection conn = DapperAdapter.OpenConnection())
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    //avePrice = conn.Query<ProjectCase_AvePrice>(sql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTable(sql, param);
                }
            }
            #endregion
            #region 按月份
            if ((cycle == "month") && string.IsNullOrWhiteSpace(area)) //按月份
            {
                sql = @"
select 
	ISNULL(casemonth,'') as casedate
	,'' as AreaName
	,'' as SubAreaName
    ,0 as ProjectId
	,'' as ProjectName
	,'' as BuildingDateName
	,num
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
from (
	select 
		casemonth
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql + @" and CaseID is not null
	)T
	group by casemonth with cube
)T";
                sql1 = @"
select 
	ISNULL(casemonth,'') as casedate
	,'' as AreaName
	,'' as SubAreaName
	,'' as ProjectName
	,'' as BuildingDateName
	,num
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
from (
	select 
		casemonth
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql1 + @" and CaseID is not null
	)T
	group by casemonth with cube
)T";
                string strsql = @"
select
	case when T1.casedate = '' then '合计' else T1.casedate end as [案例时间]
	,T1.AreaName as [行政区]
	,T1.SubAreaName as [片区]
	,T1.ProjectName as [楼盘名称]
	,T1.BuildingDateName as [建筑年代]
	,T1.num as [案例总数]
	,T1.avgprice as [均价]
	,case when T2.avgprice > 0 then CONVERT(numeric(18,2),(CONVERT(numeric(18,0),T1.avgprice)-convert(numeric(18,0),T2.avgprice))/convert(numeric(18,0),T2.avgprice) * 100) else 0 end as [涨跌幅]
from (" + sql + @"
)T1
left join (" + sql1 + @"
)T2 on T1.casedate = DATEADD(MM,1,T2.casedate)
order by case when T1.casedate = '' then 0 else 1 end";
                using (IDbConnection conn = DapperAdapter.OpenConnection())
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    //avePrice = conn.Query<ProjectCase_AvePrice>(strsql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTable(strsql, param);
                }
            }
            #endregion
            #region 按行政区
            if (string.IsNullOrWhiteSpace(cycle) && area == "area") //按行政区
            {
                sql = @"
select 
	'" + date + @"' as [案例时间]
	,ISNULL(AreaName,'合计') as [行政区]
	--,'' as [片区]
    --,0 as ProjectId
	--,'' as [楼盘名称]
    --,'' as [建筑年代]
	,num as [案例总数]
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as [均价]
    --,'' as [涨跌幅]
from (
	select 
		AreaID
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql + @" and CaseID is not null
	)T
	group by AreaID with cube
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
order by case when T.AreaID is null then 0 else 1 end";
                using (IDbConnection conn = DapperAdapter.OpenConnection())
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd");
                    //avePrice = conn.Query<ProjectCase_AvePrice>(sql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTable(sql, param);
                }
            }
            #endregion
            #region 按片区
            if (string.IsNullOrWhiteSpace(cycle) && area == "subarea") //按片区
            {
                sql = @"
select 
	'" + date + @"' as [案例时间]
	,ISNULL(AreaName,'') as [行政区]
	,case when AreaName is null then '合计' when AreaName is not null and SubAreaName is null then '' else SubAreaName end as [片区]
    --,0 as ProjectId
	--,'' as [楼盘名称]
    --,'' as [建筑年代]
	,num as [案例总数]
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as [均价]
    --,'' as [涨跌幅]
from (
	select 
		AreaID
		,SubAreaId
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql + @" and CaseID is not null
	)T
	group by AreaID,SubAreaId with cube
	having (GROUPING(AreaID) = 0 and GROUPING(SubAreaId) = 0) or (GROUPING(AreaID) = 1 and GROUPING(SubAreaId) = 1)
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea sa with(nolock) on T.SubAreaId = sa.SubAreaId
order by case when T.AreaID is null and T.SubAreaId is null then 0 else 1 end";
                using (IDbConnection conn = DapperAdapter.OpenConnection())
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    //avePrice = conn.Query<ProjectCase_AvePrice>(sql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTable(sql, param);
                }
            }
            #endregion
            #region 按楼盘
            if (string.IsNullOrWhiteSpace(cycle) && area == "project") //按楼盘
            {
                sql = @"
select 
	'" + date + @"' as [案例时间]
	,ISNULL(AreaName,'') as [行政区]
	,case when AreaName is null then '' when AreaName is not null and SubAreaName is null then '' else SubAreaName end as [片区]
    ,ProjectId
	,ISNULL(ProjectName,'合计') as [楼盘名称]
	,ISNULL(c.CodeName,'') as [建筑年代]
	,num as [案例总数]
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as [均价]
    --,'' as [涨跌幅]
from (
	select 
		AreaID
		,SubAreaId
		,ProjectId
		,ProjectName
		,BuildingDateCode
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql + @"
	)T
	group by AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode with cube
	having (GROUPING(AreaID) = 0 and GROUPING(SubAreaId) = 0 and GROUPING(ProjectId) = 0 and GROUPING(ProjectName) = 0 and GROUPING(BuildingDateCode) = 0) or 
	(GROUPING(AreaID) = 1 and GROUPING(SubAreaId) = 1 and GROUPING(ProjectId) = 1 and GROUPING(ProjectName) = 1 and GROUPING(BuildingDateCode) = 1)
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea sa with(nolock) on T.SubAreaId = sa.SubAreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on T.BuildingDateCode = c.Code
order by case when T.AreaID is null and T.SubAreaId is null and T.ProjectId is null then 0 when T.AreaID is null and T.SubAreaId is null then 2 else 1 end";
                using (IDbConnection conn = DapperAdapter.OpenConnection())
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    //avePrice = conn.Query<ProjectCase_AvePrice>(sql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTable(sql, param);
                }
            }
            #endregion
            #region 按月、行政区
            if ((cycle == "month") && area == "area") //按月，行政区
            {
                sql = @"
select 
	ISNULL(casemonth,'') as casedate
	,ISNULL(AreaName,'合计') as AreaName
	,'' as SubAreaName
    ,0 as ProjectId
	,'' as ProjectName
	,'' as BuildingDateName
	,num
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
from (
	select 
		casemonth
		,AreaID
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql + @" and CaseID is not null
	)T
	group by casemonth,AreaID with cube
	having (GROUPING(casemonth) = 0 and GROUPING(AreaID) = 0) or (GROUPING(casemonth) = 1 and GROUPING(AreaID) = 1)
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId";

                sql1 = @"
select 
	ISNULL(casemonth,'') as casedate
	,ISNULL(AreaName,'合计') as AreaName
	,'' as SubAreaName
	,'' as ProjectName
	,'' as BuildingDateName
	,num
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
from (
	select 
		casemonth
		,AreaID
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql1 + @" and CaseID is not null
	)T
	group by casemonth,AreaID with cube
	having (GROUPING(casemonth) = 0 and GROUPING(AreaID) = 0) or (GROUPING(casemonth) = 1 and GROUPING(AreaID) = 1)
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId";

                string strsql = @"
select
	T1.casedate as [案例时间]
	,T1.AreaName as [行政区]
	,T1.SubAreaName as [片区]
	,T1.ProjectName as [楼盘名称]
	,T1.BuildingDateName as [建筑年代]
	,T1.num as [案例总数]
	,T1.avgprice as [均价]
	,case when T2.avgprice > 0 then CONVERT(numeric(18,2),(CONVERT(numeric(18,0),T1.avgprice)-convert(numeric(18,0),T2.avgprice))/convert(numeric(18,0),T2.avgprice) * 100) else 0 end as [涨跌幅]
from (" + sql + @"
)T1
left join (" + sql1 + @"
)T2 on T1.AreaName = T2.AreaName and T1.casedate = DATEADD(mm,1,T2.casedate)
order by [案例时间],case when T1.AreaName = '合计' then 1 else 0 end";
                using (IDbConnection conn = DapperAdapter.OpenConnection())
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd");
                    //avePrice = conn.Query<ProjectCase_AvePrice>(strsql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTable(strsql, param);
                }
            }
            #endregion
            #region 按月、片区
            if ((cycle == "month") && area == "subarea") //按月，片区
            {
                sql = @"
select 
	ISNULL(casemonth,'') as casedate
	,ISNULL(AreaName,'') as AreaName
	,case when AreaName is null then '合计' when AreaName is not null and SubAreaName is null then '' else SubAreaName end as SubAreaName
    ,0 as ProjectId
	,'' as ProjectName
    ,'' as BuildingDateName
	,num
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
from (
	select 
		casemonth
		,AreaID
		,SubAreaId
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql + @" and CaseID is not null
	)T
	group by casemonth,AreaID,SubAreaId with cube
	having (GROUPING(casemonth) = 0 and GROUPING(AreaID) = 0 and GROUPING(SubAreaId) = 0) or (GROUPING(casemonth) = 1 and GROUPING(AreaID) = 1 and GROUPING(SubAreaId) = 1)
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea sa with(nolock) on T.SubAreaId = sa.SubAreaId";
                sql1 = @"
select 
	ISNULL(casemonth,'') as casedate
	,ISNULL(AreaName,'') as AreaName
	,case when AreaName is null then '合计' when AreaName is not null and SubAreaName is null then '' else SubAreaName end as SubAreaName
	,'' as ProjectName
    ,'' as BuildingDateName
	,num
	,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
from (
	select 
		casemonth
		,AreaID
		,SubAreaId
		,COUNT(CaseID) as num
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingarea
	from (" + sql1 + @" and CaseID is not null
	)T
	group by casemonth,AreaID,SubAreaId with cube
	having (GROUPING(casemonth) = 0 and GROUPING(AreaID) = 0 and GROUPING(SubAreaId) = 0) or (GROUPING(casemonth) = 1 and GROUPING(AreaID) = 1 and GROUPING(SubAreaId) = 1)
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea sa with(nolock) on T.SubAreaId = sa.SubAreaId";

                string strsql = @"
select
	T1.casedate as [案例时间]
	,T1.AreaName as [行政区]
	,T1.SubAreaName as [片区]
	,T1.ProjectName as [楼盘名称]
	,T1.BuildingDateName as [建筑年代]
	,T1.num as [案例总数]
	,T1.avgprice as [均价]
	,case when T2.avgprice > 0 then CONVERT(numeric(18,2),(CONVERT(numeric(18,0),T1.avgprice)-convert(numeric(18,0),T2.avgprice))/convert(numeric(18,0),T2.avgprice) * 100) else 0 end as [涨跌幅]
from (" + sql + @"
)T1
left join (" + sql1 + @"
)T2 on T1.AreaName = T2.AreaName and T1.SubAreaName = T2.SubAreaName and T1.casedate = DATEADD(MM,1,T2.casedate)
order by [案例时间],[行政区],case when T1.SubAreaName = '合计' then 1 else 0 end";
                using (IDbConnection conn = DapperAdapter.OpenConnection())
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd");
                    //avePrice = conn.Query<ProjectCase_AvePrice>(strsql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTable(strsql, param);
                }
            }
            #endregion
            #region 按月、楼盘
            if ((cycle == "month") && area == "project") //按月，楼盘
            {
                //                string strsql = @"
                //declare @date1 varchar(10),
                //        @date2 varchar(10)
                //set @date1 = DATEADD(mm,-1,@datefrom)
                //set @date2 = @dateto
                //select convert(varchar(7),cast(ltrim(year(@date1)) + '-'+ ltrim(number) + '-01' as datetime),120) + '-01' as casemonth
                //into #monthtemp
                //from master..spt_values where type = 'p' and number between month(@date1) and month(@date2)
                //
                //select * into #projecttemp
                //from (
                //    select 
                //	    AreaId,SubAreaId,ProjectId,ProjectName from " + ptable + @" p with(nolock)
                //    where not exists(
                //	    select ProjectId from " + ptable + @"_sub ps with(nolock)
                //	    where ps.ProjectId = p.ProjectId
                //	    and ps.CityID = @cityid 
                //	    and ps.Fxt_CompanyId = @fxtcompanyid
                //    )
                //    and p.Valid = 1
                //    and p.CityID = @cityid
                //    and p.FxtCompanyId in (" + showcomId + @") 
                //    union
                //    select 
                //	    AreaId,SubAreaId,ProjectId,ProjectName from " + ptable + @"_sub p with(nolock)
                //    where p.Valid = 1
                //    and p.CityID = @cityid
                //    and p.Fxt_CompanyId = @fxtcompanyid 
                //)T
                //
                //select T.casemonth,T.AreaID,T.SubAreaId,T.ProjectId,T.ProjectName,T1.BuildingDateCode,T1.num,T1.totalprice,T1.totalbuildingarea,T1.avgprice
                //into #projectcasetemp
                //from (select * from #projecttemp p left join #monthtemp on 1 = 1)T
                //left join (
                //	select 
                //		casemonth,AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode
                //		,num
                //		,totalprice
                //		,totalbuildingarea
                //		,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
                //	from (
                //		select
                //			ROW_NUMBER() over(partition by casemonth,AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode order by casemonth,AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode) as rank1
                //			,casemonth,AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode
                //			,COUNT(CaseID) over(partition by casemonth,AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode) as num
                //			,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) over(partition by casemonth,AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode) as totalprice
                //			,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) over(partition by casemonth,AreaID,SubAreaId,ProjectId,ProjectName,BuildingDateCode) as totalbuildingarea
                //		from (" + sql + @" and casemonth is not null
                //		)T1
                //	)T2
                //	where rank1 = 1
                //)T1 on T.casemonth = T1.casemonth and T.ProjectId = T1.ProjectId
                //
                //select
                //	casemonth as CaseDate
                //	,ISNULL((select top 1 AreaName from FxtDataCenter.dbo.SYS_Area where AreaID = T.AreaID),'') as AreaName
                //	,ISNULL((select top 1 SubAreaName from FxtDataCenter.dbo.SYS_SubArea where SubAreaId = T.SubAreaId),'') as SubAreaName
                //	,ProjectId
                //	,ProjectName
                //    ,(select top 1 CodeName from FxtDataCenter.dbo.SYS_Code where Code = T.BuildingDateCode) as BuildingDateName
                //	,num
                //	,avgprice
                //	,PricePercent
                //from (
                //	select
                //		'' as casemonth
                //		,0 as AreaID
                //		,0 as SubAreaId
                //		,0 as ProjectId
                //		,'合计' as ProjectName
                //        ,0 as BuildingDateCode
                //		,num
                //		,totalprice
                //		,totalbuildingarea
                //		,case when totalbuildingarea > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingarea) else 0 end as avgprice
                //		,0 as PricePercent
                //	from (
                //		select
                //			SUM(num) as num
                //			,SUM(totalprice) as totalprice
                //			,SUM(totalbuildingarea) as totalbuildingarea
                //		from (
                //			select
                //				T1.*
                //				,case when T2.avgprice > 0 then CONVERT(numeric(18,2),(CONVERT(numeric(18,0),T1.avgprice)-convert(numeric(18,0),T2.avgprice))/convert(numeric(18,0),T2.avgprice) * 100) else 0 end as PricePercent
                //			from (
                //				select * from #projectcasetemp
                //				where casemonth between CONVERT(varchar(7), @datefrom, 120) + '-01'
                //					and CONVERT(varchar(7), @dateto, 120) + '-01'
                //			) T1
                //			left join #projectcasetemp T2 on T1.AreaID = T2.AreaID and T1.SubAreaId = T2.SubAreaId and T1.ProjectId = T2.ProjectId and T1.casemonth = DATEADD(MM,1,T2.casemonth)
                //		)T
                //	)T
                //	union
                //	select
                //		T1.*
                //		,case when T2.avgprice > 0 then CONVERT(numeric(18,2),(CONVERT(numeric(18,0),T1.avgprice)-convert(numeric(18,0),T2.avgprice))/convert(numeric(18,0),T2.avgprice) * 100) else 0 end as PricePercent
                //	from (
                //		select * from #projectcasetemp
                //		where casemonth between CONVERT(varchar(7), @datefrom, 120) + '-01'
                //			and CONVERT(varchar(7), @dateto, 120) + '-01'
                //	) T1
                //	left join #projectcasetemp T2 on T1.AreaID = T2.AreaID and T1.SubAreaId = T2.SubAreaId and T1.ProjectId = T2.ProjectId and T1.casemonth = DATEADD(MM,1,T2.casemonth)
                //)T
                //order by casemonth,AreaID,SubAreaId,ProjectName
                //
                //drop table #monthtemp
                //drop table #projecttemp
                //drop table #projectcasetemp
                //";
                using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    //var datefrom = parameters.timeFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                    //var dateto = parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    //var datefrom = parameters.timeFrom.Value.Year + "-" + parameters.timeFrom.Value.Month + "-" + parameters.timeFrom.Value.Day;
                    //var dateto = parameters.timeTo.Value.Year + "-" + parameters.timeTo.Value.Month + "-" + parameters.timeTo.Value.Day;
                    //avePrice = conn.Query<ProjectCase_AvePrice>(strsql, new { cityid = parameters.cityid, fxtcompanyid = parameters.fxtcompanyid, datefrom = datefrom, dateto = dateto }).AsQueryable();

                    string strsql = "[dbo].[procGetProjectCaseMonthPrice]";
                    paramet.Add(new SqlParameter("@cityid", parameters.cityid));
                    paramet.Add(new SqlParameter("@fxtcompanyid", parameters.fxtcompanyid));
                    paramet.Add(new SqlParameter("@datefrom", parameters.timeFrom.Value.AddMonths(-1).ToString("yyyy-MM-dd") + " 00:00:00"));
                    paramet.Add(new SqlParameter("@dateto", parameters.timeTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"));
                    paramet.Add(new SqlParameter("@where", where));
                    paramet.Add(new SqlParameter("@cwhere", cwhere));
                    paramet.Add(new SqlParameter("@sampleprojectwhere", sampleprojectwhere));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    avePrice = DBHelperSql.ExecuteDataTableProc(strsql, param);
                }
            }
            #endregion

            //var avePriceList = avePrice.ToList();
            //return avePriceList;
            return avePrice;
        }

        #endregion

        #region 增加
        public int AddProjectCase(DAT_Case modal)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(modal.cityid ?? -1, modal.fxtcompanyid, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            string strSql = @"insert into  " + ctable + @" (projectid,buildingid,houseid,companyid,casedate,purposecode,floornumber,buildingname,houseno,buildingarea,usablearea,frontcode,unitprice,moneyunitcode,sightcode,casetypecode,structurecode,buildingtypecode,housetypecode,createdate,creator,remark,totalprice,oldid,cityid,fxtcompanyid,totalfloor,remainyear,depreciation,fitmentcode,surveyid,savedatetime,saveuser,sourcename,sourcelink,sourcephone,areaid,areaname,buildingdate,zhuangxiu,subhouse,peitao) 
values(@projectid,@buildingid,@houseid,@companyid,@casedate,@purposecode,@floornumber,@buildingname,@houseno,@buildingarea,@usablearea,@frontcode,@unitprice,@moneyunitcode,@sightcode,@casetypecode,@structurecode,@buildingtypecode,@housetypecode,@createdate,@creator,@remark,@totalprice,@oldid,@cityid,@fxtcompanyid,@totalfloor,@remainyear,@depreciation,@fitmentcode,@surveyid,@savedatetime,@saveuser,@sourcename,@sourcelink,@sourcephone,@areaid,@areaname,@buildingdate,@zhuangxiu,@subhouse,@peitao)";

            //SqlParameter[] parameters;
            //BindParams(modal, out parameters);

            //DBHelperSql.ConnectionString = ConfigurationHelper.FXTProject;
            //return DBHelperSql.ExecuteNonQuery(strSql, parameters);

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, modal);
            }
        }

        public int AddProjectCase(DAT_CaseTemp[] caseTemp)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(caseTemp[0].CityID, caseTemp[0].FXTCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            string strSql = @"insert into  " + ctable + @" (projectid,buildingid,houseid,companyid,casedate,purposecode,floornumber,buildingname,houseno,buildingarea,usablearea,frontcode,unitprice,moneyunitcode,sightcode,casetypecode,structurecode,buildingtypecode,housetypecode,createdate,creator,remark,totalprice,oldid,cityid,fxtcompanyid,totalfloor,remainyear,depreciation,fitmentcode,surveyid,sourcename,sourcelink,sourcephone,areaid,areaname,buildingdate,zhuangxiu,subhouse,peitao) 
values(@projectid,@buildingid,@houseid,@companyid,@casedate,@purposecode,@floornumber,@buildingname,@houseno,@buildingarea,@usablearea,@frontcode,@unitprice,@moneyunitcode,@sightcode,@casetypecode,@structurecode,@buildingtypecode,@housetypecode,GETDATE(),@creator,@remark,@totalprice,@oldid,@cityid,@fxtcompanyid,@totalfloor,@remainyear,@depreciation,@fitmentcode,@surveyid,@sourcename,@sourcelink,@sourcephone,@areaid,@areaname,@buildingdate,@zhuangxiu,@subhouse,@peitao)";

            //            string strSql =string.Format(@"insert into  " + ctable + @" (projectid,buildingid,houseid,companyid,casedate,purposecode,floornumber,buildingname,houseno,buildingarea,usablearea,frontcode,unitprice,moneyunitcode,sightcode,casetypecode,structurecode,buildingtypecode,housetypecode,createdate,creator,remark,totalprice,oldid,cityid,fxtcompanyid,totalfloor,remainyear,depreciation,fitmentcode,surveyid,savedatetime,saveuser,sourcename,sourcelink,sourcephone,areaid,areaname,buildingdate,zhuangxiu,subhouse,peitao) 
            //select projectid,buildingid,houseid,companyid,casedate,purposecode,floornumber,buildingname,houseno,buildingarea,usablearea,frontcode,unitprice,moneyunitcode,sightcode,casetypecode,structurecode,buildingtypecode,housetypecode,createdate,creator,remark,totalprice,oldid,cityid,fxtcompanyid,totalfloor,remainyear,depreciation,fitmentcode,surveyid,getdate() as savedatetime,saveuser,sourcename,sourcelink,sourcephone,areaid,areaname,buildingdate,zhuangxiu,subhouse,peitao
            //from FxtDataCenter.dbo.DAT_CaseTemp with(nolock) 
            //where projectid = {0}
            //and cityid = {1}
            //and fxtcompanyid = {2}
            //and taskId in (
            //	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
            //	where NameErrNumber > 0
            //	and CityID = {1}
            //	and FXTCompanyId = {2})",projectid,cityid,fxtcompanyid);


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, caseTemp);
            }

        }
        #endregion

        #region 修改
        public int UpdateProjectCase(DAT_Case projectCase)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(projectCase.cityid ?? -1, projectCase.fxtcompanyid, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            var strSql = @"update  " + ctable + @" with(rowlock)  set projectid = @projectid,buildingid = @buildingid,houseid = @houseid,companyid = @companyid,casedate = @casedate,purposecode = @purposecode,floornumber = @floornumber,buildingname = @buildingname,houseno = @houseno,buildingarea = @buildingarea,usablearea = @usablearea,frontcode = @frontcode,unitprice = @unitprice,moneyunitcode = @moneyunitcode,sightcode = @sightcode,casetypecode = @casetypecode,structurecode = @structurecode,buildingtypecode = @buildingtypecode,housetypecode = @housetypecode,remark = @remark,totalprice = @totalprice,totalfloor = @totalfloor,remainyear = @remainyear,depreciation = @depreciation,fitmentcode = @fitmentcode,surveyid = @surveyid,savedatetime = @savedatetime,saveuser = @saveuser,sourcename = @sourcename,sourcelink = @sourcelink,sourcephone = @sourcephone,areaid = @areaid,areaname = @areaname,buildingdate = @buildingdate,zhuangxiu = @zhuangxiu,subhouse = @subhouse,peitao = @peitao
where caseid = @caseid";

            //SqlParameter[] parameters;
            //BindParams(projectCase, out parameters);
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, projectCase);
            }

            //DBHelperSql.ConnectionString = ConfigurationHelper.FXTProject;
            //return DBHelperSql.ExecuteNonQuery(strSql, parameters);

        }
        #endregion

        #region 删除
        public int DeleteProjectCase(int caseId, int cityId, int fxtCompanyId, string saveusername)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            var strSql = "update  " + ctable + @" with(rowlock) set valid = 0,SaveDateTime = GETDATE(),SaveUser = @saveuser where caseid = @caseid and fxtcompanyid = @fxtcompanyid";

            SqlParameter[] parameter = { 
                                           new SqlParameter("@caseid",SqlDbType.Int),
                                           new SqlParameter("@saveuser",SqlDbType.NVarChar),
                                           new SqlParameter("@fxtcompanyid",SqlDbType.Int)
                                       };
            parameter[0].Value = caseId;
            parameter[1].Value = saveusername;
            parameter[2].Value = fxtCompanyId;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
            return DBHelperSql.ExecuteNonQuery(strSql, parameter);
        }

        public int DeleteExProjectCase(ExceptionCaseParams ec)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(ec.cityId, ec.fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ptable)) return 0;
            if (string.IsNullOrWhiteSpace(ctable)) return 0;

            var projectsqlWhere = string.Empty;
            var casesqlWhere = string.Empty;
            if (ec.ProjectId > 0)
            {
                projectsqlWhere += " and ProjectId = @ProjectId";
                casesqlWhere += " and ProjectId = @ProjectId";
            }
            if (ec.AreaId != -1) projectsqlWhere += " and AreaId = @areaId ";
            if (ec.CaseDateFrom != null) casesqlWhere += " and CaseDate >=@caseDateFrom ";
            if (ec.CaseDateTo != null) casesqlWhere += " and CaseDate <=@caseDateTo ";
            if (ec.PurposeCode != -1) casesqlWhere += " and PurposeCode = @purposeCode ";
            if (ec.BuildingAreaFrom != null) casesqlWhere += " and BuildingArea >= @buildingAreaFrom ";
            if (ec.BuildingAreaTo != null) casesqlWhere += " and BuildingArea <= @buildingAreaTo ";
            if (ec.BuildingTypeCode != -1) casesqlWhere += " and BuildingTypeCode = @buildingTypeCode ";
            if (ec.HouseTypeCode != -1) casesqlWhere += " and HouseTypeCode = @houseTypeCode ";
            if (ec.FrontCode != -1) casesqlWhere += " and FrontCode = @frontCode ";
            if (!string.IsNullOrWhiteSpace(ec.BuildingDate)) casesqlWhere += " and BuildingDate = @buildingDate ";
            if (!string.IsNullOrWhiteSpace(ec.Zhuangxiu)) casesqlWhere += " and ZhuangXiu = @zhuangxiu ";

            //整个城市案例均价的计算
            //            string avgSql = @"
            //select
            //	(case when totalBuildingArea > 0 then totalPrice / totalBuildingArea else 0 end) as unitprice
            //from (
            //	select 
            //		sum(Case when UnitPrice > 0 and BuildingArea > 0 then UnitPrice * BuildingArea else 0 end) as totalPrice
            //		,sum(Case when UnitPrice > 0 and BuildingArea > 0 then BuildingArea else 0 end) as totalBuildingArea
            //	from " + ctable + @" WITH (NOLOCK)
            //	where cityId = @cityId AND fxtCompanyId = @fxtCompanyId and CaseTypeCode in (3001001,3001002,3001003,3001004,3001005) " + sqlWhere + @"
            //)T";
            //            string exSql = @"
            //select CaseId,(CASE @AvgPrice WHEN 0 THEN 0 ELSE (UnitPrice - @AvgPrice) / @AvgPrice END) AS Rate
            //from fxtproject.dbo.DAT_Case_xb WITH (NOLOCK)
            //where cityId = @cityId AND fxtCompanyId = @fxtCompanyId and CaseTypeCode in (3001001,3001002,3001003,3001004,3001005) " + sqlWhere;

            //分楼盘案例均价计算
            string strsql = @"
select
	CaseID
	,case when avgPrice = 0 then 0 else (UnitPrice - avgPrice) / avgPrice end as rate
from (
	select
		c.ProjectId,c.CaseID,c.UnitPrice,u.unitprice as avgPrice
	from (
		SELECT P.*,C.CaseID,C.BuildingArea,C.UnitPrice
		FROM (
			SELECT ProjectId,AreaID
			FROM " + ptable + @" p WITH (NOLOCK)
			WHERE NOT EXISTS (
					SELECT ProjectId
					FROM " + ptable + @"_sub ps WITH (NOLOCK)
					WHERE ps.ProjectId = p.ProjectId
						AND ps.CityID = @cityid
						AND ps.Fxt_CompanyId = @fxtcompanyid
					)
				AND p.Valid = 1
				AND p.CityID = @cityid
				AND p.FxtCompanyId IN (" + showcomId + @") " + projectsqlWhere + @"
			UNION			
			SELECT ProjectId,AreaID
			FROM " + ptable + @"_sub p WITH (NOLOCK)
			WHERE p.Valid = 1
				AND p.CityID = @cityid
				AND p.Fxt_CompanyId = @fxtcompanyid " + projectsqlWhere + @"
			) P
		inner JOIN (
			SELECT ProjectId,CaseID,UnitPrice,BuildingArea
			FROM " + ctable + @" c WITH (NOLOCK)
			WHERE c.Valid = 1
				AND c.CityID = @cityid
				AND c.FXTCompanyId IN (" + casecomId + @")
				AND CaseTypeCode IN (3001001,3001002,3001003,3001004,3001005)
				and PurposeCode in (1002001,1002002,1002003,1002012,1002013,1002021,1002023)
				AND c.CaseDate BETWEEN @caseDateFrom AND @caseDateTo " + casesqlWhere + @"
			) C ON P.ProjectId = C.ProjectId 
	)c left join (
		SELECT ProjectId,UnitPrice FROM ( 
			SELECT 
				((MAX(rank1) OVER ( PARTITION BY ProjectId )) + 1) / 2 AS MiddleRowIndex
				 ,*
			FROM ( 
				select
					ROW_NUMBER() over(partition by ProjectId,AreaID order by ProjectId,AreaID,UnitPrice desc) as rank1
					,*
				from (
					SELECT P.*,C.CaseID,C.BuildingArea,C.UnitPrice
					FROM (
						SELECT ProjectId,AreaID
						FROM " + ptable + @" p WITH (NOLOCK)
						WHERE NOT EXISTS (
								SELECT ProjectId
								FROM " + ptable + @"_sub ps WITH (NOLOCK)
								WHERE ps.ProjectId = p.ProjectId
									AND ps.CityID = @cityid
									AND ps.Fxt_CompanyId = @fxtcompanyid
								)
							AND p.Valid = 1
							AND p.CityID = @cityid
							AND p.FxtCompanyId IN (" + showcomId + @")	" + projectsqlWhere + @"	
						UNION					
						SELECT ProjectId,AreaID
						FROM " + ptable + @"_sub p WITH (NOLOCK)
						WHERE p.Valid = 1
							AND p.CityID = @cityid
							AND p.Fxt_CompanyId = @fxtcompanyid " + projectsqlWhere + @"
						) P
					inner JOIN (
						SELECT ProjectId,CaseID,UnitPrice,BuildingArea
						FROM " + ctable + @" c WITH (NOLOCK)
						WHERE c.Valid = 1
							AND c.CityID = @cityid
							AND c.FXTCompanyId IN (" + casecomId + @")
							AND c.CaseTypeCode IN (3001001,3001002,3001003,3001004,3001005)
							and c.PurposeCode in (1002001,1002002,1002003,1002012,1002013,1002021,1002023)
							AND c.CaseDate BETWEEN @caseDateFrom AND @caseDateTo " + casesqlWhere + @"
						) C ON P.ProjectId = C.ProjectId
				)r
			) AS T
		) AS T
		WHERE   T.rank1 = T.MiddleRowIndex
	) u
	on c.ProjectId = u.ProjectId
)T";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                //var avgPrice = conn.Query<decimal>(avgSql, ec).FirstOrDefault();
                //ec.AvgPrice = avgPrice;
                //var result = conn.Query<ExCase>(exSql + sqlWhere, ec);

                var result = conn.Query<ExCase>(strsql, ec);

                int count = 0;
                foreach (var item in result.Where(item => item.Rate > ec.Uprate || item.Rate < ec.Downrate))
                {
                    count += DeleteProjectCase(item.CaseId, ec.cityId, ec.fxtCompanyId, ec.SaveUserName);
                }
                return count;
            }
        }

        public int DeleteSameProjectCaseCount(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            var dateFrom = caseDateFrom.ToString("yyyy-MM-dd") + " 00:00:00";
            var dateTo = caseDateTo.ToString("yyyy-MM-dd") + " 23:59:59";

            var strSql = @"
select COUNT(*) from (
	select
		ROW_NUMBER() over(partition by areaid,projectid,buildingarea,unitprice,purposecode,casetypecode,FloorNumber,TotalFloor,FrontCode,BuildingTypeCode,HouseTypeCode order by caseid desc) as rank1
		,areaid,projectid,buildingarea,unitprice,purposecode,casetypecode,FloorNumber,TotalFloor,FrontCode,BuildingTypeCode,HouseTypeCode
	from (
		select
            CaseID
            ,AreaId
            ,ProjectId
            ,BuildingId
            ,HouseId
            ,CaseDate
            ,PurposeCode
            ,FloorNumber
            ,TotalFloor
            ,BuildingDate
            ,BuildingArea
            ,(case when FrontCode = -1 then null else FrontCode end) as FrontCode
            ,(case when HouseTypeCode = -1 then null else HouseTypeCode end) as HouseTypeCode
            ,UnitPrice
            ,CaseTypeCode
            ,(case when BuildingTypeCode = -1 then null else BuildingTypeCode end) as BuildingTypeCode
            ,Valid
            ,FXTCompanyId
        from " + ctable + @"
		where CityID = " + cityId + @"
		and FXTCompanyId = " + fxtCompanyId + @"
		and Valid = 1
		and CaseDate between '" + dateFrom + @"' and  '" + dateTo + @"'
	)T
)T1
where rank1 <> 1";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = strSql;
                int result = (int)cmd.ExecuteScalar();
                return result;
            }
        }

        public int DeleteSameProjectCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            var dateFrom = caseDateFrom.ToString("yyyy-MM-dd") + " 00:00:00";
            var dateTo = caseDateTo.ToString("yyyy-MM-dd") + " 23:59:59";

            var strSql = @"
update " + ctable + @" set Valid = 0,SaveDateTime = GETDATE(),SaveUser = '" + saveUser + @"'
where CaseID in(
	select CaseID from (
		select
			ROW_NUMBER() over(partition by areaid,projectid,buildingarea,unitprice,purposecode,casetypecode,FloorNumber,TotalFloor,FrontCode,BuildingTypeCode,HouseTypeCode order by caseid desc) as rank1
			,CaseID,areaid,projectid,buildingarea,unitprice,purposecode,casetypecode,FloorNumber,TotalFloor,FrontCode,BuildingTypeCode,HouseTypeCode
		from (
			select
				CaseID
				,AreaId
				,ProjectId
				,BuildingId
				,HouseId
				,CaseDate
				,PurposeCode
				,FloorNumber
				,TotalFloor
				,BuildingDate
				,BuildingArea
				,(case when FrontCode = -1 then null else FrontCode end) as FrontCode
				,(case when HouseTypeCode = -1 then null else HouseTypeCode end) as HouseTypeCode
				,UnitPrice
				,CaseTypeCode
				,(case when BuildingTypeCode = -1 then null else BuildingTypeCode end) as BuildingTypeCode
				,Valid
				,FXTCompanyId
			from " + ctable + @"
			where CityID = " + cityId + @"
			and FXTCompanyId = " + fxtCompanyId + @"
			and Valid = 1
			and CaseDate between '" + dateFrom + @"' and  '" + dateTo + @"'
		)T
	)T1
	where rank1 <> 1
)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, commandTimeout: 600);
            }
        }

        //一键删除，只能删除本评估机构案例数据
        public int GetProjectCaseAll(int CityId, int FxtCompanyId, string SaveUser, int areaid, DateTime casedateStart, DateTime casedateEnd, int caseTypeCode, decimal? buildingAreaFrom, decimal? buildingAreaTo, int purposeCode, decimal? unitPriceFrom, decimal? unitPriceTo, string key, int buildingTypeCode, bool self)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(CityId, FxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = FxtCompanyId.ToString();
            if (ptable == "" || ctable == "")
            {
                return 0;
            }
            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;
            var strWhere0 = string.Empty;
            if (areaid != -1 && areaid != 0)
                strWhere0 += " and p.AreaID = @areaid";
            if (!string.IsNullOrWhiteSpace(key))
                strWhere0 += " and p.ProjectName like '%'+@key+'%' ";
            if (purposeCode != -1)
                strWhere0 += " and c.PurposeCode = @purposeCode";
            if (caseTypeCode != -1)
                strWhere0 += " and c.CaseTypeCode =@caseTypeCode";
            if (buildingAreaFrom != null)
                strWhere0 += " and c.BuildingArea >= @buildingAreaFrom ";
            if (buildingAreaTo != null)
                strWhere0 += " and c.BuildingArea <=@buildingAreaTo  ";
            if (unitPriceFrom != null)
                strWhere0 += " and c.UnitPrice >= @unitPriceFrom  ";
            if (unitPriceTo != null)
                strWhere0 += " and c.UnitPrice <=  @unitPriceTo ";
            if (buildingTypeCode != -1)
                strWhere0 += " and c.BuildingTypeCode = @buildingTypeCode";
            if (self)
                strWhere0 += " and c.Creator = @SaveUser";

            string sql = @"
select 
	count(c.CaseID)
from (
	select 
    ProjectId,ProjectName,AreaID
	from " + ptable + @" p with(nolock)
	where not exists(
		select ProjectId from " + ptable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = @cityid
		and ps.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + showcomId + @")
	union
	select 
    ProjectId,ProjectName,AreaID
	from " + ptable + @"_sub ps with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
)P
inner join " + ctable + @" c with(nolock) on P.ProjectId = c.ProjectId 
where 1 = 1
and c.valid = 1
and c.CityID = @cityid
and c.FXTCompanyId = @fxtcompanyid
and c.CaseDate between @casedateStart and @casedateEnd " + strWhere0;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                int totalCount = conn.Query<int>(sql, new { CityId, FxtCompanyId, SaveUser, areaid, casedateStart, casedateEnd, caseTypeCode, buildingAreaFrom, buildingAreaTo, purposeCode, unitPriceFrom, unitPriceTo, key, buildingTypeCode }).FirstOrDefault();
                return totalCount;
            }
        }

        //一键删除，只能删除本评估机构案例数据
        public int DeleteProjectCaseAll(int CityId, int FxtCompanyId, string SaveUser, int areaid, DateTime casedateStart, DateTime casedateEnd, int caseTypeCode, decimal? buildingAreaFrom, decimal? buildingAreaTo, int purposeCode, decimal? unitPriceFrom, decimal? unitPriceTo, string key, int buildingTypeCode, bool self)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(CityId, FxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = FxtCompanyId.ToString();
            if (ptable == "" || ctable == "")
            {
                return 0;
            }

            var strWhere0 = string.Empty;
            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;

            if (areaid != -1 && areaid != 0)
                strWhere0 += " and p.AreaID = @areaid";
            if (!string.IsNullOrWhiteSpace(key))
                strWhere0 += " and p.ProjectName like '%'+@key+'%' ";
            if (purposeCode != -1)
                strWhere0 += " and c.PurposeCode = @purposeCode";
            if (caseTypeCode != -1)
                strWhere0 += " and c.CaseTypeCode =@caseTypeCode";
            if (buildingAreaFrom != null)
                strWhere0 += " and c.BuildingArea >= @buildingAreaFrom ";
            if (buildingAreaTo != null)
                strWhere0 += " and c.BuildingArea <=@buildingAreaTo  ";
            if (unitPriceFrom != null)
                strWhere0 += " and c.UnitPrice >= @unitPriceFrom  ";
            if (unitPriceTo != null)
                strWhere0 += " and c.UnitPrice <=  @unitPriceTo ";
            if (buildingTypeCode != -1)
                strWhere0 += " and c.BuildingTypeCode = @buildingTypeCode";
            if (self)
                strWhere0 += " and c.Creator = @SaveUser";

            string sql = @"
update " + ctable + @" set Valid = 0 ,SaveDateTime = GETDATE(),SaveUser = @SaveUser
where CaseID in (
	select c.CaseID
	from (
		select 
		ProjectId,ProjectName,AreaID
		from " + ptable + @" p with(nolock)
		where not exists(
			select ProjectId from " + ptable + @"_sub ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityID = @cityid
			and ps.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + showcomId + @")
		union
		select 
		ProjectId,ProjectName,AreaID
		from " + ptable + @"_sub ps with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
	)P
	inner join " + ctable + @" c with(nolock) on P.ProjectId = c.ProjectId
	where 1 = 1
	and c.valid = 1
	and c.CityID = @cityid
	and c.FXTCompanyId = @fxtcompanyid
	and c.CaseDate between @casedateStart and @casedateEnd " + strWhere0 + @"
)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(sql, new { CityId, FxtCompanyId, SaveUser, areaid, casedateStart, casedateEnd, caseTypeCode, buildingAreaFrom, buildingAreaTo, purposeCode, unitPriceFrom, unitPriceTo, key, buildingTypeCode });
            }
        }

        //        public int DeleteCaseCount(int fxtCompanyId, int cityId, string createtimefrom, string createtimeto,string creator)
        //        {
        //            string ptable, ctable, btable, casecomId, showcomId;
        //            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
        //            if (string.IsNullOrWhiteSpace(ctable)) return -1;

        //            string where = string.Empty;
        //            if (!string.IsNullOrWhiteSpace(createtimeto))
        //            {
        //                where = " and CreateDate <= '" + createtimeto + "'";
        //            }
        //            if (!string.IsNullOrWhiteSpace(creator))
        //            {
        //                where = " and Creator = '" + creator + "'";
        //            }

        //            var strSql = @"
        //select COUNT(*) from " + ctable + @"
        //WHERE CityId = " + cityId + @"
        //AND FXTCompanyId = " + fxtCompanyId + @"
        //and CreateDate >= '" + createtimefrom + "'" + where;

        //            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //            {
        //                var cmd = conn.CreateCommand();
        //                cmd.CommandText = strSql;
        //                int result = (int)cmd.ExecuteScalar();
        //                return result;
        //            }
        //        }
        #endregion

        #region 公共

        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string casecomId, out string showcomId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.CaseCompanyId,s.ShowCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),FxtDataCenter.dbo.[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            SqlParameter[] parameter = { 
                                           new SqlParameter("@cityid",SqlDbType.Int),
                                           new SqlParameter("@fxtcompanyid",SqlDbType.Int)
                                       };
            parameter[0].Value = cityid;
            parameter[1].Value = fxtcompanyid;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
            if (dt.Rows.Count == 0)
            {
                ptable = "";
                ctable = "";
                btable = "";
                casecomId = "";
                showcomId = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                ctable = dt.Rows[0]["CaseTable"].ToString();
                btable = dt.Rows[0]["BuildingTable"].ToString();
                casecomId = dt.Rows[0]["CaseCompanyId"].ToString();
                showcomId = dt.Rows[0]["ShowCompanyId"].ToString();
            }
        }

        private class ExCase
        {
            public int CaseId { get; set; }

            public decimal Rate { get; set; }
        }

        #endregion

        public DataTable GetMisMatchProjectCase(int taskId)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                string sql = @"
select 
	ProjectName as '*楼盘名称'
	,AreaName as 行政区
	,T.BuildingName as 楼栋名称
	,T.FloorNumber as 楼层
	,T.HouseNo as 房号
	,TotalFloor as 总楼层
	,CaseDate as '*案例时间'
	,ISNULL(c.CodeName,'') as '*用途'
	,BuildingArea as '*建筑面积'
	,UnitPrice as '*单价'
	,TotalPrice as '*总价'
	,ISNULL(c4.CodeName,'') as '*案例类型'
	,ISNULL(c1.CodeName,'') as 朝向
	,ISNULL(c6.CodeName,'') as 建筑类型
	,ISNULL(c7.CodeName,'') as 户型
	,ISNULL(c5.CodeName,'') as 户型结构
	,BuildingDate as 建筑年代
	,ZhuangXiu as 装修
    ,UsableArea as 使用面积
    ,RemainYear as 剩余年限
    ,Depreciation as 成新率
	,ISNULL(c2.CodeName,'') as 币种
	,SubHouse as 附属房屋
	,PeiTao as 配套
	,SourceName as 来源
	,SourceLink as 链接
	,SourcePhone as 电话
	,T.Remark as 备注
from (
	select * from fxtdatacenter.dbo.DAT_CaseTemp ca
	where TaskID = @taskid
)T
left join FxtDataCenter.dbo.SYS_Code c on T.PurposeCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 on T.FrontCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 on T.MoneyUnitCode = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 on T.SightCode = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 on T.CaseTypeCode = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 on T.StructureCode = c5.Code
left join FxtDataCenter.dbo.SYS_Code c6 on T.BuildingTypeCode = c6.Code
left join FxtDataCenter.dbo.SYS_Code c7 on T.HouseTypeCode = c7.Code";
                paramet.Add(new SqlParameter("@taskid", taskId));
                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
                DataTable dtable = DBHelperSql.ExecuteDataTable(sql, param);
                return dtable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int DeleteMisMatchProjectCase(int taskid, string areaName, string projectName)
        {
            var strSql = "delete fxtdatacenter.dbo.DAT_CaseTemp where TaskID = @taskid and AreaName = @areaname and ProjectName = @projectname";
            SqlParameter[] parameter = { new SqlParameter("@taskid", SqlDbType.Int), new SqlParameter("@areaname", SqlDbType.NVarChar), new SqlParameter("@projectname", SqlDbType.NVarChar) };
            parameter[0].Value = taskid;
            parameter[1].Value = areaName;
            parameter[2].Value = projectName;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            return DBHelperSql.ExecuteNonQuery(strSql, parameter);
        }

        public int DeleteAllMisMatchProjectCase(int taskid)
        {
            var strSql = "delete fxtdatacenter.dbo.DAT_CaseTemp where TaskID = @taskid";
            SqlParameter[] parameter = { new SqlParameter("@taskid", SqlDbType.Int) };
            parameter[0].Value = taskid;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            return DBHelperSql.ExecuteNonQuery(strSql, parameter);
        }
    }
}
