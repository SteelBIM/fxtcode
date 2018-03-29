using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Domain.Models.QueryObjects.House;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IProjectCase
    {
        /// <summary>
        /// 住宅案例（案例均价统计）
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable GetProjectCaseAvePrice(ProjectCase_AvgPrice parameters);
        /// <summary>
        /// 住宅案例（楼盘案例统计）
        /// </summary>
        /// <returns></returns>
        IQueryable<ProjectCase_Statist> GetProjectCaseCount(string casedateFrom, string casedateTo, int caseTypeCode, string condition, int count, int cityid, int fxtcompanyid);

        /// <summary>
        /// 住宅案例（工作量统计）
        /// </summary>
        /// <returns></returns>
        IQueryable<ProjectCase_WorkLoad> GetProjectCaseCount(string datefrom, string dateto, int cityid, int fxtcompanyid);

        /// <summary>  
        /// 根据案例ID 获取楼盘案例
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IQueryable<DAT_Case> GetProjectCaseById(int caseId, int fxtCompanyId, int cityId);

        /// <summary>
        /// 获取楼盘案例
        /// </summary>
        /// <param name="pcp">参数对象</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="self">true为查询自己，false查询全部</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">页面条数</param>
        /// <returns></returns>
        IQueryable<DAT_Case> GetProjectCase(ProjectCaseParams pcp);

        /// <summary>
        /// 获取楼盘列表
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IQueryable<DAT_Project> GetProjectList(int fxtCompanyId, int cityId);

        /// <summary>
        /// 获取楼盘列表网络名
        /// </summary>
        /// <returns></returns>
        //IQueryable<SYS_ProjectMatch> GetProjectNetName();
        /// <summary>
        /// 获取楼栋列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DAT_Building> GetBuildingList(int cityId, int fxtCompanyId);

        /// <summary>
        /// 新增楼盘案例
        /// </summary>
        /// <param name="projectCase"></param>
        /// <returns></returns>
        int AddProjectCase(DAT_Case projectCase);

        /// <summary>
        /// 新增楼盘案例
        /// </summary>
        /// <returns></returns>
        int AddProjectCase(DAT_CaseTemp[] caseTemp);

        /// <summary>
        /// 修改楼盘案例 
        /// </summary>
        /// <param name="projectCase"></param>
        /// <returns></returns>
        int UpdateProjectCase(DAT_Case projectCase);

        /// <summary>
        ///  删除案例
        /// </summary>
        /// <returns></returns>
        int DeleteProjectCase(int caseId, int cityId, int fxtCompanyId, string saveusername);

        /// <summary>
        /// 删除重复案例
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="caseDateFrom"></param>
        /// <param name="caseDateTo"></param>
        /// <returns></returns>
        int DeleteSameProjectCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser);

        /// <summary>
        /// 获取删除重复案例总数
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="caseDateFrom"></param>
        /// <param name="caseDateTo"></param>
        /// <returns></returns>
        int DeleteSameProjectCaseCount(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo);

        /// <summary>
        /// 删除异常案例
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        int DeleteExProjectCase(ExceptionCaseParams ec);

        /// <summary>
        /// 获取不匹配楼盘案例
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        DataTable GetMisMatchProjectCase(int taskId);

        /// <summary>
        /// 获取删除案例条数DeleteCase
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="createtimefrom"></param>
        /// <param name="createtimeto"></param>
        /// <returns></returns>
        //int DeleteCaseCount(int fxtCompanyId, int cityId, string createtimefrom, string createtimeto, string creator);

        /// <summary>
        /// 住宅案例（楼盘可估统计）
        /// </summary>
        /// <returns></returns>
        IQueryable<ProjectCase_ProjectEValue> GetProjectEValueCount(string datefrom, string dateto, List<int> peareaname, string ProjectEValueProjectName, int cityid, int fxtcompanyid, string projectUEReason);

        /// <summary>
        /// 住宅案例（楼栋可估统计）
        /// </summary>
        /// <returns></returns>
        IQueryable<ProjectCase_BuildingEValue> GetBuildingEValueCount(string datefrom, string dateto, List<int> peareaname, string BuildingEValueProjectName, int cityid, int fxtcompanyid, string buildingUEReason);

        //一键删除，只能删除本评估机构案例数据
        int GetProjectCaseAll(int CityId, int FxtCompanyId, string SaveUser, int areaid, DateTime casedateStart, DateTime casedateEnd, int caseTypeCode, decimal? buildingAreaFrom, decimal? buildingAreaTo, int purposeCode, decimal? unitPriceFrom, decimal? unitPriceTo, string key, int buildingTypeCode, bool self);

        //一键删除，只能删除本评估机构案例数据
        int DeleteProjectCaseAll(int CityId, int FxtCompanyId, string SaveUser, int areaid, DateTime casedateStart, DateTime casedateEnd, int caseTypeCode, decimal? buildingAreaFrom, decimal? buildingAreaTo, int purposeCode, decimal? unitPriceFrom, decimal? unitPriceTo, string key, int buildingTypeCode, bool self);

        //删除不匹配楼盘
        int DeleteMisMatchProjectCase(int taskid, string areaName, string projectName);

        //删除所有不匹配楼盘
        int DeleteAllMisMatchProjectCase(int taskid);
    }
}
