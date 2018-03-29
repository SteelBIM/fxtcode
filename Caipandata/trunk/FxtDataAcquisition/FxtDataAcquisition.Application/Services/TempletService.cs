using AutoMapper;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Services
{
    public class TempletService : ITempletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TempletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Templet GetTempletDefult(int datType, int fxtCompanyId)
        {
            var templet = _unitOfWork.TempletRepository.GetBy(m => m.FxtCompanyId == fxtCompanyId && m.DatType == datType && m.Vaild == 1 && m.IsCurrent == true);

            return templet;
        }
    }
}
