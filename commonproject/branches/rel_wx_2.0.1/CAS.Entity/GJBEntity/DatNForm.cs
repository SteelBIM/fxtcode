using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class Dat_NForm : DatNForm
    {
        [SQLReadOnly]
        public int processingdocument { get; set; }

        private List<Dat_NForm> _subformlist = new List<Dat_NForm>();
        [SQLReadOnly]
        public List<Dat_NForm> subformlist
        {
            get
            { return _subformlist; }
            set
            {
                _subformlist = value;
            }
        }

        private List<Dat_NWorkFlow> _workflowlist = new List<Dat_NWorkFlow>();
        [SQLReadOnly]
        public List<Dat_NWorkFlow> workflowlist
        {
            get
            {
                return _workflowlist;
            }
            set
            {
                _workflowlist = value;
            }
        }

        [SQLReadOnly]
        public string truename { get; set; }
        [SQLReadOnly]
        public int workflowcount { get; set; }
    }
}
