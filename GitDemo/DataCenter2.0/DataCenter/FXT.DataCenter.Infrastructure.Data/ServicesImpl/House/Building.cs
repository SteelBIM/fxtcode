using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.QueryObjects;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Infrastructure.Common.Common;
using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    /// <summary>
    /// 楼栋
    /// </summary>
    public class DAT_BuildingDAL : IDAT_Building
    {
        private int FxtComId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);//房讯通

        private IQueryable<SYS_City_Table> GetCityTable(int CityId, int FxtCompanyId = 0)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                if (FxtCompanyId > 0)
                {
                    string strsql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.ShowCompanyId FROM [dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and s.FxtCompanyId=@FxtCompanyId and typecode= 1003002";
                    return con.Query<SYS_City_Table>(strsql, new { CityId = CityId, FxtCompanyId = FxtCompanyId }).AsQueryable();
                }
                else
                {
                    string strsql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable] FROM [dbo].[SYS_City_Table] with(nolock) where CityId=@CityId";
                    return con.Query<SYS_City_Table>(strsql, new { CityId = CityId }).AsQueryable();
                }
            }

        }

        public IQueryable<DAT_Building> GetBuildNameList(int cityId, int projectId, int fxtCompanyId)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {

                var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
                if (dt == null) return new List<DAT_Building>().AsQueryable();

                string bTable = dt.buildingtable,
                comId = dt.ShowCompanyId,
                sql = @"SELECT b.BuildingId, [BuildingName]
                        FROM " + bTable + @" b WITH (NOLOCK)
                        WHERE b.CityId = @CityId
	                        AND b.ProjectId = @ProjectId
	                        AND b.FxtCompanyId IN (" + comId + @")
	                        AND NOT EXISTS (
		                        SELECT BuildingId
		                        FROM " + bTable + @"_sub bs WITH (NOLOCK)
		                        WHERE bs.[ProjectId] = @ProjectId
			                        AND b.BuildingId = bs.BuildingId
			                        AND bs.Fxt_CompanyId = @FxtCompanyId
			                        AND bs.CityId = b.CityId
		                        )
	                        AND b.Valid = 1

                        UNION

                        SELECT b.BuildingId, b.[BuildingName]
                        FROM " + bTable + @"_sub b WITH (NOLOCK)
                        WHERE b.CityId = @CityId
	                        AND b.ProjectId = @ProjectId
	                        AND b.Fxt_CompanyId = @FxtCompanyId
	                        AND b.Valid = 1";
                var reslut = con.Query<DAT_Building>(sql, new { CityId = cityId, ProjectId = projectId, FxtCompanyId = fxtCompanyId }).AsQueryable();

                return reslut;
            }
        }

        public IQueryable<DAT_Building> GetBuildingInfo(int cityId, int projectId, int fxtCompanyId, int buildId = 0)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {

                //int r = 0;
                var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
                if (dt != null)
                {
                    string bTable = dt.buildingtable,
                     pTable = dt.projecttable,
                     comId = dt.ShowCompanyId,
                     where = "";
                    if (buildId > 0)
                    {
                        where = "  and b.BuildingId=@BuildingId";
                    }
                    var sql = @"
SELECT b.BuildingId,b.[ProjectId],b.[BuildingName],b.[PurposeCode],b.[BuildingTypeCode],b.[BuildDate],b.[SaleDate],b.[SalePrice],b.[UnitsNumber],b.[TotalFloor],b.[TotalNumber],b.[TotalBuildArea],b.IsEValue,b.AveragePrice,b.[SaveUser],b.[FxtCompanyId],b.CityId,b.AverageFloor AS AverageFloor,b.Weight,b.Wall,b.IsElevator,b.LocationCode,b.SightCode,b.FrontCode,b.StructureWeight,b.BuildingTypeWeight,b.YearWeight,b.PurposeWeight,b.LocationWeight,b.SightWeight,b.FrontWeight,b.ElevatorRate,b.StructureCode,b.FloorHigh,b.X,b.Y,b.BHouseTypeCode,b.BHouseTypeWeight,b.Creator,b.Distance,b.DistanceWeight,b.BaseMent,b.PriceDetail PriceDetail,b.IsYard,b.YardWeight,b.ElevatorRateWeight,b.Remark,b.othername othername,b.joindate joindate,b.licencedate licencedate,b.salelicence salelicence,b.subaverageprice subaverageprice,b.Doorplate,b.RightCode,b.IsVirtual,b.FloorSpread,b.PodiumBuildingFloor,b.PodiumBuildingArea,b.TowerBuildingArea,b.BasementArea,b.BasementPurpose,b.HouseNumber,b.HouseArea,b.OtherNumber,b.OtherArea,b.innerFitmentCode,b.FloorHouseNumber,b.LiftNumber,b.LiftBrand,b.Facilities,b.PipelineGasCode,b.HeatingModeCode,b.WallTypeCode,b.MaintenanceCode, b.Weight as PriceWeight,b.isTotalFloor
    ,b.FxtCompanyId as belongcompanyid
    ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=b.FxtCompanyId)
FROM " + bTable + @" b WITH (NOLOCK)
WHERE b.CityId = @CityId
	AND b.ProjectId = @ProjectId
	AND b.Valid = 1
    AND b.FxtCompanyId IN (" + comId + @")
	AND NOT EXISTS (
		SELECT BuildingId
		FROM " + bTable + @"_sub bs WITH (NOLOCK)
		WHERE bs.[ProjectId] = @ProjectId
			AND b.BuildingId = bs.BuildingId
			AND bs.Fxt_CompanyId = @FxtCompanyId
			AND bs.CityId = b.CityId
		) " + where + @"
UNION
SELECT b.BuildingId,b.[ProjectId],b.[BuildingName],b.[PurposeCode],b.[BuildingTypeCode],b.[BuildDate],b.[SaleDate],b.[SalePrice],b.[UnitsNumber],b.[TotalFloor],b.[TotalNumber],b.[TotalBuildArea],b.IsEValue,b.AveragePrice,b.[SaveUser],b.[Fxt_CompanyId],b.CityId,b.AverageFloor AS AverageFloor,b.Weight,b.Wall,b.IsElevator,b.LocationCode,b.SightCode,b.FrontCode,b.StructureWeight,b.BuildingTypeWeight,b.YearWeight,b.PurposeWeight,b.LocationWeight,b.SightWeight,b.FrontWeight,b.ElevatorRate,b.StructureCode,b.FloorHigh,b.X,b.Y,b.BHouseTypeCode,b.BHouseTypeWeight,b.Creator,b.Distance,b.DistanceWeight,b.BaseMent,b.PriceDetail PriceDetail,b.IsYard,b.YardWeight,b.ElevatorRateWeight,b.Remark,b.othername othername,b.joindate joindate,b.licencedate licencedate,b.salelicence salelicence,b.subaverageprice subaverageprice,b.Doorplate,b.RightCode,b.IsVirtual,b.FloorSpread,b.PodiumBuildingFloor,b.PodiumBuildingArea,b.TowerBuildingArea,b.BasementArea,b.BasementPurpose,b.HouseNumber,b.HouseArea,b.OtherNumber,b.OtherArea,b.innerFitmentCode,b.FloorHouseNumber,b.LiftNumber,b.LiftBrand,b.Facilities,b.PipelineGasCode,b.HeatingModeCode,b.WallTypeCode,b.MaintenanceCode,b.Weight as PriceWeight,b.isTotalFloor
    ,bi.FxtCompanyId as belongcompanyid
    ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=bi.FxtCompanyId)
FROM " + bTable + "_sub b WITH (NOLOCK) left join " + bTable + @" bi WITH (NOLOCK) on b.BuildingId = bi.BuildingId
WHERE b.CityId = @CityId
	AND b.ProjectId = @ProjectId
	AND b.Fxt_CompanyId = @FxtCompanyId
	AND b.Valid = 1 " + where;

                    var reslut = con.Query<DAT_Building>(sql, new { CityID = cityId, ProjectId = projectId, FxtCompanyId = fxtCompanyId, BuildingId = buildId }).AsQueryable();
                    return reslut;

                }
                return new List<DAT_Building>().AsQueryable();

            }
        }

        public IQueryable<BuildStatiParam> GetBuildingInfo(int cityId, BuildStatiParam parame, int fxtCompanyId)
        {
            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            var bTable = dt.buildingtable;
            var comId = dt.ShowCompanyId ?? parame.FxtCompanyId.ToString();
            var where = "";

            if (!string.IsNullOrEmpty(parame.BuildingName))
            {
                where += " and b.BuildingName like @BuildingName";
            }
            if (parame.PurposeCode > 0)
            {
                where += " and b.PurposeCode=@PurposeCode";
            }
            if (parame.BuildingTypeCode > 0)
            {
                where += " and b.BuildingTypeCode=@BuildingTypeCode";
            }
            if (parame.IsEValue > 0)
            {
                where += " and b.IsEValue=@IsEValue";
            }
            if (parame.BuildSaleDate != null)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(parame.BuildSaleDate.ToString(), "")))
                {
                    where += "  and b.SaleDate>@BuildSaleDate";
                }

            }
            if (parame.BuildSaleDateTo != null)
            {
                if (!string.IsNullOrEmpty(TryParseHelper.StrToDateTime(parame.BuildSaleDateTo.ToString(), "")))
                {
                    where += "  and b.SaleDate<@BuildSaleDateTo";
                }

            }

            var sql = @"
