using CBSS.Framework.Contract;
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
    public interface IMarketClassifyService
    {
        /// <summary>
        /// 根据parentId获取子市场分类列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<MarketClassify> GetMarketClassifies(int parentId);
        /// <summary>
        /// 获取市场书籍分类信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<MarketClassify> GetMarketClassifyList(Expression<Func<MarketClassify, bool>> expression);
        /// <summary>
        /// 获取商品策略所有的模块
        /// </summary>
        /// <param name="goodid"></param>
        /// <returns></returns>

        IEnumerable<GoodModuleItem> GetGoodModuleItem(int goodid);

        /// <summary>
        /// 获取市场书籍分类信息 (添加AppID)
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<v_AppMarketClassify> GetV_AppMarketClassifyList(Expression<Func<v_AppMarketClassify, bool>> expression);

        /// <summary>
        /// 修改市场分类
        /// </summary>
        /// <param name="model"></param>
        void UpdateClassify(MarketClassify model);
        /// <summary>
        /// 添加市场分类
        /// </summary>
        /// <param name="model"></param>
        int AddClassify(MarketClassify model);
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        KingResponse DeleteClassify(int id);
        /// <summary>
        /// 获取类别全名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fullName"></param>
        void GetFullName(int id, ref string fullName);
    }
   
}
