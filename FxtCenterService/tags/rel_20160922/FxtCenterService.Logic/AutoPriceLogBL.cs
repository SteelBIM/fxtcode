using CAS.Entity.FxtLog;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class AutoPriceLogBL
    {
        public static int Add(AutoPriceLog log)
        {
            return AutoPriceLogDA.Add(log);
        }
    }
}
