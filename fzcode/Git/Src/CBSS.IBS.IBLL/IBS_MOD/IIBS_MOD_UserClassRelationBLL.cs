
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    /// <summary>
    /// IBS用户班级关系变动
    /// </summary>
    public interface IIBS_MOD_UserClassRelationBLL
    {
        /// <summary>
        /// 更改用户班级关系
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
         bool ClassUserRemove(IBS_UserInfo UserInfo);

         bool ClassUserAdd(IBS_UserInfo UserInfo);
    }
}
