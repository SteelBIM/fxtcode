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
    [Table("MODSubjects")]
    public class MODSubjects
    {
        [Required(ErrorMessage = "MODSubjectId不能为空")]
        public long MODSubjectId { get; set; }
        /// <summary>
        /// 书本名称
        /// </summary>
        public string MODSubjectName { get; set; }
        /// <summary>
        /// 书本封面
        /// </summary>
        public int EnumValue { get; set; }
        public DateTime? CreateDate { get; set; }

        public int CreateUser { get; set;  }
    }
}
