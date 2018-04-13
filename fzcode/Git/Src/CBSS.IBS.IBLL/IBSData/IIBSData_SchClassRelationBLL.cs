
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.IBLL
{
    /// <summary>
    /// 学校班级关系
    /// </summary>
    public interface IIBSData_SchClassRelationBLL
    {
        /// <summary>
        /// 获取学校班级关系
        /// </summary>
        /// <param name="SchlId">用户ID</param>
        /// <returns></returns>
        TBX_SchClassRelation GetSchClassRelationBySchlId(int SchlId);
            /// <summary>
        /// 获取学校班级关系+区域名字
        /// </summary>
        /// <param name="schId">用户ID</param>
        /// <returns></returns>
        TBX_SchClassRelation GetSchoolALLInfoBySchoolId(int SchlId);
        /// <summary>
        /// 新增学校信息
        /// </summary>
        /// <param name="SchInfo"></param>
        /// <returns></returns>
        bool Add(TBX_SchClassRelation SchInfo);

        /// <summary>
        /// 更新学校信息
        /// </summary>
        /// <param name="SchInfo"></param>
        /// <returns></returns>
        bool Update(TBX_SchClassRelation SchInfo);
        
    }
}
