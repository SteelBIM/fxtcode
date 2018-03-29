using System;

/**
 * 作者: 曾智磊
 * 时间: 2014-04-25
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //DAT_AvgPrice_Day
    public class DATAvgPriceDay
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
        /// CityId
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// AreaId
        /// </summary>
        public virtual int AreaId
        {
            get;
            set;
        }
        /// <summary>
        /// SubAreaId
        /// </summary>
        public virtual int SubAreaId
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
        /// AvgPriceDate
        /// </summary>
        public virtual DateTime AvgPriceDate
        {
            get;
            set;
        }
        /// <summary>
        /// AvgPrice
        /// </summary>
        public virtual int AvgPrice
        {
            get;
            set;
        }
        /// <summary>
        /// BuildingAreaType
        /// </summary>
        public virtual int? BuildingAreaType
        {
            get;
            set;
        }

    }
}