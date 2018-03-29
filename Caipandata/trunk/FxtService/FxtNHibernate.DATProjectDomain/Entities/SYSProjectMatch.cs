using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-20
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    /// <summary>
    ///SYS_ProjectMatch
    /// </summary>
    public class SYSProjectMatch
    {

        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectNameId
        /// </summary>
        public virtual int? ProjectNameId
        {
            get;
            set;
        }
        /// <summary>
        /// NetName
        /// </summary>
        public virtual string NetName
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectName
        /// </summary>
        public virtual string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int? CityId
        {
            get;
            set;
        }
        /// <summary>
        /// FXTCompanyId
        /// </summary>
        public virtual int? FXTCompanyId
        {
            get;
            set;
        }

    }
}