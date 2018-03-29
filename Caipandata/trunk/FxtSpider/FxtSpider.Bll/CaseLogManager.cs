using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using log4net;
using FxtSpider.Common;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    /// <summary>
    /// 案例上传记录
    /// </summary>
    public static class CaseLogManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CaseLogManager));
        public static List<VIEW_案例信息_城市表_网站表> 获取当前要整理入库的案例(string 城市名称, string 网站名称, int 个数)
        {
            
            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            try
            {
                if (string.IsNullOrEmpty(城市名称) || string.IsNullOrEmpty(网站名称) || 个数 < 1)
                {
                    return list;
                }
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    var result = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>("exec 获取当前要整理入库的案例_根据城市_网站 {0},{1},{2}", new object[] { 城市名称, 网站名称, 个数 });
                    list = result.ToList<VIEW_案例信息_城市表_网站表>();
                }
                if (list != null && list.Count > 0)
                {
                    list.ForEach(delegate(VIEW_案例信息_城市表_网站表 obj)
                    {
                        obj.来源 = 网站名称;
                        obj.城市 = 城市名称;
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("获取当前要整理入库的案例_异常,(城市名称:{0},网站名称{1},个数:{2})", 
                    城市名称 == null ? "null" : 城市名称, 网站名称 == null ? "null" : 网站名称, 个数),
                    ex);
            }
            return list;
        }
        public static List<VIEW_案例信息_城市表_网站表> 获取当前要整理入库的案例(int? cityId, int? webId, int count, DataClassesDataContext db = null)
        {

            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            bool exstisDb = true;
            try
            {
                if (db == null)
                {
                    exstisDb = false;
                    db = new DataClassesDataContext();
                }
                StringBuilder sbSql = new StringBuilder("select top({0}) * from VIEW_案例信息_城市表_网站表 with(nolock) where ID=109322"); //new StringBuilder("select top({0}) * from VIEW_案例信息_城市表_网站表 with(nolock) where 是否已进行入库整理=0");
                //if (cityId != null)
                //{
                //    sbSql.Append(" and 城市ID=").Append(Convert.ToInt32(cityId));
                //}
                //if (webId != null)
                //{
                //    sbSql.Append(" and 网站ID=").Append(Convert.ToInt32(cityId));
                //}
                var result = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>(sbSql.ToString(), new object[] { count });
                list = result.ToList<VIEW_案例信息_城市表_网站表>();
                //var query = db.VIEW_案例信息_城市表_网站表.Where(tbl => tbl.是否已进行入库整理 == 0);
                //if (cityId != null)
                //{
                //    query = query.Where(tbl => tbl.城市ID == Convert.ToInt32(cityId));
                //}
                //if (webId != null)
                //{
                //    query = query.Where(tbl => tbl.网站ID == Convert.ToInt32(webId));
                //}
                //list = query.OrderByDescending(tbl => tbl.ID).Take(count).ToList<VIEW_案例信息_城市表_网站表>();
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("获取当前要整理入库的案例_异常,(城市ID:{0},网站ID{1},个数:{2})",
                    cityId == null ? "null" : cityId.ToString(), webId == null ? "null" : webId.ToString(), count),
                    ex);
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
            }
            return list;
        }


        public static List<VIEW_案例信息_城市表_网站表> 获取当前要整理入库的案例(string 网站名称, int 个数)
        {

            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            try
            {
                if (string.IsNullOrEmpty(网站名称) || 个数 < 1)
                {
                    return list;
                }
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    var result = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>("exec 获取当前要整理入库的案例_根据网站 {0},{1}", new object[] {网站名称, 个数 });
                    list = result.ToList<VIEW_案例信息_城市表_网站表>();
                }
                if (list != null && list.Count > 0)
                {
                    list.ForEach(delegate(VIEW_案例信息_城市表_网站表 obj)
                    {
                        obj.来源 = 网站名称;
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("获取当前要整理入库的案例_异常,(城市名称:所有,网站名称{0},个数:{1})",
                   网站名称 == null ? "null" : 网站名称, 个数),
                    ex);
            }
            return list;
        }
        public static List<VIEW_案例信息_城市表_网站表> 获取当前要整理入库的案例(int 个数)
        {
            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            try
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    var result = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>("exec 获取当前要整理入库的案例 {0}", new object[] { 个数 });
                    list = result.ToList<VIEW_案例信息_城市表_网站表>();
                }
                if (list != null && list.Count > 0)
                {
                    list.ForEach(delegate(VIEW_案例信息_城市表_网站表 obj)
                    {
                        obj.来源 = obj.网站;
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("获取当前要整理入库的案例_异常,(个数:{0})",个数), ex);
            }
            return list;
        }
        public static bool 将当前已经整理入库的案例记录表中(int 城市ID, int 网站ID, List<VIEW_案例信息_城市表_网站表> viewList, Dictionary<long, int> 原始库ID对应成功的房讯通ID)
        {
            try
            {
                if (viewList == null || viewList.Count < 1)
                {
                    return true;
                }
                List<long> longs = new List<long>();
                viewList.ForEach(delegate(VIEW_案例信息_城市表_网站表 obj)
                {
                    longs.Add(obj.ID);
                });
                long[] 当前案例ID = longs.ToArray();
                if (当前案例ID == null || 当前案例ID.Length < 1)
                {
                    return true;
                }
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    var objWhere = from t in db.案例信息
                                where 当前案例ID.Contains(t.ID)
                                select t;
                    List<案例信息> list = objWhere.ToList<案例信息>();
                    
                    if (list == null || list.Count < 1)
                    {
                        return true;
                    }
                    else
                    {
                        list.ForEach(delegate(案例信息 obj)
                        {
                            obj.是否已进行入库整理 = 1;
                            obj.进行入库整理时间 = DateTime.Now;
                            if (原始库ID对应成功的房讯通ID!=null&&原始库ID对应成功的房讯通ID.ContainsKey(obj.ID))
                            {
                                obj.fxtId = 原始库ID对应成功的房讯通ID[obj.ID];
                            }
                        });
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("记录当前已经整理入库的案例_异常,(城市ID:{0},网站ID{1},当前案例个数:{2})",
                    城市ID, 网站ID, viewList.Count),
                    ex);
                return false;
            }
            return true;
        }
        public static bool 将当前已经整理入库的案例记录表中(List<VIEW_案例信息_城市表_网站表> viewList, Dictionary<long, int> 原始库ID对应成功的房讯通ID, DataClassesDataContext db = null)
        {
            bool exstisDb = true;
            try
            {
                if (viewList == null || viewList.Count < 1)
                {
                    return true;
                }
                //获取所有案例ID
                List<long> longs = new List<long>();
                viewList.ForEach(delegate(VIEW_案例信息_城市表_网站表 obj)
                {
                    longs.Add(obj.ID);
                });
                long[] 当前案例ID = longs.ToArray();
                if (当前案例ID == null || 当前案例ID.Length < 1)
                {
                    return true;
                }
                if (db == null)
                {
                    exstisDb = false;
                    db = new DataClassesDataContext();
                }
                DataClass dc = new DataClass(db);
                //根据多个案例ID获取案例
                var objWhere = from t in db.案例信息
                               where 当前案例ID.Contains(t.ID)
                               select t;
                List<案例信息> list = objWhere.ToList<案例信息>();

                if (list == null || list.Count < 1)
                {
                    return true;
                }
                else
                {
                    list.ForEach(delegate(案例信息 obj)
                    {
                        obj.是否已进行入库整理 = 1;
                        obj.进行入库整理时间 = DateTime.Now;
                        if (原始库ID对应成功的房讯通ID != null && 原始库ID对应成功的房讯通ID.ContainsKey(obj.ID))
                        {
                            obj.fxtId = 原始库ID对应成功的房讯通ID[obj.ID];
                        }
                    });
                }
                db.SubmitChanges();
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("记录当前已经整理入库的案例_异常,(当前案例个数:{0})", viewList.Count),
                    ex);
                if (!exstisDb && db != null)
                {
                    db.Connection.Close();
                }
                return false;
            }
            return true;
        }
    }
}
