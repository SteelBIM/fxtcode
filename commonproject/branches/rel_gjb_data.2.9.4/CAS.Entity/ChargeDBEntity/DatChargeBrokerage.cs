using System;
using CAS.Entity.BaseDAModels;

namespace GJB.Entity.DBEntity
{
    /// <summary>
    /// 收费结单时其他成员提成信息
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_ChargeBrokerage")]
    public class DatChargeBrokerage : BaseTO
    {
        private long _chargebrokerageid;
        [SQLField("chargebrokerageid", EnumDBFieldUsage.PrimaryKey, true)]
        public long chargebrokerageid
        {
            get { return _chargebrokerageid; }
            set { _chargebrokerageid = value; }
        }
        private int _entrustoverid;
        /// <summary>
        /// 结单ID，主要关联结单表
        /// </summary>
        public int entrustoverid
        {
            get { return _entrustoverid; }
            set { _entrustoverid = value; }
        }
        private int _chargeentrustid;
        /// <summary>
        /// 收费ID，主要便于查询
        /// </summary>
        public int chargeentrustid
        {
            get { return _chargeentrustid; }
            set { _chargeentrustid = value; }
        }
        private string _businesstypestage;
        /// <summary>
        /// 业务环节
        /// </summary>
        public string businesstypestage
        {
            get { return _businesstypestage; }
            set { _businesstypestage = value; }
        }
        private int _userid;
        /// <summary>
        /// 人员
        /// </summary>
        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private decimal _brokeragepercentage;
        /// <summary>
        /// 提出比例
        /// </summary>
        public decimal brokeragepercentage
        {
            get { return _brokeragepercentage; }
            set { _brokeragepercentage = value; }
        }
        private decimal _brokerage;
        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal brokerage
        {
            get { return _brokerage; }
            set { _brokerage = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        /// <summary>
        /// 是否为系统定义的
        /// </summary>
        private bool _issystem = false;
        public bool issystem
        {
            get { return _issystem; }
            set { _issystem = value; }
        }
        private int _joinchargeinoutrecordid = 0;
        /// <summary>
        /// 关联到收支ID
        /// </summary>
        public int joinchargeinoutrecordid
        {
            get { return _joinchargeinoutrecordid; }
            set { _joinchargeinoutrecordid = value; }
        }
        #region 扩展字段
        /// <summary>
        /// 姓名
        /// </summary>
        [SQLReadOnly]
        public string truename
        {
            get;
            set;
        }

        
        #endregion
    }

}
