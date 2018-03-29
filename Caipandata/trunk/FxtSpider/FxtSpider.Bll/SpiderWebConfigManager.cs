using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using System.Data;

namespace FxtSpider.Bll
{
    public static class SpiderWebConfigManager
    {
        public static List<网站爬取配置> 根据城市获取搜房网爬取配置(string cityName)
        {
            List<网站爬取配置> list = new List<网站爬取配置>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.搜房网_ID);
                list = query.ToList<网站爬取配置>();              
            }
            return list;
        }
        public static 网站爬取配置 根据城市获取安居客爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>(); 
               var  query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.安居客_ID);
               list = query.ToList<网站爬取配置>();
               if (list != null && list.Count > 0)
               {
                   result = list[0];
               }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取新浪二手房爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.新浪二手房_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取赶集网爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.赶集网_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取五八同城爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.五八同城_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取黄石信息港爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.黄石信息港_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取住在九江爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.住在九江_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取城市房产爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.城市房产_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取河源置业网爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.河源置业网_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取邯郸恋家网爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.邯郸恋家网_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取常州房产网爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.常州房产网_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取楼盘网爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.楼盘网_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取搜狐二手房爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.搜狐二手房_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取满堂红地产网爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.满堂红地产网_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取置家网爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.置家网_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取中国房产超市爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.中国房产超市_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 根据城市获取中原地产爬取配置(string cityName)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == WebsiteManager.中原地产_ID);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }

        public static List<VIEW_网站爬取配置_城市表_网站表> 获取搜房网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.搜房网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取安居客下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.安居客_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取新浪二手房下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.新浪二手房_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取赶集网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.赶集网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取五八同城下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.五八同城_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取黄石信息港下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.黄石信息港_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取住在九江下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.住在九江_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取城市房产下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.城市房产_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取河源置业网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.河源置业网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取邯郸恋家网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.邯郸恋家网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取常州房产网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.常州房产网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取楼盘网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.楼盘网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取搜狐二手房下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.搜狐二手房_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取满堂红地产网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.满堂红地产网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取置家网下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.置家网_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取中国房产超市下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.中国房产超市_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> 获取中原地产下所有城市爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表.Where(p => p.网站ID == WebsiteManager.中原地产_ID);
                list = result.ToList<VIEW_网站爬取配置_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_网站爬取配置_城市表_网站表> GetAllVIEW网站爬取配置()
        {
            List<VIEW_网站爬取配置_城市表_网站表> list = new List<VIEW_网站爬取配置_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.VIEW_网站爬取配置_城市表_网站表;
                list = result.ToList();
            }
            return list;
        }

        public static 网站爬取配置 get网站爬取配置_by城市ID_网站ID(int cityId, int webId)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市ID == cityId && p.网站ID == webId);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }
        public static 网站爬取配置 get网站爬取配置_by城市名称_网站ID(string cityName, int webId)
        {
            网站爬取配置 result = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                List<网站爬取配置> list = new List<网站爬取配置>();
                var query = db.网站爬取配置.Where(p => p.城市表.城市名称 == cityName && p.网站ID == webId);
                list = query.ToList<网站爬取配置>();
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            return result;
        }



    }
}
