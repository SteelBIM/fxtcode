using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Domain.Models;
using OpenPlatform.Domain.Repositories;
using Dapper;
using OpenPlatform.Framework.Data.SQL;

namespace OpenPlatform.Framework.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Customer GetCustomer(string userId)
        {
            var strSql = CustomerSql.GetCustomerByUserId;

            using (var conn = Dapper.MySqlConnection())
            {
                return conn.Query<Customer>(strSql, new { userId }).FirstOrDefault();
            }
        }
    }
}
