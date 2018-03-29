using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity;
using CAS.Entity.BaseDAModels;
using CAS.DataAccess.BaseDAModels;
using System.Reflection;
using System.Configuration;
using CAS.Entity.DBEntity;
namespace CAS.DataAccess.DA
{
    public class Base:BaseDA
    {   
        private const string CITYTABLECACHE = "CITYTABLECACHE";

        /// <summary>
        /// 登录的时候，根据CITYID，第一次加载并缓存对应的数据表名集合。
        /// </summary>
        public static CityTable TableByCity
        {
            set
            {
                string cityid = WebCommon.LoginInfo.cityid.ToString();
                string cache = CITYTABLECACHE + "_" + cityid;
                CacheHelper.Set<CityTable>(cache, value, 60 * 24);
            }
            get
            {
                int cityid = WebCommon.LoginInfo.cityid;
                string cache = CITYTABLECACHE + "_" + cityid.ToString();
                if (!CacheHelper.Contains<CityTable>(cache))
                {
                    CacheHelper.Set<CityTable>(cache, GetCityTable(cityid), 60 * 24);
                }
                return CacheHelper.Get<CityTable>(cache);
            }
        }

        /// <summary>
        /// 取对应的城市表名 kevin
        /// </summary>
        /// <returns></returns>
        private static CityTable GetCityTable(int cityid)
        {
            CityTable table = ExecuteToEntityByPrimaryKey<CityTable>(cityid);
            return table;
        }       
    }
}
