using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.4.16
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //LNK_P_Photo
    public class LNKPPhoto
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
        /// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// PhotoTypeCode
        /// </summary>
        public virtual int? PhotoTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// Path
        /// </summary>
        public virtual string Path
        {
            get;
            set;
        }
        /// <summary>
        /// PhotoDate
        /// </summary>
        public virtual DateTime? PhotoDate
        {
            get;
            set;
        }
        /// <summary>
        /// PhotoName
        /// </summary>
        public virtual string PhotoName
        {
            get;
            set;
        }
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// Valid
        /// </summary>
        public virtual int? Valid
        {
            get;
            set;
        }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public virtual int FxtCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// BuildingId
        /// </summary>
        public virtual long? BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// X
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        /// <summary>
        /// Y
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }

    }
}