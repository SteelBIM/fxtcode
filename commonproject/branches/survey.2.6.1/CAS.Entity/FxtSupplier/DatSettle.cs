using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.Dat_Settle")]
    public class DatSettle : BaseTO
    {
        private int _settleid;
        /// <summary>
        /// 账单Id
        /// </summary>
        [SQLField("settleid", EnumDBFieldUsage.PrimaryKey, true)]
        public int settleid
        {
            get { return _settleid; }
            set { _settleid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _clientcompanyid = 25;
        /// <summary>
        /// 委托方ID：没第三方委托方介入前，默认为25：房讯通ID
        /// </summary>
        public int clientcompanyid
        {
            get { return _clientcompanyid; }
            set { _clientcompanyid = value; }
        }
        private int _bill;
        /// <summary>
        /// 账单：201110，月末出账单
        /// </summary>
        public int bill
        {
            get { return _bill; }
            set { _bill = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private decimal? _money;
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? money
        {
            get { return _money; }
            set { _money = value; }
        }
        private DateTime? _suredate;
        /// <summary>
        /// 确定日期
        /// </summary>
        public DateTime? suredate
        {
            get { return _suredate; }
            set { _suredate = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _billstate;
        /// <summary>
        /// 账单状态:0--驳回；1--结算；2--未结算
        /// </summary>
        public int? billstate
        {
            get { return _billstate; }
            set { _billstate = value; }
        }
        private int? _businessnumber;
        /// <summary>
        /// 业务数量
        /// </summary>
        public int? businessnumber
        {
            get { return _businessnumber; }
            set { _businessnumber = value; }
        }
        private bool _valid = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }

}
