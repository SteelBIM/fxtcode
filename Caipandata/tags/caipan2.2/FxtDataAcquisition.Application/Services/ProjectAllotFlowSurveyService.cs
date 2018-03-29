using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FxtDataAcquisition.Domain.Repositories;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;

namespace FxtDataAcquisition.Application.Services
{
    public class ProjectAllotFlowSurveyService : IProjectAllotFlowSurveyService
    {
        private readonly IProjectAllotFlowSurveyRepository _projectAllotFlowSurvey;

        public ProjectAllotFlowSurveyService(IProjectAllotFlowSurveyRepository projectAllotFlowSurvey)
        {
            _projectAllotFlowSurvey = projectAllotFlowSurvey;
        }

        public IQueryable<ProjectAllotFlowSurveyDto> FindAll(
            Expression<Func<Project, bool>> projectFilter,
            Expression<Func<AllotFlow, bool>> allotFlowFilter,
            Expression<Func<AllotSurvey, bool>> allotSurveyFilter,
            Expression<Func<AllotSurvey, object>> orderby,
            int pageIndex, int pageSize, out int records
            )
        {
            return _projectAllotFlowSurvey.FindAll(projectFilter, allotFlowFilter, allotSurveyFilter, orderby, pageIndex, pageSize, out records);
        }
    }
}
