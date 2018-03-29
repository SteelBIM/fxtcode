using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class SYS_User_TimeDAL : ISYS_User_TimeDAL
    {

        /// <summary>
        /// 根据用户名查找，未找到返回null
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>未找到返回null</returns>
        public SYS_User_Time Find(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException();
            }

            var strSql = @"select * from SYS_User_Time where UserName=@userName";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_User_Time>(strSql, new { userName = userName }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>成功返回1，失败返回0</returns>
        public int Insert(SYS_User_Time entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回更新的记录数</returns>
        public int Update(SYS_User_Time entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除指定用户记录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>返回删除的记录数</returns>
        public int Delete(string userName)
        {
            throw new NotImplementedException();
        }


    }
}
