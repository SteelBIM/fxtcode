using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ReportBackUp : DatReportBackUp
    {
        ///// <summary>
        ///// 报告Id
        ///// </summary>
        //[SQLReadOnly]
        //public long reportid { get; set; }
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
        /// <summary>
        /// 归档资料负责人
        /// </summary>
        public int backupdatauser { get; set; }
        /// <summary>
        /// 归档资料负责人
        /// </summary>
        public string backupdatausername { get; set; }

        /// <summary>
        /// 归档编号
        /// </summary>
        [SQLReadOnly]        
        public string BackUpNO { get; set; }
        /// <summary>
        /// 档案存放地点
        /// </summary>
        [SQLReadOnly]
        public string BackUpFilePlace { get; set; }

        /// <summary>
        /// 业务状态 Alex 2016-07-13
        /// </summary>
        [SQLReadOnly]
        public string entruststagename { get; set; }


        /// <summary>
        /// 报告撰写人  Alex 2016-07-13
        /// </summary>
        [SQLReadOnly]
        public int? writerid { get; set; }

        /// <summary>
        /// 报告状态 
        /// </summary>
        [SQLReadOnly]
        public int? reportstate { get; set; }
        /// <summary> 
        /// 业务员  Alex 2016-07-13
        /// </summary>
        [SQLReadOnly]
        public int? businessuserid { get; set; }
    
        /// <summary>
        /// 提交时间 Alex 2016-07-20
        /// </summary>
        [SQLReadOnly]
        public DateTime committime { get; set; }

        /// <summary>
        /// 报告类型  Alex 2016-08-16
        /// </summary>
        [SQLReadOnly]
        public string reporttypename { get; set; }

        /// <summary>
        /// 客户全称 Alex 2016-08-16
        /// </summary>
        [SQLReadOnly]
        public string customercompanyfullname { get; set; }
    }
}
