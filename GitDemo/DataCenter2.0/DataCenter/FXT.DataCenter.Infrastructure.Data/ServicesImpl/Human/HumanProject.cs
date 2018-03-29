using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class HumanProject : IHumanProject
    {
        public IQueryable<DAT_Human> GetHumanProjects(int? areaid, string projectName, int cityId, int fxtcompanyId, bool self)
        {
            string ptable, showcomId;
            Access(cityId, fxtcompanyId, out ptable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_Human>().AsQueryable();
            }
            if (self) showcomId = fxtcompanyId.ToString();

            ptable = "FxtProject." + ptable;

            string strSql = @"
select HumanId,Name,c.CodeName as AgeGroupName,c1.CodeName as MarriageName,c2.CodeName as EducationName,c3.CodeName as OccupationName,P.AreaID,a.AreaName,P.ProjectId,ProjectName,FxtcompanyId from (
	select * from FxtData_Human.dbo.DAT_Human
	where 1 = 1
	and Valid = 1
	and IsGroup = 1
	and CityId = @cityid
	and FxtcompanyId = @fxtcompanyid
)H 
inner join (
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
)P on p.ProjectId = h.ProjectId
left join FxtDataCenter.dbo.SYS_Area a on P.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c on H.AgeGroup = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 on H.Marriage = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 on H.Education = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 on H.Occupation = c3.Code
where 1 = 1";

            if (!string.IsNullOrWhiteSpace(projectName)) strSql += " and P.ProjectName like @projectName";
            if (areaid > 0) strSql += " and P.AreaID = @areaid";
            strSql += " order by ProjectName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_Human>(strSql, new { areaid, projectName = "%" + projectName + "%", cityId, fxtcompanyId }).AsQueryable();
            }
        }

        public DAT_Human GetHumanProjectById(long humanid, int fxtcompanyid, int cityid)
        {
            string ptable, showcomId;
            Access(cityid, fxtcompanyid, out ptable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyid.ToString();
            if (ptable == "")
            {
                return new DAT_Human();
            }

            ptable = "FxtProject." + ptable;
            var strSql = @"
select 
	HumanId,Name,Sex,Age,AgeGroup,Origin,CONVERT(nvarchar(10),Birthday,121) as Birthday,IDCard,Marriage,Telephone,Education,Occupation,Position,Company,Salary,Transportation,FamilyNum,Houses,H.CityId,H.AreaId,H.ProjectId,BuildingId,HouseId,IsGroup,H.Remark,FxtcompanyId
	,c.CodeName as AgeGroupName
	,a.AreaName
	,P.ProjectName
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
inner join (
	select * from FxtData_Human.dbo.DAT_Human
	where 1 = 1
	and Valid = 1
	and IsGroup = 1
	and CityId = @cityid
	and FxtcompanyId = @fxtcompanyid
)H on p.ProjectId = h.ProjectId
left join FxtDataCenter.dbo.SYS_Area a on P.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c on H.AgeGroup = c.Code
where 1 = 1
and HumanId = @humanid";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_Human>(strSql, new { humanid, fxtcompanyid, cityid }).ToList().FirstOrDefault();
            }
        }

        public int AddHumanProject(DAT_Human dh)
        {
            string strSql = @"insert into FxtData_Human.dbo.DAT_Human(Name,Sex,Age,AgeGroup,Origin,Birthday,IDCard,Marriage,Telephone,Education,Occupation,Position,Company,Salary,Transportation,FamilyNum,Houses,CityId,AreaId,ProjectId,BuildingId,HouseId,IsGroup,Remark,FxtcompanyId,Creator,CreateTime,Saver,SaveTime,Valid)
values (@Name,@Sex,@Age,@AgeGroup,@Origin,@Birthday,@IDCard,@Marriage,@Telephone,@Education,@Occupation,@Position,@Company,@Salary,@Transportation,@FamilyNum,@Houses,@CityId,@AreaId,@ProjectId,@BuildingId,@HouseId,@IsGroup,@Remark,@FxtcompanyId,@Creator,@CreateTime,@Saver,@SaveTime,@Valid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Execute(strSql, dh);
            }
        }

        public int UpdateHumanProject(DAT_Human dh)
        {
            var strSql = @"update FxtData_Human.dbo.DAT_Human with(rowlock) set Name = @Name,Sex = @Sex,Age = @Age,AgeGroup = @AgeGroup,Origin = @Origin,Birthday = @Birthday,IDCard = @IDCard,Marriage = @Marriage,Telephone = @Telephone,Education = @Education,Occupation = @Occupation,Position = @Position,Company = @Company,Salary = @Salary,Transportation = @Transportation,FamilyNum = @FamilyNum,Houses = @Houses,CityId = @CityId,AreaId = @AreaId,ProjectId = @ProjectId,BuildingId = @BuildingId,HouseId = @HouseId,Remark = @Remark,Saver = @Saver,SaveTime = @SaveTime
where HumanId = @HumanId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Execute(strSql, dh);
            }
        }

        public int DeleteHumanProject(long humanid, string saver, DateTime savetime)
        {
            var strSql = @"update FxtData_Human.dbo.DAT_Human with(rowlock) set Valid = 0,Saver = @Saver,SaveTime = @SaveTime
where HumanId = @HumanId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Execute(strSql, new { humanid, saver, savetime });
            }
        }

        public IQueryable<DAT_Human> GetProjectList(int fxtCompanyId, int cityId, long projectId)
        {
            string ptable, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_Human>().AsQueryable();
            }
            ptable = "FxtProject." + ptable;
            string where = projectId > 0 ? " and ProjectId = @ProjectId" : "";
            string str = @"
