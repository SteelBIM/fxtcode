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
    public class ProjectWeightRevised : IProjectWeightRevised
    {
        //已经更新
        public IQueryable<DatWeightProject> GetWeightProjects(string projectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(datWeightProject.FxtCompanyId, typecode, datWeightProject.CityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, btable, wptable;
            Access(CityId, out ptable, out btable, out wptable);

            ptable = "FXTProject." + ptable;
            btable = "FXTProject." + btable;
            wptable = "FXTProject." + wptable;

            var day = int.Parse(ConfigurationHelper.AvgPriceUpdatedDate);
            var point = DateTime.Now.ToString("yyyy-MM") + "-" + day + " 00:00:00";

            var where = string.Empty;
            if (!string.IsNullOrEmpty(projectName)) where += " and ( T.ProjectName like @ProjectName or OtherName like @ProjectName)";
            if (type == 0) where += " and wp.updateDate >= '" + point + "'";
            if (type == 1) where += " and(wp.updateDate < '" + point + "' or wp.updateDate is null)";

            var strSql = @"
SELECT 
	T.CityID
	,T.AreaID
	,c.CityName
	,a.AreaName
	,T.ProjectId
	,ProjectName
    ,OtherName
	,wp.Id
	,wp.FxtCompanyId
	,ROUND(wp.ProjectAvgPrice,0) as ProjectAvgPrice
	,wp.LowLayerPrice
	,wp.MultiLayerPrice
	,wp.SmallHighLayerPrice
	,wp.HighLayerPrice
	,wp.SingleVillaPrice
	,wp.PlatoonVillaPrice
    ,wp.SuperpositionVillaPrice
    ,wp.DuplexesVillaPrice
	,wp.MoveBackHousePrice
	,wp.UpdateDate
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
	) T
LEFT JOIN (
	select * from " + wptable + @" with(nolock)
	where CityId = @cityId
	and FxtCompanyId = @configfxtCompanyId
)wp ON wp.ProjectId = T.ProjectId
left join FxtDataCenter.dbo.SYS_City c with(nolock) on T.CityID = c.CityId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
	where 1 = 1" + where;

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
                totalCount = conn.Query<int>(totalCountSql, new { ProjectName = "%" + projectName + "%", CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).FirstOrDefault();
                return conn.Query<DatWeightProject>(pagenatedSql, new { ProjectName = "%" + projectName + "%", CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).AsQueryable();
            }
        }

        //已经更新
        public DatWeightProject GetWeightProject(int projectId, int cityId, int parentFxtCompanyId, int parentProductTypeCode)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(fxtCompanyId, typecode, cityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, btable, wptable;
            Access(cityId, out ptable, out btable, out wptable);

            ptable = "FXTProject." + ptable;
            btable = "FXTProject." + btable;
            wptable = "FXTProject." + wptable;

            var strSql = @"
DECLARE @T TABLE (
	CityID INT
	,AreaID INT
	,ProjectId INT
	,ProjectName NVARCHAR(500)
	,BuildingTypeCodeName NVARCHAR(500)
	)

INSERT INTO @T
SELECT p1.CityID,p1.AreaID,p1.ProjectId,p1.ProjectName,(SELECT CodeName FROM FxtDataCenter.dbo.SYS_Code c WHERE c.Code = b.BuildingTypeCode) AS BuildingTypeCodeName
FROM (
	SELECT p.CityID,p.AreaId,p.ProjectId,p.ProjectName
	FROM " + ptable + @" p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		and p.ProjectId = @projectId
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
		AND p.Fxt_CompanyId = @fxtCompanyId
		and p.ProjectId = @projectId
	) p1
LEFT JOIN (
	SELECT b.ProjectId
		,b.BuildingTypeCode
	FROM " + btable + @" b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityID = @cityId
		and b.ProjectId = @projectId
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(b.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT BuildingId
			FROM " + btable + @"_sub bs WITH (NOLOCK)
			WHERE b.BuildingId = bs.BuildingId
				AND bs.CityID = @cityId
				AND bs.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT b.ProjectId
		,b.BuildingTypeCode
	FROM " + btable + @"_sub b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityID = @cityId
		AND b.Fxt_CompanyId = @fxtCompanyId
		and b.ProjectId = @projectId
	) b ON p1.ProjectId = b.ProjectId
    where 1 = 1 
SELECT 
	T.CityID
	,T.AreaID
	,(select CityName from FxtDataCenter.dbo.SYS_City c where c.CityId = T.CityID) as CityName
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a where a.AreaId = T.AreaID) as AreaName
	,T.ProjectId
	,ProjectName
	,CASE 
		WHEN LEN(BuildingTypeCodeName) > 0
			THEN LEFT(BuildingTypeCodeName, LEN(BuildingTypeCodeName) - 1)
		ELSE BuildingTypeCodeName
		END AS BuildingTypeCodeName
	,wp.Id
	,wp.FxtCompanyId
	,ROUND(wp.ProjectAvgPrice,0) as ProjectAvgPrice
	,wp.LowLayerPrice
	,wp.MultiLayerPrice
	,wp.SmallHighLayerPrice
	,wp.HighLayerPrice
	,wp.SingleVillaPrice
	,wp.PlatoonVillaPrice
    ,wp.SuperpositionVillaPrice
    ,wp.DuplexesVillaPrice
	,wp.MoveBackHousePrice
	,wp.UpdateDate
FROM (
	SELECT CityID
		,AreaID
		,ProjectId
		,ProjectName
		,(
			SELECT BuildingTypeCodeName + ','
			FROM @T
			WHERE CityID = t.CityID
				AND AreaID = t.AreaID
				AND ProjectId = t.ProjectId
				AND ProjectName = t.ProjectName
			FOR XML path('')
			) AS BuildingTypeCodeName
	FROM @T t
	GROUP BY CityID
		,AreaID
		,ProjectId
		,ProjectName
	) T
LEFT JOIN (
	select * from " + wptable + @" WITH (NOLOCK)
	where ProjectId = @projectId
	and CityId = @cityid
	and FxtCompanyId = @configfxtCompanyId
)wp ON wp.ProjectId = T.ProjectId
	AND wp.CityId = T.CityID
	where 1 = 1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DatWeightProject>(strSql, new { projectId, cityId, fxtCompanyId = parentFxtCompanyId, typecode = parentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).AsQueryable().FirstOrDefault();
            }
        }

        //已经更新
        public IQueryable<DatWeightProject> GetNotUpdatedAvrPriceProjects(string projectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, bool self = true)
        {
            var day = int.Parse(ConfigurationHelper.AvgPriceUpdatedDate);
            var point = DateTime.Now.ToString("yyyy-MM") + "-" + day;

            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(datWeightProject.FxtCompanyId, typecode, datWeightProject.CityId, out parentFxtCompanyId, out parentProductTypeCode);

            string ptable, btable, wptable;
            Access(CityId, out ptable, out btable, out wptable);

            var pwhere = string.Empty;
            if (!string.IsNullOrEmpty(projectName)) pwhere += " and (p1.ProjectName like @ProjectName or p1.OtherName like @ProjectName)";

            var where1 = string.Empty;
            if (type == 0) where1 += " and wp.updateDate >= '" + point + "'";
            if (type == 1) where1 += " and(wp.updateDate < '" + point + "' or wp.updateDate is null)";

            var strSql = @"
DECLARE @T TABLE (
	CityID INT
	,AreaID INT
	,ProjectId INT
	,ProjectName NVARCHAR(500)
    ,OtherName NVARCHAR(500)
	,BuildingTypeCodeName NVARCHAR(500)
	)

INSERT INTO @T
SELECT p1.CityID,p1.AreaID,p1.ProjectId,p1.ProjectName,p1.OtherName,(SELECT CodeName FROM FxtDataCenter.dbo.SYS_Code c WHERE c.Code = b.BuildingTypeCode) AS BuildingTypeCodeName
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
	) p1
LEFT JOIN (
	SELECT b.ProjectId
		,b.BuildingTypeCode
	FROM " + btable + @" b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityID = @cityId
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(b.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT BuildingId
			FROM " + btable + @"_sub bs WITH (NOLOCK)
			WHERE b.BuildingId = bs.BuildingId
				AND bs.CityID = @cityId
				AND bs.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT b.ProjectId
		,b.BuildingTypeCode
	FROM " + btable + @"_sub b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityID = @cityId
		AND b.Fxt_CompanyId = @fxtCompanyId
	) b ON p1.ProjectId = b.ProjectId
    where 1 = 1 " + pwhere + @"
SELECT 
	T.CityID
	,T.AreaID
	,(select CityName from FxtDataCenter.dbo.SYS_City c where c.CityId = T.CityID) as CityName
	,(select AreaName from FxtDataCenter.dbo.SYS_Area a where a.AreaId = T.AreaID) as AreaName
	,T.ProjectId
	,ProjectName
    ,OtherName
	,CASE 
		WHEN LEN(BuildingTypeCodeName) > 0
			THEN LEFT(BuildingTypeCodeName, LEN(BuildingTypeCodeName) - 1)
		ELSE BuildingTypeCodeName
		END AS BuildingTypeCodeName
	,wp.Id
	,wp.FxtCompanyId
	,ROUND(wp.ProjectAvgPrice,0) as ProjectAvgPrice
	,wp.LowLayerPrice
	,wp.MultiLayerPrice
	,wp.SmallHighLayerPrice
	,wp.HighLayerPrice
	,wp.SingleVillaPrice
	,wp.PlatoonVillaPrice
    ,wp.SuperpositionVillaPrice
    ,wp.DuplexesVillaPrice
	,wp.MoveBackHousePrice
	,wp.UpdateDate
	,wp.UpdateUser
FROM (
	SELECT CityID
		,AreaID
		,ProjectId
		,ProjectName
        ,OtherName
		,(
			SELECT BuildingTypeCodeName + ','
			FROM @T
			WHERE CityID = t.CityID
				AND AreaID = t.AreaID
				AND ProjectId = t.ProjectId
				AND ProjectName = t.ProjectName
			FOR XML path('')
			) AS BuildingTypeCodeName
	FROM @T t
	GROUP BY CityID
		,AreaID
		,ProjectId
		,ProjectName,OtherName
	) T
LEFT JOIN " + wptable + @" wp WITH (NOLOCK) ON wp.ProjectId = T.ProjectId
	AND wp.CityId = T.CityID
	AND wp.FxtCompanyId = @configfxtCompanyId
	where 1 = 1" + where1;
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DatWeightProject>(strSql, new { ProjectName = "%" + projectName + "%", CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).AsQueryable();
            }
        }

        //已经更新
        public int UpdateWeightProject(DatWeightProject datWeightProject)
        {
            string ptable, btable, wptable;
            Access(datWeightProject.CityId, out ptable, out btable, out wptable);

            var strSql = @"
update FXTProject." + wptable + @" with(rowlock) 
set projectavgprice = @projectavgprice
,lowlayerprice = @lowlayerprice
,multilayerprice = @multilayerprice
,smallhighlayerprice = @smallhighlayerprice
,highlayerprice = @highlayerprice
,singlevillaprice = @singlevillaprice
,platoonvillaprice = @platoonvillaprice
,SuperpositionVillaPrice = @SuperpositionVillaPrice
,DuplexesVillaPrice = @DuplexesVillaPrice
,movebackhouseprice = @movebackhouseprice
,EvaluationCompanyId=@EvaluationCompanyId
,updatedate = getdate()
,updateuser = @updateuser
where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightProject);
            }
        }

        //已经更新
        public int AddWeightProject(DatWeightProject datWeightProject)
        {
            string ptable, btable, wptable;
            Access(datWeightProject.CityId, out ptable, out btable, out wptable);

            var strSql = @"insert into FXTProject." + wptable + @" (fxtcompanyid,cityid,projectid,lowlayerweight,multilayerweight,smallhighlayerweight,highlayerweight,projectavgprice,lowlayerprice,multilayerprice,smallhighlayerprice,highlayerprice,singlevillaprice,platoonvillaprice,SuperpositionVillaPrice,DuplexesVillaPrice,movebackhouseprice,updatedate,evaluationcompanyid,UpdateUser) 
values(@fxtcompanyid,@cityid,@projectid,@lowlayerweight,@multilayerweight,@smallhighlayerweight,@highlayerweight,@projectavgprice,@lowlayerprice,@multilayerprice,@smallhighlayerprice,@highlayerprice,@singlevillaprice,@platoonvillaprice,@SuperpositionVillaPrice,@DuplexesVillaPrice,@movebackhouseprice,getdate(),@evaluationcompanyid,@UpdateUser)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightProject);
            }
        }

        private static void Access(int cityid, out string ptable, out string btable, out string wptable)
        {
            string sql = @"SELECT ProjectTable,BuildingTable,WeightProject FROM FxtDataCenter.dbo.SYS_City_Table WHERE CityId = @cityid";
            SqlParameter[] parameter = { new SqlParameter("@cityid", SqlDbType.Int) };
            parameter[0].Value = cityid;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
            if (dt.Rows.Count == 0)
            {
                ptable = "";
                btable = "";
                wptable = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                btable = dt.Rows[0]["BuildingTable"].ToString();
                wptable = dt.Rows[0]["WeightProject"].ToString();
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
    }
}
