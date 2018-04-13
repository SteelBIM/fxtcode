using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSWF.Framework.BLL;
using KSWF.WFM.Constract.Models;
using KSWF.Core.Utility;
using System.Linq.Expressions;
using KSWF.Framework.DAL;
using MySql.Data.MySqlClient;
using KSWF.Web.Admin.Models;


namespace KSWF.WFM.BLL
{
    public class AccountManager : Manage
    {
        Repository repository = new Repository();
        public com_master GetAccountInfo(string masterName)
        {
            return null;
        }

        /// <summary>
        /// 查询用户名对应的所有产品策略
        /// </summary>
        /// <param name="masterName"></param>
        /// <returns></returns>
       
        public List<join_masterbpolicypr> GetMasterPolicyPros(string masterName)
        {
            var sql = @"SELECT *,value as categoryname FROM join_masterbpolicypr mp
LEFT JOIN cfg_bpolicyproduct ppr on mp.bid=ppr.bid
LEFT JOIN cfg_bpolicy p on ppr.bid=p.bid
LEFT JOIN cfg_product pr on ppr.bid=pr.id
LEFT JOIN cfg_keyvalue k ON pr.category=k.Key
WHERE mp.mastername=@mastername";
            var result = SqlQuery<join_masterbpolicypr>(sql, new List<MySqlParameter> { new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "mastername", Value = masterName } });
            return result;
        }

        public List<masterbpolicypr> GetmasterPolicy(string mastername)
        {
            string sql = @"select mp.*,vbp.versionid,vbp.version,vbp.category,vbp.categorykey,vbp.divided,vbp.class_divided from vw_masterbpolicypr as mp
                         join vw_bpolicyproduct as vbp on vbp.bid=mp.bid
                        where mp.mastername=@mastername and startdate<now()";
            var result = SqlQuery<masterbpolicypr>(sql, new List<MySqlParameter> { new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "mastername", Value = mastername } });
            return result;
        }
       
    }
}
