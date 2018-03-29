using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Appraisal_Global")]
    public class DatAppraisalGlobal : BaseTO
    {
        private int _globalid;
        [SQLField("globalid", EnumDBFieldUsage.PrimaryKey, true)]
        public int globalid
        {
            get { return _globalid; }
            set { _globalid = value; }
        }
        private bool? _queryenable;
        public bool? queryenable
        {
            get { return _queryenable; }
            set { _queryenable = value; }
        }
        private int? _querystage;
        public int? querystage
        {
            get { return _querystage; }
            set { _querystage = value; }
        }
        private int? _querystatisticsstage;
        public int? querystatisticsstage
        {
            get { return _querystatisticsstage; }
            set { _querystatisticsstage = value; }
        }
        private bool? _ypenable;
        public bool? ypenable
        {
            get { return _ypenable; }
            set { _ypenable = value; }
        }
        private int? _ypstage;
        public int? ypstage
        {
            get { return _ypstage; }
            set { _ypstage = value; }
        }
        private int? _ypstatisticsstage;
        public int? ypstatisticsstage
        {
            get { return _ypstatisticsstage; }
            set { _ypstatisticsstage = value; }
        }
        private bool? _reportenable;
        public bool? reportenable
        {
            get { return _reportenable; }
            set { _reportenable = value; }
        }
        private int? _reportstage;
        public int? reportstage
        {
            get { return _reportstage; }
            set { _reportstage = value; }
        }
        private int? _reportstatisticsstage;
        public int? reportstatisticsstage
        {
            get { return _reportstatisticsstage; }
            set { _reportstatisticsstage = value; }
        }
        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int _createduserid;
        public int createduserid
        {
            get { return _createduserid; }
            set { _createduserid = value; }
        }
        private string _companyids;
        public string companyids
        {
            get { return _companyids; }
            set { _companyids = value; }
        }
        private string _companytext;
        public string companytext
        {
            get { return _companytext; }
            set { _companytext = value; }
        }
        private string _biztypes;
        public string biztypes
        {
            get { return _biztypes; }
            set { _biztypes = value; }
        }
        private string _biztexttypes;
        public string biztexttypes
        {
            get { return _biztexttypes; }
            set { _biztexttypes = value; }
        }
        private string _querytypes;
        public string querytypes
        {
            get { return _querytypes; }
            set { _querytypes = value; }
        }
        private string _querytexttypes;
        public string querytexttypes
        {
            get { return _querytexttypes; }
            set { _querytexttypes = value; }
        }
        private string _reportsubtypes;
        public string reportsubtypes
        {
            get { return _reportsubtypes; }
            set { _reportsubtypes = value; }
        }
        private string _reportsubtexttypes;
        public string reportsubtexttypes
        {
            get { return _reportsubtexttypes; }
            set { _reportsubtexttypes = value; }
        }

        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

}
