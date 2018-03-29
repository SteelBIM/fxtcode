using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-04-08
 * 摘要: 新建实体类
 *       2014.05.13 修改人:李晓东
 *                  新增:TenPrice、TwentyPrice、TenArrivedLoanRates、TwentyArrivedLoanRates 四列
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities{
		/// <summary>
 	///Data_Reassessment
		/// </summary>
    [Serializable]
    [TableAttribute("dbo.Data_Reassessment")]
    public class DataReassessment : BaseTO
	{
	
      	/// <summary>
		/// 复估表
        /// </summary>
        [SQLField("ID", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// 押品ID
        /// </summary>
        public virtual int CollateralId
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
        /// 百分之十下跌复估值
        /// </summary>
        public virtual decimal TenPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 百分之二十下跌复估值
        /// </summary>
        public virtual decimal TwentyPrice
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
		/// 月份:201312
        /// </summary>
        public virtual string Months
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
        /// 百分之十下跌抵贷率
        /// </summary>
        public virtual decimal? TenArrivedLoanRates
        {
            get;
            set;
        }
        /// <summary>
        /// 百分之二十下跌抵贷率
        /// </summary>
        public virtual decimal? TwentyArrivedLoanRates
        {
            get;
            set;
        }   
		/// <summary>
		/// 风险状况(小数保留4位)
        /// </summary>
        public virtual decimal? RiskStatus
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
		   
	}
}