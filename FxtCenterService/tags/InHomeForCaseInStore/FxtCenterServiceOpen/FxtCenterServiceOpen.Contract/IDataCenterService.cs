using CAS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterServiceOpen.Contract
{
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
