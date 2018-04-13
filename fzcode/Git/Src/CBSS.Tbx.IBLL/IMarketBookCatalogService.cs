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
    public interface IMarketMarketBookCatalogService
    {
        /// <summary>
        /// 根据marketBookCatalogID获取目录模块
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        List<RV_AppBookCatalogModuleItem> GetCatalogModule(int marketBookCatalogID,string appID,long? userId);
        /// <summary>
        /// 判断模块权限
        /// </summary>
        /// <param name="moduleIds"></param>
        /// <param name="appID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        object CheckModulePermission(List<int> moduleIds, string appID, long? userId);
        /// <summary>
        /// 根据bookID获取所有的目录
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        List<MarketBookCatalog> GetBookCatalog(int bookID);

        /// <summary>
        /// 根据parentId获取子市场目录列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<MarketBookCatalog> GetMarketBookCatalogs(int parentId);
        /// <summary>
        /// 获取市场书籍目录信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<MarketBookCatalog> GetMarketBookCatalogsList(Expression<Func<MarketBookCatalog, bool>> expression);
        /// <summary>
        /// 修改市场目录
        /// </summary>
        /// <param name="model"></param>
        void UpdateMarketBookCatalog(MarketBookCatalog model);
        /// <summary>
        /// 添加市场目录
        /// </summary>
        /// <param name="model"></param>
        int AddMarketBookCatalog(MarketBookCatalog model);
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        KingResponse DeleteMarketBookCatalog(int id);

        /// <summary>
        /// 获取书本目录名称
        /// </summary>
        /// <returns></returns>
        MarketBookCatalog GetMarketBookCatalogByModId(int? modId);

        /// <summary>
        /// 目录上下架
        /// </summary>
        /// <param name="marketBookCatalogID"></param>
        /// <param name="status"></param>
        void UpdateCatalogStatus(int marketBookCatalogID, int status);

        /// <summary>
        /// 更新目录封面
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="coverUrl"></param>
        void UpdateCatalogCover(int catalogId, string coverUrl);
        ///// <summary>
        ///// 获取类别全名
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="fullName"></param>
        //void GetFullName(int id, ref string fullName);
    }

}
