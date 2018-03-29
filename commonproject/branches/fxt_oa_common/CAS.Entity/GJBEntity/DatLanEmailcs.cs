using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_LanEmail : DatLanEmail
    {
        [SQLReadOnly]
        public string tousername { get; set; }
        [SQLReadOnly]
        public List<Dat_Files> DatFileslist { get; set; }
    }
}
