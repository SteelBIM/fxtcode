using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ReportBackUp : DatReportBackUp
    {
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
        /// <summary>
        /// 报告名称
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }

        /// <summary>
        /// 报告类型code
        /// 房地产、土地、资产
        /// </summary>
        public int reporttypecode { get; set; }

        /// <summary>
        /// 归档人
        /// </summary>
        public string backusername { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        [SQLReadOnly]
        public long entrustid { get; set; }

        /// <summary>
        /// 分支机构
        /// </summary>
        [SQLReadOnly]
        public int subcompanyid { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        [SQLReadOnly]
        public int approvalstatus { get; set; }
    }
}
