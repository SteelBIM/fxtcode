using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.FxtLog
{
    /// <summary>
    /// 自动估价记录
    /// </summary>
    [Serializable]
    [TableAttribute("FxtLog.dbo.AutoPrice_Log")]
    public class AutoPriceLog : BaseTO
    {
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public int Id { get; set; }
        public int FxtCompanyId { get; set; }
        public int ProductTypeCode { get; set; }
        public int CityId { get; set; }
        /// <summary>
        /// 楼盘
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public int? BuildingId { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public int? HouseId { get; set; }
        /// <summary>
        /// 总楼层
        /// </summary>
        public int? TotalFloor { get; set; }
        /// <summary>
        /// 所作层
        /// </summary>
        public int? FloorNo { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        public int? FrontCode { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 是否可估（1.可估，-1.无基准均价，-2.无案例均价，-3.无总楼层，4.-无楼盘均价，-5.无楼层差系数（VQ），-6.无楼层差系数（CAS），2.VQ房号系数，3.VQ楼层差，4.CAS房号系数，5.CAS楼层差，6.楼盘基准均价，7.楼盘案例均价）
        /// </summary>
        public int Estimable { get; set; }
        /// <summary>
        /// 估价类型（1.楼盘估价，2.房号估价，3.楼盘、房号估价，4.押品复估）
        /// </summary>
        public int AutoType { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        public DateTime AddTime { get; set; }
    }
}
