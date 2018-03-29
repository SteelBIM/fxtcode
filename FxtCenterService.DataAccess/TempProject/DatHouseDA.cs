using CAS.DataAccess.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    public class DatHouseDA : BaseDA
    {
        public static int Add(DatHouse model)
        {
            return InsertFromEntity<DatHouse>(model);
        }
    }
}
