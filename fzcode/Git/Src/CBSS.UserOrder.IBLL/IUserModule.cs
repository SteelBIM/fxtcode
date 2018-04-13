using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.UserOrder.Contract.DataModel;
using CBSS.UserOrder.Contract.ViewModel;

namespace CBSS.UserOrder.IBLL
{
    public interface IUserModule
    {
        IEnumerable<v_UserModuleItem> GetUserUserModuleList(out int totalaount, UserModuleRequest request = null);

        UserModuleItem GetUserModule (int id);

        bool DelUserModule(int UserId);

        IEnumerable<UserModuleItem> GetUserUserModuleList(int UserId);

        bool SaveUserModule(int UserId, List<UserModuleItem> list);

        bool DeleteUserModule(List<int> ids);

    }
}
