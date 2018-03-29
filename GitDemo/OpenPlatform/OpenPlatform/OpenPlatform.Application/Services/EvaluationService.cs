using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Domain.Repositories;

namespace OpenPlatform.Application.Services
{
    public class EvaluationService : IEvaluationService
    {

        private readonly IEvaluationRepository _evaluationRepository;

        public EvaluationService(IEvaluationRepository evaluationRepository)
        {
            this._evaluationRepository = evaluationRepository;
        }

        public IQueryable<long> GetEoId(string id)
        {
            return _evaluationRepository.GetEoId(id);
        }

        public IQueryable<long> GetAppraiseStatusById(string id, int fxtCompanyId)
        {
            return _evaluationRepository.GetAppraiseStatusById(id,fxtCompanyId);
        }

        public IQueryable<EvaluationDto> GetEntrustAppraiseById(string id)
        {
            return _evaluationRepository.GetEntrustAppraiseById(id);
        }


        public IQueryable<EntrustObjectDto> GetEntrustObjectById(long id)
        {
            return _evaluationRepository.GetEntrustObjectById(id);
        }

        public IQueryable<PropertyInfoDto> GetPropertyBuyerById(long objectId)
        {
            return _evaluationRepository.GetPropertyBuyerById(objectId);
        }

        public IQueryable<string> GetPathListById(long objectId)
        {
            return _evaluationRepository.GetPathListById(objectId);
        }

        public IQueryable<BuyerInfoDto> GetBuyerInfoById(long objectId)
        {
            return _evaluationRepository.GetBuyerInfoById(objectId);
        }

        public int AddEntrust(Evaluation4GjbDto ed)
        {
            return _evaluationRepository.AddEntrust(ed);
        }

        public int AddEntrustObject(EntrustObject4GjbDto eo)
        {
            return _evaluationRepository.AddEntrustObject(eo);
        }

        public int AddProperty(PropertyInfo pi)
        {
            return _evaluationRepository.AddProperty(pi);
        }


        public int AddBuyer(BuyerInfo bi)
        {
            return _evaluationRepository.AddBuyer(bi);
        }

        public int AddPictures(Domain.Models.Surveyfiles sf)
        {
            return _evaluationRepository.AddPictures(sf);
        }

    }
}
