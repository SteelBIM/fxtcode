using System;
using System.Collections.Generic;
using System.Linq;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ProjectCaseTask : IProjectCaseTask
    {
        public IQueryable<MisMatchProjectDTO> GetDatCaseTemp(long taskid, int cityid, int fxtcompanyid, string key)
        {
            string where = string.IsNullOrEmpty(key) ? "" : " and ProjectName like '%" + key + "%'";
            string strSql = string.Format(@"
SELECT ProjectName
	,(case when ProjectId is not null then 1 else 0 end) as IsWaitProject
	,AreaId
	,AreaName
	,COUNT(CaseID) AS Num
FROM(
	select 
		T1.ProjectName
		,T1.AreaId
		,T1.AreaName
		,T1.CaseID
		,T2.WaitProjectId as ProjectId
	from (
		select ProjectName,ProjectId,AreaId,AreaName,CaseID
		from FxtDataCenter.dbo.DAT_CaseTemp with(nolock) where TaskID = {0} " + where + @"
	)T1
	left join (select * from FXTProject.dbo.Dat_WaitProject with(nolock) where CityId = {1} and FxtCompanyId = {2})T2
	on T1.ProjectId = T2.WaitProjectId
)T
GROUP BY ProjectName
	,ProjectId
	,AreaId
	,AreaName
ORDER BY IsWaitProject DESC, Num DESC
	,ProjectName", taskid, cityid, fxtcompanyid);
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<MisMatchProjectDTO>(strSql).AsQueryable();
            }
        }

        public int AddProjectNameMisMatch(DAT_CaseTemp dct)
        {
            dct.CreateDate = DateTime.Now;

            const string strSql = @"insert into FxtDataCenter.dbo.DAT_CaseTemp (taskid,projectname,projectid,buildingid,houseid,companyid,casedate,purposecode,floornumber,buildingname,houseno,buildingarea,usablearea,frontcode,unitprice,moneyunitcode,sightcode,casetypecode,structurecode,buildingtypecode,housetypecode,createdate,creator,remark,totalprice,oldid,cityid,fxtcompanyid,totalfloor,remainyear,depreciation,fitmentcode,surveyid,savedatetime,saveuser,sourcename,sourcelink,sourcephone,areaid,areaname,buildingdate,zhuangxiu,subhouse,peitao,errremark,errtype) 
values(@taskid,@projectname,@projectid,@buildingid,@houseid,@companyid,@casedate,@purposecode,@floornumber,@buildingname,@houseno,@buildingarea,@usablearea,@frontcode,@unitprice,@moneyunitcode,@sightcode,@casetypecode,@structurecode,@buildingtypecode,@housetypecode,@createdate,@creator,@remark,@totalprice,@oldid,@cityid,@fxtcompanyid,@totalfloor,@remainyear,@depreciation,@fitmentcode,@surveyid,@savedatetime,@saveuser,@sourcename,@sourcelink,@sourcephone,@areaid,@areaname,@buildingdate,@zhuangxiu,@subhouse,@peitao,@errremark,@errtype)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, dct);
            }
        }

        public IQueryable<DAT_CaseTemp> GetDatCaseTemp(long taskId, int cityid, int fxtcompanyid, int projectid)
        {
            var strSql = string.Format(@"
select * from FxtDataCenter.dbo.DAT_CaseTemp with(nolock) 
where projectid = {0}
and cityid = {1}
and fxtcompanyid = {2}
and taskId in (
	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
	where NameErrNumber > 0
	and CityID = {1}
	and FXTCompanyId = {2}
)", projectid, cityid, fxtcompanyid, taskId);

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_CaseTemp>(strSql).AsQueryable();
            }
        }

        public int AddProjectCase(DAT_CaseTemp modal)
        {
            string strSql = @"insert into  FXTProject.dbo.DAT_Case (projectid,buildingid,houseid,companyid,casedate,purposecode,floornumber,buildingname,houseno,buildingarea,usablearea,frontcode,unitprice,moneyunitcode,sightcode,casetypecode,structurecode,buildingtypecode,housetypecode,createdate,creator,remark,totalprice,oldid,cityid,fxtcompanyid,totalfloor,remainyear,depreciation,fitmentcode,surveyid,savedatetime,saveuser,sourcename,sourcelink,sourcephone,areaid,areaname,buildingdate,zhuangxiu,subhouse,peitao) 
values(@projectid,@buildingid,@houseid,@companyid,@casedate,@purposecode,@floornumber,@buildingname,@houseno,@buildingarea,@usablearea,@frontcode,@unitprice,@moneyunitcode,@sightcode,@casetypecode,@structurecode,@buildingtypecode,@housetypecode,@createdate,@creator,@remark,@totalprice,@oldid,@cityid,@fxtcompanyid,@totalfloor,@remainyear,@depreciation,@fitmentcode,@surveyid,@savedatetime,@saveuser,@sourcename,@sourcelink,@sourcephone,@areaid,@areaname,@buildingdate,@zhuangxiu,@subhouse,@peitao)";

            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                return conn.Execute(strSql, modal);
            }
        }

        public int DelteDatCaseTemp(List<long> caseId)
        {
            var caseIds = string.Join(",", caseId);

            var strSql = @"delete from FxtDataCenter.dbo.DAT_CaseTemp where caseid in (" + caseIds + ") ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql);
            }
        }

        public int UpdateDatCaseTemp(long taskId, string sourceName, string areaName, int destProjectid, int cityid, int fxtcompanyid)
        {
            string strSql = @"
update FxtDataCenter.dbo.DAT_CaseTemp with(rowlock) set projectid = @ProjectId 
where ProjectName = @ProjectName and (AreaName = @areaName or (@areaName = '' and AreaName is null))
and cityid = @CityId
and fxtcompanyid = @FxtCompanyId
and taskId in (
	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
	where NameErrNumber > 0
	and CityID = @cityid
	and FXTCompanyId = @fxtcompanyid
)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { taskId, ProjectId = destProjectid, ProjectName = sourceName, areaName = areaName, CityId = cityid, FxtCompanyId = fxtcompanyid });
            }
        }

        public int AddProjectMatch(SYS_ProjectMatch pm)
        {
            string strSql = @"insert into FXTProject.dbo.SYS_ProjectMatch (projectnameid,netareaname,netname,projectname,cityid,fxtcompanyid,creator,createtime) 
values(@projectnameid,@netareaname,@netname,@projectname,@cityid,@fxtcompanyid,@creator,@createtime)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, pm);
            }
        }

        public int MatchProjectCase(int cityId, int fxtCompanyId, string userName, string ids)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;

            string where = string.Empty;
            if (!string.IsNullOrEmpty(ids))
            {
                where = " and TaskID in (" + ids + ")";
            }

            string updateStr = @"
