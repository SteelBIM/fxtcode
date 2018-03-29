using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using System.Collections.Generic;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IBuildingService
    {
        List<BuildingDto> GetBuildingDetals(int projectId, int cityId,  int fxtCompanyId);

        /// <summary>
        /// 生成楼栋模板
        /// </summary>
        /// <param name="building">楼栋</param>
        /// <param name="templet">模板</param>
        /// <returns></returns>
        TempletDto CreateBuildingTempletDto(Building building, Templet templet);
    }
}
