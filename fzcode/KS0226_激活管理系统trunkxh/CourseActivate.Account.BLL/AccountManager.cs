using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseActivate.Framework.BLL;
using CourseActivate.Account.Constract.Models;
using CourseActivate.Core.Utility;
using System.Linq.Expressions;
using CourseActivate.Framework.DAL;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;


namespace CourseActivate.Account.BLL
{
    public class AccountManager : Manage
    {
        Repository repository = new Repository();
        public com_master GetAccountInfo(string masterName)
        {
            return null;
        }
    }
}
