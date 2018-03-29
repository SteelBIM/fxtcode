using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.DataAccess.DA.Log;
using CAS.Entity;
using CAS.Common;

namespace CAS.Logic.Log
{
    public class LogBL
    {
        public static long Add(SYSLog model)
        {
            model.SetIgnoreFields(new string[] { "CreateTime" });
            return LogDA.Add(model);
        }
        public static int Update(SYSLog model)
        {
            return LogDA.Update(model);
        }
        public static int Delete(long id)
        {
            return LogDA.Delete(id);
        }
        public static SYSLog GetSYSLogByPK(long id)
        {
            return LogDA.GetSYSLogByPK(id);
        }
        public static List<SYSLog> GetSYSLogList(SearchBase search, int eventtype, int logtype, string key)
        {
            return LogDA.GetSYSLogList(search, eventtype, logtype, key);
        }
    }
}
