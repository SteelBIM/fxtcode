using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IOfficeProject
    {
        /// <summary>
        /// 查询办公楼盘
        /// </summary>
        /// <param name="projectOffice"></param>
        /// <param name="totalCount"></param>
        /// <param name="self">是否是自己</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<DatProjectOffice> GetProjectOffices(DatProjectOffice projectOffice, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DatProjectOffice> GetOfficeProjects(int cityId, int fxtCompanyId, int areaId = -1, int subAreaId = -1, int projectId = -1, bool self = true);

        /// <summary>
        /// 根据Id查询办公楼盘
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        DatProjectOffice GetProjectNameById(long id, int fxtCompanyId);

        /// <summary>
        /// 验证办公楼盘是否存在（存在，返回办公楼盘ID，不存在，则返回0）
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <param name="projectId">办公楼盘ID(新增状态时：projectId= -1)</param>
        /// <param name="projectName">办公楼盘名称</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        bool IsExistProjectOffice(int areaId, long projectId, string projectName, int cityId, int fxtCompanyId);

        /// <summary>
        /// 修改办公楼盘信息
        /// </summary>
        /// <param name="projectOffice"></param>
        /// <param name="currentCompanyId">当前登录的评估机构公司ID</param>
        /// <returns></returns>
        int UpdateProjectOffice(DatProjectOffice projectOffice, int currentCompanyId);

        /// <summary>
        /// 新增办公楼盘
        /// </summary>
        /// <param name="projectOffice"></param>
        /// <returns></returns>
        int AddProjectOffice(DatProjectOffice projectOffice);

        /// <summary>
        /// 删除办公楼盘
        /// </summary>
        /// <param name="projectOffice"></param>
        /// <param name="currentCompanyId">当前登录的评估机构公司ID</param>
        /// <returns></returns>
        int DeleteProjectOffice(DatProjectOffice projectOffice, int currentCompanyId);

        /// <summary>
        /// 获取办公楼盘图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="self">是否为查询自己, true：查询自己</param>
        /// <returns></returns>
        IQueryable<LnkPPhoto> GetOfficeProjectPhotoes(LnkPPhoto lnkPPhoto, bool self = true);

        /// <summary>
        /// 获取办公楼盘图片单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LnkPPhoto GetOfficeProjectPhoto(int id, int fxtCompanyId);

        /// <summary>
        /// 添加办公楼盘图片
        /// </summary>
        /// <param name="lnkPPhoto"></param>
        /// <returns></returns>
        int AddOfficeProjectPhoto(LnkPPhoto lnkPPhoto);

        /// <summary>
        /// 更新办公楼盘图片
        /// </summary>
        /// <param name="lnkPPhoto"></param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int UpdateOfficeProjectPhoto(LnkPPhoto lnkPPhoto, int currentCompanyId);

        /// <summary>
        /// 删除办公楼盘图片
        /// </summary>
        /// <param name="lnkPPhoto"></param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int DeleteOfficeProjectPhoto(LnkPPhoto lnkPPhoto, int currentCompanyId);


        /// <summary>
        /// 获取办公商务配套Id
        /// </summary>
        /// <param name="projectName">办公楼盘</param>
        /// <param name="areaId">行政区ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="companyId">评估机构ID</param>
        /// <returns></returns>
        IQueryable<long> GetProjectIdByName(string projectName, int areaId, int cityId, int companyId);

        /// <summary>
        /// 统计办公楼盘下的楼栋总数
        /// </summary>
        int GetBuildingCounts(int subAreaId, int cityId, int fxtCompanyId, bool self);

        /// <summary>
        /// 统计商务中心下的楼盘总数
        /// </summary>
        int GetProjectCounts(int subAreaId, int cityId, int fxtCompanyId, bool self);

        ///// <summary>
        ///// 根据projectId获取楼盘名称
        ///// </summary>
        //string GetProjectNameByProjectId(long projectId, int cityId, int fxtCompanyId);

        ///// <summary>
        ///// 根据projectId获取FxtCompanyId
        ///// </summary>
        //int GetProjectFxtCompanyIdByProjectId(long projectId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 自定义导出
        /// </summary>
        DataTable ProjectSelfDefineExport(DatProjectOffice projectOffice, List<string> projectAttr, int CityId, int FxtCompanyId, bool self = true);

    }
}