update a set a.ProjectId = b.ProjectNameId,a.AreaId = b.AreaID,a.AreaName = b.AreaName
from
(	select *  
	from FxtDataCenter.dbo.DAT_CaseTemp
		where CityID = @cityid
		and Valid = 1
		and FXTCompanyId = @fxtCompanyId
		and TaskID in (
			select TaskID from FxtDataCenter.dbo.DAT_ImportTask
			where cityid = @cityid
			and FXTCompanyId = @fxtCompanyId
			and ImportType = 1212003
		) " + where + @"
)a
,(
	SELECT distinct sp.ProjectNameId
		,sp.NetName
		,sp.NetAreaName
		,sp.CityId
		,sp.FXTCompanyId
		,p1.AreaID
		,a.AreaName
	FROM (
		SELECT *
		FROM FXTProject.dbo.SYS_ProjectMatch
		WHERE cityid = @cityid
			AND FXTCompanyId = @fxtCompanyId
		) sp
	INNER JOIN (
		SELECT p.ProjectId
			,p.ProjectName
			,p.AreaID
		FROM " + ptable + @" p with(nolock)
		WHERE p.CityID = @cityId
			AND p.Valid = 1
			AND p.FxtCompanyId IN (" + showcomId + @")
			AND NOT EXISTS (
				SELECT ProjectId
				FROM " + ptable + @"_sub p1 with(nolock)
				WHERE p.ProjectId = p1.ProjectId
					AND p1.CityID = @cityId
					AND p1.Fxt_CompanyId = @fxtCompanyId
				)	
		UNION	
		SELECT p.ProjectId
			,p.ProjectName
			,p.AreaID
		FROM " + ptable + @"_sub p with(nolock)
		WHERE p.Valid = 1
			AND p.CityID = @cityId
			AND p.Fxt_CompanyId = @fxtCompanyId
		) p1 ON p1.projectId = sp.ProjectNameId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p1.AreaID = a.AreaId
)b
where a.ProjectName = b.NetName and a.AreaName = b.NetAreaName";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var updatere = conn.Execute(updateStr, new { cityId, fxtCompanyId });
                if (updatere <= 0)
                {
                    return 0;
                }

                string insertStr = @"
