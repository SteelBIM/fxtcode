using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.FxtDataBiz;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class BusinessStreet : IBusinessStreet
    {
        #region 商业街道

        public IQueryable<Dat_Project_Biz> GetProjectBizs(Dat_Project_Biz projectBiz, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(projectBiz.CityId, projectBiz.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = projectBiz.FxtCompanyId.ToString();
            if (self) comId = projectBiz.FxtCompanyId.ToString();

            var where = string.Empty;
            if (!(new[] { 0, -1 }).Contains(projectBiz.SubAreaId)) where += "  and p.SubAreaId = @SubAreaId";
            if (!(new[] { 0, -1 }).Contains(projectBiz.AreaId)) where += " and p.AreaId = @AreaId";
            if (!string.IsNullOrEmpty(projectBiz.ProjectName)) where += " and p.ProjectName like @ProjectName";
            // where += "order by p.CreateTime desc";

            //查询全部字段时，select * 与 select a,b...效率差不多
            var strSql = @"select p.*,c.CodeName as CorrelationTypeName,c1.CodeName as TrafficTypeName,a.AreaName,
                            sb.SubAreaName
                            from FxtData_Biz.dbo.Dat_Project_Biz p with(nolock)
                            left join FxtDataCenter.dbo.SYS_SubArea_Biz sb with(nolock) on p.SubAreaId = sb.SubAreaId
                            left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.CorrelationType = c.Code
                            left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on p.TrafficType = c1.Code
                            where not exists 
(select ProjectId from FxtData_Biz.dbo.Dat_Project_Biz_sub p1 with(nolock)  where p1.areaId=p.areaId and  p1.cityId=p.cityId and p1.fxtCompanyId=@fxtCompanyId and p1.projectId = p.projectId)
                            and p.valid = 1 and  p.CityId = @CityId and p.FxtCompanyId in(" + comId + @")
                            " + where + @"
                            union 
                            select p.*,c.CodeName as CorrelationTypeName,c1.CodeName as TrafficTypeName,a.AreaName,
                            sb.SubAreaName
                            from FxtData_Biz.dbo.Dat_Project_Biz_sub p with(nolock)
                            left join FxtDataCenter.dbo.SYS_SubArea_Biz sb with(nolock) on p.SubAreaId = sb.SubAreaId
                            left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.CorrelationType = c.Code
                            left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on p.TrafficType = c1.Code
                            where p.valid = 1 and p.CityId = @CityId and p.FxtCompanyId = @fxtCompanyId
                            " + where;
            //strSql += " order by 20 desc";

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.projectId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                totalCount = conn.Query<int>(totalCountSql, new { projectBiz.FxtCompanyId, projectBiz.SubAreaId, projectBiz.CityId, projectBiz.AreaId, ProjectName = "%" + projectBiz.ProjectName + "%" }).FirstOrDefault();
                return conn.Query<Dat_Project_Biz>(pagenatedSql, new { projectBiz.FxtCompanyId, projectBiz.SubAreaId, projectBiz.CityId, projectBiz.AreaId, ProjectName = "%" + projectBiz.ProjectName + "%" }).AsQueryable();
            }
        }

        public IQueryable<Dat_Project_Biz> GetProjectBizs(int cityId, int fxtCompanyId, int areaId = -1, int subAreaId = -1, int projectId = -1, bool self = true)
        {

            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId) || self) comId = fxtCompanyId.ToString();

            var where = string.Empty;
            if (areaId > 0) where = "  and p.AreaId = @AreaId";
            if (subAreaId > 0) where = "  and p.SubAreaId = @SubAreaId";
            if (projectId > 0) where = "  and p.ProjectId = @ProjectId";

            var strSql = @"
SELECT p.ProjectId
	,p.AreaId
	,p.SubAreaId
	,p.ProjectName
	,p.Address
	,a.AreaName
	,sa.SubAreaName
