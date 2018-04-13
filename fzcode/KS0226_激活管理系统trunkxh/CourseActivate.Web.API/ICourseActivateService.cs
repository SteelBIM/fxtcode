using CourseActivate.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Web.API
{
    /**
    *契约 
    * **/
    [ServiceContract]
    interface ICourseActivateService
    {
        /// <summary>
        /// 入口
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "active", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        KingResponse Entrance(string Key, string Info, string FunName);
    }
}
