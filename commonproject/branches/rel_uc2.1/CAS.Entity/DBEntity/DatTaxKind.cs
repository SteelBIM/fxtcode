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
        public long? objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _purpose;
        public int purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        private int _ownertypecode;
        public int ownertypecode
        {
            get { return _ownertypecode; }
            set { _ownertypecode = value; }
        }
        private int _expiryfiveyear;
        public int expiryfiveyear
        {
            get { return _expiryfiveyear; }
            set { _expiryfiveyear = value; }
        }
        private int _onlylivingroom;
        public int onlylivingroom
        {
            get { return _onlylivingroom; }
            set { _onlylivingroom = value; }
        }
        private int _firstbuye;
        public int firstbuye
        {
            get { return _firstbuye; }
            set { _firstbuye = value; }
        }
        private int _areasegment;
        public int areasegment
        {
            get { return _areasegment; }
            set { _areasegment = value; }
        }
        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private decimal _totaltax;
        public decimal totaltax
        {
            get { return _totaltax; }
            set { _totaltax = value; }
        }
        private string _guid;
        public string guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        private int _businesstype;
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

    }


}
