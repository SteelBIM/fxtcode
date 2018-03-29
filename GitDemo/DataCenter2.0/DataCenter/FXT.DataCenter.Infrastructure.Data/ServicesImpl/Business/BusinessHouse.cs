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
    /// 商业房号
    /// </summary>
    public class Dat_House_BizDAL : IDat_House_Biz
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
        /// 获取商业房号列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<Dat_House_Biz> GetDat_House_BizList(Dat_House_Biz model, bool self = true)
        {
            try
            {
                var city_table = GetCityTable(model.CityId, model.FxtCompanyId);
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = model.FxtCompanyId.ToString();
                string where = string.Empty;
                string strSql = @"
select
	H.*,F.FloorNo,F.FloorNum,B.BuildingName as BuildName
	,(CASE h.IsMezzanine WHEN 0 THEN '无' WHEN 1 THEN '有' ELSE '' END) AS IsMezzanineName
	,(CASE h.IsEValue WHEN 0 THEN '否' WHEN 1 THEN '是' ELSE '' END) AS IsEValueName
	,city.CityName
	,PurposeCode.CodeName as PurposeCodeName
	,SJPurposeCode.CodeName as SJPurposeCodeName
	,FrontCode.CodeName as FrontCodeName
	,Shape.CodeName as ShapeName
	,BizCutOff.CodeName as BizCutOffName
	,BizHouseType.CodeName as BizHouseTypeName
	,BizHouseLocation.CodeName as BizHouseLocationName
	,FlowType.CodeName as FlowTypeName
from (
	select *
	FROM FxtData_Biz.dbo.Dat_House_Biz h WITH (NOLOCK)
	WHERE h.CityId = @CityId
		AND h.Valid = 1
		AND h.BuildingId = @BuildingId
		AND h.FloorId = @FloorId
		AND NOT EXISTS (
			SELECT sub.HouseId
			FROM FxtData_Biz.dbo.Dat_House_Biz_sub sub WITH (ROWLOCK)
			WHERE sub.FxtCompanyId = @FxtCompanyId
				AND h.HouseId = sub.HouseId
				AND h.CityId = sub.CityId
			)
		AND h.FxtCompanyId IN (" + comId + @")
	UNION
	SELECT *
	FROM FxtData_Biz.dbo.Dat_House_Biz_sub h WITH (NOLOCK)
	WHERE h.CityId = @CityId
		AND h.FxtCompanyId = @FxtCompanyId
		AND h.Valid = 1
		AND h.BuildingId = @BuildingId
		AND h.FloorId = @FloorId
)H
inner join (
	select FloorId,FloorNo,FloorNum from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock)
	where not exists(
		select FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub fs with(nolock)
		where fs.FxtCompanyId = f.FxtCompanyId
		and fs.CityId = f.CityId
		and fs.FloorId = f.FloorId
	)
	and f.CityId = @CityId
	and f.FxtCompanyId in (" + comId + @")
	and f.BuildingId = @BuildingId
	union
	select FloorId,FloorNo,FloorNum from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock)
	where f.FxtCompanyId = @FxtCompanyId 
	and f.CityId = @CityId
	and f.FloorId = @FloorId
)F on H.FloorId = F.FloorId
inner join (
	select BuildingId,BuildingName from FxtData_Biz.dbo.Dat_Building_Biz B with(nolock)
	where not exists(
		select BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub bs with(nolock)
		where bs.FxtCompanyId = b.FxtCompanyId
		and bs.CityId = b.CityId
		and bs.BuildingId = b.BuildingId
	)
	and b.CityId = @CityId
	and b.FxtCompanyId in (" + comId + @")
	and b.BuildingId = @BuildingId
	union
	select BuildingId,BuildingName from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(nolock)
	where b.FxtCompanyId = @FxtCompanyId 
	and b.CityId = @CityId
	and b.BuildingId = @BuildingId
)B on H.BuildingId = B.BuildingId
LEFT JOIN FxtDataCenter.dbo.SYS_City city WITH (NOLOCK) ON city.CityId = h.CityId
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code PurposeCode WITH (NOLOCK) ON PurposeCode.code = h.PurposeCode
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code SJPurposeCode WITH (NOLOCK) ON SJPurposeCode.code = h.SJPurposeCode
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code FrontCode WITH (NOLOCK) ON FrontCode.code = h.FrontCode
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code Shape WITH (NOLOCK) ON Shape.code = h.Shape
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code BizCutOff WITH (NOLOCK) ON BizCutOff.code = h.BizCutOff
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code BizHouseType WITH (NOLOCK) ON BizHouseType.code = h.BizHouseType
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code BizHouseLocation WITH (NOLOCK) ON BizHouseLocation.code = h.BizHouseLocation
LEFT JOIN [FXTDataCenter].[dbo].SYS_Code FlowType WITH (NOLOCK) ON FlowType.code = h.FlowType
where 1 = 1 " + where;
                if (self)//查看自己
                {
                    where += " and h.FxtCompanyId=@FxtCompanyId";
                }
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var floor_list = con.Query<Dat_House_Biz>(strSql, new { CityId = model.CityId, FxtCompanyId = model.FxtCompanyId, BuildingId = model.BuildingId, FloorId = model.FloorId }).AsQueryable();
                    return floor_list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取商业房号信息
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <returns></returns>
        public Dat_House_Biz GetDat_House_BizById(int houseId, int CityId, int FxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(CityId, FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_House_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = FxtCompanyId.ToString();
                string houseattr = @" select h.HouseId, h.BuildingId, h.FloorId, h.CityId, h.UnitNo, h.HouseName, 
                                    h.PurposeCode, h.SJPurposeCode, h.BuildingArea, h.InnerBuildingArea, 
                                    h.FrontCode, h.Shape, h.Width, h.[Length], h.IsMezzanine, h.BizCutOff, 
                                    h.BizHouseType, h.BizHouseLocation, h.Location, h.FlowType, h.DoorNum, 
                                    h.RentRate, h.UnitPrice, h.[Weight], h.IsEValue, h.FxtCompanyId, h.Creator, 
                                    h.CreateTime, h.SaveDateTime, h.SaveUser, h.Valid, h.Remarks ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(houseattr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_House_Biz h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.Valid=1 and h.HouseId=@HouseId ");
                strSql.Append(" and not exists(select sub.HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub sub with(rowlock) where ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId and h.HouseId=sub.HouseId and h.CityId=sub.CityId) ");
                strSql.Append(" and h.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(houseattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_House_Biz_sub h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.FxtCompanyId=@FxtCompanyId and h.Valid=1 and h.HouseId=@HouseId ");
                strSql.Append(" order by h.HouseId desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var house_list = con.Query<Dat_House_Biz>(strSql.ToString(), new { CityId = CityId, FxtCompanyId = FxtCompanyId, HouseId = houseId }).FirstOrDefault();
                    return house_list;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 新增商业房号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDat_House_Biz(Dat_House_Biz model)
        {
            //var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId);
            //if (city_table != null && city_table.Count() > 0)
            //{
            //    return 0;
            //}
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into FxtData_Biz.dbo.Dat_House_Biz with(rowlock) (");
                strSql.Append("BuildingId, FloorId, CityId, UnitNo, HouseName, PurposeCode, ");
                strSql.Append("SJPurposeCode, BuildingArea, InnerBuildingArea, FrontCode, Shape, Width, [Length],");
                strSql.Append("IsMezzanine, BizCutOff, BizHouseType, BizHouseLocation, Location, FlowType, DoorNum, ");
                strSql.Append("RentRate, UnitPrice, [Weight], IsEValue, FxtCompanyId, Creator, CreateTime, SaveDateTime, ");
                strSql.Append("SaveUser, Valid, Remarks ");
                strSql.Append(") values (");
                strSql.Append("@BuildingId, @FloorId, @CityId, @UnitNo, @HouseName, @PurposeCode, ");
                strSql.Append("@SJPurposeCode, @BuildingArea, @InnerBuildingArea, @FrontCode, @Shape, @Width, @Length,");
                strSql.Append("@IsMezzanine, @BizCutOff, @BizHouseType, @BizHouseLocation, @Location, @FlowType, @DoorNum, ");
                strSql.Append("@RentRate, @UnitPrice, @Weight, @IsEValue, @FxtCompanyId, @Creator, @CreateTime, @SaveDateTime, ");
                strSql.Append("@SaveUser, @Valid, @Remarks ");
                strSql.Append(") ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    model.SaveDateTime = model.CreateTime = DateTime.Now;
                    model.Valid = 1;
                    int result = con.Execute(strSql.ToString(), model);
                    if (result > 0)
                    {
                        strSql.Clear();
                        strSql.Append(" select top 1 HouseId from FxtData_Biz.dbo.Dat_House_Biz ");
                        strSql.Append(" with(nolock) order by HouseId desc");
                        var obj = con.Query<Dat_House_Biz>(strSql.ToString()).FirstOrDefault();
                        return Convert.ToInt32(obj.HouseId);

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
        /// 更新商业房号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDat_House_Biz(Dat_House_Biz model, int currFxtCompanyId)
        {
            //var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId);
            //if (city_table != null && city_table.Count() > 0)
            //{
            //    return 0;
            //}
            try
            {

                StringBuilder strSql = new StringBuilder();
                string sql;
                #region 更新字段
                strSql.Append(" UnitNo = @UnitNo,");
                strSql.Append(" HouseName = @HouseName,");
                strSql.Append(" PurposeCode = @PurposeCode,");
                strSql.Append(" SJPurposeCode = @SJPurposeCode,");
                strSql.Append(" BuildingArea = @BuildingArea,");
                strSql.Append(" InnerBuildingArea = @InnerBuildingArea,");
                strSql.Append(" FrontCode = @FrontCode,");
                strSql.Append(" Shape = @Shape,");
                strSql.Append(" Width = @Width,");
                strSql.Append(" Length = @Length,");
                strSql.Append(" IsMezzanine = @IsMezzanine,");
                strSql.Append(" BizCutOff = @BizCutOff,");
                strSql.Append(" BizHouseType = @BizHouseType,");
                strSql.Append(" BizHouseLocation = @BizHouseLocation,");
                strSql.Append(" Location = @Location,");
                strSql.Append(" FlowType = @FlowType,");
                strSql.Append(" DoorNum = @DoorNum,");
                strSql.Append(" RentRate = @RentRate,");
                strSql.Append(" UnitPrice = @UnitPrice,");
                strSql.Append(" Weight = @Weight,");
                strSql.Append(" IsEValue = @IsEValue,");
                strSql.Append(" SaveDateTime = @SaveDateTime,");
                strSql.Append(" SaveUser = @SaveUser,");
                strSql.Append(" Remarks = @Remarks ");
                #endregion
                strSql.Append(" where HouseId=@HouseId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    //房讯通
                    if (model.FxtCompanyId == FxtComId)
                    {
                        sql = " update FxtData_Biz.dbo.Dat_House_Biz with(rowlock) set " + strSql.ToString();
                        int reslut = con.Execute(sql, model);
                        return reslut;
                    }
                    if (model.FxtCompanyId == currFxtCompanyId)
                    {
                        sql = " update FxtData_Biz.dbo.Dat_House_Biz with(rowlock) set " + strSql.ToString();
                        int reslut = con.Execute(sql, model);
                        return reslut;
                    }
                    else
                    {
                        sql = "select HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub with(nolock) where HouseId=@HouseId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        Dat_House_Biz house_sub = con.Query<Dat_House_Biz>(sql, new { HouseId = model.HouseId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                        if (house_sub != null)//子表存在
                        {
                            sql = " update FxtData_Biz.dbo.Dat_House_Biz_sub with(rowlock) set ";
                            int reslut = con.Execute(sql + strSql.ToString(), model);
                            return reslut;
                        }
                        else
                        {
                            sql = "select HouseId from FxtData_Biz.dbo.Dat_House_Biz with(nolock) where HouseId=@HouseId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                            Dat_House_Biz house = con.Query<Dat_House_Biz>(sql, new { HouseId = model.HouseId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                            if (house != null)//主表存在
                            {
                                sql = " update FxtData_Biz.dbo.Dat_House_Biz with(rowlock) set ";
                                int reslut = con.Execute(sql + strSql.ToString(), model);
                                return reslut;
                            }
                            else
                            {
                                //主表字表不在(在字表中插入一条记录来自主表)
                                strSql.Clear();
                                strSql.Append("insert into FxtData_Biz.dbo.Dat_House_Biz_sub with(rowlock) (");
                                strSql.Append("HouseId,BuildingId, FloorId, CityId, UnitNo, HouseName, PurposeCode, ");
                                strSql.Append("SJPurposeCode, BuildingArea, InnerBuildingArea, FrontCode, Shape, Width, [Length],");
                                strSql.Append("IsMezzanine, BizCutOff, BizHouseType, BizHouseLocation, Location, FlowType, DoorNum, ");
                                strSql.Append("RentRate, UnitPrice, [Weight], IsEValue, FxtCompanyId, Creator, CreateTime, SaveDateTime, ");
                                strSql.Append("SaveUser, Valid, Remarks )");
                                strSql.Append(" select ");
                                strSql.Append("HouseId,BuildingId, FloorId, CityId, UnitNo, HouseName, PurposeCode, ");
                                strSql.Append("SJPurposeCode, BuildingArea, InnerBuildingArea, FrontCode, Shape, Width, [Length],");
                                strSql.Append("IsMezzanine, BizCutOff, BizHouseType, BizHouseLocation, Location, FlowType, DoorNum, ");
                                strSql.Append("RentRate, UnitPrice, [Weight], IsEValue, '" + currFxtCompanyId + "' as FxtCompanyId, '" + model.SaveUser + " as ' Creator, getdate() as CreateTime, getdate() as SaveDateTime, ");
                                strSql.Append("'" + model.SaveUser + " as ' SaveUser, Valid, Remarks ");
                                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz with(nolock) ");
                                strSql.Append(" where HouseId=@HouseId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                                int reslut = con.Execute(strSql.ToString(), new { HouseId = model.HouseId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId });
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
        /// 删除商业房号
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public bool DeleteDat_House_Biz(int houseId, string userName, int cityId, int fxtCompanyId, int poductTypeCode, int currFxtCompanyId)
        {
            try
            {
                //////暂不判断是否IsDeleteTrue20161029
                //////CompanyProduct compro = null;
                //////using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
                //////{
                //////    string sql = "SELECT CompanyId,IsDeleteTrue FROM CompanyProduct WITH(NOLOCK) WHERE CompanyId=@CompanyId and CityId=@CityId and ProductTypeCode=@ProductTypeCode";
                //////    compro = con.Query<CompanyProduct>(sql, new { CompanyId = fxtCompanyId, CityId = cityId, ProductTypeCode = poductTypeCode }).FirstOrDefault();

                //////}
                //////if (compro != null)
                //////{
                //////    if (compro.IsDeleteTrue == 1)
                //////    {
                //////        return DeleteHouse(cityId, fxtCompanyId, houseId);
                //////    }
                //////    else
                //////    {
                //////        return DeleteHouse(cityId, fxtCompanyId, houseId, userName, poductTypeCode, currFxtCompanyId);
                //////    }
                //////}
                //////else
                //////{
                //////    return false;
                //////}
                return DeleteHouse(cityId, fxtCompanyId, houseId, userName, poductTypeCode, currFxtCompanyId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="houseId"></param>
        /// <returns></returns>
        private bool DeleteHouse(int cityId, int fxtCompanyId, int houseId)
        {
            try
            {
                string sql = " delete FxtData_Biz.dbo.Dat_House_Biz with(rowlock) where FxtCompanyId in(25," + fxtCompanyId + ") and HouseId=" + houseId + " and CityId=" + cityId + " "
                    + " delete FxtData_Biz.dbo.Dat_House_Biz_sub with(rowlock) where FxtCompanyId in(25," + fxtCompanyId + ") and HouseId=" + houseId + " and CityId=" + cityId + " ";
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
        /// 虚删除
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="houseId"></param>
        /// <param name="userName"></param>
        /// <param name="poductTypeCode"></param>
        /// <returns></returns>
        private bool DeleteHouse(int cityId, int fxtCompanyId, int houseId, string userName, int poductTypeCode, int currFxtCompanyId)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append(" set Valid=0,[SaveDateTime]=GetDate(),[SaveUser]='" + userName + "' ");
                    strsql.Append(" where [HouseId]=@HouseId and CityId=@CityId and FxtCompanyId=@FxtCompanyId");

                    if (fxtCompanyId == FxtComId)
                    {

                        string sql = "update FxtData_Biz.dbo.Dat_House_Biz with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { HouseId = houseId, CityId = cityId, FxtCompanyId = fxtCompanyId });
                        return result > 0;
                    }
                    if (fxtCompanyId == currFxtCompanyId)
                    {

                        string sql = "update FxtData_Biz.dbo.Dat_House_Biz with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { HouseId = houseId, CityId = cityId, FxtCompanyId = fxtCompanyId });
                        return result > 0;
                    }
                    else
                    {
                        string sql_query = " select HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub with(nolock) where [HouseId]=@HouseId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        Dat_House_Biz sub = con.Query<Dat_House_Biz>(sql_query, new { HouseId = houseId, CityId = cityId, FxtCompanyId = fxtCompanyId }).FirstOrDefault();
                        if (sub != null)
                        {
                            string sql = "update FxtData_Biz.dbo.Dat_House_Biz_sub with(rowlock) " + strsql.ToString();
                            int result = con.Execute(sql, new { HouseId = houseId, CityId = cityId, FxtCompanyId = fxtCompanyId });
                            return result > 0;
                        }
                        else
                        {
                            strsql.Clear();
                            strsql.Append("insert into FxtData_Biz.dbo.Dat_House_Biz_sub with(rowlock) (");
                            strsql.Append("HouseId,BuildingId, FloorId, CityId, UnitNo, HouseName, PurposeCode, ");
                            strsql.Append("SJPurposeCode, BuildingArea, InnerBuildingArea, FrontCode, Shape, Width, [Length],");
                            strsql.Append("IsMezzanine, BizCutOff, BizHouseType, BizHouseLocation, Location, FlowType, DoorNum, ");
                            strsql.Append("RentRate, UnitPrice, [Weight], IsEValue, FxtCompanyId, Creator, CreateTime, SaveDateTime, ");
                            strsql.Append("SaveUser, Valid, Remarks )");
                            strsql.Append(" select ");
                            strsql.Append("HouseId,BuildingId, FloorId, CityId, UnitNo, HouseName, PurposeCode, ");
                            strsql.Append("SJPurposeCode, BuildingArea, InnerBuildingArea, FrontCode, Shape, Width, [Length],");
                            strsql.Append("IsMezzanine, BizCutOff, BizHouseType, BizHouseLocation, Location, FlowType, DoorNum, ");
                            strsql.Append("RentRate, UnitPrice, [Weight], IsEValue, '" + currFxtCompanyId + "' as FxtCompanyId, '" + userName + " as ' Creator, getdate() as CreateTime, getdate() as SaveDateTime, ");
                            strsql.Append("'" + userName + " as ' SaveUser, Valid, Remarks ");
                            strsql.Append(" from FxtData_Biz.dbo.Dat_House_Biz ");
                            strsql.Append(" where HouseId=@HouseId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                            int reslut = con.Execute(strsql.ToString(), new { FloorId = houseId, CityId = cityId, FxtCompanyId = fxtCompanyId });
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
        /// 获取房号列表
        /// </summary>
        /// <param name="floorId">楼层ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        public IQueryable<Dat_House_Biz> GetDat_House_BizByFloorId(long floorId, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_House_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                string houseattr = @" select h.HouseId, h.BuildingId, h.FloorId, h.CityId, h.UnitNo, h.HouseName, 
                                    h.PurposeCode, h.SJPurposeCode, h.BuildingArea, h.InnerBuildingArea, 
                                    h.FrontCode, h.Shape, h.Width, h.[Length], h.IsMezzanine, h.BizCutOff, 
                                    h.BizHouseType, h.BizHouseLocation, h.Location, h.FlowType, h.DoorNum, 
                                    h.RentRate, h.UnitPrice, h.[Weight], h.IsEValue, h.FxtCompanyId, h.Creator, 
                                    h.CreateTime, h.SaveDateTime, h.SaveUser, h.Valid, h.Remarks ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(houseattr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_House_Biz h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.Valid=1 and h.FloorId=@FloorId ");
                strSql.Append(" and not exists(select sub.HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub sub with(rowlock) where ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId and h.HouseId=sub.HouseId and h.CityId=sub.CityId) ");
                strSql.Append(" and h.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(houseattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_House_Biz_sub h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.FxtCompanyId=@FxtCompanyId and h.Valid=1 and h.FloorId=@FloorId");
                strSql.Append(" order by h.HouseId desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var house_list = con.Query<Dat_House_Biz>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, FloorId = floorId }).AsQueryable();
                    return house_list;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        public IQueryable<Dat_House_Biz> GetHouseList(int buildingId, int floorId, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_House_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                string houseattr = @" select h.HouseId, h.BuildingId, h.FloorId, h.CityId, h.UnitNo, h.HouseName, 
                                    h.PurposeCode, h.SJPurposeCode, h.BuildingArea, h.InnerBuildingArea, 
                                    h.FrontCode, h.Shape, h.Width, h.[Length], h.IsMezzanine, h.BizCutOff, 
                                    h.BizHouseType, h.BizHouseLocation, h.Location, h.FlowType, h.DoorNum, 
                                    h.RentRate, h.UnitPrice, h.[Weight], h.IsEValue, h.FxtCompanyId, h.Creator, 
                                    h.CreateTime, h.SaveDateTime, h.SaveUser, h.Valid, h.Remarks ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(houseattr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_House_Biz h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.Valid=1 and h.FloorId=@FloorId  and h.BuildingId=@BuildingId ");
                strSql.Append(" and not exists(select sub.HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub sub with(rowlock) where ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId and h.HouseId=sub.HouseId and h.CityId=sub.CityId) ");
                strSql.Append(" and h.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(houseattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_House_Biz_sub h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.FxtCompanyId=@FxtCompanyId and h.Valid=1 and h.FloorId=@FloorId and h.BuildingId=@BuildingId");
                strSql.Append(" order by h.HouseId desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var house_list = con.Query<Dat_House_Biz>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, FloorId = floorId, BuildingId = buildingId }).AsQueryable();
                    return house_list;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 房号名称是否存在
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="houseName"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        public bool IsExistHouseName(long buildingId, long floorId, string houseName, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_House_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                string houseattr = @" select h.HouseId, h.BuildingId, h.FloorId, h.CityId, h.UnitNo, h.HouseName, 
                                    h.PurposeCode, h.SJPurposeCode, h.BuildingArea, h.InnerBuildingArea, 
                                    h.FrontCode, h.Shape, h.Width, h.[Length], h.IsMezzanine, h.BizCutOff, 
                                    h.BizHouseType, h.BizHouseLocation, h.Location, h.FlowType, h.DoorNum, 
                                    h.RentRate, h.UnitPrice, h.[Weight], h.IsEValue, h.FxtCompanyId, h.Creator, 
                                    h.CreateTime, h.SaveDateTime, h.SaveUser, h.Valid, h.Remarks ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(houseattr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_House_Biz h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.Valid=1 and h.FloorId=@FloorId  and h.BuildingId=@BuildingId  and h.HouseName=@HouseName ");
                strSql.Append(" and not exists(select sub.HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub sub with(rowlock) where ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId and h.HouseId=sub.HouseId and h.CityId=sub.CityId) ");
                strSql.Append(" and h.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(houseattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_House_Biz_sub h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.FxtCompanyId=@FxtCompanyId and h.Valid=1 and h.FloorId=@FloorId and h.BuildingId=@BuildingId and h.HouseName=@HouseName ");
                strSql.Append(" order by h.HouseId desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var house_list = con.Query<Dat_House_Biz>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, FloorId = floorId, BuildingId = buildingId, HouseName = houseName }).FirstOrDefault();
                    return house_list != null ? true : false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取房号数量
        /// </summary>
        /// <param name="projectId">商业街Id</param>
        /// <param name="buildingId">楼栋数量</param>
        /// <param name="floorId">楼层号</param>
        /// <returns></returns>
        public int GetHouseCount(int projectId, int buildingId, int floorId, int cityId, int fxtcompanyId)
        {
            //var city_table=GetCityTable(cityId, fxtcompanyId);
            //if (city_table == null || city_table.Count <= 0)
            //{
            //    return new List<Dat_Floor_Biz>();
            //}
            string strSql = @"select HouseId from FxtData_Biz.dbo.Dat_House_Biz h with(nolock)  
                              where h.BuildingId=@BuildingId and h.FloorId=@FloorId and h.CityId=@CityId 
                              and h.FxtCompanyId =@FxtCompanyId and h.Valid =1 and 
                               not exists(select sub.houseId from FxtData_Biz.dbo.Dat_House_Biz_sub sub with(nolock)  
                                        where sub.BuildingId=@BuildingId and sub.FloorId=@FloorId and sub.CityId=@CityId 
                              and sub.FxtCompanyId =@FxtCompanyId and sub.Valid =0)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var list = conn.Query<Dat_House_Biz>(strSql, new { BuildingId = buildingId, FloorId = floorId, CityId = cityId, FxtCompanyId = fxtcompanyId });
                if (list == null || list.Count() == 0) return 0;
                else return list.Count();
                //return conn.Query<int>(strSql, new { buildingId,floorId, cityId, fxtcompanyId }).FirstOrDefault();
            }
        }


        public long GetHouseId(long buildingId, string houseName, int cityId, int fxtCompanyId)
        {
            var cityTable = GetCityTable(cityId, fxtCompanyId).FirstOrDefault();
            var comId = cityTable == null ? "" : cityTable.ShowCompanyId;
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();


            var strSql = @"select HouseId from FxtData_Biz.dbo.Dat_House_Biz h with(nolock)
                            where  h.CityId=@CityId and h.Valid=1 and h.BuildingId=@BuildingId  and h.HouseName=@HouseName 
                            and not exists(select sub.HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub sub with(rowlock) where sub.FxtCompanyId=@FxtCompanyId and h.HouseId=sub.HouseId and h.CityId=sub.CityId)
                            and h.FxtCompanyId in (" + comId + @") 
                            union
                            select HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub h with(nolock) 
                            where h.CityId=@CityId and h.FxtCompanyId=@FxtCompanyId and h.Valid=1 and h.BuildingId=@BuildingId and h.HouseName=@HouseName ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                return conn.Query<long>(strSql, new { buildingId, houseName, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public DataTable HouseSelfDefineExport(Dat_House_Biz houseBiz, List<string> houseAttr, int CityId, int FxtCompanyId, bool self = true)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                var city_table = GetCityTable(Convert.ToInt32(houseBiz.CityId), houseBiz.FxtCompanyId).FirstOrDefault();
                string comId = city_table.ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = houseBiz.FxtCompanyId.ToString();
                if (self) comId = houseBiz.FxtCompanyId.ToString();

                var buildingWhere = string.Empty;
                var projectWhere = string.Empty;
                if (!string.IsNullOrEmpty(houseBiz.ProjectName))
                {
                    projectWhere += " and p.ProjectName like @ProjectName";
                    paramet.Add(new SqlParameter("@ProjectName", houseBiz.ProjectName));
                }
                if (!string.IsNullOrEmpty(houseBiz.ProjectName))
                {
                    projectWhere += " and p.OtherName like @ProjectOtherName";
                    paramet.Add(new SqlParameter("@ProjectOtherName", houseBiz.ProjectOtherName));
                }
                if (!(new[] { 0, -1 }).Contains(houseBiz.AreaId))
                {
                    projectWhere += " and p.AreaId = @AreaId";
                    paramet.Add(new SqlParameter("@AreaId", houseBiz.AreaId));
                }
                if (!string.IsNullOrEmpty(houseBiz.BuildName))
                {
                    buildingWhere += " and buildingTable.BuildingName like @BuildingName";
                    paramet.Add(new SqlParameter("@BuildingName", houseBiz.BuildName));
                }

                var strSql = @"
select 
	houseTable.*
	,FloorTable.FloorNo as floorName
	,buildingTable.BuildingName
	,projectTable.ProjectName
    ,projectTable.OtherName as projectOtherName
	,projectTable.AreaId
	,a.AreaName
	,sa.SubAreaName
	,c.CodeName AS PurposeCodeName
	,c1.CodeName AS SJPurposeCodeName
	,c2.CodeName AS FrontCodeName
	,c3.CodeName AS ShapeName
	,c4.CodeName AS BizCutOffName
	,c5.CodeName AS BizHouseTypeName
	,c6.CodeName AS BizHouseLocationName
	,c7.CodeName AS FlowTypeName
	,(case when houseTable.IsMezzanine = 0 then '否' when houseTable.IsMezzanine = 1 then '是' else '' end) as IsMezzanineName
	,(case when houseTable.IsEValue = 0 then '否' when houseTable.IsEValue = 1 then '是' else '' end) as IsEValueName
from (
	select * from FxtData_Biz.dbo.Dat_House_Biz h with(nolock)
	where not exists(
		select HouseId from fxtdata_biz.dbo.Dat_House_Biz_sub hs with(nolock)
		where hs.HouseId = h.HouseId
		and hs.CityId = h.CityId
		and hs.FxtCompanyId = @fxtCompanyId
	)
	and h.Valid = 1
	and h.CityId = @CityId
	and h.FxtCompanyId in (" + comId + @")
	union
	select * from FxtData_Biz.dbo.Dat_House_Biz_sub h with(nolock)
	where h.Valid = 1
	and h.CityId = @CityId
	and h.FxtCompanyId = @fxtCompanyId
)houseTable
inner join (
	select * from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock)
	where not exists(
		select FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub fs with(nolock)
		where fs.CityId = f.CityId
		and fs.FloorId = f.FloorId
		and fs.FxtCompanyId = @fxtCompanyId
	)
	and f.Valid = 1
	and f.CityId = @cityId
	and f.FxtCompanyId in (" + comId + @")
	union
	select * from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock)
	where f.Valid = 1
	and f.CityId = @cityId
	and f.FxtCompanyId = @fxtCompanyId
)FloorTable on houseTable.FloorId = FloorTable.FloorId
inner join (
	select * from FxtData_Biz.dbo.Dat_Building_Biz b with(nolock)
	where not exists(
		select BuildingId from FxtData_Biz.dbo.Dat_Building_Biz_sub b1 with(nolock)
		where b1.BuildingId = b.BuildingId
		and b1.CityId = b.CityId
		and b1.FxtCompanyId = @fxtCompanyId
	)
	and b.Valid = 1
	and b.CityId = @CityId
	and b.FxtCompanyId in (" + comId + @")" + buildingWhere + @"
	union 
	select * from FxtData_Biz.dbo.Dat_Building_Biz_sub b with(nolock)
	where b.Valid = 1
	and b.CityId = @CityId
	and b.FxtCompanyId = @fxtCompanyId" + buildingWhere + @"
)buildingTable on buildingTable.BuildingId = houseTable.BuildingId
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
	and p.FxtCompanyId = @fxtCompanyId " + projectWhere + @"
)projectTable on buildingTable.ProjectId = projectTable.ProjectId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on projectTable.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Biz sa with(nolock) on projectTable.SubAreaId = sa.SubAreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) ON c.code = houseTable.PurposeCode
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) ON c1.code = houseTable.SJPurposeCode
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) ON c2.code = houseTable.FrontCode
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) ON c3.code = houseTable.Shape
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) ON c4.code = houseTable.BizCutOff
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) ON c5.code = houseTable.BizHouseType
left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) ON c6.code = houseTable.BizHouseLocation
left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) ON c7.code = houseTable.FlowType
where 1 = 1";

                paramet.Add(new SqlParameter("@CityId", CityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                //string paramList = string.Empty;
                //for (int i = 0; i < houseAttr.Count; i++)
                //{
                //    paramList += houseAttr[i].Replace("&", " as ") + ",";
                //}
                var paramList = new StringBuilder();
                foreach (var t in houseAttr)
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

        public long IsExistHouseId(long buildingId, long floorId, string houseName, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                string houseattr = @" select h.HouseId, h.BuildingId, h.FloorId, h.CityId, h.UnitNo, h.HouseName, 
                                    h.PurposeCode, h.SJPurposeCode, h.BuildingArea, h.InnerBuildingArea, 
                                    h.FrontCode, h.Shape, h.Width, h.[Length], h.IsMezzanine, h.BizCutOff, 
                                    h.BizHouseType, h.BizHouseLocation, h.Location, h.FlowType, h.DoorNum, 
                                    h.RentRate, h.UnitPrice, h.[Weight], h.IsEValue, h.FxtCompanyId, h.Creator, 
                                    h.CreateTime, h.SaveDateTime, h.SaveUser, h.Valid, h.Remarks ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(houseattr);
                strSql.Append("  from FxtData_Biz.dbo.Dat_House_Biz h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.Valid=1 and h.FloorId=@FloorId  and h.BuildingId=@BuildingId  and h.HouseName=@HouseName ");
                strSql.Append(" and not exists(select sub.HouseId from FxtData_Biz.dbo.Dat_House_Biz_sub sub with(rowlock) where ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId and h.HouseId=sub.HouseId and h.CityId=sub.CityId) ");
                strSql.Append(" and h.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(houseattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_House_Biz_sub h with(nolock) ");
                strSql.Append(" where h.CityId=@CityId and h.FxtCompanyId=@FxtCompanyId and h.Valid=1 and h.FloorId=@FloorId and h.BuildingId=@BuildingId and h.HouseName=@HouseName ");
                strSql.Append(" order by h.HouseId desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var houseid = con.Query<Dat_House_Biz>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, FloorId = floorId, BuildingId = buildingId, HouseName = houseName }).FirstOrDefault().HouseId;
                    return houseid;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
