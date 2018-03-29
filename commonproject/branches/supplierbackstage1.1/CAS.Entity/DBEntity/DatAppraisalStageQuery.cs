using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Appraisal_StageQuery")]
    public class DatAppraisalStageQuery : BaseTO
    {
        private int _stagequeryid;
        [SQLField("stagequeryid", EnumDBFieldUsage.PrimaryKey, true)]
        public int stagequeryid
        {
            get { return _stagequeryid; }
            set { _stagequeryid = value; }
        }
        private int _stagetype;
        public int stagetype
        {
            get { return _stagetype; }
            set { _stagetype = value; }
        }
        private int _stage;
        public int stage
        {
            get { return _stage; }
            set { _stage = value; }
        }
        private string _query;
        public string query
        {
            get { return _query; }
            set { _query = value; }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
