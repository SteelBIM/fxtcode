using CAS.Entity.BaseDAModels;
using CAS.Entity.DBEntity;
using System;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ReportDeliver : DatReportDeliver
    {
        [SQLReadOnly]
        public long entrustid { get; set; }//业务编号

        [SQLReadOnly]
        public long deliverid { get; set; }//投递记录编号

        [SQLReadOnly]
        public long reportid { get; set; }//报告id

        [SQLReadOnly]
        public string reportno { get; set; }//报告编号

        [SQLReadOnly]
        public int reporttype { get; set; }//???报告类型

        [SQLReadOnly]
        public string projectname { get; set; }//项目名称

        [SQLReadOnly]
        public string delivermodename { get; set; }

        [SQLReadOnly]
        public string reporttypename { get; set; }//报告类型

        [SQLReadOnly]
        public string customercompanyfullname { get; set; }//委托客户

        [SQLReadOnly]
        public long businessuserid { get; set; }//业务员id

        [SQLReadOnly]
        public string ypnumber { get; set; }//预评编号

        [SQLReadOnly]
        public DateTime completedate { get; set; }//报告完成时间

        [SQLReadOnly]
        public DateTime ypdate { get; set; }//预评完成时间

        [SQLReadOnly]
        public string subcompanyname { get; set; }//分支机构

        [SQLReadOnly]
        public long id { get; set; }//预评id

        [SQLReadOnly]
        public string businessusername { get; set; }//业务员姓名

        [SQLReadOnly]
        public long enid { get; set; }

        [SQLReadOnly]
        public string biztypename { get; set; }

        [SQLReadOnly]
        public int totaldelivercount { get; set; }//投递的总份数

        [SQLReadOnly]
        public int Countentrustid { get; set; }//投递的次数

        [SQLReadOnly]
        public string isReceiveUser { get; set; }//签收人 用来判断是否签收
        [SQLReadOnly]
        public string path { get; set; }//路径

        [SQLReadOnly]
        public long reportstate { get; set; }//报告完成状态

        [SQLReadOnly]
        public long statecode { get; set; }//预评完成状态

        /// <summary>
        /// 预评退还份数
        /// </summary>
        [SQLReadOnly]
        public int? ypdeliverreturncount { get; set; }
        /// <summary>
        /// 报告退还份数
        /// </summary>
        [SQLReadOnly]
        public int? reportdeliverreturncount { get; set; }
    }
}
