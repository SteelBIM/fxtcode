using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.HTMLHistory")]
    public class HTMLHistory : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _qid;
        public long qid
        {
            get { return _qid; }
            set { _qid = value; }
        }
        private string _signatureid;
        public string signatureid
        {
            get { return _signatureid; }
            set { _signatureid = value; }
        }
        private string _signaturename;
        public string signaturename
        {
            get { return _signaturename; }
            set { _signaturename = value; }
        }
        private string _signatureunit;
        public string signatureunit
        {
            get { return _signatureunit; }
            set { _signatureunit = value; }
        }
        private string _signatureuser;
        public string signatureuser
        {
            get { return _signatureuser; }
            set { _signatureuser = value; }
        }
        private string _keysn;
        public string keysn
        {
            get { return _keysn; }
            set { _keysn = value; }
        }
        private string _signaturesn;
        public string signaturesn
        {
            get { return _signaturesn; }
            set { _signaturesn = value; }
        }
        private string _signatureguid;
        public string signatureguid
        {
            get { return _signatureguid; }
            set { _signatureguid = value; }
        }
        private string _ip;
        public string ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _logtype;
        public string logtype
        {
            get { return _logtype; }
            set { _logtype = value; }
        }
        private DateTime _logtime = DateTime.Now;
        public DateTime logtime
        {
            get { return _logtime; }
            set { _logtime = value; }
        }
    }
}