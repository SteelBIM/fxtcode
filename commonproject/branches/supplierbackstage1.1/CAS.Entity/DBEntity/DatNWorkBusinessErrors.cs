using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkBusinessErrors")]
	public class DatNWorkBusinessErrors : BaseTO
	{
		private int _businesserrorid;
		[SQLField("businesserrorid", EnumDBFieldUsage.PrimaryKey, true)]
		public int businesserrorid
		{
			get{ return _businesserrorid;}
			set{ _businesserrorid=value;}
		}
        private int _fk_typeid;
        public int fk_typeid
        {
            get { return _fk_typeid; }
            set { _fk_typeid = value; }
        }
		private string _errordescript;
		public string errordescript
		{
			get{ return _errordescript;}
			set{ _errordescript=value;}
		}
		private DateTime _createdon = DateTime.Now;
		public DateTime createdon
		{
			get{ return _createdon;}
			set{ _createdon=value;}
		}
		private int _valid = 0;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
	}
}