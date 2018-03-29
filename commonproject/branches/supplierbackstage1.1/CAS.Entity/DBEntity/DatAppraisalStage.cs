using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Appraisal_Stage")]
    public class DatAppraisalStage : BaseTO
    {
        private int _stageid;
        [SQLField("stageid", EnumDBFieldUsage.PrimaryKey, true)]
        public int stageid
        {
            get { return _stageid; }
            set { _stageid = value; }
        }
        private int? _globalid;
        public int? globalid
        {
            get { return _globalid; }
            set { _globalid = value; }
        }
        private int _stagetype;
        /// <summary>
        /// 1询价
        /// 2预评
        /// 3报告
        /// 4查勘
        /// </summary>
        public int stagetype
        {
            get { return _stagetype; }
            set { _stagetype = value; }
        }
        private int? _stage;
        public int? stage
        {
            get { return _stage; }
            set { _stage = value; }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private decimal _score;
        public decimal score
        {
            get { return _score; }
            set { _score = value; }
        }
        private bool _issystemstage = false;
        public bool issystemstage
        {
            get { return _issystemstage; }
            set { _issystemstage = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }

}
