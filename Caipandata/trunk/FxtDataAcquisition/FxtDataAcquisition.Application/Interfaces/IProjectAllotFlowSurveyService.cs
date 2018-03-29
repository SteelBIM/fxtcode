using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IProjectAllotFlowSurveyService
    {
        IQueryable<ProjectAllotFlowSurveyDto> FindAll(
            Expression<Func<Project, bool>> projectFilter,
            Expression<Func<AllotFlow, bool>> allotFlowFilter,
            Expression<Func<AllotSurvey, bool>> allotSurveyFilter,
            Expression<Func<AllotSurvey, object>> orderby,
            int pageIndex, int pageSize,out int records
        );
    }
}
