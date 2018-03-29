using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using CAS.Entity.DBEntity;

namespace FxtCenterService.DataAccess
{
    public class SYSCodeDA : BaseDA
    {
        /// <summary>
        /// 获取CODE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codename"></param>
        /// <returns></returns>
        public static SYSCode GetCode(int id, string codename)
        {
            string sql = SQL.SQLName.SysCode.CodeList;
            sql += string.Format(" and id={0} and codename = '{1}'", id, codename);
            return ExecuteToEntity<SYSCode>(sql, System.Data.CommandType.Text, null);
        }

        /// <summary>
        /// 获取CODE列表
        /// </summary>
        /// <param name="codeinfo"></param>
        /// <returns></returns>
        public static List<SYSCode> GetCodeList(Dictionary<int, string> codeinfo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.SQLName.SysCode.CodeList;
            sql += " and (1=2";
            foreach (int key in codeinfo.Keys)
            {
                //SQLFilterHelper.ProcessSqlStr(
                sql += string.Format(" or (id={0} and codename = '{1}')", key, codeinfo[key]);
            }
            sql += " )";
            return ExecuteToEntityList<SYSCode>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据code获取code列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<CAS.Entity.SurveyDBEntity.SYSCode> GetSYSCodeList(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.SQLName.SysCode.CodeList;
            if (id > 0)
            {
                string where = " and id=@id";
                parameters.Add(SqlHelper.GetSqlParameter("@id", id, SqlDbType.Int));
                sql += where;
            }
            return ExecuteToEntityList<CAS.Entity.SurveyDBEntity.SYSCode>(sql, System.Data.CommandType.Text, parameters);
        }
        
    }
}
