using System.Collections.Generic;
using System.Linq;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IProjectSample
    {
        /// <summary>
        /// 根据ID获取样本楼盘
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <returns></returns>
        IQueryable<DAT_SampleProject> GetProjectSample(int id, int cityid, int fxtcompanyid);

        /// <summary>
        /// 获取样本楼盘
        /// </summary>
        /// <param name="sp">参数对象</param>
        /// <param name="pageInde">当前索引页</param>
        /// <param name="pageSize">页面条数</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DAT_SampleProject> GetProjectSample(DAT_SampleProject sp);
        /// <summary>
        /// 添加样本楼盘
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        int AddProjectSample(DAT_SampleProject sp);

        /// <summary>
        /// 修改样本楼盘
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        int UpdateProjectSample(DAT_SampleProject sp);

        /// <summary>
        /// 删除样本楼盘
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        int DeleteProjectSample(int id);

        /// <summary>
        /// 根据ID获取样本楼盘的关联楼盘
        /// </summary>
        /// <param name="sampleProjecId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DAT_SampleProject_Weight> GetProjectSampleWeightById(int sampleProjecId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 根据ID集合获取样本楼盘的关联楼盘
        /// </summary>
        /// <param name="sampleProjecIds"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DAT_SampleProject_Weight> GetProjectSampleWeights(IQueryable<int> sampleProjecIds, int cityId, int fxtCompanyId);

        /// <summary>
        /// 根据关联楼盘ID获取楼盘信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DAT_SampleProject_Weight> GetProjectSampleWeight(int projectId, int cityId, int fxtCompanyId, int buildingTypeCode);


        /// <summary>
        /// 添加关联楼盘
        /// </summary>
        /// <param name="spw"></param>
        /// <returns></returns>
        int AddProjectSampleWeight(DAT_SampleProject_Weight spw);
        /// <summary>
        /// 修改关联楼盘
        /// </summary>
        /// <param name="spw"></param>
        /// <returns></returns>
        int UpdateProjectSampleWeight(DAT_SampleProject_Weight spw);
        /// <summary>
        /// 删除关联楼盘
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteProjectSampleWeight(int id);

        /// <summary>
        /// 根据楼盘名称获取楼盘ID
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="name"></param>
        /// <param name="cityid"></param>  
        /// <param name="fxtcompanyid"></param>
        /// <returns></returns>
        int GetProjectIdByName(int areaId, string name, int cityid, int fxtcompanyid);

        /// <summary>
        /// 获取楼盘
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <returns></returns>
        IQueryable<DAT_Project> GetProjects(int cityid, int fxtcompanyid);

        /// <summary>
        /// 检测样本楼盘是否存在
        /// </summary>
        /// <param name="projectId">样本楼盘ID</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        int SampleProjectIsExit(int projectId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 根据projectid获取楼盘数据，将信息自动带出来
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        DAT_Project GetProjectInfo(int projectId, int cityId, int fxtCompanyId);
    }
}
