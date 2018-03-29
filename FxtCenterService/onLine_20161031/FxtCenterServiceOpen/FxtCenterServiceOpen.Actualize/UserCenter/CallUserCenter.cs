using FXT.VQ.UserService;
using FxtCenterServiceOpen.Actualize.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterServiceOpen.Actualize.UserCenter
{
    public class CallUserCenter
    {
        public static CompanyModel GetCompanyModelBySignName(string signName, out JsonReturnData returnData, out string outJson)
        {
            CompanyModel company = JsonDataHelper.JSONToObject<CompanyModel>(
                            UserDataServices.GetCompanyInfoBySignName(signName, out outJson), out returnData
                            );

            if (returnData.returntype == 1)
            {
                return company;
            }
            else
            {
                return null;
            }

        }
    }
}
