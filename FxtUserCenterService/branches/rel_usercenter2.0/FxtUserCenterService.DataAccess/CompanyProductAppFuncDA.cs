using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Entity;

namespace FxtUserCenterService.DataAccess
{
    public class CompanyProductAppFuncDA : Base
    {
        //这里要加上producttypecode的判断 kevin 20140330
        public static IEnumerable<CompanyProductAppFunction> GetList(string appid, string apppwd, string signname, string splatype, string producttypecode)
        {
            //这里可以为空吗？应该都是必填参数吧？sql语句也不应该写在这里 kevin
            /*
            int filterForAppid = string.IsNullOrWhiteSpace(appid) ? 0 : 1;
            int filterForApppwd = string.IsNullOrWhiteSpace(apppwd) ? 0 : 1;
            int filterForSignName = string.IsNullOrWhiteSpace(signname) ? 0 : 1;
            int filterForPlatType = string.IsNullOrWhiteSpace(splatype) ? 0 : 1;
            */
            var sql = string.Format(@"select cpaf.* from CompanyInfo ci with(nolock)
                        inner join CompanyProduct cp with(nolock) on cp.CompanyId = ci.CompanyID
                        left join dbo.Product_App cpa with(nolock)
                            on cp.producttypecode =cpa.producttypecode 
                        left join dbo.app_function cpaf with(nolock)
                            on cpa.appid = cpaf.appid
                        where not exists(
	                        select 1 from dbo.product_app_black pab with(nolock)
	                        where cp.producttypecode =pab.producttypecode 
	                        and cp.CompanyId=pab.CompanyId
	                        and cpa.appid=pab.appid
	                        )
                         and not exists(
	                        select 1 from dbo.app_function_black afb with(nolock)
	                        where cpaf.functionname =afb.functionname 
	                        and cp.CompanyId=afb.CompanyId
	                        and cpa.appid=afb.appid
	                        and cp.producttypecode=afb.producttypecode
	                        and afb.SplaType='{3}'
	                        )

                        and cpa.AppId = {0} 
                        and cpa.AppPwd = '{1}'
                        and ci.SignName = '{2}'
                        and cp.producttypecode={4}", int.Parse(appid), apppwd, signname, splatype, producttypecode);

            return ExecuteToEntityList<CompanyProductAppFunction>(sql, CommandType.Text, null);
        }
    }
}
