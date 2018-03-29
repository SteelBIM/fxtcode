using CAS.Entity.DBEntity;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IProjectService
    {
        /// <summary>
        /// 获取楼盘详情
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <param name="allotid"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        ProjectDto GetProjectDetals(int projectid, int cityid, long allotid,int fxtCompanyId);

        /// <summary>
        /// 上传任务
        /// </summary>
        /// <param name="allotid"></param>
        /// <param name="username"></param>
        /// <param name="userTrueName"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="cityid"></param>
        /// <param name="project"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        int SetAllotProjectInfo(int allotid, string username, string userTrueName, int FxtCompanyId, int cityid, ProjectApiDto project, int isValid);

        /// <summary>
        /// 导入数据（从数据中心导入数据到踩盘助手）
        /// </summary>
        /// <param name="projectList">数据中心楼盘</param>
        /// <param name="userName"></param>
        /// <param name="userTrueName"></param>
        /// <param name="message"></param>
        bool ImputProject(List<DATProject> projectList, string userName, string userTrueName, out string message);

        /// <summary>
        /// 生成模板string
        /// </summary>
        /// <param name="project">楼盘</param>
        /// <param name="templet">模板</param>
        /// <returns></returns>
        TempletDto CreateProjectTempletDto(Project project, Templet templet);
    }
}
