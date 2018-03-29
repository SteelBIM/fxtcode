using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Repositories
{
    public class ProjectAllotFlowSurveyRepository : IProjectAllotFlowSurveyRepository
    {
        private IUnitOfWork _unitOfWork;
        public ProjectAllotFlowSurveyRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<ProjectAllotFlowSurveyDto> FindAll(
            Expression<Func<Project, bool>> projectFilter,
            Expression<Func<AllotFlow, bool>> allotFlowFilter,
            Expression<Func<AllotSurvey, bool>> allotSurveyFilter,
            Expression<Func<AllotSurvey, object>> orderby,
            int pageIndex, int pageSize, out int records
            )
        {
            var projects = _unitOfWork.ProjectRepository.Get(projectFilter);
            var allotFlows = _unitOfWork.AllotFlowRepository.Get(allotFlowFilter);
            //var allotsurveys = _unitOfWork.AllotSurveyRepository.Get(allotSurveyFilter);
            var buildings = _unitOfWork.BuildingRepository.Get();
            var photos = _unitOfWork.P_PhotoRepository.Get();

            var data = from p in projects
                       join f in allotFlows on p.ProjectId equals f.DatId
                       //join s in allotsurveys on f.DatId equals s.AllotId
                       //orderby f.StateDate descending
                       select new ProjectAllotFlowSurveyDto()
                       {
                           FxtprojectId = p.FxtProjectId,
                           ProjectId = p.ProjectId,
                           projectName = p.ProjectName,
                           CityId = p.CityID,
                           AreaID = p.AreaID,
                           SubAreaId = p.SubAreaId,
                           UserName = f.UserName,
                           UserTrueName = f.UserTrueName,
                           SurveyUserName = f.SurveyUserName,
                           SurveyUserTrueName = f.SurveyUserTrueName,
                           TatolBuildingNum = (
                                        from b in buildings
                                        where b.ProjectId == p.ProjectId
                                        && b.CityID == p.CityID && b.FxtCompanyId == p.FxtCompanyId
                                        select b
                                    ).Count(),
                           PhotoCount = (
                                        from ph in photos
                                        where ph.ProjectId == p.ProjectId 
                                        && ph.CityId == p.CityID && ph.FxtCompanyId == p.FxtCompanyId
                                        && (!ph.BuildingId.HasValue || ph.BuildingId == 0)
                                        select ph
                                    ).Count(),
                           //totalnum = p.TotalNum,
                           Address = p.Address,
                           X = p.X,
                           Y = p.Y,
                           AllotId = f.Id,
                           AllotDate = f.StateDate,
                           //StateDate = f.StateDate,
                           AllotState = f.StateCode
                       };

            records = data.Count();
            //Expression<Func<ProjectAllotFlowSurveyDto, object>> orderby2 = m=>m.AllotDate;

            return data.OrderByDescending(m => m.AllotDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
