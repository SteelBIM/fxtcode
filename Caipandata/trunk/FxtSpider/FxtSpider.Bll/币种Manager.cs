using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class 币种Manager
    {
        public static readonly string 港币 = "港币";
        public static readonly string 美元 = "美元";
        public static readonly string 欧元 = "欧元";
        public static readonly string 人民币 = "人民币";
        public static readonly string 英镑 = "英镑";
        public static List<SysData_币种> 所有币种;
        static 币种Manager()
        {
            所有币种 = GetAll();
        }
        /// <summary>
        /// 获取所有币种
        /// </summary>
        /// <returns></returns>
        public static List<SysData_币种> GetAll()
        {

            List<SysData_币种> list = new List<SysData_币种>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_币种;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据币种名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get币种_根据名称(string _name)
        {
            int id = 0;
            SysData_币种 obj = 所有币种.Find(delegate(SysData_币种 _obj) { return !string.IsNullOrEmpty(_name) && _name.Equals(_obj.币种); });
            if (obj != null)
            {
                id = obj.ID;
            }
            return id;
        }
    }
}
