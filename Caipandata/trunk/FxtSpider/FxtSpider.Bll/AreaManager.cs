using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;
using System.Data.Linq.SqlClient;

namespace FxtSpider.Bll
{
    public static class AreaManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(AreaManager));
        /// <summary>
        /// 获取行政区
        /// </summary>
        /// <param name="areaName">行政区名称</param>
        /// <param name="cityId">城市id</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static SysData_Area GetAreaByAreaNameLikeByCityId(string areaName, int cityId, DataClass _db = null)
        {

            DataClass db = new DataClass(_db);
            SysData_Area area = null;
            string sql = string.Format("select top 1 * from SysData_Area with(nolock) where CityId={0} and (AreaName like '%{1}%' or '{1}' like '%'+AreaName+'%')", cityId, areaName);
            area = db.DB.ExecuteQuery<SysData_Area>(sql).FirstOrDefault();
            //area = db.DB.SysData_Area.FirstOrDefault(p => p.CityId == cityId && (SqlMethods.Like(p.AreaName, "%" + areaName + "%") || SqlMethods.Like(areaName, "%" + p.AreaName + "%")));
            db.Connection_Close();
            db.Dispose();
            return area;
        }
        public static SysData_Area GetAreaByAreaNameByCityId(string areaName, int cityId, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_Area area = null;
            area = db.DB.SysData_Area.FirstOrDefault(p => p.CityId == cityId &&p.AreaName==areaName);
            db.Connection_Close();
            db.Dispose();
            return area;
        }
        /// <summary>
        /// 插入一条行政区
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="cityId"></param>
        /// <param name="webId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static SysData_Area InsertArea(string areaName, int cityId, int webId, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_Area area = new SysData_Area();
            area.AreaName = areaName;
            area.CityId = cityId;
            area.WebId = webId;
            area = Insert(area, db);

            db.Connection_Close();
            db.Dispose();
            return area;
        }
        /// <summary>
        /// 模糊获取行政区或新增一条行政区
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="cityId"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static SysData_Area GetLikeOrInsertArea(string areaName, int cityId, DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            SysData_Area area = GetAreaByAreaNameByCityId(areaName, cityId, _db: dc);
            if (area == null)
            {
                area = InsertArea(areaName, cityId, 0, _db: dc);
            }
            dc.Connection_Close();
            dc.Dispose();
            return area;
        }


        public static SysData_Area Insert(SysData_Area obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.SysData_Area_Insert(obj.AreaName, obj.CityId, obj.WebId, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
    }
}
