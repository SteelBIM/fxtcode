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
    public static class SubAreaManager
    {

        public static readonly ILog log = LogManager.GetLogger(typeof(SubAreaManager));
        /// <summary>
        /// 获取片区
        /// </summary>
        /// <param name="subAreaName">片区名称</param>
        /// <param name="cityId">城市id</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static SysData_SubArea GetAreaByAreaNameByCityId(string subAreaName, int cityId, DataClass _db = null)
        {

            DataClass db = new DataClass(_db);
            SysData_SubArea subArea = null;
            string sql = string.Format("select top 1 * from SysData_SubArea with(nolock) where CityId={0} and (SubAreaName like '%{1}%' or '{1}' like '%'+SubAreaName+'%')", cityId, subAreaName);
            subArea = db.DB.ExecuteQuery<SysData_SubArea>(sql).FirstOrDefault();
            //subArea = db.DB.SysData_SubArea.FirstOrDefault(p => p.CityId == cityId && (SqlMethods.Like(p.SubAreaName, "%" + subAreaName + "%") || SqlMethods.Like(subAreaName, "%" + p.SubAreaName + "%")));
            db.Connection_Close();
            db.Dispose();
            return subArea;
        }
        /// <summary>
        /// 插入一条片区
        /// </summary>
        /// <param name="subAreaName"></param>
        /// <param name="cityId"></param>
        /// <param name="webId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static SysData_SubArea InsertArea(string subAreaName, int cityId, int webId, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_SubArea subArea = new SysData_SubArea();
            subArea.SubAreaName = subAreaName;
            subArea.CityId = cityId;
            subArea.WebId = webId;
            subArea = Insert(subArea);

            db.Connection_Close();
            db.Dispose();
            return subArea;
        }
        /// <summary>
        /// 模糊获取行政区或新增一条片区
        /// </summary>
        /// <param name="subAreaName"></param>
        /// <param name="cityId"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static SysData_SubArea GetLikeOrInsertArea(string subAreaName, int cityId, DataClass _dc = null)
        {
            DataClass dc = new DataClass(_dc);
            SysData_SubArea subArea = GetAreaByAreaNameByCityId(subAreaName, cityId, _db: dc);
            if (subArea == null)
            {
                subArea = InsertArea(subAreaName, cityId, 0, _db: dc);
            }
            dc.Connection_Close();
            dc.Dispose();
            return subArea;
        }
        public static List<SysData_SubArea> GetSubAreaByLikeAndCityId(string subAreaLike, int cityId,int maxCount, DataClass _dc = null)
        {

            DataClass db = new DataClass(_dc);
            List<SysData_SubArea> list = db.DB.SysData_SubArea.Where(p => p.CityId == cityId && (SqlMethods.Like(p.SubAreaName, "%" + subAreaLike + "%") || SqlMethods.Like(subAreaLike, "%" + p.SubAreaName + "%"))).Take(maxCount).ToList();
            db.Connection_Close();
            db.Dispose();
            return list;
        }

        public static SysData_SubArea Insert(SysData_SubArea obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.SysData_SubArea_Insert(obj.SubAreaName, obj.CityId, obj.WebId, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
    }
}
