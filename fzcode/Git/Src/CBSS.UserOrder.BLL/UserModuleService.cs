using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.UserOrder.Contract.DataModel;
using CBSS.UserOrder.IBLL;
using CBSS.Framework.DAL;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using CBSS.UserOrder.Contract.ViewModel;

namespace CBSS.UserOrder.BLL
{
    public partial class UserOrderService : IUserModule
    {

        public IEnumerable<v_UserModuleItem> GetUserUserModuleList(out int totalcount, UserModuleRequest request = null)
        {
            PageParameter<v_UserModuleItem> pageParameter = new PageParameter<v_UserModuleItem>();
            pageParameter.Wheres = Exprlists(request);
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateTime;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            if (request.MarketClassifyIdList != null && request.MarketClassifyIdList.Count>0)
            {
                pageParameter.Field = "MarketClassifyId";
                pageParameter.In = request.MarketClassifyIdList.ConvertAll<string>(x => x.ToString());
            }
            return repository.SelectPage<v_UserModuleItem>(pageParameter, out totalcount);
        }
        public List<Expression<Func<v_UserModuleItem, bool>>> Exprlists(UserModuleRequest request = null)
        {
            request = request ?? new UserModuleRequest();

            List<Expression<Func<v_UserModuleItem, bool>>> exprlist = new List<Expression<Func<v_UserModuleItem, bool>>>();

            if (!string.IsNullOrEmpty(request.MarketBookName))
                exprlist.Add(u => u.MarketBookName.Contains(request.MarketBookName.Trim()));

            if (!string.IsNullOrEmpty(request.UserPhone))
                exprlist.Add(u => u.UserPhone.Contains(request.UserPhone.Trim()));

            return exprlist;
        }

        public bool DeleteUserModule(List<int> ids)
        {
            string[] stringIDs = ids.Select(o => o.ToString()).ToArray();
            return repository.DeleteMore<UserModuleItem>(stringIDs);
        }


        public UserModuleItem GetUserModule(int id)
        {
            return repository.GetByID<UserModuleItem>(id);
        }



        public IEnumerable<UserModuleItem> GetUserUserModuleList(int UserId)
        {
            return repository.SelectSearch<UserModuleItem>(um => um.UserID == UserId && um.PayOrderID == Core.Utility.SystemDefault.DefaultOrderNo);
        }



        public bool DelUserModule(int UserId)
        {
            try
            {
                return repository.Delete<UserModuleItem>(um => um.UserID == UserId && um.PayOrderID == CBSS.Core.Utility.SystemDefault.DefaultOrderNo);
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 根据GoodID删除对应的模块后再批量插入GoodModuleItem表
        /// </summary>
        /// <param name="GoodID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SaveUserModule(int UserId, List<UserModuleItem> list)
        {
            try
            {
                //DelUserModule(UserId);
                return repository.InsertBatch<UserModuleItem>(list);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
