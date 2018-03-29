using System;
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
        /// 估价师手机号码
        /// </summary>
        [SQLReadOnly]
        public string appraisersmobile { get; set; }
        /// <summary>
        /// 报告使用方
        /// </summary>
        [SQLReadOnly]
        public string reportuserfullname { get; set; }
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
        /// 报告子类型名
        /// </summary>
        [SQLReadOnly]
        public string reportsubtypename { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        [SQLReadOnly]
        public string biztypecodename { get; set; }
        /// <summary>
        /// 业务跟进人姓名
        /// </summary>
        [SQLReadOnly]
        public string businessfollowername { get; set; }
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
        /// 收费状态
        /// </summary>
        [SQLReadOnly]
        public string sfstatusname { get; set; }
        /// <summary>
        /// 收费状态-审批中
        /// </summary>
        [SQLReadOnly]
        public int isapprovaliding { get; set; }
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
        /// 客户经理/业务员
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
        [SQLReadOnly]
        public string salemanname { get; set; }
       
        /// <summary>
        /// 错误类型
        /// </summary>
        [SQLReadOnly]
        public string errordescript { get; set; }

        /// <summary>
        /// 错误数量
        /// </summary>
        [SQLReadOnly]
        public int errorcount { get; set; } 

        /// <summary>
        /// 委托备注
        /// </summary>
        [SQLReadOnly]
        public string entrust_remark { get; set; }
        /// <summary>
        /// 业务撤销人
        /// </summary>
        [SQLReadOnly]
        public string cancelentrustuser { get; set; }
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
        /// 查勘人
        /// </summary>
        [SQLReadOnly]
        public string surveyuser { get; set; }
        /// <summary>
        /// 业务联系人
        /// </summary>
        [SQLReadOnly]
        public string bankcontact { get; set; }

        /// <summary>
        /// 预评编号
        /// </summary>
        [SQLReadOnly]
        public string YPNumber { get; set; }

        /// <summary>
        /// 业务联系人
        /// </summary>
        [SQLReadOnly]
        public string bankusername { get; set; }
        /// <summary>
        /// 业务联系人电话号码
        /// </summary>
        [SQLReadOnly]
        public string bankuserphone { get; set; }
        /// <summary>
        /// 委托客户
        /// </summary>
        [SQLReadOnly]
        public string bankname { get; set; }
        /// <summary>
        /// 预评ID Alex ADD by 2015-08-11
        /// </summary>
        [SQLReadOnly]
        public long ypid { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 业务员电话
        /// </summary>
        public string mobile { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 评估目的
        /// </summary>
        public string assesstype { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 委托人
        /// </summary>
        public string clientname { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 归档人
        /// </summary>
        public string backupuser { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 归档资料负责人
        /// </summary>
        public string backupdatausername { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 委估对象个数
        /// </summary>
        public int objnumber { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 委估对名称数组
        /// </summary>
        public string objnamearray { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 提醒时间
        /// </summary>
        public int remindtime { get; set; }
        /// <summary>
        /// 委托类型
        /// </summary>
        [SQLReadOnly]
        public int biztype { get; set; }
        [SQLReadOnly]
        public string biztypename { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 匹配相应规则的提醒时间(数据列表显示进度条的关联数据)
        /// </summary>
        public int ruleremindtime { get; set; }
        [SQLReadOnly]
        public long eid { get; set; }
        /// <summary>
        /// 预评撰写人
        /// </summary>
        [SQLReadOnly]
        public string ypwriterusername { get; set; }
        /// <summary>
        /// 预评状态码
        /// </summary>
        [SQLReadOnly]
        public int? ypstate { get; set; }
        /// <summary>
        /// 预评时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? ypdate { get; set; }
        /// <summary>
        /// 报告撰写辅助人
        /// </summary>
        [SQLReadOnly]
        public string assistedwriternames { get; set; }
        /// <summary>
        /// 报告收费时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? chargeovertime { get; set; }
        /// <summary>
        /// 报告归档时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? reportbackupdate { get; set; }

        /// <summary>
        /// 环节 Alex 2016-04-21
        /// </summary>
        [SQLReadOnly]
        public string detailstatus { get; set; }
        /// <summary>
        /// 当前处理人 Alex 2016-04-21
        /// </summary>
        [SQLReadOnly]
        public string currenthandleruser { get; set; }
        /// <summary>
        /// 权利人
        /// </summary>
        [SQLReadOnly]
        public string owner { get; set; }

        /// <summary>
        /// 冗余字段，报告打印人
        /// </summary>
        [SQLReadOnly]
        public string rname_reportprintuser { get; set; }
        /// <summary>
        /// 冗余字段，报告盖章人
        /// </summary>
        [SQLReadOnly]
        public string rname_reportsealuser { get; set; }
        /// <summary>
        /// 技术团队
        /// </summary>
        [SQLReadOnly]
        public int businesstypeid { get; set; }
        /// <summary>
        /// 完成状态 统计明细中使用 Alex 2016-09-27
        /// </summary>
        [SQLReadOnly]
        public string complatestatus { get; set; }
        /// <summary>
        /// 报告盖章状态 Alex 2016-11-17
        /// </summary>
        [SQLReadOnly]
        public string reportsealstatusname { get; set; }
        /// <summary>
        /// 撰写人的电子签名
        /// </summary>
        [SQLReadOnly]
        public string writersign { get; set; }

    }
}
