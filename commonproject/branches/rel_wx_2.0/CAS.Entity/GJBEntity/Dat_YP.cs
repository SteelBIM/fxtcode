using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_YP : DatYP
    {
        [SQLReadOnly]
        public string createusername { get; set; }

        [SQLReadOnly]
        public string reporttype { get; set; }

        [SQLReadOnly]
        public string righttype { get; set; }

        [SQLReadOnly]
        public string subcompanyname { get; set; }

        [SQLReadOnly]
        public string needchargetext
        {
            get
            {
                return needcharge ? "是" : "否";
            }
        }

        [SQLReadOnly]
        public string appraisersusername { get; set; }

        /// <summary>
        /// 报告撰写人名称
        /// </summary>
        [SQLReadOnly]
        public string reportwriteusername { get; set; }

        [SQLReadOnly]
        public int provinceid { get; set; }
        /// <summary>
        /// 撤销人
        /// </summary>
        [SQLReadOnly]
        public string cancelusername { get; set; }
        /// <summary>
        /// 预评状态
        /// </summary>
        [SQLReadOnly]
        public string statename { get; set; }

        /// <summary>
        /// 由于js会把最后一位显示为0或者显示错误，所以直接按字符串输出
        /// </summary>
        [SQLReadOnly]
        public string entrustid_str { get { return entrustid.ToString(); } }

        [SQLReadOnly]
        public string projectname { get; set; }

        [SQLReadOnly]
        public string workflowname { get; set; }

        [SQLReadOnly]
        public int workflowid { get; set; }

        [SQLReadOnly]
        public string clientname { get; set; }

        [SQLReadOnly]
        public string reporttypename { get; set; }

        [SQLReadOnly]
        public string biztypename { get; set; }

        [SQLReadOnly]
        public string assigninperson { get; set; }

        [SQLReadOnly]
        public string assignoutperson { get; set; }

        [SQLReadOnly]
        public string assignstatename { get; set; }

        /// <summary>
        /// 当前所在的节点
        /// </summary>
        [SQLReadOnly]
        public string jiedianname { get; set; }
        /// <summary>
        /// 已审批通过的审批人
        /// </summary>
        [SQLReadOnly]
        public string okuserlist { get; set; }
        /// <summary>
        /// 当前需要审批的用户
        /// </summary>
        [SQLReadOnly]
        public string shenpiuserlist { get; set; }

        /// <summary>
        /// 节点id
        /// </summary>
        [SQLReadOnly]
        public int jiedianid { get; set; }

        /// <summary>
        /// 是否来自审批
        /// </summary>
        [SQLReadOnly]
        public int isfromapprovallist { get; set; }

        /// <summary>
        /// 是否允许审批
        /// </summary>
        [SQLReadOnly]
        public int allowedapply { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>
        [SQLReadOnly]
        public int formid { get; set; }

        [SQLReadOnly]
        public string ypwriterusername { get; set; }

        /// <summary>
        /// 委托备注
        /// </summary>
        [SQLReadOnly]
        public string entrust_remark { get; set; }
        /// <summary>
        /// 剩余的待审批用户姓名
        /// </summary>
        [SQLReadOnly]
        public string shengyushenpitruenamelist { get; set; }
    }

    [Serializable]
    /// <summary>
    /// 用于各项需要统计的tab待办数
    /// </summary>    
    public class TabBubble : BaseTO
    {
        /// <summary>
        /// 待我做
        /// </summary>
        public int daiwozuo { get; set; }
        /// <summary>
        /// 待我转交
        /// </summary>
        public int daizhuanjiao { get; set; }
        /// <summary>
        /// 待我审批
        /// </summary>
        public int daishenpi { get; set; }

        /// <summary>
        /// 待分配
        /// </summary>
        public int daifenpei { get; set; }
    }
}
