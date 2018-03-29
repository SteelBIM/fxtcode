using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Entrust_BONAFinancial")]
    public class DatEntrustBONAFinancial : BaseTO
    {
        private long _entrustid;
        [SQLField("entrustid", EnumDBFieldUsage.PrimaryKey,IsIdentify=false)]
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _entrusttype;
        public string entrusttype
        {
            get { return _entrusttype; }
            set { _entrusttype = value; }
        }
        private string _entrusttypecode;
        public string entrusttypecode
        {
            get { return _entrusttypecode; }
            set { _entrusttypecode = value; }
        }
        private string _mortgagetype;
        public string mortgagetype
        {
            get { return _mortgagetype; }
            set { _mortgagetype = value; }
        }
        private string _borrowerwhetherowner;
        public string borrowerwhetherowner
        {
            get { return _borrowerwhetherowner; }
            set { _borrowerwhetherowner = value; }
        }
        private string _customercompanyfullname;
        public string customercompanyfullname
        {
            get { return _customercompanyfullname; }
            set { _customercompanyfullname = value; }
        }
        private string _clientcontact;
        public string clientcontact
        {
            get { return _clientcontact; }
            set { _clientcontact = value; }
        }
        private string _clientphone;
        public string clientphone
        {
            get { return _clientphone; }
            set { _clientphone = value; }
        }
        private DateTime _createdate;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid;
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private int _ispushed;
        public int ispushed
        {
            get { return _ispushed; }
            set { _ispushed = value; }
        }
        private string _loanrole;
        public string loanrole
        {
            get { return _loanrole; }
            set { _loanrole = value; }
        }
    }

}