using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-06
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
		/// <summary>
 	///DAT_ProjectAvgPrice
		/// </summary>
	public class DATProjectAvgPrice
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
		/// 默认25
        /// </summary>
        public virtual int FxtCompanyId
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
        public virtual string AvgPriceDate
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
		/// 面积段
        /// </summary>
        public virtual int? BuildingAreaType
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 用途
        /// </summary>
        public virtual int? PurposeType
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 建筑类型
        /// </summary>
        public virtual int? BuildingTypeCode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CreateTime
        /// </summary>
        public virtual DateTime? CreateTime
        {
            get; 
            set; 
        }

        /// <summary>
        /// 计算方式
        /// </summary>
        public virtual string JSFS
        {
            get;
            set;
        }
        /// <summary>
        /// 计算范围(月数,默认3个月)
        /// </summary>
        public virtual int DateRange
        {
            get;
            set;
        } 
		   
	}
}