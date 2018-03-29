using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll
{
    public static class 用途Manager
    {
        public static readonly string 普通住宅 = "普通住宅";
        public static readonly string 非普通住宅 = "非普通住宅";
        public static readonly string 公寓 = "公寓";
        public static readonly string 别墅 = "别墅";
        public static readonly string 办公 = "办公";
        public static readonly string 补差商品住房 = "补差商品住房";
        public static readonly string 仓储室 = "仓储室";
        public static readonly string 仓库 = "仓库";
        public static readonly string 厂房 = "厂房";
        public static readonly string 车位 = "车位";
        public static readonly string 地下室 = "地下室";
        public static readonly string 叠加别墅 = "叠加别墅";
        public static readonly string 独立别墅 = "独立别墅";
        public static readonly string 花园洋房 = "花园洋房";
        public static readonly string 经济适用房 = "经济适用房";
        public static readonly string 酒店 = "酒店";
        public static readonly string 酒店式公寓 = "酒店式公寓";
        public static readonly string 旧式里弄 = "旧式里弄";
        public static readonly string 老洋房 = "老洋房";
        public static readonly string 联排别墅 = "联排别墅";
        public static readonly string 旅馆 = "旅馆";
        public static readonly string 其他 = "其他";
        public static readonly string 商业 = "商业";
        public static readonly string 商住 = "商住";
        public static readonly string 双拼别墅 = "双拼别墅";
        public static readonly string 新式里弄 = "新式里弄";
        public static readonly string 综合 = "综合";
        public static readonly string 地下室储藏室 = "地下室,储藏室";
        public static readonly string 车库 = "车库";
        public static List<SysData_用途> 所有用途;
        static 用途Manager()
        {
            所有用途 = GetAll();
        }
        /// <summary>
        /// 获取所有用途
        /// </summary>
        /// <returns></returns>
        public static List<SysData_用途> GetAll()
        {

            List<SysData_用途> list = new List<SysData_用途>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.SysData_用途;
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据用途名称获取ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static int Get用途_根据名称(string _name)
        {
            int id = 0;
            SysData_用途 obj = 所有用途.Find(delegate(SysData_用途 _obj) { return !string.IsNullOrEmpty(_name) && _name.Equals(_obj.用途); });
            if (obj != null)
            {
                id = obj.ID;
            }
            return id;
        }
        /// <summary>
        /// 获取所有已经用到的用途
        /// </summary>
        /// <returns></returns>
        public static List<SysData_用途> GetExistsPurpose()
        {
            List<SysData_用途> allList = GetAll();
            string[] ints = new[] {
            "1",//普通住宅
            "2",//非普通住宅
            "27",//别墅
            "5",//独立别墅
            "6",//联排别墅
            "7",//叠加别墅
            "8"//双拼别墅
            };
            List<SysData_用途> list = allList.FindAll(delegate(SysData_用途 obj) { return ints.Contains(obj.code); });
            return list;
        }

        public static SysData_用途 GetById(int id)
        {
            SysData_用途 obj = 所有用途.Find(delegate(SysData_用途 _obj) { return _obj.ID == id; });
            return obj;
        }
    }
}
