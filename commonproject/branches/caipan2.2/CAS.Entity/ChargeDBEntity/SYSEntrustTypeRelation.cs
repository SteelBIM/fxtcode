using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_EntrustTypeRelation")]
    public class SYSEntrustTypeRelation : BaseTO
    {
        private int _entrusttyperelationid;
        [SQLField("entrusttyperelationid", EnumDBFieldUsage.PrimaryKey, true)]
        public int entrusttyperelationid
        {
            get { return _entrusttyperelationid; }
            set { _entrusttyperelationid = value; }
        }
        private int _customchargeid;
        /// <summary>
        /// 自定义收费标准类型Id
        /// </summary>
        public int customchargeid
        {
            get { return _customchargeid; }
            set { _customchargeid = value; }
        }
        private string _chargetypecode;
        /// <summary>
        /// 评估收费类型编码
        /// </summary>
        public string chargetypecode
        {
            get { return _chargetypecode; }
            set { _chargetypecode = value; }
        }
    }
}