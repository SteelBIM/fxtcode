using Kingsun.IBS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL
{
    /// <summary>
    /// 区域学校关系
    /// </summary>
    public interface IIBSData_AreaSchRelationBLL
    {
        /// <summary>
        /// 获取区域学校关系
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        IBS_AreaSchRelation GetAreaSchRelationByAreaId(int AreaId);


        /// <summary>
        /// 新增区域信息
        /// </summary>
        /// <param name="AreaID">AreaID</param>
        /// <returns></returns>
        bool Add(IBS_AreaSchRelation areaInfo);

        /// <summary>
        /// 更新区域信息
        /// </summary>
        /// <param name="AreaID">AreaID</param>
        /// <returns></returns>
        bool Update(IBS_AreaSchRelation areaInfo);

        /// <summary>
        /// 删除区域信息
        /// </summary>
        /// <param name="AreaID">AreaID</param>
        /// <returns></returns>
        bool Delete(int AreaID);

    }
}
