using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using CBSS.UserOrder.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        /// <summary>
        /// 根据bookID获取所有目录
        /// </summary>
        /// <returns></returns>
        public List<MarketBookCatalog> GetBookCatalog(int bookID)
        {
            var bookCatalogs = repository.SelectSearch<MarketBookCatalog>(o => o.MarketBookID == bookID && o.IsShow == 1);
            return bookCatalogs.ToList();
        }

        public object CheckModulePermission(List<int> moduleIds, string appID, long? userId)
        {
            #region 权限
            //查询需要收费的模块 needPayModules
            var appGoodItem = repository.SelectSearch<AppGoodItem>(o => o.AppID == appID);
            var goodIds = appGoodItem.Select(a => a.GoodID);
            var chargeModules = repository.SelectSearch<GoodModuleItem>(o => goodIds.Contains(o.GoodID));

            //查询用户已购买的模块ID hasBuyModules
            var hasBuyModules = new List<int>();
            if (userId > 0)
            {
                hasBuyModules = tbxRecordRepository.SelectSearch<UserModuleItem>(o => o.UserID == userId && DateTime.Now >= o.StartDate && DateTime.Now <= o.EndDate).Select(o => o.ModuleID).ToList();
                //needPayModules = needPayModules.Where(o => !hasBuyModules.Contains(o.ModuleID));
            }

            var needPayModuleIds = chargeModules.Select(o => o.ModuleID);//需要付款的模块id        

            List<RV_AppBookCatalogModuleItem> modules = new List<RV_AppBookCatalogModuleItem>();
            moduleIds.ForEach(o=> {
                modules.Add(new RV_AppBookCatalogModuleItem { ModelID=o, HasBuy=hasBuyModules.Contains(o)?1:0, IsCharge=needPayModuleIds.Contains(o)?1:0 });
            });

            return modules.Select(o=> new{ o.ModelID,o.HasBuy,o.IsCharge});
            #endregion


        }
        /// <summary>
        /// 根据marketBookCatalogID获取所有目录
        /// </summary>
        /// <returns></returns>
        public List<RV_AppBookCatalogModuleItem> GetCatalogModule(int marketBookCatalogID, string appID, long? userId)
        {
            var guidAppID = Guid.Parse(appID);
            var appBookCatalogModuleItems = repository.SelectSearch<V_AppBookCatalogModuleItem>(o => o.MarketBookCatalogID == marketBookCatalogID
            && o.AppID == guidAppID && o.Status == 1);

            var re = appBookCatalogModuleItems.ToJson().ToObject<List<RV_AppBookCatalogModuleItem>>();

            #region 权限
            //查询需要收费的模块 needPayModules
            var appGoodItem = repository.SelectSearch<AppGoodItem>(o => o.AppID == appID);
            var goodIds = appGoodItem.Select(a => a.GoodID);
            var chargeModules = repository.SelectSearch<GoodModuleItem>(o => goodIds.Contains(o.GoodID));

            //查询用户已购买的模块ID hasBuyModules
            var hasBuyModules = new List<int>();
            if (userId > 0)
            {
                hasBuyModules = tbxRecordRepository.SelectSearch<UserModuleItem>(o => o.UserID == userId && DateTime.Now >= o.StartDate && DateTime.Now <= o.EndDate).Select(o => o.ModuleID).ToList();
               
            }

            var needPayModuleIds = chargeModules.Select(o => o.ModuleID);//需要付款的模块id
            re.ForEach(x =>
            {
                x.IsCharge = needPayModuleIds.Contains(x.ModuleID) ? 1 : 0;
                x.HasBuy = hasBuyModules.Contains(x.ModelID)?1:0;
            });

            var result = re.Where(o => o.ParentModuleID == 0).ToList();
            result.ForEach(o => o.Children = result.Where(r => r.ParentModuleID == o.ModuleID).ToList());
            #endregion
            return result;
        }

        /// <summary>
        /// 根据parentId获取子目录
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<MarketBookCatalog> GetMarketBookCatalogs(int parentId)
        {
            var classify = repository.SelectSearch<MarketBookCatalog>(o => o.ParentCatalogID == parentId);
            return classify.ToList();
        }
        public IEnumerable<MarketBookCatalog> GetMarketBookCatalogsList(Expression<Func<MarketBookCatalog, bool>> expression)
        {

            return repository.SelectSearch<MarketBookCatalog>(expression);
        }

        public void UpdateCatalogStatus(int marketBookCatalogID, int status)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (status == 1)//当前目录的所有父级和子级目录上架
                {
                    List<int> updateCatas = new List<int>();
                    GetAllParentCatalogs(marketBookCatalogID, ref updateCatas);
                    GetAllChildrenCatalogs(marketBookCatalogID, ref updateCatas);
                    var cur = repository.SelectSearch<MarketBookCatalog>(o => o.MarketBookCatalogID == marketBookCatalogID).FirstOrDefault();
                    cur.IsShow = 1;
                    repository.CustomUpdateEntity(o => o.MarketBookCatalogID.ToString(), cur, o => o.IsShow.ToString());
                    var parentCatas = repository.SelectSearch<MarketBookCatalog>(o => updateCatas.Contains(o.MarketBookCatalogID)).ToList();
                    parentCatas.ForEach(o => o.IsShow = 1);
                    repository.CustomUpdateRange<MarketBookCatalog>(o => o.MarketBookCatalogID.ToString(), parentCatas, o => o.IsShow.ToString());
                }
                else//当前目录和子目录下架
                {
                    List<int> childrens = new List<int>();
                    GetAllChildrenCatalogs(marketBookCatalogID, ref childrens);
                    var cur = repository.SelectSearch<MarketBookCatalog>(o => o.MarketBookCatalogID == marketBookCatalogID).FirstOrDefault();
                    cur.IsShow = 0;
                    repository.CustomUpdateEntity(o => o.MarketBookCatalogID.ToString(), cur, o => o.IsShow.ToString());
                    var childrenCatas = repository.SelectSearch<MarketBookCatalog>(o => childrens.Contains(o.MarketBookCatalogID)).ToList();

                    childrenCatas.ForEach(o => o.IsShow = 0);

                    repository.CustomUpdateRange(o => o.MarketBookCatalogID.ToString(), childrenCatas, o => o.IsShow.ToString());
                }
                scope.Complete();
            }


        }

        private void GetAllParentCatalogs(int marketBookCatalogID, ref List<int> parents)
        {
            var cur = repository.SelectSearch<MarketBookCatalog>(o => o.MarketBookCatalogID == marketBookCatalogID).FirstOrDefault();
            if (cur != null)
            {
                var parent = repository.SelectSearch<MarketBookCatalog>(o => o.MarketBookCatalogID == cur.ParentCatalogID).FirstOrDefault();
                if (parent != null)
                {
                    parents.Add(parent.MarketBookCatalogID);
                    GetAllParentCatalogs(parent.MarketBookCatalogID, ref parents);
                }

            }
        }

        private void GetAllChildrenCatalogs(int MarketBookCatalogID, ref List<int> childrens)
        {
            var childrenList = repository.SelectSearch<MarketBookCatalog>(o => o.ParentCatalogID == MarketBookCatalogID).ToList();
            if (childrenList != null && childrenList.Any())
            {
                childrens.AddRange(childrenList.Select(o => o.MarketBookCatalogID));

                foreach (var c in childrenList)
                {
                    GetAllChildrenCatalogs(c.MarketBookCatalogID, ref childrens);
                }
            }
        }

        public MarketBookCatalog GetMarketBookCatalogByModId(int? modId)
        {
            return repository.SelectSearch<MarketBookCatalog>(o => o.MODBookCatalogID == modId).FirstOrDefault();
        }
        public void UpdateMarketBookCatalog(MarketBookCatalog model)
        {
            repository.CustomIgnoreUpdate<MarketBookCatalog>(o => o.MarketBookCatalogID.ToString(), model, o => o.CreateDate.ToString());
        }

        public void UpdateCatalogCover(int catalogId, string coverUrl)
        {
            repository.CustomUpdateEntity<MarketBookCatalog>(o => o.MarketBookCatalogID.ToString(), new MarketBookCatalog { MarketBookCatalogID = catalogId, MarketBookCatalogCover = coverUrl }, o => o.MarketBookCatalogCover.ToString());
        }

        public int AddMarketBookCatalog(MarketBookCatalog model)
        {
            model.ParentCatalogID = model.ParentCatalogID ?? 0;
            model.CreateDate = DateTime.Now;
            return Convert.ToInt32(repository.Insert(model));
        }

        //public void GetFullName(int id, ref string fullName)
        //{
        //    var cur = repository.SelectSearch<MarketBookCatalogs>(o => o.MarketBookCatalogsID == id).FirstOrDefault();
        //    if (cur == null) {
        //        return;
        //    }
        //    fullName = cur.MarketBookCatalogsName + fullName;
        //    var parent = repository.SelectSearch<MarketBookCatalogs>(o => o.MarketBookCatalogsID == cur.ParentId).FirstOrDefault();
        //    if (parent != null)
        //    {
        //        GetFullName(parent.MarketBookCatalogsID, ref fullName);
        //    }
        //}

        public KingResponse DeleteMarketBookCatalog(int id)
        {
            var r = new KingResponse();
            try
            {
                var children = repository.SelectSearch<MarketBookCatalog>(o => o.ParentCatalogID == id);
                if (children.Any())
                {
                    r.ErrorMsg = "请先删除子目录！";
                }
                else
                {
                    repository.Delete<MarketBookCatalog>(o => o.MarketBookCatalogID == id);
                    r.Success = true;
                }
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message + ex.StackTrace;
            };

            return r;
        }
    }

}
