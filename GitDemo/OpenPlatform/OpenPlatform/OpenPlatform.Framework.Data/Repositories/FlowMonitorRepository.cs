using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Domain.Repositories;
using OpenPlatform.Framework.Data.SQL;
using OpenPlatform.Framework.Utils;

namespace OpenPlatform.Framework.Data.Repositories
{
    public class FlowMonitorRepository : IFlowMonitorRepository
    {
        //流量控制数据库连接
        private static readonly string ConnString =  ConfigurationHelper.ConnString;
        //需要控制的产品code
        private static readonly List<int> listProductTypeCode = new List<int>() { 0 };
    

        /// <summary>
        /// 增加接口调用日志
        /// zhoub 20160927 edit(可根据传入)
        /// </summary>
        /// <param name="apiInvokeLog"></param> 
        /// <returns></returns>
        public int AddApiInvokeLog(ApiInvokeLogDto apiInvokeLog)
        {
            string strSql = "";
            if (apiInvokeLog.ApiType == 21)
            {
                strSql = FlowMonitorSql.AddApiInvokeLogUserCenter;
            }
            else {
                strSql = FlowMonitorSql.AddApiInvokeLog;
            }
            //var connString = ConfigurationHelper.ConnString;

            using (var conn = Dapper.MySqlConnection(ConnString))
            {
                return conn.Execute(strSql, apiInvokeLog);
            }
        }

        /// <summary>
        /// 根据指定条件获取接口调用日志信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="invokedDate"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public FlowAccessDto GetInvokeLog(int companyId, string invokedDate, int apiType, int productTypeCode)
        {
            var strSql = FlowMonitorSql.GetInvokeLog;

            using (var conn = Dapper.MySqlConnection(ConnString))
            {
                return conn.Query<FlowAccessDto>(strSql, new { companyId, invokedDate, apiType, productTypeCode }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据指定条件获取控制表信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public FlowAccessDto GetFlowControlConfig(int companyId, int apiType, int productTypeCode)
        {
            var strSql = FlowMonitorSql.GetFlowControlConfig;
            if (listProductTypeCode.Contains(productTypeCode))
            {
                strSql = strSql+" AND ProductTypeCode=" + productTypeCode;
            }
            using (var conn = Dapper.MySqlConnection(ConnString))
            {
                var query = conn.Query<FlowAccessDto>(strSql, new { companyId, apiType }).FirstOrDefault();
                return query ?? new FlowAccessDto();
            }
        }
    }
}
