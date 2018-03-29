using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class 结构Manager
    {
        public static readonly string LOFT = "LOFT";
        public static readonly string 错层 = "错层";
        public static readonly string 复式 = "复式";
        public static readonly string 平面 = "平面";
        public static readonly string 跃式 = "跃式";
        public static List<SysData_结构> 所有结构;
        static 结构Manager()
        {
            所有结构 = GetAll();
        }
        /// <summary>
        /// 获取所有结构
        /// </summary>
        /// <returns></returns>
        public static List<SysData_结构> GetAll()
        {

            List<SysData_结构> list = new List<SysData_结构>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_结构;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据结构名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get结构_根据名称(string _name)
        {
            int id = 0;
            SysData_结构 obj = 所有结构.Find(delegate(SysData_结构 _obj) { return !string.IsNullOrEmpty(_name) && _name.Equals(_obj.结构); });
            if (obj != null)
            {
                id = obj.ID;
            }
            return id;
        }
    }
}
