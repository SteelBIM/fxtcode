using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Entity;

namespace FxtUserCenterService.Logic
{
    public class CompanyProductAppFuncBL
    {
        public static IEnumerable<CompanyProductAppFunction> GetList(string appid, string apppwd, string signname, string platType, string producttypecode)
        {
            return CompanyProductAppFuncDA.GetList(appid, apppwd, signname, platType,producttypecode);
        }
    }
}
