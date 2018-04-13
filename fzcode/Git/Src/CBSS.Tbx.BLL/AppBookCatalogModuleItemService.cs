using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        public DataTable GetAppBookCatalogModuleItemList(int MarketBookID, int MarketBookCatalogID, string AppID)
        {
            string sql = "";
            if (MarketBookCatalogID == 0)
            {
                sql = @" select *from dbo.Module where MarketBookID='" + MarketBookID + "' and Status=1";
            }
            else
            {
                sql = @" select a.*,b.AppBookCatalogModuleItemID,b.MarketBookCatalogID,b.BeforeBuyingImg,b.BuyLaterImg,b.ModuleName'ModuleNameShow',b.Status'StatusShow',b.Sort,b.BeforeBuyingClickImg,b.BuyLaterClickImg 
                            from  Module a left join  (select *from  AppBookCatalogModuleItem where MarketBookCatalogID='" + MarketBookCatalogID + "' and AppID='" + AppID + @"') b
                             on a.ModuleID = b.ModuleID  where MarketBookID = '" + MarketBookID + "' and a.Status=1  order by b.Sort";
            }
            DataTable dt = repository.SelectDataTable(sql);
            return dt;
            //return repository.SelectSearch<v_AppBookCatalogModuleItem>(expression);
        }
        public bool SaveAppBookCatalogModuleItem(AppBookCatalogModuleItem model)
        {
            var list = repository.SelectSearch<AppBookCatalogModuleItem>(a => a.AppBookCatalogModuleItemID == model.AppBookCatalogModuleItemID && a.MarketBookCatalogID == model.MarketBookCatalogID);

            if (model.AppBookCatalogModuleItemID > 0 && list.Count() > 0)
            {
                bool flag = repository.Update<AppBookCatalogModuleItem>(model);
                return flag;
            }
            else
            {
                return Convert.ToInt32(repository.Insert<AppBookCatalogModuleItem>(model)) > 0 ? true : false;
            }
        }
        /// <summary>
        /// 批量插入AppBookCatalogModuleItem表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool BatchSaveAppBookCatalogModuleItem(List<AppBookCatalogModuleItem> list)
        {
            return repository.InsertBatch<AppBookCatalogModuleItem>(list);
        }
        public IEnumerable<AppBookCatalogModuleItem> GetAppBookCatalogModuleItemList(Expression<Func<AppBookCatalogModuleItem, bool>> expression)
        {
            return repository.SelectSearch<AppBookCatalogModuleItem>(expression);
        }
        public bool UpdateAppBookCatalogModuleItem(AppBookCatalogModuleItem entity)
        {
            return repository.Update<AppBookCatalogModuleItem>(entity);
        }
        /// <summary>
        /// 根据条件删除AppBookCatalogModuleItem表
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool DelAppBookCatalogModuleItem(Expression<Func<AppBookCatalogModuleItem, bool>> expression)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    repository.Delete<AppBookCatalogModuleItem>(expression);
                    ts.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 修改AppBookCatalogModuleItem状态
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="MarketBookCatalogID"></param>
        /// <returns></returns>
        public bool UpdateAppBookCatalogModuleItemStatus(string AppID, int MarketBookCatalogID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    repository.SelectString(string.Format("update dbo.AppBookCatalogModuleItem set Status=0 where AppID='{0}' and MarketBookCatalogID='{1}'", AppID, MarketBookCatalogID));
                    ts.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
