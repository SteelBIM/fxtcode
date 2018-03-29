using AutoMapper;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Common;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FxtDataAcquisition.Application.Services
{
    public class HouseService : IHouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICodeService _codeService;

        public HouseService(IUnitOfWork unitOfWork, ICodeService codeService)
        {
            this._unitOfWork = unitOfWork;
            this._codeService = codeService;
        }

        public List<HouseDto> GetHouseByBuildingIdAndFloorNo(int buildingId, int cityId, int floorNo)
        {
            List<HouseDto> houseDtos = new List<HouseDto>();
            var houses = _unitOfWork.HouseRepository.Get(m => m.BuildingId == buildingId && m.CityID == cityId && m.Valid == 1).ToList();
            houses.ForEach((o) =>
            {
                var unitNo = o.UnitNo;
                HouseDto houseDto = Mapper.Map<House, HouseDto>(o);
                houseDto.UnitNo = GetUnitNoByUnitNoStr(unitNo);
                houseDto.HouseNo = GetHouseNoByUnitNoStr(unitNo);
                houseDtos.Add(houseDto);
            }
                );
            return houseDtos;
        }

        #region commom
        /// <summary>
        /// 根据单元字段分隔单元号和室号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <param name="unitno"></param>
        /// <param name="houseno"></param>
        public void GetHouseUnitNoAndHouseNo(string unitNoStr, out string unitno, out string houseno)
        {
            unitno = "";
            houseno = "";
            if (unitNoStr != null && unitNoStr.Contains("$"))
            {
                unitno = unitNoStr.Split('$')[0];
                houseno = unitNoStr.Split('$')[1];
            }
            else
            {
                unitno = unitNoStr;
            }
        }
        /// <summary>
        /// 获取单元号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <returns></returns>
        public string GetUnitNoByUnitNoStr(string unitNoStr)
        {
            string unitno = "";
            string houseno = "";
            GetHouseUnitNoAndHouseNo(unitNoStr, out unitno, out houseno);
            return unitno;
        }
        /// <summary>
        /// 获取室号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <returns></returns>
        public string GetHouseNoByUnitNoStr(string unitNoStr)
        {
            string unitno = "";
            string houseno = "";
            GetHouseUnitNoAndHouseNo(unitNoStr, out unitno, out houseno);
            return houseno;
        }
        /// <summary>
        /// 单元号和室号组成单元字段
        /// </summary>
        /// <param name="unitno"></param>
        /// <param name="houseno"></param>
        /// <returns></returns>
        public string SetHouseUnitNoAndHouseNo(string unitno, string houseno)
        {
            string unitNoStr = "{0}${1}";
            return string.Format(unitNoStr, unitno, houseno);

        }
        #endregion

        /// <summary>
        /// 生成单元室号模板
        /// </summary>
        /// <param name="house">单元室号</param>
        /// <param name="templet">模板</param>
        /// <returns></returns>
        public TempletDto CreateHouseTempletDto(House house, Templet templet)
        {
            TempletDto templetDto = new TempletDto();

            templetDto.TempletId = templet.TempletId;

            templetDto.TempletName = templet.TempletName;

            templetDto.FieldGroups = new List<FieldGroupDto>();

            //获取实体属性
            var propertys = house.GetType().GetProperties();

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
                        //fieldDto.FieldType = field.FieldType;
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
                            case "StructureCode":
                                codes = _codeService.StructureCodeManager();
                                break;
                            case "VDCode":
                                codes = _codeService.LevelManager();
                                break;
                            case "NoiseCode":
                                codes = _codeService.NoiseManager();
                                break;
                            case "PurposeCode":
                                codes = _codeService.HousePurposeCodeManager();
                                break;
                            case "FrontCode":
                                codes = _codeService.HouseFrontCodeManager();
                                break;
                            case "SightCode":
                                codes = _codeService.HouseSightCodeManager();
                                break;
                            case "HouseTypeCode":
                                codes = _codeService.HouseTypeCodeManager();
                                break;
                            case "SubHouseType":
                                codes = _codeService.HouseSubHouseTypeManager();
                                break;
                            case "FitmentCode":
                                codes = _codeService.HouseFitmentCodeTypeManager();
                                break;
                            default:
                                break;
                        }

                        //取值
                        var property = propertys.Where(pInfo => pInfo.Name == field.FieldName).FirstOrDefault();

                        if (property != null)
                        {
                            var value = property.GetValue(house);

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
