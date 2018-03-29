using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class ProjectBase
    {
        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// ProjectGUID
        /// </summary>
        public string ProjectGUID { get; set; }
        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// FXTCompanyId
        /// </summary>
        public int? FXTCompanyId { get; set; }
        /// <summary>
        /// LandValueInTermsOfPerUnitFloor
        /// </summary>
        public decimal? LandValueInTermsOfPerUnitFloor { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }
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
