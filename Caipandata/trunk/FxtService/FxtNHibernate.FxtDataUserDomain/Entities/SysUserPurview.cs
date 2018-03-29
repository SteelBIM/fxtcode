using System;

/**
 * 作者: 李晓东
 * 时间: 2014-01-15
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtDataUserDomain.Entities
{
    /// <summary>
    ///Sys_UserPurview
    /// </summary>
    public class SysUserPurview
    {

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 用户
        /// </summary>
        public virtual string UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int? ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 菜单与权限ID
        /// </summary>
        public virtual int? MenuPurviewId
        {
            get;
            set;
        }

    }
}