using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class 建筑类型Manager
    {
        public static readonly string 低层 = "低层";
        public static readonly string 多层 = "多层";
        public static readonly string 高层 = "高层";
        public static readonly string 小高层 = "小高层";
        public static List<SysData_建筑类型> 所有建筑类型;
        static 建筑类型Manager()
        {
            所有建筑类型 = GetAll();
        }
        /// <summary>
        /// 获取所有建筑类型
        /// </summary>
        /// <returns></returns>
        public static List<SysData_建筑类型> GetAll()
        {

            List<SysData_建筑类型> list = new List<SysData_建筑类型>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_建筑类型;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据建筑类型名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get建筑类型_根据名称(string _name)
        {
            int id = 0;
            SysData_建筑类型 obj = 所有建筑类型.Find(delegate(SysData_建筑类型 _obj) { return !string.IsNullOrEmpty(_name) && _name.Equals(_obj.建筑类型); });
            if (obj != null)
            {
                id = obj.ID;
            }
            return id;
        }
    }
}
