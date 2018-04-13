using CBSS.Framework.DAL;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CBSS.Tbx.BLL
{
    public class TreeData
    {

        /// <summary>
        /// 加载所有分类
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="MarketClassifyList"></param>
        /// <returns></returns>
        public List<TreeView> GetMarketClassifyNode(int MarketClassifyID, List<MarketClassify> MarketClassifyList, bool IsMarkBook = false)
        {
            List<TreeView> treeview = new List<TreeView>();
            if (IsMarkBook)//读取到书籍
            {
                // Predicate<MarketClassify> dept = delegate (MarketClassify p) { return p.ParentId == MarketClassifyID; };
                List<MarketClassify> nodelist = MarketClassifyList.Where(ma => ma.ParentId == MarketClassifyID).ToList();
                if (nodelist != null && nodelist.Count > 0)
                {
                    for (int i = 0; i < nodelist.Count; i++)
                    {
                        List<TreeView> tv = GetMarketClassifyNode(nodelist[i].MarketClassifyID, MarketClassifyList, IsMarkBook);

                        bool isContainNods = false;
                        if (tv != null && tv.Count > 0)
                            isContainNods = true;

                        treeview.Add(new TreeView()
                        {
                            tag = nodelist[i].MarketID.ToString(),
                            ParentId = nodelist[i].ParentId.ToString(),
                            //text = "<span onclick=\"itemOnclick(" + nodelist[i].MarketClassifyID.ToString() + ")\">" + GetClassifyName(nodelist[i].MarketClassifyName, nodelist[i].ModClassifyName) + "</span>",
                            text = "<span>" + GetClassifyName(nodelist[i].MarketClassifyName, nodelist[i].ModClassifyName) + "</span>",
                            nodes = tv,
                            isContainNods = isContainNods
                        });
                    }
                }
                else//没有子分类，读取书籍
                {
                    List<MarketBook> marketBookList = new TbxService().GetMarketBooks(MarketClassifyID);
                    if (marketBookList != null && marketBookList.Count() > 0)
                    {
                        for (int i = 0; i < marketBookList.Count; i++)
                        {
                            treeview.Add(new TreeView()
                            {
                                tag = marketBookList[i].MarketBookID.ToString(),
                                ParentId = "0",
                                text = "<span onclick=\"itemOnclick(" + marketBookList[i].MarketBookID + ")\">" + marketBookList[i].MarketBookName + "</span>",
                                nodes = null,
                                isContainNods = false
                            });
                        }
                    }
                }
            }
            else
            {
                // Predicate<MarketClassify> dept = delegate (MarketClassify p) { return p.ParentId == MarketClassifyID; };
                List<MarketClassify> nodelist = MarketClassifyList.Where(ma => ma.ParentId == MarketClassifyID).ToList();
                if (nodelist != null && nodelist.Count > 0)
                {
                    for (int i = 0; i < nodelist.Count; i++)
                    {
                        List<TreeView> tv = GetMarketClassifyNode(nodelist[i].MarketClassifyID, MarketClassifyList, IsMarkBook);

                        bool isContainNods = false;
                        if (tv != null && tv.Count > 0)
                            isContainNods = true;

                        treeview.Add(new TreeView() { tag = nodelist[i].MarketID.ToString(), ParentId = nodelist[i].ParentId.ToString(), text = "<span onclick=\"itemOnclick(" + nodelist[i].MarketClassifyID.ToString() + ")\">" + GetClassifyName(nodelist[i].MarketClassifyName, nodelist[i].ModClassifyName) + "</span>", nodes = tv, isContainNods = isContainNods });
                    }
                }
            }
            return treeview;
        }

        private string GetClassifyName(string MarketClassifyName, string ModClassifyName)
        {
            if (!string.IsNullOrEmpty(MarketClassifyName))
                return MarketClassifyName;
            return ModClassifyName;
        }

        List<int> MarketClassifyIDList = new List<int>();
        /// <summary>
        /// 获取分类下所有子分类ID
        /// </summary>
        /// <param name="MarketClassifyID"></param>
        /// <param name="MarketClassifyList"></param>
        /// <returns></returns>
        public List<int> GetMarketClassifyID(int? MarketClassifyID, List<MarketClassify> MarketClassifyList)
        {

            // Predicate<MarketClassify> dept = delegate (MarketClassify p) { return p.ParentId == MarketClassifyID; };
            List<MarketClassify> nodelist = MarketClassifyList.Where(ma => ma.ParentId == MarketClassifyID).ToList();
            if (nodelist != null && nodelist.Count > 0)
            {
                for (int i = 0; i < nodelist.Count; i++)
                {
                    MarketClassifyIDList.Add(nodelist[i].MarketClassifyID);
                    GetMarketClassifyID(nodelist[i].MarketClassifyID, MarketClassifyList);
                }
            }
            return MarketClassifyIDList;
        }


        string MarketClassNmaes = "";

        /// <summary>
        /// 获取完整书籍分类名称
        /// </summary>
        /// <param name="MarketClassifyId"></param>
        /// <param name="MarketClassifyList"></param>
        /// <returns></returns>
        public string GetMarketClassName(int MarketClassifyId, List<MarketClassify> MarketClassifyList)
        {
            MarketClassNmaes = "";
            GetMarketClassNames(MarketClassifyId, MarketClassifyList);
            return MarketClassNmaes.TrimStart('_');
        }
        /// <summary>
        /// 获取完整书籍分类名称
        /// </summary>
        /// <param name="MarketClassifyId"></param>
        /// <param name="MarketClassifyList"></param>
        /// <returns></returns>
        public void GetMarketClassNames(int MarketClassifyId, List<MarketClassify> MarketClassifyList)
        {
            if (MarketClassifyId > 0)
            {
                MarketClassify marketList = MarketClassifyList.Where(a => a.MarketClassifyID == MarketClassifyId).FirstOrDefault();
                if (marketList != null)
                {
                    MarketClassNmaes += string.IsNullOrEmpty(marketList.MarketClassifyName)?marketList.ModClassifyName: marketList.MarketClassifyName;
                    GetMarketClassNames(Convert.ToInt32(marketList.ParentId), MarketClassifyList);
                }
            }
        }

    }
}
