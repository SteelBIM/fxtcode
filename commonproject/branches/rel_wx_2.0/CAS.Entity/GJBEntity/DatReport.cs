using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Report : DatReport
    {
        /// <summary>
        /// 估价师
        /// </summary>
        [SQLReadOnly]
        public string appraisersuser { get; set; }

        /// <summary>
        /// 客户经理
        /// </summary>
        [SQLReadOnly]
        public string businessuser { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }

        /// <summary>
        /// 分支机构
        /// </summary>
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        [SQLReadOnly]
        public int reporttypecode { get; set; }

        /// <summary>
        /// 报告类型名
        /// </summary>
        [SQLReadOnly]
        public string reporttypename { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [SQLReadOnly]
        public string biztypecodename { get; set; }

        /// <summary>
        /// 报告份数
        /// </summary>
        [SQLReadOnly]
        public int count { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SQLReadOnly]
        public string createusername { get; set; }

        /// <summary>
        /// 报告撰写人
        /// </summary>
        [SQLReadOnly]
        public string writer { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [SQLReadOnly]
        public int provinceid { get; set; }

        /// <summary>
        /// 估价师
        /// </summary>
        [SQLReadOnly]
        public string appraisersusername { get; set; }
        [SQLReadOnly]
        public string appraisersusername1 { get; set; }
        [SQLReadOnly]
        public string appraisersusername2 { get; set; }
        /// <summary>
        /// 报告撰写人
        /// </summary>
        [SQLReadOnly]
        public string writename { get; set; }
        /// <summary>
        /// 客户经理
        /// </summary>
        [SQLReadOnly]
        public string businessusername { get; set; }

        /// <summary>
        /// 报告状态
        /// </summary>
        [SQLReadOnly]
        public string reportstatename { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        [SQLReadOnly]
        public string submitusername { get; set; }
        /// <summary>
        /// 分配人
        /// </summary>
        [SQLReadOnly]
        public string assignusername { get; set; }
        /// <summary>
        /// 打印人
        /// </summary>
        [SQLReadOnly]
        public string printusername { get; set; }

        /// <summary>
        /// 由于js会把最后一位显示为0或者显示错误，所以直接按字符串输出
        /// </summary>
        [SQLReadOnly]
        public string entrustid_str { get { return entrustid.ToString(); } }

        [SQLReadOnly]
        public string workflowname { get; set; }

        [SQLReadOnly]
        public int workflowid { get; set; }

        [SQLReadOnly]
        public string customercompanyfullname { get; set; }
        [SQLReadOnly]
        public string assignoutperson { get; set; }

        [SQLReadOnly]
        public string assigninperson { get; set; }
        [SQLReadOnly]
        public string assignstatename { get; set; }
        [SQLReadOnly]
        public string shenpiuserlist { get; set; }
        [SQLReadOnly]
        public string okuserlist { get; set; }
        [SQLReadOnly]
        public string shengyushenpitruenamelist { get; set; }
        [SQLReadOnly]
        public int formid { get; set; }
        [SQLReadOnly]
        public int jiedianid { get; set; }
        [SQLReadOnly]
        public string jiedianname { get; set; }
        [SQLReadOnly]
        public int isfromapprovallist { get; set; }
        [SQLReadOnly]
        public int allowedapply { get; set; }
        /// <summary>
        /// 委托备注
        /// </summary>
        [SQLReadOnly]
        public string entrust_remark { get; set; }
    }
}
