using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Province
    {

        /// <summary>
        /// ProvinceId
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 省的名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// GIS_ID
        /// </summary>
        public int? GIS_ID { get; set; }
        /// <summary>
        /// OldId
        /// </summary>
        public int? OldId { get; set; }
        /// <summary>
        /// X坐标
        /// </summary>
        public decimal? X { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public decimal? Y { get; set; }
        /// <summary>
        /// 比例尺
        /// </summary>
        public int? XYScale { get; set; }
        /// <summary>
        /// 是否直辖市
        /// </summary>
        public int? IsZXS { get; set; }

    }
}
