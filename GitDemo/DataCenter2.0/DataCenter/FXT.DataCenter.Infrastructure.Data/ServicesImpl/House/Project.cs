using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.QueryObjects;
using FXT.DataCenter.Domain.Models.QueryObjects.House;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Domain.Models.DTO;
using Dapper;
using FXT.DataCenter.Domain.Models.FxtProject;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;


namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    /// <summary>
    /// 楼盘
    /// </summary>
    public class DAT_ProjectDAL : IDAT_Project
    {
        /// <summary>
        /// 房讯通
        /// </summary>
        private readonly int _fxtComId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);//房讯通

        /// <summary>
        /// 获取Table
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">公司Id</param>
        /// <returns></returns>
        private IQueryable<SYS_City_Table> GetCityTable(int CityId, int FxtCompanyId = 0)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                if (FxtCompanyId > 0)
                {
                    string strsql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],subhousepricetable,s.ShowCompanyId,s.CaseCompanyId FROM [SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and FxtCompanyId=@FxtCompanyId and typecode= 1003002";
                    return (con.Query<SYS_City_Table>(strsql, new { CityId = CityId, FxtCompanyId = FxtCompanyId })).AsQueryable();
                }
                else
                {
                    string strsql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],subhousepricetable FROM [SYS_City_Table] with(nolock) where CityId=@CityId";
                    return (con.Query<SYS_City_Table>(strsql, new { CityId = CityId })).AsQueryable();
                }
            }

        }

        /// <summary>
        /// 判断是否已删除的楼盘(valid=0)或者 楼盘已经存在(valid=1)
        /// </summary>
        /// <returns></returns>
        private IQueryable<DAT_Project> ProjectDeleteOrExissts(string ptable, string ComId, DAT_Project project, int valid)
        {
            string strsql = @"
select ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,SourceName,FxtCompanyId
from " + ptable + @" p with(nolock)
where ProjectName=@ProjectName 
    and AreaId=@AreaId and valid=@valid and CityId=@CityId and p.FxtCompanyId in (" + ComId + @")
    and not exists (select ProjectId from " + ptable + @"_sub ps with(nolock) 
    where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId 
    and ps.CityId=p.CityId and ps.AreaId=p.AreaId ) 
union 
select ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,SourceName,Fxt_CompanyId
from " + ptable + @"_sub p with(nolock)
where ProjectName=@ProjectName and AreaId=@AreaId 
and valid=@valid and CityId=@CityId and p.Fxt_CompanyId=@FxtCompanyId";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var result = con.Query<DAT_Project>(strsql, new { ProjectName = project.projectname, AreaId = project.areaid, CityId = project.cityid, FxtCompanyId = project.fxtcompanyid, valid = valid }).AsQueryable();
                return result;
            }
        }

        /// <summary>
        /// 读取楼盘信息
        /// </summary>
        /// <param name="project">楼盘参数</param>
        /// <returns></returns>
        public IQueryable<DAT_Project> GetProjectInfo(ProjectQueryParams project)
        {

            var dt = GetCityTable(project.CityId, project.FxtCompanyId).FirstOrDefault();
            if (dt != null)
            {
                string ptable = dt.projecttable;
                string btable = dt.buildingtable;
                string htable = dt.housetable;
                string ComId = dt.ShowCompanyId;
                string QueryWhere = "";
                if (project.ProjectId > 0)
                {
                    QueryWhere += " and p.ProjectId=@ProjectId";
                }
                if (project.AreaId > 0)
                {
                    QueryWhere += " and p.AreaId=@AreaId";
                }
                if (project.SubAreaId > 0)
                {
                    QueryWhere += " and p.SubAreaId=@SubAreaId";
                }
                if (project.RightCode > 0)
                {
                    QueryWhere += " and p.RightCode=@RightCode";
                }
                if (project.BuildingTypeCode > 0)
                {
                    QueryWhere += " and p.BuildingTypeCode=@BuildingTypeCode";
                }
                if (!string.IsNullOrEmpty(project.FieldNo))
                {
                    QueryWhere += " and p.FieldNo like @FieldNo";

                }
                if (!string.IsNullOrEmpty(project.Key))
                {
                    QueryWhere += " and (p.projectName like @projectName or p.OtherName like @projectName)";
                }
                if (project.PlanPurpose > 0)
                {
                    QueryWhere += " and p.PurposeCode=@PurposeCode";

                }
                if (project.UsableYear > 0)
                {
                    QueryWhere += " and p.UsableYear>@UsableYear";

                }

                #region ziduan
                string ziduan = @"SELECT 
	                            p.[ProjectId]
	                            ,[projectName]
	                            ,[OtherName]
	                            ,p.[AreaId]
	                            ,P.[Address]
	                            ,[RightCode]
	                            ,[PurposeCode]
	                            ,[BuildingTypeCode]
	                            ,Convert(VARCHAR(10), [BuildingDate], 120) [BuildingDate]
	                            ,Convert(VARCHAR(10), [SaleDate], 120) [SaleDate]
	                            ,BuildingNum
	                            ,p.FxtCompanyId
	                            ,TotalNum
	                            ,DeveCompanyName = (
		                            SELECT top 1 ChineseName
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001001
		                            )
	                            ,DeveCompanyId = (
		                            SELECT top 1 pc.CompanyId
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001001
		                            )
	                            ,ManagerCompanyName = (
		                            SELECT top 1 ChineseName
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001004
		                            )
	                            ,ManagerCompanyId = (
		                            SELECT top 1 pc.CompanyId
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001004
		                            )
	                            ,[BuildingArea]
	                            ,[Detail]
	                            ,[PinYin]
	                            ,[SalePrice]
	                            ,[AveragePrice]
	                            ,[ManagerPrice]
	                            ,p.[IsEValue]
	                            ,[CubageRate]
	                            ,Convert(VARCHAR(10), [EndDate], 120) [EndDate]
	                            ,[FieldNo]
	                            ,[GreenRate]
	                            ,Convert(VARCHAR(10), [JoinDate], 120) [JoinDate]
	                            ,[LandArea]
	                            ,[ManagerTel]
	                            ,[ParkingNumber]
                                ,ParkingDesc
	                            ,Convert(VARCHAR(10), [StartDate], 120) [StartDate]
                                ,Convert(VARCHAR(10), [StartEndDate], 120) [StartEndDate]
	                            ,[UsableYear]
	                            ,[SalableArea]
	                            ,p.[SubAreaId]
	                            ,p.[X]
	                            ,p.[Y]
	                            ,P.IsComplete
	                            ,P.Creator
	                            ,Convert(VARCHAR(10), p.CreateTime, 120) CreateTime
	                            ,P.SaveUser
	                            ,Convert(VARCHAR(10), p.SaveDateTime, 120) SaveDateTime
	                            ,ci.cityName AS cityName
	                            ,sa.AreaName AS AreaName
	                            ,ssa.SubAreaName AS SubAreaName
	                            ,Convert(VARCHAR(10), p.InnerSaleDate, 120) InnerSaleDate
	                            ,Convert(VARCHAR(10), p.UpdateDateTime, 120) UpdateDateTime
	                            ,p.OfficeArea AS OfficeArea
	                            ,p.OtherArea AS OtherArea
	                            ,Convert(VARCHAR(10), p.CoverDate, 120) CoverDate
	                            ,p.PlanPurpose AS PlanPurpose
	                            ,Convert(VARCHAR(10), p.PriceDate, 120) PriceDate
	                            ,p.[Weight] AS [Weight]
	                            ,p.BusinessArea AS BusinessArea
	                            ,p.IndustryArea AS IndustryArea
	                            ,p.OldId AS OldId
	                            ,p.AreaLineId AS AreaLineId
	                            ,p.Valid AS Valid
	                            ,p.PinYinAll AS PinYinAll
	                            ,p.XYScale AS XYScale
	                            ,p.IsEmpty AS IsEmpty
	                            ,p.TotalId AS TotalId
	                            ,p.East AS East
	                            ,p.West AS West
	                            ,p.South AS South
	                            ,p.North AS North
	                            ,p.BuildingQuality AS BuildingQuality
	                            ,p.HousingScale AS HousingScale
	                            ,p.BuildingDetail AS BuildingDetail
	                            ,p.HouseDetail AS HouseDetail
	                            ,p.BasementPurpose AS BasementPurpose
	                            ,p.ManagerQuality AS ManagerQuality
	                            ,p.Facilities AS Facilities
	                            ,p.AppendageClass AS AppendageClass
	                            ,p.RegionalAnalysis AS RegionalAnalysis
	                            ,p.Wrinkle AS Wrinkle
	                            ,p.Aversion AS Aversion
	                            ,p.CityId AS CityId
	                            ,FXTProject.dbo.Fun_GetProjectXYList(landcoo.ProjectId, landcoo.FxtCompanyId, landcoo.CityID, '|') AS LngOrLat
                                ,CompanyName=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=p.fxtcompanyId)
                                ,SourceName
                                ,p.fxtcompanyId as belongcompanyid
                                ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=p.fxtcompanyId)";
                #endregion

                #region ziduan2
                string ziduan2 = @" SELECT p.[ProjectId]
	                            ,p.[projectName]
	                            ,p.[OtherName]
	                            ,p.[AreaId]
	                            ,P.[Address]
	                            ,p.[RightCode]
	                            ,p.[PurposeCode]
	                            ,p.[BuildingTypeCode]
	                            ,Convert(VARCHAR(10), p.[BuildingDate], 120) [BuildingDate]
	                            ,Convert(VARCHAR(10), p.[SaleDate], 120) [SaleDate]
	                            ,p.BuildingNum
	                            ,p.Fxt_CompanyId
	                            ,p.TotalNum
	                            ,DeveCompanyName = (
		                            SELECT top 1 ChineseName
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001001
		                            )
	                            ,DeveCompanyId = (
		                            SELECT top 1 pc.CompanyId
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001001
		                            )
	                            ,ManagerCompanyName = (
		                            SELECT top 1 ChineseName
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001004
		                            )
	                            ,ManagerCompanyId = (
		                            SELECT top 1 pc.CompanyId
		                            FROM FXTProject.dbo.LNK_P_Company pc
			                            ,FxtDataCenter.dbo.DAT_Company d
		                            WHERE pc.CompanyId = d.CompanyId
			                            AND pc.CityId = @CityId
			                            AND pc.ProjectId = @ProjectId
			                            AND PC.CompanyType = 2001004
		                            )
	                            ,p.[BuildingArea]
	                            ,p.[Detail]
	                            ,p.[PinYin]
	                            ,p.[SalePrice]
	                            ,p.[AveragePrice]
	                            ,p.[ManagerPrice]
	                            ,p.[IsEValue]
	                            ,p.[CubageRate]
	                            ,Convert(VARCHAR(10), p.[EndDate], 120) [EndDate]
	                            ,p.[FieldNo]
	                            ,p.[GreenRate]
	                            ,Convert(VARCHAR(10), p.[JoinDate], 120) [JoinDate]
	                            ,p.[LandArea]
	                            ,p.[ManagerTel]
	                            ,p.[ParkingNumber]
	                            ,p.[ParkingDesc]
	                            ,Convert(VARCHAR(10),p.[StartDate], 120) [StartDate]
	                            ,Convert(VARCHAR(10),p.[StartEndDate], 120) [StartEndDate]
	                            ,p.[UsableYear]
	                            ,p.[SalableArea]
	                            ,p.[SubAreaId]
	                            ,p.[X]
	                            ,p.[Y]
	                            ,P.IsComplete
	                            ,P.Creator
	                            ,Convert(VARCHAR(10), p.CreateTime, 120) CreateTime
	                            ,P.SaveUser
	                            ,Convert(VARCHAR(10), p.SaveDateTime, 120) SaveDateTime
	                            ,ci.cityName AS cityName
	                            ,sa.AreaName AS AreaName
	                            ,ssa.SubAreaName AS SubAreaName
	                            ,Convert(VARCHAR(10), p.InnerSaleDate, 120) InnerSaleDate
	                            ,Convert(VARCHAR(10), p.UpdateDateTime, 120) UpdateDateTime
	                            ,p.OfficeArea AS OfficeArea
	                            ,p.OtherArea AS OtherArea
	                            ,Convert(VARCHAR(10), p.CoverDate, 120) CoverDate
	                            ,p.PlanPurpose AS PlanPurpose
	                            ,Convert(VARCHAR(10), p.PriceDate, 120) PriceDate
	                            ,p.[Weight] AS [Weight]
	                            ,p.BusinessArea AS BusinessArea
	                            ,p.IndustryArea AS IndustryArea
	                            ,p.OldId AS OldId
	                            ,p.AreaLineId AS AreaLineId
	                            ,p.Valid AS Valid
	                            ,p.PinYinAll AS PinYinAll
	                            ,p.XYScale AS XYScale
	                            ,p.IsEmpty AS IsEmpty
	                            ,p.TotalId AS TotalId
	                            ,p.East AS East
	                            ,p.West AS West
	                            ,p.South AS South
	                            ,p.North AS North
	                            ,p.BuildingQuality AS BuildingQuality
	                            ,p.HousingScale AS HousingScale
	                            ,p.BuildingDetail AS BuildingDetail
	                            ,p.HouseDetail AS HouseDetail
	                            ,p.BasementPurpose AS BasementPurpose
	                            ,p.ManagerQuality AS ManagerQuality
	                            ,p.Facilities AS Facilities
	                            ,p.AppendageClass AS AppendageClass
	                            ,p.RegionalAnalysis AS RegionalAnalysis
	                            ,p.Wrinkle AS Wrinkle
	                            ,p.Aversion AS Aversion
	                            ,p.CityId AS CityId
	                            ,FXTProject.dbo.Fun_GetProjectXYList(landcoo.ProjectId, landcoo.FxtCompanyId, landcoo.CityID, '|') AS LngOrLat
                                ,CompanyName=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=p.fxt_companyId)
								,p.SourceName
                                ,pi.FxtCompanyId as belongcompanyid
                                ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=pi.FxtCompanyId) ";
                #endregion

                string table1 = @" from " + ptable + " p with(nolock) ";
                string lefttable = @"  LEFT JOIN FXTProject.dbo.LNK_P_Company PC WITH (NOLOCK) ON PC.ProjectId = P.ProjectId AND PC.CityId = P.CityId AND PC.CompanyType = 2001001
LEFT JOIN FxtDataCenter.dbo.SYS_Area sa WITH (NOLOCK) ON sa.AreaId = p.AreaID
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea ssa WITH (NOLOCK) ON ssa.SubAreaId = p.SubAreaId
LEFT JOIN FXTProject.dbo.DAT_Project_Coordinate landcoo WITH (NOLOCK) ON landcoo.ProjectId = p.ProjectId
LEFT JOIN FxtDataCenter.dbo.SYS_City ci WITH (NOLOCK) ON ci.cityid = p.cityid";


                string where1 = @" where p.valid=1 and p.CityId=@CityId   
                                     and not exists (select ProjectId from " + ptable + @"_sub ps with(nolock) 
                                     where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId and ps.CityId=p.CityId) and p.FxtCompanyId in(" + ComId + @") ";
                string union = @" union ";
                string table2 = @" from " + ptable + "_sub p with(nolock) left join " + ptable + " pi on p.ProjectId = pi.ProjectId";

                string where2 = @" where p.valid=1 and p.CityId=@CityId and p.Fxt_CompanyId=@FxtCompanyId";

                string strsql = ziduan + table1 + lefttable + where1 + QueryWhere;
                strsql += union + ziduan2 + table2 + lefttable + where2 + QueryWhere;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var projectList = con.Query<DAT_Project>(strsql, new
                    {
                        CityId = project.CityId,
                        FxtCompanyId = project.FxtCompanyId,
                        ProjectId = project.ProjectId,
                        AreaId = project.AreaId,
                        SubAreaId = project.SubAreaId,
                        RightCode = project.RightCode,
                        BuildingTypeCode = project.BuildingTypeCode,
                        FieldNo = "%" + project.FieldNo + "%",
                        projectName = "%" + project.Key + "%",
                        PurposeCode = project.PlanPurpose,
                        UsableYear = project.UsableYear
                    });
                    return projectList.AsQueryable();
                }
            }
            else
                return new List<DAT_Project>().AsQueryable();
        }

        //        /// <summary>
        //        /// 一键导出
        //        /// 2015-01-16
        //        /// </summary>
        //        /// <param name="project"></param>
        //        /// <param name="self"></param>
        //        /// <returns></returns>
        //        public IQueryable<DAT_Project> ExportGetProjectInfo(ProjectQueryParams project, bool self)
        //        {
        //            var dt = GetCityTable(project.CityId, project.FxtCompanyId).FirstOrDefault();
        //            if (dt != null)
        //            {
        //                string ptable = dt.projecttable, comId = dt.ShowCompanyId,
        //                ziduan = @"SELECT p.[ProjectId],[projectName],[OtherName],p.[AreaId],P.[Address],[RightCode],[PurposeCode],[BuildingTypeCode],
        //                                             Convert(varchar(10),[BuildingDate],120)[BuildingDate],Convert(varchar(10),[SaleDate],120)[SaleDate],
        //                                              p.FxtCompanyId,
        //                                              TotalNum,BuildingNum,
        //                                             [BuildingArea],[Detail],[PinYin],[SalePrice],[AveragePrice],[ManagerPrice],p.[IsEValue],[CubageRate],
        //                                             Convert(varchar(10),[EndDate],120)[EndDate],[FieldNo],[GreenRate],Convert(varchar(10),[JoinDate],120)[JoinDate],
        //                                             [LandArea],[ManagerTel],[ParkingNumber],[ParkingDesc],Convert(varchar(10),[StartDate],120)[StartDate],[UsableYear],[SalableArea],p.[SubAreaId],p.[X],p.[Y],P.IsComplete,
        //                                             (select UserName from privi_user with(nolock) where userid=p.Creator) Creator,Convert(varchar(10),p.CreateTime,120)CreateTime,
        //                                             (select UserName from privi_user with(nolock) where userid=p.SaveUser) SaveUser,Convert(varchar(10),p.SaveDateTime,120)SaveDateTime,ci.cityName as cityName,sa.AreaName as AreaName,ssa.SubAreaName as SubAreaName,
        //                                              Convert(varchar(10),p.InnerSaleDate,120)InnerSaleDate,Convert(varchar(10),p.UpdateDateTime,120)UpdateDateTime,p.OfficeArea as OfficeArea,p.OtherArea as OtherArea,Convert(varchar(10),p.CoverDate,120)CoverDate,
        //                                                p.PlanPurpose as PlanPurpose,Convert(varchar(10),p.PriceDate,120)PriceDate,
        //                                                p.[Weight] as [Weight],p.BusinessArea as BusinessArea,p.IndustryArea as IndustryArea,
        //                                                p.OldId as OldId,p.AreaLineId as AreaLineId,p.Valid as Valid,p.PinYinAll as PinYinAll,
        //                                                p.XYScale as XYScale,p.IsEmpty as IsEmpty,p.TotalId as TotalId,
        //                                                p.East as East,p.West as West,p.South as South,p.North as North,
        //                                                p.BuildingQuality as BuildingQuality,p.HousingScale as HousingScale,
        //                                                p.BuildingDetail as BuildingDetail,p.HouseDetail as HouseDetail,
        //                                                p.BasementPurpose as BasementPurpose,p.ManagerQuality as ManagerQuality,
        //                                                p.Facilities as Facilities,p.AppendageClass as AppendageClass,
        //                                                p.RegionalAnalysis as RegionalAnalysis,p.Wrinkle as Wrinkle,
        //                                                p.Aversion as Aversion,p.CityId as CityId";
        //                string ziduan2 = @"SELECT p.[ProjectId],[projectName],[OtherName],p.[AreaId],P.[Address],[RightCode],[PurposeCode],[BuildingTypeCode],
        //                                             Convert(varchar(10),[BuildingDate],120)[BuildingDate],Convert(varchar(10),[SaleDate],120)[SaleDate],
        //                                              p.Fxt_CompanyId,
        //                                              TotalNum,BuildingNum,
        //                                             [BuildingArea],[Detail],[PinYin],[SalePrice],[AveragePrice],[ManagerPrice],p.[IsEValue],[CubageRate],
        //                                             Convert(varchar(10),[EndDate],120)[EndDate],[FieldNo],[GreenRate],Convert(varchar(10),[JoinDate],120)[JoinDate],
        //                                             [LandArea],[ManagerTel],[ParkingNumber],[ParkingDesc],Convert(varchar(10),[StartDate],120)[StartDate],[UsableYear],[SalableArea],p.[SubAreaId],p.[X],p.[Y],P.IsComplete,
        //                                             (select UserName from privi_user with(nolock) where userid=p.Creator) Creator,Convert(varchar(10),p.CreateTime,120)CreateTime,
        //                                             (select UserName from privi_user with(nolock) where userid=p.SaveUser) SaveUser,Convert(varchar(10),p.SaveDateTime,120)SaveDateTime,ci.cityName as cityName,sa.AreaName as AreaName,ssa.SubAreaName as SubAreaName,
        //                                              Convert(varchar(10),p.InnerSaleDate,120)InnerSaleDate,Convert(varchar(10),p.UpdateDateTime,120)UpdateDateTime,p.OfficeArea as OfficeArea,p.OtherArea as OtherArea,Convert(varchar(10),p.CoverDate,120)CoverDate,
        //                                                p.PlanPurpose as PlanPurpose,Convert(varchar(10),p.PriceDate,120)PriceDate,
        //                                                p.[Weight] as [Weight],p.BusinessArea as BusinessArea,p.IndustryArea as IndustryArea,
        //                                                p.OldId as OldId,p.AreaLineId as AreaLineId,p.Valid as Valid,p.PinYinAll as PinYinAll,
        //                                                p.XYScale as XYScale,p.IsEmpty as IsEmpty,p.TotalId as TotalId,
        //                                                p.East as East,p.West as West,p.South as South,p.North as North,
        //                                                p.BuildingQuality as BuildingQuality,p.HousingScale as HousingScale,
        //                                                p.BuildingDetail as BuildingDetail,p.HouseDetail as HouseDetail,
        //                                                p.BasementPurpose as BasementPurpose,p.ManagerQuality as ManagerQuality,
        //                                                p.Facilities as Facilities,p.AppendageClass as AppendageClass,
        //                                                p.RegionalAnalysis as RegionalAnalysis,p.Wrinkle as Wrinkle,
        //                                                p.Aversion as Aversion,p.CityId as CityId";
        //                string table1 = @" from " + ptable + " p with(nolock) ";
        //                string lefttable = @"  left join FxtDataCenter.dbo.SYS_Area sa with(nolock)  on sa.AreaId=p.AreaID
        //                                           left join FxtDataCenter.dbo.SYS_SubArea ssa with(nolock) on ssa.SubAreaId=p.SubAreaId
        //                                           left join FxtDataCenter.dbo.SYS_City ci with(nolock) on ci.cityid=p.cityid";
        //                string where1 = @" where p.valid=1 and p.CityId=@CityId   
        //                                     and not exists (select ProjectId from " + ptable + @"_sub ps with(nolock) 
        //                                     where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId and ps.CityId=p.CityId) 
        //                                     " + (self ? " and p.FxtCompanyId=@FxtCompanyId " : " and p.FxtCompanyId in(" + comId + @") ");
        //                string union = @" union ";
        //                string table2 = @" from " + ptable + "_sub p with(nolock)";
        //                string where2 = @" where p.valid=1 and p.CityId=@CityId and p.Fxt_CompanyId=@FxtCompanyId";
        //                string QueryWhere = "";
        //                if (project.AreaId > 0)
        //                {
        //                    QueryWhere += " and p.AreaId=@AreaId";
        //                }
        //                if (project.SubAreaId > 0)
        //                {
        //                    QueryWhere += " and p.SubAreaId=@SubAreaId";
        //                }
        //                if (project.BuildingTypeCode > 0)
        //                {
        //                    QueryWhere += " and p.BuildingTypeCode=@BuildingTypeCode";
        //                }
        //                if (!string.IsNullOrEmpty(project.Key))
        //                {
        //                    QueryWhere += " and (p.projectName like @projectName or p.OtherName like @projectName)";
        //                }
        //                if (project.PlanPurpose > 0)
        //                {
        //                    QueryWhere += " and p.PurposeCode=@PurposeCode";

        //                }
        //                string strsql = ziduan + table1 + lefttable + where1 + QueryWhere;
        //                strsql += union + ziduan2 + table2 + lefttable + where2 + QueryWhere;
        //                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //                {
        //                    var projectList = con.Query<DAT_Project>(strsql, new
        //                    {
        //                        CityId = project.CityId,
        //                        FxtCompanyId = project.FxtCompanyId,
        //                        AreaId = project.AreaId,
        //                        SubAreaId = project.SubAreaId,
        //                        BuildingTypeCode = project.BuildingTypeCode,
        //                        projectName = "%" + project.Key + "%",
        //                        PurposeCode = project.PlanPurpose
        //                    });
        //                    return projectList.AsQueryable();
        //                }
        //            }
        //            return new List<DAT_Project>().AsQueryable();
        //        }

        public IQueryable<DAT_Project> GetProjectInfoList(ProjectQueryParams project)
        {
            var dt = GetCityTable(project.CityId, project.FxtCompanyId).FirstOrDefault();
            if (dt != null)
            {
                var ptable = dt.projecttable;
                var btable = dt.buildingtable;
                var comId = dt.ShowCompanyId ?? project.FxtCompanyId.ToString();

                var queryWhere = string.Empty;
                if (project.ProjectId > 0)
                {
                    queryWhere += " and p.ProjectId=@ProjectId";
                }
                if (project.AreaId > 0)
                {
                    queryWhere += " and p.AreaId=@AreaId";
                }
                if (project.SubAreaId > 0)
                {
                    queryWhere += " and p.SubAreaId=@SubAreaId";
                }
                if (project.BuildingTypeCode > 0)
                {
                    queryWhere += " and p.BuildingTypeCode=@BuildingTypeCode";
                }
                if (!string.IsNullOrEmpty(project.Key))
                {
                    queryWhere += " and (p.projectName like @projectName or p.OtherName like @projectName or p.Address like @projectName)";
                }
                if (project.PlanPurpose > 0)
                {
                    queryWhere += " and p.PurposeCode=@PurposeCode";
                }
                var strsql = @"
select distinct
	p.ProjectId
    ,p.Creator
    ,p.fxtcompanyid
	,p.belongcompanyid
    ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=p.belongcompanyid)
	,p.AreaID
	,p.ProjectName
	,p.OtherName
	,p.Address
	,ISNULL(c.BuildingNumber,0) as BuildingNumber
	,ISNULL(c.HouseNumber,0) as HouseNumber
    ,ci.CityName
	,sa.AreaName
	,ssa.SubAreaName
from (
	select
		ProjectId,ProjectName,OtherName,Address,SubAreaId,CityID,AreaID,Valid,Creator,FxtCompanyId,FxtCompanyId as belongcompanyid,BuildingTypeCode,PurposeCode
	from " + ptable + @" p with(nolock)
	where not exists(
		select ProjectId from " + ptable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.Fxt_CompanyId = @fxtcompanyid
		and ps.CityID = @cityid
	)
	and p.Valid = 1
	and p.FxtCompanyId in (" + comId + @")
	and p.CityID = @cityid
	union
	select 
		p.ProjectId,p.ProjectName,p.OtherName,p.Address,p.SubAreaId,p.CityID,p.AreaID,p.Valid,p.Creator,p.Fxt_CompanyId,isnull(pi.FxtCompanyId,0),p.BuildingTypeCode,p.PurposeCode
	from " + ptable + "_sub p with(nolock) left join " + ptable + @" pi with(nolock) on p.ProjectId = pi.ProjectId
	where p.Valid = 1
	and p.Fxt_CompanyId = @fxtcompanyid
	and p.CityID = @cityid
)p
LEFT JOIN FxtDataCenter.dbo.SYS_Area sa WITH (NOLOCK) ON sa.AreaId = p.AreaID
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea ssa WITH (NOLOCK) ON ssa.SubAreaId = p.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_City ci WITH (NOLOCK) ON ci.cityid = p.cityid
LEFT JOIN (
	select * from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK)
	where c.cityId = @cityid and c.fxtcompanyId = @fxtcompanyid and c.BuildingId = 0
)c on c.projectId = p.projectId
where 1 = 1  " + queryWhere;

                strsql = string.IsNullOrEmpty(project.KeyString)
                    ? strsql
                    : (
@"
select T.* from (" + strsql + @" 
)T
inner join (
	select 
		distinct Tp.ProjectId
	from (
		select			
			ProjectId
			,ProjectName
			,PinYin
			,PinYinAll
			,OtherName as POtherName
			,Address
		from " + ptable + @" p with(nolock)
		where not exists(
			select ProjectId from " + ptable + @"_sub ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.Fxt_CompanyId = @fxtcompanyid
			and ps.CityID = @cityid
		)
		and p.FxtCompanyId in (" + comId + @")
		and p.CityID = @cityid
		union
		select 		
			ProjectId
			,ProjectName
			,PinYin
			,PinYinAll
			,OtherName as POtherName
			,Address
		from " + ptable + @"_sub p with(nolock)
		where 1 = 1
		and p.Fxt_CompanyId = @fxtcompanyid
		and p.CityID = @cityid
	)Tp
	left join (
		select
			ProjectId
			,BuildingId
			,BuildingName
			,Doorplate
			,OtherName as BOtherName
		from " + btable + @" b with(nolock)
		where not exists(
			select BuildingId from " + btable + @"_sub bs with(nolock)
			where bs.BuildingId = b.BuildingId
			and bs.Fxt_CompanyId = @fxtcompanyid
			and bs.CityID = @cityid
		)
		and b.FxtCompanyId in (" + comId + @")
		and b.CityID = @cityid
		union
		select 
			ProjectId
			,BuildingId
			,BuildingName
			,Doorplate
			,OtherName as BOtherName
		from " + btable + @"_sub b with(nolock)
		where 1 = 1
		and b.Fxt_CompanyId = @fxtcompanyid
		and b.CityID = @cityid
	)Tb on Tp.ProjectId = Tb.ProjectId
	where 1 = 1 and (Tp.ProjectName like @keystring
	or Tp.PinYin like @keystring 
	or Tp.PinYinAll like @keystring 
	or Tp.POtherName like @keystring
	or Tp.Address like @keystring
	or Tb.BuildingName like @keystring
	or Tb.Doorplate like @keystring
	or Tb.BOtherName like @keystring
	)
)T1 on T.ProjectId = T1.ProjectId");

                var pageSql = strsql;

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var projectList = con.Query<DAT_Project>(pageSql, new
                    {
                        project.CityId,
                        project.FxtCompanyId,
                        project.ProjectId,
                        project.AreaId,
                        project.SubAreaId,
                        project.BuildingTypeCode,
                        projectName = "%" + project.Key + "%",
                        PurposeCode = project.PlanPurpose,
                        keystring = "%" + project.KeyString + "%"
                    }).AsQueryable();

                    return projectList;
                }
            }
            return new List<DAT_Project>().AsQueryable();
        }

        public IQueryable<DAT_Project> ExportProjectInfoList(ProjectQueryParams project)
        {
            var dt = GetCityTable(project.CityId, project.FxtCompanyId).FirstOrDefault();
            if (dt != null)
            {
                var ptable = dt.projecttable;
                var btable = dt.buildingtable;
                var comId = dt.ShowCompanyId ?? project.FxtCompanyId.ToString();

                var queryWhere = string.Empty;
                if (project.ProjectId > 0)
                {
                    queryWhere += " and p.ProjectId=@ProjectId";
                }
                if (project.AreaId > 0)
                {
                    queryWhere += " and p.AreaId=@AreaId";
                }
                if (project.SubAreaId > 0)
                {
                    queryWhere += " and p.SubAreaId=@SubAreaId";
                }
                if (project.BuildingTypeCode > 0)
                {
                    queryWhere += " and p.BuildingTypeCode=@BuildingTypeCode";
                }
                if (!string.IsNullOrEmpty(project.Key))
                {
                    queryWhere += " and (p.projectName like @projectName or p.OtherName like @projectName)";
                }
                if (project.PlanPurpose > 0)
                {
                    queryWhere += " and p.PurposeCode=@PurposeCode";
                }
                var strsql = @"
select
	(case when p.PlanPurpose like '1111%' then 
		 (select CodeName+',' from (
			select c.CodeName from FXTProject.dbo.SplitToTable(p.PlanPurpose,',')S,FxtDataCenter.dbo.SYS_Code c 
			where S.value = c.Code
		)T for XML Path(''))
		else p.PlanPurpose end
	) as opValue,
	p.*
	,(
	  select top 1 ISNULL(c.BuildingNumber,0) 
	  from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK)
	  where c.projectId = p.projectId and c.cityId = @cityid and c.fxtcompanyId = @fxtcompanyid
	  and c.BuildingId = 0
	  ) as BuildingNumber
	  ,(
	  select top 1 ISNULL(c.HouseNumber,0) 
	  from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK)
	  where c.projectId = p.projectId and c.cityId = @cityid and c.fxtcompanyId = @fxtcompanyid
	  and c.BuildingId = 0
	  ) as HouseNumber
	,ci.CityName
	,sa.AreaName
	,ssa.SubAreaName
    ,SourceName
    ,DeveCompanyName = (SELECT top 1 ChineseName FROM FXTProject.dbo.LNK_P_Company pc ,FxtDataCenter.dbo.DAT_Company d WHERE pc.CompanyId = d.CompanyId AND pc.CityId = @CityId AND pc.ProjectId = p.ProjectId AND PC.CompanyType = 2001001)
    ,ManagerCompanyName = (SELECT top 1 ChineseName FROM FXTProject.dbo.LNK_P_Company pc,FxtDataCenter.dbo.DAT_Company d WHERE pc.CompanyId = d.CompanyId AND pc.CityId = @CityId AND pc.ProjectId = p.ProjectId AND PC.CompanyType = 2001004)
