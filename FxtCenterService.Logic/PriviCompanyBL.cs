using CAS.Entity.FxtProject;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class PriviCompanyBL
    {
        public static Privi_Company AddPriviCompanyForVQ(Privi_Company model)
        {
            var company = PriviCompanyDA.GetPriviCompanyForVQ(model.companyname,model.companycode);
            if (company == null)
            {
                var id = PriviCompanyDA.AddPriviCompanyForVQ(model);
                if (id > 0)
                {
                    company = model;
                    company.companyid = id;
                }
                else
                {
                    throw new Exception("新增Privi_Company失败，PriviCompanyBL.AddPriviCompanyForVQ");
                }
            }
            return company;
        }
    }
}
