using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Knowledge_SubjectType")]
    public class DatKnowledgeSubjectType : BaseTO
    {
        private int _typecode = 0;
        [SQLField("typecode", EnumDBFieldUsage.PrimaryKey)]
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _typename = "";
        public string typename
        {
            get { return _typename; }
            set { _typename = value; }
        }
        private int? _parentcode;
        public int? parentcode
        {
            get { return _parentcode; }
            set { _parentcode = value; }
        }
    }
}
