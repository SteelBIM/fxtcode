
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    /// <summary>
    /// IBS班级信息变动
    /// </summary>
    public interface IIBS_MOD_ClassInfoBLL
    {
        /// <summary>
        /// 新增班级信息
        /// </summary>
        /// <param name="ClassID">ClassID</param>
        /// <returns></returns>
        bool Add(IBS_ClassUserRelation ClassInfo);

        /// <summary>
        /// 更新班级信息
        /// </summary>
        /// <param name="ClassID">ClassID</param>
        /// <returns></returns>
        bool Update(IBS_ClassUserRelation ClassInfo);

        /// <summary>
        /// 删除班级信息
        /// </summary>
        /// <param name="ClassID">ClassIDId</param>
        /// <returns></returns>
        bool Delete(IBS_ClassUserRelation ClassInfo);
    }
}
