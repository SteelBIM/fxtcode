using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IDynamicPriceSurvey
    {
        /// <summary>
        /// 获取动态价格调查全部数据
        /// </summary>
        /// <param name="dynamicPriceSurvey">传参对象</param>
        /// <param name="totalCount"></param>
        /// <param name="self">是否为查询自己，默认是</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Dat_P_B_Price_Biz> GetDynamicPriceSurveys(Dat_P_B_Price_Biz dynamicPriceSurvey, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 获取动态价格调查单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Dat_P_B_Price_Biz GetDynamicPriceSurveyById(int id);

         /// <summary>
         /// 获取动态价格调查对象ID
         /// </summary>
         /// <param name="projectId">商业街ID</param>
         /// <param name="buildingId">商业楼栋ID</param>
         /// <param name="cityId">城市ID</param>
         /// <param name="fxtCompanyId">评估机构ID</param>
         /// <returns></returns>
        int GetDynamicPriceSurveyId(long projectId,long buildingId,int cityId,int fxtCompanyId);

        /// <summary>
        /// 增加动态价格调查
        /// </summary>
        /// <param name="dynamicPriceSurvey">传参对象</param>
        /// <returns></returns>
        int AddDynamicPriceSurvey(Dat_P_B_Price_Biz dynamicPriceSurvey);

        /// <summary>
        /// 修改动态价格调查
        /// </summary>
        /// <param name="dynamicPriceSurvey">传参对象</param>
        /// <returns></returns>
        int UpdateDynamicPriceSurvey(Dat_P_B_Price_Biz dynamicPriceSurvey);

        /// <summary>
        /// 删除动态价格调查
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteDynamicPriceSurvey(int id);

    }
}
