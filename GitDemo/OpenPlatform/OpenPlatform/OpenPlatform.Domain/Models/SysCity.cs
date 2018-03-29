using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class SysCity
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
        /// ProvinceId
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// CityCode
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// GIS_ID
        /// </summary>
        public int? GIS_ID { get; set; }
        /// <summary>
        /// ProjectCount
        /// </summary>
        public int? ProjectCount { get; set; }
        /// <summary>
        /// PriceBP
        /// </summary>
        public double? PriceBP { get; set; }
        /// <summary>
        /// PriceCJ
        /// </summary>
        public double? PriceCJ { get; set; }
        /// <summary>
        /// IsCase
        /// </summary>
        public int? IsCase { get; set; }
        /// <summary>
        /// IsEValue
        /// </summary>
        public int? IsEValue { get; set; }
        /// <summary>
        /// OldId
        /// </summary>
        public int? OldId { get; set; }
        /// <summary>
        /// CaseMonth
        /// </summary>
        public int? CaseMonth { get; set; }
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
        /// Alias
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// GBCode
        /// </summary>
        public string GBCode { get; set; }      
    }
}
