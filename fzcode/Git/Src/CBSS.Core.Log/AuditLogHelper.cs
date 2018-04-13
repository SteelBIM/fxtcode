using System;
using System.IO;
using System.Text;
using log4net;
using Newtonsoft.Json;
using CBSS.Core.Config;
using CBSS.Framework.Contract;

namespace CBSS.Core.Log
{
    /// <summary>
    /// 操作日志写入DB
    /// </summary>
    public class AuditLogHelper
    {
        /// <summary>
        /// 写入AuditLog
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="userName"></param>
        /// <param name="moduleName"></param>
        /// <param name="tableName"></param>
        /// <param name="eventType"></param>
        /// <param name="newValues"></param>
        public void WriteLog(int modelId, string userName, string moduleName, string tableName, string eventType, ModelBase newValues)
        {
            
        }
    }

    public class AuditLog : ModelBase
    {
        public int ModelId { get; set; }
        public string UserName { get; set; }
        public string ModuleName { get; set; }
        public string TableName { get; set; }
        public string EventType { get; set; }
        public string NewValues { get; set; }
    }

}
