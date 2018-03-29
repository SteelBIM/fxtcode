using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IOfficeBuilding
    {
        #region 办公楼栋

        /// <summary>
        /// 获取办公楼栋数据
        /// </summary>
        /// <param name="datBuildingOffice">传参模型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="self">是否为查看自己，为true时，查看自己，false时，查看全部</param>
        /// <returns>办公楼栋数据列表</returns>
        IQueryable<DatBuildingOffice> GetOfficeBuildings(DatBuildingOffice datBuildingOffice, bool self, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 获取办公楼盘
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DatBuildingOffice> GetOfficeBuildings(long projectId, int fxtCompanyId);

        /// <summary>
        /// 获取办公楼栋下拉列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DatBuildingOffice> GetOfficeBuildings(int cityId, int fxtCompanyId, int areaId, int subAreaId, int projectId = -1, int buildingId = -1, bool self = true);

        /// <summary>
        /// 获取单条办公楼栋数据
        /// </summary>
        /// <param name="buildingId">楼栋ID</param>
        /// <param name="fxtCompanyId"></param>
        /// <returns>办公楼栋数据</returns>
        DatBuildingOffice GetOfficeBuilding(int buildingId, int fxtCompanyId);

        /// <summary>
        /// 获取办公楼栋Id
        /// </summary>
        /// <param name="projectId">办公楼盘ID</param>
        /// <param name="buildingId">办公楼栋ID(没有则传buildingId=0)</param>
        /// <param name="buildingName">楼栋名称</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="companyId">评估机构ID</param>
        /// <returns></returns>
        long GetOfficeBuildingId(long projectId, long buildingId, string buildingName, int cityId, int companyId);

        /// <summary>
        /// 复制楼栋
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="buildingName">原楼栋名称</param>
        /// <param name="destBuildingName">目标楼栋名称</param>
        /// <param name="buildingId"></param>
        /// <param name="projectId"></param>
        /// <returns>-1：目标楼栋下已存在房号；-2：楼栋复制失败；大于0：新增的房号数</returns>
        int CopyBuilding(int cityId, int companyId, string buildingName, string destBuildingName, int buildingId,
            int projectId);

        /// <summary>
        /// 新增办公楼栋数据
        /// </summary>
        /// <param name="datBuildingOffice">传参模型</param>
        /// <returns>返回成功条数</returns>
        int AddOfficeBuilding(DatBuildingOffice datBuildingOffice);

        /// <summary>
        /// 修改办公楼栋
        /// </summary>
        /// <param name="datBuildingOffice">传参模型</param>
        /// <param name="currentCompanyId">当前操作者所在的公司ID</param>
        /// <returns>返回成功条数</returns>
        int UpdateOfficeBuilding(DatBuildingOffice datBuildingOffice, int currentCompanyId);

        /// <summary>
        /// 删除办公楼栋
        /// </summary>
        /// <param name="datBuildingOffice">传参模型</param>
        /// <param name="currentCompanyId">当前操作者所在的公司ID</param>
        /// <returns>返回成功条数</returns>
        int DeleteOfficeBuilding(DatBuildingOffice datBuildingOffice, int currentCompanyId);

        #endregion

        #region 办公楼栋图片

        /// <summary>
        /// 获取办公楼栋图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="self">是否为查询自己, true：查询自己</param>
        /// <returns></returns>
        IQueryable<LnkBPhoto> GetOfficeBuildingPhotoes(LnkBPhoto lnkPPhoto, bool self = true);

        /// <summary>
        /// 获取办公楼栋图片单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LnkBPhoto GetOfficeBuildingPhoto(int id, int fxtCompanyId);

        /// <summary>
        /// 添加办公楼栋图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <returns></returns>
        int AddOfficeBuildingPhoto(LnkBPhoto lnkPPhoto);

        /// <summary>
        /// 更新办公楼栋图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int UpdateOfficeBuildingPhoto(LnkBPhoto lnkPPhoto, int currentCompanyId);

        /// <summary>
        /// 删除办公楼栋图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int DeleteOfficeBuildingPhoto(LnkBPhoto lnkPPhoto, int currentCompanyId);

        #endregion

        int GetHouseCounts(int buildingId, int cityId, int fxtCompanyId, bool self);

        /// <summary>
        /// 办公楼栋自定义导出
        /// </summary>
        DataTable BuildingSelfDefineExport(DatBuildingOffice buildingOffice, List<string> buildingAttr, int CityId, int FxtCompanyId, bool self = true);
    }
}