from (
	select
		ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,ParkingDesc,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,SourceName,FxtCompanyId
        ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=FxtCompanyId)
	from " + ptable + @" p with(nolock)
	where not exists(
		select ProjectId from " + ptable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.Fxt_CompanyId = @fxtcompanyid
		and ps.CityID = @cityid
	)
	and p.Valid = 1
	and p.FxtCompanyId in (" + comId + @")
	and p.CityID = @cityid
	union
	select 
		p.ProjectId,p.ProjectName,p.SubAreaId,p.FieldNo,p.PurposeCode,p.Address,p.LandArea,p.StartDate,p.StartEndDate,p.UsableYear,p.BuildingArea,p.SalableArea,p.CubageRate,p.GreenRate,p.BuildingDate,p.CoverDate,p.SaleDate,p.JoinDate,p.EndDate,p.InnerSaleDate,p.RightCode,p.ParkingNumber,p.ParkingDesc,p.AveragePrice,p.ManagerTel,p.ManagerPrice,p.TotalNum,p.BuildingNum,p.Detail,p.BuildingTypeCode,p.UpdateDateTime,p.OfficeArea,p.OtherArea,p.PlanPurpose,p.PriceDate,p.IsComplete,p.OtherName,p.SaveDateTime,p.SaveUser,p.Weight,p.BusinessArea,p.IndustryArea,p.IsEValue,p.PinYin,p.CityID,p.AreaID,p.OldId,p.CreateTime,p.AreaLineId,p.Valid,p.SalePrice,p.PinYinAll,p.X,p.Y,p.XYScale,p.Creator,p.IsEmpty,p.TotalId,p.East,p.West,p.South,p.North,p.BuildingQuality,p.HousingScale,p.BuildingDetail,p.HouseDetail,p.BasementPurpose,p.ManagerQuality,p.Facilities,p.AppendageClass,p.RegionalAnalysis,p.Wrinkle,p.Aversion,p.SourceName,p.Fxt_CompanyId
        ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=pi.FxtCompanyId)
	from " + ptable + "_sub p with(nolock) left join " + ptable + @" pi with(nolock) on p.ProjectId = pi.ProjectId
	where p.Valid = 1
	and p.Fxt_CompanyId = @fxtcompanyid
	and p.CityID = @cityid
)p
LEFT JOIN FxtDataCenter.dbo.SYS_Area sa WITH (NOLOCK) ON sa.AreaId = p.AreaID
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea ssa WITH (NOLOCK) ON ssa.SubAreaId = p.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_City ci WITH (NOLOCK) ON ci.cityid = p.cityid
where 1 = 1  " + queryWhere;

                strsql = string.IsNullOrEmpty(project.KeyString)
                    ? strsql
                    : (
@"
select T.* from (" + strsql + @" 
)T
inner join (
	select 
		distinct Tp.ProjectId
	from (
		select			
			ProjectId
			,ProjectName
			,PinYin
			,PinYinAll
			,OtherName as POtherName
			,Address
		from " + ptable + @" p with(nolock)
		where not exists(
			select ProjectId from " + ptable + @"_sub ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.FxtCompanyId = @fxtcompanyid
			and ps.CityID = @cityid
		)
		and p.FxtCompanyId in (" + comId + @")
		and p.CityID = @cityid
		union
		select 		
			ProjectId
			,ProjectName
			,PinYin
			,PinYinAll
			,OtherName as POtherName
			,Address
		from " + ptable + @"_sub p with(nolock)
		where 1 = 1
		and p.FxtCompanyId = @fxtcompanyid
		and p.CityID = @cityid
	)Tp
	left join (
		select
			ProjectId
			,BuildingId
			,BuildingName
			,Doorplate
			,OtherName as BOtherName
		from " + btable + @" b with(nolock)
		where not exists(
			select BuildingId from " + btable + @"_sub bs with(nolock)
			where bs.BuildingId = b.BuildingId
			and bs.FxtCompanyId = @fxtcompanyid
			and bs.CityID = @cityid
		)
		and b.FxtCompanyId in (" + comId + @")
		and b.CityID = @cityid
		union
		select 
			ProjectId
			,BuildingId
			,BuildingName
			,Doorplate
			,OtherName as BOtherName
		from " + btable + @"_sub b with(nolock)
		where 1 = 1
		and b.FxtCompanyId = @fxtcompanyid
		and b.CityID = @cityid
	)Tb on Tp.ProjectId = Tb.ProjectId
	where 1 = 1 and ( ProjectName like @keystring
	or PinYin like @keystring 
	or PinYinAll like @keystring 
	or POtherName like @keystring
	or Address like @keystring
	or BuildingName like @keystring
	or Doorplate like @keystring
	or BOtherName like @keystring
	)
)T1 on T.ProjectId = T1.ProjectId");

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var projectList = con.Query<DAT_Project>(strsql, new
                    {
                        project.CityId,
                        project.FxtCompanyId,
                        project.ProjectId,
                        project.AreaId,
                        project.SubAreaId,
                        project.BuildingTypeCode,
                        projectName = "%" + project.Key + "%",
                        PurposeCode = project.PlanPurpose,
                        keystring = "%" + project.KeyString + "%"
                    }).AsQueryable();

                    return projectList;
                }
            }
            return new List<DAT_Project>().AsQueryable();
        }

        /// <summary>
        /// 新增楼盘
        /// </summary>
        /// <param name="project">楼盘对象</param>
        /// <returns></returns>
        public int AddProject(DAT_Project project)
        {
            try
            {
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var projectId = 0;
                var list = GetCityTable(project.cityid, project.fxtcompanyid).FirstOrDefault();
                if (list != null)
                {
                    var ptable = list.projecttable;
                    var comId = list.ShowCompanyId;
                    //判断是否已删除的楼盘
                    var listProject = ProjectDeleteOrExissts(ptable, comId, project, 0).FirstOrDefault();
                    if (listProject != null)
                    {
                        string pid = listProject.projectid.ToString();
                        project.projectid = Convert.ToInt32(pid);
                        project.savedatetime = DateTime.Now;
                        project.saveuser = project.creator;
                        ModifyProject(project, project.fxtcompanyid);
                        return Convert.ToInt32(pid);

                    }
                    //楼盘已经存在

                    var listProjectOne = ProjectDeleteOrExissts(ptable, comId, project, 1).FirstOrDefault();
                    if (listProjectOne != null)
                    {
                        var pid = listProjectOne.projectid.ToString();
                        return Convert.ToInt32(pid);
                    }

                    string strsql = @"
INSERT INTO " + ptable + @" (ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,FxtCompanyId,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,ParkingDesc,SourceName)
VALUES(@ProjectName,@SubAreaId,@FieldNo,@PurposeCode,@Address,@LandArea,@StartDate,@StartEndDate,@UsableYear,@BuildingArea,@SalableArea,@CubageRate,@GreenRate,@BuildingDate,@CoverDate,@SaleDate,@JoinDate,@EndDate,@InnerSaleDate,@RightCode,@ParkingNumber,@AveragePrice,@ManagerTel,@ManagerPrice,@TotalNum,@BuildingNum,@Detail,@BuildingTypeCode,@UpdateDateTime,@OfficeArea,@OtherArea,@PlanPurpose,@PriceDate,@IsComplete,@OtherName,@Weight,@BusinessArea,@IndustryArea,@IsEValue,@PinYin,@CityID,@AreaID,@OldId,@CreateTime,@AreaLineId,@Valid,@SalePrice,@FxtCompanyId,@PinYinAll,@X,@Y,@XYScale,@Creator,@IsEmpty,@TotalId,@East,@West,@South,@North,@BuildingQuality,@HousingScale,@BuildingDetail,@HouseDetail,@BasementPurpose,@ManagerQuality,@Facilities,@AppendageClass,@RegionalAnalysis,@Wrinkle,@Aversion,@ParkingDesc,@SourceName);
SELECT SCOPE_IDENTITY() AS Id;";

                    project.createtime = DateTime.Now;
                    project.savedatetime = DateTime.Now;
                    project.updatedatetime = DateTime.Now;
                    if (project.purposecode == null)
                    {
                        project.purposecode = 1001001;
                    }
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        dynamic identity = con.Query(strsql, project).Single();
                        projectId = Convert.ToInt32(identity.Id);
                        //int r = con.Execute(strsql, project);
                        //开发商与物业公司
                        if (projectId > 0)
                        {
                            //dynamic identity = con.Query("SELECT SCOPE_IDENTITY() AS Id").Single();
                            //projectId = Convert.ToInt32(identity.Id);
                            //记录字段修改日志
                            if (project.enddate != null)
                            {
                                Log log = new Log();
                                log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", projectId, "enddate", "", ((DateTime)project.enddate).ToString("yyyy-MM-dd"), project.creator);
                            }
                            if (project.x != null || project.y != null)
                            {
                                Log log = new Log();
                                log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", projectId, "xy", "", project.x.ToString() + "," + project.y.ToString(), project.creator);
                            }
                            if (!string.IsNullOrWhiteSpace(project.address))
                            {
                                Log log = new Log();
                                log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", projectId, "address", "", project.address, project.creator);
                            }
                            if (!string.IsNullOrWhiteSpace(project.othername))
                            {
                                Log log = new Log();
                                log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", projectId, "othername", "", project.othername, project.creator);
                            }
                            if (project.buildingtypecode != null && project.buildingtypecode > 0)
                            {
                                Log log = new Log();
                                log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", projectId, "buildingtypecode", "", project.buildingtypecode.ToString(), project.creator);
                            }

                            if (project.DeveCompanyName != null && project.DeveCompanyName.Trim() != "")
                            {
                                int ri = addRelatedCompany(project.DeveCompanyName.Trim(), "", "2001001", project.cityid, "", "", "", project.fxtcompanyid);
                                if (ri > 0)
                                {
                                    project.DeveCompanyId = ri;
                                    AddCityCompany(project.cityid, ri);
                                }
                            }
                            if (project.DeveCompanyId > 0)
                            {
                                strsql = @"insert into FXTProject.dbo.LNK_P_Company (ProjectId,CompanyType,CityId,CompanyId) 
                                                               values (@ProjectId,2001001,@CityId,@DeveCompanyId)";
                                int number = con.Execute(strsql, new { ProjectId = projectId, CityId = project.cityid, DeveCompanyId = project.DeveCompanyId });
                            }
                            if (project.ManagerCompanyName != null && project.ManagerCompanyName.Trim() != "")
                            {
                                if (project.ManagerCompanyName.Trim() == project.DeveCompanyName.Trim())
                                {
                                    project.ManagerCompanyId = project.DeveCompanyId;
                                }
                                else
                                {
                                    int ri = addRelatedCompany(project.ManagerCompanyName.Trim(), "", "2001004", project.cityid, "", "", "", project.fxtcompanyid);
                                    if (ri > 0)
                                    {
                                        project.ManagerCompanyId = ri;
                                        AddCityCompany(project.cityid, ri);
                                    }
                                }
                            }
                            if (project.ManagerCompanyId > 0)
                            {
                                strsql = @"insert into FXTProject.dbo.LNK_P_Company (ProjectId,CompanyType,CityId,CompanyId) 
                                                               values (@ProjectId,2001004,@CityId,@ManagerCompanyId)";
                                int number = con.Execute(strsql, new { ProjectId = projectId, CityId = project.cityid, ManagerCompanyId = project.ManagerCompanyId });
                            }
                        }
                    }
                    if (projectId > 0)
                    {
                        if (!string.IsNullOrEmpty(project.LngOrLat))
                        {
                            var a = new ProjectCoordinate();
                            if (project.LngOrLat.Contains("|"))
                            {
                                string[] lngOrlat = project.LngOrLat.Split('|');
                                for (int i = 0; i < lngOrlat.Length; i++)
                                {
                                    DAT_Project_Coordinate mo = new DAT_Project_Coordinate();
                                    mo.ProjectId = projectId;
                                    mo.CityID = project.cityid;
                                    mo.FxtCompanyId = project.fxtcompanyid;
                                    mo.X = Convert.ToDecimal(lngOrlat[i].Split(',')[0]);
                                    mo.Y = Convert.ToDecimal(lngOrlat[i].Split(',')[1]);
                                    mo.Valid = project.valid;
                                    a.AddProjectCoordinate(mo);
                                }
                            }
                            else
                            {
                                DAT_Project_Coordinate mo = new DAT_Project_Coordinate();
                                mo.ProjectId = projectId;
                                mo.CityID = project.cityid;
                                mo.FxtCompanyId = project.fxtcompanyid;
                                if (project.LngOrLat.Contains(","))
                                {
                                    if (Convert.ToInt32(Convert.ToDecimal(project.LngOrLat.Split(',')[0])) > 0 || Convert.ToInt32(Convert.ToDecimal(project.LngOrLat.Split(',')[1])) > 0)
                                    {
                                        mo.Valid = 1;
                                    }
                                    else
                                    {
                                        mo.Valid = 0;
                                    }
                                    mo.X = Convert.ToDecimal(project.LngOrLat.Split(',')[0]);
                                    mo.Y = Convert.ToDecimal(project.LngOrLat.Split(',')[1]);
                                }
                                else
                                {
                                    mo.Valid = 0;
                                    mo.X = Convert.ToDecimal(project.LngOrLat);
                                    mo.Y = Convert.ToDecimal(project.LngOrLat);
                                }
                                a.AddProjectCoordinate(mo);
                            }
                        }
                    }
                    return projectId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加对应公司
        /// </summary>
        /// <param name="cityId">城市Id</param>
        /// <param name="Companyid">公司Id</param>
        /// <returns></returns>
        private int AddCityCompany(int cityId, int Companyid)
        {

            try
            {
                string selectSql =
                    @"select count(1) from LNK_City_Company where cityid=@cityId and companyId=@companyid";

                string sql = @"insert into LNK_City_Company (Cityid,Companyid) values(@CityId,@Companyid)";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {

                    var count = con.Query<int>(selectSql, new { cityId, Companyid }).FirstOrDefault();
                    if (count == 0)
                    {
                        return con.Execute(sql, new { CityId = cityId, Companyid });
                    }
                    return 0;
                }

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 新增公司信息
        /// </summary>
        /// <param name="ChineseName">中文公司名称</param>
        /// <param name="EnglishName">英文公司名称</param>
        /// <param name="CompanyTypeCode">公司code</param>
        /// <param name="CityId">城市Id</param>
        /// <param name="Address">公司地址</param>
        /// <param name="Telephone">公司电话</param>
        /// <param name="Fax"></param>
        /// <returns></returns>
        private int addRelatedCompany(string ChineseName, string EnglishName, string CompanyTypeCode, int CityId, string Address, string Telephone, string Fax, int fxtcompanyId)
        {
            int compandid = 0;
            try
            {
                string sql = "select CompanyId,valid from FxtDataCenter.dbo.DAT_Company with(nolock) where [ChineseName]=@ChineseName";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var com = con.Query<DAT_Company>(sql, new { ChineseName = ChineseName }).FirstOrDefault();
                    if (com != null)
                    {
                        compandid = com.CompanyId;
                        if (com.Valid == 0)
                        {
                            sql = "update FxtDataCenter.dbo.DAT_Company set valid=1,CreateDate=Getdate(),fxtcompanyid=@fxtcompanyid where CompanyId=@CompanyId";
                            con.Execute(sql, new { CompanyId = compandid, fxtcompanyid = fxtcompanyId });
                        }
                        return compandid;
                    }
                    sql = @"INSERT INTO FxtDataCenter.dbo.DAT_Company ([ChineseName],[EnglishName],CompanyTypeCode,[CityId],[Address],[Telephone],[Fax],fxtcompanyid) 
                    VALUES(@ChineseName,@EnglishName,@CompanyTypeCode,@CityId,@Address,@Telephone,@Fax,@fxtcompanyid);SELECT SCOPE_IDENTITY() AS Id";
                    //int num = con.Execute(sql, new
                    //{
                    //    ChineseName = ChineseName,
                    //    EnglishName = EnglishName,
                    //    CompanyTypeCode = CompanyTypeCode,
                    //    CityId = CityId,
                    //    Address = Address,
                    //    Telephone = Telephone,
                    //    Fax = Fax,
                    //    fxtcompanyid = fxtcompanyId
                    //});
                    //if (num > 0)
                    //{
                    //    dynamic identity = con.Query("SELECT @@IDENTITY AS Id").Single();
                    //    compandid = Convert.ToInt32(identity.Id);
                    //}
                    //return compandid;

                    dynamic identity = con.Query(sql, new
                    {
                        ChineseName = ChineseName,
                        EnglishName = EnglishName,
                        CompanyTypeCode = CompanyTypeCode,
                        CityId = CityId,
                        Address = Address,
                        Telephone = Telephone,
                        Fax = Fax,
                        fxtcompanyid = fxtcompanyId
                    }).Single();
                    compandid = Convert.ToInt32(identity.Id);
                    return compandid;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region    修改楼盘

        public int ModifyProject(DAT_Project project, int currFxtCompanyId)
        {
            try
            {
                var list = GetCityTable(project.cityid, currFxtCompanyId).FirstOrDefault();
                if (list != null)
                {
                    var ptable = "FXTProject." + list.projecttable;
                    var comId = list.ShowCompanyId;

                    //获取之前的楼盘信息
                    var projectBe = ProjectDeleteOrExissts(ptable, comId, new DAT_Project { projectname = project.projectname, areaid = project.areaid, cityid = project.cityid, fxtcompanyid = currFxtCompanyId }, 1).FirstOrDefault();
                    if (projectBe == null)
                    {
                        projectBe = ProjectDeleteOrExissts(ptable, comId, new DAT_Project { projectname = project.projectname, areaid = project.areaid, cityid = project.cityid, fxtcompanyid = currFxtCompanyId }, 0).FirstOrDefault();
                    }
                    DateTime? enddateBe = projectBe == null ? null : projectBe.enddate;
                    decimal? xBe = projectBe == null ? null : projectBe.x;
                    decimal? yBe = projectBe == null ? null : projectBe.y;
                    string addressBe = projectBe == null ? "" : projectBe.address;
                    string othernameBe = projectBe == null ? "" : projectBe.othername;
                    int buildingtypecodeBe = projectBe == null || projectBe.buildingtypecode == null ? 0 : ((int)projectBe.buildingtypecode <= 0 ? 0 : (int)projectBe.buildingtypecode);

                    var r = 0; //对主表或子表修改结果
                    var sql = string.Empty;

                    var mainTable = "Update " + ptable + " with(rowlock) ";
                    var subTable = "Update " + ptable + "_sub with(rowlock) ";
                    var where = " Where ProjectId=@ProjectId and CityId=@CityId and [Fxt_CompanyId] = @FxtCompanyId";
                    var updateFields = @" SET [ProjectName] = @ProjectName,[SubAreaId] = @SubAreaId,[FieldNo] = @FieldNo,[PurposeCode] = @PurposeCode,[Address] = @Address,[LandArea] = @LandArea,[StartDate] = @StartDate,[StartEndDate]=@StartEndDate,[UsableYear] = @UsableYear,[BuildingArea] = @BuildingArea,[SalableArea] = @SalableArea,[CubageRate] = @CubageRate,[GreenRate] = @GreenRate,[BuildingDate] = @BuildingDate,[CoverDate] = @CoverDate,[SaleDate] = @SaleDate,[JoinDate] = @JoinDate,[EndDate] = @EndDate,[InnerSaleDate] = @InnerSaleDate,[RightCode] = @RightCode,[ParkingNumber] = @ParkingNumber,[ParkingDesc] = @ParkingDesc,[AveragePrice] = @AveragePrice,[ManagerTel] = @ManagerTel,[ManagerPrice] = @ManagerPrice,[TotalNum] = @TotalNum,[BuildingNum] = @BuildingNum,[Detail] = @Detail,[BuildingTypeCode] = @BuildingTypeCode,[UpdateDateTime] = @UpdateDateTime,[OfficeArea] = @OfficeArea,[OtherArea] = @OtherArea,[PlanPurpose] = @PlanPurpose,[PriceDate] = @PriceDate,[IsComplete] = @IsComplete,[OtherName] = @OtherName,[SaveDateTime] = @SaveDateTime,[SaveUser] = @SaveUser,[Weight] = @Weight,[BusinessArea] = @BusinessArea,[IndustryArea] = @IndustryArea,[IsEValue] = @IsEValue,[PinYin] = @PinYin,[CityID] = @CityID,[AreaID] = @AreaID,[AreaLineId] = @AreaLineId,[Valid] = @Valid,[SalePrice] = @SalePrice,[PinYinAll] = @PinYinAll,[X] = @X,[Y] = @Y,[XYScale] = @XYScale,[IsEmpty] = @IsEmpty,[TotalId] = @TotalId,[East] = @East,[West] = @West,[South] = @South,[North] = @North,[BuildingQuality] = @BuildingQuality,[HousingScale] = @HousingScale,[BuildingDetail] = @BuildingDetail,[HouseDetail] = @HouseDetail,[BasementPurpose] = @BasementPurpose,[ManagerQuality] = @ManagerQuality,[Facilities] = @Facilities,[AppendageClass] = @AppendageClass,[RegionalAnalysis] = @RegionalAnalysis,[Wrinkle] = @Wrinkle,[Aversion] = @Aversion ,SourceName = @SourceName";

                    var searchtable = string.Empty;
                    if (currFxtCompanyId == _fxtComId) //当前操作者房讯通, 只更新主表，不插入子表修改
                    {
                        sql = "delete from " + ptable + @"_sub with(rowlock) WHERE [ProjectId]=@ProjectId and CityId=@CityId and [Fxt_CompanyId] =" + _fxtComId;
                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            con.Execute(sql, project);//删除掉子表中等于companyId=25的数据（以前的错误数据）
                        }

                        sql = mainTable + updateFields + " where projectId=@ProjectId and CityId=@CityId";
                        searchtable = "mainTable";
                    }
                    else if (project.fxtcompanyid == currFxtCompanyId)
                    {
                        //本评估机构修改本评估机构数据时，先检查一下，主子表是否同时存在相同fxtcompanyid，相同projectid的数据。如果存在，先把子表的数据给删掉。20151123
                        string fxtcompanysql = "select * from  " + ptable + "  where ProjectId = " + project.projectid + " and CityId = " + project.cityid + " and FxtCompanyId = " + currFxtCompanyId;
                        string fxt_companysql = "select * from  " + ptable + "_sub  where ProjectId = " + project.projectid + " and CityId = " + project.cityid + " and Fxt_CompanyId = " + currFxtCompanyId;
                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            DataTable obj = DBHelperSql.ExecuteDataTable(fxtcompanysql);
                            DataTable obj1 = DBHelperSql.ExecuteDataTable(fxt_companysql);
                            if (obj != null && obj.Rows.Count == 1 && obj1 != null && obj1.Rows.Count == 1)
                            {
                                string deletefxt_companysql = "delete from " + ptable + @"_sub with(rowlock) WHERE ProjectId = " + project.projectid + " and CityId = " + project.cityid + " and Fxt_CompanyId =" + currFxtCompanyId;
                                con.Execute(deletefxt_companysql);
                            }
                        }

                        var subSql = subTable + updateFields + "  WHERE [ProjectId]=@ProjectId and CityId=@CityId and [Fxt_CompanyId] =@FxtCompanyId";

                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            r = con.Execute(subSql, project);
                            searchtable = "subTable";
                            if (r < 1) //子表不存在就更新主表
                            {
                                sql = mainTable + updateFields + " where projectId=@ProjectId and CityId=@CityId";
                                searchtable = "mainTable";
                            }
                        }

                    }
                    else
                    {
                        sql = @"SELECT [ProjectId] FROM " + ptable + @"_sub with(nolock) 
                                   WHERE [ProjectId]=@ProjectId and CityId=@CityId and [Fxt_CompanyId] =@FxtCompanyId";
                        SqlParameter[] parameters = {
                                             new SqlParameter("@ProjectId",project.projectid),
                                             new SqlParameter("@CityId",project.cityid),
                                             new SqlParameter("@FxtCompanyId",currFxtCompanyId),
                                         };
                        DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                        DataTable obj = DBHelperSql.ExecuteDataTable(sql, parameters);
                        if (obj != null && obj.Rows.Count > 0)
                        {
                            sql = subTable + updateFields + where;
                        }
                        else
                        {
                            sql = @"
insert into " + ptable + @"_sub(ProjectId,[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address],[LandArea],[StartDate],[StartEndDate],[UsableYear],[BuildingArea],[SalableArea]
,[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[ParkingDesc]
,[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode],[UpdateDateTime],[OfficeArea],[OtherArea]
,[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue]
,[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId],[Valid],[SalePrice],[Fxt_CompanyId],[PinYinAll],[X],[Y],[XYScale]
,[Creator],[IsEmpty],[TotalId],[East],[West],[South],[North],[BuildingQuality],[HousingScale],[BuildingDetail],[HouseDetail]
,[BasementPurpose],[ManagerQuality],[Facilities],[AppendageClass],[RegionalAnalysis],[Wrinkle],[Aversion],SourceName)
select 
    ProjectId,@ProjectName,@SubAreaId,@FieldNo,@PurposeCode,@Address,@LandArea,@StartDate,@StartEndDate,@UsableYear,@BuildingArea,@SalableArea
,@CubageRate,@GreenRate,@BuildingDate,@CoverDate,@SaleDate,@JoinDate,@EndDate,@InnerSaleDate,@RightCode,@ParkingNumber,@ParkingDesc
,@AveragePrice,@ManagerTel,@ManagerPrice,@TotalNum,@BuildingNum,@Detail,@BuildingTypeCode,@UpdateDateTime,@OfficeArea,@OtherArea
,@PlanPurpose,@PriceDate,@IsComplete,@OtherName,@SaveDateTime,@SaveUser,@Weight,@BusinessArea,@IndustryArea,@IsEValue
,@PinYin,@CityID,@AreaID,@OldId,CreateTime,@AreaLineId,1 as Valid,@SalePrice,'" + currFxtCompanyId + @"' as FxtCompanyId,@PinYinAll,@X,@Y,@XYScale
,Creator,@IsEmpty,@TotalId,@East,@West,@South,@North,@BuildingQuality,@HousingScale,@BuildingDetail,@HouseDetail
,@BasementPurpose,@ManagerQuality,@Facilities,@AppendageClass,@RegionalAnalysis,@Wrinkle,@Aversion,@SourceName
from " + ptable + @" with(nolock) where ProjectId =@ProjectId and CityId=@CityId";

                        }
                        searchtable = "subTable";
                    }
                    project.savedatetime = DateTime.Now;
                    project.updatedatetime = DateTime.Now;
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        if (!string.IsNullOrEmpty(sql))
                        {
                            r = con.Execute(sql, project);
                            ////更新search表
                            //if (searchtable == "mainTable")
                            //{
                            //    string tablesql = @"select * from " + ptable + @" with(nolock) where valid = 1 and cityid = " + project.cityid + @" and projectid = " + project.projectid;
                            //    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                            //    {
                            //        var result = conn.Query<DAT_Project>(tablesql).FirstOrDefault();
                            //        if (result != null)
                            //        {
                            //            AddProjectSearch(ptable + "_Search", result.cityid, result.fxtcompanyid, result.projectid, result.projectname, result.address, result.othername, result.pinyin, result.pinyinall);
                            //        }
                            //    }
                            //}
                            //if (searchtable == "subTable")
                            //{
                            //    AddProjectSubSearch(ptable + "_sub_Search", project.cityid, currFxtCompanyId, project.projectid, project.projectname, project.address, project.othername, project.pinyin, project.pinyinall);
                            //}
                        }
                    }
                    if (r > 0)
                    {
                        //记录字段修改日志
                        var dataAfter = ProjectDeleteOrExissts(ptable, comId, new DAT_Project { projectname = project.projectname, areaid = project.areaid, cityid = project.cityid, fxtcompanyid = currFxtCompanyId }, 1).FirstOrDefault();
                        DateTime? enddateAf = dataAfter.enddate;
                        decimal? xAf = dataAfter.x;
                        decimal? yAf = dataAfter.y;
                        string addressAf = dataAfter.address;
                        string othernameAf = dataAfter.othername;
                        int buildingtypecodeAf = dataAfter.buildingtypecode == null ? 0 : ((int)dataAfter.buildingtypecode <= 0 ? 0 : (int)dataAfter.buildingtypecode);
                        if (enddateBe != enddateAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "enddate", enddateBe == null ? "" : ((DateTime)enddateBe).ToString("yyyy-MM-dd"), enddateAf == null ? "" : ((DateTime)enddateAf).ToString("yyyy-MM-dd"), project.saveuser);
                        }
                        if (xBe != xAf || yBe != yAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "xy", xBe.ToString() + "," + yBe.ToString(), xAf.ToString() + "," + yAf.ToString(), project.saveuser);
                        }
                        if (addressBe != addressAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "address", addressBe, addressAf, project.saveuser);
                        }
                        if (othernameBe != othernameAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "othername", othernameBe, othernameAf, project.saveuser);
                        }
                        if (buildingtypecodeBe != buildingtypecodeAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "buildingtypecode", buildingtypecodeBe.ToString(), buildingtypecodeAf.ToString(), project.saveuser);
                        }

                        //开发商
                        if (!string.IsNullOrEmpty(project.DeveCompanyName))
                        {
                            int ri = addRelatedCompany(project.DeveCompanyName, "", "2001001", project.cityid, "", "", "", currFxtCompanyId);
                            if (ri > 0)
                            {
                                project.DeveCompanyId = ri;
                                AddCityCompany(project.cityid, ri);
                            }
                        }
                        if (project.DeveCompanyId > 0)
                        {
                            sql = @"select CompanyId  from FXTProject.dbo.LNK_P_Company with(nolock) where CityId =@CityId and CompanyType = 2001001 and ProjectId=@ProjectId";
                            SqlParameter[] parameters1 =
                                              {
                                                  new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                            var dt = new DataTable();
                            dt.Reset();
                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            dt = DBHelperSql.ExecuteDataTable(sql, parameters1);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string companyId = dt.Rows[0]["CompanyId"].ToString();
                                if (companyId != project.DeveCompanyId.ToString())
                                {
                                    sql = @"update FXTProject.dbo.LNK_P_Company with(rowlock) set CompanyId=@DeveCompanyId where CityId =@CityId and CompanyType = 2001001  and ProjectId=@ProjectId";
                                    SqlParameter[] parameters2 =
                                              {
                                                  new SqlParameter("@DeveCompanyId",project.DeveCompanyId),
                                                  new SqlParameter("@CityId",project.cityid),
                                                   new SqlParameter("@ProjectId",project.projectid)
                                              };
                                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                                    DBHelperSql.ExecuteNonQuery(sql, parameters2);

                                }

                            }
                            else
                            {
                                sql = "insert into FXTProject.dbo.LNK_P_Company(ProjectId,CompanyType,CityId,CompanyId) "
                                       + "values (@ProjectId,2001001,@CityId,@DeveCompanyId)";
                                SqlParameter[] parameters2 =
                                              {
                                                  new SqlParameter("@DeveCompanyId",project.DeveCompanyId),
                                                  new SqlParameter("@CityId",project.cityid),
                                                   new SqlParameter("@ProjectId",project.projectid)
                                              };
                                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                                DBHelperSql.ExecuteNonQuery(sql, parameters2);
                            }
                        }
                        else
                        {
                            sql = "delete FXTProject.dbo.LNK_P_Company with(rowlock) where CityId =@CityId and CompanyType = 2001001 and ProjectId=@ProjectId";
                            SqlParameter[] parameters4 ={
                                                  new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            DBHelperSql.ExecuteNonQuery(sql, parameters4);
                        }
                        //end开发商
                        //物业管理公司
                        if (!string.IsNullOrEmpty(project.ManagerCompanyName))
                        {
                            if (project.ManagerCompanyName == project.DeveCompanyName)
                            {
                                project.ManagerCompanyId = project.DeveCompanyId;
                            }
                            else
                            {
                                int ri = addRelatedCompany(project.ManagerCompanyName, "", "2001004", project.cityid, "", "", "", currFxtCompanyId);
                                if (ri > 0)
                                {
                                    project.ManagerCompanyId = ri;
                                    AddCityCompany(project.cityid, ri);
                                }
                            }
                        }
                        if (project.ManagerCompanyId > 0)
                        {
                            sql = "select CompanyId  from FXTProject.dbo.LNK_P_Company with(nolock) where CityId =@CityId and CompanyType = 2001004 and ProjectId=@ProjectId";
                            SqlParameter[] parameters5 ={
                                                  new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                            var dt = new DataTable();
                            dt.Reset();
                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            dt = DBHelperSql.ExecuteDataTable(sql, parameters5);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string companyId = dt.Rows[0]["CompanyId"].ToString();
                                if (companyId != project.ManagerCompanyId.ToString())
                                {
                                    sql = @"update FXTProject.dbo.LNK_P_Company with(rowlock) set CompanyId=@ManagerCompanyId where CityId =@CityId 
                                        and CompanyType = 2001004 and ProjectId=@ProjectId";
                                    SqlParameter[] parameters6 ={
                                                  new SqlParameter("@ManagerCompanyId",project.ManagerCompanyId),
                                                   new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                                    DBHelperSql.ExecuteNonQuery(sql, parameters6);
                                }
                            }
                            else
                            {
                                sql = @"insert into FXTProject.dbo.LNK_P_Company(ProjectId,CompanyType,CityId,CompanyId)
                                    values (@ProjectId,2001004,@CityId,@ManagerCompanyId)";

                                SqlParameter[] parameters6 ={
                                                  new SqlParameter("@ManagerCompanyId",project.ManagerCompanyId),
                                                   new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                                DBHelperSql.ExecuteNonQuery(sql, parameters6);
                            }
                        }
                        else
                        {
                            sql = "delete FXTProject.dbo.LNK_P_Company with(rowlock) where CityId =@CityId and CompanyType = 2001004 and ProjectId=@ProjectId";
                            SqlParameter[] parameters8 ={
                                                  new SqlParameter("@ProjectId",project.projectid),
                                                  new SqlParameter("@CityId",project.cityid)
                                              };
                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            DBHelperSql.ExecuteNonQuery(sql, parameters8);
                        }
                        //end物业管理公司
                        //if (project.ModifyPrice != null && project.ModifyPrice == "1" && project.averageprice != null && Convert.ToDouble(project.averageprice) > 0)
                        //{
                        //    ModifyProjectPrice(project.cityid, project.fxtcompanyid, project.projectid, project.averageprice.ToString(), project.saveuser, currFxtCompanyId);
                        //}

                    }
                    if (r > 0)
                    {
                        var a = new ProjectCoordinate();
                        if (!string.IsNullOrEmpty(project.LngOrLat))
                        {
                            if (project.LngOrLat.Contains("|"))
                            {
                                a.DeleteProjectCoordinate(project.fxtcompanyid, project.cityid, project.projectid);
                                string[] lngOrlat = project.LngOrLat.Split('|');
                                for (int j = 0; j < lngOrlat.Length; j++)
                                {
                                    var mo = new DAT_Project_Coordinate
                                    {
                                        ProjectId = project.projectid,
                                        CityID = project.cityid,
                                        FxtCompanyId = project.fxtcompanyid,
                                        X = Convert.ToDecimal(lngOrlat[j].Split(',')[0]),
                                        Y = Convert.ToDecimal(lngOrlat[j].Split(',')[1]),
                                        Valid = project.valid
                                    };
                                    a.AddProjectCoordinate(mo);
                                }
                            }
                            else
                            {
                                a.DeleteProjectCoordinate(project.fxtcompanyid, project.cityid, project.projectid);
                                var mo = new DAT_Project_Coordinate
                                {
                                    ProjectId = project.projectid,
                                    CityID = project.cityid,
                                    FxtCompanyId = project.fxtcompanyid
                                };
                                if (project.LngOrLat.Contains(","))
                                {
                                    if (Convert.ToInt32(Convert.ToDecimal(project.LngOrLat.Split(',')[0])) > 0 || Convert.ToInt32(Convert.ToDecimal(project.LngOrLat.Split(',')[1])) > 0)
                                    {
                                        mo.Valid = 1;
                                    }
                                    else
                                    {
                                        mo.Valid = 0;
                                    }
                                    mo.X = Convert.ToDecimal(project.LngOrLat.Split(',')[0]);
                                    mo.Y = Convert.ToDecimal(project.LngOrLat.Split(',')[1]);
                                }
                                else
                                {
                                    mo.Valid = 0;
                                    mo.X = Convert.ToDecimal(project.LngOrLat);
                                    mo.X = Convert.ToDecimal(project.LngOrLat);
                                }
                                a.AddProjectCoordinate(mo);
                            }
                        }
                        else
                        {
                            a.DeleteProjectCoordinate(project.fxtcompanyid, project.cityid, project.projectid);
                        }
                    }
                    return r;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UpdateProjectInfo4Excel(DAT_Project project, int currFxtCompanyId, List<string> modifiedProperty)
        {
            try
            {
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var list = GetCityTable(project.cityid, currFxtCompanyId).FirstOrDefault();
                if (list != null)
                {
                    if (!modifiedProperty.Any()) return 0;

                    var updateziduan = modifiedProperty.Aggregate(" set ", (current, item) => current + item);

                    string ptable = "FXTProject." + list.projecttable;
                    string comId = list.ShowCompanyId;

                    //获取之前的楼盘信息
                    var projectBe = ProjectDeleteOrExissts(ptable, comId, new DAT_Project { projectname = project.projectname, areaid = project.areaid, cityid = project.cityid, fxtcompanyid = currFxtCompanyId }, 1).FirstOrDefault();
                    if (projectBe == null)
                    {
                        projectBe = ProjectDeleteOrExissts(ptable, comId, new DAT_Project { projectname = project.projectname, areaid = project.areaid, cityid = project.cityid, fxtcompanyid = currFxtCompanyId }, 0).FirstOrDefault();
                    }
                    DateTime? enddateBe = projectBe == null ? null : projectBe.enddate;
                    decimal? xBe = projectBe == null ? null : projectBe.x;
                    decimal? yBe = projectBe == null ? null : projectBe.y;
                    string addressBe = projectBe == null ? "" : projectBe.address;
                    string othernameBe = projectBe == null ? "" : projectBe.othername;
                    int buildingtypecodeBe = projectBe == null || projectBe.buildingtypecode == null ? 0 : ((int)projectBe.buildingtypecode <= 0 ? 0 : (int)projectBe.buildingtypecode);

                    int r;
                    string sql = string.Empty;
                    string table = "update " + ptable + " with(rowlock) ";
                    string tableSub = "update " + ptable + "_sub with(rowlock) ";

                    SqlParameter[] paras = {
                                             new SqlParameter("@ProjectId",project.projectid),
                                             new SqlParameter("@CityId",project.cityid),
                                             new SqlParameter("@FxtCompanyId",project.fxtcompanyid),
                                         };
                    string mainSql = @"SELECT [ProjectId] FROM " + ptable + @" with(nolock) 
                                   WHERE [ProjectId]=@ProjectId and CityId=@CityId and [FxtCompanyId] =@FxtCompanyId";

                    string subSql = @"SELECT [ProjectId] FROM " + ptable + @"_sub with(nolock) 
                                   WHERE [ProjectId]=@ProjectId and CityId=@CityId and [Fxt_CompanyId] =@FxtCompanyId";

                    var searchTable = string.Empty;

                    var dtSub = DBHelperSql.ExecuteDataTable(subSql, paras);//先检查子表是否存在我自身评估机构的数据（老系统遗留的问题）
                    if (dtSub.Rows.Count > 0)  //大于0 ，说明子表存在
                    {
                        var dtMain = DBHelperSql.ExecuteDataTable(mainSql, paras);

                        if (dtMain.Rows.Count > 0) //若主表存在，说明这是老数据遗留的问题，这个时候需要删除子表的数据，并且更新主表数据
                        {
                            string deleSubSql = @"delete from " + ptable + "_sub WHERE [ProjectId]=@ProjectId and CityId=@CityId and [Fxt_CompanyId] =@FxtCompanyId";

                            DBHelperSql.ExecuteNonQuery(deleSubSql, paras);

                            sql = table + updateziduan + " where projectId=@ProjectId and CityId=@CityId and [FxtCompanyId] =@FxtCompanyId";
                            searchTable = "table";
                        }
                        else//不存在，说明子表的这条数据是其他评估机构的数据，那么只需要更新子表就可以了
                        {
                            sql = tableSub + updateziduan + " where projectId=@ProjectId and CityId=@CityId and [Fxt_CompanyId] = @FxtCompanyId";
                            searchTable = "tableSub";
                        }
                    }
                    else // 1.我自身评估机构的数据2.其他评估机构的数据
                    {
                        if (currFxtCompanyId == _fxtComId) //如果当前操作者是房讯通，只更新主表，不插入子表修改
                        {
                            sql = table + updateziduan + " where projectId=@ProjectId and CityId=@CityId ";
                            searchTable = "table";
                        }
                        else
                        {
                            var dtMain = DBHelperSql.ExecuteDataTable(mainSql, paras);

                            if (dtMain.Rows.Count > 0) //大于0，说明是我自身评估机构的数据
                            {
                                sql = table + updateziduan + " where projectId=@ProjectId and CityId=@CityId and [FxtCompanyId] =@FxtCompanyId";
                                searchTable = "table";
                            }
                            else //说明是其他评估机构的数据，这个时候需要拷贝到子表再进行修改
                            {
                                var insertSubSql = @"insert into " + ptable + @"_sub(
                                     ProjectId,[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address]
                                    ,[LandArea],[StartDate],[StartEndDate],[UsableYear],[BuildingArea],[SalableArea]
                                    ,[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate]
                                    ,[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[ParkingDesc]
                                    ,[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum]
                                    ,[Detail],[BuildingTypeCode],[UpdateDateTime],[OfficeArea],[OtherArea]
                                    ,[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime]
                                    ,[SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue]
                                    ,[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId]
                                    ,[Valid],[SalePrice],[Fxt_CompanyId],[PinYinAll],[X],[Y],[XYScale]
                                    ,[Creator],[IsEmpty],[TotalId],[East],[West],[South],[North]
                                    ,[BuildingQuality],[HousingScale],[BuildingDetail],[HouseDetail]
                                    ,[BasementPurpose],[ManagerQuality],[Facilities],[AppendageClass]
                                    ,[RegionalAnalysis],[Wrinkle],[Aversion],SourceName)
                            select 
                                    ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address
                                   ,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea
                                   ,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate
                                   ,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,ParkingDesc
                                   ,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum
                                   ,Detail,BuildingTypeCode,getdate(),OfficeArea,OtherArea
                                   ,PlanPurpose,PriceDate,IsComplete,OtherName,getdate()
                                   ,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue
                                   ,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId
                                   ,Valid,SalePrice,'" + currFxtCompanyId + @"' as Fxt_CompanyId,PinYinAll,X,Y,XYScale
                                   ,Creator,IsEmpty,TotalId,East,West,South,North
                                   ,BuildingQuality,HousingScale,BuildingDetail,HouseDetail
                                   ,BasementPurpose,ManagerQuality,Facilities,AppendageClass
                                   ,RegionalAnalysis,Wrinkle,Aversion,SourceName
                                     from " + ptable + @" with(nolock) where ProjectId =@ProjectId and CityId=@CityId";

                                DBHelperSql.ExecuteNonQuery(insertSubSql, paras);

                                sql = tableSub + updateziduan + " where projectId=@ProjectId and CityId=@CityId and [Fxt_CompanyId] = @FxtCompanyId";
                                searchTable = "tableSub";
                            }
                        }
                    }
                    project.savedatetime = DateTime.Now;
                    project.updatedatetime = DateTime.Now;
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        r = con.Execute(sql, project);

                    }
                    //if (r > 0)
                    //{
                    //    //更新search表
                    //    if (searchTable == "table")
                    //    {
                    //        string tablesql = @"select * from " + ptable + @" with(nolock) where valid = 1 and cityid = " + project.cityid + @" and projectid = " + project.projectid;
                    //        using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    //        {
                    //            var result = conn.Query<DAT_Project>(tablesql).FirstOrDefault();
                    //            if (result != null)
                    //            {
                    //                AddProjectSearch(ptable + "_Search", result.cityid, result.fxtcompanyid, result.projectid, result.projectname, result.address, result.othername, result.pinyin, result.pinyinall);
                    //            }
                    //        }
                    //    }
                    //    if (searchTable == "tableSub")
                    //    {
                    //        AddProjectSubSearch(ptable + "_sub_Search", project.cityid, currFxtCompanyId, project.projectid, project.projectname, project.address, project.othername, project.pinyin, project.pinyinall);
                    //    }
                    //}
                    if (r > 0)
                    {
                        //记录字段修改日志
                        var dataAfter = ProjectDeleteOrExissts(ptable, comId, new DAT_Project { projectname = project.projectname, areaid = project.areaid, cityid = project.cityid, fxtcompanyid = currFxtCompanyId }, 1).FirstOrDefault();
                        DateTime? enddateAf = dataAfter.enddate;
                        decimal? xAf = dataAfter.x;
                        decimal? yAf = dataAfter.y;
                        string addressAf = dataAfter.address;
                        string othernameAf = dataAfter.othername;
                        int buildingtypecodeAf = dataAfter.buildingtypecode == null ? 0 : ((int)dataAfter.buildingtypecode <= 0 ? 0 : (int)dataAfter.buildingtypecode);
                        if (enddateBe != enddateAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "enddate", enddateBe == null ? "" : ((DateTime)enddateBe).ToString("yyyy-MM-dd"), enddateAf == null ? "" : ((DateTime)enddateAf).ToString("yyyy-MM-dd"), project.saveuser);
                        }
                        if (xBe != xAf || yBe != yAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "xy", xBe.ToString() + "," + yBe.ToString(), xAf.ToString() + "," + yAf.ToString(), project.saveuser);
                        }
                        if (addressBe != addressAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "address", addressBe, addressAf, project.saveuser);
                        }
                        if (othernameBe != othernameAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "othername", othernameBe, othernameAf, project.saveuser);
                        }
                        if (buildingtypecodeBe != buildingtypecodeAf)
                        {
                            Log log = new Log();
                            log.InsertOperateLog(project.cityid, project.fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅楼盘信息, "projectid", project.projectid, "buildingtypecode", buildingtypecodeBe.ToString(), buildingtypecodeAf.ToString(), project.saveuser);
                        }

                        //开发商
                        if (!string.IsNullOrEmpty(project.DeveCompanyName))
                        {
                            int ri = addRelatedCompany(project.DeveCompanyName, "", "2001001", project.cityid, "", "", "", currFxtCompanyId);
                            if (ri > 0)
                            {
                                project.DeveCompanyId = ri;
                                int result = AddCityCompany(project.cityid, ri);
                            }
                        }
                        if (project.DeveCompanyId > 0)
                        {
                            sql = @"select CompanyId  from FXTProject.dbo.LNK_P_Company with(nolock) where CityId =@CityId and CompanyType = 2001001 and ProjectId=@ProjectId";
                            SqlParameter[] par =
                                              {
                                                  new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                            DataTable dt = new DataTable();
                            dt.Reset();
                            dt = DBHelperSql.ExecuteDataTable(sql, par);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string CompanyId = dt.Rows[0]["CompanyId"].ToString();
                                if (CompanyId != project.DeveCompanyId.ToString())
                                {
                                    sql = @"update FXTProject.dbo.LNK_P_Company with(rowlock) set CompanyId=@DeveCompanyId where CityId =@CityId and CompanyType = 2001001 
                                        and ProjectId=@ProjectId";
                                    SqlParameter[] par1 =
                                              {
                                                  new SqlParameter("@DeveCompanyId",project.DeveCompanyId),
                                                  new SqlParameter("@CityId",project.cityid),
                                                   new SqlParameter("@ProjectId",project.projectid)
                                              };
                                    DBHelperSql.ExecuteNonQuery(sql, par1);

                                }

                            }
                            else
                            {
                                sql = "insert into FXTProject.dbo.LNK_P_Company(ProjectId,CompanyType,CityId,CompanyId) "
                                       + "values (@ProjectId,2001001,@CityId,@DeveCompanyId)";
                                SqlParameter[] par1 =
                                              {
                                                  new SqlParameter("@ProjectId",project.projectid),
                                                  new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@DeveCompanyId",project.DeveCompanyId) 
                                              };
                                DBHelperSql.ExecuteNonQuery(sql, par1);
                            }
                        }
                        else
                        {
                            sql = "delete FXTProject.dbo.LNK_P_Company  where CityId =@CityId and CompanyType = 2001001 and ProjectId=@ProjectId";
                            SqlParameter[] par1 ={
                                                  new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                            DBHelperSql.ExecuteNonQuery(sql, par1);
                        }
                        //end开发商
                        //物业管理公司
                        if (project.ManagerCompanyName != null && project.ManagerCompanyName != "")
                        {
                            if (project.ManagerCompanyName == project.DeveCompanyName)
                            {
                                project.ManagerCompanyId = project.DeveCompanyId;
                            }
                            else
                            {
                                int ri = addRelatedCompany(project.ManagerCompanyName, "", "2001004", project.cityid, "", "", "", currFxtCompanyId);
                                if (ri > 0)
                                {
                                    project.ManagerCompanyId = ri;
                                    AddCityCompany(project.cityid, ri);
                                }
                            }
                        }
                        if (project.ManagerCompanyId > 0)
                        {
                            sql = "select CompanyId  from FXTProject.dbo.LNK_P_Company with(nolock) where CityId =@CityId and CompanyType = 2001004 and ProjectId=@ProjectId";
                            SqlParameter[] par1 ={
                                                  new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                            DataTable dt = new DataTable();
                            dt.Reset();
                            dt = DBHelperSql.ExecuteDataTable(sql, par1);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string CompanyId = dt.Rows[0]["CompanyId"].ToString();
                                if (CompanyId != project.ManagerCompanyId.ToString())
                                {
                                    sql = @"update FXTProject.dbo.LNK_P_Company with(rowlock) set CompanyId=@ManagerCompanyId where CityId =@CityId 
                                        and CompanyType = 2001004 and ProjectId=@ProjectId";
                                    SqlParameter[] par2 ={
                                                  new SqlParameter("@ManagerCompanyId",project.ManagerCompanyId),
                                                   new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ProjectId",project.projectid) 
                                              };
                                    DBHelperSql.ExecuteNonQuery(sql, par2);
                                }
                            }
                            else
                            {
                                sql = @"insert into FXTProject.dbo.LNK_P_Company(ProjectId,CompanyType,CityId,CompanyId)
                                    values (@ProjectId,2001004,@CityId,@ManagerCompanyId)";

                                SqlParameter[] par2 ={
                                                  new SqlParameter("@ProjectId",project.projectid),
                                                   new SqlParameter("@CityId",project.cityid),
                                                  new SqlParameter("@ManagerCompanyId",project.ManagerCompanyId) 
                                              };
                                DBHelperSql.ExecuteNonQuery(sql, par2);
                            }
                        }
                        else
                        {
                            sql = "delete FXTProject.dbo.LNK_P_Company where CityId =@CityId and CompanyType = 2001004 and ProjectId=@ProjectId";
                            SqlParameter[] par2 ={
                                                  new SqlParameter("@ProjectId",project.projectid),
                                                  new SqlParameter("@CityId",project.cityid)
                                              };
                            DBHelperSql.ExecuteNonQuery(sql, par2);
                        }
                        //end物业管理公司
                        //if (project.ModifyPrice != null && project.ModifyPrice == "1" && project.averageprice != null && Convert.ToDouble(project.averageprice) > 0)
                        //{
                        //    ModifyProjectPrice(project.cityid, project.fxtcompanyid, project.projectid, project.averageprice.ToString(), project.saveuser, currFxtCompanyId);
                        //}

                    }
                    if (r > 0)
                    {
                        var a = new ProjectCoordinate();
                        if (!string.IsNullOrEmpty(project.LngOrLat))
                        {
                            if (project.LngOrLat.Contains("|"))
                            {
                                a.DeleteProjectCoordinate(project.fxtcompanyid, project.cityid, project.projectid);
                                string[] lngOrlat = project.LngOrLat.Split('|');
                                for (int j = 0; j < lngOrlat.Length; j++)
                                {
                                    DAT_Project_Coordinate mo = new DAT_Project_Coordinate();
                                    mo.ProjectId = project.projectid;
                                    mo.CityID = project.cityid;
                                    mo.FxtCompanyId = project.fxtcompanyid;
                                    mo.X = Convert.ToDecimal(lngOrlat[j].Split(',')[0]);
                                    mo.Y = Convert.ToDecimal(lngOrlat[j].Split(',')[1]);
                                    mo.Valid = project.valid;
                                    a.AddProjectCoordinate(mo);
                                }
                            }
                            else
                            {
                                a.DeleteProjectCoordinate(project.fxtcompanyid, project.cityid, project.projectid);
                                DAT_Project_Coordinate mo = new DAT_Project_Coordinate();
                                mo.ProjectId = project.projectid;
                                mo.CityID = project.cityid;
                                mo.FxtCompanyId = project.fxtcompanyid;
                                if (project.LngOrLat.Contains(","))
                                {
                                    if (Convert.ToInt32(Convert.ToDecimal(project.LngOrLat.Split(',')[0])) > 0 || Convert.ToInt32(Convert.ToDecimal(project.LngOrLat.Split(',')[1])) > 0)
                                    {
                                        mo.Valid = 1;
                                    }
                                    else
                                    {
                                        mo.Valid = 0;
                                    }
                                    mo.X = Convert.ToDecimal(project.LngOrLat.Split(',')[0]);
                                    mo.Y = Convert.ToDecimal(project.LngOrLat.Split(',')[1]);
                                }
                                else
                                {
                                    mo.Valid = 0;
                                    mo.X = Convert.ToDecimal(project.LngOrLat);
                                    mo.X = Convert.ToDecimal(project.LngOrLat);
                                }
                                a.AddProjectCoordinate(mo);
                            }


                        }
                    }
                    return r;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 修改楼盘均价后批量修改
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="fxtcompanyid">公司Id</param>
        /// <param name="projectid">楼盘Id</param>
        /// <param name="ProjectPrice">楼盘均价</param>
        /// <param name="saveuser">修改人</param>
        //private int ModifyProjectPrice(int CityId, int FxtCompanyId, int ProjectId, string ProjectPrice, string saveuser, int currFxtCompanyId)
        //{

        //    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
        //    var list = GetCityTable(CityId).FirstOrDefault();
        //    int r = 0;
        //    try
        //    {
        //        if (list != null)
        //        {
        //            string ptable = list.projecttable,
        //            btable = list.buildingtable,
        //            htable = list.housetable,
        //            strsql = "";
        //            if (FxtCompanyId == _fxtComId)//房讯通
        //            {
        //                strsql = " update " + btable + " with(rowlock) set AveragePrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(AveragePrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString()
        //                      + " update " + btable + "_sub with(rowlock) set AveragePrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(AveragePrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + " and Fxt_CompanyId=" + FxtCompanyId.ToString()
        //                      + " update " + htable + " with(rowlock) set UnitPrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(UnitPrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where BuildingId in "
        //                      + " (select BuildingId from " + btable + " with(nolock) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + ") and CityId=" + CityId.ToString()
        //                      + " update " + htable + "_sub with(rowlock) set UnitPrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(UnitPrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where BuildingId in "
        //                      + " (select BuildingId from " + btable + " with(nolock) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + ") and CityId=" + CityId.ToString() + " and FxtCompanyId=" + FxtCompanyId.ToString();
        //            }
        //            else if (FxtCompanyId == currFxtCompanyId)
        //            {
        //                strsql = " update " + btable + " with(rowlock) set AveragePrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(AveragePrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString()
        //                           + " update " + btable + "_sub with(rowlock) set AveragePrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(AveragePrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + " and Fxt_CompanyId=" + FxtCompanyId.ToString()
        //                           + " update " + htable + " with(rowlock) set UnitPrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(UnitPrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where BuildingId in "
        //                           + " (select BuildingId from " + btable + " with(nolock) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + ") and CityId=" + CityId.ToString()
        //                           + " update " + htable + "_sub with(rowlock) set UnitPrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(UnitPrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where BuildingId in "
        //                           + " (select BuildingId from " + btable + " with(nolock) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + ") and CityId=" + CityId.ToString() + " and FxtCompanyId=" + FxtCompanyId.ToString();
        //            }
        //            else
        //            {
        //                #region 刘晓博 2014-03-05
        //                /*
        //                  * 添加院子、梯户字段
        //                  * 
        //                  * IsYard,YardWeight,ElevatorRateWeight
        //                  */
        //                #endregion
        //                strsql = "if EXISTS(select buildingId from " + btable + " with(nolock) where CityId=" + CityId.ToString() + " and FxtCompanyId <>25 and FxtCompanyId<>" + FxtCompanyId.ToString() + " and "
        //                    + "valid=1 and not exists(select buildingId from " + btable + "_sub  with(nolock) where CityId=" + CityId.ToString() + " and Fxt_CompanyId=" + FxtCompanyId.ToString() + " and projectId=" + ProjectId.ToString() + ") and "
        //                    + "projectId=" + ProjectId.ToString() + ") "
        //                    + "begin "
        //                    + "INSERT INTO " + btable + "_sub([BuildingId],[Fxt_CompanyId],[BuildingName],[ProjectId],[PurposeCode],[StructureCode]"
        //                    + ",[BuildingTypeCode],[TotalFloor],[FloorHigh],[SaleLicence],[ElevatorRate],[UnitsNumber],[TotalNumber]"
        //                    + ",[TotalBuildArea],[BuildDate],[SaleDate],[AveragePrice],[AverageFloor],[JoinDate],[LicenceDate]"
        //                    + ",[OtherName],[Weight],[IsEValue],[CityID],[CreateTime],[OldId],[Valid],[SalePrice],[SaveDateTime],"
        //                    + "[SaveUser],Wall,IsElevator,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark) "
        //                    + " SELECT [BuildingId],'" + currFxtCompanyId + @"' as [Fxt_CompanyId],[BuildingName],[ProjectId],[PurposeCode],[StructureCode]"
        //                    + ",[BuildingTypeCode],[TotalFloor],[FloorHigh],[SaleLicence],[ElevatorRate],[UnitsNumber],[TotalNumber]"
        //                    + ",[TotalBuildArea],[BuildDate],[SaleDate],[AveragePrice],[AverageFloor],[JoinDate],[LicenceDate]"
        //                    + ",[OtherName],[Weight],[IsEValue],[CityID],[CreateTime],[OldId],[Valid],[SalePrice],[SaveDateTime],"
        //                    + "[SaveUser],Wall,IsElevator,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark "
        //                    + " FROM " + btable + " with(nolock) where valid=1 and projectid=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + " and FxtCompanyId<>25 and FxtCompanyId<>" + FxtCompanyId + " and "
        //                    + " not exists(select BuildingId from " + btable + "_sub  with(nolock) where CityId=" + CityId.ToString() + " and projectid=" + ProjectId.ToString() + " and Fxt_CompanyId=" + FxtCompanyId.ToString() + ") "
        //                    + "end ";
        //                int result = DBHelperSql.ExecuteNonQuery(strsql);

        //                strsql = "if EXISTS(select buildingId from " + htable + " h with(nolock) where CityId=" + CityId + " and FxtCompanyId <>25 and FxtCompanyId<>" + FxtCompanyId + " and "
        //                    + "valid=1 and not exists(select houseId from " + htable + "_sub h1 with(nolock) where CityId=" + CityId + " and FxtCompanyId=" + FxtCompanyId + " and h.buildingId=h1.buildingId) "
        //                    + "and buildingId in(select buildingId from " + btable + " with(nolock) where CityId=" + CityId + " and projectId=" + ProjectId + ")) "
        //                    + "begin "
        //                    + "INSERT INTO " + htable + "_sub ([HouseId],[BuildingId],[HouseName],[HouseTypeCode],[FloorNo],[UnitNo],[BuildArea],[FrontCode]"
        //                    + ",[SightCode],[UnitPrice],[SalePrice],[Weight],[PhotoName],[Remark],[StructureCode],[TotalPrice]"
        //                    + ",[PurposeCode],[IsEValue],[CityID],[OldId],[CreateTime],[Valid],[SaveDateTime]"
        //                    + ",[SaveUser],[FxtCompanyId],[IsShowBuildingArea],[InnerBuildingArea],SubHouseType,SubHouseArea,Creator) "
        //                    + "SELECT [HouseId],[BuildingId],[HouseName],[HouseTypeCode],[FloorNo],[UnitNo],[BuildArea],[FrontCode]"
        //                    + ",[SightCode],[UnitPrice],[SalePrice],[Weight],[PhotoName],[Remark],[StructureCode],[TotalPrice]"
        //                    + ",[PurposeCode],[IsEValue],[CityID],[OldId],[CreateTime],[Valid],getdate() as [SaveDateTime]"
        //                    + ",[SaveUser],'" + currFxtCompanyId + @"' as [FxtCompanyId],[IsShowBuildingArea],[InnerBuildingArea],SubHouseType,SubHouseArea,Creator "
        //                    + "FROM " + htable + " h with(nolock) where valid=1 and CityId=" + CityId + " and FxtCompanyId <>25 and FxtCompanyId<>" + FxtCompanyId + " and "
        //                    + "not exists(select HouseId from " + htable + "_sub h1 with(nolock) where CityId=" + CityId + " and FxtCompanyId=" + FxtCompanyId + " and h.BuildingId=h1.BuildingId) "
        //                    + "and BuildingId in(select buildingId from " + btable + " with(nolock) where CityId=" + CityId + " and projectId=" + ProjectId + ") "
        //                    + "end ";
        //                int num = DBHelperSql.ExecuteNonQuery(strsql);
        //                strsql = " update " + btable + " with(rowlock) set AveragePrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(AveragePrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + " and (FxtCompanyId=" + FxtCompanyId.ToString() + " or FxtCompanyId=25) "
        //                  + " update " + btable + "_sub with(rowlock) set AveragePrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(AveragePrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + " and Fxt_CompanyId=" + FxtCompanyId.ToString()
        //                  + " update " + htable + " with(rowlock) set UnitPrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(UnitPrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where BuildingId in "
        //                  + " (select BuildingId from " + btable + " with(nolock) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + ") and CityId=" + CityId.ToString() + " and (FxtCompanyId=" + FxtCompanyId.ToString() + " or FxtCompanyId=25) "
        //                  + " update " + htable + "_sub with(rowlock) set UnitPrice=" + ProjectPrice.ToString() + "*(Isnull(Weight,Isnull(UnitPrice," + ProjectPrice.ToString() + ")/" + ProjectPrice.ToString() + ")) where BuildingId in "
        //                  + " (select BuildingId from " + btable + " with(nolock) where ProjectId=" + ProjectId.ToString() + " and CityId=" + CityId.ToString() + ") and CityId=" + CityId.ToString() + " and FxtCompanyId=" + FxtCompanyId.ToString();
        //            }
        //            r = DBHelperSql.ExecuteNonQuery(strsql);

        //        }
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}

        /// <summary>
        /// 得到楼盘的配套
        /// </summary>
        /// <param name="CityId">城市ID</param>
        /// <param name="ProjectId">楼盘ID</param>
        /// <param name="pid">楼盘配套ID</param>
        /// <returns></returns>
        public IQueryable<LNK_P_Appendage> GetProjectAppendageById(int ProjectId, int CityId, int pid)
        {
            string strsql = @"select ProjectId, A.Id, S.Code AppendageCode,S.CodeName AppendageName,A.P_AName,A.Area,A.IsInner,'old' IsAdd,
                                    ClassCode,C.CodeName ClassName
                                    from LNK_P_Appendage A with(nolock) left join FxtDataCenter.dbo.Sys_Code C with(nolock) on C.Code=A.ClassCode,
                                    FxtDataCenter.dbo.Sys_Code S with(nolock) 
                                    where ProjectId=@ProjectId and A.AppendageCode = S.Code and CityId=@CityId";
            if (pid > 0)
            {
                strsql += " and A.Id=@PId";
            }
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<LNK_P_Appendage>(strsql, new { ProjectId = ProjectId, CityId = CityId, PId = pid, }).AsQueryable();
            }

        }

        /// <summary>
        /// 新增项目配套
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public int AddProjectAppendage(LNK_P_Appendage obj)
        {
            try
            {
                string strsql = @"INSERT INTO [dbo].[LNK_P_Appendage](
                                         [AppendageCode],[ProjectId],[Area],[P_AName],[IsInner],[CityId],[ClassCode]) 
                                  VALUES 
                                         (@AppendageCode,@ProjectId,@Area,@P_AName,@IsInner,@CityId,@ClassCode)";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    int result = con.Execute(strsql, obj);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改项目配套
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public int ModifyProjectAppendage(LNK_P_Appendage obj)
        {
            try
            {
                string strsql = @"UPDATE [dbo].[LNK_P_Appendage] with(rowlock) 
                                  SET 
                                    [AppendageCode] =@AppendageCode,[Area] = @Area,[P_AName] =@P_AName,
                                    [IsInner] =@IsInner,[ClassCode]=@ClassCode 
                                  WHERE [Id]=@Id";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    int result = con.Execute(strsql, obj);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到某个楼盘的附属房屋单价
        /// 附属房价格
        /// </summary>
        /// <param name="CityId">城市ID</param>
        /// <param name="ProjectId">楼栋ID</param>
        /// <param name="FxtCompanyId">公司ID</param>
        /// <returns></returns>
        public IQueryable<DAT_SubHousePrice> GetSubHousePriceByProjectId(int CityId, int ProjectId, int FxtCompanyId, string codeName)
        {
            try
            {
                var list_citytable = GetCityTable(CityId).FirstOrDefault();
                if (list_citytable != null)
                {

                    string ptable = list_citytable.subhousepricetable,
                    strsql = @"select p.ID,C.CodeName,C.Code,p.SubHouseUnitPrice, p.ProjectId
                                      from FxtDataCenter.dbo.sys_code c with(nolock) left join " + ptable + @" p with(nolock) on p.ProjectId=@ProjectId and p.CityId=@CityId 
                                      and p.FxtCompanyId=@FxtCompanyId
                                      and p.SubHouseType=c.Code 
                                      where c.Id=1015";
                    if (!string.IsNullOrEmpty(codeName.Trim()))
                    {
                        strsql += " and C.CodeName=@CodeName  order by c.id";

                    }
                    else
                    {
                        strsql += "  order by c.id";
                    }
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        return con.Query<DAT_SubHousePrice>(strsql, new { ProjectId = ProjectId, CityId = CityId, FxtCompanyId = FxtCompanyId, CodeName = codeName.Trim() }).AsQueryable();
                    }

                }
                else
                    return new List<DAT_SubHousePrice>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改附属房屋价格
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="SubHouseUnitPrice"></param>
        /// <param name="SubHouseType"></param>
        /// <returns></returns>
        public int SaveSubHousePrice(int CityId, string ProjectId, string FxtCompanyId, string SId, string SubHouseUnitPrice, string SubHouseType, string UserId)
        {
            try
            {
                var list_city_table = GetCityTable(CityId).FirstOrDefault();
                if (list_city_table != null)
                {
                    string ptable = list_city_table.subhousepricetable,
                    strsql = "";
                    List<SqlParameter> list = new List<SqlParameter>();
                    if (!string.IsNullOrEmpty(SId) && SId != "0")
                    {
                        strsql = @"update " + ptable + @" with(rowlock) set SubHouseUnitPrice=@SubHouseUnitPrice,SaveDate=GetDate(),SaveUserId=@UserId where ID=@SId";
                        list.Add(new SqlParameter("@SubHouseUnitPrice", SubHouseUnitPrice));
                        list.Add(new SqlParameter("@UserId", UserId));
                        list.Add(new SqlParameter("@SId", SId));
                    }
                    else
                    {
                        strsql = @"INSERT INTO " + ptable + @"
                                    ([CityId],[FxtCompanyId],[ProjectId],[SubHouseType],[SubHouseUnitPrice],[CreateDate],[Creator],[SaveDate],[SaveUserId])
                                  VALUES
                                     (@CityId,@FxtCompanyId,@ProjectId,@SubHouseType,@SubHouseUnitPrice,getdate(),@UserId,getdate(),@UserId)";
                        list.Add(new SqlParameter("@CityId", CityId));
                        list.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                        list.Add(new SqlParameter("@ProjectId", ProjectId));
                        list.Add(new SqlParameter("@SubHouseType", SubHouseType));
                        list.Add(new SqlParameter("@SubHouseUnitPrice", SubHouseUnitPrice));
                        list.Add(new SqlParameter("@UserId", UserId));
                    }
                    SqlParameter[] par = list.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    return DBHelperSql.ExecuteNonQuery(strsql, par);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到楼盘的图片列表(包含楼栋图片)
        /// </summary>
        /// <param name="cityId">cityId</param>
        /// <param name="fxtId">公司ID</param>
        /// <param name="projectId">楼盘ID</param>
        /// <returns></returns>
        public IQueryable<LNK_P_Photo> GetProjectPhotoList(int cityId, int fxtId, int projectId, int photoId = 0)
        {
            try
            {
                var dt = GetCityTable(cityId, fxtId).FirstOrDefault();
                if (dt != null)
                {
                    string btable = "FXTProject." + dt.buildingtable;
                    string ComId = dt.ShowCompanyId;
                    string where = string.Empty;
                    if (photoId > 0)
                    {
                        where += " and PH.Id = @photoId";
                    }

                    string strsql = @"
select 
	PH.*
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = PH.PhotoTypeCode) as PhotoTypeCodeName
	,B.BuildingName
from (
	select * from FXTProject.dbo.LNK_P_Photo p with(nolock)
	where not exists(
		select Id from FXTProject.dbo.LNK_P_Photo_sub ps with(nolock)
		where ps.Id = p.Id 
		and ps.CityId = @cityid
		and ps.FxtCompanyId = @fxtCompanyId
		and ps.ProjectId = @projectid
	)
	and Valid = 1
	and CityId = @cityid
	and FxtCompanyId in (" + ComId + @")
	and ProjectId = @projectid
	union
	select * from FXTProject.dbo.LNK_P_Photo_sub p with(nolock)
	where Valid = 1
	and CityId = @cityid
	and FxtCompanyId = @fxtCompanyId
	and ProjectId = @projectid
)PH
left join (
	SELECT b.ProjectId
		,b.BuildingId
		,b.BuildingName
	FROM " + btable + @" b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityID = @cityId
		AND FxtCompanyId in (" + ComId + @")
		AND NOT EXISTS (
			SELECT BuildingId
			FROM " + btable + @"_sub bs WITH (NOLOCK)
			WHERE b.BuildingId = bs.BuildingId
				AND bs.CityID = @cityId
				AND bs.Fxt_CompanyId = @fxtCompanyId
			)
		and ProjectId = @projectid
	UNION	
	SELECT b.ProjectId
		,b.BuildingId
		,b.BuildingName
	FROM " + btable + @"_sub b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityID = @cityId
		AND b.Fxt_CompanyId = @fxtCompanyId
		and ProjectId = @projectid
)B on PH.BuildingId = B.BuildingId
where 1 = 1 
and (PH.BuildingId = 0 or (PH.BuildingId > 0 and B.BuildingName is not null))" + where + " order by BuildingName,Id desc";
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        return con.Query<LNK_P_Photo>(strsql, new { projectid = projectId, cityId = cityId, fxtCompanyId = fxtId, photoId = photoId }).AsQueryable();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到楼栋的图片列表
        /// </summary>
        /// <param name="cityId">cityId</param>
        /// <param name="fxtId">公司ID</param>
        /// <param name="projectId">楼盘ID</param>
        /// <returns></returns>
        public IQueryable<LNK_P_Photo> GetBuildingPhotoList(int cityId, int fxtId, int projectId, int BuildingId, int photoId = 0)
        {
            try
            {
                string ziduan = " select p.Id,[Path],PhotoName,pt.CodeName as PhotoTypeCodeName,p.PhotoTypeCode,p.ProjectId,p.BuildingId,p.FxtCompanyId ";
                string table1 = " from [dbo].LNK_P_Photo p with(nolock) ";
                string table2 = " from [dbo].LNK_P_Photo_sub p with(nolock) ";
                string left_tb = " left join FxtDataCenter.dbo.sys_code pt with(nolock) on pt.Code=p.PhotoTypeCode ";
                string where1 = @" where valid=1 and ProjectId=@ProjectId and CityId=@CityId and not exists(select ps.Id from LNK_P_Photo_sub ps with(nolock) where 
                                        p.Id=ps.Id and ps.FxtCompanyId=@FxtCompanyId and ps.CityId=p.CityId)";
                string where2 = " where valid=1 and ProjectId=@ProjectId and CityId=@CityId and p.FxtCompanyId=@FxtCompanyId";
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@ProjectId", projectId));
                list.Add(new SqlParameter("@CityId", cityId));
                list.Add(new SqlParameter("@FxtCompanyId", fxtId));

                if (photoId > 0)
                {
                    where1 += " and p.Id=@photoId";
                    where2 += " and p.Id=@photoId";
                }
                if (BuildingId > 0)
                {
                    where1 += " and p.BuildingId=@BuildingId";
                    where2 += " and p.BuildingId=@BuildingId";

                }
                string strsql = ziduan + table1 + left_tb + where1;
                strsql += " union ";
                strsql += ziduan + table2 + left_tb + where2;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    return con.Query<LNK_P_Photo>(strsql, new
                    {
                        ProjectId = projectId,
                        CityId = cityId,
                        FxtCompanyId = fxtId,
                        photoId = photoId,
                        BuildingId = BuildingId
                    }).AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除楼盘
        /// 2014-04-04 
        /// 刘晓博
        /// </summary>
        public int DeleteProject(int projectId, int cityId, int fxtCompanyId, string userId, int productTypeCode, int currFxtCompanyId, int isDeleteTrue)
        {
            try
            {
                //string sql = "SELECT CompanyId,IsDeleteTrue FROM CompanyProduct WITH(NOLOCK) WHERE CompanyId=@CompanyId and CityId=@CityId and ProductTypeCode=@ProductTypeCode";
                //SqlParameter[] par ={ 
                //                        new SqlParameter("@CompanyId", fxtCompanyId),
                //                         new SqlParameter("@CityId", cityId),
                //                         new SqlParameter("@ProductTypeCode", productTypeCode),
                //                    };

                //DBHelperSql.ConnectionString = ConfigurationHelper.FxtUserCenter;
                //DataTable dt = DBHelperSql.ExecuteDataTable(sql, par);
                if (isDeleteTrue == 1)//dt != null && dt.Rows.Count > 0 && dt.Rows[0]["IsDeleteTrue"].ToString() == "1"
                {
                    return DeleteProject2(cityId, projectId, fxtCompanyId);
                }
                string strsql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable] FROM [FXTProject].[dbo].[SYS_City_Table] with(nolock) where CityId=" + cityId;
                DataTable ta = DBHelperSql.ExecuteDataTable(strsql);
                if (ta != null && ta.Rows.Count > 0)
                {
                    string ptable = ta.Rows[0]["ProjectTable"].ToString();

                    if (currFxtCompanyId == _fxtComId)//当前操作者为房讯通
                    {
                        strsql = @"
if EXISTS(SELECT [ProjectId] FROM  [FXTProject]." + ptable + "_sub with(nolock) WHERE [ProjectId]=" + projectId + " and CityId=" + cityId + " and [Fxt_CompanyId] = " + _fxtComId + @")
begin
delete top(1) [FXTProject]." + ptable + "_sub with(rowlock)  where [ProjectId]=" + projectId + " and CityId=" + cityId + " and [Fxt_CompanyId] = " + _fxtComId + @"
end
";
                        strsql += " update [FXTProject]." + ptable + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]='" + userId + "' where [ProjectId]=" + projectId + " and CityId=" + cityId;

                    }
                    else if (fxtCompanyId == currFxtCompanyId)//自身评估机构的数据
                    {
                        strsql = "update [FXTProject]." + ptable + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]='" + userId + "' where [ProjectId]=" + projectId + " and CityId=" + cityId + " and [FxtCompanyId] = " + fxtCompanyId;

                        strsql += "  update [FXTProject]." + ptable + "_sub with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]='" + userId + "' where [ProjectId]=" + projectId + " and CityId=" + cityId + " and [Fxt_CompanyId] = " + fxtCompanyId;

                        #region 旧的代码
                        //strsql += " if EXISTS(SELECT [ProjectId] FROM  [FXTProject]." + ptable + "_sub with(nolock) WHERE [ProjectId]=" + projectId + " and CityId=" + cityId + " and [Fxt_CompanyId] = " + fxtCompanyId + ") "
                        //        + " begin "
                        //        + "delete top(1) [FXTProject]." + ptable + "_sub with(rowlock)  where [ProjectId]=" + projectId + " and CityId=" + cityId + " and [Fxt_CompanyId] = " + fxtCompanyId
                        //        + " end ";
                        #endregion
                    }
                    else
                    {
                        strsql = @"INSERT INTO [FXTProject]." + ptable + @"_sub ([ProjectId],[Fxt_CompanyId],[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address],[LandArea],[StartDate],[StartEndDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[ParkingDesc],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode],[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId],[Valid],[SalePrice],Creator)
                                 SELECT [ProjectId],'" + currFxtCompanyId + @"' as [FxtCompanyId],[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address],[LandArea],[StartDate],[StartEndDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[ParkingDesc],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode],[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],GetDate() as [SaveDateTime],'" + userId + "' as [SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId],0 as [Valid],[SalePrice],Creator  FROM [FXTProject]." + ptable + " with(nolock) where ProjectId=" + projectId + " and CityId=" + cityId;

                        #region 旧的代码

                        //strsql = "if EXISTS(SELECT [ProjectId] FROM  [FXTProject]." + ptable + "_sub with(nolock) WHERE [ProjectId]=" + projectId + " and CityId=" + cityId + " and [Fxt_CompanyId] = " + fxtCompanyId + ") "
                        //+ " begin "
                        //+ "update [FXTProject]." + ptable + "_sub with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]='" + userId + "' where [ProjectId]=" + projectId + " and CityId=" + cityId + " and [Fxt_CompanyId] = " + fxtCompanyId
                        //+ " end "
                        //+ "else "
                        //+ " begin "
                        //+ "INSERT INTO [FXTProject]." + ptable + "_sub ([ProjectId],[Fxt_CompanyId],[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address]"
                        //+ ",[LandArea],[StartDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate]"
                        //+ ",[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[AveragePrice]"
                        //+ ",[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode],[UpdateDateTime]"
                        //+ ",[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser]"
                        //+ ",[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime]"
                        //+ ",[AreaLineId],[Valid],[SalePrice],Creator)"
                        //+ "SELECT [ProjectId],'" + currFxtCompanyId + @"' as [FxtCompanyId],[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address]"
                        //+ ",[LandArea],[StartDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate]"
                        //+ ",[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[AveragePrice]"
                        //+ ",[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode],[UpdateDateTime]"
                        //+ ",[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],GetDate() as [SaveDateTime],'" + userId + "' as [SaveUser]"
                        //+ ",[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime]"
                        //+ ",[AreaLineId],0 as [Valid],[SalePrice],Creator "
                        //+ " FROM [FXTProject]." + ptable + " with(nolock) where ProjectId=" + projectId + " and CityId=" + cityId + " "
                        //+ " end";

                        #endregion
                    }
                    int r = DBHelperSql.ExecuteNonQuery(strsql);
                    return r;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 直接删除楼盘(谨慎使用)
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <returns></returns>
        private int DeleteProject2(int CityId, int ProjectId, int FxtCompanyId)
        {
            try
            {
                string strsql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable] FROM [FXTProject].[dbo].[SYS_City_Table] with(nolock) where CityId=" + CityId;
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                DataTable dt = DBHelperSql.ExecuteDataTable(strsql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string ptable = dt.Rows[0]["ProjectTable"].ToString();

                    strsql = "delete [FXTProject]." + ptable + " with(rowlock)  where FxtCompanyId in(25," + FxtCompanyId + ") and [ProjectId]=" + ProjectId + " and CityId=" + CityId
                        + " delete [FXTProject]." + ptable + "_sub with(rowlock)  where Fxt_CompanyId=" + FxtCompanyId + " and [ProjectId]=" + ProjectId + " and CityId=" + CityId;

                    int r = DBHelperSql.ExecuteNonQuery(strsql);
                    return r;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int DeleteProjectPeiTao(int id)
        {
            const string sql = "delete from lnk_p_appendage where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(sql, new { id });
            }
        }

        /// <summary>
        ///  删除楼盘项目图片
        /// </summary>
        /// <param name="photoId">图片ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtId">公司ID</param>
        /// <returns></returns>
        public int DeleteProjectPhoto(int photoId, int cityId, int fxtCompanyId, int currFxtCompanyId)
        {
            try
            {
                string strsql = "";
                if (fxtCompanyId == _fxtComId)
                {
                    strsql = "update [FXTProject].[dbo].[LNK_P_Photo] set Valid=0 where Id=" + photoId;
                }
                else if (fxtCompanyId == currFxtCompanyId)
                {
                    strsql = "update [FXTProject].[dbo].[LNK_P_Photo] set Valid=0 where Id=" + photoId;
                }
                else
                {
                    strsql = "if EXISTS(SELECT ProjectId FROM [FXTProject].[dbo].[LNK_P_Photo_sub] with(nolock) WHERE Id=" + photoId + " and CityId=" + cityId
                            + " and [FxtCompanyId] = " + fxtCompanyId + ") "
                            + "begin "
                            + "update [FXTProject].[dbo].[LNK_P_Photo_sub] with(rowlock) set [Valid]=0 where [Id]=" + photoId + " and " + "CityId=" + cityId
                            + " and [FxtCompanyId] = " + fxtCompanyId
                            + " end "
                            + "else "
                            + "begin "
                            + "INSERT INTO [FXTProject].[dbo].[LNK_P_Photo_sub] (Id, ProjectId, PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid, FxtCompanyId, BuildingId, X, Y)"
                            + "select Id,[ProjectId],[PhotoTypeCode],[Path],GetDate() as PhotoDate,[PhotoName]"
                            + ",[CityId],0 as Valid,'" + currFxtCompanyId + "' as [FxtCompanyId],BuildingId, X, Y from [dbo].[LNK_P_Photo] where Id=" + photoId
                            + " end";
                }
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                int r = DBHelperSql.ExecuteNonQuery(strsql);
                return r;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 增加楼盘项目图片
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtId">公司ID</param>
        /// <param name="projectId">楼盘ID</param>
        /// <param name="photoTypeCode">图片code</param>
        /// <param name="path">图片地址</param>
        /// <param name="photoName">图片名称</param>
        /// <returns></returns>
        public int AddProjectPhoto(int cityId, int fxtId, int projectId, int photoTypeCode, string path, string photoName, long builId)
        {
            try
            {
                string strsql = @"INSERT INTO [dbo].[LNK_P_Photo]([ProjectId],[PhotoTypeCode],[Path],[PhotoName],[CityId],[FxtCompanyId],[BuildingId])
                                    VALUES(@ProjectId,@PhotoTypeCode,@Path,@PhotoName,@CityId,@FxtCompanyId,@BuildingId);Select SCOPE_IDENTITY()";
                SqlParameter[] par = {
                                         new SqlParameter("@ProjectId",projectId),
                                         new SqlParameter("@PhotoTypeCode",photoTypeCode),
                                         new SqlParameter("@Path",path),
                                         new SqlParameter("@PhotoName",string.IsNullOrEmpty(photoName)?"":photoName),
                                         new SqlParameter("@CityId",cityId),
                                         new SqlParameter("@FxtCompanyId",fxtId),
                                         new SqlParameter("@BuildingId",builId)
                                     };
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                object obj = DBHelperSql.ExecuteScalar(strsql, par);
                if (obj != null && Convert.ToInt32(obj) > 0) return Convert.ToInt32(obj);
                else return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改楼盘项目图片
        /// </summary>
        /// <param name="projectId">图片ID</param>
        /// <param name="photoTypeCode">图片code</param>
        /// <param name="path">图片地址</param>
        /// <param name="photoName">图片名称</param>
        /// <returns></returns>
        public int UpdataProjectPhoto(int Id, int photoTypeCode, string path, string photoName, int cityId, int fxtCompanyId, int currFxtCompanyId)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    string sql;
                    if (fxtCompanyId == _fxtComId)
                    {
                        sql = @"UPDATE [FXTProject].[dbo].[LNK_P_Photo] with(rowlock)
                           SET [PhotoTypeCode] = @PhotoTypeCode
                              ,[Path] = @Path
                              ,[PhotoDate] = GETDATE()
                              ,[PhotoName] = @PhotoName
                           WHERE Id=@Id and CityId=@CityId";

                    }
                    else if (fxtCompanyId == currFxtCompanyId)
                    {
                        sql = @"UPDATE [FXTProject].[dbo].[LNK_P_Photo] with(rowlock)
                           SET [PhotoTypeCode] = @PhotoTypeCode
                              ,[Path] = @Path
                              ,[PhotoDate] = GETDATE()
                              ,[PhotoName] = @PhotoName
                           WHERE Id=@Id and CityId=@CityId";
                    }
                    else
                    {
                        sql = @"SELECT ProjectId FROM [FXTProject].[dbo].[LNK_P_Photo_sub] with(nolock) WHERE Id=@Id and CityId=@CityId
                             and [FxtCompanyId] =@FxtCompanyId";
                        var psub = con.Query<LNK_P_Photo>(sql, new { Id = Id, CityId = cityId, FxtCompanyId = fxtCompanyId });
                        if (psub != null && psub.Count() > 0)
                        {
                            sql = @"UPDATE [FXTProject].[dbo].[LNK_P_Photo_sub] with(rowlock)
                           SET [PhotoTypeCode] = @PhotoTypeCode
                              ,[Path] = @Path
                              ,[PhotoDate] = GETDATE()
                              ,[PhotoName] = @PhotoName
                           WHERE Id=@Id and CityId=@CityId";
                        }
                        else
                        {
                            sql = @"SELECT ProjectId FROM [FXTProject].[dbo].[LNK_P_Photo] with(nolock) WHERE Id=@Id and CityId=@CityId
                             and [FxtCompanyId] =@FxtCompanyId";
                            var p = con.Query<LNK_P_Photo>(sql, new { Id = Id, CityId = cityId, FxtCompanyId = fxtCompanyId });
                            if (p != null && p.Count() > 0)
                            {
                                sql = @"UPDATE [FXTProject].[dbo].[LNK_P_Photo] with(rowlock)
                           SET [PhotoTypeCode] = @PhotoTypeCode
                              ,[Path] = @Path
                              ,[PhotoDate] = GETDATE()
                              ,[PhotoName] = @PhotoName
                           WHERE Id=@Id and CityId=@CityId";
                            }
                            else
                            {
                                sql = @"INSERT INTO [FXTProject].[dbo].[LNK_P_Photo_sub](Id,[ProjectId],[PhotoTypeCode],[Path],[PhotoName],[CityId],[FxtCompanyId]) 
                                        select Id,ProjectId,'" + photoTypeCode + @"' as PhotoTypeCode,'" + path + @"' as Path,
                                        '" + photoName + @"' as PhotoName,CityId,'" + currFxtCompanyId + @"' as FxtCompanyId from 
                                        [FXTProject].[dbo].[LNK_P_Photo] where Id=" + Id + @" and CityId='" + cityId + @"' and FxtCompanyId='" + fxtCompanyId + @"'";
                            }
                        }
                    }
                    int num = con.Execute(sql, new { PhotoTypeCode = photoTypeCode, Path = path, PhotoName = photoName, Id = Id, CityId = cityId, FxtCompanyId = fxtCompanyId });
                    return num;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取所有的楼盘名称
        /// </summary>
        /// <returns></returns>
        public IQueryable<DAT_Project> GetAllProjectName(int cityId, int fxtId, string projectName)
        {
            // string bookTitles = String.Empty;
            var listCityTable = GetCityTable(cityId, fxtId).FirstOrDefault();
            if (listCityTable != null)
            {
                var ptable = listCityTable.projecttable;
                var leftTable = " left join FxtDataCenter.dbo.SYS_Area  are with(nolock) on p.AreaID=are.AreaId ";
                if (!string.IsNullOrEmpty(projectName))
                {
                    leftTable += " where p.ProjectName='" + projectName + "' and p.valid=1 ";
                }
                var sql = @"select ProjectId,p.ProjectName,p.OtherName,are.AreaName 
                               from " + ptable + @" p with(nolock) 
                                " + leftTable + @"
                                union 
                               select ProjectId,p.ProjectName,p.OtherName,are.AreaName 
                               from " + ptable + @"_sub p with(nolock)
                               " + leftTable + @" ";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    return con.Query<DAT_Project>(sql).AsQueryable();

                }
                //if (result != null)
                //{
                //    foreach (var item in result)
                //    {
                //        //bookTitles += item.projectname + "#" + item.othername + "|" + item.AreaName + "&";
                //        bookTitles += item.projectname + "|" + item.AreaName + split;
                //    }
                //    if (!string.IsNullOrEmpty(bookTitles))
                //    {
                //        bookTitles = bookTitles.Substring(0, bookTitles.Length - 1);
                //    }
                //}
            }
            return new List<DAT_Project>().AsQueryable();
        }

        /// <summary>
        /// 获取所有的楼盘名称
        /// </summary>
        /// <returns></returns>
        public IQueryable<DAT_Project> GetAllProjectName(int cityId, int fxtId)
        {
            string strsql = "";
            var list_city_table = GetCityTable(cityId, fxtId).FirstOrDefault();
            if (list_city_table != null)
            {
                string ptable = list_city_table.projecttable,
                ComId = list_city_table.ShowCompanyId;
                using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    strsql = @" select ProjectName,ProjectId,Area.AreaName AreaName from " + ptable + @" p with(nolock) left join dbo.SYS_Area Area with(nolock) on p.AreaID=Area.AreaId 
                    	                                where p.Valid=1 and p.CityID=@CityID   
                                                        and  p.FxtCompanyId in(" + ComId + @") and not exists (select ProjectId from " + ptable + @"_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId and ps.CityId=p.CityId) 
                    	                                union 
                    	            select ProjectName,ProjectId,Area.AreaName AreaName from " + ptable + @"_sub p with(nolock)  left join dbo.SYS_Area Area with(nolock) on p.AreaID=Area.AreaId 
                    	            where p.Valid=1 and p.CityID=@CityID and p.Fxt_CompanyId=@FxtCompanyId";
                    var result = conn.Query<DAT_Project>(strsql, new { CityID = cityId, FxtCompanyId = fxtId }).AsQueryable();
                    return result;
                }
            }
            return new List<DAT_Project>().AsQueryable();
        }

        /// <summary>
        /// 根据楼盘名称获取楼盘Info
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IQueryable<DAT_Project> GetProjectIdByName(int CityID, int AreaID, int FxtCompanyId, string ProjectName)
        {
            string strsql = "";
            var list_city_table = GetCityTable(CityID, FxtCompanyId).FirstOrDefault();
            if (list_city_table != null)
            {
                string ptable = list_city_table.projecttable,
                ComId = list_city_table.ShowCompanyId;

                var strWhere = string.Empty;
                if (AreaID > 0)
                {
                    strWhere = " and p.AreaID=@AreaID ";
                }

                using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    strsql = @" select [ProjectId],[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address],[LandArea]
                                          ,[StartDate],[StartEndDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate]
                                          ,[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode]
                                          ,[ParkingNumber],[ParkingDesc],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum]
                                          ,[Detail],[BuildingTypeCode],[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose]
                                          ,[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea]
                                          ,[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId]
                                          ,[Valid],[SalePrice],[FxtCompanyId],[PinYinAll],[X],[Y],[XYScale],[Creator],[IsEmpty]
                                          ,[TotalId],[East],[West],[South],[North],[BuildingQuality],[HousingScale],[BuildingDetail]
                                          ,[HouseDetail],[BasementPurpose],[ManagerQuality],[Facilities],[AppendageClass],
                                          [RegionalAnalysis],[Wrinkle],[Aversion],SourceName from " + ptable + @" p with(nolock) 
                    	                                where p.Valid=1 and p.CityID=@CityID " + strWhere + @" and  p.ProjectName=@ProjectName  
                                                        and  p.FxtCompanyId in(" + ComId + @") and not exists (select ProjectId from " + ptable + @"_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId and ps.CityId=p.CityId) 
                    	                                union 
                    	            select [ProjectId],[ProjectName],[SubAreaId],[FieldNo],[PurposeCode],[Address],[LandArea]
                                          ,[StartDate],[StartEndDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate]
                                          ,[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode]
                                          ,[ParkingNumber],[ParkingDesc],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum]
                                          ,[Detail],[BuildingTypeCode],[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose]
                                          ,[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea]
                                          ,[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId]
                                          ,[Valid],[SalePrice],[Fxt_CompanyId],[PinYinAll],[X],[Y],[XYScale],[Creator],[IsEmpty]
                                          ,[TotalId],[East],[West],[South],[North],[BuildingQuality],[HousingScale],[BuildingDetail]
                                          ,[HouseDetail],[BasementPurpose],[ManagerQuality],[Facilities],[AppendageClass],
                                          [RegionalAnalysis],[Wrinkle],[Aversion],SourceName from " + ptable + @"_sub p with(nolock) 
                    	            where p.Valid=1 and p.CityID=@CityID and  p.ProjectName=@ProjectName  and p.Fxt_CompanyId=@FxtCompanyId " + strWhere;
                    //if(AreaID != -1) strsql += " and p.AreaID=@AreaID";
                    var result = conn.Query<DAT_Project>(strsql, new { CityID = CityID, ProjectName = ProjectName, AreaID = AreaID, FxtCompanyId = FxtCompanyId }).AsQueryable();
                    return result;
                }
            }
            return new List<DAT_Project>().AsQueryable();

        }

        /// <summary>
        /// 根据楼盘名称获取楼盘Info
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IQueryable<DAT_Project> GetProjectNameList(int CityID, string ProjectName, int fxtCompanyId, int areaId)
        {

            //string strsql = "";
            var list_city_table = GetCityTable(CityID, fxtCompanyId).FirstOrDefault();
            if (list_city_table != null)
            {
                string ptable = list_city_table.projecttable,
                ComId = list_city_table.ShowCompanyId;
                using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var sb = new StringBuilder();
                    sb.Append("select P.ProjectId,A.AreaName,p.ProjectName,P.OtherName,P.PinYin,P.AreaId,P.SubAreaId,P.IsComplete,P.IsEValue from " + ptable + " P with(nolock) ");
                    sb.Append(",FxtDataCenter.dbo.Sys_Area A with(nolock) where P.CityId=@CityId and A.AreaId = P.AreaId and p.valid=1 and A.CityId=@CityId and p.FxtCompanyId in(" + ComId + ") and ");
                    sb.Append(" (p.ProjectName=@ProjectName or P.OtherName=@ProjectName or PinYin=@ProjectName) and ");
                    sb.Append(" not exists (select ProjectId from " + ptable + "_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@fxtCompanyId and ps.CityId=p.CityId) ");
                    if (areaId > 0)
                    {
                        sb.Append(" and p.AreaId=@AreaId ");
                    }
                    sb.Append(" union ");
                    sb.Append(" select P.ProjectId,A.AreaName,p.ProjectName,P.OtherName,P.PinYin,P.AreaId,P.SubAreaId,P.IsComplete,P.IsEValue from " + ptable + "_sub P with(nolock) ");
                    sb.Append(",FxtDataCenter.dbo.Sys_Area A with(nolock) where P.CityId=@CityId and A.AreaId = P.AreaId and p.valid=1 and A.CityId=@CityId ");
                    sb.Append(" and p.Fxt_CompanyId=@fxtCompanyId and (p.ProjectName=@ProjectName or P.OtherName=@ProjectName or PinYin=@ProjectName) ");
                    if (areaId > 0)
                    {
                        sb.Append(" and p.AreaId=@AreaId ");
                    }
                    sb.Append(" order by P.AreaId,p.ProjectName ");
                    var result = conn.Query<DAT_Project>(sb.ToString(), new { CityId = CityID, ProjectName = ProjectName, fxtCompanyId = fxtCompanyId, AreaId = areaId }).AsQueryable();
                    return result;
                }
            }
            return new List<DAT_Project>().AsQueryable();

        }

        /// <summary>
        /// 根据楼盘名称获取楼盘ID
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public int GetProjectIdByName(int CityID, string ProjectName, int FxtCompanyId)
        {
            try
            {
                string strsql = "";
                var list_city_table = GetCityTable(CityID, FxtCompanyId).FirstOrDefault();
                if (list_city_table != null)
                {
                    string ptable = list_city_table.projecttable;
                    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        strsql = @" select ProjectName,ProjectId from " + ptable + @" p with(nolock) 
                    	                                where Valid=1 and CityID=@CityID and  ProjectName=@ProjectName 
                    	                                union 
                    	                                select ProjectName,ProjectId from " + ptable + @"_sub p with(nolock) 
                    	                                where Valid=1 and CityID=@CityID and  ProjectName=@ProjectName";
                        var result = conn.Query<DAT_Project>(strsql, new { CityID = CityID, ProjectName = ProjectName }).AsQueryable().FirstOrDefault();
                        if (result != null)
                            return result.projectid;
                        else
                            return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 楼盘合并
        /// 0:没有开通权限;-1:原始楼盘暂无可以复制的楼栋;-2:添加楼栋失败;-3:目标楼盘存在数据,不能复制楼盘;-4：程序异常;
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int MergerProject(DAT_Project project, int projectIdTo, int currfxtcompanyid)
        {
            IDAT_Building ib = new DAT_BuildingDAL();
            //原始楼盘下的楼栋列表
            var b_list = ib.GetBuildingInfo(project.cityid, project.projectid, project.fxtcompanyid);
            var pro_listTo = ib.GetBuildingInfo(project.cityid, projectIdTo, project.fxtcompanyid);
            try
            {
                //是否存在该楼栋名称
                bool flag = ProjectRepeat(b_list, pro_listTo);
                if (flag)
                {
                    return -1;//目标楼盘存在数据，不能复制楼盘;
                }
                else
                {
                    int num = ib.MergerProject(project, projectIdTo, currfxtcompanyid);
                    if (num > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 复制楼盘
        /// 0:没有开通权限;-1:原始楼盘暂无可以复制的楼栋;-2:添加楼栋失败;-3:目标楼盘存在数据,不能复制楼盘;-4：程序异常;
        /// </summary>
        /// <param name="project">楼盘对象</param>
        /// <returns></returns>
        public string CopyProject(DAT_Project project, int ProductTypeCode, int currfxtcompanyid, int isDeleteTrue)
        {
            try
            {
                IDAT_Building dat = new DAT_BuildingDAL();
                int pId = project.projectid;//原始楼盘ID
                var result = GetProjectIdByName(project.cityid, project.AreaIdTo, currfxtcompanyid, project.projectname.Trim()).FirstOrDefault();
                //原始楼盘下的楼栋列表
                var b_list = dat.GetBuildingInfo(project.cityid, pId, currfxtcompanyid);
                if (result != null)
                {
                    //目标楼盘的楼栋集合
                    var pro_listTo = dat.GetBuildingInfo(project.cityid, result.projectid, project.fxtcompanyid);
                    //是否存在该楼栋名称
                    bool flag = ProjectRepeat(b_list, pro_listTo);
                    if (flag)
                    {
                        return "被复制楼盘存在相同楼栋,不能复制楼盘";//目标楼盘存在数据，不能复制楼盘;
                    }
                    else
                    {
                        int num = dat.AddBuilding(project.cityid, pId, project.fxtcompanyid, project.saveuser, result.projectid, b_list, currfxtcompanyid);
                        if (num > 0)
                        {
                            result.address = project.address;
                            result.othername = project.othername;
                            result.saveuser = project.creator;
                            result.savedatetime = DateTime.Now;
                            ModifyProject(result, currfxtcompanyid);
                            return "复制楼盘成功";
                        }
                        else
                        {
                            return "复制楼盘失败";
                        }
                    }
                }
                else
                {
                    //新增楼栋 楼盘  房号
                    ProjectQueryParams pa = new ProjectQueryParams();
                    pa.ProjectId = pId;
                    pa.CityId = project.cityid;
                    pa.FxtCompanyId = project.fxtcompanyid;
                    pa.AreaId = project.areaid;
                    var pro = GetProjectInfo(pa).FirstOrDefault();

                    pro.creator = project.creator;
                    pro.fxtcompanyid = currfxtcompanyid;
                    pro.areaid = project.AreaIdTo;
                    pro.address = project.address;
                    pro.othername = project.othername;
                    pro.projectname = project.projectname.Trim();
                    pro.weight = pro.weight == null ? 1 : pro.weight;
                    int projectId = AddProject(pro);
                    if (projectId > 0)
                    {
                        if (b_list.Count() > 0)
                        {
                            int re = dat.AddBuilding(project.cityid, pId, project.fxtcompanyid, project.creator, projectId, b_list, currfxtcompanyid);
                            if (re <= 0)
                            {
                                DeleteProject(projectId, project.cityid, project.fxtcompanyid, project.creator, ProductTypeCode, currfxtcompanyid, isDeleteTrue);
                                return "复制楼盘失败";
                            }
                            else
                            {
                                //复制楼盘图片
                                CopyProjectPhoto(project.cityid, project.fxtcompanyid, pId, currfxtcompanyid, projectId);
                                return "复制楼盘成功";
                            }
                        }
                        else
                        {
                            return "复制楼盘成功";
                        }
                    }
                    else
                    {
                        return "复制楼盘失败";
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CopyProjectPhoto(int cityid, int fxtcompanyid, int projectIdFrom, int currfxtcompanyid, int projectTo)
        {
            try
            {
                var list = GetCityTable(cityid, fxtcompanyid).FirstOrDefault();
                var comId = list != null ? list.ShowCompanyId : "";

                string sql = @"
insert into FXTProject.dbo.LNK_P_Photo(ProjectId,PhotoTypeCode,[Path],PhotoDate,PhotoName,CityId,Valid,FxtCompanyId,BuildingId,X,Y)
SELECT 
	@projectTo as ProjectId
	,PhotoTypeCode
	,[Path]
	,GETDATE() AS PhotoDate
	,PhotoName
	,CityId
	,1 AS Valid
	,@currfxtcompanyid as FxtCompanyId
	,BuildingId
	,X
	,Y
FROM FXTProject.dbo.LNK_P_Photo p WITH (NOLOCK)
WHERE 1 = 1
	AND CityId = @cityid
	and FxtCompanyId in (" + comId + @")
	and BuildingId = 0
	and Valid = 1
	and ProjectId = @projectIdFrom
	AND NOT EXISTS (
		SELECT ps.Id
		FROM FXTProject.dbo.LNK_P_Photo_sub ps WITH (NOLOCK)
		WHERE p.Id = ps.Id
			AND ps.FxtCompanyId = @FxtCompanyId
			AND ps.CityId = p.CityId
		)
UNION
SELECT 
	@projectTo as ProjectId
	,PhotoTypeCode
	,[Path]
	,GETDATE() AS PhotoDate
	,PhotoName
	,CityId
	,1 AS Valid
	,@currfxtcompanyid as FxtCompanyId
	,BuildingId
	,X
	,Y
FROM FXTProject.dbo.LNK_P_Photo_sub p WITH (NOLOCK)
WHERE 1 = 1
	AND CityId = @cityid
	and FxtCompanyId = @fxtcompanyid
	and BuildingId = 0
	and Valid = 1
	and ProjectId = @projectIdFrom";

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    return con.Execute(sql, new { cityid, fxtcompanyid, projectIdFrom, currfxtcompanyid, projectTo });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 楼盘拆分
        /// </summary>
        /// <param name="build_list">原始楼栋集合</param>
        /// <returns>0:没有开通权限;-1:原始楼盘暂无可以复制的楼栋;-2:添加楼栋失败;-3:目标楼盘存在数据,不能复制楼盘;-4：程序异常;</returns>
        public string SplitProject(DAT_Project project, string buildIdlist, string build_Name, int currfxtcompanyid)
        {
            IDAT_Building ib = new DAT_BuildingDAL();
            int pId = project.projectid;//原始楼盘ID
            var pro = GetProjectIdByName(project.cityid, project.AreaIdTo, project.fxtcompanyid, project.projectname.Trim()).FirstOrDefault();
            //原始楼盘下的楼栋列表
            //var b_list = ib.GetBuildingInfo(project.cityid, pId, project.fxtcompanyid);

            try
            {
                if (pro != null)
                {
                    //目标楼盘的楼栋集合
                    var pro_listTo = ib.GetBuildingInfo(project.cityid, pro.projectid, project.fxtcompanyid);
                    //是否存在该楼栋名称
                    bool flag = ProjectRepeat(build_Name, pro_listTo);
                    if (flag)
                    {
                        return "被拆分楼盘存在相同楼栋,不能拆分楼盘";//目标楼盘存在数据，不能拆分楼盘;
                    }
                    else
                    {
                        //存在该楼盘,更新楼栋的楼盘ID
                        int num = ib.SplitBuild(pId, pro.projectid, project.cityid, project.fxtcompanyid, project.saveuser, buildIdlist, currfxtcompanyid);
                        if (num > 0)
                        {
                            return "楼盘拆分成功";
                        }
                        else
                        {
                            return "楼盘拆分失败";
                        }
                    }
                }
                else
                {
                    //新增楼盘,
                    //新增楼栋 楼盘  房号
                    ProjectQueryParams pa = new ProjectQueryParams();
                    pa.ProjectId = pId;
                    pa.CityId = project.cityid;
                    pa.FxtCompanyId = project.fxtcompanyid;
                    var proj = GetProjectInfo(pa).FirstOrDefault();
                    proj.areaid = project.AreaIdTo;
                    proj.address = project.address;
                    proj.othername = project.othername;
                    proj.projectname = project.projectname.Trim();
                    proj.weight = proj.weight == null ? 1 : proj.weight;
                    int projectId = AddProject(proj);
                    if (projectId > 0)
                    {
                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            int num = ib.SplitBuild(pId, projectId, project.cityid, project.fxtcompanyid, project.saveuser, buildIdlist, currfxtcompanyid);
                            if (num > 0)
                                return "楼盘拆分成功";
                            else
                                return "楼盘拆分失败";
                        }

                    }
                    else
                    {
                        return "楼盘拆分失败";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 楼盘基础信息统计
        /// </summary>
        /// <param name="project">楼盘Model</param>
        /// <param name="areaid">行政区</param>
        /// <param name="subareaid">片区</param>
        /// <param name="ProjectName">楼盘名称</param>
        /// <param name="CityId">城市ID</param>
        /// <param name="FxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        public DataTable ProjectStatistcs(List<string> projectAttr, ProStatiParam pro, int CityId, int FxtCompanyId, bool self)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                var dt = GetCityTable(CityId, FxtCompanyId).FirstOrDefault();
                if (dt != null)
                {
                    string sql = "", selectAttr = "", selectAttrSub = "", where = "",
                    ptable = "FXTProject." + dt.projecttable, btable = "FXTProject." + dt.buildingtable, htable = "FXTProject." + dt.housetable, ctable = "FXTProject." + dt.casetable,
                    ComId = dt.ShowCompanyId;

                    where = ImportWhere_Project(pro, paramet, where);
                    if (projectAttr != null && projectAttr.Count > 0)
                    {
                        for (int i = 0; i < projectAttr.Count; i++)
                        {
                            selectAttr = ProjectConveCode(projectAttr, CityId, selectAttr, i);
                        }
                        if (!string.IsNullOrEmpty(selectAttr))
                        {
                            selectAttrSub = selectAttr.Replace("FxtCompanyId", "Fxt_CompanyId");
                        }
                        selectAttr = selectAttr.Substring(0, selectAttr.Length - 1);
                        selectAttrSub = selectAttrSub.Substring(0, selectAttrSub.Length - 1);

                        if (self)
                        {
                            if (FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                            {
                                where += " and p.FxtCompanyId = @fxtcompanyid";
                            }
                            else
                            {
                                where += " and p.Creator = @Creator";
                                paramet.Add(new SqlParameter("@Creator", pro.Creator));
                            }
                        }

                        sql = @"
select 
	(case when p.PlanPurpose like '1111%' then 
		 (select CodeName+',' from (
			select c.CodeName from FXTProject.dbo.SplitToTable(p.PlanPurpose,',')S,FxtDataCenter.dbo.SYS_Code c 
			where S.value = c.Code
		)T for XML Path(''))
		else p.PlanPurpose end
	) as opValue
	,p.*
	,(select top 1 ISNULL(c.BuildingNumber,0) from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK) where c.projectId = p.projectId and c.cityId = @cityid and c.fxtcompanyId = @fxtcompanyid and c.BuildingId = 0) as BuildingNumber
    ,(select top 1 ISNULL(c.HouseNumber,0) from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK) where c.projectId = p.projectId and c.cityId = @cityid and c.fxtcompanyId = @fxtcompanyid and c.BuildingId = 0) as HouseNumber
	,ci.CityName
	,sa.AreaName
	,ssa.SubAreaName
	,DeveCompanyName = (select top 1 ChineseName from FXTProject.dbo.LNK_P_Company pc,FxtDataCenter.dbo.DAT_Company d where pc.CompanyId=d.CompanyId and  pc.CityId=@CityId and pc.ProjectId=p.ProjectId and PC.CompanyType=2001001)
	,DeveCompanyId = (select top 1 pc.CompanyId from FXTProject.dbo.LNK_P_Company pc,FxtDataCenter.dbo.DAT_Company d where pc.CompanyId=d.CompanyId and  pc.CityId=@CityId and pc.ProjectId=p.ProjectId and PC.CompanyType=2001001)
	,ManagerCompanyName = (select top 1 ChineseName from FXTProject.dbo.LNK_P_Company pc,FxtDataCenter.dbo.DAT_Company d where pc.CompanyId=d.CompanyId and  pc.CityId=@CityId and pc.ProjectId=p.ProjectId and PC.CompanyType=2001004)
	,ManagerCompanyId = (select top 1 pc.CompanyId from FXTProject.dbo.LNK_P_Company pc,FxtDataCenter.dbo.DAT_Company d where pc.CompanyId=d.CompanyId and  pc.CityId=@CityId and pc.ProjectId=p.ProjectId and PC.CompanyType=2001004)
from (
	select
		ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,ParkingDesc,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,SourceName
        ,FxtCompanyId,FxtCompanyId as belongcompanyid
        ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=FxtCompanyId)
	from " + ptable + @" p with(nolock)
	where not exists(
		select ProjectId from " + ptable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.Fxt_CompanyId = @fxtcompanyid
		and ps.CityID = @cityid
	)
	and p.Valid = 1
	and p.FxtCompanyId in (" + ComId + @")
	and p.CityID = @cityid
	union
	select 
		p.ProjectId,p.ProjectName,p.SubAreaId,p.FieldNo,p.PurposeCode,p.Address,p.LandArea,p.StartDate,p.StartEndDate,p.UsableYear,p.BuildingArea,p.SalableArea,p.CubageRate,p.GreenRate,p.BuildingDate,p.CoverDate,p.SaleDate,p.JoinDate,p.EndDate,p.InnerSaleDate,p.RightCode,p.ParkingNumber,p.ParkingDesc,p.AveragePrice,p.ManagerTel,p.ManagerPrice,p.TotalNum,p.BuildingNum,p.Detail,p.BuildingTypeCode,p.UpdateDateTime,p.OfficeArea,p.OtherArea,p.PlanPurpose,p.PriceDate,p.IsComplete,p.OtherName,p.SaveDateTime,p.SaveUser,p.Weight,p.BusinessArea,p.IndustryArea,p.IsEValue,p.PinYin,p.CityID,p.AreaID,p.OldId,p.CreateTime,p.AreaLineId,p.Valid,p.SalePrice,p.PinYinAll,p.X,p.Y,p.XYScale,p.Creator,p.IsEmpty,p.TotalId,p.East,p.West,p.South,p.North,p.BuildingQuality,p.HousingScale,p.BuildingDetail,p.HouseDetail,p.BasementPurpose,p.ManagerQuality,p.Facilities,p.AppendageClass,p.RegionalAnalysis,p.Wrinkle,p.Aversion,p.SourceName
        ,p.Fxt_CompanyId,pi.FxtCompanyId as belongcompanyid
        ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=pi.FxtCompanyId)
	from " + ptable + "_sub p with(nolock) left join " + ptable+ @" pi with(nolock) on p.ProjectId = pi.ProjectId
	where p.Valid = 1
	and p.Fxt_CompanyId = @fxtcompanyid
	and p.CityID = @cityid
)p
LEFT JOIN FxtDataCenter.dbo.SYS_Area sa WITH (NOLOCK) ON sa.AreaId = p.AreaID
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea ssa WITH (NOLOCK) ON ssa.SubAreaId = p.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_City ci WITH (NOLOCK) ON ci.cityid = p.cityid
where 1 = 1" + where;

                        sql = "select p.ProjectId as 楼盘ID," + selectAttr + @" from (" + sql + ")p";
                    }
                    paramet.Add(new SqlParameter("@CityId", CityId));
                    paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    DataTable dtable = DBHelperSql.ExecuteDataTable(sql, param);
                    return dtable;
                }
                return new DataTable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 楼栋code转换Name
        /// </summary>
        /// <param name="projectAttr"></param>
        /// <param name="CityId"></param>
        /// <param name="selectAttr"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string ProjectConveCode(List<string> projectAttr, int CityId, string selectAttr, int i)
        {
            if (projectAttr[i].Split('&')[0] == "CityName")
            {
                selectAttr += " (select CityName from FxtDataCenter.dbo.SYS_City with(nolock) where CityId=" + CityId + ") as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "AreaName")
            {
                selectAttr += " (select AreaName from FxtDataCenter.dbo.SYS_Area with(nolock) where AreaId=p.AreaID) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "SubAreaName")
            {
                selectAttr += " (select SubAreaName from FxtDataCenter.dbo.SYS_SubArea with(nolock) where SubAreaId=p.SubAreaId) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "PurposeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where  Code=p.PurposeCode) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "RightCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=p.RightCode) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "BuildingTypeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=p.BuildingTypeCode) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "BuildingQualityName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=p.BuildingQuality) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "AppendageClassName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=p.AppendageClass) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "HousingScaleName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=p.HousingScale) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "ManagerQualityName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=p.ManagerQuality) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "IsComplete")
            {
                selectAttr += " (case isnull(p.IsComplete,-1) when 0 then '否' when 1 then '是' else '' end) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "IsEmpty")
            {
                selectAttr += " (case isnull(p.IsEmpty,-1) when 0 then '否' when 1 then '是' else '' end) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "IsEValue")
            {
                selectAttr += " (case isnull(p.IsEValue,-1) when 0 then '否' when 1 then '是' else '' end) as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "buildingNumber")
            {
                selectAttr += " p." + projectAttr[i].Split('&')[0] + " as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else if (projectAttr[i].Split('&')[0] == "houseNumber")
            {
                selectAttr += " p." + projectAttr[i].Split('&')[0] + " as '" + projectAttr[i].Split('&')[1] + "',";
            }
            else
            {
                selectAttr += " p." + projectAttr[i].Split('&')[0] + " as '" + projectAttr[i].Split('&')[1] + "',";
            }
            return selectAttr;
        }

        /// <summary>
        /// 楼盘基础信息---楼栋统计
        /// </summary>
        /// <param name="building">楼栋model</param>
        /// <param name="areaid"></param>
        /// <param name="subareaid"></param>
        /// <param name="ProjectName"></param>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public DataTable BuildStatistcs(List<string> buildAttr, BuildStatiParam build, int CityId, int FxtCompanyId, bool self)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                var dt = GetCityTable(CityId, FxtCompanyId).FirstOrDefault();
                if (dt != null)
                {
                    string sql = "", selectAttr = "", selectAttrSub = "", where = "",
                    ptable = "FXTProject." + dt.projecttable, btable = "FXTProject." + dt.buildingtable, htable = "FXTProject." + dt.housetable, ctable = "FXTProject." + dt.casetable,
                    ComId = dt.ShowCompanyId;
                    where = ImportWhere_Build(build, paramet, where);
                    for (int i = 0; i < buildAttr.Count; i++)
                    {
                        selectAttr = BuildConveCode(buildAttr, CityId, selectAttr, i);
                    }
                    if (!string.IsNullOrEmpty(selectAttr))
                    {
                        selectAttrSub = selectAttr.Replace("FxtCompanyId", "Fxt_CompanyId");
                    }
                    selectAttr = selectAttr.Substring(0, selectAttr.Length - 1);
                    selectAttrSub = selectAttrSub.Substring(0, selectAttrSub.Length - 1);

                    if (self)
                    {
                        if (FxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                        {
                            where += " and T.FxtCompanyId = @fxtcompanyid and T.projectFxtCompanyId = @fxtcompanyid";
                        }
                        else
                        {
                            where += " and T.Creator = @Creator and T.projectCreator = @Creator";
                            paramet.Add(new SqlParameter("@Creator", build.Creator));
                        }
                    }

                    sql = @"
select * from(
	select 
		ci.CityName
		,p.AreaID
		,p.SubAreaId
		,sa.AreaName
		,ssa.SubAreaName
		,P.ProjectName
		,P.Creator as projectCreator
        ,P.FxtCompanyId as projectFxtCompanyId
		,B.*
		,(select top 1 ISNULL(c.HouseNumber,0) from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK) where c.projectId = p.projectId and c.cityId = @CityId and c.fxtcompanyId = @fxtcompanyid and c.BuildingId = B.BuildingId) as totalHouseNumber
	from (
		select 
			ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,ParkingDesc,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,SourceName,FxtCompanyId
		from " + ptable + @" p with(nolock)
		where not exists(
			select ProjectId from " + ptable + @"_sub ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityID = @CityId
			and ps.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @CityId
		and FxtCompanyId in (" + ComId + @")
		union
		select
			ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,ParkingDesc,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,SourceName,Fxt_CompanyId
		from " + ptable + @"_sub p with(nolock)
		where Valid = 1
		and CityID = @CityId
		and Fxt_CompanyId = @fxtcompanyid
	)P
	inner join (
		select 
			BuildingId,BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,Valid,SalePrice,SaveDateTime,SaveUser,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalfloor
        ,FxtCompanyId,FxtCompanyId as belongcompanyid
        ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=FxtCompanyId)
		from " + btable + @" b with(nolock)
		where not exists(
			select BuildingId from " + btable + @"_sub bs with(nolock)
			where bs.BuildingId = b.BuildingId
			and bs.CityID = @CityId
			and bs.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @CityId
		and FxtCompanyId in (" + ComId + @")
		union
		select
			b.BuildingId,b.BuildingName,b.ProjectId,b.PurposeCode,b.StructureCode,b.BuildingTypeCode,b.TotalFloor,b.FloorHigh,b.SaleLicence,b.ElevatorRate,b.UnitsNumber,b.TotalNumber,b.TotalBuildArea,b.BuildDate,b.SaleDate,b.AveragePrice,b.AverageFloor,b.JoinDate,b.LicenceDate,b.OtherName,b.Weight,b.IsEValue,b.CityID,b.CreateTime,b.OldId,b.Valid,b.SalePrice,b.SaveDateTime,b.SaveUser,b.LocationCode,b.SightCode,b.FrontCode,b.StructureWeight,b.BuildingTypeWeight,b.YearWeight,b.PurposeWeight,b.LocationWeight,b.SightWeight,b.FrontWeight,b.X,b.Y,b.XYScale,b.Wall,b.IsElevator,b.SubAveragePrice,b.PriceDetail,b.BHouseTypeCode,b.BHouseTypeWeight,b.Creator,b.Distance,b.DistanceWeight,b.basement,b.Remark,b.ElevatorRateWeight,b.IsYard,b.YardWeight,b.Doorplate,b.RightCode,b.IsVirtual,b.FloorSpread,b.PodiumBuildingFloor,b.PodiumBuildingArea,b.TowerBuildingArea,b.BasementArea,b.BasementPurpose,b.HouseNumber,b.HouseArea,b.OtherNumber,b.OtherArea,b.innerFitmentCode,b.FloorHouseNumber,b.LiftNumber,b.LiftBrand,b.Facilities,b.PipelineGasCode,b.HeatingModeCode,b.WallTypeCode,b.MaintenanceCode,b.isTotalfloor
            ,b.Fxt_CompanyId,bi.FxtCompanyId
            ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=bi.FxtCompanyId)
		from " + btable + "_sub b with(nolock) left join " + btable + @" bi with(nolock) on b.BuildingId = bi.BuildingId
		where b.Valid = 1
		and b.CityID = @CityId
		and Fxt_CompanyId = @fxtcompanyid
	)B on P.ProjectId = B.ProjectId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area sa WITH (NOLOCK) ON sa.AreaId = p.AreaID
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea ssa WITH (NOLOCK) ON ssa.SubAreaId = p.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_City ci WITH (NOLOCK) ON ci.cityid = p.cityid
	where 1 = 1
)T
where 1 = 1" + where;
                    sql = @"select ProjectId as 楼盘ID,BuildingId as 楼栋ID, " + selectAttr + @" from (" + sql + @")T";

                    paramet.Add(new SqlParameter("@CityId", CityId));
                    paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                    SqlParameter[] param = paramet.ToArray();
                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    DataTable dtable = DBHelperSql.ExecuteDataTable(sql, param);
                    return dtable;
                }
                return new DataTable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 楼栋code转换Name
        /// </summary>
        /// <param name="buildAttr"></param>
        /// <param name="CityId"></param>
        /// <param name="selectAttr"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string BuildConveCode(List<string> buildAttr, int CityId, string selectAttr, int i)
        {
            if (buildAttr[i].Split('&')[0] == "IsVirtual")
            {
                selectAttr += " (case isnull(IsVirtual,-1) when 0 then '否' when 1 then '是' else '' end) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "RightCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=RightCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "PurposeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=PurposeCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "LocationCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=LocationCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "StructureCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=StructureCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "BuildingTypeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=BuildingTypeCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "FrontCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=FrontCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "SightCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=SightCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "WallName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=Wall) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "WallTypeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=WallTypeCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "MaintenanceCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=MaintenanceCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "innerFitmentCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=innerFitmentCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "PipelineGasCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=PipelineGasCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "HeatingModeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code=HeatingModeCode) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "IsElevator")
            {
                selectAttr += " (case isnull(IsElevator,-1) when 0 then '否' when 1 then '是' else '' end) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "IsYard")
            {
                selectAttr += " (case isnull(IsYard,-1) when 0 then '否' when 1 then '是' else '' end) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "IsEValue")
            {
                selectAttr += " (case isnull(IsEValue,-1) when 0 then '否' when 1 then '是' else '' end) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else if (buildAttr[i].Split('&')[0] == "isTotalfloorName")
            {
                selectAttr += " (case isnull(isTotalfloor,-1) when 0 then '否' when 1 then '是' else '' end) as '" + buildAttr[i].Split('&')[1] + "',";
            }
            else
            {
                selectAttr += buildAttr[i].Split('&')[0] + " as '" + buildAttr[i].Split('&')[1] + "',";
            }
            return selectAttr;
        }

        public DataTable HouseStatistcs(List<string> houseAttr, HouseStatiParam house, int cityId, int fxtCompanyId, bool self)
        {
            var paramet = new List<SqlParameter>();
            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            try
            {
                if (dt == null) return new DataTable();

                var ptable = "FXTProject." + dt.projecttable;
                var btable = "FXTProject." + dt.buildingtable;
                var htable = "FXTProject." + dt.housetable;
                var comId = string.IsNullOrEmpty(dt.ShowCompanyId) ? fxtCompanyId.ToString() : dt.ShowCompanyId;

                var where = ImportWhere_House(house, paramet);
                var selectAttr = "";

                for (var i = 0; i < houseAttr.Count; i++)
                {
                    selectAttr = HouseConveCode(houseAttr, cityId, selectAttr, i);

                }
                selectAttr = selectAttr.Substring(0, selectAttr.Length - 1);

                if (self)
                {
                    if (fxtCompanyId == Convert.ToInt32(ConfigurationHelper.FxtCompanyId))
                    {
                        where += " and T.ProjectFxtCompanyId = @fxtcompanyid and T.BuildingFxtCompanyId = @fxtcompanyid and T.FxtCompanyId = @fxtcompanyid";
                    }
                    else
                    {
                        where += " and T.ProjectCreator = @Creator and T.BuildingCreator = @Creator and T.Creator = @Creator";
                        paramet.Add(new SqlParameter("@Creator", house.Creator));
                    }
                }

                var sql = @"
select * from (
	select 
		ci.CityName
		,P.ProjectId
		,P.ProjectName
		,p.AreaID
		,a.AreaName
		,p.SubAreaId
		,sa.SubAreaName
		,B.BuildingName
        ,p.AveragePrice as PAveragePrice
        ,B.AveragePrice as BAveragePrice
        ,B.Weight as BWeight
        ,P.Creator as ProjectCreator
        ,P.FxtCompanyId as ProjectFxtCompanyId
        ,B.Creator as BuildingCreator
        ,B.FxtCompanyId as BuildingFxtCompanyId
		,H.*
	from (
		select 
			ProjectId,ProjectName,SubAreaId,AveragePrice,AreaID,CityID,Creator,FxtCompanyId
		from " + ptable + @" p with(nolock)
		where not exists(
			select ProjectId from " + ptable + @"_sub ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityID = @cityid
			and ps.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + comId + @")
		union
		select 
			ProjectId,ProjectName,SubAreaId,AveragePrice,AreaID,CityID,Creator,Fxt_CompanyId
		from " + ptable + @"_sub p with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
	)P
	inner join
	(
		select 
			ProjectId,BuildingId,BuildingName,AveragePrice,Weight,Creator,FxtCompanyId
		from " + btable + @" b with(nolock)
		where not exists(
			select * from " + btable + @"_sub bs with(nolock)
			where bs.BuildingId = b.BuildingId
			and bs.CityID = @cityid
			and bs.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + comId + @")
		union
		select 	
			ProjectId,BuildingId,BuildingName,AveragePrice,Weight,Creator,Fxt_CompanyId
		from " + btable + @"_sub b with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
	)B on P.ProjectId = B.ProjectId
	inner join (
		select 
			HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice,Weight,PhotoName,Remark,StructureCode,TotalPrice,PurposeCode,IsEValue,CityID,OldId,CreateTime,Valid,SaveDateTime,SaveUser,FxtCompanyId,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode
            ,FxtCompanyId as belongcompanyid
            ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=FxtCompanyId)
		from " + htable + @" h with(nolock)
		where not exists(
			select HouseId from " + htable + @"_sub hs with(nolock)
			where hs.HouseId = h.HouseId
			and hs.CityID = @cityid
			and hs.FxtCompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + comId + @")
		union
		select 
			h.HouseId,h.BuildingId,h.HouseName,h.HouseTypeCode,h.FloorNo,h.UnitNo,h.BuildArea,h.FrontCode,h.SightCode,h.UnitPrice,h.SalePrice,h.Weight,h.PhotoName,h.Remark,h.StructureCode,h.TotalPrice,h.PurposeCode,h.IsEValue,h.CityID,h.OldId,h.CreateTime,h.Valid,h.SaveDateTime,h.SaveUser,h.FxtCompanyId,h.IsShowBuildingArea,h.InnerBuildingArea,h.SubHouseType,h.SubHouseArea,h.Creator,h.NominalFloor,h.VDCode,h.FitmentCode,h.Cookroom,h.Balcony,h.Toilet,h.NoiseCode
            ,hi.FxtCompanyId
            ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=hi.FxtCompanyId)
		from " + htable + "_sub h with(nolock) left join " + htable + @"  hi with(nolock) on h.HouseId = hi.HouseId
		where h.Valid = 1
		and h.CityID = @cityid
		and h.FxtCompanyId = @fxtcompanyid
	)H on B.BuildingId = H.BuildingId
	left join FxtDataCenter.dbo.SYS_City ci with(nolock) on P.CityID = ci.CityId
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on P.AreaID = a.AreaId
	left join FxtDataCenter.dbo.SYS_SubArea sa with(nolock) on P.SubAreaId = sa.SubAreaId
)T
where 1 = 1" + where;
                sql = @"select ProjectId as 楼盘ID,BuildingId as 楼栋ID,HouseId as 房号ID, " + selectAttr + @" from (" + sql + @")T";

                paramet.Add(new SqlParameter("@CityId", cityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", fxtCompanyId));
                var param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var dtable = DBHelperSql.ExecuteDataTable(sql, param);
                return dtable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 房号code转换Name
        /// </summary>
        /// <param name="houseAttr"></param>
        /// <param name="CityId"></param>
        /// <param name="selectAttr"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string HouseConveCode(IList<string> houseAttr, int CityId, string selectAttr, int i)
        {
            if (houseAttr[i].Split('&')[0] == "HouseTypeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = HouseTypeCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "StructureCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = StructureCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "PurposeCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = PurposeCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "FrontCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = FrontCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "SightCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = SightCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "VDCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = VDCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "SubHouseTypeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock)  where Code = SubHouseType) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "FitmentCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = FitmentCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "Cookroom")
            {
                selectAttr += " (case isnull(Cookroom,-1) when 0 then '否' when 1 then '是' else '' end) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "IsShowBuildingArea")
            {
                selectAttr += " (case isnull(IsShowBuildingArea,-1) when 0 then '否' when 1 then '是' else '' end) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "IsEValue")
            {
                selectAttr += " (case isnull(IsEValue,-1) when 0 then '否' when 1 then '是' else '' end) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else if (houseAttr[i].Split('&')[0] == "NoiseCodeName")
            {
                selectAttr += " (select CodeName from FxtDataCenter.dbo.sys_code with(nolock) where Code = NoiseCode) as '" + houseAttr[i].Split('&')[1] + "',";
            }
            else
            {
                selectAttr += houseAttr[i].Split('&')[0] + " as '" + houseAttr[i].Split('&')[1] + "',";
            }
            return selectAttr;
        }

        /// <summary>
        /// 房号导出筛选条件
        /// </summary>
        /// <param name="house"></param>
        /// <param name="paramet"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private string ImportWhere_House(HouseStatiParam house, ICollection<SqlParameter> paramet)
        {
            var where = string.Empty;
            if (house.AreaId > 0)
            {
                where += " and AreaID = @areaid ";
                paramet.Add(new SqlParameter("@areaid", house.AreaId));
            }
            if (house.SubAreaId > 0)
            {
                where += " and SubAreaId = @subareaid ";
                paramet.Add(new SqlParameter("@subareaid", house.SubAreaId));
            }
            if (!string.IsNullOrEmpty(house.ProjectName))
            {
                where += "  and ProjectName = @ProjectName";
                paramet.Add(new SqlParameter("@ProjectName", house.ProjectName));
            }
            if (house.HouseTypeCode != null && house.HouseTypeCode > 0)
            {
                where += "  and HouseTypeCode = @HouseTypeCode";
                paramet.Add(new SqlParameter("@HouseTypeCode", house.HouseTypeCode));
            }
            if (house.HouseStructureCode > 0)
            {
                where += "  and StructureCode = @StructureCode";
                paramet.Add(new SqlParameter("@StructureCode", house.HouseStructureCode));
            }
            if (house.FrontCode != null && house.FrontCode > 0)
            {
                where += "  and FrontCode = @FrontCode";
                paramet.Add(new SqlParameter("@FrontCode", house.FrontCode));
            }
            if (house.SightCode != null && house.SightCode > 0)
            {
                where += "  and SightCode = @SightCode";
                paramet.Add(new SqlParameter("@SightCode", house.SightCode));
            }
            if (house.PurposeCode != null && house.PurposeCode > 0)
            {
                where += "  and PurposeCode = @PurposeCode";
                paramet.Add(new SqlParameter("@PurposeCode", house.PurposeCode));
            }
            if (house.HstarTime != null && house.HstarTime > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(house.HstarTime.ToString(), "")))
                {
                    where += "  and CreateTime >= @HstarTime";
                    paramet.Add(new SqlParameter("@HstarTime", house.HstarTime));
                }
            }
            if (house.HendTime != null && house.HendTime > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(house.HendTime.ToString(), "")))
                {
                    where += "  and CreateTime <= @HendTime";
                    house.HendTime = ((DateTime)house.HendTime).AddDays(1);
                    paramet.Add(new SqlParameter("@HendTime", house.HendTime));
                }
            }
            return where;
        }

        /// <summary>
        /// 楼栋导出筛选条件
        /// </summary>
        /// <param name="build"></param>
        /// <param name="paramet"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private string ImportWhere_Build(BuildStatiParam build, List<SqlParameter> paramet, string where)
        {
            if (build.AreaId > 0)
            {
                where += " and AreaID = @areaid ";
                paramet.Add(new SqlParameter("@areaid", build.AreaId));
            }
            if (build.SubAreaId > 0)
            {
                where += " and SubAreaId = @subareaid ";
                paramet.Add(new SqlParameter("@subareaid", build.SubAreaId));
            }
            if (!string.IsNullOrEmpty(build.ProjectName))
            {
                where += "  and ProjectName = @ProjectName";
                paramet.Add(new SqlParameter("@ProjectName", build.ProjectName));
            }
            if (build.PurposeCode != null && build.PurposeCode > 0)
            {
                where += " and PurposeCode = @PurposeCode ";
                paramet.Add(new SqlParameter("@PurposeCode", build.PurposeCode));
            }
            if (build.BuildingTypeCode != null && build.BuildingTypeCode > 0)
            {
                where += " and BuildingTypeCode = @BuildingTypeCode ";
                paramet.Add(new SqlParameter("@BuildingTypeCode", build.BuildingTypeCode));
            }
            if (build.StructureCode != null && build.StructureCode > 0)
            {
                where += " and StructureCode = @StructureCode ";
                paramet.Add(new SqlParameter("@StructureCode", build.StructureCode));
            }
            if (build.TotalFloor != null && build.TotalFloor >= 0)
            {
                if (build.TotalFloorCompany_build > 0)
                {
                    where += " and TotalFloor >= @TotalFloor ";
                }
                else
                {
                    where += " and TotalFloor <= @TotalFloor ";
                }
                paramet.Add(new SqlParameter("@TotalFloor", build.TotalFloor));
            }
            if (build.TotalNumber != null && build.TotalNumber >= 0)
            {
                if (build.TotalNumberCompany_build > 0)
                {
                    where += " and TotalNumber >= @TotalNumber ";
                }
                else
                {
                    where += " and TotalNumber <= @TotalNumber ";
                }
                paramet.Add(new SqlParameter("@TotalNumber", build.TotalNumber));
            }
            if (build.TotalBuildArea != null && build.TotalBuildArea >= 0)
            {
                if (build.TotalBuildAreaCompany_build > 0)
                {
                    where += " and TotalBuildArea >= @TotalBuildArea ";
                }
                else
                {
                    where += " and TotalBuildArea <= @TotalBuildArea ";
                }
                paramet.Add(new SqlParameter("@TotalBuildArea", build.TotalBuildArea));
            }
            if (build.BuildDate != null && build.BuildDate > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(build.BuildDate.ToString(), "")))
                {
                    where += "  and BuildDate >= @BuildDate";
                    paramet.Add(new SqlParameter("@BuildDate", build.BuildDate));
                }
            }
            if (build.BuildDateTo != null && build.BuildDateTo > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(build.BuildDateTo.ToString(), "")))
                {
                    where += "  and BuildDate <= @BuildDateTo";
                    paramet.Add(new SqlParameter("@BuildDateTo", build.BuildDateTo));
                }
            }
            if (build.BstarTime != null && build.BstarTime > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(build.BstarTime.ToString(), "")))
                {
                    where += "  and CreateTime >= @createtime";
                    paramet.Add(new SqlParameter("@createtime", build.BstarTime));
                }
            }
            if (build.BendTime != null && build.BendTime > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(build.BendTime.ToString(), "")))
                {
                    where += "  and CreateTime < @createtimeto";
                    build.BendTime = ((DateTime)build.BendTime).AddDays(1);
                    paramet.Add(new SqlParameter("@createtimeto", build.BendTime));
                }
            }
            return where;
        }

        /// <summary>
        /// 楼盘导出筛选条件
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="paramet"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private static string ImportWhere_Project(ProStatiParam pro, List<SqlParameter> paramet, string where)
        {
            if (pro.AreaID > 0)
            {
                where += " and p.areaid = @areaid ";
                paramet.Add(new SqlParameter("@areaid", pro.AreaID));
            }
            if (pro.SubAreaId > 0)
            {
                where += " and p.subareaid = @subareaid ";
                paramet.Add(new SqlParameter("@subareaid", pro.SubAreaId));
            }
            if (!string.IsNullOrEmpty(pro.ProjectName))
            {
                where += "  and p.ProjectName = @ProjectName";
                paramet.Add(new SqlParameter("@ProjectName", pro.ProjectName));
            }
            if (pro.RightCode != null && pro.RightCode > 0)
            {
                where += "  and p.RightCode = @RightCode";
                paramet.Add(new SqlParameter("@RightCode", pro.RightCode));
            }
            if (pro.BuildingTypeCode != null && pro.BuildingTypeCode > 0)
            {
                where += "  and p.BuildingTypeCode = @BuildingTypeCode";
                paramet.Add(new SqlParameter("@BuildingTypeCode", pro.BuildingTypeCode));
            }
            if (pro.GreenRate != null && pro.GreenRate >= 0)
            {
                if (pro.GreenRateCompany_project > 0)
                {
                    where += "  and p.GreenRate >= @GreenRate";
                }
                else
                {
                    where += "  and p.GreenRate <= @GreenRate";
                }
                paramet.Add(new SqlParameter("@GreenRate", pro.GreenRate));
            }
            if (pro.CubageRate != null && pro.CubageRate >= 0)
            {
                if (pro.CubageRateCompany_project > 0)
                {
                    where += "  and p.CubageRate >= @CubageRate";
                }
                else
                {
                    where += "  and p.CubageRate <= @CubageRate";
                }
                paramet.Add(new SqlParameter("@CubageRate", pro.CubageRate));
            }
            if (pro.LandArea != null && pro.LandArea >= 0)
            {
                if (pro.LandAreaCompany_project > 0)
                {
                    where += "  and p.LandArea >= @LandArea";
                }
                else
                {
                    where += "  and p.LandArea <= @LandArea";
                }
                paramet.Add(new SqlParameter("@LandArea", pro.LandArea));
            }
            if (pro.ParkingNumber != null && pro.ParkingNumber >= 0)
            {
                if (pro.ParkingNumberCompany_project > 0)
                {
                    where += "  and p.ParkingNumber >= @ParkingNumber";
                }
                else
                {
                    where += "  and p.ParkingNumber <= @ParkingNumber";
                }
                paramet.Add(new SqlParameter("@ParkingNumber", pro.ParkingNumber));
            }
            if (pro.TotalNum != null && pro.TotalNum >= 0)
            {
                if (pro.TotalNumCompany_project > 0)
                {
                    where += "  and p.TotalNum >= @TotalNum";
                }
                else
                {
                    where += "  and p.TotalNum <= @TotalNum";
                }
                paramet.Add(new SqlParameter("@TotalNum", pro.TotalNum));
            }
            if (pro.BuildingNum != null && pro.BuildingNum >= 0)
            {
                if (pro.BuildingNumCompany_project > 0)
                {
                    where += "  and p.BuildingNum >= @BuildingNum";
                }
                else
                {
                    where += "  and p.BuildingNum <= @BuildingNum";
                }
                paramet.Add(new SqlParameter("@BuildingNum", pro.BuildingNum));
            }
            if (pro.EndDate != null && pro.EndDate > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(pro.EndDate.ToString(), "")))
                {
                    where += "  and p.EndDate >= @EndDate";
                    paramet.Add(new SqlParameter("@EndDate", pro.EndDate));
                }
            }
            if (pro.EndDateTo != null && pro.EndDateTo > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(pro.EndDateTo.ToString(), "")))
                {
                    where += "  and p.EndDate <= @EndDateTo";
                    paramet.Add(new SqlParameter("@EndDateTo", pro.EndDateTo));
                }
            }
            if (pro.PstarTime != null && pro.PstarTime > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(pro.PstarTime.ToString(), "")))
                {
                    where += "  and p.CreateTime >= @CreateTime";
                    paramet.Add(new SqlParameter("@CreateTime", pro.PstarTime));
                }
            }
            if (pro.PendTime != null && pro.PendTime > DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(pro.PendTime.ToString(), "")))
                {
                    where += "  and p.CreateTime < @CreateTimeTo";
                    pro.PendTime = ((DateTime)pro.PendTime).AddDays(1);
                    paramet.Add(new SqlParameter("@CreateTimeTo", pro.PendTime));
                }
            }
            return where;
        }

        /// <summary>
        /// 楼盘合并
        /// 删除目标楼盘
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public int MergerProjectDel(int projectId, int CityId, int FxtCompanyId, string UserName, int ProductTypeCode, int currFxtCompanyId, int isDeleteTrue)
        {
            try
            {
                int num = DeleteProject(projectId, CityId, FxtCompanyId, UserName, ProductTypeCode, currFxtCompanyId, isDeleteTrue);
                if (num > 0)
                {
                    var dt = GetCityTable(CityId, FxtCompanyId).FirstOrDefault();
                    if (dt != null)
                    {
                        string btable = dt.buildingtable,
                        htable = dt.housetable;
                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            string sql_str = "  update  " + btable + " set valid = 0,SaveDateTime=GETDATE(),SaveUser=@UserName  where ProjectId=@ProjectId and CityId=@CityId and FxtCompanyId=@FxtCompanyId"
                                      + " update  " + btable + "_sub set valid = 0,SaveDateTime=GETDATE(),SaveUser=@UserName where ProjectId=@ProjectId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId"
                                      + " update  " + htable + " set valid = 0,SaveDateTime=GETDATE(),SaveUser=@UserName where CityId=@CityId and FxtCompanyId=@FxtCompanyId and (BuildingId in(select BuildingId from " + btable + " where ProjectId=@ProjectId and CityId=@CityId and FxtCompanyId=@FxtCompanyId) or BuildingId in(select BuildingId from " + btable + "_sub where ProjectId=@ProjectId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId))"
                                      + " update  " + htable + "_sub set valid = 0,SaveDateTime=GETDATE(),SaveUser=@UserName where CityId=@CityId and FxtCompanyId=@FxtCompanyId and (BuildingId in(select BuildingId from " + btable + " where ProjectId=@ProjectId and CityId=@CityId and FxtCompanyId=@FxtCompanyId) or BuildingId in(select BuildingId from " + btable + "_sub where ProjectId=@ProjectId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId))";
                            int result = con.Execute(sql_str, new { ProjectId = projectId, CityId = CityId, FxtCompanyId = FxtCompanyId, UserName = UserName });
                        }
                    }
                }
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int AddLnkPCompany(LNK_P_Company lnkPCompany)
        {
            var strSql = @"Insert into FXTProject.dbo.LNK_P_Company(ProjectId,CompanyId,CompanyType,CityId) values(@projectId,@companyId,@companyType,@cityId)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, lnkPCompany);
            }

        }

        /// <summary>
        /// 楼栋名称是否重复
        /// </summary>
        /// <param name="pro_list"></param>
        /// <param name="pro_listTo"></param>
        /// <returns></returns>
        private bool ProjectRepeat(string buildIdlist, IEnumerable<DAT_Building> pro_listTo)
        {
            bool flag = false;
            foreach (var listTo in pro_listTo)
            {
                if (listTo.buildingname == buildIdlist)
                {
                    flag = true;
                    break;
                }
                if (flag)
                {
                    break;
                }
            }
            return flag;


        }

        /// <summary>
        /// 验证宗地号
        /// </summary>
        /// <param name="CityID"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="fieldNo"></param>
        /// <returns></returns>
        public bool ValidFieldNo(int CityID, int fxtCompanyId, string fieldNo)
        {
            try
            {
                if (string.IsNullOrEmpty(fieldNo)) return false;

                var list_city_table = GetCityTable(CityID, fxtCompanyId).FirstOrDefault();
                if (list_city_table != null)
                {
                    string ptable = list_city_table.projecttable,
                    ComId = list_city_table.ShowCompanyId;
                    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        var sb = new StringBuilder();
                        sb.Append("select p.fieldNo from " + ptable + " P with(nolock) ");
                        sb.Append(" where P.CityId=@CityId  and p.valid=1 and p.fieldNo=@fieldNo  and p.FxtCompanyId in(" + ComId + ") and ");
                        sb.Append(" not exists (select ProjectId from " + ptable + "_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@fxtCompanyId and ps.CityId=p.CityId) ");
                        sb.Append(" union ");
                        sb.Append(" select p.fieldNo from " + ptable + "_sub P with(nolock) ");
                        sb.Append(" where P.CityId=@CityId and p.fieldNo=@fieldNo and p.valid=1 and ");
                        sb.Append(" p.Fxt_CompanyId=@fxtCompanyId");
                        var result = conn.Query<DAT_Project>(sb.ToString(), new { CityId = CityID, fieldNo = fieldNo, fxtCompanyId = fxtCompanyId });
                        if (result != null && result.Count() > 0)
                            return true;
                        else
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 楼栋名称是否重复
        /// </summary>
        /// <param name="pro_list"></param>
        /// <param name="pro_listTo"></param>
        /// <returns></returns>
        private bool ProjectRepeat(IEnumerable<DAT_Building> pro_list, IEnumerable<DAT_Building> pro_listTo)
        {
            bool flag = false;
            if (pro_list != null && pro_list.Count() > 0)
            {
                if (pro_listTo != null && pro_listTo.Count() > 0)
                {
                    foreach (var list in pro_list)
                    {
                        foreach (var listTo in pro_listTo)
                        {
                            if (list.buildingname == listTo.buildingname)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }
                }
            }
            return flag;
        }

        public int IsProjectExist(int cityId, int areaId, int fxtCompanyId, string projectName)
        {
            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            if (dt == null) return -1;

            var companyId = string.IsNullOrEmpty(dt.ShowCompanyId) ? fxtCompanyId.ToString() : dt.ShowCompanyId;
            var projectTable = dt.projecttable;

            var strsql = @"select p.ProjectId from " + projectTable + @" p with(nolock) where ProjectName=@ProjectName and Valid = 1
                                  and AreaId=@AreaId  and CityId=@CityId and p.FxtCompanyId in (" + companyId + @")
                                  and not exists (select ProjectId from " + projectTable + @"_sub ps with(nolock) 
                                  where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId 
                                  and ps.CityId=p.CityId and ps.AreaId=p.AreaId ) 
                            union 
                           select ProjectId from " + projectTable + @"_sub p with(nolock) where ProjectName=@ProjectName and Valid = 1 and AreaId=@AreaId  and CityId=@CityId and p.Fxt_CompanyId=@FxtCompanyId";

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<int>(strsql, new { ProjectName = projectName, AreaId = areaId, CityId = cityId, FxtCompanyId = fxtCompanyId }).AsQueryable().FirstOrDefault();

            }
        }

        public DAT_Project GetSingleProject(int projectId, int cityId, int fxtCompanyId)
        {
            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            if (dt == null) return new DAT_Project();

            var companyId = string.IsNullOrEmpty(dt.ShowCompanyId) ? fxtCompanyId.ToString() : dt.ShowCompanyId;
            var projectTable = dt.projecttable;

            var strsql = @"select p.BuildingTypeCode from " + projectTable + @" p with(nolock) where projectId=@projectId 
                                  and CityId=@CityId and p.FxtCompanyId in (" + companyId + @")
                                  and not exists (select ProjectId from " + projectTable + @"_sub ps with(nolock) 
                                  where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId 
                                  and ps.CityId=p.CityId) 
                            union 
                           select p.BuildingTypeCode from " + projectTable + @"_sub p with(nolock) where projectId=@projectId  and CityId=@CityId and p.Fxt_CompanyId=@FxtCompanyId";

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<DAT_Project>(strsql, new { projectId, CityId = cityId, FxtCompanyId = fxtCompanyId }).AsQueryable().FirstOrDefault();

            }
        }

        public int IsExistsProjectOnEdit(int cityId, int fxtCompanyId, int areaId, int projectId, string projectName)
        {
            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            if (dt == null) return 0;

            var companyId = string.IsNullOrEmpty(dt.ShowCompanyId) ? fxtCompanyId.ToString() : dt.ShowCompanyId;
            var projectTable = dt.projecttable;

            var strSql = @"SELECT p.projectId
                        FROM " + projectTable + @" P WITH (NOLOCK)
                        WHERE P.CityId = @cityId
	                        AND p.AreaId = @areaId
	                        AND p.valid = 1
	                        AND p.FxtCompanyId IN (" + companyId + @")
	                        AND p.projectName = @projectName
	                        AND p.projectId != @projectId
	                        AND NOT EXISTS (
		                        SELECT ProjectId
		                        FROM " + projectTable + @"_sub ps WITH (NOLOCK)
		                        WHERE p.ProjectId = ps.ProjectId
			                        AND ps.Fxt_CompanyId = @fxtCompanyId
			                        AND ps.CityId = p.CityId
		                        )

                        UNION

                        SELECT p.projectId
                        FROM " + projectTable + @"_sub P WITH (NOLOCK) 
                        WHERE P.CityId = @cityId
	                        AND p.AreaId = @areaId
	                        AND p.valid = 1
	                        AND p.Fxt_CompanyId = @fxtCompanyId
	                        AND p.projectName = @projectName
	                        AND p.projectId != @projectId";

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<int>(strSql, new { projectId, projectName, cityId, fxtCompanyId, areaId }).AsQueryable().FirstOrDefault();
            }
        }

        //public int AddProjectSearch(string tablename, int cityid, int fxtcompanyid, int projectId, string projectName, string address, string otherName, string pinYin, string pinYinAll)
        //{
        //    //先删除，后新增
        //    string deletesql = "delete from " + tablename + @" WITH (ROWLOCK) WHERE ProjectId=@ProjectId and CityId=@CityId and FxtCompanyId =@fxtcompanyid";
        //    string insertsql = @"insert into " + tablename + @" WITH (ROWLOCK) (ProjectId,ProjectName,Address,OtherName,PinYin,PinYinAll,CityId,FxtcompanyId) VALUES (@ProjectId,@ProjectName,@Address,@OtherName,@PinYin,@PinYinAll,@CityId,@FxtcompanyId)";
        //    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //    {
        //        var d = con.Execute(deletesql, new { projectId, cityid, fxtcompanyid });
        //        var r = con.Execute(insertsql, new { projectId, projectName, address, otherName, pinYin, pinYinAll, cityid, fxtcompanyid });
        //        return r;
        //    }
        //}

        //public int AddProjectSubSearch(string tablename, int cityid, int fxtcompanyid, int projectId, string projectName, string address, string otherName, string pinYin, string pinYinAll)
        //{
        //    //先删除，后新增
        //    string deletesql = "delete from " + tablename + @" WITH (ROWLOCK) WHERE ProjectId=@ProjectId and CityId=@CityId and FxtCompanyId =@fxtcompanyid";
        //    string insertsql = @"insert into " + tablename + @" WITH (ROWLOCK) (ProjectId,ProjectName,Address,OtherName,PinYin,PinYinAll,CityId,FxtcompanyId) VALUES (@ProjectId,@ProjectName,@Address,@OtherName,@PinYin,@PinYinAll,@CityId,@FxtcompanyId)";
        //    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //    {
        //        var d = con.Execute(deletesql, new { projectId, cityid, fxtcompanyid });
        //        var r = con.Execute(insertsql, new { projectId, projectName, address, otherName, pinYin, pinYinAll, cityid, fxtcompanyid });
        //        return r;
        //    }
        //}


        public IQueryable<DAT_Project> GetProjectIds(int cityId, int fxtCompanyId)
        {

            var cityTable = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            if (cityTable == null) return new List<DAT_Project>().AsQueryable();

            var ptable = cityTable.projecttable;
            var companyIds = cityTable.ShowCompanyId;

            var strsql = @" select [ProjectId],ProjectName,AreaId from " + ptable + @" p with(nolock) 
                    	        where p.Valid=1 
                                and p.CityID=@CityID  
                                and  p.FxtCompanyId in(" + companyIds + @") 
                                and not exists (select ProjectId from " + ptable + @"_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId and ps.CityId=p.CityId) 
                    	        union 
                    	        select [ProjectId],ProjectName,AreaId from " + ptable + @"_sub p with(nolock) 
                    	        where p.Valid=1 
                                and p.CityID=@CityID 
                                and p.Fxt_CompanyId=@FxtCompanyId ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Project>(strsql, new { cityId, fxtCompanyId }).AsQueryable();

            }
        }

        public List<Project_BHCount> GetBHCount(List<int> bhareaname, int CityId, int FxtCompanyId)
        {
            string pwhere = string.Empty;
            //行政区
            if (bhareaname != null && bhareaname.Count > 0 && !bhareaname.Contains(-1))
            {
                var areanames = string.Join(",", bhareaname);
                pwhere += " and p.AreaId in (" + areanames + ")";
            }

            var dt = GetCityTable(CityId, FxtCompanyId).FirstOrDefault();
            if (dt != null)
            {
                var ptable = dt.projecttable;
                var comId = dt.ShowCompanyId ?? FxtCompanyId.ToString();

                var strsql = @"
select 
	(select AreaName from FxtDataCenter.dbo.SYS_Area with(nolock) where AreaId = p.AreaId) as AreaName
    ,P.ProjectId
    ,ProjectName
	,ISNULL(PC.BuildingNumber,0) as BuildingNum
	,ISNULL(PC.HouseNumber,0) as HouseName
    ,Creator
    ,P.FxtCompanyId
from (
	select
		ProjectId,ProjectName,AreaId,Creator,FxtCompanyId
	from " + ptable + @" p with(nolock)
	where not exists(
		select ProjectId from " + ptable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.Fxt_CompanyId = @fxtcompanyid
		and ps.CityID = @cityid
	)
	and p.Valid = 1
	and p.FxtCompanyId in (" + comId + @")
	and p.CityID = @cityid
	union
	select 
		ProjectId,ProjectName,AreaId,Creator,Fxt_CompanyId
	from " + ptable + @"_sub p with(nolock)
	where p.Valid = 1
	and p.Fxt_CompanyId = @fxtcompanyid
	and p.CityID = @cityid
)p
left join (
	select * from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK)
	where c.cityId = @cityid and c.fxtcompanyId = @fxtcompanyid and c.BuildingId = 0
)PC on p.projectId = PC.ProjectId
where 1 = 1 " + pwhere;

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var projectList = con.Query<Project_BHCount>(strsql, new { CityId, FxtCompanyId }).AsQueryable();
                    return projectList.ToList();
                }
            }
            return new List<Project_BHCount>();
        }

        public List<Project_PPCount> GetPPCount(List<int> ppareaname, string ProjectName, int CityId, int FxtCompanyId)
        {
            string pwhere = string.Empty;
            //行政区
            if (ppareaname != null && ppareaname.Count > 0 && !ppareaname.Contains(-1))
            {
                var areanames = string.Join(",", ppareaname);
                pwhere += " and AreaId in (" + areanames + ")";
            }
            //楼盘名称
            if (!string.IsNullOrEmpty(ProjectName))
            {
                pwhere += " and ProjectName like '%" + ProjectName.Trim() + "%'";
            }

            var dt = GetCityTable(CityId, FxtCompanyId).FirstOrDefault();
            if (dt != null)
            {
                var ptable = "FXTProject." + dt.projecttable;
                var comId = dt.ShowCompanyId ?? FxtCompanyId.ToString();

                var strsql = @"
select
	ProjectId
	,AreaName
	,ProjectName
    ,Creator
    ,FxtCompanyId
	,SUM(logo + 标准层平面图 + 户型图 + 实景图 + 外立面图 + 位置图 + 效果图 + 总平面图 + 其他) as PicCount
	,SUM(logo) as PicLogoNum
	,SUM(标准层平面图) as PicBZCNum
	,SUM(户型图) as PicHXNum
	,SUM(实景图) as PicSJNum
	,SUM(外立面图) as PicWLMNum
	,SUM(位置图) as PicWZNum
	,SUM(效果图) as PicXGNum
	,SUM(总平面图) as PicZPMNum
	,SUM(其他) as PicQTNum
from (
	select 
		AreaName
		,ProjectId
		,ProjectName
		,Creator
        ,FxtCompanyId
		,case when PhotoTypeCode = 2009001 then totalnum else 0 end as logo
		,case when PhotoTypeCode = 2009002 then totalnum else 0 end as 标准层平面图
		,case when PhotoTypeCode = 2009003 then totalnum else 0 end as 户型图
		,case when PhotoTypeCode = 2009004 then totalnum else 0 end as 实景图
		,case when PhotoTypeCode = 2009005 then totalnum else 0 end as 外立面图
		,case when PhotoTypeCode = 2009006 then totalnum else 0 end as 位置图
		,case when PhotoTypeCode = 2009007 then totalnum else 0 end as 效果图
		,case when PhotoTypeCode = 2009008 then totalnum else 0 end as 总平面图
		,case when PhotoTypeCode = 2009009 then totalnum else 0 end as 其他
	from (
		select 
			ProjectId
			,(select AreaName from FxtDataCenter.dbo.SYS_Area with(nolock) where AreaId = T1.AreaID) as AreaName
			,ProjectName
			,Creator
            ,FxtCompanyId
			,PhotoTypeCode
			,(select CodeName from FxtDataCenter.dbo.SYS_Code with(nolock) where Code = T1.PhotoTypeCode) as phototypecodename
			,totalnum
		from (
			select 
				ProjectId,AreaID,ProjectName,PhotoTypeCode,Creator,FxtCompanyId,COUNT(*) as totalnum
			from (
				select 
					P.ProjectId
					,p.CityID
					,p.AreaID
					,p.ProjectName
					,p.Creator
                    ,p.FxtCompanyId
					,Photo.Id
					,Photo.PhotoTypeCode
				from (
					select 
						ProjectId,ProjectName,CityID,AreaID,Creator,FxtCompanyId
					from " + ptable + @" p with(nolock)
					where not exists(
						select ProjectId from " + ptable + @"_sub ps with(nolock)
						where ps.ProjectId = p.ProjectId
						and ps.CityID = @cityid
						and ps.Fxt_CompanyId = @fxtcompanyid
					)
					and Valid = 1
					and CityID = @cityid
					and FxtCompanyId in (" + comId + @")
					union
					select 
						ProjectId,ProjectName,CityID,AreaID,Creator,Fxt_CompanyId
					from " + ptable + @"_sub p with(nolock)
					where Valid = 1
					and CityID = @cityid
					and Fxt_CompanyId = @fxtcompanyid
				)P
				left join (
					select * from FXTProject.dbo.LNK_P_Photo p with(nolock)
					where not exists(
						select * from FXTProject.dbo.LNK_P_Photo_sub ps with(nolock)
						where ps.Id = p.Id 
						and ps.CityId = @cityid
						and ps.FxtCompanyId = @fxtcompanyid
					)
					and Valid = 1
					and CityId = @cityid
					and FxtCompanyId in (" + comId + @")
					union
					select * from FXTProject.dbo.LNK_P_Photo_sub p with(nolock)
					where Valid = 1
					and CityId = @cityid
					and FxtCompanyId = @fxtcompanyid
				)Photo on p.ProjectId = Photo.ProjectId and p.CityID = Photo.CityId
                where 1 = 1 " + pwhere + @"
			)T
			group by ProjectId,AreaID,ProjectName,PhotoTypeCode,Creator,FxtCompanyId
		)T1
	)T
)T1
group by AreaName
		,ProjectId
		,ProjectName
        ,Creator
        ,FxtCompanyId
order by AreaName
		,ProjectId
		,ProjectName";

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var projectList = con.Query<Project_PPCount>(strsql, new { CityId, FxtCompanyId }).AsQueryable();
                    return projectList.ToList();
                }
            }
            return new List<Project_PPCount>();
        }

        /// <summary>
        /// 楼盘合并成功后，复制案例
        /// </summary>
        public int MergerProjectCase(int projectId, int projectIdTo, int cityID, int fxtcompanyid, string userName)
        {
            try
            {
                var dt = GetCityTable(cityID, fxtcompanyid).FirstOrDefault();
                if (dt != null)
                {
                    int result = 0;
                    string ctable = "FXTProject." + dt.casetable;
                    string caseCompanyid = dt.CaseCompanyId;
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        string sql_str = @"
insert into " + ctable + @"(ProjectId,BuildingId,HouseId,CompanyId,CaseDate,PurposeCode,FloorNumber,BuildingName,HouseNo,BuildingArea,UsableArea,FrontCode,UnitPrice,MoneyUnitCode,SightCode,CaseTypeCode,StructureCode,BuildingTypeCode,HouseTypeCode,CreateDate,Creator,Remark,TotalPrice,OldID,CityID,Valid,FXTCompanyId,TotalFloor,RemainYear,Depreciation,FitmentCode,SurveyId,SourceName,SourceLink,SourcePhone,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao)
select @ProjectId,BuildingId,HouseId,CompanyId,CaseDate,PurposeCode,FloorNumber,BuildingName,HouseNo,BuildingArea,UsableArea,FrontCode,UnitPrice,MoneyUnitCode,SightCode,CaseTypeCode,StructureCode,BuildingTypeCode,HouseTypeCode,GETDATE() as CreateDate,@Creator,Remark,TotalPrice,OldID,CityID,Valid,@FXTCompanyId,TotalFloor,RemainYear,Depreciation,FitmentCode,SurveyId,SourceName,SourceLink,SourcePhone,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao
from " + ctable + @"
where CityID = @cityID
and ProjectId = @projectIdTo
and Valid = 1
and FXTCompanyId in (" + caseCompanyid + @")";
                        result = con.Execute(sql_str, new { ProjectId = projectId, projectIdTo = projectIdTo, CityId = cityID, FxtCompanyId = fxtcompanyid, Creator = userName });
                    }
                    return result;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 楼盘合并成功后，复制楼盘图片
        /// </summary>
        public int MergerProjectPhoto(int projectId, int projectidTo, int fxtcompanyid, int cityId, int currFxtCompanyId)
        {
            try
            {
                int result = 0;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    string sql_str = @"
insert into FXTProject.dbo.LNK_P_Photo(ProjectId,PhotoTypeCode,[Path],PhotoDate,PhotoName,CityId,Valid,FxtCompanyId,BuildingId,X,Y)
SELECT @ProjectId as ProjectId,PhotoTypeCode,[Path],GETDATE() as PhotoDate,PhotoName,CityId,1 as Valid,@CurrFxtCompanyId as FxtCompanyId,BuildingId,X,Y
FROM FXTProject.dbo.LNK_P_Photo p WITH (NOLOCK)
WHERE valid = 1
	AND ProjectId = @ProjectidTo
	and BuildingId = 0
	AND CityId = @CityId
	AND NOT EXISTS (
		SELECT ps.Id
		FROM FXTProject.dbo.LNK_P_Photo_sub ps WITH (NOLOCK)
		WHERE p.Id = ps.Id
			AND ps.FxtCompanyId = @FxtCompanyId
			AND ps.CityId = p.CityId
		)
UNION
SELECT @ProjectId as ProjectId,PhotoTypeCode,[Path],GETDATE() as PhotoDate,PhotoName,CityId,1 as Valid,@CurrFxtCompanyId as FxtCompanyId,BuildingId,X,Y
FROM FXTProject.dbo.LNK_P_Photo_sub p WITH (NOLOCK)
WHERE valid = 1
	AND ProjectId = @ProjectidTo
	and BuildingId = 0
	AND CityId = @CityId
	AND p.FxtCompanyId = @FxtCompanyId";
                    result = con.Execute(sql_str, new { ProjectId = projectId, ProjectidTo = projectidTo, CityId = cityId, FxtCompanyId = fxtcompanyid, CurrFxtCompanyId = currFxtCompanyId });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
