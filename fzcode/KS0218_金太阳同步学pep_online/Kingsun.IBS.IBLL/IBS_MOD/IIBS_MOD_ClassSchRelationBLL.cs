using Kingsun.IBS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL.IBS2MOD
{
    /// <summary>
    /// IBS班级学校关系变动
    /// </summary>
    public interface IIBS_MOD_ClassSchRelationBLL
    {
        /// <summary>
        /// 更改班级学校关系
        /// </summary>
        /// <param name="ClassID">ClassID</param>
        /// <returns></returns>
        bool Change(string ClassID);
    }
}
