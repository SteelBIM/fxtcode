using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static  class 案例类型Manager
    {
        public static readonly string 买卖报盘 = "买卖报盘";
        public static readonly string 买卖成交 = "买卖成交";
        public static readonly string 拍卖报盘 = "拍卖报盘";
        public static readonly string 拍卖成交 = "拍卖成交";
        public static readonly string 评估案例 = "评估案例";
        public static readonly string 天平方米租报盘 = "天平方米租报盘";
        public static readonly string 天平方米租成交 = "天平方米租成交";
        public static readonly string 月平方米租报盘 = "月平方米租报盘";
        public static readonly string 月平方米租成交 = "月平方米租成交";
        public static List<SysData_案例类型> 所有案例类型;
        static 案例类型Manager()
        {
            所有案例类型 = GetAll();

        }
        /// <summary>
        /// 获取所有案例类型
        /// </summary>
        /// <returns></returns>
        public static List<SysData_案例类型> GetAll()
        {

            List<SysData_案例类型> list = new List<SysData_案例类型>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_案例类型;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据案例名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get案例类型_根据名称(string _name)
        {
            int id = 0;
            SysData_案例类型 obj = 所有案例类型.Find(delegate(SysData_案例类型 _obj) { return !string.IsNullOrEmpty(_name) && _name.Equals(_obj.案例类型); });
            if (obj != null)
            {
                id = obj.ID;
            }
            return id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static SysData_案例类型 GetById(int id)
        {
            SysData_案例类型 obj = 所有案例类型.Find(delegate(SysData_案例类型 _obj) { return _obj.ID == id; });
            return obj;
        }

    }
}
