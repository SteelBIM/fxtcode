using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Manager.Web.Common
{
    public static class MvcWebHelp
    {
        /// <summary>
        /// 根据城市ID顺序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<VIEW_网站爬取配置_城市表_网站表> Sort_CityId_Ase(this List<VIEW_网站爬取配置_城市表_网站表> list)
        {
            if (list == null || list.Count < 1)
            {
                return list;
            }

            list.Sort((x, y) =>
            {
                if (x.城市ID > y.城市ID)
                {
                    return 1;
                }
                if (x.城市ID == y.城市ID)
                {
                    return 0;
                }
                return -1;
            });
            return list;
        }
    }
}