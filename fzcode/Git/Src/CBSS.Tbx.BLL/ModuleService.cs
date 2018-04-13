using CBSS.Framework.DAL;
using CBSS.Core.Utility;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CBSS.Tbx.IBLL;
using CBSS.Tbx.Contract.ViewModel;
using System.Data;
using System.Transactions;

namespace CBSS.Tbx.BLL
{
    /// <summary>
    /// 模块
    /// </summary>
    public partial class TbxService : ITbxService
    {
        public int SaveModule(Module model)
        {
            if (model.SourceAccessMode==2)
            {
                model.MODSourceType = 0;
            }
            if (model.ModuleID > 0)
            {
                ////验证重名
                //var list = repository.SelectSearch<Module>(a => a.ModuleName == model.ModuleName && a.ModuleID != model.ModuleID);
                //if (list != null && list.Count() > 0)
                //{
                //    return 2;
                //}
                return repository.Update<Module>(model) ? 1 : 0;
            }
            else
            {
                ////验证重名
                //var list = repository.SelectSearch<Module>(a => a.ModuleName == model.ModuleName);
                //if (list != null && list.Count() > 0)
                //{
                //    return 2;
                //}
                return Convert.ToInt32(repository.Insert<Module>(model).ToString()) > 0 ? 1 : 0;
            }
        }

        //public IEnumerable<Module> GetModuleList(ModuleRequest request = null)
        //{
        //    request = request ?? new ModuleRequest();

        //    var Module = repository.ListAll<Module>();

        //    if (!string.IsNullOrEmpty(request.ModuleName))
        //    {
        //        Module = Module.Where(u => u.ModuleName.Contains(request.ModuleName));
        //    }

        //    return Module.OrderByDescending(m => m.ModuleID).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
        //}
        /// <summary>
        /// 获取模块集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<Module> GetModuleList(Expression<Func<Module, bool>> expression)
        {
            return repository.SelectSearch<Module>(expression);
        }
        public IEnumerable<Module> GetModuleList()
        {
            return repository.ListAll<Module>();
        }

        public IEnumerable<Module> GetModuleList(out int totalcount, ModuleRequest request = null)
        {
            request = request ?? new ModuleRequest();

            List<Expression<Func<Module, bool>>> exprlist = new List<Expression<Func<Module, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.ModuleName))
                exprlist.Add(u => u.ModuleName.Contains(request.ModuleName.Trim()));

            PageParameter<Module> pageParameter = new PageParameter<Module>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.ModelID;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<Module>(pageParameter, out totalcount);
        }


        public Module GetModule(int id)
        {
            return repository.SelectSearch<Module>(m => m.ModuleID == id).SingleOrDefault();
        }
        public IEnumerable<Module> GetModule(Expression<Func<Module, bool>> expression)
        {
            return repository.SelectSearch<Module>(expression);
        }
        public void DeleteModule(List<int> ids)
        {
            string[] stringIDs = ids.Select(o => o.ToString()).ToArray();
            repository.DeleteMore<Module>(stringIDs);
        }
        /// <summary>
        /// 模块管理
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<v_ModuleManage> GetModuleManage(out int totalcount, ModuleManageRequest request = null)
        {
            request = request ?? new ModuleManageRequest();

            List<Expression<Func<v_ModuleManage, bool>>> exprlist = new List<Expression<Func<v_ModuleManage, bool>>>();
            exprlist.Add(o => true);
            exprlist.Add(u => u.MarketBookID == request.MarketBookID);
            if (!string.IsNullOrEmpty(request.ModuleName))
            {
                exprlist.Add(u => u.ModuleName.Contains(request.ModuleName.Trim()));
            }
            PageParameter<v_ModuleManage> pageParameter = new PageParameter<v_ModuleManage>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.ModuleID;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<v_ModuleManage>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 根据ModuleID删除模块信息(0：已被引用的模块不能删除，1：删除成功，2：删除失败)
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="MarketBookID"></param>
        /// <returns></returns>
        public int DelModuleByModuleID(int ModuleID, int MarketBookID)
        {
            IEnumerable<AppBookCatalogModuleItem> list = repository.SelectSearch<AppBookCatalogModuleItem>(a => a.ModuleID == ModuleID);
            IEnumerable<GoodModuleItem> listGoodModuleItem = repository.SelectSearch<GoodModuleItem>(a => a.ModuleID == ModuleID);
            if ((list != null && list.Count() > 0)||(listGoodModuleItem != null && listGoodModuleItem.Count() > 0))
            {
                return 0;
            }
            else
            {
                return repository.Delete<Module>(ModuleID) ? 1 : 2;
            }
        }

        public List<v_GoodModuleItem> GetGoodModuleByMarketClassifyId(int GoodID, int MarketBookID)
        {
            string sql = string.Format(@"select a.*,c.MarketClassifyId,case when (d.GoodID>0) then 1 else 0 end 'IsChecked'
 from Module a left join MarketBook b on a.MarketBookID=b.MarketBookID left join 
MarketClassify c on b.MarketClassifyId=c.MarketClassifyID left join 
( select *from GoodModuleItem where GoodID='{0}')
 d on
a.ModuleID=d.ModuleID  where b.MarketBookID={1} ", GoodID, MarketBookID);
            return JsonConvertHelper.JSONStringToList<v_GoodModuleItem>(JsonConvertHelper.DataTableToJson(repository.SelectDataTable(sql)));
        }
        /// <summary>
        /// 根据GoodID删除对应的模块后再批量插入GoodModuleItem表
        /// </summary>
        /// <param name="GoodID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SaveGoodModule(int GoodID, List<GoodModuleItem> list)
        {
            try
            {
                repository.Delete<GoodModuleItem>(a => a.GoodID == GoodID);
                return repository.InsertBatch<GoodModuleItem>(list);


                //using (TransactionScope ts = new TransactionScope())
                //{
                //    repository.Delete<GoodModuleItem>(a=>a.GoodID== GoodID);
                //    repository.InsertBatch<GoodModuleItem>(list);
                //    return true;
                //}
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DelGoodModule(int GoodID)
        {
            try
            {
                repository.Delete<GoodModuleItem>(a => a.GoodID == GoodID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
