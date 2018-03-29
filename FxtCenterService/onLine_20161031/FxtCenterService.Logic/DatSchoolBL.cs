using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class DatSchoolBL
    {
        public static int Add(DatSchool model)
        {
            return DatSchoolDA.Add(model);

        }
        public static int Update(DatSchool model)
        {
            return DatSchoolDA.Update(model);

        }

        public static List<DatSchool> GetSchoolByCityId(int cityId)
        {
            return DatSchoolDA.GetSchoolByCityId(cityId);
        }
    }
}
