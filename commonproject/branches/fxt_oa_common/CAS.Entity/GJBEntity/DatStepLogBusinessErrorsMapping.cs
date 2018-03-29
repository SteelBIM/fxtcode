using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_StepLogBusinessErrorsMapping : DatStepLogBusinessErrorsMapping
    {
        [SQLReadOnly]
        public string errordescript { get; set; }
        [SQLReadOnly]
        public string typename { get; set; }
        [SQLReadOnly]
        public string errorsremark { get; set; }
    }
}
