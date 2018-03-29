using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Framework.Data.SQL
{
    public struct FlowMonitorSql
    {
        /// <summary>
        /// 增加接口调用日志
        /// </summary>
        public const string AddApiInvokeLog = @"INSERT INTO API_Invoke_Log(CompanyId,FunctionName,APIType,IP,DataItem,ProductTypeCode)  VALUES(@CompanyId, @FunctionName, @APIType, @IP, @DataItem, @ProductTypeCode)";

        /// <summary>
        /// 增加用户中心接口调用日志
        /// zhoub 20160927
        /// </summary>
        public const string AddApiInvokeLogUserCenter = @"INSERT INTO api_invoke_log_usercenter(CompanyId,FunctionName,APIType,IP,DataItem,ProductTypeCode,RequestParameter)  VALUES(@CompanyId, @FunctionName, @APIType, @IP, @DataItem, @ProductTypeCode,@RequestParameter)";

        /// <summary>
        /// 根据指定条件获取接口调用日志信息
        /// </summary>
        public const string GetInvokeLog = @"SELECT COUNT(1) AS AccessedTimes	
	                                        ,SUM(DataItem)	AS TotalDataItems
	                                        FROM API_Invoke_Log
	                                        WHERE companyid = @companyId
	                                        AND apitype = @apiType
                                            AND producttypecode=@productTypeCode
	                                        AND DATE_FORMAT(invokeTime, '%Y-%m-%d') = @invokedDate
	                                        GROUP BY companyid";
        /// <summary>
        /// 根据指定条件获取控制表信息
        /// </summary>
        public const string GetFlowControlConfig = @"SELECT maxcount as AccessedTimes
	                                                ,maxdataitem as TotalDataItems	
	                                                FROM Flow_Control_Config
	                                                WHERE companyid = @companyId
	                                                AND apitype = @apiType";
    }
}
