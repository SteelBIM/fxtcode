using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using FxtUserCenterService.Entity;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using System.IO;

namespace FxtUserCenterService.DataAccess
{
    public class UserDA : Base
    {
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int Update(FxtUserCenterService.Entity.UserInfo model)
        {
            model.SetAvailableFields(new string[] { "userpwd" });
            return UpdateFromEntity<FxtUserCenterService.Entity.UserInfo>(model);
        }

        /// <summary>
        /// 新增用户 kevin 2013-4-2
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(FxtUserCenterService.Entity.UserInfo model)
        {
            string sql = @"select 1 from dbo.userinfo with(nolock) where username=@username and companyid=@companyid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@username", model.username, SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", model.companyid, SqlDbType.Int));
            FxtUserCenterService.Entity.UserInfo user = ExecuteToEntity<FxtUserCenterService.Entity.UserInfo>(sql, CommandType.Text, parameters);
            if (user == null)
            {
                return InsertFromEntity<FxtUserCenterService.Entity.UserInfo>(model);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(user.username))
                {
                    return InsertFromEntity<FxtUserCenterService.Entity.UserInfo>(model);
                }
                else
                {
                    return 1;
                }

            }

        }
        /// <summary>
        /// 删除用户 kevin 2013-4-2
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static int Delete(string username, int companyid,int valid)
        {
            SqlCommand cmd = new SqlCommand();
            string[] usernameArray = username.Split(',');
            int loop = 0;
            string sql = @"update dbo.userinfo set valid=@valid where ";
            sql += " (";
            foreach (string name in usernameArray)
            {
                if (0 == loop)
                {
                    sql += " username = @username" + loop.ToString();
                }
                else
                {
                    sql += " Or username = @username" + loop.ToString();
                }
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@username" + loop.ToString(), name, SqlDbType.NVarChar));
                loop++;
            }
            sql += " )";
            sql += " And companyid=@companyid";
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@valid", valid, SqlDbType.Bit));
            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 检查用户 kevin 2013-4-2
        /// </summary>
        /// <param name="username"></param>
        /// <param name="systypecode"></param>
        /// <returns></returns>
        public static UserCheck GetCheckUser(string username, int systypecode)
        {
            string sql = @"select a.username,a.companyid,a.valid as uservalid,a.emailstr,a.mobile,a.wxopenid,b.valid as companyvalid,b.businessdb,c.currentversion 
                ,c.startdate,c.overdate,c.weburl,c.apiurl,c.msgserver,c.valid as productvalid,b.companycode,b.companyname
                ,a.truename,b.SignName,a.isinner
                from dbo.userinfo a with(nolock) 
                inner join dbo.companyinfo b with(nolock) on a.companyid=b.companyid
                inner join dbo.companyproduct c with(nolock) on b.companyid=c.companyid and c.ProductTypeCode=@ProductTypeCode
                where a.username=@username";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@ProductTypeCode", systypecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.NVarChar));
            return ExecuteToEntity<UserCheck>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 检查用户 caoq 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static UserCheck GetCheckUser(string username)
        {
            string sql = @"select a.username,a.companyid,a.valid as uservalid,a.emailstr,a.mobile,a.wxopenid, b.valid as companyvalid,b.businessdb,b.companycode,b.companyname,a.isinner
                from dbo.userinfo a with(nolock) 
                inner join dbo.companyinfo b with(nolock) on a.companyid=b.companyid
                where a.username=@username";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.NVarChar));
            return ExecuteToEntity<UserCheck>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 查询用户 sun 2013-12-27 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static UserCheck GetFindUser(string username)
        {
            string sql = @"select a.username,a.companyid,a.valid as uservalid,a.emailstr,a.mobile,a.wxopenid, b.valid as companyvalid,b.businessdb,b.companycode,b.companyname,a.isinner,a.truename
                from dbo.userinfo a with(nolock) 
                inner join dbo.companyinfo b with(nolock) on a.companyid=b.companyid
                where a.username=@username";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.NVarChar));
            return ExecuteToEntity<UserCheck>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 编辑用户 sun 2013-12-27 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public static int EditUser(FxtUserCenterService.Entity.UserInfo userinfo, string password)
        {
            StringBuilder sql = new StringBuilder("update  dbo.userinfo set updateDate=@updateDate");
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@updateDate", DateTime.Now.ToString(), SqlDbType.NVarChar));
            //用户密码不为空，修改用户密码 caoq 2014-4-4
            if (!string.IsNullOrWhiteSpace(password))
            {
                sql.Append(",userpwd=@userpwd");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@userpwd", password, SqlDbType.NVarChar));
            }

            sql.Append(",emailstr=@emailstr");
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@emailstr", userinfo.emailstr, SqlDbType.NVarChar));

            sql.Append(",mobile=@mobile");
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@mobile", userinfo.mobile, SqlDbType.NVarChar));
            //wxopenid
            if (!string.IsNullOrWhiteSpace(userinfo.wxopenid))
            {
                sql.Append(",wxopenid=@wxopenid");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@wxopenid", userinfo.wxopenid, SqlDbType.NVarChar));
            }
            sql.Append(",valid=@valid");
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@valid", userinfo.valid, SqlDbType.Int));


            sql.Append(",truename=@truename");
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@truename", userinfo.truename, SqlDbType.VarChar));

            string sqlstr = sql.ToString() + " where username=@username and companyid=@companyid";
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@username", userinfo.username, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", userinfo.companyid, SqlDbType.Int));
            cmd.CommandText = sqlstr;
            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 获取用户列表 caoq 2014-03-04
        /// </summary>
        /// <param name="search"></param>
        /// <param name="companyid">公司ID</param>
        /// <param name="companycode">公司CODE</param>
        /// <param name="valid">是否可用,1:可用,0:不可用,null:所有</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="userids">用户ID列表</param>
        /// <returns></returns>
        public static List<UserCheck> GetUserList(SearchBase search, int companyid, string companycode,int? valid ,string keyword, string[] userids)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            //UserPwd  不获取用户密码
            string sql = @"select UserName, a.CompanyId, a.CreateDate, a.Valid as uservalid, EmailStr, Mobile, wxopenid, UpdateDate,TrueName,b.CompanyName,a.isinner  
                from dbo.UserInfo a with(nolock)
                inner join dbo.CompanyInfo b with(nolock) on a.CompanyId=b.CompanyId
                where 1=1";
            if (companyid > 0) //根据公司ID查询用户
            {
                sql += " and b.companyid=@companyid";
                parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(companycode))//根据CompanyCode查询用户
            {
                sql += " and b.companycode=@companycode";
                parameters.Add(SqlHelper.GetSqlParameter("@companycode", companycode, SqlDbType.VarChar));
            }
            if (valid != null)
            {
                sql += " and a.Valid=@valid";
                parameters.Add(SqlHelper.GetSqlParameter("@valid", Convert.ToInt32(valid), SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " and ( a.username like @keyword or  a.truename like @keyword )";
                parameters.Add(SqlHelper.GetSqlParameter("@keyword", "%" + keyword + "%", SqlDbType.VarChar));
            }
            if (userids != null && userids.Length > 0)//根据用户ID查询用户  
            {
                string uNames = "";
                foreach (string item in userids)
                {
                    uNames += ",'" + item + "'";//需要做安全性验证
                }
                uNames = uNames.Substring(1);

                sql += " and a.username in (" + uNames + ")";
            }
            search.OrderBy = " UserName";
            sql = HandleSQL(search, sql);

            return ExecuteToEntityList<UserCheck>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取用户信息与安全信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="productTypeCode">产品Code</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static InheritUserInfo GetUserAndAppInfo(SearchBase search, int productTypeCode, string userName, string appabbreviation, string weburl)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string sql = SQLName.UserInfo.GetUserAndAppInfo;
                if (!string.IsNullOrEmpty(appabbreviation) && appabbreviation == "yck")
                {
                    sql += "and cp.AppAbbreviation = @appabbreviation ";
                    parameters.Add(SqlHelper.GetSqlParameter("@appabbreviation", appabbreviation, SqlDbType.VarChar));
                }
                else
                {
                    
                    sql += "and cp.ProductTypeCode =@producttypecode ";
                    parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", productTypeCode, SqlDbType.Int));
                }

                if (productTypeCode == 1003001 && !string.IsNullOrEmpty(weburl)) 
                {
                    sql += "and (cp.WebUrl = @weburl or cp.WebUrl1 = @weburl)  ";
                    parameters.Add(SqlHelper.GetSqlParameter("@weburl", weburl, SqlDbType.VarChar));
                }

                sql += " order by cp.OverDate desc ";
                
                parameters.Add(SqlHelper.GetSqlParameter("@username", userName, SqlDbType.NVarChar));

                return ExecuteToEntity<InheritUserInfo>(sql, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        /// <summary>
        /// 获取用户信息与安全信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="productTypeCode">产品Code</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static List<InheritUserInfo> GetApps(SearchBase search, int productTypeCode, string userName, string appabbreviation)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string sql = SQLName.UserInfo.GetApps;
                if (productTypeCode > 0)
                {

                    sql += " and cp.ProductTypeCode =@producttypecode ";
                    parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", productTypeCode, SqlDbType.Int));
                }

                if (!string.IsNullOrEmpty(appabbreviation))
                {
                    sql += " and cp.AppAbbreviation =@appabbreviation ";
                    parameters.Add(SqlHelper.GetSqlParameter("@appabbreviation", appabbreviation, SqlDbType.VarChar));
                }
                sql += "group by cpa.AppId,cpa.AppKey,cpa.AppPwd,cpa.ApiUrl,ui.isinner";

                parameters.Add(SqlHelper.GetSqlParameter("@username", userName, SqlDbType.NVarChar));

                return ExecuteToEntityList<InheritUserInfo>(sql, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        /// <summary>
        /// 验证用户密码是否正确 caoq 2014-4-4
        /// modify: 获取用户密码 hody 2014-04-08
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetUserPassword(string username) 
        {
            string sql = SQLName.UserInfo.CheckUserPwd;
            SqlCommand command=new SqlCommand();
            command.CommandText=sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.VarChar));
            return ExecuteScalar(command)!=null?ExecuteScalar(command).ToString():"";
        }

        /// <summary>
        /// 根据用户名获取用户信息 caoq 2014-06-10
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static FxtUserCenterService.Entity.UserInfo GetUserInfoByUserName(string username) 
        {
            string sql = SQLName.UserInfo.GetUserInfo;
            sql += " and username=@username ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.VarChar));
            return ExecuteToEntity<FxtUserCenterService.Entity.UserInfo>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 修改用户密码 hody 2014-07-25
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static int UpdatePassWord(string username, string userpwd, string oldpwd)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = SQLName.UserInfo.UpdatePassWord;
            cmd.CommandText = sql;
            List<SqlParameter> parameters = new List<SqlParameter>();
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@userpwd", userpwd, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@oldpwd", oldpwd, SqlDbType.VarChar));
            return (int)ExecuteScalar(cmd);
        }

        /// <summary>
        /// 根据用户名获取用户真实姓名（多个用户名用逗号隔开）
        /// zhoub 20160908
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public static List<UserCheck> GetUserInfoByUserNames(string username)
        {
            string sql = SQLName.UserInfo.GetUserInfoByUserNames;
            sql = sql.Replace("@username", username);
            return ExecuteToEntityList<UserCheck>(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 根据公司ID、用户名查询有效的客户帐号信息(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="search"></param>
        /// <param name="companyid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<FxtUserCenterService.Entity.UserInfo> GetUserListByUserName(SearchBase search, int companyid, string username)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQLName.UserInfo.GetUserListByUserName;
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@username", "%" + username + "%", SqlDbType.VarChar));
            search.OrderBy = " CreateDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<FxtUserCenterService.Entity.UserInfo>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据公司ID、用户名或真实姓名查询客户帐号信息(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="search"></param>
        /// <param name="companyid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<FxtUserCenterService.Entity.UserInfo> GetUserListByUserNameOrTrueName(SearchBase search, int companyid, string username)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQLName.UserInfo.GetUserListByUserNameOrTrueName;
            if (!string.IsNullOrWhiteSpace(username))
            {
                sql = sql.Replace("@strwhere", " and (username like '%" + username + "%' or truename like '%" + username + "%')");
            }
            else
            {
                sql = sql.Replace("@strwhere", "");
            }
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            search.OrderBy = " CreateDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<FxtUserCenterService.Entity.UserInfo>(sql, CommandType.Text, parameters);
        }
    }

}
