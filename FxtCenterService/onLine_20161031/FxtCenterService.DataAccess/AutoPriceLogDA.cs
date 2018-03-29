using CAS.Common;
using CAS.Entity.FxtLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.DataAccess
{
    public class AutoPriceLogDA : Base
    {
        public static int Add(AutoPriceLog model)
        {
            GlobleCache.CenterDBCityTable.Reset();
            return InsertFromEntity<AutoPriceLog>(model);
        }
    }
}
