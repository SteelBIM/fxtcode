using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class PropertyBuyer
    {

        /// <summary>
        /// auto_increment
        /// </summary>
        public long POId { get; set; }
        /// <summary>
        /// TranId
        /// </summary>
        public long? TranId { get; set; }
        /// <summary>
        /// PersonId
        /// </summary>
        public long? PersonId { get; set; }
        /// <summary>
        /// RightPercent
        /// </summary>
        public int? RightPercent { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }  
    }
}
