using AutoMapper;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHouseService _houseService;

        public BuildingService(IUnitOfWork unitOfWork, IHouseService houseService)
        {
            this._unitOfWork = unitOfWork;
            this._houseService = houseService;
        }
        public List<BuildingDto> GetBuildingDetals(int projectId, int cityId, int fxtCompanyId)
        {

            List<BuildingDto> buildingDtos = new List<BuildingDto>();
            var buildings = _unitOfWork.BuildingRepository.Get(m => m.ProjectId == projectId);
            if (buildings != null && buildings.Count() > 0)
            {
                foreach (var item in buildings)
                {
                    BuildingDto buildingDto = Mapper.Map<Building, BuildingDto>(item);
                    //照片数
                    var photoCount = _unitOfWork.P_PhotoRepository.Get(m => m.CityId == cityId && m.ProjectId == projectId && m.BuildingId == item.BuildingId && m.FxtCompanyId == fxtCompanyId).Count();
                    buildingDto.BuildImageCount = photoCount;
                    var houses = _unitOfWork.HouseRepository.Get(m => m.BuildingId == item.BuildingId && m.CityID == item.CityID && m.FxtCompanyId == item.FxtCompanyId).ToList().Count();
                    buildingDto.UnitsNumber = houses;
                    buildingDtos.Add(buildingDto);
                }

            }
            return buildingDtos;
        }
    }
}
