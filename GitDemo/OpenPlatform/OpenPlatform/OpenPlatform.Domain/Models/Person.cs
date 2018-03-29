using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class Person
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public long PersonId { get; set; }
        /// <summary>
        /// PersonGUID
        /// </summary>
        public string PersonGUID { get; set; }
        /// <summary>
        /// PersonName
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// IDNum
        /// </summary>
        public string IDNum { get; set; }
        /// <summary>
        /// Sexual
        /// </summary>
        public int? Sexual { get; set; }
        /// <summary>
        /// Birthday
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// Phone1
        /// </summary>
        public int? Phone1 { get; set; }
        /// <summary>
        /// Phone2
        /// </summary>
        public int? Phone2 { get; set; }
        /// <summary>
        /// Source
        /// </summary>
        public int? Source { get; set; }
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
