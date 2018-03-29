using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ProjectAvgPrice : IProjectAvgPrice
    {
        public IQueryable<Dat_ProjectAvg> GetProjectAvgPrices(string ProjectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth, int IsPrices, int pageIndex, int pageSize, out int totalCount)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(datProjectAvg.FxtCompanyId, typecode, datProjectAvg.CityID, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, pavgtable, phistoryavgtable, weightprojecttable;
            Access(CityId, out ptable, out pavgtable, out phistoryavgtable);
            ptable = "FXTProject." + ptable;
            pavgtable = "FXTProject." + pavgtable;
            phistoryavgtable = "FXTProject." + phistoryavgtable;
            weightprojecttable = ptable.Replace("dbo.DAT_Project", "dbo.DAT_WeightProject");

            string paTable = useMonth != Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")) ? phistoryavgtable : pavgtable;

            var pwhere = string.Empty;
            if (!string.IsNullOrEmpty(ProjectName)) pwhere += " and (P.ProjectName like @ProjectName or P.OtherName like @ProjectName)";
            if (IsPrices == 1) pwhere += " and pa.ProjectAvgPrice > 0";
            if (IsPrices == 0) pwhere += " and (pa.ProjectAvgPrice = 0 or pa.ProjectAvgPrice is null)";

            var strSql = @"
SELECT P.CityID
	,P.AreaID
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = P.AreaID) as AreaName
	,P.ProjectId
	,P.ProjectName
	,p.OtherName
	,pa.ProjectAvgId
	,pa.ProjectAvgPrice
	,pa.CaseCount
	,pa.CaseMinPrice
	,pa.ProjectGained
    ,@useMonth as UseMonth
	,pa.UpdateDate
	,pa.UpdateUser
	,pa.IsEValue
	,(case when pa.IsEValue = 1 then '是' when pa.IsEValue = 0 then '否' else '' end) as IsEValueName
	,ROUND(wp.ProjectAvgPrice,0) as WeightProjectPrice
FROM (
	SELECT p.CityID,p.AreaId,p.ProjectId,p.ProjectName,OtherName
	FROM " + ptable + @" p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(p.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 WITH (NOLOCK)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT p.CityID,p.AreaId,p.ProjectId,p.ProjectName,OtherName
	FROM " + ptable + @"_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) P
left join " + paTable + @" pa with(nolock) on pa.CityId = @cityid and pa.FxtCompanyId = @configfxtCompanyId and P.ProjectId = pa.ProjectId and UseMonth = @useMonth
left join (select * from " + weightprojecttable + @" with(nolock) where CityId = @cityId and FxtCompanyId = @configfxtCompanyId)wp on wp.ProjectId = P.ProjectId
WHERE 1 = 1" + pwhere;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.ProjectId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                totalCount = conn.Query<int>(totalCountSql, new { ProjectName = "%" + ProjectName + "%", CityId, useMonth, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).FirstOrDefault();
                return conn.Query<Dat_ProjectAvg>(pagenatedSql, new { ProjectName = "%" + ProjectName + "%", CityId, useMonth, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).AsQueryable();
            }
        }

        public IQueryable<Dat_ProjectAvg> ExportProjectAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, string ProjectName, DateTime useMonth, int IsPrices)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(FxtCompanyId, typecode, CityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, pavgtable, phistoryavgtable, weightprojecttable;
            Access(CityId, out ptable, out pavgtable, out phistoryavgtable);
            ptable = "FXTProject." + ptable;
            pavgtable = "FXTProject." + pavgtable;
            phistoryavgtable = "FXTProject." + phistoryavgtable;
            weightprojecttable = ptable.Replace("dbo.DAT_Project", "dbo.DAT_WeightProject");

            string paTable = useMonth != Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")) ? phistoryavgtable : pavgtable;

            var pwhere = string.Empty;
            if (!string.IsNullOrEmpty(ProjectName)) pwhere += " and (P.ProjectName like @ProjectName or P.OtherName like @ProjectName)";
            if (IsPrices == 1) pwhere += " and pa.ProjectAvgPrice > 0";
            if (IsPrices == 0) pwhere += " and (pa.ProjectAvgPrice = 0 or pa.ProjectAvgPrice is null)";

            var strSql = @"
SELECT P.CityID
	,P.AreaID
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = P.AreaID) as AreaName
	,P.ProjectId
	,P.ProjectName
	,P.OtherName
	,pa.ProjectAvgId
	,pa.ProjectAvgPrice
	,CONVERT(numeric(18,2),pa.ProjectGained * 100) as ProjectGained
	,'案例条数：' + CONVERT(nvarchar(10),pa.CaseCount) + '条；案例均价：' + CONVERT(nvarchar(10),pa.CaseMinPrice) + '元/㎡' as caseremark
	,pa.CreateDate
    ,@useMonth as UseMonth
    ,convert(nvarchar(10),@useMonth,121) as UseMonthN
	,pa.UpdateDate
	,pa.UpdateUser
	,pa.IsEValue
	,(case when pa.IsEValue = 1 then '是' when pa.IsEValue = 0 then '否' else '' end) as IsEValueName
	,ROUND(wp.ProjectAvgPrice,0) as WeightProjectPrice
