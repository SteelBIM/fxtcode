using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OpenPlatform.Framework.FlowMonitor.DataAccess;
using OpenPlatform.Framework.FlowMonitor.Models;

namespace OpenPlatform.Framework.FlowMonitor
{
    public class Api
    {
        private Api()
        {
        }

        private static readonly Api _api = new Api();

        public static Api Flow
        {
            get { return _api; }
        }


        /// <summary>
        /// 流量溢出
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="invokedTime">访问时间</param>
        /// <param name="type">接口类型</param>
        /// <param name="dataItems"></param>
        /// <param name="ip"></param>
        /// <param name="functionName"></param>
        /// <param name="functionName">产品code</param>
        /// <returns>true:溢出，false:没有溢出</returns>
        //public bool Overflow(int companyId
        //    , DateTime invokedTime
        //    , ApiType type
        //    , int dataItems
        //    , string ip = null
        //    , string functionName = null
        //    , int productTypeCode = 0)
        //{
        //    //访问次数key
        //    var key1 = companyId + invokedTime.ToString("yyyyMMdd") + type + "accessedtimes";
        //    //传输的总数据量key
        //    var key2 = companyId + invokedTime.ToString("yyyyMMdd") + type + "totaldataitems";

        //    //缓存修复
        //    _cachRepair(key1, key2, companyId, invokedTime, type);

        //    //访问阀值
        //    var dic = _accessLimts(companyId, type);

        //    var accessedTimes = Convert.ToInt32(HttpContext.Current.Cache[key1]) + 1;//到当前的访问次数
        //    var totalDataItems = Convert.ToInt32(HttpContext.Current.Cache[key2]) + dataItems;//到当前的总数据量

        //    if (accessedTimes > dic.Keys.FirstOrDefault())
        //    {
        //        return true;
        //    }
        //    if (accessedTimes <= dic.Keys.FirstOrDefault())
        //    {
        //        if (dic.Values.FirstOrDefault() > 0)
        //        {
        //            if (totalDataItems > dic.Values.FirstOrDefault())
        //            {
        //                return true;
        //            }
        //        }


        //    }


        //    HttpContext.Current.Cache[key1] = accessedTimes;
        //    HttpContext.Current.Cache[key2] = totalDataItems;

        //    //异步写入调用日志
        //    Task.Factory.StartNew(() => Monitor(companyId, invokedTime, type, dataItems, ip, functionName, productTypeCode));

        //    return false;
        //}

        /// <summary>
        /// 流量溢出(根据公司ID和产品编号去区分)
        /// zhoub 20160418
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="invokedTime">访问时间</param>
        /// <param name="type">接口类型</param>
        /// <param name="dataItems"></param>
        /// <param name="ip"></param>
        /// <param name="functionName"></param>
        /// <param name="productTypeCode">产品code</param>
        /// <returns>true:溢出，false:没有溢出</returns>
        public bool Overflow(int companyId
            , DateTime invokedTime
            , ApiType type
            , int dataItems
            , string ip = null
            , string functionName = null
            , int productTypeCode = 0
            )
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                System.Diagnostics.Stopwatch swPublic = new System.Diagnostics.Stopwatch();
                swPublic.Start();
                //访问次数key
                var key1 = companyId.ToString() + productTypeCode.ToString() + invokedTime.ToString("yyyyMMdd") + type + "accessedtimes";
                //传输的总数据量key
                var key2 = companyId.ToString() + productTypeCode.ToString() + invokedTime.ToString("yyyyMMdd") + type + "totaldataitems";

                //缓存修复
                string dailyReportFullPath = GetFilePath();
                System.Diagnostics.Stopwatch swCachRepair = new System.Diagnostics.Stopwatch();
                swCachRepair.Start();
                _cachRepair(key1, key2, companyId, invokedTime, type, productTypeCode);
                swCachRepair.Stop();
                TimeSpan tsCachRepair = swCachRepair.Elapsed;
                sb.Append("缓" + tsCachRepair.TotalMilliseconds + Environment.NewLine);

