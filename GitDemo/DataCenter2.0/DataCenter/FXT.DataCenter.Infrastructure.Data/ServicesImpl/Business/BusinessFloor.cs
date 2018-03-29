using System;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    /// <summary>
    /// 楼层
    /// </summary>
    public class Dat_Floor_BizDAL : IDat_Floor_Biz
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
        /// 获取楼层列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<Dat_Floor_Biz> GetDat_Floor_BizList(Dat_Floor_Biz model, bool self = true)
        {
            try
            {
                var city_table = GetCityTable(model.CityId, model.FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Floor_Biz();
                //}
                string jqueryStr = JqueryWhere(model);
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = model.FxtCompanyId.ToString();
                string floorattr = @"select f.FloorId, f.BuildingBizType, f.BizType, f.FxtCompanyId,
                              f.Creator, f.CreateTime, f.SaveDateTime, f.SaveUser, f.Valid, f.Remarks,
                              f.CityId, f.BuildingId, f.FloorNo, f.FloorNum, f.BuildingArea, f.FloorHigh,
                              f.FloorPicture, f.RentSaleType,
                              city.CityName CityName,build.BuildingName BuildingName,
                              BizType.CodeName BizTypeName,BuildingBizType.CodeName BuildingBizTypeName,
                              RentSaleType.CodeName RentSaleTypeName,f.AveragePrice,f.Weight ";
                string leftTable = @" left join FxtDataCenter.dbo.SYS_City city with(nolock) on city.CityId=f.CityId  
                                      left join FxtData_Biz.dbo.Dat_Building_Biz build with(nolock) on build.BuildingId=f.BuildingId  
                                      left join [FXTDataCenter].[dbo].SYS_Code BizType with(nolock) on BizType.code=f.BizType  
                                      left join [FXTDataCenter].[dbo].SYS_Code BuildingBizType with(nolock) on BuildingBizType.code=f.BuildingBizType  
                                      left join [FXTDataCenter].[dbo].SYS_Code RentSaleType with(nolock) on RentSaleType.code=f.RentSaleType  ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) ");
                strSql.Append(leftTable);
                strSql.Append(" where f.CityId=@CityId and f.Valid=1 and f.BuildingId=@BuildingId ");
                strSql.Append(" and not exists(select sub.FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub sub with(rowlock) where sub.CityId=f.CityId ");
                strSql.Append(" and sub.FxtCompanyId=@FxtCompanyId and f.FloorId=sub.FloorId ) ");
                if (self)//查看自己
                {
                    strSql.Append(" and f.FxtCompanyId=@FxtCompanyId  ");
                }
                else
                {
                    strSql.Append(" and f.FxtCompanyId in (" + comId + ") ");
                }
                strSql.Append(jqueryStr);
                strSql.Append(" union ");
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock) ");
                strSql.Append(leftTable);
                strSql.Append(" where f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId and f.Valid=1 and f.BuildingId=@BuildingId");
                strSql.Append(jqueryStr);
                strSql.Append(" order by f.FloorNo asc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var floor_list = con.Query<Dat_Floor_Biz>(strSql.ToString(), model).AsQueryable();
                    return floor_list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 获取楼层信息
        /// </summary>
        /// <param name="floorId"></param>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <returns></returns>
        public Dat_Floor_Biz GetDat_Floor_BizById(int floorId, int CityId, int FxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(CityId, FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Floor_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = FxtCompanyId.ToString();
                string floorattr = @"select f.FloorId, f.BuildingBizType, f.BizType, f.FxtCompanyId,
                              f.Creator, f.CreateTime, f.SaveDateTime, f.SaveUser, f.Valid, f.Remarks,
                              f.CityId, f.BuildingId, f.FloorNo, f.FloorNum, f.BuildingArea, f.FloorHigh,
                              f.FloorPicture, f.RentSaleType,f.AveragePrice,f.Weight ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.Valid=1 and f.FloorId=@FloorId ");
                strSql.Append(" and not exists(select sub.FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub sub with(rowlock) where  ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId  and f.FloorId=sub.FloorId and f.CityId=sub.CityId) ");
                strSql.Append(" and f.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId and f.Valid=1 and f.FloorId=@FloorId");
                strSql.Append(" order by f.FloorNo asc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var floor_list = con.Query<Dat_Floor_Biz>(strSql.ToString(), new { CityId = CityId, FxtCompanyId = FxtCompanyId, FloorId = floorId }).FirstOrDefault();
                    return floor_list;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 新增楼层
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDat_Floor_Biz(Dat_Floor_Biz model)
        {
            try
            {
                //var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return 0;
                //}
                //判断是否已删除
                //var floorList = FloorIsDelete(model, 0);
                //if (floorList != null && floorList.Count() > 0)
                //{
                //    int floorId = Convert.ToInt32(floorList.FirstOrDefault().FloorId);
                //    model.FloorId = floorId;
                //    model.Valid = 1;
                //    UpdateDat_Floor_Biz(model);
                //    return floorId;
                //}
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into FxtData_Biz.dbo.Dat_Floor_Biz with(rowlock) (");
                strSql.Append("BuildingBizType,BizType,FxtCompanyId,Creator,CreateTime,SaveDateTime,");
                strSql.Append("SaveUser,Valid,Remarks,CityId,BuildingId,FloorNo,FloorNum,BuildingArea,");
                strSql.Append("FloorHigh,FloorPicture,RentSaleType,AveragePrice,Weight");
                strSql.Append(") values (");
                strSql.Append("@BuildingBizType,@BizType,@FxtCompanyId,@Creator,@CreateTime,@SaveDateTime,");
                strSql.Append("@SaveUser,@Valid,@Remarks,@CityId,@BuildingId,@FloorNo,@FloorNum,@BuildingArea,");
                strSql.Append("@FloorHigh,@FloorPicture,@RentSaleType,@AveragePrice,@Weight");
                strSql.Append(") ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    model.CreateTime = DateTime.Now;
                    model.Valid = 1;
                    int result = con.Execute(strSql.ToString(), model);
                    if (result > 0)
                    {
                        strSql.Clear();
                        strSql.Append(" select top 1 FloorId from FxtData_Biz.dbo.Dat_Floor_Biz");
                        strSql.Append(" with(nolock) order by FloorId desc");
                        var obj = con.Query<Dat_Floor_Biz>(strSql.ToString()).FirstOrDefault();
                        return Convert.ToInt32(obj.FloorId);
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
        /// 更新楼层
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDat_Floor_Biz(Dat_Floor_Biz model, int currFxtCompanyId)
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
                strSql.Append(" FloorNo = @FloorNo , ");
                strSql.Append(" FloorNum = @FloorNum , ");
                strSql.Append(" BuildingArea = @BuildingArea , ");
                strSql.Append(" FloorHigh = @FloorHigh , ");
                strSql.Append(" FloorPicture = @FloorPicture , ");
                strSql.Append(" RentSaleType = @RentSaleType , ");
                strSql.Append(" BuildingBizType = @BuildingBizType , ");
                strSql.Append(" BizType = @BizType , ");
                strSql.Append(" SaveDateTime = @SaveDateTime , ");
                strSql.Append(" SaveUser = @SaveUser , ");
                strSql.Append(" Remarks = @Remarks, ");
                strSql.Append(" AveragePrice = @AveragePrice, ");
                strSql.Append(" Weight = @Weight ");
                strSql.Append(" where FloorId=@FloorId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    //房讯通
                    if (model.FxtCompanyId == FxtComId)
                    {
                        sql = " update FxtData_Biz.dbo.Dat_Floor_Biz with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    //房讯通
                    if (model.FxtCompanyId == currFxtCompanyId)
                    {
                        sql = " update FxtData_Biz.dbo.Dat_Floor_Biz with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    else
                    {
                        sql = "select FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub with(nolock) where FloorId=@FloorId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        Dat_Floor_Biz buil_sub = con.Query<Dat_Floor_Biz>(sql, new { FloorId = model.FloorId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                        if (buil_sub != null)//子表存在
                        {
                            sql = " update FxtData_Biz.dbo.Dat_Floor_Biz_sub with(rowlock) set ";
                            int reslut = con.Execute(sql + strSql.ToString(), model);
                            return reslut;
                        }
                        else
                        {
                            sql = "select FloorId from FxtData_Biz.dbo.Dat_Floor_Biz with(nolock) where FloorId=@FloorId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                            Dat_Floor_Biz buil = con.Query<Dat_Floor_Biz>(sql, new { FloorId = model.FloorId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                            if (buil != null)//主表存在
                            {
                                sql = " update FxtData_Biz.dbo.Dat_Floor_Biz with(rowlock) set ";
                                int reslut = con.Execute(sql + strSql.ToString(), model);
                                return reslut;
                            }
                            else
                            {
                                //主表字表不在(在字表中插入一条记录来自主表)
                                strSql.Clear();
                                strSql.Append("insert into FxtData_Biz.dbo.Dat_Floor_Biz_sub with(rowlock) (");
                                strSql.Append("FloorId,BuildingBizType,BizType,FxtCompanyId,Creator,CreateTime,SaveDateTime,");
                                strSql.Append("SaveUser,Valid,Remarks,CityId,BuildingId,FloorNo,FloorNum,BuildingArea,");
                                strSql.Append("FloorHigh,FloorPicture,RentSaleType,AveragePrice,Weight)");
                                strSql.Append(" select ");
                                strSql.Append(" FloorId,BuildingBizType,BizType,'" + currFxtCompanyId + "' as FxtCompanyId,'" + model.SaveUser + " as ' Creator,getdate() as CreateTime,getdate() as SaveDateTime,");
                                strSql.Append(" '" + model.SaveUser + " as ' SaveUser,Valid,Remarks,CityId,BuildingId,FloorNo,FloorNum,BuildingArea,");
                                strSql.Append(" FloorHigh,FloorPicture,RentSaleType,AveragePrice,Weight ");
                                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz with(nolock) ");
                                strSql.Append(" where FloorId=@FloorId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                                int reslut = con.Execute(strSql.ToString(), new { FloorId = model.FloorId, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId });
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
        /// 删除楼层
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public bool DeleteDat_Floor_Biz(int floorId, string userName, int cityId, int fxtCompanyId, int poductTypeCode, int currFxtCompanyId)
        {
            try
            {
                //var city_table = GetCityTable(cityId, FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return false;
                //}
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
                //////        return DeleteFloor(cityId, fxtCompanyId, floorId);
                //////    }
                //////    else
                //////    {
                        
                //////    }
                //////}
                //////else
                //////{
                //////    return false;
                //////}
                return DeleteFloor(cityId, fxtCompanyId, floorId, userName, poductTypeCode, currFxtCompanyId);
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
        /// <param name="floorId"></param>
        /// <param name="userName"></param>
        /// <param name="poductTypeCode"></param>
        /// <returns></returns>
        private bool DeleteFloor(int cityId, int fxtCompanyId, int floorId, string userName, int poductTypeCode, int currFxtCompanyId)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append(" set Valid=0,[SaveDateTime]=GetDate(),[SaveUser]='" + userName + "' ");
                    strsql.Append(" where [FloorId]=@FloorId and CityId=@CityId and FxtCompanyId=@FxtCompanyId");

                    if (fxtCompanyId == FxtComId)
                    {

                        string sql = "update FxtData_Biz.dbo.Dat_Floor_Biz with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { FloorId = floorId, CityId = cityId, FxtCompanyId = fxtCompanyId });
                        return result > 0;
                    }
                    if (fxtCompanyId == currFxtCompanyId)
                    {

                        string sql = "update FxtData_Biz.dbo.Dat_Floor_Biz with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { FloorId = floorId, CityId = cityId, FxtCompanyId = fxtCompanyId });
                        return result > 0;
                    }
                    else
                    {
                        string sql_query = " select FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub with(nolock) where [FloorId]=@FloorId and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        Dat_Building_Biz sub = con.Query<Dat_Building_Biz>(sql_query, new { FloorId = floorId, CityId = cityId, FxtCompanyId = fxtCompanyId }).FirstOrDefault();
                        if (sub != null)
                        {
                            string sql = "update FxtData_Biz.dbo.Dat_Floor_Biz_sub with(rowlock) " + strsql.ToString();
                            int result = con.Execute(sql, new { FloorId = floorId, CityId = cityId, FxtCompanyId = fxtCompanyId });
                            return result > 0;
                        }
                        else
                        {
                            strsql.Clear();
                            strsql.Append("insert into FxtData_Biz.dbo.Dat_Floor_Biz_sub with(rowlock) (");
                            strsql.Append("FloorId,BuildingBizType,BizType,FxtCompanyId,Creator,CreateTime,SaveDateTime,");
                            strsql.Append("SaveUser,Valid,Remarks,CityId,BuildingId,FloorNo,FloorNum,BuildingArea,");
                            strsql.Append("FloorHigh,FloorPicture,RentSaleType,AveragePrice,Weight)");
                            strsql.Append(" select ");
                            strsql.Append(" FloorId,BuildingBizType,BizType,'" + currFxtCompanyId + "' as FxtCompanyId,Creator,getdate() as CreateTime,getdate() as SaveDateTime,");
                            strsql.Append(" '" + userName + " as ' SaveUser,Valid,Remarks,CityId,BuildingId,FloorNo,FloorNum,BuildingArea,");
                            strsql.Append(" FloorHigh,FloorPicture,RentSaleType,AveragePrice,Weight ");
                            strsql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz ");
                            strsql.Append(" where FloorId=@FloorId and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                            int reslut = con.Execute(strsql.ToString(), new { FloorId = floorId, CityId = cityId, FxtCompanyId = fxtCompanyId });
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
        /// 物理删除
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="floorId"></param>
        /// <returns></returns>
        private bool DeleteFloor(int cityId, int fxtCompanyId, int floorId)
        {
            try
            {
                string sql = " delete FxtData_Biz.dbo.Dat_Floor_Biz with(rowlock) where FxtCompanyId in(25," + fxtCompanyId + ") and FloorId=" + floorId + " and CityId=" + cityId + " "
                      + " delete FxtData_Biz.dbo.Dat_Floor_Biz_sub with(rowlock) where FxtCompanyId in(25," + fxtCompanyId + ") and FloorId=" + floorId + " and CityId=" + cityId + " ";
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
        /// 根据楼栋id获取楼层集合
        /// </summary>
        /// <param name="buildId">楼栋ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        public IQueryable<Dat_Floor_Biz> GetDat_Floor_BizByBuildId(int buildId, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Floor_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                string floorattr = @"select f.FloorId, f.BuildingBizType, f.BizType, f.FxtCompanyId,
                              f.Creator, f.CreateTime, f.SaveDateTime, f.SaveUser, f.Valid, f.Remarks,
                              f.CityId, f.BuildingId, f.FloorNo, f.FloorNum, f.BuildingArea, f.FloorHigh,
                              f.FloorPicture, f.RentSaleType,AveragePrice,Weight ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.Valid=1 and f.BuildingId=@BuildingId ");
                strSql.Append(" and not exists(select sub.FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub sub with(rowlock) where ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId  and f.FloorId=sub.FloorId and f.CityId=sub.CityId) ");
                strSql.Append(" and f.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId and f.Valid=1 and f.BuildingId=@BuildingId");
                strSql.Append(" order by f.FloorNo asc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var floor_list = con.Query<Dat_Floor_Biz>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, BuildingId = buildId }).AsQueryable();
                    return floor_list;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// form提交验证
        /// </summary>
        /// <param name="floorNo"></param>
        /// <param name="floorNum"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public Dat_Floor_Biz FormValidFloor(string floorNo, string buildingId, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Floor_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                string floorattr = @"select f.FloorId, f.BuildingBizType, f.BizType, f.FxtCompanyId,
                              f.Creator, f.CreateTime, f.SaveDateTime, f.SaveUser, f.Valid, f.Remarks,
                              f.CityId, f.BuildingId, f.FloorNo, f.FloorNum, f.BuildingArea, f.FloorHigh,
                              f.FloorPicture, f.RentSaleType,AveragePrice,Weight ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) ");
                strSql.Append(" where f.BuildingId=@BuildingId and f.Valid=1 and f.FloorNo=@FloorNo ");
                strSql.Append(" and not exists(select sub.FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub sub with(rowlock) ");
                strSql.Append(" where  f.FloorId=sub.FloorId ) ");
                strSql.Append(" and f.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock) ");
                strSql.Append(" where  f.BuildingId=@BuildingId and f.Valid=1 and f.FloorNo=@FloorNo ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var floorObj = con.Query<Dat_Floor_Biz>(strSql.ToString(), new { BuildingId = buildingId, FloorNo = floorNo }).FirstOrDefault();
                    return floorObj;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取楼层Id
        /// </summary>
        /// <param name="buildingId">楼栋Id</param>
        /// <param name="floorName">物理层</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        public long GetFloorIdByName(long buildingId, string floorNo, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new Dat_Floor_Biz();
                //}
                string comId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
                string floorattr = @"select f.FloorId, f.BuildingBizType, f.BizType, f.FxtCompanyId,
                              f.Creator, f.CreateTime, f.SaveDateTime, f.SaveUser, f.Valid, f.Remarks,
                              f.CityId, f.BuildingId, f.FloorNo, f.FloorNum, f.BuildingArea, f.FloorHigh,
                              f.FloorPicture,f.RentSaleType,f.AveragePrice,f.Weight ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) ");
                strSql.Append(" where f.BuildingId=@BuildingId and f.Valid=1 and f.FloorNo=@FloorNo and f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId ");
                strSql.Append(" and not exists(select sub.FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub sub with(rowlock) ");
                strSql.Append(" where  f.FloorId=sub.FloorId ) ");
                strSql.Append(" and f.FxtCompanyId in (" + comId + ") ");
                strSql.Append(" union ");
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock) ");
                strSql.Append(" where  f.BuildingId=@BuildingId and f.Valid=1 and f.FloorNo=@FloorNo and f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId  ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var floorObj = con.Query<Dat_Floor_Biz>(strSql.ToString(), new { BuildingId = buildingId, FloorNo = floorNo, CityId = cityId, FxtCompanyId = fxtCompanyId }).FirstOrDefault();
                    return floorObj == null ? -1 : Convert.ToInt32(floorObj.FloorId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 验证物理层、实际层唯一性
        /// 刘晓博
        /// 2014-09-11
        /// </summary>
        /// <param name="floor">楼层(物理层、实际层)</param>
        /// <param name="dataAttr">属性(物理层:FloorNo、实际层:FloorNum)</param>
        /// <returns></returns>
        public bool ValidFloor(string floor, string dataAttr, string buildingId, int valid = 1)
        {
            try
            {

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    string sql = @" select FloorNo,FloorNum 
                                    from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) 
                                    where BuildingId=@BuildingId  and valid=@valid ";
                    if (dataAttr.Trim() == "FloorNo")//物理层
                    {
                        sql += " and FloorNo=@FloorNo";
                    }
                    else if (dataAttr.Trim() == "FloorNum")//实际层
                    {
                        sql += " and FloorNum=@FloorNo";
                    }
                    var floorObj = con.Query<Dat_Floor_Biz>(sql, new { BuildingId = buildingId, FloorNo = floor, valid = valid }).FirstOrDefault();
                    return floorObj != null ? true : false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 是否删除楼层
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<Dat_Floor_Biz> FloorIsDelete(Dat_Floor_Biz model, int valid)
        {
            try
            {
                //var city_table=GetCityTable(cityId, fxtCompanyId);
                //if (city_table == null || city_table.Count <= 0)
                //{
                //    return new List<Dat_Floor_Biz>();
                //}
                string floorattr = @"select f.FloorId, f.BuildingBizType, f.BizType, f.FxtCompanyId,
                              f.Creator, f.CreateTime, f.SaveDateTime, f.SaveUser, f.Valid, f.Remarks,
                              f.CityId, f.BuildingId, f.FloorNo, f.FloorNum, f.BuildingArea, f.FloorHigh,
                              f.FloorPicture, f.RentSaleType,f.AveragePrice,f.Weight ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.Valid=@Valid and f.FloorNo=@FloorNo and f.FloorNum=@FloorNum ");
                strSql.Append(" and not exists(select sub.FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub sub with(rowlock) where  ");
                strSql.Append(" sub.FxtCompanyId=@FxtCompanyId and f.FloorId=sub.FloorId and f.CityId=sub.CityId) ");
                strSql.Append(" union ");
                strSql.Append(floorattr);
                strSql.Append(" from FxtData_Biz.dbo.Dat_Floor_Biz_sub f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId and f.Valid=@Valid and f.FloorNo=@FloorNo and f.FloorNum=@FloorNum");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var floor_list = con.Query<Dat_Floor_Biz>(strSql.ToString(), new { CityId = model.CityId, FxtCompanyId = model.FxtCompanyId, Valid = valid, FloorNo = model.FloorNo, FloorNum = model.FloorNum }).AsQueryable();
                    return floor_list;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取楼层数量
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <returns></returns>
        public int GetFloorCount(int projectId, int buildingId, int cityId, int fxtcompanyId)
        {
            //var city_table=GetCityTable(cityId, fxtcompanyId);
            //if (city_table == null || city_table.Count <= 0)
            //{
            //    return new List<Dat_Floor_Biz>();
            //}
            string strSql = @"select FloorId from FxtData_Biz.dbo.Dat_Floor_Biz f with(nolock) 
                              where f.BuildingId=@BuildingId and f.CityId=@CityId 
                              and f.FxtCompanyId =@FxtCompanyId and f.Valid =1 
                              and not exists(select sub.FloorId from FxtData_Biz.dbo.Dat_Floor_Biz_sub sub with(nolock) 
                              where sub.BuildingId=@BuildingId and sub.CityId=@CityId 
                              and sub.FxtCompanyId =@FxtCompanyId and sub.Valid =0 )";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
            {
                var list = conn.Query<Dat_Floor_Biz>(strSql, new { BuildingId = buildingId, CityId = cityId, FxtCompanyId = fxtcompanyId });
                if (list == null || list.Count() == 0) return 0;
                else return list.Count();
                //return conn.Query<int>(strSql, new { buildingId, cityId, fxtcompanyId }).FirstOrDefault();
            }
        }
        /// <summary>
        /// 查询功能条件拼凑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string JqueryWhere(Dat_Floor_Biz model)
        {
            string querStr = "";
            if (!string.IsNullOrEmpty(model.FloorNum))
            {
                querStr += " and (f.FloorNum=@FloorNum or f.FloorNum like '%" + model.FloorNum.Trim() + "%')";
            }
            if (model.RentSaleType > 0)
            {
                querStr += " and f.RentSaleType=@RentSaleType";
            }
            if (model.BuildingBizType > 0)
            {
                querStr += " and f.BuildingBizType=@BuildingBizType";
            }
            if (model.BizType > 0)
            {
                querStr += " and f.BizType=@BizType";
            }
            return querStr;
        }

        public long IsExistFloor(string floor, string dataAttr, string buildingId)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    string sql = @"select * from FxtData_Biz.dbo.Dat_Floor_Biz where BuildingId = @BuildingId and Valid = 1";
                    if (dataAttr.Trim() == "FloorNo")//物理层
                    {
                        sql += " and FloorNo=@FloorNo";
                    }
                    else if (dataAttr.Trim() == "FloorNum")//实际层
                    {
                        sql += " and FloorNum=@FloorNo";
                    }
                    var floorId = con.Query<Dat_Floor_Biz>(sql, new { BuildingId = buildingId, FloorNo = floor }).FirstOrDefault().FloorId;

                    return floorId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
