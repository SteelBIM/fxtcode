using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;

namespace FXT.DataCenter.Domain.Services
{
    public interface IProjectCaseTask
    {

        /// <summary>
        /// 添加楼盘名称不匹配数据
        /// </summary>
        /// <param name="dct"></param>
        /// <returns></returns>
        int AddProjectNameMisMatch(DAT_CaseTemp dct);

        /// <summary>
        /// 增加楼盘名称匹配
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        int AddProjectMatch(SYS_ProjectMatch pm);
        /// <summary>
        /// 添加修改后的楼盘案例
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        int AddProjectCase(DAT_CaseTemp modal);
        /// <summary>
        /// 删除添加成功后的楼盘
        /// </summary>
        /// <returns></returns>
        int DelteDatCaseTemp(List<long> caseId);

        /// <summary>
        /// 修改楼盘ID
        /// </summary>
        /// <param name="caseid"></param>
        /// <param name="projectid"></param>
        /// <param name="taskId"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        int UpdateDatCaseTemp(long taskId, string sourceName, string areaName, int destProjectid, int cityid, int fxtcompanyid);

        /// <summary>
        /// 获取楼盘名称不匹配的楼盘
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        IQueryable<DAT_CaseTemp> GetDatCaseTemp(long taskId, int cityid, int fxtcompanyid, int projectid);

        /// <summary>
        /// 根据任务ID 获取不匹配楼盘数据
        /// </summary>
        /// <returns></returns>
        IQueryable<MisMatchProjectDTO> GetDatCaseTemp(long taskid, int cityid, int fxtcompanyid, string key);

        /// <summary>
        /// 自动匹配楼盘名称
        /// </summary>
        /// <returns></returns>
        int MatchProjectCase(int cityId, int fxtCompanyId, string userName, string ids);

        /// <summary>
        /// 待建楼盘转正式楼盘后，自动匹配楼盘名称
        /// </summary>
        /// <returns></returns>
        int MatchProjectCaseWaitProject(int waitProjectId, int projectId, int areaId, string projectName, int cityId, int fxtCompanyId, string userName);

        /// <summary>
        /// 转为待建楼盘后，更新楼盘ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="projectName"></param>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int UpdateDatCaseTempWaitProject(long taskId, long waitProjectId, string projectName, int cityid, int fxtcompanyid);
    }
}
