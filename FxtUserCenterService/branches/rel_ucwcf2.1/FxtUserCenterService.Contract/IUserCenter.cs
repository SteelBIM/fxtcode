using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using CAS.Entity;

namespace FxtUserCenterService.Contract
{
    [ServiceContract]
    public interface IUserCenter
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "active"
            , Method = "POST"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData Entrance(string sinfo, string info);

        [OperationContract]
        [WebInvoke(UriTemplate = "start"
            , Method = "POST"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData UserLogin(string sinfo, string info);

        [OperationContract]
        [WebInvoke(UriTemplate = "update"
            , Method = "POST"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData UpdatePassWord(string d, string um, string np);
    }
}
