using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class TB_APPManagement
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        public int? VersionID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }
    }
}
