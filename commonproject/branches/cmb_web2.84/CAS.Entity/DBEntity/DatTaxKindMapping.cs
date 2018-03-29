using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{

    [Serializable]
    [TableAttribute("dbo.Dat_TaxKind_Mapping")]
    public class DatTaxKindMapping : BaseTO
    {
        private long _mappingid;
        [SQLField("mappingid", EnumDBFieldUsage.PrimaryKey, true)]
        public long mappingid
        {
            get { return _mappingid; }
            set { _mappingid = value; }
        }
        private long _taxkindid;
        public long taxkindid
        {
            get { return _taxkindid; }
            set { _taxkindid = value; }
        }
        private decimal _tax;
        public decimal tax
        {
            get { return _tax; }
            set { _tax = value; }
        }
        private decimal _netprice;
        public decimal netprice
        {
            get { return _netprice; }
            set { _netprice = value; }
        }
    }
}
