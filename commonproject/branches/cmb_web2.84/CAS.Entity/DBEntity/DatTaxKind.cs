using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_TaxKind")]
    public class DatTaxKind : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long? _objectid;
        /// <summary>
        /// 对象的id
        /// </summary>
        public long? objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private string _guid;
        public string guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        private int _businesstype;
        /// <summary>
        /// 值为1来自询价(objectid)、2来自委估对象(objectid)、3来自业务、4来自预评、5来自报告、6来自多套
        /// </summary>
        public int businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private int _step;
        public int step
        {
            get { return _step; }
            set { _step = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }


}
