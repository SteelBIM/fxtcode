using CBSS.Framework.Contract;
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
        /// <summary>
        /// 根据parentId获取子分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<MarketClassify> GetMarketClassifies(int parentId)
        {
            var classify = repository.SelectSearch<MarketClassify>(o => o.ParentId == parentId);
            return classify.ToList();
        }
        public IEnumerable<MarketClassify> GetMarketClassifyList(Expression<Func<MarketClassify, bool>> expression)
        {
            return repository.SelectSearch<MarketClassify>(expression);
        }

        public List<v_AppMarketClassify> GetV_AppMarketClassifyList(Expression<Func<v_AppMarketClassify, bool>> expression)
        {
            return repository.SelectSearch<v_AppMarketClassify>(expression).ToList();
        }

        public void UpdateClassify(MarketClassify model)
        {
            model.ParentId = model.ParentId ?? 0;
            repository.CustomIgnoreUpdate<MarketClassify>(o => o.MarketClassifyID.ToString(), model, o => o.CreateDate.ToString());
        }

        public int AddClassify(MarketClassify model)
        {
            model.ParentId = model.ParentId ?? 0;
            model.CreateDate = DateTime.Now;
            return Convert.ToInt32(repository.Insert(model));
        }

        public void GetFullName(int id, ref string fullName)
        {
            var cur = repository.SelectSearch<MarketClassify>(o => o.MarketClassifyID == id).FirstOrDefault();
            if (cur == null)
            {
                return;
            }
            fullName =string.IsNullOrEmpty(cur.MarketClassifyName)?cur.ModClassifyName: cur.MarketClassifyName + fullName;
            var parent = repository.SelectSearch<MarketClassify>(o => o.MarketClassifyID == cur.ParentId).FirstOrDefault();
            if (parent != null)
            {
                GetFullName(parent.MarketClassifyID, ref fullName);
            }
        }

        public KingResponse DeleteClassify(int id)
        {
            var r = new KingResponse();
            try
            {
                var children = repository.SelectSearch<MarketClassify>(o => o.ParentId == id);
                if (children.Any())
                {
                    r.ErrorMsg = "请先删除子分类！";
                }
                else if (repository.SelectSearch<MarketBook>(o=>o.MarketClassifyId==id).Any())
                {
                    r.ErrorMsg = "该分类下有绑定书籍，不能删除！";
                }
                else
                {
                    repository.Delete<MarketClassify>(o => o.MarketClassifyID == id);
                    r.Success = true;
                }
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message + ex.StackTrace;
            };

            return r;
        }

        public IEnumerable<GoodModuleItem> GetGoodModuleItem(int goodid)
        {
            return repository.SelectSearch<GoodModuleItem>(gmi => gmi.GoodID == goodid);
        }
    }

}
