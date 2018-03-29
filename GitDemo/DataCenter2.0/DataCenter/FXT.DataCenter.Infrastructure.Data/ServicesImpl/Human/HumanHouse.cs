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
    public class HumanHouse : IHumanHouse
    {
        public IQueryable<DAT_HumanHouse> GetHumanHouses(int? areaId, string key, int cityId, int fxtcompanyId, bool self)
        {
            string ptable, btable, htable, showcomId;
            Access(cityId, fxtcompanyId, out ptable, out btable, out htable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_HumanHouse>().AsQueryable();
            }
            if (self) showcomId = fxtcompanyId.ToString();

            ptable = "FxtProject." + ptable;
            btable = "FxtProject." + btable;
            htable = "FxtProject." + htable;
            string pwhere = string.Empty;
            if (areaId > 0)
            {
                pwhere += " and AreaID = " + areaId;
            }
            if (!string.IsNullOrEmpty(key))
            {
                pwhere += " and ProjectName like '%" + key + "%'";
            }
            string strSql = @"
select 
	HumanId
	,AreaName
	,ProjectId
	,ProjectName
	,BuildingId
	,BuildingName
	,HouseId
	,HouseName
	,PropertyNumber
	,Name
	,BuildingArea
	,BuildingStructureCode
	,c.CodeName as BuildingStructureCodeName
	,StructureCode
	,c1.CodeName as StructureCodeName
    ,FxtcompanyId
from (
	select 
		DP.HumanId
		,P.AreaID
		,DP.ProjectId
		,P.ProjectName as ProjectName
		,DP.BuildingId
		,B.BuildingName as BuildingName
		,DP.HouseId
		,H.HouseName as HouseName
		,DH.PropertyNumber
		,DP.Name
		,DH.BuildingArea
		,DH.BuildingStructureCode
		,DH.StructureCode
		,H.BuildArea as HBuildArea
		,B.StructureCode as BBuildingStructureCode
		,H.StructureCode as HStructureCode
		,DP.FxtcompanyId
	from (
		select * from FxtData_Human.dbo.DAT_Human
		where 1 = 1
		and Valid = 1
		and IsGroup = 0
		and CityId = @cityid
		and FxtcompanyId = @fxtcompanyid
	)DP
	inner join (
		select * from FxtData_Human.dbo.DAT_HumanHouse
		where Valid = 1
		and CityId = @cityid
		and FxtcompanyId = @fxtcompanyid
	)DH on DP.HumanId = DH.HumanId and DP.HouseId = DH.HouseId
	inner join(
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
		and FxtCompanyId in (" + showcomId + @") " + pwhere + @"
		union
		select 
		ProjectId,ProjectName,AreaID
		from " + ptable + @"_sub ps with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid  " + pwhere + @"
	)P on DP.ProjectId = P.ProjectId
	inner join(
		select 
		BuildingId,BuildingName,ProjectId,StructureCode,RightCode
		from " + btable + @" b with(nolock)
		where not exists(
			select BuildingId from " + btable + @"_sub bs with(nolock)
			where bs.BuildingId = b.BuildingId
			and bs.CityID = @cityid
			and bs.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + showcomId + @")	
		union
		select 
		BuildingId,BuildingName,ProjectId,StructureCode,RightCode
		from " + btable + @"_sub bs with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
	)B on DP.BuildingId = B.BuildingId and P.ProjectId = B.ProjectId
	inner join(
		select 
		HouseId,HouseName,BuildingId,BuildArea,StructureCode,FitmentCode
		from " + htable + @" h with(nolock)
		where not exists(
			select HouseId from " + htable + @"_sub hs with(nolock)
			where hs.HouseId = h.HouseId
			and hs.CityID = @cityid
			and hs.FxtCompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + showcomId + @")	
		union
		select 
		HouseId,HouseName,BuildingId,BuildArea,StructureCode,FitmentCode
		from " + htable + @"_sub hs with(nolock)
		where Valid = 1
		and CityID = @cityid
		and FxtCompanyId = @fxtcompanyid
	)H on DP.HouseId = H.HouseId and H.BuildingId = B.BuildingId
	where 1 = 1
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on T.BuildingStructureCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on T.StructureCode = c1.Code
where 1 = 1
order by ProjectName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_HumanHouse>(strSql, new { cityId, fxtcompanyId }).AsQueryable();
            }
        }

        public DAT_HumanHouse GetHumanHouseById(long humanid, long houseid, int fxtcompanyid, int cityid)
        {
            string ptable, btable, htable, showcomId;
            Access(cityid, fxtcompanyid, out ptable, out btable, out htable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyid.ToString();
            if (ptable == "")
            {
                return new DAT_HumanHouse();
            }
            ptable = "FxtProject." + ptable;
            btable = "FxtProject." + btable;
            htable = "FxtProject." + htable;

            var strSql = @"
select 
	DP.*
	,DH.BuildingArea
	,DH.BuildingDate
	,DH.BuildingStructureCode
	,DH.IsLoaned
	,DH.IsMortgage
	,DH.IsParking
	,DH.IsSealUp
	,DH.IsSelf
	,DH.LoanedDeadline
	,DH.LoanedLines
	,DH.OwnerProportion
	,DH.PropertyNumber
	,DH.Remark
	,DH.RightCode
	,DH.Share
	,DH.StructureCode
	,DH.SubHouse
	,DH.ZhuangXiu
	,P.ProjectName as ProjectName
	,B.BuildingName as BuildingName
	,H.HouseName as HouseName
from (
	select * from FxtData_Human.dbo.DAT_Human
	where 1 = 1
	and Valid = 1
	and IsGroup = 0
	and CityId = @cityid
	and FxtcompanyId = @fxtcompanyid
	and HumanId = @humanid
)DP
inner join (
	select * from FxtData_Human.dbo.DAT_HumanHouse
	where Valid = 1
	and CityId = @cityid
	and FxtcompanyId = @fxtcompanyid
	and HumanId = @humanid
)DH on DP.HumanId = DH.HumanId and DP.HouseId = DH.HouseId
inner join(
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
)P on DP.ProjectId = P.ProjectId
inner join(
	select 
	BuildingId,BuildingName,ProjectId,StructureCode,RightCode
	from " + btable + @" b with(nolock)
	where not exists(
		select BuildingId from " + btable + @"_sub bs with(nolock)
		where bs.BuildingId = b.BuildingId
		and bs.CityID = @cityid
		and bs.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + showcomId + @")	
	union
	select 
	BuildingId,BuildingName,ProjectId,StructureCode,RightCode
	from " + btable + @"_sub bs with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
)B on DP.BuildingId = B.BuildingId and P.ProjectId = B.ProjectId
inner join(
	select 
	HouseId,HouseName,BuildingId,BuildArea,StructureCode,FitmentCode
	from " + htable + @" h with(nolock)
	where not exists(
		select HouseId from " + htable + @"_sub hs with(nolock)
		where hs.HouseId = h.HouseId
		and hs.CityID = @cityid
		and hs.FxtCompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + showcomId + @")	
	union
	select 
	HouseId,HouseName,BuildingId,BuildArea,StructureCode,FitmentCode
	from " + htable + @"_sub hs with(nolock)
	where Valid = 1
	and CityID = @cityid
	and FxtCompanyId = @fxtcompanyid
)H on DP.HouseId = H.HouseId and H.BuildingId = B.BuildingId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on P.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on DH.BuildingStructureCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on DH.StructureCode = c1.Code
where 1 = 1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_HumanHouse>(strSql, new { humanid, fxtcompanyid, cityid }).ToList().FirstOrDefault();
            }
        }

        public IQueryable<DAT_HumanHouse> GetProjectList(int fxtCompanyId, int cityId, long projectId)
        {
            string ptable, btable, htable, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out btable, out htable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_HumanHouse>().AsQueryable();
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
                return conn.Query<DAT_HumanHouse>(str, new { cityId, fxtCompanyId, projectId }).AsQueryable();
            }
        }

        public int AddHumanHouse(DAT_HumanHouse dh)
        {
            string humanStrSql = @"insert into FxtData_Human.dbo.DAT_Human(Name,Sex,Age,AgeGroup,Origin,Birthday,IDCard,Marriage,Telephone,Education,Occupation,Position,Company,Salary,Transportation,FamilyNum,Houses,CityId,AreaId,ProjectId,BuildingId,HouseId,IsGroup,FxtcompanyId,Creator,CreateTime,Valid)
values ('" + dh.Name + "'," + (dh.Sex == null ? "null" : dh.Sex.ToString()) + "," + (dh.Age == null ? "null" : dh.Age.ToString()) + "," + dh.AgeGroup + "," + (dh.Origin == null ? "null" : "'" + dh.Origin.ToString() + "'") + "," + (dh.Birthday == null ? "null" : "'" + ((DateTime)dh.Birthday).ToString("yyyy-MM-dd") + "'") + "," + (dh.IDCard == null ? "null" : "'" + dh.IDCard.ToString() + "'") + "," + dh.Marriage + "," + (dh.Telephone == null ? "null" : "'" + dh.Telephone.ToString() + "'") + "," + dh.Education + "," + dh.Occupation + "," + dh.Position + "," + (dh.Company == null ? "null" : "'" + dh.Company.ToString() + "'") + "," + dh.Salary + "," + dh.Transportation + "," + (dh.FamilyNum == null ? "null" : dh.FamilyNum.ToString()) + "," + (dh.Houses == null ? "null" : dh.Houses.ToString()) + "," + dh.CityId + "," + dh.AreaId + "," + dh.ProjectId + "," + dh.BuildingId + "," + dh.HouseId + "," + dh.IsGroup + "," + dh.FxtcompanyId + ",'" + dh.Creator + @"',GETDATE(),1);
SELECT SCOPE_IDENTITY();";

            string houseStrSql = @"insert into FxtData_Human.dbo.DAT_HumanHouse(HumanId,CityId,AreaId,ProjectId,BuildingId,HouseId,BuildingArea,BuildingStructureCode,StructureCode,IsParking,SubHouse,BuildingDate,ZhuangXiu,IsLoaned,LoanedLines,LoanedDeadline,IsSelf,Share,OwnerProportion,PropertyNumber,IsMortgage,IsSealUp,RightCode,Remark,FxtcompanyId,Creator,CreateTime,Valid)
values (@HumanId,@CityId,@AreaId,@ProjectId,@BuildingId,@HouseId,@BuildingArea,@BuildingStructureCode,@StructureCode,@IsParking,@SubHouse,@BuildingDate,@ZhuangXiu,@IsLoaned,@LoanedLines,@LoanedDeadline,@IsSelf,@Share,@OwnerProportion,@PropertyNumber,@IsMortgage,@IsSealUp,@RightCode,@Remark,@FxtcompanyId,@Creator,GetDate(),1)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = humanStrSql;
                var obj = cmd.ExecuteScalar();
                if (obj == null || Convert.ToInt64(obj) <= 0)
                {
                    return -1;
                }
                var humanId = Convert.ToInt32(obj);

                return conn.Execute(houseStrSql, new { humanId, dh.CityId, dh.AreaId, dh.ProjectId, dh.BuildingId, dh.HouseId, dh.BuildingArea, dh.BuildingStructureCode, dh.StructureCode, dh.IsParking, dh.SubHouse, dh.BuildingDate, dh.ZhuangXiu, dh.IsLoaned, dh.LoanedLines, dh.LoanedDeadline, dh.IsSelf, dh.Share, dh.OwnerProportion, dh.PropertyNumber, dh.IsMortgage, dh.IsSealUp, dh.RightCode, dh.Remark, dh.FxtcompanyId, dh.Creator });
            }
        }

        public int UpdateHumanHouse(DAT_HumanHouse dh)
        {
            string humanStrSql = @"update FxtData_Human.dbo.DAT_Human set Name = @Name,Sex = @Sex,Age = @Age,AgeGroup = @AgeGroup,Origin = @Origin,Birthday = @Birthday,IDCard = @IDCard,Marriage = @Marriage,Telephone = @Telephone,Education = @Education,Occupation = @Occupation,Position = @Position,Company = @Company,Salary = @Salary,Transportation = @Transportation,FamilyNum = @FamilyNum,Houses = @Houses,Saver = @Saver,SaveTime = GetDate()
where HumanId = @HumanId";

            string houseStrSql = @"update FxtData_Human.dbo.DAT_HumanHouse set BuildingArea = @BuildingArea,BuildingStructureCode = @BuildingStructureCode,StructureCode = @StructureCode,IsParking = @IsParking,SubHouse = @SubHouse,BuildingDate = @BuildingDate,ZhuangXiu = @ZhuangXiu,IsLoaned = @IsLoaned,LoanedLines = @LoanedLines,LoanedDeadline = @LoanedDeadline,IsSelf = @IsSelf,Share = @Share,OwnerProportion = @OwnerProportion,PropertyNumber = @PropertyNumber,IsMortgage = @IsMortgage,IsSealUp = @IsSealUp,RightCode = @RightCode,Remark = @Remark,Saver = @Saver,SaveTime = GetDate()
where HumanId = @HumanId and HouseId = @HouseId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                var result = conn.Execute(humanStrSql, dh);
                if (result <= 0)
                {
                    return -1;
                }
                return conn.Execute(houseStrSql, dh);
            }
        }

        public int DeleteHumanHouse(long humanid, long houseid, string saver, DateTime savetime)
        {
            var humanStrSql = @"update FxtData_Human.dbo.DAT_Human with(rowlock) set Valid = 0,Saver = @Saver,SaveTime = @SaveTime
where HumanId = @HumanId";
            var houseStrSql = @"update FxtData_Human.dbo.DAT_HumanHouse with(rowlock) set Valid = 0,Saver = @Saver,SaveTime = @SaveTime
where HumanId = @HumanId and HouseId = @HouseId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                var result = conn.Execute(humanStrSql, new { humanid, saver, savetime });
                if (result <= 0)
                {
                    return -1;
                }
                return conn.Execute(houseStrSql, new { humanid, houseid, saver, savetime });
            }
        }

        public IQueryable<DAT_HumanHouse> ExportHumanHouses(int cityId, int fxtcompanyId, bool self)
        {

            string ptable, btable, htable, showcomId;
            Access(cityId, fxtcompanyId, out ptable, out btable, out htable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtcompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_HumanHouse>().AsQueryable();
            }
            if (self) showcomId = fxtcompanyId.ToString();

            ptable = "FxtProject." + ptable;
            btable = "FxtProject." + btable;
            htable = "FxtProject." + htable;

            string strSql = @"
select 
    ci.CityName
	,Name
	,c.CodeName as SexName
	,Age
	,c1.CodeName as AgeGroupName
	,Origin
	,Birthday
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
	,AreaName
	,ProjectName
	,BuildingName
	,HouseName
	,BuildingArea
	,c8.CodeName as BuildingStructureCodeName
	,c9.CodeName as StructureCodeName
	,case when IsParking = 0 then '否' when IsParking = 1 then '是' else '' end as IsParkingName
	,SubHouse
	,c10.CodeName as BuildingDateName
	,c11.CodeName as ZhuangXiuName
	,case when IsLoaned = 0 then '否' when IsLoaned = 1 then '是' else '' end as IsLoanedName
	,c12.CodeName as LoanedLinesName
	,LoanedDeadline
	,case when IsSelf = 0 then '否' when IsSelf = 1 then '是' else '' end as IsSelfName
	,Share
	,OwnerProportion
	,PropertyNumber
	,case when IsMortgage = 0 then '否' when IsMortgage = 1 then '是' else '' end as IsMortgageName
	,case when IsSealUp = 0 then '否' when IsSealUp = 1 then '是' else '' end as IsSealUpName
	,c13.CodeName as RightCodeName
	,DH.Remark	
from (
	select * from FxtData_Human.dbo.DAT_Human
	where 1 = 1
	and Valid = 1
	and IsGroup = 0
	and CityId = @cityid
	and FxtcompanyId = @fxtcompanyid
)DP
inner join (
	select * from FxtData_Human.dbo.DAT_HumanHouse
	where Valid = 1
	and CityId = @cityid
	and FxtcompanyId = @fxtcompanyid
)DH on DP.HumanId = DH.HumanId and DP.HouseId = DH.HouseId
inner join(
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
)P on DP.ProjectId = P.ProjectId
inner join(
	select 
	BuildingId,BuildingName,ProjectId,StructureCode,RightCode
	from " + btable + @" b with(nolock)
	where not exists(
		select BuildingId from " + btable + @"_sub bs with(nolock)
		where bs.BuildingId = b.BuildingId
		and bs.CityID = @cityid
		and bs.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + showcomId + @")	
	union
	select 
	BuildingId,BuildingName,ProjectId,StructureCode,RightCode
	from " + btable + @"_sub bs with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
)B on DP.BuildingId = B.BuildingId and P.ProjectId = B.ProjectId
inner join(
	select 
	HouseId,HouseName,BuildingId,BuildArea,StructureCode,FitmentCode
	from " + htable + @" h with(nolock)
	where not exists(
		select HouseId from " + htable + @"_sub hs with(nolock)
		where hs.HouseId = h.HouseId
		and hs.CityID = @cityid
		and hs.FxtCompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + showcomId + @")	
	union
	select 
	HouseId,HouseName,BuildingId,BuildArea,StructureCode,FitmentCode
	from " + htable + @"_sub hs with(nolock)
	where Valid = 1
	and CityID = @cityid
	and FxtCompanyId = @fxtcompanyid
)H on DP.HouseId = H.HouseId and H.BuildingId = B.BuildingId
left join FxtDataCenter.dbo.SYS_City ci with(nolock) on DP.CityID = ci.CityID
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on P.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on DP.Sex = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on DP.AgeGroup = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on DP.Marriage = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on DP.Education = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on DP.Occupation = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on DP.Position = c5.Code
left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) on DP.Salary = c6.Code
left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) on DP.Transportation = c7.Code
left join FxtDataCenter.dbo.SYS_Code c8 with(nolock) on DH.BuildingStructureCode = c8.Code
left join FxtDataCenter.dbo.SYS_Code c9 with(nolock) on DH.StructureCode = c9.Code
left join FxtDataCenter.dbo.SYS_Code c10 with(nolock) on DH.BuildingDate = c10.Code
left join FxtDataCenter.dbo.SYS_Code c11 with(nolock) on DH.ZhuangXiu = c11.Code
left join FxtDataCenter.dbo.SYS_Code c12 with(nolock) on DH.LoanedLines = c12.Code
left join FxtDataCenter.dbo.SYS_Code c13 with(nolock) on DH.RightCode = c13.Code
where 1 = 1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_HumanHouse>(strSql, new { cityId, fxtcompanyId }).AsQueryable();
            }
        }

        #region 获取楼栋、房号
        public IQueryable<DAT_Building> GetBuildings(int cityId, int fxtCompanyId, int projectId, int buildingId = -1, bool self = true)
        {
            string ptable, btable, htable, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out btable, out htable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_Building>().AsQueryable();
            }
            if (self) showcomId = fxtCompanyId.ToString();

            ptable = "FxtProject." + ptable;
            btable = "FxtProject." + btable;
            htable = "FxtProject." + htable;

            //查询条件
            var buildingWhere = new StringBuilder();
            if (buildingId > 0)
                buildingWhere.Append(" and BuildingId = @buildingid");

            //查询语句
            var strSql = @"