                //访问阀值
                System.Diagnostics.Stopwatch swCccessLimts = new System.Diagnostics.Stopwatch();
                swCccessLimts.Start();
                var dic = _accessLimts(companyId, type, productTypeCode);
                swCccessLimts.Stop();
                TimeSpan tsCccessLimts = swCccessLimts.Elapsed;
                sb.Append("阀" + tsCccessLimts.TotalMilliseconds);
                ip = sb.ToString();
                //System.IO.File.AppendAllText(dailyReportFullPath, sb.ToString() + Environment.NewLine);

                var accessedTimes = Convert.ToInt32(HttpContext.Current.Cache[key1]) + 1;//到当前的访问次数
                var totalDataItems = Convert.ToInt32(HttpContext.Current.Cache[key2]) + dataItems;//到当前的总数据量

                if (accessedTimes > dic.Keys.FirstOrDefault())
                {
                    return true;
                }
                if (accessedTimes <= dic.Keys.FirstOrDefault())
                {
                    if (dic.Values.FirstOrDefault() > 0)
                    {
                        if (totalDataItems > dic.Values.FirstOrDefault())
                        {
                            return true;
                        }
                    }
                }

                HttpContext.Current.Cache[key1] = Convert.ToInt32(HttpContext.Current.Cache[key1]) + 1;
                HttpContext.Current.Cache[key2] = Convert.ToInt32(HttpContext.Current.Cache[key2]) + dataItems;
                swPublic.Stop();
                TimeSpan tsPublic = swPublic.Elapsed;
                sb.Append("总" + tsPublic.TotalMilliseconds);
                ip = sb.ToString();
                //异步写入调用日志
                Task.Factory.StartNew(() => Monitor(companyId, invokedTime, type, dataItems, ip, functionName, productTypeCode));
                return false;
            }
            catch (Exception ex)
            {
               CAS.Common.LogHelper.Error(ex.ToString());
               return false;
            }
        }


        /// <summary>
        /// 用户中心接口日志记录
        /// zhoub 20160927
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="invokedTime">访问时间</param>
        /// <param name="type">接口类型</param>
        /// <param name="dataItems"></param>
        /// <param name="ip"></param>
        /// <param name="functionName"></param>
        /// <param name="productTypeCode">产品code</param>
        /// <returns>true:溢出，false:没有溢出</returns>
        public bool OverflowUserCenter(int companyId
            , DateTime invokedTime
            , ApiType type
            , int dataItems
            , string ip = null
            , string functionName = null
            , int productTypeCode = 0
            , string requestParameter = null)
        {
            //异步写入调用日志
            Task.Factory.StartNew(() => Monitor(companyId, invokedTime, type, dataItems, ip, functionName, productTypeCode, requestParameter));
            return false;
        }


        /// <summary>
        /// 流量监控
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="invokedTime">访问时间</param>
        /// <param name="type">接口类型</param>
        /// <param name="totalDataItems"></param>
        /// <param name="ip">访问时的IP</param>
        /// <param name="functionName">方法名</param>
        /// <param name="functionName">产品code</param>
        /// <returns></returns>
        private static void Monitor(int companyId
            , DateTime invokedTime
            , ApiType type
            , int totalDataItems
            , string ip = null
            , string functionName = null
            , int productTypeCode = 0
            , string requestParameter = null)
        {

            var aipInvokeLog = new
            {
                CompanyId = companyId,
                InvokeTime = invokedTime,
                ApiType = (int)type,
                DataItem = totalDataItems,
                Ip = ip,
                FunctionName = functionName,
                ProductTypeCode = productTypeCode,
                RequestParameter = requestParameter
            };
            ApiRepository.AddApiInvokeLog(aipInvokeLog);

        }

        //访问控制阀值
        private readonly Func<int, ApiType, int, Dictionary<int, int>> _accessLimts = (companyId, type, productTypeCode) =>
        {
            int accessedTimeLimts;
            int totalDataItemsLimts;

            var keyLimt1 = companyId.ToString() + productTypeCode.ToString() + type.ToString() + "accessedtimeslimt";
            var keyLimt2 = companyId.ToString() + productTypeCode.ToString() + type.ToString() + "totaldataitemslimt";

            if (HttpContext.Current.Cache[keyLimt1] == null || HttpContext.Current.Cache[keyLimt2] == null)
            {
                var obj = ApiRepository.GetFlowControlConfig(companyId, (int)type, productTypeCode);

                HttpContext.Current.Cache[keyLimt1] = obj.AccessedTimes;
                HttpContext.Current.Cache[keyLimt2] = obj.TotalDataItems;

                accessedTimeLimts = obj.AccessedTimes;
                totalDataItemsLimts = obj.TotalDataItems;
            }
            else
            {
                accessedTimeLimts = Convert.ToInt32(HttpContext.Current.Cache[keyLimt1]);
                totalDataItemsLimts = Convert.ToInt32(HttpContext.Current.Cache[keyLimt2]);
            }

            return new Dictionary<int, int> { { accessedTimeLimts, totalDataItemsLimts } };
        };

        /// <summary>
        /// 缓存修复(接口调用过程中，IIS重启或其他原因造成缓存丢失)
        /// </summary>
        private readonly Action<string, string, int, DateTime, ApiType, int> _cachRepair = (key1, key2, companyId, invokedTime, type, productTypeCode) =>
        {
            var obj1 = HttpContext.Current.Cache[key1];
            var obj2 = HttpContext.Current.Cache[key2];

            //缓存不存在
            if (obj1 == null || obj2 == null)
            {
                CAS.Common.LogHelper.Info("根据公司ID(" + companyId + ")、时间(" + invokedTime.ToString("yyyy-MM-dd") + ")、类型(" + (int)type + ")、产品code(" + productTypeCode + ")查询访问量");
                //从数据库里查询
                //var objtemp = ApiRepository.GetInvokeLog(companyId, invokedTime.ToString("yyyy-MM-dd"), (int)type, productTypeCode);
                var obj = Task.Factory.StartNew(() => ApiRepository.GetInvokeLog(companyId, invokedTime.ToString("yyyy-MM-dd"), (int)type, productTypeCode,key1,key2));
                ////如果数据库中不存在，说明这是第一次调用
                //if (obj.Result == null)
                //{
                //    //清理昨天缓存
                //    Clear(companyId, invokedTime, type, productTypeCode);
                //    HttpContext.Current.Cache[key1] = 0;
                //    HttpContext.Current.Cache[key2] = 0;
                //}
                //else
                //{
                //    HttpContext.Current.Cache[key1] = obj.Result.AccessedTimes;
                //    HttpContext.Current.Cache[key2] = obj.Result.TotalDataItems;
                //}
            }
        };

        /// <summary>
        /// 清理昨天的缓存
        /// </summary>
        public static readonly Action<int, DateTime, ApiType, int> Clear = (companyId, invokedTime, type, productTypeCode) =>
        {
            var key1 = companyId.ToString() + productTypeCode.ToString() + invokedTime.AddDays(-1).ToString("yyyyMMdd") + type.ToString() + "accessedtimes";
            var key2 = companyId.ToString() + productTypeCode.ToString() + invokedTime.AddDays(-1).ToString("yyyyMMdd") + type.ToString() + "totaldataitems";

            HttpContext.Current.Cache.Remove(key1);
            HttpContext.Current.Cache.Remove(key2);
        };

        /// <summary>
        /// 获取日志文件存放路径
        /// zhoub 20160627
        /// </summary>
        /// <returns></returns>
        private static string GetFilePath()
        {
            var reportDirectory = string.Format("~/Log/overflow/{0}/", DateTime.Now.ToString("yyyy-MM"));

            reportDirectory = System.Web.Hosting.HostingEnvironment.MapPath(reportDirectory);

            if (!System.IO.Directory.Exists(reportDirectory))
            {
                System.IO.Directory.CreateDirectory(reportDirectory);
            }
            return string.Format("{0}report_{1}.log", reportDirectory, DateTime.Now.Day);
        }
    }

    /// <summary>
    /// 接口类型
    /// </summary>
    public enum ApiType
    {
        Project = 1,
        Building = 2,
        House = 3,
        Case = 4,
        UserCenterApi = 21
    }
}
