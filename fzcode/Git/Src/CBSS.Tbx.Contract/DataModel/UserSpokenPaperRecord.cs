using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    [Auditable]
    [Table("UserSpokenPaperRecord")]
    public class UserSpokenPaperRecord
    {
        /// <summary>
        /// 应用选择商品策略ID
        /// </summary>

        public Guid ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public long PaperID { get; set; }

        /// <summary>
        /// 商品策略ID
        /// </summary>

        public long StuID { get; set; }
       

        public int Score { get; set; }

        public int StuScore { get; set; }

        public string StuAnswer { get; set; }
    }
}
