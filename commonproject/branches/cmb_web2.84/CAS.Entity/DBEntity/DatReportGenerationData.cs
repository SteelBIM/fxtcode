using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ReportGenerationData")]
    public class DatReportGenerationData : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int? _type;
        public int? type
        {
            get { return _type; }
            set { _type = value; }
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
        private string _text;
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        private int? _addressid;
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? addressid
        {
            get { return _addressid; }
            set { _addressid = value; }
        }
        private int? _addressidtype;
        /// <summary>
        /// 1为省份、2为城市、3为区域、4为片区
        /// </summary>
        public int? addressidtype
        {
            get { return _addressidtype; }
            set { _addressidtype = value; }
        }
        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int _datatype;
        public int datatype
        {
            get { return _datatype; }
            set { _datatype = value; }
        }
        private bool _valid;
        public bool valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }
    }

}