select BuildingId,BuildingName,RightCode,StructureCode
from " + btable + @" b with(nolock)
where not exists(
	select BuildingId from " + btable + @"_sub bs with(nolock)
	where bs.BuildingId = b.BuildingId
	and bs.CityID = @cityid 
	and bs.Fxt_CompanyId = @fxtcompanyid
)
and Valid = 1
and CityID = @cityid
and FxtCompanyId in (" + showcomId + @")
and ProjectId = @projectId " + buildingWhere + @"
union
select BuildingId,BuildingName,RightCode,StructureCode
from " + btable + @"_sub b with(nolock)
where Valid = 1
and CityID = @cityid
and Fxt_CompanyId = @fxtcompanyid
and ProjectId = @projectId " + buildingWhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_Building>(strSql, new { CityId = cityId, fxtCompanyId = fxtCompanyId, ProjectId = projectId, BuildingId = buildingId }).AsQueryable();
            }
        }

        public IQueryable<DAT_House> GetHouses(int cityId, int fxtCompanyId, int buildingId, int houseId = -1, bool self = true)
        {
            string ptable, btable, htable, showcomId;
            Access(cityId, fxtCompanyId, out ptable, out btable, out htable, out showcomId);
            if (string.IsNullOrEmpty(showcomId)) showcomId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<DAT_House>().AsQueryable();
            }
            if (self) showcomId = fxtCompanyId.ToString();

            ptable = "FxtProject." + ptable;
            btable = "FxtProject." + btable;
            htable = "FxtProject." + htable;

            //查询条件
            var where = new StringBuilder();
            if (houseId > 0)
                where.Append(" and HouseId = @houseid");

            //查询语句
            var strSql = @"
select HouseId,HouseName,BuildArea,StructureCode from " + htable + @" h with(nolock)
where not exists(
	select HouseId from " + htable + @"_sub hs with(nolock)
	where hs.HouseId = h.HouseId
	and hs.CityID = @cityid 
	and hs.FxtCompanyId = @fxtcompanyid
)
and Valid = 1
and CityID = @cityid
and FxtCompanyId in (" + showcomId + @")
and BuildingId = @buildingid " + where + @"
union
select HouseId,HouseName,BuildArea,StructureCode from " + htable + @"_sub h with(nolock)
where Valid = 1
and CityID = @cityid
and FxtCompanyId = @fxtcompanyid
and BuildingId = @buildingid" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataHuman))
            {
                return conn.Query<DAT_House>(strSql, new { cityId, fxtCompanyId, buildingId, houseId }).AsQueryable();
            }
        }
        #endregion

        #region 公共

        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string btable, out string htable, out string showcomId)
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
                btable = "";
                htable = "";
                showcomId = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                btable = dt.Rows[0]["BuildingTable"].ToString();
                htable = dt.Rows[0]["HouseTable"].ToString();
                showcomId = dt.Rows[0]["ShowCompanyId"].ToString();
            }
        }

        #endregion
    }
}
