using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    /// <summary>
    /// 商业楼栋
    /// </summary>
    public class Dat_Building_BizDAL : IDat_Building_Biz
    {
        /// <summary>
        /// 房讯通
        /// </summary>
        private int FxtComId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);//房讯通
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
                    string strsql = "SELECT c.CityId, ProjectTable, BuildingTable, HouseTable, CaseTable, QueryInfoTable, ReportTable, PrintTable, HistoryTable, QueryTaxTable, CaseBusinessTable, CaseLandTable, QueryAdjustTable, QueryTaxSOATable, QueryFlowTable, CASHistoryTable, MessageTable, QueryCheckTable, ReportCheckTable, SurveyTable, SurveyBusinessTable, SurveyFactoryTable, SurveyLandTable, SurveyOfficeTable, SurveyOtherTable, QueryFilesTable, SurveyTextTable, SubHousePriceTable, QProjectTable, ReportEntrustTable, SurveyCaseTable, SearchHistoryTable, QueryYPDat, QueryYPCheckTable,s.BizCompanyId as ShowCompanyId FROM [dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and s.FxtCompanyId=@FxtCompanyId and typecode= 1003002";
                    return con.Query<SYS_City_Table>(strsql, new { CityId = CityId, FxtCompanyId = FxtCompanyId }).AsQueryable();
                }
                else
                {
                    string strsql = "SELECT CityId, ProjectTable, BuildingTable, HouseTable, CaseTable, QueryInfoTable, ReportTable, PrintTable, HistoryTable, QueryTaxTable, CaseBusinessTable, CaseLandTable, QueryAdjustTable, QueryTaxSOATable, QueryFlowTable, CASHistoryTable, MessageTable, QueryCheckTable, ReportCheckTable, SurveyTable, SurveyBusinessTable, SurveyFactoryTable, SurveyLandTable, SurveyOfficeTable, SurveyOtherTable, QueryFilesTable, SurveyTextTable, SubHousePriceTable, QProjectTable, ReportEntrustTable, SurveyCaseTable, SearchHistoryTable, QueryYPDat, QueryYPCheckTable FROM [dbo].[SYS_City_Table] with(nolock) where CityId=@CityId";
                    return con.Query<SYS_City_Table>(strsql, new { CityId = CityId }).AsQueryable();
                }
            }

        }
        /// <summary>
        /// 获取商业楼栋列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="self">true查看自己;false查看全部</param>
        /// <returns></returns>
        public IQueryable<Dat_Building_Biz> GetDat_Building_BizList(Dat_Building_Biz model, bool self = true)
        {
            try
            {
                var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId).FirstOrDefault();
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new List<Dat_Building_Biz>();
                //}
                string querStr = QqueryWhere(model),
                comId = city_table.ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = model.FxtCompanyId.ToString();
                StringBuilder strSql = new StringBuilder();
                string attr = @" b.BuildingId, b.StructureCode, b.BuildingTypeCode, b.ManagerPrice, b.ManagerTel, b.BuildingArea,
                                b.EndDate, b.OpenDate, b.RentSaleType, b.BizArea, b.BizFloor, b.CityId, b.UpFloorNum, b.DownFloorNum, b.Functional,
                                b.BizNum, b.LiftNum, b.LiftFitment, b.LiftBrand, b.EscalatorsNum, b.EscalatorsBrand, b.ToiletBrand, b.AreaId,
                                b.PublicFitment, b.WallFitment, b.TrafficType, b.TrafficDetails, b.ParkingLevel, b.AirConditionType, b.Details,
                                b.HouseID, b.OfficeID, b.East, b.SubAreaId, b.south, b.west, b.north, b.BuildingBizType, b.BizType, b.BusinessHours,
                                b.ProRoad, b.BizCutOff, b.Flows, b.CustomerType, b.ProjectId, b.IsBenchmarks, b.Creator, b.CreateTime, b.SaveDateTime,
                                b.SaveUser, b.PinYin, b.PinYinAll, b.Valid, b.FxtCompanyId, b.X, b.CorrelationType, b.Y, b.Remarks, b.ZZProjectId, b.BGProjectId, 
                                b.BuildingName, b.Address, b.FieldNo,b.averageprice,b.weight,
                                city.CityName CityName,area.AreaName AreaName,sub_biz.SubAreaName SubAreaName,
                                CorrelationType.CodeName CorrelationTypeName,StructureCode.CodeName StructureCodeName,
                                BuildingTypeCode.CodeName BuildingTypeCodeName,RentSaleType.CodeName RentSaleTypeName,
                                LiftFitment.CodeName LiftFitmentName,PublicFitment.CodeName PublicFitmentName,
                                WallFitment.CodeName WallFitmentName,TrafficType.CodeName TrafficTypeName,
                                ParkingLevel.CodeName ParkingLevelName,AirConditionType.CodeName AirConditionTypeName,
                                BuildingBizType.CodeName BuildingBizTypeName,BizType.CodeName BizTypeName,
                                ProRoad.CodeName ProRoadName,BizCutOff.CodeName BizCutOffName,
                                Flows.CodeName FlowsName,CustomerType.CodeName CustomerTypeName,
                               (case b.IsBenchmarks when 0 then '否' when 1 then '是' else '' end) as IsBenchmark,b.AveragePrice,b.Weight ";
                string leftTable = @" left join FxtDataCenter.dbo.SYS_City city with(nolock) on city.CityId=b.CityId  
                                      left join FxtDataCenter.dbo.SYS_Area area with(nolock) on area.AreaId=b.AreaId    
                                      left join [FXTDataCenter].[dbo].SYS_SubArea_Biz sub_biz with(nolock) on sub_biz.SubAreaId=b.SubAreaId  
                                      left join [FXTDataCenter].[dbo].SYS_Code CorrelationType with(nolock) on CorrelationType.code=b.CorrelationType  
                                      left join [FXTDataCenter].[dbo].SYS_Code StructureCode with(nolock) on StructureCode.code=b.StructureCode  
                                      left join [FXTDataCenter].[dbo].SYS_Code BuildingTypeCode with(nolock) on BuildingTypeCode.code=b.BuildingTypeCode  
                                      left join [FXTDataCenter].[dbo].SYS_Code RentSaleType with(nolock) on RentSaleType.code=b.RentSaleType  
                                      left join [FXTDataCenter].[dbo].SYS_Code LiftFitment with(nolock) on LiftFitment.code=b.LiftFitment  
                                      left join [FXTDataCenter].[dbo].SYS_Code PublicFitment with(nolock) on PublicFitment.code=b.PublicFitment  
                                      left join [FXTDataCenter].[dbo].SYS_Code WallFitment with(nolock) on WallFitment.code=b.WallFitment  
                                      left join [FXTDataCenter].[dbo].SYS_Code TrafficType with(nolock) on TrafficType.code=b.TrafficType  
                                      left join [FXTDataCenter].[dbo].SYS_Code ParkingLevel with(nolock) on ParkingLevel.code=b.ParkingLevel  
                                      left join [FXTDataCenter].[dbo].SYS_Code AirConditionType with(nolock) on AirConditionType.code=b.AirConditionType  
                                      left join [FXTDataCenter].[dbo].SYS_Code BuildingBizType with(nolock) on BuildingBizType.code=b.BuildingBizType  
                                      left join [FXTDataCenter].[dbo].SYS_Code BizType with(nolock) on BizType.code=b.BizType  
                                      left join [FXTDataCenter].[dbo].SYS_Code ProRoad with(nolock) on ProRoad.code=b.ProRoad  
                                      left join [FXTDataCenter].[dbo].SYS_Code BizCutOff with(nolock) on BizCutOff.code=b.BizCutOff  
                                      left join [FXTDataCenter].[dbo].SYS_Code Flows with(nolock) on Flows.code=b.Flows  
                                      left join [FXTDataCenter].[dbo].SYS_Code CustomerType with(nolock) on CustomerType.code=b.CustomerType  ";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    strSql.Append(" select ");
                    strSql.Append(attr);
                    strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz b with(nolock) ");
                    strSql.Append(leftTable);
                    strSql.Append(" where b.CityId=@CityId and b.Valid=1 and b.ProjectId=@ProjectId ");
                    strSql.Append(" and not exists(select sub.BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub sub with(nolock) where ");
                    strSql.Append(" sub.FxtCompanyId=@FxtCompanyId and b.BuildingId=sub.BuildingId and b.CityId=sub.CityId) ");
                    if (self)//查看自己
                    {
                        strSql.Append(" and b.FxtCompanyId=@FxtCompanyId  ");
                    }
                    else
                    {
                        strSql.Append(" and b.FxtCompanyId in (" + comId + ") ");
                    }
                    strSql.Append(querStr);
                    strSql.Append(" union ");
                    strSql.Append(" select ");
                    strSql.Append(attr);
                    strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(nolock) ");
                    strSql.Append(leftTable);
                    strSql.Append(" where b.CityId=@CityId and b.FxtCompanyId=@FxtCompanyId and b.Valid=1 and b.ProjectId=@ProjectId");
                    strSql.Append(querStr);
                    strSql.Append(" order by b.BuildingId desc");
                    //var Building_Bizlist = con.Query<Dat_Building_Biz>(strSql.ToString(), model);
                    var Building_Bizlist = con.Query<Dat_Building_Biz>(strSql.ToString(),
                        new
                        {
                            CityId = model.CityId,
                            ProjectId = model.ProjectId,
                            FxtCompanyId = model.FxtCompanyId,
                            BuildingName = "%" + model.BuildingName + "%",
                            BuildingBizType = model.BuildingBizType,
                            AreaId = model.AreaId,
                            SubAreaId = model.SubAreaId,
                            ProRoad = model.ProRoad,
                            BizCutOff = model.BizCutOff,
                            Flows = model.Flows,
                            OpenDate = model.OpenDate,
                            OpenDateEnd = model.OpenDateEnd
                        }).AsQueryable();
                    return Building_Bizlist;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public IQueryable<Dat_Building_Biz> GetBusinessBuildings(long projectId, int cityId, int fxtCompanyId)
        {
            var cityTable = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();

            var comId = cityTable == null ? "" : cityTable.ShowCompanyId;
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strSql = @"select b.BuildingId,b.buildingName 
                          from FxtData_Biz.dbo.Dat_Building_Biz b with(nolock)
                          where b.CityId=@CityId and b.Valid=1 and b.ProjectId=@ProjectId and b.FxtCompanyId in(" + comId + @")
                          and not exists(select sub.BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub sub with(rowlock) where sub.FxtCompanyId=@FxtCompanyId and b.BuildingId=sub.BuildingId and b.CityId=sub.CityId)  
                          union
                          select b.BuildingId,b.buildingName 
                          from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(nolock)
                         where b.CityId=@CityId and b.FxtCompanyId=@FxtCompanyId and b.Valid=1 and b.ProjectId=@ProjectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<Dat_Building_Biz>(strSql, new { projectId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        /// <summary>
        /// 获取商业楼栋信息
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <returns></returns>
        public Dat_Building_Biz GetDat_Building_BizById(int buildingId, int CityId, int FxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(CityId, FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Building_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = FxtCompanyId.ToString();
                StringBuilder strSql = new StringBuilder();
                string attr = @" b.BuildingId, b.StructureCode, b.BuildingTypeCode, b.ManagerPrice, b.ManagerTel, b.BuildingArea,
                                b.EndDate, b.OpenDate, b.RentSaleType, b.BizArea, b.BizFloor, b.CityId, b.UpFloorNum, b.DownFloorNum, b.Functional,
                                b.BizNum, b.LiftNum, b.LiftFitment, b.LiftBrand, b.EscalatorsNum, b.EscalatorsBrand, b.ToiletBrand, b.AreaId,
                                b.PublicFitment, b.WallFitment, b.TrafficType, b.TrafficDetails, b.ParkingLevel, b.AirConditionType, b.Details,
                                b.HouseID, b.OfficeID, b.East, b.SubAreaId, b.south, b.west, b.north, b.BuildingBizType, b.BizType, b.BusinessHours,
                                b.ProRoad, b.BizCutOff, b.Flows, b.CustomerType, b.ProjectId, b.IsBenchmarks, b.Creator, b.CreateTime, b.SaveDateTime,
                                b.SaveUser, b.PinYin, b.PinYinAll, b.Valid, b.FxtCompanyId, b.X, b.CorrelationType, b.Y, b.Remarks, b.ZZProjectId, b.BGProjectId, 
                                b.BuildingName, b.Address, b.FieldNo,AveragePrice,Weight ";

                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz b with(nolock) ");
                strSql.Append(" where b.CityId=@CityId and b.Valid=1 and b.BuildingId=@BuildingId ");
                strSql.Append(" and not exists(select sub.BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub sub with(nolock) where ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId  and b.BuildingId=sub.BuildingId and b.CityId=sub.CityId) ");
                strSql.Append(" and b.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(nolock) ");
                strSql.Append(" where b.CityId=@CityId and b.FxtCompanyId=@FxtCompanyId and b.Valid=1 and b.BuildingId=@BuildingId");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var Building_Bizlist = con.Query<Dat_Building_Biz>(strSql.ToString(), new { CityId = CityId, FxtCompanyId = FxtCompanyId, BuildingId = buildingId }).FirstOrDefault();
                    return Building_Bizlist;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public int GetBusinessBuildingCount(int projectId, int cityId, int fxtCompanyId)
        {
            var cityTable = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            var companyIds = cityTable == null ? string.Empty : cityTable.ShowCompanyId;
            if (companyIds.Equals(string.Empty)) companyIds = fxtCompanyId.ToString();

            var strSql = @"SELECT BuildingId
                FROM FxtData_Biz.dbo.Dat_Building_Biz b WITH (NOLOCK)
                WHERE b.ProjectId = @projectId
	                AND b.CityId = @cityId
	                AND b.FxtCompanyId in (" + companyIds + @")
	                AND b.Valid = 1
	                AND NOT EXISTS (
		                SELECT sub.BuildingId
		                FROM FxtData_Biz.dbo.Dat_Building_Biz_sub sub WITH (NOLOCK)
		                WHERE sub.BuildingId = b.BuildingId
			                AND sub.CityId = @cityId
			                AND sub.FxtCompanyId = @fxtCompanyId
		                )
                union

                SELECT BuildingId
                FROM FxtData_Biz.dbo.Dat_Building_Biz_Sub b WITH (NOLOCK)
                WHERE b.ProjectId = @projectId
	                AND b.CityId = @cityId
	                AND b.FxtCompanyId = @fxtCompanyId
	                AND b.Valid = 1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<long>(strSql, new { projectId, cityId, fxtCompanyId }).Count();

            }
        }

        /// <summary>
        /// 根据楼栋名称回去楼栋Id
        /// </summary>
        /// <param name="buildName">楼栋名称</param>
        /// <returns>null:-1</returns>
        public long GetBuildIdByName(long projectId, string buildName, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Building_Biz();
                //}
                string comId = city_table.ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                StringBuilder strSql = new StringBuilder();
                string attr = @" b.BuildingId";

                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz b with(nolock) ");
                strSql.Append(" where b.CityId=@CityId and b.Valid=1 and b.BuildingName=@BuildingName  and b.ProjectId=@ProjectId ");
                strSql.Append(" and not exists(select sub.BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub sub with(nolock) where  sub.AreaId=b.AreaId ");
                strSql.Append(" and sub.FxtCompanyId=@FxtCompanyId  and b.BuildingId=sub.BuildingId and b.CityId=sub.CityId) ");
                strSql.Append(" and b.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(nolock) ");
                strSql.Append(" where b.CityId=@CityId and b.FxtCompanyId=@FxtCompanyId and b.Valid=1 and b.BuildingName=@BuildingName  and b.ProjectId=@ProjectId ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    return con.Query<long>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, BuildingName = buildName, ProjectId = projectId }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 添加商业楼栋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDat_Building_Biz(Dat_Building_Biz model)
        {

            try
            {
                //var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return 0;
                //}
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) (");
                strSql.Append("StructureCode,BuildingTypeCode,ManagerPrice,ManagerTel,BuildingArea,EndDate,");
                strSql.Append("OpenDate,RentSaleType,BizArea,BizFloor,CityId,UpFloorNum,DownFloorNum,");
                strSql.Append("Functional,BizNum,LiftNum,LiftFitment,LiftBrand,EscalatorsNum,EscalatorsBrand,");
                strSql.Append("ToiletBrand,AreaId,PublicFitment,WallFitment,TrafficType,TrafficDetails,");
                strSql.Append("ParkingLevel,AirConditionType,Details,HouseID,OfficeID,East,SubAreaId,");
                strSql.Append("south,west,north,BuildingBizType,BizType,BusinessHours,ProRoad,BizCutOff,");
                strSql.Append("Flows,CustomerType,ProjectId,IsBenchmarks,Creator,CreateTime,SaveDateTime,");
                strSql.Append("SaveUser,PinYin,PinYinAll,Valid,FxtCompanyId,X,CorrelationType,Y,Remarks,ZZProjectId,BGProjectId,BuildingName,Address,FieldNo,AveragePrice,Weight ");
                strSql.Append(") values (");
                strSql.Append("@StructureCode,@BuildingTypeCode,@ManagerPrice,@ManagerTel,@BuildingArea,@EndDate,@OpenDate,@RentSaleType,@BizArea,@BizFloor,@CityId,@UpFloorNum,@DownFloorNum,@Functional,@BizNum,@LiftNum,@LiftFitment,@LiftBrand,@EscalatorsNum,@EscalatorsBrand,@ToiletBrand,@AreaId,@PublicFitment,@WallFitment,@TrafficType,@TrafficDetails,@ParkingLevel,@AirConditionType,@Details,@HouseID,@OfficeID,@East,@SubAreaId,@south,@west,@north,@BuildingBizType,@BizType,@BusinessHours,@ProRoad,@BizCutOff,@Flows,@CustomerType,@ProjectId,@IsBenchmarks,@Creator,@CreateTime,@SaveDateTime,@SaveUser,@PinYin,@PinYinAll,@Valid,@FxtCompanyId,@X,@CorrelationType,@Y,@Remarks,@ZZProjectId,@BGProjectId,@BuildingName,@Address,@FieldNo,@AveragePrice,@Weight");
                strSql.Append(") ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    model.CreateTime = DateTime.Now;
                    model.Valid = 1;
                    int result = con.Execute(strSql.ToString(), model);
                    if (result > 0)
                    {
                        strSql.Clear();
                        strSql.Append(" select top 1 BuildingId from FxtData_Biz.dbo.Dat_Building_Biz ");
                        strSql.Append(" with(nolock) order by BuildingId desc");
                        var obj = con.Query<Dat_Building_Biz>(strSql.ToString()).FirstOrDefault();
                        return Convert.ToInt32(obj.BuildingId);
                    }
                    return result;

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 更新商业楼栋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDat_Building_Biz(Dat_Building_Biz model, int currFxtCompanyId)
        {
            try
            {
                //var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return 0;
                //}
                StringBuilder strSql = new StringBuilder();
                string sql;
                //strSql.Append("update FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) set ");
                #region ---------------商业楼栋公共字段-----------------------
                strSql.Append(" StructureCode = @StructureCode , ");
                strSql.Append(" BuildingTypeCode = @BuildingTypeCode , ");
                strSql.Append(" ManagerPrice = @ManagerPrice , ");
                strSql.Append(" ManagerTel = @ManagerTel , ");
                strSql.Append(" BuildingArea = @BuildingArea , ");
                strSql.Append(" EndDate = @EndDate , ");
                strSql.Append(" OpenDate = @OpenDate , ");
                strSql.Append(" RentSaleType = @RentSaleType , ");
                strSql.Append(" BizArea = @BizArea , ");
                strSql.Append(" BizFloor = @BizFloor , ");
                strSql.Append(" CityId = @CityId , ");
                strSql.Append(" UpFloorNum = @UpFloorNum , ");
                strSql.Append(" DownFloorNum = @DownFloorNum , ");
                strSql.Append(" Functional = @Functional , ");
                strSql.Append(" BizNum = @BizNum , ");
                strSql.Append(" LiftNum = @LiftNum , ");
                strSql.Append(" LiftFitment = @LiftFitment , ");
                strSql.Append(" LiftBrand = @LiftBrand , ");
                strSql.Append(" EscalatorsNum = @EscalatorsNum , ");
                strSql.Append(" EscalatorsBrand = @EscalatorsBrand , ");
                strSql.Append(" ToiletBrand = @ToiletBrand , ");
                strSql.Append(" AreaId = @AreaId , ");
                strSql.Append(" PublicFitment = @PublicFitment , ");
                strSql.Append(" WallFitment = @WallFitment , ");
                strSql.Append(" TrafficType = @TrafficType , ");
                strSql.Append(" TrafficDetails = @TrafficDetails , ");
                strSql.Append(" ParkingLevel = @ParkingLevel , ");
                strSql.Append(" AirConditionType = @AirConditionType , ");
                strSql.Append(" Details = @Details , ");
                strSql.Append(" HouseID = @HouseID , ");
                strSql.Append(" OfficeID = @OfficeID , ");
                strSql.Append(" East = @East , ");
                strSql.Append(" SubAreaId = @SubAreaId , ");
                strSql.Append(" south = @south , ");
                strSql.Append(" west = @west , ");
                strSql.Append(" north = @north , ");
                strSql.Append(" BuildingBizType = @BuildingBizType , ");
                strSql.Append(" BizType = @BizType , ");
                strSql.Append(" BusinessHours = @BusinessHours , ");
                strSql.Append(" ProRoad = @ProRoad , ");
                strSql.Append(" BizCutOff = @BizCutOff , ");
                strSql.Append(" Flows = @Flows , ");
                strSql.Append(" CustomerType = @CustomerType , ");
                //strSql.Append(" ProjectId = @ProjectId , ");
                strSql.Append(" IsBenchmarks = @IsBenchmarks , ");
                //strSql.Append(" Creator = @Creator , ");
                //strSql.Append(" CreateTime = @CreateTime , ");
                strSql.Append(" SaveDateTime = @SaveDateTime , ");
                strSql.Append(" SaveUser = @SaveUser , ");
                strSql.Append(" PinYin = @PinYin , ");
                strSql.Append(" PinYinAll = @PinYinAll , ");
                //strSql.Append(" Valid = @Valid , ");
                strSql.Append(" FxtCompanyId = @FxtCompanyId , ");
                strSql.Append(" X = @X , ");
                strSql.Append(" CorrelationType = @CorrelationType , ");
                strSql.Append(" Y = @Y , ");
                strSql.Append(" Remarks = @Remarks , ");
                strSql.Append(" ZZProjectId = @ZZProjectId , ");
                strSql.Append(" BGProjectId = @BGProjectId , ");
                strSql.Append(" BuildingName = @BuildingName , ");
                strSql.Append(" Address = @Address , ");
                strSql.Append(" FieldNo = @FieldNo,  ");
                strSql.Append(" AveragePrice = @AveragePrice,  ");
                strSql.Append(" Weight = @Weight  ");
                strSql.Append(" where BuildingId=@BuildingId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId");
                #endregion
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    //房讯通
                    if (model.FxtCompanyId == FxtComId)
                    {
                        sql = " update FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    if (model.FxtCompanyId == currFxtCompanyId)
                    {
                        sql = " update FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    else
                    {
                        sql = "select BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub with(nolock) where BuildingId=@BuildingId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        Dat_Building_Biz buil_sub = con.Query<Dat_Building_Biz>(sql, new { BuildingId = model.BuildingId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                        if (buil_sub != null)//子表存在
                        {
                            sql = " update FxtData_Biz.dbo.Dat_Building_Biz_sub with(rowlock) set ";
                            int reslut = con.Execute(sql + strSql.ToString(), model);
                            return reslut;
                        }
                        else
                        {
                            sql = "select BuildingId from FxtData_Biz.dbo.Dat_Building_Biz with(nolock) where BuildingId=@BuildingId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                            Dat_Building_Biz buil = con.Query<Dat_Building_Biz>(sql, new { BuildingId = model.BuildingId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                            if (buil != null)//主表存在
                            {
                                sql = " update FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) set ";
                                int reslut = con.Execute(sql + strSql.ToString(), model);
                                return reslut;
                            }
                            else
                            {
                                //主表字表不在(在字表中插入一条记录来自主表)
                                strSql.Clear();
                                strSql.Append("insert into FxtData_Biz.dbo.Dat_Building_Biz_sub with(rowlock) (");
                                strSql.Append("BuildingId,StructureCode,BuildingTypeCode,ManagerPrice,ManagerTel,BuildingArea,EndDate,");
                                strSql.Append("OpenDate,RentSaleType,BizArea,BizFloor,CityId,UpFloorNum,DownFloorNum,");
                                strSql.Append("Functional,BizNum,LiftNum,LiftFitment,LiftBrand,EscalatorsNum,EscalatorsBrand,");
                                strSql.Append("ToiletBrand,AreaId,PublicFitment,WallFitment,TrafficType,TrafficDetails,");
                                strSql.Append("ParkingLevel,AirConditionType,Details,HouseID,OfficeID,East,SubAreaId,");
                                strSql.Append("south,west,north,BuildingBizType,BizType,BusinessHours,ProRoad,BizCutOff,");
                                strSql.Append("Flows,CustomerType,ProjectId,IsBenchmarks,Creator,CreateTime,SaveDateTime,");
                                strSql.Append("SaveUser,PinYin,PinYinAll,Valid,FxtCompanyId,X,CorrelationType,Y,Remarks,ZZProjectId,BGProjectId,BuildingName,Address,FieldNo,AveragePrice,Weight ");
                                strSql.Append(") ");
                                strSql.Append(" select ");
                                strSql.Append(" BuildingId,StructureCode, BuildingTypeCode, ManagerPrice, ManagerTel, BuildingArea, ");
                                strSql.Append(" EndDate, OpenDate, RentSaleType, BizArea, BizFloor, CityId, UpFloorNum, DownFloorNum, Functional, ");
                                strSql.Append(" BizNum, LiftNum, LiftFitment, LiftBrand, EscalatorsNum, EscalatorsBrand, ToiletBrand, AreaId, ");
                                strSql.Append(" PublicFitment, WallFitment, TrafficType, TrafficDetails, ParkingLevel, AirConditionType, Details, ");
                                strSql.Append(" HouseID, OfficeID, East, SubAreaId, south, west, north, BuildingBizType, BizType, BusinessHours,");
                                strSql.Append(" ProRoad, BizCutOff, Flows, CustomerType, ProjectId, IsBenchmarks, Creator, getdate() as CreateTime, getdate() as SaveDateTime,");
                                strSql.Append(" '" + model.SaveUser + " as ' SaveUser, PinYin, PinYinAll, Valid, '" + currFxtCompanyId + "' as FxtCompanyId, X, CorrelationType, Y, Remarks, ZZProjectId, BGProjectId,");
                                strSql.Append(" BuildingName, Address, FieldNo,AveragePrice,Weight ");
                                strSql.Append(" from FxtData_Biz.dbo.Dat_Building_Biz with(nolock) ");
                                strSql.Append(" where BuildingId=@BuildingId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                                int reslut = con.Execute(strSql.ToString(), new { BuildingId = model.BuildingId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId });
                                return reslut;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除商业楼栋
        /// </summary>
        /// <param name="buildingId">楼栋Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="cityId">城市Id</param>
        /// <param name="FxtCompanyId">评估机构Id</param>
        /// <param name="ProductTypeCode">产品Id</param>
        /// <returns></returns>
        public bool DeleteDat_Building_Biz(int buildingId, string userId, int cityId, int FxtCompanyId, int ProductTypeCode, int currFxtCompanyId)
        {
            try
            {
                //var city_table = GetCityTable(cityId, FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return false;
                //}
                //////暂不判断是否IsDeleteTrue20161029
                ////CompanyProduct compro = null;
                ////using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
                ////{
                ////    string sql = "SELECT CompanyId,IsDeleteTrue FROM CompanyProduct WITH(NOLOCK) WHERE CompanyId=@CompanyId and CityId=@CityId and ProductTypeCode=@ProductTypeCode";
                ////    compro = con.Query<CompanyProduct>(sql, new { CompanyId = FxtCompanyId, CityId = cityId, ProductTypeCode = ProductTypeCode }).FirstOrDefault();

                ////}
                ////if (compro != null)
                ////{
                ////    if (compro.IsDeleteTrue == 1)
                ////    {
                ////        return DeleteBuild(cityId, FxtCompanyId, buildingId);
                ////    }
                ////    else
                ////    {

                ////    }
                ////}
                ////else
                ////{
                ////    return false;
                ////}
                return DeleteBuild(cityId, FxtCompanyId, buildingId, userId, ProductTypeCode, currFxtCompanyId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 直接删除楼栋(谨慎使用)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        private bool DeleteBuild(int cityId, int FxtCompanyId, int buildingId)
        {
            try
            {
                string sql = " delete FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) where FxtCompanyId in(25," + FxtCompanyId + ") and buildingId=" + buildingId + " and CityId=" + cityId + " "
                      + " delete FxtData_Biz.dbo.Dat_Building_Biz_sub with(rowlock) where FxtCompanyId in(25," + FxtCompanyId + ") and buildingId=" + buildingId + " and CityId=" + cityId + " ";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    int result = con.Execute(sql);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除楼栋(更新valid)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="buildingId"></param>
        /// <param name="userId"></param>
        /// <param name="ProductTypeCode"></param>
        /// <returns></returns>
        private bool DeleteBuild(int cityId, int FxtCompanyId, int buildingId, string userId, int ProductTypeCode, int currFxtCompanyId)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append(" set Valid=0,[SaveDateTime]=GetDate(),[SaveUser]='" + userId + "' ");
                    strsql.Append(" where [BuildingId]=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId");

                    if (FxtCompanyId == FxtComId)
                    {

                        string sql = "update FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { BuildingId = buildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
                        return result > 0;
                    }
                    if (FxtCompanyId == currFxtCompanyId)
                    {

                        string sql = "update FxtData_Biz.dbo.Dat_Building_Biz with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { BuildingId = buildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
                        return result > 0;
                    }
                    else
                    {
                        string sql_query = " select BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub with(nolock) where [BuildingId]=@BuildingId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        Dat_Building_Biz sub = con.Query<Dat_Building_Biz>(sql_query, new { BuildingId = buildingId, CityId = cityId, FxtCompanyId = FxtCompanyId }).FirstOrDefault();
                        if (sub != null)
                        {
                            string sql = "update FxtData_Biz.dbo.Dat_Building_Biz_sub with(rowlock) " + strsql.ToString();
                            int result = con.Execute(sql, new { BuildingId = buildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
                            return result > 0;
                        }
                        else
                        {
                            strsql.Clear();
                            strsql.Append("insert into FxtData_Biz.dbo.Dat_Building_Biz_sub with(rowlock) (");
                            strsql.Append("BuildingId,StructureCode,BuildingTypeCode,ManagerPrice,ManagerTel,BuildingArea,EndDate,");
                            strsql.Append("OpenDate,RentSaleType,BizArea,BizFloor,CityId,UpFloorNum,DownFloorNum,");
                            strsql.Append("Functional,BizNum,LiftNum,LiftFitment,LiftBrand,EscalatorsNum,EscalatorsBrand,");
                            strsql.Append("ToiletBrand,AreaId,PublicFitment,WallFitment,TrafficType,TrafficDetails,");
                            strsql.Append("ParkingLevel,AirConditionType,Details,HouseID,OfficeID,East,SubAreaId,");
                            strsql.Append("south,west,north,BuildingBizType,BizType,BusinessHours,ProRoad,BizCutOff,");
                            strsql.Append("Flows,CustomerType,ProjectId,IsBenchmarks,Creator,CreateTime,SaveDateTime,");
                            strsql.Append("SaveUser,PinYin,PinYinAll,Valid,FxtCompanyId,X,CorrelationType,Y,Remarks,ZZProjectId,BGProjectId,BuildingName,Address,FieldNo,AveragePrice,Weight ");
                            strsql.Append(") ");
                            strsql.Append(" select ");
                            strsql.Append(" BuildingId,StructureCode, BuildingTypeCode, ManagerPrice, ManagerTel, BuildingArea, ");
                            strsql.Append(" EndDate, OpenDate, RentSaleType, BizArea, BizFloor, CityId, UpFloorNum, DownFloorNum, Functional, ");
                            strsql.Append(" BizNum, LiftNum, LiftFitment, LiftBrand, EscalatorsNum, EscalatorsBrand, ToiletBrand, AreaId, ");
                            strsql.Append(" PublicFitment, WallFitment, TrafficType, TrafficDetails, ParkingLevel, AirConditionType, Details, ");
                            strsql.Append(" HouseID, OfficeID, East, SubAreaId, south, west, north, BuildingBizType, BizType, BusinessHours,");
                            strsql.Append(" ProRoad, BizCutOff, Flows, CustomerType, ProjectId, IsBenchmarks, Creator, getdate() as CreateTime, getdate() as SaveDateTime,");
                            strsql.Append(" '" + userId + " as ' SaveUser, PinYin, PinYinAll, 0 as Valid, '" + currFxtCompanyId + "' as FxtCompanyId, X, CorrelationType, Y, Remarks, ZZProjectId, BGProjectId,");
                            strsql.Append(" BuildingName, Address, FieldNo,AveragePrice,Weight ");
                            strsql.Append(" from FxtData_Biz.dbo.Dat_Building_Biz ");
                            strsql.Append(" where BuildingId=@BuildingId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                            int reslut = con.Execute(strsql.ToString(), new { BuildingId = buildingId, CityId = cityId, FxtCompanyId = FxtCompanyId });
                            return reslut > 0;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 检查楼栋名称是否存在
        /// </summary>
        /// <param name="projectId">商业街ID</param>
        /// <param name="buildNameTo">楼栋名称</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        public Dat_Building_Biz CheckBuild(int projectId, string buildNameTo, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Building_Biz();
                //}
                string comdId = city_table.FirstOrDefault().ShowCompanyId;
                StringBuilder strSql = new StringBuilder();
                string attr = @" b.BuildingId, b.StructureCode, b.BuildingTypeCode, b.ManagerPrice, b.ManagerTel, b.BuildingArea,
                                b.EndDate, b.OpenDate, b.RentSaleType, b.BizArea, b.BizFloor, b.CityId, b.UpFloorNum, b.DownFloorNum, b.Functional,
                                b.BizNum, b.LiftNum, b.LiftFitment, b.LiftBrand, b.EscalatorsNum, b.EscalatorsBrand, b.ToiletBrand, b.AreaId,
                                b.PublicFitment, b.WallFitment, b.TrafficType, b.TrafficDetails, b.ParkingLevel, b.AirConditionType, b.Details,
                                b.HouseID, b.OfficeID, b.East, b.SubAreaId, b.south, b.west, b.north, b.BuildingBizType, b.BizType, b.BusinessHours,
                                b.ProRoad, b.BizCutOff, b.Flows, b.CustomerType, b.ProjectId, b.IsBenchmarks, b.Creator, b.CreateTime, b.SaveDateTime,
                                b.SaveUser, b.PinYin, b.PinYinAll, b.Valid, b.FxtCompanyId, b.X, b.CorrelationType, b.Y, b.Remarks, b.ZZProjectId, b.BGProjectId, 
                                b.BuildingName, b.Address, b.FieldNo,b.AveragePrice,b.Weight ";

                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz b with(rowlock) ");
                strSql.Append(" where b.CityId=@CityId and b.Valid=1 and b.ProjectId=@ProjectId and BuildingName=@BuildingName ");
                strSql.Append(" and not exists(select sub.BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub sub with(rowlock) where  ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId  and b.BuildingId=sub.BuildingId and b.CityId=sub.CityId) ");
                strSql.Append(" and b.FxtCompanyId in (" + comdId + ") ");
                strSql.Append(" union ");
                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(rowlock) ");
                strSql.Append(" where b.CityId=@CityId and b.FxtCompanyId=@FxtCompanyId and b.Valid=1 and b.ProjectId=@ProjectId and BuildingName=@BuildingName ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var Building_Bizlist = con.Query<Dat_Building_Biz>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, ProjectId = projectId, BuildingName = buildNameTo }).FirstOrDefault();
                    return Building_Bizlist;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据行政区Id获取商圈列表
        /// </summary>
        /// <param name="areaId">行政区Id</param>
        /// <returns></returns>
        public IQueryable<SYS_SubArea_Biz> GetSubAreaBizByAreaId(int areaId)
        {
            string strSql = @"select subAreaId,subAreaName from FxtDataCenter.dbo.SYS_SubArea_Biz with(nolock) where areaId = @areaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_SubArea_Biz>(strSql, new { areaId }).AsQueryable();
            }

        }

        /// <summary>
        /// 验证楼栋名称是否存在
        /// </summary>
        /// <param name="_cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="_fxtCompanyId"></param>
        /// <param name="buildName"></param>
        /// <param name="subAreaId">商圈Id</param>
        /// <returns></returns>
        public Dat_Building_Biz IsExistBuildName(int cityId, int areaId, int fxtCompanyId, string buildName, int subAreaId, int projectId)
        {
            StringBuilder str_attr = new StringBuilder();
            str_attr.Append("BuildingId, CityId, AreaId, SubAreaId, ProjectId, CorrelationType,");
            str_attr.Append("BuildingName, [Address], FieldNo, StructureCode, BuildingTypeCode,");
            str_attr.Append("ManagerPrice, ManagerTel, BuildingArea, EndDate, OpenDate,");
            str_attr.Append("RentSaleType, BizArea, BizFloor, UpFloorNum, DownFloorNum, ");
            str_attr.Append("Functional, BizNum, LiftNum, LiftFitment, LiftBrand,");
            str_attr.Append("EscalatorsNum, EscalatorsBrand, ToiletBrand, PublicFitment,");
            str_attr.Append("WallFitment, TrafficType, TrafficDetails, ParkingLevel,");
            str_attr.Append("AirConditionType, Details, HouseID, OfficeID, East,");
            str_attr.Append("south, west, north, BuildingBizType, BizType, BusinessHours, ProRoad, BizCutOff,");
            str_attr.Append("Flows, CustomerType, IsBenchmarks, Creator, CreateTime, SaveDateTime, SaveUser,");
            str_attr.Append("PinYin, PinYinAll, Valid, FxtCompanyId, X, Y, Remarks, ZZProjectId, BGProjectId  ");
            string strSql = @"select  " + str_attr.ToString() + @"  from FxtData_Biz.dbo.Dat_Building_Biz with(nolock) 
                              where CityId = @CityId and AreaId=@AreaId and BuildingName=@BuildingName  
                                    and SubAreaId=@SubAreaId and projectid = @projectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<Dat_Building_Biz>(strSql, new
                {
                    CityId = cityId,
                    AreaId = areaId,
                    BuildingName = buildName,
                    SubAreaId = subAreaId,
                    projectId = projectId
                }).FirstOrDefault();
            }
        }
        /// <summary>
        /// 验证宗地号是否唯一
        /// </summary>
        /// <param name="_cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="FieldNo"></param>
        /// <returns></returns>
        public bool ValidFieldNo(int cityId, int areaId, string fieldNo)
        {
            string strSql = @"select BuildingName from FxtData_Biz.dbo.Dat_Building_Biz with(nolock) 
                                    where CityId = @CityId and AreaId=@AreaId and FieldNo=@FieldNo";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_SubArea_Biz>(strSql, new
                {
                    CityId = cityId,
                    AreaId = areaId,
                    FieldNo = fieldNo
                }).FirstOrDefault() == null;
            }
        }

        /// <summary>
        /// 查询功能所需条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string QqueryWhere(Dat_Building_Biz model)
        {
            string jquerStr = "";
            if (!string.IsNullOrEmpty(model.BuildingName))
            {
                jquerStr += " and (b.BuildingName like @BuildingName)";
            }
            if (model.BuildingBizType > 0)
            {
                jquerStr += " and b.BuildingBizType=@BuildingBizType";
            }
            if (model.AreaId > 0)
            {
                jquerStr += " and b.AreaId=@AreaId";
            }
            if (model.SubAreaId > 0)
            {
                jquerStr += " and b.SubAreaId=@SubAreaId";
            }
            if (model.ProRoad > 0)
            {
                jquerStr += " and b.ProRoad=@ProRoad";
            }
            if (model.BizCutOff > 0)
            {
                jquerStr += " and b.BizCutOff=@BizCutOff";
            }
            if (model.Flows > 0)
            {
                jquerStr += " and b.Flows=@Flows";
            }
            if (model.OpenDate != null && model.OpenDate != default(DateTime))
            {
                jquerStr += " and b.OpenDate >= @OpenDate";
            }
            if (model.OpenDateEnd != null && model.OpenDateEnd != default(DateTime))
            {
                jquerStr += " and b.OpenDate<=@OpenDateEnd";
            }
            return jquerStr;

        }

        public DataTable BuildingSelfDefineExport(Dat_Building_Biz buildingBiz, List<string> buildingAttr, int CityId, int FxtCompanyId, bool self = true)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                var city_table = GetCityTable(Convert.ToInt32(buildingBiz.CityId), buildingBiz.FxtCompanyId).FirstOrDefault();
                string comId = city_table.ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = buildingBiz.FxtCompanyId.ToString();
                if (self) comId = buildingBiz.FxtCompanyId.ToString();

                var buildingWhere = string.Empty;
                var projectWhere = string.Empty;
                if (!string.IsNullOrEmpty(buildingBiz.ProjectName))
                {
                    projectWhere += " and p.ProjectName like @ProjectName";
                    paramet.Add(new SqlParameter("@ProjectName", buildingBiz.ProjectName));
                }
                if (!(new[] { 0, -1 }).Contains(buildingBiz.AreaId))
                {
                    projectWhere += " and p.AreaId = @AreaId";
                    paramet.Add(new SqlParameter("@AreaId", buildingBiz.AreaId));
                }
                if (!string.IsNullOrEmpty(buildingBiz.BuildingName))
                {
                    buildingWhere += " and buildingTable.BuildingName like @BuildingName";
                    paramet.Add(new SqlParameter("@BuildingName", "%" + buildingBiz.BuildingName + "%"));
                }
                if (!(new[] { 0, -1 }).Contains(buildingBiz.BuildingBizType))
                {
                    buildingWhere += " and buildingTable.BuildingBizType = @BuildingBizType";
                    paramet.Add(new SqlParameter("@BuildingBizType", buildingBiz.BuildingBizType));
                }
                if (!(new[] { 0, -1 }).Contains(buildingBiz.ProRoad))
                {
                    buildingWhere += " and buildingTable.ProRoad = @ProRoad";
                    paramet.Add(new SqlParameter("@ProRoad", buildingBiz.ProRoad));
                }
                if (!(new[] { 0, -1 }).Contains(buildingBiz.BizCutOff))
                {
                    buildingWhere += " and buildingTable.BizCutOff = @BizCutOff";
                    paramet.Add(new SqlParameter("@BizCutOff", buildingBiz.BizCutOff));
                }
                if (!(new[] { 0, -1 }).Contains(buildingBiz.Flows))
                {
                    buildingWhere += " and buildingTable.Flows = @Flows";
                    paramet.Add(new SqlParameter("@Flows", buildingBiz.Flows));
                }
                if (!(new[] { -1 }).Contains(buildingBiz.IsBenchmarks))
                {
                    buildingWhere += " and buildingTable.IsBenchmarks = @IsBenchmarks";
                    paramet.Add(new SqlParameter("@IsBenchmarks", buildingBiz.IsBenchmarks));
                }

                var strSql = @"
select 
	buildingTable.*
	,projectTable.ProjectName
	,a.AreaName
	,sa.SubAreaName
	,c.CodeName as CorrelationTypeName
	,c1.CodeName as StructureCodeName
	,c2.CodeName as BuildingTypeCodeName
	,c3.CodeName as RentSaleTypeName
	,c4.CodeName as LiftFitmentName
	,c5.CodeName as PublicFitmentName
	,c6.CodeName as WallFitmentName
	,c7.CodeName as TrafficTypeName
	,c8.CodeName as ParkingLevelName
	,c9.CodeName as AirConditionTypeName
	,c10.CodeName as BuildingBizTypeName
	,c11.CodeName as BizTypeName
	,c12.CodeName as ProRoadName
	,c13.CodeName as BizCutOffName
	,c14.CodeName as FlowsName
	,c15.CodeName as CustomerTypeName
	,(
		CASE buildingTable.IsBenchmarks
			WHEN 0
				THEN '否'
			WHEN 1
				THEN '是'
			ELSE ''
			END
		) AS IsBenchmarksName
from (
	select * from FxtData_Biz.dbo.Dat_Building_Biz b with(nolock)
	where not exists(
		select BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub b1 with(nolock)
		where b1.BuildingId = b.BuildingId
		and b1.CityId = b.CityId
		and b1.FxtCompanyId = @fxtCompanyId
	)
	and b.Valid = 1
	and b.CityId = @CityId
	and b.FxtCompanyId in (" + comId + @")
	union 
	select * from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(nolock)
	where b.Valid = 1
	and b.CityId = @CityId
	and b.FxtCompanyId = @fxtCompanyId
)buildingTable
inner join (
	select * from FxtData_Biz.dbo.Dat_Project_Biz p with(nolock)
	where not exists(
		select ProjectId from FxtData_Biz.dbo.Dat_Project_Biz_sub p1 with(nolock)
		where p1.ProjectId = p.ProjectId
		and p1.CityId = p.CityId
		and p1.FxtCompanyId = @fxtCompanyId
	)
	and p.Valid = 1
	and p.CityId = @CityId
	and p.FxtCompanyId in (" + comId + @")" + projectWhere + @"
	union
	select * from FxtData_Biz.dbo.Dat_Project_Biz_sub p with(nolock)
	where p.Valid = 1
	and p.CityId = @CityId
	and p.FxtCompanyId = @fxtCompanyId" + projectWhere + @"
)projectTable on buildingTable.ProjectId = projectTable.ProjectId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on projectTable.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Biz sa with(nolock) on projectTable.SubAreaId = sa.SubAreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) ON c.code = buildingTable.CorrelationType
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) ON c1.code = buildingTable.StructureCode
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) ON c2.code = buildingTable.BuildingTypeCode
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) ON c3.code = buildingTable.RentSaleType
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) ON c4.code = buildingTable.LiftFitment
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) ON c5.code = buildingTable.PublicFitment
left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) ON c6.code = buildingTable.WallFitment
left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) ON c7.code = buildingTable.TrafficType
left join FxtDataCenter.dbo.SYS_Code c8 with(nolock) ON c8.code = buildingTable.ParkingLevel
left join FxtDataCenter.dbo.SYS_Code c9 with(nolock) ON c9.code = buildingTable.AirConditionType
left join FxtDataCenter.dbo.SYS_Code c10 with(nolock) ON c10.code = buildingTable.BuildingBizType
left join FxtDataCenter.dbo.SYS_Code c11 with(nolock) ON c11.code = buildingTable.BizType
left join FxtDataCenter.dbo.SYS_Code c12 with(nolock) ON c12.code = buildingTable.ProRoad
left join FxtDataCenter.dbo.SYS_Code c13 with(nolock) ON c13.code = buildingTable.BizCutOff
left join FxtDataCenter.dbo.SYS_Code c14 with(nolock) ON c14.code = buildingTable.Flows
left join FxtDataCenter.dbo.SYS_Code c15 with(nolock) ON c15.code = buildingTable.CustomerType
where 1 = 1" + buildingWhere;

                paramet.Add(new SqlParameter("@CityId", CityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                //string paramList = string.Empty;
                //for (int i = 0; i < buildingAttr.Count; i++)
                //{
                //    paramList += buildingAttr[i].Replace("&", " as ") + ",";
                //}
                var paramList = new StringBuilder();
                foreach (var t in buildingAttr)
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

    }
}
