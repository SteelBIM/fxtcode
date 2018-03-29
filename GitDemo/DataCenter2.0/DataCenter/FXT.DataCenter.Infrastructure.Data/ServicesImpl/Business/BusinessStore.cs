using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class BusinessStore : IBusinessStore
    {
        #region 商铺

        public IQueryable<Dat_Tenant_Biz> GetTenantBiz(Dat_Tenant_Biz tenantBiz, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {

            string ptable, ctable, btable, comId;
            Access(tenantBiz.CityId, tenantBiz.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = tenantBiz.FxtCompanyId.ToString();
            if (self) comId = tenantBiz.FxtCompanyId.ToString();

            #region 查询条件

            var filterForProjectName = string.Empty;
            var filterForBuildingName = string.Empty;
            var filterForStoreName = string.Empty;
            var filterForAreaId = string.Empty;
            var filterForSubAreaBizId = string.Empty;

            if (tenantBiz.AreaId > 0)
            {
                filterForAreaId = " and p.AreaId = @AreaId";
            }

            if (tenantBiz.SubAreaBizId > 0)
            {
                filterForSubAreaBizId = " and p.SubAreaId = @SubAreaBizId";
            }

            if (!string.IsNullOrEmpty(tenantBiz.ProjectName))
            {
                tenantBiz.ProjectName = "%" + tenantBiz.ProjectName + "%";
                filterForProjectName = " and p.projectName like @projectName";
            }

            if (!string.IsNullOrEmpty(tenantBiz.BuildingName))
            {
                tenantBiz.BuildingName = "%" + tenantBiz.BuildingName + "%";
                filterForBuildingName = " and b.BuildingName like @buildingName";
            }

            if (!string.IsNullOrEmpty(tenantBiz.BizHouseName))
            {
                tenantBiz.BizHouseName = "%" + tenantBiz.BizHouseName + "%";
                filterForStoreName = " and p.BizHouseName like @BizHouseName";
            }

            #endregion

            var strSql = new StringBuilder();

            #region 商铺查询SQL

            strSql.Append(@"select tb.*,pb.AreaName,pb.SubAreaName,pb.ProjectName,bb.BuildingName
                           from  (
	                            select p.*
		                            ,c.CodeName as RentTypeName
		                            ,c1.CodeName as BizTypeName
		                            ,c2.CodeName as BizName
		                            ,c3.CodeName as BizSubName
	                            from FxtData_Biz.dbo.Dat_Tenant_Biz p with (nolock)
	                            left join FxtDataCenter.dbo.SYS_Code c with (nolock) on p.RentTypeCode = c.Code
	                            left join FxtDataCenter.dbo.SYS_Code c1 with (nolock) on p.BizType = c1.Code
	                            left join FxtDataCenter.dbo.SYS_Code c2 with (nolock) on p.BizCode = c2.Code
	                            left join FxtDataCenter.dbo.SYS_Code c3 with (nolock) on p.BizSubCode = c3.Code
	                            where not exists (
			                            select HouseTenantId
			                            from FxtData_Biz.dbo.Dat_Tenant_Biz_Sub p1 with (nolock)
			                            where p1.projectId = p.projectId
				                            and p1.buildingId = p.buildingId
				                            and p1.houseId = p.houseId
				                            and p1.cityId = p.cityId
				                            and p1.fxtCompanyId = @fxtCompanyId
			                            )
		                            and p.valid = 1
		                            and p.CityId = @CityId
		                            and p.FxtCompanyId in (" + comId + @")
	                                " + filterForStoreName + @"
	                            union
	
	                            select p.*
		                            ,c.CodeName as RentTypeName
		                            ,c1.CodeName as BizTypeName
		                            ,c2.CodeName as BizName
		                            ,c3.CodeName as BizSubName
	                            from FxtData_Biz.dbo.Dat_Tenant_Biz_sub p with (nolock)
	                            left join FxtDataCenter.dbo.SYS_Code c with (nolock) on p.RentTypeCode = c.Code
	                            left join FxtDataCenter.dbo.SYS_Code c1 with (nolock) on p.BizType = c1.Code
	                            left join FxtDataCenter.dbo.SYS_Code c2 with (nolock) on p.BizCode = c2.Code
	                            left join FxtDataCenter.dbo.SYS_Code c3 with (nolock) on p.BizSubCode = c3.Code
	                            where p.valid = 1
		                            and p.CityId = @CityId
		                            and p.FxtCompanyId = @fxtCompanyId
                                    " + filterForStoreName + @"
	                            ) tb");
            #endregion

            #region 关联商业街SQL

            strSql.Append(@"  inner join (
	                            select projectId,ProjectName,a.AreaName,sa.SubAreaName
	                            from FxtData_Biz.dbo.Dat_Project_Biz p with (nolock)
	                            LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
	                            LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sa WITH (NOLOCK) ON p.SubAreaId = sa.SubAreaId
	                            where p.valid = 1
		                            and p.CityId = @cityId

			                             " + filterForProjectName + filterForSubAreaBizId + filterForAreaId + @"
			                            
		                            and p.FxtCompanyId in (" + comId + @")
		                            and not exists (
			                            select ProjectId
			                            from FxtData_Biz.dbo.Dat_Project_Biz_sub p1 with (nolock)
			                            where p1.areaId = p.areaId
				                            and p1.cityId = p.cityId
				                            and p1.fxtCompanyId = @fxtCompanyId
				                            and p1.projectId = p.projectId
			                            )
	
	                            union
	
	                            select projectId,ProjectName,a.AreaName,sa.SubAreaName
	                            from FxtData_Biz.dbo.Dat_Project_Biz_Sub p with (nolock)
	                            LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
	                            LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sa WITH (NOLOCK) ON p.SubAreaId = sa.SubAreaId
	                            where p.valid = 1
		                            and p.CityId = @cityId

			                           " + filterForProjectName + filterForSubAreaBizId + filterForAreaId + @"
			                            
		                            and p.FxtCompanyId = @fxtCompanyId
	                            ) pb
                                on pb.projectId = tb.projectId");
            #endregion

            #region 关联商业楼栋

            strSql.Append(@" inner join (
	                            select projectId
		                            ,BuildingId,BuildingName
	                            from FxtData_Biz.dbo.Dat_Building_Biz b with (nolock)
	                            where b.valid = 1
		                            and b.CityId = @cityId		                            
			                            " + filterForBuildingName + @"			                            
		                            and b.FxtCompanyId in (" + comId + @")
		                            and not exists (
			                            select BuildingId
			                            from FxtData_Biz.dbo.Dat_Building_Biz_sub b1 with (nolock)
			                            where b1.cityId = b.cityId
				                            and b1.fxtCompanyId = @fxtCompanyId
				                            and b1.BuildingId = b.BuildingId
			                            )	
	                            union	
	                            select projectId
		                            ,BuildingId,BuildingName
	                            from FxtData_Biz.dbo.Dat_Building_Biz_sub b with (nolock)
	                            where b.valid = 1
		                            and b.CityId = @cityId		                            
			                           " + filterForBuildingName + @"			                            
		                            and b.FxtCompanyId = @fxtCompanyId
	                            ) bb on bb.ProjectId = pb.ProjectId 
                                and bb.buildingId = tb.buildingId");

            #endregion

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.HouseTenantId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                totalCount = conn.Query<int>(totalCountSql, tenantBiz).FirstOrDefault();
                return conn.Query<Dat_Tenant_Biz>(pagenatedSql, tenantBiz).AsQueryable();
            }
        }

        public Dat_Tenant_Biz GetTenantBizById(int id, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strSql = @"select p.* 
	                            from FxtData_Biz.dbo.Dat_Tenant_Biz p with (nolock)
	                            where not exists (
			                            select HouseTenantId
			                            from FxtData_Biz.dbo.Dat_Tenant_Biz_Sub p1 with (nolock)
			                            where p1.projectId = p.projectId
				                            and p1.buildingId = p.buildingId
				                            and p1.houseId = p.houseId
				                            and p1.cityId = p.cityId
				                            and p1.fxtCompanyId = @fxtCompanyId
			                            )
		                            and p.valid = 1
		                            and p.CityId = @CityId
		                            and p.FxtCompanyId in (" + comId + @")
                                    and p.HouseTenantId = @id 	                                
	                            union	
	                            select p.*
	                            from FxtData_Biz.dbo.Dat_Tenant_Biz p with (nolock)
	                            where p.valid = 1
		                            and p.CityId = @CityId
		                            and p.FxtCompanyId = @fxtCompanyId
                                    and p.HouseTenantId = @id ";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<Dat_Tenant_Biz>(strSql, new { id, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddTenantBiz(Dat_Tenant_Biz tenantBiz)
        {
            var strSql = @"insert into FxtData_Biz.dbo.Dat_Tenant_Biz (cityid,projectid,buildingid,houseid,isvacant,buildingarea,rent,renttypecode,saleunitprice,bizhousename,brandname,tenantid,biztype,bizcode,bizsubcode,joindate,surveydate,surveyuser,fxtcompanyid,creator,savedatetime,saveuser,remarks,istypical) 
values(@cityid,@projectid,@buildingid,@houseid,@isvacant,@buildingarea,@rent,@renttypecode,@saleunitprice,@bizhousename,@brandname,@tenantid,@biztype,@bizcode,@bizsubcode,@joindate,@surveydate,@surveyuser,@fxtcompanyid,@creator,@savedatetime,@saveuser,@remarks,@istypical)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, tenantBiz);
            }
        }

        public int UpdateTenantBiz(Dat_Tenant_Biz tenantBiz, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            tenantBiz.FxtCompanyId = currentCompanyId;

            var strSqlMainUpdate = @"update FxtData_Biz.dbo.Dat_Tenant_Biz set isvacant = @isvacant,buildingarea = @buildingarea,rent = @rent,renttypecode = @renttypecode,saleunitprice = @saleunitprice,bizhousename = @bizhousename,brandname = @brandname,tenantid = @tenantid,biztype = @biztype,bizcode = @bizcode,bizsubcode = @bizsubcode,joindate = @joindate,surveydate = @surveydate,surveyuser = @surveyuser,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks,istypical = @istypical
where housetenantid = @housetenantid and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";

            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.Dat_Tenant_Biz_Sub (houseTenantId,cityid,projectid,buildingid,houseid,isvacant,buildingarea,rent,renttypecode,saleunitprice,bizhousename,brandname,tenantid,biztype,bizcode,bizsubcode,joindate,surveydate,surveyuser,fxtcompanyid,creator,savedatetime,saveuser,remarks,istypical) 
values(@houseTenantId,@cityid,@projectid,@buildingid,@houseid,@isvacant,@buildingarea,@rent,@renttypecode,@saleunitprice,@bizhousename,@brandname,@tenantid,@biztype,@bizcode,@bizsubcode,@joindate,@surveydate,@surveyuser,@fxtcompanyid,@creator,@savedatetime,@saveuser,@remarks,@istypical)";

            var strSqlSubUpdate = @"update FxtData_Biz.dbo.Dat_Tenant_Biz_Sub set isvacant = @isvacant,buildingarea = @buildingarea,rent = @rent,renttypecode = @renttypecode,saleunitprice = @saleunitprice,bizhousename = @bizhousename,brandname = @brandname,tenantid = @tenantid,biztype = @biztype,bizcode = @bizcode,bizsubcode = @bizsubcode,joindate = @joindate,surveydate = @surveydate,surveyuser = @surveyuser,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks,istypical = @istypical
where housetenantid = @housetenantid and fxtCompanyId= @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainUpdate, tenantBiz);
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubUpdate, tenantBiz);
                return r1 == 0 ? conn.Execute(strSqlSubAdd, tenantBiz) : r1;
            }
        }

        public int DeleteTenantBiz(Dat_Tenant_Biz tenantBiz, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            tenantBiz.FxtCompanyId = currentCompanyId;
            tenantBiz.Valid = 0;

            var strSqlMainDelete = @"Update FxtData_Biz.dbo.Dat_Tenant_Biz  with(rowlock) set valid = 0 where housetenantid = @housetenantid and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";
            var strSqlSubDelete = @"Update FxtData_Biz.dbo.Dat_Tenant_Biz_Sub  with(rowlock) set valid = 0 where housetenantid = @housetenantid and fxtCompanyId= @fxtCompanyId ";
            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.Dat_Tenant_Biz_Sub (houseTenantId,cityid,projectid,buildingid,houseid,isvacant,buildingarea,rent,renttypecode,saleunitprice,bizhousename,brandname,tenantid,biztype,bizcode,bizsubcode,joindate,surveydate,surveyuser,fxtcompanyid,creator,savedatetime,saveuser,remarks,istypical) 
values(@houseTenantId,@cityid,@projectid,@buildingid,@houseid,@isvacant,@buildingarea,@rent,@renttypecode,@saleunitprice,@bizhousename,@brandname,@tenantid,@biztype,@bizcode,@bizsubcode,@joindate,@surveydate,@surveyuser,@fxtcompanyid,@creator,@savedatetime,@saveuser,@remarks,@istypical)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainDelete, new { tenantBiz.HouseTenantId, tenantBiz.FxtCompanyId });
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubDelete, new { tenantBiz.HouseTenantId, tenantBiz.FxtCompanyId });
                return r1 == 0 ? conn.Execute(strSqlSubAdd, tenantBiz) : r1;

            }
        }

        #endregion

        #region 商铺图片

        public IQueryable<LNK_H_Photo> GetBusinessStorePhotoes(LNK_H_Photo lnkPPhoto, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(lnkPPhoto.CityId, lnkPPhoto.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = lnkPPhoto.FxtCompanyId.ToString();
            if (self) comId = lnkPPhoto.FxtCompanyId.ToString();

            var strSql = @"select p.*,c.CodeName as PhotoTypeName
                            from FxtData_Biz.dbo.LNK_H_Photo p with(nolock)
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.PhotoTypeCode = c.Code
                            where not exists (select id from FxtData_Biz.dbo.LNK_H_Photo_Sub p1 with(nolock) where p1.cityId=p.cityId and p1.fxtCompanyId = @fxtCompanyId  and p1.TenantId=p.TenantId)
                            and p.valid = 1 and  p.CityId = @CityId and p.FxtCompanyId in(" + comId + @")
                            and p.HouseId=@houseId and p.TenantId=@tenantId
                            union 
                            select  p.*,c.CodeName as PhotoTypeName
                            from FxtData_Biz.dbo.LNK_H_Photo_Sub p with(nolock)
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.PhotoTypeCode = c.Code
                            where p.valid = 1 and p.CityId = @CityId and p.FxtCompanyId =@fxtCompanyId
                            and p.HouseId=@houseId and p.TenantId=@tenantId
                            order by 1 desc";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<LNK_H_Photo>(strSql, lnkPPhoto).AsQueryable();
            }
        }

        public LNK_H_Photo GetBusinessStorePhoto(int id, int fxtCompanyId)
        {
            var strSql = @"
SELECT p.*
	,c.CodeName AS PhotoTypeName
FROM FxtData_Biz.dbo.LNK_H_Photo p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE NOT EXISTS (
		SELECT id
		FROM FxtData_Biz.dbo.LNK_H_Photo_Sub p1 WITH (NOLOCK)
		WHERE p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
			AND p1.TenantId = p.TenantId
		)
	AND p.valid = 1
	AND p.id = @id
UNION
SELECT p.*
	,c.CodeName AS PhotoTypeName
FROM FxtData_Biz.dbo.LNK_H_Photo_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE p.valid = 1
	AND p.id = @id
	AND p.FxtCompanyId = @fxtCompanyId
ORDER BY 1 DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<LNK_H_Photo>(strSql, new { id, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddBusinessStorePhoto(LNK_H_Photo lnkPPhoto)
        {
            var strSql = @"insert into FxtData_Biz.dbo.LNK_H_Photo  (HouseId,TenantId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@HouseId,@TenantId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Execute(strSql, lnkPPhoto);
            }
        }

        public int UpdateBusinessStorePhoto(LNK_H_Photo lnkPPhoto, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            lnkPPhoto.FxtCompanyId = currentCompanyId;

            var strSqlMainUpdate = @"update FxtData_Biz.dbo.LNK_H_Photo  set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";
            var strSqlSubUpdate = @"update FxtData_Biz.dbo.LNK_H_Photo_Sub set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and fxtCompanyId= @fxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.LNK_H_Photo  (id,HouseId,TenantId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate,valid) 
values(@id,@HouseId,@TenantId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate,@valid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainUpdate, lnkPPhoto);
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubUpdate, lnkPPhoto);
                return r1 == 0 ? conn.Execute(strSqlSubAdd, lnkPPhoto) : r1;
            }
        }

        public int DeleteBusinessStorePhoto(LNK_H_Photo lnkPPhoto, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            lnkPPhoto.FxtCompanyId = currentCompanyId;
            lnkPPhoto.Valid = 0;

            var strSqlMainDelete = @"Update FxtData_Biz.dbo.LNK_H_Photo  with(rowlock) set valid = 0 where id = @id and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";
            var strSqlSubDelete = @"Update FxtData_Biz.dbo.LNK_H_Photo_Sub  with(rowlock) set valid = 0 where id = @id and fxtCompanyId= @fxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.LNK_H_Photo  (id,HouseId,TenantId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@id,@HouseId,@TenantId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainDelete, new { lnkPPhoto.Id, lnkPPhoto.FxtCompanyId });
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubDelete, new { lnkPPhoto.Id, lnkPPhoto.FxtCompanyId });
                return r1 == 0 ? conn.Execute(strSqlSubAdd, lnkPPhoto) : r1;
            }
        }

        #endregion

        #region 公共
        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.BizCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

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
                comId = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                ctable = dt.Rows[0]["CaseTable"].ToString();
                btable = dt.Rows[0]["BuildingTable"].ToString();
                comId = dt.Rows[0]["BizCompanyId"].ToString();
            }

        }
        #endregion



    }
}
