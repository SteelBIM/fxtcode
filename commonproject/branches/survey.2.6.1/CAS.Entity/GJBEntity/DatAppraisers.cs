using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;
using GJB.Entity;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Appraisers : DatAppraisers
    {
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        [SQLReadOnly]
        public string truename { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [SQLReadOnly]
        public string sex { get; set; }
        /// <summary>
        /// 证书到期时间
        /// </summary>
        [SQLReadOnly]
        public DateTime certvaliddate { get; set; }
        /// <summary>
        /// 估价师类型
        /// </summary>
        [SQLReadOnly]
        public string appraisertypetext{ get; set; }
        /// <summary>
        /// 估价师电话
        /// </summary>
        [SQLReadOnly]
        public string mobile { get; set; }
        /// <summary>
        /// 房地产估价师证书号码
        /// </summary>
        [SQLReadOnly]
        public string fdccertno { get; set; }
    }
}
