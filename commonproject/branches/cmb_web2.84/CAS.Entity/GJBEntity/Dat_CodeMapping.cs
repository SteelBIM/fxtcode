﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_CodeMapping : DatCodeMapping
    {
        [SQLReadOnly]
        public string codename { get; set; }
    }
}
