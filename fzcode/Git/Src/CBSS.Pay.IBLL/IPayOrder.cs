using CBSS.Pay.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.UserOrder.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Framework.Contract.API;

namespace CBSS.Pay.IBLL
{
    public interface IPayOrder
    {
        List<CommodityDetails> GetCommodity(string appId, int moduleId);
        object InsertUserPayOrder(UserPayOrder userPayOrder);
        object InsertUserPayOrderGoodItem(UserPayOrderGoodItem userOrderInfo);
        UserPayOrder GetUserPayOrderByOrderId(string orderId);
        bool UpdateUserPayOrder(UserPayOrder userPayOrder);
        APIResponse OrderByWeChatPay(UserPayOrder order, string goodName, string packageName);
        APIResponse OrderByAliPay(UserPayOrder order, string goodName, double totalPrice,int Quantity);
        APIResponse InsertUserPayOrderAndOrderInfo(string userPayOrderId, UserPayOrder order, UserPayOrderGoodItem userOrderInfo, int channel);
        APIResponse PaySuccessByAliPay(string orderId, int channel, int userId, int bookId, int moduleId, UserPayOrder userPayOrder);
        APIResponse PaySuccessByWeChatPay(string orderId, string packageName, int channel, int userId, int bookId, int moduleId, UserPayOrder userPayOrder);
        List<UserModuleItem> GetUserModuleItemInfoByUserId(int userId, int bookId, int moduleId);
    }
}
