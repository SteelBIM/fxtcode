using System;

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
	 	//SYS_SubArea
		public class SYSSubArea
	{
	
      	/// <summary>
		/// SubAreaId
        /// </summary>
        public virtual int SubAreaId
        {
            get; 
            set; 
        }        
		/// <summary>
		/// SubAreaName
        /// </summary>
        public virtual string SubAreaName
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
        public virtual int? GISID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// RegionPlacePicName
        /// </summary>
        public virtual string RegionPlacePicName
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
		   
	}
}