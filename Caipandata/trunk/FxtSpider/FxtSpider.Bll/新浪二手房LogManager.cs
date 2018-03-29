using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class 新浪二手房LogManager
    {
        #region (数据库操作)
        public static 新浪二手房_Log 获取新浪二手房_Log(string cityName)
        {
            List<新浪二手房_Log> list=new List<新浪二手房_Log> ();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var exist = db.新浪二手房_Log.Where(p => p.城市 == cityName);
                list = exist.ToList<新浪二手房_Log>();
                if (list == null || list.Count < 1)
                {
                    return null;
                }
            }
            return list[0];

        }
        public static 新浪二手房_Log 初始化新浪二手房_Log(string cityName)
        {
            新浪二手房_Log xlLog = new 新浪二手房_Log { 城市 = cityName, 创建时间 = DateTime.Now, 当前列表页面页码 = null, 当前详细页面Url = null, 更新时间 = null, 开始爬取时间 = DateTime.Now };
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.新浪二手房_Log.InsertOnSubmit(xlLog);
                db.SubmitChanges();
            }
            return xlLog;
        }
        public static void 设置Log(新浪二手房_Log _log)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                新浪二手房_Log exist = db.新浪二手房_Log.Single(p => p.ID == _log.ID);
                exist.城市 = _log.城市;
                exist.创建时间 = _log.创建时间;
                exist.当前列表页面页码 = _log.当前列表页面页码;
                exist.当前详细页面Url = _log.当前详细页面Url;
                exist.更新时间 = _log.更新时间;
                exist.开始爬取时间 = _log.开始爬取时间;
                db.SubmitChanges();
            }
        }
        #endregion
    }
}
