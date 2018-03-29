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
        [SQLReadOnly]
        public string appraisersmobile { get; set; }
        [SQLReadOnly]
        public string appraisersusername1 { get; set; }
        [SQLReadOnly]
        public string appraisersusername2 { get; set; }

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
        /// <summary>
        /// 转入人
        /// </summary>
        [SQLReadOnly]
        public string assigninperson { get; set; }
        /// <summary>
        /// 转出人
        /// </summary>
        [SQLReadOnly]
        public string assignoutperson { get; set; }
        /// <summary>
        /// 预评转交状态（显示值）
        /// </summary>
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
        /// <summary>
        /// 预评撰写人
        /// </summary>
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

        /// <summary>
        /// 报告撰写人
        /// </summary>
        [SQLReadOnly]
        public string reportwriteusername { get; set; }
        /// <summary>
        /// 报告状态(显示值)
        /// </summary>
        [SQLReadOnly]
        public string reportstate { get; set; }
        /// <summary>
        /// 报告状态Code
        /// </summary>
        [SQLReadOnly]
        public int reportstatecode { get; set; }
        /// <summary>
        /// 报告审批节点
        /// </summary>
        [SQLReadOnly]
        public string reportjiedianname { get; set; }
        /// <summary>
        /// 报告审批人列表
        /// </summary>
        [SQLReadOnly]
        public string reportshengyushenpitruenamelist { get; set; }
        /// <summary>
        /// 报告分配人
        /// </summary>
        [SQLReadOnly]
        public string reportassignusername { get; set; }
        /// <summary>
        /// 报告转交状态
        /// </summary>
        [SQLReadOnly]
        public int reportassignstate { get; set; }
        /// <summary>
        /// 报告转交状态（显示值）
        /// </summary>
        [SQLReadOnly]
        public string reportassignstatename { get; set; }

        /// <summary>
        /// 业务撤销人
        /// </summary>
        [SQLReadOnly]
        public string cancelentrustuser { get; set; }

        /// <summary>
        /// 报告主键
        /// </summary>
        [SQLReadOnly]
        public long reportid { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        [SQLReadOnly]
        public string reportno { get; set; }
        /// <summary>
        /// 查勘状态
        /// </summary>
        [SQLReadOnly]
        public int surveystate { get; set; }
        /// <summary>
        /// 查勘状态
        /// </summary>
        [SQLReadOnly]
        public string surveystatename { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        [SQLReadOnly]
        public int reporttypecode { get; set; }
        /// <summary>
        /// 委托类型
        /// </summary>
        [SQLReadOnly]
        public int biztype { get; set; }
        /// <summary>
        /// 银行联系人
        /// </summary>
        [SQLReadOnly]
        public string bankcontact { get; set; }
        /// <summary>
        /// 业务员电话
        /// </summary>
        public string mobile { get; set; }
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
