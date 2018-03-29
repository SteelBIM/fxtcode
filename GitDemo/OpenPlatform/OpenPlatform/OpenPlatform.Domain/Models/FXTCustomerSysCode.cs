using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class FXTCustomerSysCode
    {
        /// <summary>
        /// CodeId
        /// </summary>
        public int CodeId { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int? ID { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public int? Code { get; set; }
        /// <summary>
        /// CodeName
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// CodeType
        /// </summary>
        public string CodeType { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// SubCode
        /// </summary>
        public int? SubCode { get; set; }
        /// <summary>
        /// CanEdit
        /// </summary>
        public bool CanEdit { get; set; }
        /// <summary>
        /// OrderId
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// DicType
        /// </summary>
        public int? DicType { get; set; }
        /// <summary>
        /// SubType
        /// </summary>
        public int? SubType { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public bool Valid { get; set; }
        /// <summary>
        /// FXTCompanyId
        /// </summary>
        public int? FXTCompanyId { get; set; }        
    }
}
