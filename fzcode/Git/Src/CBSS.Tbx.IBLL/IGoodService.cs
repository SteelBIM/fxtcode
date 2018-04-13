using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface IGoodService
    {
        IEnumerable<Good> GetGoodList(Expression<Func<Good, bool>> expression);
        IEnumerable<v_Good> GetGoodList(out int totalcount, GoodRequest request = null);
        int SaveGood(Good model);
        Good GetGood(int id);
        /// <summary>
        /// 根据GoodID删除商品策略(0：策略被引用不能删除，1：删除成功，2：删除失败)
        /// </summary>
        /// <param name="GoodID"></param>
        /// <returns></returns>
        int DelGoodByGoodID(int GoodID);
    }
}
