using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
  public  interface IMarketAnalysis
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
    }
}
