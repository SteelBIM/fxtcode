using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class Dat_EntrustOver : DatEntrustOver
    {
        /// <summary>
        ///业务员姓名
        /// </summary>
        public string salesmanname
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string createrName
        {
            get;
            set;
        }
        /// <summary>
        /// 报告编号/预评编号
        /// </summary>
        [SQLReadOnly]
        public string reportno
        {
            get;
            set;
        }
         /// <summary>
        /// 业务阶段
        /// </summary>
        [SQLReadOnly]
        public string yewujieduan
        {
            get;
            set;
        }
        /// <summary>
        /// 委托类型
        /// </summary>
        [SQLReadOnly]
        public int biztype
        {
            get;
            set;
        }
        /// <summary>
        /// 报告类型
        /// </summary>
        [SQLReadOnly]
        public string reporttypename
        {
            get;
            set;
        }
        /// <summary>
        /// 提成环节
        /// </summary>
        [SQLReadOnly]
        public string tichenghuanjie
        {
            get;
            set;
        }
        /// <summary>
        /// 评估目的
        /// </summary>
        [SQLReadOnly]
        public string assesstype
        {
            get;
            set;
        }
        /// <summary>
        /// 评估总值
        /// </summary>
        [SQLReadOnly]
        public decimal querytotalprice { get; set; }
        /// <summary>
        /// 实收
        /// </summary>
        [SQLReadOnly]
        public decimal realityreceive { get; set; }
        /// <summary>
        /// 退费
        /// </summary>
        [SQLReadOnly]
        public decimal refundamount { get; set; }
        /// <summary>
        /// 返利
        /// </summary>
        [SQLReadOnly]
        public decimal fanli { get; set; }
        /// <summary>
        /// 业务创建时间 Alex 2016-09-27
        /// </summary>
        [SQLReadOnly]
        public DateTime entrustcreatetime { get; set; }

    }
}
