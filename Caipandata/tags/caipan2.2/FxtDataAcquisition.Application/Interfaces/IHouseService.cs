using FxtDataAcquisition.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IHouseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="floorNo"></param>
        /// <returns></returns>
        List<HouseDto> GetHouseByBuildingIdAndFloorNo(int buildingId, int cityId, int floorNo);

        /// <summary>
        /// 根据单元字段分隔单元号和室号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <param name="unitno"></param>
        /// <param name="houseno"></param>
        void GetHouseUnitNoAndHouseNo(string unitNoStr, out string unitno, out string houseno);

        /// <summary>
        /// 获取单元号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <returns></returns>
        string GetUnitNoByUnitNoStr(string unitNoStr);

        /// <summary>
        /// 获取室号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <returns></returns>
        string GetHouseNoByUnitNoStr(string unitNoStr);

        /// <summary>
        /// 单元号和室号组成单元字段
        /// </summary>
        /// <param name="unitno"></param>
        /// <param name="houseno"></param>
        /// <returns></returns>
        string SetHouseUnitNoAndHouseNo(string unitno, string houseno);
    }
}
