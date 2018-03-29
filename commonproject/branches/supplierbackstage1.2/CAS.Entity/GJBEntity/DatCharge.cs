using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Charge : DatCharge
    {
        [SQLReadOnly]
        public string businesstypename { get; set; }
        /// <summary>
        /// 业务名称
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string businessuser { get; set; }
        [SQLReadOnly]
        public int businessuserid { get; set; }
        // <summary>
        /// 收费人
        /// </summary>
        [SQLReadOnly]
        public string chargeusername { get; set; }
        /// <summary>
        /// 是否月结
        /// </summary>
        [SQLReadOnly]
        public bool ischargemonthly { get; set; }
        /// <summary>
        /// <summary>
        /// 业务编号
        /// </summary>
        [SQLReadOnly]
        public long entrustid { get; set; }
        [SQLReadOnly]
        public int typecode { get; set; }
        [SQLReadOnly]
        public string typecodename { get; set; }
        [SQLReadOnly]
        public int subcompanyid { get; set; }
        [SQLReadOnly]
        public int ownertypecode { get; set; }
        [SQLReadOnly]
        public string paymentuser { get; set; }
        /// <summary>
        /// 评估总值
        /// </summary>
        [SQLReadOnly]
        public decimal querytotalprice { get; set; }
        
        /// <summary>
        /// 国家标准
        /// </summary>
        [SQLReadOnly]
        public double nationalstandard { get; set; }
        /// <summary>
        /// 公司标准
        /// </summary>
        [SQLReadOnly]
        public double companystandard { get; set; }
        /// <summary>
        /// 最低标准
        /// </summary>
        [SQLReadOnly]
        public double lowestprice { get; set; }

        /// <summary>
        /// 报告Id
        /// </summary>
        [SQLReadOnly]
        public long reportid { get; set; }

        /// <summary>
        /// 报告编号
        /// </summary>
        [SQLReadOnly]
        public string reportno { get; set; }
    }
}
