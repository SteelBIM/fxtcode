using CAS.Entity;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Logic
{
    public class ExecuteTimeLogBL
    {
        public static int Add(ExecuteTimeLog model)
        {
            return ExecuteTimeLogDA.Add(model);
        }

        public static int Add09(ExecuteTimeLog model)
        {
            return ExecuteTimeLogDA.Add09(model);
        }
    }
}