select ProjectId,ProjectName,AreaID
from " + ptable + @" p with(nolock)
where not exists(
	select ProjectId from " + ptable + @"_sub ps with(nolock)
	where ps.ProjectId = p.ProjectId
	and ps.CityID = @cityid
	and ps.Fxt_CompanyId = @fxtcompanyid
)
and Valid = 1
and CityID = @cityid
and FxtCompanyId in (" + showcomId + @")" + where + @"
union
select ProjectId,ProjectName,AreaID
from " + ptable + @"_sub ps with(nolock)
where Valid = 1
and CityID = @cityid
and Fxt_CompanyId = @fxtcompanyid" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_Human>(str, new { cityId, fxtCompanyId, projectId }).AsQueryable();
            }
        }

        public IQueryable<DAT_Human> ExportHumanProjects(int? areaid, string projectName, int cityId, int fxtcompanyId, bool self)
        {
            string ptable, showcomId;
            Access(cityId, fxtcompanyId, out ptable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_Human>().AsQueryable();
            }
            if (self) showcomId = fxtcompanyId.ToString();

            ptable = "FxtProject." + ptable;

            string strSql = @"
select 
	Name
	,c.CodeName as SexName
	,Age
	,c1.CodeName as AgeGroupName
	,Origin
	,CONVERT(nvarchar(10),Birthday,121) as BirthdayName
	,IDCard
	,c2.CodeName as MarriageName
	,Telephone
	,c3.CodeName as EducationName
	,c4.CodeName as OccupationName
	,c5.CodeName as PositionName
	,Company
	,c6.CodeName as SalaryName
	,c7.CodeName as TransportationName
	,FamilyNum
	,Houses
	,ci.CityName
	,a.AreaName
	,ProjectName
	,H.Remark
from (
	select * from FxtData_Human.dbo.DAT_Human
	where 1 = 1
	and Valid = 1
	and IsGroup = 1
	and CityId = @cityid
	and FxtcompanyId = @fxtcompanyid
)H
inner join (
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
)P on p.ProjectId = h.ProjectId
left join FxtDataCenter.dbo.SYS_City ci on H.CityId = ci.CityId
left join FxtDataCenter.dbo.SYS_Area a on P.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c on H.Sex = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 on H.AgeGroup = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 on H.Marriage = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 on H.Education = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 on H.Occupation = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 on H.Position = c5.Code
left join FxtDataCenter.dbo.SYS_Code c6 on H.Salary = c6.Code
left join FxtDataCenter.dbo.SYS_Code c7 on H.Transportation = c7.Code
where 1 = 1";

            if (!string.IsNullOrWhiteSpace(projectName)) strSql += " and P.ProjectName like @projectName";
            if (areaid > 0) strSql += " and P.AreaID = @areaid";
            strSql += " order by ProjectName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_Human>(strSql, new { areaid, projectName = "%" + projectName + "%", cityId, fxtcompanyId }).AsQueryable();
            }
        }

        #region 公共

        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string showcomId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.CaseCompanyId,s.ShowCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

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
                showcomId = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                showcomId = dt.Rows[0]["ShowCompanyId"].ToString();
            }
        }

        #endregion
    }
}
