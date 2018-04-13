
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    /// <summary>
    /// IBS区域信息变动
    /// </summary>
    public interface IIBS_MOD_AreaInfoBLL
    {
        /// <summary>
        /// 新增区域信息
        /// </summary>
        /// <param name="AreaID">AreaID</param>
        /// <returns></returns>
        bool Add(IBS_AreaSchRelation AreaInfo);

        /// <summary>
        /// 更新区域信息
        /// </summary>
        /// <param name="AreaID">AreaID</param>
        /// <returns></returns>
        bool Update(IBS_AreaSchRelation AreaInfo);

        /// <summary>
        /// 删除区域信息
        /// </summary>
        /// <param name="AreaID">AreaID</param>
        /// <returns></returns>
        bool Delete(IBS_AreaSchRelation AreaInfo);
    }
}
