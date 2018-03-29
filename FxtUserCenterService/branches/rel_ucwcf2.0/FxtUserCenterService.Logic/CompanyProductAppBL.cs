using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using FxtUserCenterService.DataAccess;

namespace FxtUserCenterService.Logic
{
    public class CompanyProductAppBL
    {
        public static int Add(CompanyProductApp model)
        {
            return CompanyProductAppDA.Add(model);
        }
        public static int Update(CompanyProductApp model)
        {
            return CompanyProductAppDA.Update(model);
        }
        //批量更新
        public static int UpdateMul(CompanyProductApp model, int[] ids)
        {
            return CompanyProductAppDA.UpdateMul(model, ids);
        }
        public static int Delete(int id)
        {
            return CompanyProductAppDA.Delete(id);
        }
        public static CompanyProductApp GetAppkey(int appid, string apppwd, int systypecode,string functionName, string signname,string splatype)
        {
            return CompanyProductAppDA.GetAppkey(appid, apppwd, systypecode, functionName,signname, splatype);// functionName,
        }

        /// <summary>
        /// 获得产品API信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="systypecode"></param>
        /// <param name="signname"></param>
        /// <returns></returns>
        public static List<CompanyProductApp> GetProductAPIInfo(int appid, int systypecode, string signname)
        {
            return CompanyProductAppDA.GetProductAPIInfo(appid, systypecode,  signname);
        }
    }

}
