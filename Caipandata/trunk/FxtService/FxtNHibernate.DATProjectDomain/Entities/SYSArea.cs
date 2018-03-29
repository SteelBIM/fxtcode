using System;

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    /// <summary>
    /// SYS_Area
    /// </summary>
    public class SYSArea
    {

        /// <summary>
        /// AreaId
        /// </summary>
        public virtual int AreaId
        {
            get;
            set;
        }
        /// <summary>
        /// AreaName
        /// </summary>
        public virtual string AreaName
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
        /// ConstructionCount
        /// </summary>
        public virtual int? ConstructionCount
        {
            get;
            set;
        }
        /// <summary>
        /// GIS_ID
        /// </summary>
        public virtual int? GIS_ID
        {
            get;
            set;
        }
        /// <summary>
        /// AreaPlacePicName
        /// </summary>
        public virtual string AreaPlacePicName
        {
            get;
            set;
        }
        /// <summary>
        /// OldId
        /// </summary>
        public virtual int? OldId
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
        /// <summary>
        /// 比例尺
        /// </summary>
        public virtual int? XYScale
        {
            get;
            set;
        }
        /// <summary>
        /// 排序ID
        /// </summary>
        public virtual int? OrderId
        {
            get;
            set;
        }

    }
}