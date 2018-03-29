using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("[FxtLog].[dbo].ExecuteTimeCountLog")]
    public class ExecuteTimeCountLog : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private DateTime? _executetime;
        public DateTime? executetime
        {
            get { return _executetime; }
            set { _executetime = value; }
        }
        private int? _total;
        public int? total
        {
            get { return _total; }
            set { _total = value; }
        }
        private string _serverid;
        public string serverid
        {
            get { return _serverid; }
            set { _serverid = value; }
        }
    }
}