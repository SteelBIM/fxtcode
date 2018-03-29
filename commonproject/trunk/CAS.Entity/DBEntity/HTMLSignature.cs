using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.HTMLSignature")]
    public class HTMLSignature : BaseTO
    {
        private long _qid;
        public long qid
        {
            get { return _qid; }
            set { _qid = value; }
        }
        private string _signatureid;
        [SQLField("signatureid", EnumDBFieldUsage.PrimaryKey)]
        public string signatureid
        {
            get { return _signatureid; }
            set { _signatureid = value; }
        }
        private string _signature;
        public string signature
        {
            get { return _signature; }
            set { _signature = value; }
        }
        private int _querysheettype;
        /// <summary>
        /// 询价单类型
        /// </summary>
        public int querysheettype
        {
            get { return _querysheettype; }
            set { _querysheettype = value; }
        }
    }
}