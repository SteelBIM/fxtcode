using CBSS.Core.Utility;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        public IEnumerable<Good> GetGoodList(Expression<Func<Good, bool>> expression)
        {
            return repository.SelectSearch<Good>(expression);
        }
        public IEnumerable<v_Good> GetGoodList(out int totalcount, GoodRequest request = null)
        {
            request = request ?? new GoodRequest();

            List<Expression<Func<v_Good, bool>>> exprlist = new List<Expression<Func<v_Good, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.GoodName))
                exprlist.Add(u => u.GoodName.Contains(request.GoodName.Trim()));
            if (request.GoodWay != 0)
                exprlist.Add(u => u.GoodWay == request.GoodWay);
            PageParameter<v_Good> pageParameter = new PageParameter<v_Good>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<v_Good>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 保存商品
        /// </summary>
        /// <param name="model"></param>
        public int SaveGood(Good model)
        {
            if (model.GoodID > 0)
            {
                //验证重名
                var list = repository.SelectSearch<Good>(a => a.GoodName == model.GoodName && a.GoodID != model.GoodID);
                if (list!=null&&list.Count()>0)
                {
                    return 2;
                }
               return repository.Update<Good>(model) ? 1 : 0;
            }
            else
            {
                //验证重名
                var list = repository.SelectSearch<Good>(a => a.GoodName == model.GoodName);
                if (list != null && list.Count() > 0)
                {
                    return 2;
                } 
                var obj = repository.InsertReturnEntity<Good>(model); 
                return obj != null ? 1 : 0;
            }
        }
        public Good GetGood(int id)
        {
            return repository.SelectSearch<Good>(m => m.GoodID == id).SingleOrDefault();
        }
        /// <summary>
        /// 根据GoodID删除商品策略(0：策略被引用不能删除，1：删除成功，2：删除失败)
        /// </summary>
        /// <param name="GoodID"></param>
        /// <returns></returns>
        public int DelGoodByGoodID(int GoodID)
        {
            IEnumerable<GoodPrice> listGoodPrice = repository.SelectSearch<GoodPrice>(a => a.GoodID == GoodID);
            IEnumerable<GoodModuleItem> listGoodModuleItem = repository.SelectSearch<GoodModuleItem>(a => a.GoodID == GoodID);
            IEnumerable<AppGoodItem> listAppGoodItem = repository.SelectSearch<AppGoodItem>(a => a.GoodID == GoodID);
            if ((listGoodPrice != null && listGoodPrice.Count() > 0)|| (listGoodModuleItem != null && listGoodModuleItem.Count() > 0)|| (listAppGoodItem != null && listAppGoodItem.Count() > 0))
            {
                return 0;
            }
            else
            {
                return repository.Delete<Good>(GoodID) ? 1 : 2;
            }
        }
    }
}
