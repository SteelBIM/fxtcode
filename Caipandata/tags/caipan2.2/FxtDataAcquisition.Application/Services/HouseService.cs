using AutoMapper;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace FxtDataAcquisition.Application.Services
{
    public class HouseService : IHouseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HouseService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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
    }
}
