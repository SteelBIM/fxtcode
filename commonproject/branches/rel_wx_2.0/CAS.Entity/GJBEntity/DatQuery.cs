using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Query : DatQuery
    {
        [SQLReadOnly]
        public string cityname { get; set; }

        [SQLReadOnly]
        public int provinceid { get; set; }

        [SQLReadOnly]
        public string salemanname { get; set; }

        [SQLReadOnly]
        public string createusername { get; set; }

        [SQLReadOnly]
        public string appraisersusername { get; set; }

        [SQLReadOnly]
        public string statecodename { get; set; }

        [SQLReadOnly]
        public string purposename { get; set; }

        [SQLReadOnly]
        public string querytypename { get; set; }

        [SQLReadOnly]
        public long reportid { get; set; }
        [SQLReadOnly]
        public string reportno { get; set; }
        [SQLReadOnly]
        public long ypid { get; set; }
        [SQLReadOnly]
        public string ypno { get; set; }

        [SQLReadOnly]
        public string workflowname { get; set; }

        [SQLReadOnly]
        public decimal buildingarea { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SQLReadOnly]
        public string address { get; set; }

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
        /// 区域
        /// </summary>
        [SQLReadOnly]
        public string areaname { get; set; }

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
        /// 权利人性质
        /// </summary>
        [SQLReadOnly]
        public string ownertypename { get; set; }
        [SQLReadOnly]
        public string shenpiuserlist { get; set; }
        [SQLReadOnly]
        public string okuserlist { get; set; }
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
        /// 剩余的待审批用户姓名
        /// </summary>
        [SQLReadOnly]
        public string shengyushenpitruenamelist{get;set;}
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
