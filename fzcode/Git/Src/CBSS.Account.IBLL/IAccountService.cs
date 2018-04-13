using CBSS.Account.Contract;
using CBSS.Account.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Account.IBLL
{
    public interface IAccountService
    {
        Sys_LoginInfo GetLoginInfo(Guid token);
        Sys_LoginInfo Login(string loginName, string password);
        void Logout(Guid token);
        bool ModifyPwd(Sys_User user);

        Sys_User GetUser(int id);
        IEnumerable<Sys_User> GetUserList(out int totalcount, UserRequest request = null);
        void SaveUser(Sys_User user);
        void DeleteUser(List<int> ids);

        Sys_Role GetRole(int id);
        IEnumerable<Sys_Role> GetRoleList(out int totalcount, RoleRequest request = null);
        IEnumerable<Sys_Role> GetRoleList();
        void SaveRole(Sys_Role role);
        void DeleteRole(List<int> ids);

        int GetRoleUserNumber(int roleId);

        IEnumerable<v_allaction> GetAllAction();
        IEnumerable<v_allaction> Get_AllActionByUserID(string UserID);
        IEnumerable<v_action> GetRoleAction(int roleid);
    }
}
