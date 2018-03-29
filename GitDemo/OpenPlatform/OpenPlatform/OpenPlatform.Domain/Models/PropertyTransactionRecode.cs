using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class PropertyTransactionRecode
    {
        /// <summary>
        /// TranId
        /// </summary>
        public long TranId { get; set; }
        /// <summary>
        /// HouseId
        /// </summary>
        public long? HouseId { get; set; }
        /// <summary>
        /// TranDate
        /// </summary>
        public DateTime? TranDate { get; set; }
        /// <summary>
        /// TranPrice
        /// </summary>
        public decimal? TranPrice { get; set; }
        /// <summary>
        /// PropertyCertificateNum
        /// </summary>
        public string PropertyCertificateNum { get; set; }
        /// <summary>
        /// LandCertificateNum
        /// </summary>
        public string LandCertificateNum { get; set; }
        /// <summary>
        /// PropertyCertificateRegisteDate
        /// </summary>
        public DateTime? PropertyCertificateRegisteDate { get; set; }
        /// <summary>
        /// PropertyCertificateRegistePrice
        /// </summary>
        public decimal? PropertyCertificateRegistePrice { get; set; }
        /// <summary>
        /// LandCertificateDate
        /// </summary>
        public DateTime? LandCertificateDate { get; set; }
        /// <summary>
        /// LandCertificateArea
        /// </summary>
        public decimal? LandCertificateArea { get; set; }
        /// <summary>
        /// LandCertificateAddress
        /// </summary>
        public string LandCertificateAddress { get; set; }
        /// <summary>
        /// IsFirstBuy
        /// </summary>
        public bool IsFirstBuy { get; set; }
        /// <summary>
        /// PrepareLoanAmount
        /// </summary>
        public decimal? PrepareLoanAmount { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public bool Valid { get; set; }        
    }
}
