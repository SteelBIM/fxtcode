using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    /// <summary>
    /// SYS_City
    /// </summary>
    public class SYSCity 
    {

        /// <summary>
        /// CityId
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// CityName
        /// </summary>
        public virtual string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 省份ID
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// CityCode
        /// </summary>
        public virtual string CityCode
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
        /// 楼盘数
        /// </summary>
        public virtual int? ProjectCount
        {
            get;
            set;
        }
        /// <summary>
        /// 报盘案例占在线估价的比重
        /// </summary>
        public virtual decimal? PriceBP
        {
            get;
            set;
        }
        /// <summary>
        /// 成交案例占在线估价的比重
        /// </summary>
        public virtual decimal? PriceCJ
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可以查案例
        /// </summary>
        public virtual int? IsCase
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可以在线估价
        /// </summary>
        public virtual int? IsEValue
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
        /// 在线估价选取案例时间（月）
        /// </summary>
        public virtual int? CaseMonth
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
        /// 简称
        /// </summary>
        public virtual string Alias
        {
            get;
            set;
        }

    }
}