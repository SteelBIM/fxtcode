using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
   public interface IAppBookCatalogModuleItemService
    {
        /// <summary>
        /// 根据书籍ID和目录ID获取对应的模块信息
        /// </summary>
        /// <param name="MarketBookID"></param>
        /// <param name="MarketBookCatalogID"></param>
        /// <returns>Json</returns>
        DataTable GetAppBookCatalogModuleItemList(int MarketBookID, int MarketBookCatalogID, string AppID);
        bool SaveAppBookCatalogModuleItem(AppBookCatalogModuleItem model);
        /// <summary>
        /// 批量插入AppBookCatalogModuleItem表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool BatchSaveAppBookCatalogModuleItem(List<AppBookCatalogModuleItem> list);
        IEnumerable<AppBookCatalogModuleItem> GetAppBookCatalogModuleItemList(Expression<Func<AppBookCatalogModuleItem, bool>> expression);
        /// <summary>
        /// 修改AppBookCatalogModuleItem表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateAppBookCatalogModuleItem(AppBookCatalogModuleItem entity);
        /// <summary>
        /// 根据条件删除AppBookCatalogModuleItem表
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool DelAppBookCatalogModuleItem(Expression<Func<AppBookCatalogModuleItem, bool>> expression);
        /// <summary>
        /// 修改AppBookCatalogModuleItem状态
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="MarketBookCatalogID"></param>
        /// <returns></returns>
        bool UpdateAppBookCatalogModuleItemStatus(string AppID, int MarketBookCatalogID);
    }
}
