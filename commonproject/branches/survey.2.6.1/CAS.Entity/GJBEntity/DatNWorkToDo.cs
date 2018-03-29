﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkToDo : DatNWorkToDo
    {
        [SQLReadOnly]
        public string formname { get; set; }
        [SQLReadOnly]
        public string workflowname { get; set; }
        [SQLReadOnly]
        public string nodename { get; set; }
        [SQLReadOnly]
        public string fullname { get; set; }
        [SQLReadOnly]
        public string shenpitruenamelist { get; set; }
        [SQLReadOnly]
        public string consignertruenamelist { get; set; }
        [SQLReadOnly]
        public string oktruenamelist { get; set; }
        [SQLReadOnly]
        public string readertruenamelist { get; set; }
        [SQLReadOnly]
        public int userid { get; set; }
        [SQLReadOnly]
        public string truename { get; set; }
        private List<Dat_NWorkStepLog> _steploglist = new List<Dat_NWorkStepLog>();
        [SQLReadOnly]
        public List<Dat_NWorkStepLog> steploglist
        {
            get
            {
                return _steploglist;
            }
            set
            {
                _steploglist = value;
            }
        }
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        [SQLReadOnly]
        public string pstype { get; set; }
        [SQLReadOnly]
        public string sptype { get; set; }
        [SQLReadOnly]
        public string reportno { get; set; }
        [SQLReadOnly]
        public string ypnumber { get; set; }
        [SQLReadOnly]
        public string ifcanedit { get; set; }
        [SQLReadOnly]
        public int? workflowtype { get; set; }
        [SQLReadOnly]
        public string wtremark { get; set; }
        /// <summary>
        /// 流程已操作的步骤是否包含已盖章
        /// </summary>
        [SQLReadOnly]
        public bool containsstampnode { get; set; }
        [SQLReadOnly]
        public string queryno { get; set; }
        /// <summary>
        /// 业务编号
        /// </summary>
        [SQLReadOnly]
        public long? entrustid{ get; set; }
        /// <summary>
        /// 客户全称
        /// </summary>
        [SQLReadOnly]
        public string customercompanyfullname { get; set; }
        /// <summary>
        /// 委托方
        /// </summary>
        [SQLReadOnly]
        public string clientname { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string businessusername { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        [SQLReadOnly]
        public string businesstypename { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        [SQLReadOnly]
        public string reporttypename { get; set; }
        /// <summary>
        /// 是否月结（收费审批专有字段）
        /// </summary>
        [SQLReadOnly]
        public bool? ischargemonthly { get; set; }
        /// <summary>
        /// 评估总价
        /// </summary>
        [SQLReadOnly]
        public decimal? querytotalprice { get; set; }
        /// <summary>
        /// 标准收费
        /// </summary>
        [SQLReadOnly]
        public decimal? totalamount { get; set; }
        /// <summary>
        /// 优惠折扣
        /// </summary>
        [SQLReadOnly]
        public decimal? privilegediscount { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        [SQLReadOnly]
        public decimal? receivable { get; set; }
        
        /// <summary>
        /// 预评撰写人
        /// </summary>
        [SQLReadOnly]
        public string ypwriterusername { get; set; }
        /// <summary>
        /// 报告撰写人
        /// </summary>
        [SQLReadOnly]
        public string reportwriteusername { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [SQLReadOnly]
        public string approvalname { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }

        /// <summary>
        /// 询价编号
        /// </summary>
        [SQLReadOnly]
        public string queryid { get; set; }
        
    }
}