FROM FxtData_Biz.dbo.Dat_Project_Biz p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sa WITH (NOLOCK) ON p.SubAreaId = sa.SubAreaId
WHERE NOT EXISTS (
		SELECT ProjectId
		FROM FxtData_Biz.dbo.Dat_Project_Biz_sub p1 WITH (NOLOCK)
		WHERE p1.areaId = p.areaId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
			AND p1.projectId = p.projectId
		)
	AND p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId IN (" + comId + @")" + where + @"
UNION
SELECT p.ProjectId
	,p.AreaId
	,p.SubAreaId
	,p.ProjectName
	,p.Address
	,a.AreaName
	,sa.SubAreaName
FROM FxtData_Biz.dbo.Dat_Project_Biz_sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sa WITH (NOLOCK) ON p.SubAreaId = sa.SubAreaId
WHERE p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId = @fxtCompanyId" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<Dat_Project_Biz>(strSql, new { cityId, fxtCompanyId, areaId, subAreaId, projectId }).AsQueryable();
            }
        }

        public Dat_Project_Biz GetProjectBizById(int projectId, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strSql = @"select p.*,c.CodeName as CorrelationTypeName,c1.CodeName as TrafficTypeName,a.AreaName,
                            sb.SubAreaName
                            from FxtData_Biz.dbo.Dat_Project_Biz p with(nolock)
                            left join FxtDataCenter.dbo.SYS_SubArea_Biz sb with(nolock) on p.SubAreaId = sb.SubAreaId
                            left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.CorrelationType = c.Code
                            left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on p.TrafficType = c1.Code
                            where not exists 
(select ProjectId from FxtData_Biz.dbo.Dat_Project_Biz_sub p1 with(nolock)  where p1.areaId=p.areaId and  p1.cityId=p.cityId and p1.fxtCompanyId= @fxtCompanyId and p1.projectId = p.projectId)
                            and p.projectId = @projectId and p.valid = 1 and  p.CityId = @CityId and p.FxtCompanyId in(" + comId + @")
                            union 
                            select p.*,c.CodeName as CorrelationTypeName,c1.CodeName as TrafficTypeName,a.AreaName,
                            sb.SubAreaName
                            from FxtData_Biz.dbo.Dat_Project_Biz_sub p with(nolock)
                            left join FxtDataCenter.dbo.SYS_SubArea_Biz sb with(nolock) on p.SubAreaId = sb.SubAreaId
                            left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.CorrelationType = c.Code
                            left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on p.TrafficType = c1.Code
                            where p.projectId = @projectId and  p.valid = 1 and p.CityId = @CityId and p.FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<Dat_Project_Biz>(strSql, new { projectId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public long ValidateProjectBiz(int areaId, long projectId, string projectName, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strWhere = projectId == -1 ? "" : " and p.ProjectId != @projectId";

            var strSql = @"select projectId
            from FxtData_Biz.dbo.Dat_Project_Biz p with (nolock)
            where not exists (select ProjectId from FxtData_Biz.dbo.Dat_Project_Biz_sub p1 with(nolock)  where p1.areaId=p.areaId and  p1.cityId=p.cityId and p1.fxtCompanyId= @fxtCompanyId and p1.projectId = p.projectId)
	            and p.valid = 1
	            and p.ProjectName=@ProjectName
                and p.AreaId= @AreaId	
	            and p.CityId = @CityId
	            and p.FxtCompanyId in (" + comId + @")
                " + strWhere + @"

            union

            select projectId
            from FxtData_Biz.dbo.Dat_Project_Biz_sub p with (nolock)
            where p.valid = 1
	            and p.ProjectName=@ProjectName
                and p.AreaId= @AreaId	
	            and p.CityId = @CityId
	            and p.FxtCompanyId = @fxtCompanyId " + strWhere;


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<long>(strSql, new { areaId, projectId, projectName, cityId, comId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public IQueryable<long> GetProjectId(int cityId, int fxtCompanyId, int areaId, string projectName)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strWhere = string.Empty;
            if (areaId > 0) strWhere = " and p.AreaId = @areaId ";

            var strSql = @"select projectId from FxtData_Biz.dbo.Dat_Project_Biz p with(nolock) 
                            where p.valid = 1
                            and p.CityId=@cityId
                            " + strWhere + @"
                            and p.projectName = @projectName 
                            and p.FxtCompanyId in (" + comId + @")
                            and not exists (select ProjectId from FxtData_Biz.dbo.Dat_Project_Biz_sub p1 with(nolock)  where p1.areaId=p.areaId and  p1.cityId=p.cityId and p1.fxtCompanyId= @fxtCompanyId and p1.projectId = p.projectId)

                            union 

                            select projectId from FxtData_Biz.dbo.Dat_Project_Biz_Sub p with(nolock) 
                            where p.valid = 1
                            and p.CityId=@cityId
                           " + strWhere + @"
                            and p.projectName = @projectName 
                            and p.FxtCompanyId =@fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<long>(strSql, new { cityId, areaId, projectName, fxtCompanyId }).AsQueryable();
            }
        }

        public int GetProjectCountsBySubAreaId(int subAreaId, int cityId, int fxtCompanyId)
        {
            const string strSql = "select count(projectId) from FxtData_Biz.dbo.Dat_Project_Biz p with(nolock) where CityId=@cityId and fxtCompanyId=@fxtCompanyId and SubAreaId = @subAreaId and valid = 1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<int>(strSql, new { subAreaId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddProjectBiz(Dat_Project_Biz projectBiz)
        {
            var strSql = @"insert into FxtData_Biz.dbo.Dat_Project_Biz (
                cityid,areaid,subareaid,correlationtype,projectname,othername,address,traffictype,trafficdetails,parkinglevel,details,opendate,areadetails,east,south,west,north,creator,createtime,
                pinyin,pinyinall,fxtcompanyid,x,y,remarks,IsTypical,AveragePrice,Weight,StartDate,StartEndDate) 
                values(@cityid,@areaid,@subareaid,@correlationtype,@projectname,@othername,@address,@traffictype,@trafficdetails,@parkinglevel,@details,@opendate,@areadetails,@east,@south,@west,@north,
                @creator,@createtime,@pinyin,@pinyinall,@fxtcompanyid,@x,@y,@remarks,@IsTypical,@AveragePrice,@Weight,@StartDate,@StartEndDate)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Execute(strSql, projectBiz);
            }
        }

        public int UpdateProjectBiz(Dat_Project_Biz projectBiz, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            projectBiz.FxtCompanyId = currentCompanyId;

            var strSqlMainUpdate = @"update FxtData_Biz.dbo.Dat_Project_Biz with(rowlock)  
                set areaid = @areaid,subareaid = @subareaid,correlationtype = @correlationtype,projectname = @projectname,othername = @othername,address = @address,traffictype = @traffictype,
                    trafficdetails = @trafficdetails,parkinglevel = @parkinglevel,details = @details,opendate = @opendate,areadetails = @areadetails,east = @east,south = @south,west = @west,
                    north = @north,savedatetime = @savedatetime,saveuser = @saveuser,pinyin = @pinyin,pinyinall = @pinyinall,x = @x,y = @y,remarks = @remarks,IsTypical = @IsTypical,
                    AveragePrice = @AveragePrice,Weight = @Weight,StartDate=@StartDate,StartEndDate=@StartEndDate
                where projectid = @projectid and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";

            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.Dat_Project_Biz_Sub (
                projectId,cityid,areaid,subareaid,correlationtype,projectname,othername,address,traffictype,trafficdetails,parkinglevel,details,opendate,areadetails,east,south,west,north,
                creator,createtime,savedatetime,saveuser,pinyin,pinyinall,fxtCompanyId,x,y,remarks,IsTypical,AveragePrice,Weight,StartDate,StartEndDate) 
                values(
                @projectId,@cityid,@areaid,@subareaid,@correlationtype,@projectname,@othername,@address,@traffictype,@trafficdetails,@parkinglevel,@details,@opendate,@areadetails,@east,@south,
                @west,@north,@creator,@createtime,@savedatetime,@saveuser,@pinyin,@pinyinall,@fxtCompanyId,@x,@y,@remarks,@IsTypical,@AveragePrice,@Weight,@StartDate,@StartEndDate)";

            var strSqlSubUpdate = @"update FxtData_Biz.dbo.Dat_Project_Biz_Sub with(rowlock) 
                set areaid = @areaid,subareaid = @subareaid,correlationtype = @correlationtype,projectname = @projectname,othername = @othername,address = @address,traffictype = @traffictype,
                    trafficdetails = @trafficdetails,parkinglevel = @parkinglevel,details = @details,opendate = @opendate,areadetails = @areadetails,east = @east,south = @south,west = @west,
                    north = @north,creator = @creator,createtime = @createtime,savedatetime = @savedatetime,saveuser = @saveuser,pinyin = @pinyin,pinyinall = @pinyinall,valid = @valid,x = @x,y = @y,
                    remarks = @remarks,IsTypical = @IsTypical,AveragePrice = @AveragePrice,Weight = @Weight,StartDate=@StartDate,StartEndDate=@StartEndDate
                where projectid = @projectid and fxtCompanyId= @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainUpdate, projectBiz);
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubUpdate, projectBiz);
                return r1 == 0 ? conn.Execute(strSqlSubAdd, projectBiz) : r1;

                #region 废弃
                ////如果当前登录者为房讯通公司，不管该条数据是哪个评估公司创建的，都修改主表数据
                //if (currentCompanyId.ToString() == ConfigurationHelper.FxtCompanyId)
                //{
                //    return conn.Execute(strSqlMainUpdate, projectBiz);
                //}

                ////如果当前登录者是评估公司，并且是自己创建的，则修改主表
                //if (projectBiz.FxtCompanyId == currentCompanyId)
                //{
                //    return conn.Execute(strSqlMainUpdate, projectBiz);
                //}

                ////(接上)不是自己创建的，修改子表（有则更新，没有则新增）
                //projectBiz.FxtCompanyId = currentCompanyId;
                //var result = conn.Execute(strSqlSubUpdate, projectBiz);
                //return result == 0 ? conn.Execute(strSqlSubAdd, projectBiz) : result;
                #endregion
            }
        }

        public int DeleteProjectBiz(Dat_Project_Biz projectBiz, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            projectBiz.FxtCompanyId = currentCompanyId;
            projectBiz.Valid = 0;

            var strSqlMainDelete = @"Update FxtData_Biz.dbo.Dat_Project_Biz  with(rowlock) set valid = 0 where projectid = @projectid and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";
            var strSqlSubDelete = @"Update FxtData_Biz.dbo.Dat_Project_Biz_Sub  with(rowlock) set valid = 0 where projectid = @projectid and fxtCompanyId= @fxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.Dat_Project_Biz_Sub (
                projectid,cityid,areaid,subareaid,correlationtype,projectname,othername,address,traffictype,trafficdetails,parkinglevel,details,opendate,areadetails,east,south,west,north,creator,
                createtime,savedatetime,saveuser,pinyin,pinyinall,fxtCompanyId,x,y,remarks,IsTypical,AveragePrice,Weight,StartDate,StartEndDate) 
                values(@projectid,@cityid,@areaid,@subareaid,@correlationtype,@projectname,@othername,@address,@traffictype,@trafficdetails,@parkinglevel,@details,@opendate,@areadetails,@east,@south,
                @west,@north,@creator,@createtime,@savedatetime,@saveuser,@pinyin,@pinyinall,@fxtCompanyId,@x,@y,@remarks,@IsTypical,@AveragePrice,@Weight,@StartDate,@StartEndDate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainDelete, new { projectBiz.ProjectId, projectBiz.FxtCompanyId });
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubDelete, new { projectBiz.ProjectId, projectBiz.FxtCompanyId });
                return r1 == 0 ? conn.Execute(strSqlSubAdd, projectBiz) : r1;

                #region 废弃
                ////如果当前登录者为房讯通公司，不管该条数据是哪个评估公司创建的，都修改主表数据
                //if (currentCompanyId.ToString() == ConfigurationHelper.FxtCompanyId)
                //{
                //    return conn.Execute(strSqlMainDelete, new { projectBiz.ProjectId });
                //}

                ////如果当前登录者是评估公司，并且是自己创建的，则修改主表
                //if (projectBiz.FxtCompanyId == currentCompanyId)
                //{
                //    return conn.Execute(strSqlMainDelete, new { projectBiz.ProjectId });
                //}

                ////(接上)不是自己创建的，修改子表（有则更新，没有则新增）
                //var result = conn.Execute(strSqlSubDelete, new { projectBiz.ProjectId });

                //if (result != 0) return result;
                //projectBiz.Valid = 0;
                //return conn.Execute(strSqlSubAdd, projectBiz);
                #endregion
            }
        }

        public DataTable ProjectSelfDefineExport(Dat_Project_Biz projectBiz, List<string> projectAttr, int CityId, int FxtCompanyId, bool self = true)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                string ptable, ctable, btable, comId;
                Access(projectBiz.CityId, projectBiz.FxtCompanyId, out ptable, out ctable, out btable, out comId);
                if (string.IsNullOrEmpty(comId)) comId = projectBiz.FxtCompanyId.ToString();
                if (self) comId = projectBiz.FxtCompanyId.ToString();

                var where = string.Empty;
                //if (!(new[] { 0, -1 }).Contains(projectBiz.SubAreaId))
                //{
                //    where += "  and p.SubAreaId = @SubAreaId";
                //    paramet.Add(new SqlParameter("@SubAreaId", projectBiz.SubAreaId));
                //}
                if (!(new[] { 0, -1 }).Contains(projectBiz.AreaId))
                {
                    where += " and p.AreaId = @AreaId";
                    paramet.Add(new SqlParameter("@AreaId", projectBiz.AreaId));
                }
                if (!string.IsNullOrEmpty(projectBiz.ProjectName))
                {
                    where += " and p.ProjectName like @ProjectName";
                    paramet.Add(new SqlParameter("@ProjectName", projectBiz.ProjectName));
                }
                if (!string.IsNullOrEmpty(projectBiz.OtherName))
                {
                    where += " and p.OtherName like @OtherName";
                    paramet.Add(new SqlParameter("@OtherName", projectBiz.OtherName));
                }

                var strSql = @"
