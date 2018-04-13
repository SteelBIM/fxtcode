using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class UserClassRelationDAL : BaseManagement
    {
        /// <summary>
        /// 新增关系信息
        /// </summary>
        /// <param name="relationInfo"></param>
        /// <returns></returns>
        public bool AddRelation(TB_UserClassRelation relationInfo)
        {
            return Insert<TB_UserClassRelation>(relationInfo);
        }

        /// <summary>
        /// 根据userid获取用户班级信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public TB_UserClassRelation GetRelationByUserID(string userid)
        {
            TB_UserClassRelation relation = SelectByCondition<TB_UserClassRelation>("UserID='" + userid + "'");
            if (userid == null)
            {
                return null;
            }
            else
            {
                return relation;
            }
        }

        /// <summary>
        /// 更新关系信息
        /// </summary>
        /// <param name="relationInfo"></param>
        /// <returns></returns>
        public bool UpdateRelation(TB_UserClassRelation relationInfo)
        {
            return Update<TB_UserClassRelation>(relationInfo);
        }

        /// <summary>
        /// 插入关系信息
        /// </summary>
        /// <param name="relationInfo"></param>
        /// <returns></returns>
        public bool InsertRelationInfo(TB_UserClassRelation relationInfo)
        {
            return Insert<TB_UserClassRelation>(relationInfo);
        }

        /// <summary>
        /// 根据班级ID获取班级学生信息
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public IList<TB_UserClassRelation> GetRelationByClassID(string classid)
        {
            string where = "ClassLongID=" + classid + " order by CreateDate desc";
            IList<TB_UserClassRelation> list = Search<TB_UserClassRelation>(where);
            return list;
        }

    }
}
