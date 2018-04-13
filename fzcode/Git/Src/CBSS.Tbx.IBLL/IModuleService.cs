using CBSS.Tbx.Contract;
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
    /// <summary>
    /// 模块
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// 保存(0:失败，1:成功，2:重名)
        /// </summary>
        /// <param name="model"></param>
        int SaveModule(Module model);

        IEnumerable<Module> GetModuleList();
        /// <summary>
        /// 获取模块集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<Module> GetModuleList(Expression<Func<Module, bool>> expression);
        IEnumerable<Module> GetModuleList(out int totalaount, ModuleRequest request = null);
        Module GetModule(int id);

        void DeleteModule(List<int> ids);

        IEnumerable<v_ModuleManage> GetModuleManage(out int totalcount, ModuleManageRequest request = null);
        int DelModuleByModuleID(int ModuleID, int MarketBookID);
        IEnumerable<Module> GetModule(Expression<Func<Module, bool>> expression);
        /// <summary>
        /// 根据MarketBookID获取模块信息
        /// </summary>
        /// <param name="MarketBookID"></param>
        /// <param name="GoodID"></param>
        /// <returns></returns>
        List<v_GoodModuleItem> GetGoodModuleByMarketClassifyId(int GoodID, int MarketBookID);

        /// <summary>
        /// 根据GoodID删除对应的模块后再批量插入GoodModuleItem表
        /// </summary>
        /// <param name="GoodID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        bool SaveGoodModule(int GoodID, List<GoodModuleItem> list);
        /// <summary>
        /// 根据GoodID删除GoodModuleItem表
        /// </summary>
        /// <param name="GoodID"></param>
        /// <returns></returns>
        bool DelGoodModule(int GoodID);
    }
}
