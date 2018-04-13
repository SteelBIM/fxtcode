using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.UserOrder.IBLL;
using CBSS.Framework.DAL;
using CBSS.UserOrder.Contract.DataModel;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.UserOrder.Contract.ViewModel;

namespace CBSS.UserOrder.BLL
{
    public partial  class UserOrderService:IUserOrderService
    {
        Repository repository = new Repository("TbxRecord");

        public IEnumerable<v_UserPayOrder> GetUserPayOrderList(out int totalcount, UserPayOrderRequest request = null)
        {
            PageParameter<v_UserPayOrder> pageParameter = new PageParameter<v_UserPayOrder>();
            pageParameter.Wheres = Exprlists(request);
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.PayDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<v_UserPayOrder>(pageParameter, out totalcount);
        }


        public List<Expression<Func<v_UserPayOrder, bool>>> Exprlists(UserPayOrderRequest request = null)
        {
            request = request ?? new UserPayOrderRequest();

            List<Expression<Func<v_UserPayOrder, bool>>> exprlist = new List<Expression<Func<v_UserPayOrder, bool>>>();

            if (request.PayWay > 0)
                exprlist.Add(u => u.PayWay == request.PayWay);

            if (request.Status > 0)
                exprlist.Add(u => u.Status == request.Status);

            if (!string.IsNullOrEmpty(request.AppName))
                exprlist.Add(u => u.AppName.Contains(request.AppName.Trim()));

            if (!string.IsNullOrEmpty(request.UserName))
                exprlist.Add(u => u.UserName.Contains(request.UserName.Trim()));

            if (!string.IsNullOrEmpty(request.UserPhone))
                exprlist.Add(u => u.UserPhone.Contains(request.UserPhone.Trim()));

            if (!string.IsNullOrEmpty(request.OrderID))
                exprlist.Add(u => u.OrderID.Contains(request.OrderID.Trim()));

            return exprlist;
        }

        public IEnumerable<v_UserPayOrder> GetUserPayOrderList(UserPayOrderRequest request = null)
        {
            return repository.SelectSearchs<v_UserPayOrder>(Exprlists(request),  " PayDate desc");
        }
    }
}
