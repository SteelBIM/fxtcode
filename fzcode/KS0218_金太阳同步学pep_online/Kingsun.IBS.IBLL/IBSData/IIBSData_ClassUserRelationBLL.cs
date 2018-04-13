using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL
{
    /// <summary>
    /// 班级用户关系
    /// </summary>
    public interface IIBSData_ClassUserRelationBLL
    {
        /// <summary>
        /// 获取班级用户关系
        /// </summary>
        /// <param name="classId">ClassId</param>
        /// <returns></returns>
        IBS_ClassUserRelation GetClassUserRelationByClassId(string classId);

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
        /// <param name="type">查找类型（ClassID=0,ClassNum=1）</param>
        /// <returns></returns>
        IBS_ClassUserRelation GetClassUserRelationByClassOtherId(string classOtherId, int type);

        KingResponse GetUserClassInfoList(string userIds);
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
        string Add(IBS_ClassUserRelation classInfo,int operatorUserId);
        /// <summary>
        /// 财富分账新增班级
        /// </summary>
        /// <param name="classInfo"></param>
        /// <param name="operatorUserId"></param>
        /// <returns></returns>
        string CFAdd(IBS_ClassUserRelation classInfo, int operatorUserId);
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

        KingResponse UnBindClass(UserClassData data);
        KingResponse AddUserToClass(UserClassData data);

        string UnBindClassByClassId(string classId);
        /// <summary>
        /// 查询Redis班级全部信息
        /// </summary>
        /// <returns></returns>
        List<IBS_ClassUserRelation> SearchALL();
        List<UserClass> GetUserClassRelationByNum(string classNum, out List<StudentCount> stuCount);

    }
}
