using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkStepCheckLog : DatNWorkStepCheckLog
    {
        [SQLReadOnly]
        public string truename { get; set; }
        [SQLReadOnly]
        public string consignertruename { get; set; }
        List<Dat_StepLogBusinessErrorsMapping> _errors = new List<Dat_StepLogBusinessErrorsMapping>();
        [SQLReadOnly]
        public List<Dat_StepLogBusinessErrorsMapping> errors 
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }
    }
}
