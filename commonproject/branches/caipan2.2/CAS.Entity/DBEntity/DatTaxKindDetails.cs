using System;
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
        /// <summary>
        /// 1：营业税，2：城建税，3：教育费附加，4：印花税，5：处置费用，6：契税，7：交易手续费，8：土地增值税，9：所得税，10：扣除项目金额，11：增值额，12：改良投资，13：应补地价，14：案件受理费，15：拍卖费，16：诉讼费，17：申请执行费，18：地方教育费附加，19：公证费，20：转移登记费
        /// E：营业税，F：城建税，G：教育费附加，H：印花税，L：处置费用，I：契税，J：交易手续费，K：土地增值税，M：所得税， N：扣除项目金额， O：增值额， P：改良投资， Q：应补地价， R：案件受理费， S：拍卖费， T：诉讼费， U：申请执行费， V、地方教育费附加， W：公证费， X：转移登记费
        /// </summary>
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
