using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity.FxtDataCenter;

namespace FxtCenterService.Logic
{
    public class DatProjectCaseBL
    {
        public static string Add(int cityid, string values)
        {
            return DatProjectCaseDA.Add(cityid, values);
        }
    }
}
