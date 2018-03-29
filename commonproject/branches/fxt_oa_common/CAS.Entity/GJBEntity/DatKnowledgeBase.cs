using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    [Serializable]
    public class DatKnowledgeBase : CAS.Entity.DBEntity.DatKnowledgeBase
    {
        [SQLReadOnly]
        public string knowledgetype { get; set; }
                
        [SQLReadOnly]
        public string lastupdateusername { get; set; }

        [SQLReadOnly]
        public string createusername { get; set; }
    }
}