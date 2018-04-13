
using CBSS.Framework.Contract.API;
using CBSS.IBS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.IBLL
{
    /// <summary>
    /// 班级用户关系
    /// </summary>
    public interface IIBSData_ClassUserRelationBLL
    {
        /// <summary>
        /// 获取班级用户关系
        /// </summary>
        /// <param name="ClassId">ClassId</param>
        /// <returns></returns>
        IBS_ClassUserRelation GetClassUserRelationByClassId(string ClassId);

        /// <summary>
        /// 获取班级用户关系+学校名称
        /// </summary>
        /// <param name="ClassId">ClassId</param>
        /// <returns></returns>
        TBX_ClassUserRelation GetClassUserRelationALLInfoByClassId(string ClassId);

        /// <summary>
        /// 通过ClassOtherId获取班级用户关系
        /// </summary>
        /// <param name="phoneOrUserName">班级编码</param>
        /// <param name="Type">查找类型（ClassID=0,ClassNum=1）</param>
        /// <returns></returns>
        IBS_ClassUserRelation GetClassUserRelationByClassOtherId(string ClassOtherId, int Type);

        APIResponse GetUserClassInfoList(string userIds);
        /// <summary>
        /// 构建MOD班级数据
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        IBS_ClassUserRelation BuildClassInfoByClassId(string classId);

        /// <summary>
        /// 新增班级信息
        /// </summary>
        /// <param name="classInfo">AreaID</param>
        /// <returns></returns>
        string Add(IBS_ClassUserRelation classInfo,int operatorUserID);

        /// <summary>
        /// 更新班级信息
        /// </summary>
        /// <param name="classInfo">AreaID</param>
        /// <returns></returns>
        bool Update(IBS_ClassUserRelation classInfo);

        /// <summary>
        /// 删除区域信息
        /// </summary>
        /// <param name="AreaID">AreaID</param>
        /// <returns></returns>
        bool Delete(string classId);

        APIResponse UnBindClass(UserClassData data);
        APIResponse AddUserToClass(UserClassData data);

        string UnBindClassByClassId(string classId);
        /// <summary>
        /// 查询Redis班级全部信息
        /// </summary>
        /// <returns></returns>
        List<IBS_ClassUserRelation> SearchALL();

        List<TBX_UserClass> GetUserClassRelationByNum(string ClassNum, out List<TBX_StudentCount> stuCount);

    }
}
