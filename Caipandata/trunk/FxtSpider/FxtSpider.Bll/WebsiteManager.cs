using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    public static class WebsiteManager
    {
        /// <summary>
        /// 封IP类型:需要验证码
        /// </summary>
        public const int BlockadeOfIPType1 = 1;
        /// <summary>
        /// 封IP类型:拒绝访问
        /// </summary>
        public const int BlockadeOfIPType2 = 2;
        public static readonly List<网站表> 所有网站;
        public static readonly string 搜房网 = "搜房网";
        public static readonly string 安居客 = "安居客";
        public static readonly string 新浪二手房 = "新浪二手房";
        public static readonly string 赶集网 = "赶集网";
        public static readonly string 五八同城 = "58同城";
        public static readonly string 黄石信息港 = "黄石信息港";
        public static readonly string 住在九江 = "住在九江";
        public static readonly string 城市房产 = "城市房产";
        public static readonly string 河源置业网 = "河源置业网";
        public static readonly string 邯郸恋家网 = "邯郸恋家网";
        public static readonly string 常州房产网 = "常州房产网";
        public static readonly string 楼盘网 = "楼盘网";
        public static readonly string 搜狐二手房 = "搜狐二手房";
        public static readonly string 满堂红地产网 = "满堂红地产网";
        public static readonly string 置家网 = "置家网";
        public static readonly string 中国房产超市 = "中国房产超市";
        public static readonly string 中原地产 = "中原地产";
        public static readonly int 搜房网_ID;
        public static readonly int 安居客_ID ;
        public static readonly int 新浪二手房_ID;
        public static readonly int 赶集网_ID;
        public static readonly int 五八同城_ID;
        public static readonly int 黄石信息港_ID;
        public static readonly int 住在九江_ID;
        public static readonly int 城市房产_ID;
        public static readonly int 河源置业网_ID;
        public static readonly int 邯郸恋家网_ID;
        public static readonly int 常州房产网_ID;
        public static readonly int 楼盘网_ID;
        public static readonly int 搜狐二手房_ID;
        public static readonly int 满堂红地产网_ID;
        public static readonly int 置家网_ID;
        public static readonly int 中国房产超市_ID;
        public static readonly int 中原地产_ID;
        
        static WebsiteManager()
        {
            所有网站 = GetAllWebsite();
            搜房网_ID = 0;
            安居客_ID = 0;
            新浪二手房_ID = 0;
            黄石信息港_ID = 0;
            住在九江_ID = 0;
            城市房产_ID = 0;
            河源置业网_ID = 0;
            邯郸恋家网_ID = 0;
            常州房产网_ID = 0;
            楼盘网_ID = 0;
            搜狐二手房_ID = 0;
            满堂红地产网_ID = 0;
            置家网_ID = 0;
            中国房产超市_ID = 0;
            中原地产_ID = 0;
            网站表 obj1 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(搜房网); });
            if (obj1 != null)
            {
                搜房网_ID = obj1.ID;
            }
            网站表 obj2 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(安居客); });
            if (obj2 != null)
            {
                安居客_ID = obj2.ID;
            }
            网站表 obj3 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(新浪二手房); });
            if (obj3 != null)
            {
                新浪二手房_ID = obj3.ID;
            }
            网站表 obj4 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(赶集网); });
            if (obj4 != null)
            {
                赶集网_ID = obj4.ID;
            }
            网站表 obj5 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(五八同城); });
            if (obj5 != null)
            {
                五八同城_ID = obj5.ID;
            }
            网站表 obj6 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(黄石信息港); });
            if (obj6 != null)
            {
                黄石信息港_ID = obj6.ID;
            }
            网站表 obj7 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(住在九江); });
            if (obj7 != null)
            {
                住在九江_ID = obj7.ID;
            }
            网站表 obj8 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(城市房产); });
            if (obj8 != null)
            {
                城市房产_ID = obj8.ID;
            }
            网站表 obj9 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(河源置业网); });
            if (obj9 != null)
            {
                河源置业网_ID = obj9.ID;
            }
            网站表 obj10 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(邯郸恋家网); });
            if (obj10 != null)
            {
                邯郸恋家网_ID = obj10.ID;
            }
            网站表 obj11 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(常州房产网); });
            if (obj11 != null)
            {
                常州房产网_ID = obj11.ID;
            }
            网站表 obj12 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(楼盘网); });
            if (obj12 != null)
            {
                楼盘网_ID = obj12.ID;
            }
            网站表 obj13 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(搜狐二手房); });
            if (obj13 != null)
            {
                搜狐二手房_ID = obj13.ID;
            }
            网站表 obj14 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(满堂红地产网); });
            if (obj14 != null)
            {
                满堂红地产网_ID = obj14.ID;
            }
            网站表 obj15 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(置家网); });
            if (obj15 != null)
            {
                置家网_ID = obj15.ID;
            }
            网站表 obj16 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(中国房产超市); });
            if (obj16 != null)
            {
                中国房产超市_ID = obj16.ID;
            }
            网站表 obj17 = 所有网站.Find(delegate(网站表 obj) { return obj.网站名称.Equals(中原地产); });
            if (obj17 != null)
            {
                中原地产_ID = obj17.ID;
            }
        }
        /// <summary>
        /// 获取所有网站
        /// </summary>
        /// <returns></returns>
        public static List<网站表> GetAllWebsite()
        {
            List<网站表> list = new List<网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.网站表;
                list = result.ToList();
            }
            return list;
        }        
        /// <summary>
        /// 根据网站名称获取ID
        /// </summary>
        /// <param name="webName"></param>
        /// <returns></returns>
        public static int GetWebIdByWebName(string webName)
        {
            int webId = 0;

            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                网站表 website = 所有网站.Find(delegate(网站表 _website) { return !string.IsNullOrEmpty(webName) && webName.Equals(_website.网站名称); });
                if (website != null)
                {
                    webId = website.ID;
                }
            }
            return webId;
        }


        public static 网站表 GetWebById(int id)
        {
            网站表 web = 所有网站.Find(delegate(网站表 _web) { return _web.ID == id; });

            return web;
        }

        public static List<网站表> GetWebByCityId(int cityId, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            //var query = from t in db.DB.网站表
            //            where (from t2 in db.DB.网站爬取配置 where t2.城市ID == cityId select t2.网站ID).Contains(t.ID)
            //            select t;
            var query = db.DB.网站表.Where(tbl => db.DB.网站爬取配置.Where(tbl2 => tbl2.城市ID == cityId).Select(tbl3 => tbl3.网站ID).Contains(tbl.ID));
            List<网站表> list = query.ToList();
            db.Connection_Close();


            return list;
            
        }
    }
}
