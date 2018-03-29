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
  public class MarketAnalysis : IMarketAnalysis
    {

        private IQueryable<SYS_City_Table> GetCityTable(int cityId, int fxtCompanyId = 0)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                if (fxtCompanyId > 0)
                {
                    string strsql = "SELECT c.CityId, ProjectTable, BuildingTable, HouseTable, CaseTable, QueryInfoTable, ReportTable, PrintTable, HistoryTable, QueryTaxTable, CaseBusinessTable, CaseLandTable, QueryAdjustTable, QueryTaxSOATable, QueryFlowTable, CASHistoryTable, MessageTable, QueryCheckTable, ReportCheckTable, SurveyTable, SurveyBusinessTable, SurveyFactoryTable, SurveyLandTable, SurveyOfficeTable, SurveyOtherTable, QueryFilesTable, SurveyTextTable, SubHousePriceTable, QProjectTable, ReportEntrustTable, SurveyCaseTable, SearchHistoryTable, QueryYPDat, QueryYPCheckTable,s.BizCompanyId as ShowCompanyId FROM [dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and s.FxtCompanyId=@FxtCompanyId and typecode= 1003002";
                    return con.Query<SYS_City_Table>(strsql, new { CityId = cityId, FxtCompanyId = fxtCompanyId }).AsQueryable();
                }
                else
                {
                    string strsql = "SELECT CityId, ProjectTable, BuildingTable, HouseTable, CaseTable, QueryInfoTable, ReportTable, PrintTable, HistoryTable, QueryTaxTable, CaseBusinessTable, CaseLandTable, QueryAdjustTable, QueryTaxSOATable, QueryFlowTable, CASHistoryTable, MessageTable, QueryCheckTable, ReportCheckTable, SurveyTable, SurveyBusinessTable, SurveyFactoryTable, SurveyLandTable, SurveyOfficeTable, SurveyOtherTable, QueryFilesTable, SurveyTextTable, SubHousePriceTable, QProjectTable, ReportEntrustTable, SurveyCaseTable, SearchHistoryTable, QueryYPDat, QueryYPCheckTable FROM [dbo].[SYS_City_Table] with(nolock) where CityId=@CityId";
                    return con.Query<SYS_City_Table>(strsql, new { CityId = cityId }).AsQueryable();
                }
            }

        }
        /// <summary>
        /// 获取区域分析列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<DatAnalysisMarket> GetAnalysisList(DatAnalysisMarket model, bool self = true)
        {
            try
            {
                var cityTable = GetCityTable(model.CityId, model.FxtCompanyId).FirstOrDefault();
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new List<DAT_Analysis_City>();
                //}
                var comId = cityTable == null ? "" : cityTable.ShowCompanyId;
                if (string.IsNullOrEmpty(comId)) comId = model.FxtCompanyId.ToString();

                var where = "";
                if (model.AreaId > 0)
                {
                    where += " and a.AreaId=@AreaId";
                }
                //if (model.SubAreaId > 0)
                //{
                //    where += " and a.SubAreaId=@SubAreaId";
                //}
                if (self)//查看自己
                {
                    where += " and a.FxtCompanyId in (" + comId + ") ";
                }
                var strSql = new StringBuilder();
                strSql.Append("select c.CityName CityName,are.AreaName AreaName,subare.SubAreaName SubAreaName,bizsubare.SubAreaName BizSubAreaName,officesubarea.SubAreaName OfficeSubAreaName,Industrysubarea.SubAreaName IndustrySubAreaName, a.Id, a.Valid, a.AreaId, a.SubAreaId, a.DataTypeCode,");
                strSql.Append("a.FxtCompanyId, a.CityID, a.Analysis, a.Creator, a.CreateTime, a.SaveUser, a.SaveDateTime");
                strSql.Append("  from FxtDataCenter.dbo.DAT_Analysis_Market a with(nolock)");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_City c with(nolock) on a.CityID=c.CityId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_Area are with(nolock) on are.AreaId=a.AreaId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_SubArea subare with(nolock) on subare.SubAreaId=a.SubAreaId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_SubArea_Biz bizsubare with(nolock) on bizsubare.SubAreaId=a.SubAreaId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_SubArea_Office officesubarea with(nolock) on officesubarea.SubAreaId=a.SubAreaId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_SubArea_Industry Industrysubarea with(nolock) on Industrysubarea.SubAreaId=a.SubAreaId");
                strSql.Append("  where a.DataTypeCode=@DataTypeCode and a.FxtCompanyId=@FxtCompanyId");
                strSql.Append("  and a.CityID=@CityID and a.Valid=1");
                strSql.Append(where);
                strSql.Append(" order by a.Id desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
                {
                    var analyList = con.Query<DatAnalysisMarket>(strSql.ToString(), model).AsQueryable();
                    return analyList;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增一条区域分析数据
        /// </summary>
        public int AddAnalysis(DatAnalysisMarket model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into FxtDataCenter.dbo.DAT_Analysis_Market with(rowlock) (");
                strSql.Append("DataTypeCode, FxtCompanyId, CityID, Analysis,");
                strSql.Append("Creator, CreateTime, SaveUser, SaveDateTime, Valid, AreaId, SubAreaId");
                strSql.Append(") values (");
                strSql.Append("@DataTypeCode, @FxtCompanyId, @CityID, @Analysis,");
                strSql.Append("@Creator, @CreateTime, @SaveUser, @SaveDateTime, @Valid, @AreaId, @SubAreaId");
                strSql.Append(") ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
                {
                    int result = con.Execute(strSql.ToString(), model);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 更新一条区域分析数据
        /// </summary>
        public bool Update(DatAnalysisMarket model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update DAT_Analysis_Market with(rowlock) set ");
                strSql.Append(" AreaId = @AreaId ,");
                strSql.Append(" SubAreaId = @SubAreaId ,");
                strSql.Append(" Analysis = @Analysis ,");
                strSql.Append(" SaveUser = @SaveUser ,");
                strSql.Append(" SaveDateTime = @SaveDateTime ");
                strSql.Append(" where Id=@Id and DataTypeCode=@DataTypeCode and FxtCompanyId=@FxtCompanyId");
                //strSql.Append(" and CityID=@CityID and AreaId=@AreaId and SubAreaId=@SubAreaId");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
                {
                    int result = con.Execute(strSql.ToString(), model);
                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取一条分析记录
        /// </summary>
        /// <param name="id">分析Id</param>
        /// <param name="dataCode">分析数据类型</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtcompanyId">评估机构ID</param>
        /// <returns></returns>
        public DatAnalysisMarket GetAnalysisById(int id, int dataCode, int cityId, int fxtcompanyId)
        {
            try
            {
                var cityTable = GetCityTable(cityId, fxtcompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new List<DAT_Analysis_City>();
                //}
                string comId = cityTable.FirstOrDefault().ShowCompanyId;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select Id, Valid, AreaId, SubAreaId, DataTypeCode,");
                strSql.Append("FxtCompanyId, CityID, Analysis, Creator, CreateTime, SaveUser, SaveDateTime");
                strSql.Append("  from FxtDataCenter.dbo.DAT_Analysis_Market with(nolock)");
                strSql.Append("  where Id=@Id and DataTypeCode=@DataTypeCode and FxtCompanyId=@FxtCompanyId");
                strSql.Append("  and CityID=@CityID and Valid=1");
                strSql.Append("  and FxtCompanyId in (" + comId + ")");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
                {
                    var analyList = con.Query<DatAnalysisMarket>(strSql.ToString(), new { Id = id, DataTypeCode = dataCode, FxtCompanyId = fxtcompanyId, CityID = cityId }).FirstOrDefault();
                    return analyList;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
