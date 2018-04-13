using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{

    public class tb_code_edition
    {
        /// <summary>
        /// 版本ID
        /// </summary>
        public int EditionID { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string EditionName { get; set; }

        /// <summary>
        /// 学科ID
        /// </summary>
        public int? SubjectID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
