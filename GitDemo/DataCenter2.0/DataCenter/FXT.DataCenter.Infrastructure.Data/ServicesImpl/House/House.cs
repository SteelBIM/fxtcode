using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Dictionary;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class DAT_HouseDAL : IDAT_House
    {
        private int FxtComId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);//房讯通

        private IQueryable<SYS_City_Table> GetCityTable(int CityId, int FxtCompanyId = 0)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
                {
                    if (FxtCompanyId > 0)
                    {
                        string strsql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],
                                   [QueryTaxTable],s.ShowCompanyId FROM [dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) 
                                    where c.CityId=@CityId and c.CityId=s.CityId and s.FxtCompanyId=@FxtCompanyId and typecode= 1003002";
                        var reslut = con.Query<SYS_City_Table>(strsql, new { CityId = CityId, FxtCompanyId = FxtCompanyId }).AsQueryable();
                        return reslut;
                    }
                    else
                    {
                        string strsql = "SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable] FROM [dbo].[SYS_City_Table] with(nolock) where CityId=@CityId";
                        var reslut = con.Query<SYS_City_Table>(strsql, new { CityId = CityId }).AsQueryable();
                        return reslut;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public IQueryable<DAT_House> GetHouseInfoByBuild(int BuildingId, int CityID, int FxtCompanyId)
        {
            string str_sql = "";
            var dt = GetCityTable(CityID, FxtCompanyId).FirstOrDefault();
            if (dt != null)
            {
                string h_table = dt.housetable,
                ComId = dt.ShowCompanyId;
                str_sql = @"
                            select h.HouseId, h.BuildingId, h.HouseName, h.HouseTypeCode, h.FloorNo, h.UnitNo, h.BuildArea, h.FrontCode, h.SightCode, h.UnitPrice, h.SalePrice, 
                                   h.Weight, h.PhotoName, h.Remark, h.StructureCode, h.TotalPrice, h.PurposeCode, h.IsEValue, h.CityID, h.OldId, h.CreateTime, h.Valid, h.SaveDateTime,
                                   h.SaveUser, h.FxtCompanyId, h.IsShowBuildingArea, h.InnerBuildingArea, h.SubHouseType, h.SubHouseArea, h.Creator, h.NominalFloor, h.VDCode, h.FitmentCode,
                                   h.Cookroom, h.Balcony, h.Toilet ,h.NoiseCode
                                   from  " + h_table + @" h  with(nolock) where h.CityID=@CityID and h.BuildingId=@BuildingId 
                                   and h.Valid=1 and h.FxtCompanyId in(" + ComId + @") and  not exists (select HouseId from " + h_table + @"_sub bs with(nolock) 
                                          where bs.[BuildingId]=@BuildingId and h.HouseId=bs.HouseId and bs.FxtCompanyId=@FxtCompanyId and bs.CityId=h.CityId) 
                            union  
                            select h.HouseId, h.BuildingId, h.HouseName, h.HouseTypeCode, h.FloorNo, h.UnitNo, h.BuildArea, h.FrontCode, h.SightCode, h.UnitPrice, h.SalePrice, 
                                   h.Weight, h.PhotoName, h.Remark, h.StructureCode, h.TotalPrice, h.PurposeCode, h.IsEValue, h.CityID, h.OldId, h.CreateTime, h.Valid, h.SaveDateTime,
                                   h.SaveUser, h.FxtCompanyId, h.IsShowBuildingArea, h.InnerBuildingArea, h.SubHouseType, h.SubHouseArea, h.Creator, h.NominalFloor, h.VDCode, h.FitmentCode,
                                   h.Cookroom, h.Balcony, h.Toilet ,h.NoiseCode
                                   from  " + h_table + @"_sub h  with(nolock) where h.CityID=@CityID and h.FxtCompanyId=@FxtCompanyId and h.BuildingId=@BuildingId and h.valid=1";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var houserList = con.Query<DAT_House>(str_sql, new { CityID = CityID, FxtCompanyId = FxtCompanyId, BuildingId = BuildingId }).AsQueryable();
                    return houserList;
                }
            }
            return new List<DAT_House>().AsQueryable();

        }

        public IQueryable<DAT_House> GetHouseNameList(int buildingId, int cityId, int fxtCompanyId)
        {

            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            var houseTable = dt.housetable;
            var companyIds = dt.ShowCompanyId;

            var strSql = @"SELECT h.HouseId,h.HouseName
                            FROM " + houseTable + @" h WITH (NOLOCK)
                            WHERE h.valid = 1
	                            AND h.BuildingId = @BuildingId  
	                            AND h.CityId = @CityId 
	                            And h.FxtCompanyId in (" + companyIds + @")
	                            AND NOT EXISTS (
		                            SELECT hs.cityId,hs.HouseId,hs.HouseName
		                            FROM " + houseTable + @"_sub hs WITH (NOLOCK)
		                            WHERE h.HouseId = hs.HouseId
			                            AND hs.FxtCompanyId = @FxtCompanyId
			                            AND hs.CityId = h.CityId
		                            )
                            UNION

                            SELECT h.HouseId,h.HouseName
                            FROM " + houseTable + @"_sub h WITH (NOLOCK)
                            WHERE h.valid = 1
	                            AND h.BuildingId = @BuildingId
	                            AND h.CityId = @CityId
	                            AND h.FxtCompanyId = @FxtCompanyId";

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var houserList = con.Query<DAT_House>(strSql, new { buildingId, cityId, fxtCompanyId }).AsQueryable();
                return houserList;
            }

        }

        // 得到房号列表
        public DataSet GetHouseListByBuildingId(int CityId, int FxtCompanyId, int BuildingId)
        {
            var city_table = GetCityTable(CityId, FxtCompanyId).FirstOrDefault();
            if (city_table == null) return null;
            string htable = "FXTProject." + city_table.housetable,
            btable = "FXTProject." + city_table.buildingtable,
            projtable = "FXTProject." + city_table.projecttable,
            ComId = city_table.ShowCompanyId;
            if (string.IsNullOrEmpty(ComId)) ComId = FxtCompanyId.ToString();
            #region -----------------------------------房号列表sql-----------------------------------
            string strsql = "select UnitNo from " + htable + " h with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId="
                    + CityId + "  and h.FxtCompanyId in(" + ComId + ")  and not exists (select HouseId from " + htable + "_sub hs with(nolock) where h.HouseId=hs.HouseId and hs.FxtCompanyId="
                    + FxtCompanyId + " and hs.CityId=h.CityId) group by  UnitNo union  "
                    + "select UnitNo from " + htable + "_sub with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId="
                    + CityId + " and FxtCompanyId=" + FxtCompanyId + " group by  UnitNo order by UnitNo "

                    + "select FloorNo from " + htable + " h with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId=" + CityId
                    + "  and h.FxtCompanyId in(" + ComId + ")  and not exists(select HouseId from " + htable + "_sub hs with(nolock) where h.HouseId=hs.HouseId and hs.FxtCompanyId=" + FxtCompanyId
                    + " and hs.CityId=h.CityId) group by FloorNo  union  "
                    + "select FloorNo from " + htable + "_sub with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId="
                    + CityId + " and FxtCompanyId=" + FxtCompanyId + " group by FloorNo order by FloorNo "

+ @"select 
	h.*,b.PriceDetail,b.AveragePrice,b.ProjectId,p.pAveragePrice
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = PurposeCode) as PurposeName
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = housetypeCode) as HouseTypeName
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = StructureCode) as StructureName
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = FrontCode) as FrontName
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = SightCode) as SightName
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = SubHouseType) as SubHouse
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = VDCode) as VDName
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = FitmentCode) as FitmentName 
	,(select top 1 CodeName from FxtDataCenter.dbo.Sys_Code with(nolock) where Code = NoiseCode) as NoiseCodeName 
