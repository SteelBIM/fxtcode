using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class SysCode
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public int? Code { get; set; }
        /// <summary>
        /// CodeName
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// TypeCode
        /// </summary>
        public int? TypeCode { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }   
    }
}
