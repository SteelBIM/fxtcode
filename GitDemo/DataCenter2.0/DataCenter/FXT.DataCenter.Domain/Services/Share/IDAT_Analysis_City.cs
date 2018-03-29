using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 区域分析接口
    /// </summary>
    public interface IDAT_Analysis_City
    {
        /// <summary>
        /// 获取区域分析列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<DAT_Analysis_City> GetAnalysisList(DAT_Analysis_City model, bool self = true);
        
        /// <summary>
        /// 新增一条区域分析记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddAnalysis(DAT_Analysis_City model);

        /// <summary>
        /// 更新一条区域分析记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(DAT_Analysis_City model);
        /// <summary>
        /// 获取一条分析记录
        /// </summary>
        /// <param name="id">分析Id</param>
        /// <param name="dataCode">分析数据类型</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtcompanyId">评估机构ID</param>
        /// <returns></returns>
        DAT_Analysis_City GetAnalysisById(int id, int dataCode, int cityId, int fxtcompanyId);
    }
}
