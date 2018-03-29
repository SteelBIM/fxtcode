using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkBusinessErrorTypes : DatNWorkBusinessErrorTypes
    {
        private List<Dat_NWorkBusinessErrors> _errors = new List<Dat_NWorkBusinessErrors>();
        [SQLReadOnly]
        public List<Dat_NWorkBusinessErrors> errors {
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
