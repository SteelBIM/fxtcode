using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.UserOrder.Contract.DataModel;
using CBSS.UserOrder.Contract.ViewModel;

namespace CBSS.UserOrder.IBLL
{
    public interface IUserOrderService:IUserModule
    {
        IEnumerable<v_UserPayOrder> GetUserPayOrderList(out int totalaount, UserPayOrderRequest request = null);
        IEnumerable<v_UserPayOrder> GetUserPayOrderList( UserPayOrderRequest request = null);


    }
}
