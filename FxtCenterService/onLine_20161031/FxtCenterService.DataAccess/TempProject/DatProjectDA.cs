using CAS.DataAccess.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    public class DatProjectDA : BaseDA
    {
        public static int Add(DatProject model)
        {
            return InsertFromEntity<DatProject>(model);
        }
    }
}