insert into " + ctable + @"
(Valid,ProjectId,BuildingId,HouseId,CompanyId,CaseDate,PurposeCode,FloorNumber,HouseNo,BuildingArea,UsableArea,FrontCode,UnitPrice,MoneyUnitCode,SightCode,CaseTypeCode,StructureCode,BuildingTypeCode,HouseTypeCode
,CreateDate,Creator,Remark,TotalPrice,OldID,CityID,FXTCompanyId,BuildingName,TotalFloor,RemainYear,Depreciation,FitmentCode,SurveyId,SourceName,SourceLink,SourcePhone,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao)
select 	
1,ProjectId,BuildingId,HouseId,CompanyId,CaseDate,PurposeCode,FloorNumber,HouseNo,BuildingArea,UsableArea,FrontCode,UnitPrice,MoneyUnitCode,SightCode,CaseTypeCode,StructureCode,BuildingTypeCode,HouseTypeCode
,GETDATE(),@userName,Remark,TotalPrice,OldID,CityID,FXTCompanyId,BuildingName,TotalFloor,RemainYear,Depreciation,FitmentCode,SurveyId,SourceName,SourceLink,SourcePhone,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao
from FxtDataCenter.dbo.DAT_CaseTemp
where CityID = @cityid
and ProjectId > 0
and FXTCompanyId = @fxtcompanyid
and TaskID in (
	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
	where cityid = @cityid
	and FXTCompanyId = @fxtcompanyid
	and ImportType = 1212003
) " + where;
                var insertre = conn.Execute(insertStr, new { cityId, fxtCompanyId, userName });
                if (insertre <= 0)
                {
                    return -1;
                }

                var deleteStr = @"
delete FxtDataCenter.dbo.DAT_CaseTemp where CityID = @cityid and ProjectId > 0 and FXTCompanyId = @fxtcompanyid
and TaskID in (
	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
	where cityid = @cityid
	and FXTCompanyId = @fxtcompanyid
	and ImportType = 1212003
)" + where;
                var deletere = conn.Execute(deleteStr, new { cityId, fxtCompanyId });
                if (deletere <= 0)
                {
                    return -1;
                }

                var updateImport = @"
update I set SucceedNumber = SucceedNumber + (case when NameErrNumber - (case when T.num is null then 0 else (case when T.num is null then 0 else T.num end) end) < 0 then 0 else NameErrNumber - (case when T.num is null then 0 else T.num end) end)
,NameErrNumber = (case when T.num is null then 0 else T.num end) from
(
	select TaskID,ImportType,TaskName,CityID,FXTCompanyId,SucceedNumber,NameErrNumber from FxtDataCenter.dbo.DAT_ImportTask
	where cityid = @cityid
	and FXTCompanyId = @fxtcompanyid
	and ImportType = 1212003
	and NameErrNumber > 0 " + where + @"
)I left join(
	select TaskID,COUNT(*) as num from FxtDataCenter.dbo.DAT_CaseTemp
	where CityID = @cityid
	and Valid = 1
	and FXTCompanyId = @fxtcompanyid
	and TaskID in (
		select TaskID from FxtDataCenter.dbo.DAT_ImportTask
		where cityid = @cityid
		and FXTCompanyId = @fxtcompanyid
		and ImportType = 1212003
	)" + where + @"
	group by TaskID
)T on T.TaskID = I.TaskID";
                return conn.Execute(updateImport, new { cityId, fxtCompanyId });
            }
        }

        public int MatchProjectCaseWaitProject(int waitProjectId, int projectId, int areaId, string projectName, int cityId, int fxtCompanyId, string userName)
        {
            string ptable, ctable, btable, casecomId, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out casecomId, out showcomId);
            if (string.IsNullOrWhiteSpace(ctable)) return -1;

            ptable = "FXTProject." + ptable;
            ctable = "FXTProject." + ctable;

            string updateStr = @"
update a set a.ProjectId = @ProjectId,a.AreaId = @AreaId
from(
	select *  
	from FxtDataCenter.dbo.DAT_CaseTemp
		where CityID = @cityid
		and Valid = 1
		and FXTCompanyId = @fxtCompanyId
		and TaskID in (
			select TaskID from FxtDataCenter.dbo.DAT_ImportTask
			where cityid = @cityid
			and FXTCompanyId = @fxtCompanyId
			and ImportType = 1212003
		)
		and ProjectId = @waitProjectId
)a";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var updatere = conn.Execute(updateStr, new { waitProjectId = waitProjectId, ProjectId = projectId, AreaId = areaId, projectname = projectName, cityId, fxtCompanyId });
                if (updatere <= 0)
                {
                    return 0;
                }

                string insertStr = @"
