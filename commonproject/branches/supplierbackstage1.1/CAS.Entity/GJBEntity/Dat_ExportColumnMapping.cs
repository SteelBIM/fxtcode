using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ExportColumnMapping : DatExportColumnMapping
    {
        [SQLReadOnly]
        public string fieldname { get; set; }
        [SQLReadOnly]
        public string showname { get; set; }
        [SQLReadOnly]
        public string querycolumn { get; set; }
    }
}
