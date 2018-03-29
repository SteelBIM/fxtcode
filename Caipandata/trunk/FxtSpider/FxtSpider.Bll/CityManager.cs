using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.DAL.LinqToSql;
using System.Linq.Expressions;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    public static  class CityManager
    {
        public static List<城市表> 所有城市;
        static CityManager()
        {
            所有城市 = GetAllCity();
        }
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        public static List<城市表> GetAllCity()
        {
            List<城市表> list = new List<城市表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.城市表;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据城市名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int GetCityIdByCityName(string cityName)
        {
            int cityId = 0;
            城市表 city = 所有城市.Find(delegate(城市表 _city) { return !string.IsNullOrEmpty(cityName) && cityName.Equals(_city.城市名称); });
            if (city != null)
            {
                cityId = city.ID;
            }
            return cityId;
        }
        /// <summary>
        /// 根据城市名称获取城市信息
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static 城市表 GetCityByCityName(string cityName)
        {            
            城市表 city = 所有城市.Find(delegate(城市表 _city) { return !string.IsNullOrEmpty(cityName) && cityName.Equals(_city.城市名称); });            
            return city;
        }
        /// <summary>
        /// 根据城市名称获取城市信息
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static 城市表 Get城市_byLike城市名称(string cityName)
        {
            城市表 city = 所有城市.Find(delegate(城市表 _city) { return !string.IsNullOrEmpty(cityName) && cityName.Contains(_city.城市名称); });
            return city;
        }
        public static List<城市表> Get网站爬取配置城市_by网站Id(int webId)
        {
            List<城市表> list = new List<城市表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                //db.城市表.Where().
                var query = from d in db.城市表
                            where (from c in db.网站爬取配置 where c.网站ID == webId select c.城市ID).Contains(d.ID)
                            select d;
                list = query.ToList<城市表>();
            }
            return list;
        }

        public static 城市表 GetCityById(int id)
        {
            城市表 city = 所有城市.Find(delegate(城市表 _city) { return  id==_city.ID; });

            return city;
        }
        /// <summary>
        /// 获取已经加入爬取的城市
        /// </summary>
        /// <returns></returns>
        public static List<城市表> GetSpiderCity(DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            List<城市表> list = new List<城市表>();
            //db.城市表.Where().
            var query = from d in dc.DB.城市表
                        where (from c in dc.DB.网站爬取配置 select c.城市ID).Contains(d.ID)
                        select d;
            list = query.ToList<城市表>();
            dc.Connection_Close();
            dc.Dispose();
            return list;
        }
    }
}
