using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity.FxtDataCenter
{
    /// <summary>
    /// 自动估价设置表
    /// </summary>
    public class DatEvalueSet:BaseTO
    {
        public int Id { get; set; }
        public int FxtCompanyId { get; set; }
        public int CityId { get; set; }
        /// <summary>
        /// 物业类型,1001
        /// </summary>
        public int PurposeCode { get; set; }
        /// <summary>
        /// 设置项,3006
        /// </summary>
        public int TypeCode { get; set; }
        /// <summary>
        /// 设置值,code
        /// </summary>
        public int ValueCode { get; set; }
        /// <summary>
        /// 设置值1（报盘比例）
        /// </summary>
        public decimal Value1 { get; set; }
        /// <summary>
        /// 设置值2（成交评估比例）
        /// </summary>
        public decimal Value2 { get; set; }

        public bool valid { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