FROM (
	SELECT p.CityID,p.AreaId,p.ProjectId,p.ProjectName,p.OtherName
	FROM " + ptable + @" p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(p.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 WITH (NOLOCK)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT p.CityID,p.AreaId,p.ProjectId,p.ProjectName,p.OtherName
	FROM " + ptable + @"_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) P
left join " + paTable + @" pa with(nolock) on pa.CityId = @cityid and pa.FxtCompanyId = @configfxtCompanyId and P.ProjectId = pa.ProjectId and UseMonth = @useMonth
left join (select * from " + weightprojecttable + @" with(nolock) where CityId = @cityId and FxtCompanyId = @configfxtCompanyId)wp ON wp.ProjectId = P.ProjectId
WHERE 1 = 1  " + pwhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<Dat_ProjectAvg>(strSql, new { ProjectName = "%" + ProjectName + "%", CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), useMonth }).AsQueryable();
            }
        }

        public Dat_ProjectAvg GetProjectAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(FxtCompanyId, typecode, CityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, pavgtable, phistoryavgtable;
            Access(CityId, out ptable, out pavgtable, out phistoryavgtable);
            ptable = "FXTProject." + ptable;
            pavgtable = "FXTProject." + pavgtable;
            phistoryavgtable = "FXTProject." + phistoryavgtable;

            var strSql = @"
SELECT P.CityID
	,P.AreaID
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = P.AreaID) as AreaName
	,P.ProjectId
	,P.ProjectName
	,pa.ProjectAvgId
	,pa.ProjectAvgPrice
	,pa.ProjectGained
    ,@useMonth as UseMonth
	,pa.UpdateDate
	,pa.UpdateUser
	,pa.IsEValue
FROM (
	SELECT p.CityID
		,p.AreaId
		,p.ProjectId
		,p.ProjectName
	FROM " + ptable + @" p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		and p.ProjectId = @projectid
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(p.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 WITH (NOLOCK)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT p.CityID
		,p.AreaId
		,p.ProjectId
		,p.ProjectName
	FROM " + ptable + @"_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		and p.ProjectId = @projectid
		AND p.Fxt_CompanyId = @fxtCompanyId
	) P
left join " + pavgtable + @" pa with(nolock) on pa.CityId = @cityid and pa.FxtCompanyId = @configfxtCompanyId and P.ProjectId = pa.ProjectId and useMonth = @useMonth
WHERE 1 = 1 ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<Dat_ProjectAvg>(strSql, new { projectId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), useMonth }).AsQueryable().FirstOrDefault();
            }
        }

        public int UpdateProjectAvg(Dat_ProjectAvg datProjectAvg)
        {
            string ptable, pavgtable, phistoryavgtable;
            Access(datProjectAvg.CityID, out ptable, out pavgtable, out phistoryavgtable);
            pavgtable = "FXTProject." + pavgtable;
            var strSql = @"
update " + pavgtable + @" with(rowlock) 
set ProjectAvgPrice = @ProjectAvgPrice,ProjectGained = @ProjectGained,UpdateDate = @UpdateDate,UpdateUser = @UpdateUser,IsEValue = @IsEValue
where CityId = @CityId and ProjectId = @ProjectId and UseMonth = @UseMonth and fxtcompanyid = @fxtcompanyid";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datProjectAvg);
            }
        }

        public int AddProjectAvg(Dat_ProjectAvg datProjectAvg)
        {
            string ptable, pavgtable, phistoryavgtable;
            Access(datProjectAvg.CityID, out ptable, out pavgtable, out phistoryavgtable);
            pavgtable = "FXTProject." + pavgtable;

            var strSql = @"
insert into " + pavgtable + @" (FxtCompanyId,CityId,AreaId,ProjectAvgPrice,ProjectGained,UseMonth,CreateDate,UpdateDate,UpdateUser,ProjectId,valid,casemaxprice,caseminprice,casecount,IsEValue)
values(@FxtCompanyId,@CityId,@AreaId,@ProjectAvgPrice,@ProjectGained,@UseMonth,@CreateDate,@UpdateDate,@UpdateUser,@ProjectId,1,@casemaxprice,@caseminprice,@casecount,@IsEValue)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datProjectAvg);
            }
        }

        public Dat_ProjectAvg GetProjectHistoryAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(FxtCompanyId, typecode, CityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, pavgtable, phistoryavgtable;
            Access(CityId, out ptable, out pavgtable, out phistoryavgtable);
            ptable = "FXTProject." + ptable;
            pavgtable = "FXTProject." + pavgtable;
            phistoryavgtable = "FXTProject." + phistoryavgtable;

            var strSql = @"
SELECT P.CityID
	,P.AreaID
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = P.AreaID) as AreaName
	,P.ProjectId
	,P.ProjectName
	,pa.ProjectAvgId
	,pa.ProjectAvgPrice
	,pa.ProjectGained
	,pa.CaseCount
	,pa.CaseMinPrice
    ,@useMonth as UseMonth
	,pa.UpdateDate
	,pa.UpdateUser
	,pa.IsEValue
