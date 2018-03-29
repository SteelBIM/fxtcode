using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-04-28
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities{
		/// <summary>
 	///Data_ReassessmentHistory
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Data_Reassessment")]
	public class DataReassessmentHistory:BaseTO
	{
	
      	/// <summary>
		/// 复估ID
        /// </summary>
        public virtual int ReassessmentID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 复估值
        /// </summary>
        public virtual decimal Price
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 是否初次复估
        /// </summary>
        public virtual int IsFirst
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 创建时间
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 抵贷率(贷款余额/复估值, 小数保留4位)
        /// </summary>
        public virtual decimal? ArrivedLoanRates
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 风险状况
        /// </summary>
        public virtual int? RiskStatus
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 计算方式
        /// </summary>
        public virtual string CalculationMode
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 操作者
        /// </summary>
        public virtual int? Operator
        {
            get; 
            set; 
        }        
		   
	}
}