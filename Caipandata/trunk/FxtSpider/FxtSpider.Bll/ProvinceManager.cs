using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class ProvinceManager
    {
        public static List<省份表> 所有省份;
        private static readonly ILog log = LogManager.GetLogger(typeof(ProvinceManager));
        static ProvinceManager()
        {
            所有省份 = GetAllProvince();
        }
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        public static List<省份表> GetAllProvince()
        {
            List<省份表> list = new List<省份表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.省份表;
                list = result.ToList();
            }
            return list;
        }
    }
}
