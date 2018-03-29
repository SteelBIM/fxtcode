using System;
using System.Linq;
using System.Text;
using Dapper;
using System.Data;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    /// <summary>
    /// 商业区域分析数据类
    /// </summary>
    public class DAT_Analysis_CityDAL : IDAT_Analysis_City
    {

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
        /// 获取区域分析列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<DAT_Analysis_City> GetAnalysisList(DAT_Analysis_City model, bool self = true)
        {
            try
            {
                var cityTable = GetCityTable(model.CityID, model.FxtCompanyId).FirstOrDefault();
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new List<DAT_Analysis_City>();
                //}
                var comId = cityTable==null?"":cityTable.ShowCompanyId;
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
                strSql.Append("select c.CityName CityName,are.AreaName AreaName,subare.SubAreaName SubAreaName,bizsubare.SubAreaName BizSubAreaName,officesubarea.SubAreaName OfficeSubAreaName,a.Id, a.Valid, a.AreaId, a.SubAreaId, a.DataTypeCode,");
                strSql.Append("a.FxtCompanyId, a.CityID, a.Analysis, a.Creator, a.CreateTime, a.SaveUser, a.SaveDateTime");
                strSql.Append("  from FxtDataCenter.dbo.DAT_Analysis_City a with(nolock)");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_City c with(nolock) on a.CityID=c.CityId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_Area are with(nolock) on are.AreaId=a.AreaId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_SubArea subare with(nolock) on subare.SubAreaId=a.SubAreaId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_SubArea_Biz bizsubare with(nolock) on bizsubare.SubAreaId=a.SubAreaId");
                strSql.Append(" left join FxtDataCenter.dbo.SYS_SubArea_Office officesubarea with(nolock) on officesubarea.SubAreaId=a.SubAreaId");
                strSql.Append("  where a.DataTypeCode=@DataTypeCode and a.FxtCompanyId=@FxtCompanyId");
                strSql.Append("  and a.CityID=@CityID and a.Valid=1");
                strSql.Append(where);
                strSql.Append(" order by a.Id desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
                {
                    var analyList = con.Query<DAT_Analysis_City>(strSql.ToString(), model).AsQueryable();
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
        public int AddAnalysis(DAT_Analysis_City model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into FxtDataCenter.dbo.DAT_Analysis_City with(rowlock) (");
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
        public bool Update(DAT_Analysis_City model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update DAT_Analysis_City with(rowlock) set ");
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
        public DAT_Analysis_City GetAnalysisById(int id, int dataCode, int cityId, int fxtcompanyId)
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
                strSql.Append("  from FxtDataCenter.dbo.DAT_Analysis_City with(nolock)");
                strSql.Append("  where Id=@Id and DataTypeCode=@DataTypeCode and FxtCompanyId=@FxtCompanyId");
                strSql.Append("  and CityID=@CityID and Valid=1");
                strSql.Append("  and FxtCompanyId in (" + comId + ")");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
                {
                    var analyList = con.Query<DAT_Analysis_City>(strSql.ToString(), new { Id = id, DataTypeCode = dataCode, FxtCompanyId = fxtcompanyId, CityID = cityId }).FirstOrDefault();
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
