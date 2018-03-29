using FxtNHibernate.FxtLoanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/* 作者:李晓东
 * 时间:2014.05.05
 * 摘要:新增
 * **/
namespace FxtNHibernate.DTODomain.FxtLoanDTO
{
    public class ReassessmentCollateral:DataCollateral
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 行政区名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 面积段
        /// </summary>
        public string BuildingAreaSection { get; set; }
        /// <summary>
        /// 建筑年代段
        /// </summary>
        public string JianZhuYearSection { get; set; }
        /// <summary>
        /// 贷款额度段
        /// </summary>
        public string LoanAmountSection { get; set; }
        /// <summary>
        /// 年龄段
        /// </summary>
        public string AgeSection { get; set; }
        /// <summary>
        /// 物业类型
        /// </summary>
        public string PurposeCodeName { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public string Months { get; set; }
        /// <summary>
        /// 现复古价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 抵贷率(风险状况)
        /// </summary>
        public decimal ArrivedLoanRates { get; set; }
        /// <summary>
        /// 计算方式(复估方法)
        /// </summary>
        public string CalculationMode { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public decimal PriceChange {
            get {
                if (!OldRate.Equals(0))
                    return decimal.Round(((Price / OldRate) - 1) * 100, 2);
                return 0;
            }
        }
    }
}
