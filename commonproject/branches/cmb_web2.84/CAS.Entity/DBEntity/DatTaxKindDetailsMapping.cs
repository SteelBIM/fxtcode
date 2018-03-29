using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{

    [Serializable]
    [TableAttribute("dbo.Dat_TaxKindDetails_Mapping")]
    public class DatTaxKindDetailsMapping : BaseTO
    {
        private long _mappingid;
        [SQLField("mappingid", EnumDBFieldUsage.PrimaryKey, true)]
        public long mappingid
        {
            get { return _mappingid; }
            set { _mappingid = value; }
        }
        private long _taxkindsubid;
        public long taxkindsubid
        {
            get { return _taxkindsubid; }
            set { _taxkindsubid = value; }
        }
        private long _taxkinddetailsid;
        public long taxkinddetailsid
        {
            get { return _taxkinddetailsid; }
            set { _taxkinddetailsid = value; }
        }
        private bool _checked1 = true;
        public bool checked1
        {
            get { return _checked1; }
            set { _checked1 = value; }
        }
    }

}
