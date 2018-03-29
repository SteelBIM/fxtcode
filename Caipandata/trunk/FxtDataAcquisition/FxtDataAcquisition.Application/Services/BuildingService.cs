using AutoMapper;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Common;
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
        private readonly ICodeService _codeService;

        public BuildingService(IUnitOfWork unitOfWork, IHouseService houseService, ICodeService codeService)
        {
            this._unitOfWork = unitOfWork;
            this._houseService = houseService;
            this._codeService = codeService;
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

        /// <summary>
        /// 生成楼栋模板
        /// </summary>
        /// <param name="building">楼栋</param>
        /// <param name="templet">模板</param>
        /// <returns></returns>
        public TempletDto CreateBuildingTempletDto(Building building, Templet templet)
        {
            TempletDto templetDto = new TempletDto();

            templetDto.TempletId = templet.TempletId;

            templetDto.TempletName = templet.TempletName;

            templetDto.FieldGroups = new List<FieldGroupDto>();

            //获取实体属性
            var propertys = building.GetType().GetProperties();

            foreach (var fieldGroup in templet.FieldGroups)
            {
                FieldGroupDto fieldGroupDto = new FieldGroupDto();

                fieldGroupDto.FieldGroupName = fieldGroup.FieldGroupName;

                fieldGroupDto.Fields = new List<FieldDto>();

                if (fieldGroup.Fields != null && fieldGroup.Fields.Count > 0)
                {
                    foreach (var field in fieldGroup.Fields)
                    {
                        FieldDto fieldDto = new FieldDto();
                        fieldDto.EdiTextType = field.EdiTextType;
                        fieldDto.FieldName = field.FieldName;
                        fieldDto.FieldType = field.FieldType;
                        fieldDto.Title = field.Title;
                        fieldDto.MaxLength = field.MaxLength;
                        fieldDto.MinLength = field.MinLength;
                        fieldDto.IsRequired = field.IsRequired;
                        fieldDto.IsNull = field.IsNull;

                        switch (field.FieldType)
                        {
                            case 1:
                                fieldDto.Type = "E";
                                break;
                            case 2:
                                fieldDto.Type = "T";
                                break;
                            case 3:
                                fieldDto.Type = "R";
                                break;
                            case 5:
                                fieldDto.Type = "C";
                                break;
                            case 6:
                                fieldDto.Type = "DT";
                                break;
                            default:
                                break;
                        }

                        List<FxtDataAcquisition.Domain.Models.SYSCode> codes = new List<FxtDataAcquisition.Domain.Models.SYSCode>();

                        switch (field.FieldName)
                        {
                            case "PurposeCode":
                                codes = _codeService.PurposeCodeManager();
                                break;
                            case "MaintenanceCode":
                                codes = _codeService.LevelManager();
                                break;
                            case "BuildingTypeCode":
                                codes = _codeService.BuildingTypeCodeManager();
                                break;
                            case "StructureCode":
                                codes = _codeService.BuildingStructureCodeManager();
                                break;
                            case "Wall":
                                codes = _codeService.WallCodeManager();
                                break;
                            case "InnerFitmentCode":
                                codes = _codeService.InnerFitmentCodeManager();
                                break;
                            case "PipelineGasCode":
                                codes = _codeService.PipelineGasCodeManager();
                                break;
                            case "HeatingModeCode":
                                codes = _codeService.HeatingModeCodeManager();
                                break;
                            case "WallTypeCode":
                                codes = _codeService.WallTypeCodeManager();
                                break;
                            case "BHouseTypeCode":
                                codes = _codeService.BHousetypeCodeManager();
                                break;
                            default:
                                break;
                        }

                        //取值
                        var property = propertys.Where(pInfo => pInfo.Name == field.FieldName).FirstOrDefault();

                        if (property != null)
                        {
                            var value = property.GetValue(building);

                            if (value != null)
                            {
                                if (value is DateTime)
                                {
                                    fieldDto.Value = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    fieldDto.Value = value.ToString();

                                    if (codes != null && codes.Count > 0)
                                    {
                                        //选项值
                                        if (fieldDto.FieldType == 3)
                                        {
                                            var code = codes.FirstOrDefault(m => m.Code == int.Parse(fieldDto.Value));

                                            fieldDto.Value = code == null ? "" : code.CodeName;
                                        }
                                        else if (fieldDto.FieldType == 5)
                                        {
                                            //多选
                                            var vs = fieldDto.Value.Split(',').ConvertToIntList();

                                            var codeList = codes.Where(m => vs.Contains(m.Code));

                                            if (codeList != null)
                                            {
                                                var vsname = codeList.Select(m => m.CodeName);

                                                fieldDto.Value = string.Join(",", vsname);
                                            }

                                        }

                                        fieldDto.Choise = codes.Select(m => m.CodeName).ToList();
                                    }
                                }
                            }
                        }

                        fieldGroupDto.Fields.Add(fieldDto);
                    }
                }

                templetDto.FieldGroups.Add(fieldGroupDto);
            }

            return templetDto;
        }
    }
}
