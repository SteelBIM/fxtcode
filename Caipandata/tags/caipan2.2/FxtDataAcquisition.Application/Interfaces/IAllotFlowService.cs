using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IAllotFlowService
    {
        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <param name="allotId"></param>
        /// <param name="oldCode">原来的任务状态</param>
        /// <param name="stateCode">任务状态</param>
        /// <returns></returns>
        int SetAllotStatus(string userName, int cityId, long[] allotIds, int oldCode, int stateCode, string remark
             , string userTrueName = "", string surveyUserName = "", string surveyUserTrueName = "");

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="project">楼盘</param>
        /// <param name="developersCompany">开发商</param>
        /// <param name="managerCompany">物管公司</param>
        /// <returns>任务已存在返回任务状态，1.成功，0失败</returns>
        ProjectAllotFlowSurveyDto AddAllot(Project project, string developersCompany, string managerCompany, string remark, int cityId, int fxtCompanyId, string userName, string userTrueName, int status);

        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Project ExistsAllot(Project project);

        int ImportToDataCenter(long allowId, int cityId, int companyId, string userName, string userTrueName, string signName, List<UserCenter_Apps> appList, out string message);
    }
}
