using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_TaxKindSub : DatTaxKindSub
    {
        [SQLReadOnly]
        public List<DatTaxKindDetailsMapping> mappings
        {
            get;
            set;
        }
    }
}
