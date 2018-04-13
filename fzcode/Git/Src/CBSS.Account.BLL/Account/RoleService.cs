using CBSS.Account.Contract.ViewModel;
using CBSS.Account.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Account.BLL.Account
{
    public class RoleService : IRoleService
    {
        Framework.DAL.Repository repository = new Framework.DAL.Repository("Account");
        public IEnumerable<v_allaction> GetAllAction()
        {
            return repository.ListAll<v_allaction>();
        }
    }
}