insert into " + ctable + @"
(Valid,ProjectId,BuildingId,HouseId,CompanyId,CaseDate,PurposeCode,FloorNumber,HouseNo,BuildingArea,UsableArea,FrontCode,UnitPrice,MoneyUnitCode,SightCode,CaseTypeCode,StructureCode,BuildingTypeCode,HouseTypeCode
,CreateDate,Creator,Remark,TotalPrice,OldID,CityID,FXTCompanyId,BuildingName,TotalFloor,RemainYear,Depreciation,FitmentCode,SurveyId,SourceName,SourceLink,SourcePhone,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao)
select 	
1,ProjectId,BuildingId,HouseId,CompanyId,CaseDate,PurposeCode,FloorNumber,HouseNo,BuildingArea,UsableArea,FrontCode,UnitPrice,MoneyUnitCode,SightCode,CaseTypeCode,StructureCode,BuildingTypeCode,HouseTypeCode
,GETDATE(),@userName,Remark,TotalPrice,OldID,CityID,FXTCompanyId,BuildingName,TotalFloor,RemainYear,Depreciation,FitmentCode,SurveyId,SourceName,SourceLink,SourcePhone,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao
from FxtDataCenter.dbo.DAT_CaseTemp
where CityID = @cityid
and ProjectId = @ProjectId
and AreaId = @AreaId
and FXTCompanyId = @fxtcompanyid
and TaskID in (
	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
	where cityid = @cityid
	and FXTCompanyId = @fxtcompanyid
	and ImportType = 1212003
)";
                var insertre = conn.Execute(insertStr, new { ProjectId = projectId, AreaId = areaId, cityId, fxtCompanyId, userName });
                if (insertre <= 0)
                {
                    return -1;
                }

                var deleteStr = @"
delete FxtDataCenter.dbo.DAT_CaseTemp where ProjectId = @ProjectId and AreaId = @AreaId and CityID = @cityid and FXTCompanyId = @fxtCompanyId
and TaskID in (
	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
	where cityid = @cityid
	and FXTCompanyId = @fxtcompanyid
	and ImportType = 1212003
)";
                var deleteStrre = conn.Execute(deleteStr, new { ProjectId = projectId, AreaId = areaId, cityId, fxtCompanyId });
                if (deleteStrre <= 0)
                {
                    return -1;
                }
                var updateImport = @"
update I set SucceedNumber = SucceedNumber + (case when NameErrNumber - (case when T.num is null then 0 else (case when T.num is null then 0 else T.num end) end) < 0 then 0 else NameErrNumber - (case when T.num is null then 0 else T.num end) end)
,NameErrNumber = (case when T.num is null then 0 else T.num end) from
(
	select TaskID,ImportType,TaskName,CityID,FXTCompanyId,SucceedNumber,NameErrNumber from FxtDataCenter.dbo.DAT_ImportTask
	where cityid = @cityid
	and FXTCompanyId = @fxtcompanyid
	and ImportType = 1212003
	and NameErrNumber > 0 
)I left join(
	select TaskID,COUNT(*) as num from FxtDataCenter.dbo.DAT_CaseTemp
	where CityID = @cityid
	and Valid = 1
	and FXTCompanyId = @fxtcompanyid
	and TaskID in (
		select TaskID from FxtDataCenter.dbo.DAT_ImportTask
		where cityid = @cityid
		and FXTCompanyId = @fxtcompanyid
		and ImportType = 1212003
	)
	group by TaskID
)T on T.TaskID = I.TaskID";
                return conn.Execute(updateImport, new { cityId, fxtCompanyId });
            }
        }

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

        public int UpdateDatCaseTempWaitProject(long taskId, long waitProjectId, string projectName, int cityid, int fxtcompanyid)
        {
            string strSql = @"
update FxtDataCenter.dbo.DAT_CaseTemp with(rowlock) set projectid = @ProjectId 
where ProjectName = @ProjectName and (AreaId <= 0 or AreaId is null)
and cityid = @CityId
and fxtcompanyid = @FxtCompanyId
and taskId in (
	select TaskID from FxtDataCenter.dbo.DAT_ImportTask
	where NameErrNumber > 0
	and CityID = @cityid
	and FXTCompanyId = @fxtcompanyid
)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { taskId, ProjectId = waitProjectId, ProjectName = projectName, CityId = cityid, FxtCompanyId = fxtcompanyid });
            }
        }
    }
}
