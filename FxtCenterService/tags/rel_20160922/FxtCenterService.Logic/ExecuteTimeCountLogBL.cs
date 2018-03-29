using CAS.Entity;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Logic
{
    public class ExecuteTimeCountLogBL
    {
        public static int Add(DateTime dt)
        {
            return ExecuteTimeCountLogDA.Add(dt);
        }

        public static int Add(ExecuteTimeCountLog model)
        {
            return ExecuteTimeCountLogDA.Add(model);
        }
        public static int Add09(ExecuteTimeCountLog model)
        {
            return ExecuteTimeCountLogDA.Add09(model);
        }
    }
}
