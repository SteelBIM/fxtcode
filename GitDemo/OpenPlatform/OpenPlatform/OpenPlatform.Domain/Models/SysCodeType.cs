using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class SysCodeType
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// TypeCode
        /// </summary>
        public int? TypeCode { get; set; }
        /// <summary>
        /// TypeName
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }        
    }
}
