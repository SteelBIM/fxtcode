using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class UserClassRelationBLL
    {

        UserClassRelationDAL userClassRelationDAL = new UserClassRelationDAL();
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool AddRelation(TB_UserClassRelation relationInfo)
        {
            return userClassRelationDAL.AddRelation(relationInfo);
        }

        /// <summary>
        /// 根据userid获取用户班级信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public TB_UserClassRelation GetRelationByUserID(string userid)
        {
            TB_UserClassRelation relation = userClassRelationDAL.GetRelationByUserID(userid);
            return relation;
        }

        /// <summary>
        /// 更新关系
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateRelation(TB_UserClassRelation relationInfo)
        {
            return userClassRelationDAL.UpdateRelation(relationInfo);
        }

        /// <summary>
        /// 插入关系信息
        /// </summary>
        /// <param name="relationInfo"></param>
        /// <returns></returns>
        public bool InsertRelationInfo(TB_UserClassRelation relationInfo)
        {
            return userClassRelationDAL.InsertRelationInfo(relationInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IList<TB_UserClassRelation> GetRelationByClassID(string classid)
        {
            IList<TB_UserClassRelation> relationList = userClassRelationDAL.GetRelationByClassID(classid);
            return relationList;
        }

    }
}