from (
	SELECT HouseId,HouseName,PurposeCode,HouseTypeCode,StructureCode,BuildArea,UnitPrice,FloorNo,UnitNo,FrontCode,SightCode,FxtCompanyId,Weight
		,CASE IsEValue WHEN '1' THEN '是' ELSE '否' END IsEValue
		,case IsShowBuildingArea when '1' then '是' else '否' end IsShowBuildingArea
		,SubHouseType,SubHouseArea,BuildingId,NominalFloor,VDCode,FitmentCode
		,CASE Cookroom WHEN '1' THEN '有' when '0' then '无' ELSE '' END Cookroom
		,Balcony,Toilet,NoiseCode
	FROM " + htable + @" h WITH (NOLOCK)
	WHERE h.valid = 1
		AND h.BuildingId = " + BuildingId + @"
		AND h.CityId = " + CityId + @"
		AND h.FxtCompanyId IN (" + ComId + @")
		AND NOT EXISTS (
			SELECT HouseId
			FROM " + htable + @"_sub hs WITH (NOLOCK)
			WHERE h.HouseId = hs.HouseId
				AND hs.FxtCompanyId = " + FxtCompanyId + @"
				AND hs.CityId = h.CityId
			)
	UNION
	SELECT HouseId,HouseName,PurposeCode,HouseTypeCode,StructureCode,BuildArea,UnitPrice,FloorNo,UnitNo,FrontCode,SightCode,FxtCompanyId,Weight
		,CASE IsEValue WHEN '1' THEN '是' ELSE '否' END IsEValue
		,case IsShowBuildingArea when '1' then '是' else '否' end IsShowBuildingArea
		,SubHouseType,SubHouseArea,BuildingId,NominalFloor,VDCode,FitmentCode
		,CASE Cookroom WHEN '1' THEN '有' when '0' then '无' ELSE '' END Cookroom
		,Balcony,Toilet,NoiseCode
	FROM " + htable + @"_sub h WITH (NOLOCK)
	WHERE h.valid = 1
		AND h.BuildingId = " + BuildingId + @"
		AND h.CityId = " + CityId + @"
		AND FxtCompanyId = " + FxtCompanyId + @"
)h
inner join (
	select ProjectId,BuildingId,AveragePrice,PriceDetail 
	from " + btable + @" b with(nolock)
	where not exists(
		select BuildingId from " + btable + @"_sub bs with(nolock)
		where bs.BuildingId = b.BuildingId
		and bs.CityID = " + CityId + @"
		and bs.Fxt_CompanyId = " + FxtCompanyId + @"
	)
	and Valid = 1
	and CityID = " + CityId + @"
	and FxtCompanyId in (" + ComId + @")
	and BuildingId = " + BuildingId + @"
	union
	select ProjectId,BuildingId,AveragePrice,PriceDetail 
	from " + btable + @"_sub b with(nolock)
	where Valid = 1
	and CityID = " + CityId + @"
	and Fxt_CompanyId = " + FxtCompanyId + @"
	and BuildingId = " + BuildingId + @"
)b on h.BuildingId = b.BuildingId
inner join (
	select ProjectId,AveragePrice AS pAveragePrice
	from " + projtable + @" p with(nolock)
	where not exists(
		select ProjectId from " + projtable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = " + CityId + @"
		and ps.Fxt_CompanyId = " + FxtCompanyId + @"
	)
	and Valid = 1
	and CityID = " + CityId + @"
	and FxtCompanyId in (" + ComId + @")
	union
	select ProjectId,AveragePrice AS pAveragePrice
	from " + projtable + @"_sub p with(nolock)
	where Valid = 1
	and CityID = " + CityId + @"
	and Fxt_CompanyId = " + FxtCompanyId + @"
)p on b.ProjectId = p.ProjectId
ORDER BY FloorNo,UnitNo
"

                    + "select distinct (select UserName from privi_user where userid=h.Creator)Creator,Convert(varchar(10),CreateTime,120)CreateTime from " + htable + " h with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId="
                    + CityId + "  and h.FxtCompanyId in(" + ComId + ")  and not exists (select HouseId from " + htable + "_sub hs with(nolock) where h.HouseId=hs.HouseId and hs.FxtCompanyId="
                    + FxtCompanyId + " and hs.CityId=h.CityId) union  "
                    + "select distinct (select UserName from privi_user where userid=h.Creator)Creator,Convert(varchar(10),CreateTime,120)CreateTime from " + htable + "_sub h with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId="
                    + CityId + " and FxtCompanyId=" + FxtCompanyId + " order by CreateTime "

                    + "select distinct (select UserName from privi_user where userid=h.SaveUser)SaveUser,Convert(varchar(10),SaveDateTime,120)SaveDateTime,BuildingId from " + htable + " h with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId="
                    + CityId + "  and h.FxtCompanyId in(" + ComId + ")  and not exists (select HouseId from " + htable + "_sub hs with(nolock) where h.HouseId=hs.HouseId and hs.FxtCompanyId="
                    + FxtCompanyId + " and hs.CityId=h.CityId) union  "
                    + "select distinct (select UserName from privi_user where userid=h.SaveUser)SaveUser,Convert(varchar(10),SaveDateTime,120)SaveDateTime,BuildingId from " + htable + "_sub h with(nolock) where valid=1 and BuildingId=" + BuildingId.ToString() + " and CityId="
                    + CityId + " and FxtCompanyId=" + FxtCompanyId + " order by SaveDateTime desc ";
            #endregion
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
            var data = DBHelperSql.ExecuteDataSet(strsql);
            return data;
        }
        public IQueryable<DAT_House> GetUnitNo(int buildId, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
                string htable = city_table.housetable,
                ComId = city_table.ShowCompanyId;
                if (string.IsNullOrEmpty(ComId)) ComId = fxtCompanyId.ToString();
                string strsql = @"select UnitNo from " + htable + @" h with(nolock) 
                                              where valid=1 and BuildingId=@BuildingId and CityId=@CityId and h.FxtCompanyId in(" + ComId + @") 
                                                and not exists (select HouseId from " + htable + @"_sub hs with(nolock) where h.HouseId=hs.HouseId and hs.FxtCompanyId=@FxtCompanyId 
                                   and hs.CityId=h.CityId) group by  UnitNo union 
                                            select UnitNo from " + htable + @"_sub with(nolock) 
                                            where valid=1 and BuildingId=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId  group by  UnitNo order by UnitNo ";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    return con.Query<DAT_House>(strsql, new { BuildingId = buildId, CityId = cityId, FxtCompanyId = fxtCompanyId }).AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // 添加房号  buidlist：原始楼盘ID，buidlistTo：目标楼盘ID
        private int CopyHouse(IEnumerable<DAT_Building> buidlist, IEnumerable<DAT_Building> buidlistTo, string h_table)
        {
            foreach (var b_list in buidlist)
            {
                foreach (var b_listTo in buidlistTo)
                {
                    string sql = "@insert into " + h_table + @" ([BuildingId],[BuildingName])
                                       select " + b_listTo.buildingid + " as BuildingId,BuildingName from " + h_table + @"
                                       where BuildingId=" + b_list.buildingid + "";
                }
            }
            return 0;
        }

        // 新增房号
        public int AddHouse(DAT_House item)
        {
            try
            {
                var dt = GetCityTable(item.cityid, item.fxtcompanyid);
                if (dt == null || !dt.Any()) return 0;
                var sysCityTable = dt.FirstOrDefault();
                if (sysCityTable == null) return 0;

                var houseTable = sysCityTable.housetable;
                var buildingTable = sysCityTable.buildingtable;
                var companyIds = sysCityTable.ShowCompanyId;

                var strSql = @"
INSERT INTO " + houseTable + @" ([BuildingId],[HouseName],[HouseTypeCode],[FloorNo],[UnitNo],[BuildArea],[FrontCode],[SightCode],[UnitPrice],[SalePrice],[Weight],[PhotoName],[Remark],[StructureCode],[TotalPrice],[PurposeCode],[IsEValue],[CityID],[OldId],[CreateTime],[Valid],[FxtCompanyId],[IsShowBuildingArea],[InnerBuildingArea],[SubHouseType],[SubHouseArea],[Creator],[NominalFloor],[VDCode],[FitmentCode],[Cookroom],[Balcony],[Toilet],NoiseCode)
VALUES (@BuildingId,@HouseName,@HouseTypeCode,@FloorNo,@UnitNo,@BuildArea,@FrontCode,@SightCode,@UnitPrice,@SalePrice,@Weight,@PhotoName,@Remark,@StructureCode,@TotalPrice,@PurposeCode,@IsEValue,@CityID,@OldId,GETDATE(),@Valid,@FxtCompanyId,@IsShowBuildingArea,@InnerBuildingArea,@SubHouseType,@SubHouseArea,@Creator,@NominalFloor,@VDCode,@FitmentCode,@Cookroom,@Balcony,@Toilet,@NoiseCode) ";
                //查询楼栋总楼层的Sql
                var buildingSql = @"select b.TotalFloor 
                                    from FXTProject." + buildingTable + @" b with(nolock)
                                    where b.Valid = 1
                                    and b.CityID= @cityId
                                    and b.FxtCompanyId in (" + companyIds + @")
                                    and b.buildingId = @buildingId
                                    and not exists(select BuildingId from FXTProject." + buildingTable + @"_sub b1 with(nolock)
                                    where b.BuildingId = b1.BuildingId and b1.CityID=@cityId and b1.Fxt_CompanyId =@fxtCompanyId)
                                    union
                                    select b.TotalFloor
                                    from FXTProject." + buildingTable + @"_sub b with(nolock)
                                    where b.Valid = 1
                                    and b.CityID= @cityId
                                    and b.Fxt_CompanyId = @fxtCompanyId
                                    and b.buildingId = @buildingId";

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var result = con.Execute(strSql, item);

                    var buildingTotalfloor = con.Query<int>(buildingSql, new { item.cityid, item.fxtcompanyid, item.buildingid }).FirstOrDefault();
                    var totalFloor = buildingTotalfloor == 0 ? 1000 : buildingTotalfloor;

                    //如果房号的物理层大于楼栋的总楼层，则修改楼栋的总楼层为房号的物理层数

                    if (item.floorno > totalFloor)
                    {
                        var sql = "update FXTProject." + buildingTable + @"_sub with(rowlock) set totalfloor =" + item.floorno + ", SaveDateTime=getdate(),SaveUser='" + item.creator + @"' where buildingId=" + item.buildingid + " and cityId=" + item.cityid + " and fxt_CompanyId=" + item.fxtcompanyid;

                        var ret = con.Execute(sql);
                        if (ret == 0)
                        {
                            sql = "update FXTProject." + buildingTable + @" with(rowlock) set totalfloor =" + item.floorno + ", SaveDateTime=getdate(),SaveUser='" + item.creator + @"' where buildingId=" + item.buildingid + " and cityId=" + item.cityid + " and fxtCompanyId=" + item.fxtcompanyid;

                            ret = con.Execute(sql);
                            if (ret == 0)
                            {
                                sql = @"INSERT INTO " + buildingTable + @"_sub (
	[ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[Fxt_CompanyId],CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate,MaintenanceCode,isTotalFloor
	)
SELECT [ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber]," + item.floorno + ",[TotalNumber],[TotalBuildArea],[IsEValue],[AveragePrice],'" + item.creator + @"' AS [SaveUser],'" + item.fxtcompanyid + @"' AS FxtCompanyId,'" + item.cityid + @"' AS CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate,MaintenanceCode,isTotalFloor
FROM " + buildingTable + @" WITH (NOLOCK)
WHERE BuildingId = " + item.buildingid + @"
	AND CityId = " + item.cityid;

                                con.Execute(sql);
                            }
                        }

                    }

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        // 更新房号
        public int UpdateHouse(DAT_House item, int currFxtCompanyId)
        {
            var dt = GetCityTable(item.cityid, item.fxtcompanyid).FirstOrDefault();
            if (dt != null)
            {
                var h_table = "FXTProject." + dt.housetable;
                var comId = dt.ShowCompanyId;
                try
                {
                    var sql = string.Empty;
                    var mainTable = "Update " + h_table + " with(rowlock) ";
                    var subTable = "Update " + h_table + "_sub with(rowlock) ";

                    string house_attr = @" set buildingid = @buildingid,housename = @housename,housetypecode = @housetypecode,
floorno = @floorno,unitno = @unitno,buildarea = @buildarea,frontcode = @frontcode,
sightcode = @sightcode,unitprice = @unitprice,saleprice = @saleprice,weight = @weight,
photoname = @photoname,remark = @remark,structurecode = @structurecode,totalprice = @totalprice,
purposecode = @purposecode,isevalue = @isevalue,cityid = @cityid,
valid = @valid,savedatetime = getdate(),saveuser = @saveuser,
isshowbuildingarea = @isshowbuildingarea,innerbuildingarea = @innerbuildingarea,subhousetype = @subhousetype,
subhousearea = @subhousearea,vdcode = @vdcode,
fitmentcode = @fitmentcode,cookroom = @cookroom,balcony = @balcony,toilet = @toilet,NoiseCode = @NoiseCode,nominalfloor = @nominalfloor";

                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        if (item.houseid <= 0) //excel 导入相同数据时，要先把houseid查询出来 -- added by qiuyan
                        {
                            var strSql = @"
                            select h.HouseId
                        from " + h_table + @" h with (nolock)
                        where h.CityID = @CityID
	                        and h.BuildingId = @BuildingId
	                        and h.Valid = 1
                            and h.houseName = @houseName
                            and h.UnitNo = @unitNo
	                        and h.FxtCompanyId in (" + comId + @")
	                        and not exists (
		                        select HouseId
		                        from " + h_table + @"_sub bs with (nolock)
		                        where bs.[BuildingId] = @BuildingId
			                        and h.HouseId = bs.HouseId
			                        and bs.FxtCompanyId = @FxtCompanyId
			                        and bs.CityId = h.CityId
		                        )
                        union
                        select h.HouseId
                        from " + h_table + @"_sub h with (nolock)
                        where h.CityID = @CityID
	                        and h.FxtCompanyId = @FxtCompanyId
	                        and h.BuildingId = @BuildingId
                            and h.houseName = @houseName
                            and h.UnitNo = @unitNo
	                        and h.valid = 1";

                            item.houseid = con.Query<int>(strSql, item).FirstOrDefault();
                        }

                        if (currFxtCompanyId == FxtComId) //当前操作者房讯通, 只更新主表，不插入子表修改
                        {
                            sql = "delete from " + h_table + @"_sub with(rowlock) WHERE CityID = @CityID and HouseId = @houseid and FxtCompanyId = " + FxtComId;
                            con.Execute(sql, item);//删除掉子表中等于companyId=25的数据（以前的错误数据）

                            sql = mainTable + house_attr + " where HouseId=@houseid and CityId=@CityId";
                        }
                        else if (item.fxtcompanyid == currFxtCompanyId)
                        {
                            //本评估机构修改本评估机构数据时，先检查一下，主子表是否同时存在相同fxtcompanyid，相同Buildingid的数据。如果存在，先把子表的数据给删掉。20151123
                            string fxtcompanysql = "select * from " + h_table + " with(nolock) where HouseId = " + item.houseid + " and CityId = " + item.cityid + " and FxtCompanyId = " + currFxtCompanyId;
                            string fxt_companysql = "select * from " + h_table + "_sub with(nolock) where HouseId = " + item.houseid + " and CityId = " + item.cityid + " and FxtCompanyId = " + currFxtCompanyId;

                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            DataTable obj = DBHelperSql.ExecuteDataTable(fxtcompanysql);
                            DataTable obj1 = DBHelperSql.ExecuteDataTable(fxt_companysql);
                            if (obj != null && obj.Rows.Count == 1 && obj1 != null && obj1.Rows.Count == 1)
                            {
                                string deletefxt_companysql = "delete from " + h_table + @"_sub with(rowlock) WHERE HouseId = " + item.houseid + " and CityId = " + item.cityid + " and FxtCompanyId =" + currFxtCompanyId;
                                con.Execute(deletefxt_companysql);
                            }

                            var subSql = subTable + house_attr + "  WHERE CityID = @CityID and HouseId = @houseid and FxtCompanyId = @FxtCompanyId";

                            int r = con.Execute(subSql, item);
                            if (r < 1) //子表不存在就更新主表
                            {
                                sql = mainTable + house_attr + " where HouseId = @houseid and CityID = @CityID";
                            }
                        }
                        else
                        {
                            sql = @"SELECT HouseId FROM " + h_table + @"_sub with(nolock) 
                                   WHERE CityID = @CityID and HouseId = @houseid and FxtCompanyId = @FxtCompanyId";
                            SqlParameter[] parameters = {
                                             new SqlParameter("@houseid",item.houseid),
                                             new SqlParameter("@CityId",item.cityid),
                                             new SqlParameter("@FxtCompanyId",currFxtCompanyId),
                                         };
                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            DataTable obj = DBHelperSql.ExecuteDataTable(sql, parameters);
                            if (obj != null && obj.Rows.Count > 0)
                            {
                                sql = subTable + house_attr + " Where HouseId = @houseid and CityId=@CityId and FxtCompanyId = @FxtCompanyId"; ;
                            }
                            else
                            {
                                sql = @"
insert into " + h_table + @"_sub(HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice,Weight,PhotoName,Remark,StructureCode,TotalPrice,PurposeCode
,IsEValue,CityID,OldId,CreateTime,Valid,SaveDateTime,SaveUser,FxtCompanyId,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode)
select 
    HouseId,BuildingId,@HouseName,@HouseTypeCode,FloorNo,UnitNo,@BuildArea,@FrontCode,@SightCode,@UnitPrice,@SalePrice,@Weight,@PhotoName,@Remark,@StructureCode,@TotalPrice,@PurposeCode,@IsEValue
    ,CityID,OldId,CreateTime,1 as Valid,@SaveDateTime,@SaveUser,'" + currFxtCompanyId + @"' as FxtCompanyId,@IsShowBuildingArea,@InnerBuildingArea,@SubHouseType,@SubHouseArea
    ,Creator,@NominalFloor,@VDCode,@FitmentCode,@Cookroom,@Balcony,@Toilet,@NoiseCode
from " + h_table + @" with(nolock) where HouseId = @houseid and CityId=@CityId";
                            }
                        }
                        item.savedatetime = DateTime.Now;

                        if (!string.IsNullOrEmpty(sql))
                        {
                            return con.Execute(sql, item);
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            return 0;
        }

        public int UpdateHouse4Excel(DAT_House item, int currFxtCompanyId, List<string> modifiedProperty)
        {
            var dt = GetCityTable(item.cityid, item.fxtcompanyid).FirstOrDefault();
            if (dt == null) return 0;
            var hTable = "FXTProject." + dt.housetable;
            var comId = dt.ShowCompanyId;

            if (!modifiedProperty.Any()) return 0;

            var sql = string.Empty;
            var mainTable = "Update " + hTable + " with(rowlock) ";
            var subTable = "Update " + hTable + "_sub with(rowlock) ";
            var houseAttr = modifiedProperty.Aggregate(" set ", (current, m) => current + m);

            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    if (currFxtCompanyId == FxtComId) //当前操作者房讯通, 只更新主表，不插入子表修改
                    {
                        sql = "delete from " + hTable + @"_sub with(rowlock) WHERE CityID = @CityID and HouseId = @houseid and FxtCompanyId = " + FxtComId;
                        con.Execute(sql, item);//删除掉子表中等于companyId=25的数据（以前的错误数据）

                        sql = mainTable + houseAttr + " where HouseId = @houseid and CityId=@CityId";
                    }
                    else if (item.fxtcompanyid == currFxtCompanyId)
                    {
                        //本评估机构修改本评估机构数据时，先检查一下，主子表是否同时存在相同fxtcompanyid，相同Buildingid的数据。如果存在，先把子表的数据给删掉。20151123
                        string fxtcompanysql = "select * from " + hTable + " with(nolock) where HouseId = " + item.houseid + " and CityId = " + item.cityid + " and FxtCompanyId = " + currFxtCompanyId;
                        string fxt_companysql = "select * from " + hTable + "_sub with(nolock) where HouseId = " + item.houseid + " and CityId = " + item.cityid + " and FxtCompanyId = " + currFxtCompanyId;

                        DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                        DataTable obj = DBHelperSql.ExecuteDataTable(fxtcompanysql);
                        DataTable obj1 = DBHelperSql.ExecuteDataTable(fxt_companysql);
                        if (obj != null && obj.Rows.Count == 1 && obj1 != null && obj1.Rows.Count == 1)
                        {
                            string deletefxt_companysql = "delete from " + hTable + @"_sub with(rowlock) WHERE HouseId = " + item.houseid + " and CityId = " + item.cityid + " and FxtCompanyId =" + currFxtCompanyId;
                            con.Execute(deletefxt_companysql);
                        }

                        var subSql = subTable + houseAttr + "  WHERE CityID = @CityID and HouseId = @houseid and FxtCompanyId = @FxtCompanyId";

                        int r = con.Execute(subSql, item);
                        if (r < 1) //子表不存在就更新主表
                        {
                            sql = mainTable + houseAttr + " where HouseId = @houseid and CityID = @CityID";
                        }
                        else
                        {
                            return r;
                        }
                    }
                    else
                    {
                        sql = @"SELECT HouseId FROM " + hTable + @"_sub with(nolock) WHERE CityID = @CityID and HouseId = @houseid and FxtCompanyId = @FxtCompanyId";
                        SqlParameter[] parameters = {
                                             new SqlParameter("@houseid",item.houseid),
                                             new SqlParameter("@CityId",item.cityid),
                                             new SqlParameter("@FxtCompanyId",currFxtCompanyId),
                                         };
                        DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                        DataTable obj = DBHelperSql.ExecuteDataTable(sql, parameters);
                        if (obj != null && obj.Rows.Count > 0)
                        {
                            sql = subTable + houseAttr + " Where HouseId = @houseid and CityId=@CityId and FxtCompanyId = @FxtCompanyId"; ;
                        }
                        else
                        {
                            sql = @"
insert into " + hTable + @"_sub(HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice,Weight,PhotoName,Remark,StructureCode,TotalPrice,PurposeCode
,IsEValue,CityID,OldId,CreateTime,Valid,SaveDateTime,SaveUser,FxtCompanyId,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode)
select 
    HouseId,BuildingId,@HouseName,@HouseTypeCode,FloorNo,UnitNo,@BuildArea,@FrontCode,@SightCode,@UnitPrice,@SalePrice,@Weight,@PhotoName,@Remark,@StructureCode,@TotalPrice,@PurposeCode,@IsEValue
    ,CityID,OldId,CreateTime,1 as Valid,@SaveDateTime,@SaveUser,'" + currFxtCompanyId + @"' as FxtCompanyId,@IsShowBuildingArea,@InnerBuildingArea,@SubHouseType,@SubHouseArea
    ,Creator,@NominalFloor,@VDCode,@FitmentCode,@Cookroom,@Balcony,@Toilet,@NoiseCode
from " + hTable + @" with(nolock) where HouseId = @houseid and CityId=@CityId";
                        }
                    }
                    item.savedatetime = DateTime.Now;

                    if (!string.IsNullOrEmpty(sql))
                    {
                        return con.Execute(sql, item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        //        // 删除房号
        //        public int DelHouse(int cityId, int FxtCompanyId, int BuildingId, string userName, int currFxtCompanyId)
        //        {
        //            var dt = GetCityTable(cityId, FxtCompanyId).FirstOrDefault();
        //            if (dt != null)
        //            {
        //                string t_house = dt.housetable;
        //                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //                {
        //                    if (FxtCompanyId == FxtComId)
        //                    {
        //                        string strsql = @"update " + t_house + @" with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId 
        //                                  and FxtCompanyId=@FxtCompanyId";
        //                        int result = con.Execute(strsql, new { SaveUser = userName, BuildingId = BuildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
        //                        return result;
        //                    }
        //                    if (FxtCompanyId == currFxtCompanyId)
        //                    {
        //                        string strsql = @"update " + t_house + @" with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId 
        //                                  and FxtCompanyId=@FxtCompanyId";
        //                        int result = con.Execute(strsql, new { SaveUser = userName, BuildingId = BuildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
        //                        return result;
        //                    }
        //                    else
        //                    {
        //                        string strsql = @"select CityID,FxtCompanyId,Valid,BuildingId from " + t_house + @"_sub with(nolock) where [BuildingId]=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
        //                        var result_sub = con.Query<DAT_House>(strsql, new { BuildingId = BuildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
        //                        if (result_sub != null && result_sub.Count() > 0)
        //                        {
        //                            strsql = @"update " + t_house + @"_sub with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId 
        //                                  and FxtCompanyId=@FxtCompanyId";
        //                            int res = con.Execute(strsql, new { SaveUser = userName, BuildingId = BuildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
        //                            return res;
        //                        }
        //                        else
        //                        {
        //                            strsql = @"select CityID,FxtCompanyId,Valid,BuildingId from " + t_house + @" with(nolock) where [BuildingId]=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
        //                            var result = con.Query<DAT_House>(strsql, new { BuildingId = BuildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
        //                            if (result != null && result.Count() > 0)
        //                            {
        //                                strsql = @"update " + t_house + @" with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where [BuildingId]=@BuildingId and CityId=@CityId 
        //                                  and FxtCompanyId=@FxtCompanyId";
        //                                int res = con.Execute(strsql, new { SaveUser = userName, BuildingId = BuildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
        //                                return res;
        //                            }
        //                            else
        //                            {
        //                                strsql = @"INSERT INTO " + t_house + @"_sub with(rowlock) 
        //                                            ([HouseId],[BuildingId],[HouseName],[HouseTypeCode],[FloorNo],[UnitNo],[BuildArea]
        //           ,[FrontCode],[SightCode],[UnitPrice],[SalePrice],[Weight],[PhotoName],[Remark],[StructureCode],[TotalPrice],[PurposeCode],[IsEValue]
        //           ,[CityID],[OldId],[CreateTime],[Valid],[SaveDateTime],[SaveUser],[FxtCompanyId],[IsShowBuildingArea],[InnerBuildingArea],[SubHouseType],[SubHouseArea] 
        //           ,[Creator],[NominalFloor],[VDCode],[FitmentCode],[Cookroom],[Balcony],[Toilet])
        //            SELECT [HouseId],[BuildingId],[HouseName],[HouseTypeCode],[FloorNo],[UnitNo],[BuildArea]
        //           ,[FrontCode],[SightCode],[UnitPrice],[SalePrice],[Weight],[PhotoName],[Remark],[StructureCode],[TotalPrice],[PurposeCode],[IsEValue]
        //           ,[CityID],[OldId],[CreateTime],[Valid],GetDate(),'" + userName + @"' as [SaveUser],'" + currFxtCompanyId + @"' as [FxtCompanyId],[IsShowBuildingArea],[InnerBuildingArea],[SubHouseType],[SubHouseArea] 
        //           ,[Creator],[NominalFloor],[VDCode],[FitmentCode],[Cookroom],[Balcony],[Toilet] from " + t_house + @"  where [BuildingId]=@BuildingId and CityId=@CityId 
        //                                  and FxtCompanyId=@FxtCompanyId ";
        //                                int res = con.Execute(strsql, new { BuildingId = BuildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
        //                                return res;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            return 0;
        //        }

        // 获取房号数量
        public int GetHouseCount(int cityId, int fxtcompanyId, int projectId)
        {
            var city_table = GetCityTable(cityId, fxtcompanyId).FirstOrDefault();
            string htable = city_table.housetable,
            btable = city_table.buildingtable,
             ComId = city_table.ShowCompanyId;
            if (string.IsNullOrEmpty(ComId)) ComId = fxtcompanyId.ToString();
            string strSql = @"select HouseId from " + htable + @" h with(nolock) where h.BuildingId in(
            	                    select b.BuildingId from " + btable + @" b with(nolock)   
            	                    where b.CityID=@cityId and b.FxtCompanyId in(" + ComId + @") and b.Valid=1 and  b.ProjectId=@projectId and 
            	                    not exists(select sub.BuildingId from " + btable + @"_sub sub with(nolock) where b.CityID=sub.CityID 
                                                and sub.Fxt_CompanyId=@fxtcompanyId and  sub.ProjectId=@projectId and b.BuildingId=sub.BuildingId) 
            	                    union
            	                    select b.BuildingId from " + btable + @"_sub b with(nolock) 
            	                    where b.CityID=@cityId and b.Fxt_CompanyId=@fxtcompanyId and b.Valid=1 and  b.ProjectId=@projectId
                                ) and h.CityID=@cityId and h.FxtCompanyId in(" + ComId + @") and h.Valid=1  
                                    and not exists (select sub.houseId 
            			            from " + htable + @"_sub sub 
            			            with(nolock) 
            			            where h.CityID=sub.CityID and h.houseid=sub.houseid and 
                                    sub.FxtCompanyId=@fxtcompanyId)  
                              union 
                            select HouseId from " + htable + @"_sub h with(nolock)  where h.BuildingId in(
            	                    select b.BuildingId from " + btable + @" b with(nolock)   
            	                    where b.CityID=@cityId and b.FxtCompanyId in(" + ComId + @") and b.Valid=1 and  b.ProjectId=@projectId and 
            	                    not exists(select sub.BuildingId from " + btable + @"_sub sub with(nolock) where sub.CityID=b.CityID 
                                                and sub.Fxt_CompanyId=@fxtcompanyId and  sub.ProjectId=@projectId and b.BuildingId=sub.BuildingId) 
            	                    union
            	                    select BuildingId from " + btable + @"_sub b with(nolock) 
            	                    where b.CityID=@cityId and b.Fxt_CompanyId=@fxtcompanyId and b.Valid=1 and  b.ProjectId=@projectId
                                ) and h.CityID=@cityId and h.FxtCompanyId=@fxtcompanyId and h.Valid=1";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var list = conn.Query<DAT_House>(strSql, new { cityId = cityId, projectId = projectId, fxtcompanyId = fxtcompanyId });
                if (list == null || list.Count() == 0) return 0;
                else return list.Count();
            }

        }

        public DAT_House ValidateHouseNo(int cityId, int fxtCompanyId, int buildingId, string floorNo, string unitNo)
        {
            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            if (dt == null) return new DAT_House { houseid = 0 };
            var houseTable = dt.housetable;
            var comId = dt.ShowCompanyId;
            var strSql = @"
select h.HouseId,h.fxtcompanyid
from " + houseTable + @" h with (nolock)
where h.CityID = @CityID
	and h.BuildingId = @BuildingId
	and h.Valid = 1
    and h.floorNo = @floorNo
    and h.UnitNo = @unitNo
	and h.FxtCompanyId in (" + comId + @")
	and not exists (
		select HouseId
		from " + houseTable + @"_sub bs with (nolock)
		where bs.[BuildingId] = @BuildingId
			and h.HouseId = bs.HouseId
			and bs.FxtCompanyId = @FxtCompanyId
			and bs.CityId = h.CityId
		)
union
select h.houseid,h.fxtcompanyid
from " + houseTable + @"_sub h with (nolock)
where h.CityID = @CityID
	and h.FxtCompanyId = @FxtCompanyId
	and h.BuildingId = @BuildingId
    and h.floorNo = @floorNo
    and h.UnitNo = @unitNo
	and h.valid = 1";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<DAT_House>(strSql, new { cityId, fxtCompanyId, buildingId, floorNo, unitNo }).ToList().OrderByDescending(m => m.createtime).FirstOrDefault();

            }
        }

        public DAT_House ValidateHouseName(int cityId, int fxtCompanyId, int buildingId, string floorNo, string unitNo, string HouseName)
        {
            var dt = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            if (dt == null) return new DAT_House { houseid = 0 };
            var houseTable = dt.housetable;
            var comId = dt.ShowCompanyId;
            var strSql = @"
select h.HouseId,h.fxtcompanyid
from " + houseTable + @" h with (nolock)
where h.CityID = @CityID
	and h.BuildingId = @BuildingId
	and h.Valid = 1
    and h.floorNo = @floorNo
    and h.UnitNo = @unitNo
	and h.FxtCompanyId in (" + comId + @")
	and not exists (
		select HouseId
		from " + houseTable + @"_sub bs with (nolock)
		where bs.[BuildingId] = @BuildingId
			and h.HouseId = bs.HouseId
			and bs.FxtCompanyId = @FxtCompanyId
			and bs.CityId = h.CityId
		)
union
select h.houseid,h.fxtcompanyid
from " + houseTable + @"_sub h with (nolock)
where h.CityID = @CityID
	and h.FxtCompanyId = @FxtCompanyId
	and h.BuildingId = @BuildingId
    and h.floorNo = @floorNo
    and h.UnitNo = @unitNo
	and h.valid = 1";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<DAT_House>(strSql, new { cityId, fxtCompanyId, buildingId, floorNo, unitNo }).ToList().OrderByDescending(m => m.createtime).FirstOrDefault();

            }
        }

        public string GetProjectName(string buildId, int cityId, int fxtcompanyId)
        {
            var city_table = GetCityTable(cityId, fxtcompanyId);
            string btable = city_table.FirstOrDefault().buildingtable,
                ptable = city_table.FirstOrDefault().projecttable,
            strSql = @"select projectId from " + btable + " where BuildingId=@BuildingId",
                proSql = "select projectName from " + ptable + " where projectId=@projectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                int projectid = conn.Query<int>(strSql, new { BuildingId = buildId }).AsQueryable().FirstOrDefault();
                return conn.Query<string>(proSql, new { projectId = projectid }).AsQueryable().FirstOrDefault();
            }
        }

        public string GetBuildName(string buildId, int cityId, int fxtcompanyId)
        {
            var city_table = GetCityTable(cityId, fxtcompanyId);
            string btable = city_table.FirstOrDefault().buildingtable,
            strSql = @"select BuildingName from " + btable + " where BuildingId=@BuildingId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<string>(strSql, new { BuildingId = buildId }).AsQueryable().FirstOrDefault();
            }
        }

        // 房号新增
        public int AddHouseEndity(DAT_House item)
        {
            try
            {
                var dt = GetCityTable(item.cityid, item.fxtcompanyid).FirstOrDefault();
                if (dt == null) return 0;
                var hTable = dt.housetable;
                var buildingTable = dt.buildingtable;
                var companyIds = dt.ShowCompanyId;

                //插入到房号表的sql
                var strSql = @"INSERT INTO " + hTable + @" with(rowlock)(BuildingId,HouseName,BuildArea,PurposeCode,SubHouseType,SubHouseArea,HouseTypeCode,StructureCode,UnitPrice,Weight,FrontCode,SightCode,IsEValue,IsShowBuildingArea,VDCode,FitmentCode,Cookroom,Balcony,Toilet,UnitNo,CityID,Valid,FxtCompanyId,FloorNo,NominalFloor,NoiseCode,CreateTime,Creator)
VALUES (@BuildingId,@HouseName,@BuildArea,@PurposeCode,@SubHouseType,@SubHouseArea,@HouseTypeCode,@StructureCode,@UnitPrice,@Weight,@FrontCode,@SightCode,@IsEValue,@IsShowBuildingArea,@VDCode,@FitmentCode,@Cookroom,@Balcony,@Toilet,@UnitNo,@CityID,1,@FxtCompanyId,@FloorNo,@NominalFloor,@NoiseCode,@CreateTime,@Creator)";

                //查询楼栋总楼层的Sql
                var buildingSql = @"select b.TotalFloor 
                                    from FXTProject." + buildingTable + @" b with(nolock)
                                    where b.Valid = 1
                                    and b.CityID= @cityId
                                    and b.FxtCompanyId in (" + companyIds + @")
                                    and b.buildingId = @buildingId
                                    and not exists(select BuildingId from FXTProject." + buildingTable + @"_sub b1 with(nolock)
                                    where b.BuildingId = b1.BuildingId and b1.CityID=@cityId and b1.Fxt_CompanyId =@fxtCompanyId)
                                    union
                                    select b.TotalFloor
                                    from FXTProject." + buildingTable + @"_sub b with(nolock)
                                    where b.Valid = 1
                                    and b.CityID= @cityId
                                    and b.Fxt_CompanyId = @fxtCompanyId
                                    and b.buildingId = @buildingId";

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    var result = con.Execute(strSql, item);

                    var buildingTotalfloor = con.Query<int>(buildingSql, new { item.cityid, item.fxtcompanyid, item.buildingid }).FirstOrDefault();
                    var totalFloor = buildingTotalfloor == 0 ? 1000 : buildingTotalfloor;

                    //如果房号的物理层大于楼栋的总楼层，则修改楼栋的总楼层为房号的物理层数

                    if (item.floorno > totalFloor)
                    {
                        var sql = "update FXTProject." + buildingTable + @"_sub with(rowlock) set totalfloor =" + item.floorno + ", SaveDateTime=getdate(),SaveUser='" + item.creator + @"' where buildingId=" + item.buildingid + " and cityId=" + item.cityid + " and fxt_CompanyId=" + item.fxtcompanyid;

                        var ret = con.Execute(sql);
                        if (ret == 0)
                        {
                            sql = "update FXTProject." + buildingTable + @"  with(rowlock) set totalfloor =" + item.floorno + ", SaveDateTime=getdate(),SaveUser='" + item.creator + @"' where buildingId=" + item.buildingid + " and cityId=" + item.cityid + " and fxtCompanyId=" + item.fxtcompanyid;

                            ret = con.Execute(sql);
                            if (ret == 0)
                            {
                                sql = @"INSERT INTO " + buildingTable + @"_sub (
	[ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber],[TotalFloor],[TotalNumber],[TotalBuildArea],IsEValue,AveragePrice,[SaveUser],[Fxt_CompanyId],CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate,MaintenanceCode,isTotalFloor)
SELECT [ProjectId],[BuildingId],[BuildingName],[OtherName],[PurposeCode],[BuildingTypeCode],[BuildDate],[SaleDate],[SalePrice],[UnitsNumber]," + item.floorno + ",[TotalNumber],[TotalBuildArea],[IsEValue],[AveragePrice],'" + item.creator + @"' AS [SaveUser],'" + item.fxtcompanyid + @"' AS FxtCompanyId,'" + item.cityid + @"' AS CityId,AverageFloor,Weight,LocationCode,FrontCode,SightCode,YearWeight,LocationWeight,FrontWeight,SightWeight,BuildingTypeWeight,Wall,ElevatorRate,IsElevator,StructureCode,FloorHigh,X,Y,PriceDetail,BHouseTypeCode,BHouseTypeWeight,Creator,Distance,DistanceWeight,BaseMent,IsYard,YardWeight,ElevatorRateWeight,Remark,Doorplate,RightCode,IsVirtual,FloorSpread,PodiumBuildingFloor,PodiumBuildingArea,TowerBuildingArea,BasementArea,BasementPurpose,HouseNumber,HouseArea,OtherNumber,OtherArea,innerFitmentCode,FloorHouseNumber,LiftNumber,LiftBrand,Facilities,PipelineGasCode,HeatingModeCode,WallTypeCode,subaverageprice,salelicence,licencedate,joindate,MaintenanceCode,isTotalFloor
FROM " + buildingTable + @" WITH (NOLOCK)
WHERE BuildingId = " + item.buildingid + @"
	AND CityId = " + item.cityid;

                                con.Execute(sql);
                            }
                        }
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /*// 房号更新
        public int UpdateHouseEndity(DAT_House item, int currfxtcompanyId)
        {
            try
            {
                var dt = GetCityTable(item.cityid, item.fxtcompanyid).FirstOrDefault();
                if (dt != null)
                {
                    var h_table = dt.housetable;
                    string setsql = " set HouseName=@HouseName,BuildArea=@BuildArea,PurposeCode=@PurposeCode,SubHouseType=@SubHouseType,SubHouseArea=@SubHouseArea,HouseTypeCode=@HouseTypeCode,StructureCode=@StructureCode,UnitPrice=@UnitPrice,Weight=@Weight,FrontCode=@FrontCode,SightCode=@SightCode,IsEValue=@IsEValue,IsShowBuildingArea=@IsShowBuildingArea,VDCode=@VDCode,FitmentCode=@FitmentCode,Cookroom=@Cookroom,Balcony=@Balcony,Toilet=@Toilet,NoiseCode = @NoiseCode";
                    string where = " where BuildingId=@BuildingId and FloorNo=@FloorNo and HouseName=@HouseName and CityID=@CityID and FxtCompanyId=@FxtCompanyId";

                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        if (item.fxtcompanyid == FxtComId)//房讯通
                        {
                            var table = "update " + h_table + " with(rowlock) ";
                            return con.Execute(table + setsql + where, item);
                        }
                        if (item.fxtcompanyid == currfxtcompanyId)//自己
                        {
                            var table = new StringBuilder();
                            table.Append("update " + h_table + " with(rowlock) ");
                            return con.Execute(table + setsql + where, item);
                        }
                        else
                        {
                            var strSqlSub = new StringBuilder();
                            strSqlSub.Append("SELECT [HouseId] FROM " + h_table + @"_sub with(nolock) ");
                            strSqlSub.Append("WHERE [BuildingId]=@BuildingId and CityId=@CityId and [FxtCompanyId] =@FxtCompanyId and valid=1 ");
                            strSqlSub.Append("and HouseName=@HouseName and FloorNo=@FloorNo");
                            var resultSub = con.Query<DAT_House>(strSqlSub.ToString(), item);
                            if (resultSub != null && resultSub.Any())
                            {
                                return con.Execute("update " + h_table + "_sub with(rowlock) " + setsql.ToString() + where.ToString(), item);
                            }
                            var strSql = new StringBuilder();
                            strSql.Append("SELECT [HouseId] FROM " + h_table + @" with(nolock) ");
                            strSql.Append("WHERE [BuildingId]=@BuildingId and CityId=@CityId and [FxtCompanyId] =@FxtCompanyId and valid=1 ");
                            strSql.Append("and HouseName=@HouseName and FloorNo=@FloorNo");
                            var result = con.Query<DAT_House>(strSql.ToString(), item);
                            if (result != null && result.Any())
                            {
                                return con.Execute("update " + h_table + " with(rowlock) " + setsql.ToString() + where.ToString(), item);
                            }
                            else
                            {
                                var add = new StringBuilder();
                                add.Append("insert into " + h_table + @"_sub (");
                                add.Append("HouseId,buildingid,housename,housetypecode,floorno,unitno,buildarea,frontcode,sightcode,unitprice,saleprice,");
                                add.Append("weight,photoname,remark,structurecode,totalprice,purposecode,isevalue,cityid,oldid,createtime,valid,savedatetime,");
                                add.Append("saveuser,fxtcompanyid,isshowbuildingarea,innerbuildingarea,subhousetype,subhousearea,creator,nominalfloor,vdcode,");
                                add.Append("fitmentcode,cookroom,balcony,toilet,NoiseCode) ");
                                add.Append("SELECT ");
                                add.Append("HouseId,buildingid,housename,housetypecode,floorno,unitno,buildarea,frontcode,sightcode,unitprice,saleprice,");
                                add.Append("weight,photoname,remark,structurecode,totalprice,purposecode,isevalue,cityid,oldid,createtime,valid,getdate() as [SaveDateTime],");
                                add.Append("'" + item.saveuser + @"' as [SaveUser],'" + currfxtcompanyId + @"' as [FxtCompanyId],isshowbuildingarea,");
                                add.Append("innerbuildingarea,subhousetype,subhousearea,creator,nominalfloor,vdcode,");
                                add.Append("fitmentcode,cookroom,balcony,toilet,NoiseCode ");
                                add.Append("FROM  " + h_table + @" with(nolock) ");
                                add.Append("where BuildingId=@BuildingId and FloorNo=@FloorNo and HouseName=@HouseName and CityID=@CityID and FxtCompanyId=@FxtCompanyId");
                                return con.Execute(add.ToString(), item);
                            }

                        }
                    }

                }
                else { return 0; }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message); ;
            }
        }
        */

        // 房号删除
        public int DeleteHouseEndity(DAT_House item, int currfxtcompanyId)
        {
            var dt = GetCityTable(item.cityid, item.fxtcompanyid).FirstOrDefault();
            if (dt != null)
            {
                string t_house = "fxtproject." + dt.housetable;
                string strsql = string.Empty;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    //var attr = new StringBuilder();
                    //var where = new StringBuilder();
                    //attr.Append(" set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser ");
                    //where.Append(" where BuildingId=@BuildingId and FloorNo=@FloorNo ");
                    //where.Append(" and HouseName=@HouseName and CityID=@CityID and FxtCompanyId=@FxtCompanyId");
                    if (currfxtcompanyId == FxtComId)//当前操作者为房讯通
                    {
                        strsql = @"
if EXISTS(SELECT HouseId FROM " + t_house + "_sub with(nolock) WHERE BuildingId = @BuildingId and FloorNo = @FloorNo and HouseName = @HouseName and CityID = @CityID and FxtCompanyId = " + FxtComId + @")
begin
delete " + t_house + "_sub with(rowlock)  where BuildingId = @BuildingId and FloorNo = @FloorNo and HouseName = @HouseName and CityID = @CityID and FxtCompanyId = " + FxtComId + @"
end
";
                        strsql += " update " + t_house + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where BuildingId = @BuildingId and FloorNo = @FloorNo and HouseName = @HouseName and CityID = @CityID";

                    }
                    else if (item.fxtcompanyid == currfxtcompanyId)//自身评估机构的数据
                    {
                        strsql = "update " + t_house + " with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where BuildingId = @BuildingId and FloorNo = @FloorNo and HouseName = @HouseName and CityID = @CityID and [FxtCompanyId] = " + item.fxtcompanyid;

                        strsql += "  update " + t_house + "_sub with(rowlock) set [Valid]=0,[SaveDateTime]=GetDate(),[SaveUser]=@SaveUser where BuildingId = @BuildingId and FloorNo = @FloorNo and HouseName = @HouseName and CityID = @CityID and [FxtCompanyId] =" + item.fxtcompanyid;

                    }
                    else
                    {
                        strsql = @"
INSERT INTO " + t_house + @"_sub (HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice,Weight,PhotoName,Remark,StructureCode,TotalPrice,PurposeCode
,IsEValue,CityID,OldId,CreateTime,Valid,SaveDateTime,SaveUser,FxtCompanyId,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode)
SELECT 
    HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice,Weight,PhotoName,Remark,StructureCode,TotalPrice,PurposeCode,IsEValue,CityID,OldId,CreateTime
    ,0 as Valid
    ,getdate() as SaveDateTime
    ,@SaveUser as SaveUser
    ,'" + currfxtcompanyId + @"' as FxtCompanyId
    ,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode
FROM " + t_house + " with(nolock) where BuildingId = @BuildingId and FloorNo = @FloorNo and HouseName = @HouseName and CityID = @CityID";
                    }
                    int r = con.Execute(strsql, item);
                    return r;
                }
            }
            return 0;
        }

        // 获取房号信息
        public DAT_House GetHouseInfoById(int houseId, int fxtcompanyId, int cityId)
        {
            var dt = GetCityTable(cityId, fxtcompanyId).FirstOrDefault();
            if (dt != null)
            {
                var htable = dt.housetable;
                var ComId = dt.ShowCompanyId;
                if (string.IsNullOrEmpty(ComId)) ComId = fxtcompanyId.ToString();
                var sb = new StringBuilder();
                sb.Append("select HouseId, BuildingId, HouseName, HouseTypeCode, FloorNo, ");
                sb.Append("UnitNo, BuildArea, FrontCode, SightCode, UnitPrice, SalePrice, ");
                sb.Append("[Weight], PhotoName, Remark, StructureCode, TotalPrice, PurposeCode, ");
                sb.Append("IsEValue, CityID, OldId, CreateTime, Valid, SaveDateTime, SaveUser,");
                sb.Append("FxtCompanyId, IsShowBuildingArea, InnerBuildingArea, SubHouseType,");
                sb.Append("SubHouseArea, Creator, NominalFloor, VDCode, FitmentCode, Cookroom,Balcony, Toilet ,NoiseCode,FxtCompanyId as belongcompanyid,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=FxtCompanyId) ");
                sb.Append(" from " + htable + "  h with(nolock) ");
                sb.Append(" where valid=1 and HouseId=@HouseId and CityID=@CityID and FxtCompanyId in(" + ComId + ")  ");
                sb.Append(" and not exists(select HouseId from " + htable + "_sub sub with(nolock) where h.houseId=sub.houseId and h.cityId=sub.cityId and sub.FxtCompanyId=@FxtCompanyId) ");
                sb.Append(" union ");
                sb.Append("select h.HouseId, h.BuildingId, h.HouseName, h.HouseTypeCode, h.FloorNo, ");
                sb.Append("h.UnitNo, h.BuildArea, h.FrontCode, h.SightCode, h.UnitPrice, h.SalePrice, ");
                sb.Append("h.[Weight], h.PhotoName, h.Remark, h.StructureCode, h.TotalPrice, h.PurposeCode, ");
                sb.Append("h.IsEValue, h.CityID, h.OldId, h.CreateTime, h.Valid, h.SaveDateTime, h.SaveUser,");
                sb.Append("h.FxtCompanyId, h.IsShowBuildingArea, h.InnerBuildingArea, h.SubHouseType,");
                sb.Append("h.SubHouseArea, h.Creator, h.NominalFloor, h.VDCode, h.FitmentCode, h.Cookroom,h.Balcony, h.Toilet ,h.NoiseCode ,hi.FxtCompanyId,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=hi.FxtCompanyId) ");
                sb.Append(" from " + htable + "_sub  h with(nolock) left join " + htable + " hi with(nolock) on h.HouseId = hi.HouseId");
                sb.Append(" where h.valid=1 and h.HouseId=@HouseId and h.CityID=@CityID and h.FxtCompanyId=@FxtCompanyId");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    return con.Query<DAT_House>(sb.ToString(), new { HouseId = houseId, CityID = cityId, FxtCompanyId = fxtcompanyId }).FirstOrDefault();
                }
            }
            else
            {
                return new DAT_House();
            }
        }

        public int GetHouseId(int buildingId, string houseName, int cityId, int fxtCompanyId)
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
                var selectSql = @"
SELECT HouseId
FROM " + cityTable.housetable + @" h WITH (NOLOCK)
WHERE Valid = 1
	AND CityID = @CityID
	AND BuildingId = @BuildingId
	AND HouseName = @HouseName
	AND FxtCompanyId IN (" + cityTable.ShowCompanyId + @")
	AND NOT EXISTS (
		SELECT HouseId
		FROM " + cityTable.housetable + @"_sub hs WITH (NOLOCK)
		WHERE hs.HouseId = h.HouseId
			AND hs.FxtCompanyId = @fxtcompanyid
			AND hs.CityID = @CityID
		)
UNION
SELECT BuildingId
FROM " + cityTable.housetable + @"_sub h WITH (NOLOCK)
WHERE Valid = 1
	AND CityID = @CityID
	AND BuildingId = @BuildingId
	AND HouseName = @HouseName
	AND FxtCompanyId = @FxtCompanyId";

                var query = conn.Query<int>(selectSql, new { buildingId, houseName, cityId, fxtCompanyId }).AsQueryable();
                return query == null ? -1 : query.FirstOrDefault();
            }
        }

        //设置房号差  sussnum:返回，成功更新多少条房号，count：返回，总共有多少条房号。
        public void SetHouseRatio(int cityid, int fxtcompanyid, string saveUserName, int key, int projectid, string buildingids, out int sussnum, out int count)
        {
            sussnum = 0; count = 0;
            var dt = GetCityTable(cityid, fxtcompanyid).FirstOrDefault();
            if (dt == null)
            {
                sussnum = 0; count = 0;
            }
            var housetable = "FXTProject." + dt.housetable;
            var buildingtable = "FXTProject." + dt.buildingtable;
            var projecttable = "FXTProject." + dt.projecttable;
            var showcompanyIds = dt.ShowCompanyId;

            string where = string.Empty;
            where = (key == 0 ? "" : " and (Weight > 3  or Weight < 0.5 or Weight is null)");
            if (projectid > 0)
            {
                where += " and P.ProjectId = " + projectid;
            }
            if (!string.IsNullOrEmpty(buildingids))
            {
                where += " and B.BuildingId in (" + buildingids + ")";
            }

            string sql = @"
select *
into #temptable
from FXTProject.dbo.Sys_FloorPrice f where f.CityID = @cityid and f.fxtcompanyid = @fxtcompanyid and 是否百分比 = 1

select 
	HouseId
    ,fxtcompanyid
	,convert(numeric(18,4)
	    ,(case when BWeight is null or BWeight = 0 then 1 else BWeight end) * 
        ( 1
	    + isnull((select Price from FXTProject.dbo.sys_CodePrice c where c.CityID = @cityid and c.fxtcompanyid = @fxtcompanyid
		    and c.TypeCode = 1033005 and c.PurposeCode = 1002001 and c.SubCode = T.BuildAreaCode and c.Code = T.BuildingTypeCode),0) / 100
	    + isnull((select Price from FXTProject.dbo.sys_CodePrice c where c.CityID = @cityid and c.fxtcompanyid = @fxtcompanyid
		    and c.TypeCode = 1033001 and c.PurposeCode = 1002001 and c.Code = T.FrontCode),0) / 100
	    + isnull((select Price from FXTProject.dbo.sys_CodePrice c where c.CityID = @cityid and c.fxtcompanyid = @fxtcompanyid
		    and c.TypeCode = 1033002 and c.PurposeCode = 1002001 and c.Code = T.SightCode),0) / 100
	    + isnull((select Price from FXTProject.dbo.sys_CodePrice c where c.CityID = @cityid and c.fxtcompanyid = @fxtcompanyid
		    and c.TypeCode = 1033006 and c.PurposeCode = 1002001 and c.Code = T.VDCode),0) / 100
	    + isnull((select Price from FXTProject.dbo.sys_CodePrice c where c.CityID = @cityid and c.fxtcompanyid = @fxtcompanyid
		    and c.TypeCode = 1033004 and c.PurposeCode = 1002001 and c.Code = T.FitmentCode),0) / 100
	    + isnull((select 楼层差 from #temptable f where f.总楼层开始 <= T.TotalFloor and f.总楼层结束 >= T.TotalFloor 
		    and f.是否带电梯 = T.IsElevator and f.所在楼层 = T.FloorNo),0) / 100
        )
    ) as Weight
	,(select 楼层差 from #temptable f where f.总楼层开始 <= T.TotalFloor and f.总楼层结束 >= T.TotalFloor 
			and f.是否带电梯 = T.IsElevator and f.所在楼层 = T.FloorNo) as UnitPrice
from (
	select
		AreaID
		,P.ProjectId
		,ProjectName
		,B.BuildingId
		,BuildingName
		,TotalFloor
		,IsElevator
		,BuildingTypeCode
		,HouseId
		,HouseName
		,BWeight
		,Weight
		,UnitNo
		,FloorNo
		,BuildArea
		,(case when BuildArea < 30 then 8003001 
			when BuildArea >= 30 and BuildArea < 60 then 8003002
			when BuildArea >= 60 and BuildArea < 90 then 8003003
			when BuildArea >= 90 and BuildArea < 120 then 8003004
			when BuildArea >= 120 and BuildArea < 144 then 8003005
			when BuildArea >= 144 and BuildArea < 180 then 8003006
			when BuildArea >= 180 and BuildArea < 250 then 8003007
			when BuildArea >= 250 and BuildArea < 350 then 8003008
			when BuildArea >= 350 then 8003009 else null end) as BuildAreaCode
		,FrontCode
		,SightCode
		,VDCode
		,FitmentCode
		,FxtCompanyId
	from (
		select ProjectId,ProjectName,AreaID	from " + projecttable + @" p with(nolock)
		where not exists(
			select ProjectId from " + projecttable + @"_sub ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityID = @cityid
			and ps.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + showcompanyIds + @")
		union
		select ProjectId,ProjectName,AreaID from " + projecttable + @"_sub ps with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
	)P
	inner join
	(
		select ProjectId,BuildingId,BuildingName,TotalFloor,IsElevator,BuildingTypeCode,Weight as BWeight from " + buildingtable + @" b with(nolock)
		where not exists(
			select BuildingId from " + buildingtable + @"_sub bs with(nolock)
			where bs.BuildingId = b.BuildingId
			and bs.CityID = @cityid
			and bs.Fxt_CompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + showcompanyIds + @")
		union
		select ProjectId,BuildingId,BuildingName,TotalFloor,IsElevator,BuildingTypeCode,Weight as BWeight from " + buildingtable + @"_sub b with(nolock)
		where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
	)B on P.ProjectId = B.ProjectId
	inner join
	(
		select BuildingId,HouseId,HouseName,Weight,UnitNo,FloorNo,BuildArea,FrontCode,SightCode,VDCode,FitmentCode,FxtCompanyId from " + housetable + @" h with(nolock)
		where not exists(
			select HouseId from " + housetable + @"_sub hs with(nolock)
			where hs.HouseId = h.HouseId
			and hs.CityID = @cityid
			and hs.FxtCompanyId = @fxtcompanyid
		)
		and Valid = 1
		and CityID = @cityid
		and FxtCompanyId in (" + showcompanyIds + @")
		union
		select BuildingId,HouseId,HouseName,Weight,UnitNo,FloorNo,BuildArea,FrontCode,SightCode,VDCode,FitmentCode,FxtCompanyId from " + housetable + @"_sub b with(nolock)
		where Valid = 1
		and CityID = @cityid
		and FxtCompanyId = @fxtcompanyid
	)H on B.BuildingId = H.BuildingId
	where 1 = 1 " + where + @"
)T
drop table #temptable";

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var resultlist = con.Query<DAT_House>(sql, new { CityID = cityid, FxtCompanyId = fxtcompanyid }).AsQueryable();
                count = resultlist.Count();
                foreach (var result in resultlist)
                {
                    if (result.unitprice != null)
                    {
                        Log log = new Log();
                        var weightBe = GetHouseInfoById(result.houseid, fxtcompanyid, cityid).weight;
                        result.cityid = cityid;
                        result.saveuser = saveUserName;
                        sussnum += UpdateHouseWeight(result, fxtcompanyid);
                        var weightAf = result.weight;
                        log.InsertOperateLog(cityid, fxtcompanyid, SYS_Code_Dict.批量导入类型.住宅房号信息, "houseid", result.houseid, "weight", weightBe.ToString(), weightAf.ToString(), saveUserName);
                    }
                }
            }
        }

        public int UpdateHouseWeight(DAT_House house, int currFxtCompanyId)
        {
            try
            {
                var list = GetCityTable(house.cityid).FirstOrDefault();
                if (list != null)
                {
                    var htable = "FXTProject." + list.housetable;
                    var r = 0; //对主表或子表修改结果
                    var sql = string.Empty;

                    var mainTable = "Update " + htable + " with(rowlock) ";
                    var subTable = "Update " + htable + "_sub with(rowlock) ";
                    var where = " Where HouseId=@HouseId and CityId=@CityId and FxtCompanyId = @FxtCompanyId";
                    var updateFields = @" SET weight = @weight,SaveDateTime = GETDATE(),SaveUser = @saveuser";

                    if (currFxtCompanyId == FxtComId) //当前操作者房讯通, 只更新主表，不插入子表修改
                    {
                        sql = "delete from " + htable + @"_sub with(rowlock) WHERE HouseId=@HouseId and CityId=@CityId and FxtCompanyId =" + FxtComId;
                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            con.Execute(sql, new { HouseId = house.houseid, CityId = house.cityid });//删除掉子表中等于companyId=25的数据（以前的错误数据）
                        }

                        sql = mainTable + updateFields + " where HouseId=@HouseId and CityId=@CityId";
                    }
                    else if (house.fxtcompanyid == currFxtCompanyId)
                    {
                        //本评估机构修改本评估机构数据时，先检查一下，主子表是否同时存在相同fxtcompanyid，相同Buildingid的数据。如果存在，先把子表的数据给删掉。20151123
                        string fxtcompanysql = "select * from " + htable + " with(nolock) where HouseId = " + house.houseid + " and CityId = " + house.cityid + " and FxtCompanyId = " + currFxtCompanyId;
                        string fxt_companysql = "select * from " + htable + "_sub with(nolock) where HouseId = " + house.houseid + " and CityId = " + house.cityid + " and FxtCompanyId = " + currFxtCompanyId;
                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                            DataTable obj = DBHelperSql.ExecuteDataTable(fxtcompanysql);
                            DataTable obj1 = DBHelperSql.ExecuteDataTable(fxt_companysql);
                            if (obj != null && obj.Rows.Count == 1 && obj1 != null && obj1.Rows.Count == 1)
                            {
                                string deletefxt_companysql = "delete from " + htable + @"_sub with(rowlock) WHERE HouseId = " + house.houseid + " and CityId = " + house.cityid + " and FxtCompanyId =" + currFxtCompanyId;
                                con.Execute(deletefxt_companysql);
                            }
                        }

                        var subSql = subTable + updateFields + "  WHERE HouseId=@HouseId and CityId=@CityId and FxtCompanyId =@FxtCompanyId";

                        using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                        {
                            r = con.Execute(subSql, house);
                            if (r < 1) //子表不存在就更新主表
                            {
                                sql = mainTable + updateFields + " where HouseId=@HouseId and CityId=@CityId";
                            }
                        }
                    }
                    else
                    {
                        sql = @"SELECT HouseId FROM " + htable + @"_sub with(nolock) WHERE HouseId=@HouseId and CityId=@CityId and FxtCompanyId =@FxtCompanyId";
                        SqlParameter[] parameters = {
                                             new SqlParameter("@HouseId",house.houseid),
                                             new SqlParameter("@CityId",house.cityid),
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
insert into " + htable + @"_sub(HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice,Weight,PhotoName,Remark,StructureCode,TotalPrice,PurposeCode
,IsEValue,CityID,OldId,CreateTime,Valid,SaveDateTime,SaveUser,FxtCompanyId,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode)
SELECT HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice
,@Weight as Weight
,PhotoName
,Remark
,StructureCode,TotalPrice,PurposeCode,IsEValue,CityID
,OldId,CreateTime,Valid,GETDATE() as SaveDateTime,@saveuser as SaveUser," + currFxtCompanyId + @" as FxtCompanyId,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode
,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode
FROM " + htable + @" WITH (NOLOCK) WHERE HouseId = @HouseId AND CityId = @CityId";
                        }
                    }
                    using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                    {
                        if (!string.IsNullOrEmpty(sql))
                        {
                            r = con.Execute(sql, house);
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

        //设置VQ房号系数
        public int SetVQHouseRatio(int cityid, int fxtcompanyid, string saveUserName, int projectid, int parentProductTypeCode)
        {
            try
            {
                var dt = GetCityTable(cityid, fxtcompanyid).FirstOrDefault();
                if (dt == null)
                {
                    return 0;
                }
                var housetable = "FXTProject." + dt.housetable;
                var buildingtable = "FXTProject." + dt.buildingtable;
                var projecttable = "FXTProject." + dt.projecttable;
                var weighthousetable = projecttable.Replace("dbo.DAT_Project", "dbo.DAT_WeightHouse");
                int result = 0;
                string sql = @"
declare @table table(
	CityID int
	,ProjectId int
	,BuildingTypeCode int 
	,AvgWeight numeric(18,4)
)

insert into @table
select
	CityID
	,ProjectId
	,BuildingTypeCode
	,AVG(Weight) as AvgWeight
from (
	select
		CityID
		,ProjectId
		,BuildingId
		,HouseId
		,HouseName
		,UnitNo
		,FloorNo
		,Weight
		,SaveDateTime
		,BuildingTypeCode
	from (
		select
			ROW_NUMBER() over(partition by ProjectId,BuildingId,FloorNo,UnitNo order by ProjectId,BuildingId,FloorNo,UnitNo,SaveDateTime desc) as r1
			,*
		from (
			select
				B.ProjectId
				,B.BuildingTypeCode
				,H.*
			from (
				select 
					ProjectId,BuildingId,BuildingTypeCode
				from " + buildingtable + @" b with(nolock)
				where not exists(
					select * from " + buildingtable + @"_sub bs with(nolock)
					where bs.BuildingId = b.BuildingId
					and bs.CityID = @cityid
					and bs.Fxt_CompanyId = @fxtcompanyid
				)
				and Valid = 1
				and CityID = @cityid
				and FxtCompanyId in (select value from FXTProject.dbo.SplitToTable(
					(select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode)
					,','))
				and ProjectId = @projectid
				union
				select 	
					ProjectId,BuildingId,BuildingTypeCode
				from " + buildingtable + @"_sub b with(nolock)
				where Valid = 1
				and CityID = @cityid
				and Fxt_CompanyId = @fxtcompanyid
				and ProjectId = @projectid
			)B
			inner join (			
				select 
					HouseId,BuildingId,HouseName,FloorNo,UnitNo,CityID,SaveDateTime,Weight
				from " + housetable + @" h with(nolock)
				where not exists(
					select HouseId from " + housetable + @"_sub hs with(nolock)
					where hs.HouseId = h.HouseId
					and hs.CityID = @cityid
					and hs.FxtCompanyId = @fxtcompanyid
				)
				and Valid = 1
				and CityID = @cityid
				and FxtCompanyId in (select value from FXTProject.dbo.SplitToTable(
					(select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode)
					,','))
				union
				select 
					HouseId,BuildingId,HouseName,FloorNo,UnitNo,CityID,SaveDateTime,Weight
				from " + housetable + @"_sub h with(nolock)
				where Valid = 1
				and CityID = @cityid
				and FxtCompanyId = @fxtcompanyid
			)H on B.BuildingId = H.BuildingId
		)T
		where 1 = 1
		and BuildingId > 0
		and BuildingTypeCode > 0
	)T1
	where 1 = 1
	and T1.r1 = 1
)T
where Weight > 0
group by CityID,ProjectId,BuildingTypeCode

delete from " + weighthousetable + @" where CityId = @cityid and FxtCompanyId = 25 and ProjectId = @projectid

insert into " + weighthousetable + @" (FxtCompanyId,CityId,ProjectId,BuildingId,HouseId,Weight,UpdateDate,EvaluationCompanyId,UpdateUser)
select 
	25 as FxtCompanyId
	,CityId
	,ProjectId
	,BuildingId
	,HouseId
	,vqweight
	,GETDATE() as UpdateDate
    ,@fxtcompanyid as EvaluationCompanyId
    ,@username as UpdateUser
from (
	select
		a.*,b.AvgWeight,CONVERT(numeric(18,4),a.Weight / b.AvgWeight) as vqweight
	from (
		select * from (
			select
				p.CityID
				,p.AreaID
				,p.ProjectId
				,b.BuildingId
				,b.BuildingTypeCode
				,H.HouseId
				,H.FloorNo
				,H.UnitNo
				,H.HouseName
				,H.Weight
			from (
				select 
					ProjectId,AreaID,CityID
				from " + projecttable + @" p with(nolock)
				where not exists(
					select ProjectId from " + projecttable + @"_sub ps with(nolock)
					where ps.ProjectId = p.ProjectId
					and ps.CityID = @cityid
					and ps.Fxt_CompanyId = @fxtcompanyid
				)
				and Valid = 1
				and CityID = @cityid
				and FxtCompanyId in (select value from FXTProject.dbo.SplitToTable(
					(select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode)
					,','))
				and ProjectId = @projectid
				union
				select 
					ProjectId,AreaID,CityID
				from " + projecttable + @"_sub p with(nolock)
				where Valid = 1
				and CityID = @cityid
				and Fxt_CompanyId = @fxtcompanyid
				and ProjectId = @projectid
			)P
			inner join
			(
				select 
					ProjectId,BuildingId,BuildingTypeCode
				from " + buildingtable + @" b with(nolock)
				where not exists(
					select * from " + buildingtable + @"_sub bs with(nolock)
					where bs.BuildingId = b.BuildingId
					and bs.CityID = @cityid
					and bs.Fxt_CompanyId = @fxtcompanyid
				)
				and Valid = 1
				and CityID = @cityid
				and FxtCompanyId in (select value from FXTProject.dbo.SplitToTable(
					(select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode)
					,','))
				and ProjectId = @projectid
				union
				select 	
					ProjectId,BuildingId,BuildingTypeCode
				from " + buildingtable + @"_sub b with(nolock)
				where Valid = 1
				and CityID = @cityid
				and Fxt_CompanyId = @fxtcompanyid
				and ProjectId = @projectid
			)B on P.ProjectId = B.ProjectId
			inner join (
				select 
					HouseId,BuildingId,HouseName,FloorNo,UnitNo,CityID,Weight
				from " + housetable + @" h with(nolock)
				where not exists(
					select HouseId from " + housetable + @"_sub hs with(nolock)
					where hs.HouseId = h.HouseId
					and hs.CityID = @cityid
					and hs.FxtCompanyId = @fxtcompanyid
				)
				and Valid = 1
				and CityID = @cityid
				and FxtCompanyId in (select value from FXTProject.dbo.SplitToTable(
					(select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode)
					,','))
				union
				select 
					HouseId,BuildingId,HouseName,FloorNo,UnitNo,CityID,Weight
				from " + housetable + @"_sub h with(nolock)
				where Valid = 1
				and CityID = @cityid
				and FxtCompanyId = @fxtcompanyid
			)H on B.BuildingId = H.BuildingId
		)T
		where 1 = 1
		and T.Weight > 0
	)a
	inner join @table b on a.CityID = b.CityID and a.ProjectId = b.ProjectId and a.BuildingTypeCode = b.BuildingTypeCode
)T
where 1 = 1";

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    result = con.Execute(sql, new { cityid, fxtcompanyid, typecode = parentProductTypeCode, projectid, username = saveUserName });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IQueryable<DAT_House> ExportHouseList(int projectid, int buildingid, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
                string ptable = "FXTProject." + city_table.projecttable;
                string btable = "FXTProject." + city_table.buildingtable;
                string htable = "FXTProject." + city_table.housetable;
                string ComId = city_table.ShowCompanyId;
                if (string.IsNullOrEmpty(ComId)) ComId = fxtCompanyId.ToString();
                string strsql = @"
select 
	P.ProjectId
	,P.ProjectName
	,p.AreaID
	,a.AreaName
	,B.BuildingName
    ,P.Creator as ProjectCreator
    ,B.Creator as BuildingCreator
	,H.*
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.HouseTypeCode) as HouseTypeCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.FrontCode) as FrontCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.SightCode) as SightCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.StructureCode) as StructureCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.PurposeCode) as PurposeCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.SubHouseType) as SubHouseTypeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.VDCode) as VDCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.FitmentCode) as FitmentCodeName
	,(select CodeName from FxtDataCenter.dbo.SYS_Code c with(nolock) where c.Code = H.NoiseCode) as NoiseCodeName
	,(case when H.Cookroom = 1 then '是' when H.Cookroom = 0 then '否' else '' end) as CookroomName
	,(case when H.IsShowBuildingArea = 1 then '是' when H.IsShowBuildingArea = 0 then '否' else '' end) as IsShowBuildingAreaName
	,(case when H.IsEValue = 1 then '是' when H.IsEValue = 0 then '否' else '' end) as IsEValueName
