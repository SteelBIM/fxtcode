using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class DatPeitaoBL
    {
        public static int Add(DatPeitao model)
        {
            return DatPeitaoDA.Add(model);

        }
        public static int Update(DatPeitao model)
        {
            return DatPeitaoDA.Update(model);

        }

        public static List<DatPeitao> GetPeitaoByCityId(int cityId)
        {
            return DatPeitaoDA.GetPeitaoByCityId(cityId);
        }
    }
}
