using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;


namespace FXT.DataCenter.Domain.Services
{
   public interface ILandCase
    {
       ///  <summary>
       /// 获取统计数据
       ///  </summary>
       ///  <param name="startDate"></param>
       ///  <param name="endDate"></param>
       ///  <param name="dataType">0：成交数据 1：供应数据</param>
       /// <param name="areaid"></param>
       /// <param name="fxtCompanyId"></param>
       /// <param name="cityId"></param>
       /// <returns></returns>
       IQueryable<LandCaseStatisticDTO> GetStatisticsData(string startDate, string endDate, string dataType, int areaid, int fxtCompanyId, int cityId);

       /// <summary>
       /// 查询案例数据
       /// </summary>
       /// <param name="caseLand"></param>
       /// <param name="pageSize">页面条数</param>
       /// <param name="pageIndex">页面索引</param>
       /// <param name="totalCount">总条数</param>
       /// <param name="self">默认查询自己的</param>
       /// <returns></returns>
       IQueryable<DAT_CaseLand> GetLandCases(DAT_CaseLand caseLand, int pageIndex, int pageSize, out int totalCount, bool self = true);

       /// <summary>
       /// 通过caseid获取案例
       /// </summary>
       /// <param name="caseId"></param>
       /// <param name="fxtcompanyId"></param>
       /// <param name="cityId"></param>
       /// <returns></returns>
       IQueryable<DAT_CaseLand> GetLandCaseByCaseId(int caseId, int fxtcompanyId, int cityId);

       /// <summary>
       /// 新增案例
       /// </summary>
       /// <param name="caseLand"></param>
       /// <returns></returns>
       int AddLandCase(DAT_CaseLand caseLand);

       /// <summary>
       /// 删除案例
       /// </summary>
       /// <param name="caseId"></param>
       /// <returns></returns>
       int DeleteLandCase(int caseId);

       /// <summary>
       /// 修改案例
       /// </summary>
       /// <param name="caseLand"></param>
       /// <returns></returns>
       int UpdateLandCase(DAT_CaseLand caseLand);
       /// <summary>
       /// 获取可访问的评估机构ID
       /// </summary>
       /// <param name="fxtCompanyId"></param>
       /// <param name="cityId"></param>
       /// <returns></returns>
       List<int> GetAccessFxtCompanyId(int fxtCompanyId,int cityId);
    }
}
