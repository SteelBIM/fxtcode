using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NFormType : DatNFormType
    {
        private List<Dat_NForm> _formlist = new List<Dat_NForm>();
        [SQLReadOnly]
        public List<Dat_NForm> formlist { get { return _formlist; } set { _formlist = value; } }
        [SQLReadOnly]
        public int formcount { get; set; }
    }
}
