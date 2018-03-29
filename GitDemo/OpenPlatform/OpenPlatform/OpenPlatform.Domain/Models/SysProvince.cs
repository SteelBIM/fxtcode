using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class SysProvince
    {
        /// <summary>
        /// ProvinceId
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// ProvinceName
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// Alias
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
        /// X
        /// </summary>
        public double? X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public double? Y { get; set; }
        /// <summary>
        /// XYScale
        /// </summary>
        public int? XYScale { get; set; }
        /// <summary>
        /// IsZXS
        /// </summary>
        public int? IsZXS { get; set; }
        /// <summary>
        /// GBCode
        /// </summary>
        public string GBCode { get; set; }      
    }
}
