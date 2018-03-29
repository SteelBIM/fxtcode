using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_CustomApproveConditionStandard")]
    public class SYSCustomApproveConditionStandard : BaseTO
    {
        private int _customapproveid;
        [SQLField("customapproveid", EnumDBFieldUsage.PrimaryKey, true)]
        public int customapproveid
        {
            get { return _customapproveid; }
            set { _customapproveid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司id
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _idorcodetype;
        /// <summary>
        /// 编码类型
        /// </summary>
        public int idorcodetype
        {
            get { return _idorcodetype; }
            set { _idorcodetype = value; }
        }

        private int _chargesettingidorcode;
        /// <summary>
        /// 国家标准/自定义标准
        /// </summary>
        public int chargesettingidorcode
        {
            get { return _chargesettingidorcode; }
            set { _chargesettingidorcode = value; }
        }
        private decimal _privilegerange;
        /// <summary>
        /// 优惠幅度
        /// </summary>
        public decimal privilegerange
        {
            get { return _privilegerange; }
            set { _privilegerange = value; }
        }
    }
}