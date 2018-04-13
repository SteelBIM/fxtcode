using CBSS.Core.Utility;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
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
        public IEnumerable<GoodPrice> GetGoodPriceList(out int totalcount, GoodPriceRequest request)
        {
            request = request ?? new GoodPriceRequest();

            List<Expression<Func<GoodPrice, bool>>> exprlist = new List<Expression<Func<GoodPrice, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.GoodID.ToString()) && request.GoodID > 0)
                exprlist.Add(u => u.GoodID == request.GoodID);
            PageParameter<GoodPrice> pageParameter = new PageParameter<GoodPrice>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.Sort;
            pageParameter.IsOrderByASC = 1;
            pageParameter.OrderColumns2 = p => p.CreateDate;
            pageParameter.IsOrderByASC2 = 0;
            totalcount = 0;
            return repository.SelectPage<GoodPrice>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 保存商品价格
        /// </summary>
        /// <param name="model"></param>
        public void SaveGoodPrice(GoodPrice model)
        {
            if (model.GoodPriceID > 0)
            {
                repository.Update<GoodPrice>(model);
            }
            else
            {
                repository.Insert<GoodPrice>(model);
            }
        }
        public GoodPrice GetGoodPrice(int id)
        {
            return repository.SelectSearch<GoodPrice>(m => m.GoodPriceID == id).SingleOrDefault();
        }
        public int DelGoodPriceByGoodPriceID(int GoodPriceID)
        {
            return repository.Delete<GoodPrice>(GoodPriceID) ? 1 : 0;
        }
    }
}
