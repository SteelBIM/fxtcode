using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.QueryObjects;
using FXT.DataCenter.Domain.Models.DTO;
using System.Data;
using FXT.DataCenter.Domain.Models.FxtProject;
using FXT.DataCenter.Domain.Models.QueryObjects.House;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 楼盘接口IDAT_Project
    /// </summary>
    public interface IDAT_Project
    {
        // 读取楼盘信息
        IQueryable<DAT_Project> GetProjectInfo(ProjectQueryParams project);

        // 新增楼盘
        int AddProject(DAT_Project project);

        /// <summary>
        /// 复制楼盘
        /// </summary>
        /// <param name="project">楼盘对象</param>
        /// <returns>
        /// 0:没有开通权限
        /// -1:原始楼盘暂无可以复制的楼栋
        /// -2:添加楼栋失败
        /// 3:目标楼盘存在数据，不能复制楼盘;
        ///  -4://程序异常
        /// </returns>
        string CopyProject(DAT_Project project, int ProductTypeCode, int currfxtcompanyid, int isDeleteTrue);

        // 修改楼盘
        int ModifyProject(DAT_Project project, int currFxtCompanyId);

        // 修改楼盘信息
        int UpdateProjectInfo4Excel(DAT_Project project, int currFxtCompanyId, List<string> modifiedProperty);

        // 得到楼盘的配套
        IQueryable<LNK_P_Appendage> GetProjectAppendageById(int ProjectId, int CityId, int pid = 0);

        // 新增项目配套
        int AddProjectAppendage(LNK_P_Appendage obj);

        // 修改项目配套
        int ModifyProjectAppendage(LNK_P_Appendage obj);

        // 得到某个楼盘的附属房屋单价 附属房价格
        IQueryable<DAT_SubHousePrice> GetSubHousePriceByProjectId(int CityId, int ProjectId, int FxtCompanyId, string codename);

        // 修改附属房屋价格
        int SaveSubHousePrice(int CityId, string ProjectId, string FxtCompanyId, string SId, string SubHouseUnitPrice, string SubHouseType, string UserId);

        // 得到楼盘的图片列表
        IQueryable<LNK_P_Photo> GetProjectPhotoList(int cityId, int fxtId, int projectId, int photoId = 0);

        // 得到楼栋的图片列表
        IQueryable<LNK_P_Photo> GetBuildingPhotoList(int cityId, int fxtId, int projectId, int buildId, int photoId = 0);

        // 删除楼盘
        int DeleteProject(int projectId, int cityId, int fxtCompanyId, string userId, int productTypeCode, int currFxtCompanyId, int isDeleteTrue);

        // 删除楼盘配套
        int DeleteProjectPeiTao(int id);

        //  删除楼盘项目图片
        int DeleteProjectPhoto(int photoId, int cityId, int fxtId, int currFxtCompanyId);

        // 增加楼盘项目图片
        int AddProjectPhoto(int cityId, int fxtId, int projectId, int photoTypeCode, string path, string photoName, long buildId = 0);

        // 修改楼盘项目图片
        int UpdataProjectPhoto(int Id, int photoTypeCode, string path, string photoName, int CityId, int FxtCompanyId, int currFxtCompanyId);

        // 获取所有的楼盘名称
        IQueryable<DAT_Project> GetAllProjectName(int cityId, int fxtId, string projectName);

        // 获取所有的楼盘名称
        IQueryable<DAT_Project> GetAllProjectName(int cityId, int fxtId);

        // 根据楼盘名称获取楼盘ID
        IQueryable<DAT_Project> GetProjectIdByName(int cityId, int areaid, int fxtId, string projectName);

        // 楼盘拆分  0:没有开通权限  -1:原始楼盘暂无可以复制的楼栋  -2:添加楼栋失败  3:目标楼盘存在数据，不能复制楼盘   -4:程序异常
        string SplitProject(DAT_Project project, string buidIdList, string build_Name, int currfxtcompanyid);

        // 楼盘合并  0:没有开通权限  -1:原始楼盘暂无可以复制的楼栋  -2:添加楼栋失败  3:目标楼盘存在数据，不能复制楼盘   -4:程序异常
        int MergerProject(DAT_Project project, int projectidTo, int currfxtcompanyid);

        // 楼盘合并 // 删除
        int MergerProjectDel(int projectidTo, int CityId, int FxtCompanyId, string UserName, int ProductTypeCode, int currFxtCompanyId, int isDeleteTrue);

        // 楼盘基础信息统计
        DataTable ProjectStatistcs(List<string> project, ProStatiParam param, int CityId, int FxtCompanyId, bool self);

        // 楼盘基础信息---楼栋统计
        DataTable BuildStatistcs(List<string> attr, BuildStatiParam build, int CityId, int FxtCompanyId, bool self);

        // 楼盘基础信息---房号统计
        DataTable HouseStatistcs(List<string> attr, HouseStatiParam house, int cityId, int FxtCompanyId, bool self);

        // 增加与楼盘相关的开发公司，物管公司
        int AddLnkPCompany(LNK_P_Company lnkPCompany);

        // 根据楼盘名称获取楼盘Info
        IQueryable<DAT_Project> GetProjectNameList(int CityID, string ProjectName, int fxtCompanyId, int areaId = 0);

        IQueryable<DAT_Project> GetProjectInfoList(ProjectQueryParams project);

        IQueryable<DAT_Project> ExportProjectInfoList(ProjectQueryParams project);

        bool ValidFieldNo(int CityId, int FxtCompanyId, string fieldNo);

        // 一键导出
        //IQueryable<DAT_Project> ExportGetProjectInfo(ProjectQueryParams project, bool self);

        // 判断楼盘是否存在
        int IsProjectExist(int cityId, int areaId, int fxtCompanyId, string projectName);

        // 获取单个楼盘对象
        DAT_Project GetSingleProject(int projectId, int cityId, int fxtCompanyId);

        // 新增或修改楼盘时，判断楼盘名字是否存在
        int IsExistsProjectOnEdit(int cityId, int fxtCompanyId, int areaId, int projectId, string projectName);

        /// <summary>
        /// 新增楼盘检索数据
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="projectId"></param>
        /// <param name="projectName"></param>
        /// <param name="address"></param>
        /// <param name="otherName"></param>
        /// <param name="pinYin"></param>
        /// <param name="pinYinAll"></param>
        /// <returns></returns>
        //int AddProjectSearch(string tablename, int cityid, int fxtcompanyid, int projectId, string projectName, string address, string otherName, string pinYin, string pinYinAll);

        /// <summary>
        /// 新增楼盘子表检索数据
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="projectId"></param>
        /// <param name="projectName"></param>
        /// <param name="address"></param>
        /// <param name="otherName"></param>
        /// <param name="pinYin"></param>
        /// <param name="pinYinAll"></param>
        /// <returns></returns>
        //int AddProjectSubSearch(string tablename, int cityid, int fxtcompanyid, int projectId, string projectName, string address, string otherName, string pinYin, string pinYinAll);

        // 获取指定评估机构在指定城市的所有楼盘Id
        IQueryable<DAT_Project> GetProjectIds(int cityId, int fxtCompanyId);

        //楼盘房号统计
        List<Project_BHCount> GetBHCount(List<int> bhareaname, int CityId, int FxtCompanyId);

        //楼盘图片统计
        List<Project_PPCount> GetPPCount(List<int> ppareaname, string ProjectName, int CityId, int FxtCompanyId);

        // 楼盘合并成功后，复制案例
        int MergerProjectCase(int project, int projectidTo, int cityID, int fxtcompanyid, string userName);

        // 楼盘合并成功后，复制楼盘图片
        int MergerProjectPhoto(int projectId, int projectidTo, int fxtcompanyid, int cityId, int currFxtCompanyId);
    }
}
