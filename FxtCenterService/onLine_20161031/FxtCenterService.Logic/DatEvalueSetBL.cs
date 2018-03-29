using CAS.Entity.FxtDataCenter;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class DatEvalueSetBL
    {
        public static DatEvalueSet GetEvalueSetBy(int FxtCompanyId, int CityId, int valid = 1)
        {
            return DatEvalueSetDA.GetEvalueSetBy(FxtCompanyId, CityId, valid);
        }
    }
}
