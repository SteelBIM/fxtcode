using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Entrust_ExtDSF")]
    public class DatEntrustExtDSF : BaseTO
    {
        private long _etid;
        [SQLField("etid", EnumDBFieldUsage.PrimaryKey, true)]
        public long etid
        {
            get { return _etid; }
            set { _etid = value; }
        }
        private long _entrustid;
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private string _externalstatetext;
        /// <summary>
        /// 外部状态
        /// </summary>
        public string externalstatetext
        {
            get { return _externalstatetext; }
            set { _externalstatetext = value; }
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
        private DateTime _changedon;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime changedon
        {
            get { return _changedon; }
            set { _changedon = value; }
        }
    }
}