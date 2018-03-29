using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;


namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustrySubArea
    {
        /// <summary>
        /// 获取片区列表
        /// </summary>
        IQueryable<SYS_SubArea_Industry> GetSubAreas(SYS_SubArea_Industry subArea, int pageIndex, int pageSize, out int totalCount);
        /// <summary>
        /// 根据SubAreaId获取SubArea
        /// </summary>
        /// <param name="subAreaId"></param>
        /// <returns></returns>
        SYS_SubArea_Industry GetSubAreaById(int subAreaId);
        /// <summary>
        /// 验证片区是否存在
        /// </summary>
        bool IsExistSubAreaIndustry(int areaId, int subAreaId, string subAreaName, int fxtCompanyId);
        /// <summary>
        /// 修改片区
        /// </summary>
        int UpdateSubArea(SYS_SubArea_Industry subArea);
        /// <summary>
        /// 新增片区
        /// </summary>
        int AddSubArea(SYS_SubArea_Industry subArea);
        /// <summary>
        /// 判断该片区是否可以删除
        /// </summary>
        int CanDelete(int subAreaId, int companyId);
        /// <summary>
        /// 删除片区
        /// </summary>
        int DeleteSubArea(SYS_SubArea_Industry subArea, int FxtCompanyId);
        /// <summary>
        /// 根据AreaId获取片区列表
        /// </summary>
        IQueryable<SYS_SubArea_Industry> GetSubAreaNamesByAreaId(int areaId, int fxtCompanyId, int cityId);

        /// <summary>
        /// 查询商务中心范围，返回记录条数
        /// </summary>
        /// <returns>记录条数</returns>
        int GetSubAreaIndustryCoordinate(int subAreaId, int areaId, int fxtCompanyId);

        /// <summary>
        /// 插入
        /// </summary>
        int AddSubAreaIndustryCoordinate(SYS_SubArea_Industry_Coordinate subAreaIndustryCoordinate);

        /// <summary>
        /// 修改
        /// </summary>
        int UpdateSubAreaIndustryCoordinate(int subAreaId, int areaId, int fxtCompanyId);

        /// <summary>
        /// 统计
        /// </summary>
        IQueryable<SubAreaIndustryStatisticDTO> GetSubAreaIndustryStatistic(int areaId, int fxtCompanyId, int cityId, bool self);

        /// <summary>
        /// 通过名称获取到subareaid
        /// </summary>
        int GetSubAreaIdByName(string name, int areaId, int fxtCompanyId);
    }
}
