using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using FxtUserCenterService.DataAccess;

namespace FxtUserCenterService.Logic
{
    public class CompanyProductSafeBL
    {
        public static int Add(CompanyProductSafe model)
        {
            return CompanyProductSafeDA.Add(model);
        }
        public static int Update(CompanyProductSafe model)
        {
            return CompanyProductSafeDA.Update(model);
        }
        //批量更新
        public static int UpdateMul(CompanyProductSafe model, int[] ids)
        {
            return CompanyProductSafeDA.UpdateMul(model, ids);
        }
        public static int Delete(int id)
        {
            return CompanyProductSafeDA.Delete(id);
        }
        /// <summary>
        /// 验证应用身份
        /// </summary>
        /// <param name="search"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static CompanyProductSafe ValidateCallIdentity(int productTypeCode, string validate)
        {
            return CompanyProductSafeDA.ValidateCallIdentity(productTypeCode, validate);
        }
    }

}
