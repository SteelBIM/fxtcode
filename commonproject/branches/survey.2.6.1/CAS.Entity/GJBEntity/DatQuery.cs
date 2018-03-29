using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Query : DatQuery
    {
        /// <summary>
        /// 省份
        /// </summary>
        [SQLReadOnly]
        public int provinceid { get; set; }
        /// <summary>
        /// 询价分配人
        /// </summary>
        [SQLReadOnly]
        public string queryassignuser { get; set; }
        /// <summary>
        /// 估价师手机号码
        /// </summary>
        [SQLReadOnly]
        public string appraisersmobile { get; set; }

        [SQLReadOnly]
        public string purposename { get; set; }
        /// <summary>
        /// 业务撤销人
        /// </summary>
        [SQLReadOnly]
        public string CancelEntrustUser { get; set; }
        /// <summary>
        /// 权利人性质
        /// </summary>
        [SQLReadOnly]
        public string ownertypename { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        [SQLReadOnly]
        public string buildingarea { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SQLReadOnly]
        public string address { get; set; }
        /// <summary>
        /// 楼盘Id
        /// </summary>
        [SQLReadOnly]
        public int projectid { get; set; }
        /// <summary>
        /// 楼盘名
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        [SQLReadOnly]
        public string name { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        [SQLReadOnly]
        public string path { get; set; }


        /// <summary>
        /// 报告id
        /// </summary>
        [SQLReadOnly]
        public long reportid { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        [SQLReadOnly]
        public string reportno { get; set; }
        /// <summary>
        /// 报告状态名称
        /// </summary>
        [SQLReadOnly]
        public string reportstatename { get; set; }
        /// <summary>
        /// 报告状态
        /// </summary>
        [SQLReadOnly]
        public int reportstatecode { get; set; }

        /// <summary>
        /// 报告审批节点名称
        /// </summary>
        [SQLReadOnly]
        public string reportjiedianname { get; set; }
        /// <summary>
        /// 报告审批人
        /// </summary>
        [SQLReadOnly]
        public string reportshenpiuserlist { get; set; }
        /// <summary>
        /// 报告待确认状态
        /// </summary>
        [SQLReadOnly]
        public int rassignstate { get; set; }
        /// <summary>
        /// 报告待确认用户
        /// </summary>
        [SQLReadOnly]
        public string rassigninpersion { get; set; }
        /// <summary>
        /// 报告撰写人
        /// </summary>
        [SQLReadOnly]
        public string rwriteuser { get; set; }
        /// <summary>
        /// 预评撰写人
        /// </summary>
        [SQLReadOnly]
        public string ywriteuser { get; set; }
        /// <summary>
        /// 预评id
        /// </summary>
        [SQLReadOnly]
        public long ypid { get; set; }
        /// <summary>
        /// 预评编号
        /// </summary>
        [SQLReadOnly]
        public string ypno { get; set; }
        /// <summary>
        /// 预评状态名称
        /// </summary>
        [SQLReadOnly]
        public string ypstatename { get; set; }
        /// <summary>
        /// 预评状态code
        /// </summary>
        [SQLReadOnly]
        public int ypstatecode { get; set; }
        /// <summary>
        /// 预评审批节点
        /// </summary>
        [SQLReadOnly]
        public string ypjiedianname { get; set; }
        /// <summary>
        /// 预评审批处理人
        /// </summary>
        [SQLReadOnly]
        public string ypshenpiuserlist { get; set; }
        /// <summary>
        /// 预评待确认状态
        /// </summary>
        [SQLReadOnly]
        public int yassignstate { get; set; }
        /// <summary>
        /// 预评待确认用户
        /// </summary>
        [SQLReadOnly]
        public string yassigninpersion { get; set; }
        /// <summary>
        /// 已委托出报告
        /// </summary>
        [SQLReadOnly]
        public bool entrustreport { get; set; }
        /// <summary>
        /// 已委托出预估
        /// </summary>
        [SQLReadOnly]
        public bool entrustyp { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        [SQLReadOnly]
        public string entruststatename { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        [SQLReadOnly]
        public int entruststatecode { get; set; }
        /// <summary>
        /// 业务分配人
        /// </summary>
        [SQLReadOnly]
        public string entrustassignuser { get; set; }
        /// <summary>
        /// 工作流名称
        /// </summary>
        [SQLReadOnly]
        public string workflowname { get; set; }
        /// <summary>
        /// 价格纠错流程名
        /// </summary>
        [SQLReadOnly]
        public string adjustflowname { get; set; }

        /// <summary>
        /// 价格纠错流程Id
        /// </summary>
        [SQLReadOnly]
        public int priceworkflowid { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        [SQLReadOnly]
        public string shenpiuserlist { get; set; }
        /// <summary>
        /// 已审批用户
        /// </summary>
        [SQLReadOnly]
        public string okuserlist { get; set; }

        [SQLReadOnly]
        public int formid { get; set; }
        /// <summary>
        /// 节点id
        /// </summary>
        [SQLReadOnly]
        public int jiedianid { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        [SQLReadOnly]
        public string jiedianname { get; set; }
        /// <summary>
        /// 是否来自审批
        /// </summary>
        [SQLReadOnly]
        public int isfromapprovallist { get; set; }
        /// <summary>
        /// 是否允许办理
        /// </summary>
        [SQLReadOnly]
        public int allowedapply { get; set; }
        /// <summary>
        /// 剩余的待审批用户姓名
        /// </summary>
        [SQLReadOnly]
        public string shengyushenpitruenamelist { get; set; }

        /// <summary>
        /// 权利人 
        /// </summary>
        [SQLReadOnly]
        public string owner { get; set; }

        private decimal _yptotalprice = 0.00M;
        /// <summary>
        /// 预评评估总值
        /// </summary>
        [SQLReadOnly]
        public decimal yptotalprice
        {
            get { return _yptotalprice; }
            set { _yptotalprice = value; }
        }

        private decimal _reporttotalprice = 0.00M;
        /// <summary>
        /// 预评评估总值
        /// </summary>
        [SQLReadOnly]
        public decimal reporttotalprice
        {
            get { return _reporttotalprice; }
            set { _reporttotalprice = value; }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        [SQLReadOnly]
        public string entrustprojectname { get; set; }

        /// <summary>
        /// 评估总价
        /// </summary>
        [SQLReadOnly]
        public decimal? landtotalprice { get; set; }

        /// <summary>
        /// 附属房屋总价
        /// </summary>
        [SQLReadOnly]
        public decimal? subhousealltotalprice { get; set; }

        /// <summary>
        /// 评估总价
        /// <summary>
        [SQLReadOnly]
        public decimal? housetotalprice { get; set; }
        /// <summary>
        /// 正常估价单价
        /// <summary>
        [SQLReadOnly]
        public decimal? normalprice { get; set; }
        /// <summary>
        /// 业务员电话
        /// </summary>
        [SQLReadOnly]
        public string mobile { get; set; }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        [SQLReadOnly]
        public string buildingname { get; set; }
        /// <summary>
        /// 房号名称
        /// </summary>
        [SQLReadOnly]
        public string housename { get; set; }
        
        
        /// <summary>
        /// 业务类型
        /// </summary>
        [SQLReadOnly]
        public string righttype { get; set; }
        /// <summary>
        /// 宗地名称
        /// </summary>
        [SQLReadOnly]
        public string fieldname { get; set; }
        /// <summary>
        /// 宗地号
        /// </summary>
        [SQLReadOnly]
        public string fieldno { get; set; }
         /// <summary>
        /// 房产证号
        /// </summary>
        [SQLReadOnly]
        public string housecertno { get; set; }
        /// <summary>
        /// 总楼层
        /// </summary>
        [SQLReadOnly]
        public string totalfloor { get; set; }
        /// <summary>
        /// 土地证发证日期
        /// </summary>
        [SQLReadOnly]
        public DateTime? landregisterdata { get; set; }

        /// <summary>
        /// 房产证登记日期
        /// </summary>
        [SQLReadOnly]
        public DateTime? housecertdate { get; set; }

        /// <summary>
        /// 土地面积
        /// </summary>
        [SQLReadOnly]
        public string landarea { get; set; }
        /// <summary>
        /// 查勘ID
        /// </summary>
        [SQLReadOnly]
        public long sid { get; set; }
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
        /// 最后一次打开的用户姓名
        /// </summary>
        [SQLReadOnly]
        public string lastopentruename { get; set; }
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        /// <summary>
        /// 盖章状态
        /// </summary>
        [SQLReadOnly]
        public string stampstatename { get; set; }
        /// <summary>
        /// 盖章人
        /// </summary>
        [SQLReadOnly]
        public string stampusername { get; set; }
        /// <summary>
        /// 询价单类型
        /// </summary>
        [SQLReadOnly]
        public int querysheettype { get; set; }
        /// <summary>
        /// 询价单类型
        /// </summary>
        [SQLReadOnly]
        public string querysheettypename { get; set; }
        /// <summary>
        /// 建筑年代
        /// </summary>
        [SQLReadOnly]
        public string buildingdate { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        [SQLReadOnly]
        public string housetypecode { get; set; }
        /// <summary>
        /// 对应的业务主键ID
        /// </summary>
        [SQLReadOnly]
        public long eid { get; set; }
    }

    [Serializable]
    /// <summary>
    /// 用于询价Tab各项统计需要的变量
    /// </summary>
    public class QueryTabTongJi : BaseTO
    {
        /// <summary>
        /// 待我回价
        /// </summary>
        public int daihuijia { get; set; }
        /// <summary>
        /// 待我分配
        /// </summary>
        public int daifenpei { get; set; }
        /// <summary>
        /// 待我审批
        /// </summary>
        public int daishenpi { get; set; }
    }
}
