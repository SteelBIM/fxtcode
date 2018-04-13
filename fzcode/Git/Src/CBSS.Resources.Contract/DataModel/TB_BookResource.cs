using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.ResourcesManager.Contract.DataModel
{
    public class TB_BookResource
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public int? BookID { get; set; }

        /// <summary>
        /// 资源地址
        /// </summary>
        public string ResourceUrl { get; set; }

    }
}
