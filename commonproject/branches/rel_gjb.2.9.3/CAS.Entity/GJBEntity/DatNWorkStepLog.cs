using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;


namespace CAS.Entity.GJBEntity
{
    public class Dat_NWorkStepLog : DatNWorkStepLog
    {
        private List<Dat_NWorkStepCheckLog> _checkloglist = new List<Dat_NWorkStepCheckLog>();
        [SQLReadOnly]
        public List<Dat_NWorkStepCheckLog> checkloglist
        {
            get
            {
                return _checkloglist;
            }
            set
            {
                _checkloglist = value;
            }
        }
    }
}
