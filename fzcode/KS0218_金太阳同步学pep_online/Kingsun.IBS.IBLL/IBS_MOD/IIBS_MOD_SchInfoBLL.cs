using Kingsun.IBS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL.IBS2MOD
{
    /// <summary>
    /// IBS学校信息变动
    /// </summary>
    public interface IIBS_MOD_SchInfoBLL
    {
        /// <summary>
        /// 新增学校信息
        /// </summary>
        /// <param name="SchID">SchID</param>
        /// <returns></returns>
        bool Add(IBS_SchClassRelation SchInfo);

        /// <summary>
        /// 更新学校信息
        /// </summary>
        /// <param name="SchID">SchID</param>
        /// <returns></returns>
        bool Update(IBS_SchClassRelation SchInfo);

        /// <summary>
        /// 删除学校信息
        /// </summary>
        /// <param name="SchID">SchID</param>
        /// <returns></returns>
        bool Delete(IBS_SchClassRelation SchInfo);

    }
}
