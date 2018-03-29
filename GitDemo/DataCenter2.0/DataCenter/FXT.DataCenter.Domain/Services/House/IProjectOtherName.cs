using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IProjectOtherName
    {
        /// <summary>
        /// 根据ID获取楼盘别名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<SYS_ProjectMatch> GetProjectMatchById(int id, int cityId, int fxtCompanyId);

        /// <summary>
        /// 获取楼盘别名
        /// </summary>
        /// <param name="pm">传参对象</param>
        /// <param name="totalCount"></param>
        /// <param name="self">是否是查看自己，默认为是</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<SYS_ProjectMatch> GetProjectMatch(SYS_ProjectMatch pm);

        /// <summary>
        /// 判断网络名和系统名是否同时存在
        /// </summary>
        /// <param name="netName">网络名</param>
        /// <param name="systemName">系统名</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        //bool IsNetAndSysExist(string netName, int cityId, int fxtCompanyId, int? id = null);

        /// <summary>
        /// 新增楼盘别名
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        int AddProjectOtherName(SYS_ProjectMatch pm);

        /// <summary>
        /// 修改楼盘别名
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        int UpdateProjectOtherName(SYS_ProjectMatch pm);

        /// <summary>
        ///  更新系统楼盘名
        /// </summary>
        /// <param name="projectId">系统楼盘ID</param>
        /// <param name="projectName">系统楼盘名</param>
        /// <returns></returns>
        //int UpdateProjectName(int projectId, string projectName);

        /// <summary>
        /// 获取楼盘名称ID
        /// </summary>
        /// <param name="projectNetName"></param>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <returns></returns>
        SYS_ProjectMatch GetProjectMatchProjectId(string projectNetName, string netAreaName, int cityid, int fxtcompanyid);

        /// <summary>
        /// 删除楼盘别名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteProjectOtherName(int id);

        //删除楼盘别名
        int DeleteProjectOtherName(string netName, string netAreaName, int cityid, int fxtcompanyid);
    }
}
