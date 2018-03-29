using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using CAS.Entity;

namespace FxtCenterService.Contract
{
    /**
    *契约 
    * **/
    [ServiceContract]
    public interface IDataCenterService
    {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="functionname">功能方法</param>
        /// <param name="certifyArgs">验证参数</param>
        /// <param name="funArgs">功能参数</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "active", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData Entrance(string sinfo, string info);
    }
}
