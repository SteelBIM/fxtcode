using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Appraisal_Stage_Mapping")]
    public class DatAppraisalStageMapping : BaseTO
    {
        private int _mappingid;
        [SQLField("mappingid", EnumDBFieldUsage.PrimaryKey, true)]
        public int mappingid
        {
            get { return _mappingid; }
            set { _mappingid = value; }
        }
        private int _globalid;
        public int globalid
        {
            get { return _globalid; }
            set { _globalid = value; }
        }
        private int _stageid;
        public int stageid
        {
            get { return _stageid; }
            set { _stageid = value; }
        }
        private long _businessid;
        public long businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private int _businesstype;
        public int businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private string _userids;
        public string userids
        {
            get { return _userids; }
            set { _userids = value; }
        }
        private string _truenames;
        public string truenames
        {
            get { return _truenames; }
            set { _truenames = value; }
        }
        private decimal? _coefficient;
        public decimal? coefficient
        {
            get { return _coefficient; }
            set { _coefficient = value; }
        }
        private decimal _basescore;
        public decimal basescore
        {
            get { return _basescore; }
            set { _basescore = value; }
        }
        private decimal? _score;
        public decimal? score
        {
            get { return _score; }
            set { _score = value; }
        }
        private int _statisticsstage;
        public int statisticsstage
        {
            get { return _statisticsstage; }
            set { _statisticsstage = value; }
        }
        private int _statisticsstagebusinessid;
        public int statisticsstagebusinessid
        {
            get { return _statisticsstagebusinessid; }
            set { _statisticsstagebusinessid = value; }
        }
        private int _createduserid;
        public int createduserid
        {
            get { return _createduserid; }
            set { _createduserid = value; }
        }
        private DateTime _createdon;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int? _updateduserid;
        public int? updateduserid
        {
            get { return _updateduserid; }
            set { _updateduserid = value; }
        }
        private DateTime? _updatedon;
        public DateTime? updatedon
        {
            get { return _updatedon; }
            set { _updatedon = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _operating;
        public string operating
        {
            get { return _operating; }
            set { _operating = value; }
        }
    }

}
