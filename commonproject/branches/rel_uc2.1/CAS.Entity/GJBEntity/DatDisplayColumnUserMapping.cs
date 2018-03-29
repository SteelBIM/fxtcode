using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_DisplayColumnUserMapping : DatDisplayColumnUserMapping
    {
        [SQLReadOnly]
        public string displayname { get; set; }
    }
}
