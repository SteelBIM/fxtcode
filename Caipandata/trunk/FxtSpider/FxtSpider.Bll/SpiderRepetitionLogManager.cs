using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.DB;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class SpiderRepetitionLogManager
    {
        /// <summary>
        /// 记录重复个数
        /// </summary>
        /// <param name="webId"></param>
        /// <param name="cityId"></param>
        /// <param name="startSpiderDate">开始爬取的时间</param>
        /// <param name="_dc"></param>
        public static void SetSpiderRepetitionCount(int webId, int cityId,DateTime startSpiderDate, DataClass _dc = null)
        {
            string date = startSpiderDate.ToString("yyyy-MM-dd");
            int h = Convert.ToInt32(startSpiderDate.ToString("HH"));
            if (h > 12)
            {
                date = startSpiderDate.AddDays(1).ToString("yyyy-MM-dd");
            }
            DataClass dc = new DataClass(_dc);
            string sql = string.Format(" select top 1 * from Dat_SpiderRepetitionLog with(nolock) where CityId={0} and WebId={1} and Date='{2}'", cityId, webId, date);
            //Dat_SpiderRepetitionLog obj = dc.DB.Dat_SpiderRepetitionLog.Where(tbl => tbl.CityId == cityId && tbl.WebId == webId && tbl.Date == date).FirstOrDefault();
            Dat_SpiderRepetitionLog obj = dc.DB.ExecuteQuery<Dat_SpiderRepetitionLog>(sql).FirstOrDefault();
            if (obj == null)
            {
                obj = new Dat_SpiderRepetitionLog { CityId = cityId, Date = date, WebId = webId, RepetitionCount = 1, UpdateTime = DateTime.Now };
                Insert(obj, dc);
            }
            else
            {
                obj.RepetitionCount = obj.RepetitionCount + 1;
                Update(obj, dc);
            }
            dc.Connection_Close();
            dc.Dispose();

        }


        public static Dat_SpiderRepetitionLog Insert(Dat_SpiderRepetitionLog obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.Dat_SpiderRepetitionLog_Insert(obj.WebId,obj.CityId,obj.RepetitionCount,obj.Date,obj.UpdateTime, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
        public static Dat_SpiderRepetitionLog Update(Dat_SpiderRepetitionLog obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                db.DB.Dat_SpiderRepetitionLog_Update(obj.WebId, obj.CityId, obj.RepetitionCount, obj.Date, obj.UpdateTime, obj.ID);
            }
            return obj;
        }
    }
}