SELECT p.*
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS TrafficTypeName
	,c2.CodeName AS ParkingLevelName
	,a.AreaName
	,sb.SubAreaName
FROM FxtData_Biz.dbo.Dat_Project_Biz p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.TrafficType = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON p.ParkingLevel = c2.Code
WHERE NOT EXISTS (
		SELECT ProjectId
		FROM FxtData_Biz.dbo.Dat_Project_Biz_sub p1 WITH (NOLOCK)
		WHERE p1.areaId = p.areaId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
			AND p1.projectId = p.projectId
		)
	AND p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId IN (" + comId + @")" + where + @"
UNION
SELECT p.*
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS TrafficTypeName
	,c2.CodeName AS ParkingLevelName
	,a.AreaName
	,sb.SubAreaName
FROM FxtData_Biz.dbo.Dat_Project_Biz_sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.TrafficType = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON p.ParkingLevel = c2.Code
WHERE p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId = @fxtCompanyId" + where;

                paramet.Add(new SqlParameter("@CityId", CityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                //string paramList = string.Empty;
                //for (int i = 0; i < projectAttr.Count; i++)
                //{
                //    paramList += projectAttr[i].Replace("&", " as ") + ",";
                //}
                var paramList = new StringBuilder();
                foreach (var t in projectAttr)
                {
                    paramList.Append(t.Replace("&", " as "));
                    paramList.Append(",");
                }
                string sql = "select " + paramList.ToString().TrimEnd(',') + " from (" + strSql + ")T";

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataBiz;
                DataTable dtable = DBHelperSql.ExecuteDataTable(sql, param);
                return dtable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region 商业街道图片

        public IQueryable<LNK_P_Photo> GetBusinessStreetPhotoes(LNK_P_Photo lnkPPhoto, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(lnkPPhoto.CityId, lnkPPhoto.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = lnkPPhoto.FxtCompanyId.ToString();
            if (self) comId = lnkPPhoto.FxtCompanyId.ToString();

            // where += "order by p.CreateTime desc";

            //查询全部字段时，select * 与 select a,b...效率差不多
            var strSql = @"select p.*,c.CodeName as PhotoTypeName
                            from FxtData_Biz.dbo.LNK_P_Photo p with(nolock)
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.PhotoTypeCode = c.Code
                            where not exists  (select id from FxtData_Biz.dbo.LNK_P_Photo_Sub p1 with(nolock) where p1.projectId= p.projectId and p1.cityId = p.cityId and p1.fxtCompanyId = @fxtCompanyId)
                            and p.valid = 1 and  p.CityId = @CityId and p.FxtCompanyId in(" + comId + @")
                            and p.ProjectId = @projectId
                            union 
                            select  p.*,c.CodeName as PhotoTypeName
                            from FxtData_Biz.dbo.LNK_P_Photo_Sub p with(nolock)
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.PhotoTypeCode = c.Code
                            where p.valid = 1 and p.CityId = @CityId and p.FxtCompanyId = @fxtCompanyId
                            and p.ProjectId = @projectId
                            order by 1 desc";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<LNK_P_Photo>(strSql, lnkPPhoto).AsQueryable();
            }
        }

        public LNK_P_Photo GetBusinessStreetPhoto(int id, int fxtCompanyId)
        {
            var strSql = @"select p.*,c.CodeName as PhotoTypeName
                            from FxtData_Biz.dbo.LNK_P_Photo p with(nolock)
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.PhotoTypeCode = c.Code
                            where not exists  (select id from FxtData_Biz.dbo.LNK_P_Photo_Sub p1 with(nolock) where p1.projectId= p.projectId and p1.cityId = p.cityId and p1.fxtCompanyId = @fxtCompanyId)
                            and p.valid = 1 and  p.id=@id
                            union 
                            select  p.*,c.CodeName as PhotoTypeName
                            from FxtData_Biz.dbo.LNK_P_Photo_Sub p with(nolock)
                            left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.PhotoTypeCode = c.Code
                            where p.valid = 1 and p.id=@id
                            order by 1 desc";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<LNK_P_Photo>(strSql, new { id, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddBusinessStreetPhoto(LNK_P_Photo lnkPPhoto)
        {
            var strSql = @"insert into FxtData_Biz.dbo.LNK_P_Photo  (projectid,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@projectid,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Execute(strSql, lnkPPhoto);
            }
        }

        public int UpdateBusinessStreetPhoto(LNK_P_Photo lnkPPhoto, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            lnkPPhoto.FxtCompanyId = currentCompanyId;

            var strSqlMainUpdate = @"update FxtData_Biz.dbo.LNK_P_Photo  set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";
            var strSqlSubUpdate = @"update FxtData_Biz.dbo.LNK_P_Photo_Sub set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and fxtCompanyId= @fxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.LNK_P_Photo_Sub  (id,projectid,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@id,@projectid,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainUpdate, lnkPPhoto);
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubUpdate, lnkPPhoto);
                return r1 == 0 ? conn.Execute(strSqlSubAdd, lnkPPhoto) : r1;
            }
        }

        public int DeleteBusinessStreetPhoto(LNK_P_Photo lnkPPhoto, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            lnkPPhoto.FxtCompanyId = currentCompanyId;
            lnkPPhoto.Valid = 0;

            var strSqlMainDelete = @"Update FxtData_Biz.dbo.LNK_P_Photo  with(rowlock) set valid = 0 where id = @id and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";
            var strSqlSubDelete = @"Update FxtData_Biz.dbo.LNK_P_Photo_Sub  with(rowlock) set valid = 0 where id = @id and fxtCompanyId = @fxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Biz.dbo.LNK_P_Photo_Sub  (id,projectid,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@id,@projectid,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var r = conn.Execute(strSqlMainDelete, new { lnkPPhoto.Id, fxtCompanyId = lnkPPhoto.FxtCompanyId });
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
