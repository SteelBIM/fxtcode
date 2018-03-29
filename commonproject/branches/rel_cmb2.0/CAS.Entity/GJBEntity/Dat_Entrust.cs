using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Entrust : DatEntrust
    {
        /// <summary>
        /// 分支机构名称
        /// </summary>
        [SQLReadOnly]
        public string subcompanyname { get; set; }

        /// <summary>
        /// 委托类型名称
        /// </summary>
        [SQLReadOnly]
        public string biztypename { get; set; }

        /// <summary>
        /// 登记人姓名
        /// </summary>
        [SQLReadOnly]
        public string createusername { get; set; }

        [SQLReadOnly]
        public string statecodename { get; set; }

        //[SQLReadOnly]
        //public int provinceid { get; set; }

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
        /// 估价师
        /// </summary>
        [SQLReadOnly]
        public string appraisersusername { get; set; }

        /// <summary>
        /// 业务员姓名
        /// </summary>
        [SQLReadOnly]
        public string businessusername { get; set; }

        /// <summary>
        /// 由于js会把最后一位显示为0或者显示错误，所以直接按字符串输出
        /// </summary>
        [SQLReadOnly]
        public string entrustid_str { get { return entrustid.ToString(); } }

        /// <summary>
        /// 委托对象
        /// </summary>
        [SQLReadOnly]
        public string projectfullname { get; set; }

        [SQLReadOnly]
        public string reporttypename { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [SQLReadOnly]
        public string querytypename { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SQLReadOnly]
        public string address { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        [SQLReadOnly]
        public decimal buildingarea { get; set; }

        /// <summary>
        /// 权利人性质
        /// </summary>
        [SQLReadOnly]
        public string ownertypename { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string salemanname { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [SQLReadOnly]
        public string contactuser { get; set; }

        /// <summary>
        /// 预评状态
        /// </summary>
        [SQLReadOnly]
        public int ypstatecode { get; set; }

        /// <summary>
        /// 报告状态
        /// </summary>
        [SQLReadOnly]
        public int reportstate { get; set; }

        /// <summary>
        /// 预评撰写人
        /// </summary>
        [SQLReadOnly]
        public string ypwriter { get; set; }

        /// <summary>
        /// 报告撰写人
        /// </summary>
        [SQLReadOnly]
        public string reportwriter { get; set; }

        /// <summary>
        /// 预评节点
        /// </summary>
        [SQLReadOnly]
        public string ypjiedian { get; set; }

        /// <summary>
        /// 报告节点
        /// </summary>
        [SQLReadOnly]
        public string reportjiedian { get; set; }

        /// <summary>
        /// 预评审批人
        /// </summary>
        [SQLReadOnly]
        public string ypshenpiuser { get; set; }
        /// <summary>
        /// 报告审批人
        /// </summary>
        [SQLReadOnly]
        public string reportshenpiuser { get; set; }
        /// <summary>
        /// 转交状态(报告)
        /// </summary>
        [SQLReadOnly]
        public int rassignstate { get; set; }
        /// <summary>
        /// 转出人  谁转的(报告)
        /// </summary>
        [SQLReadOnly]
        public string rassignoutperson { get; set; }
        /// <summary>
        /// 转入人 转给谁(报告)
        /// </summary>
        [SQLReadOnly]
        public string rassigninperson { get; set; }

        /// <summary>
        /// 转交状态(预评)
        /// </summary>
        [SQLReadOnly]
        public int yassignstate { get; set; }
        /// <summary>
        /// 转出人  谁转的(预评)
        /// </summary>
        [SQLReadOnly]
        public string yassignoutperson { get; set; }
        /// <summary>
        /// 转入人 转给谁(预评)
        /// </summary>
        [SQLReadOnly]
        public string yassigninperson { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 主体预评、报告是否操作"修改报告"
        /// </summary>
        public bool? changedreport { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 预评撰写人
        /// </summary>
        public int? ypwriteruserid { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 报告撰写人
        /// </summary>
        public int? writerid { get; set; }

        /// <summary>
        /// 权利人
        /// </summary>
        [SQLReadOnly]
        public string owner { get; set; }

        /// <summary>
        /// 查勘状态名
        /// </summary>
        [SQLReadOnly]
        public string surveystatename { get; set; }
        /// <summary>
        ///业务类型 预评或者报告 
        /// </summary>
        [SQLReadOnly]
        public int businesstype { get; set; }
    }

    [Serializable]
    /// <summary>
    /// 业务统计条目相关的变量
    /// </summary>
    public class DatEntrustTongJi : BaseTO
    {
        public int daifenpei { get; set; }
    }
}


