using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 商业楼栋接口
    /// </summary>
    public interface IDat_Building_Biz
    {
        /// <summary>
        /// 获取商业楼栋列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="self">true查看自己;false查看全部</param>
        /// <returns></returns>
        IQueryable<Dat_Building_Biz> GetDat_Building_BizList(Dat_Building_Biz model, bool self = true);

        /// <summary>
        /// 获取商业楼栋下拉列表
        /// </summary>
        /// <param name="projectId">商业街ID</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<Dat_Building_Biz> GetBusinessBuildings(long projectId, int cityId, int fxtCompanyId);
        /// <summary>
        /// 获取商业楼栋信息
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        Dat_Building_Biz GetDat_Building_BizById(int buildingId, int CityId, int FxtCompanyId);

        /// <summary>
        /// 获取商业楼栋数
        /// </summary>
        /// <param name="projectId">所属商业街ID</param>
        /// <param name="cityId">所属城市ID</param>
        /// <param name="fxtCompanyId">所属评估机构ID</param>
        /// <returns></returns>
        int GetBusinessBuildingCount(int projectId,int cityId,int fxtCompanyId);
        /// <summary>
        /// 新增商业楼栋
        /// </summary>
        /// <param name="modal">商业楼栋model</param>
        /// <returns></returns>
        int AddDat_Building_Biz(Dat_Building_Biz model);

        /// <summary>
        /// 修改商业楼栋
        /// </summary>
        /// <param name="modal">商业楼栋model</param>
        /// <returns></returns>
        int UpdateDat_Building_Biz(Dat_Building_Biz model, int currFxtCompanyId);

        /// <summary>
        /// 删除商业楼栋
        /// </summary>
        /// <param name="buildingId">楼栋Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="cityId">城市Id</param>
        /// <param name="FxtCompanyId">评估机构Id</param>
        /// <param name="ProductTypeCode">产品Id</param>
        /// <returns></returns>
        bool DeleteDat_Building_Biz(int buildingId, string userId, int cityId, int FxtCompanyId, int ProductTypeCode,int currFxtCompanyId);
        /// <summary>
        /// 检查楼栋名称是否存在
        /// </summary>
        /// <param name="projectId">商业街ID</param>
        /// <param name="buildNameTo">楼栋名称</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        Dat_Building_Biz CheckBuild(int projectId, string buildNameTo, int cityId, int fxtCompanyId);

         /// <summary>
        /// 根据行政区Id获取商圈列表
        /// </summary>
        /// <param name="areaId">行政区Id</param>
        /// <returns></returns>
        IQueryable<SYS_SubArea_Biz> GetSubAreaBizByAreaId(int areaId);
        /// <summary>
        /// 验证楼栋名称是否存在
        /// </summary>
        /// <param name="_cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="_fxtCompanyId"></param>
        /// <param name="buildName"></param>
        /// <returns></returns>
        Dat_Building_Biz IsExistBuildName(int _cityId, int areaId, int _fxtCompanyId, string buildName, int subAreaId, int projectId);
        /// <summary>
        /// 验证宗地号是否唯一
        /// </summary>
        /// <param name="_cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="FieldNo"></param>
        /// <returns></returns>
        bool ValidFieldNo(int _cityId, int areaId, string FieldNo);

        /// <summary>
        /// 根据楼栋名称回去楼栋Id
        /// </summary>
        /// <param name="buildName">楼栋名称</param>
        /// <returns>null:-1</returns>
        long GetBuildIdByName(long projectId,string buildName, int cityId, int fxtCompanyId);

        /// <summary>
        /// 商业楼栋自定义导出
        /// </summary>
        DataTable BuildingSelfDefineExport(Dat_Building_Biz buildingBiz, List<string> buildingAttr, int CityId, int FxtCompanyId, bool self = true);
    }
}
