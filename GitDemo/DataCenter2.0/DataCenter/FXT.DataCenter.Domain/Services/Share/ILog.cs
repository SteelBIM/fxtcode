using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Services
{
    public interface ILog
    {
        /// <summary>
        /// 写系统日志
        /// </summary>        
        /// <param name="CityId">城市ID</param>
        /// <param name="FxtCompanyId">评估机构ID</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="UserName">用户名</param>
        /// <param name="LogType">对象类型7002006（楼盘、楼栋、房号、住宅案例）</param>
        /// <param name="EventType">操作类型7001001(新增、修改、删除)</param>
        /// <param name="ObjectId">对象ID</param>
        /// <param name="ObjectName">对象名称</param>
        /// <param name="Remarks">操作描述</param>
        /// <param name="WebIP">IP地址</param>
        /// <returns></returns>
        int InsertLog(int CityId, int FxtCompanyId, string UserId, string UserName, int LogType, int EventType, string ObjectId, string ObjectName, string Remarks, string WebIP);

        int InsertOperateLog(int CityId, int FxtCompanyId, int typecode, string typecodeIDType, int typecodeIDValue, string fields, string Value1, string Value2, string UserName);
    }
}
