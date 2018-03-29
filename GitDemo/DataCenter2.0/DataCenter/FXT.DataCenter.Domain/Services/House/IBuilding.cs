using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;
using FXT.DataCenter.Domain.Models.QueryObjects;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 楼栋接口
    /// </summary>
    public interface IDAT_Building
    {
        /// <summary>
        /// 获取楼栋名称集合
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DAT_Building> GetBuildNameList(int cityId, int projectId, int fxtCompanyId);
        /// <summary>
        /// 得到楼栋
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="projectId">楼盘ID</param>
        /// <returns></returns>
        IQueryable<DAT_Building> GetBuildingInfo(int cityId, int projectId, int fxtCompanyId, int buildId = 0);
        /// <summary>
        /// 得到楼栋
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="projectId">楼盘ID</param>
        /// <returns></returns>
        IQueryable<BuildStatiParam> GetBuildingInfo(int cityId, BuildStatiParam parame, int fxtCompanyId);
        /// <summary>
        /// 添加楼栋
        /// </summary>
        /// <param name="item">楼栋对象</param>
        /// <returns></returns>
        int AddBuild(DAT_Building item);
        /// <summary>
        /// 添加楼栋、房号
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="projectidTo">目标楼盘ID</param>
        /// <returns></returns>
        int AddBuilding(int cityId, int projectid, int fxtcompanyid, string saveuser, int projectidTo, IEnumerable<DAT_Building> b_list, int currfxtcompanyid);

        /// <summary>
        /// 修改楼栋
        /// </summary>
        /// <param name="con">数据连接对象</param>
        /// <param name="b_table">buildingTable</param>
        /// <param name="h_table">houseTable</param>
        /// <param name="item">BuildModel</param>
        /// <param name="transaction">事务对象</param>
        int ModifyBuilding(DAT_Building item, int currfxtcompanyid);

        /// <summary>
        /// 修改楼栋
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currFxtCompanyId"></param>
        /// <param name="modifiedProperty">需要修改的属性</param>
        /// <returns></returns>
        int UpdateBuilding4Excel(DAT_Building item, int currFxtCompanyId, List<string> modifiedProperty);
        /// <summary>
        /// 删除楼栋
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="UserId"></param>
        /// <param name="ProductTypeCode"></param>
        /// <returns></returns>
        int DeleteBuilding(int buildId, int CityId, int FxtCompanyId, string UserId, int ProductTypeCode, int currFxtCompanyId, int isDeleteTrue);
        /// <summary>
        /// 拆分楼盘下面的楼栋、房号
        /// </summary>
        /// <param name="projectid">原始楼盘ID</param>
        /// <param name="projectidTo">目标楼盘ID</param>
        /// <param name="cityid">城市ID</param>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="con">连接obj</param>
        /// <param name="transaction">事务obj</param>
        /// <param name="saveuser">操作人</param>
        /// <param name="build_list">原始楼栋集合</param>
        /// <returns></returns>
        int SplitBuild(int projectid, int projectidTo, int cityid, int fxtcompanyid, string saveuser, string build_list, int currfxtcompanyid);

        /// <summary>
        /// 更新楼栋ID的状态
        /// 楼栋剪切
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="build_list"></param>
        /// <returns></returns>
        //int UpdateBuildId(int projectId, int cityId, int fxtcompanyid, string build_list);

        /// <summary>
        /// 楼盘合并
        /// </summary>
        /// <param name="project">原始楼盘obj</param>
        /// <param name="projectIdTo">目标楼盘ID</param>
        /// <returns></returns>
        int MergerProject(DAT_Project project, int projectIdTo, int currfxtcompanyid);
        /// <summary>
        /// 楼栋复制
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildName"></param>
        /// <returns></returns>
        string BuildCopy(int CityId, int FxtCompanyId, int projectId, int buildId, string buildName, string buildNameTo, string userId);

        /// <summary>
        /// 获取楼栋ID
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="buildingName"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        int GetBuildingId(int projectId, string buildingName, int cityId, int fxtCompanyId);



        /// <summary>
        /// 获取楼栋Info
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="buildingName"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        DAT_Building GetBuildingInfo(int projectId, string buildingName, int cityId, int fxtCompanyId);
        /// <summary>
        /// 获取楼栋数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        int GetBuildCount(int cityId, int fxtcompanyId, int projectId);

        /// <summary>
        /// 获取房号数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        int GetHouseCount(int cityId, int fxtcompanyId, int buildingId);
        /// <summary>
        /// 根据projectId获取楼盘名称
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DAT_Project GetProjectNameById(string projectId, int cityId, int fxtcompanyId);

        //根据楼盘ID获取楼盘对象
        //DAT_Project GetSingleProjectByProjectId(int projectId, int cityId, int fxtcompanyId);

        /// <summary>
        /// 更新房号价格系数说明
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="priceDetail"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <returns></returns>
        int EndtityPriceDetail(int buildId, string priceDetail, int cityId, int fxtcompanyId, int currfxtcompanyId, string userName);

        /// <summary>
        /// 批量设置是否可估
        /// </summary>
        /// <returns></returns>
        int BatchSetEvalue(DAT_Building building);

        /// <summary>
        /// 根据楼盘ID获取单个楼盘对象
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <returns></returns>
        //DAT_Project GetSingleProjectById(string projectId, int cityId, int fxtcompanyId);

        /// <summary>
        /// 新增楼盘检索表数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="buildingId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingName"></param>
        /// <param name="otherName"></param>
        /// <param name="doorplate"></param>
        /// <returns></returns>
        //int AddBuildingSearch(string tablename, int cityid, int fxtcompanyid, int buildingId, int projectId, string buildingName, string otherName, string doorplate);

        /// <summary>
        /// 新增楼盘检索子表数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="buildingId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingName"></param>
        /// <param name="otherName"></param>
        /// <param name="doorplate"></param>
        /// <returns></returns>
        //int AddBuildingSubSearch(string tablename, int cityid, int fxtcompanyid, int buildingId, int projectId, string buildingName, string otherName, string doorplate);


        /// <summary>
        /// 获取指定评估机构指定城市的所有楼栋
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DAT_Building> GetBuildingIds(int cityId, int fxtCompanyId);

        /// <summary>
        /// 复制楼栋图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="currfxtCompanyId"></param>
        /// <param name="buildingIdFrom"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        int AddBuildingPhoto(int projectId, int buildingId, int currfxtCompanyId, int buildingIdFrom, int fxtCompanyId, int cityId);
    }
}
