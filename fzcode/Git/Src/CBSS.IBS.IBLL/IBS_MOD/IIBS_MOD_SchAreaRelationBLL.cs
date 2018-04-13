
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    /// <summary>
    /// 学校区域关系变动
    /// </summary>
    public interface IIBS_MOD_SchAreaRelationBLL
    {
        /// <summary>
        /// 更改学校区域关系
        /// </summary>
        /// <param name="SchID">ClassID</param>
        /// <returns></returns>
        bool Change(int SchID);
    }
}
