using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtSpider.DAL.DB;
using FxtSpider.DAL.LinqToSql;
using System.Data;
using System.Data.Linq;

namespace FxtSpider.Bll
{
    public static class DatSpiderErrorLogManager
    {
        /// <summary>
        /// 记录爬取时错误信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="webId"></param>
        /// <param name="url"></param>
        /// <param name="errorCode"></param>
        /// <param name="remark"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static bool InsertError(int cityId, int webId, string url,int errorCode, string remark, DataClass _dc = null)
        {
            if (cityId <= 0 || webId <= 0)
            {
                return false;
            }
            DataClass dc = new DataClass(_dc);
            Dat_SpiderErrorLog obj = new Dat_SpiderErrorLog
            {
                CityId = cityId,
                WebId = webId,
                Url = url,
                Remark = remark,
                CreateTime = DateTime.Now,
                ErrorTypeCode = errorCode
            };
            Insert(obj, dc);
            dc.Connection_Close();
            dc.Dispose();
            return true;
        }

        #region (查询)
        /// <summary>
        /// 获取爬取失败的信息信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="webId"></param>
        /// <param name="startDate">失败时间</param>
        /// <param name="endDate">失败时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <param name="count"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static List<Dat_SpiderErrorLog> GetDatSpiderErrorLogByWebIdAndCityIdAndDate(int cityId, int webId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, bool isGetCount, out int count,DataClass _dc=null)
        {
            count = 0;
            DataClass dc = new DataClass(_dc);
            var query = dc.DB.Dat_SpiderErrorLog.Where(tbl => tbl.CityId == cityId && tbl.WebId == webId && tbl.CreateTime >= startDate && tbl.CreateTime <= endDate);
            if (isGetCount)
            {
                count = query.Count();
            }
            List<Dat_SpiderErrorLog> list = query.OrderByDescending(tbl => tbl.ID)
                    .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                    .ToList<Dat_SpiderErrorLog>();
            dc.Connection_Close();
            dc.Dispose();
            return list;
        }
        /// <summary>
        /// 获取爬取失败的次数
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="webId"></param>
        /// <param name="errorCode"></param>
        /// <param name="startDate">失败时间</param>
        /// <param name="endDate">失败时间</param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static int GetDatSpiderErrorLogCountByWebIdAndCityIdAndErrorCodeAndDate(int cityId, int webId, int errorCode, DateTime startDate, DateTime endDate, DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            var query = dc.DB.Dat_SpiderErrorLog.Where(tbl => tbl.CityId == cityId && tbl.WebId == webId && tbl.ErrorTypeCode == errorCode && tbl.CreateTime >= startDate && tbl.CreateTime <= endDate);
            int count = query.Count();
            dc.Connection_Close();
            dc.Dispose();
            return count;
        }
        #endregion



        public static Dat_SpiderErrorLog Insert(Dat_SpiderErrorLog obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.Dat_SpiderErrorLog_Insert(obj.WebId, obj.CityId,obj.Url,obj.ErrorTypeCode,obj.CreateTime,obj.Remark, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
        public static Dat_SpiderErrorLog Update(Dat_SpiderErrorLog obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                db.DB.Dat_SpiderErrorLog_Update(obj.WebId, obj.CityId, obj.Url, obj.ErrorTypeCode, obj.CreateTime, obj.Remark, obj.ID);
            }
            return obj;
        }
    }
}
