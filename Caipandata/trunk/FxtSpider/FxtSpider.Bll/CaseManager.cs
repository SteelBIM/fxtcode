using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using System.Data;
using System.Data.Linq;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Common;
using System.Text.RegularExpressions;
using log4net;
using System.Data.Linq.SqlClient;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json.Linq;
using FxtSpider.DAL.DB;
using FxtSpider.DAL.Manager;

namespace FxtSpider.Bll
{
    public static class CaseManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(CaseManager));

        #region (查询数据)

        #region VIEW_案例信息_城市表_网站表

        /// <summary>
        /// 根据城市ID,网站ID和爬取时间获取案例信息
        /// </summary>
        /// <param name="城市ID"></param>
        /// <param name="网站ID"></param>
        /// <param name="开始日期"></param>
        /// <param name="结束日期"></param>
        /// <param name="start">当前页码</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="isGetCount">是否获取总条数</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public static List<VIEW_案例信息_城市表_网站表> GetVIEW案例信息_根据城市网站爬取日期的区间获取案例(int? 城市ID, int? 网站ID, string 开始日期, string 结束日期, int start, int pageLength, bool isGetCount, out int count)
        {
            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            count = 0;
            int startIndex = ((start - 1) * pageLength) + 1;
            int endIndex = start * pageLength;
            DateTime startTime = Convert.ToDateTime(开始日期);
            DateTime endTime = Convert.ToDateTime(结束日期);
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                int test = db.CommandTimeout;
                if (!isGetCount)
                {
                    if (网站ID != null && 网站ID.Value > 0 && 城市ID != null && 城市ID.Value > 0)
                    {
                        list = db.get_案例信息_获取爬取数据_根据城市ID_网站_创建时间区间_分页(Convert.ToInt32(城市ID), Convert.ToInt32(网站ID), startTime, endTime, startIndex, endIndex).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                    else if (城市ID != null && 城市ID.Value > 0)
                    {
                        list = db.get_案例信息_获取爬取数据_根据城市ID_创建时间区间_分页(Convert.ToInt32(城市ID), startTime, endTime, startIndex, endIndex).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                    else if (网站ID != null && 网站ID.Value > 0)
                    {
                        list = db.get_案例信息_获取爬取数据_根据多条件(0, Convert.ToInt32(网站ID), startTime, endTime, startIndex, endIndex).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                    else
                    {
                        list = db.get_案例信息_获取爬取数据_根据多条件(0, 0, startTime, endTime, startIndex, endIndex).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                }
                else
                {
                    if (网站ID != null && 网站ID.Value > 0 && 城市ID != null && 城市ID.Value > 0)
                    {
                        list = db.get_案例信息_获取爬取数据_根据城市ID_网站_创建时间区间_分页2(Convert.ToInt32(城市ID), Convert.ToInt32(网站ID), startTime, endTime, startIndex, endIndex,out count).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                    else if (城市ID != null && 城市ID.Value > 0)
                    {
                        list = db.get_案例信息_获取爬取数据_根据城市ID_创建时间区间_分页2(Convert.ToInt32(城市ID), startTime, endTime, startIndex, endIndex,out count).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                    else if (网站ID != null && 网站ID.Value > 0)
                    {
                        list = db.get_案例信息_获取爬取数据_根据多条件_getCount(0, Convert.ToInt32(网站ID), startTime, endTime, startIndex, endIndex,out count).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                    else
                    {
                        list = db.get_案例信息_获取爬取数据_根据多条件_getCount(0, 0, startTime, endTime, startIndex, endIndex, out count).ToList<VIEW_案例信息_城市表_网站表>();
                    }
                }
            }
            return list;
        }

        public static List<VIEW_案例信息_城市表_网站表> GetVIEW案例信息_根据城市网站案例日期的区间获取案例(int? cityId, int? webId, DateTime 开始日期, DateTime 结束日期, int start, int pageLength, bool isGetCount, out int count, out string message)
        {
            string startDate = 开始日期.ToString("yyyy-MM-dd HH:mm:ss");
            string endDate = 结束日期.ToString("yyyy-MM-dd HH:mm:ss");
            message = "";
            count = 0;
            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                StringBuilder sbSql = new StringBuilder("select * from VIEW_案例信息_城市表_网站表 with(nolock) where 1=1");
                StringBuilder sbSql2 = new StringBuilder("select count(*) from VIEW_案例信息_城市表_网站表 with(nolock) where 1=1");

                try
                {
                    if (cityId != null)
                    {
                        sbSql.Append(" and 城市ID=").Append(cityId);
                        sbSql2.Append(" and 城市ID=").Append(cityId);
                    }
                    if (webId != null)
                    {
                        sbSql.Append(" and 网站ID=").Append(webId);
                        sbSql2.Append(" and 网站ID=").Append(webId);
                    }
                    sbSql.Append(" and 案例时间>='").Append(startDate).Append("'");
                    sbSql.Append(" and 案例时间<='").Append(endDate).Append("'");
                    sbSql2.Append(" and 案例时间>='").Append(startDate).Append("'");
                    sbSql2.Append(" and 案例时间<='").Append(endDate).Append("'");
                    if (isGetCount)
                    {
                        count = db.ExecuteQuery<int>(sbSql2.ToString()).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    message = "系统异常：sql(" + sbSql2.ToString() + "),异常信息：" + ex.Message; ;
                    return new List<VIEW_案例信息_城市表_网站表>();
                }
                list = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>(sbSql.ToString().GetPageSql("ID desc", start, pageLength)).ToList();
            }
            return list;

        }

        /// <summary>
        /// 查询入库或者未入库的案例(戴数量统计字段视图)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount">是否获取总个数</param>
        /// <param name="count"></param>
        /// <param name="webIds"></param>
        /// <param name="用途IDs"></param>
        /// <param name="是否成功入库"></param>
        /// <param name="是否为整理入库时过滤掉的信息"></param>
        /// <param name="行政区"></param>
        /// <param name="楼盘名"></param>
        /// <param name="创建时间start"></param>
        /// <param name="创建时间end"></param>
        /// <param name="案例时间start"></param>
        /// <param name="案例时间end"></param>
        /// <param name="入库整理时间start"></param>
        /// <param name="入库整理时间end"></param>
        /// <returns></returns>
        public static List<VIEW_案例信息_城市表_网站表2> GetVIEW案例信息2_根据高级查询条件(int cityId, int pageIndex, int pageSize, bool isGetCount, out int count,
            int[] webIds = null, int[] 用途IDs = null, bool? 是否成功入库 = null, bool? 是否为整理入库时过滤掉的信息 = null, long[] 行政区IDs = null, long[] 楼盘IDs = null,
            DateTime? 创建时间start = null, DateTime? 创建时间end = null,
            DateTime? 案例时间start = null, DateTime? 案例时间end = null,
            DateTime? 入库整理时间start = null, DateTime? 入库整理时间end = null
            )
        {

            count = 0;
            List<VIEW_案例信息_城市表_网站表2> list = new List<VIEW_案例信息_城市表_网站表2>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var query2 = db.案例信息.Where(tbl => tbl.城市ID == cityId);
                var query3 = db.案例信息.Where(tbl => tbl.城市ID == cityId);
                var query = db.VIEW_案例信息_城市表_网站表2.Where(tbl => tbl.城市ID == cityId);
                #region (查询条件)
                if (webIds != null && webIds.Length > 0)
                {
                    query = query.Where(tbl => webIds.Contains(Convert.ToInt32(tbl.网站ID)));
                    query2 = query2.Where(tbl => webIds.Contains(Convert.ToInt32(tbl.网站ID)));
                }
                if (用途IDs != null && 用途IDs.Length > 0)
                {
                    query = query.Where(tbl => 用途IDs.Contains(Convert.ToInt32(tbl.用途ID)));
                    query2 = query2.Where(tbl => 用途IDs.Contains(Convert.ToInt32(tbl.用途ID)));
                }
                if (行政区IDs != null && 行政区IDs.Length>0)
                {
                    query = query.Where(tbl => 行政区IDs.Contains(Convert.ToInt64(tbl.AreaId)));
                    query2 = query2.Where(tbl => 行政区IDs.Contains(Convert.ToInt64(tbl.AreaId)));
                }
                if (楼盘IDs != null && 楼盘IDs.Length>0)
                {
                    query = query.Where(tbl =>楼盘IDs.Contains(Convert.ToInt64(tbl.ProjectId)));
                    query2 = query2.Where(tbl => 楼盘IDs.Contains(Convert.ToInt64(tbl.ProjectId)));
                }
                if (创建时间start != null)
                {
                    query = query.Where(tbl => tbl.创建时间 >= 创建时间start);
                    query2 = query2.Where(tbl => tbl.创建时间 >= 创建时间start);
                }
                if (创建时间end != null)
                {
                    query = query.Where(tbl => tbl.创建时间 <= 创建时间end);
                    query2 = query2.Where(tbl => tbl.创建时间 <= 创建时间end);
                }
                if (案例时间start != null)
                {
                    query = query.Where(tbl => tbl.案例时间 >= 案例时间start);
                    query2 = query2.Where(tbl => tbl.案例时间 >= 案例时间start);
                }
                if (案例时间end != null)
                {
                    query = query.Where(tbl => tbl.案例时间 <= 案例时间end);
                    query2 = query2.Where(tbl => tbl.案例时间 <= 案例时间end);
                }
                if (入库整理时间start != null)
                {
                    query = query.Where(tbl => tbl.进行入库整理时间 >= 入库整理时间start);
                    query2 = query2.Where(tbl => tbl.进行入库整理时间 >= 入库整理时间start);
                }
                if (入库整理时间end != null)
                {
                    query = query.Where(tbl => tbl.进行入库整理时间 <= 入库整理时间end);
                    query2 = query2.Where(tbl => tbl.进行入库整理时间 <= 入库整理时间end);
                }
                if (是否成功入库 != null)
                {
                    if (是否成功入库.Value)
                    {
                        query = query.Where(tbl => tbl.是否已进行入库整理 == 1 && tbl.fxtId != 0);
                        query2 = query2.Where(tbl => tbl.是否已进行入库整理 == 1 && tbl.fxtId != 0);
                        query3 = query3.Where(tbl => tbl.是否已进行入库整理 == 1 && tbl.fxtId != 0);
                    }
                    else
                    {
                        query = query.Where(tbl => tbl.是否已进行入库整理 == 0 || tbl.fxtId == 0);
                        query2 = query2.Where(tbl => tbl.是否已进行入库整理 == 0 || tbl.fxtId == 0);
                        query3 = query3.Where(tbl => tbl.是否已进行入库整理 == 0 || tbl.fxtId == 0);
                    }
                }
                if (是否为整理入库时过滤掉的信息 != null)
                {
                    if (是否为整理入库时过滤掉的信息.Value)
                    {
                        query = query.Where(tbl => tbl.是否已进行入库整理 == 1 && tbl.fxtId == 0);
                        query2 = query2.Where(tbl => tbl.是否已进行入库整理 == 1 && tbl.fxtId == 0);
                        query3 = query3.Where(tbl => tbl.是否已进行入库整理 == 1 && tbl.fxtId == 0);
                    }
                    else
                    {
                        query = query.Where(tbl => tbl.是否已进行入库整理 == 0 || tbl.fxtId != 0);
                        query2 = query2.Where(tbl => tbl.是否已进行入库整理 == 0 || tbl.fxtId != 0);
                        query3 = query3.Where(tbl => tbl.是否已进行入库整理 == 0 || tbl.fxtId != 0);
                    }
                }
                #endregion
                if (isGetCount)
                {
                    count = query2.Count();
                }
                //方法一
                //var resultlist = query.OrderByDescending(tbl => tbl.楼盘名).OrderByDescending(tbl => query3.Where(tbl2 => tbl2.楼盘名 == tbl.楼盘名 && (SqlMethods.Like(tbl.行政区, "%" + tbl2.行政区 + "%") || SqlMethods.Like(tbl2.行政区, "%" + tbl.行政区 + "%"))).Count())
                //   .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                //   .Select(tbl => new { tbl, cou = query3.Where(tbl2 => tbl2.楼盘名 == tbl.楼盘名 && tbl2.行政区 == tbl.行政区).Count() }).ToList();
                //foreach (var obj in resultlist)
                //{
                //    obj.tbl.动态排序字段 = obj.cou;
                //    list.Add(obj.tbl);
                //}
                //方法二
                list = query.OrderByDescending(tbl => tbl.ProjectId).OrderByDescending(tbl => tbl.NotImportCaseCount)
                    .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                    .ToList<VIEW_案例信息_城市表_网站表2>();
                //if (list != null && list.Count > 0)
                //{
                //    List<long> longList = new List<long>();
                //    foreach (VIEW_案例信息_城市表_网站表2 view in list)
                //    {
                //        longList.Add(view.ID);
                //    }
                //    var resultlist = query2.Where(tbl => longList.ToArray().Contains(tbl.ID))
                //                           .Select(tbl => new { Id = tbl.ID, Count = query3.Where(tbl2 => tbl2.楼盘名 == tbl.楼盘名 && (SqlMethods.Like(tbl.行政区, "%" + tbl2.行政区 + "%") || SqlMethods.Like(tbl2.行政区, "%" + tbl.行政区 + "%"))).Count() })
                //                           .ToList();
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        list[i].动态排序字段 = 0;
                //        var obj = resultlist.Find(tbl => tbl.Id == list[i].ID);
                //        if (obj != null)
                //        {
                //            list[i].动态排序字段 = obj.Count;
                //        }
                //    }
                //    //long[] longs = new long[] { 3041, 3037, 3070, 3069, 3176, 3177, 3071 };
                //    //var qu = from t in db.案例信息
                //    //         where longs.Contains(t.ID)
                //    //         select new { t,cou= query2.Where(p => p.楼盘名 == t.楼盘名 && p.行政区 == t.行政区).Count() };
                //    //var res = qu.ToList();
                //}
            }
            return list;
        }

        public static List<VIEW_案例信息_城市表_网站表> GetCaseViewByIds(long[] ids)
        {

            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                list = db.VIEW_案例信息_城市表_网站表.Where(p => ids.Contains(p.ID)).ToList();
            }
            return list;
        }
        #endregion
        #region 案例信息
        public static 案例信息 根据_URL_当前案例时间_获取同一天的取爬取数据(string 城市名称, string 网站名称, string url, DateTime 案例时间)
        {
            案例信息 obj = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                //var result = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>("exec 根据_URL_当前案例时间_获取同一天的取爬取数据 {0},{1},{2},{3}", new object[] { 城市名称, 网站名称, url, 案例时间 });
                //list = result.ToList<VIEW_案例信息_城市表_网站表>();
                int cityId = CityManager.GetCityIdByCityName(城市名称);
                int webId = WebsiteManager.GetWebIdByWebName(网站名称);
                obj = db.案例信息.FirstOrDefault(p => p.城市ID == cityId && p.网站ID == webId && p.URL == url && p.案例时间 >= Convert.ToDateTime(案例时间.ToString("yyyy-MM-dd")) && p.案例时间 <= Convert.ToDateTime(案例时间.ToString("yyyy-MM-dd")+" 23:59:59"));
                
            }
            return obj;
        }
        public static 案例信息 根据_URL_当前案例时间_获取同一天的取爬取数据(int 城市ID, int 网站ID, string url, DateTime 案例时间)
        {
            案例信息 obj = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                obj = db.案例信息.FirstOrDefault(p => p.城市ID == 城市ID && p.网站ID == 网站ID && p.URL == url && p.案例时间 >= Convert.ToDateTime(案例时间.ToString("yyyy-MM-dd")) && p.案例时间 <= Convert.ToDateTime(案例时间.ToString("yyyy-MM-dd") + " 23:59:59"));

            }
            return obj;
        }
        public static 案例信息 GetCaseById(long id)
        {
            案例信息 caseInfo = null;
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                caseInfo = db.案例信息.FirstOrDefault(p => p.ID == id);
            }
            return caseInfo;
        }
        public static List<案例信息> GetCaseByIds(long[] ids)
        {
            List<案例信息> list = new List<案例信息>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                list = db.案例信息.Where(p => ids.Contains(p.ID)).ToList();
            }
            return list;
        }
        //public static List<案例信息> GetCaseByProjectNameAndCityNameAndLikeAreaName(string projectName, string cityName, string likeAreaName)
        //{
        //    int cityId = CityManager.GetCityIdByCityName(cityName);
        //    List<案例信息> list = new List<案例信息>();
        //    using (DataClassesDataContext db = new DataClassesDataContext())
        //    {
        //        var query = db.案例信息.Where(p => p.城市ID == cityId && p.楼盘名 == projectName && (SqlMethods.Like(p.行政区, "%" + likeAreaName + "%") || SqlMethods.Like(likeAreaName, "%" + p.行政区 + "%")));
        //        list = query.ToList<案例信息>();
        //    }
        //    return list;

        //}
        /// <summary>
        /// 获取指定时间类爬取的重复案例
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        private static 案例信息 GetCaseIdentical(案例信息 _obj, DataClass _db = null)
        {
            //return null;
            int nowH = Convert.ToInt32(DateTime.Now.ToString("HH"));
            //白天不做去重查询
            //if (nowH > 9 && nowH < 23)
            //{
            //    return null;
            //}
            DateTime dt = _obj.案例时间 == null ? DateTime.Now : Convert.ToDateTime(_obj.案例时间);//2014/6/29 0:00
            DateTime startDt = dt.AddDays(-3).Date;//2014/6/26 0:00
            DateTime endDt = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") + " 23:59:59");//2014/6/29 23:59:59
            案例信息 obj = null;
            DataClass db = new DataClass(_db);
            StringBuilder sbSql = new StringBuilder("select top 1 * from 案例信息 with(nolock) where 1=1");
            if (_obj.建筑类型ID != 0)
            {
                string where = "=" + Convert.ToInt32(_obj.建筑类型ID);
                sbSql.Append(" and 建筑类型ID ").Append(where);
            }
            if (_obj.用途ID != 0)
            {
                string where = "=" + Convert.ToInt32(_obj.用途ID);
                sbSql.Append(" and 用途ID ").Append(where);
            }
            if (_obj.户型ID != 0)
            {
                string where = "=" + Convert.ToInt32(_obj.户型ID);
                sbSql.Append(" and 户型ID ").Append(where);
            }
            if (_obj.朝向ID != 0)
            {
                string where = "=" + Convert.ToInt32(_obj.朝向ID);
                sbSql.Append(" and 朝向ID ").Append(where);
            }
            if (_obj.装修ID != 0)
            {
                string where = "=" + Convert.ToInt32(_obj.装修ID);
                sbSql.Append(" and 装修ID ").Append(where);
            }
            if (_obj.面积 != null && _obj.面积 > 0)
            {
                sbSql.Append("and 面积 =").Append(_obj.面积).Append("");
            }
            if (_obj.单价 != null && _obj.单价 > 0)
            {
                sbSql.Append(" and 单价=").Append(_obj.单价).Append("");
            }
            if (_obj.所在楼层 != null)
            {
                sbSql.Append(" and 所在楼层=").Append(_obj.所在楼层).Append("");
            }
            if (_obj.总楼层 != null)
            {
                sbSql.Append(" and 总楼层=").Append(_obj.总楼层).Append("");
            }
            sbSql.Append(" and 案例时间 >='").Append(startDt).Append("'");
            sbSql.Append(" and 案例时间 <='").Append(endDt).Append("'");
            sbSql.Append(" and ( ProjectId=").Append(_obj.ProjectId).Append(" or ProjectId in (select id from SysData_project with(nolock) where projectname='").Append(_obj.楼盘名).Append("' and cityid=").Append(_obj.城市ID).Append(") or 楼盘名='").Append(_obj.楼盘名).Append("')");
            obj = db.DB.ExecuteQuery<案例信息>(sbSql.ToString()).FirstOrDefault();

            db.Connection_Close();
            db.Dispose();
            return null;// obj;
        }

        #endregion
        #region 其他

        /// <summary>
        /// 获取时间段内城市网站的案例爬取个数
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public static List<get_案例信息_获取时间段内城市网站的爬取个数Result> GetCaseSpiderCountInfoByDate(DateTime startDate, DateTime endDate)
        {
            //DateTime.Now.add
            List<get_案例信息_获取时间段内城市网站的爬取个数Result> list =new List<get_案例信息_获取时间段内城市网站的爬取个数Result> ();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {                
                var result = db.get_案例信息_获取时间段内城市网站的爬取个数(startDate, endDate);
                list = result.ToList<get_案例信息_获取时间段内城市网站的爬取个数Result>();
            }
            return list;
        }
        /// <summary>
        /// 获取时间段内城市网站的案例已入库个数
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public static List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result> GetCaseImportCountInfoByDate(DateTime startDate, DateTime endDate)
        {

            //DateTime.Now.add
            List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result> list = new List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.get_案例信息_获取时间段内城市网站的已入库的案例个数(startDate, endDate);
                list = result.ToList<get_案例信息_获取时间段内城市网站的已入库的案例个数Result>();
            }
            return list;
        }
        /// <summary>
        /// 获取时间段内城市网站的案例未入库个数
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public static List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result> GetCaseNotImportCountInfoByDate(DateTime startDate, DateTime endDate)
        {

            //DateTime.Now.add
            List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result> list = new List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.get_案例信息_获取时间段内城市网站的未入库的案例个数(startDate, endDate);
                list = result.ToList<get_案例信息_获取时间段内城市网站的未入库的案例个数Result>();
            }
            return list;
        }       

        /// <summary>
        /// 根据关键字模糊获取楼盘名
        /// </summary>
        /// <param name="like"></param>
        /// <param name="maxCount"></param>
        /// <param name="cityId"></param>
        /// <returns>{\"ProjectName\":"",\"AreaName\":""}格式数组</returns>
        public static string[] GetProjectInfoStringsByLike(string like, int maxCount, int? cityId = null)
        {
            List<string> list = new List<string>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var query = db.VIEW_案例信息_城市表_网站表.Where(p => 1 == 1);
                if (cityId != null)
                {
                    query = query.Where(p => p.城市ID == Convert.ToInt32(cityId));
                }
                query = query.Where(p => p.ProjectName.Contains(like));
                var query2 = query.Select(p => new { ProjectName = p.ProjectName, AreaName = p.AreaName }).Distinct().Take(maxCount);
                var obj=query2.ToList();
                foreach (var _obj in obj)
                {
                    JObject _jobject = new JObject();
                    _jobject.Add(new JProperty("ProjectName", _obj.ProjectName.EncodeField()));
                    _jobject.Add(new JProperty("AreaName", _obj.AreaName.EncodeField()));
                    list.Add(_jobject.ToJSONjss());
                }

            }
            return list.ToArray();
        }

        #endregion

        #endregion 
        #region (更新数据)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="网站名称"></param>
        /// <param name="城市名称"></param>
        /// <param name="网站ID"></param>
        /// <param name="城市ID"></param>
        /// <param name="楼盘名"></param>
        /// <param name="案例时间"></param>
        /// <param name="行政区"></param>
        /// <param name="片区"></param>
        /// <param name="楼栋"></param>
        /// <param name="房号"></param>
        /// <param name="用途"></param>
        /// <param name="面积"></param>
        /// <param name="单价"></param>
        /// <param name="案例类型"></param>
        /// <param name="结构"></param>
        /// <param name="建筑类型"></param>
        /// <param name="总价"></param>
        /// <param name="所在楼层"></param>
        /// <param name="总楼层"></param>
        /// <param name="户型"></param>
        /// <param name="朝向"></param>
        /// <param name="装修"></param>
        /// <param name="建筑年代"></param>
        /// <param name="信息"></param>
        /// <param name="电话"></param>
        /// <param name="URL"></param>
        /// <param name="币种"></param>
        /// <param name="地址"></param>
        /// <param name="创建时间"></param>
        /// <param name="车位数量"></param>
        /// <param name="地下室面积"></param>
        /// <param name="花园面积"></param>
        /// <param name="建筑形式"></param>
        /// <param name="配套设施"></param>
        /// <param name="厅结构"></param>
        /// <param name="中介公司"></param>
        /// <param name="门店"></param>
        /// <param name="_db"></param>
        /// <returns>1:成功,0:失败,-1:失败,已存在</returns>
        public static int 往案例表插入爬取数据(string 网站名称, string 城市名称, int 网站ID, int 城市ID, string 楼盘名, string 案例时间, string 行政区,
                                        string 片区, string 楼栋, string 房号, string 用途, string 面积, string 单价,
                                        string 案例类型, string 结构, string 建筑类型, string 总价, string 所在楼层, string 总楼层,
                                        string 户型, string 朝向, string 装修, string 建筑年代, string 信息, string 电话,
                                        string URL, string 币种, string 地址, DateTime 创建时间, string 车位数量, string 地下室面积,
                                        string 花园面积, string 建筑形式, string 配套设施, string 厅结构, string 中介公司, string 门店,DateTime startSpiderDate, DataClass _db=null)
        {
        work:
            if (!WorkItemManager.CheckPassSpider())//****检查数据库是否有维护程序在执行******//
            {
                System.Threading.Thread.Sleep(60000);
                goto work;
            }
            DataClass db = new DataClass(_db);
            try
            {
                //
                案例信息 caseObj = new 案例信息
                {
                    城市ID = 城市ID,
                    网站ID = 网站ID,
                    楼盘名 = 楼盘名.TrimBlank().RemoveHeml(),
                    行政区 = 行政区.TrimBlank().RemoveHeml(),
                    片区 = 片区.TrimBlank().RemoveHeml(),
                    楼栋 = 楼栋.GetSubstring(100),
                    房号 = 房号.GetSubstring(100),
                    用途 = 用途,
                    面积String = 面积.TrimBlank().RemoveHeml(),
                    单价String = 单价.TrimBlank().RemoveHeml(),
                    案例类型 = 案例类型.TrimBlank().RemoveHeml(),
                    结构 = 结构.TrimBlank().RemoveHeml(),
                    建筑类型 = 建筑类型.TrimBlank().RemoveHeml(),
                    总价String = 总价.TrimBlank().RemoveHeml(),
                    所在楼层String = 所在楼层.TrimBlank().RemoveHeml(),
                    总楼层String = 总楼层.TrimBlank().RemoveHeml(),
                    户型 = 户型.TrimBlank().RemoveHeml(),
                    朝向 = 朝向.TrimBlank().RemoveHeml(),
                    装修 = 装修.TrimBlank().RemoveHeml().GetSubstring(50),
                    建筑年代 = 建筑年代.TrimBlank().RemoveHeml().GetSubstring(50),
                    信息 = 信息.RemoveHTMLTags(),
                    电话 = 电话.TrimBlank().RemoveHeml().GetSubstring(50),
                    URL = URL,
                    币种 = 币种,
                    地址 = 地址.RemoveHeml(),
                    创建时间 = 创建时间,
                    建筑形式 = 建筑形式.TrimBlank().RemoveHeml().GetSubstring(100),
                    花园面积String = 花园面积.TrimBlank().RemoveHeml(),
                    厅结构 = 厅结构.TrimBlank().RemoveHeml().GetSubstring(100),
                    车位数量String = 车位数量.TrimBlank().RemoveHeml(),
                    配套设施 = 配套设施.RemoveHeml().GetSubstring(200),
                    地下室面积String = 地下室面积.TrimBlank().RemoveHeml(),
                    来源 = 网站名称
                };
                //验证传过来的用途是否存在
                if (!string.IsNullOrEmpty(StringHelp.TrimBlank(caseObj.用途).ToRemoveSpe()))
                {
                    if ( caseObj.用途.Contains("其它")||用途Manager.Get用途_根据名称(StringHelp.TrimBlank(caseObj.用途).ToRemoveSpe()) < 1)
                    {
                        caseObj.用途 = "";
                    }
                }
                //数据格式计算整理
                caseObj = 爬取信息数据格式计算整理(caseObj,案例时间, 城市名称);
                //检查数据字符串
                if (caseObj.行政区.Contains("东莞") || caseObj.行政区.Contains("惠州"))
                {
                    db.Connection_Close();
                    db.Dispose();
                    return 0;
                }
                string checkMessage = "";
                if (!SpiderHelp.CheckHouseAll(caseObj, 城市名称, out checkMessage))
                {
                    db.Connection_Close();
                    db.Dispose();
                    log.Debug(checkMessage);
                    return 0;
                }
                //数据库插入操作
                int? 案例类型ID = 案例类型Manager.Get案例类型_根据名称(caseObj.案例类型); caseObj.案例类型 = null;
                int? 币种ID = 币种Manager.Get币种_根据名称(caseObj.币种); caseObj.币种 = null;
                int? 朝向ID = 朝向Manager.Get朝向_根据名称(caseObj.朝向); caseObj.朝向 = null;
                int? 户型ID = 户型Manager.Get户型_根据名称(caseObj.户型); caseObj.户型 = null;
                int? 建筑类型ID = 建筑类型Manager.Get建筑类型_根据名称(caseObj.建筑类型); caseObj.建筑类型 = null;
                int? 结构ID = 结构Manager.Get结构_根据名称(caseObj.结构); caseObj.结构 = null;
                int? 用途ID = 用途Manager.Get用途_根据名称(caseObj.用途); caseObj.用途 = null;
                //int? 装修ID = 装修Manager.Get装修_根据名称(caseObj.装修); caseObj.装修 = null;//暂时不限制
                caseObj.案例类型ID = Convert.ToInt32(案例类型ID);// == 0 ? null : 案例类型ID;
                caseObj.币种ID = Convert.ToInt32(币种ID);//== 0 ? null : 币种ID;
                caseObj.朝向ID = Convert.ToInt32(朝向ID);// == 0 ? null : 朝向ID;
                caseObj.户型ID = Convert.ToInt32(户型ID);// == 0 ? null : 户型ID;
                caseObj.建筑类型ID = Convert.ToInt32(建筑类型ID);// == 0 ? null : 建筑类型ID;
                caseObj.结构ID = Convert.ToInt32(结构ID);// == 0 ? null : 结构ID;
                caseObj.用途ID = Convert.ToInt32(用途ID);// == 0 ? null : 用途ID;
                //caseObj.装修ID = Convert.ToInt32(装修ID) == 0 ? null : 装修ID;//暂时不限制
                中介公司 = 中介公司.TrimBlank();
                门店 = 门店.TrimBlank();
                //获取中介公司
                //中介公司 = 中介公司 + "test";
                caseObj.CompanyId = 0;
                if (!string.IsNullOrEmpty(中介公司))
                {
                    SysData_Company com = CompanyManager.GetByCompanyName(中介公司, _db: db);
                    if (com == null)
                    {
                        com = CompanyManager.Insert(中介公司, _db: db);
                    }
                    caseObj.CompanyId = com.ID;
                }
                //获取中介公司门店
                //门店 = 门店 + "test";
                caseObj.CompanyAreaId = 0;
                if (!string.IsNullOrEmpty(门店))
                {
                    SysData_CompanyArea comArea = CompanyAreaManager.GetByCompanyAreaName(门店, _db: db);
                    if (comArea == null)
                    {
                        comArea = CompanyAreaManager.Insert(门店, _db: db);
                    }
                    caseObj.CompanyAreaId = comArea.ID;
                }
                //获取楼盘ID
                //caseObj.楼盘名 = caseObj.楼盘名 + "test";
                caseObj.ProjectId = 0;
                if (string.IsNullOrEmpty(caseObj.楼盘名)) 
                {
                    log.Debug(string.Format("数据保存结束:网站:{0}--城市:{1}-(url:{2}--),(楼盘名为null)", 网站名称, 城市名称, caseObj.URL));
                    db.Connection_Close();
                    db.Dispose();
                    return 0;
                }
                //if (!string.IsNullOrEmpty(caseObj.楼盘名))
                //{
                //    SysData_Project project = ProjectManager.GetProjectByProjectNameAndCityId(caseObj.楼盘名, 城市ID, _db: db);
                //    if (project == null)
                //    {
                //        project = ProjectManager.InsertProject(caseObj.楼盘名, 城市ID, 网站ID, _db: db);
                //    }
                //    caseObj.ProjectId = project.ID;
                //}
                //else 
                //{
                //    log.Debug(string.Format("数据保存结束:网站:{0}--城市:{1}-(url:{2}--),(楼盘名为null)", 网站名称, 城市名称, caseObj.URL));
                //    db.Connection_Close();
                //    db.Dispose();
                //    return 0;
                //}
                //caseObj.楼盘名 = null;

                //获取行政区ID
                caseObj.AreaId = 0;
                if (!string.IsNullOrEmpty(caseObj.行政区))
                {
                    SysData_Area areaObj = AreaManager.GetAreaByAreaNameLikeByCityId(caseObj.行政区, 城市ID, _db: db);
                    if (areaObj == null)
                    {
                        areaObj = AreaManager.InsertArea(caseObj.行政区, 城市ID, 网站ID, _db: db);
                    }
                    caseObj.AreaId = areaObj.ID;
                }
                else
                {
                    caseObj.AreaId = 0;
                }
                caseObj.行政区 = null;
                //获取片区ID
                //caseObj.片区 = caseObj.片区 + "test";
                caseObj.SubAreaId = 0;
                if (!string.IsNullOrEmpty(caseObj.片区))
                {
                    SysData_SubArea subAreaObj = SubAreaManager.GetAreaByAreaNameByCityId(caseObj.片区, 城市ID, _db: db);
                    if (subAreaObj == null)
                    {
                        subAreaObj = SubAreaManager.InsertArea(caseObj.片区, 城市ID, 网站ID, _db: db);
                    }
                    caseObj.SubAreaId = subAreaObj.ID;
                }
                caseObj.片区 = null;
                //获取装修
                //caseObj.装修 = caseObj.装修 + "test";
                caseObj.装修ID = 0;
                if (!string.IsNullOrEmpty(caseObj.装修))
                {
                    SysData_装修 zhuangxiuObj = 装修Manager.Get装修_根据名称(caseObj.装修, db);
                    if (zhuangxiuObj == null)
                    {
                        zhuangxiuObj = 装修Manager.Insert(caseObj.装修, db);
                    }
                    caseObj.装修ID = zhuangxiuObj.ID;
                }
                caseObj.装修 = null;
                //验证数据是否存在
                案例信息 obj = CaseManager.GetCaseIdentical(caseObj, _db: db);
                if (obj != null)
                {
                    //记录个数
                    SpiderRepetitionLogManager.SetSpiderRepetitionCount(网站ID, 城市ID, startSpiderDate, db);
                    log.Debug(string.Format("{0}-数据保存结束:网站:{1}--城市:{2}-(url:{3}--),(数据已存在)",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 网站名称, 城市名称, caseObj.URL));
                    db.Connection_Close();
                    db.Dispose();
                    return -1;
                }
                Insert(caseObj, db);
                //db.DB.案例信息.InsertOnSubmit(caseObj);
                //db.DB.SubmitChanges();

                db.Connection_Close();
                db.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}-系统异常:网站:{1}--城市:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 网站名称, 城市名称), ex);
                db.Connection_Close();
                db.Dispose();
                return 0;
            }
            return 1;
        }

        public static bool Update(案例信息 caseInfo, DataClassesDataContext db = null)
        {
            if (caseInfo == null)
            {
                return true;
            }
            bool existsDb = true;
            if (db == null)
            {
                existsDb = false;
                db = new DataClassesDataContext();
            }

            案例信息 _case = db.案例信息.FirstOrDefault(p => p.ID == caseInfo.ID);
            if (_case != null)
            {
                caseInfo.CopyValueTo(_case);
                db.SubmitChanges();
            }
            if (!existsDb)
            {
                db.Connection.Close();
                db.Dispose();
            }

            return true;
        }
        public static bool Update(List<案例信息> caseList, DataClassesDataContext db = null)
        {
            if (caseList == null || caseList.Count < 1)
            {
                return true;
            }
            bool existsDb = true;
            if (db == null)
            {
                existsDb = false;
                db = new DataClassesDataContext();
            }
            foreach (案例信息 obj in caseList)
            {
                Update(obj, db);
            }
            if (!existsDb)
            {
                db.Connection.Close();
                db.Dispose();
            }

            return true;
        }
        /// <summary>
        /// 更新楼盘名
        /// </summary>
        /// <param name="caseList"></param>
        /// <param name="projectName"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool UpdateProjectName(List<案例信息> caseList,string projectName, DataClassesDataContext db = null)
        {

            if (caseList == null || caseList.Count < 1)
            {
                return true;
            }
            bool existsDb = true;
            if (db == null)
            {
                existsDb = false;
                db = new DataClassesDataContext();
            }
            caseList.ForEach(delegate(案例信息 _obj) {
                _obj.楼盘名 = projectName;
            });
            bool result = Update(caseList, db: db);
            if (!existsDb)
            {
                db.Connection.Close();
                db.Dispose();
            }
            return result;

        }

        public static bool DeleteCaseByIds(long[] ids,out string message, DataClassesDataContext db = null)
        {
            message = "";
            string idsStr = ids.ConvertToString();
            if (string.IsNullOrEmpty(idsStr))
            {
                return true;
            }
            bool existsDb = true;
            bool existsTn = true;
            if (db == null)
            {
                existsDb = false;
                db = new DataClassesDataContext();
            }
            if (db.Transaction == null)
            {
                db.Connection.Open();
                var tran = db.Connection.BeginTransaction();
                db.Transaction = tran;
                existsTn = false;
            }
            try
            {
                List<案例信息> caseList = db.案例信息.Where(p => ids.Contains(p.ID) && p.是否已进行入库整理 == 1 && p.fxtId == null).ToList();
                bool result = ProjectCaseCountManager.UpdateNotImportCaseCount(caseList, new List<案例库上传信息过滤表>(), out message, new DataClass(db));
                if (!result)
                {
                    if (!existsTn)
                    {
                        db.Transaction.Rollback();
                    }
                    if (!existsDb)
                    {
                        db.Connection.Close();
                        db.Dispose();
                    }
                    return false;
                }
                db.案例信息_DeleteByIds(idsStr);
                CaseFilterLogManager.DeleteCaseFilterByCaseIds(ids, db: db);
                if (!existsTn)
                {
                    db.Transaction.Commit();
                }
                if (!existsDb)
                {
                    db.Connection.Close();
                    db.Dispose();
                }
            }
            catch (Exception ex)
            {
                message = "系统异常";
                log.Debug(string.Format("删除案例信息失败,个数{0}", ids.Length));
                if (!existsTn)
                {
                    db.Transaction.Rollback();
                }
                if (!existsDb)
                {
                    db.Connection.Close();
                    db.Dispose();
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 更新入库失败案例的楼盘名称
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="areaId">无行政区时则传0</param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static bool UpdateImportFailCaseProjectBy(string projectName, int cityId, long projectId, long areaId,out string message,DataClass _dc=null)
        {
            message = "";
            if (string.IsNullOrEmpty(projectName))
            {
                return true;
            }
            DataClass dc = new DataClass(_dc);
            try
            {
                string sql = " update 案例信息 set ProjectId={0} , 楼盘名=null where 城市ID={1} and ProjectId={2} and {3} and 是否已进行入库整理=1 and fxtId is null ";
                SysData_Project project = ProjectManager.GetProjectByProjectNameAndCityId(projectName, cityId, _db: dc);
                if (project == null)
                {
                    project = ProjectManager.InsertProject(projectName, cityId, 0, _db: dc);
                }
                if (areaId > 0)
                {
                    sql = string.Format(sql, project.ID, cityId, projectId, "AreaId=" + areaId);
                }
                else
                {
                    sql = string.Format(sql, project.ID, cityId, projectId, "(AreaId is null or AreaId=0)");
                }
                int count = dc.DB.ExecuteCommand(sql);
                ProjectCaseCountManager.UpdateNotImportCaseCount(projectId, areaId, -count, _db: dc);
                ProjectCaseCountManager.UpdateNotImportCaseCount(project.ID, areaId, count, _db: dc);
            }
            catch (Exception ex)
            {
                message = "更新入库失败案例楼盘名称_系统异常";
                log.Error("UpdateImportFailCaseProjectBy异常", ex);
                dc.Connection_Close();
                dc.Dispose();
                return false;
            }
            dc.Connection_Close();
            dc.Dispose();
            return true;

        }

        #endregion

        #region common
        public static int? CaseColumnConvertToInt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (StringHelp.IsInteger(str))
            {
                return Convert.ToInt32(str);
            }
            return null;
        }
        public static decimal? CaseColumnConvertToDecimal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (StringHelp.CheckDecimal(str))
            {
                return Convert.ToDecimal(str);
            }
            return null;
        }
        public static 案例信息 爬取信息数据格式计算整理(案例信息 ent, string 案例时间, string 城市名称)
        {
            //整理数据字符串
            ent.总楼层String = StringHelp.TrimBlank(ent.总楼层String).ToRemoveSpe();ent.总楼层 = CaseColumnConvertToInt(ent.总楼层String);
            ent.所在楼层String = StringHelp.TrimBlank(ent.所在楼层String).ToRemoveSpe();ent.所在楼层 = CaseColumnConvertToInt(ent.所在楼层String);
            ent.建筑形式 = StringHelp.TrimBlank(ent.建筑形式).ToRemoveSpe();
            ent.楼盘名 = StringHelp.TrimBlank(ent.楼盘名).ToRemoveSpe();
            ent.行政区 = StringHelp.TrimBlank(ent.行政区.Trim().ToRemoveSpe());
            ent.总价String = StringHelp.TrimBlank(ent.总价String).ToRemoveSpe(); ent.总价 = CaseColumnConvertToDecimal(ent.总价String);
            ent.用途 = StringHelp.TrimBlank(ent.用途).ToRemoveSpe();
            ent.朝向 = SpiderHelp.处理朝向字符(StringHelp.TrimBlank(ent.朝向).ToRemoveSpe());
            if (朝向Manager.Get朝向_根据名称(ent.朝向) < 1)
            {
                ent.朝向 = "";
            }
            ent.电话 = StringHelp.TrimBlank(ent.电话).ToRemoveSpe();
            ent.面积String = Regex.Replace(ent.面积String, @"\..*", "", RegexOptions.IgnoreCase);ent.面积 = CaseColumnConvertToDecimal(ent.面积String);
            ent.单价String = Regex.Replace(ent.单价String, @"\..*", "", RegexOptions.IgnoreCase);ent.单价 = CaseColumnConvertToDecimal(ent.单价String);
            ent.花园面积String = Regex.Replace(ent.花园面积String, @"\..*", "", RegexOptions.IgnoreCase); ent.花园面积 = CaseColumnConvertToDecimal(ent.花园面积String);
            ent.地下室面积String = Regex.Replace(ent.地下室面积String, @"\..*", "", RegexOptions.IgnoreCase); ent.地下室面积 = CaseColumnConvertToDecimal(ent.地下室面积String);
            ent.车位数量String = StringHelp.TrimBlank(ent.车位数量String).ToRemoveSpe(); ent.车位数量 = CaseColumnConvertToInt(ent.车位数量String);
            ent.结构 = StringHelp.TrimBlank(ent.结构.Replace("平层", 结构Manager.平面));
            ent.户型 = Regex.Replace(ent.户型, @"(\d*厨|\d*卫)", "", RegexOptions.IgnoreCase);
            ent.户型 = ent.户型.Replace("室", "房");
            ent.户型 = StringHelp.NumberConvertToChinese(ent.户型);
            ent.案例类型 = StringHelp.TrimBlank(string.IsNullOrEmpty(ent.案例类型) ? 案例类型Manager.买卖报盘 : ent.案例类型);
            ent.币种 = StringHelp.TrimBlank(string.IsNullOrEmpty(ent.币种) ? 币种Manager.人民币 : ent.币种);
            ent.信息 = Regex.Replace(ent.信息, @"\\&[a-z0-9A-Z_]*;", "", RegexOptions.IgnoreCase);
            ent.地址 = Regex.Replace(ent.地址, @"\\&[a-z0-9A-Z_]*;", "", RegexOptions.IgnoreCase);
            //计算数据
            ent.结构 = SpiderHelp.获取户型结构(ent.结构);
            ent.建筑类型 = SpiderHelp.GetBuildingType(Convert.ToString(ent.总楼层));//获取计算建筑类型;
            ent.用途 = SpiderHelp.GetHousePurposes(ent.用途, 城市名称, Convert.ToString(ent.单价), Convert.ToString(ent.面积), Convert.ToString(ent.总楼层),Convert.ToString(ent.所在楼层), ent.建筑形式, ent.建筑类型);//获取计算用途 
            ent.户型 = SpiderHelp.GetHouseType(ent.户型).ToRemoveSpe();
            案例时间 = 案例时间 != null ? 案例时间.Trim() : 案例时间;
            ent.装修 = SpiderHelp.获取装修类型(ent.装修);
            if (!StringHelp.CheckStrIsDate(案例时间))
            {
                ent.案例时间 = DateTime.Now;
            }
            else
            {
                ent.案例时间 = Convert.ToDateTime(案例时间);
            }
            return ent;
        }


        public static 案例信息 Insert(案例信息 obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.案例信息_Insert(obj.楼盘名, obj.案例时间, obj.行政区, obj.片区, obj.楼栋, obj.房号,
            obj.面积, obj.单价,  obj.总价, obj.所在楼层, obj.总楼层,  obj.装修, obj.建筑年代, obj.信息, obj.电话, obj.URL, 
            obj.地址, obj.创建时间, obj.来源, obj.建筑形式, obj.花园面积, obj.厅结构, obj.车位数量, obj.配套设施,
            obj.地下室面积, obj.城市ID, obj.网站ID, obj.案例类型ID, obj.币种ID, obj.朝向ID, obj.户型ID, obj.建筑类型ID,
            obj.结构ID, obj.用途ID, obj.装修ID, obj.是否已进行入库整理, obj.进行入库整理时间, obj.fxtId, obj.CompanyId,
            obj.CompanyAreaId, obj.ProjectId, obj.AreaId, obj.SubAreaId, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
        public static 案例信息 Update(案例信息 obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                db.DB.案例信息_Update(obj.楼盘名, obj.案例时间, obj.行政区, obj.片区, obj.楼栋, obj.房号,
            obj.面积, obj.单价, obj.总价, obj.所在楼层, obj.总楼层, obj.装修, obj.建筑年代, obj.信息, obj.电话, obj.URL, 
            obj.地址, obj.创建时间, obj.来源, obj.建筑形式, obj.花园面积, obj.厅结构, obj.车位数量, obj.配套设施,
            obj.地下室面积, obj.城市ID, obj.网站ID, obj.案例类型ID, obj.币种ID, obj.朝向ID, obj.户型ID, obj.建筑类型ID,
            obj.结构ID, obj.用途ID, obj.装修ID, obj.是否已进行入库整理, obj.进行入库整理时间, obj.fxtId, obj.CompanyId,
            obj.CompanyAreaId, obj.ProjectId, obj.AreaId, obj.SubAreaId, obj.ID);
            }
            return obj;
        }
        #endregion


    }
}
