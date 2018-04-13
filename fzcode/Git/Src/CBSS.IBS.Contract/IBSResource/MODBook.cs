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
    [Table("MODBook")]
    public class MODBook
    {
        [Required(ErrorMessage = "MODBookID不能为空")]
        public int MODBookID { get; set; }
        /// <summary>
        /// 书本名称
        /// </summary>
        public string MODBookName { get; set; }
        /// <summary>
        /// 书本全称
        /// </summary>
        public string MODBookFullName { get; set; }
        /// <summary>
        /// 书本封面
        /// </summary>
        public string MODBookCover { get; set; }

        public int MODBookKeyId { get; set; }
        public string MODBookKey { get; set; }

        public long SubjectID { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public int VerId { get; set; }

        public int Stage { get;set; }
        public int GradeID { get; set; }
       
        public int ReelID { get; set; }
        public int PageStart { get; set; }

        public int PageEnd { get; set;  }
    }
}
