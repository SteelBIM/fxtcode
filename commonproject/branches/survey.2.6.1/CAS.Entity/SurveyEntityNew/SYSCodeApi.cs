using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.SurveyEntityNew
{
    public class SYSCodeApi
    {
        /// <summary>
        /// code 的idd
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 是否能更改
        /// </summary>
        public bool canEdit { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string codeName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string codeType { get; set; }
        /// <summary>
        /// 查勘类型
        /// </summary>
        public int dicType { get; set; }

        public int fxtCompanyId { get; set; }
    }
}
