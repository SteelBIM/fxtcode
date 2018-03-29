using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_TaxKind : DatTaxKind
    {
        [SQLReadOnly]
        public List<DatTaxKindDetails> details
        {
            get;
            set;
        }
        [SQLReadOnly]
        public List<Dat_TaxKindSub> subs
        {
            get;
            set;
        }
    }
}
