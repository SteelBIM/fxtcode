using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using CAS.DataAccess;
using CAS.Entity.DBEntity;
using CAS.Common;
using FxtUserCenterService.DataAccess;

namespace FxtUserCenterService.Logic
{
    public class DatPhoneMessageBL
    {
        public static int Add(DatPhoneMessage model)
        {
            return DatPhoneMessageDA.Add(model);
        }
        public static int Update(DatPhoneMessage model)
        {
            return DatPhoneMessageDA.Update(model);
        }
        public static int Delete(int id)
        {
            return DatPhoneMessageDA.Delete(id);
        }
        public static DatPhoneMessage GetDatPhoneMessageByPK(int id)
        {
            return DatPhoneMessageDA.GetDatPhoneMessageByPK(id);
        }
        public static List<DatPhoneMessage> GetDatPhoneMessageList(SearchBase search, string key)
        {
            return DatPhoneMessageDA.GetDatPhoneMessageList(search, key);
        }
    }
}