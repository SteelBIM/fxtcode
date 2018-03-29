using FxtDataAcquisition.Domain.DTO;
using System.Collections.Generic;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IBuildingService
    {
        List<BuildingDto> GetBuildingDetals(int projectId, int cityId,  int fxtCompanyId);
    }
}
