using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    public static class 装修Manager
    {
        public static readonly string 豪华 = "豪华";
        public static readonly string 高档 = "高档";
        public static readonly string 中档 = "中档";
        public static readonly string 普通 = "普通";
        public static readonly string 简易 = "简易";
        public static readonly string 毛坯 = "毛坯";

        static 装修Manager()
        {
        }
        /// <summary>
        /// 获取所有装修
        /// </summary>
        /// <returns></returns>
        public static List<SysData_装修> GetAll()
        {

            List<SysData_装修> list = new List<SysData_装修>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_装修;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据装修名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static SysData_装修 Get装修_根据名称(string _name, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_装修 obj = db.DB.SysData_装修.Where(tbl => tbl.装修 == _name).FirstOrDefault();
            db.Connection_Close();
            db.Dispose();
            return obj;
        }    /// <summary>
        /// 根据装修名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get装修ID_根据名称(string _name, DataClass _db = null)
        {
            int id=0;
            DataClass db = new DataClass(_db);
            SysData_装修 obj = db.DB.SysData_装修.Where(tbl => tbl.装修 == _name).FirstOrDefault();
            if (obj != null)
            {
                id = obj.ID;
            }
            db.Connection_Close();
            db.Dispose();
            return id;
        }
        public static SysData_装修 Insert(string name, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_装修 obj = new SysData_装修 { 装修 = name };
            obj = Insert(obj, db);
            db.Connection_Close();
            db.Dispose();
            return obj;
        }

        public static SysData_装修 Insert(SysData_装修 obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                int nowID = 0;
                db.DB.SysData_装修_Insert(obj.装修, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
    }

}