FROM (
	SELECT p.CityID
		,p.AreaId
		,p.ProjectId
		,p.ProjectName
	FROM " + ptable + @" p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		and p.ProjectId = @projectid
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(p.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 WITH (NOLOCK)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT p.CityID
		,p.AreaId
		,p.ProjectId
		,p.ProjectName
	FROM " + ptable + @"_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		and p.ProjectId = @projectid
		AND p.Fxt_CompanyId = @fxtCompanyId
	) P
left join " + phistoryavgtable + @" pa with(nolock) on pa.CityId = @cityid and pa.FxtCompanyId = @configfxtCompanyId and P.ProjectId = pa.ProjectId and useMonth = @useMonth
WHERE 1 = 1 ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<Dat_ProjectAvg>(strSql, new { projectId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), useMonth }).AsQueryable().FirstOrDefault();
            }
        }

        public int UpdateProjectHistoryAvg(Dat_ProjectAvg datProjectAvg)
        {
            string ptable, pavgtable, phistoryavgtable;
            Access(datProjectAvg.CityID, out ptable, out pavgtable, out phistoryavgtable);
            phistoryavgtable = "FXTProject." + phistoryavgtable;

            var strHistorySql = @"
update " + phistoryavgtable + @" with(rowlock) 
set ProjectAvgPrice = @ProjectAvgPrice,ProjectGained = @ProjectGained,UpdateDate = @UpdateDate,UpdateUser = @UpdateUser,IsEValue = @IsEValue
where CityId = @CityId and ProjectId = @ProjectId and UseMonth = @UseMonth and fxtcompanyid = @fxtcompanyid";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strHistorySql, datProjectAvg);
            }
        }

        public int AddProjectHistoryAvg(Dat_ProjectAvg datProjectAvg)
        {
            string ptable, pavgtable, phistoryavgtable;
            Access(datProjectAvg.CityID, out ptable, out pavgtable, out phistoryavgtable);
            phistoryavgtable = "FXTProject." + phistoryavgtable;

            var strHistorySql = @"
insert into " + phistoryavgtable + @" (FxtCompanyId,CityId,AreaId,ProjectAvgPrice,ProjectGained,UseMonth,CreateDate,UpdateDate,UpdateUser,ProjectId,valid,casemaxprice,caseminprice,casecount,IsEValue)
values(@FxtCompanyId,@CityId,@AreaId,@ProjectAvgPrice,@ProjectGained,@UseMonth,@CreateDate,@UpdateDate,@UpdateUser,@ProjectId,1,@casemaxprice,@caseminprice,@casecount,@IsEValue)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strHistorySql, datProjectAvg);
            }
        }

        private static void Access(int cityid, out string ptable, out string pavgtable, out string phistoryavgtable)
        {
            string sql = @"SELECT ProjectTable,ProjectAvgTable,ProjectHistoryAvgTable FROM FxtDataCenter.dbo.SYS_City_Table WHERE CityId = @cityid";
            SqlParameter[] parameter = { new SqlParameter("@cityid", SqlDbType.Int) };
            parameter[0].Value = cityid;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
            if (dt.Rows.Count == 0)
            {
                ptable = "";
                pavgtable = "";
                phistoryavgtable = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                pavgtable = dt.Rows[0]["ProjectAvgTable"].ToString();
                phistoryavgtable = dt.Rows[0]["ProjectHistoryAvgTable"].ToString();
            }
        }

        //private static void GetFPInfo(int fxtcompanyid, int producttypecode, int cityid, out int ParentFxtCompanyId, out int ParentProductTypeCode)
        //{
        //    string sql = @"select ParentShowDataCompanyId,ParentProductTypeCode from CompanyProduct where CompanyId = @fxtcompanyid and ProductTypeCode = @producttypecode and cityid = @cityid and Valid = 1";
        //    SqlParameter[] parameter = { new SqlParameter("@fxtcompanyid", SqlDbType.Int), new SqlParameter("@producttypecode", SqlDbType.Int), new SqlParameter("@cityid", SqlDbType.Int) };
        //    parameter[0].Value = fxtcompanyid;
        //    parameter[1].Value = producttypecode;
        //    parameter[2].Value = cityid;

        //    DBHelperSql.ConnectionString = ConfigurationHelper.FxtUserCenter;
        //    var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
        //    if (dt.Rows.Count == 0)
        //    {
        //        ParentFxtCompanyId = 0;
        //        ParentProductTypeCode = 0;
        //    }
        //    else
        //    {
        //        ParentFxtCompanyId = int.Parse(dt.Rows[0]["ParentShowDataCompanyId"].ToString());
        //        ParentProductTypeCode = int.Parse(dt.Rows[0]["ParentProductTypeCode"].ToString());
        //    }
        //}

        public IQueryable<Dat_ProjectAvg> GetProjectHistoryAvgPrices(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime? useMonth, int pageIndex, int pageSize, out int totalCount)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(FxtCompanyId, typecode, CityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, pavgtable, phistoryavgtable;
            Access(CityId, out ptable, out pavgtable, out phistoryavgtable);
            ptable = "FXTProject." + ptable;
            pavgtable = "FXTProject." + pavgtable;
            phistoryavgtable = "FXTProject." + phistoryavgtable;

            var pwhere = string.Empty;
            if (useMonth != null && useMonth != DateTime.MinValue)
            {
                pwhere += " and UseMonth = @usemonth";
            }
            var strSql = @"
select ProjectAvgId,CityId,FxtCompanyId,ProjectId,ProjectAvgPrice,ProjectGained,CaseCount,CaseMinPrice,UseMonth,CreateDate,UpdateDate,UpdateUser,IsEValue,(case when IsEValue = 1 then '是' when IsEValue = 0 then '否' else '' end) as IsEValueName
from " + phistoryavgtable + @"
where CityId = @cityId and FxtCompanyId = @configfxtCompanyId and ProjectId = @projectid" + pwhere;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.ProjectId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                totalCount = conn.Query<int>(totalCountSql, new { projectId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), useMonth }).FirstOrDefault();
                return conn.Query<Dat_ProjectAvg>(pagenatedSql, new { projectId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), useMonth }).AsQueryable();
            }
        }

        public IQueryable<Dat_ProjectAvg> ExportProjectHistoryAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int projectId, DateTime? useMonth)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(FxtCompanyId, typecode, CityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, pavgtable, phistoryavgtable, weightprojecttable;
            Access(CityId, out ptable, out pavgtable, out phistoryavgtable);
            ptable = "FXTProject." + ptable;
            pavgtable = "FXTProject." + pavgtable;
            phistoryavgtable = "FXTProject." + phistoryavgtable;
            weightprojecttable = ptable.Replace("dbo.DAT_Project", "dbo.DAT_WeightProject");

            var pwhere = string.Empty;
            if (useMonth != null && useMonth != DateTime.MinValue)
            {
                pwhere += " and UseMonth = @usemonth";
            }

            var strSql = @"
SELECT P.CityID
	,P.AreaID
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = P.AreaID) as AreaName
	,P.ProjectId
	,P.ProjectName
    ,P.OtherName
	,pa.ProjectAvgId
	,pa.ProjectAvgPrice
	,CONVERT(numeric(18,2),pa.ProjectGained * 100) as ProjectGained
	,'案例条数：' + CONVERT(nvarchar(10),pa.CaseCount) + '条；案例均价：' + CONVERT(nvarchar(10),pa.CaseMinPrice) + '元/㎡' as caseremark
	,pa.CreateDate
    ,pa.UseMonth
    ,convert(nvarchar(10),pa.UseMonth,121) as UseMonthN
	,pa.UpdateDate
	,pa.UpdateUser
	,pa.IsEValue
	,(case when pa.IsEValue = 1 then '是' when pa.IsEValue = 0 then '否' else '' end) as IsEValueName
	,ROUND(wp.ProjectAvgPrice,0) as WeightProjectPrice
FROM (
	SELECT p.CityID,p.AreaId,p.ProjectId,p.ProjectName,p.OtherName
	FROM " + ptable + @" p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(p.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 WITH (NOLOCK)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)
		AND ProjectId = @projectId
	UNION	
	SELECT p.CityID,p.AreaId,p.ProjectId,p.ProjectName,p.OtherName
	FROM " + ptable + @"_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
		AND ProjectId = @projectId
	) P
left join " + phistoryavgtable + @" pa with(nolock) on pa.CityId = @cityid and pa.FxtCompanyId = @configfxtCompanyId and P.ProjectId = pa.ProjectId
left join (select * from " + weightprojecttable + @" with(nolock) where CityId = @cityId and FxtCompanyId = @configfxtCompanyId)wp ON wp.ProjectId = P.ProjectId
WHERE 1 = 1 " + pwhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<Dat_ProjectAvg>(strSql, new { projectId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId), useMonth }).AsQueryable();
            }
        }


    }
}
