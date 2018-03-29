using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
	[TableAttribute("dbo.Dat_TaxKindDetails")]
	public class DatTaxKindDetails : BaseTO
	{
		private long _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public long id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private long? _objectid;
		public long? objectid
		{
			get{ return _objectid;}
			set{ _objectid=value;}
		}
		private int _taxtype;
		public int taxtype
		{
			get{ return _taxtype;}
			set{ _taxtype=value;}
		}
		private decimal _tax;
		public decimal tax
		{
			get{ return _tax;}
			set{ _tax=value;}
		}
		private bool _checked1 = false;
		public bool checked1
		{
            get { return _checked1; }
            set { _checked1 = value; }
		}
		private string _guid;
		public string guid
		{
			get{ return _guid;}
			set{ _guid=value;}
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
