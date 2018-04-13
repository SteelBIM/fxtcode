using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.BLL
{
    public static class ExtendService
    {
        /// <summary>
        /// 全部目录，递归映射，生成父级包含子级
        /// </summary>
        /// <param name="catalogues"></param>
        /// <returns></returns>
        public static List<V_MarketBookCatalog> CatalogRecursive(this List<MarketBookCatalog> catalogs, int parentCatalogID)
        {
            var res = catalogs.Where(o => o.ParentCatalogID == parentCatalogID).ToList();
            if (res.Count == 0)
            {
                return new List<V_MarketBookCatalog>();//返回空list终止递归
            }
            return res.Select(o => new V_MarketBookCatalog
            {
                MarketBookCatalogCover = o.MarketBookCatalogCover,
                MarketBookCatalogID = o.MarketBookCatalogID,
                MarketBookCatalogName = o.MarketBookCatalogName,
                MarketBookID = o.MarketBookID,
                MODBookCatalogID = o.MODBookCatalogID,
                ParentCatalogID = o.ParentCatalogID,
                CreateDate = o.CreateDate,
                CreateUser = o.CreateUser,
                 EndPage=o.EndPage,
                  StartPage=o.StartPage,
                V_MarketBookCatalogs = CatalogRecursive(catalogs, o.MarketBookCatalogID)
            }).ToList();
        }

        /// <summary>
        /// 全部类别，递归判断，是否每一级都只有一层
        /// </summary>
        /// <param name="catalogues"></param>
        /// <returns></returns>
        public static List<v_AppMarketClassify> Judge(this List<v_AppMarketClassify> classifys, int parentID)
        {
            var res = classifys.Where(o => o.ParentId == parentID).ToList();

            if (res.Count == 0)
            {
                return new List<v_AppMarketClassify>();
            }

            if (res.Count == 1)
            {
                var judgeRes = classifys.Judge(res.First().MarketClassifyID);
                if (judgeRes.Count == 0)
                {
                    return res;
                }
                else
                {
                    return judgeRes;
                }
            }
            else
            {
                return res;
            }
        }


        public static void Add<T>(this List<T> list,T addItem)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            list.Add(addItem);
        }
    }
}