select *,(select top 1 CityName from FxtDataCenter.dbo.SYS_City with(nolock) where CityId = b.CityID) as CityName
,(select top 1 codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = b.HeatingModeCode) as HeatingModeName
,(select top 1 codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = b.PipelineGasCode) as PipelineGasName
,(select top 1 codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = b.WallTypeCode) as WallTypeName
,(select top 1 codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = b.MaintenanceCode) as MaintenanceCodeName
,(select top 1 ISNULL(c.HouseNumber,0) from FXTProject.dbo.DAT_P_B_H_Count c WITH (NOLOCK) where c.projectId = @ProjectId and c.cityId = @CityId and c.fxtcompanyId = @FxtCompanyId and c.BuildingId = b.BuildingId) as houseNum
from (
	SELECT BuildingId,BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight
    ,(case when IsEValue = 1 then 1 else 0 end) as IsEValue
    ,CityID,CreateTime,OldId,Valid,SalePrice,SaveDateTime,SaveUser,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail
    ,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose
    ,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor
    ,FxtCompanyId
    ,FxtCompanyId as belongcompanyid
    ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=FxtCompanyId)
	FROM " + bTable + @" b WITH (NOLOCK)
	WHERE b.CityId = @CityId
		AND b.ProjectId = @ProjectId
		AND b.Valid = 1 
		AND b.FxtCompanyId IN (" + comId + @")
		AND NOT EXISTS (
			SELECT BuildingId
			FROM " + bTable + @"_sub bs WITH (NOLOCK)
			WHERE bs.[ProjectId] = @ProjectId
				AND b.BuildingId = bs.BuildingId
				AND bs.Fxt_CompanyId = @FxtCompanyId
				AND bs.CityId = b.CityId
			) 
	UNION
	SELECT b.BuildingId,b.BuildingName,b.ProjectId,b.PurposeCode,b.StructureCode,b.BuildingTypeCode,b.TotalFloor,b.FloorHigh,b.SaleLicence,b.ElevatorRate,b.UnitsNumber,b.TotalNumber,b.TotalBuildArea,b.BuildDate,b.SaleDate,b.AveragePrice,b.AverageFloor,b.JoinDate,b.LicenceDate,b.OtherName,b.Weight
    ,(case when b.IsEValue = 1 then 1 else 0 end) as IsEValue
    ,b.CityID,b.CreateTime,b.OldId,b.Valid,b.SalePrice,b.SaveDateTime,b.SaveUser,b.LocationCode,b.SightCode,b.FrontCode,b.StructureWeight,b.BuildingTypeWeight,b.YearWeight,b.PurposeWeight,b.LocationWeight,b.SightWeight,b.FrontWeight,b.X,b.Y,b.XYScale,b.Wall,b.IsElevator,b.SubAveragePrice,b.PriceDetail,b.BHouseTypeCode,b.BHouseTypeWeight,b.Creator,b.Distance,b.DistanceWeight,b.basement,b.Remark,b.ElevatorRateWeight,b.IsYard,b.YardWeight,b.Doorplate,b.RightCode,b.IsVirtual,b.FloorSpread,b.PodiumBuildingFloor,b.PodiumBuildingArea,b.TowerBuildingArea,b.BasementArea,b.BasementPurpose,b.HouseNumber,b.HouseArea,b.OtherNumber,b.OtherArea,b.innerFitmentCode,b.FloorHouseNumber,b.LiftNumber,b.LiftBrand,b.Facilities,b.PipelineGasCode,b.HeatingModeCode,b.WallTypeCode,b.MaintenanceCode,b.isTotalFloor
    ,b.Fxt_CompanyId,bi.fxtcompanyid as belongcompanyid
    ,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=bi.fxtcompanyid)
	FROM " + bTable + @"_sub b WITH (NOLOCK) left join " + bTable + @" bi WITH (NOLOCK) on b.BuildingId = bi.BuildingId
	WHERE b.CityId = @CityId
		AND b.ProjectId = @ProjectId
		AND b.Fxt_CompanyId = @FxtCompanyId
		AND b.Valid = 1 
)b
where 1 = 1" + where;

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<BuildStatiParam>(sql, new
                {
                    ProjectId = parame.ProjectId,
                    CityId = cityId,
                    FxtCompanyId = fxtCompanyId,
                    BuildingName = "%" + parame.BuildingName + "%",
                    PurposeCode = parame.PurposeCode,
                    BuildingTypeCode = parame.BuildingTypeCode,
                    IsEValue = parame.IsEValue,
                    BuildSaleDate = parame.BuildSaleDate + " 00:00:00",
                    BuildSaleDateTo = parame.BuildSaleDateTo + " 23:59:59"
                }).AsQueryable();
            };
        }

        // 获取楼栋Info  build_list:原始楼栋集合 比如:125,126,127
        private IQueryable<DAT_Building> GetBuildingInfo(int CityId, string build_list, int fxtCompanyId)
        {

            //int r = 0;
            var dt = GetCityTable(CityId, fxtCompanyId).FirstOrDefault();
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                if (dt != null)
                {
                    string b_table = dt.buildingtable,
                    ComId = dt.ShowCompanyId,
                    h_table = dt.housetable,
                    sql = @"select BuildingId, [ProjectId],[BuildingName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],
                                          [TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[FxtCompanyId],CityId,AverageFloor,
                                          Weight,Wall,IsElevator,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,
                                          LocationWeight,SightWeight,FrontWeight,ElevatorRate,StructureCode,FloorHigh,X,Y,BHouseTypeCode,BHouseTypeWeight,Creator,
                                          Distance,DistanceWeight,BaseMent,PriceDetail,IsYard,YardWeight,ElevatorRateWeight,Remark,
                                          b.Doorplate, b.RightCode,  b.IsVirtual,  b.FloorSpread,  b.PodiumBuildingFloor,  b.PodiumBuildingArea,  b.TowerBuildingArea, 
                                          b.BasementArea, b.BasementPurpose,  b.HouseNumber,  b.HouseArea,  b.OtherNumber,  b.OtherArea,  b.innerFitmentCode,  b.FloorHouseNumber,
                                          b.LiftNumber, b.LiftBrand,  b.Facilities,  b.PipelineGasCode,  b.HeatingModeCode,  b.WallTypeCode ,b.MaintenanceCode,b.isTotalFloor
                                    from " + b_table + @" b with(nolock)  
                                    where b.CityId=@CityId and  b.BuildingId in (" + build_list + @") and b.Valid=1 and b.FxtCompanyId in(" + ComId + @") 
                                          and not exists (select BuildingId from " + b_table + @"_sub bs with(nolock) 
                                          where bs.[BuildingId] in (" + build_list + @") and b.BuildingId=bs.BuildingId and bs.Fxt_CompanyId=@FxtCompanyId and bs.CityId=b.CityId)  
                                    union  
                                    select BuildingId,[ProjectId],[BuildingName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],
                                          [TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[Fxt_CompanyId],CityId,AverageFloor,
                                          Weight,Wall,IsElevator,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,
                                          LocationWeight,SightWeight,FrontWeight,ElevatorRate,StructureCode,FloorHigh,X,Y,BHouseTypeCode,BHouseTypeWeight,Creator,
                                          Distance,DistanceWeight,BaseMent,PriceDetail,IsYard,YardWeight,ElevatorRateWeight,Remark,
                                          b.Doorplate, b.RightCode,  b.IsVirtual,  b.FloorSpread,  b.PodiumBuildingFloor,  b.PodiumBuildingArea,  b.TowerBuildingArea, 
                                          b.BasementArea, b.BasementPurpose,  b.HouseNumber,  b.HouseArea,  b.OtherNumber,  b.OtherArea,  b.innerFitmentCode,  b.FloorHouseNumber,
                                          b.LiftNumber, b.LiftBrand,  b.Facilities,  b.PipelineGasCode,  b.HeatingModeCode,  b.WallTypeCode ,b.MaintenanceCode,b.isTotalFloor
                                    from " + b_table + @"_sub b with(nolock) 
                                    where b.CityId=@CityId and b.BuildingId in (" + build_list + @") and b.Fxt_CompanyId=@FxtCompanyId and b.Valid=1";

                    var reslut = con.Query<DAT_Building>(sql, new { CityID = CityId, FxtCompanyId = fxtCompanyId }).AsQueryable();
                    return reslut;
                }
                return new List<DAT_Building>().AsQueryable();

            }
        }

        // 添加楼栋  0:没有开通权限  -1:原始楼盘暂无可以复制的楼栋  -2:添加楼栋失败 -3:目标楼盘存在数据，不能复制楼盘;  -4://程序异常
        public int AddBuilding(int CityId, int ProjectId, int FxtCompanyId, string SaveUser, int ProjectIdTo, IEnumerable<DAT_Building> b_list, int currfxtcompanyid)
        {

            int returnVal = 0;
            var result = GetCityTable(CityId).FirstOrDefault();
            try
            {
                if (result == null) return returnVal;
                var bTable = result.buildingtable;
                var hTable = result.housetable;
                returnVal = FeachAddBuild(CityId, FxtCompanyId, SaveUser, ProjectIdTo, bTable, hTable, b_list, currfxtcompanyid);
                return returnVal;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        // 循环添加楼栋、房号
        private int FeachAddBuild(int CityId, int FxtCompanyId, string SaveUser, int ProjectIdTo, string b_table, string h_table, IEnumerable<DAT_Building> b_list, int currfxtcompanyid)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var house = new DAT_HouseDAL();
                    if (b_list != null && b_list.Count() > 0)
                    {
                        foreach (var item in b_list)
                        {
                            item.projectid = ProjectIdTo;
                            item.saveuser = item.creator = SaveUser;
                            item.fxtcompanyid = currfxtcompanyid;
                            item.cityid = CityId;
                            int buil_id = AddBuild(item);
                            //添加楼栋成功
                            if (buil_id > 0)
                            {
                                //dynamic identity = con.Query("SELECT max(BuildingId) as Id from " + b_table + " with(nolock)").Single();
                                //int buil_id = Convert.ToInt32(number);
                                //楼栋复制成功后，把楼栋图片带过去。
                                AddBuildingPhoto(ProjectIdTo, buil_id, currfxtcompanyid, item.buildingid, FxtCompanyId, CityId);

                                //获取原始楼盘下的楼栋下的房号Sql----------------------
                                var h_list = house.GetHouseInfoByBuild(item.buildingid, CityId, currfxtcompanyid);
                                ////目标房号
                                //var h_list_hou = house.GetHouseInfoByBuild(buil_id, CityId, FxtCompanyId);
                                if (h_list != null && h_list.Count() > 0)
                                {
                                    foreach (var h_item in h_list)
                                    {
                                        h_item.buildingid = buil_id;
                                        h_item.fxtcompanyid = currfxtcompanyid;
                                        h_item.createtime = DateTime.Now;
                                        h_item.creator = SaveUser;
                                        house.AddHouse(h_item);
                                    }
                                }
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        return 1;
                    }
                    else
                    {
                        //return 0;
                        return 1;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // 添加楼栋
        public int AddBuild(DAT_Building item)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var ctable = GetCityTable(item.cityid).FirstOrDefault();
                string comId = ctable.ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = item.fxtcompanyid.ToString();
                //判断是否原来就存在的
                string strsql = @"select B.BuildingId from " + ctable.buildingtable + @" b with(nolock) where BuildingName=@BuildingName and ProjectId=@ProjectId and valid=@valid 
                                and CityId=@CityId and not exists (select BuildingId from " + ctable.buildingtable + @"_sub bs with(nolock) where b.BuildingId=bs.BuildingId 
                                and bs.Fxt_CompanyId=@FxtCompanyId and bs.CityId=b.CityId and bs.ProjectId=b.ProjectId and bs.BuildingName=b.BuildingName) and b.FxtCompanyId in(" + comId + @")  
                                union 
                             select BuildingId from " + ctable.buildingtable + @"_sub b with(nolock) where BuildingName=@BuildingName and ProjectId=@ProjectId and valid=@valid 
                                and CityId=@CityId and b.Fxt_CompanyId=@FxtCompanyId";
                var buildList = con.Query<DAT_Building>(strsql, new
                {
                    BuildingName = item.buildingname.Trim(),
                    ProjectId = item.projectid,
                    CityId = item.cityid,
                    FxtCompanyId = item.fxtcompanyid,
                    valid = 0
                });
                if (buildList != null && buildList.Any())
                {
                    int bid = buildList.FirstOrDefault().buildingid;
                    item.buildingid = bid;
                    ModifyBuilding(item, item.fxtcompanyid);
                    return bid;
                }
                var build = con.Query<DAT_Building>(strsql, new
                {
                    BuildingName = item.buildingname.Trim(),
                    ProjectId = item.projectid,
                    CityId = item.cityid,
                    FxtCompanyId = item.fxtcompanyid,
                    valid = 1
                });
                if (build != null && build.Any())
                {
                    return -2;//存在相同的楼栋名称
                }
                #region ------------插入楼栋Sql----------------------
                strsql = "insert into " + ctable.buildingtable + " with(rowlock) ([ProjectId],[BuildingName],[PurposeCode],[BuildingTypeCode],[BuildDate],"
               + "[SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,"
               + "[SaveUser],[FxtCompanyId],CityId,AverageFloor,Weight,Wall,IsElevator,"
               + "LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,"
               + "PurposeWeight,LocationWeight,SightWeight,FrontWeight,ElevatorRate,StructureCode,FloorHigh,X,Y,BHouseTypeCode,"
               + "BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,PriceDetail,IsYard,YardWeight,ElevatorRateWeight,Remark,"
               + " Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, PodiumBuildingArea, TowerBuildingArea, BasementArea, "
               + " BasementPurpose, HouseNumber, HouseArea, OtherNumber, OtherArea, innerFitmentCode, FloorHouseNumber, LiftNumber, LiftBrand,"
               + " Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode,OtherName,JoinDate,LicenceDate,SaleLicence,SubAveragePrice,MaintenanceCode,isTotalFloor) "
               + "VALUES"
               + "(@ProjectId,@BuildingName,@PurposeCode,@BuildingTypeCode,@BuildDate,"
               + " @SaleDate,@SalePrice,@UnitsNumber,@TotalFloor,@TotalNumber,@TotalBuildArea,@IsEValue,@AveragePrice,"
               + " @SaveUser,@FxtCompanyId,@CityId,@AverageFloor,@Weight,@Wall,@IsElevator,"
               + " @LocationCode,@SightCode,@FrontCode,@StructureWeight,@BuildingTypeWeight,@YearWeight,"
               + " @PurposeWeight,@LocationWeight,@SightWeight,@FrontWeight,@ElevatorRate,@StructureCode,@FloorHigh,@X,@Y,@BHouseTypeCode,"
               + " @BHouseTypeWeight,@Creator,@Distance,@DistanceWeight,@BaseMent,@PriceDetail,@IsYard,@YardWeight,@ElevatorRateWeight,@Remark,"
               + " @Doorplate, @RightCode, @IsVirtual, @FloorSpread, @PodiumBuildingFloor, @PodiumBuildingArea, @TowerBuildingArea, @BasementArea, "
               + " @BasementPurpose, @HouseNumber, @HouseArea, @OtherNumber, @OtherArea, @innerFitmentCode, @FloorHouseNumber, @LiftNumber, @LiftBrand,"
               + " @Facilities, @PipelineGasCode, @HeatingModeCode, @WallTypeCode,@OtherName,@JoinDate,@LicenceDate,@SaleLicence,@SubAveragePrice,@MaintenanceCode,@isTotalFloor)";
                #endregion
                int number = con.Execute(strsql, item);
                var buildingid = 0;
                if (number > 0)
                {
                    string buildingidsql = @"select B.BuildingId from " + ctable.buildingtable + @" b with(nolock) where BuildingName=@BuildingName and ProjectId=@ProjectId and valid=@valid 
                                and CityId=@CityId and not exists (select BuildingId from " + ctable.buildingtable + @"_sub bs with(nolock) where b.BuildingId=bs.BuildingId 
                                and bs.Fxt_CompanyId=@FxtCompanyId and bs.CityId=b.CityId and bs.ProjectId=b.ProjectId and bs.BuildingName=b.BuildingName) and b.FxtCompanyId in(" + comId + @")  
                                union 
                             select BuildingId from " + ctable.buildingtable + @"_sub b with(nolock) where BuildingName=@BuildingName and ProjectId=@ProjectId and valid=@valid 
                                and CityId=@CityId and b.Fxt_CompanyId=@FxtCompanyId";
                    buildingid = con.Query<DAT_Building>(buildingidsql, new
                    {
                        BuildingName = item.buildingname.Trim(),
                        ProjectId = item.projectid,
                        CityId = item.cityid,
                        FxtCompanyId = item.fxtcompanyid,
                        valid = 1
                    }).FirstOrDefault().buildingid;
                }

                ////更新search表
                //if (number > 0)
                //{
                //    string sql = @"select * from " + ctable.buildingtable + @" with(nolock) where Valid = 1 and CityID = @cityid and FxtCompanyId = @fxtcompanyid and ProjectId = @projectid and buildingName = @buildingName ";
                //    var result = con.Query<DAT_Building>(sql, new { cityid = item.cityid, fxtcompanyid = item.fxtcompanyid, projectid = item.projectid, buildingName = item.buildingname }).FirstOrDefault();

                //    if (result != null)
                //    {
                //        AddBuildingSearch(ctable.buildingtable + "_Search", result.cityid, result.fxtcompanyid, result.buildingid, result.projectid, result.buildingname, result.othername, result.doorplate);
                //    }
                //}

                return buildingid;
            }
        }

        // 修改楼栋
        public int ModifyBuilding(DAT_Building item, int currFxtCompanyId)
        {
            try
            {
                string strsql = "";
                var ctable = GetCityTable(item.cityid).FirstOrDefault();
                string searchTable = string.Empty;
                if (Convert.ToDecimal(item.weight) <= 0)
                {
                    item.weight = Convert.ToDecimal(1.0);
                }

                double d = Convert.ToDouble(item.weight) / Convert.ToDouble(item.OldWeight == 0 ? Convert.ToDouble(item.weight) : item.OldWeight);

                string buildingTable = "FXTProject." + ctable.buildingtable;
                string houseTable = "FXTProject." + ctable.housetable;

                if (currFxtCompanyId == FxtComId) //当前操作者房讯通, 只更新主表，不插入子表修改
                {
                    strsql = "delete from " + buildingTable + @"_sub with(rowlock) WHERE [BuildingId]=@BuildingId and CityId=@CityId and [Fxt_CompanyId] =" + FxtComId;
                    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        conn.Execute(strsql, item);//删除掉子表中等于companyId=25的数据（以前的错误数据）
                    }

                    strsql = @"Update " + buildingTable + @" with(rowlock) set 
[BuildingName]=@BuildingName,[OtherName]=@OtherName,[PurposeCode]=@PurposeCode,[BuildingTypeCode]=@BuildingTypeCode,[BuildDate]=@BuildDate,[SaleDate]=@SaleDate,[SalePrice]=@SalePrice,[UnitsNumber]=@UnitsNumber,[TotalFloor]=@TotalFloor,[TotalNumber]=@TotalNumber,[TotalBuildArea]=@TotalBuildArea,IsEValue=@IsEValue,valid=1,SaveUser=@SaveUser,SaveDateTime=GetDate(),Weight=@Weight,AveragePrice=@AveragePrice,LocationCode=@LocationCode,FrontCode=@FrontCode,SightCode=@SightCode,YearWeight=@YearWeight,LocationWeight=@LocationWeight,FrontWeight=@FrontWeight,SightWeight=@SightWeight,BuildingTypeWeight=@BuildingTypeWeight,Wall=@Wall,ElevatorRate=@ElevatorRate,IsElevator=@IsElevator,StructureCode=@StructureCode,FloorHigh=@FloorHigh,X=@X,Y=@Y,BHouseTypeCode=@BHouseTypeCode,BHouseTypeWeight=@BHouseTypeWeight,Distance=@Distance,DistanceWeight=@DistanceWeight,BaseMent=@BaseMent,IsYard=@IsYard,YardWeight=@YardWeight,ElevatorRateWeight=@ElevatorRateWeight,Remark=@Remark,Doorplate=@Doorplate,RightCode=@RightCode, IsVirtual=@IsVirtual, FloorSpread=@FloorSpread, PodiumBuildingFloor=@PodiumBuildingFloor,PodiumBuildingArea=@PodiumBuildingArea, TowerBuildingArea=@TowerBuildingArea, BasementArea=@BasementArea, BasementPurpose=@BasementPurpose, HouseNumber=@HouseNumber, HouseArea=@HouseArea, OtherNumber=@OtherNumber, OtherArea=@OtherArea, innerFitmentCode=@innerFitmentCode, FloorHouseNumber=@FloorHouseNumber, LiftNumber=@LiftNumber, LiftBrand=@LiftBrand, Facilities=@Facilities, PipelineGasCode=@PipelineGasCode, HeatingModeCode=@HeatingModeCode, WallTypeCode=@WallTypeCode,pricedetail=@pricedetail,subaverageprice=@subaverageprice,salelicence=@salelicence,licencedate=@licencedate,joindate=@joindate,averagefloor=@averagefloor,MaintenanceCode=@MaintenanceCode,isTotalFloor=@isTotalFloor
where BuildingId=@BuildingId and CityId=@CityId";
                }
                else if (item.fxtcompanyid == currFxtCompanyId)
                {
                    //本评估机构修改本评估机构数据时，先检查一下，主子表是否同时存在相同fxtcompanyid，相同Buildingid的数据。如果存在，先把子表的数据给删掉。20151123
                    string fxtcompanysql = "select * from " + buildingTable + " with(nolock) where BuildingId = " + item.buildingid + " and CityId = " + item.cityid + " and FxtCompanyId = " + currFxtCompanyId;
                    string fxt_companysql = "select * from " + buildingTable + "_sub with(nolock) where BuildingId = " + item.buildingid + " and CityId = " + item.cityid + " and Fxt_CompanyId = " + currFxtCompanyId;
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                        DataTable obj = DBHelperSql.ExecuteDataTable(fxtcompanysql);
                        DataTable obj1 = DBHelperSql.ExecuteDataTable(fxt_companysql);
                        if (obj != null && obj.Rows.Count == 1 && obj1 != null && obj1.Rows.Count == 1)
                        {
                            string deletefxt_companysql = "delete from " + buildingTable + @"_sub with(rowlock) WHERE BuildingId = " + item.buildingid + " and CityId = " + item.cityid + " and Fxt_CompanyId =" + currFxtCompanyId;
                            con.Execute(deletefxt_companysql);
                        }
                    }

                    var subSql = @"Update " + buildingTable + @"_sub with(rowlock) set 
[BuildingName]=@BuildingName,[OtherName]=@OtherName,[PurposeCode]=@PurposeCode,[BuildingTypeCode]=@BuildingTypeCode,[BuildDate]=@BuildDate,[SaleDate]=@SaleDate,[SalePrice]=@SalePrice,[UnitsNumber]=@UnitsNumber,[TotalFloor]=@TotalFloor,[TotalNumber]=@TotalNumber,[TotalBuildArea]=@TotalBuildArea,IsEValue=@IsEValue,valid=1,SaveUser=@SaveUser,SaveDateTime=GetDate(),Weight=@Weight,AveragePrice=@AveragePrice,LocationCode=@LocationCode,FrontCode=@FrontCode,SightCode=@SightCode,YearWeight=@YearWeight,LocationWeight=@LocationWeight,FrontWeight=@FrontWeight,SightWeight=@SightWeight,BuildingTypeWeight=@BuildingTypeWeight,Wall=@Wall,ElevatorRate=@ElevatorRate,IsElevator=@IsElevator,StructureCode=@StructureCode,FloorHigh=@FloorHigh,X=@X,Y=@Y,BHouseTypeCode=@BHouseTypeCode,BHouseTypeWeight=@BHouseTypeWeight,Distance=@Distance,DistanceWeight=@DistanceWeight,BaseMent=@BaseMent,IsYard=@IsYard,YardWeight=@YardWeight,ElevatorRateWeight=@ElevatorRateWeight,Remark=@Remark,Doorplate=@Doorplate,RightCode=@RightCode, IsVirtual=@IsVirtual, FloorSpread=@FloorSpread, PodiumBuildingFloor=@PodiumBuildingFloor,PodiumBuildingArea=@PodiumBuildingArea, TowerBuildingArea=@TowerBuildingArea, BasementArea=@BasementArea, BasementPurpose=@BasementPurpose, HouseNumber=@HouseNumber, HouseArea=@HouseArea, OtherNumber=@OtherNumber, OtherArea=@OtherArea, innerFitmentCode=@innerFitmentCode, FloorHouseNumber=@FloorHouseNumber, LiftNumber=@LiftNumber, LiftBrand=@LiftBrand, Facilities=@Facilities, PipelineGasCode=@PipelineGasCode, HeatingModeCode=@HeatingModeCode, WallTypeCode=@WallTypeCode,pricedetail=@pricedetail,subaverageprice=@subaverageprice,salelicence=@salelicence,licencedate=@licencedate,joindate=@joindate,averagefloor=@averagefloor,MaintenanceCode=@MaintenanceCode,isTotalFloor=@isTotalFloor
where BuildingId=@BuildingId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId";

                    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        var subresult = conn.Execute(subSql, item);
                        if (subresult < 1) //子表不存在就更新主表
                        {
                            strsql = @"Update " + buildingTable + @" with(rowlock) set 
[BuildingName]=@BuildingName,[OtherName]=@OtherName,[PurposeCode]=@PurposeCode,[BuildingTypeCode]=@BuildingTypeCode,[BuildDate]=@BuildDate,[SaleDate]=@SaleDate,[SalePrice]=@SalePrice,[UnitsNumber]=@UnitsNumber,[TotalFloor]=@TotalFloor,[TotalNumber]=@TotalNumber,[TotalBuildArea]=@TotalBuildArea,IsEValue=@IsEValue,valid=1,SaveUser=@SaveUser,SaveDateTime=GetDate(),Weight=@Weight,AveragePrice=@AveragePrice,LocationCode=@LocationCode,FrontCode=@FrontCode,SightCode=@SightCode,YearWeight=@YearWeight,LocationWeight=@LocationWeight,FrontWeight=@FrontWeight,SightWeight=@SightWeight,BuildingTypeWeight=@BuildingTypeWeight,Wall=@Wall,ElevatorRate=@ElevatorRate,IsElevator=@IsElevator,StructureCode=@StructureCode,FloorHigh=@FloorHigh,X=@X,Y=@Y,BHouseTypeCode=@BHouseTypeCode,BHouseTypeWeight=@BHouseTypeWeight,Distance=@Distance,DistanceWeight=@DistanceWeight,BaseMent=@BaseMent,IsYard=@IsYard,YardWeight=@YardWeight,ElevatorRateWeight=@ElevatorRateWeight,Remark=@Remark,Doorplate=@Doorplate,RightCode=@RightCode, IsVirtual=@IsVirtual, FloorSpread=@FloorSpread, PodiumBuildingFloor=@PodiumBuildingFloor,PodiumBuildingArea=@PodiumBuildingArea, TowerBuildingArea=@TowerBuildingArea, BasementArea=@BasementArea, BasementPurpose=@BasementPurpose, HouseNumber=@HouseNumber, HouseArea=@HouseArea, OtherNumber=@OtherNumber, OtherArea=@OtherArea, innerFitmentCode=@innerFitmentCode, FloorHouseNumber=@FloorHouseNumber, LiftNumber=@LiftNumber, LiftBrand=@LiftBrand, Facilities=@Facilities, PipelineGasCode=@PipelineGasCode, HeatingModeCode=@HeatingModeCode, WallTypeCode=@WallTypeCode,pricedetail=@pricedetail,subaverageprice=@subaverageprice,salelicence=@salelicence,licencedate=@licencedate,joindate=@joindate,averagefloor=@averagefloor,MaintenanceCode=@MaintenanceCode,isTotalFloor=@isTotalFloor
where BuildingId=@BuildingId and CityId=@CityId";
                        }
                    }
                }
                else
                {
                    var sql = @"SELECT BuildingId FROM " + buildingTable + @"_sub with(nolock) WHERE BuildingId=" + item.buildingid + " and CityId=" + item.cityid + " and Fxt_CompanyId=" + currFxtCompanyId;

                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    DataTable obj = DBHelperSql.ExecuteDataTable(sql);
                    if (obj != null && obj.Rows.Count > 0)
                    {
                        strsql = @"Update " + buildingTable + @"_sub with(rowlock) set 
[BuildingName]=@BuildingName,[OtherName]=@OtherName,[PurposeCode]=@PurposeCode,[BuildingTypeCode]=@BuildingTypeCode,[BuildDate]=@BuildDate,[SaleDate]=@SaleDate,[SalePrice]=@SalePrice,[UnitsNumber]=@UnitsNumber,[TotalFloor]=@TotalFloor,[TotalNumber]=@TotalNumber,[TotalBuildArea]=@TotalBuildArea,IsEValue=@IsEValue,valid=1,SaveUser=@SaveUser,SaveDateTime=GetDate(),Weight=@Weight,AveragePrice=@AveragePrice,LocationCode=@LocationCode,FrontCode=@FrontCode,SightCode=@SightCode,YearWeight=@YearWeight,LocationWeight=@LocationWeight,FrontWeight=@FrontWeight,SightWeight=@SightWeight,BuildingTypeWeight=@BuildingTypeWeight,Wall=@Wall,ElevatorRate=@ElevatorRate,IsElevator=@IsElevator,StructureCode=@StructureCode,FloorHigh=@FloorHigh,X=@X,Y=@Y,BHouseTypeCode=@BHouseTypeCode,BHouseTypeWeight=@BHouseTypeWeight,Distance=@Distance,DistanceWeight=@DistanceWeight,BaseMent=@BaseMent,IsYard=@IsYard,YardWeight=@YardWeight,ElevatorRateWeight=@ElevatorRateWeight,Remark=@Remark,Doorplate=@Doorplate,RightCode=@RightCode, IsVirtual=@IsVirtual, FloorSpread=@FloorSpread, PodiumBuildingFloor=@PodiumBuildingFloor,PodiumBuildingArea=@PodiumBuildingArea, TowerBuildingArea=@TowerBuildingArea, BasementArea=@BasementArea, BasementPurpose=@BasementPurpose, HouseNumber=@HouseNumber, HouseArea=@HouseArea, OtherNumber=@OtherNumber, OtherArea=@OtherArea, innerFitmentCode=@innerFitmentCode, FloorHouseNumber=@FloorHouseNumber, LiftNumber=@LiftNumber, LiftBrand=@LiftBrand, Facilities=@Facilities, PipelineGasCode=@PipelineGasCode, HeatingModeCode=@HeatingModeCode, WallTypeCode=@WallTypeCode,pricedetail=@pricedetail,subaverageprice=@subaverageprice,salelicence=@salelicence,licencedate=@licencedate,joindate=@joindate,averagefloor=@averagefloor,MaintenanceCode=@MaintenanceCode,isTotalFloor=@isTotalFloor
where BuildingId=@BuildingId and CityId=@CityId and Fxt_CompanyId=" + currFxtCompanyId;
                    }
                    else
                    {
                        //拷贝到子表修改
                        strsql = @"INSERT INTO " + buildingTable + @"_sub ([ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[Fxt_CompanyId],CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate,MaintenanceCode,isTotalFloor)
SELECT ProjectId,BuildingId,@BuildingName,@OtherName,@PurposeCode,@BuildingTypeCode,@BuildDate,@SaleDate,@SalePrice,@UnitsNumber,@TotalFloor,@TotalNumber,@TotalBuildArea,@IsEValue,@AveragePrice,' "
+ item.saveuser + @"' AS [SaveUser],'"
+ currFxtCompanyId + @"' AS Fxt_CompanyId,'"
+ item.cityid + @"' AS CityId,@AverageFloor,@Weight,@LocationCode,@FrontCode,@SightCode,@YearWeight,@LocationWeight,@FrontWeight,@SightWeight,@BuildingTypeWeight,@Wall,@ElevatorRate,@IsElevator,@StructureCode,@FloorHigh,@X,@Y,@PriceDetail,@BHouseTypeCode,@BHouseTypeWeight,Creator,@Distance,@DistanceWeight,@BaseMent,@IsYard,@YardWeight,@ElevatorRateWeight,@Remark,@Doorplate,@RightCode,@IsVirtual,@FloorSpread,@PodiumBuildingFloor,@PodiumBuildingArea,@TowerBuildingArea,@BasementArea,@BasementPurpose,@HouseNumber,@HouseArea,@OtherNumber,@OtherArea,@innerFitmentCode,@FloorHouseNumber,@LiftNumber,@LiftBrand,@Facilities,@PipelineGasCode,@HeatingModeCode,@WallTypeCode,@subaverageprice,@salelicence,@licencedate,@joindate,@MaintenanceCode,@isTotalFloor
FROM " + buildingTable + @" WITH (NOLOCK) WHERE BuildingId = '" + item.buildingid + @"' AND CityId = '" + item.cityid + @"'";
                    }
                }
                item.savedatetime = DateTime.Now;

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    if (!string.IsNullOrEmpty(strsql))
                    {
                        con.Execute(strsql, item);
                    }
                    if (d != 1.0)
                    {
                        var updateHouseSql = @"Update " + houseTable + @" with(rowlock) set Weight = Weight*" + d + "," +
                                              "UnitPrice=UnitPrice*" + d + @" where Cityid=@CityId and BuildingId=@BuildingId and FxtCompanyId=@FxtCompanyId 
                                          Update " + houseTable + @"_sub with(rowlock) set Weight = Weight*" + d + ",UnitPrice=UnitPrice*" +
                                      d + " where Cityid=@CityId and BuildingId=@BuildingId and Fxt_CompanyId=@FxtCompanyId";

                        return con.Execute(updateHouseSql, item);
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public int UpdateBuilding4Excel(DAT_Building item, int currFxtCompanyId, List<string> modifiedProperty)
        {
            try
            {
                //using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                //{
                string searchTable = string.Empty;
                var ctable = GetCityTable(item.cityid).FirstOrDefault();
                var buildingTable = "FXTProject." + ctable.buildingtable;
                var houseTable = "FXTProject." + ctable.housetable;

                if (Convert.ToDecimal(item.weight) <= 0)
                    item.weight = Convert.ToDecimal(1.0);
                var d = Convert.ToDouble(item.weight) /
                        Convert.ToDouble(item.OldWeight == 0 ? Convert.ToDouble(item.weight) : item.OldWeight);
                if (!modifiedProperty.Any()) return 0;

                var field = modifiedProperty.Aggregate(" set ", (current, m) => current + m);

                #region 注释

                //var field = @" set [BuildingName]=@BuildingName,[OtherName]=@OtherName,[PurposeCode]=@PurposeCode,[BuildingTypeCode]=@BuildingTypeCode,[BuildDate]=@BuildDate,[SaleDate]=@SaleDate,[SalePrice]=@SalePrice,[UnitsNumber]=@UnitsNumber,[TotalFloor]=@TotalFloor,[TotalNumber]=@TotalNumber,[TotalBuildArea]=@TotalBuildArea,IsEValue=@IsEValue,valid=1,SaveUser=@SaveUser,SaveDateTime=GetDate(),Weight=@Weight,AveragePrice=@AveragePrice,LocationCode=@LocationCode,FrontCode=@FrontCode,SightCode=@SightCode,YearWeight=@YearWeight,LocationWeight=@LocationWeight,FrontWeight=@FrontWeight,SightWeight=@SightWeight,BuildingTypeWeight=@BuildingTypeWeight,Wall=@Wall,ElevatorRate=@ElevatorRate,IsElevator=@IsElevator,StructureCode=@StructureCode,FloorHigh=@FloorHigh,X=@X,Y=@Y,BHouseTypeCode=@BHouseTypeCode,BHouseTypeWeight=@BHouseTypeWeight,Distance=@Distance,DistanceWeight=@DistanceWeight,BaseMent=@BaseMent,IsYard=@IsYard,YardWeight=@YardWeight,ElevatorRateWeight=@ElevatorRateWeight,Remark=@Remark,Doorplate=@Doorplate,RightCode=@RightCode, IsVirtual=@IsVirtual, FloorSpread=@FloorSpread, PodiumBuildingFloor=@PodiumBuildingFloor,PodiumBuildingArea=@PodiumBuildingArea, TowerBuildingArea=@TowerBuildingArea, BasementArea=@BasementArea, BasementPurpose=@BasementPurpose, HouseNumber=@HouseNumber, HouseArea=@HouseArea, OtherNumber=@OtherNumber, OtherArea=@OtherArea, innerFitmentCode=@innerFitmentCode, FloorHouseNumber=@FloorHouseNumber, LiftNumber=@LiftNumber, LiftBrand=@LiftBrand, Facilities=@Facilities, PipelineGasCode=@PipelineGasCode, HeatingModeCode=@HeatingModeCode, WallTypeCode=@WallTypeCode,pricedetail=@pricedetail,subaverageprice=@subaverageprice,salelicence=@salelicence,licencedate=@licencedate,joindate=@joindate,averagefloor=@averagefloor  ";

                #endregion
                #region 旧程序20150730
                //                var strMainsql = "SELECT [BuildingId] FROM " + buildingTable +
                //                                 @" with(nolock) WHERE [BuildingId]=@BuildingId and CityId=@CityId and [FxtCompanyId] = @FxtCompanyId";
                //                var strSubsql = "SELECT [BuildingId] FROM " + buildingTable +
                //                                @"_sub with(nolock) WHERE [BuildingId]=@BuildingId and CityId=@CityId and [Fxt_CompanyId] = @FxtCompanyId";

                //                var subExists = con.Query<int>(strSubsql,
                //                    new { BuildingId = item.buildingid, CityId = item.cityid, FxtCompanyId = item.fxtcompanyid });
                //                if (subExists != null && subExists.Any()) //子表存在我自身的数据
                //                {
                //                    var mainExists = con.Query<int>(strMainsql,
                //                        new
                //                        {
                //                            BuildingId = item.buildingid,
                //                            CityId = item.cityid,
                //                            FxtCompanyId = item.fxtcompanyid
                //                        });
                //                    if (mainExists != null && mainExists.Any()) //主表也存在我自身的数据
                //                    {
                //                        //删除子表数据
                //                        var deleteSubSql = @"delete from " + buildingTable +
                //                                           "_sub WHERE  [BuildingId]=@BuildingId and CityId=@CityId and [Fxt_CompanyId] = @FxtCompanyId";

                //                        con.Execute(deleteSubSql,
                //                            new
                //                            {
                //                                BuildingId = item.buildingid,
                //                                CityId = item.cityid,
                //                                FxtCompanyId = item.fxtcompanyid
                //                            });

                //                        //更新主表数据
                //                        var updateMainSql = @"Update " + buildingTable + @" with(rowlock) " + field +
                //                                            "  where BuildingId=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                //                        con.Execute(updateMainSql, item);
                //                        searchTable = "mainTable";
                //                    }
                //                    else
                //                    {
                //                        //更新子表数据
                //                        var updateSubSql = @"Update " + ctable.buildingtable + @"_sub with(rowlock) " + field +
                //                                           "  where BuildingId=@BuildingId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId";

                //                        con.Execute(updateSubSql, item);
                //                        searchTable = "subTable";
                //                    }
                //                }
                //                else // 1.我自身评估机构的数据 2.其他评估机构的数据
                //                {

                //                    var mainExists = con.Query<int>(strMainsql,
                //                        new
                //                        {
                //                            BuildingId = item.buildingid,
                //                            CityId = item.cityid,
                //                            FxtCompanyId = item.fxtcompanyid
                //                        });

                //                    if (mainExists != null && mainExists.Any())
                //                    {
                //                        //更新主表数据
                //                        var updateMainSql = @"Update " + buildingTable + @" with(rowlock) " + field +
                //                                            "  where BuildingId=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                //                        con.Execute(updateMainSql, item);
                //                        searchTable = "mainTable";
                //                    }
                //                    else
                //                    {
                //                        //拷贝到子表再进行修改
                //                        var insertSubSql = @"INSERT INTO " + buildingTable + @"_sub ([ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[Fxt_CompanyId],CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate)
                //SELECT [ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],[IsEValue],[AveragePrice],'" +
                //                                           item.saveuser + @"' AS [SaveUser],'" + currFxtCompanyId +
                //                                           @"' AS Fxt_CompanyId,'" + item.cityid +
                //                                           @"' AS CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate 
                //FROM " + buildingTable + @" WITH (NOLOCK)
                //WHERE BuildingId = '" + item.buildingid + @"'
                //	AND CityId = '" + item.cityid + @"'";

                //                        con.Execute(insertSubSql);

                //                        //更新子表数据
                //                        var updateSubSql = @"Update " + ctable.buildingtable + @"_sub with(rowlock) " + field +
                //                                           "  where BuildingId=@BuildingId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId";

                //                        con.Execute(updateSubSql, item);
                //                        searchTable = "subTable";
                //                    }
                //                }
                #endregion
                string strsql = string.Empty;
                if (currFxtCompanyId == FxtComId) //当前操作者房讯通, 只更新主表，不插入子表修改
                {
                    strsql = "delete from " + buildingTable + @"_sub with(rowlock) WHERE BuildingId = @BuildingId and CityId=@CityId and [Fxt_CompanyId] =" + FxtComId;
                    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        conn.Execute(strsql, item);//删除掉子表中等于companyId=25的数据（以前的错误数据）
                    }

                    strsql = @"Update " + buildingTable + @" with(rowlock) " + field + @" where BuildingId=@BuildingId and CityId=@CityId";
                }
                else if (item.fxtcompanyid == currFxtCompanyId)
                {
                    //本评估机构修改本评估机构数据时，先检查一下，主子表是否同时存在相同fxtcompanyid，相同Buildingid的数据。如果存在，先把子表的数据给删掉。20151123
                    string fxtcompanysql = "select * from " + buildingTable + " with(nolock) where BuildingId = " + item.buildingid + " and CityId = " + item.cityid + " and FxtCompanyId = " + currFxtCompanyId;
                    string fxt_companysql = "select * from " + buildingTable + "_sub with(nolock) where BuildingId = " + item.buildingid + " and CityId = " + item.cityid + " and Fxt_CompanyId = " + currFxtCompanyId;
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                        DataTable obj = DBHelperSql.ExecuteDataTable(fxtcompanysql);
                        DataTable obj1 = DBHelperSql.ExecuteDataTable(fxt_companysql);
                        if (obj != null && obj.Rows.Count == 1 && obj1 != null && obj1.Rows.Count == 1)
                        {
                            string deletefxt_companysql = "delete from " + buildingTable + @"_sub WHERE BuildingId = " + item.buildingid + " and CityId = " + item.cityid + " and Fxt_CompanyId =" + currFxtCompanyId;
                            con.Execute(deletefxt_companysql);
                        }
                    }

                    var subSql = @"Update " + buildingTable + @"_sub with(rowlock) " + field + @" where BuildingId=@BuildingId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId";

                    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        var subresult = conn.Execute(subSql, item);
                        if (subresult < 1) //子表不存在就更新主表
                        {
                            strsql = @"Update " + buildingTable + @" with(rowlock) " + field + @" where BuildingId=@BuildingId and CityId=@CityId";
                        }
                    }
                }
                else
                {
                    var sql = @"SELECT BuildingId FROM " + buildingTable + @"_sub with(nolock) WHERE BuildingId=" + item.buildingid + " and CityId=" + item.cityid + " and Fxt_CompanyId=" + currFxtCompanyId;

                    DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                    DataTable obj = DBHelperSql.ExecuteDataTable(sql);
                    if (obj != null && obj.Rows.Count > 0)
                    {
                        strsql = @"Update " + buildingTable + @"_sub with(rowlock) " + field + @" where BuildingId=@BuildingId and CityId=@CityId and Fxt_CompanyId=" + currFxtCompanyId;
                    }
                    else
                    {
                        //拷贝到子表修改
                        string insertstr = @"INSERT INTO " + buildingTable + @"_sub ([ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[Fxt_CompanyId],CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate,MaintenanceCode,isTotalFloor)
SELECT ProjectId,BuildingId,BuildingName,OtherName,PurposeCode,BuildingTypeCode,BuildDate,SaleDate,SalePrice,UnitsNumber,TotalFloor,TotalNumber,TotalBuildArea,IsEValue,AveragePrice,"
+ @"'" + item.saveuser + @"' AS [SaveUser],"
+ currFxtCompanyId + @" AS Fxt_CompanyId,"
+ item.cityid + @" AS CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,"
+ @"'" + item.saveuser + @"' as Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate,MaintenanceCode,isTotalFloor
FROM " + buildingTable + @" WITH (NOLOCK) WHERE BuildingId = " + item.buildingid + @" AND CityId = " + item.cityid;
                        using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            conn.Execute(insertstr, item);
                        }

                        //更新子表数据
                        strsql = @"Update " + ctable.buildingtable + @"_sub with(rowlock) " + field + " where BuildingId=@BuildingId and CityId=@CityId and Fxt_CompanyId=" + currFxtCompanyId;
                    }
                }
                item.savedatetime = DateTime.Now;

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    if (!string.IsNullOrEmpty(strsql))
                    {
                        con.Execute(strsql, item);
                    }
                    if (d != 1.0)
                    {
                        var updateHouseSql = @"Update " + houseTable + @" with(rowlock) set Weight = Weight*" + d + "," +
                                              "UnitPrice=UnitPrice*" + d + @" where Cityid=@CityId and BuildingId=@BuildingId and FxtCompanyId=@FxtCompanyId 
                                          Update " + houseTable + @"_sub with(rowlock) set Weight = Weight*" + d + ",UnitPrice=UnitPrice*" +
                                      d + " where Cityid=@CityId and BuildingId=@BuildingId and Fxt_CompanyId=@FxtCompanyId";

                        return con.Execute(updateHouseSql, item);
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        // 删除楼栋
        public int DeleteBuilding(int buildId, int CityId, int FxtCompanyId, string UserId, int ProductTypeCode, int currFxtCompanyId, int isDeleteTrue)
        {
            string sql = string.Empty;
            //string sql = "SELECT CompanyId,IsDeleteTrue FROM CompanyProduct WITH(NOLOCK) WHERE CompanyId=@FxtCompanyId and CityId=@CityId and ProductTypeCode=@ProductTypeCode";
            //using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
            //{
            //    var result = con.Query<CompanyProduct>(sql, new { FxtCompanyId = FxtCompanyId, CityId = CityId, ProductTypeCode = ProductTypeCode });
            //    if (result != null && result.Count() > 0)
            //    {
            //        if (result.FirstOrDefault().IsDeleteTrue == 1)
            //        {
            //            return DeleteBuilding2(CityId, buildId, FxtCompanyId, UserId);
            //        }
            //    }
            //}
            if (isDeleteTrue == 1)
            {
                return DeleteBuilding2(CityId, buildId, FxtCompanyId, UserId);
            }
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                try
                {
                    var ctable = GetCityTable(CityId).FirstOrDefault();
                    if (ctable != null)
                    {
                        string btable = "FXTProject." + ctable.buildingtable;
                        #region 错误逻辑
                        //                        if (FxtCompanyId == FxtComId)
                        //                        {
                        //                            sql = "update " + btable + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId";
                        //                            sql += " if EXISTS(SELECT [BuildingId] FROM " + btable + "_sub with(nolock) WHERE [BuildingId] = @BuildingId and CityId = @CityId and [Fxt_CompanyId] = " + FxtCompanyId + ") "
                        //                                + " begin "
                        //                                + "delete top(1)" + btable + "_sub with(rowlock) where [BuildingId] = @BuildingId and CityId = @CityId and [Fxt_CompanyId] = " + FxtCompanyId
                        //                                + " end ";
                        //                        }
                        //                        else if (FxtCompanyId == currFxtCompanyId)
                        //                        {
                        //                            sql = "update " + btable + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId";
                        //                            sql += " if EXISTS(SELECT [BuildingId] FROM " + btable + "_sub with(nolock) WHERE [BuildingId] = @BuildingId and CityId = @CityId and [Fxt_CompanyId] = " + FxtCompanyId + ") "
                        //                               + " begin "
                        //                               + "delete top(1)" + btable + "_sub with(rowlock) where [BuildingId] = @BuildingId and CityId = @CityId and [Fxt_CompanyId] = " + FxtCompanyId
                        //                               + " end ";
                        //                        }
                        //                        else
                        //                        {
                        //                            sql = "SELECT BuildingId FROM " + btable + "_sub with(nolock) WHERE [BuildingId]=@BuildingId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId";
                        //                            var build = con.Query<DAT_Building>(sql, new { BuildingId = buildId, CityId = CityId, FxtCompanyId = FxtCompanyId }).FirstOrDefault();
                        //                            if (build != null)
                        //                            {
                        //                                sql = "update " + btable + @"_sub with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId";
                        //                            }
                        //                            else
                        //                            {
                        //                                sql = "SELECT BuildingId FROM " + btable + " with(nolock) WHERE [BuildingId]=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        //                                var build_sub = con.Query<DAT_Building>(sql, new { BuildingId = buildId, CityId = CityId, FxtCompanyId = FxtCompanyId }).FirstOrDefault();
                        //                                if (build_sub != null)
                        //                                {
                        //                                    sql = "update " + btable + @" with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        //                                }
                        //                                else
                        //                                                        {
                        //                                                            sql = "INSERT INTO " + btable + @"_sub ([BuildingId],[Fxt_CompanyId],[BuildingName],[ProjectId],[PurposeCode],[StructureCode]
                        //                                                                                            ,[BuildingTypeCode],[TotalFloor],[FloorHigh],[SaleLicence],[ElevatorRate],[UnitsNumber],[TotalNumber]
                        //                                                                                            ,[TotalBuildArea],[BuildDate],[SaleDate],[AveragePrice],[AverageFloor],[JoinDate],[LicenceDate]
                        //                                                                                            ,[OtherName],[Weight],[IsEValue],[CityID],[CreateTime],[OldId],[Valid],[SalePrice],[SaveDateTime],[SaveUser]
                        //                                                                                            ,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight
                        //                                                                                            ,PurposeWeight,LocationWeight,SightWeight,FrontWeight,X,Y,XYScale,Wall,IsElevator,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark) 
                        //                                                            SELECT [BuildingId],'" + currFxtCompanyId + @"' as [FxtCompanyId],[BuildingName],[ProjectId],[PurposeCode],[StructureCode]
                        //                                                                    ,[BuildingTypeCode],[TotalFloor],[FloorHigh],[SaleLicence],[ElevatorRate],[UnitsNumber],[TotalNumber]
                        //                                                                    ,[TotalBuildArea],[BuildDate],[SaleDate],[AveragePrice],[AverageFloor],[JoinDate],[LicenceDate]
                        //                                                                    ,[OtherName],[Weight],[IsEValue],[CityID],[CreateTime],[OldId],0 as [Valid],[SalePrice],GetDate() as [SaveDateTime]
                        //                                                                    ,'" + UserId + @"' as [SaveUser] ,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight
                        //                                                                    ,PurposeWeight,LocationWeight,SightWeight,FrontWeight,X,Y,XYScale,Wall,IsElevator,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark  
                        //                                                                    FROM  " + btable + @"  with(nolock) where BuildingId=@BuildingId and CityId=@CityId";
                        //                                }
                        //                            }
                        //                        }
                        #endregion
                        if (currFxtCompanyId == FxtComId)//当前操作者为房讯通
                        {
                            sql = @"
if EXISTS(SELECT BuildingId FROM " + btable + "_sub with(nolock) WHERE BuildingId=@BuildingId and CityId=@CityId and [Fxt_CompanyId] = " + FxtComId + @")
begin
delete top(1) " + btable + "_sub with(rowlock)  where BuildingId=@BuildingId and CityId=@CityId and [Fxt_CompanyId] = " + FxtComId + @"
end
";
                            sql += " update " + btable + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where BuildingId=@BuildingId and CityId=@CityId";

                        }
                        else if (FxtCompanyId == currFxtCompanyId)//自身评估机构的数据
                        {
                            sql = "update " + btable + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where BuildingId=@BuildingId and CityId=@CityId and [FxtCompanyId] = " + FxtCompanyId;
                            sql += "  update " + btable + "_sub with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where BuildingId=@BuildingId and CityId=@CityId and [Fxt_CompanyId] = " + FxtCompanyId;
                        }
                        else
                        {
                            sql = @"INSERT INTO " + btable + @"_sub (
BuildingId,Fxt_CompanyId,BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,Valid,SalePrice,SaveDateTime,SaveUser,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,X,Y,XYScale,Wall,IsElevator,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark
,SubAveragePrice,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor)
SELECT 
BuildingId,'" + currFxtCompanyId + @"' AS FxtCompanyId,BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,0 AS Valid,SalePrice,GetDate() AS SaveDateTime,@SaveUser AS SaveUser,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,X,Y,XYScale,Wall,IsElevator,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark
,SubAveragePrice,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor
FROM " + btable + @" WITH (NOLOCK) WHERE BuildingId = @BuildingId AND CityId = @CityId";
                        }
                        int resultId = con.Execute(sql, new { BuildingId = buildId, CityId = CityId, FxtCompanyId = FxtCompanyId, SaveUser = UserId });
                        //IDAT_House hou = new DAT_HouseDAL();
                        //hou.DelHouse(CityId, FxtCompanyId, buildId, UserId);
                        return resultId;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        // 楼栋复制
        public string BuildCopy(int CityId, int FxtCompanyId, int projectId, int buildId, string buildName, string buildNameTo, string userId)
        {
            string msg = "";
            var cityTable = GetCityTable(CityId, FxtCompanyId).FirstOrDefault();
            if (cityTable != null)
            {
                string b_table = "FXTProject." + cityTable.buildingtable,
                ComId = cityTable.ShowCompanyId,
                sql = @"select [BuildingId], [ProjectId],[BuildingName]  
                                   from " + b_table + @" b with(nolock) 
                                   where b.CityId=@CityId and b.ProjectId=@ProjectId and b.FxtCompanyId in(" + ComId + @")  
                                          and not exists (select BuildingId from " + b_table + @"_sub bs with(nolock) 
                                          where bs.[ProjectId]=@ProjectId and b.BuildingId=bs.BuildingId and bs.Fxt_CompanyId=@FxtCompanyId and bs.CityId=b.CityId)
                                         and b.Valid=1  and b.BuildingName=@BuildingName 
                                    union   
                                      select b.BuildingId, b.[ProjectId],b.[BuildingName] 
                                      from " + b_table + @"_sub b with(nolock)   
                                      where b.CityId=@CityId and b.ProjectId=@ProjectId and b.Fxt_CompanyId=@FxtCompanyId and b.Valid=1 and b.BuildingName=@BuildingName ";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var reslut = con.Query<DAT_Building>(sql, new { CityId = CityId, ProjectId = projectId, FxtCompanyId = FxtCompanyId, BuildingName = buildNameTo }).FirstOrDefault();
                    try
                    {
                        IDAT_House hou = new DAT_HouseDAL();

                        //原始楼栋下面的房号集合
                        var houseList = hou.GetHouseInfoByBuild(buildId, CityId, FxtCompanyId);
                        if (reslut != null)//目标楼栋已存在
                        {
                            //目标楼栋下面的房号集合
                            var house = hou.GetHouseInfoByBuild(reslut.buildingid, CityId, FxtCompanyId);
                            if (house != null && house.Count() > 0)//目标楼栋下面已存在房号
                            {
                                msg = "目标楼栋存在房号";
                                return msg;
                            }
                            else//目标楼栋下面房号为空
                            {
                                foreach (var item in houseList)
                                {
                                    item.buildingid = reslut.buildingid;
                                    item.cityid = CityId;
                                    item.fxtcompanyid = FxtCompanyId;
                                    item.saveuser = userId;
                                    hou.AddHouse(item);
                                }
                                return "楼栋复制成功";

                            }

                        }
                        else
                        {
                            var dat = GetBuildingInfo(CityId, projectId, FxtCompanyId, buildId).FirstOrDefault();
                            dat.buildingname = buildNameTo;
                            int num = AddBuild(dat);
                            if (num > 0)
                            {

                                foreach (var item in houseList)
                                {
                                    item.buildingid = num;
                                    item.cityid = CityId;
                                    item.fxtcompanyid = FxtCompanyId;
                                    item.saveuser = userId;
                                    hou.AddHouse(item);
                                }
                                return "楼栋复制成功";
                            }
                            else
                            {
                                return "楼栋复制失败";
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(ex.Message);
                    }
                }

            }
            return "尚无开通权限";
        }

        // 直接删除楼栋(谨慎使用)
        private int DeleteBuilding2(int CityId, int buildId, int FxtCompanyId, string userName)
        {
            var city_table = GetCityTable(CityId).FirstOrDefault();
            if (city_table != null)
            {
                string btable = city_table.buildingtable,
                strsql = "delete " + btable + @"  where FxtCompanyId in(25," + FxtCompanyId + ") and [BuildingId]=@BuildingId and CityId=@CityId"
                        + " delete " + btable + @"_sub  where Fxt_CompanyId=" + FxtCompanyId + " and BuildingId]=@BuildingId and CityId=@CityId";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    int result = con.Execute(strsql, new { BuildingId = buildId, CityId = CityId });
                    IDAT_House hou = new DAT_HouseDAL();
                    //hou.DelHouse(CityId, FxtCompanyId, buildId, userName);
                    return result;
                }
            }
            return 0;
        }

        // 拆分楼盘下面的楼栋、房号  projectid：原始楼盘ID；projectidTo：目标楼盘ID；build_list：原始楼栋集合
        public int SplitBuild(int projectid, int ProjectIdTo, int CityId, int FxtCompanyId, string SaveUser, string build_list, int currFxtCompanyId)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                int returnVal = 0;
                try
                {
                    string strsql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable] FROM [dbo].[SYS_City_Table] c with(nolock) where c.CityId=@CityId";
                    var city_table = con.Query<SYS_City_Table>(strsql, new { CityId = CityId }).AsQueryable().FirstOrDefault();
                    if (city_table != null)
                    {
                        string b_table = city_table.buildingtable;
                        string h_table = city_table.housetable;
                        returnVal = SplitBuild_Shear(projectid, ProjectIdTo, build_list, CityId, FxtCompanyId, SaveUser, currFxtCompanyId);
                        return returnVal;
                    }
                    else
                    {
                        return 0;//没有权限
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
        }

        // 楼盘拆分 ProjectIdTo：目标楼盘ID；build_list：原始楼栋ID集合
        private int SplitBuild_Shear(int projectId, int ProjectIdTo, string build_list, int CityId, int FxtCompanyId, string SaveUser, int currFxtCompanyId)
        {
            try
            {
                IDAT_House house = new DAT_HouseDAL();
                var dt = GetCityTable(CityId).FirstOrDefault();
                string sql = "";
                int num = 0;
                if (dt != null)
                {
                    string b_table = dt.buildingtable;
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        string[] bid = build_list.Split(',');
                        for (int i = 0; i < bid.Length; i++)
                        {
                            int BuildingNew = 0;
                            var build = GetBuildingInfo(CityId, projectId, currFxtCompanyId, int.Parse(bid[i])).FirstOrDefault();
                            //修改之前，先判断一下目标楼盘，是否已经包含相同的楼栋名称。
                            var buildTo = GetBuildingId(ProjectIdTo, build.buildingname, CityId, currFxtCompanyId);
                            if (buildTo > 0)
                            {
                                break;
                            }
                            if (currFxtCompanyId == FxtComId) //当前操作者房讯通, 只更新主表，不插入子表修改
                            {
                                sql = "delete from " + b_table + @"_sub WHERE  BuildingId in (" + bid[i] + @") and CityId=@CityId and [Fxt_CompanyId] =" + FxtComId;
                                con.Execute(sql, new { CityId });//删除掉子表中等于companyId=25的数据（以前的错误数据）

                                sql = "Update " + b_table + @"  with(rowlock) set ProjectId=@ProjectId,SaveDateTime=GETDATE(),SaveUser=@SaveUser where BuildingId in (" + bid[i] + @") and CityId=@CityId";
                                num = con.Execute(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId });
                                BuildingNew = int.Parse(bid[i]);
                            }
                            else if (build.fxtcompanyid == currFxtCompanyId)
                            {
                                //本评估机构修改本评估机构数据时，先检查一下，主子表是否同时存在相同fxtcompanyid，相同Buildingid的数据。如果存在，先把子表的数据给删掉。20151123
                                string fxtcompanysql = "select * from " + b_table + " with(nolock) where BuildingId = " + bid[i] + " and CityId = " + CityId + " and FxtCompanyId = " + currFxtCompanyId;
                                string fxt_companysql = "select * from " + b_table + "_sub with(nolock) where BuildingId = " + bid[i] + " and CityId = " + CityId + " and Fxt_CompanyId = " + currFxtCompanyId;

                                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                                DataTable obj = DBHelperSql.ExecuteDataTable(fxtcompanysql);
                                DataTable obj1 = DBHelperSql.ExecuteDataTable(fxt_companysql);
                                if (obj != null && obj.Rows.Count == 1 && obj1 != null && obj1.Rows.Count == 1)
                                {
                                    string deletefxt_companysql = "delete from " + b_table + @"_sub WHERE BuildingId = " + bid[i] + " and CityId = " + CityId + " and Fxt_CompanyId =" + currFxtCompanyId;
                                    con.Execute(deletefxt_companysql);
                                }

                                //如果存在子表，先把子表的该条楼栋，valid = 0，然后再把子表的这条数据，插入到主表里。
                                var subSql = "Update " + b_table + @"_sub  with(rowlock) set Valid = 0,SaveDateTime = GETDATE(),SaveUser = @SaveUser where BuildingId in (" + bid[i] + @") and CityId=@CityId and [Fxt_CompanyId] =@FxtCompanyId";

                                int r = con.Execute(subSql, new { ProjectId = ProjectIdTo, SaveUser = SaveUser, CityId = CityId, FxtCompanyId = currFxtCompanyId });
                                if (r > 0)
                                {
                                    sql = @"
insert into " + b_table + @"(BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,Valid,SalePrice,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,FxtCompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor)
select BuildingName,@ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,GETDATE(),OldId,1,SalePrice,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,@FxtCompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,@SaveUser,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor
from " + b_table + @"_sub with(nolock) where BuildingId in (" + bid[i] + @") and CityId=@CityId and Fxt_CompanyId = @FxtCompanyId;
SELECT SCOPE_IDENTITY() AS Id;";
                                    //num = con.Execute(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId });
                                    //if (num <= 0) return num;
                                    //var result = con.Query("SELECT @@IDENTITY AS Id").Single();
                                    //BuildingNew = (int)result.Id;
                                    var result = con.Query(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId }).Single();
                                    num = BuildingNew = Convert.ToInt32(result.Id);
                                }
                                else //子表不存在就更新主表
                                {
                                    sql = "Update " + b_table + @" with(rowlock) set ProjectId=@ProjectId,SaveDateTime=GETDATE(),SaveUser=@SaveUser where BuildingId in (" + bid[i] + @") and CityId=@CityId";
                                    num = con.Execute(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId });
                                    BuildingNew = int.Parse(bid[i]);
                                }
                            }
                            else
                            {
                                sql = @"SELECT Buildingid FROM " + b_table + @"_sub with(nolock) WHERE BuildingId in (" + bid[i] + @") and CityId=@CityId and [Fxt_CompanyId] =@FxtCompanyId";
                                SqlParameter[] parameters = { new SqlParameter("@CityId", CityId), new SqlParameter("@FxtCompanyId", currFxtCompanyId), };
                                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                                DataTable obj = DBHelperSql.ExecuteDataTable(sql, parameters);
                                if (obj != null && obj.Rows.Count > 0)
                                {
                                    //如果存在子表，先把子表的该条楼栋，valid = 0，然后再把子表的这条数据，插入到主表里。
                                    var subSql = "Update " + b_table + @"_sub  with(rowlock) set Valid = 0,SaveDateTime = GETDATE(),SaveUser = @SaveUser where BuildingId in (" + bid[i] + @") and CityId=@CityId and [Fxt_CompanyId] = @FxtCompanyId";
                                    int r = con.Execute(subSql, new { ProjectId = ProjectIdTo, SaveUser = SaveUser, CityId = CityId, FxtCompanyId = currFxtCompanyId });
                                    if (r > 0)
                                    {
                                        sql = @"
insert into " + b_table + @"(BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,Valid,SalePrice,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,FxtCompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor)
select BuildingName,@ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,GETDATE(),OldId,1,SalePrice,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,@FxtCompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,@SaveUser,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor
from " + b_table + @"_sub with(nolock) where BuildingId in (" + bid[i] + @") and CityId=@CityId and Fxt_CompanyId = @FxtCompanyId;
select SCOPE_IDENTITY() AS Id;";
                                        //num = con.Execute(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId });
                                        //if (num <= 0) return num;
                                        //var result = con.Query("SELECT @@IDENTITY AS Id").Single();
                                        //BuildingNew = (int)result.Id;
                                        var result = con.Query(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId }).Single();
                                        num = BuildingNew = Convert.ToInt32(result.Id);
                                    }
                                }
                                else
                                {
                                    //如果存在主表，且不是自己的数据，需要把主表的数据复制到子表，valid = 0，且把主表的数据，复制到更新后的projectid，valid = 1。
                                    sql = @"
insert into " + b_table + @"_sub(BuildingId,BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,Valid,SalePrice,SaveDateTime,SaveUser,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,Fxt_CompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor)
select BuildingId,BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,0,SalePrice,GETDATE(),@SaveUser,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,@FxtCompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor
from " + b_table + @" with(nolock) where BuildingId in (" + bid[i] + @") and CityId=@CityId

insert into " + b_table + @"(BuildingName,ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,CreateTime,OldId,Valid,SalePrice,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,FxtCompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor)
select BuildingName,@ProjectId,PurposeCode,StructureCode,BuildingTypeCode,TotalFloor,FloorHigh,SaleLicence,ElevatorRate,UnitsNumber,TotalNumber,TotalBuildArea,BuildDate,SaleDate,AveragePrice,AverageFloor,JoinDate,LicenceDate,OtherName,Weight,IsEValue,CityID,GETDATE(),OldId,1,SalePrice,LocationCode,SightCode,FrontCode,StructureWeight,BuildingTypeWeight,YearWeight,PurposeWeight,LocationWeight,SightWeight,FrontWeight,@FxtCompanyId,X,Y,XYScale,Wall,IsElevator,SubAveragePrice,PriceDetail,BHouseTypeCode,BHouseTypeWeight,@SaveUser,Distance,DistanceWeight,basement,Remark,ElevatorRateWeight,IsYard,YardWeight,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,MaintenanceCode,isTotalFloor
from " + b_table + @" with(nolock) where BuildingId in (" + bid[i] + @") and CityId=@CityId
;select SCOPE_IDENTITY() AS Id;";
                                    //num = con.Execute(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId });
                                    //if (num <= 0) return num;
                                    //var result = con.Query("SELECT @@IDENTITY AS Id").Single();
                                    //BuildingNew = (int)result.Id;
                                    var result = con.Query(sql, new { ProjectId = ProjectIdTo, SaveUser, FxtCompanyId = currFxtCompanyId, CityId }).Single();
                                    num = BuildingNew = Convert.ToInt32(result.Id);
                                }
                            }
                            if (num > 0)
                            {
                                //楼栋拆分成功后，复制楼栋图片。
                                AddBuildingPhoto(ProjectIdTo, BuildingNew, currFxtCompanyId, int.Parse(bid[i]), FxtCompanyId, CityId);
                                //楼栋拆分成功后，复制房号。
                                var hou_obj = house.GetHouseInfoByBuild(int.Parse(bid[i]), CityId, FxtCompanyId);
                                foreach (var item_h in hou_obj)
                                {
                                    item_h.buildingid = BuildingNew;
                                    item_h.createtime = DateTime.Now;
                                    item_h.creator = SaveUser;
                                    item_h.savedatetime = DateTime.Now;
                                    item_h.fxtcompanyid = currFxtCompanyId;
                                    house.AddHouse(item_h);
                                }
                            }
                        }
                    }
                    return num;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*// 更新楼栋状态
        public int UpdateBuildId(int projectId, int CityId, int FxtCompanyId, string build_list)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var dt = GetCityTable(CityId).FirstOrDefault();
                if (dt != null)
                {
                    string b_table = dt.buildingtable,
                    sql = " Update " + b_table + @" with(rowlock) set valid=0 where BuildingId in (" + build_list + @") and CityId=@CityId and FxtCompanyId=@FxtCompanyId"
                          + " Update " + b_table + @"_sub  with(rowlock) set valid=0 where BuildingId in (" + build_list + @") and CityId=@CityId and Fxt_CompanyId=@FxtCompanyId";
                    int num = con.Execute(sql, new { CityId = CityId, FxtCompanyId = FxtCompanyId });
                    return num;
                }
                return 0;
            }
        }*/

        // 楼盘合并  project：原始楼盘obj；projectIdTo：目标楼盘ID
        public int MergerProject(DAT_Project project, int projectIdTo, int currFxtcomyId)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                int returnVal = 0; ;
                try
                {
                    string strsql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.ShowCompanyId FROM [dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and s.FxtCompanyId=@FxtCompanyId and typecode= 1003002";
                    var city_table = con.Query<SYS_City_Table>(strsql, new { CityId = project.cityid, FxtCompanyId = project.fxtcompanyid }).FirstOrDefault();
                    if (city_table != null)
                    {
                        string b_table = city_table.buildingtable;
                        string h_table = city_table.housetable;

                        //////原始楼盘下的楼栋列表
                        //var b_list = GetBuildingInfo(project.cityid, project.projectid, project.fxtcompanyid);
                        var b_listTo = GetBuildingInfo(project.cityid, projectIdTo, project.fxtcompanyid);
                        returnVal = FeachAddBuild(project.cityid, project.fxtcompanyid, project.saveuser, project.projectid, b_table, h_table, b_listTo, currFxtcomyId);
                        return returnVal;
                    }
                    else
                    {
                        return returnVal;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        // 楼栋名称是否重复
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

        public int GetBuildingId(int projectId, string buildingName, int cityId, int fxtCompanyId)
        {
            var globalSql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.ShowCompanyId FROM [dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and FxtCompanyId=@FxtCompanyId and typecode= 1003002";
            SYS_City_Table cityTable = null;
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                cityTable = conn.Query<SYS_City_Table>(globalSql, new { CityId = cityId, FxtCompanyId = fxtCompanyId }).AsQueryable().FirstOrDefault();
                if (cityTable == null) return -1;
            }
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var selectSql = @"select BuildingId from " + cityTable.buildingtable + @" b with(nolock)  where b.Valid=1 and b.CityID=@CityID and b.projectId = @projectId  and b.BuildingName=@BuildingName and  b.FxtCompanyId in(" + cityTable.ShowCompanyId + @") and not exists (select BuildingId from " + cityTable.buildingtable + @"_sub bs with(nolock) where b.BuildingId=bs.BuildingId and bs.Fxt_CompanyId=@FxtCompanyId and bs.CityId=@CityID) 
union  
select BuildingId from " + cityTable.buildingtable + @"_sub b with(nolock) where b.Valid=1 and b.CityID=@CityID and b.projectId = @projectId and  b.BuildingName=@BuildingName  and b.Fxt_CompanyId=@FxtCompanyId ";

                var query = conn.Query<int>(selectSql, new { projectId, buildingName, cityId, fxtCompanyId }).AsQueryable();
                return query == null ? -1 : query.FirstOrDefault();
            }
        }

        public DAT_Building GetBuildingInfo(int projectId, string buildingName, int cityId, int fxtCompanyId)
        {
            var globalSql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.ShowCompanyId FROM [dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and FxtCompanyId=@FxtCompanyId and typecode= 1003002";
            SYS_City_Table cityTable = null;
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                cityTable = conn.Query<SYS_City_Table>(globalSql, new { CityId = cityId, FxtCompanyId = fxtCompanyId }).AsQueryable().FirstOrDefault();
                if (cityTable == null) return new DAT_Building();
            }
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var selectSql = @"select  BuildingId, BuildingName, ProjectId, PurposeCode, StructureCode, BuildingTypeCode, 
                                 TotalFloor, FloorHigh, SaleLicence, ElevatorRate, UnitsNumber, TotalNumber, 
                                 TotalBuildArea, BuildDate, SaleDate, AveragePrice, AverageFloor, JoinDate, 
                                 LicenceDate, OtherName, [Weight], IsEValue, CityID, CreateTime, OldId, Valid, 
                                 SalePrice, SaveDateTime, SaveUser, LocationCode, SightCode, FrontCode, 
                                 StructureWeight, BuildingTypeWeight, YearWeight, PurposeWeight, LocationWeight, 
                                 SightWeight, FrontWeight, FxtCompanyId, X, Y, XYScale, Wall, IsElevator, 
                                 SubAveragePrice, PriceDetail, BHouseTypeCode, BHouseTypeWeight, Creator, 
                                 Distance, DistanceWeight, basement, Remark, ElevatorRateWeight, IsYard, 
                                 YardWeight, Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, 
                                 PodiumBuildingArea, TowerBuildingArea, BasementArea, BasementPurpose, HouseNumber, 
                                 HouseArea, OtherNumber, OtherArea, innerFitmentCode, FloorHouseNumber, LiftNumber, 
                                 LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode, MaintenanceCode,isTotalFloor from  " + cityTable.buildingtable + @" b with(nolock)  where b.Valid=1 and b.CityID=@CityID and b.projectId = @projectId  and b.BuildingName=@BuildingName and  b.FxtCompanyId in(" + cityTable.ShowCompanyId + @") and not exists (select BuildingId from " + cityTable.buildingtable + @"_sub bs with(nolock) where b.BuildingId=bs.BuildingId and bs.Fxt_CompanyId=@FxtCompanyId and bs.CityId=@CityID) 
union  
select  BuildingId, BuildingName, ProjectId, PurposeCode, StructureCode, BuildingTypeCode, 
 TotalFloor, FloorHigh, SaleLicence, ElevatorRate, UnitsNumber, TotalNumber, 
 TotalBuildArea, BuildDate, SaleDate, AveragePrice, AverageFloor, JoinDate, 
 LicenceDate, OtherName, [Weight], IsEValue, CityID, CreateTime, OldId, Valid, 
 SalePrice, SaveDateTime, SaveUser, LocationCode, SightCode, FrontCode, 
 StructureWeight, BuildingTypeWeight, YearWeight, PurposeWeight, LocationWeight, 
 SightWeight, FrontWeight, Fxt_CompanyId, X, Y, XYScale, Wall, IsElevator, 
 SubAveragePrice, PriceDetail, BHouseTypeCode, BHouseTypeWeight, Creator, 
 Distance, DistanceWeight, basement, Remark, ElevatorRateWeight, IsYard, 
 YardWeight, Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, 
 PodiumBuildingArea, TowerBuildingArea, BasementArea, BasementPurpose, HouseNumber, 
 HouseArea, OtherNumber, OtherArea, innerFitmentCode, FloorHouseNumber, LiftNumber, 
 LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode, MaintenanceCode,isTotalFloor from " + cityTable.buildingtable + @"_sub b with(nolock) where b.Valid=1 and b.CityID=@CityID and b.projectId = @projectId and  b.BuildingName=@BuildingName  and b.Fxt_CompanyId=@FxtCompanyId ";

                var query = conn.Query<DAT_Building>(selectSql, new { projectId, buildingName, cityId, fxtCompanyId });
                return query.AsQueryable().FirstOrDefault();
            }
        }

        // 获取楼栋数量
        public int GetBuildCount(int cityId, int fxtcompanyId, int projectId)
        {
            var city_table = GetCityTable(cityId, fxtcompanyId).FirstOrDefault();
            string btable = city_table.buildingtable,
                ComId = city_table.ShowCompanyId;
            if (string.IsNullOrEmpty(ComId)) ComId = fxtcompanyId.ToString();
            string strSql = @"select b.BuildingId from " + btable + @" b with(nolock) 
                              where b.cityid=@cityId and b.valid=1 and b.Projectid=@projectId and b.FxtCompanyId in(" + ComId + @") 
                              and not exists(select sub.BuildingId from " + btable + @"_sub sub with(nolock) 
                                             where sub.ProjectId=@projectId and b.BuildingId=sub.BuildingId 
                                             and b.CityId=sub.CityId and sub.Fxt_CompanyId=@fxtcompanyId) 
                            union  
                            select b.BuildingId from " + btable + @"_sub b with(nolock) 
                            where b.cityid=@cityId and b.valid=1 and b.Projectid=@projectId and b.Fxt_CompanyId=@fxtcompanyId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var list = conn.Query<DAT_Building>(strSql, new { cityId = cityId, projectId = projectId, fxtcompanyId = fxtcompanyId });
                if (list == null || list.Count() == 0) return 0;
                else return list.Count();
            }
        }

        // 获取房号数量
        public int GetHouseCount(int cityId, int fxtcompanyId, int buildingId)
        {
            var city_table = GetCityTable(cityId, fxtcompanyId).FirstOrDefault();
            string htable = city_table.housetable,
                 ComId = city_table.ShowCompanyId;
            if (string.IsNullOrEmpty(ComId)) ComId = fxtcompanyId.ToString();
            string strSql = @"select h.houseId from " + htable + @" h with(nolock) where 
                        h.cityid=@cityId and h.valid=1 and h.BuildingId=@BuildingId and h.FxtCompanyId in(" + ComId + @") 
                        and not exists(select sub.houseId from " + htable + @"_sub sub with(nolock) 
                        where sub.cityid=h.cityId and sub.BuildingId=h.BuildingId and sub.FxtCompanyId=@fxtcompanyId) 
                        union 
                              select h.houseId from " + htable + @"_sub h with(nolock) where 
                        h.cityid=@cityId and h.valid=1 and h.BuildingId=@BuildingId and h.FxtCompanyId=@fxtcompanyId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var list = conn.Query<DAT_House>(strSql, new { cityId = cityId, BuildingId = buildingId, fxtcompanyId = fxtcompanyId });
                if (list == null || list.Count() == 0) return 0;
                else return list.Count();
            }
        }

        public DAT_Project GetProjectNameById(string projectId, int cityId, int fxtcompanyId)
        {
            var cityTable = GetCityTable(cityId, fxtcompanyId).FirstOrDefault();
            string ptable = cityTable.projecttable,
                ComId = cityTable.ShowCompanyId;
            if (string.IsNullOrEmpty(ComId)) ComId = fxtcompanyId.ToString();
            string strSql = @"select projectid,projectName,AreaID,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = p.AreaID) as AreaName,AveragePrice from " + ptable + @" p with(nolock) where 
            p.projectId=@projectId and p.valid=1 and FxtCompanyId in(" + ComId + @") 
             and not exists(select projectId from " + ptable + @"_sub sub with(nolock) where 
                            sub.projectId=@projectId and sub.Fxt_CompanyId=@FxtCompanyId) 
                            union 
                            select projectid,projectName,AreaID,(select AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = p.AreaID) as AreaName,AveragePrice from " + ptable + @"_sub p with(nolock) where 
            p.projectId=@projectId and p.valid=1 and Fxt_CompanyId=@FxtCompanyId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Project>(strSql, new { projectId, fxtcompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        //        public DAT_Project GetSingleProjectById(string projectId, int cityId, int fxtcompanyId)
        //        {
        //            var cityTable = GetCityTable(cityId, fxtcompanyId).FirstOrDefault();
        //            string ptable = cityTable.projecttable,
        //                ComId = cityTable.ShowCompanyId;
        //            if (string.IsNullOrEmpty(ComId)) ComId = fxtcompanyId.ToString();
        //            string strSql = @"select projectName,AveragePrice from " + ptable + @" p with(nolock) where 
        //            p.projectId=@projectId and p.valid=1 and FxtCompanyId in(" + ComId + @") 
        //             and not exists(select projectId from " + ptable + @"_sub sub with(nolock) where 
        //                            sub.projectId=@projectId and sub.Fxt_CompanyId=@FxtCompanyId) 
        //                            union 
        //                            select projectName,AveragePrice from " + ptable + @"_sub p with(nolock) where 
        //            p.projectId=@projectId and p.valid=1 and Fxt_CompanyId=@FxtCompanyId";
        //            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //            {
        //                return conn.Query<DAT_Project>(strSql, new { projectId, fxtcompanyId }).AsQueryable().FirstOrDefault();
        //            }
        //        }

        // 更新房号价格系数说明
        public int EndtityPriceDetail(int buildId, string priceDetail, int cityId, int fxtcompanyId, int currfxtcompanyId, string userName)
        {
            var city_table = GetCityTable(cityId, Convert.ToInt32(fxtcompanyId)).FirstOrDefault();
            if (city_table != null)
            {
                string btable = city_table.buildingtable, ComId = city_table.ShowCompanyId;
                if (string.IsNullOrEmpty(ComId)) ComId = fxtcompanyId.ToString();
                using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    if (fxtcompanyId == FxtComId)//房讯通
                    {
                        string sql = "update  " + btable + "  with(rowlock) set PriceDetail=@PriceDetail where BuildingId=@BuildingId and FxtCompanyId=@FxtCompanyId";
                        return conn.Execute(sql, new { PriceDetail = priceDetail, BuildingId = buildId, FxtCompanyId = FxtComId });
                    }
                    if (fxtcompanyId == currfxtcompanyId)//自己
                    {
                        string sql = "update  " + btable + "  with(rowlock) set PriceDetail=@PriceDetail where BuildingId=@BuildingId and FxtCompanyId=@FxtCompanyId";
                        return conn.Execute(sql, new { PriceDetail = priceDetail, BuildingId = buildId, FxtCompanyId = currfxtcompanyId });
                    }
                    else
                    {
                        string sql = "select BuildingId from  " + btable + "_sub with(nolock) where BuildingId=@BuildingId and Fxt_CompanyId=@FxtCompanyId";
                        var resultsub = conn.Query<DAT_Building>(sql, new { BuildingId = buildId, FxtCompanyId = fxtcompanyId });
                        if (resultsub != null && resultsub.Any())
                        {
                            sql = "update  " + btable + "_sub with(rowlock) set PriceDetail=@PriceDetail where BuildingId=@BuildingId and Fxt_CompanyId=@FxtCompanyId";
                            return conn.Execute(sql, new { PriceDetail = priceDetail, BuildingId = buildId, FxtCompanyId = fxtcompanyId });
                        }
                        sql = "select BuildingId from  " + btable + "  with(nolock) where BuildingId=@BuildingId and FxtCompanyId=@FxtCompanyId";
                        var result = conn.Query<DAT_Building>(sql, new { BuildingId = buildId, FxtCompanyId = fxtcompanyId });
                        if (result != null && result.Any())
                        {
                            sql = "update " + btable + " with(rowlock) set PriceDetail=@PriceDetail where BuildingId=@BuildingId and FxtCompanyId=@FxtCompanyId";
                            return conn.Execute(sql, new { PriceDetail = priceDetail, BuildingId = buildId, FxtCompanyId = fxtcompanyId });
                        }
                        else
                        {
                            sql = @"insert into " + btable + @"_sub([BuildingId],[ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],
                                            [SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[Fxt_CompanyId],
                                            CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,
                                            Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,
                                            DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,
                                            Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, PodiumBuildingArea, 
                                            TowerBuildingArea, BasementArea, BasementPurpose, HouseNumber, HouseArea, OtherNumber, OtherArea, 
                                            innerFitmentCode, FloorHouseNumber, LiftNumber, LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode
                                            ,subaverageprice,salelicence,licencedate,joindate, MaintenanceCode,isTotalFloor) 
                                      select [BuildingId],[ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode], [BuildDate],
                                                [SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],[IsEValue],[AveragePrice],'" + userName + @"' as [SaveUser],'" + currfxtcompanyId + @"' as FxtCompanyId,
                                                '" + cityId + @"' as CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,
                                            Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,'" + priceDetail + @"' as PriceDetail,BHouseTypeCode,BHouseTypeWeight,
                                            Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,
                                            Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, PodiumBuildingArea, TowerBuildingArea, 
                                            BasementArea, BasementPurpose, HouseNumber, HouseArea, OtherNumber, OtherArea, innerFitmentCode, FloorHouseNumber, 
                                            LiftNumber, LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode,subaverageprice,salelicence,licencedate,joindate ,MaintenanceCode,isTotalFloor
                                      from " + btable + @" with(nolock) where BuildingId='" + buildId + @"' and CityId='" + cityId + @"'";
                            return conn.Execute(sql);
                        }

                    }
                }
            }
            return 0;
        }

        public int BatchSetEvalue(DAT_Building building)
        {
            var where = string.Empty;
            var where1 = string.Empty;
            if (!string.IsNullOrEmpty(building.buildingname))
            {
                where += " and b.BuildingName like @BuildingName";
                where1 += " and BuildingName like @BuildingName";
            }

            if (building.purposecode > 0)
            {
                where += " and b.PurposeCode=@PurposeCode";
                where1 += " and PurposeCode=@PurposeCode";
            }

            if (building.buildingtypecode > 0)
            {
                where += " and b.BuildingTypeCode=@BuildingTypeCode";
                where1 += " and BuildingTypeCode=@BuildingTypeCode";
            }


            var dt = GetCityTable(building.cityid, building.fxtcompanyid).FirstOrDefault();
            var buidlingTable = dt == null ? "" : dt.buildingtable;
            var companyId = dt == null
                ? building.fxtcompanyid.ToString()
                : string.IsNullOrEmpty(dt.ShowCompanyId) ? building.fxtcompanyid.ToString() : dt.ShowCompanyId;

            var companyIds = companyId.Split(',');

            //只有自己公司的companyId
            //说明当前要批量修改的数据都是自己公司的数据
            if (companyIds.Count() == 1) return SetSelf(buidlingTable, building, where1);

            var ret = 0;
            foreach (var item in companyIds)
            {
                //如果当前companyId为自己公司就更新主表和子表
                //否，则把主表的数据复制到子表，再进行修改
                if (item == building.fxtcompanyid.ToString())
                {
                    ret = SetSelf(buidlingTable, building, where1);
                }
                else
                {
                    ret += SetOther(buidlingTable, building, int.Parse(item), where, where1);
                }

            }

            return ret;

        }

        private static int SetSelf(string buidlingTable, DAT_Building building, string where)
        {
            var strSql = "update " + buidlingTable + "  with(rowlock) set isEvalue = @isEvalue where FxtCompanyId=@FxtCompanyId and cityId = @cityId and valid = 1 and projectId=@projectId " + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, building);
            }
        }

        private static int SetOther(string buidlingTable, DAT_Building building, int fxtCompanyId, string where, string where1)
        {

            var strSql = @"insert into " + buidlingTable + "_Sub([BuildingId],[BuildingName],[ProjectId],[PurposeCode],[StructureCode],[BuildingTypeCode],[TotalFloor],[FloorHigh],[SaleLicence],[ElevatorRate],[UnitsNumber],[TotalNumber],[TotalBuildArea],[BuildDate],[SaleDate],[AveragePrice],[AverageFloor],[JoinDate],[LicenceDate],[OtherName],[Weight],[IsEValue],[CityID],[CreateTime],[OldId],[Valid],[SalePrice],[SaveDateTime],[SaveUser],[LocationCode],[SightCode],[FrontCode],[StructureWeight],[BuildingTypeWeight],[YearWeight],[PurposeWeight],[LocationWeight],[SightWeight],[FrontWeight],[Fxt_CompanyId],[X],[Y],[XYScale],[Wall],[IsElevator],[SubAveragePrice],[PriceDetail],[BHouseTypeCode],[BHouseTypeWeight],[Creator],[Distance],[DistanceWeight],[basement],[Remark],[ElevatorRateWeight],[IsYard],[YardWeight],[Doorplate],[RightCode],[IsVirtual],[FloorSpread],[PodiumBuildingFloor],[PodiumBuildingArea],[TowerBuildingArea],[BasementArea],[BasementPurpose],[HouseNumber],[HouseArea],[OtherNumber],[OtherArea],[innerFitmentCode],[FloorHouseNumber],[LiftNumber],[LiftBrand],[Facilities],[PipelineGasCode],[HeatingModeCode],[WallTypeCode],MaintenanceCode,isTotalFloor)  ";
            strSql += @" SELECT [BuildingId],[BuildingName],[ProjectId],[PurposeCode],[StructureCode],[BuildingTypeCode],[TotalFloor],[FloorHigh],[SaleLicence],[ElevatorRate],[UnitsNumber],[TotalNumber],[TotalBuildArea],[BuildDate],[SaleDate]=getdate(),[AveragePrice],[AverageFloor],[JoinDate],[LicenceDate],[OtherName],[Weight],[IsEValue]=" + building.isevalue + @",[CityID],[CreateTime],[OldId],[Valid],[SalePrice],[SaveDateTime],[SaveUser],[LocationCode],[SightCode],[FrontCode],[StructureWeight],[BuildingTypeWeight],[YearWeight],[PurposeWeight],[LocationWeight],[SightWeight],[FrontWeight],[FxtCompanyId]=" + building.fxtcompanyid + @",[X],[Y],[XYScale],[Wall],[IsElevator],[SubAveragePrice],[PriceDetail],[BHouseTypeCode],[BHouseTypeWeight],[Creator],[Distance],[DistanceWeight],[basement],[Remark],[ElevatorRateWeight],[IsYard],[YardWeight],[Doorplate],[RightCode],[IsVirtual],[FloorSpread],[PodiumBuildingFloor],[PodiumBuildingArea],[TowerBuildingArea],[BasementArea],[BasementPurpose],[HouseNumber],[HouseArea],[OtherNumber],[OtherArea],[innerFitmentCode],[FloorHouseNumber],[LiftNumber],[LiftBrand],[Facilities],[PipelineGasCode],[HeatingModeCode],[WallTypeCode],MaintenanceCode,isTotalFloor
FROM " + buidlingTable + " b with(nolock) where b.FxtCompanyId= " + fxtCompanyId + " and b.cityId = @cityId and b.valid = 1 and b.projectId=@projectId and not exists(select buildingId from " + buidlingTable + "_sub bs with(nolock) where bs.buildingId = b.buildingId and bs.Fxt_CompanyId = @FxtCompanyId and bs.cityId = @cityId )" + where;

            var updateSubSql = "  update " + buidlingTable + "_Sub with(rowlock)  set isEvalue = @isEvalue where Fxt_CompanyId=@FxtCompanyId and cityId = @cityId and valid = 1 and projectId=@projectId " + where1;


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var ret = conn.Execute(strSql, building);
                ret += conn.Execute(updateSubSql, building);

                return ret;
            }

        }

        //        public DAT_Project GetSingleProjectByProjectId(int projectId, int cityId, int fxtcompanyId)
        //        {
        //            var dt = GetCityTable(cityId, fxtcompanyId).FirstOrDefault();
        //            if (dt == null) return new DAT_Project();

        //            var companyId = string.IsNullOrEmpty(dt.ShowCompanyId) ? fxtcompanyId.ToString() : dt.ShowCompanyId;
        //            var projectTable = dt.projecttable;

        //            var strsql = @"select p.projectid,p.projectname,p.averageprice from " + projectTable + @" p with(nolock) where projectId=@projectId and CityId=@CityId and p.FxtCompanyId in (" + companyId + @")
        //                                  and not exists (select ProjectId from " + projectTable + @"_sub ps with(nolock) 
        //                                  where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@FxtCompanyId 
        //                                  and ps.CityId=p.CityId and ps.AreaId=p.AreaId ) 
        //                            union 
        //                           select  p.projectid,p.projectname,p.averageprice  from " + projectTable + @"_sub p with(nolock) where projectId=@projectId  and CityId=@CityId and p.Fxt_CompanyId=@FxtCompanyId";
        //            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //            {
        //                return con.Query<DAT_Project>(strsql, new { projectId = projectId, CityId = cityId, FxtCompanyId = fxtcompanyId }).AsQueryable().FirstOrDefault();

        //            }
        //        }

        //public int AddBuildingSearch(string tablename, int cityid, int fxtcompanyid, int buildingId, int projectId, string buildingName, string otherName, string doorplate)
        //{
        //    //先删除，后新增
        //    string deletesql = "delete from " + tablename + @" WHERE BuildingId=@BuildingId and CityId=@CityId and FxtCompanyId =@fxtcompanyid";
        //    string insertsql = @"insert into " + tablename + @" WITH (ROWLOCK) (BuildingId,BuildingName,OtherName,Doorplate,ProjectId,CityID,FxtCompanyId) VALUES (@BuildingId,@BuildingName,@OtherName,@Doorplate,@ProjectId,@CityID,@FxtCompanyId)";
        //    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //    {
        //        var d = con.Execute(deletesql, new { buildingId, cityid, fxtcompanyid });
        //        var r = con.Execute(insertsql, new { buildingId, buildingName, otherName, doorplate, projectId, cityid, fxtcompanyid });
        //        return r;
        //    }
        //}

        //public int AddBuildingSubSearch(string tablename, int cityid, int fxtcompanyid, int buildingId, int projectId, string buildingName, string otherName, string doorplate)
        //{
        //    //先删除，后新增
        //    string deletesql = "delete from " + tablename + @" WHERE BuildingId=@BuildingId and CityId=@CityId and FxtCompanyId =@fxtcompanyid";
        //    string insertsql = @"insert into " + tablename + @" WITH (ROWLOCK) (BuildingId,BuildingName,OtherName,Doorplate,ProjectId,CityID,FxtCompanyId) VALUES (@BuildingId,@BuildingName,@OtherName,@Doorplate,@ProjectId,@CityID,@FxtCompanyId)";
        //    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //    {
        //        var d = con.Execute(deletesql, new { buildingId, cityid, fxtcompanyid });
        //        var r = con.Execute(insertsql, new { buildingId, buildingName, otherName, doorplate, projectId, cityid, fxtcompanyid });
        //        return r;
        //    }
        //}

        public IQueryable<DAT_Building> GetBuildingIds(int cityId, int fxtCompanyId)
        {
            var cityTable = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            if (cityTable == null) return new List<DAT_Building>().AsQueryable();


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var selectSql = @"select ProjectId, BuildingId,BuildingName from " + cityTable.buildingtable + @" b with(nolock)  where b.Valid=1 and b.CityID=@CityID and  b.FxtCompanyId in(" + cityTable.ShowCompanyId + @") and not exists (select BuildingId from " + cityTable.buildingtable + @"_sub bs with(nolock) where b.BuildingId=bs.BuildingId and bs.Fxt_CompanyId=@FxtCompanyId and bs.CityId=@CityID) 
union  
select ProjectId, BuildingId,BuildingName from " + cityTable.buildingtable + @"_sub b with(nolock) where b.Valid=1 and b.CityID=@CityID and b.Fxt_CompanyId=@FxtCompanyId ";

                return conn.Query<DAT_Building>(selectSql, new { cityId, fxtCompanyId }).AsQueryable();
            }
        }

        //复制楼栋图片
        public int AddBuildingPhoto(int projectId, int buildingId, int currfxtCompanyId, int buildingIdFrom, int fxtCompanyId, int cityId)
        {
            try
            {
                string sql = @"
insert into FXTProject.dbo.LNK_P_Photo(ProjectId,PhotoTypeCode,[Path],PhotoDate,PhotoName,CityId,Valid,FxtCompanyId,BuildingId,X,Y)
SELECT @ProjectId as ProjectId,PhotoTypeCode,[Path],GETDATE() as PhotoDate,PhotoName,CityId,1 as Valid,@CurrfxtCompanyId as FxtCompanyId,@BuildingId as BuildingId,X,Y
FROM FXTProject.dbo.LNK_P_Photo p WITH (NOLOCK)
WHERE valid = 1
	AND BuildingId = @BuildingIdFrom
	AND CityId = @CityId
	AND NOT EXISTS (
		SELECT ps.Id
		FROM FXTProject.dbo.LNK_P_Photo_sub ps WITH (NOLOCK)
		WHERE p.Id = ps.Id
			AND ps.FxtCompanyId = @FxtCompanyId
			AND ps.CityId = p.CityId
		)
UNION
SELECT @ProjectId as ProjectId,PhotoTypeCode,[Path],GETDATE() as PhotoDate,PhotoName,CityId,1 as Valid,@CurrfxtCompanyId as FxtCompanyId,@BuildingId as BuildingId,X,Y
FROM FXTProject.dbo.LNK_P_Photo_sub p WITH (NOLOCK)
WHERE valid = 1
	AND BuildingId = @BuildingIdFrom
	AND CityId = @CityId
	AND p.FxtCompanyId = @FxtCompanyId";

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    return con.Execute(sql, new { ProjectId = projectId, BuildingId = buildingId, CurrfxtCompanyId = currfxtCompanyId, BuildingIdFrom = buildingIdFrom, FxtCompanyId = fxtCompanyId, CityId = cityId });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
