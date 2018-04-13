using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract.IBSResource
{
    [Auditable]
    [Table("MODVersions")]
    public class MODVersions
    {
        [Required(ErrorMessage = "MODBookID不能为空")]
        public int MODVersionId { get; set; }
        /// <summary>
        /// 书本名称
        /// </summary>
        public string MODVersionName { get; set; }
        /// <summary>
        /// 书本封面
        /// </summary>
        public long SubjectId { get; set; }
        
    }
}
