using System;
using CAS.Entity.BaseDAModels;
using System.Collections.Generic;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ObjectPrice : BaseTO
    {
        /// <summary>
        /// 委估对象ID
        /// </summary>
        [SQLReadOnly]
        public long objectid { get; set; }
         /// <summary>
        /// 预评单价
        /// </summary>
        [SQLReadOnly]
        public decimal? ypunitprice { get; set; }
           /// <summary>
        /// 预评总价
        /// </summary>
        [SQLReadOnly]
        public decimal? totalpricebyyp { get; set; }
         /// <summary>
        /// 预评税费
        /// </summary>
        [SQLReadOnly]
        public decimal? yptax { get; set; }
        /// <summary>
        /// 预评净值
        /// </summary>
        [SQLReadOnly]
        public decimal? ypnetprice { get; set; }
         /// <summary>
        /// 预评法定优先受偿款
        /// </summary>
        [SQLReadOnly]
        public decimal? yplegalpayment { get; set; }
         /// <summary>
        /// 预评回价时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? ypbiddate { get; set; }
        /// <summary>
        /// 预评应补地价
        /// </summary>
        [SQLReadOnly]
        public decimal? ypshouldfilllandprice { get; set; }
        /// <summary>
        /// 预评强制变现值
        /// </summary>
        [SQLReadOnly]
        public decimal? ypliquidityvalue { get; set; }
         /// <summary>
        /// 预评强制变现税费额
        /// </summary>
        [SQLReadOnly]
        public decimal? ypliquiditytaxvalue { get; set; }
         /// <summary>
        /// 预评价格说明
        /// </summary>
        [SQLReadOnly]
        public string yppriceremark { get; set; }
        /// <summary>
        /// 预评土地价格
        /// </summary>
        [SQLReadOnly]
        public decimal? yplandunitprice { get; set; }
        /// <summary>
        /// 预评土地总价
        /// </summary>
        [SQLReadOnly]
        public decimal? yplandtotalprice { get; set; }
        /// <summary>
        /// 预评正常单价
        /// </summary>
        [SQLReadOnly]
        public decimal? ypnormalprice { get; set; }
        /// <summary>
        /// 预评评估总价
        /// </summary>
        [SQLReadOnly]
        public decimal? yphousetotalprice { get; set; }
         /// <summary>
        /// 报告单价
        /// </summary>
        [SQLReadOnly]
        public decimal? rpunitprice { get; set; }
         /// <summary>
        /// 报告总价
        /// </summary>
        [SQLReadOnly]
        public decimal? totalpricebyrp { get; set; }
         /// <summary>
        /// 报告税费
        /// </summary>
        [SQLReadOnly]
        public decimal? rptax { get; set; }

        /// <summary>
        /// 报告净值
        /// </summary>
         [SQLReadOnly]
        public decimal? rpnetprice { get; set; }
         /// <summary>
        /// 报告法定优先受偿款
        /// </summary>
        [SQLReadOnly] 
        public decimal? rplegalpayment { get; set; }
        /// <summary>
        /// 报告回价时间
        /// </summary>
        [SQLReadOnly] 
        public DateTime? rpbiddate { get; set; }
         /// <summary>
        /// 报告应补地价
        /// </summary>
        [SQLReadOnly] 
        public decimal? rpshouldfilllandprice { get; set; }
         /// <summary>
        /// 报告强制变现值
        /// </summary>
        [SQLReadOnly] 
        public decimal? rpliquidityvalue { get; set; }
         /// <summary>
        /// 报告强制变现税费额
        /// </summary>
        [SQLReadOnly] 
        public decimal? rpliquiditytaxvalue { get; set; }
      
         /// <summary>
        /// 报告价格说明
        /// </summary>
          [SQLReadOnly]   
        public string rppriceremark { get; set; }
          /// <summary>
          /// 报告土地价格
          /// </summary>
          [SQLReadOnly]
          public decimal? rplandunitprice { get; set; }
          /// <summary>
          /// 报告土地总价
          /// </summary>
          [SQLReadOnly]
          public decimal? rplandtotalprice { get; set; }
          /// <summary>
          /// 报告正常单价
          /// </summary>
          [SQLReadOnly]
          public decimal? rpnormalprice { get; set; }
          /// <summary>
          /// 报告评估总价
          /// </summary>
          [SQLReadOnly]
          public decimal? rphousetotalprice { get; set; }
    }
}
