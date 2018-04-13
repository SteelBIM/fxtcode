using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
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
        /// 获取市场数据
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<v_MarketBookModule> GetMarketBookList(out int totalcount, MarketBookRequest request = null)
        {
            request = request ?? new MarketBookRequest();
            List<Expression<Func<v_MarketBookModule, bool>>> exprlist = new List<Expression<Func<v_MarketBookModule, bool>>>();
            exprlist.Add(o => true);

            if (request.MarketClassifyId > 0)
            {
                List<int> ids = new List<int> { request.MarketClassifyId.Value };
                GetClassifyIDs(request.MarketClassifyId.Value, ids);

                exprlist.Add(u => ids.Contains(u.MarketClassifyId));
            }

            if (!string.IsNullOrEmpty(request.MarketBookName))
                exprlist.Add(u => u.MarketBookName.Contains(request.MarketBookName.Trim()));

            PageParameter<v_MarketBookModule> pageParameter = new PageParameter<v_MarketBookModule>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            pageParameter.WhereSql = request.MarketClassifyIdList;
            totalcount = 0;
            return repository.SelectPage<v_MarketBookModule>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 通过AppID获取市场书籍
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<v_AppModule> GetMarketBookByAppIDList(out int totalcount, MarketBookRequest request = null)
        {
            request = request ?? new MarketBookRequest();
            List<Expression<Func<v_AppModule, bool>>> exprlist = new List<Expression<Func<v_AppModule, bool>>>();
            exprlist.Add(o => true);

            if (request.MarketClassifyId > 0)
            {
                List<int> ids = new List<int> { request.MarketClassifyId.Value };
                GetClassifyIDs(request.MarketClassifyId.Value, ids);

                exprlist.Add(u => ids.Contains(u.MarketClassifyId));
            }

            if (!string.IsNullOrEmpty(request.MarketBookName))
                exprlist.Add(u => u.MarketBookName.Contains(request.MarketBookName.Trim()));
            if (!string.IsNullOrEmpty(request.AppID))
                exprlist.Add(u => u.AppID.Contains(request.AppID.Trim()));

            PageParameter<v_AppModule> pageParameter = new PageParameter<v_AppModule>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            pageParameter.WhereSql = request.MarketClassifyIdList;
            totalcount = 0;
            return repository.SelectPage<v_AppModule>(pageParameter, out totalcount);
        }

        /// <summary>
        /// 通过AppID获取市场书籍和分类排序
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<v_AppModule> GetMarketBookByAppIDSortList(int SortType, out int totalcount, MarketBookRequest request = null)
        {
            request = request ?? new MarketBookRequest();
            List<Expression<Func<v_AppModule, bool>>> exprlist = new List<Expression<Func<v_AppModule, bool>>>();
            exprlist.Add(o => true);

            if (request.MarketClassifyId > 0)
            {
                List<int> ids = new List<int> { request.MarketClassifyId.Value };
                GetClassifyIDs(request.MarketClassifyId.Value, ids);

                exprlist.Add(u => ids.Contains(u.MarketClassifyId));
            }

            if (!string.IsNullOrEmpty(request.MarketBookName))
                exprlist.Add(u => u.MarketBookName.Contains(request.MarketBookName.Trim()));
            if (!string.IsNullOrEmpty(request.AppID))
                exprlist.Add(u => u.AppID.Contains(request.AppID.Trim()));

            PageParameter<v_AppModule> pageParameter = new PageParameter<v_AppModule>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            if (SortType == 1)
            {
                pageParameter.OrderColumns = p => p.BookSort;
            }
            else if (SortType == 2)
            {
                pageParameter.OrderColumns = p => p.ClassifySort;
            }
            else
            {
                pageParameter.OrderColumns = p => p.CreateDate;
            }
            pageParameter.IsOrderByASC = 1;
            pageParameter.WhereSql = request.MarketClassifyIdList;
            totalcount = 0;
            return repository.SelectPage<v_AppModule>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 根据MarketBookID获取市场书籍实体
        /// </summary>
        /// <param name="MarketBookID"></param>
        /// <returns></returns>
        public MarketBook GetMarketBook(int MarketBookID)
        {
            return repository.SelectSearch<MarketBook>(m => m.MarketBookID == MarketBookID).SingleOrDefault();
        }

        public MarketBook GetMarketBookByModID(int MODBookID)
        {
            return repository.SelectSearch<MarketBook>(o => o.MODBookID == MODBookID).FirstOrDefault();
        }
        public MarketBook GetMarketBookByID(int id)
        {
            var books = repository.SelectSearch<MarketBook>(o => o.MarketBookID == id);
            return books.FirstOrDefault();
        }

        public List<MarketBook> GetMarketBooks(int classifyID)
        {
            return repository.SelectSearch<MarketBook>(x => x.MarketClassifyId == classifyID).ToList();
        }

        public List<v_AppMarketBook> GetAppMarketBooks(int classifyID, string appID)
        {
            return repository.SelectSearch<v_AppMarketBook>(x => x.MarketClassifyId == classifyID && x.AppID == appID).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MarketClassifyIds"></param>
        /// <returns></returns>
        public List<MarketBook> GetMarketBookList(string[] MarketClassifyIds)
        {
            return repository.SelectSearch<MarketBook>(o => MarketClassifyIds.Contains(o.MarketClassifyId.ToString())).ToList();
        }
        /// <summary>
        /// 获取应用下所有分类 
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public IEnumerable<AppMarketClassify> GetMarketMarketClassifyList(string AppID)
        {
            return repository.SelectSearch<AppMarketClassify>(amc => amc.AppID == AppID);
        }
        /// <summary>
        /// 获取应用下所有书籍
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public IEnumerable<AppMarketBook> GetMarketMarketBookList(string AppID)
        {
            return repository.SelectSearch<AppMarketBook>(amc => amc.AppID == AppID);
        }

        /// <summary>
        /// 编辑书籍
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMarketBook(MarketBook model)
        {
            var exist = repository.SelectSearch<MarketBook>(o => o.MODBookID == model.MODBookID&&o.MarketBookID!=model.MarketBookID);
            if (exist.Any())
            {
                throw new Exception("该mod书籍已存在！");
            }
            repository.CustomIgnoreUpdate<MarketBook>(o => o.MarketBookID.ToString(), model, o => o.CreateDate.ToString());
        }
        /// <summary>
        /// 添加书箱
        /// </summary>
        /// <param name="model"></param>
        public int AddMarketBook(MarketBook model)
        {
            model.CreateDate = DateTime.Now;
            var exist = repository.SelectSearch<MarketBook>(o => o.MODBookID == model.MODBookID);
            if (exist.Any())
            {
                throw new Exception("不能添加相同的mod书籍！");
            }
            return Convert.ToInt32(repository.Insert(model));
        }
        public bool AddAppMarketBookBatch(IEnumerable<AppMarketBook> entites)
        {
            try
            {
                return repository.InsertBatch(entites);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 批量向AppMarketClassify表添加数据
        /// </summary>
        /// <param name="entites"></param>
        /// <returns></returns>
        public bool AddAppMarketClassifyBatch(IEnumerable<AppMarketClassify> entites)
        {
            try
            {
                return repository.InsertBatch(entites);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 根据AppID删除AppMarketBook表和AppMarketClassify表
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public bool DelAppMarketBookAppMarketClassify(string AppID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    repository.Delete<AppMarketBook>(a => a.AppID == AppID);
                    repository.Delete<AppMarketClassify>(a => a.AppID == AppID);
                    ts.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public KingResponse DeleteMarketBook(int id)
        {
            var r = new KingResponse();
            try
            {
                var module = repository.SelectSearch<Module>(o => o.MarketBookID == id).FirstOrDefault();

                if (module != null) throw new Exception("模块\"" + module.ModuleName + "\"已使用该书籍，不能删除");

                var appMarketBook = repository.SelectSearch<AppMarketBook>(o => o.MarketBookID == id).FirstOrDefault();
                if (appMarketBook != null)
                    throw new Exception("应用AppID\"" + appMarketBook.AppID + "\"已配置该书籍，不能删除");

                repository.Delete<MarketBook>(o => o.MarketBookID == id);
                r.Success = true;
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            };

            return r;
        }

        private void GetClassifyIDs(int classifyId, List<int> classifyIDs)
        {
            var classifies = repository.SelectSearch<MarketClassify>(o => o.ParentId == classifyId).ToList();
            if (classifies.Any())
            {
                classifyIDs.AddRange(classifies.Select(o => o.MarketClassifyID).ToList());
                classifies.ForEach(o =>
                {
                    GetClassifyIDs(o.MarketClassifyID, classifyIDs);
                });
            }
        }
        /// <summary>
        /// 编辑课本和分类的排序
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public bool UpdateBookClassifySort(string AppID, int MarketBookID, int MarketClassifyID, int BookSort, int ClassifySort)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    string sql_Book = string.Format("update dbo.AppMarketBook set Sort={0} where AppID='{1}' and MarketBookID={2} and MarketClassifyID={3}", BookSort, AppID, MarketBookID, MarketClassifyID);
                    string sql_Classify = string.Format("update  dbo.AppMarketClassify set Sort={0} where AppID='{1}' and MarketClassifyID={2} ", ClassifySort, AppID, MarketClassifyID);
                    repository.SelectString(sql_Book);
                    repository.SelectString(sql_Classify);
                    ts.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<AppMarketBook> GetAppMarketBookList(Expression<Func<AppMarketBook, bool>> expression)
        {
            return repository.SelectSearch<AppMarketBook>(expression);
        }
        public IEnumerable<AppMarketClassify> GetAppMarketClassifyList(Expression<Func<AppMarketClassify, bool>> expression)
        {
            return repository.SelectSearch<AppMarketClassify>(expression);
        }
    }
}
