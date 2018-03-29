using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class  朝向Manager
    {
        public static readonly string 北 = "北";
        public static readonly string 东 = "东";
        public static readonly string 东北 = "东北";
        public static readonly string 东南 = "东南";
        public static readonly string 南 = "南";
        public static readonly string 西 = "西";
        public static readonly string 西北 = "西北";
        public static readonly string 西南 = "西南";
        public static readonly string 南北 = "南北";
        public static readonly string 东西 = "东西";
        public static List<SysData_朝向> 所有朝向;
        static 朝向Manager()
        {
            所有朝向 = GetAll();
        }
        /// <summary>
        /// 获取所有朝向
        /// </summary>
        /// <returns></returns>
        public static List<SysData_朝向> GetAll()
        {

            List<SysData_朝向> list = new List<SysData_朝向>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_朝向;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据朝向名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get朝向_根据名称(string _name)
        {
            int id = 0;
            SysData_朝向 obj = 所有朝向.Find(delegate(SysData_朝向 _obj) { return !string.IsNullOrEmpty(_name) && _name.Equals(_obj.朝向); });
            if (obj != null)
            {
                id = obj.ID;
            }
            return id;
        }
    }
}
