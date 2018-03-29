using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class 户型Manager
    {
        public static readonly string 单身公寓 = "单身公寓";
        public static readonly string 两房两厅 = "两房两厅";
        public static readonly string 两房一厅 = "两房一厅";
        public static readonly string 六房 = "六房";
        public static readonly string 七房及以上 = "七房及以上";
        public static readonly string 三房两厅 = "三房两厅";
        public static readonly string 三房一厅 = "三房一厅";
        public static readonly string 四房两厅 = "四房两厅";
        public static readonly string 四房三厅 = "四房三厅";
        public static readonly string 四房一厅 = "四房一厅";
        public static readonly string 五房 = "五房";
        public static readonly string 一房一厅 = "一房一厅";
        public static readonly string 单房 = "单房";
        public static readonly string 一房两厅 = "一房两厅";
        public static readonly string 两房零厅 = "两房零厅";
        public static readonly string 三房零厅 = "三房零厅";
        public static List<SysData_户型> 所有户型;
        static 户型Manager()
        {
            所有户型 = GetAll();
        }
        /// <summary>
        /// 获取所有户型
        /// </summary>
        /// <returns></returns>
        public static List<SysData_户型> GetAll()
        {

            List<SysData_户型> list = new List<SysData_户型>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_户型;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据户型名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get户型_根据名称(string _name)
        {
            int id = 0;
            SysData_户型 obj = 所有户型.Find(delegate(SysData_户型 _obj) { return !string.IsNullOrEmpty(_name) && _name.Equals(_obj.户型); });
            if (obj != null)
            {
                id = obj.ID;
            }
            return id;
        }
    }
}
