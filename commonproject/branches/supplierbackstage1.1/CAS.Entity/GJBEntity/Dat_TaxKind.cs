using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class Dat_TaxKind : DatTaxKind
    {
        public List<DatTaxKindDetails> details
        {
            get;
            set;
        }
    }
}