from (
	select 
		ProjectId,ProjectName,AreaID,CityID,Creator
	from " + ptable + @" p with(nolock)
	where not exists(
		select ProjectId from " + ptable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = @cityid
		and ps.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + ComId + @")
	and ProjectId = @Projectid
	union
	select 
		ProjectId,ProjectName,AreaID,CityID,Creator
	from " + ptable + @"_sub p with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
	and ProjectId = @Projectid
)P
inner join
(
	select 
		ProjectId,BuildingId,BuildingName,Creator
	from " + btable + @" b with(nolock)
	where not exists(
		select * from " + btable + @"_sub bs with(nolock)
		where bs.BuildingId = b.BuildingId
		and bs.CityID = @cityid
		and bs.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + ComId + @")
	and BuildingId = @BuildingId
	union
	select 	
		ProjectId,BuildingId,BuildingName,Creator
	from " + btable + @"_sub b with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
	and BuildingId = @BuildingId
)B on P.ProjectId = B.ProjectId
inner join (
	select 
		HouseId,BuildingId,HouseName,HouseTypeCode,FloorNo,UnitNo,BuildArea,FrontCode,SightCode,UnitPrice,SalePrice,Weight,PhotoName,Remark,StructureCode,TotalPrice,PurposeCode,IsEValue,CityID,OldId,CreateTime,Valid,SaveDateTime,SaveUser,FxtCompanyId,IsShowBuildingArea,InnerBuildingArea,SubHouseType,SubHouseArea,Creator,NominalFloor,VDCode,FitmentCode,Cookroom,Balcony,Toilet,NoiseCode
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
	and FxtCompanyId in (" + ComId + @")
	and BuildingId = @BuildingId
	union
	select 
		h.HouseId,h.BuildingId,h.HouseName,h.HouseTypeCode,h.FloorNo,h.UnitNo,h.BuildArea,h.FrontCode,h.SightCode,h.UnitPrice,h.SalePrice,h.Weight,h.PhotoName,h.Remark,h.StructureCode,h.TotalPrice,h.PurposeCode,h.IsEValue,h.CityID,h.OldId,h.CreateTime,h.Valid,h.SaveDateTime,h.SaveUser,h.FxtCompanyId,h.IsShowBuildingArea,h.InnerBuildingArea,h.SubHouseType,h.SubHouseArea,h.Creator,h.NominalFloor,h.VDCode,h.FitmentCode,h.Cookroom,h.Balcony,h.Toilet,h.NoiseCode,belongcompanyname=(select CompanyName from FxtUserCenter.dbo.CompanyInfo where CompanyID=hi.FxtCompanyId)
	from " + htable + "_sub h with(nolock) left join " + htable + @" as hi with(nolock) on h.HouseId = hi.HouseId
	where h.Valid = 1
	and h.CityID = @cityid
	and h.FxtCompanyId = @fxtcompanyid
	and h.BuildingId = @BuildingId
)H on B.BuildingId = H.BuildingId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on P.AreaID = a.AreaId";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
                {
                    return con.Query<DAT_House>(strsql, new { projectid, buildingid, cityId, fxtCompanyId }).AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
