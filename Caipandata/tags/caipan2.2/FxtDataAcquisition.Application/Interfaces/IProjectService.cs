using FxtDataAcquisition.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IProjectService
    {
        ProjectDto GetProjectDetals(int projectid, int cityid, long allotid,int fxtCompanyId);

        int SetAllotProjectInfo(int allotid, string username, string userTrueName, int FxtCompanyId, int cityid, ProjectDto project, int isValid, out long returnAllotId, out int returnProjectId);
    }
}
