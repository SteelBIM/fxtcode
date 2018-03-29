using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IHumanProject
    {
        IQueryable<DAT_Human> GetHumanProjects(int? areaid, string projectName, int cityId, int fxtcompanyId, bool self);
        DAT_Human GetHumanProjectById(long humanid, int fxtcompanyid, int cityid);
        int AddHumanProject(DAT_Human dh);
        int UpdateHumanProject(DAT_Human dh);
        int DeleteHumanProject(long humanid, string saver, DateTime savetime);

        IQueryable<DAT_Human> GetProjectList(int fxtCompanyId, int cityId, long projectId);

        IQueryable<DAT_Human> ExportHumanProjects(int? areaid, string projectName, int cityId, int fxtcompanyId, bool self);
    }
}
