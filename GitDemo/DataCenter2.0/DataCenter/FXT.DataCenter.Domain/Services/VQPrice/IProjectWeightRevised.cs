using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IProjectWeightRevised
    {
        /// <summary>
        /// 获取楼盘修正系数
        /// </summary>
        /// <param name="datWeightProject"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DatWeightProject> GetWeightProjects(string projectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 获取单个楼盘修正系数对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        DatWeightProject GetWeightProject(int projectId, int cityId, int parentFxtCompanyId, int parentProductTypeCode);

        /// <summary>
        /// 获取带更新均价的楼盘
        /// </summary>
        /// <param name="datWeightProject"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DatWeightProject> GetNotUpdatedAvrPriceProjects(string projectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, bool self = true);

        /// <summary>
        /// 根据楼盘ID获取楼盘名称
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        //string GetProjectName(int projectId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 修改楼盘修正系数
        /// </summary>
        /// <param name="datWeightProject"></param>
        /// <returns></returns>
        int UpdateWeightProject(DatWeightProject datWeightProject);

        /// <summary>
        /// 根据算法更新楼盘均价
        /// </summary>
        /// <param name="datWeightProject"></param>
        //int UpdateWeightProjectAvgPrice(DatWeightProject datWeightProject);

        /// <summary>
        /// 增加楼盘修正系数
        /// </summary>
        /// <param name="datWeightProject"></param>
        /// <returns></returns>
        int AddWeightProject(DatWeightProject datWeightProject);

        /// <summary>
        /// 删除楼盘修正系数
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        //int DeleteWeightProject(int projectId, int cityId, int fxtCompanyId);
    }
}
