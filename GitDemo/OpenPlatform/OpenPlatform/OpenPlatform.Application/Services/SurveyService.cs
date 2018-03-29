using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Domain.Models;
using OpenPlatform.Domain.Repositories;

namespace OpenPlatform.Application.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;

        public SurveyService(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        public IQueryable<int> GetCompanyId(string idNum)
        {
            return _surveyRepository.GetCompanyId(idNum);
        }

        public IQueryable<Surveyfiles> GetPicturesById(long objectId)
        {
            return _surveyRepository.GetPicturesById(objectId);
        }

        public IQueryable<SurveyInfoDto> GetSurveyInfoById(long objectId)
        {
            return _surveyRepository.GetSurveyInfoById(objectId);
        }
    }
}
