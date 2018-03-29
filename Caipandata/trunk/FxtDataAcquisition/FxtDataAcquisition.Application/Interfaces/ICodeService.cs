using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface ICodeService
    {
        /// <summary>
        /// 照片类型code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> PhotoTypeCodeManager();

        /// <summary>
        /// 产权形式code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> RightCodeManager();

        /// <summary>
        /// 土地用途code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> PurposeCodeManager();

        /// <summary>
        /// 小区规模code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HousingScaleCodeManager();

        /// <summary>
        /// 等级code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> LevelManager();

        /// <summary>
        /// 建筑类型code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> BuildingTypeCodeManager();

        /// <summary>
        /// 建筑结构code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> BuildingStructureCodeManager();

        /// <summary>
        /// 楼栋位置code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> LocationCodeManager();

        /// <summary>
        /// 外墙装修code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> WallCodeManager();

        /// <summary>
        /// 内部装修code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> InnerFitmentCodeManager();

        /// <summary>
        /// 管道燃气code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> PipelineGasCodeManager();

        /// <summary>
        /// 采暖方式code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HeatingModeCodeManager();

        /// <summary>
        /// 墙体类型code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> WallTypeCodeManager();

        /// <summary>
        /// 房号用途code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HousePurposeCodeManager();

        /// <summary>
        /// 房号朝向code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HouseFrontCodeManager();

        /// <summary>
        /// 房号景观code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HouseSightCodeManager();

        /// <summary>
        /// 通风采光code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> VDCodeManager();

        /// <summary>
        /// 噪音情况code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> NoiseManager();

        /// <summary>
        /// 户型结构code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> StructureCodeManager();

        /// <summary>
        /// 户型code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HouseTypeCodeManager();

        /// <summary>
        /// 附属房屋类型code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HouseSubHouseTypeManager();

        /// <summary>
        /// 装修code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> HouseFitmentCodeTypeManager();

        /// <summary>
        /// 停车状况code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> GetAllParkingStatusList();

        /// <summary>
        /// 任务状态code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> AllotStatusCodeManager();

        /// <summary>
        /// 户型面积code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> BHousetypeCodeManager();

        /// <summary>
        /// 土地规划用途code列表
        /// </summary>
        /// <returns></returns>
        List<SYSCode> PlanPurposeCodeManager();

        /// <summary>
        /// 获取所有查询相关functioncode
        /// </summary>
        /// <returns></returns>
        int[] GetViewFunctionCodes();
    }
}
