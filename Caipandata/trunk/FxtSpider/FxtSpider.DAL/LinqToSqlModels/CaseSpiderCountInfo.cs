using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using System.Reflection;
using System.Collections;

namespace FxtSpider.DAL.LinqToSqlModels
{
    /// <summary>
    /// 对象比较时使用的信息类
    /// </summary>
    public struct ReverserInfo
    {
        /// <summary>
        /// 比较的方向，如下：
        /// ASC：升序
        /// DESC：降序
        /// </summary>
        public enum Direction
        {
            ASC = 0,
            DESC,
        };

        public enum Target
        {
            CUSTOMER = 0,
            FORM,
            FIELD,
            SERVER,
        };

        public string name;
        public Direction direction;
        public Target target;
    }
    public class Reverser<T> : IComparer<T>
    {
        private Type type = null;
        private ReverserInfo info;
        public Reverser()
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">进行比较的类类型</param>
        /// <param name="name">进行比较对象的属性名称</param>
        /// <param name="direction">比较方向(升序/降序)</param>
        public Reverser(Type type, string name, ReverserInfo.Direction direction)
        {
            this.type = type;
            this.info.name = name;
            if (direction != ReverserInfo.Direction.ASC)
                this.info.direction = direction;
        }      
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="className">进行比较的类名称</param>
        /// <param name="name">进行比较对象的属性名称</param>
        /// <param name="direction">比较方向(升序/降序)</param>
        public Reverser(string className, string name, ReverserInfo.Direction direction)
        {
            try
            {
                this.type = Type.GetType(className, true);
                this.info.name = name;
                this.info.direction = direction;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="t">进行比较的类型的实例</param>
        /// <param name="name">进行比较对象的属性名称</param>
        /// <param name="direction">比较方向(升序/降序)</param>
        public Reverser(T t, string name, ReverserInfo.Direction direction)
        {
            this.type = t.GetType();
            this.info.name = name;
            this.info.direction = direction;
        }
        //必须！实现IComparer<T>的比较方法。
        int IComparer<T>.Compare(T t1, T t2)
        {
            object x = this.type.InvokeMember(this.info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t1, null);
            object y = this.type.InvokeMember(this.info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t2, null);
            if (this.info.direction != ReverserInfo.Direction.ASC)
            {
                Swap(ref x, ref y);
            }
            return (new CaseInsensitiveComparer()).Compare(x, y);

            //list.Sort((left, rigth) =>
            //{
            //    if (left.FloatValue > rigth.FloatValue)
            //    {
            //        return 1;
            //    }
            //    if (left.FloatValue == rigth.FloatValue)
            //    {
            //        return 0;
            //    }
            //    return -1;

            //});
        }

        //交换操作数
        private void Swap(ref object x, ref object y)
        {
            object temp = null;
            temp = x;
            x = y;
            y = temp;
        }
    }
    public class CaseSpiderCountInfo
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName
        {
            get;
            set;
        }
        /// <summary>
        /// 网站ID
        /// </summary>
        public int WebId
        {
            get;
            set;
        }
        /// <summary>
        /// (上周)总量
        /// </summary>
        public int LastWeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// (本周)总量
        /// </summary>
        public int WeekCount
        {
            get;
            set;
        }
        /// <summary>
        /// 浮动值
        /// </summary>
        public double FloatValue
        {
            get;
            set;
        }
        /// <summary>
        /// 已入库案例
        /// </summary>
        public int ImportCount
        {
            get;
            set;
        }
        /// <summary>
        /// 未入库案例
        /// </summary>
        public int NotImportCount
        {
            get;
            set;
        }

        
        public CaseSpiderCountInfo()
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="cityId">城市Id</param>
        /// <param name="webName">网站名称</param>
        /// <param name="webId">网站Id</param>
        /// <param name="ajk_LastWeekCount">安居客(上周)总量</param>
        /// <param name="ajk_WeekCount">安居客(本周)总量</param>
        /// <param name="ajk_FloatValue">安居客浮动值</param>
        /// <param name="ajk_ImportCount"> 安居客已入库案例</param>
        /// <param name="ajk_NotImportCount"> 安居客未入库案例</param>
        public CaseSpiderCountInfo(string cityName, int cityId, string webName, int webId, int lastWeekCount, int weekCount, double floatValue, int importCount, int notImportCount)
        {
            this.CityName = cityName;
            this.CityId = cityId;
            this.WebName = webName;
            this.WebId = webId;
            this.LastWeekCount = lastWeekCount;
            this.WeekCount = weekCount;
            this.FloatValue = floatValue;
            this.ImportCount = importCount;
            this.NotImportCount = notImportCount;
        }
        public static CaseSpiderCountInfo GetCaseSpiderInfo(List<get_案例信息_获取时间段内城市网站的爬取个数Result> lastWeekSpiderCaseCountList,
         List<get_案例信息_获取时间段内城市网站的爬取个数Result> weekSpiderCaseCountList,
          List<get_案例信息_获取时间段内城市网站的已入库的案例个数Result> importCaseCountList,
         List<get_案例信息_获取时间段内城市网站的未入库的案例个数Result> notImportCaseCountList, int cityId, string cityName, int webId, string webName)
        {

            int lastWeekCount = 0; int weekCount = 0; double floatValue = 0; int importCount = 0; int notImportCount = 0;
            //上周数量
            get_案例信息_获取时间段内城市网站的爬取个数Result ajkCount1 = lastWeekSpiderCaseCountList.Find(
                delegate(get_案例信息_获取时间段内城市网站的爬取个数Result obj)
                { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (ajkCount1 != null) { lastWeekCount = Convert.ToInt32(ajkCount1.个数); }
            //本周数量
            get_案例信息_获取时间段内城市网站的爬取个数Result ajkCount2 = weekSpiderCaseCountList.Find(
                delegate(get_案例信息_获取时间段内城市网站的爬取个数Result obj)
                { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (ajkCount2 != null) { weekCount = Convert.ToInt32(ajkCount2.个数); }
            //浮动值
            floatValue = 0;
            if (lastWeekCount > 0)
            {
                floatValue = (Convert.ToDouble(weekCount) / Convert.ToDouble(lastWeekCount)) - 1;
                floatValue = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(floatValue), 4));
            }
            else if (weekCount>0)
            {
                floatValue = 1;
            }
            //已入库案例数
            get_案例信息_获取时间段内城市网站的已入库的案例个数Result import = importCaseCountList.Find(
                 delegate(get_案例信息_获取时间段内城市网站的已入库的案例个数Result obj)
                 { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (import != null) { importCount = Convert.ToInt32(import.个数); }
            //已入库案例数
            get_案例信息_获取时间段内城市网站的未入库的案例个数Result notImport = notImportCaseCountList.Find(
                 delegate(get_案例信息_获取时间段内城市网站的未入库的案例个数Result obj)
                 { return obj.城市ID == cityId && obj.网站ID == webId; });
            if (notImport != null) { notImportCount = Convert.ToInt32(notImport.个数); }
            CaseSpiderCountInfo en = new CaseSpiderCountInfo(cityName, cityId, webName, webId, lastWeekCount, weekCount, floatValue, importCount, notImportCount);

            return en;
        }
        //public int CompareTo(CaseSpiderCountInfo other)
        //{
        //    if (this.FloatValue > other.FloatValue)
        //    {
        //        return 1;
        //    }
        //    if (this.FloatValue == other.FloatValue)
        //    {
        //        return 0;
        //    }
        //    return -1;

        //}
    }
}
