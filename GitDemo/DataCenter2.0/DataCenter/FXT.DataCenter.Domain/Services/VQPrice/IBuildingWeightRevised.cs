using System.Linq;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IBuildingWeightRevised
    {
        /// <summary>
        /// 获取楼栋修正系数
        /// </summary>
        /// <param name="datWeightBuilding"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DatWeightBuilding> GetWeightBuildings(int ProjectId, string BuildingName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true);

    }
}
