using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Application.Interfaces.Share
{
    public interface IMarketAnalysisServices
    {
        /// <summary>
        /// 获取区域分析列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<DatAnalysisMarket> GetAnalysisList(DatAnalysisMarket model, bool self = true);

        /// <summary>
        /// 新增一条区域分析记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddAnalysis(DatAnalysisMarket model);

        /// <summary>
        /// 更新一条区域分析记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(DatAnalysisMarket model);
        /// <summary>
        /// 获取一条分析记录
        /// </summary>
        /// <param name="id">分析Id</param>
        /// <param name="dataCode">分析数据类型</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtcompanyId">评估机构ID</param>
        /// <returns></returns>
        DatAnalysisMarket GetAnalysisById(int id, int dataCode, int cityId, int fxtcompanyId);


        #region 非业务接口

        /// <summary>
        /// 获取行政区名称列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetAreaName(int cityId);

        /// <summary>
        /// 获取片区名称列表
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetSubAreaName(int areaId);

        /// <summary>
        /// 写系统日志
        /// </summary>        
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="logType">对象类型7002006（楼盘、楼栋、房号、住宅案例）</param>
        /// <param name="eventType">操作类型7001001(新增、修改、删除)</param>
        /// <param name="objectId">对象ID</param>
        /// <param name="objectName">对象名称</param>
        /// <param name="remarks">操作描述</param>
        /// <param name="webIp">IP地址</param>
        /// <returns></returns>
        int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp);

        IQueryable<SYS_SubArea_Office> GetOfficeSubAreaNamesByAreaId(int areaid, int fxtCompanyId, int CityId);

        IQueryable<SYS_SubArea_Industry> GetIndustrySubAreaNamesByAreaId(int areaid, int fxtCompanyId, int CityId);

        #endregion
    }
}
