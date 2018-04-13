using CBSS.Framework.Contract.API;
using CourseActivate.Web.API.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Web.API
{
    /**
    *契约 
    * **/
    [ServiceContract]
    interface ICBSSService
    {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="requestdate">请求参数</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "active", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        APIResponse Entrance(APIRequest requestdate);
    }
}
