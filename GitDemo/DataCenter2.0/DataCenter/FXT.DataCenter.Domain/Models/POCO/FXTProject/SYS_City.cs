using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_City
    {

        /// <summary>
        /// CityId
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// CityName
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 省份ID
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// CityCode
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// GIS_ID
        /// </summary>
        public int? GIS_ID { get; set; }
        /// <summary>
        /// 楼盘数
        /// </summary>
        public int? ProjectCount { get; set; }
        /// <summary>
        /// 报盘案例占在线估价的比重
        /// </summary>
        public decimal? PriceBP { get; set; }
        /// <summary>
        /// 成交案例占在线估价的比重
        /// </summary>
        public decimal? PriceCJ { get; set; }
        /// <summary>
        /// 是否可以查案例
        /// </summary>
        public int? IsCase { get; set; }
        /// <summary>
        /// 是否可以在线估价
        /// </summary>
        public int? IsEValue { get; set; }
        /// <summary>
        /// OldId
        /// </summary>
        public int? OldId { get; set; }
        /// <summary>
        /// 在线估价选取案例时间（月）
        /// </summary>
        public int? CaseMonth { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public decimal? X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public decimal? Y { get; set; }
        /// <summary>
        /// 比例尺
        /// </summary>
        public int? XYScale { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string Alias { get; set; }

    }
}
