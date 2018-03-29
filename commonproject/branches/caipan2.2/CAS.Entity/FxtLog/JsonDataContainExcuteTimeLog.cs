using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("FxtLog.dbo.JsonDataContainExcuteTimeLog")]
    public class JsonDataContainExcuteTimeLog : BaseTO
    {
        private string _jsondata;
        public string jsondata
        {
            get { return _jsondata; }
            set { _jsondata = value; }
        }

        private ExecuteTimeLog _executetimeLog;
        public ExecuteTimeLog executetimeLog
        {
            get { return _executetimeLog; }
            set { _executetimeLog = value; }
        }
        
    }
}