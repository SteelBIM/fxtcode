using System.Linq;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Application.Interfaces.VQPrice
{
    public interface IProjectWeightRevisedServices
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
        /// 新建楼盘修正系数
        /// </summary>
        /// <returns></returns>
        int AddWeightProject(DatWeightProject datWeightProject);

        /// <summary>
        /// 获取楼盘名称列表
        /// </summary>
        /// <returns></returns>
        IQueryable<DAT_Project> GetProjectNameList(int fxtCompanyId, int cityId);

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IQueryable<DAT_Company> GetCompanyNameList(int cityId);

        /// <summary>
        /// 新增公司
        /// </summary>
        /// <param name="datCompany"></param>
        /// <returns></returns>
        int AddCompany(DAT_Company datCompany);

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        IQueryable<DAT_ImportTask> GetTask(int importType, int cityid, int fxtcompanyid);

        /// <summary>
        /// 删除任务列表
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        int DeleteTask(long taskId);

        #region 非业务接口

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

        #endregion
    }
}
