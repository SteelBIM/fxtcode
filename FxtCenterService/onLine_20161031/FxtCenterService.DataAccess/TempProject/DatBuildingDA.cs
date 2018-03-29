using CAS.DataAccess.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    public class DatBuildingDA : BaseDA
    {
        public static int Add(DatBuilding model)
        {
            return InsertFromEntity<DatBuilding>(model);
        }
    }
}
