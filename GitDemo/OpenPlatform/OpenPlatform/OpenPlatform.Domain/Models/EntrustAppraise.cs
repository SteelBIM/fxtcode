using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class EntrustAppraise
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public long EAId { get; set; }
        /// <summary>
        /// FXTCompanyId
        /// </summary>
        public int FXTCompanyId { get; set; }
        /// <summary>
        /// GJBEntrustId
        /// </summary>
        public long? GJBEntrustId { get; set; }
        /// <summary>
        /// GJBObjId
        /// </summary>
        public long? GJBObjId { get; set; }
        /// <summary>
        /// TranId
        /// </summary>
        public long? TranId { get; set; }
        /// <summary>
        /// HouseId
        /// </summary>
        public long? HouseId { get; set; }
        /// <summary>
        /// BuildingId
        /// </summary>
        public long? BuildingId { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public long? ProjectId { get; set; }
        /// <summary>
        /// ObjectFullName
        /// </summary>
        public string ObjectFullName { get; set; }
        /// <summary>
        /// IsSurvey
        /// </summary>
        public bool IsSurvey { get; set; }
        /// <summary>
        /// BuyingType
        /// </summary>
        public int? BuyingType { get; set; }
        /// <summary>
        /// ClientPersonId
        /// </summary>
        public long? ClientPersonId { get; set; }
        /// <summary>
        /// ClientContact
        /// </summary>
        public string ClientContact { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// UpdateDate
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public bool Valid { get; set; }        
    }
}
