using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;

namespace FXT.DataCenter.Domain.Services
{
    public interface IOfficeSubArea
    {
        /// <summary>
        /// 根据行政区ID获取其所有片区名称
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        IQueryable<SYS_SubArea_Office> GetSubAreaNamesByAreaId(int areaId, int fxtCompanyId, int cityId);

        /// <summary>
        /// 根据片区名称获取片区ID
        /// </summary>
        int GetSubAreaIdByName(string name, int areaId, int fxtCompanyId);

        /// <summary>
        /// 获取片区列表
        /// </summary>
        IQueryable<SYS_SubArea_Office> GetSubAreas(SYS_SubArea_Office subAreaOffice, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 根据subAreaId获取subArea
        /// </summary>
        /// <param name="subAreaId"></param>
        /// <returns></returns>
        SYS_SubArea_Office GetSubAreaById(int subAreaId);

        /// <summary>
        /// 修改办公商务中心信息
        /// </summary>
        /// <param name="subAreaOffice"></param>
        /// <returns></returns>
        int UpdateSubArea(SYS_SubArea_Office subAreaOffice);

        /// <summary>
        /// 新增办公商务中心信息
        /// </summary>
        int AddSubArea(SYS_SubArea_Office subAreaOffice);

        /// <summary>
        /// 判断该商务中心能否删除，是否有办公楼盘属于这个商务中心。
        /// </summary>
        int CanDelete(int subAreaId, int companyId);

        /// <summary>
        /// 删除办公商务中心信息
        /// </summary>
        int DeleteSubArea(SYS_SubArea_Office subAreaOffices, int FxtCompanyId);

        /// <summary>
        /// 商务中心统计
        /// </summary>
        IQueryable<SubAreaOfficeStatisticDTO> GetSubAreaOfficeStatistic(int areaId, int fxtCompanyId, int cityId, bool self);

        /// <summary>
        /// 验证商务中心是否存在
        /// </summary>
        bool IsExistSubAreaOffice(int areaId, int subAreaId, string subAreaName, int fxtCompanyId);

        #region 商务中心范围
        /// <summary>
        /// 查询商务中心范围，返回记录条数
        /// </summary>
        /// <returns>记录条数</returns>
        int GetSubAreaOfficeCoordinate(int subAreaId, int areaId, int fxtCompanyId);

        /// <summary>
        /// 插入
        /// </summary>
        /// <returns></returns>
        int AddSubAreaOfficeCoordinate(SYS_SubArea_Office_Coordinate subAreaOfficeCoordinate);

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        int UpdateSubAreaOfficeCoordinate(int subAreaId, int areaId, int fxtCompanyId);

        #endregion
    }
}
