using CAS.Common;
using CAS.Entity.FxtProject;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;

namespace FxtCenterService.Logic
{
    public class ProjectMatchBL
    {
        public static List<ProjectMatch> GetProjectMatchList(SearchBase search, string netName)
        {
            return ProjectMatchDA.GetProjectMatchList(search, netName);
        }
    }
}
