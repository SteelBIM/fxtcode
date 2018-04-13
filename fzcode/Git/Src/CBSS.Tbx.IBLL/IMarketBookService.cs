using CBSS.Framework.Contract;
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
    public interface IMarketBookService
    {
        /// <summary>
        /// 获取市场书籍信息
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<v_MarketBookModule> GetMarketBookList(out int totalcount, MarketBookRequest request = null);
        /// <summary>
        /// 通过AppID获取市场书籍
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<v_AppModule> GetMarketBookByAppIDList(out int totalcount, MarketBookRequest request = null);

        /// <summary>
        /// 通过AppID获取市场书籍和分类排序
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<v_AppModule> GetMarketBookByAppIDSortList(int SortType, out int totalcount, MarketBookRequest request = null);

        /// <summary>
        /// 更新市场书籍
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void UpdateMarketBook(MarketBook model);
        /// <summary>
        /// 添加市场书籍
        /// </summary>
        /// <param name="model"></param>
        int AddMarketBook(MarketBook model);

        /// <summary>
        /// 添加市场书籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        KingResponse DeleteMarketBook(int id);

        /// <summary>
        /// 获取市场书籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MarketBook GetMarketBookByID(int id);

        /// <summary>
        /// 根据MarketBookID获取市场书籍实体
        /// </summary>
        /// <param name="MarketBookID"></param>
        /// <returns></returns>
        MarketBook GetMarketBook(int MarketBookID);

        /// <summary>
        /// 根据MODBOOKID获取市场书籍实体
        /// </summary>
        /// <param name="MarketBookID"></param>
        /// <returns></returns>
        MarketBook GetMarketBookByModID(int MODBookID);

        /// <summary>
        /// 根据市场分类获取书籍
        /// </summary>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        List<MarketBook> GetMarketBooks(int classifyID);


        /// <summary>
        /// 根据市场分类获取书籍
        /// </summary>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        List<v_AppMarketBook> GetAppMarketBooks(int classifyID,string appID);

        ///// <summary>
        ///// 根据书籍分类ID判断是否选择
        ///// </summary>
        ///// <param name="MarketClassifyId"></param>
        ///// <returns></returns>
        //bool GetMarketBookCkByMarketClassifyId(int MarketClassifyId,string AppID);
        ///// <summary>
        ///// 判断书籍ID是否选择
        ///// </summary>
        ///// <param name="MarketBookId"></param>
        ///// <param name="AppID"></param>
        ///// <returns></returns>
        //bool GetMarketBookCkByMarketBookId(int MarketBookId, string AppID);

        /// <summary>
        /// 获取应用下所有分类 
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        IEnumerable<AppMarketClassify> GetMarketMarketClassifyList(string AppID);

        /// <summary>
        ///  获取应用下所有书籍
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        IEnumerable<AppMarketBook> GetMarketMarketBookList(string AppID);



        /// <summary>
        /// 根据市场分类ID集合获取书籍信息
        /// </summary>
        /// <param name="MarketClassifyIds"></param>
        /// <returns></returns>
        List<MarketBook> GetMarketBookList(string[] MarketClassifyIds);
        /// <summary>
        /// 批量添加AppModuleItem应用选择
        /// </summary>
        /// <param name="entites"></param>
        /// <returns></returns>
        bool AddAppMarketBookBatch(IEnumerable<AppMarketBook> entites);
        /// <summary>
        /// 批量向AppMarketClassify表添加数据
        /// </summary>
        /// <param name="entites"></param>
        /// <returns></returns>
        bool AddAppMarketClassifyBatch(IEnumerable<AppMarketClassify> entites);
        /// <summary>
        /// 根据AppID删除AppMarketBook表和AppMarketClassify表
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        bool DelAppMarketBookAppMarketClassify(string AppID);
        /// <summary>
        /// 编辑课本和分类的排序
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        bool UpdateBookClassifySort(string AppID, int MarketBookID, int MarketClassifyID, int BookSort, int ClassifySort);
        IEnumerable<AppMarketBook> GetAppMarketBookList(Expression<Func<AppMarketBook, bool>> expression);
        IEnumerable<AppMarketClassify> GetAppMarketClassifyList(Expression<Func<AppMarketClassify, bool>> expression);
    }
}
