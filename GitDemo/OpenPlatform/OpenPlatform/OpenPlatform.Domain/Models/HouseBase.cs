using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class HouseBase
    {
        /// <summary>
        /// HouseId
        /// </summary>
        public long HouseId { get; set; }
        /// <summary>
        /// HouseGUID
        /// </summary>
        public string HouseGUID { get; set; }
        /// <summary>
        /// FXTCompanyId
        /// </summary>
        public int? FXTCompanyId { get; set; }
        /// <summary>
        /// HouseName
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// BuildingId
        /// </summary>
        public long? BuildingId { get; set; }
        /// <summary>
        /// BuildingArea
        /// </summary>
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// Floor
        /// </summary>
        public int? Floor { get; set; }
        /// <summary>
        /// Hall
        /// </summary>
        public int? Hall { get; set; }
        /// <summary>
        /// Room
        /// </summary>
        public int? Room { get; set; }
        /// <summary>
        /// Washroom
        /// </summary>
        public int? Washroom { get; set; }
        /// <summary>
        /// Balcony
        /// </summary>
        public int? Balcony { get; set; }
        /// <summary>
        /// Kitchen
        /// </summary>
        public int? Kitchen { get; set; }
        /// <summary>
        /// LandArea
        /// </summary>
        public decimal? LandArea { get; set; }
        /// <summary>
        /// PracticalArea
        /// </summary>
        public decimal? PracticalArea { get; set; }
        /// <summary>
        /// Fitment
        /// </summary>
        public int? Fitment { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public bool Valid { get; set; }        
    }
}
